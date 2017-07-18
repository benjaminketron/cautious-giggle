using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CautiousGiggle.Core.Data.Models
{
    /// <summary>
    /// TODO move the SQLite-net attributes to a class that extends this one and overrides the parameters they decorate.
    /// </summary>
    public class SyncToken
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string sync_token { get; set; }
    }
}
