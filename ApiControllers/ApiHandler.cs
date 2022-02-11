using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.ApiControllers
{
    public static class ApiHandler
    {
        public static HttpClient ApiServer { get; set; }

        public static void InitializeClient()
        {
            ApiServer = new HttpClient(); 
            ApiServer.DefaultRequestHeaders.Accept.Clear();
            ApiServer.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
        }
    }
}
