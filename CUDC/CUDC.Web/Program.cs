using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Extensibility;
using NLog;
using PostSharp.Patterns.Diagnostics.Backends.NLog;

[assembly: Log(AttributePriority = 1, AttributeTargetMemberAttributes = MulticastAttributes.Protected | MulticastAttributes.Internal | MulticastAttributes.Public)]
[assembly: Log(AttributePriority = 2, AttributeExclude = true, AttributeTargetMembers = "get_*")]
[assembly: Log(AttributePriority = 3, AttributeExclude = true, AttributeTargetMembers = "set_*")]
[assembly: Log(AttributePriority = 4, AttributeExclude = true, AttributeTargetMembers = "*.ctor*")]
[assembly: Log(AttributePriority = 5, AttributeExclude = true, AttributeTargetMembers = "*Views_*")]

namespace CUDC.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {           
            LogManager.ResumeLogging();            
            LoggingServices.DefaultBackend = new NLogLoggingBackend();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
