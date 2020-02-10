using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using ReactiveUI;
using Splat;
using ThaiDust.ViewModels;

namespace ThaiDust.Core.ViewModel
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
                    return Router.Navigate.Execute(new DashboardViewModel());
                case "Stations":
                    return Router.Navigate.Execute(new StationManagerViewModel());
                case "Setting":
                    return Router.Navigate.Execute(new StationManagerViewModel());
                default:
                    throw new ArgumentException("Navigation key is not existed", nameof(key));
            }
        }
    }
}
