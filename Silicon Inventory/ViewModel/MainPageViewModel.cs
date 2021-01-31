using Silicon_Inventory.Commands;
using Silicon_Inventory.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Silicon_Inventory.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;
        public string _LoadingVisibility { get; set; }
        public string _txtColor { get; set; }
        public string _txt { get; set; }
        public string _usersTxt { get; set; }
        public string _passTxt { get; set; }
        public string _userName { get; set; }
        public string _onlinetxt { get; set; }
        public string _onlinetxtColoe { get; set; }
        public bool _usEn { get; set; }
        public bool _passEn { get; set; }
        public ObservableCollection<User> user = new ObservableCollection<User>();
        bool IsRightusInfo, IsRightpassInfo;
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
        public string txtColor { get { return _txtColor; } set { _txtColor = value; OnPropertyChanged(nameof(txtColor)); } }
        public string userName { get { return _userName; } set { _userName = value; OnPropertyChanged(nameof(userName)); } }
        public string onlinetxt { get { return _onlinetxt; } set { _onlinetxt = value; OnPropertyChanged(nameof(onlinetxt)); } }
        public string onlinetxtColoe { get { return _onlinetxtColoe; } set { _onlinetxtColoe = value; OnPropertyChanged(nameof(onlinetxtColoe)); } }
        public bool usEn { get { return _usEn; } set { _usEn = value; OnPropertyChanged(nameof(usEn)); } }
        public bool passEn { get { return _passEn; } set { _passEn = value; OnPropertyChanged(nameof(passEn)); } }
        public string usersTxt { get { return _usersTxt; } set 
            
            { _usersTxt = value;
                
                for(int i = 0; i < user.Count; i++)
                {
                    if(user[i].userName == usersTxt)
                    {
                        IsRightusInfo = true;
                    }
                }
                
                if(IsRightpassInfo == true && IsRightusInfo == true)
                {
                    LoadingVisibility = "Hidden";
                }
                OnPropertyChanged(nameof(usersTxt)); } }
        public string passTxt { get { return _passTxt; } set 
            
            { _passTxt = value;

                for (int i = 0; i < user.Count; i++)
                {
                    if (user[i].password == passTxt)
                    {
                        userName = user[i].userName;
                        IsRightpassInfo = true;
                    }
                }
                if (IsRightpassInfo == true && IsRightusInfo == true)
                {
                    onlinetxtColoe = "LightSeaGreen";
                    onlinetxt = "Online";
                    LoadingVisibility = "Hidden";
                }               
                OnPropertyChanged(nameof(passTxt)); } }
        public ICommand UpdateViewCommand { get; set; }
        StaticPageForAllData getData = new StaticPageForAllData();
        public MainPageViewModel()
        {
            passEn = false;
            usEn = false;
            IsRightpassInfo = false;
            IsRightusInfo = false;
            txt = "Fetching Data From Database....";
            txtColor = "White";
            LoadingVisibility = "Visible";
            UpdateViewCommand = new UpdateViewCommand(this);
            Thread th = new Thread(() =>
            {
                bool wasOffline = true;
                while (true)
                {
                    if(!CheckForInternetConnection())
                    {
                        wasOffline = true;
                        onlinetxt = "Offline";
                        onlinetxtColoe = "red";
                        txt = "Offline";
                        txtColor = "Red";
                        StaticPageForAllData.isOnline = false;
                        
                    }
                    else
                    {
                        if(wasOffline)
                        {                          
                            txt = "Fetching Data From Database....";
                            txtColor = "White";
                            GetData();
                            wasOffline = false;
                        }
                        else
                        {
                            wasOffline = false ;
                        }
                        onlinetxt = "Online";
                        onlinetxtColoe = "LightSeaGreen";
                        StaticPageForAllData.isOnline = true;
                    }
                }
            });

            th.Start();
        }
        public async Task GetData()
        {
           
            await getData.GetAllData().ConfigureAwait(false);
            if(getData.error == null)
            {
                txtColor = "#1FE972";
                txt = "Fetching Done.";
            }
            else
            {
                txt = getData.error;
                passEn = false;
                usEn = false;
            }
            user = StaticPageForAllData.Users;
            passEn = true;
            usEn = true;
            //LoadingVisibility = "Hidden";
        }


        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }






























    }


}
