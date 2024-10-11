using CUDC.Api.Services;
using CUDC.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CUDC.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _service;

        public SecurityController(ISecurityService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetRoles")]
        public IEnumerable<RoleDto> GetRoles()
        {
            return _service.GetRoles();
        }

        [HttpGet]
        [Route("{userId}/GetUserRole")]
        public UserRoleDto GetUserRole(string userId)
        {
            return _service.GetUserRole(userId);
        }

        [HttpPost]
        [Route("GetSurveyOwner")]
        public SurveyOwnerResponse GetSurveyOwner(SurveyOwnerRequest request)
        {
            return _service.GetSurveyOwner(request);
        }

        [HttpGet]
        [Route("{userId}/IsSeUser")]
        public bool IsSeUser(string userId)
        {
            return _service.IsSeUser(userId);
        }

        [HttpGet]
        [Route("{charterNumber}/{userId}/IsClaimedCU")]
        public bool IsClaimedCU(int charterNumber, string userId)
        {
            return _service.IsClaimedCU(charterNumber, userId);
        }

        [HttpGet]
        [Route("{region}/{surveyType}/GetNcuaUsers")]
        public IEnumerable<UserRoleDto> GetNcuaUsers(int region, int surveyType)
        {
            return _service.GetNcuaUsers(region, surveyType);
        }
    }
}
