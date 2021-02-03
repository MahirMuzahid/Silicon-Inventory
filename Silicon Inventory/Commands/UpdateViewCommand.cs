using Silicon_Inventory.Model;
using Silicon_Inventory.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Silicon_Inventory.Commands
{
    class UpdateViewCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private MainPageViewModel viewModel;

        public UpdateViewCommand(MainPageViewModel viewModel)
        {
            this.viewModel = viewModel;
            viewModel.SelectedViewModel = new DashBoardViewModel();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            StaticPageForAllData.isGoingNewView = true;
            if (parameter.ToString() == "dashboard")
            {
                viewModel.SelectedViewModel = new DashBoardViewModel();
            }
            else if (parameter.ToString() == "issue_meterial")
            {
                viewModel.SelectedViewModel = new IssueMeterialViewModel();
            }
            else if (parameter.ToString() == "met_recp")
            {
                viewModel.SelectedViewModel = new MeterialReceiptViewModel();
            }
            else if (parameter.ToString() == "mt_return")
            {
                viewModel.SelectedViewModel = new MeterialReturnViewModel();
            }
            else if (parameter.ToString() == "periodic_rpt_rcpt")
            {
                viewModel.SelectedViewModel = new PeriodicRecieptReportViewModel();
            }
            else if (parameter.ToString() == "WO_issue_stmt")
            {
                viewModel.SelectedViewModel = new WOWisePeriodicIssueStatementViewModel();
            }
            else if (parameter.ToString() == "periodic_return_stmt")
            {
                viewModel.SelectedViewModel = new PeriodicReturnStatementViewModel();
            }
            else if (parameter.ToString() == "req.stt")
            {
                viewModel.SelectedViewModel = new RequisitionStatement();
            }
            else if (parameter.ToString() == "crrsttblnc")
            {
                viewModel.SelectedViewModel = new StockBalanceReportViewModel();
            }
            else if (parameter.ToString() == "stock_ld_dtl")
            {
                viewModel.SelectedViewModel = new StockLadgerViewModel();
            }
            else if (parameter.ToString() == "add_item")
            {
                viewModel.SelectedViewModel = new AddItemViewModel();
            }
            else if (parameter.ToString() == "all_list")
            {
                viewModel.SelectedViewModel = new AllListViewModel();
            }
        }

    }
}
