using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicon_Inventory.Model
{
    public class ReturnVoucher
    {
        public int SL { get; set; }
        public string returnDate { get; set; }
        public int retID { get; set; }
        public int IssueQuantity { get; set; }
        public string ret_warehouse { get; set; }
        public string itemNumber { get; set; }
        public string description { get; set; }
        public int wareHouseID { get; set; }
        public string cause { get; set; }
        public string ret_type { get; set; }
        public string condition { get; set; }
        public string contructor { get; set; }
        public int isPrinted { get; set; }
        public string ItemNo { get; set; }
        public string Description { get; set; }
        public int quentity { get; set; }
        public string dltBtnVisibility { get; set; }
        public string workOrderNo { get; set; }
        public string unit { get; set; }
        public string Response { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
}
