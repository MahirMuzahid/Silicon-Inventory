using Silicon_Inventory.Commands;
using Silicon_Inventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Silicon_Inventory.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;
        public string _LoadingVisibility { get; set; }
        public string _txt { get; set; }
        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }
        public string txt { get { return _txt; } set { _txt = value; OnPropertyChanged(nameof(txt)); } }
        public string LoadingVisibility { get { return _LoadingVisibility; } set { _LoadingVisibility = value;  OnPropertyChanged(nameof(LoadingVisibility)); } }
        public ICommand UpdateViewCommand { get; set; }
        StaticPageForAllData getData = new StaticPageForAllData();
        public MainPageViewModel()
        {
            txt = "Fetching Data From Database....";
            LoadingVisibility = "Visible";
            GetData();
            UpdateViewCommand = new UpdateViewCommand(this);
            
        }

        public async Task GetData()
        {
            await getData.GetAllData().ConfigureAwait(false);
            if(getData.error == null)
            {
                LoadingVisibility = "Hidden";
            }
            else
            {
                txt = getData.error;
            }
            
        }
    }
}
