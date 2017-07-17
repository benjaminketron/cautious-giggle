using CautiousGiggle.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CautiousGiggle.Core.Data.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace CautiousGiggle.Data
{
    public class Todoist : ITodoist
    {
        /// <summary>
        /// TODO Create mockable wraper class inheriting from HttpClient.
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        public Todoist(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Gets sync data from Todoist rest API
        /// </summary>
        /// <param name="token"></param>
        /// <param name="sync_token"></param>
        /// <param name="resource_types"></param>
        /// <returns></returns>
        public SyncResponse Sync(string token, string sync_token, string[] resource_types)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("token", token),
                new KeyValuePair<string, string>("sync_token", sync_token),
                new KeyValuePair<string, string>("resource_types", JsonConvert.SerializeObject(resource_types))
            });
            
            var response = httpClient.PostAsync("https://todoist.com/api/v7/sync", content).Result;
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<SyncResponse>(response.Content.ReadAsStringAsync().Result);
        }
    }
}
