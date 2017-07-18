using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace CautiousGiggle.App.ViewModels
{
    public interface IItemsViewModel
    {
        // Properties
        ObservableCollection<ItemViewModel> Items { get; set; }
        int SelectedIndex { get; set; }
        ItemViewModel SelectedItem { get; }
        int SyncProgressPercent { get; set; }
        bool Syncing { get; set; }
        Visibility SyncingVisibility { get; }

        // Methods
        void SyncAsync();
        Dictionary<long, ItemViewModel> Sync();
    }
}
