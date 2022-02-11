using NLog;
using NLog.Targets;
using NLog.Config;
using System;
using NLog.Targets.Wrappers;


namespace TwitchBot.Logging
{
    class MyLogging
    {
        
        public static Logger GetLogger()
        {

            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            // Step 2. Create targets
            var consoleTarget = new ColoredConsoleTarget("target1")
            {
                Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}"
            };
            config.AddTarget(consoleTarget);
            var fileTarget = new FileTarget("target2")
            {
                FileName = "E:/Streaming Stuff/CodingDevelopment/My Projects/Planning/TwitchBot/Logs/AppLog.txt",
                Layout = "${longdate} ${level} ${message}  ${exception}"
            };
            config.AddTarget(fileTarget);


            // Step 3. Define rules
            config.AddRule(LogLevel.Info, LogLevel.Error, fileTarget);
            config.AddRuleForAllLevels(consoleTarget); // all to console

            // Step 4. Activate the configuration
            LogManager.Configuration = config;

            // Example usage
            Logger logger = LogManager.GetLogger("Kigbot");

            //logger.Trace("trace log message");
            //logger.Debug("debug log message");
            //logger.Info("info log message");
            //logger.Warn("warn log message");
            //logger.Error("error log message");
            //logger.Fatal("fatal log message");

            //////Example of logging exceptions

            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    logger.Error(ex, "ow noos!");
            //    throw;
            //}

            return logger;

        }

    }
}
