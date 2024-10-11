using CUDC.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CUDC.Api.Services
{
    public interface ISurveyService
    {
        IEnumerable<SurveyDto> GetSurveys();

        IEnumerable<SectionDto> GetSections(Guid surveyId);

        IEnumerable<QuestionDto> GetQuestions(Guid surveyId);

        IEnumerable<QuestionOptionDto> GetQuestionOptions(Guid surveyId);

        IEnumerable<QuestionReferenceDto> GetQuestionReferences(Guid surveyGuId);

        ResponseDto GetAnswers(AnswersRequest request);

        void SetAnswers(ResponseDto response);

        void SubmitAnswers(SubmitRequest request);

        void UnlockSurvey(SubmitRequest request);

        void TransferOwnership(SubmitRequest request);
       
    }
}
