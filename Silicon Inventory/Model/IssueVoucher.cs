using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicon_Inventory.Model
{
    public class IssueVoucher
    {
        public int SL { get; set; }
        public int IssueVoucherID { get; set; }
        public string IssueVoucherDate { get; set; }
        public string itemNumber { get; set; }
        public string unit { get; set; }
        public string description { get; set; }
        public int IssueQuantity { get; set; }
        public int wareHouseID { get; set; }
        public string workOrderNo { get; set; }
        public string Condition { get; set; }
        public string contructorName { get; set; }
        public string requisition { get; set; }
        public string wareHouseName { get; set; }
        public int IsPrinted { get; set; }
        public string Response { get; set; }
        public string dltBtnVisibility { get; set; }
    }
}
