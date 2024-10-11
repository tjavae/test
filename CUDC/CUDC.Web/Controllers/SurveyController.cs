using CUDC.Common.Dtos;
using CUDC.Common.Enums;
using CUDC.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CUDC.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _service;
        private readonly ICreditUnionService _creditUnionService;
        internal static readonly Logger fileLogger = LogManager.GetLogger("fileLogger");
        private readonly ILogService _databaseLogService;

        public SurveyController(ISurveyService service, ICreditUnionService creditUnionService, ILogService logService)
        {
            _service = service;
            _creditUnionService = creditUnionService;
            _databaseLogService = logService;
        }

        [HttpGet]
        [Route("{surveyType}/IsSurveyActive")]
        public async Task<IActionResult> IsSurveyActive(SurveyType surveyType)
        {
            try
            {
                var surveys = await _service.GetSurveys(); //Get all active surveys
                return Ok(surveys.FirstOrDefault(x => (x.Type == surveyType && x.IsActive)) != null);
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "IsSurveyActive() error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("{charterNumber}/{surveyType}/GetSurveyInfo")]
        public async Task<IActionResult> GetSurveyInfo(int charterNumber, SurveyType surveyType, string userId = null)
        {
            try
            {
                var basicInfo = await _creditUnionService.GetCreditUnionBasicInfo(charterNumber);
                var surveys = await _service.GetSurveys();
                var surveyId = surveys.FirstOrDefault(x => x.IsActive && x.Type == surveyType)!.Id;              

                Guid preSurveyId = Guid.Empty;

                if (surveyType == SurveyType.SE)
                {                    
                    preSurveyId = surveys.FirstOrDefault(x => x.IsActive && x.Type == SurveyType.CAT)!.Id;
                }
                else if (surveyType == SurveyType.DOS)
                {
                    preSurveyId =  surveys.FirstOrDefault(x => x.IsActive && x.Type == SurveyType.SE)!.Id;                   
                }

                var hasPreSubmitedSurvey = false;
                if (surveyType == SurveyType.CAT)
                {
                    hasPreSubmitedSurvey = false;
                }
                else
                {
                    var Answers = basicInfo == null ? null : await _service.GetAnswers(new AnswersRequest
                    {
                        SurveyId = preSurveyId,
                        UserId = userId ?? User.Identity.Name,
                        CuNumber = basicInfo.CharterNumber,
                        JoinNumber = basicInfo.JoinNumber
                    });
                    if (Answers != null && Answers.SubmittedOn != null)
                    {
                        hasPreSubmitedSurvey = true;
                    }
                };

                var surveyInfo = new SurveyInfo
                {
                    BasicInfo = basicInfo,
                    Survey = surveys.FirstOrDefault(x => x.IsActive && x.Type == surveyType),
                    Sections = await _service.GetSections(surveyId),
                    Questions = await _service.GetQuestions(surveyId),
                    QuestionOptions = await _service.GetQuestionOptions(surveyId),
                    Answers = basicInfo == null ? null : await _service.GetAnswers(new AnswersRequest
                    {
                        SurveyId = surveyId,
                        UserId = userId ?? User.Identity.Name,
                        CuNumber = basicInfo.CharterNumber,
                        JoinNumber = basicInfo.JoinNumber
                    }),
                    QuestionReferences = await _service.GetQuestionReferences(surveyId),
                    HasPreSubmitedSurvey = hasPreSubmitedSurvey
                };

                return Ok(surveyInfo);
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "GetSurveyInfo() error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("{charterNumber}/{surveyType}/{userId}/GetSurveyInfoAdminView")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSurveyInfoAdminView(int charterNumber, SurveyType surveyType, string userId)
        {
            try
            {
                return await GetSurveyInfo(charterNumber, surveyType, userId);
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "GetSurveyInfoAdminView() error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("{charterNumber}/{surveyType}/{cycleDate}/GetSurveyInfoByCycle")]
        public async Task<IActionResult> GetSurveyInfoByCycle(int charterNumber, SurveyType surveyType, string cycleDate, string userId = null)
        {
            try
            {
                var basicInfo = await _creditUnionService.GetCreditUnionBasicInfo(charterNumber);
                var surveys = await _service.GetSurveys();
                SurveyDto surveyDto;
                var preSurveyDto = new SurveyDto();
                if (cycleDate == null)
                {
                    surveyDto = surveys.FirstOrDefault(x => (x.IsActive && x.Type == surveyType));
                    if (surveyType == SurveyType.SE)
                    {
                        preSurveyDto = surveys.FirstOrDefault(x => (x.IsActive && x.Type == SurveyType.CAT));
                    }
                    else if (surveyType == SurveyType.DOS)
                    {
                        preSurveyDto = surveys.FirstOrDefault(x => (x.IsActive && x.Type == SurveyType.SE));
                    }
                }
                else
                {                   
                    surveyDto = surveys.FirstOrDefault(x =>
                        x.Type == surveyType &&
                        x.StartDate != null &&
                        x.StartDate.Value.ToString("yyyyMMdd") == cycleDate                    
                    );
                    if (surveyType == SurveyType.SE)
                    {
                        preSurveyDto = surveys.FirstOrDefault(x =>
                            x.Type == SurveyType.CAT &&
                            x.StartDate != null &&
                            x.StartDate.Value.ToString("yyyyMMdd") == cycleDate
                        );
                    }
                    else if (surveyType == SurveyType.DOS)
                    {
                        preSurveyDto = surveys.FirstOrDefault(x =>
                            x.Type == SurveyType.SE &&
                            x.StartDate != null &&
                            x.StartDate.Value.ToString("yyyyMMdd") == cycleDate
                        );
                    }
                }
                if (surveyDto != null)
                {
                    var hasPreSubmitedSurvey = false;
                    if (surveyType == SurveyType.CAT)
                    {
                        hasPreSubmitedSurvey = false;
                    }
                    else
                    {
                        if (preSurveyDto == null) 
                        { 
                            return Ok(null); 
                        }
                        else {
                            var Answers = basicInfo == null ? null : await _service.GetAnswers(new AnswersRequest
                            {
                                SurveyId = preSurveyDto.Id,
                                UserId = userId ?? User.Identity.Name,
                                CuNumber = basicInfo.CharterNumber,
                                JoinNumber = basicInfo.JoinNumber
                            });
                            if (Answers != null && Answers.SubmittedOn != null)
                            {
                                hasPreSubmitedSurvey = true;
                            }
                        }                        
                    };

                    var surveyInfo = new SurveyInfo
                    {
                        BasicInfo = basicInfo,
                        Survey = surveyDto,
                        Sections = await _service.GetSections(surveyDto.Id),
                        Questions = await _service.GetQuestions(surveyDto.Id),
                        QuestionOptions = await _service.GetQuestionOptions(surveyDto.Id),
                        Answers = basicInfo == null ? null : await _service.GetAnswers(new AnswersRequest
                        {
                            SurveyId = surveyDto.Id,
                            UserId = userId ?? User.Identity.Name,
                            CuNumber = basicInfo.CharterNumber,
                            JoinNumber = basicInfo.JoinNumber
                        }),
                        QuestionReferences = await _service.GetQuestionReferences(surveyDto.Id),
                        HasPreSubmitedSurvey = hasPreSubmitedSurvey
                    };
                    return Ok(surveyInfo);
                }
                else
                {
                    return Ok(null);
                }
            }
            catch (Exception ex)
            {
                fileLogger.Error(ex);
                await _databaseLogService.NLogError(ex, "GetSurveyInfoByCycle() error", MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        [HttpGet]
        [Route("{charterNumber}/{surveyType}/{userId}/{cycleDate}/GetSurveyInfoAdminViewByCycle")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSurveyInfoAdminViewByCycle(int charterNumber, SurveyType surveyType, string userId, string cycleDate)
        {
            try
            {
                return await GetSurveyInfoByCycle(charterNumber, surveyType, cycleDate, userId);
            }
            catch (Exception ex)
            {
                fileLogger.Error(ex);
                await _databaseLogService.NLogError(ex, "GetSurveyInfoAdminViewByCycle() error", MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        [HttpPost]
        [Route("{charterNumber}/{surveyType}/SaveAnswers")]
        public async Task<IActionResult> SaveAnswers(int charterNumber, SurveyType surveyType, ResponseDto answers)
        {
            try
            {
                var basicInfo = await _creditUnionService.GetCreditUnionBasicInfo(charterNumber);
                var surveys = await _service.GetSurveys();
                var survey = surveys.FirstOrDefault(x => x.Type == surveyType && x.IsActive);
                if (survey != null) 
                {
                    answers.SurveyId = survey.Id;
                    answers.UserId = User?.Identity?.Name;
                    answers.CuNumber = basicInfo.CharterNumber;
                    answers.JoinNumber = basicInfo.JoinNumber;
                    await _service.SetAnswers(answers);
                    return Ok();
                }
                else 
                { 
                    return Ok(null); 
                }
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "SaveAnswers error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("{responseId:guid}/SubmitAnswers")]
        public async Task<IActionResult> SubmitAnswers(Guid responseId)
        {
            try
            {
                await _service.SubmitAnswers(new SubmitRequest
                {
                    ResponseId = responseId,
                    UserId = User?.Identity?.Name
                });
                return Ok();
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "SubmitAnswers() error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("{responseId:guid}/UnlockSurvey")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnlockSurvey(Guid responseId)
        {
            try
            {
                await _service.UnlockSurvey(new SubmitRequest
                {
                    ResponseId = responseId,
                    UserId = User?.Identity?.Name
                });
                return Ok();
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "UnlockSurvey() error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("{responseId:guid}/{userId}/TransferOwnership")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TransferOwnership(Guid responseId, string userId)
        {
            try
            {
                await _service.TransferOwnership(new SubmitRequest
                {
                    ResponseId = responseId,
                    UserId = userId
                });
                return Ok();
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "TransferOwnership() error", MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(ex);
                throw;
            }
        }
    }
}
