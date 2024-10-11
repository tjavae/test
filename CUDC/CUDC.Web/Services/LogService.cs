using CUDC.Common.Dtos;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Void = CUDC.Common.Util.Void;

namespace CUDC.Web.Services
{
    public class LogService : ServiceBase, ILogService
    {
        private readonly IConfiguration _config;

        public LogService(IConfiguration config) : base(config)
        {
            _config = config;
        }
        private string WebApiUrl(string path)
        {
            return $"{_config["WebApiBaseUrl"]}{path}";
        }

        public async Task NLogError(Exception exception, string message, string callSite)
        {
            var log = new NLogDto
            {
                MachineName = Environment.MachineName,
                Level = "Error",
                Message = message,
                Callsite = callSite,
                Exception = exception.ToString()
            };

            var requestUri = WebApiUrl("Log/NLogExt");
            await PostAsync<Void>(requestUri, log);
        }
        public async Task NLogInfo(Exception exception, string message, string callSite)
        {
            var log = new NLogDto
            {
                MachineName = Environment.MachineName,
                Level = "Info",
                Message = message,
                Callsite = callSite,
                Exception = exception.ToString()
            };

            var requestUri = WebApiUrl("Log/NLogExt");
            await PostAsync<Void>(requestUri, log);
        }
        public async Task NLogWarn(Exception exception, string message, string callSite)
        {
            var log = new NLogDto
            {
                MachineName = Environment.MachineName,
                Level = "Warn",
                Message = message,
                Callsite = callSite,
                Exception = exception.ToString()
            };

            var requestUri = WebApiUrl("Log/NLogExt");
            await PostAsync<Void>(requestUri, log);
        }

        public async Task NLogFatal(Exception exception, string message, string callSite)
        {
            var log = new NLogDto
            {
                MachineName = Environment.MachineName,
                Level = "Fatal",
                Message = message,
                Callsite = callSite
            };

            var requestUri = WebApiUrl("Log/NLogExt");
            await PostAsync<Void>(requestUri, log);
        }
    }
}
