using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CautiousGiggle.App.ViewModels
{
    public interface IItemViewModel
    {
        // Properties
        string Content { get; set; }
        long Id { get; set; }
    }
}
