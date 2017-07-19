using CautiousGiggle.App.ViewModels;
using CautiousGiggle.Core.Config;
using CautiousGiggle.Core.Data;
using CautiousGiggle.Core.Storage;
using CautiousGiggle.Data;
using CautiousGiggle.Storage;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CautiousGiggle.App.Container
{
    public class UnityContainerBootstraper
    {
        public static void Configure(IUnityContainer container)
        {
            var configurationSettings = new ConfigurationSettings("config.json");

            container.RegisterType<IItemsViewModel, ItemsViewModel>()
                .RegisterType<IItemViewModel, ItemViewModel>()
                .RegisterType<ITodoist, Todoist>()
                .RegisterType<ITodoistStorage, TodoistStorage>()
                .RegisterInstance<HttpClient>(new HttpClient())
                .RegisterInstance<ConfigurationSettings>(configurationSettings);
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
