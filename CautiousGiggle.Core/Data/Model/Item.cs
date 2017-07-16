using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CautiousGiggle.Core.Data.Model
{
    public class Item
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int project_id { get; set; }
        public string content { get; set; }
        public string date_string { get; set; }
        public string date_lang { get; set; }
        public string due_date_utc { get; set; }
        public int priority { get; set; }
        public int indent { get; set; }
        public int item_order { get; set; }
        public int day_order { get; set; }
        public int collapsed { get; set; }
        public int[] labels { get; set; }
        public int assigned_by_uid { get; set; }
        public int responsible_uid { get; set; }
        public int @checked { get;set; }
        public int in_history { get; set; }
        public int is_deleted { get; set; }
        public int is_archived { get; set; }
        public int sync_id { get; set; }
        public string date_added { get; set; }
    }
}
