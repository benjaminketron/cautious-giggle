using CautiousGiggle.Core.Data;
using CautiousGiggle.Core.Data.Models;
using CautiousGiggle.Core.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace CautiousGiggle.App.ViewModels
{
    public class ItemsViewModel : NotificationBase, IItemsViewModel
    {
        private readonly ITodoist todoist;
        private readonly ITodoistStorage todoistStorage;
        
        private ObservableCollection<ItemViewModel> items;
        private int selectedIndex;
        private int syncProgressPercent;
        private bool syncing;        

        public ItemsViewModel(ITodoist todoist,
            ITodoistStorage todoistStorage)
        {
            this.todoist = todoist;
            this.todoistStorage = todoistStorage;

            // get saved items
            var items = this.todoistStorage.GetItems().Select(i => new ItemViewModel(i)).ToList();

            this.items = new ObservableCollection<ItemViewModel>(items);
            
            this.selectedIndex = -1;
            this.syncing = false;
            this.syncProgressPercent = -1;
            
        }

        public CoreDispatcher Dispatcher { get; set; }

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

        public bool Syncing
        {
            get
            {
                return syncing;
            }
            set
            {
                if (SetProperty(ref syncing, value))
                { 
                    RaisePropertyChanged(nameof(Syncing));
                    RaisePropertyChanged(nameof(SyncingVisibility));
                }
            }
        }

        public Visibility SyncingVisibility
        {
            get
            {
                return syncing ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public int SyncProgressPercent
        {
            get
            {
                return syncProgressPercent;
            }
            set
            {
                if (SetProperty(ref syncProgressPercent, value))
                { RaisePropertyChanged(nameof(SyncProgressPercent)); }
            }
        }

        public async void SyncAsync()
        {
            if (!this.Syncing)
            {
                this.Syncing = true;
                this.SyncProgressPercent = 0;

                var uiUpdates = await Task.Run<Dictionary<long, ItemViewModel>>(() => Sync());

                ItemsUpdatesUI(uiUpdates);

                this.Syncing = false;
                this.SyncProgressPercent = 100;
            }
        }

        public virtual Dictionary<long, ItemViewModel> Sync()
        {
            Dictionary<long, ItemViewModel> uiUpdates = new Dictionary<long, ItemViewModel>();

            // sync updated items list
            string syncToken = GetSyncToken();
            var syncResponse = SyncItems(syncToken);
            if (syncResponse != null)
            {
                string newSnycToken = syncResponse.sync_token;

                if (syncResponse.items != null &&
                    syncResponse.items.Count() > 0)
                {
                    // At the moment we are not concerned about archived items. This is just an update to its status
                    var addedOrUpdatedItems = syncResponse.items.Where(i => i.is_deleted != 1).ToList();
                    var deletedItems = syncResponse.items.Where(i => i.is_deleted == 1).ToList();

                    var count = 0;
                    int total = syncResponse.items.Length * 2;

                    // update ui
                    count += AddUpdateItems(addedOrUpdatedItems, uiUpdates);

                    DispatchSyncProgressPercentUpdate((int)(count / total * 100.0));

                    count += DeleteItems(deletedItems, uiUpdates);

                    DispatchSyncProgressPercentUpdate((int)(count / total * 100.0));

                    // update storage
                    count += AddUpdatedItemsStorage(addedOrUpdatedItems);

                    DispatchSyncProgressPercentUpdate((int)(count / total * 100.0));

                    count += DeleteItemStorage(deletedItems);

                    DispatchSyncProgressPercentUpdate((int)(count / total * 100.0));
                }

                this.SetSyncToken(newSnycToken);
            }

            return uiUpdates;
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
        public virtual int AddUpdateItems(IEnumerable<Item> items, Dictionary<long, ItemViewModel> uiUpdates)
        {
            int result = 0;
            
            if (items != null)
            {
                foreach (var item in items)
                {
                    // non-null indicates items with this id should be added or updated when we get back to the UI Thread
                    uiUpdates[item.id] = new ItemViewModel(item); 
                }
                result = items.Count();
            }
            return result;
        }

        /// <summary>
        /// Removes the items in the collection from the UI. 
        /// TODO Shifts the scrollbar as necessary to compensate.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public virtual int DeleteItems(IEnumerable<Item> items, Dictionary<long, ItemViewModel> uiUpdates)
        {
            var result = 0;

            if (items != null)
            {
                foreach (var item in items)
                {
                    var itemViewModel = this.items.FirstOrDefault(i => i.Id == item.id);
                    if (itemViewModel != null)
                    {
                        uiUpdates[item.id] = null; // null indicates items with this id should be removed when we get back to the UI thread
                    }
                }
                result = items.Count();
            }
            return result;
        }

        /// <summary>
        /// Updates the Items observable collection in the UI thread utilising the uiUpdates dictionary. The keys are item ids and the values are
        /// ItemViewModel objects. If a value is null this indicates a deletetion. If a value is not null then it is an addition or udpate which
        /// are currently the same operation as we do not currently need to transfer information from the old object to new object at udpate.
        /// </summary>
        /// <param name="uiUpdates"></param>
        public virtual void ItemsUpdatesUI(Dictionary<long, ItemViewModel> uiUpdates) 
        {
            if (uiUpdates != null)
            {
                foreach (var key in uiUpdates.Keys)
                {
                    // remove item if it exists in preparation for add / update / delete
                    var currentItemViewModel = this.items.FirstOrDefault(i => i.Id == key);
                    if (currentItemViewModel != null)
                    {
                        var index = this.items.IndexOf(currentItemViewModel);                            
                        if (index > 0)
                        {
                            this.items.RemoveAt(index);
                        }                            
                    }
                    
                    var itemViewModel = uiUpdates[key];
                    
                    // indicates deletion
                    if (itemViewModel == null)
                    {
                        // do nothing. we alreaddy removed this item
                    }
                    // indicated add / update
                    else
                    {
                        var index = 0;
                        // item just after the insertion point which is located at the index where we would like to insert
                        var itemViewModelAfter = this.items.FirstOrDefault(i => (i?.Content ?? "").CompareTo(itemViewModel.Content ?? "") > 0);
                        if (itemViewModelAfter != null)
                        {
                            index = this.items.IndexOf(itemViewModelAfter);
                        }
                        else if (this.items.Count() > 0)
                        {
                            index = this.items.Count();
                        }
                        this.items.Insert(index, itemViewModel);
                    }
                }
            }
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
        public virtual int DeleteItemStorage(IEnumerable<Item> items)
        {
            return this.todoistStorage.DeleteItems(items);
        }

        /// <summary>
        /// Uses dispatcher to update the ProgressPercentUpdate property
        /// </summary>
        /// <param name="percent"></param>
        public virtual void DispatchSyncProgressPercentUpdate(int percent)
        {
            if (Dispatcher != null)
            {
                var ignored = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    this.SyncProgressPercent = percent;

                });
            }
        }
    }
}
