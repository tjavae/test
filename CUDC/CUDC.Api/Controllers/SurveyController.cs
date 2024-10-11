using CUDC.Api.Services;
using CUDC.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CUDC.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _service;

        public SurveyController(ISurveyService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetSurveys")]
        public IEnumerable<SurveyDto> GetSurveys()
        {
            return _service.GetSurveys();
        }

        [HttpGet]
        [Route("{surveyId:guid}/GetSections")]
        public IEnumerable<SectionDto> GetSections(Guid surveyId)
        {
            return _service.GetSections(surveyId);
        }

        [HttpGet]
        [Route("{surveyId:guid}/GetQuestions")]
        public IEnumerable<QuestionDto> GetQuestions(Guid surveyId)
        {
            return _service.GetQuestions(surveyId);
        }

        [HttpGet]
        [Route("{surveyId:guid}/GetQuestionOptions")]
        public IEnumerable<QuestionOptionDto> GetQuestionOptions(Guid surveyId)
        {
            return _service.GetQuestionOptions(surveyId);
        }

        [HttpPost]
        [Route("GetAnswers")]
        public ResponseDto GetAnswers(AnswersRequest request)
        {
            return _service.GetAnswers(request);
        }

        [HttpPost]
        [Route("SetAnswers")]
        public void SetAnswers(ResponseDto response)
        {
            _service.SetAnswers(response);
        }

        [HttpPost]
        [Route("SubmitAnswers")]
        public void SubmitAnswers(SubmitRequest request)
        {
            _service.SubmitAnswers(request);
        }

        [HttpPost]
        [Route("UnlockSurvey")]
        public void UnlockSurvey(SubmitRequest request)
        {
            _service.UnlockSurvey(request);
        }

        [HttpPost]
        [Route("TransferOwnership")]
        public void TransferOwnership(SubmitRequest request)
        {
            _service.TransferOwnership(request);
        }

        [HttpGet]
        [Route("{surveyGuId:guid}/GetQuestionReferences")]
        public IEnumerable<QuestionReferenceDto> GetQuestionReferences(Guid surveyGuId)
        {
            return _service.GetQuestionReferences(surveyGuId);
        }
    }
}