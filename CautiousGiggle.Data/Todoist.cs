using CautiousGiggle.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CautiousGiggle.Core.Data.Model;

namespace CautiousGiggle.Data
{
    public class Todoist : ITodoist
    {
        public SyncResponse Sync(string token, string sync_token, string[] resource_types)
        {
            throw new NotImplementedException();
        }
    }
}
