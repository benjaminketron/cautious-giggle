using CautiousGiggle.Core.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CautiousGiggle.App.ViewModels
{
    public class ItemViewModel : NotificationBase<Item>, IItemViewModel
    {
        public ItemViewModel(Item item = null) : base(item)
        {

        }

        public string Content
        {
            get { return This.content; }
            set { SetProperty(This.content, value, () => This.content = value); }
        }

        public int Id
        {
            get { return This.id;  }
            set { SetProperty(This.id, value, () => This.id = value);  }
        }

    }
}
