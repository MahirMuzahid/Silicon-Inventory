using System;
using System.Collections.Generic;
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
    /// Interaction logic for PeriodicReturnStatementView.xaml
    /// </summary>
    public partial class PeriodicReturnStatementView : UserControl
    {
        public PeriodicReturnStatementView()
        {
            InitializeComponent();
            DataContext = new PeriodicReturnStatementViewModel();
        }
    }
}
