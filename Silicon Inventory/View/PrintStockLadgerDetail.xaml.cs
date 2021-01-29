using Silicon_Inventory.Model;
using Silicon_Inventory.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Silicon_Inventory.View
{
    /// <summary>
    /// Interaction logic for PrintStockLadgerDetail.xaml
    /// </summary>
    public partial class PrintStockLadgerDetail : UserControl
    {
        Size pageSize = new Size(8.26 * 96, 11.69 * 96);
        FixedDocument document = new FixedDocument();
        PrintDialog myPrintDialog = new PrintDialog();
        ObservableCollection<ObservableCollection<PrintForSteackLadger>> item = new ObservableCollection<ObservableCollection<PrintForSteackLadger>>();
        public PrintStockLadgerDetail(ObservableCollection<ObservableCollection<PrintForSteackLadger>> itemInfo)
        {
            InitializeComponent();
            item = itemInfo;
            DataContext = new printStockLadgerViewModel();
            storeName.Content = "Store Name: " + itemInfo[0][0].storename;
            reportDate.Content = "Reporting Date: " + DateTime.Now.ToString("dd/M/yyyy");
            itemRange.Content = "Item Range: ";
            placeLineinGrid();
        }
        FixedPage fixedPage = new FixedPage();
        FixedPage fixedPage2 = new FixedPage();
        public void placeLineinGrid()
        {
            int startlng;
            pageno.Content = "Page: " + 1;        
            int prvlineLength = 21;
            int lineno = 0;
            ObservableCollection<PrintForSteackLadger> showThisPage = new ObservableCollection<PrintForSteackLadger>();
            int itemCount = 0, infoCount = 0;
            for (int i = 0; i < item.Count; i++)
            {
                itemCount = i;
                infoCount = 0;
                bool isBkrad = false;
                startlng = prvlineLength+21;
                for (int j = 0; j < item[i].Count;j++)
                {
                    
                    showThisPage.Add(item[i][j]);

                    prvlineLength = prvlineLength + 21;
                    if(j == item[i].Count-1)
                    {
                        Line line = new Line();
                        line.X1 = 180;
                        line.Y1 = prvlineLength;
                        line.X2 = 670;
                        line.Y2 = prvlineLength;
                        line.Stroke = Brushes.Black;
                        line.StrokeThickness = 1;

                        listGrid.Children.Add(line);
                    }
                    else
                    {
                        Line line = new Line();
                        line.X1 = 0;
                        line.Y1 = prvlineLength;
                        line.X2 = 7000;
                        line.Y2 = prvlineLength;
                        line.Stroke = Brushes.Black;
                        line.StrokeThickness = 1;

                        listGrid.Children.Add(line);
                    }
                    lineno++;
                    infoCount = j;
                    if (lineno == 35)
                    {
                        isBkrad = true;
                        break;
                    }
                    
                }
                if(isBkrad == false)
                {
                    HorizontalLine(startlng, prvlineLength - 21, isBkrad);
                }
                else
                {
                    HorizontalLine(startlng, prvlineLength,isBkrad);
                }
                if (lineno == 35)
                {
                    break;
                }
                
              
            }

            l1.Y2 = prvlineLength;
            r1.Y2 = prvlineLength;
            ItemList.ItemsSource = showThisPage;
            if(itemCount ==  item.Count-1 && infoCount == item[itemCount].Count-1)
            {
                PrintThisPage();
                sendToPrint();
            }
            else
            {
                PrintThisPage();             
                int pageCount  = 0;
                int lineCount = 0;
                for (int i = itemCount; i < item.Count; i++) {
                    for(int j = 0; j < item[i].Count; j++)
                    {
                        if(i == itemCount && j < infoCount)
                        {
                            continue;
                        }
                        else
                        {
                            lineCount++;
                        }
                        
                    }
                    lineCount += 2;
                }
                pageCount = lineCount / 35;
                if(lineCount % 35 != 0)
                {
                    pageCount += 1;
                }
                int fit = itemCount;
                int infoc = infoCount;
                for(int i = 0; i < pageCount; i++)
                {
                    StaticPageForAllData.pageNumber = i + 1;
                    FixedPage fpage = PrintAnotherPage(item, fit, infoc + 1);
                    fixedPage2 = fpage;
                    var pageContent = new PageContent();
                    ((IAddChild)pageContent).AddChild(fixedPage2);
                    document.Pages.Add(pageContent);
                    int lno = 0;
                    itemCount = fit;
                    infoCount = infoc;
                    fit = 0;
                    infoc = 0;
                    for (int k = itemCount; k < item.Count; k++)
                    {
                        fit = k;
                       if( k != itemCount)
                        {
                            infoCount = 0;
                        }
                        for (int l = 0; l< item[k].Count; l++)
                        {
                            if (k == itemCount && l <= infoCount)
                            {
                                continue;
                            }
                            else
                            {
                                infoc = l;
                                lno++;
                            }
                            if (lno == 35)
                            {
                                break;
                            }
                        }
                        
                        
                        if(lno == 35)
                        {
                            break;
                        }
                        else
                        {
                            lno += 2;
                        }
                    }
                }
                sendToPrint();
            }
        }
        public void HorizontalLine(int startLength, int endLenght, bool isBreakd)
        {          
            for(int i =0; i < 9; i++)
            {
                Line HorizontalLine = new Line();
                HorizontalLine.Stroke = Brushes.Black;
                HorizontalLine.StrokeThickness = 1;
                if(i == 0)
                {
                    HorizontalLine.X1= 50;
                    HorizontalLine.X2 = 50;
                }
                if (i == 1)
                {
                    HorizontalLine.X1 = 120;
                    HorizontalLine.X2 = 120;
                }
                if (i == 2)
                {
                    HorizontalLine.X1 = 180;
                    HorizontalLine.X2 = 180;
                }
                if (i == 3)
                {
                    HorizontalLine.X1 = 270;
                    HorizontalLine.X2 = 270;
                }
                if (i == 4)
                {
                    HorizontalLine.X1 = 355;
                    HorizontalLine.X2 = 355;
                }
                if (i == 5)
                {
                    HorizontalLine.X1 = 434;
                    HorizontalLine.X2 = 434;
                }
                if (i == 6)
                {
                    HorizontalLine.X1 = 502;
                    HorizontalLine.X2 = 502;
                }
                if (i == 7)
                {
                    HorizontalLine.X1 = 585;
                    HorizontalLine.X2 = 585;
                }
                if (i == 8)
                {
                    HorizontalLine.X1 = 670;
                    HorizontalLine.X2 = 670;
                }
                if((i == 2 || i == 3 || i == 4 || i == 5 || i == 6 || i == 7|| i == 8 )&& isBreakd == false)
                {
                    HorizontalLine.Y1 = startLength;
                    HorizontalLine.Y2 = endLenght+21;
                }
                else
                {
                    HorizontalLine.Y1 = startLength;
                    HorizontalLine.Y2 = endLenght;
                }
                

                listGrid.Children.Add(HorizontalLine);
            }
        }
        public void PrintThisPage()
        {
            // Create Fixed Page.

            fixedPage.Width = pageSize.Width;
            fixedPage.Height = pageSize.Height;

            // Add visual, measure/arrange page.
            PrintStockLadgerDetail print = this;
            fixedPage.Children.Add(print);
            fixedPage.Measure(pageSize);
            fixedPage.Arrange(new Rect(new Point(), pageSize));
            fixedPage.UpdateLayout();

            // Add page to document
            var pageContent = new PageContent();
            ((IAddChild)pageContent).AddChild(fixedPage);
            document.Pages.Add(pageContent);          
        }

        public FixedPage PrintAnotherPage (ObservableCollection<ObservableCollection<PrintForSteackLadger>> allit, int lastIt, int lastInfo)
        {
            // Create Fixed Page.
            FixedPage fp = new FixedPage();
            fp.Width = pageSize.Width;
            fp.Height = pageSize.Height;

            // Add visual, measure/arrange page.
            PrintPeriodicStockLedgerPages print = new PrintPeriodicStockLedgerPages(allit, lastIt, lastInfo);
            fp.Children.Add((UIElement) print);
            fp.Measure(pageSize);
            fp.Arrange(new Rect(new Point(), pageSize));
            fp.UpdateLayout();

            return fp;
            
            
        }
        public void PrintExtraPage()
        {
            
        }

        public void sendToPrint()
        {
            // Send to the printer.
            var pd = new PrintDialog();
            if (myPrintDialog.ShowDialog() == true)
            {
                pd.PrintDocument(document.DocumentPaginator, "My Document");
            }
        }
    }
}
