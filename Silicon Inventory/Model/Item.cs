using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicon_Inventory.Model
{
    public class Item
    {
        public int itemID { get; set; }
        public string itemNumber { get; set; }
        public string itemName { get; set; }
        public string unit { get; set; }
        public string Response { get; set; }
        public int SL { get; set; }
    }
}
