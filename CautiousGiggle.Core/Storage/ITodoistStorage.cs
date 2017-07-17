using CautiousGiggle.Core.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CautiousGiggle.Core.Storage
{
    public interface ITodoistStorage
    {
        int AddUpdateItems(IEnumerable<Item> items);
        int DeleteItems(IEnumerable<Item> items);
        bool ItemExists(Item item);
        string GetSyncToken();
        bool SetSyncToken(string syncToken);
    }
}
