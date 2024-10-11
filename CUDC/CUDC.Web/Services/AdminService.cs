using CUDC.Common.Dtos;
using CUDC.Common.Dtos.UserSearch;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Void = CUDC.Common.Util.Void;

namespace CUDC.Web.Services
{
    public class AdminService : ServiceBase, IAdminService
    {
        private readonly IConfiguration _config;

        public AdminService(IConfiguration config) : base(config)
        {
            _config = config;
        }

        public async Task<IEnumerable<SurveyDto>> GetSurveys()
        {
            var requestUri = WebApiUrl("Admin/GetSurveys");
            return await GetAsync<List<SurveyDto>>(requestUri);
        }

        public async Task<SurveyDto> GetSurvey(Guid surveyId)
        {
            var requestUri = WebApiUrl($"Admin/GetSurvey/{surveyId}");
            return await GetAsync<SurveyDto>(requestUri);
        }

        public async Task CreateSurvey(SurveyDto survey)
        {
            var requestUri = WebApiUrl("Admin/CreateSurvey");
            await PostAsync<Void>(requestUri, survey);
        }

        public async Task UpdateSurvey(SurveyDto survey)
        {
            var requestUri = WebApiUrl("Admin/UpdateSurvey");
            await PostAsync<Void>(requestUri, survey);
        }

        public async Task CopySurvey(CopySurveyRequest request)
        {
            var requestUri = WebApiUrl($"Admin/CopySurvey");
            await PostAsync<Void>(requestUri, request);
        }

        public async Task CreateQuestions(QuestionsEdit questions)
        {
            var requestUri = WebApiUrl("Admin/CreateQuestions");
            await PostAsync<Void>(requestUri, questions);
        }

        public async Task UpdateQuestions(QuestionsEdit questions)
        {
            var requestUri = WebApiUrl("Admin/UpdateQuestions");
            await PostAsync<Void>(requestUri, questions);
        }

        public async Task<IEnumerable<ResponseDto>> GetResponses(Guid surveyId)
        {
            var requestUri = WebApiUrl($"Admin/GetResponses/{surveyId}");
            return await GetAsync<List<ResponseDto>>(requestUri);
        }

        public async Task RejectResponse(Guid responseId)
        {
            var requestUri = WebApiUrl($"Admin/RejectResponse/{responseId}");
            await GetAsync<Void>(requestUri);
        }

        public async Task<IEnumerable<UserRoleDto>> GetUsers()
        {
            var requestUri = WebApiUrl("Admin/GetUsers");
            return await GetAsync<List<UserRoleDto>>(requestUri);
        }

        public async Task CreateUser(UserRoleDto user)
        {
            var requestUri = WebApiUrl("Admin/CreateUser");
            await PostAsync<Void>(requestUri, user);
        }

        public async Task UpdateUser(UserRoleDto user)
        {
            var requestUri = WebApiUrl("Admin/UpdateUser");
            await PostAsync<Void>(requestUri, user);
        }

        public async Task DeleteUser(Guid id)
        {
            var requestUri = WebApiUrl($"Admin/DeleteUser/{id}");
            await GetAsync<Void>(requestUri);
        }

        private string WebApiUrl(string path)
        {
            return $"{_config["WebApiBaseUrl"]}{path}";
        }

        public async Task<IEnumerable<CycleDateDto>> GetCycleDates()
        {
            var requestUri = WebApiUrl("Admin/GetCycleDates");
            return await GetAsync<List<CycleDateDto>>(requestUri);
        }

        public async Task DeleteSurvey(Guid id)
        {
            var requestUri = WebApiUrl($"Admin/DeleteSurvey/{id}");
            await GetAsync<Void>(requestUri);
        }

        public async Task<IEnumerable<SearchUserResult>> GetSearchUserResults(SearchUserRequest request)
        {
            var requestUri = WebApiUrl("Admin/GetSearchUserResults");
            return await PostAsync<List<SearchUserResult>>(requestUri, request);
        }

    }
}
