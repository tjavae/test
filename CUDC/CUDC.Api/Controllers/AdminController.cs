using CUDC.Api.Services;
using CUDC.Common.Dtos;
using CUDC.Common.Dtos.UserSearch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CUDC.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _service;

        public AdminController(IAdminService service)
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
        [Route("GetSurvey/{surveyId:guid}")]
        public SurveyDto GetSurvey(Guid surveyId)
        {
            return _service.GetSurvey(surveyId);
        }

        [HttpPost]
        [Route("CreateSurvey")]
        public void CreateSurvey(SurveyDto survey)
        {
            _service.CreateSurvey(survey);
        }

        [HttpPost]
        [Route("UpdateSurvey")]
        public void UpdateSurvey(SurveyDto survey)
        {
            _service.UpdateSurvey(survey);
        }

        [HttpPost]
        [Route("CopySurvey")]
        public void CopySurvey(CopySurveyRequest request)
        {
            _service.CopySurvey(request);
        }

        [HttpPost]
        [Route("CreateQuestions")]
        public void CreateQuestions(QuestionsEdit questions)
        {
            _service.CreateQuestions(questions);
        }

        [HttpPost]
        [Route("UpdateQuestions")]
        public void UpdateQuestions(QuestionsEdit questions)
        {
            _service.UpdateQuestions(questions);
        }

        [HttpGet]
        [Route("GetResponses/{surveyId:guid}")]
        public IEnumerable<ResponseDto> GetResponses(Guid surveyId)
        {
            return _service.GetResponses(surveyId);
        }

        [HttpGet]
        [Route("RejectResponse/{responseId:guid}")]
        public void RejectResponse(Guid responseId)
        {
            _service.RejectResponse(responseId);
        }

        [HttpGet]
        [Route("GetUsers")]
        public IEnumerable<UserRoleDto> GetUsers()
        {
            return _service.GetUsers();
        }

        [HttpPost]
        [Route("CreateUser")]
        public void CreateUser(UserRoleDto user)
        {
            _service.CreateUser(user);
        }

        [HttpPost]
        [Route("UpdateUser")]
        public void UpdateUser(UserRoleDto user)
        {
            _service.UpdateUser(user);
        }

        [HttpGet]
        [Route("DeleteUser/{id:guid}")]
        public void DeleteUser(Guid id)
        {
            _service.DeleteUser(id);
        }

        [HttpGet]
        [Route("GetCycleDates")]
        public IEnumerable<CycleDateDto> GetCycleDates()
        {
            return _service.GetCycleDates();
        }

        [HttpGet]
        [Route("DeleteSurvey/{id:guid}")]
        public void DeleteSurvey(Guid id)
        {
            _service.DeleteSurvey(id);
        }

        [HttpPost]
        [Route("GetSearchUserResults")]
        public IEnumerable<SearchUserResult> GetSearchUserResults(SearchUserRequest request)
        {
            return _service.GetSearchUserResults(request);
        }
    }
}
