using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Silicon_Inventory.ViewModel;

namespace Silicon_Inventory.View
{
    /// <summary>
    /// Interaction logic for PeriodicReciepReport.xaml
    /// </summary>
    public partial class PeriodicReciepReport : UserControl
    {
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        public PeriodicReciepReport()
        {
            InitializeComponent();
            DataContext = new PeriodicRecieptReportViewModel();
        }
      
    }
}
