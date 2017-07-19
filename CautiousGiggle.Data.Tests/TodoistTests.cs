
using CautiousGiggle.Core.Config;
using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CautiousGiggle.Data.Tests
{
    /// <summary>
    /// TODO Turn these two facts into a Theory
    /// </summary>
    public class TodoistTests
    {
        [Fact]
        public void Sync_Full()
        {
            HttpClient httpClient = new HttpClient();
            ConfigurationSettings configurationSettings = new ConfigurationSettings()
            {
                DatabasePath = "Todoist",
                TodoistApiUrl = "https://todoist.com/api/v7/sync",
                Token = "4238b2aba013852a793f55e6bca4825332cda0dd"
            };

            Todoist todoist = new Todoist(httpClient, configurationSettings);
            string sync_token = "*";
            string[] resource_types = new string[] { "items" };
            var result = todoist.Sync(configurationSettings.Token, sync_token, resource_types);

            Assert.NotNull(result);
        }

        [Fact]
        public void Sync_Partial()
        {
            HttpClient httpClient = new HttpClient();
            ConfigurationSettings configurationSettings = new ConfigurationSettings()
            {
                DatabasePath = "Todoist",
                TodoistApiUrl = "https://todoist.com/api/v7/sync",
                Token = "4238b2aba013852a793f55e6bca4825332cda0dd"
            };

            Todoist todoist = new Todoist(httpClient, configurationSettings);
            string token = "4238b2aba013852a793f55e6bca4825332cda0dd";
            string sync_token = "Q5oFYwWnuURxDlwqHittB4zXnWuEmOmxEP1IZ88hqIC6Sqy8H3ULCWt3YAEF0rCnElYZdx7qnIgnu6n7VXVdTMnw7_9YvLw5NYdgci9yLahb";
            string[] resource_types = new string[] { "items" };
            var result = todoist.Sync(configurationSettings.Token, sync_token, resource_types);

            Assert.NotNull(result);
        }
    }
}
