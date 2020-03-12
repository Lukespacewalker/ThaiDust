using System;
using ReactiveUI;
using Splat;
using ThaiDust.Core.ViewModel;

namespace ThaiDust.ViewModels
{
    public class RootPageViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; }

        public ReactiveCommand<string, IRoutableViewModel> NavigateCommand { get; }

        public RootPageViewModel()
        {
            // Initialize the Router.
            Router = new RoutingState();

            NavigateCommand = ReactiveCommand.CreateFromObservable<string,IRoutableViewModel>(Navigate);
        }

        private IObservable<IRoutableViewModel> Navigate(string key)
        {
            switch (key)
            {
                case "Dashboard":
                    return Router.Navigate.Execute(Locator.Current.GetService<DashboardViewModel>());
                case "Stations":
                    return Router.Navigate.Execute(Locator.Current.GetService<StationManagerViewModel>());
                case "Setting":
                    return Router.Navigate.Execute(Locator.Current.GetService<SettingViewModel>());
                default:
                    throw new ArgumentException("Navigation key is not existed", nameof(key));
            }
        }
    }
}
