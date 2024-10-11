using CUDC.Common.Dtos;
using System;


namespace CUDC.Api.Services
{
    public interface ILogService
    {
        void NLog(string level, string message, Exception exception, string callSite);
        void NLogExt(NLogDto logDto);
    }
}
