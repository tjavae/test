using CUDC.Common.Dtos;
using CUDC.Api.Services;
using System;
using Microsoft.AspNetCore.Mvc;


namespace CUSORegistry.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogController : ControllerBase
    {
        private readonly CUDC.Api.Services.ILogService _service;

        public LogController(ILogService service)
        {
            _service = service;
        }

        [HttpPost("[action]")]
        public void NLogExt([FromBody] NLogDto logDto)
        {
            _service.NLogExt(logDto);
        }
    }
}
