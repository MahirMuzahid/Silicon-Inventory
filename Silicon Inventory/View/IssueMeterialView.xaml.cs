using Newtonsoft.Json;
using Silicon_Inventory.Model;
using Silicon_Inventory.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
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
    /// Interaction logic for IssueMeterialView.xaml
    /// </summary>
    public partial class IssueMeterialView : UserControl
    {

        public IssueMeterialView()
        {
            InitializeComponent();
            DataContext = new IssueMeterialViewModel();
            this.KeyDown += new KeyEventHandler(IssueMeterialView_KeyDown);
        }
        int click = 0;
        void IssueMeterialView_KeyDown(object sender, KeyEventArgs e)        
        {
            if (click == 1)
            {
                itemListCombo.Focus();
                click = 0;
            }
            if (SaveItemBtn.IsFocused)
            {                              
                click++;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void conNamecombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void woCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void itemLisCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void amountTxbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SaveItemBtn.Focus();
            }
            
        }

        private void itemListCombo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                amountTxbx.Focus();
            }          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
