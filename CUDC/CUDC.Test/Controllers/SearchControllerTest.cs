using CUDC.Common.Dtos;
using CUDC.Common.Dtos.CuSearch;
using CUDC.Common.Enums;
using CUDC.Web.Controllers;
using CUDC.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class SearchControllerTest
    {
        [Fact]
        public async Task GetStatesTest()
        {
            // Arrange
            var mockMisService = new Mock<IMisService>();
            var mockLogger = new NullLogger<SearchController>();
            var mockILogService = new Mock<ILogService>();            
            mockMisService.Setup(repo => repo.GetStates()).ReturnsAsync(GetStates());
            var controller = new SearchController(mockMisService.Object, (ILogger<SearchController>)mockLogger, mockILogService.Object);

            // Act
            var result = await controller.GetStates();

            // Assert
            var okResult = result as OkObjectResult;
            var states = new List<SelectListItem>((IEnumerable<SelectListItem>)okResult.Value);
            var expectedName = "California";
            Assert.NotEmpty(states);
            Assert.Equal(expectedName, states.Find(s => s.Text == "CA").Value);
            Assert.Equal(3, states.Count);
        }

        [Theory(DisplayName = "GetSearchResultsTest")]
        [InlineData("Charter 1", "1", "A")]
        public async Task GetSearchResultsTest(string name, string region, string se)
        {
            // Arrange
            var mockMisService = new Mock<IMisService>();
            var mockLogger = new NullLogger<SearchController>();
            var sRequest = new SearchRequest
            {
                Name = name,
                Region = region,
                SE = se
            };
            var mockILogService = new Mock<ILogService>();
            mockMisService.Setup(repo => repo.GetCreditUnionSearchResults(sRequest)).ReturnsAsync(GetSearResults());
            var controller = new SearchController(mockMisService.Object, (ILogger<SearchController>)mockLogger, mockILogService.Object);
                      
            // Act
            var result = await controller.GetSearchResults(sRequest);

            // Assert
            var okResult = result as OkObjectResult;
            var sResults = new List<SearchResult>((IEnumerable<SearchResult>)okResult.Value);
            var expectedNumber = 10;
            Assert.NotEmpty(sResults);
            Assert.Equal(expectedNumber, sResults.Find(s => s.Name == name).CharterNumber);
            Assert.Equal(3, sResults.Count);
        }

        [Theory(DisplayName = "GetBasicInfoTest")]
        [InlineData(12)]
        public async Task GetBasicInfoTest(int charterNumber)
        {
            // Arrange
            var mockMisService = new Mock<IMisService>();
            var mockLogger = new NullLogger<SearchController>();            
            mockMisService.Setup(repo => repo.GetCreditUnionBasicInfo(charterNumber)).ReturnsAsync(GetBasicInfo());
            var mockILogService = new Mock<ILogService>();
            var controller = new SearchController(mockMisService.Object, (ILogger<SearchController>)mockLogger, mockILogService.Object);

            // Act
            var result = await controller.GetBasicInfo(charterNumber);

            // Assert
            var okResult = result as OkObjectResult;  
            
            BasicInfo basicInfo = (BasicInfo)okResult.Value;
            var expectedNumber = 11;            
            Assert.Equal(expectedNumber, basicInfo.JoinNumber);
        }

        [Theory(DisplayName = "GetCreditUnionInformationTest")]
        [InlineData(12)]
        public async Task GetCreditUnionInformationTest(int charterNumber)
        {
            // Arrange
            var mockMisService = new Mock<IMisService>();
            var mockLogger = new NullLogger<SearchController>();
            mockMisService.Setup(repo => repo.GetCreditUnionInformation(charterNumber)).ReturnsAsync(GetCuInfo());
            var mockILogService = new Mock<ILogService>();
            var controller = new SearchController(mockMisService.Object, (ILogger<SearchController>)mockLogger, mockILogService.Object);

            // Act
            var result = await controller.GetCreditUnionInformation(charterNumber);

            // Assert
            var okResult = result as OkObjectResult;

            CUInformationDto cuInfo = (CUInformationDto)okResult.Value;
            var expectedNumber = 11;
            Assert.Equal(expectedNumber, cuInfo.JoinNumber);
        }

        private List<SelectListItem> GetStates()
        {
            var states = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = "MA",
                    Value = "Massachusetts"

                },
                new SelectListItem()
                {
                    Text = "CA",
                    Value = "California"
                },
                new SelectListItem()
                {
                    Text = "FL",
                    Value = "Florida"
                }
            };
            return states;
        }

        private List<SearchResult> GetSearResults()
        {
            var sResults = new List<SearchResult>
            {
                new SearchResult()
                {
                    Name = "Charter 1",
                    CharterNumber = 10,
                    JoinNumber = 11
                },
                new SearchResult()
                {
                    Name = "Charter 2",
                    CharterNumber = 11,
                    JoinNumber = 12
                },
                new SearchResult()
                {
                    Name = "Charter 3",
                    CharterNumber = 12,
                    JoinNumber = 13
                }
            };
            return sResults;
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

        private CUInformationDto GetCuInfo()
        {
            return new CUInformationDto()
            {
                CuNumber = 12,                
                JoinNumber = 11,
                Region = "1",
                ActualState = "MA",
                SE = "A"
            };
        }
    }
}
