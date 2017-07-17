
using CautiousGiggle.Core.Data.Models;
using CautiousGiggle.Core.Storage;
using CautiousGiggle.Storage;
using System;
using System.Linq;
using Xunit;

namespace CautiousGiggle.Storage.Tests
{
    public class TodoistStorageTests
    {
        [Fact]
        public void AddUpdateItems()
        {
            // make sure we are starting with an empty database
            var todoistStorage = new TodoistStorage($"todoist{DateTime.UtcNow.Ticks}");

            var item = new Item()
            {
                id = 1,
                content = "Content 1"
            };

            var result = todoistStorage.AddUpdateItems(new Item[] { item });

            Assert.Equal(1, result);

            var items = todoistStorage.GetItems();

            Assert.Equal(1, items.Count());
            Assert.Equal("Content 1", items.First().content);

            item.content = "Modified Content 1";

            todoistStorage.AddUpdateItems(new Item[] { item });

            items = todoistStorage.GetItems();

            Assert.Equal(1, items.Count());
            Assert.Equal("Modified Content 1", items.First().content);
        }

        [Fact]
        public void AddUpdateItems_GetItems()
        {
            // make sure we are starting with an empty database
            var todoistStorage = new TodoistStorage($"todoist{DateTime.UtcNow.Ticks}");

            var items = todoistStorage.GetItems();
            Assert.Equal(0, items.Count());

            var item = new Item()
            {
                id = 1,
                content = "Content 1"
            };

            var result = todoistStorage.AddUpdateItems(new Item[] { item });

            Assert.Equal(1, result);

            items = todoistStorage.GetItems();

            Assert.Equal(1, items.Count());
            Assert.Equal("Content 1", items.First().content);

            item.content = "Modified Content 1";


            result = todoistStorage.AddUpdateItems(new Item[] { item });

            Assert.Equal(1, result);

            items = todoistStorage.GetItems();

            Assert.Equal(1, items.Count());
            Assert.Equal("Modified Content 1", items.First().content);

            var item2 = new Item()
            {
                id = 2,
                content = "Content 2"
            };

            result = todoistStorage.AddUpdateItems(new Item[] { item2 });

            Assert.Equal(1, result);

            items = todoistStorage.GetItems();

            Assert.Equal(2, items.Count());

            result = todoistStorage.DeleteItems(new Item[] { item });

            Assert.Equal(1, result);

            items = todoistStorage.GetItems();

            Assert.Equal(1, items.Count());
            Assert.Equal(2, items.FirstOrDefault().id);
            Assert.Equal("Content 2", items.FirstOrDefault().content);
        }

        [Fact]
        public void SyncTokens()
        {
            // make sure we are starting with an empty database
            var todoistStorage = new TodoistStorage($"todoist{DateTime.UtcNow.Ticks}");

            var syncToken = todoistStorage.GetSyncToken();

            Assert.Null(syncToken);

            syncToken = "syncToken1";

            var result = todoistStorage.SetSyncToken(syncToken);

            Assert.True(result);

            var resultSyncToken = todoistStorage.GetSyncToken();

            Assert.Equal(syncToken, resultSyncToken);

            syncToken = "syncToken2";

            result = todoistStorage.SetSyncToken(syncToken);

            Assert.True(result);

            resultSyncToken = todoistStorage.GetSyncToken();

            Assert.Equal(syncToken, resultSyncToken);
        }
    }
}