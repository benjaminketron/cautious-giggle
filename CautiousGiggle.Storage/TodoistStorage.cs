﻿using CautiousGiggle.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CautiousGiggle.Core.Data.Models;
using SQLite;
using CautiousGiggle.Core.Config;

namespace CautiousGiggle.Storage
{
    public class TodoistStorage : ITodoistStorage
    {
        // wonder if there are downsides to holding this connection
        private SQLiteConnection connection;
        private readonly ConfigurationSettings configurationSettings;

        /// <summary>
        /// Creates SQLite Database and Tables
        /// </summary>
        public TodoistStorage(ConfigurationSettings configurationSettings)
        {
            this.configurationSettings = configurationSettings;

            // TODO refactor storage layer to use same database, add User table, and refactor queries to indicate user id or token.
            connection = new SQLiteConnection($"{this.configurationSettings.DatabasePath}{this.configurationSettings.Token}");

            connection.CreateTable<Item>();
            connection.CreateTable<ItemFilter>();
            connection.CreateTable<SyncToken>();
        }

        /// <summary>
        /// Upsert opration for items
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public int AddUpdateItems(IEnumerable<Item> items)
        {
            var result = 0;
            if (items != null)
            {
                foreach (var item in items)
                {
                    var exists = ItemExists(item);
                    if (!exists)
                    {
                        connection.Insert(item);
                    }
                    else
                    {
                        connection.Update(item);
                    }
                }

                result = items.Count();
            }
            
            return result;
        }

        /// <summary>
        /// Attempts to delete each item if it exists
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public int DeleteItems(IEnumerable<Item> items)
        {
            var result = 0;
            if (items != null)
            {
                foreach (var item in items)
                {
                    var exists = ItemExists(item);
                    if (exists)
                    {
                        connection.Delete(item);
                    }
                }

                result = items.Count();
            }

            return result;
        }

        /// <summary>
        /// Returns a list of items that have been stored.
        /// TODO Implement paging.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Item> GetItems()
        {
            var items = connection.Query<Item>("select * from Item order by content asc", new object [] { });

            // TODO select filters for each item in one select and mapp back onto items collection

            return items;
        }

        /// <summary>
        /// Checks for item existence
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool ItemExists(Item item)
        {
            var exists = connection.ExecuteScalar<long>($"select count(*) from Item where Id = {item.id};") == 1;
            return exists;
        }

        /// <summary>
        /// Gets the latest sync token
        /// </summary>
        /// <returns></returns>
        public string GetSyncToken()
        {
            var result = connection.ExecuteScalar<string>("select sync_token from SyncToken order by id desc limit 1;");
            return result;
        }

        /// <summary>
        /// SEts a new sync token
        /// </summary>
        /// <param name="syncToken"></param>
        /// <returns></returns>
        public bool SetSyncToken(string syncToken)
        {
            var result = connection.Insert(new SyncToken
            {
                sync_token = syncToken
            }) > 0;
            return result;
        }
    }
}
