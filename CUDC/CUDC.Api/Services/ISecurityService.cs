using CUDC.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CUDC.Api.Services
{
    public interface ISecurityService
    {
        IEnumerable<RoleDto> GetRoles();

        UserRoleDto GetUserRole(string userId);

        SurveyOwnerResponse GetSurveyOwner(SurveyOwnerRequest request);

        bool IsSeUser(string userId);

        bool IsClaimedCU(int charterNumber, string userId);

        IEnumerable<UserRoleDto> GetNcuaUsers(int region, int surveyType);
    }
}
