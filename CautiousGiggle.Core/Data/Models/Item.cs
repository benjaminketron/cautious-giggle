using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CautiousGiggle.Core.Data.Models
{
    public class Item
    {
        [PrimaryKey]
        public long id { get; set; }
        public long user_id { get; set; }
        public long project_id { get; set; }
        public string content { get; set; }
        public string date_string { get; set; }
        public string date_lang { get; set; }
        public string due_date_utc { get; set; }
        public int priority { get; set; }
        public int indent { get; set; }
        public long item_order { get; set; }
        public long day_order { get; set; }
        public int collapsed { get; set; }
        [Ignore]
        public int[] labels { get; set; }
        public long? assigned_by_uid { get; set; }
        public long? responsible_uid { get; set; }
        public int @checked { get;set; }
        public int in_history { get; set; }
        public int is_deleted { get; set; }
        public int is_archived { get; set; }
        public long? sync_id { get; set; }
        public string date_added { get; set; }
    }
}
