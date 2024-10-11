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
    public class SurveyControllerTest
    {
        [Theory(DisplayName = "GetSurveyInfoTest")]
        [InlineData(12, SurveyType.CAT, "")]
        public async Task GetSurveyInfoTest(int cuNumber, SurveyType type, string userId)
        {
            // Arrange           
            var mockSurveyService = new Mock<ISurveyService>();
            var mockImisService = new Mock<IMisService>();
            var mockLogger = new NullLogger<SurveyController>();
            var mockILogService = new Mock<ILogService>();
            mockSurveyService.Setup(repo => repo.GetSurveys()).ReturnsAsync(GetSurveys());
            mockImisService.Setup(repo => repo.GetCreditUnionBasicInfo(cuNumber)).ReturnsAsync(GetBasicInfo());
            var controller = new SurveyController(mockSurveyService.Object, mockImisService.Object, (ILogger<SurveyController>)mockLogger, mockILogService.Object);

            // Act
            var result = await controller.GetSurveyInfo(cuNumber, type, userId);

            // Assert
            var okResult = result as OkObjectResult;
            SurveyInfo surveyInfo = (SurveyInfo)okResult.Value;

            //var surveys = new List<SurveyDto>((IEnumerable<SurveyDto>)okResult.Value);
            var expectedCuName = "CU_Test";
            var expectedSurveyTitle = "Survey_1";
            Assert.NotNull(surveyInfo);
            Assert.Equal(expectedCuName, surveyInfo.BasicInfo.Name);
            Assert.Equal(expectedSurveyTitle, surveyInfo.Survey.Title);
        }

        [Theory(DisplayName = "IsSurveyActiveTest")]
        [InlineData(SurveyType.CAT)]
        public async Task IsSurveyActiveTest(SurveyType type)
        {
            // Arrange           
            var mockSurveyService = new Mock<ISurveyService>();
            var mockImisService = new Mock<IMisService>();
            var mockLogger = new NullLogger<SurveyController>();
            var mockILogService = new Mock<ILogService>();
            mockSurveyService.Setup(repo => repo.GetSurveys()).ReturnsAsync(GetSurveys());
            var controller = new SurveyController(mockSurveyService.Object, mockImisService.Object, (ILogger<SurveyController>)mockLogger, mockILogService.Object);

            // Act
            var result = await controller.IsSurveyActive(type);

            // Assert
            var okResult = result as OkObjectResult;
            var isActive = okResult.Value;
            var expectedValue = true;
            Assert.Equal(expectedValue, isActive);
        }
       
        //SaveAnswers
        [Theory(DisplayName = "SaveAnswersTest")]
        [InlineData(12, SurveyType.CAT)]
        public async Task SaveAnswersTest(int cuNumber, SurveyType type)
        {
            // Arrange        
            var mockSurveyService = new Mock<ISurveyService>();
            var mockImisService = new Mock<IMisService>();
            var mockLogger = new NullLogger<SurveyController>();
            var expectedValue = false;
            ResponseDto responseDto = new ResponseDto()
            {
                SurveyId = new Guid("A0000000-0000-0000-0000-000000000001"),
                CuNumber = cuNumber,
                JoinNumber = 11
            };
            var mockILogService = new Mock<ILogService>();
            mockSurveyService.Setup(repo => repo.GetSurveys()).ReturnsAsync(GetSurveys());
            mockImisService.Setup(repo => repo.GetCreditUnionBasicInfo(cuNumber)).ReturnsAsync(GetBasicInfo());
            mockSurveyService.Setup(repo => repo.SetAnswers(responseDto)).Callback(()=> { expectedValue = true; });
            var controller = new SurveyController(mockSurveyService.Object, mockImisService.Object, (ILogger<SurveyController>)mockLogger, mockILogService.Object);
                              
            
            // Act
            var result = await controller.SaveAnswers(cuNumber, type, responseDto);
           
            // Assert
            var okResult = result as OkResult;
            Assert.Equal(200, okResult.StatusCode);
            Assert.True(expectedValue);
        }

        //SubmitAnswers
        [Theory(DisplayName = "SubmitAnswersTest")]
        [InlineData("A0000000-0000-0000-0000-000000000002")]
        public async Task SubmitAnswersTest(string responseId)
        {
            // Arrange        
            var mockSurveyService = new Mock<ISurveyService>();
            var mockImisService = new Mock<IMisService>();
            var mockLogger = new NullLogger<SurveyController>();
            var rspGuId = new Guid(responseId);            
            var expectedValue = false;
            var mockILogService = new Mock<ILogService>();
            mockSurveyService.Setup(repo => repo.GetSurveys()).ReturnsAsync(GetSurveys());
            mockSurveyService.Setup(repo => repo.SubmitAnswers(It.IsAny<SubmitRequest>())).Callback(() => { expectedValue = true; });
            var controller = new SurveyController(mockSurveyService.Object, mockImisService.Object, (ILogger<SurveyController>)mockLogger, mockILogService.Object);
          
            // Act
            var result = await controller.SubmitAnswers(rspGuId);
           
            // Assert
            var okResult = result as OkResult;
            Assert.Equal(200, okResult.StatusCode);
            Assert.True(expectedValue);
        }

        //UnlockSurvey
        [Theory(DisplayName = "UnlockSurveyTest")]
        [InlineData("A0000000-0000-0000-0000-000000000002")]
        public async Task UnlockSurveyTest(string responseId)
        {              
            // Arrange        
            var mockSurveyService = new Mock<ISurveyService>();
            var mockImisService = new Mock<IMisService>();
            var mockLogger = new NullLogger<SurveyController>();
            var rspGuId = new Guid(responseId);
            var expectedValue = false;
            var mockILogService = new Mock<ILogService>();
            mockSurveyService.Setup(repo => repo.GetSurveys()).ReturnsAsync(GetSurveys());
            mockSurveyService.Setup(repo => repo.UnlockSurvey(It.IsAny<SubmitRequest>())).Callback(() => { expectedValue = true; });
            var controller = new SurveyController(mockSurveyService.Object, mockImisService.Object, (ILogger<SurveyController>)mockLogger, mockILogService.Object);

            // Act
            var result = await controller.UnlockSurvey(new Guid(responseId));

            // Assert
            var okResult = result as OkResult;
            Assert.Equal(200, okResult.StatusCode);
            Assert.True(expectedValue);
        }

        //TransferOwnership
        [Theory(DisplayName = "TransferOwnershipTest")]
        [InlineData("A0000000-0000-0000-0000-000000000002", "hqnt\tester")]
        public async Task TransferOwnershipTest(string responseId, string userId)
        {
            // Arrange           
            var mockSurveyService = new Mock<ISurveyService>();
            var mockImisService = new Mock<IMisService>();
            var mockLogger = new NullLogger<SurveyController>();
            var expectedValue = false;
            var mockILogService = new Mock<ILogService>();
            mockSurveyService.Setup(repo => repo.GetSurveys()).ReturnsAsync(GetSurveys());
            //mockImisService.Setup(repo => repo.GetCreditUnionBasicInfo(12)).ReturnsAsync(GetBasicInfo());
            mockSurveyService.Setup(repo => repo.TransferOwnership(It.IsAny<SubmitRequest>())).Callback(()=> { expectedValue = true; });
            var controller = new SurveyController(mockSurveyService.Object, mockImisService.Object, (ILogger<SurveyController>)mockLogger, mockILogService.Object);

            // Act
            var result = await controller.TransferOwnership(new Guid(responseId), userId);

            // Assert
            var okResult = result as OkResult;            
            Assert.Equal(200, okResult.StatusCode);
            Assert.True(expectedValue);
        }
        private List<SurveyDto> GetSurveys()
        {
            var surveys = new List<SurveyDto>();
            surveys.Add(new SurveyDto()
            {
                Id = new Guid("A0000000-0000-0000-0000-000000000001"),
                Title = "Survey_1",
                CreatedBy = "Tester",
                Type = SurveyType.CAT,
                IsActive = true
            });
            surveys.Add(new SurveyDto()
            {
                Id = new Guid("A0000000-0000-0000-0000-000000000002"),
                Title = "Survey_2",
                CreatedBy = "Tester",
                Type = SurveyType.CAT,
                IsActive = false
            });
            surveys.Add(new SurveyDto()
            {
                Id = new Guid("A0000000-0000-0000-0000-000000000002"),
                Title = "Survey_2",
                CreatedBy = "Tester",
                Type = SurveyType.CAT,
                IsActive = false
            });
            surveys.Add(new SurveyDto()
            {
                Id = new Guid("A0000000-0000-0000-0000-000000000003"),
                Title = "Survey_3",
                CreatedBy = "Tester",
                Type = SurveyType.SE,
                IsActive = false
            });
            return surveys;
        }

        private BasicInfo GetBasicInfo()
        {
            return new BasicInfo()
            {
                Name = "CU_Test",
                CharterNumber = 12,
                JoinNumber = 11
            };
        }
    }
}
