using Silicon_Inventory.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignThemes;
using MaterialDesignColors;
using System.Windows.Controls;
using Silicon_Inventory.View;
using System.Windows.Media;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using Silicon_Inventory.Commands;

namespace Silicon_Inventory.ViewModel
{
    public class StockLadgerViewModel : BaseViewModel
    {
        #region Variable
        int sl = 1;
        public ObservableCollection<Warehouse> _warehouse { get; set; }
        public Warehouse _wareHouseName { get; set; }
        public ObservableCollection<IssueVoucher> allIssueVoucher { get; set; }
        public ObservableCollection<ReturnVoucher> allRetrunVocuher { get; set; }
        public ObservableCollection<ReceiptVoucher> allRecieptVocuher { get; set; }
        public ObservableCollection<StockData> stockData { get; set; }
        public ObservableCollection<StockLadgerModel> _ShowingReport { get; set; }
        public ObservableCollection<Contructor> _contructor { get; set; }
        public Contructor _contructorName { get; set; }
        public Item _itemNumberEnd { get; set; }

        public Item _itemNumberStart { get; set; }

        public ObservableCollection<WorkOrder> _workorder { get; set; }
        public ObservableCollection<Item> _item { get; set; }

        public WorkOrder _workorderNo { get; set; }
        public string _FdateTxbx { get; set; }
        public string _FmonthTxbx { get; set; }
        public string _FyearTxbx { get; set; }
        public string _TdateTxbx { get; set; }
        public string _TmonthTxbx { get; set; }
        public string _TyearTxbx { get; set; }

        public string _name1 { get; set; }
        public string _name2 { get; set; }
        public string _name3 { get; set; }

        public bool _IsprintEnable { get; set; }

        public int _name2Width { get; set; }

        public bool _dchkEnabled { get; set; }
        public bool _schkEnabled { get; set; }

        public bool _IsdetailChkChecked { get; set; }
        public bool _IssummaryChkChecked { get; set; }

        public bool _allItemEnabled { get; set; }
        public bool _sItemEnabled { get; set; }

        public bool _IsallItemChkChecked { get; set; }
        public bool _IssItemChkChecked { get; set; }

        public bool _itemEnabled { get; set; }

        public string _rcqt { get; set; }
        public string _isqt { get; set; }
        public string _retqt { get; set; }
        public string _cb { get; set; }

        public string _Error { get; set; }
        public string _ErrorColor { get; set; }
        public int slOrder = 0, typeOrder = 0, opBlncOrder = 0, issOrder = 0, retOrder = 0, rcptOrder = 0, clBlncOrder = 0, tikOrder = 0;
        DateTime From = new DateTime();
        DateTime To = new DateTime();
        public string red = "#E53E3E", green = "#18CB51 ", black = "#000000", blue = "#0C84F6";
        List<string> onlyDateLast = new List<string>();
        int dclick = 0, sclick = 0;
        Size pageSize = new Size(8.26 * 96, 11.69 * 96);
        FixedDocument document = new FixedDocument();
        PrintDialog myPrintDialog = new PrintDialog();
        MainPageViewModel viewModel;
        UpdateViewCommand up;
        ObservableCollection<ObservableCollection<StockLadgerModel>> StockLadgerListInfo = new ObservableCollection<ObservableCollection<StockLadgerModel>>();
        ObservableCollection<ObservableCollection<PrintForSteackLadger>> converrtedListList = new ObservableCollection<ObservableCollection<PrintForSteackLadger>>();
        ObservableCollection<StockLadgerModel> printList = new ObservableCollection<StockLadgerModel>();
        public ICommand Updater { get; set; }
        #endregion

        #region method
        public StockLadgerViewModel()
        {
            //name2Width = 80;
            //itemEnabled = true;
            //Updater = new UpdaterForStockLadgerReport(this);
            //contructor = StaticPageForAllData.Contructor;
            //warehouse = StaticPageForAllData.WareHouse;
            //workorder = StaticPageForAllData.WorkOrder;
            //item = StaticPageForAllData.Items;
            //allIssueVoucher = StaticPageForAllData.AllIssueVoucher;
            //allRetrunVocuher = StaticPageForAllData.AllReturnVoucher;
            //stockData = StaticPageForAllData.StockData;
            //allRecieptVocuher = StaticPageForAllData.AllReceiptVoucher;
            //name1 = "Date";
            //name2 = "Ticket#";
            //name3 = "Type#";
            //dchkEnabled = false;
            //schkEnabled = false;
            //allItemEnabled = false;
            //sItemEnabled = false;
        }

        public async Task Fresh()
        {
            StaticPageForAllData refresh = new StaticPageForAllData();
            await refresh.GetAllData().ConfigureAwait(false);

            contructor = StaticPageForAllData.Contructor;
            warehouse = StaticPageForAllData.WareHouse;
            workorder = StaticPageForAllData.WorkOrder;
            item = StaticPageForAllData.Items;
            allIssueVoucher = StaticPageForAllData.AllIssueVoucher;
            allRetrunVocuher = StaticPageForAllData.AllReturnVoucher;
            stockData = StaticPageForAllData.StockData;
            allRecieptVocuher = StaticPageForAllData.AllReceiptVoucher;

            ObservableCollection<StockLadgerModel> fresh = new ObservableCollection<StockLadgerModel>();
            ShowingReport = fresh;
        }
        public void SortDateInList ()
        {
            if(ShowingReport != null)
            {
                ShowingReport.Clear();
                
            }
            onlyDateLast.Clear();
            int rrID = 1001;
            List<string> onlyDate = new List<string>();

            for (int i = 0; i < allRetrunVocuher.Count; i++)
            {
                if (rrID == allRetrunVocuher[i].retID)
                {
                    onlyDate.Add(allRetrunVocuher[i].returnDate);
                    rrID = rrID + 1;
                    i = -1;
                }
            }
            rrID = 1001;
            for (int i = 0; i < allIssueVoucher.Count; i++)
            {
                if (rrID == allIssueVoucher[i].IssueVoucherID)
                {
                    onlyDate.Add(allIssueVoucher[i].IssueVoucherDate);
                    rrID = rrID + 1;
                    i = -1;
                }
            } 
            
            rrID = 1001;
            for (int i = 0; i < allRecieptVocuher.Count; i++)
            {
                if (rrID == allRecieptVocuher[i].RRVoucherID)
                {
                    onlyDate.Add(allRecieptVocuher[i].RRDate);
                    rrID = rrID + 1;
                    i = -1;
                }
            }

            

            onlyDateLast.Add(onlyDate[0]);
            for(int i = 0; i < onlyDate.Count; i++ )
            {
                int match = 0;
                for(int j = 0; j < onlyDateLast.Count; j++)
                {
                    if(onlyDate[i] == onlyDateLast[j])
                    {
                        match = 1;
                        break;
                    }
                }
                if(match == 0)
                {
                    onlyDateLast.Add(onlyDate[i]);
                }
            }
            onlyDateLast.Sort((a, b) => a.CompareTo(b));
            ShowList();
        }
        public void ShowList()
        {
            if (IsdetailChkChecked)
            {
                showDetail(itemNumberStart.itemNumber , 0);

            }
            else if (IssummaryChkChecked)
            {
                showSummary();
            }
        }
        public void showSummary()
        {
            name1 = "Item#";
            name2 = "Description";
            name3 = "Unit";
            itemEnabled = false;
            IsprintEnable = true;
            name2Width = 250;
            try
            {
                ObservableCollection<StockLadgerModel> newObjectList = new ObservableCollection<StockLadgerModel>();

                int prvClosingBlnc = 0;
                int sl = 1;
                for (int i = 0; i < item.Count; i++)
                {
                    StockLadgerModel newObj = new StockLadgerModel();
                    int rcpt = 0,isQ = 0, retQ= 0;
                    for (int j = 0; j < allRecieptVocuher.Count; j++)
                    {                        
                        DateTime thisVoucherDate = DateTime.ParseExact(allRecieptVocuher[j].RRDate, "dd/M/yyyy", null);
                        if (allRecieptVocuher[j].itemNumber == item[i].itemNumber &&
                            allRecieptVocuher[j].warehouse == wareHouseName.wareHouseName &&
                            allRecieptVocuher[j].isPrinted == 1 &&
                            (thisVoucherDate >= From && thisVoucherDate <= To))
                        {                          
                            rcpt += allRecieptVocuher[j].RCVQuantity;
                        }
                    }
                    newObj.RecieptQty = rcpt;
                    for (int j = 0; j < allIssueVoucher.Count; j++)
                    {
                        DateTime thisVoucherDate = DateTime.ParseExact(allIssueVoucher[j].IssueVoucherDate, "dd/M/yyyy", null);
                        if (allIssueVoucher[j].itemNumber == item[i].itemNumber &&
                            allIssueVoucher[j].wareHouseName == wareHouseName.wareHouseName &&
                             allIssueVoucher[j].IsPrinted == 1 &&
                            (thisVoucherDate >= From && thisVoucherDate <= To))
                        {
                            isQ += allIssueVoucher[j].IssueQuantity;
                        }
                    }
                    newObj.IssueQty = isQ;
                    for (int j = 0; j < allRetrunVocuher.Count; j++)
                    {
                        DateTime thisVoucherDate = DateTime.ParseExact(allRetrunVocuher[j].returnDate, "dd/M/yyyy", null);
                        if (allRetrunVocuher[j].ItemNo == item[i].itemNumber &&
                            allRetrunVocuher[j].ret_warehouse == wareHouseName.wareHouseName &&
                             allRetrunVocuher[j].isPrinted == 1 &&
                            (thisVoucherDate >= From && thisVoucherDate <= To))
                        {
                            retQ += allRetrunVocuher[j].quentity;
                        }
                    }
                    newObj.ReturnQty = retQ;

                    newObj.SL = sl;
                    newObj.Date = item[i].itemNumber;
                    newObj.Ticket = item[i].itemName;
                    newObj.OpeningBalance = "";
                    newObj.ClosingBalance = (rcpt+ retQ)- isQ;
                    newObj.WorkOrderNo = "";
                    newObj.Type = "Unit";
                    newObj.color = "";
                    sl++;
                    newObjectList.Add(newObj);
                }

                ShowingReport = newObjectList;
                IsprintEnable = true;
            }
            catch
            {
                IsprintEnable = false;
            }
        }
        public void printInPrinterSummery()
        {
            ObservableCollection<StockLadgerModel> printList = new ObservableCollection<StockLadgerModel>();
            if(IsallItemChkChecked)
            {
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    if (ShowingReport[i].IssueQty == 0 && ShowingReport[i].RecieptQty == 0 && ShowingReport[i].ReturnQty == 0)
                    {
                        continue;
                    }
                    else
                    {
                        printList.Add(ShowingReport[i]);
                    }
                }
                printList[0].itemRange = "All Item (Exclude 0)";
            }
            else
            {
                printList[0].itemRange = "All Item";
                printList = ShowingReport;
            }
            
            StaticPageForAllData.printStockLadgerSummery = printList;
            StaticPageForAllData.printNumber = 3;

            if (ShowingReport.Count % 30 > 0)
            {
                StaticPageForAllData.pageNumber = (ShowingReport.Count / 30) + 1;
            }
            else
            {
                StaticPageForAllData.pageNumber = (ShowingReport.Count / 30);
            }
            StaticPageForAllData.PrintedpageNumber = 1;

            PrintDialog myPrintDialog = new PrintDialog();

            var pageSize = new Size(8.26 * 96, 11.69 * 96);
            var document = new FixedDocument();
            document.DocumentPaginator.PageSize = pageSize;
            for (int i = 0; i < StaticPageForAllData.pageNumber; i++)
            {
                StockLadgerSummery print = new StockLadgerSummery();
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
        public void showDetail(string item,int m)
        {
          
            name1 = "Date";
            name2 = "Ticket#";
            name3 = "Type#";
            name2Width = 80;
            sl = 1;
            itemEnabled = true;
            try
            {
                ObservableCollection<StockLadgerModel> newObjectList = new ObservableCollection<StockLadgerModel>();

                int prvClosingBlnc = 0;
                int sl = 1;
                for (int i = 0; i < onlyDateLast.Count; i++)
                {
                    for (int j = 0; j < allRecieptVocuher.Count; j++)
                    {
                        DateTime thisVoucherDate = DateTime.ParseExact(allRecieptVocuher[j].RRDate, "dd/M/yyyy", null);
                        if (allRecieptVocuher[j].RRDate == onlyDateLast[i] && allRecieptVocuher[j].itemNumber == item && allRecieptVocuher[j].isPrinted == 1 &&
                            allRecieptVocuher[j].warehouse == wareHouseName.wareHouseName &&
                            (thisVoucherDate >= From && thisVoucherDate <= To))
                        {
                            StockLadgerModel newObj = new StockLadgerModel();
                            newObj.SL = sl;
                            newObj.Date = allRecieptVocuher[j].RRDate;
                            newObj.Ticket = ""+allRecieptVocuher[j].RRVoucherID;
                            newObj.OpeningBalance = ""+prvClosingBlnc;
                            newObj.RecieptQty = allRecieptVocuher[j].RCVQuantity;
                            newObj.ReturnQty = 0;
                            newObj.IssueQty = 0;
                            newObj.ClosingBalance = prvClosingBlnc + newObj.RecieptQty;
                            prvClosingBlnc = newObj.ClosingBalance;
                            newObj.WorkOrderNo = allRecieptVocuher[j].wordOrderNo;
                            newObj.Type = "Reciept Voucher";
                            newObj.color = "#504AE2CD";
                            newObjectList.Add(newObj);
                            sl++;

                        }
                    }
                    for (int j = 0; j < allIssueVoucher.Count; j++)
                    {
                        DateTime thisVoucherDate = DateTime.ParseExact(allIssueVoucher[j].IssueVoucherDate, "dd/M/yyyy", null);
                        if (allIssueVoucher[j].IssueVoucherDate == onlyDateLast[i] && allIssueVoucher[j].itemNumber == item && allIssueVoucher[j].IsPrinted == 1 &&
                            allIssueVoucher[j].wareHouseName == wareHouseName.wareHouseName &&
                            (thisVoucherDate >= From && thisVoucherDate <= To))
                        {
                            StockLadgerModel newObj = new StockLadgerModel();
                            newObj.SL = sl;
                            newObj.Date = allIssueVoucher[j].IssueVoucherDate;
                            newObj.Ticket = ""+allIssueVoucher[j].IssueVoucherID;
                            newObj.OpeningBalance = ""+prvClosingBlnc;
                            newObj.RecieptQty = 0;
                            newObj.ReturnQty = 0;
                            newObj.IssueQty = allIssueVoucher[j].IssueQuantity;
                            newObj.ClosingBalance = prvClosingBlnc - newObj.IssueQty;
                            newObj.WorkOrderNo = allIssueVoucher[j].workOrderNo;
                            prvClosingBlnc = newObj.ClosingBalance;
                            newObj.Type = "Issue Voucher";
                            newObj.color = "#50A15CE2";
                            newObjectList.Add(newObj);
                            sl++;
                        }

                    }

                    for (int j = 0; j < allRetrunVocuher.Count; j++)
                    {
                        DateTime thisVoucherDate = DateTime.ParseExact(allRetrunVocuher[j].returnDate, "dd/M/yyyy", null);
                        if (allRetrunVocuher[j].returnDate == onlyDateLast[i] && allRetrunVocuher[j].ItemNo == item && allRetrunVocuher[j].isPrinted == 1 &&
                            allRetrunVocuher[j].ret_warehouse == wareHouseName.wareHouseName &&
                            (thisVoucherDate >= From && thisVoucherDate <= To))
                        {
                            StockLadgerModel newObj = new StockLadgerModel();
                            newObj.SL = sl;
                            newObj.Date = allRetrunVocuher[j].returnDate;
                            newObj.Ticket = ""+allRetrunVocuher[j].retID;
                            newObj.OpeningBalance =""+ prvClosingBlnc;
                            newObj.RecieptQty = 0;
                            newObj.ReturnQty = allRetrunVocuher[j].quentity;
                            newObj.IssueQty = 0;
                            newObj.ClosingBalance = prvClosingBlnc + newObj.ReturnQty;
                            newObj.WorkOrderNo = allRetrunVocuher[j].workOrderNo;
                            prvClosingBlnc = newObj.ClosingBalance;
                            newObj.Type = "Return Voucher";
                            newObj.color = "#50E8EF5A";
                            newObjectList.Add(newObj);
                            sl++;
                        }
                    }
                }

                int top = 0, rq = 0, iq = 0, retq = 0;
                for(int i = 0; i < newObjectList.Count; i++)
                {                   
                    rq += newObjectList[i].RecieptQty;
                    iq += newObjectList[i].IssueQty;
                    retq += newObjectList[i].ReturnQty;
                    
                }

                rcqt = ""+rq;
                isqt = "" + iq;
                retqt = "" + retq;
                int cbt =  (rq + retq) - iq;
                cb = "" + cbt;

                if(m == 0)
                {
                    ShowingReport = newObjectList;
                }
                else if(m == 1)
                {
                    printList = newObjectList;
                }
                
                IsprintEnable = true;
            }
            catch
            {
                IsprintEnable = false;
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

        public void checkDate()
        {
            if(itemNumberStart != null)
            {
                if (IsdetailChkChecked == true)
                {
                    schkEnabled = false;
                }
                else
                {
                    schkEnabled = true;
                }
                if (IssummaryChkChecked == true)
                {
                    dchkEnabled = false;
                }
                else
                {
                    dchkEnabled = true;
                }
            }
           

            
            string exTxt = "";
            int error = 0; ;
            int fd = 0, fm = 0, fy = 0, td = 0, tm = 0, ty = 0;
            try
            {
                fd = int.Parse(FdateTxbx);
                fm = int.Parse(FmonthTxbx);
                fy = int.Parse(FyearTxbx);

                td = int.Parse(TdateTxbx);
                tm = int.Parse(TmonthTxbx);
                ty = int.Parse(TyearTxbx);

                if (wareHouseName == null)
                {
                    error = 3;

                }
                else if ((fd > 31 || fd < 1) || (td > 31 || td < 1))
                {
                    error = 1;
                }
                else if ((fm > 12 || fm < 1) || (tm > 12 || tm < 1))
                {
                    error = 1;
                }
                else if ((fy < 2018 || fy > int.Parse(DateTime.Now.ToString("yyyy"))) || (ty < 2018 || ty > int.Parse(DateTime.Now.ToString("yyyy"))))
                {
                    error = 1;
                }
                
                if(error == 0)
                {
                    From = new DateTime(fy, fm, fd);
                    To = new DateTime(ty, tm, td);
                    if (From > DateTime.Now || To > DateTime.Now)
                    {
                        error = 5;
                    }
                    if (From > To)
                    {
                        error = 6;
                    }
                }
                
            }
            catch (Exception ex)
            {
                error = 2;
                exTxt = ex.Message;
            }
            if (error == 1)
            {
                IsprintEnable = false;
                sendMsg("Enter Valid Date.", 1);
            }
            else if (error == 2)
            {
                IsprintEnable = false;
                sendMsg("Fill up all data", 0);
            }
            else if (error == 4)
            {
                IsprintEnable = false;
                sendMsg("Select Warehouse or Contructor", 1);
            }
            else if (error == 5)
            {
                IsprintEnable = false;
                sendMsg("Future date is not valid", 1);
            }
            else if (error == 6)
            {
                IsprintEnable = false;
                sendMsg("\"From\" date can't bigger then \"To\" date ", 1);
            }
            else if (IsdetailChkChecked == false && IssummaryChkChecked == false)
            {
                if(ShowingReport != null)
                {
                    ShowingReport.Clear();
                }
                IsprintEnable = false;
                rcqt = "";
                isqt = "";
                retqt = "";
                cb = "";
                itemEnabled = true;
                sendMsg("", 2);
            }
            else if (error == 0 && wareHouseName != null && itemNumberStart != null && (IsdetailChkChecked == true || IssummaryChkChecked == true))
            {
                sendMsg("", 2);
                From = new DateTime(fy, fm, fd);
                To = new DateTime(ty, tm, td);
                SortDateInList();
            }
            
        }
        
        public void PrintNow(int k)
        {
            if (IsallItemChkChecked == true)
            {
                document.DocumentPaginator.PageSize = pageSize;
                if (k == 0)
                {
                    StockLadgerListInfo.Add(printList);
                }
                else
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        showDetail(item[i].itemNumber, 1);
                        StockLadgerListInfo.Add(printList);

                    }
                }
                int sl;

                for (int i = 0; i < item.Count; i++)
                {
                    ObservableCollection<PrintForSteackLadger> converrtedList = new ObservableCollection<PrintForSteackLadger>();
                    sl = 1;
                    int rc = 0, iss = 0, re = 0;
                    for (int j = 0; j < StockLadgerListInfo[i].Count + 1; j++)
                    {
                        PrintForSteackLadger converrted = new PrintForSteackLadger();
                        if (j == 0)
                        {

                            converrted.SL = "Item";
                            converrted.Date = item[i].itemNumber;
                            converrted.Ticket = "";
                            converrted.Type = "";
                            converrted.OpeningBalance = "";
                            converrted.RecieptQty = "";
                            converrted.IssueQty = "";
                            converrted.ReturnQty = "";
                            converrted.ClosingBalance = "";
                            converrted.WorkOrderNo = "";
                        }
                        else if (j == StockLadgerListInfo[i].Count)
                        {

                            converrted.SL = "";
                            converrted.Date = "";
                            converrted.Ticket = "";
                            converrted.Type = "Total";
                            converrted.OpeningBalance = "";
                            converrted.RecieptQty = "" + rc;
                            converrted.IssueQty = "" + iss;
                            converrted.ReturnQty = "" + re;
                            converrted.ClosingBalance = "" + ((rc + re) - iss);
                            converrted.WorkOrderNo = "";
                        }
                        else
                        {
                            rc = rc + StockLadgerListInfo[i][j].RecieptQty;
                            iss = iss + StockLadgerListInfo[i][j].IssueQty;
                            re = re + StockLadgerListInfo[i][j].ReturnQty;

                            converrted.SL = "" + sl;
                            converrted.Date = "" + StockLadgerListInfo[i][j].Date;
                            converrted.Ticket = "" + StockLadgerListInfo[i][j].Ticket;
                            converrted.Type = "" + StockLadgerListInfo[i][j].Type;
                            converrted.OpeningBalance = "" + StockLadgerListInfo[i][j].OpeningBalance;
                            converrted.RecieptQty = "" + StockLadgerListInfo[i][j].RecieptQty;
                            converrted.IssueQty = "" + StockLadgerListInfo[i][j].IssueQty;
                            converrted.ReturnQty = "" + StockLadgerListInfo[i][j].ReturnQty;
                            converrted.ClosingBalance = "" + StockLadgerListInfo[i][j].ClosingBalance;
                            converrted.WorkOrderNo = "" + StockLadgerListInfo[i][j].WorkOrderNo;
                            converrted.storename = "" + wareHouseName.wareHouseName;
                            sl++;

                        }
                        converrtedList.Add(converrted);

                    }
                    if (converrtedList[converrtedList.Count - 1].RecieptQty == "0" && converrtedList[converrtedList.Count - 1].IssueQty == "0" && converrtedList[converrtedList.Count - 1].ReturnQty == "0" && converrtedList[converrtedList.Count - 1].ClosingBalance == "0")
                    {
                        continue;
                    }
                    else
                    {
                        converrtedListList.Add(converrtedList);
                    }
                }
                converrtedListList[0][0].itemRange = "All Item (Exclude 0)";
                PrintStockLadgerDetail print = new PrintStockLadgerDetail(converrtedListList);
            }
            else if (IssItemChkChecked == true)
            {
                showDetail(itemNumberStart.itemNumber, 0);
                ObservableCollection<PrintForSteackLadger> converrtedList = new ObservableCollection<PrintForSteackLadger>();
                sl = 1;
                int rc = 0, iss = 0, re = 0;
                for (int i = 0; i < ShowingReport.Count + 1; i++)
                {

                    if (i == 0)
                    {
                        PrintForSteackLadger converrted = new PrintForSteackLadger();
                        converrted.SL = "Item";
                        converrted.Date = itemNumberStart.itemNumber;
                        converrted.Ticket = "";
                        converrted.Type = "";
                        converrted.OpeningBalance = "";
                        converrted.RecieptQty = "";
                        converrted.IssueQty = "";
                        converrted.ReturnQty = "";
                        converrted.ClosingBalance = "";
                        converrted.WorkOrderNo = "";
                        converrtedList.Add(converrted);
                    }
                    if (i == ShowingReport.Count)
                    {
                        PrintForSteackLadger converrted = new PrintForSteackLadger();
                        converrted.SL = "";
                        converrted.Date = "";
                        converrted.Ticket = "";
                        converrted.Type = "Total";
                        converrted.OpeningBalance = "";
                        converrted.RecieptQty = "" + rc;
                        converrted.IssueQty = "" + iss;
                        converrted.ReturnQty = "" + re;
                        converrted.ClosingBalance = "" + ((rc + re) - iss);
                        converrted.WorkOrderNo = "";
                        converrtedList.Add(converrted);
                    }
                    else
                    {
                        rc = rc + ShowingReport[i].RecieptQty;
                        iss = iss + ShowingReport[i].IssueQty;
                        re = re + ShowingReport[i].ReturnQty;
                        PrintForSteackLadger converrted = new PrintForSteackLadger();
                        converrted.SL = "" + sl;
                        converrted.Date = "" + ShowingReport[i].Date;
                        converrted.Ticket = "" + ShowingReport[i].Ticket;
                        converrted.Type = "" + ShowingReport[i].Type;
                        converrted.OpeningBalance = "" + ShowingReport[i].OpeningBalance;
                        converrted.RecieptQty = "" + ShowingReport[i].RecieptQty;
                        converrted.IssueQty = "" + ShowingReport[i].IssueQty;
                        converrted.ReturnQty = "" + ShowingReport[i].ReturnQty;
                        converrted.ClosingBalance = "" + ShowingReport[i].ClosingBalance;
                        converrted.WorkOrderNo = "" + ShowingReport[i].WorkOrderNo;
                        converrted.storename = "" + wareHouseName.wareHouseName;
                        sl++;
                        converrtedList.Add(converrted);
                    }

                }
                converrtedListList.Add(converrtedList);
                converrtedListList[0][0].itemRange = itemNumberStart.itemNumber;
                PrintStockLadgerDetail print = new PrintStockLadgerDetail(converrtedListList);
            }
            else
            {
                document.DocumentPaginator.PageSize = pageSize;
                if (k == 0)
                {
                    StockLadgerListInfo.Add(printList);
                }
                else
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        showDetail(item[i].itemNumber, 1);
                        StockLadgerListInfo.Add(printList);

                    }
                }
                int sl;

                for (int i = 0; i < item.Count; i++)
                {
                    ObservableCollection<PrintForSteackLadger> converrtedList = new ObservableCollection<PrintForSteackLadger>();
                    sl = 1;
                    int rc = 0, iss = 0, re = 0;
                    for (int j = 0; j < StockLadgerListInfo[i].Count + 1; j++)
                    {
                        PrintForSteackLadger converrted = new PrintForSteackLadger();
                        if (j == 0)
                        {

                            converrted.SL = "Item";
                            converrted.Date = item[i].itemNumber;
                            converrted.Ticket = "";
                            converrted.Type = "";
                            converrted.OpeningBalance = "";
                            converrted.RecieptQty = "";
                            converrted.IssueQty = "";
                            converrted.ReturnQty = "";
                            converrted.ClosingBalance = "";
                            converrted.WorkOrderNo = "";
                        }
                        else if (j == StockLadgerListInfo[i].Count)
                        {

                            converrted.SL = "";
                            converrted.Date = "";
                            converrted.Ticket = "";
                            converrted.Type = "Total";
                            converrted.OpeningBalance = "";
                            converrted.RecieptQty = "" + rc;
                            converrted.IssueQty = "" + iss;
                            converrted.ReturnQty = "" + re;
                            converrted.ClosingBalance = "" + ((rc + re) - iss);
                            converrted.WorkOrderNo = "";
                        }
                        else
                        {
                            rc = rc + StockLadgerListInfo[i][j].RecieptQty;
                            iss = iss + StockLadgerListInfo[i][j].IssueQty;
                            re = re + StockLadgerListInfo[i][j].ReturnQty;

                            converrted.SL = "" + sl;
                            converrted.Date = "" + StockLadgerListInfo[i][j].Date;
                            converrted.Ticket = "" + StockLadgerListInfo[i][j].Ticket;
                            converrted.Type = "" + StockLadgerListInfo[i][j].Type;
                            converrted.OpeningBalance = "" + StockLadgerListInfo[i][j].OpeningBalance;
                            converrted.RecieptQty = "" + StockLadgerListInfo[i][j].RecieptQty;
                            converrted.IssueQty = "" + StockLadgerListInfo[i][j].IssueQty;
                            converrted.ReturnQty = "" + StockLadgerListInfo[i][j].ReturnQty;
                            converrted.ClosingBalance = "" + StockLadgerListInfo[i][j].ClosingBalance;
                            converrted.WorkOrderNo = "" + StockLadgerListInfo[i][j].WorkOrderNo;
                            converrted.storename = "" + wareHouseName.wareHouseName;
                            sl++;

                        }
                        converrtedList.Add(converrted);

                    }
                    converrtedListList.Add(converrtedList);
                }
               
                converrtedListList[0][0].itemRange = "All Item";
                PrintStockLadgerDetail print = new PrintStockLadgerDetail(converrtedListList);
            }
        }
        #region Sort 
        public void SortSL(int order)
        {
            if (order == 0)
            {
                List<StockLadgerModel> wos = new List<StockLadgerModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderByDescending(o => o.SL).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                slOrder = 1;
            }
            else if (order == 1)
            {
                List<StockLadgerModel> wos = new List<StockLadgerModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderBy(o => o.SL).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                slOrder = 0;
            }
        }
        public void SortTicket(int order)
        {
            if (order == 0)
            {
                List<StockLadgerModel> wos = new List<StockLadgerModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderByDescending(o => o.Ticket).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                tikOrder = 1;
            }
            else if (order == 1)
            {
                List<StockLadgerModel> wos = new List<StockLadgerModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderBy(o => o.Ticket).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                tikOrder = 0;
            }
        }
        public void SortOpBlnc(int order)
        {
            if (order == 0)
            {
                List<StockLadgerModel> wos = new List<StockLadgerModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderByDescending(o => o.OpeningBalance).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                opBlncOrder = 1;
            }
            else if (order == 1)
            {
                List<StockLadgerModel> wos = new List<StockLadgerModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderBy(o => o.OpeningBalance).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                opBlncOrder = 0;
            }
        }
        public void SortrcptQty(int order)
        {
            if (order == 0)
            {
                List<StockLadgerModel> wos = new List<StockLadgerModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderByDescending(o => o.RecieptQty).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                rcptOrder = 1;
            }
            else if (order == 1)
            {
                List<StockLadgerModel> wos = new List<StockLadgerModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderBy(o => o.RecieptQty).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                rcptOrder = 0;
            }
        }
        public void SortissQty(int order)
        {
            if (order == 0)
            {
                List<StockLadgerModel> wos = new List<StockLadgerModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderByDescending(o => o.IssueQty).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                issOrder = 1;
            }
            else if (order == 1)
            {
                List<StockLadgerModel> wos = new List<StockLadgerModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderBy(o => o.IssueQty).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                issOrder = 0;
            }
        }
        public void SortrtQty(int order)
        {
            if (order == 0)
            {
                List<StockLadgerModel> wos = new List<StockLadgerModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderByDescending(o => o.ReturnQty).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                retOrder = 1;
            }
            else if (order == 1)
            {
                List<StockLadgerModel> wos = new List<StockLadgerModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderBy(o => o.ReturnQty).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                retOrder = 0;
            }
        }
        public void SortClQty(int order)
        {
            if (order == 0)
            {
                List<StockLadgerModel> wos = new List<StockLadgerModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderByDescending(o => o.ClosingBalance).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                clBlncOrder = 1;
            }
            else if (order == 1)
            {
                List<StockLadgerModel> wos = new List<StockLadgerModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    wos.Add(ShowingReport[i]);
                }
                wos = wos.OrderBy(o => o.ClosingBalance).ToList();
                ShowingReport.Clear();
                for (int i = 0; i < wos.Count; i++)
                {
                    ShowingReport.Add(wos[i]);
                }
                clBlncOrder = 0;
            }
        }
        public void SortType(int itemorder)
        {
            if (itemorder == 0)
            {
                List<StockLadgerModel> spr = new List<StockLadgerModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr.Sort((x, y) => string.Compare(x.Type, y.Type));
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                typeOrder = 1;
            }
            else if (itemorder == 1)
            {
                List<StockLadgerModel> spr = new List<StockLadgerModel>();
                for (int i = 0; i < ShowingReport.Count; i++)
                {
                    spr.Add(ShowingReport[i]);
                }
                spr.Sort((y, x) => string.Compare(x.Type, y.Type));
                ShowingReport.Clear();
                for (int i = 0; i < spr.Count; i++)
                {
                    ShowingReport.Add(spr[i]);
                }
                typeOrder = 0;
            }
        }
        #endregion
        #endregion

        #region Veriable Binding
        public string FdateTxbx { get { return _FdateTxbx; } set { _FdateTxbx = value; checkDate(); OnPropertyChanged(nameof(FdateTxbx)); } }
        public string FmonthTxbx { get { return _FmonthTxbx; } set { _FmonthTxbx = value; checkDate(); OnPropertyChanged(nameof(FmonthTxbx)); } } 
        public string FyearTxbx { get { return _FyearTxbx; } set { _FyearTxbx = value; checkDate(); OnPropertyChanged(nameof(FyearTxbx)); } }

        public string TdateTxbx { get { return _TdateTxbx; } set { _TdateTxbx = value; checkDate(); OnPropertyChanged(nameof(TdateTxbx)); } }
        public string TmonthTxbx { get { return _TmonthTxbx; } set { _TmonthTxbx = value; checkDate(); OnPropertyChanged(nameof(TmonthTxbx)); } }
        public string TyearTxbx { get { return _TyearTxbx; } set { _TyearTxbx = value; checkDate(); OnPropertyChanged(nameof(TyearTxbx)); } }
        public Contructor contructorName { get { return _contructorName; } set { _contructorName = value; checkDate(); OnPropertyChanged(nameof(contructorName)); } }
        public ObservableCollection<Item> item { get { return _item; } set { _item = value; OnPropertyChanged(nameof(item)); } }
        public Item itemNumberEnd { get { return _itemNumberEnd; } set { _itemNumberEnd = value; checkDate(); OnPropertyChanged(nameof(itemNumberEnd)); } }
        public Item itemNumberStart { get { return _itemNumberStart; } set { _itemNumberStart = value; checkDate();  OnPropertyChanged(nameof(itemNumberStart)); } }
        public ObservableCollection<Contructor> contructor { get { return _contructor; } set { _contructor = value; OnPropertyChanged(nameof(contructor)); } }
        public ObservableCollection<StockLadgerModel> ShowingReport { get { return _ShowingReport; } set { _ShowingReport = value; OnPropertyChanged(nameof(ShowingReport)); } }
        public ObservableCollection<WorkOrder> workorder { get { return _workorder; } set { _workorder = value; OnPropertyChanged(nameof(workorder)); } }
        public Warehouse wareHouseName { get { return _wareHouseName; } set { _wareHouseName = value; checkDate(); OnPropertyChanged(nameof(wareHouseName)); } }
        public ObservableCollection<Warehouse> warehouse { get { return _warehouse; } set { _warehouse = value; OnPropertyChanged(nameof(warehouse)); } }
        public WorkOrder workorderNo { get { return _workorderNo; } set { _workorderNo = value; checkDate(); OnPropertyChanged(nameof(workorderNo)); } }
        public string Error { get { return _Error; } set { _Error = value; OnPropertyChanged(nameof(Error)); } }
        public string ErrorColor { get { return _ErrorColor; } set { _ErrorColor = value; OnPropertyChanged(nameof(ErrorColor)); } }
        public bool IsprintEnable { get { return _IsprintEnable; } set { _IsprintEnable = value; OnPropertyChanged(nameof(IsprintEnable)); } }
        public string rcqt { get { return _rcqt; } set { _rcqt = value;  OnPropertyChanged(nameof(rcqt)); } }
        public string isqt { get { return _isqt; } set { _isqt = value;  OnPropertyChanged(nameof(isqt)); } }
        public string retqt { get { return _retqt; } set { _retqt = value;  OnPropertyChanged(nameof(retqt)); } }
        public string cb { get { return _cb; } set { _cb = value;  OnPropertyChanged(nameof(cb)); } }

        public bool itemEnabled { get { return _itemEnabled; } set { _itemEnabled = value; OnPropertyChanged(nameof(itemEnabled)); } }
        public bool dchkEnabled { get { return _dchkEnabled; } set { _dchkEnabled = value; allItemEnabled = true; sItemEnabled = true; OnPropertyChanged(nameof(dchkEnabled)); } }
        public bool schkEnabled { get { return _schkEnabled; } set { _schkEnabled = value; 
                OnPropertyChanged(nameof(schkEnabled)); } }

        public bool allItemEnabled { get { return _allItemEnabled; } set { _allItemEnabled = value; OnPropertyChanged(nameof(allItemEnabled)); } }
        public bool sItemEnabled { get { return _sItemEnabled; } set { _sItemEnabled = value; OnPropertyChanged(nameof(sItemEnabled)); } }

        public bool IsallItemChkChecked { get { return _IsallItemChkChecked; } set { _IsallItemChkChecked = value; OnPropertyChanged(nameof(IsallItemChkChecked)); } }
        public bool IssItemChkChecked { get { return _IssItemChkChecked; } set { _IssItemChkChecked = value; OnPropertyChanged(nameof(IssItemChkChecked)); } }

        public int name2Width { get { return _name2Width; } set { _name2Width = value; OnPropertyChanged(nameof(name2Width)); } }
        public bool IsdetailChkChecked { get { return _IsdetailChkChecked; } set { _IsdetailChkChecked = value; checkDate();
                OnPropertyChanged(nameof(IsdetailChkChecked)); } }
        public bool IssummaryChkChecked { get { return _IssummaryChkChecked; } set {_IssummaryChkChecked = value; checkDate(); OnPropertyChanged(nameof(IssummaryChkChecked)); } }

        public string name1 { get { return _name1; } set { _name1 = value; OnPropertyChanged(nameof(name1)); } }
        public string name2 { get { return _name2; } set { _name2 = value; OnPropertyChanged(nameof(name2)); } }
        public string name3 { get { return _name3; } set { _name3 = value;  OnPropertyChanged(nameof(name3)); } }

        #endregion
    }
    class UpdaterForStockLadgerReport : ICommand
    {
        #region ICommand Members  
        public static string txtbx;
        private StockLadgerViewModel viewModel;
        public UpdaterForStockLadgerReport(StockLadgerViewModel view)
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
            
            if (parameter.ToString() == "sl")
            {
                viewModel.SortSL(viewModel.slOrder);
            }
            else if (parameter.ToString() == "ty")
            {
                if(viewModel.name3 == "Type#")
                {
                    viewModel.SortType(viewModel.typeOrder);
                }                
            }
            else if (parameter.ToString() == "it")
            {
                if (viewModel.name1 == "Item#")
                {
                    viewModel.SortType(viewModel.typeOrder);
                }
                
            }
            else if (parameter.ToString() == "to")
            {
                if (viewModel.name2 == "Ticket#")
                {
                    viewModel.SortTicket(viewModel.tikOrder);
                }
            }
            
            else if (parameter.ToString() == "opb")
            {
                viewModel.SortOpBlnc(viewModel.opBlncOrder);
            }
            else if (parameter.ToString() == "rcq")
            {
                viewModel.SortrcptQty(viewModel.rcptOrder);
            }
            else if (parameter.ToString() == "iq")
            {
                viewModel.SortissQty(viewModel.issOrder);
            }
            else if (parameter.ToString() == "retq")
            {
                viewModel.SortrtQty(viewModel.retOrder);
            }
            else if (parameter.ToString() == "cb")
            {
                viewModel.SortClQty(viewModel.clBlncOrder);
            }
            else if (parameter.ToString() == "print")
            {
                if(viewModel.IssummaryChkChecked == true)
                {
                    viewModel.printInPrinterSummery();
                }
                else
                {
                    viewModel.PrintNow(1);
                }
                
            }
            else if (parameter.ToString() == "refresh")
            {
                viewModel.Fresh();
            }

        }



        #endregion
    }
}
