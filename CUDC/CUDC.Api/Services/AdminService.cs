using CUDC.Api.Data;
using CUDC.Api.Helpers;
using CUDC.Common.Dtos;
using CUDC.Common.Dtos.UserSearch;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CUDC.Api.Services
{
    public class AdminService : IAdminService
    {
        private readonly SurveyContext _context;
        private readonly Logger fileLogger = LogManager.GetLogger("fileLogger");
        private readonly ILogService _databaseLogService;
        private readonly IEmployeeService _employeeService;     

        public AdminService(SurveyContext context, ILogService logService, IEmployeeService employeeService)
        {
            _context = context;
            _databaseLogService = logService;
            _employeeService = employeeService;            
        }

        public IEnumerable<SurveyDto> GetSurveys()
        {
            try
            {
                var query = from s in _context.Surveys
                            join st in _context.SurveyTypes on s.SurveyTypeId equals st.Id
                            where st.IsActive
                            orderby s.Title
                            select new SurveyDto
                            {
                                Id = s.UniqueId,
                                Title = s.Title,
                                Description = s.Description,
                                Type = (Common.Enums.SurveyType)(s.SurveyTypeId - 1),
                                TypeString = st.Name,
                                StartDate = s.StartDate,
                                EndDate = s.EndDate,
                                IsActive = s.IsActive,
                                CreatedOn = s.CreatedOn,
                                CreatedBy = s.CreatedBy,
                                ModifiedOn = s.ModifiedOn,
                                ModifiedBy = s.ModifiedBy,
                                InformationText = s.InformationText
                            };
                List<SurveyDto> sortedList = query.OrderByDescending(o => o.IsActive).ThenByDescending(o => o.StartDate).ToList();
                return sortedList;

            }
            catch (Exception e)
            {
                fileLogger.Error(e);
                _databaseLogService.NLog("Error", "GetSurveys error", e, MethodBase.GetCurrentMethod().Name);
                throw;
            }

        }

        public SurveyDto GetSurvey(Guid surveyId)
        {
            try
            {
                var query = from s in _context.Surveys
                            join st in _context.SurveyTypes on s.SurveyTypeId equals st.Id
                            where s.UniqueId == surveyId && st.IsActive
                            select new SurveyDto
                            {
                                Id = s.UniqueId,
                                Title = s.Title,
                                Description = s.Description,
                                TypeString = st.Name,
                                StartDate = s.StartDate,
                                EndDate = s.EndDate,
                                IsActive = s.IsActive,
                                CreatedOn = s.CreatedOn,
                                CreatedBy = s.CreatedBy,
                                ModifiedOn = s.ModifiedOn,
                                ModifiedBy = s.ModifiedBy,
                                InformationText = s.InformationText
                            };
                return query.Single();
            }
            catch (Exception e)
            {
                string message = "GetSurvey(" + surveyId + ") error";
                _databaseLogService.NLog("Error", message, e, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(e);
                throw;
            }

        }

        public void CreateSurvey(SurveyDto survey)
        {
            try
            {
                var surveyType = _context.SurveyTypes.Single(x => x.Name == survey.Type.ToString());

                //Deactivate previously active survey if the new survey needs to be active instead
                if (survey.IsActive)
                {
                    var activeSurvey = _context.Surveys.SingleOrDefault(x => x.SurveyTypeId == surveyType.Id && x.IsActive);
                    if (activeSurvey != null)
                    {
                        activeSurvey.IsActive = false;
                        _context.Surveys.Update(activeSurvey);
                    }
                }

                var newSurvey = new Survey
                {
                    UniqueId = survey.Id,
                    Title = survey.Title,
                    Description = survey.Description,
                    SurveyTypeId = surveyType.Id,
                    StartDate = survey.StartDate,
                    EndDate = survey.EndDate,
                    IsActive = survey.IsActive,
                    CreatedOn = survey.CreatedOn,
                    CreatedBy = survey.CreatedBy,
                    InformationText = survey.InformationText
                };
                _context.Surveys.Add(newSurvey);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                string message = "CreateSurvey: '" + survey.Title + "' error";
                _databaseLogService.NLog("Error", message, e, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(e);
                throw;
            }
        }

        public void UpdateSurvey(SurveyDto survey)
        {
            try
            {
                var currSurvey = _context.Surveys.Single(x => x.UniqueId == survey.Id);
                if (currSurvey.Title != survey.Title)
                {
                    var duplicatedSurvey = _context.Surveys.SingleOrDefault(x => x.Title == survey.Title);
                    if (duplicatedSurvey != null) throw new Exception();
                }

                var surveyType = _context.SurveyTypes.Single(x => x.Name == survey.Type.ToString());


                //Deactivate previously active survey if this survey needs to be active instead
                if (survey.IsActive && !currSurvey.IsActive)
                {
                    var activeSurvey = _context.Surveys.SingleOrDefault(x => x.SurveyTypeId == surveyType.Id && x.IsActive);
                    if (activeSurvey != null)
                    {
                        activeSurvey.IsActive = false;
                        _context.Surveys.Update(activeSurvey);
                    }
                }

                currSurvey.Title = survey.Title;
                currSurvey.Description = survey.Description;
                currSurvey.SurveyTypeId = surveyType.Id;
                currSurvey.StartDate = survey.StartDate;
                currSurvey.EndDate = survey.EndDate;
                currSurvey.IsActive = survey.IsActive;
                currSurvey.ModifiedOn = survey.ModifiedOn;
                currSurvey.ModifiedBy = survey.ModifiedBy;
                currSurvey.InformationText = survey.InformationText;
                _context.Surveys.Update(currSurvey);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                fileLogger.Error(e);
                string message = "UpdateSurvey: '" + survey.Title + "' error";
                _databaseLogService.NLog("Error", message, e, MethodBase.GetCurrentMethod().Name);
                throw;
            }

        }

        public void CopySurvey(CopySurveyRequest request)
        {
            try
            {
                var helper = new CopySurveyHelper(_context);
                helper.CopySurvey(request);
            }
            catch (Exception e)
            {
                fileLogger.Error(e);
                string message = "CopySurvey error";
                _databaseLogService.NLog("Error", message, e, MethodBase.GetCurrentMethod().Name);
                throw;
            }

        }

        public void CreateQuestions(QuestionsEdit questions)
        {
            throw new NotImplementedException();
        }

        public void UpdateQuestions(QuestionsEdit questions)
        {
            try
            {
                var helper = new QuestionsEditHelper(_context);
                helper.SaveChanges(questions);
            }
            catch (Exception e)
            {
                fileLogger.Error(e);
                string message = "UpdateQuestions error";
                _databaseLogService.NLog("Error", message, e, MethodBase.GetCurrentMethod().Name);
                throw;
            }

        }

        public IEnumerable<ResponseDto> GetResponses(Guid surveyId)
        {
            try
            {
                var query = from s in _context.Surveys
                            join r in _context.Responses on s.Id equals r.SurveyId
                            where s.UniqueId == surveyId
                            orderby r.UserId, r.CuNumber
                            select new ResponseDto
                            {
                                Id = r.UniqueId,
                                SurveyId = s.UniqueId,
                                UserId = r.UserId,
                                CuNumber = r.CuNumber,
                                JoinNumber = r.JoinNumber,
                                CreatedOn = r.CreatedOn,
                                CreatedBy = r.CreatedBy,
                                ModifiedOn = r.ModifiedOn,
                                ModifiedBy = r.ModifiedBy,
                                SubmittedOn = r.SubmittedOn,
                                IsRejected = r.IsRejected
                            };

                return query.ToList();

            }
            catch (Exception e)
            {
                fileLogger.Error(e);
                string message = "GetResponses: " + surveyId + " error";
                _databaseLogService.NLog("Error", message, e, MethodBase.GetCurrentMethod().Name);
                throw;
            }

        }

        public void RejectResponse(Guid responseId)
        {
            try
            {
                var response = _context.Responses.SingleOrDefault(x => x.UniqueId == responseId);
                if (response == null)
                {
                    fileLogger.Info("No Response found");
                    return;
                }                
                else 
                {
                    response.IsRejected = true;
                    _context.Responses.Update(response);
                    _context.SaveChanges();
                }
                
            }
            catch (Exception e)
            {
                fileLogger.Error(e);
                string message = "RejectResponse: " + responseId + " error";
                _databaseLogService.NLog("Error", message, e, MethodBase.GetCurrentMethod().Name);
                throw;
            }

        }

        public IEnumerable<UserRoleDto> GetUsers()
        {
            var employees = _employeeService.GetAllEmployees().Result;

            var query = from e in employees
                        where e.Status == "3"
                        orderby e.FirstName
                        select new UserRoleDto
                        {
                            Id = null,
                            UserId = e.MailBox,
                            EmployeeNumber = e.EmployeeNumber,
                            FirstName = e.FirstName,
                            LastName = e.LastName
                        };
            return query.ToList();
        }

        [Obsolete("Method is deprecated, it no longer use")]
        public void CreateUser(UserRoleDto user)
        {
            var role = _context.Roles.Single(x => x.Name == user.Role.ToString());
            var newUser = new UserRole
            {
                UniqueId = (Guid)user.Id,
                UserId = user.UserId,
                RoleId = role.Id,
                CreatedOn = (DateTime)user.CreatedOn,
                CreatedBy = user.CreatedBy
            };
            _context.UserRoles.Add(newUser);
            _context.SaveChanges();
        }

        [Obsolete("Method is deprecated, it no longer use")]
        public void UpdateUser(UserRoleDto user)
        {
            var role = _context.Roles.Single(x => x.Name == user.Role.ToString());
            var currUser = _context.UserRoles.Single(x => x.UniqueId == user.Id);
            currUser.UserId = user.UserId;
            currUser.RoleId = role.Id;
            currUser.ModifiedOn = user.ModifiedOn;
            currUser.ModifiedBy = user.ModifiedBy;
            _context.UserRoles.Update(currUser);
            _context.SaveChanges();
        }

        [Obsolete("Method is deprecated, it no longer use")]
        public void DeleteUser(Guid id)
        {
            var user = _context.UserRoles.Single(x => x.UniqueId == id);
            _context.UserRoles.Remove(user);
            _context.SaveChanges();
        }

        public IEnumerable<CycleDateDto> GetCycleDates()
        {
            try
            {
                var results = _context.Surveys
                .Where(row => row.StartDate != null)
                .Select(row => row.StartDate)
                .Distinct();
                var dtoList = new List<CycleDateDto>();
                foreach (DateTime? s in results)
                {
                    if (s.HasValue)
                    {
                        var dto = new CycleDateDto
                        {
                            CycleDateFormat1 = s.Value.ToString("MM/dd/yyyy"),
                            CycleDateFormat2 = s.Value.ToString("yyyyMMdd")
                        };
                        dtoList.Add(dto);
                    }
                }
                return dtoList.GroupBy(d => d.CycleDateFormat2).Select(g => g.First()).OrderByDescending(g => g.CycleDateFormat2);

            }
            catch (Exception e)
            {
                fileLogger.Error(e);
                string message = "GetCycleDates error";
                _databaseLogService.NLog("Error", message, e, MethodBase.GetCurrentMethod().Name);
                throw;
            }

        }

        // This DeleteSurvey function is for Selenium.
        // Only CUDC survey that created by AutomatedTestUser01 or user in CUDC TESTER group can be deleted.
        public void DeleteSurvey(Guid id)
        {
            try
            {
                var survey = _context.Surveys.Single(x => x.UniqueId == id);
                if (survey.SurveyTypeId == 1)
                {
                    var sections = _context.Sections.Where(sec => sec.SurveyId == survey.Id);
                    var responses = _context.Responses.Where(r => r.SurveyId == survey.Id);
                    var responseIds = responses.Select(r => r.Id).Distinct();
                    var answers = _context.Answers.Where(a => responseIds.Contains(a.ResponseId));
                    var questions = _context.Questions.Where(q => q.SurveyId == survey.Id);
                    var qIds = questions.Select(q => q.Id).Distinct();
                    var qUniqueIds = questions.Select(q => q.UniqueId).Distinct();
                    var qReferences = _context.QuestionReferences.Where(qr => qUniqueIds.Contains(qr.QuestionId) || qUniqueIds.Contains(qr.ReferenceQuestionId));
                    var qRevisions = _context.QuestionRevisions.Where(qr => qIds.Contains(qr.QuestionId));
                    var qRevisionIds = qRevisions.Select(r => r.Id).Distinct();
                    var qOptions = _context.QuestionOptions.Where(qo => qRevisionIds.Contains(qo.QuestionRevisionId));

                    _context.Answers.RemoveRange(answers);
                    _context.Responses.RemoveRange(responses);
                    _context.QuestionReferences.RemoveRange(qReferences);
                    _context.Surveys.Remove(survey);
                    _context.Sections.RemoveRange(sections);
                    _context.Questions.RemoveRange(questions);
                    _context.QuestionRevisions.RemoveRange(qRevisions);
                    _context.QuestionOptions.RemoveRange(qOptions);

                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                fileLogger.Error(e);
                _databaseLogService.NLog("Error", "DeleteSurvey error", e, MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }


        public IEnumerable<SearchUserResult> GetSearchUserResults(SearchUserRequest request)
        {
            var results = _employeeService.SearchEmployeeByRequest(request).Result;
            return results;
        }

    }
}
