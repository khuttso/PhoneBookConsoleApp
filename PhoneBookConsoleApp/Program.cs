// See https://aka.ms/new-console-template for more information


using System;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Config;
using NLog.Targets;
namespace PhoneBookConsoleApp;

public class Program
{
    public static void Main(string[] args)
    {
        // new ConsoleAppForJsonFile().Run();
        // contactDB
        
        
        
        var config = new LoggingConfiguration();

        var logfile = new FileTarget("logfile")
        {
            FileName = "logfile.txt",
            Layout = "${longdate} ${level:uppercase=true} ${message} ${exception:format=toString,StackTrace}"
        };
        var logconsole = new ConsoleTarget("logconsole");


        LogManager.Configuration = config;
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.AddNLog(config);
        });

        var logger = loggerFactory.CreateLogger<ContactRepositoryForSqLite>();
        new ConsoleAppForRelationalDb(logger).Run();
    }
}