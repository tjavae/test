using CUDC.Common.Dtos;
using CUDC.Common.Dtos.UserSearch;
using CUDC.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace CUDC.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _service;
        private readonly ISurveyService _surveyService;
        private readonly ILogService _databaseLogService;
        private readonly Logger fileLogger = LogManager.GetLogger("fileLogger");

        public AdminController(IAdminService service,
            ISurveyService surveyService,
            ILogService logService)
        {
            _service = service;
            _surveyService = surveyService;
            _databaseLogService = logService;
        }

        [HttpGet]
        [Route("GetSurveys")]
        public async Task<IActionResult> GetSurveys()
        {
            try
            {
                return Ok(await _service.GetSurveys());
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "GetSurveys() error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("GetSurvey/{surveyId:guid}")]
        public async Task<IActionResult> GetSurvey(Guid surveyId)
        {
            try
            {
                return Ok(await _service.GetSurvey(surveyId));
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "GetSurvey() error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("CreateSurvey")]
        public async Task<IActionResult> CreateSurvey(SurveyDto survey)
        {
            try
            {
                survey.Id = Guid.NewGuid();
                survey.CreatedOn = DateTime.Now;
                survey.CreatedBy = User?.Identity?.Name;
                await _service.CreateSurvey(survey);
                return Ok(survey);
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "CreateSurvey() error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpPut]
        [Route("UpdateSurvey")]
        public async Task<IActionResult> UpdateSurvey(SurveyDto survey)
        {
            try
            {
                survey.ModifiedOn = DateTime.Now;
                survey.ModifiedBy = User?.Identity?.Name;
                await _service.UpdateSurvey(survey);
                return Ok(survey);
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "UpdateSurvey() error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("CopySurvey/{surveyId:guid}")]
        public async Task<IActionResult> CopySurvey(Guid surveyId)
        {
            try
            {
                var request = new CopySurveyRequest
                {
                    SurveyId = surveyId,
                    UserId = User.Identity.Name
                };
                await _service.CopySurvey(request);
                return Ok();
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "CopySurvey() error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("GetQuestions/{surveyId:guid}")]
        public async Task<IActionResult> GetQuestions(Guid surveyId)
        {
            try
            {
                var questions = new QuestionsEdit
                {
                    SurveyId = surveyId,
                    Sections = await _surveyService.GetSections(surveyId),
                    Questions = await _surveyService.GetQuestions(surveyId),
                    QuestionOptions = await _surveyService.GetQuestionOptions(surveyId),
                    QuestionReferences = await _surveyService.GetQuestionReferences(surveyId)
                };
                return Ok(questions);
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "GetQuestions() error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("CreateQuestions")]
        public async Task<IActionResult> CreateQuestions(QuestionsEdit questions)
        {
            try
            {
                questions.CreatedOn = DateTime.Now;
                questions.CreatedBy = User.Identity.Name;
                await _service.CreateQuestions(questions);
                return Ok();
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "CreateQuestions() error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpPut]
        [Route("UpdateQuestions")]
        public async Task<IActionResult> UpdateQuestions(QuestionsEdit questions)
        {
            try
            {
                questions.ModifiedOn = DateTime.Now;
                questions.ModifiedBy = User.Identity.Name;
                await _service.UpdateQuestions(questions);
                return Ok();
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "UpdateQuestions() error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("GetResponses/{surveyId:guid}")]
        public async Task<IActionResult> GetResponses(Guid surveyId)
        {
            try
            {
                return Ok(await _service.GetResponses(surveyId));
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "GetResponses() error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpPatch]
        [Route("RejectResponse/{responseId:guid}")]
        public async Task<IActionResult> RejectResponse(Guid responseId)
        {
            try
            {
                await _service.RejectResponse(responseId);
                return Ok();
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "RejectResponse(Guid responseId) error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                return Ok(await _service.GetUsers());
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "GetUsers() error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("CreateUser")]
        [Obsolete("Method is deprecated, it no longer use")]
        public async Task<IActionResult> CreateUser(UserRoleDto user)
        {
            user.Id = Guid.NewGuid();
            user.CreatedOn = DateTime.Now;
            user.CreatedBy = User.Identity.Name;
            await _service.CreateUser(user);
            return Ok(user);
        }

        [HttpPut]
        [Route("UpdateUser")]
        [Obsolete("Method is deprecated, it no longer use")]
        public async Task<IActionResult> UpdateUser(UserRoleDto user)
        {
            user.ModifiedOn = DateTime.Now;
            user.ModifiedBy = User.Identity.Name;
            await _service.UpdateUser(user);
            return Ok(user);
        }

        [HttpDelete]
        [Route("DeleteUser/{id:guid}")]
        [Obsolete("Method is deprecated, it no longer use")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _service.DeleteUser(id);
            return Ok();
        }

        [HttpGet]
        [Route("GetCycleDates")]
        public async Task<IActionResult> GetCycleDates()
        {
            try
            {
                return Ok(await _service.GetCycleDates());
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "GetCycleDates() error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("DeleteSurvey/{id:guid}")]
        public async Task<IActionResult> DeleteSurvey(Guid id)
        {
            await _service.DeleteSurvey(id);
            return Ok();
        }

        [HttpPost]
        [Route("GetSearchUserResults")]
        public async Task<IActionResult> GetSearchUserResults(SearchUserRequest request)
        {
            try
            {
                return Ok(await _service.GetSearchUserResults(request));
            }
            catch (Exception ex)
            {
                fileLogger.Error(ex);
                await _databaseLogService.NLogError(ex, "GetSearchUserResults error", "AdminController");
                throw;
            }
        }
    }
}
