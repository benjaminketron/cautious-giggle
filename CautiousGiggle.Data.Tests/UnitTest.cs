
using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CautiousGiggle.Data.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void TestMethod1()
        {
            Assert.Equal(true, true);

            Mock<HttpClient> test = new Mock<HttpClient>() { CallBase = true };
            test.Setup(m => m.GetAsync("")).Returns(null as Task<HttpResponseMessage>);

            var result = test.Object.GetAsync("");
            Assert.Equal(null, result);
        }
    }
}
