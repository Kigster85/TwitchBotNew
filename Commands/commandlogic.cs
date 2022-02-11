using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.ChatCommands
{
    public enum TwitchActionStatus
    {
        PENDING = 0,
        IN_PROGRESS = 1,
        DONE = 2
    }


    public enum TargetType
    {
        NONE = 0,
        COORDINATES = 1,
        DISTRICT = 2
    }

    public class TwitchDisasterInfo
    {
        public string username;
        public string command;
        public string message;
        public byte intensity;
        public byte minIntensity = 1;
        public byte maxIntensity = 255;
        public float targetX = 100000f;
        public float targetY = 100000f;
        public float targetZ = 100000f;
        public string targetDistrictName;
        public int moneyAmount;
        //public float target.....;

        public byte Intensity
        {
            get => Math.Max(minIntensity, Math.Min(intensity, maxIntensity));
            set => intensity = value;
        }

        public TargetType targetType = TargetType.NONE;
        private DateTime createdAt;
        public TwitchActionStatus status;
        //public bool isSubscriber;
        //public bool isModerator;
        //public bool isAdmin;


        public TwitchDisasterInfo(string username, string message, string command)
        {
            this.username = username;
            this.message = message;
            this.command = command;
            createdAt = DateTime.UtcNow;
            status = TwitchActionStatus.PENDING;
        }

        public static TwitchDisasterInfo FromJSON(string json)
        {
            return JsonConvert.DeserializeObject<TwitchDisasterInfo>(json);
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }


    //class Program
    //{
    //static void Main(string[] args)
    //{
    //    Console.WriteLine("Hello World!");
    //    TwitchDisasterInfo info = new TwitchDisasterInfo("Brainless", "Meteor", "Im a message", 4);
    //    Console.WriteLine(info.ToJSON());
    //    //string x = UnityEngine.JsonUtility.ToJson((object)d);

    //    TwitchDisasterInfo.FromJSON(info.ToJSON());

    //    info.Intensity = 8;
    //    Console.WriteLine(info.ToJSON());

    //    info.Intensity = 82;
    //    Console.WriteLine(info.ToJSON());
    //    Console.WriteLine($">>> {info.Intensity}");

    //    info.Intensity = 0;
    //    Console.WriteLine(info.ToJSON());

    //    Console.WriteLine($">>> {info.Intensity}");



    //    TwitchDisasterInfo infoTest = new TwitchDisasterInfo("Brainless", "Meteor", "Im a message", 4);
    //    infoTest.minIntensity = 110;
    //    infoTest.Intensity = 10;
    //    Console.WriteLine(infoTest.ToJSON());



    //}
    //}

}
