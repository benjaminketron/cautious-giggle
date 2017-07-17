
using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CautiousGiggle.Data.Tests
{
    public class TodoistTests
    {
        [Fact]
        public void Sync_Full()
        {
            HttpClient httpClient = new HttpClient();
            Todoist todoist = new Todoist(httpClient);
            string token = "4238b2aba013852a793f55e6bca4825332cda0dd";
            string sync_token = "*";
            string[] resource_types = new string[] { "items" };
            var result = todoist.Sync(token, sync_token, resource_types);

            Assert.NotNull(result);
        }

        [Fact]
        public void Sync_Partial()
        {
            HttpClient httpClient = new HttpClient();
            Todoist todoist = new Todoist(httpClient);
            string token = "4238b2aba013852a793f55e6bca4825332cda0dd";
            string sync_token = "Q5oFYwWnuURxDlwqHittB4zXnWuEmOmxEP1IZ88hqIC6Sqy8H3ULCWt3YAEF0rCnElYZdx7qnIgnu6n7VXVdTMnw7_9YvLw5NYdgci9yLahb";
            string[] resource_types = new string[] { "items" };
            var result = todoist.Sync(token, sync_token, resource_types);

            Assert.NotNull(result);
        }
    }
}
