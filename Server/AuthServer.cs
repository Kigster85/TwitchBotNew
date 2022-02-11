using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using NLog;
using TwitchLib.Client.Events;
using System.Diagnostics;
using TwitchBot.ChatCommands;

namespace TwitchBot.Server
{
    public static class AuthServer
    {
        public static string RedirectUrl = "https://localhost:13377";
        public static List<string> Scopes = new List<string>
        {   "user:edit","chat:read","chat:edit","channel:moderate","bits:read","channel:read:subscriptions","user:read:email",
            "user:read:subscriptions","channel:manage:polls","channel:manage:predictions","channel:manage:redemptions"};

        private static string ClientId = Properties.Settings.Default.ClientId;
        private static string ClientSecret = Properties.Settings.Default.ClientSecret;
        private static readonly Logger logger = Logging.MyLogging.GetLogger();
        private static CommandHandler _commandHandler;
        private static TwitchClient ChannelOwner;
        private static TwitchAPI UserTwitchAPI;
        private static string CachedClientToken;
        private static string TwitchChannelName;
        private static string TwitchChannelId;

        public static Task RunServer()
        {
            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.MapGet("/", Handler);

            return app.RunAsync("https://localhost:13377");
        }

        public static void InitializeWebServer()
        {
            Task.Run(RunServer);

            var authUrl = $"https://id.twitch.tv/oauth2/authorize?response_type=code&client_id={ClientId}&redirect_uri={RedirectUrl}&scope={string.Join("+", Scopes)}";
            Process.Start(new ProcessStartInfo(authUrl) { UseShellExecute = true });
        }

        private async static Task<string> Handler(CancellationToken cancellationToken, [FromQuery(Name = "code")] string? code)
        {
            if (code is not null)
            {
                var (accessToken, refreshToken) = await GetAccessAndRefreshTokens(code);
                CachedClientToken = accessToken;
                SetNameAndID(CachedClientToken).Wait();
                InitializeChannelOwner(TwitchChannelName, CachedClientToken);
                InitializeTwitchAPI(CachedClientToken);
            }
            return $"Hello your code was {code}! Please remember to blame Pumpkin... I blame Pumpkin";
        }

        private static void InitializeTwitchAPI(string accessToken)
        {
            UserTwitchAPI = new TwitchAPI();
            UserTwitchAPI.Settings.ClientId = ClientId;
            UserTwitchAPI.Settings.AccessToken = accessToken;
        }

        private static async Task SetNameAndID(string accessToken)
        {
            //logger.Info("TwitchAPI started");

            var api = new TwitchAPI();
            api.Settings.ClientId = ClientId;
            api.Settings.AccessToken = accessToken;

            var oauthedUser = await api.Helix.Users.GetUsersAsync();
            TwitchChannelId = oauthedUser.Users[0].Id;
            TwitchChannelName = oauthedUser.Users[0].Login;
        }

        private static void InitializeChannelOwner(string username, string accessToken)
        {

            _commandHandler = new CommandHandler();

            ChannelOwner = new TwitchClient();
            ChannelOwner.Initialize(new ConnectionCredentials(username, accessToken), TwitchChannelName);

            //////Events subscribed to through Twitch Client - (Connection => Event => Function to perform)
            ChannelOwner.OnConnected += Client_OnConnected;
            ChannelOwner.OnDisconnected += Client_OnDisconnected;
            ChannelOwner.OnLog += Client_OnLog;
            ChannelOwner.OnJoinedChannel += Client_OnJoinedChannel;
            ChannelOwner.OnMessageReceived += Client_OnMessageReceived;
            ChannelOwner.OnWhisperReceived += Client_OnWhisperReceived;
            ChannelOwner.OnNewSubscriber += Client_OnNewSubscriber;
            ChannelOwner.OnChatCommandReceived += Client_OnChatCommandReceived;
            
            //////The actual connection being made to twitch

            ChannelOwner.Connect();
        }

        private async static Task<Tuple<string, string>> GetAccessAndRefreshTokens(string code)
        {
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "code", code },
                { "grant_type", "authorization_code" },
                { "redirect_uri", RedirectUrl }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://id.twitch.tv/oauth2/token", content);

            var responseString = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseString);
            return new Tuple<string, string>(json["access_token"].ToString(), json["refresh_token"].ToString());
        }

        private static void Client_OnLog(object sender, OnLogArgs e)
        {
            logger.Info($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }

        private static void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            logger.Info("Connected to {channel}", e.AutoJoinChannel);
        }

        private static void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            ChannelOwner.SendMessage(TwitchChannelName, "I have arrived to take over your chat!");
            logger.Info($"User { e.BotUsername} Has connected (bot access)!");
            logger.Info("Joined channel {channel}", e.Channel);
        }

        private static void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            logger.Info(e.ChatMessage.Username, e.ChatMessage.Message,e.ChatMessage.Channel);
        }
        private static void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {

            _commandHandler.HandleMessage(e.Command.ChatMessage.Username, e.Command.CommandText, ChannelOwner, e.Command.ChatMessage.Channel);
            
            string commandText = e.Command.CommandText.ToLower();

            if (commandText.Equals("test", StringComparison.OrdinalIgnoreCase))
            {
                ChannelOwner.SendMessage(TwitchChannelName, "is what a test?");
            }
            if (CommandsStaticResponses.ContainsKey(commandText))
            {
                ChannelOwner.SendMessage(TwitchChannelName, CommandsStaticResponses[commandText]);
            }
        }

        private static void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            ChannelOwner.SendWhisper(e.WhisperMessage.Username, "I got your message.");
        }

        //public static void Client_OnChatCommandReceived(CommandHandler.CommandHandler.HandleMessage,e.ChatMessage.Username, e.ChatMessage.Message, client, e.ChatMessage.Channel)
        //{
        //    CommandHandler _commandhandler = new CommandHandler();

        //    if ()) ;

        //}
        //private static void ChatCommands.Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        //{
        //    _commandHandler.HandleMessage(e.ChatMessage.Username, e.ChatMessage.Message, client, e.ChatMessage.Channel);
        //}

        private static void Client_OnDisconnected(object sender, TwitchLib.Communication.Events.OnDisconnectedEventArgs e)
        {
            logger.Info($"OnDisconnected Event");

            // if (e.Subscriber.SubscriptionPlan == SubscriptionPlan.Prime)
            //     client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points! So kind of you to use your Twitch Prime on this channel!");
            // else
            //     client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points!");
        }

        public static Dictionary<string, string> CommandsStaticResponses = new Dictionary<string, string>
        {
            {"coffee", "Please sir, i could really use some more coffee." },
            {"whoopsie", "I don't beleive he just did that again!!" },
            {"test", "this is a test for the sake of doing a test!!" }


        };

        public static string username { get; private set; }

        private static void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
        }


    }
}
