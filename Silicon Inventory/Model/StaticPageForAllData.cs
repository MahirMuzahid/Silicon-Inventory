using Newtonsoft.Json;
using Silicon_Inventory.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Silicon_Inventory.Model
{
    public class StaticPageForAllData
    {
        public static int printNumber;
        public static int pageNumber;
        public static int PrintedpageNumber;
        public static ObservableCollection<IssueVoucher> printIssueVoucher { get; set; }
        public static ObservableCollection<ReceiptVoucher> printRecieptVoucher { get; set; }
        public static ObservableCollection<ReturnVoucher> printReturnVoucher { get; set; }
        public static ObservableCollection<PeriodicRecieptReportModel> printPeriodicRecieptReport { get; set; }
        public static ObservableCollection<WOWisePeriodicIssueStatementModel> printWOWisePeriodicIssue { get; set; }
        public static ObservableCollection<ReturnVoucher> printPerioDicReturn { get; set; }
        public static ObservableCollection<IssueVoucher> printRequisitionReport { get; set; }
        public static ObservableCollection<StockData> printStockBalance { get; set; }
        public static ObservableCollection<StockLadgerModel> printStockLadger { get; set; }
        public static ObservableCollection<StockLadgerModel> printStockLadgerSummery { get; set; }

        public static PrintStockLadgerDetail printPage { get; set; }
        public static FixedPage page { get; set; }



        public static ObservableCollection<Contructor> Contructor { get; set; }
        public static ObservableCollection<WorkOrder> WorkOrder { get; set; }
        public static ObservableCollection<Supplier> Supplier { get; set; }
        public static ObservableCollection<Warehouse> WareHouse { get; set; }
        public  static ObservableCollection<ReturnVoucher> AllReturnVoucher { get; set; }
        public  static ObservableCollection<ReceiptVoucher> AllReceiptVoucher { get; set; }
        public static ObservableCollection<IssueVoucher> AllIssueVoucher { get; set; }
        public static ObservableCollection<StockData> StockData { get; set; }
        public static ObservableCollection<Item> Items { get; set; }
        public string error { get; set; }


        public async Task GetAllData ()
        {
            try
            {
                await GetContructor().ConfigureAwait(false);
                await GetWorkOrder().ConfigureAwait(false);
                await GetWareHouse().ConfigureAwait(false);
                await GetSupplier().ConfigureAwait(false);
                await GetAllItem().ConfigureAwait(false);
                await GetStockData().ConfigureAwait(false);
                await GetAllIssueVoucher().ConfigureAwait(false);
                await GetAllRecieptVOucher().ConfigureAwait(false);
                await GetAllReturnVoucher().ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                error = ex.Message;
            }
            
        }

        public async Task GetContructor()
        {
            string urlt = "https://api.shikkhanobish.com/api/Silicon/GetContructor";
            HttpClient clientt = new HttpClient();
            HttpResponseMessage responset = await clientt.GetAsync(urlt).ConfigureAwait(false);
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<List<Contructor>>(resultt);
            Contructor = new ObservableCollection<Contructor>();
            ObservableCollection<Contructor> con = new ObservableCollection<Contructor>();
            for (int i = 0; i < r.Count; i++)
            {
                Contructor.Add(r[i]);
            }
        }
        public async Task GetWorkOrder()
        {
            string urlt = "https://api.shikkhanobish.com/api/Silicon/GetWorkOrder";
            HttpClient clientt = new HttpClient();
            HttpResponseMessage responset = await clientt.GetAsync(urlt).ConfigureAwait(true);
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<List<WorkOrder>>(resultt);
            WorkOrder = new ObservableCollection<WorkOrder>();
            for (int i = 0; i < r.Count; i++)
            {
                WorkOrder.Add(r[i]);
            }
        }
        public async Task GetWareHouse()
        {
            string urlt = "https://api.shikkhanobish.com/api/Silicon/GetWarehouse";
            HttpClient clientt = new HttpClient();
            HttpResponseMessage responset = await clientt.GetAsync(urlt).ConfigureAwait(true);
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<List<Warehouse>>(resultt);
            WareHouse = new ObservableCollection<Warehouse>();
            for (int i = 0; i < r.Count; i++)
            {
                WareHouse.Add(r[i]);
            }
        }
        public async Task GetSupplier()
        {
            string urlt = "https://api.shikkhanobish.com/api/Silicon/GetSupplier";
            HttpClient clientt = new HttpClient();
            HttpResponseMessage responset = await clientt.GetAsync(urlt).ConfigureAwait(true);
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<List<Supplier>>(resultt);
            Supplier = new ObservableCollection<Supplier>();
            for (int i = 0; i < r.Count; i++)
            {
                Supplier.Add(r[i]);
            }
        }
        public async Task GetAllItem()
        {
            string urlt = "https://api.shikkhanobish.com/api/Silicon/GetItem";
            HttpClient clientt = new HttpClient();
            HttpResponseMessage responset = await clientt.GetAsync(urlt).ConfigureAwait(true);
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<List<Item>>(resultt);
            Items = new ObservableCollection<Item>();
            for (int i = 0; i < r.Count; i++)
            {
                Items.Add(r[i]);
            }

        }
        public async Task GetStockData()
        {
            string urlt = "https://api.shikkhanobish.com/api/Silicon/GetStockData";
            HttpClient clientt = new HttpClient();
            HttpResponseMessage responset = await clientt.GetAsync(urlt).ConfigureAwait(true);
            string resultt = await responset.Content.ReadAsStringAsync();
            StockData = new ObservableCollection<StockData>();
            var r = JsonConvert.DeserializeObject<List<StockData>>(resultt);
            for (int i = 0; i < r.Count; i++)
            {
                StockData.Add(r[i]);
            }
        }

        public async Task GetAllIssueVoucher()
        {
            string urlt = "https://api.shikkhanobish.com/api/Silicon/GetIssueVoucher";
            HttpClient clientt = new HttpClient();
            HttpResponseMessage responset = await clientt.GetAsync(urlt).ConfigureAwait(true);
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<List<IssueVoucher>>(resultt);
            AllIssueVoucher = new ObservableCollection<IssueVoucher>();
            for (int i = 0; i < r.Count; i++)
            {
                AllIssueVoucher.Add(r[i]);
            }
        }
        public async Task GetAllRecieptVOucher()
        {
            string urlt = "https://api.shikkhanobish.com/api/Silicon/GetRRVoucher";
            HttpClient clientt = new HttpClient();
            HttpResponseMessage responset = await clientt.GetAsync(urlt).ConfigureAwait(false);
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<List<ReceiptVoucher>>(resultt);
            AllReceiptVoucher = new ObservableCollection<ReceiptVoucher>();
            for (int i = 0; i < r.Count; i++)
            {
                AllReceiptVoucher.Add(r[i]);
            }
        }
        public async Task GetAllReturnVoucher()
        {
            string urlt = "https://api.shikkhanobish.com/api/Silicon/GetReturnVoucher";
            HttpClient clientt = new HttpClient();
            HttpResponseMessage responset = await clientt.GetAsync(urlt).ConfigureAwait(false);
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<List<ReturnVoucher>>(resultt);
            AllReturnVoucher = new ObservableCollection<ReturnVoucher>();
            for (int i = 0; i < r.Count; i++)
            {
                AllReturnVoucher.Add(r[i]);
            }
        }


    }
}
