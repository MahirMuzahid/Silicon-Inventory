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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

namespace Silicon_Inventory.ViewModel
{
    public class MeterialReturnViewModel : BaseViewModel
    {
        #region MVVM Member Veriable
        UpdateViewCommand up;
        MainPageViewModel viewModel;
        public string _returnDate { get; set; }
        public string _netReturnVoucher { get; set; }
        ObservableCollection<Contructor> _Contructor { get; set; }
        ObservableCollection<WorkOrder> _WorkOrder { get; set; }
        ObservableCollection<Supplier> _Supplier { get; set; }
        ObservableCollection<Warehouse> _WareHouse { get; set; }
        ObservableCollection<ReturnVoucher> _AllReturnVoucher { get; set; }
        ObservableCollection<ReturnVoucher> _AllSearchedReturnVoucher { get; set; }
        ObservableCollection<Item> _Items { get; set; }
        ObservableCollection<ReturnVoucher> _ShowingVoucher { get; set; }
        ReturnVoucher _SearchVoucher { get; set; }
        ObservableCollection<StockData> _stockData { get; set; }
        ObservableCollection<ReturnVoucher> _searchedVoucher { get; set; }
        Contructor _contructorName { get; set; }
        Supplier _supplierName { get; set; }
        Warehouse _wareHouseName { get; set; }
        WorkOrder _workOrderNo { get; set; }
        Item _itemNumber { get; set; }
        ReturnVoucher _SelectedSearchReturnVocuher { get; set; }

        public string _cause { get; set; }
        public string _type { get; set; }
        public string _condition { get; set; }
        public string _newVoucherVisibility { get; set; }
        public string _returnDatePrv { get; set; }
        public string _returnIDPrv { get; set; }
        public string _contructorPrv { get; set; }
        public string _wareHousePrv { get; set; }
        public string _workOrderNoPrv { get; set; }
        public string _causePrv { get; set; }
        public string _returnTypePrv { get; set; }
        public string _conditionPrv { get; set; }
        public string _amountTxbx { get; set; }
        public string _currentBlnclbl { get; set; }
        public string _Error { get; set; }
        public string _ErrorColor { get; set; }
        public string _EditVisibility { get; set; }
        public bool _itemBxEnabled { get; set; }
        public bool _AddItemEnabled { get; set; }
        public bool _makeVoucherEnabled { get; set; }
        public string _popUpVisibility { get; set; }
        public string _printSrc { get; set; }
        public string _printTxt { get; set; }
        public bool _nextBtnEnabled { get; set; }


        int maxReturnVoucher = 0;
        public string red = "#E53E3E", green = "#18CB51 ", black = "#000000", blue = "#0C84F6";
        public ICommand Updater { get; set; }
        #endregion
        StaticPageForAllData data = new StaticPageForAllData();
        public MeterialReturnViewModel ()
        {
            //Updater = new UpdaterForReturn(this);
            //ShowingVoucher = new ObservableCollection<ReturnVoucher>();
            //GetAllInfo();
            //StaticPageForAllData.isGoingNewView = false;
            //Thread th = new Thread(() =>
            //{
            //    while (true)
            //    {

            //        if (StaticPageForAllData.isOnline)
            //        {
            //            newVoucherVisibility = "Hidden";

            //        }
            //        else
            //        {
            //            newVoucherVisibility = "Visible";
            //        }
            //        if (StaticPageForAllData.isGoingNewView)
            //        {
            //            break;
            //        }
            //    }
            //});
            //th.Start();
        }
        public async Task GetAllInfo()
        {
            newVoucherVisibility = "Hidden";
            nextBtnEnabled = true;
            makeVoucherEnabled = false;
            popUpVisibility = "Hidden";
            returnDate = "Return Date: "+ DateTime.Now.ToString("dd/M/yyyy");
            netReturnVoucher = "";
            
            AllReturnVoucher = StaticPageForAllData.AllReturnVoucher;
            Contructor = StaticPageForAllData.Contructor;
            WorkOrder = StaticPageForAllData.WorkOrder;
            WareHouse = StaticPageForAllData.WareHouse;
            Supplier = StaticPageForAllData.Supplier;
            Items = StaticPageForAllData.Items;
            stockData = StaticPageForAllData.StockData;
            getMaxVoucher();
            if (AllReturnVoucher.Count == 1 && AllReturnVoucher[0].returnDate == null)
            {
                sendMsg("Can't Connect with server. Please Try Again.", 1);
            }
            else
            {
                if (AllReturnVoucher.Count == 0)
                {
                    sendMsg("No voucher has been added.", 0);
                }
                else
                {
                    ShowThisVoucher(maxReturnVoucher);                 
                }
                int next = maxReturnVoucher + 1;
                netReturnVoucher = "Return ID: " + next;
                amountTxbx = "";
                currentBlnclbl = "";
                makeAllSingleSearchVoucher();
                
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
            var r = JsonConvert.DeserializeObject<ObservableCollection<StockData>>(resultt);
            stockData = r;
        }
        public async Task SaveVoucher(ReturnVoucher newRT)
        {
            newVoucherVisibility = "Hidden";
            popUpVisibility = "Visible";
            int thisVoucherID = newRT.retID;
            for (int i = 0; i < WareHouse.Count; i++)
            {
                if (newRT.ret_warehouse == WareHouse[i].wareHouseName)
                {
                    newRT.wareHouseID = WareHouse[i].wareHouseID;
                }
            }
            string urlt = "https://api.shikkhanobish.com/api/Silicon/setReturnlVoucher";
            HttpClient clientt = new HttpClient();
            string jsonDataT = JsonConvert.SerializeObject(new
            {
                retID = newRT.retID,
                returnDate = newRT.returnDate,
                contructor = newRT.contructor,
                ret_warehouse = newRT.ret_warehouse,
                wareHouseID = newRT.wareHouseID,
                workOrderNo = newRT.workOrderNo,
                cause = newRT.cause,
                ret_type = newRT.ret_type,
                condition = newRT.condition,
                ItemNo = newRT.ItemNo,
                quentity = newRT.quentity,
                Description = newRT.Description
            });
            StringContent contentT = new StringContent(jsonDataT, Encoding.UTF8, "application/json");
            HttpResponseMessage responset = await clientt.PostAsync(urlt, contentT).ConfigureAwait(true);
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<Response>(resultt);
            await RestInfo().ConfigureAwait(false);
            getMaxVoucher();
            ShowThisVoucher(thisVoucherID);
            netReturnVoucher = "";
            makeVoucherEnabled = true;
            int next = maxReturnVoucher + 1;
            netReturnVoucher = "Return ID: " + next;
            amountTxbx = "";
            currentBlnclbl = "";
            popUpVisibility = "Hidden";
            nextBtnEnabled = true; 

        }
        public async Task DeleteVoucher(string itemNo)
        {
            popUpVisibility = "Visible";
            int qnty = 0;
            int thisVoucherID = ShowingVoucher[0].retID;
            for (int i = 0; i < ShowingVoucher.Count; i++)
            {
                if(itemNo == ShowingVoucher[i].ItemNo)
                {
                    qnty = ShowingVoucher[i].quentity;
                }
            }
            string urlt = "https://api.shikkhanobish.com/api/Silicon/RemoveReturnVoucher";
            HttpClient clientt = new HttpClient();
            string jsonDataT = JsonConvert.SerializeObject(new
            {
                retID = ShowingVoucher[0].retID,
                wareHouseID = ShowingVoucher[0].wareHouseID,
                ItemNo = itemNo,
                quentity = qnty

            });
            StringContent contentT = new StringContent(jsonDataT, Encoding.UTF8, "application/json");
            HttpResponseMessage responset = await clientt.PostAsync(urlt, contentT).ConfigureAwait(true);
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<Response>(resultt);
            await RestInfo().ConfigureAwait(false);
            ShowThisVoucher(thisVoucherID);
            popUpVisibility = "Hidden";
        }
        public async Task PrintVoucher ()
        {
            try
            {
                printInPrinter();
                if (ShowingVoucher[0].isPrinted == 0)
                {
                    int thisVoucherID = ShowingVoucher[0].retID;
                    popUpVisibility = "Visible";
                    string urlt = "https://api.shikkhanobish.com/api/Silicon/PrintReturnVoucher";
                    HttpClient clientt = new HttpClient();
                    string jsonDataT = JsonConvert.SerializeObject(new
                    {
                        retID = ShowingVoucher[0].retID
                    });
                    StringContent contentT = new StringContent(jsonDataT, Encoding.UTF8, "application/json");
                    HttpResponseMessage responset = await clientt.PostAsync(urlt, contentT).ConfigureAwait(true);
                    string resultt = await responset.Content.ReadAsStringAsync();
                    var r = JsonConvert.DeserializeObject<Response>(resultt);
                    await RestInfo().ConfigureAwait(false);
                    ShowThisVoucher(thisVoucherID);
                    EditVisibility = "Hidden";
                    popUpVisibility = "Hidden";
                    SendConfirmation();
                    RestInfo();
                    sendMsg("", 2);
                }
            }
            catch
            {
                sendMsg("Please make a new file or connect with internet", 1);
            }
            
            
            
        }
        public async Task SendConfirmation()
        {
            string url = "https://siliconapi.shikkhanobish.com/api/Slicon/refreshCall?&refresh=0";
            HttpClient client = new HttpClient();
            StringContent content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content).ConfigureAwait(true);
            string result = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
        }
        public void printInPrinter()
        {
            StaticPageForAllData.printReturnVoucher = ShowingVoucher;
            ObservableCollection<ReturnVoucher> showThisPage = new ObservableCollection<ReturnVoucher>();
            for (int i = 0; i < StaticPageForAllData.printReturnVoucher.Count; i++)
            {
                StaticPageForAllData.printReturnVoucher[i].SL = i + 1;

                for (int j = 0; j < StaticPageForAllData.StockData.Count; j++)
                {
                    if (StaticPageForAllData.printReturnVoucher[i].ItemNo == StaticPageForAllData.StockData[j].itemNO)
                    {
                        StaticPageForAllData.printReturnVoucher[i].unit = StaticPageForAllData.StockData[j].unit;
                    }
                }
                showThisPage.Add(StaticPageForAllData.printReturnVoucher[i]);

            }
            StaticPageForAllData.printReturnVoucher = showThisPage;

            StaticPageForAllData.printNumber = 3;

            if (ShowingVoucher.Count % 30 > 0)
            {
                StaticPageForAllData.pageNumber = (ShowingVoucher.Count / 30) + 1;
            }
            else
            {
                StaticPageForAllData.pageNumber = (ShowingVoucher.Count / 30);
            }
            StaticPageForAllData.PrintedpageNumber = 1;

            PrintDialog myPrintDialog = new PrintDialog();

            var pageSize = new Size(8.26 * 96, 11.69 * 96);
            var document = new FixedDocument();
            document.DocumentPaginator.PageSize = pageSize;
            for (int i = 0; i < StaticPageForAllData.pageNumber; i++)
            {
                PrintIssueVoucherView print = new PrintIssueVoucherView();
                // Create Fixed Page.
                var fixedPage = new FixedPage();
                fixedPage.Width = pageSize.Width;
                fixedPage.Height = pageSize.Height;

                // Add visual, measure/arrange page.
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


        public void ShowThisVoucher(int voucherID)
        {
            if(voucherID > maxReturnVoucher)
            {
                sendMsg("Last Vocuher",0);
            }
            else if(voucherID == 1000)
            {
                sendMsg("First Vocuher", 0);
            }
            else
            {
                ObservableCollection<ReturnVoucher> thisVocher = new ObservableCollection<ReturnVoucher>();
                for (int i = 0; i < AllReturnVoucher.Count; i++)
                {
                    if (voucherID == AllReturnVoucher[i].retID)
                    {
                        thisVocher.Add(AllReturnVoucher[i]);
                    }
                }
                for (int i = 0; i < thisVocher.Count; i++)
                {
                    if(thisVocher[0].isPrinted == 0)
                    {
                        thisVocher[i].dltBtnVisibility = "Visible";
                    }
                    else
                    {
                        thisVocher[i].dltBtnVisibility = "Hidden";
                    }
                    
                }
                ShowingVoucher = thisVocher;

                conditionPrv = thisVocher[0].condition;
                returnDatePrv = thisVocher[0].returnDate;
                returnIDPrv = "" + thisVocher[0].retID;
                contructorPrv = thisVocher[0].contructor;
                wareHousePrv = thisVocher[0].ret_warehouse;
                workOrderNoPrv = thisVocher[0].workOrderNo;
                causePrv = thisVocher[0].cause;
                returnTypePrv = thisVocher[0].ret_type;
                if(thisVocher[0].isPrinted == 0)
                {
                    EditVisibility = "Visible";
                    printSrc = "/Asset/pending.png";
                    printTxt = "Pending";
                }
                else
                {
                    EditVisibility = "Hidden";
                    printSrc = "/Asset/printed_ico.png";
                    printTxt = "Printed";
                }
            }
            
            
        }


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
        public void MakeVoucher()
        {
            newVoucherVisibility = "Visible";
            nextBtnEnabled = false;
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
            conditionPrv = conditionNew;
            returnDatePrv = DateTime.Now.ToString("d/m/yyyy");
            int next = maxReturnVoucher + 1;
            returnIDPrv =""+ next;
            contructorPrv = contructorName.contructorName ;
            wareHousePrv = wareHouseName.wareHouseName;
            workOrderNoPrv = workOrderNo.workOrderNo;
            causePrv = cause;
            List<char> t = new List<char>();
            string typeNew = "";
            for (int i = type.Length - 1; i > 0; i--)
            {
                if (type[i] != ' ')
                {
                    t.Add(type[i]);
                }
                else
                {
                    break;
                }
            }
            for (int j = t.Count - 1; j >= 0; j--)
            {
                typeNew = typeNew + t[j];
            }
            returnTypePrv = typeNew;
            makeVoucherEnabled = false;
            ShowingVoucher.Clear();
            EditVisibility = "Visible";
            printSrc = "/Asset/pending.png";
            printTxt = "Pending";

        }
        public void getMaxVoucher()
        {
            
            maxReturnVoucher = 0;
            if(AllReturnVoucher.Count == 0)
            {
                maxReturnVoucher = 1000;
            }
            else
            {
                for (int j = 0; j < AllReturnVoucher.Count; j++)
                {
                    if (maxReturnVoucher < AllReturnVoucher[j].retID)
                    {
                        maxReturnVoucher = AllReturnVoucher[j].retID;
                    }
                }
            }
            
        }
        public void ReadyVoucherForSaving()
        {
            ReturnVoucher newRT = new ReturnVoucher();
            newRT.retID = int.Parse(returnIDPrv);
            newRT.returnDate = DateTime.Now.ToString("dd/M/yyyy");
            newRT.contructor = contructorPrv;
            newRT.ret_warehouse = wareHousePrv;
            newRT.workOrderNo = workOrderNoPrv;
            newRT.cause = causePrv;
            newRT.ret_type = returnTypePrv;
            newRT.condition = conditionPrv;
            newRT.ItemNo = itemNumber.itemNumber;
            newRT.quentity = int.Parse(amountTxbx);
            newRT.Description = itemNumber.itemName;
            SaveVoucher(newRT);
        }
        public async Task RestInfo()
        {
            await data.GetAllReturnVoucher().ConfigureAwait(false);
            await data.GetAllItem().ConfigureAwait(false);
            await data.GetStockData().ConfigureAwait(false);
            AllReturnVoucher = StaticPageForAllData.AllReturnVoucher;
            Items = StaticPageForAllData.Items;
            stockData = StaticPageForAllData.StockData;

        }
        public void GetNextVoucher()
        {
            ShowThisVoucher(ShowingVoucher[0].retID + 1);
        }
        public void GetPreviousVoucher()
        {
            ShowThisVoucher(ShowingVoucher[0].retID - 1);
        }
        public void makeAllSingleSearchVoucher()
        {
            int rrID = maxReturnVoucher;
            ObservableCollection<ReturnVoucher> rrV = new ObservableCollection<ReturnVoucher>();
            for (int i = 0; i < AllReturnVoucher.Count; i++)
            {
                if (rrID == AllReturnVoucher[i].retID)
                {
                    rrV.Add(AllReturnVoucher[i]);
                    rrID = rrID - 1;
                    i = -1;
                }
            }

            AllSearchedReturnVoucher = rrV;
        }
        public void CheckIfEveryInputHasGiven()
        {
            int NullFlag = 0;
            try
            {
                if (String.IsNullOrEmpty(_contructorName.ToString()))
                {
                    NullFlag++;
                }
                if (String.IsNullOrEmpty(cause))
                {
                    NullFlag++;
                }
                if (String.IsNullOrEmpty(wareHouseName.wareHouseName))
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
                if (String.IsNullOrEmpty(type))
                {
                    NullFlag++;
                }
                if (NullFlag > 0)
                {
                    makeVoucherEnabled = false;
                }
                if (!StaticPageForAllData.isOnline)
                {
                    NullFlag++;
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

        #region MVVM Binder
        public ObservableCollection<Contructor> Contructor { get { return _Contructor; } set { _Contructor = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(Contructor)); } }
        public ObservableCollection<WorkOrder> WorkOrder { get { return _WorkOrder; } set { _WorkOrder = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(WorkOrder)); } }
        public ObservableCollection<Warehouse> WareHouse { get { return _WareHouse; } set { _WareHouse = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(WareHouse)); } }
        public ObservableCollection<Supplier> Supplier { get { return _Supplier; } set { _Supplier = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(Supplier)); } }
        public ObservableCollection<ReturnVoucher> AllReturnVoucher { get { return _AllReturnVoucher; } set { _AllReturnVoucher = value; OnPropertyChanged(nameof(_AllReturnVoucher)); } }
        public ObservableCollection<Item> Items { get { return _Items; } set { _Items = value; OnPropertyChanged(nameof(Items)); } }
        public ObservableCollection<ReturnVoucher> ShowingVoucher { get { return _ShowingVoucher; } set { _ShowingVoucher = value; OnPropertyChanged(nameof(ShowingVoucher)); } }
        public ReturnVoucher SearchVoucher { get { return _SearchVoucher; } set 
            { _SearchVoucher = value;

                ShowThisVoucher(SearchVoucher.retID);                                             
                OnPropertyChanged(nameof(SearchVoucher)); } }
        public ObservableCollection<ReturnVoucher> AllSearchedReturnVoucher { get { return _AllSearchedReturnVoucher; } set { _AllSearchedReturnVoucher = value; OnPropertyChanged(nameof(AllSearchedReturnVoucher)); } }
        public ReturnVoucher SelectedSearchReturnVocuher
        {
            get { return _SelectedSearchReturnVocuher; }
            set
            {
                _SelectedSearchReturnVocuher = value;
                OnPropertyChanged(nameof(SelectedSearchReturnVocuher));
            }
        }
        public Supplier supplierName { get { return _supplierName; } set { _supplierName = value;   OnPropertyChanged(nameof(supplierName)); } }
        public Contructor contructorName { get { return _contructorName; } set { _contructorName = value;  OnPropertyChanged(nameof(contructorName)); } }
        public Warehouse wareHouseName { get { return _wareHouseName; } set { _wareHouseName = value; OnPropertyChanged(nameof(wareHouseName)); } }
        public WorkOrder workOrderNo { get { return _workOrderNo; } set { _workOrderNo = value; OnPropertyChanged(nameof(workOrderNo)); } }
        public ObservableCollection<StockData> stockData { get { return _stockData; } set { _stockData = value; OnPropertyChanged(nameof(stockData)); } }
        public string newVoucherVisibility { get { return _newVoucherVisibility; } set { _newVoucherVisibility = value; OnPropertyChanged(nameof(newVoucherVisibility)); } }
        public string returnDate { get { return _returnDate; } set { _returnDate = value; OnPropertyChanged(nameof(returnDate)); } }
        public string netReturnVoucher { get { return _netReturnVoucher; } set { _netReturnVoucher = value; OnPropertyChanged(nameof(netReturnVoucher)); } }
        public string cause { get { return _cause; } set { _cause = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(cause)); } }
        public string type { get { return _type; } set { _type = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(type)); } }
        public string condition { get { return _condition; } set { _condition = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(condition)); } }
        public string returnDatePrv { get { return _returnDatePrv; } set { _returnDatePrv = value; OnPropertyChanged(nameof(returnDatePrv)); } }
        public string returnIDPrv { get { return _returnIDPrv; } set { _returnIDPrv = value; OnPropertyChanged(nameof(returnIDPrv)); } }
        public string contructorPrv { get { return _contructorPrv; } set { _contructorPrv = value; OnPropertyChanged(nameof(contructorPrv)); } }
        public string wareHousePrv { get { return _wareHousePrv; } set { _wareHousePrv = value; OnPropertyChanged(nameof(wareHousePrv)); } }
        public string workOrderNoPrv { get { return _workOrderNoPrv; } set { _workOrderNoPrv = value; OnPropertyChanged(nameof(workOrderNoPrv)); } }
        public string causePrv { get { return _causePrv; } set { _causePrv = value; OnPropertyChanged(nameof(causePrv)); } }
        public string returnTypePrv { get { return _returnTypePrv; } set { _returnTypePrv = value; OnPropertyChanged(nameof(returnTypePrv)); } }
        public string conditionPrv { get { return _conditionPrv; } set { _conditionPrv = value; OnPropertyChanged(nameof(conditionPrv)); } }
        public bool AddItemEnabled { get { return _AddItemEnabled; } set { _AddItemEnabled = value; OnPropertyChanged(nameof(AddItemEnabled)); } }
        public string popUpVisibility { get { return _popUpVisibility; } set { _popUpVisibility = value; OnPropertyChanged(nameof(popUpVisibility)); } }
        public string printSrc { get { return _printSrc; } set { _printSrc = value; OnPropertyChanged(nameof(printSrc)); } }
        public string printTxt { get { return _printTxt; } set { _printTxt = value; OnPropertyChanged(nameof(printTxt)); } }
        public bool makeVoucherEnabled { get { return _makeVoucherEnabled; } set { _makeVoucherEnabled = value; OnPropertyChanged(nameof(makeVoucherEnabled)); } }
        public bool nextBtnEnabled { get { return _nextBtnEnabled; } set { _nextBtnEnabled = value; OnPropertyChanged(nameof(nextBtnEnabled)); } }
        public string amountTxbx { get { return _amountTxbx; } set { 
                _amountTxbx = value;


                try
                {
                    if (amountTxbx != "")
                    {
                        if (int.Parse(amountTxbx) > crrBlnc && conditionPrv == "Good")
                        {
                            sendMsg("Not enough balance!", 1);
                            AddItemEnabled = false;
                        }
                        else
                        {
                            if(conditionPrv == "Good")
                            {
                                currentBlnclbl = "Current Balance: " + (crrBlnc + int.Parse(amountTxbx));
                            }        
                            else
                            {
                                currentBlnclbl = "Current Balance: " + crrBlnc ;
                            }
                            AddItemEnabled = true;
                            sendMsg("", 2);
                        }
                    }
                    else
                    {
                        sendMsg("", 2);
                    }
                }
                catch
                {
                    sendMsg("Only numbers are allowed", 1);
                }


                OnPropertyChanged(nameof(amountTxbx)); } }
        public string currentBlnclbl { get { return _currentBlnclbl; } set { _currentBlnclbl = value; OnPropertyChanged(nameof(currentBlnclbl)); } }
        public string Error { get { return _Error; } set { _Error = value; OnPropertyChanged(nameof(Error)); } }
        public bool itemBxEnabled { get { return _itemBxEnabled; } set { _itemBxEnabled = value; OnPropertyChanged(nameof(itemBxEnabled)); } }
        public string EditVisibility { get { return _EditVisibility; } set { _EditVisibility = value; OnPropertyChanged(nameof(EditVisibility)); } }
        public string ErrorColor { get { return _ErrorColor; } set { _ErrorColor = value; OnPropertyChanged(nameof(ErrorColor)); } }
        int crrBlnc { get; set; }
        public Item itemNumber { get { return _itemNumber; } set { 
                _itemNumber = value;
                int selectedWareHouseID = 0;
                for (int i = 0; i < WareHouse.Count; i++)
                {
                    if (WareHouse[i].wareHouseName == wareHousePrv)
                    {
                        selectedWareHouseID = WareHouse[i].wareHouseID;
                    }
                }
                if (itemNumber != null)
                {
                    for (int i = 0; i < stockData.Count; i++)
                    {
                        if ((itemNumber.itemNumber == stockData[i].itemNO) && (stockData[i].wareHouseID == selectedWareHouseID))
                        {
                            currentBlnclbl = "Current Balance: " + stockData[i].currentBalance;

                            crrBlnc = stockData[i].currentBalance;
                        }
                    }
                    for (int i = 0; i < ShowingVoucher.Count; i++)
                    {
                        if (itemNumber.itemNumber == ShowingVoucher[i].ItemNo)
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

    class UpdaterForReturn : ICommand
    {
        #region ICommand Members  
        public static string txtbx;
        private MeterialReturnViewModel viewModel;
        public UpdaterForReturn(MeterialReturnViewModel view)
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
            if(parameter.ToString() == "make_voucher")
            {
                viewModel.MakeVoucher();
            }
            else if (parameter.ToString() == "save_item")
            {
                viewModel.ReadyVoucherForSaving();
            }
            else if (parameter.ToString() == "next")
            {
                viewModel.GetNextVoucher();
            }
            else if (parameter.ToString() == "previous")
            {
                viewModel.GetPreviousVoucher();
            }
            else if (parameter.ToString() == "print")
            {
                viewModel.PrintVoucher();
            }
            else if (parameter.ToString() == "refresh")
            {
                viewModel.conditionPrv = "";
                viewModel.returnDatePrv = "";
                viewModel.returnIDPrv = "";
                viewModel.contructorPrv = "";
                viewModel.wareHousePrv = "";
                viewModel.workOrderNoPrv = "";
                viewModel.causePrv = "";
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
