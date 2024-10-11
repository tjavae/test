using CUDC.Common.Dtos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Void = CUDC.Common.Util.Void;

namespace CUDC.Web.Services
{
    public class SurveyService : ServiceBase, ISurveyService
    {
        private readonly IConfiguration _config;

        public SurveyService(IConfiguration config) : base(config)
        {
            _config = config;
        }

        public async Task<IEnumerable<SurveyDto>> GetSurveys()
        {
            var requestUri = WebApiUrl("Survey/GetSurveys");
            return await GetAsync<List<SurveyDto>>(requestUri);
        }

        public async Task<IEnumerable<SectionDto>> GetSections(Guid surveyId)
        {
            var requestUri = WebApiUrl($"Survey/{surveyId}/GetSections");
            return await GetAsync<List<SectionDto>>(requestUri);
        }

        public async Task<IEnumerable<QuestionDto>> GetQuestions(Guid surveyId)
        {
            var requestUri = WebApiUrl($"Survey/{surveyId}/GetQuestions");
            return await GetAsync<List<QuestionDto>>(requestUri);
        }

        public async Task<IEnumerable<QuestionOptionDto>> GetQuestionOptions(Guid surveyId)
        {
            var requestUri = WebApiUrl($"Survey/{surveyId}/GetQuestionOptions");
            return await GetAsync<List<QuestionOptionDto>>(requestUri);
        }

        public async Task<ResponseDto> GetAnswers(AnswersRequest request)
        {
            var requestUri = WebApiUrl("Survey/GetAnswers");
            return await PostAsync<ResponseDto>(requestUri, request);
        }

        public async Task SetAnswers(ResponseDto answers)
        {
            var requestUri = WebApiUrl("Survey/SetAnswers");
            await PostAsync<Void>(requestUri, answers);
        }

        public async Task SubmitAnswers(SubmitRequest request)
        {
            var requestUri = WebApiUrl("Survey/SubmitAnswers");
            await PostAsync<Void>(requestUri, request);
        }

        private string WebApiUrl(string path)
        {
            return $"{_config["WebApiBaseUrl"]}{path}";
        }

        public async Task UnlockSurvey(SubmitRequest request)
        {
            var requestUri = WebApiUrl("Survey/UnlockSurvey");
            await PostAsync<Void>(requestUri, request);
        }

        public async Task TransferOwnership(SubmitRequest request)
        {
            var requestUri = WebApiUrl("Survey/TransferOwnership");
            await PostAsync<Void>(requestUri, request);
        }

        public async Task<IEnumerable<QuestionReferenceDto>> GetQuestionReferences(Guid surveyId)
        {
            var requestUri = WebApiUrl($"Survey/{surveyId}/GetQuestionReferences");
            return await GetAsync<List<QuestionReferenceDto>>(requestUri);
        }
    }
}
