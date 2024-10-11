using CUDC.Common.Dtos;
using CUDC.Common.Dtos.UserSearch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CUDC.Web.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<SurveyDto>> GetSurveys();

        Task<SurveyDto> GetSurvey(Guid surveyId);

        Task CreateSurvey(SurveyDto survey);

        Task UpdateSurvey(SurveyDto survey);

        Task CopySurvey(CopySurveyRequest request);

        Task CreateQuestions(QuestionsEdit questions);

        Task UpdateQuestions(QuestionsEdit questions);

        Task<IEnumerable<ResponseDto>> GetResponses(Guid surveyId);

        Task RejectResponse(Guid responseId);

        Task<IEnumerable<UserRoleDto>> GetUsers();

        Task CreateUser(UserRoleDto user);

        Task UpdateUser(UserRoleDto user);

        Task DeleteUser(Guid id);

        Task<IEnumerable<CycleDateDto>> GetCycleDates();
        Task DeleteSurvey(Guid id);

        Task<IEnumerable<SearchUserResult>> GetSearchUserResults(SearchUserRequest request);
    }
}
