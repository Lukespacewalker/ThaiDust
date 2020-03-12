﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using ThaiDust.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ThaiDust.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingPage : Page, IViewFor<SettingViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(SettingViewModel), typeof(SettingPage), new PropertyMetadata(default(SettingViewModel)));

        public SettingViewModel ViewModel
        {
            get { return (SettingViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (SettingViewModel)value;
        }

        public SettingPage()
        {
            this.InitializeComponent();

            this.WhenActivated(d =>
                {
                    this.Bind(ViewModel, vm => vm.CurrentTheme, v => v.DefaultButton.IsChecked, DefaultButton.Tag);
                    this.Bind(ViewModel, vm => vm.CurrentTheme, v => v.LightButton.IsChecked, LightButton.Tag);
                    this.Bind(ViewModel, vm => vm.CurrentTheme, v => v.DarkButton.IsChecked, DarkButton.Tag);
                });
        }
    }
}
