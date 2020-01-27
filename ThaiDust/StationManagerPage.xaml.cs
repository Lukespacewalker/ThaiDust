using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ReactiveUI;
using ThaiDust.Core.Model.Persistent;
using ThaiDust.Core.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ThaiDust
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
                    this.OneWayBind(ViewModel, vm => vm.SelectedStations, v => v.SelectedStations.ItemsSource).DisposeWith(cleanup);
                    this.BindCommand(ViewModel, vm => vm.AddStationsCommand, v => v.AddButton,
                        vm => vm.SelectedStations2)
                        .DisposeWith(cleanup);
                    Observable.FromEvent<SelectionChangedEventHandler, (object,SelectionChangedEventArgs)>(
                            rxHandler => (sender, selectionChangedEventArgs) => rxHandler((sender, selectionChangedEventArgs)),
                            handler => AvailableStations.SelectionChanged += handler,
                            handler => AvailableStations.SelectionChanged -= handler)
                        .Select(_ => AvailableStations.SelectedItems.Cast<Station>().ToArray()).Subscribe(s => ViewModel.SelectedStations2 = s)
                        .DisposeWith(cleanup);
                });

        }
    }
}
