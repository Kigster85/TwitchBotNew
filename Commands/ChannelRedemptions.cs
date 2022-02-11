using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.PubSub;

namespace TwitchBot.Commands
{
    public class ChannelRedemptions
    {

    }
    public class RewardRedeemed : TwitchLib.PubSub.Models.Responses.Messages.ChannelPointsData
    {
        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}
