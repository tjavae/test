using CUDC.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CUDC.Web.Controllers
{
    [Route("health-check")]
    [ApiController]
    [AllowAnonymous]
    public class HealthCheckController : ControllerBase
    {
        private readonly ISurveyService _surveyService;
        private readonly ICreditUnionService _creditUnionService;
        private readonly ISecurityService _securityService;
        private readonly IConfiguration _config;

        public HealthCheckController(ISurveyService surveyService, ICreditUnionService creditUnionService, ISecurityService securityService, IConfiguration config)
        {
            _surveyService = surveyService;
            _creditUnionService = creditUnionService;
            _securityService = securityService;
            _config = config;
        }
                
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Below are 3 services and associated operation used to check if each service returns valid data - 3 tests in total 
            //      Service                         Operation      
            //      creditUnionService                      GetCreditUnionBasicInfo                          
            //      surveyService                   GetSurveys             
            //      securityService                 GetRoles

            string errorMessage = "";
            Boolean getMisServiceSuccess = false;
            Boolean getSurveyServiceSuccess = false;
            Boolean getSecurityServiceSuccess = false;

            var requestSecret = HttpContext.Request.Headers["ClientSecret"];
            var clientSecret = _config["ClientSecret"];

            if (clientSecret != requestSecret)
            {
                return StatusCode(403);
            }

            // Test #1 - Hit the  Credit Union Service
            try
            {
                // Get an instance of the  Credit Union service for Charter #1
                var searchResults = await _creditUnionService.GetCreditUnionBasicInfo(1);

                // Check if Charter #1 name is not empty, null, or contain white space
                if (!string.IsNullOrWhiteSpace(searchResults.Name))
                {
                    getMisServiceSuccess = true;
                }
                else
                {
                    errorMessage += $"CUDC failed to access the Credit Union Service. Unable to access GetCreditUnionBasicInfo for Charter #1: CU Name was empty. ";
                    getMisServiceSuccess = false;
                }
            }
            catch (Exception ex)
            {
                // If an error occurs, assume the webservice communication is not functioning, and report that the system is not healthy
                errorMessage += $"CUDC failed to access the Credit Union Service. Unable to access GetCreditUnionBasicInfo for Charter #1: {ex.Message}. ";
            }

            // Test #2 - Hit the Survey Service
            try
            {
                // Get an instance of the Survey service
                var surveys = await _surveyService.GetSurveys(); //Get all active surveys

                // Attempt to interact with the service and get data
                var surveyCount = surveys.Count();
                var surveyTitle = surveyCount>0? surveys.First().Title:"";

                // Check if there is at least one survey and if Survey #1 name is not empty, null, or contain white space  
                if (surveyCount > 0 && !string.IsNullOrWhiteSpace(surveyTitle) )
                {
                    getSurveyServiceSuccess = true;
                }
                else
                {
                    errorMessage += $"CUDC failed to access the Survey Service. Unable to access GetSurveys for Survey Count: Survey Count is not greater than 0, or access GetSurveys for Survey #1: Survey Title was empty. ";
                    getSurveyServiceSuccess = false;
                }
            }
            catch (Exception ex)
            {
                // If an error occurs, assume the webservice communication is not functioning, and report that the system is not healthy
                errorMessage += $"CUDC failed to access the Survey Service. Unable to access GetSurveys for Survey Count or Survey #1: {ex.Message}. ";
            }

            // Test #3 - Hit the Security Service
            try
            {
                // Get an instance of the Security service
                var roles = await _securityService.GetRoles();

                // Attempt to interact with the service and get data
                var expectedRoleFirst = "Admin";
                var expectedRoleSecond = "User";

                var actualRoleFirst = roles.ElementAt(0).Name;
                var actualRoleSecond = roles.ElementAt(1).Name;

                // Check if the first and second roles are "Admin" and "User"
                if (actualRoleFirst.Contains(expectedRoleFirst) && actualRoleSecond.Contains(expectedRoleSecond))
                {
                    getSecurityServiceSuccess = true;
                }
                else
                {
                    errorMessage += $"CUDC failed to access the Security Service. Unable to access GetRoles for Role #1: Role not equal to Admin, or access GetRoles for Role #2: Role not equal to User. ";
                    getSecurityServiceSuccess = false;
                }
            }
            catch (Exception ex)
            {
                // If an error occurs, assume the webservice communication is not functioning, and report that the system is not healthy
                errorMessage += $"CUDC failed to access the Security Service. Unable to access GetRoles for Role #1 and Role #2: {ex.Message}. ";
            }

            // Report on result of web service operations
            if (getMisServiceSuccess == false || getSurveyServiceSuccess == false || getSecurityServiceSuccess == false)
            {
                return Ok(new { HealthCheck = "Failed", Reason = errorMessage });
            }
            else
            {
                return Ok(new { HealthCheck = "OK" });
            }
        }
    }
}
