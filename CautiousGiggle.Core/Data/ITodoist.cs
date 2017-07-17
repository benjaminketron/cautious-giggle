using CautiousGiggle.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CautiousGiggle.Core.Data
{
  public interface ITodoist
  {
        SyncResponse Sync(string token, string sync_token, string[] resource_types);
  }
}
