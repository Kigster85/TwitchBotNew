using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace TwitchBot.ChatCommands
{
    public class CooldownManager
    {

        private Dictionary<string, Dictionary<string, DateTime>> lastUsed;
        private readonly Dictionary<string, int> cooldownTimes; // In seconds

        public CooldownManager()
        {
            lastUsed = new Dictionary<string, Dictionary<string, DateTime>>();
            cooldownTimes = new Dictionary<string, int>();
        }

        public bool OnCooldown(string username, string command)
        {
            string key = CleanKey(command);

            if (!lastUsed.ContainsKey(username)) lastUsed.Add(username, new Dictionary<string, DateTime>());

            if (lastUsed[username].ContainsKey(key))
            {
                TimeSpan timeSpan = DateTime.UtcNow - lastUsed[username][key];
                if (timeSpan.Seconds < cooldownTimes.GetValueOrDefault(key, 0)) return true;
            }

            if (lastUsed[username].ContainsKey(key)) lastUsed[username].Remove(key);
            lastUsed[username].Add(key, DateTime.UtcNow);
            return false;
        }

        public void SetCooldown(string command, float minutes)
        {
            string key = CleanKey(command);

            if (cooldownTimes.ContainsKey(key)) cooldownTimes.Remove(key);
            cooldownTimes.Add(key, (int)(minutes * 60f));
        }

        private string CleanKey(string key)
        {
            return key.Trim().ToLower().TrimStart('!');
        }

    }
}
