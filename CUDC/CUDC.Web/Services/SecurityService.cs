using CUDC.Common.Dtos;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace CUDC.Web.Services
{
    public class SecurityService : ServiceBase, ISecurityService
    {
        private readonly IConfiguration _config;

        public SecurityService(IConfiguration config) : base(config)
        {
            _config = config;
        }

        public async Task<IEnumerable<RoleDto>> GetRoles()
        {
            var requestUri = WebApiUrl("Security/GetRoles");
            return await GetAsync<List<RoleDto>>(requestUri);
        }

        public async Task<UserRoleDto> GetUserRole(string userId)
        {
            var requestUri = WebApiUrl($"Security/{HttpUtility.UrlEncode(userId)}/GetUserRole");
            return await GetAsync<UserRoleDto>(requestUri);
        }

        public async Task<SurveyOwnerResponse> GetSurveyOwner(SurveyOwnerRequest request)
        {
            var requestUri = WebApiUrl("Security/GetSurveyOwner");
            return await PostAsync<SurveyOwnerResponse>(requestUri, request);
        }

        public async Task<bool> IsSeUser(string userId)
        {
            var requestUri = WebApiUrl($"Security/{HttpUtility.UrlEncode(userId)}/IsSeUser");
            return await GetAsync<bool>(requestUri);
        }

        private string WebApiUrl(string path)
        {
            return $"{_config["WebApiBaseUrl"]}{path}";
        }

        public async Task<bool> IsClaimedCU(int charterNumber, string userId)
        {
            var requestUri = WebApiUrl($"Security/{charterNumber}/{HttpUtility.UrlEncode(userId)}/IsClaimedCU");
            return await GetAsync<bool>(requestUri);
        }

        public async Task<IEnumerable<UserRoleDto>> GetNcuaUsers(int region, int surveyType)
        {
            var requestUri = WebApiUrl($"Security/{region}/{surveyType}/GetNcuaUsers");
            return await GetAsync<List<UserRoleDto>>(requestUri);
        }
    }
}
