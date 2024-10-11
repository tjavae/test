using CUDC.Api.Data;
using CUDC.Common.Dtos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using NLog;
using PostSharp.Patterns.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CUDC.Api.Services
{
    public class SecurityService : ServiceBase, ISecurityService
    {
        private readonly SurveyContext _context;
        private readonly IConfiguration _config;       
        private readonly NLog.Logger fileLogger = LogManager.GetLogger("fileLogger");
        private readonly ILogService _databaseLogService;
        private static readonly LogSource logSource = LogSource.Get();
        private readonly IEmployeeService _employeeService;

        public SecurityService(SurveyContext context, IConfiguration config, ILogService logService, IEmployeeService employeeService) : base(context, config, logService)
        {
            _context = context;
            _config = config;
            _databaseLogService = logService;
            _employeeService = employeeService;            
        }

        public IEnumerable<RoleDto> GetRoles()
        {
            try
            {
                var query = from r in _context.Roles
                            where r.IsActive == true
                            orderby r.Name
                            select new RoleDto
                            {
                                Id = r.UniqueId,
                                Name = r.Name,
                                Description = r.Description,
                                CreatedOn = r.CreatedOn,
                                CreatedBy = r.CreatedBy,
                                ModifiedOn = r.ModifiedOn,
                                ModifiedBy = r.ModifiedBy
                            };

                return query.ToList();
            }
            catch (Exception e)
            {
                _databaseLogService.NLog("Error", "GetRoles error", e, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(e);
                throw;
            }

        }

        // Get user's permission record from NcuaSecurityService        
        private string GetEmployeeNumber(string userName) 
        {
            var requestUri = NCUASecurityWebApiUrl($"GetPrincipalByUserNameAppName?userName={userName}&appName=CUDC/RADAR");
            var jsonString = Task.Run(async () => await GetAsync<string>(requestUri)).Result;
            if (jsonString == null)
            {
                return null;
            } 
            else
            {
                var json = JObject.Parse(jsonString);
                return (string)json["EmployeeNo"];
            }
            
        }
        private List<UserPermission> GetUserPermissions(string userName)
        {
            var userPermissions = new List<UserPermission>();
            var requestUri = NCUASecurityWebApiUrl($"GetPrincipalByUserNameAppName?userName={userName}&appName=CUDC/RADAR");            
            var jsonString = Task.Run(async () => await GetAsync<string>(requestUri)).Result;
            var json = JObject.Parse(jsonString);             

            foreach (var p in json["UserPermissions"]) 
            {
                if ((string)p["ExpirationDate"] == null || (DateTime)p["ExpirationDate"] > DateTime.Now) 
                {
                    var permission = new UserPermission
                    {
                        Action = (string)p["ActionName"],
                        GroupName = (string)p["GroupName"],
                        Module = (string)p["ModuleName"],
                        Region = Convert.ToInt32((string)p["Region"]),
                        Se = (string)p["SeGroup"],
                        District = Convert.ToInt32((string)p["District"]),
                        CreditUnion = Convert.ToInt32((string)p["CreditUnion"])
                    };
                    userPermissions.Add(permission);
                }
            }
            return userPermissions;
        }


        private string NCUASecurityWebApiUrl(string path)
        {
            return $"{_config["NCUASecurityWebApiBaseUrl"]}{path}";
        }        

        // Convert user permissions form NCUA Security Service to a UserRoleDto
        private UserRoleDto ConvertNcuaPrincipalToUserRoleDto(string userName)
        {
            try
            {   
                string employeeNumber = GetEmployeeNumber(userName);
                
                if (employeeNumber != null)
                {
                    var emp = _employeeService.GetEmployeeByNumber(employeeNumber).Result;

                    string firstName = null;
                    string lastName = null;
                    if (emp != null)
                    {
                        firstName = emp.FirstName ?? null;
                        lastName = emp.LastName ?? null;
                    }

                    var userPermissions = GetUserPermissions(userName);
                    if (userPermissions.Count > 0)
                    {
                        var uDto = new UserRoleDto
                        {
                            Id = null,
                            UserId = userName,
                            Role = Common.Enums.Role.Admin,
                            RoleString = "Admin",
                            UserType = new UserTypeDef
                            {
                                UserType = "Admin",
                                CanAccess = true,
                                ReviewCat = true
                            },
                            Permissions = userPermissions.ToArray(),
                            FirstName = firstName,
                            LastName = lastName,
                            EmployeeNumber = employeeNumber
                        };
                        return uDto;
                    }
                    else { return null; }
                }
                else { return null; }
            }
            catch (Exception e)
            {
                fileLogger.Error(e);
                string message = "ConvertNcuaPrincipalToUserRoleDto(userName: " + userName + ") error";
                _databaseLogService.NLog("Error", message, e, MethodBase.GetCurrentMethod().Name);
                throw;
            }


        }

        // Get UserRoleDto by userId
        public UserRoleDto GetUserRole(string userId)
        {
            if (userId != null)
            {
                string enableCudcTester = _config["EnableCudcTester"].ToString();
                if (userId.Contains("AutomatedTestUser01"))
                {
                    var userRoleDto = new UserRoleDto
                    {
                        UserId = userId,
                        Role = Common.Enums.Role.Admin,
                        RoleString = "Admin",
                        UserType = new UserTypeDef
                        {
                            UserType = "Admin",
                            CanAccess = true,
                            ReviewCat = true
                        },
                        Permissions = new UserPermission[] {
                            new() {
                                GroupName = enableCudcTester == "Yes" ? "CUDC TESTER" : "ADMIN",
                                Module = "Surveys Management"
                            },
                            new() { GroupName = "SURVEY VIEWER" },
                            new() {
                                Action = "Edit",
                                GroupName = "DISTRICT EXAMINER",
                                Module = "CAT Survey",
                                Region = 8,
                                Se = "A"
                            }
                        },
                        FirstName = "Automated",
                        LastName = "TestUser01",
                        EmployeeNumber = "99999"
                    };
                    return userRoleDto;
                }
                else
                {
                    try
                    {
                        var userRoleDto = ConvertNcuaPrincipalToUserRoleDto(userId);
                        if (userRoleDto != null)
                        {
                            logSource.Info.Write(FormattedMessageBuilder.Formatted("User {userId} logged in on {dateTime}", userId.ToString(), DateTime.Now));
                            return userRoleDto;
                        }
                        else
                        {
                            logSource.Info.Write(FormattedMessageBuilder.Formatted("User {userId} failed to log in on {dateTime}", userId.ToString(), DateTime.Now));
                            return null;
                        }

                    }
                    catch (Exception e)
                    {
                        fileLogger.Error(e);
                        string message = "GetUserRole(userId: " + userId + ") error";
                        _databaseLogService.NLog("Error", message, e, MethodBase.GetCurrentMethod().Name);
                        throw;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public SurveyOwnerResponse GetSurveyOwner(SurveyOwnerRequest request)
        {
            try
            {
                if (request.UserId.Contains("AutomatedTestUser01")) //For Selenium testing
                {
                    var ownerResp = new SurveyOwnerResponse
                    {
                        FirstName = "Automated",
                        LastName = "TestUser01"
                    };
                    var owner = (from s in _context.Surveys
                                 join r in _context.Responses on s.Id equals r.SurveyId
                                 where s.UniqueId == request.SurveyId &&
                                      r.CuNumber == request.CuNumber &&
                                      r.CreatedBy == "HQNT\\AutomatedTestUser01"
                                 select new
                                 {
                                     r.CreatedBy,
                                     ownerResp.FirstName,
                                     ownerResp.LastName
                                 }
                               ).FirstOrDefault();
                    if (owner == null)
                    {
                        ownerResp.HasOwner = false;
                        ownerResp.OwnerIsMe = false;
                    }
                    else
                    {
                        ownerResp.HasOwner = true;
                        ownerResp.OwnerIsMe = true;
                    }
                    return ownerResp;
                }

                var employees = _employeeService.GetAllEmployees().Result;

                var emp = (from e in employees
                           join r in _context.Responses on e.LanId.ToLower() equals r.UserId.ToLower()
                           join s in _context.Surveys on r.SurveyId equals s.Id                           
                           where s.UniqueId == request.SurveyId &&
                                r.CuNumber == request.CuNumber
                           orderby r.CreatedOn descending
                           select new
                           {
                               e.LanId,
                               e.FirstName,
                               e.LastName
                           }).FirstOrDefault();               

                if (emp != null)
                {
                    return new SurveyOwnerResponse
                    {
                        HasOwner = true,
                        OwnerIsMe = string.Equals(emp.LanId, request.UserId, StringComparison.OrdinalIgnoreCase),
                        FirstName = emp.FirstName,
                        LastName = emp.LastName
                    };
                }
                else
                {
                    return new SurveyOwnerResponse
                    {
                        HasOwner = false
                    };
                }

            }
            catch (Exception e)
            {
                fileLogger.Error(e);
                _databaseLogService.NLog("Error", "GetSurveyOwner error", e, MethodBase.GetCurrentMethod().Name);
                throw;
            }

        }

        public bool IsSeUser(string userId)
        {
            try
            {
                var employees = _employeeService.GetAllEmployees().Result;

                var query = from e in employees
                            where e.Status == "3" && //Active employees
                                e.GsGrade == "CU15" && //SEs have GS grade = 15
                                e.MailBox == userId
                            select e;
                return query.Any();
            }
            catch (Exception e)
            {
                string message = "IsSeUser(userId:" + userId + ") error";
                _databaseLogService.NLog("Error", message, e, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(e);
                throw;
            }
        }

        public bool IsClaimedCU(Int32 charterNumber, string userId)
        {
            try
            {
                var query = from r in _context.Responses
                            join s in _context.Surveys on r.SurveyId equals s.Id
                            where r.CuNumber == charterNumber &&
                                  r.UserId == userId &&
                                  (s.SurveyTypeId == 2 || // CAT survey
                                   s.SurveyTypeId == 3 || // SE survey
                                   s.SurveyTypeId == 4)   // DOS survey
                            select r;
                return query.Any();
            }
            catch (Exception e)
            {
                string message = "IsClaimedCU(charterNumber: " + charterNumber + ", userId: " + userId + "error";
                _databaseLogService.NLog("Error", message, e, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(e);
                throw;
            }

        }

        public IEnumerable<UserRoleDto> GetNcuaUsers(int region, int surveyType)
        {
            try
            {
                string surveyTypeString = surveyType switch
                {
                    1 => "CAT Survey",
                    2 => "SE Survey",
                    3 => "DOS Survey",
                    _ => "CUDC Survey",
                };
                var userRoleDtoList = new List<UserRoleDto>();
                
                var requestUri = NCUASecurityWebApiUrl($"GetCUDCTransferOwnershipUserList?region={region}&modulename={surveyTypeString}");
                var transferUsers = Task.Run(async () => await GetAsync<List<TransferUserDto>>(requestUri)).Result;
                if (transferUsers.Count > 0) 
                {
                    var employees = _employeeService.GetAllEmployees().Result;
                    var empNumberList = transferUsers.Select(user => user.EmployeeNo).ToList();
                    var query = from e in employees
                                where empNumberList.Contains(e.EmployeeNumber)
                                orderby e.FirstName
                                select new UserRoleDto
                                {
                                    UserId = "HQNT\\" + e.MailBox,
                                    EmployeeNumber = e.EmployeeNumber,
                                    FirstName = e.FirstName,
                                    LastName = e.LastName
                                };
                    userRoleDtoList = query.ToList();
                }

                return userRoleDtoList;

            }
            catch (Exception e)
            {
                string message = "GetNcuaUsers:region:" + region + " SurveyType: " + surveyType + " Error";
                _databaseLogService.NLog("Error", message, e, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(e);
                throw;
            }
        }
    }
}
