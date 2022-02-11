using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Models
{
    public class TwitchInfoModel
    {
        public string accessToken { get; set; }

        public string refresh_token { get; set; }

        public int Expires_in { get; set; }

        public string scope { get; set; }

        public string token_type { get; set; }
    }
}
