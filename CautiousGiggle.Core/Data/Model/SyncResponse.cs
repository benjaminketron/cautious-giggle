using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CautiousGiggle.Core.Data.Model
{
    public class SyncResponse
    {
        public string sync_token { get; set; }
        public bool full_sync { get; set; }
        public Item[] items { get; set; }

        // TODO implement the rest of the collections that could appear in this response object depending on the specified resource_type
    }
}
