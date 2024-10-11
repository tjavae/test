using System;
using System.Threading.Tasks;

namespace CUDC.Web.Services
{
    public interface ILogService
    {
        Task NLogError(Exception exception, string message, string callSite);
        Task NLogInfo(Exception exception, string message, string callSite);
        Task NLogWarn(Exception exception, string message, string callSite);
        Task NLogFatal(Exception exception, string message, string callSite);

    }
}
