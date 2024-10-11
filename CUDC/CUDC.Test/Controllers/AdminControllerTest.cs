using CUDC.Common.Dtos;
using CUDC.Common.Enums;
using CUDC.Web.Controllers;
using CUDC.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace CUDC.WebTest
{
    public class AdminControllerTest
    {
        private static List<SurveyDto> surveys = new List<SurveyDto> {
            new SurveyDto()
            {
                Id = new Guid("A0000000-0000-0000-0000-000000000001"),
                Title = "Survey 1",
                CreatedBy = "Tester"

            },
            new SurveyDto()
            {
                Id = new Guid("A0000000-0000-0000-0000-000000000002"),
                Title = "Survey 2",
                CreatedBy = "Tester2"
            }
        };

        [Fact]
        public async Task GetSurveysTest()
        {
            // Arrange
            var mockService = new Mock<IAdminService>();
            var mockSurveyService = new Mock<ISurveyService>();
            var mockLogger = new NullLogger<AdminController>();
            var mockILogService = new Mock<ILogService>();
            mockService.Setup(repo => repo.GetSurveys()).ReturnsAsync(GetSurveys());
            var controller = new AdminController(mockService.Object, mockSurveyService.Object, (ILogger<AdminController>)mockLogger, mockILogService.Object);

            // Act
            var result = await controller.GetSurveys();

            // Assert
            var okResult = result as OkObjectResult;
            var surveys = new List<SurveyDto>((IEnumerable<SurveyDto>)okResult.Value);
            var expectedName = "Survey 1";
            Assert.NotEmpty(surveys);
            Assert.Equal(expectedName, surveys.Find(s => s.Title == expectedName).Title);
        }

        private List<SurveyDto> GetSurveys()
        {
            return surveys;
        }

        [Theory(DisplayName = "GetSurveyTest")]
        [InlineData("A0000000-0000-0000-0000-000000000002")]
        public async Task GetSurveyTest(string surveyId)
        {
            // Arrange
            var mockService = new Mock<IAdminService>();
            var mockSurveyService = new Mock<ISurveyService>();
            var mockLogger = new NullLogger<AdminController>();
            var guiSurveyId = new Guid(surveyId);
            var mockILogService = new Mock<ILogService>();
            mockService.Setup(repo => repo.GetSurvey(guiSurveyId)).ReturnsAsync(GetSurveys().Find(s => s.Id == guiSurveyId));
            var controller = new AdminController(mockService.Object, mockSurveyService.Object, (ILogger<AdminController>)mockLogger, mockILogService.Object);

            // Act
            var result = await controller.GetSurvey(guiSurveyId);

            // Assert
            var okResult = result as OkObjectResult;
            var survey = (SurveyDto)okResult.Value;
            var expectedName = "Tester2";
            Assert.IsType<Guid>(survey.Id);
            Assert.Equal(expectedName, survey.CreatedBy);
        }

        [Fact]
        public async Task CreateSurveyTest()
        {
            // Arrange
            var mockService = new Mock<IAdminService>();
            var mockSurveyService = new Mock<ISurveyService>();
            var mockLogger = new NullLogger<AdminController>();
            SurveyDto newSurvey = new SurveyDto
            {
                Title = "Survey 3",
                Description = "Description",
                CreatedBy = "Tester"               
            };
            var mockILogService = new Mock<ILogService>();
            mockService.Setup(repo => repo.CreateSurvey(newSurvey)).Callback(() => { surveys.Add(newSurvey); });
            var controller = new AdminController(mockService.Object, mockSurveyService.Object, (ILogger<AdminController>)mockLogger, mockILogService.Object);

            // Act
            var result = await controller.CreateSurvey(newSurvey);

            // Assert
            var okResult = result as OkObjectResult;
            var survey = (SurveyDto)okResult.Value;
            var expectedName = "Survey 3";
            Assert.NotNull(surveys.Find(x => x.Id == survey.Id));
            Assert.Equal(expectedName, survey.Title);
        }

        [Fact]
        public async Task UpdateSurveyTest()
        {
            // Arrange
            var surveyId = new Guid("A0000000-0000-0000-0000-000000000002");
            var mockService = new Mock<IAdminService>();
            var mockSurveyService = new Mock<ISurveyService>();
            var mockLogger = new NullLogger<AdminController>();
            var updateSurvey = new SurveyDto
            {
                Id = surveyId,
                Title = "Survey 2 updated",                
                ModifiedBy = "Tester1"              
            };
            var mockILogService = new Mock<ILogService>();
            mockService.Setup(repo => repo.UpdateSurvey(updateSurvey)).Callback(() => { 
                var s = surveys.FirstOrDefault(x => x.Id == surveyId); ;
                if (s != null) s.Title = updateSurvey.Title;
            });
            var controller = new AdminController(mockService.Object, mockSurveyService.Object, (ILogger<AdminController>)mockLogger, mockILogService.Object);

            // Act
            var result = await controller.UpdateSurvey(updateSurvey);

            // Assert
            var okResult = result as OkObjectResult;
            var survey = (SurveyDto)okResult.Value;
            var expectedName = "Survey 2 updated";
            Assert.NotNull(surveys.Find(x => x.Id == survey.Id));
            Assert.Equal(expectedName, survey.Title);
        }
    }
}
