﻿using System.Reactive.Disposables;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ReactiveUI;
using ThaiDust.Core.Model.Persistent;
using ThaiDust.Core.ViewModel;
using ThaiDust.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ThaiDust.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StationManagerPage : Page, IViewFor<StationManagerViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(StationManagerViewModel), typeof(StationManagerPage), new PropertyMetadata(default(StationManagerViewModel)));

        public StationManagerViewModel ViewModel {
            get { return (StationManagerViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        object IViewFor.ViewModel {
            get => ViewModel;
            set => ViewModel = (StationManagerViewModel)value;
        }

        public StationManagerPage()
        {
            // ViewModel = new StationManagerViewModel();

            this.InitializeComponent();

            this.WhenActivated(cleanup =>
                {
                    this.OneWayBind(ViewModel, vm => vm.AvailableStations, v => v.AvailableStations.ItemsSource).DisposeWith(cleanup);
                    this.OneWayBind(ViewModel, vm => vm.ManagedStations, v => v.ManageStations.ItemsSource).DisposeWith(cleanup);

                    this.BindCommand(ViewModel, vm => vm.AddStationsCommand, v => v.AddButton, vm => vm.SelectedAvailableStation)
                        .DisposeWith(cleanup);
                    this.BindCommand(ViewModel, vm => vm.RemoveStationsCommand, v => v.RemoveButton, vm => vm.SelectedManagedStation)
                        .DisposeWith(cleanup);
                    this.BindCommand(ViewModel, vm => vm.SaveStationsCommand, v => v.SaveButton)
                        .DisposeWith(cleanup);

                    AvailableStations.GetSelectedChangedObservable<Station>()
                        .BindTo(ViewModel,vm => vm.SelectedAvailableStation)
                        .DisposeWith(cleanup);
                    ManageStations.GetSelectedChangedObservable<Station>()
                        .BindTo(ViewModel, vm => vm.SelectedManagedStation)
                        .DisposeWith(cleanup);
                });

        }
    }
}
