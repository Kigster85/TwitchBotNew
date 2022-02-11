using System.Threading.Tasks;
using System;
using System.Globalization;
using System.Collections.Generic;
using NLog;
using TwitchBot.Logging;
using TwitchBot.StreamElements;
using TwitchLib.Client;
using TwitchBot.Connections;
using TwitchLib.Client.Events;

namespace TwitchBot.ChatCommands
{
    class CommandHandler
    {

        private static readonly Logger logger = MyLogging.GetLogger();
        private static readonly CooldownManager _cooldownManager = new CooldownManager();

        public static bool disastersEnabled = true;
        public static bool discountPoints = true;


        public CommandHandler()
        {
            // TODO: Fill the final times
            _cooldownManager.SetCooldown("Meteor", 10f);
            _cooldownManager.SetCooldown("Armageddon", 30f);
            _cooldownManager.SetCooldown("Tsunami", 30f);
            _cooldownManager.SetCooldown("Twister", 10f);
            _cooldownManager.SetCooldown("SuperCell", 30f);
            _cooldownManager.SetCooldown("Sinkhole", 5f);
            _cooldownManager.SetCooldown("Thunderstorm", 5f);
            _cooldownManager.SetCooldown("Earthquake", 5f);
            _cooldownManager.SetCooldown("Fire", 2f);
            _cooldownManager.SetCooldown("DestroyRoad", 2f);
        }

        public async Task HandleMessage(string username, string msg, TwitchClient client, string channel)
        {

            // TODO: Implement logic for handling messages.
            // This will take the message from the chat and 
            // check if it exists on the given command list,
            // If so, runs the custom command.

            // * Clean up message
            // * Evaluate if its a command (ie. if it starts with ! and some string)
            // * Check if its one of our commands
            // * Execute the proper function
            // logger.Debug("Handling message");

            string[] args = msg.Split(" ");
            string chatCommand = args.GetValue(0).ToString().Trim();
            //if (!chatCommand.StartsWith("!")) { return; }
            

            if (!IsAdmin(username) && _cooldownManager.OnCooldown(username, chatCommand))
            {
                logger.Debug($"Command {chatCommand} is on cooldown for {username}.");
                return;
            }

            string responseMessage = "";
            logger.Info(">>> " + chatCommand);
            switch (chatCommand)
            {
                case "fire":
                    responseMessage = await CommandStructureFire(username, msg);
                    break;
                case "destroyroad":
                    responseMessage = await CommandDestroyRandomRoad(username, msg);
                    break;
                case "help1":
                    responseMessage = await CommandHelp1(username, msg);
                    break;
                case "help2":
                    responseMessage = await CommandHelp2(username, msg);
                    break;
                case "help3":
                    responseMessage = await CommandHelp3(username, msg);
                    break;
                case "help4":
                    responseMessage = await CommandHelp4(username, msg);
                    break;
                case "help5":
                    responseMessage = await CommandHelp5(username, msg);
                    break;
                case "meteor":
                    responseMessage = await CommandMeteor(username, msg);
                    break;
                case "sinkhole":
                    responseMessage = await CommandSinkhole(username, msg);
                    break;
                case "earthquake":
                    responseMessage = await CommandEarthQuake(username, msg);
                    break;
                case "thunderstorm":
                    responseMessage = await CommandThunderStorm(username, msg);
                    break;
                case "tornado":
                    responseMessage = await CommandTwister(username, msg);
                    break;
                case "tsunami":
                    responseMessage = await CommandTsunami(username, msg);
                    break;
                case "supercell":
                    responseMessage = await CommandSuperCell(username, msg);
                    break;
                case "armageddon":
                    responseMessage = await CommandArma(username, msg);
                    break;
                case "steal1":
                    responseMessage = await CommandSteal1(username, msg);
                    break;
                case "steal2":
                    responseMessage = await CommandSteal2(username, msg);
                    break;
                case "steal3":
                    responseMessage = await CommandSteal3(username, msg);
                    break;
                case "steal4":
                    responseMessage = await CommandSteal4(username, msg);
                    break;
                case "steal5":
                    responseMessage = await CommandSteal5(username, msg);
                    break;
                default:
                    logger.Info("Unknown command {0}", chatCommand);
                    break;
            }

            if (!string.IsNullOrEmpty(responseMessage))
            {
                client.SendMessage(channel, responseMessage);
            }


        }

        private string FormatMessage(Dictionary<string, string> kwargs = null)
        {
            /*
            owner
            command used
            intensity (if applicable)
            location (if applicable)
            amount (if applicable)
            */

            // Just for now, as DEBUG. Sending money command
            string msg = "";
            foreach (var item in kwargs)
            {
                string key = item.Key;
                string value = item.Value;
                msg += String.Format("{0}={1};", item.Key, item.Value);
            }

            return msg;
        }

        private async Task<int> GenericCommandHelp(string username, string msg, int minvalue, int maxvalue)
        {
            Random random = new Random();
            int amount = random.Next(minvalue, maxvalue);
            bool successful = await SendMessageToGame("ADD_FUNDS", username, msg, amount);
            return (successful) ? amount : 0;

        }
        private async Task<int> GenericCommandSteal(string username, string msg, int minvalue, int maxvalue)
        {
            Random random = new Random();
            int amount = random.Next(minvalue, maxvalue);
            bool successful = await SendMessageToGame("REMOVE_FUNDS", username, msg, amount);
            return (successful) ? amount : 0;
        }

        private async Task<string> CommandHelp1(string username, string msg)
        {
            if (await LoyaltyPoints.ChargePoints(username, 1000))
            {
                int moneyAmount = await GenericCommandHelp(username, msg, 1000, 100000);
                return (moneyAmount > 0) ? $".me @{username} added {FormatMoney(moneyAmount)} to the game." : "";
            }
            return "";
        }

        private async Task<string> CommandHelp2(string username, string msg)
        {
            if (await LoyaltyPoints.ChargePoints(username, 3000))
            {
                int moneyAmount = await GenericCommandHelp(username, msg, 100000, 300000);
                return (moneyAmount > 0) ? $".me @{username} added {FormatMoney(moneyAmount)} to the game." : "";
            }
            return "";
        }

        private async Task<string> CommandHelp3(string username, string msg)
        {
            if (await LoyaltyPoints.ChargePoints(username, 5000))
            {
                int moneyAmount = await GenericCommandHelp(username, msg, 300000, 500000);
                return (moneyAmount > 0) ? $".me @{username} added {FormatMoney(moneyAmount)} to the game." : "";
            }
            return "";
        }

        private async Task<string> CommandHelp4(string username, string msg)
        {
            if (await LoyaltyPoints.ChargePoints(username, 7500))
            {
                int moneyAmount = await GenericCommandHelp(username, msg, 500000, 1000000);
                return (moneyAmount > 0) ? $".me @{username} added {FormatMoney(moneyAmount)} to the game." : "";
            }
            return "";
        }

        private async Task<string> CommandHelp5(string username, string msg)
        {
            if (await LoyaltyPoints.ChargePoints(username, 15000))
            {
                int moneyAmount = await GenericCommandHelp(username, msg, 1000000, 5000000);
                return (moneyAmount > 0) ? $".me @{username} added {FormatMoney(moneyAmount)} to the game." : "";
            }
            return "";
        }

        private async Task<string> CommandSteal1(string username, string msg)
        {
            if (await LoyaltyPoints.ChargePoints(username, 1000))
            {
                int moneyAmount = await GenericCommandSteal(username, msg, 1000, 100000);
                return (moneyAmount > 0) ? $".me @{username} just stole {FormatMoney(moneyAmount)} from the bank." : "";
            }
            return "";
        }

        private async Task<string> CommandSteal2(string username, string msg)
        {
            if (await LoyaltyPoints.ChargePoints(username, 3000))
            {
                int moneyAmount = await GenericCommandSteal(username, msg, 100000, 300000);
                return (moneyAmount > 0) ? $".me @{username} just stole {FormatMoney(moneyAmount)} from the bank." : "";
            }
            return "";
        }

        private async Task<string> CommandSteal3(string username, string msg)
        {
            if (await LoyaltyPoints.ChargePoints(username, 5000))
            {
                int moneyAmount = await GenericCommandSteal(username, msg, 300000, 500000);
                return (moneyAmount > 0) ? $".me @{username} just stole {FormatMoney(moneyAmount)} from the bank." : "";
            }
            return "";
        }

        private async Task<string> CommandSteal4(string username, string msg)
        {
            if (await LoyaltyPoints.ChargePoints(username, 7500))
            {
                int moneyAmount = await GenericCommandSteal(username, msg, 500000, 1000000);
                return (moneyAmount > 0) ? $".me @{username} just stole {FormatMoney(moneyAmount)} from the bank." : "";
            }
            return "";
        }

        private async Task<string> CommandSteal5(string username, string msg)
        {
            if (await LoyaltyPoints.ChargePoints(username, 15000))
            {
                int moneyAmount = await GenericCommandSteal(username, msg, 1000000, 5000000);
                return (moneyAmount > 0) ? $".me @{username} just stole {FormatMoney(moneyAmount)} from the bank." : "";
            }
            return "";
        }

        private async Task CommandProtect()
        {

        }

        private async Task CommandRoad()
        {

        }

        private async Task CommandBuilding()
        {

        }

        private async Task CommandDistrict()
        {

        }

        private async Task CommandTourist()
        {

        }

        private async Task CommandPark()
        {

        }

        private async Task CommandCustom()
        {

        }

        private async Task CommandMinor()
        {

        }

        private async Task<string> CommandStructureFire(string username, string msg)
        {
            if (!CanTriggerDisaster(username)) { return ""; }
            logger.Info("FIRE");
            if (await LoyaltyPoints.ChargePoints(username, 500))
            {
                bool successful = await SendMessageToGame("BUILDINGFIRE", username, msg);
                logger.Info("FIRE > " + successful);
                if (successful)
                {
                    return $".me @{username} triggered a Building Fire.";
                }
                Console.WriteLine("ERROR On SendMessageToGame");
                await LoyaltyPoints.AddPoints(username, 500);  // Refund
                return "";
            }
            else
            {
                return $".me @{username} triggered a Building Fire"; ;
            }
        }

        private async Task<string> CommandDestroyRandomRoad(string username, string msg)
        {
            if (!CanTriggerDisaster(username)) { return ""; }


            if (await LoyaltyPoints.ChargePoints(username, 2000))
            {
                bool successful = await SendMessageToGame("DESTROYSEGMENT", username, msg);
                if (successful)
                {
                    return $".me @{username} triggered destroy a random road";
                }
                else
                {
                    Console.WriteLine("ERROR On SendMessageToGame");
                    await LoyaltyPoints.AddPoints(username, 2000);  // Refund
                    return "";
                }
            }
            else
            {
                return $".me @{username} not enough points to destroy a road"; ;
            }
        }

        private async Task<string> CommandSinkhole(string username, string msg)
        {
            if (!CanTriggerDisaster(username)) { return ""; }

            if (await LoyaltyPoints.ChargePoints(username, 2500))
            {
                bool successful = await SendMessageToGame("Sinkhole", username, msg);
                if (successful)
                {
                    return $".me @{username} triggered a Sinkhole.";
                }
                Console.WriteLine("ERROR On SendMessageToGame");
                await LoyaltyPoints.AddPoints(username, 2500);  // Refund
                return "";

            }
            else
            {
                return $".me @{username} not enough points to trigger a Sinkhole"; ;
            }
        }

        private async Task<string> CommandMeteor(string username, string msg)
        {
            const int cost = 25000;

            if (!CanTriggerDisaster(username)) { return ""; }

            NewMethod(msg, out string command, out string[] args);

            var info = new TwitchDisasterInfo(username, msg, command);

            if (args.Length == 1)
            {
                bool isNumeric = byte.TryParse(args[0], out byte intensity);
                if (isNumeric)
                {
                    info.intensity = intensity;
                }
            }

            if (args.Length == 2)
            {
                bool isTarget = float.TryParse(args[0], out float targetX);
                isTarget &= float.TryParse(args[1], out float targetZ);
                if (isTarget)
                {
                    info.targetX = targetX;
                    info.targetZ = targetZ;
                }
            }

            if (args.Length == 3)
            {
                bool isNumeric = byte.TryParse(args[0], out byte intensity);
                if (isNumeric)
                {
                    info.intensity = intensity;

                    bool isTarget = float.TryParse(args[1], out float targetX);
                    isTarget &= float.TryParse(args[2], out float targetZ);
                    if (isTarget)
                    {
                        info.targetX = targetX;
                        info.targetZ = targetZ;
                    }
                }

            }

            logger.Info(info.ToJSON());

            if (await LoyaltyPoints.ChargePoints(username, cost))
            {
                if (await SendMessageToGame(info))
                {
                    return $".me @{username} triggered a Meteor.";
                }
            }
            // TODO: Refund points.
            return "";
        }

        private static void NewMethod(string msg, out string command, out string[] args)
        {
            string[] words = msg.Split(' ');
            command = words[0].Trim().TrimStart('!');
            ArraySegment<string> subArray = new ArraySegment<string>(words, 1, words.Length - 1);
            args = subArray.ToArray();
        }

        private async Task<string> CommandArma(string username, string msg)
        {
            if (!CanTriggerDisaster(username)) { return ""; }


            if (await LoyaltyPoints.ChargePoints(username, 80000))
            {
                await Task.Run(async () =>
                {
                    Random random = new Random();
                    for (int i = 0; i < 30; i++)
                    {
                        await SendMessageToGame("Meteor", username, msg);
                        await Task.Delay(random.Next(500, 1500));
                    }
                });
                logger.Info($"Armageddon released by {username}.");
            }
            else
            {
                Console.WriteLine("NOT Adding Meteor");
            }
            return $".me @{username} triggered an Armageddon";
        }

        private async Task<string> CommandSuperCell(string username, string msg)
        {
            if (!CanTriggerDisaster(username)) { return ""; }

            if (await LoyaltyPoints.ChargePoints(username, 80000))
            {
                await Task.Run(async () =>
                {
                    Random random = new Random();
                    for (int i = 0; i < 5; i++)
                    {
                        await SendMessageToGame("Tornado", username, msg);
                        await Task.Delay(random.Next(500, 5000));
                    }
                });
                logger.Info($"SuperCell released by {username}.");
                return $".me @{username} triggered a SuperCell";
            }
            return "";
        }

        private async Task<string> CommandTwister(string username, string msg)
        {
            if (!CanTriggerDisaster(username)) { return ""; }

            if (await LoyaltyPoints.ChargePoints(username, 20000))
            {
                await SendMessageToGame("Tornado", username, msg);
                return $".me @{username} triggered a Tornado";
            }
            return "";
        }

        private async Task<string> CommandThunderStorm(string username, string msg)
        {
            if (!CanTriggerDisaster(username)) { return ""; }

            if (await LoyaltyPoints.ChargePoints(username, 1000))
            {
                await SendMessageToGame("ThunderStorm", username, msg);
                return $".me @{username} triggered a Thunder Storm";
            }
            return "";
        }

        private async Task<string> CommandEarthQuake(string username, string msg)
        {
            if (!CanTriggerDisaster(username)) { return ""; }

            if (await LoyaltyPoints.ChargePoints(username, 1000))
            {
                await SendMessageToGame("EarthQuake", username, msg);
                return $".me @{username} triggered an Earth Quake";
            }
            return "";
        }

        private async Task<string> CommandTsunami(string username, string msg)
        {
            if (!CanTriggerDisaster(username)) { return ""; }

            if (await LoyaltyPoints.ChargePoints(username, 40000))
            {
                await SendMessageToGame("Tsunami", username, msg);
                return $".me @{username} triggered a Tsunami";
            }
            return "";
        }

        private async Task CommandMajor()
        {

        }

        private async Task CommandSue()
        {

        }


        private async Task<bool> SendMessageToGame(TwitchDisasterInfo info)
        {
            logger.Debug(info);
            int exitCode = SocketConnection.Send(info.ToJSON());
            logger.Debug($"Exit code: {exitCode}");
            return (exitCode == 0);
        }

        private async Task<bool> SendMessageToGame(string command, string username, string msg, int amount)
        {
            TwitchDisasterInfo info = new TwitchDisasterInfo(username, msg, command)
            {
                moneyAmount = amount,
            };
            logger.Debug(info);
            int exitCode = SocketConnection.Send(info.ToJSON());
            logger.Debug($"Exit code: {exitCode}");
            return (exitCode == 0);
        }

        private async Task<bool> SendMessageToGame(string command, string username, string msg)
        {

            TwitchDisasterInfo info = new TwitchDisasterInfo(username, msg, command);
            logger.Debug(info);
            logger.Debug(info.ToJSON());
            int exitCode = SocketConnection.Send(info.ToJSON());
            logger.Debug($"Exit code: {exitCode}");
            return (exitCode == 0);
        }

        private static string FormatMoney(int value)
        {
            return value.ToString("C0", CultureInfo.CurrentCulture);
        }

        private static bool IsAdmin(string username)
        {
            username = username.ToLower();
            return (username == "brainlesssociety" || username == "pumpkinhead_live" || username == "kigster85" || username == "laslo84");
        }

        private static bool CanTriggerDisaster(string username)
        {
            // if (IsAdmin(username)) { return true; }
            // if (!disastersEnabled) { return false; }
            return IsAdmin(username);

        }

        
    }

}