using System;
using System.Windows.Input;

namespace Silicon_Inventory.ViewModel
{
    internal class CommandHandler : ICommand
    {
        #region ICommand Members  

        public static string txtbx;
        private IssueMeterialViewModel viewModel;

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void ExecuteAsync(object parameter)
        {
            if (parameter.ToString() == "gg")
            {
                viewModel.ErrorText = "This is gg";
            }


        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}