using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using ReactiveUI;
using ThaiDust.Core.ViewModel;

namespace ThaiDust.ViewModels
{
    public class DashboardViewModel : DashboardViewModelCore , IRoutableViewModel
    {
        protected override void ShowError(Exception ex)
        {
            var dialog = new MessageDialog(ex.Message, "Error");
            dialog.ShowAsync();
        }
    }
}
