﻿
using CautiousGiggle.App.ViewModels;
using CautiousGiggle.Core.Config;
using CautiousGiggle.Core.Data;
using CautiousGiggle.Core.Data.Models;
using CautiousGiggle.Core.Storage;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CautiousGiggle.App.Tests.ViewModels
{    
    public class ItemsViewModelTests
    {
        [Fact]
        public void Sync()
        {
            Mock<ITodoist> todoist = new Mock<ITodoist>();
            Mock<ITodoistStorage> todoistStorage = new Mock<ITodoistStorage>();

            ConfigurationSettings configurationSettings = new ConfigurationSettings()
            {
                DatabasePath = "Todoist",
                TodoistApiUrl = "https://todoist.com/api/v7/sync",
                Token = "4238b2aba013852a793f55e6bca4825332cda0dd"
            };

            Mock<ItemsViewModel> itemsViewModel = new Mock<ItemsViewModel>(todoist.Object, todoistStorage.Object, configurationSettings) { CallBase = true };

            var syncToken = "syncToken";
            var newSyncToken = "newSyncToken";
            SyncResponse syncResponse = new SyncResponse()
            {
                full_sync = false,
                items = new Item[]
                {
                    new Item()
                    {
                        id = 1,
                        is_deleted = 0
                    },
                    new Item()
                    {
                        id = 2,
                        is_deleted = 1
                    },
                    new Item()
                    {
                        id = 3,
                        is_deleted = 3
                    }
                },
                sync_token = newSyncToken
            };

            itemsViewModel.Setup(m => m.GetSyncToken()).Returns(syncToken).Verifiable();

            itemsViewModel.Setup(m => m.SyncItems(syncToken)).Returns(syncResponse).Verifiable();
            itemsViewModel.Setup(m => m.SetSyncToken(newSyncToken)).Returns(true).Verifiable();


            itemsViewModel.Setup(m => m.AddUpdateItems(It.IsAny<IEnumerable<Item>>(), It.IsAny<Dictionary<long, ItemViewModel>>())).Returns(2).Callback((IEnumerable<Item> items, Dictionary<long, ItemViewModel> uiUpdates) =>
            {
                Assert.Equal(2, items.Count());
            }).Verifiable();
            itemsViewModel.Setup(m => m.DeleteItems(It.IsAny<IEnumerable<Item>>(), It.IsAny< Dictionary<long, ItemViewModel>>())).Returns(1).Callback((IEnumerable<Item> items, Dictionary<long, ItemViewModel> uiUpdates) =>
            {
                Assert.Equal(1, items.Count());
                
            }).Verifiable();
            itemsViewModel.Setup(m => m.AddUpdatedItemsStorage(It.IsAny<IEnumerable<Item>>())).Returns(2).Callback((IEnumerable<Item> items) =>
            {
                Assert.Equal(2, items.Count());
            }).Verifiable();
            itemsViewModel.Setup(m => m.DeleteItemStorage(It.IsAny<IEnumerable<Item>>())).Returns(1).Callback((IEnumerable<Item> items) =>
            {
                Assert.Equal(1, items.Count());
            }).Verifiable();

            var result = itemsViewModel.Object.Sync();

            Assert.NotNull(result);
        }

        [Fact]
        public void Sync_Null_Items()
        {
            Mock<ITodoist> todoist = new Mock<ITodoist>();
            Mock<ITodoistStorage> todoistStorage = new Mock<ITodoistStorage>();

            ConfigurationSettings configurationSettings = new ConfigurationSettings()
            {
                DatabasePath = "Todoist",
                TodoistApiUrl = "https://todoist.com/api/v7/sync",
                Token = "4238b2aba013852a793f55e6bca4825332cda0dd"
            };

            Mock<ItemsViewModel> itemsViewModel = new Mock<ItemsViewModel>(todoist.Object, todoistStorage.Object, configurationSettings) { CallBase = true };

            var syncToken = "syncToken";
            var newSyncToken = "newSyncToken";
            SyncResponse syncResponse = new SyncResponse()
            {
                full_sync = false,
                sync_token = newSyncToken
            };

            itemsViewModel.Setup(m => m.GetSyncToken()).Returns(syncToken).Verifiable();

            itemsViewModel.Setup(m => m.SyncItems(syncToken)).Returns(syncResponse).Verifiable();
            
            itemsViewModel.Object.Sync();
        }

        [Fact]
        public void Sync_Null_Response()
        {
            Mock<ITodoist> todoist = new Mock<ITodoist>();
            Mock<ITodoistStorage> todoistStorage = new Mock<ITodoistStorage>();

            ConfigurationSettings configurationSettings = new ConfigurationSettings()
            {
                DatabasePath = "Todoist",
                TodoistApiUrl = "https://todoist.com/api/v7/sync",
                Token = "4238b2aba013852a793f55e6bca4825332cda0dd"
            };

            Mock<ItemsViewModel> itemsViewModel = new Mock<ItemsViewModel>(todoist.Object, todoistStorage.Object, configurationSettings) { CallBase = true };

            var syncToken = "syncToken";
            SyncResponse syncResponse = null;

            itemsViewModel.Setup(m => m.GetSyncToken()).Returns(syncToken).Verifiable();

            itemsViewModel.Setup(m => m.SyncItems(syncToken)).Returns(syncResponse).Verifiable();

            itemsViewModel.Object.Sync();
        }
    }
}
