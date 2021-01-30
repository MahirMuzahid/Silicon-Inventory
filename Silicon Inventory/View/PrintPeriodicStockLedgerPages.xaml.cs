using Silicon_Inventory.Model;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Silicon_Inventory.View
{
    /// <summary>
    /// Interaction logic for PrintPeriodicStockLedgerPages.xaml
    /// </summary>
    public partial class PrintPeriodicStockLedgerPages : UserControl
    {
        ObservableCollection<ObservableCollection<PrintForSteackLadger>> item = new ObservableCollection<ObservableCollection<PrintForSteackLadger>>();
        int LastItem = 0; int LastInfo = 0;
        public PrintPeriodicStockLedgerPages(ObservableCollection<ObservableCollection<PrintForSteackLadger>> allit, int lastIt, int lastInfo, int pagno)
        {
            InitializeComponent();
            pageno.Content = "Page: " + pagno;
            item = allit;
            LastItem = lastIt;
            LastInfo = lastInfo;
            placeLineinGrid();
        }
        public void placeLineinGrid()
        {
            int startlng;
            int prvlineLength = 21;
            int lineno = 0;
            ObservableCollection<PrintForSteackLadger> showThisPage = new ObservableCollection<PrintForSteackLadger>();
            for (int i = LastItem; i < item.Count; i++)
            {
                bool isBkrad = false;
                if( i == LastItem)
                {
                    startlng = prvlineLength;
                }
                else
                {
                    startlng = prvlineLength+21;
                }
                for (int j = 0; j < item[i].Count; j++)
                {
                    if (i == LastItem && j <= LastInfo)
                    {
                        j = LastInfo;
                    }
                    showThisPage.Add(item[i][j]);

                    prvlineLength = prvlineLength + 21;
                    if (j == item[i].Count - 1)
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

                    if (lineno == 35)
                    {
                        isBkrad = true;
                        break;
                    }
                }
                if (isBkrad == false)
                {
                    HorizontalLine(startlng, prvlineLength - 21, isBkrad);
                }
                else
                {
                    HorizontalLine(startlng, prvlineLength, isBkrad);
                }
                if (lineno == 35)
                {
                    break;
                }

            }
            l1.Y2 = prvlineLength;
            r1.Y2 = prvlineLength;
            ItemList.ItemsSource = showThisPage;

        }
        public void HorizontalLine(int startLength, int endLenght, bool isBreakd)
        {
            for (int i = 0; i < 9; i++)
            {
                Line HorizontalLine = new Line();
                HorizontalLine.Stroke = Brushes.Black;
                HorizontalLine.StrokeThickness = 1;
                if (i == 0)
                {
                    HorizontalLine.X1 = 50;
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
                if ((i == 2 || i == 3 || i == 4 || i == 5 || i == 6 || i == 7 || i == 8) && isBreakd == false)
                {
                    HorizontalLine.Y1 = startLength;
                    HorizontalLine.Y2 = endLenght + 21;
                }
                else
                {
                    HorizontalLine.Y1 = startLength;
                    HorizontalLine.Y2 = endLenght;
                }


                listGrid.Children.Add(HorizontalLine);
            }
        }
    }
}
