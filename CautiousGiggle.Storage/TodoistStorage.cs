using CautiousGiggle.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CautiousGiggle.Core.Data.Model;

namespace CautiousGiggle.Storage
{
    public class TodoistStorage : ITodoistStorage
    {
        public int AddUpdateItems(IEnumerable<Item> items)
        {
            throw new NotImplementedException();
        }

        public int DeleteItems(IEnumerable<Item> items)
        {
            throw new NotImplementedException();
        }       

        public string GetSyncToken()
        {
            throw new NotImplementedException();
        }

        public bool SetSyncToken(string syncToken)
        {
            throw new NotImplementedException();
        }
    }
}
