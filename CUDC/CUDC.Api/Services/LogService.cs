using System;
using System.Collections.Generic;
using System.Linq;
using CUDC.Api.Data;
using CUDC.Common.Dtos;

namespace CUDC.Api.Services
{
    public class LogService : ILogService
    {
        private readonly SurveyContext _context;

        public LogService(SurveyContext context)
        {
            _context = context;
        }        

        //Use this to log from the internal API server
        public void NLog(string level, string message, Exception exception, string callSite)
        {
            var log = new Data.NLog
            {
                MachineName = Environment.MachineName,
                Logged = DateTime.Now,
                Level = level,
                Message = message,
                Logger = "DatabaseLogger",
                Callsite = callSite,
                Exception = exception.ToString()
            };

            _context.NLog.Add(log);
            _context.SaveChanges();
        }

        //Use this to log from the external API server, such as Web server 
        public void NLogExt(NLogDto logDto)
        {
            var log = new Data.NLog
            {
                MachineName = logDto.MachineName,
                Logged = DateTime.Now,
                Level = logDto.Level,
                Message = logDto.Message,
                Logger = "DatabaseLogger",
                Callsite = logDto.Callsite,
                Exception = logDto.Exception
            };

            _context.NLog.Add(log);
            _context.SaveChanges();
        }
    }
}
