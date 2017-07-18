
using CautiousGiggle.App.Container;
using CautiousGiggle.App.ViewModels;
using CautiousGiggle.Core.Data;
using CautiousGiggle.Core.Data.Models;
using CautiousGiggle.Core.Storage;
using Microsoft.Practices.Unity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace CautiousGiggle.App.Tests.ViewModels
{    
    public class ContainerTests
    {
        [Fact]
        public void Container()
        {
            UnityContainer container = new UnityContainer();
            UnityContainerBootstraper.Configure(container);

            var result1 = container.Resolve<IItemsViewModel>();

            Assert.NotNull(result1);

            var result2 = container.Resolve<IItemViewModel>();

            Assert.NotNull(result2);

            var result3 = container.Resolve<ITodoistStorage>();

            Assert.NotNull(result3);

            var result4 = container.Resolve<HttpClient>();

            Assert.NotNull(result4);

        }
    }
}
