using CUDC.Common.Dtos;
using CUDC.Common.Dtos.CuSearch;
using CUDC.Common.Enums;
using CUDC.Web.Controllers;
using CUDC.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace CUDC.WebTest
{
    public class SecurityControllerTest
    {
        [Theory(DisplayName = "GetSurveyOwnerTest")]
        [InlineData("A0000000-0000-0000-0000-000000000002", 12, "")]
        public async Task GetSurveyOwnerTest(string surveyId, int cuNumber, string userId = null)
        {   
            var mockSecurityService = new Mock<ISecurityService>();
            var mockLogger = new NullLogger<SecurityController>();
            var expectedHasOwner = true;
            var mockILogService = new Mock<ILogService>();
            mockSecurityService.Setup(repo => repo.GetSurveyOwner(It.IsAny<SurveyOwnerRequest>())).ReturnsAsync(GetSurveyOwner());
            var controller = new SecurityController(mockSecurityService.Object, (ILogger<SecurityController>)mockLogger, mockILogService.Object);

            // Act
            var result = await controller.GetSurveyOwner(new Guid(surveyId), cuNumber, userId);

            // Assert
            var okResult = result as OkObjectResult;            
            SurveyOwnerResponse response = (SurveyOwnerResponse)okResult.Value;            
            Assert.Equal(expectedHasOwner, response.HasOwner);
        }


        //IsClaimedCU
        [Theory(DisplayName = "IsClaimedCUTest")]
        [InlineData(12, "hqnt\test")]
        public async Task IsClaimedCUTest(int charterNumber, string userId) { 
            var mockSecurityService = new Mock<ISecurityService>();
            var mockLogger = new NullLogger<SecurityController>();
            var expectedResult = true;
            var mockILogService = new Mock<ILogService>();
            mockSecurityService.Setup(repo => repo.IsClaimedCU(charterNumber, userId)).ReturnsAsync(expectedResult);
            var controller = new SecurityController(mockSecurityService.Object, (ILogger<SecurityController>)mockLogger, mockILogService.Object);

            // Act
            var result = await controller.IsClaimedCU(charterNumber, userId);

            // Assert
            var okResult = result as OkObjectResult;            
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expectedResult, okResult.Value);
        }

        [Theory(DisplayName = "GetNcuaUsersTest")]
        [InlineData(1, 1)]
        public async Task GetNcuaUsersTest(int region, int surveyType)
        {
            var mockSecurityService = new Mock<ISecurityService>();
            var mockLogger = new NullLogger<SecurityController>();
            var mockILogService = new Mock<ILogService>();
            mockSecurityService.Setup(repo => repo.GetNcuaUsers(region, surveyType)).ReturnsAsync(GetMockUsers());
            var controller = new SecurityController(mockSecurityService.Object, (ILogger<SecurityController>)mockLogger, mockILogService.Object);

            // Act
            var result = await controller.GetNcuaUsers(region, surveyType);

            // Assert
            var okResult = result as OkObjectResult;
            var users = new List<UserRoleDto>((IEnumerable<UserRoleDto>)okResult.Value);
            var expectedName = "F Test 1";
            Assert.NotEmpty(users);
            Assert.Equal(3, users.Count);            
            Assert.Equal(expectedName, users.Find(u => u.EmployeeNumber == "1000").FirstName);
        }

        private SurveyOwnerResponse GetSurveyOwner()
        {
            var sOwnerResp = new SurveyOwnerResponse
            {
                HasOwner = true,
                FirstName = "TestF",
                LastName = "TestL"
            };
            return sOwnerResp;
        }

        private List<UserRoleDto> GetMockUsers()
        {
            var users = new List<UserRoleDto>();
            var P1 = new UserPermission() 
            {
                Action = "Edit",
                Module = "CUDC",
                GroupName = "Examiner",
                Region = 1,
                Se = "A"
            };
            var P2= new UserPermission()
            {
                Action = "Edit",
                Module = "CUDC",
                GroupName = "Examiner",
                Region = 1,
                Se = "B"
            };
            var P3 = new UserPermission()
            {
                Action = "Edit",
                Module = "CUDC",
                GroupName = "DOS/PCO",
                Region = 1,
                Se = "C"
            };
            UserPermission[] user1Permissions = new UserPermission[] { P1, P2 };
            UserPermission[] user2Permissions = new UserPermission[] { P3 };
            users.Add(new UserRoleDto()
            {
                EmployeeNumber = "1000",
                FirstName = "F Test 1",
                LastName = "L test 1",
                Permissions = user1Permissions
            });
            users.Add(new UserRoleDto()
            {
                EmployeeNumber = "1001",
                FirstName = "F Test 2",
                LastName = "L test 2",
                Permissions = user1Permissions
            });
            users.Add(new UserRoleDto()
            {
                EmployeeNumber = "1003",
                FirstName = "F Test 3",
                LastName = "L test 3",
                Permissions = user2Permissions
            });
            return users;
        }
    }
}
