using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Domain
{
    public class MasterNavigationUserItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type Target { get; set; }
    }
}
