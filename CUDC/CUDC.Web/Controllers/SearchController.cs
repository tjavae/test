using CUDC.Common.Dtos.CuSearch;
using CUDC.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CUDC.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ICreditUnionService _creditUnionService;
        private readonly INCUAService _NCUARefservice;
        private readonly Logger fileLogger = LogManager.GetLogger("fileLogger");
        private readonly ILogService _databaseLogService;

        public SearchController(ICreditUnionService creditUnionService, INCUAService NCUARefservice, ILogService logService)
        {
            _creditUnionService = creditUnionService;
            _NCUARefservice = NCUARefservice;
            _databaseLogService = logService;
        }

        [HttpGet]
        [Route("GetStates")]
        public async Task<IActionResult> GetStates()
        {
            try
            {
                return Ok(await _NCUARefservice.GetStates());
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "GetStates error", "SearchController");
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("GetSearchResults")]
        public async Task<IActionResult> GetSearchResults(SearchRequest request)
        {
            try
            {   
                return Ok(await _creditUnionService.SearchCreditUnionsByRequest(request));
            }
            catch (Exception ex)
            {
                await _databaseLogService.NLogError(ex, "GetSearchResults error", "SearchController");
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("{charterNumber}/GetBasicInfo")]
        public async Task<IActionResult> GetBasicInfo(int charterNumber)
        {
            try
            {
                return Ok(await _creditUnionService.GetCreditUnionBasicInfo(charterNumber));
            }
            catch (Exception ex)
            {
                string message = "GetBasicInfo(" + charterNumber + ") error";
                await _databaseLogService.NLogError(ex, message, "SearchController");
                fileLogger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("{charterNumber}/GetCreditUnionInformation")]
        public async Task<IActionResult> GetCreditUnionInformation(int charterNumber)
        {
            try
            {
                return Ok(await _creditUnionService.GetCreditUnionByNumber(charterNumber));               
            }
            catch (Exception ex)
            {
                string message = "GetCreditUnionInformation(" + charterNumber + ") error";
                await _databaseLogService.NLogError(ex, message, "SearchController");
                fileLogger.Error(ex);
                throw;
            }
        }
    }
}
