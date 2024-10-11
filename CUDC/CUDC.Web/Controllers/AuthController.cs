using CUDC.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using PostSharp.Patterns.Diagnostics;

namespace CUDC.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISecurityService _service;
        private readonly NLog.Logger fileLogger = LogManager.GetLogger("fileLogger");
        private readonly ILogService _databaseLogService;
        private static readonly LogSource logSource = LogSource.Get();

        public AuthController(ISecurityService service, ILogService logService)
        {
            _service = service;
            _databaseLogService = logService;
        }

        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser(string userId = null)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var claimInfo = (ClaimsIdentity)User.Identity;
                    var userRole = await _service.GetUserRole(userId ?? claimInfo.Name);
                    if (userRole != null) 
                    {
                        logSource.Info.Write(FormattedMessageBuilder.Formatted("User {userId} logged in on {dateTime}", userRole.UserId.ToString(), DateTime.Now));
                    }                
                    
                    return Ok(userRole);
                }
                else
                {
                    logSource.Info.Write(FormattedMessageBuilder.Formatted("User {userId} logged in on {dateTime}", User.Identity.ToString(), DateTime.Now));
                    return BadRequest("Not authenticated");
                }
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "GetUser error", MethodBase.GetCurrentMethod().Name);
                logSource.Error.Write(FormattedMessageBuilder.Formatted(ex.ToString()));
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("{userId}/GetUserAdminView")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserAdminView(string userId)
        {
            try
            {
                return await GetUser(userId);
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "GetUserAdminView(string userId) error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("{userId}/GetUserById")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            try
            {
                var userRole = await _service.GetUserRole(userId);
                return Ok(userRole);
            }
            catch (Exception ex)
            {
                fileLogger.Error(ex);
                await _databaseLogService.NLogError(ex, "GetSurveys() error", MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }
    }
}
