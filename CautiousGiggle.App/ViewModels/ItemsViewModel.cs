using CautiousGiggle.Core.Data;
using CautiousGiggle.Core.Data.Model;
using CautiousGiggle.Core.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CautiousGiggle.App.ViewModels
{
    public class ItemsViewModel : NotificationBase, IItemsViewModel
    {
        private readonly ITodoist todoist;
        private readonly ITodoistStorage todoistStorage;

        private ObservableCollection<ItemViewModel> items;
        private int selectedIndex;
        private int syncProgress;

        public ItemsViewModel(
            ITodoist todoist,
            ITodoistStorage todoistStorage)
        {
            this.todoist = todoist;
            this.todoistStorage = todoistStorage;

            this.items = new ObservableCollection<ItemViewModel>();
            
            this.selectedIndex = -1;
            this.syncProgress = -1;
        }

        public ObservableCollection<ItemViewModel> Items
        {
            get
            {
                return this.items;
            }
            set
            {
                SetProperty(ref this.items, value);
            }
        }

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                if (SetProperty(ref selectedIndex, value))
                { RaisePropertyChanged(nameof(SelectedItem)); }
            }
        }

        public ItemViewModel SelectedItem
        {
            get
            {
                return this.selectedIndex >= 0 ? this.items[this.selectedIndex] : null;
            }
        }

        public int SyncProgress
        {
            get
            {
                return syncProgress;
            }
            set
            {
                if (SetProperty(ref syncProgress, value))
                { RaisePropertyChanged(nameof(SyncProgress)); }
            }
        }

        public async void SyncAsync()
        {
            await Task.Run(() =>  Sync());
        }

        public virtual void Sync()
        {
            // sync updated items list
            //string syncToken = GetSyncToken();
            //var syncResponse = SyncItems(syncToken);
            //string newSnycToken = syncResponse.sync_token;

            //// At the moment we are not concerned about archived items. This is just an update to its status
            //var addedOrUpdatedItems = syncResponse.items.Where(i => i.is_deleted != 1).ToList();
            //var deletedItems = syncResponse.items.Where(i => i.is_deleted == 1).ToList();

            //// update ui
            //AddUpdateItemsUI(addedOrUpdatedItems);
            //DeleteItemsUI(deletedItems);

            //// update storage
            //AddUpdatedItemsStorage(addedOrUpdatedItems);
            //DeleteItemsUI(deletedItems);
        }

        /// <summary>
        /// Returns "*" if a token cannot be found.
        /// </summary>
        /// <returns></returns>
        public virtual string GetSyncToken()
        {
            var syncToken = this.todoistStorage.GetSyncToken();
            if (syncToken == null)
            {
                syncToken = "*";
            }

            return syncToken;
        }

        public virtual bool SetSyncToken(string syncToken)
        {
            return this.todoistStorage.SetSyncToken(syncToken);
        }

        /// <summary>
        /// Syncs new, updated, and deleted items from the Todoist rest API.
        /// </summary>
        /// <param name="syncToken"></param>
        /// <returns></returns>
        public virtual SyncResponse SyncItems(string syncToken)
        {
            string token = "4238b2aba013852a793f55e6bca4825332cda0dd"; // TODO OAuth
            string [] resourceTypes = new string [] { "items" }; // TODO not sure if I like how this is coded. Perhaps a container for resource types?

            return this.todoist.Sync(token, syncToken, resourceTypes);
        }

        /// <summary>
        /// Adds or updates UI from the items in the collection.
        /// TODO Shifts the scrollbar as necessary to compensate.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public virtual int AddUpdateItemsUI(IEnumerable<Item> items)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the items in the collection from the UI. 
        /// TODO Shifts the scrollbar as necessary to compensate.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public virtual int DeleteItemsUI(IEnumerable<Item> items)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds or updates the items in the collection in storage.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public virtual int AddUpdatedItemsStorage(IEnumerable<Item> items)
        {
            return this.todoistStorage.AddUpdateItems(items);
        }

        /// <summary>
        /// Removes the items in the collection from storage and returns the number of items that were removed.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public virtual int DeleteItemStoreStorage(IEnumerable<Item> items)
        {
            throw new NotImplementedException();
        }
    }
}
