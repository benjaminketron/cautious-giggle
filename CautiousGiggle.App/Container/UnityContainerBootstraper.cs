using CautiousGiggle.App.ViewModels;
using CautiousGiggle.Core.Data;
using CautiousGiggle.Core.Storage;
using CautiousGiggle.Data;
using CautiousGiggle.Storage;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CautiousGiggle.App.Container
{
    public class UnityContainerBootstraper
    {
        public static void Configure(IUnityContainer container)
        {
            container.RegisterType<IItemsViewModel, ItemsViewModel>()
                .RegisterType<IItemViewModel, ItemViewModel>()
                .RegisterType<ITodoist, Todoist>()
                .RegisterType<ITodoistStorage, TodoistStorage>()
                .RegisterType<HttpClient>();
        }

        public static UnityServiceLocator ConfigureServiceLocator()
        {
            var container = new UnityContainer();
            Configure(container);
            var serviceLocator = new UnityServiceLocator(container);
            return serviceLocator;
        } 
    }
}
