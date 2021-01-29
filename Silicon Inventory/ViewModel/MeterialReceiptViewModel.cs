using Newtonsoft.Json;
using Silicon_Inventory.Commands;
using Silicon_Inventory.Model;
using Silicon_Inventory.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

namespace Silicon_Inventory.ViewModel
{
    public class MeterialReceiptViewModel : BaseViewModel
    {
        #region MVVM Member Veriable
        UpdateViewCommand up;
        MainPageViewModel viewModel;
        ObservableCollection<Contructor> _Contructor { get; set; }
        ObservableCollection<WorkOrder> _WorkOrder { get; set; }
        ObservableCollection<Supplier> _Supplier { get; set; }
        ObservableCollection<Warehouse> _WareHouse { get; set; }
        ObservableCollection<ReceiptVoucher> _AllRRVoucher { get; set; }
        ObservableCollection<Item> _Items { get; set; }
        ObservableCollection<ReceiptVoucher> _ShowingVoucher { get; set; }
        ObservableCollection<ReceiptVoucher> _SearchVoucher { get; set; }       
        List<StockData> _stockData { get; set; }
        public string _newVoucherVisibility { get; set; }

        Contructor _contructorName { get; set; }
        Supplier _supplierName { get; set; }
        Warehouse _wareHouseName { get; set; }
        WorkOrder _workOrderNo { get; set; }
        Item _itemNumber { get; set; }
        ReceiptVoucher _SelectedSearchRRVocuher { get; set; }


        public string _rrDatelbl { get; set; }
        public string _nextrrNumberlbl { get; set; }
        public string _dlvrTotxbx { get; set; }
        public string _chkedBytxbx { get; set; }
        public string _allocreqNumbertxbx { get; set; }
        public string _condition { get; set; }
        public string _challantxbx { get; set; }
        public string _Error { get; set; }
        public string _ErrorColor { get; set; }
        public string _prgsVisibility { get; set; }
        public bool _makeVoucherEnabled { get; set; }
        public string _itemVisibility { get; set; }
        public int _prgsValue { get; set; }
        public string _isEditable { get; set; }
        public string _contructorPrv { get; set; }

        public string _rrDatePrv { get; set; }
        public string _rrNumberPrv { get; set; }
        public string _allocDatePrv { get; set; }
        public string _woacNoPrv { get; set; }
        public string _dlvrToPrv { get; set; }
        public string _chkedByPrv { get; set; }
        public string _conditionPrv { get; set; }
        public string _challanPrv { get; set; }
        public string _supplierPrv { get; set; }
        public string _allocRqPrv { get; set; }
        public string _warehousePrv { get; set; }
        public string _amountTxbx { get; set; }
        public string _blncTxbx { get; set; }
        public string _printSrc { get; set; }
        public string _printlbl { get; set; }
        public string _popUpVisibility { get; set; }
        public bool _nextBtnEnabled { get; set; }
        public bool _AddItemEnabled { get; set; }
        public bool _itemBxEnabled { get; set; }
        public bool _isAmountFocused { get; set; }
        public int _savingPrgs { get; set; }

        public ICommand Updater { get; set; }
        #endregion

        #region Business Logic Veriable

        int nextTickedNumber, maxVoucherNumber;
        public string red = "#E53E3E", green = "#18CB51 ", black = "#000000", blue = "#0C84F6";
        int firstStart = 0;

        #endregion

        public MeterialReceiptViewModel()
        {           
            firstStart = 1;
            Updater = new UpdaterForReceipt(this);
            Contructor = new ObservableCollection<Contructor>();
            WorkOrder = new ObservableCollection<WorkOrder>();
            WareHouse = new ObservableCollection<Warehouse>();
            Supplier = new ObservableCollection<Supplier>();
            GetAllInfo();
        }

        public async void GetAllInfo()
        {
            newVoucherVisibility = "Hidden";
            isEditable = "Hidden";
            AddItemEnabled = false;
            nextBtnEnabled = true;
            popUpVisibility = "Hidden";         
            ShowingVoucher = new ObservableCollection<ReceiptVoucher>();
            isEditable = "Visible";
            rrDatelbl = "Receipt Date: " + DateTime.Now.ToString("dd/M/yyyy");
            nextrrNumberlbl = "Ticket Number: 0";
            makeVoucherEnabled = false;
            sendMsg("Fetching Data From Database", 0);
            AllRRVoucher = StaticPageForAllData.AllReceiptVoucher;
            Contructor = StaticPageForAllData.Contructor;
            WorkOrder = StaticPageForAllData.WorkOrder;
            WareHouse = StaticPageForAllData.WareHouse;
            Supplier = StaticPageForAllData.Supplier;
            Items = StaticPageForAllData.Items;
            ObservableCollection<StockData> sd = new ObservableCollection<StockData>();
            stockData = new List<StockData>();
            sd = StaticPageForAllData.StockData;
            for(int i = 0; i < sd.Count; i++)
            {
                stockData.Add(sd[i]);
            }
            getMaxVoucher();
            FilterForSearch();
            sendMsg("Fetching Done", 0);
            if (AllRRVoucher.Count == 1 && AllRRVoucher[0].RRDate == null)
            {
                sendMsg("Can't Connect with server. Please Try Again", 1);
            }
            else
            {
                ShowThisVocuher(maxVoucherNumber);
                nextTickedNumber = maxVoucherNumber + 1;
                if (AllRRVoucher.Count == 0)
                {
                    nextrrNumberlbl = "Ticket Number: " + 1001;
                    nextTickedNumber = 1001;
                }
                else
                {
                    nextrrNumberlbl = "Ticket Number: " + nextTickedNumber;
                }                           
                firstStart = 2;
                nextBtnEnabled = true;
            }
        }

        #region Getting  info from database
        public async Task GetAllRRVOucher()
        {
            string urlt = "https://api.shikkhanobish.com/api/Silicon/GetRRVoucher";
            HttpClient clientt = new HttpClient();
            HttpResponseMessage responset = await clientt.GetAsync(urlt).ConfigureAwait(false);
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<List<ReceiptVoucher>>(resultt);
            AllRRVoucher = new ObservableCollection<ReceiptVoucher>();
            for (int i = 0; i < r.Count; i++)
            {
                AllRRVoucher.Add(r[i]);
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
            var r = JsonConvert.DeserializeObject<List<StockData>>(resultt);
            stockData = r;
        }
        public async Task SaveVoucher(ReceiptVoucher newRRVoucher)
        {
            newVoucherVisibility = "Hidden";
            popUpVisibility = "Visible";
            savingPrgs = 10;
            int thisVoucherID = newRRVoucher.RRVoucherID;
            for (int i = 0; i < WareHouse.Count; i++)
            {
                if (newRRVoucher.warehouse == WareHouse[i].wareHouseName)
                {
                    newRRVoucher.wareHouseID = WareHouse[i].wareHouseID;
                }
            }
            string urlt = "https://api.shikkhanobish.com/api/Silicon/SetRRVoucher";
            HttpClient clientt = new HttpClient();
            string jsonDataT = JsonConvert.SerializeObject(new
            {
                RRDate = newRRVoucher.RRDate,
                RRVoucherID = newRRVoucher.RRVoucherID,
                wordOrderNo = newRRVoucher.wordOrderNo,
                deliveredTo = newRRVoucher.deliveredTo,
                contructor = newRRVoucher.contructor,
                checkedby = newRRVoucher.checkedby,
                condition = newRRVoucher.condition,
                challan = newRRVoucher.challan,
                supplier = newRRVoucher.supplier,
                allocNumber = newRRVoucher.allocNumber,
                warehouse = newRRVoucher.warehouse,
                itemNumber = newRRVoucher.itemNumber,
                description = newRRVoucher.description,
                RCVQuantity = newRRVoucher.RCVQuantity,
                alolocDate = newRRVoucher.alolocDate,
                wareHouseID = newRRVoucher.wareHouseID
            });
            StringContent contentT = new StringContent(jsonDataT, Encoding.UTF8, "application/json");
            HttpResponseMessage responset = await clientt.PostAsync(urlt, contentT).ConfigureAwait(true);
            savingPrgs = 50;
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<Response>(resultt);
            await RestInfo().ConfigureAwait(false);

            getMaxVoucher();
            savingPrgs = 100;
            ShowThisVocuher(thisVoucherID);
            nextTickedNumber = maxVoucherNumber + 1;
            nextrrNumberlbl = "Ticket Number: " + nextTickedNumber;
            nextBtnEnabled = true;
            popUpVisibility = "Hidden";
            blncTxbx = "";
            amountTxbx = "";
            itemBxEnabled = false;
            FilterForSearch();
        }
        public async Task DeleteVoucher(string itemNumber)
        {
            savingPrgs = 20;
            int thisVoucher = ShowingVoucher[0].RRVoucherID;
            if (ShowingVoucher.Count > 1)
            {
                popUpVisibility = "Visible";
                for (int i = 0; i < WareHouse.Count; i++)
                {
                    if (ShowingVoucher[0].warehouse == WareHouse[i].wareHouseName)
                    {
                        ShowingVoucher[0].wareHouseID = WareHouse[i].wareHouseID;
                    }
                }
                ReceiptVoucher thisVchr = new ReceiptVoucher();
                for (int i = 0; i < ShowingVoucher.Count; i++)
                {
                    if (ShowingVoucher[i].itemNumber == itemNumber)
                    {
                        thisVchr = ShowingVoucher[i];
                    }
                }
                savingPrgs = 40;
                string urlt = "https://api.shikkhanobish.com/api/Silicon/DeleteRRVoucherItem";
                HttpClient clientt = new HttpClient();
                string jsonDataT = JsonConvert.SerializeObject(new
                {
                    RRVoucherID = ShowingVoucher[0].RRVoucherID,
                    itemNumber = itemNumber,
                    RCVQuantity = thisVchr.RCVQuantity,
                    wareHouseID = ShowingVoucher[0].wareHouseID,
                });
                StringContent contentT = new StringContent(jsonDataT, Encoding.UTF8, "application/json");
                HttpResponseMessage responset = await clientt.PostAsync(urlt, contentT).ConfigureAwait(true);
                string resultt = await responset.Content.ReadAsStringAsync();
                var r = JsonConvert.DeserializeObject<Response>(resultt);
                await RestInfo().ConfigureAwait(false);
                getMaxVoucher();
                ShowThisVocuher(thisVoucher);
                savingPrgs = 100;
                popUpVisibility = "Hidden";
                FilterForSearch();
                blncTxbx = "";
                amountTxbx = "";
            }
            else
            {
                sendMsg("Voucher must have atleast one item", 1);
            }

        }
        public async Task PrintVoucher()
        {
            try
            {
                printInPrinter();
                if (ShowingVoucher[0].isPrinted == 0)
                {
                    popUpVisibility = "Visible";
                    savingPrgs = 10;
                    int thisVoucherID = ShowingVoucher[0].RRVoucherID;
                    string urlt = "https://api.shikkhanobish.com/api/Silicon/setRRVooucherPrinted";
                    HttpClient clientt = new HttpClient();
                    string jsonDataT = JsonConvert.SerializeObject(new
                    {
                        RRVoucherID = ShowingVoucher[0].RRVoucherID
                    });
                    StringContent contentT = new StringContent(jsonDataT, Encoding.UTF8, "application/json");
                    savingPrgs = 30;
                    HttpResponseMessage responset = await clientt.PostAsync(urlt, contentT).ConfigureAwait(true);
                    string resultt = await responset.Content.ReadAsStringAsync();
                    var r = JsonConvert.DeserializeObject<Response>(resultt);
                    printSrc = "/Asset/printed_ico.png";
                    printlbl = "Printed";
                    isEditable = "Hidden";
                    savingPrgs = 50;
                    await GetAllRRVOucher().ConfigureAwait(false);
                    savingPrgs = 60;
                    await GetAllItem().ConfigureAwait(false);
                    savingPrgs = 70;
                    await GetStockData().ConfigureAwait(false);
                    savingPrgs = 80;

                    getMaxVoucher();

                    await RestInfo().ConfigureAwait(false);
                    ShowThisVocuher(thisVoucherID);
                    savingPrgs = 100;
                    popUpVisibility = "Hidden";

                    sendMsg("",2);
                }
            }           
            catch
            {
                sendMsg("Please make a new file or connect with internet", 1);
            }

        }
        public void printInPrinter()
        {
            StaticPageForAllData.printRecieptVoucher = ShowingVoucher;
            ObservableCollection<ReceiptVoucher> showThisPage = new ObservableCollection<ReceiptVoucher>();
            for (int i = 0; i < StaticPageForAllData.printRecieptVoucher.Count; i++)
            {
                StaticPageForAllData.printRecieptVoucher[i].SL = i + 1;

                for (int j = 0; j < StaticPageForAllData.StockData.Count; j++)
                {
                    if (StaticPageForAllData.printRecieptVoucher[i].itemNumber == StaticPageForAllData.StockData[j].itemNO)
                    {
                        StaticPageForAllData.printRecieptVoucher[i].unit = StaticPageForAllData.StockData[j].unit;
                    }
                }
                showThisPage.Add(StaticPageForAllData.printRecieptVoucher[i]);

            }
            StaticPageForAllData.printRecieptVoucher = showThisPage;
            StaticPageForAllData.printNumber = 1;
            if (ShowingVoucher.Count % 30 > 0)
            {
                StaticPageForAllData.pageNumber = (ShowingVoucher.Count / 30) + 1;
            }
            else
            {
                StaticPageForAllData.pageNumber = (ShowingVoucher.Count / 30);
            }
            StaticPageForAllData.PrintedpageNumber = 2;

            PrintDialog myPrintDialog = new PrintDialog();

            var pageSize = new Size(8.26 * 96, 11.69 * 96);
            var document = new FixedDocument();
            document.DocumentPaginator.PageSize = pageSize;
            for (int i = 0; i < StaticPageForAllData.pageNumber; i++)
            {
               
                // Create Fixed Page.
                var fixedPage = new FixedPage();
                fixedPage.Width = pageSize.Width;
                fixedPage.Height = pageSize.Height;

                // Add visual, measure/arrange page.
                PrintIssueVoucherView print = new PrintIssueVoucherView();
                fixedPage.Children.Add((UIElement)print);
                fixedPage.Measure(pageSize);
                fixedPage.Arrange(new Rect(new Point(), pageSize));
                fixedPage.UpdateLayout();

                // Add page to document
                var pageContent = new PageContent();
                ((IAddChild)pageContent).AddChild(fixedPage);
                document.Pages.Add(pageContent);
            }


            // Send to the printer.
            var pd = new PrintDialog();

            if (myPrintDialog.ShowDialog() == true)
            {
                pd.PrintDocument(document.DocumentPaginator, "My Document");
            }
            //up = new UpdateViewCommand(viewModel);
        }
        #endregion

        #region Business Logic
        public void sendMsg(string msg, int msgOrError)
        {
            if (msgOrError == 0)
            {
                Error = "Message: " + msg;
                ErrorColor = blue;
            }
            else if (msgOrError == 1)
            {
                Error = "Error: " + msg;
                ErrorColor = red;
            }
            else if (msgOrError == 2)
            {
                Error = "";
            }

        }
        public void getMaxVoucher()
        {
            maxVoucherNumber = 0;
            for (int j = 0; j < AllRRVoucher.Count; j++)
            {
                if (maxVoucherNumber < AllRRVoucher[j].RRVoucherID)
                {
                    maxVoucherNumber = AllRRVoucher[j].RRVoucherID;
                }
            }
        }
        public void MakeNewVoucher()
        {
            ReceiptVoucher newRRVoucher = new ReceiptVoucher();
            
            newRRVoucher.RRDate = DateTime.Now.ToString("dd/M/yyyy");
            newRRVoucher.RRVoucherID = int.Parse(rrNumberPrv);
            newRRVoucher.wordOrderNo = woacNoPrv;
            newRRVoucher.deliveredTo = dlvrToPrv;
            newRRVoucher.contructor = contructorPrv;
            newRRVoucher.checkedby = chkedByPrv;
            newRRVoucher.condition = conditionPrv;
            newRRVoucher.challan = challanPrv;
            newRRVoucher.supplier = supplierPrv;
            newRRVoucher.allocNumber = 0;
            newRRVoucher.warehouse = warehousePrv;
            newRRVoucher.itemNumber = itemNumber.itemNumber;
            newRRVoucher.description = itemNumber.itemName;
            newRRVoucher.RCVQuantity = int.Parse(amountTxbx);
            newRRVoucher.alolocDate = "//";
            SaveVoucher(newRRVoucher);
        }
        public void CheckIfEveryInputHasGiven()
        {
            int NullFlag = 0;
            try
            {
                if (String.IsNullOrEmpty(rrDatelbl))
                {
                    NullFlag++;
                }
                if (String.IsNullOrEmpty(nextrrNumberlbl))
                {
                    NullFlag++;
                }
                if (String.IsNullOrEmpty(_contructorName.ToString()))
                {
                    NullFlag++;
                }
                if (String.IsNullOrEmpty(dlvrTotxbx))
                {
                    NullFlag++;
                }
                if (String.IsNullOrEmpty(chkedBytxbx))
                {
                    NullFlag++;
                }
                if (String.IsNullOrEmpty(supplierName.supplierName))
                {
                    NullFlag++;
                }
                if (String.IsNullOrEmpty(wareHouseName.wareHouseName))
                {
                    NullFlag++;
                }
                if (String.IsNullOrEmpty(allocreqNumbertxbx))
                {
                    NullFlag++;
                }
                if (String.IsNullOrEmpty(condition))
                {
                    NullFlag++;
                }
                if (String.IsNullOrEmpty(workOrderNo.workOrderNo))
                {
                    NullFlag++;
                }
                if (String.IsNullOrEmpty(challantxbx))
                {
                    NullFlag++;
                }
                if (NullFlag > 0)
                {
                    makeVoucherEnabled = false;
                }
                else
                {
                    makeVoucherEnabled = true;
                }
            }
            catch (NullReferenceException ex)
            {
                makeVoucherEnabled = false;
            }           
        }
        StaticPageForAllData data = new StaticPageForAllData();
        public async Task RestInfo()
        {
            await data.GetAllRecieptVOucher().ConfigureAwait(false);
            await data.GetAllItem().ConfigureAwait(false);
            await data.GetStockData().ConfigureAwait(false);
            AllRRVoucher = StaticPageForAllData.AllReceiptVoucher;
            Items = StaticPageForAllData.Items;
            ObservableCollection<StockData> sd = new ObservableCollection<StockData>();
            sd = StaticPageForAllData.StockData;
            for (int i = 0; i < sd.Count; i++)
            {
                stockData.Add(sd[i]);
            }

        }


        #endregion

        #region MVVM Data Binding
        public void ShowNextVoucher()
        {
            if (ShowingVoucher[0].RRVoucherID == maxVoucherNumber)
            {
                sendMsg("This is last voucher.", 0);
            }
            else
            {
                ShowThisVocuher(ShowingVoucher[0].RRVoucherID + 1);
            }

        }
        public void ShowNPreviousVoucher()
        {
            if (ShowingVoucher[0].RRVoucherID == 1001)
            {
                sendMsg("This is first voucher.", 0);
            }
            else
            {
                ShowThisVocuher(ShowingVoucher[0].RRVoucherID - 1);
            }

        }
        public void ShowThisVocuher(int voucherID)
        {
            try
            {
                sendMsg("", 2);
                ObservableCollection<ReceiptVoucher> newVoucher = new ObservableCollection<ReceiptVoucher>();
                for (int i = 0; i < AllRRVoucher.Count; i++)
                {
                    if (voucherID == AllRRVoucher[i].RRVoucherID)
                    {
                        newVoucher.Add(AllRRVoucher[i]);

                    }
                }
                for (int i = 0; i < newVoucher.Count; i++)
                {
                    if (newVoucher[i].isPrinted == 0)
                    {
                        newVoucher[i].dltBtnVisibility = "Visible";
                    }
                    else
                    {
                        newVoucher[i].dltBtnVisibility = "Hidden";
                    }
                }
                if (newVoucher.Count > 0)
                {
                    ShowingVoucher = newVoucher;
                    rrDatePrv = "" + ShowingVoucher[0].RRDate;
                    rrNumberPrv = "" + ShowingVoucher[0].RRVoucherID;
                    allocDatePrv = "//";
                    woacNoPrv = "" + ShowingVoucher[0].wordOrderNo;
                    dlvrToPrv = "" + ShowingVoucher[0].deliveredTo;
                    chkedByPrv = "" + ShowingVoucher[0].checkedby;
                    conditionPrv = "" + ShowingVoucher[0].checkedby;
                    challanPrv = "" + ShowingVoucher[0].challan;
                    supplierPrv = "" + ShowingVoucher[0].supplier;
                    allocRqPrv = "" + ShowingVoucher[0].allocNumber;
                    warehousePrv = "" + ShowingVoucher[0].warehouse;
                    contructorPrv = "" + ShowingVoucher[0].contructor;

                    if (ShowingVoucher[0].isPrinted == 0)
                    {
                        printSrc = "/Asset/pending.png";
                        printlbl = "Pending";
                        isEditable = "Visible";
                    }
                    if (ShowingVoucher[0].isPrinted == 1)
                    {
                        printSrc = "/Asset/printed_ico.png";
                        printlbl = "Printed";
                        isEditable = "Hidden";
                    }
                }
                else
                {
                    sendMsg("No Voucher Found", 0);
                }

            }
            catch (Exception ex)
            {
                sendMsg(ex.Message, 1);
            }
            
        }
        public void ShowNewVoucher()
        {
            newVoucherVisibility = "Visible";
            List<char> con = new List<char>();
            string conditionNew = "";
            for (int i = condition.Length - 1; i > 0; i--)
            {
                if (condition[i] != ' ')
                {
                    con.Add(condition[i]);
                }
                else
                {
                    break;
                }
            }
            for (int j = con.Count - 1; j >= 0; j--)
            {
                conditionNew = conditionNew + con[j];
            }
            makeVoucherEnabled = false;
            rrDatePrv = DateTime.Now.ToString("dd/M/yyyy");
            rrNumberPrv = "" + nextTickedNumber;
            allocDatePrv = "//";
            woacNoPrv = workOrderNo.workOrderNo;
            dlvrToPrv = dlvrTotxbx;
            dlvrTotxbx = "";
            chkedByPrv = chkedBytxbx;
            chkedBytxbx = "";
            conditionPrv = conditionNew;
            challanPrv = challantxbx;
            challantxbx = "";
            supplierPrv = supplierName.supplierName;
            allocRqPrv = allocreqNumbertxbx;
            allocreqNumbertxbx = "";
            warehousePrv = wareHouseName.wareHouseName;
            contructorPrv = contructorName.contructorName;
            ShowingVoucher.Clear();
            printSrc = "/Asset/pending.png";
            printlbl = "Pending";
            isEditable = "Visible";
            makeVoucherEnabled = false;
            nextBtnEnabled = false;
            AddItemEnabled = false;


        }
        public void FilterForSearch()
        {
            int rrID = maxVoucherNumber;
            ObservableCollection<ReceiptVoucher> rrV = new ObservableCollection<ReceiptVoucher>();
            for (int i = 0; i < AllRRVoucher.Count; i++)
            {
                if (rrID == AllRRVoucher[i].RRVoucherID)
                {
                    rrV.Add(AllRRVoucher[i]);
                    rrID = rrID - 1;                  
                    i = -1;
                }
            }

            SearchVoucher = rrV;
        }

        #endregion

        #region MVVM Binder
        public ObservableCollection<Contructor> Contructor { get { return _Contructor; } set { _Contructor = value; OnPropertyChanged(nameof(Contructor)); } }
        public ObservableCollection<WorkOrder> WorkOrder { get { return _WorkOrder; } set { _WorkOrder = value; OnPropertyChanged(nameof(WorkOrder)); } }
        public ObservableCollection<Warehouse> WareHouse { get { return _WareHouse; } set { _WareHouse = value; OnPropertyChanged(nameof(WareHouse)); } }
        public ObservableCollection<Supplier> Supplier { get { return _Supplier; } set { _Supplier = value; OnPropertyChanged(nameof(Supplier)); } }
        public ObservableCollection<ReceiptVoucher> AllRRVoucher { get { return _AllRRVoucher; } set { _AllRRVoucher = value; OnPropertyChanged(nameof(AllRRVoucher)); } }
        public ObservableCollection<Item> Items { get { return _Items; } set { _Items = value; OnPropertyChanged(nameof(Items)); } }
        public ObservableCollection<ReceiptVoucher> ShowingVoucher { get { return _ShowingVoucher; } set { _ShowingVoucher = value; OnPropertyChanged(nameof(ShowingVoucher)); } }
        public ObservableCollection<ReceiptVoucher> SearchVoucher { get { return _SearchVoucher; } set { _SearchVoucher = value; OnPropertyChanged(nameof(SearchVoucher)); } }
        public  ReceiptVoucher SelectedSearchRRVocuher { get { return _SelectedSearchRRVocuher; } set 
            { 
                _SelectedSearchRRVocuher = value;
                if(SelectedSearchRRVocuher != null)
                {
                    ShowThisVocuher(SelectedSearchRRVocuher.RRVoucherID);
                }              
                OnPropertyChanged(nameof(SelectedSearchRRVocuher)); 
            } 
        }
        public Supplier supplierName { get { return _supplierName; } set { _supplierName = value; CheckIfEveryInputHasGiven(); CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(supplierName)); } }
        public Contructor contructorName { get { return _contructorName; } set { _contructorName = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(contructorName)); } }
        public Warehouse wareHouseName { get { return _wareHouseName; } set { _wareHouseName = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(wareHouseName)); } }
        public WorkOrder workOrderNo { get { return _workOrderNo; } set { _workOrderNo = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(workOrderNo)); } }
        public List<StockData> stockData { get { return _stockData; } set { _stockData = value; OnPropertyChanged(nameof(stockData)); } }

        public string rrDatelbl { get { return _rrDatelbl; } set { _rrDatelbl = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(rrDatelbl)); } }
        public string nextrrNumberlbl { get { return _nextrrNumberlbl; } set { _nextrrNumberlbl = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(nextrrNumberlbl)); } }
        public string dlvrTotxbx { get { return _dlvrTotxbx; } set { _dlvrTotxbx = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(dlvrTotxbx)); } }
        public string chkedBytxbx { get { return _chkedBytxbx; } set { _chkedBytxbx = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(chkedBytxbx)); } }
        public string allocreqNumbertxbx { get { return _allocreqNumbertxbx; } set { _allocreqNumbertxbx = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(allocreqNumbertxbx)); } }
        public string challantxbx { get { return _challantxbx; } set { _challantxbx = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(challantxbx)); } }
        public string condition { get { return _condition; } set { _condition = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(condition)); } }
        public string Error { get { return _Error; } set { _Error = value; OnPropertyChanged(nameof(Error)); } }
        public string ErrorColor { get { return _ErrorColor; } set { _ErrorColor = value; OnPropertyChanged(nameof(ErrorColor)); } }
        public string prgsVisibility { get { return _prgsVisibility; } set { _prgsVisibility = value; OnPropertyChanged(nameof(prgsVisibility)); } }

        public string rrDatePrv { get { return _rrDatePrv; } set { _rrDatePrv = value; OnPropertyChanged(nameof(rrDatePrv)); } }
        public string rrNumberPrv { get { return _rrNumberPrv; } set { _rrNumberPrv = value; OnPropertyChanged(nameof(rrNumberPrv)); } }
        public string allocDatePrv { get { return _allocDatePrv; } set { _allocDatePrv = value; OnPropertyChanged(nameof(allocDatePrv)); } }
        public string woacNoPrv { get { return _woacNoPrv; } set { _woacNoPrv = value; OnPropertyChanged(nameof(woacNoPrv)); } }
        public string dlvrToPrv { get { return _dlvrToPrv; } set { _dlvrToPrv = value; OnPropertyChanged(nameof(dlvrToPrv)); } }
        public string chkedByPrv { get { return _chkedByPrv; } set { _chkedByPrv = value; OnPropertyChanged(nameof(chkedByPrv)); } }
        public string conditionPrv { get { return _conditionPrv; } set { _conditionPrv = value; OnPropertyChanged(nameof(conditionPrv)); } }
        public string challanPrv { get { return _challanPrv; } set { _challanPrv = value; OnPropertyChanged(nameof(challanPrv)); } }
        public string supplierPrv { get { return _supplierPrv; } set { _supplierPrv = value; OnPropertyChanged(nameof(supplierPrv)); } }
        public string allocRqPrv { get { return _allocRqPrv; } set { _allocRqPrv = value; OnPropertyChanged(nameof(allocRqPrv)); } }
        public string blncTxbx { get { return _blncTxbx; } set { _blncTxbx = value; OnPropertyChanged(nameof(blncTxbx)); } }
        public string amountTxbx { get { return _amountTxbx; } 
            set {
                _amountTxbx = value;
                try
                {
                    if(amountTxbx != "")
                    {
                        blncTxbx = "Current Balance: " + (crrBlnc + int.Parse(amountTxbx));
                        AddItemEnabled = true;
                        sendMsg("", 2);
                    }
                    else
                    {
                        sendMsg("", 2);
                    }
                }
                catch
                {
                    sendMsg("Only numbers are allowed",1);
                }              
                OnPropertyChanged(nameof(amountTxbx)); 
            } 
        }
        public string warehousePrv { get { return _warehousePrv; } set { _warehousePrv = value; OnPropertyChanged(nameof(warehousePrv)); } }
        public string newVoucherVisibility { get { return _newVoucherVisibility; } set { _newVoucherVisibility = value; OnPropertyChanged(nameof(newVoucherVisibility)); } }
        public bool makeVoucherEnabled { get { return _makeVoucherEnabled; } set { _makeVoucherEnabled = value; OnPropertyChanged(nameof(makeVoucherEnabled)); } }
        public int prgsValue { get { return _prgsValue; } set { _prgsValue = value; OnPropertyChanged(nameof(prgsValue)); } }
        public int savingPrgs { get { return _savingPrgs; } set { _savingPrgs = value; OnPropertyChanged(nameof(savingPrgs)); } }
        public string itemVisibility { get { return _itemVisibility; } set { _itemVisibility = value; OnPropertyChanged(nameof(itemVisibility)); } }
        public string isEditable { get { return _isEditable; } set { _isEditable = value; OnPropertyChanged(nameof(isEditable)); } }
        public string contructorPrv { get { return _contructorPrv; } set { _contructorPrv = value; OnPropertyChanged(nameof(contructorPrv)); } }
        public string printSrc { get { return _printSrc; } set { _printSrc = value; OnPropertyChanged(nameof(printSrc)); } }
        public string printlbl { get { return _printlbl; } set { _printlbl = value; OnPropertyChanged(nameof(printlbl)); } }
        public string popUpVisibility { get { return _popUpVisibility; } set { _popUpVisibility = value; OnPropertyChanged(nameof(popUpVisibility)); } }
        public bool nextBtnEnabled { get { return _nextBtnEnabled; } set { _nextBtnEnabled = value; OnPropertyChanged(nameof(nextBtnEnabled)); } }
        public bool AddItemEnabled { get { return _AddItemEnabled; } set { _AddItemEnabled = value; OnPropertyChanged(nameof(AddItemEnabled)); } }
        public bool isAmountFocused { get { return _isAmountFocused; } set { _isAmountFocused = value; OnPropertyChanged(nameof(isAmountFocused)); } }
        public bool itemBxEnabled { get { return _itemBxEnabled; } set { _itemBxEnabled = value; OnPropertyChanged(nameof(itemBxEnabled)); } }


        int crrBlnc { get; set; }
        public Item itemNumber { get { return _itemNumber; } set
            { _itemNumber = value;
                int selectedWareHouseID = 0;
                for(int i = 0; i < WareHouse.Count; i++)
                {
                    if(WareHouse[i].wareHouseName == warehousePrv)
                    {
                        selectedWareHouseID = WareHouse[i].wareHouseID;
                    }
                }
                if(itemNumber != null)
                {
                    for(int i = 0; i < stockData.Count; i++)
                    {
                        if((itemNumber.itemNumber == stockData[i].itemNO) && (stockData[i].wareHouseID == selectedWareHouseID))
                        {
                            blncTxbx = "Current Balance: " + stockData[i].currentBalance;
                            crrBlnc = stockData[i].currentBalance;
                        }
                    }
                    for (int i = 0; i < ShowingVoucher.Count; i++)
                    {
                        if (itemNumber.itemNumber == ShowingVoucher[i].itemNumber)
                        {
                            itemBxEnabled = false;
                            sendMsg("Item already in voucher.", 1);
                            break;
                        }
                        if (i == ShowingVoucher.Count - 1)
                        {
                            itemBxEnabled = true;
                            sendMsg("", 2);
                        }
                    }
                    if (ShowingVoucher.Count == 0)
                    {
                        itemBxEnabled = true;
                    }
                }
                
                OnPropertyChanged(nameof(itemNumber)); } }
        #endregion

    }



    class UpdaterForReceipt : ICommand
    {
        #region ICommand Members  
        public static string txtbx;
        private MeterialReceiptViewModel viewModel;
        public UpdaterForReceipt(MeterialReceiptViewModel view)
        {
            this.viewModel = view;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public void Execute(object parameter)
        {
            if(parameter.ToString() == "ready_voucher")
            {
                viewModel.ShowNewVoucher();
            }
            else if (parameter.ToString() == "Save_Voucher")
            {
                viewModel.MakeNewVoucher();
            }
            else if (parameter.ToString() == "next_voucher")
            {
                viewModel.ShowNextVoucher();
            }
            else if(parameter.ToString() == "prevous_voucher")
            {
                viewModel.ShowNPreviousVoucher();
            }
            else if (parameter.ToString() == "print")
            {
                viewModel.PrintVoucher();
            }
            else if (parameter.ToString() == "refresh")
            {
                viewModel.rrDatePrv = "";
                viewModel.rrNumberPrv = "";
                viewModel.allocDatePrv = "";
                viewModel.woacNoPrv = "";
                viewModel.dlvrToPrv = "";
                viewModel.dlvrTotxbx = "";
                viewModel.chkedByPrv = "";
                viewModel.chkedBytxbx = "";
                viewModel.conditionPrv = "";
                viewModel.challanPrv = "";
                viewModel.challantxbx = "";
                viewModel.supplierPrv = "";
                viewModel.allocRqPrv = "";
                viewModel.allocreqNumbertxbx = "";
                viewModel.warehousePrv = "";
                viewModel.contructorPrv = "";
                viewModel.printSrc = "";
                viewModel.GetAllInfo();
            }
            else
            {
                viewModel.DeleteVoucher(parameter.ToString());
            }

        }



        #endregion
    }
    
}
