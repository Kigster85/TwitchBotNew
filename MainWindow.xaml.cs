using NLog;
using System;
using System.Windows;
using System.Windows.Controls;
using TwitchBot.Logging;
using TwitchBot.Server;

namespace TwitchBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Logger logger = MyLogging.GetLogger();

        public string code { get; private set; }


        //public sealed class RichTextBoxTarget : TargetWithLayout
        //{
        //    public string WindowName { get; internal set; }
        //    public string ControlName { get; internal set; }
        //    public bool UseDefaultRowColoringRules { get; internal set; }
        //}

        //private void MainWindow_Load(object sender, EventArgs e)
        //{

        //    RichTextBoxTarget target = new RichTextBoxTarget();
        //    target.Layout = "${date:format=HH\\:MM\\:ss} ${logger} ${message}";
        //    target.ControlName = "logger_Info";
        //    target.WindowName = "MainWindow";
        //    target.UseDefaultRowColoringRules = true;

        //    NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Trace);

        //    Logger logger = LogManager.GetLogger("Example");
        //    logger.Trace("trace log message");
        //    logger.Debug("debug log message");
        //    logger.Info("info log message");
        //    logger.Warn("warn log message");
        //    logger.Error("error log message");
        //    logger.Fatal("fatal log message");
        //}

        public static void Main(string[] args)
        {
            //if (args.Length == 0)
            //{
            //    WriteLine("Please enter the channel's name.");
            //    return;
            //}
            //Connections connections = new Connections();

            Logger logger = MyLogging.GetLogger();
            logger.Info("Starting");
            //ApiControllers.ApiHandler.InitializeClient();
            
        }


        private void userNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            //BotClient botclient = new BotClient();
            //System.Diagnostics.Process.Start("C:/Program Files/Mozilla Firefox/firefox.exe", (AuthUrl));

            AuthServer.InitializeWebServer();
            //Task.Run(AuthServer.RunServer);
            Connect.Content = "Connecting......";


        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e, Button button)
        {

        }
        private void Window_Closed(object sender, EventArgs e)
        {
            





        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


    }




}
