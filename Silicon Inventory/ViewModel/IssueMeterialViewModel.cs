using Newtonsoft.Json;
using Silicon_Inventory.Commands;
using Silicon_Inventory.Model;
using Silicon_Inventory.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.MobileControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Silicon_Inventory.ViewModel
{
    public class IssueMeterialViewModel : BaseViewModel
    {
        public bool IsNewVoucher;
        public bool _isCurrentblnvEnabled { get; set; }
        public bool _searchEnabled { get; set; }
        public string _issueDate { get; set; }
        public string _dltBtnVisibility { get; set; }
        public int _newIssueVoucherTicketNumber { get; set; }
        public string _condition { get; set; }
        public Contructor _contructorName { get; set; }
        public WorkOrder _workeOrderNo { get; set; }
        public Warehouse _issuefrom { get; set; }
        
        public string _requisition { get; set; }
        public string _printImgSrc { get; set; }
        public bool _IsmVoucherEnable { get; set; }
        public string _isEditAbleVoucher { get; set; }
        public bool _ischngbtnEnabled { get; set; }
        public bool _isPrintEnable { get; set; }
        public bool _saveEnabled { get; set; }
        public bool _addItemEnabled { get; set; }
        public string _txtchangeAmount { get; set; }
        public string _popupVisibility { get; set; }    
        public string _newVoucherVisibility { get; set; }
        public string _PrVisible { get; set; }
        public int _prValue { get; set; }


        public int nowBlnc { get; set; }
        public List<Item> allItem { get; set; }
        List<Contructor> contructorList { get; set; }
        List<WorkOrder> workorder { get; set; }
        ObservableCollection<Warehouse> warehouse { get; set; }
        List<IssueVoucher> issuevoucher { get; set; }
        ObservableCollection<StockData> stockData { get; set; }

        public ObservableCollection<Contructor> name { get;  set; }
        public ObservableCollection<WorkOrder> wono { get;  set; }
        public ObservableCollection<Warehouse> wh { get; set; }
        public ObservableCollection<IssueVoucher> isv { get; set; }
        public ObservableCollection<IssueVoucher> _AllVoucherIDSearch { get; set; }
        public ObservableCollection<Item> _allItems { get; set; }
        public ObservableCollection<IssueVoucher> _newVoucher{ get; set; }
        public Item _SelectedItem { get; set; }
        public IssueVoucher _IssueVoucherID { get; set; }
        public string _SelectedItemBlnc { get; set; }

        public string ertxt { get; set; }
        public string erclr { get; set; }
        public string _issueDatePrv { get; set; }
        public string _ticketNumberPrv { get; set; }
        public string _conditionPrv { get; set; }
        public string _contructorNamePrv { get; set; }
        public string _workeOrderNoPrv { get; set; }
        public string _issuefromPrv { get; set; }
        public string _requisitionPrv { get; set; }
        public string _printTxt { get; set; }
        public int voucherCount { get; set; }
        public ICommand Updater { get; set; }


        public IssueMeterialViewModel()
        {
           
            Updater = new Updater(this);
            GetAllInfoAsync();
            
        }

        public async Task GetAllInfoAsync()
        {
            newVoucherVisibility = "Hidden";
            printTxt = "";
            ischngbtnEnabled = false;
            printImgSrc = "";
            searchEnabled = false;
            voucherCount = 0;
            IsNewVoucher = false;
            PrVisible = "Visible";
            prValue = 5;
            popupVisibility = "Hidden";
            newVoucher = new ObservableCollection<IssueVoucher>();
            isEditAbleVoucher = "Hidden";
            IsmVoucherEnable = false;
            issueDate = "Issue Date: "+DateTime.Now.ToString("dd/M/yyyy");
            Conname = StaticPageForAllData.Contructor;
            WorkOrderNo = StaticPageForAllData.WorkOrder;
            WareHouseNO = StaticPageForAllData.WareHouse;
            AllIssueVoucher = StaticPageForAllData.AllIssueVoucher;
            allItems = StaticPageForAllData.Items;
            ObservableCollection<StockData> sd = new ObservableCollection<StockData>();
            sd = StaticPageForAllData.StockData;
            stockData = StaticPageForAllData.StockData;
            warehouse = StaticPageForAllData.WareHouse;
            
            ShowVoucherOnCreate();
            if (AllIssueVoucher.Count == 1 && AllIssueVoucher[0].IssueVoucherDate == null)
            {
                ErrorText = "Error: Can't Connect with server. Please Try Again";
                ErrorColor = "#E53E3E";
            }
            else
            {
                if (AllIssueVoucher.Count == 0)
                {
                    NewIssueVoucherTicketNumber = 1001;
                    ErrorText = "Message: No voucher is added";
                    ErrorColor = "#156EAC";
                    ischngbtnEnabled = false;
                }
                else
                {
                    NewIssueVoucherTicketNumber = maxvoucher + 1;
                    ischngbtnEnabled = true;
                }
                prValue = 100;
                PrVisible = "Hidden";
                saveEnabled = false;
                searchEnabled = true;
                ischngbtnEnabled = true;
            }
           
        }
        StaticPageForAllData data = new StaticPageForAllData();
        public async Task RestInfo()
        {
            await data.GetAllIssueVoucher().ConfigureAwait(false);
            await data.GetAllItem().ConfigureAwait(false);
            await data.GetStockData().ConfigureAwait(false);
            AllIssueVoucher = StaticPageForAllData.AllIssueVoucher;
            allItems = StaticPageForAllData.Items;
            ObservableCollection<StockData> sd = new ObservableCollection<StockData>();
            sd = StaticPageForAllData.StockData;
            for (int i = 0; i < sd.Count; i++)
            {
                stockData.Add(sd[i]);
            }

        }
        public async Task RemoveIssueVoucher(IssueVoucher newVoucherToRemove)
        {
            popupVisibility = "Visible";
            string urlt = "https://api.shikkhanobish.com/api/Silicon/RemoveIssueVoucher";
            HttpClient clientt = new HttpClient();
            string jsonDataT = JsonConvert.SerializeObject(new
            {
                IssueVoucherID = newVoucherToRemove.IssueVoucherID,
                itemNumber = newVoucherToRemove.itemNumber,
                IssueQuantity = newVoucherToRemove.IssueQuantity,
                wareHouseID = newVoucherToRemove.wareHouseID
            });
            StringContent contentT = new StringContent(jsonDataT, Encoding.UTF8, "application/json");
            HttpResponseMessage responset = await clientt.PostAsync(urlt, contentT).ConfigureAwait(true);
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<Response>(resultt);
            popupVisibility = "Hidden";
            SelectedItemBlnc = "";
            txtchangeAmount = "";
            await RestInfo().ConfigureAwait(false);
            if (newVoucher.Count == 0)
            {
                ShowVoucherOnCreate();
            }
        }
        public void CheckIfEveryInputHasGiven()
        {
            int NullFlag = 0;
            try
            {
                if (String.IsNullOrEmpty(requisition.ToString()))
                {
                    NullFlag++;
                }
                if (String.IsNullOrEmpty(contructorName.ToString()))
                {
                    NullFlag++;
                }
                if (String.IsNullOrEmpty(workeOrderNo.ToString()))
                {
                    NullFlag++;
                }
                if (String.IsNullOrEmpty(condition))
                {
                    NullFlag++;
                }
                if (String.IsNullOrEmpty(issuefrom.ToString()))
                {
                    NullFlag++;
                }
                
                if (NullFlag > 0)
                {
                    IsmVoucherEnable = false;
                }
                else
                {
                    IsmVoucherEnable = true;
                }
            }
            catch (NullReferenceException ex)
            {
                IsmVoucherEnable = false;
            }



        }
        public bool isLastVoucher = false;
        public bool isFirstVoucher = false;
        public int maxvoucher { get; set; }

        public void ShowVoucherOnCreate()
        {
            ObservableCollection<IssueVoucher> voucher = new ObservableCollection<IssueVoucher>();
            for(int j = 0; j < AllIssueVoucher.Count; j++)
            {
                if(maxvoucher < AllIssueVoucher[j].IssueVoucherID)
                {
                    maxvoucher = AllIssueVoucher[j].IssueVoucherID;
                }
            }

            for (int i = 0; i < AllIssueVoucher.Count; i++)
            {
                if (AllIssueVoucher[i].IssueVoucherID == maxvoucher)
                {
                    voucher.Add(AllIssueVoucher[i]);

                }
            }
            if(voucher[0].IsPrinted == 0)
            {
                for(int j = 0; j < voucher.Count; j++)
                {
                    voucher[j].dltBtnVisibility = "Visible";
                }
            }
            else
            {
                for (int j = 0; j < voucher.Count; j++)
                {
                    voucher[j].dltBtnVisibility = "Hidden";
                }

            }
            newVoucher = voucher;
            issueDatePrv = voucher[0].IssueVoucherDate;
            ticketNumberPrv = "" + voucher[0].IssueVoucherID;
            conditionPrv = voucher[0].Condition;
            contructorNamePrv = voucher[0].contructorName;
            workeOrderNoPrv = voucher[0].workOrderNo;
            issuefromPrv = voucher[0].wareHouseName;
            requisitionPrv = voucher[0].requisition;
            if(voucher[0].IsPrinted == 0)
            {
                printImgSrc = "/Asset/pending.png";
                isEditAbleVoucher = "Visible";
                printTxt = "Printed";

            }
            else
            {
                printImgSrc = "/Asset/printed_ico.png";
                isEditAbleVoucher = "Hidden";
                printTxt = "Pending";
            }
            isLastVoucher = true;
            int prvID = maxvoucher;
            ObservableCollection<IssueVoucher> issu = new ObservableCollection<IssueVoucher>(); 
            for(int k = prvID; k > 1000;k--)
            {
                for(int l = 0; l  < AllIssueVoucher.Count; l++)
                {
                    int done = 0;
                    if (prvID == AllIssueVoucher[l].IssueVoucherID)
                    {
                        issu.Add(AllIssueVoucher[l]);
                        prvID--;
                        done = 1;
                    }
                    if(done == 1)
                    {
                        l = AllIssueVoucher.Count;
                    }
                }            
            }
            AllVoucherIDSearch = issu;
        }
        public void ShowThisVoucher()
        {
            ObservableCollection<IssueVoucher> voucher = new ObservableCollection<IssueVoucher>();
            if(maxvoucher == newVoucher[0].IssueVoucherID)
            {
                ErrorText = "Message: Last Voucher";
                ErrorColor = " #3B8DE3";
            }
            else
            {
                isFirstVoucher = false;
                int thisVOucherID = newVoucher[0].IssueVoucherID + 1;
                for (int i = 0; i < AllIssueVoucher.Count; i++)
                {
                    if (AllIssueVoucher[i].IssueVoucherID == thisVOucherID)
                    {
                        voucher.Add(AllIssueVoucher[i]);
                    }
                }
                if (voucher[0].IsPrinted == 0)
                {
                    for (int j = 0; j < voucher.Count; j++)
                    {
                        voucher[j].dltBtnVisibility = "Visible";
                    }
                }
                else
                {
                    for (int j = 0; j < voucher.Count; j++)
                    {
                        voucher[j].dltBtnVisibility = "Hidden";
                    }

                }
                newVoucher = voucher;

                issueDatePrv = voucher[0].IssueVoucherDate;
                ticketNumberPrv = "" + voucher[0].IssueVoucherID;
                conditionPrv = voucher[0].Condition;
                contructorNamePrv = voucher[0].contructorName;
                workeOrderNoPrv = voucher[0].workOrderNo;
                issuefromPrv = voucher[0].wareHouseName;
                requisitionPrv = voucher[0].requisition;
                if(thisVOucherID == maxvoucher)
                {
                    ErrorText = "Message: Last Voucher";
                    ErrorColor = "#3B8DE3";
                }
                else
                {
                    isLastVoucher = false;
                }
                if (voucher[0].IsPrinted == 0)
                {
                    printImgSrc = "/Asset/pending.png";
                    isEditAbleVoucher = "Visible";
                    printTxt = "Printed";

                }
                else
                {
                    isPrintEnable = false;
                    printImgSrc = "/Asset/printed_ico.png";
                    isEditAbleVoucher = "Hidden";
                    printTxt = "Pending";
                }
                ErrorText = "";
            }
            
        }
        public void ShowPerviousVoucher()
        {
            ObservableCollection<IssueVoucher> voucher = new ObservableCollection<IssueVoucher>();
            if (isFirstVoucher)
            {
                ErrorText = "Message: First Voucher";
                ErrorColor = " #3B8DE3";
            }
            else
            {
                isLastVoucher = false;
                int thisVOucherID = newVoucher[0].IssueVoucherID - 1;
                for (int i = 0; i < AllIssueVoucher.Count; i++)
                {
                    if (AllIssueVoucher[i].IssueVoucherID == thisVOucherID)
                    {
                        voucher.Add(AllIssueVoucher[i]);
                    }
                }
                if (voucher[0].IsPrinted == 0)
                {
                    for (int j = 0; j < voucher.Count; j++)
                    {
                        voucher[j].dltBtnVisibility = "Visible";
                    }
                }
                else
                {
                    for (int j = 0; j < voucher.Count; j++)
                    {
                        voucher[j].dltBtnVisibility = "Hidden";
                    }

                }
                newVoucher = voucher;

                issueDatePrv = voucher[0].IssueVoucherDate;
                ticketNumberPrv = "" + voucher[0].IssueVoucherID;
                conditionPrv = voucher[0].Condition;
                contructorNamePrv = voucher[0].contructorName;
                workeOrderNoPrv = voucher[0].workOrderNo;
                issuefromPrv = voucher[0].wareHouseName;
                requisitionPrv = voucher[0].requisition;
                if (thisVOucherID == 1001)
                {
                    isFirstVoucher = true;
                    ErrorText = "Message: First Voucher";
                    ErrorColor = "#3B8DE3";
                }
                else
                {
                    isFirstVoucher = false;
                }
                if (voucher[0].IsPrinted == 0)
                {

                    printImgSrc = "/Asset/pending.png";
                    isEditAbleVoucher = "Visible";
                    printTxt = "Printed";

                }
                else
                {
                    printImgSrc = "/Asset/printed_ico.png";
                    isEditAbleVoucher = "Hidden";
                    printTxt = "Pending";
                }
                ErrorText = "";
            }

        }
        public async Task MakeNewVoucherAndSave(IssueVoucher newVoucher)
        {            

            popupVisibility = "Visible";
            string urlt = "https://api.shikkhanobish.com/api/Silicon/IssueVoucer";
            HttpClient clientt = new HttpClient();
            string jsonDataT = JsonConvert.SerializeObject(new
            {
                IssueVoucherID = newVoucher.IssueVoucherID,
                IssueVoucherDate = newVoucher.IssueVoucherDate,
                itemNumber = newVoucher.itemNumber,
                description = newVoucher.description,
                IssueQuantity = newVoucher.IssueQuantity,
                wareHouseID = newVoucher.wareHouseID,
                workOrderNo = newVoucher.workOrderNo,
                Condition = newVoucher.Condition,
                contructorName = newVoucher.contructorName,
                requisition = newVoucher.requisition,
                wareHouseName = newVoucher.wareHouseName,
            });
            StringContent contentT = new StringContent(jsonDataT, Encoding.UTF8, "application/json");
            HttpResponseMessage responset = await clientt.PostAsync(urlt, contentT).ConfigureAwait(true);
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<Response>(resultt);
            await RestInfo().ConfigureAwait(false);
            popupVisibility = "Hidden";
            ErrorText = "Saving Done";
            isPrintEnable = true;
            ischngbtnEnabled = true;
            isEditAbleVoucher = "Visible";
            saveEnabled = false;
            txtchangeAmount = "";  
            if(IsNewVoucher)
            {
                maxvoucher = maxvoucher + 1;
                IsNewVoucher = false;
            }
            int prvID = maxvoucher;
            SelectedItemBlnc = "";
            ObservableCollection<IssueVoucher> issu = new ObservableCollection<IssueVoucher>();
            for (int k = prvID; k > 1000; k--)
            {
                for (int l = 0; l < AllIssueVoucher.Count; l++)
                {
                    int done = 0;
                    if (prvID == AllIssueVoucher[l].IssueVoucherID)
                    {
                        issu.Add(AllIssueVoucher[l]);
                        prvID--;
                        done = 1;
                    }
                    if (done == 1)
                    {
                        l = AllIssueVoucher.Count;
                    }
                }
            }
            AllVoucherIDSearch = issu;
            newVoucherVisibility = "Hidden";
        }
        public async Task GetStockDAta()
        {
            string urlt = "https://api.shikkhanobish.com/api/Silicon/GetStockData";
            HttpClient clientt = new HttpClient();
            HttpResponseMessage responset = await clientt.GetAsync(urlt).ConfigureAwait(true);
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<ObservableCollection<StockData>>(resultt);
            stockData = r;

        }
        public async Task GetAllItem()
        {
            string urlt = "https://api.shikkhanobish.com/api/Silicon/GetItem";
            HttpClient clientt = new HttpClient();
            HttpResponseMessage responset = await clientt.GetAsync(urlt).ConfigureAwait(true);
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<List<Item>>(resultt);
            allItem = r;
            allItems = new ObservableCollection<Item>();
            ObservableCollection<Item> allIt = new ObservableCollection<Item>();
            for (int i = 0; i < r.Count; i++)
            {
                allIt.Add(r[i]);
            }
            allItems = allIt;

        }

        public async Task GetContructor()
        {            
            string urlt = "https://api.shikkhanobish.com/api/Silicon/GetContructor";
            HttpClient clientt = new HttpClient();          
            HttpResponseMessage responset = await clientt.GetAsync(urlt).ConfigureAwait(true);            
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<List<Contructor>>(resultt);           
            contructorList = r;
            Conname = new ObservableCollection<Contructor>();
            for (int i = 0; i < r.Count; i++)
            {
                Conname.Add(r[i]);
            }         
        }
        public async Task GetWorkOrder()
        {
            string urlt = "https://api.shikkhanobish.com/api/Silicon/GetWorkOrder";
            HttpClient clientt = new HttpClient();
            HttpResponseMessage responset = await clientt.GetAsync(urlt).ConfigureAwait(true);
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<List<WorkOrder>>(resultt);
            workorder = r;
            WorkOrderNo = new ObservableCollection<WorkOrder>();
            for (int i = 0; i < r.Count; i++)
            {
                WorkOrderNo.Add(r[i]);
            }
        }
        MainPageViewModel viewModel;
        UpdateViewCommand up;
        public async Task PrintVoucher()
        {
            try
            {
                printInPrinter();
                if (newVoucher[0].IsPrinted == 0)
                {
                    popupVisibility = "Visible";
                    string urlt = "https://api.shikkhanobish.com/api/Silicon/SetIsPrinted";
                    HttpClient clientt = new HttpClient();
                    string jsonDataT = JsonConvert.SerializeObject(new { IssueVoucherID = ticketNumberPrv });
                    StringContent contentT = new StringContent(jsonDataT, Encoding.UTF8, "application/json");
                    HttpResponseMessage responset = await clientt.PostAsync(urlt, contentT).ConfigureAwait(true);
                    for (int i = 0; i < AllIssueVoucher.Count; i++)
                    {
                        if (int.Parse(ticketNumberPrv) == AllIssueVoucher[i].IssueVoucherID)
                        {
                            AllIssueVoucher[i].IsPrinted = 1;
                        }
                    }
                    ErrorText = "Saving Done";
                    isPrintEnable = false;
                    isEditAbleVoucher = "Hidden";
                    popupVisibility = "Hidden";
                    printImgSrc = "/Asset/printed_ico.png";
                    printTxt = "Printed";
                    ObservableCollection<IssueVoucher> voucher = new ObservableCollection<IssueVoucher>();
                    for (int i = 0; i < AllIssueVoucher.Count; i++)
                    {
                        if (AllIssueVoucher[i].IssueVoucherID == newVoucher[0].IssueVoucherID)
                        {
                            voucher.Add(AllIssueVoucher[i]);
                        }
                    }
                    for (int j = 0; j < newVoucher.Count; j++)
                    {
                        voucher[j].dltBtnVisibility = "Hidden";
                    }
                    await RestInfo().ConfigureAwait(false);
                    newVoucher = voucher;
                }
                ErrorText = "";
            }
           
            catch
            {
                ErrorText = "Please make a new file or connect with internet";
            }

        }
        public void printInPrinter()
        {
            StaticPageForAllData.printIssueVoucher = newVoucher;
            ObservableCollection<IssueVoucher> showThisPage = new ObservableCollection<IssueVoucher>();
            for (int i = 0; i < StaticPageForAllData.printIssueVoucher.Count; i++)
            {
                StaticPageForAllData.printIssueVoucher[i].SL = i + 1;

                for (int j = 0; j < StaticPageForAllData.StockData.Count; j++)
                {
                    if (StaticPageForAllData.printIssueVoucher[i].itemNumber == StaticPageForAllData.StockData[j].itemNO)
                    {
                        StaticPageForAllData.printIssueVoucher[i].unit = StaticPageForAllData.StockData[j].unit;
                    }
                }
                showThisPage.Add(StaticPageForAllData.printIssueVoucher[i]);

            }
            StaticPageForAllData.printIssueVoucher = showThisPage;
            StaticPageForAllData.printNumber = 1;
            if (newVoucher.Count % 30 > 0)
            {
                StaticPageForAllData.pageNumber = (newVoucher.Count / 30) + 1;
            }
            else
            {
                StaticPageForAllData.pageNumber = (newVoucher.Count / 30);
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
            up = new UpdateViewCommand(viewModel);
        }
        public async Task GetWareHouse()
        {
            string urlt = "https://api.shikkhanobish.com/api/Silicon/GetWarehouse";
            HttpClient clientt = new HttpClient();
            HttpResponseMessage responset = await clientt.GetAsync(urlt).ConfigureAwait(true);
            string resultt = await responset.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<ObservableCollection<Warehouse>>(resultt);
            warehouse = r;
            WareHouseNO = new ObservableCollection<Warehouse>();
            for (int i = 0; i < r.Count; i++)
            {
                WareHouseNO.Add(r[i]);
            }
        }

        public ObservableCollection<Contructor> Conname
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Conname));
            }
        }
        public ObservableCollection<WorkOrder> WorkOrderNo
        {
            get
            {
                return wono;
            }
            set
            {
                wono = value;
                OnPropertyChanged(nameof(WorkOrderNo));
            }
        }

        public ObservableCollection<IssueVoucher> AllIssueVoucher
        {
            get
            {
                return isv;
            }
            set
            {
                isv = value;
                OnPropertyChanged(nameof(AllIssueVoucher));
            }
        }
        public ObservableCollection<IssueVoucher> AllVoucherIDSearch
        {
            get
            {
                return _AllVoucherIDSearch;
            }
            set
            {
                _AllVoucherIDSearch = value;
                OnPropertyChanged(nameof(AllVoucherIDSearch));
            }
        }

        public ObservableCollection<Warehouse> WareHouseNO
        {
            get
            {
                return wh;
            }
            set
            {
                wh = value;
                OnPropertyChanged(nameof(WareHouseNO));
            }
        }
        public string issueDate
        {
            get
            {
                return _issueDate;
            }
            set
            {
                _issueDate = value;
                OnPropertyChanged(nameof(issueDate));
            }
        }
        public int NewIssueVoucherTicketNumber
        {
            get
            {
                return _newIssueVoucherTicketNumber;
            }
            set
            {
                _newIssueVoucherTicketNumber = value;
                OnPropertyChanged(nameof(NewIssueVoucherTicketNumber));
            }
        }
        public string ErrorText
        {
            get
            {
                return ertxt;
            }
            set
            {
                ertxt = value;
                OnPropertyChanged(nameof(ErrorText));
            }
        }
        public string ErrorColor
        {
            get
            {
                return erclr;
            }
            set
            {
                erclr = value;
                OnPropertyChanged(nameof(ErrorColor));
            }
        }
        public string issueDatePrv
        {
            get
            {
                return _issueDatePrv;
            }
            set
            {
                _issueDatePrv = value;
                OnPropertyChanged(nameof(issueDatePrv));
            }
        }
        public string ticketNumberPrv
        {
            get
            {
                return _ticketNumberPrv;
            }
            set
            {
                _ticketNumberPrv = value;
                OnPropertyChanged(nameof(ticketNumberPrv));
            }
        }

        public string conditionPrv
        {
            get
            {
                return _conditionPrv;
            }
            set
            {
                _conditionPrv = value;
                OnPropertyChanged(nameof(conditionPrv));
            }
        }
        public string contructorNamePrv
        {
            get
            {
                return _contructorNamePrv;
            }
            set
            {
                _contructorNamePrv = value;
                OnPropertyChanged(nameof(contructorNamePrv));
            }
        }
        public string workeOrderNoPrv
        {
            get
            {
                return _workeOrderNoPrv;
            }
            set
            {
                _workeOrderNoPrv = value;
                OnPropertyChanged(nameof(workeOrderNoPrv));
            }
        }
        public string issuefromPrv
        {
            get
            {
                return _issuefromPrv;
            }
            set
            {
                _issuefromPrv = value;
                OnPropertyChanged(nameof(issuefromPrv));
            }
        }

        public string requisitionPrv { get { return _requisitionPrv; } set { _requisitionPrv = value; OnPropertyChanged(nameof(requisitionPrv)); } }

        public string condition { get { return _condition; } set { _condition = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(condition)); } }
        public Contructor contructorName { get { return _contructorName; } set { _contructorName = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(contructorName)); } }
        public WorkOrder workeOrderNo { get { return _workeOrderNo; } set { _workeOrderNo = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(workeOrderNo)); } }
        public Warehouse issuefrom { get { return _issuefrom; } set { _issuefrom = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(issuefrom)); } }
        public string requisition { get { return _requisition; } set { _requisition = value; CheckIfEveryInputHasGiven(); OnPropertyChanged(nameof(requisition)); } }
        public bool IsmVoucherEnable { get { return _IsmVoucherEnable; } set { _IsmVoucherEnable = value; OnPropertyChanged(nameof(IsmVoucherEnable)); } }
        public string isEditAbleVoucher { get { return _isEditAbleVoucher; } set { _isEditAbleVoucher = value; OnPropertyChanged(nameof(isEditAbleVoucher)); } }
        public string newVoucherVisibility { get { return _newVoucherVisibility; } set { _newVoucherVisibility = value; OnPropertyChanged(nameof(newVoucherVisibility)); } }
        public string printImgSrc { get { return _printImgSrc; } set { _printImgSrc = value; OnPropertyChanged(nameof(printImgSrc)); } }
        public bool ischngbtnEnabled { get { return _ischngbtnEnabled; } set { _ischngbtnEnabled = value; OnPropertyChanged(nameof(ischngbtnEnabled)); } }
        public bool isCurrentblnvEnabled { get { return _isCurrentblnvEnabled; } set { _isCurrentblnvEnabled = value; OnPropertyChanged(nameof(isCurrentblnvEnabled)); } }
        public bool isPrintEnable { get { return _isPrintEnable; } set { _isPrintEnable = value; OnPropertyChanged(nameof(isPrintEnable)); } }
        public ObservableCollection<Item> allItems { get { return _allItems; } set { _allItems = value; OnPropertyChanged(nameof(allItems)); } }
        public Item SelectedItem { get { return _SelectedItem; } 
            set
            {
                txtchangeAmount = "";
                _SelectedItem = value;

                int nowwareHouseID = 0;
                for(int j = 0; j < warehouse.Count; j++)
                {
                    if(issuefromPrv == warehouse[j].wareHouseName)
                    {
                        nowwareHouseID = warehouse[j].wareHouseID;
                    }
                }
                if(SelectedItem != null)
                {
                    for (int i = 0; i < stockData.Count; i++)
                    {
                        if ((SelectedItem.itemNumber == stockData[i].itemNO) && (nowwareHouseID == stockData[i].wareHouseID))
                        {
                            SelectedItemBlnc = "Current Balance: " + stockData[i].currentBalance;
                            currItemBlc = stockData[i].currentBalance;
                            nowBlnc = stockData[i].currentBalance;
                            break;
                        }
                    }
                    for (int k = 0; k < newVoucher.Count; k++)
                    {
                        if (SelectedItem.itemNumber == newVoucher[k].itemNumber)
                        {
                            addItemEnabled = false;
                            ErrorText = "Error: You already added this item";
                            ErrorColor = "#FF0000";
                            isCurrentblnvEnabled = false;
                            break;
                        }
                        else
                        {
                            isCurrentblnvEnabled = true;
                            ErrorText = "";
                            addItemEnabled = true;
                        }
                    }                   
                }
                if(newVoucher.Count == 0)
                {
                    isCurrentblnvEnabled = true;
                }
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        public IssueVoucher IssueVoucherID
        {
            get { return _IssueVoucherID; }
            set
            {
                _IssueVoucherID = value;
                if(IssueVoucherID != null)
                {
                    newVoucher.Clear();
                    for (int i = 0; i < AllIssueVoucher.Count; i++)
                    {
                        if (IssueVoucherID.IssueVoucherID == AllIssueVoucher[i].IssueVoucherID)
                        {
                            newVoucher.Add(AllIssueVoucher[i]);
                        }
                    }
                    issueDatePrv = newVoucher[0].IssueVoucherDate;
                    ticketNumberPrv = "" + newVoucher[0].IssueVoucherID;
                    conditionPrv = newVoucher[0].Condition;
                    contructorNamePrv = newVoucher[0].contructorName;
                    workeOrderNoPrv = newVoucher[0].workOrderNo;
                    issuefromPrv = newVoucher[0].wareHouseName;
                    requisitionPrv = newVoucher[0].requisition;
                    if (newVoucher[0].IsPrinted == 0)
                    {
                        for (int j = 0; j < newVoucher.Count; j++)
                        {
                            newVoucher[j].dltBtnVisibility = "Visible";
                        }
                    }
                    else
                    {
                        for (int j = 0; j < newVoucher.Count; j++)
                        {
                            newVoucher[j].dltBtnVisibility = "Hidden";
                        }

                    }
                    if (newVoucher[0].IssueVoucherID == AllIssueVoucher[0].IssueVoucherID)
                    {
                        isFirstVoucher = true;
                    }
                    else
                    {
                        isFirstVoucher = false;
                    }
                    if (newVoucher[0].IssueVoucherID == maxvoucher)
                    {
                        isLastVoucher = true;
                    }
                    else
                    {
                        isLastVoucher = false;
                    }
                    if (newVoucher[0].IsPrinted == 0)
                    {
                        isPrintEnable = true;
                        printImgSrc = "/Asset/pending.png";
                        isEditAbleVoucher = "Visible";
                        printTxt = "Pending";

                    }
                    else
                    {
                        isPrintEnable = false;
                        printImgSrc = "/Asset/printed_ico.png";
                        isEditAbleVoucher = "Hidden";
                        printTxt = "Printed";
                    }
                    ErrorText = "";
                    OnPropertyChanged(nameof(IssueVoucherID));
                }
                
            }
        }
        public int currItemBlc = 0;
        public string SelectedItemBlnc { get { return _SelectedItemBlnc; } set { _SelectedItemBlnc = value; OnPropertyChanged(nameof(SelectedItemBlnc)); } }
        public bool saveEnabled { get { return _saveEnabled; } set { _saveEnabled = value; OnPropertyChanged(nameof(saveEnabled)); } }
        public bool addItemEnabled { get { return _addItemEnabled; } set { _addItemEnabled = value; OnPropertyChanged(nameof(addItemEnabled)); } }
        public bool searchEnabled { get { return _searchEnabled; } set { _searchEnabled = value; OnPropertyChanged(nameof(searchEnabled)); } }
        public string txtchangeAmount { get { return _txtchangeAmount; } 
            set { 
                _txtchangeAmount = value;
                try
                {
                    int nowwareHouseID = 0;
                    for (int j = 0; j < warehouse.Count; j++)
                    {
                        if (issuefromPrv == warehouse[j].wareHouseName)
                        {
                            nowwareHouseID = warehouse[j].wareHouseID;
                        }
                    }

                    for (int i = 0; i < stockData.Count; i++)
                    {
                        if(txtchangeAmount != "")
                        {
                            if ((SelectedItem.itemNumber == stockData[i].itemNO) && (nowwareHouseID == stockData[i].wareHouseID))
                            {
                                SelectedItemBlnc = "Current Balance: " + (stockData[i].currentBalance - int.Parse(txtchangeAmount));
                                nowBlnc = stockData[i].currentBalance;
                                break;
                            }
                        }                      
                    }
                    if (int.Parse(txtchangeAmount) > nowBlnc)
                    {
                        addItemEnabled = false;
                        ErrorText = "Error: Not enough balance";
                        ErrorColor = "#FF0000";
                    }
                    else
                    {
                        addItemEnabled = true;
                        ErrorText = "";
                    }
                    
                }
                catch
                {
                    addItemEnabled = false;
                    if(txtchangeAmount != "")
                    {
                        ErrorText = "Error: Only digits!";
                        ErrorColor = "#FF0000";
                    }
                    
                }
                
                OnPropertyChanged(nameof(txtchangeAmount)); } 
            }
        public ObservableCollection<IssueVoucher> newVoucher { get { return _newVoucher; } set { _newVoucher = value; OnPropertyChanged(nameof(newVoucher)); } }
        public string popupVisibility { get { return _popupVisibility; } set { _popupVisibility = value; OnPropertyChanged(nameof(popupVisibility)); } }
        public int prValue { get { return _prValue; } set { _prValue = value; OnPropertyChanged(nameof(prValue)); } }
        public string PrVisible { get { return _PrVisible; } set { _PrVisible = value; OnPropertyChanged(nameof(PrVisible)); } }
         public string printTxt { get { return _printTxt; } set { _printTxt = value; OnPropertyChanged(nameof(printTxt)); } }
        public string dltBtnVisibility { get { return _dltBtnVisibility; } set { dltBtnVisibility = value; OnPropertyChanged(nameof(dltBtnVisibility)); } }
    }
    class Updater : ICommand
    {
        #region ICommand Members  
        public static string  txtbx;
        private IssueMeterialViewModel viewModel;
        public Updater (IssueMeterialViewModel view)
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
            if(parameter.ToString()  == "make_voucher")
            {
                viewModel.newVoucherVisibility = "Visible";
                List<char> con = new List<char>();
                string condition = "";
                try
                {                 
                    for(int i = viewModel.condition.Length-1; i > 0; i--)
                    {
                        if (viewModel.condition[i] != ' ')
                        {
                            con.Add(viewModel.condition[i]);
                        }
                        else
                        {
                            break;
                        }                      
                    }
                    for(int j = con.Count-1; j >= 0; j--)
                    {
                        condition = condition + con[j];
                    }
                    viewModel.issueDatePrv = DateTime.Now.ToString("dd/M/yyyy");
                    viewModel.ticketNumberPrv = "" + viewModel.NewIssueVoucherTicketNumber;
                    viewModel.conditionPrv = condition;
                    viewModel.contructorNamePrv = viewModel.contructorName.contructorName;
                    viewModel.workeOrderNoPrv = viewModel.workeOrderNo.workOrderNo;
                    viewModel.issuefromPrv = viewModel.issuefrom.wareHouseName;
                    viewModel.requisitionPrv = viewModel.requisition;
                    viewModel.ErrorText = "";
                    viewModel.ischngbtnEnabled = false;
                    viewModel.isEditAbleVoucher = "Visible";
                    viewModel.IsmVoucherEnable = false;
                    viewModel.saveEnabled = false;
                    viewModel.addItemEnabled = false;
                    viewModel.txtchangeAmount = "";
                    viewModel.newVoucher.Clear();
                    viewModel.IsNewVoucher = true;
                    viewModel.NewIssueVoucherTicketNumber = viewModel.NewIssueVoucherTicketNumber + 1;
                    viewModel.searchEnabled = false;
                    viewModel.isPrintEnable = false;
                    viewModel.printImgSrc = "/Asset/pending.png";
                    viewModel.printTxt = "Pending";
                }
                catch (Exception ex)
                { 
                    
                    viewModel.ErrorText = "Error: Please check your input. Only drop down lists are available";
                    viewModel.ErrorColor = "#FF0000";
                }
               
            }

            else if (parameter.ToString() == "new_item")
            {
                IssueVoucher oneNewItem = new IssueVoucher();
                oneNewItem.IssueVoucherDate = viewModel.issueDatePrv;
                oneNewItem.IssueVoucherID = int.Parse(viewModel.ticketNumberPrv);
                List<char> con = new List<char>();
                string condition = "";
                int wareHouseID = 0;
                for(int i = 0; i < viewModel.WareHouseNO.Count; i++)
                {
                    if(viewModel.newVoucher.Count > 0)
                    {
                        if (viewModel.WareHouseNO[i].wareHouseName == viewModel.newVoucher[0].wareHouseName)
                        {
                            wareHouseID = viewModel.WareHouseNO[i].wareHouseID;
                        }
                    }
                    else if(viewModel.newVoucher.Count == 0)
                    {
                        if (viewModel.WareHouseNO[i].wareHouseName == viewModel.issuefromPrv)
                        {
                            wareHouseID = viewModel.WareHouseNO[i].wareHouseID;
                        }
                    }
                    
                }
                oneNewItem.Condition = viewModel.conditionPrv;
                oneNewItem.contructorName = viewModel.contructorNamePrv;
                oneNewItem.workOrderNo = viewModel.workeOrderNoPrv;
                oneNewItem.wareHouseID = wareHouseID;
                oneNewItem.wareHouseName = viewModel.issuefromPrv;
                oneNewItem.requisition = viewModel.requisitionPrv;
                oneNewItem.IssueQuantity = int.Parse(viewModel.txtchangeAmount);
                oneNewItem.itemNumber = viewModel.SelectedItem.itemNumber;
                oneNewItem.description = viewModel.SelectedItem.itemName;
                
                viewModel.MakeNewVoucherAndSave(oneNewItem);
                viewModel.newVoucher.Add(oneNewItem);
                viewModel.searchEnabled = true;
                viewModel.isCurrentblnvEnabled = false;
            }

            else if (parameter.ToString() == "next")
            {
                try
                {
                    viewModel.ShowThisVoucher();
                }
                catch (Exception ex)
                {
                    viewModel.ErrorText = ex.Message;
                }
                
            }
            else if (parameter.ToString() == "back")
            {
                try
                {
                    viewModel.ShowPerviousVoucher();
                }
                catch (Exception ex)
                {
                    viewModel.ErrorText = ex.Message;
                }
                
            }
            else if (parameter.ToString() == "printNow")
            {
                try
                {
                    viewModel.PrintVoucher();
                }
                catch (Exception ex)
                {
                    viewModel.ErrorText = ex.Message;
                }
                

            }
            else if (parameter.ToString() == "refresh")
            {
                viewModel.issueDatePrv = "";
                viewModel.ticketNumberPrv = "";
                viewModel.conditionPrv = "";
                viewModel.contructorNamePrv = "";
                viewModel.workeOrderNoPrv = "";
                viewModel.issuefromPrv = "";
                viewModel.requisitionPrv = "";
                viewModel.GetAllInfoAsync();

            }
            else
            {
                ObservableCollection<IssueVoucher> isv = new ObservableCollection<IssueVoucher>();
                isv = viewModel.newVoucher;
                int i = 0;
                if(viewModel.newVoucher.Count == 1)
                {
                    viewModel.ErrorText = "Error: You can not have a voucher with 0 item";
                    viewModel.ErrorColor = "#FF0000";

                }
                else
                {
                    for (i = 0; i < viewModel.newVoucher.Count; i++)
                    {
                        if (viewModel.newVoucher[i].itemNumber == parameter.ToString())
                        {
                            viewModel.RemoveIssueVoucher(viewModel.newVoucher[i]);
                            isv.RemoveAt(i);
                            break;
                        }
                    }
                    viewModel.newVoucher = isv;
                }
                 
                

            }

        }

       
        #endregion
    }
}