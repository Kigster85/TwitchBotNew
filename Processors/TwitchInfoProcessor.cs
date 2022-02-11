using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TwitchBot.ApiControllers;
using TwitchBot.Models;

namespace TwitchBot.Processors
{
    public class TwitchInfoProcessor
    {
        public async Task<TwitchInfoModel> TwitchOauthInfo(string TwitchAuthInfo)
        {
            using (HttpResponseMessage response = await ApiHandler.ApiServer.GetAsync(TwitchAuthInfo))

                if (response.IsSuccessStatusCode)
                {
                    TwitchInfoModel twitchInfoModel = await response.Content.ReadAsAsync<TwitchInfoModel>();

                    return twitchInfoModel;

                }
                else
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }
        }
    }
}
