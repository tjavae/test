using CUDC.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CUDC.Web.Services
{
    public interface ISurveyService
    {
        Task<IEnumerable<SurveyDto>> GetSurveys();

        Task<IEnumerable<SectionDto>> GetSections(Guid surveyId);

        Task<IEnumerable<QuestionDto>> GetQuestions(Guid surveyId);

        Task<IEnumerable<QuestionOptionDto>> GetQuestionOptions(Guid surveyId);

        Task<ResponseDto> GetAnswers(AnswersRequest request);

        Task SetAnswers(ResponseDto answers);

        Task SubmitAnswers(SubmitRequest request);

        Task UnlockSurvey(SubmitRequest request);

        Task TransferOwnership(SubmitRequest request);

        Task<IEnumerable<QuestionReferenceDto>> GetQuestionReferences(Guid surveyId);
    }
}
