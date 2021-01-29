using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicon_Inventory.Model
{
    public class WorkOrder
    {
        public int workOrderID { get; set; }
        public string workOrderNo { get; set; }
        public string workOrderDate { get; set; }
        public int contructorID { get; set; }
        public string contructorName { get; set; }
        public string Response { get; set; }
    }
}
