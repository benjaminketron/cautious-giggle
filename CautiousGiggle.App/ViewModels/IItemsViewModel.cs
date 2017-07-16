using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CautiousGiggle.App.ViewModels
{
    public interface IItemsViewModel
    {
        // Properties
        ObservableCollection<ItemViewModel> Items { get; set; }
        int SelectedIndex { get; set; }
        ItemViewModel SelectedItem { get; }

        // Methods
        void SyncAsync();
        void Sync();
    }
}
