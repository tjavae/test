using CUDC.Common.Enums;
using CUDC.Web.Services;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CUDC.Web.Security
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        private readonly ISecurityService _service;

        public ClaimsTransformer(ISecurityService service)
        {
            _service = service;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var claimInfo = (ClaimsIdentity)principal.Identity;
            var userRole = await _service.GetUserRole(claimInfo.Name);
            if (userRole != null)
            {
                var claim = new Claim(claimInfo.RoleClaimType, userRole.Role.ToString());
                claimInfo.AddClaim(claim);
            }
            else //Default is 'User' role
            {
                var claim = new Claim(claimInfo.RoleClaimType, Role.User.ToString());
                claimInfo.AddClaim(claim);
            }
            return principal;
        }
    }
}
