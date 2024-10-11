using CUDC.Common.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CUDC.Web.Services
{
    public interface ISecurityService
    {
        Task<IEnumerable<RoleDto>> GetRoles();

        Task<UserRoleDto> GetUserRole(string userId);

        Task<SurveyOwnerResponse> GetSurveyOwner(SurveyOwnerRequest request);

        Task<bool> IsSeUser(string userId);

        Task<bool> IsClaimedCU(int charterNumber, string userId);

        Task<IEnumerable<UserRoleDto>> GetNcuaUsers(int region, int surveyType);
    }
}
