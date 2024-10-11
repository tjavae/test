using CUDC.Common.Dtos;
using CUDC.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace CUDC.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _service;
        private readonly Logger fileLogger = LogManager.GetLogger("fileLogger");
        private readonly ILogService _databaseLogService;

        public SecurityController(ISecurityService service, ILogService logService)
        {
            _service = service;
            _databaseLogService = logService;
        }

        [HttpGet]
        [Route("{surveyId:guid}/{cuNumber:int}/GetSurveyOwner")]
        public async Task<IActionResult> GetSurveyOwner(Guid surveyId, int cuNumber, string userId = null)
        {
            try
            {
                return Ok(await _service.GetSurveyOwner(new SurveyOwnerRequest
                {
                    SurveyId = surveyId,
                    CuNumber = cuNumber,
                    UserId = userId ?? User.Identity.Name
                }));
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "GetSurveyOwner error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("{surveyId:guid}/{cuNumber:int}/{userId}/GetSurveyOwnerAdminView")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSurveyOwnerAdminView(Guid surveyId, int cuNumber, string userId)
        {
            try
            {
                return await GetSurveyOwner(surveyId, cuNumber, userId);
            }
            catch (Exception ex)
            {
                string message = "GetUserAdminView(Guid" + surveyId + ", cuNumber: " + cuNumber + ", userId" + userId + ") error";
                await _databaseLogService.NLogError(ex, message, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("IsSeUser")]
        public async Task<IActionResult> IsSeUser()
        {
            try
            {
                return Ok(await _service.IsSeUser(User.Identity.Name));
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "IsSeUser() error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("{charterNumber:int}/{userId}/IsClaimedCU")]
        public async Task<IActionResult> IsClaimedCU(int charterNumber, string userId)
        {
            try
            {
                return Ok(await _service.IsClaimedCU(charterNumber, userId));
            }
            catch (Exception ex)
            {
                string message = "IsClaimedCU(charterNumber:" + charterNumber + ", userId:" + userId + ") error";
                await _databaseLogService.NLogError(ex, message, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("{region:int}/{surveyType:int}/GetNcuaUsers")]
        public async Task<IActionResult> GetNcuaUsers(int region, int surveyType)
        {
            try
            {
                return Ok(await _service.GetNcuaUsers(region, surveyType));
            }
            catch (Exception ex)
            {
                string message = "GetNcuaUsers(region:" + region + ",surveyType:" + surveyType + ") error";
                await _databaseLogService.NLogError(ex, message, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }
    }
}
