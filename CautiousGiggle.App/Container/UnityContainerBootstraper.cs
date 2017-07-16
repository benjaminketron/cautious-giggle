using CautiousGiggle.App.ViewModels;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CautiousGiggle.App.Container
{
    public class UnityContainerBootstraper
    {
        public static void Configure(IUnityContainer container)
        {
            container.RegisterType<IItemsViewModel, ItemsViewModel>()
                .RegisterType<IItemViewModel, ItemViewModel>();
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
