using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace TwitchBot.StreamElements
{
    public static class LoyaltyPoints
    {

        private static HttpClient client = new HttpClient();

        private static readonly string Token = Environment.GetEnvironmentVariable("STREAMELEMENTS_TOKEN");

        private static readonly string BaseAPI = "https://api.streamelements.com/kappa/v2";

        private static string channelID;


        public static async Task<bool> AddPoints(string username, int amount)
        {
            return true;  // XXX: Hardcoded to avoid charging points for now.

            if (string.IsNullOrEmpty(channelID)) await GetChanelID();

            if (amount == 0) return true;

            if (amount < 0)
            {
                int currentPoints = await GetPoints(username);
                if (currentPoints + amount < 0)
                {
                    Console.WriteLine($"{username} doesnt have enough points (Has {currentPoints}).");
                    return false;
                }
            }

            HttpRequestMessage requestMessage = new HttpRequestMessage(
                HttpMethod.Put,
                $"{BaseAPI}/points/{channelID}/{username}/{amount}");
            requestMessage.Headers.Add("Authorization", $"Bearer {Token}");
            HttpResponseMessage response = await client.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                using (Stream stream = await response.Content.ReadAsStreamAsync())
                {
                    var serializer = new DataContractJsonSerializer(typeof(PointsModifyEndpoint));
                    PointsModifyEndpoint data = (PointsModifyEndpoint)serializer.ReadObject(stream);
                    Console.WriteLine($"{data.Username} used {data.Points}. Response: {data.Message}.");
                }
                return true;
            }
            return false;
        }


        public static async Task<bool> ChargePoints(string username, int amount)
        {
            return await AddPoints(username, (amount < 0) ? amount : -amount);
        }

        public static async Task<int> GetPoints(string username)
        {
            if (string.IsNullOrEmpty(channelID)) await GetChanelID();

            HttpRequestMessage requestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"{BaseAPI}/points/{channelID}/{username}");
            requestMessage.Headers.Add("Authorization", $"Bearer {Token}");
            HttpResponseMessage response = await client.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                using (Stream stream = await response.Content.ReadAsStreamAsync())
                {
                    var serializer = new DataContractJsonSerializer(typeof(PointsEndpoint));
                    PointsEndpoint data = (PointsEndpoint)serializer.ReadObject(stream);
                    return data.Points;
                }
            }

            return int.MinValue;
        }

        public static async Task<string> GetChanelID()
        {
            if (!string.IsNullOrEmpty(channelID)) return channelID;

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{BaseAPI}/channels/me");
            requestMessage.Headers.Add("Authorization", $"Bearer {Token}");
            HttpResponseMessage response = await client.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                using (Stream stream = await response.Content.ReadAsStreamAsync())
                {
                    var serializer = new DataContractJsonSerializer(typeof(ChannelEndpoint));
                    ChannelEndpoint data = (ChannelEndpoint)serializer.ReadObject(stream);
                    Console.WriteLine(data.ID);
                    channelID = data.ID;
                }
            }

            return channelID;
        }
    }

    [DataContract]
    class Profile
    {
        [DataMember(Name = "headerImage")] public string HeaderImage { get; set; }
        [DataMember(Name = "title")] public string Title { get; set; }
    }

    [DataContract]
    class ChannelEndpoint
    {
        [DataMember(Name = "profile")] public Profile Profile { get; set; }
        [DataMember(Name = "provider")] public string Provider { get; set; }
        [DataMember(Name = "broadcasterType")] public string BroadcasterType { get; set; }
        [DataMember(Name = "suspended")] public string Suspended { get; set; }
        [DataMember(Name = "_id")] public string ID { get; set; }
        [DataMember(Name = "providerId")] public string ProviderId { get; set; }
        [DataMember(Name = "email")] public string Email { get; set; }
        [DataMember(Name = "avatar")] public string Avatar { get; set; }
        [DataMember(Name = "apiToken")] public string ApiToken { get; set; }
        [DataMember(Name = "username")] public string Username { get; set; }
        [DataMember(Name = "alias")] public string Alias { get; set; }
        [DataMember(Name = "displayName")] public string DisplayName { get; set; }
        [DataMember(Name = "accessToken")] public string AccessToken { get; set; }
        [DataMember(Name = "lastLogin")] public string LastLogin { get; set; }
        [DataMember(Name = "inactive")] public bool Inactive { get; set; }
        [DataMember(Name = "isPartner")] public bool IsPartner { get; set; }
    }

    [DataContract]
    class PointsEndpoint
    {
        [DataMember(Name = "channel")] public string Channel { get; set; }
        [DataMember(Name = "username")] public string Username { get; set; }
        [DataMember(Name = "points")] public int Points { get; set; }
        [DataMember(Name = "pointsAlltime")] public int PointsAlltime { get; set; }
        [DataMember(Name = "rank")] public int Rank { get; set; }
    }

    [DataContract]
    class PointsModifyEndpoint
    {
        [DataMember(Name = "channel")] public string Channel { get; set; }
        [DataMember(Name = "username")] public string Username { get; set; }
        [DataMember(Name = "amount")] public int Points { get; set; }
        [DataMember(Name = "newAmount")] public int PointsAlltime { get; set; }
        [DataMember(Name = "message")] public string Message { get; set; }
    }
}
