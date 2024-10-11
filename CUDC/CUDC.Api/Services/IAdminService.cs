using CUDC.Common.Dtos;
using CUDC.Common.Dtos.UserSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CUDC.Api.Services
{
    public interface IAdminService
    {
        IEnumerable<SurveyDto> GetSurveys();

        SurveyDto GetSurvey(Guid surveyId);

        void CreateSurvey(SurveyDto survey);

        void UpdateSurvey(SurveyDto survey);

        void CopySurvey(CopySurveyRequest request);

        void CreateQuestions(QuestionsEdit questions);

        void UpdateQuestions(QuestionsEdit questions);

        IEnumerable<ResponseDto> GetResponses(Guid surveyId);

        void RejectResponse(Guid responseId);

        IEnumerable<UserRoleDto> GetUsers();

        void CreateUser(UserRoleDto user);

        void UpdateUser(UserRoleDto user);

        void DeleteUser(Guid id);

        IEnumerable<CycleDateDto> GetCycleDates();

        void DeleteSurvey(Guid id);
        IEnumerable<SearchUserResult> GetSearchUserResults(SearchUserRequest request); 
    }
}
