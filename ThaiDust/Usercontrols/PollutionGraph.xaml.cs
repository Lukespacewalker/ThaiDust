using System;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ThaiDust.Usercontrols
{
    public sealed partial class PollutionGraph : UserControl
    {
        public static readonly DependencyProperty PollutionTypeProperty = DependencyProperty.Register(
            "PollutionType", typeof(string), typeof(PollutionGraph), new PropertyMetadata(default(string)));

        public string PollutionType
        {
            get { return (string) GetValue(PollutionTypeProperty); }
            set { SetValue(PollutionTypeProperty, value); }
        }

        public static readonly DependencyProperty PollutionValueProperty = DependencyProperty.Register(
            "PollutionValue", typeof(double), typeof(PollutionGraph), new PropertyMetadata(default(double)));

        public double PollutionValue
        {
            get { return (double) GetValue(PollutionValueProperty); }
            set { SetValue(PollutionValueProperty, value); }
        }

        public PollutionGraph()
        {
            this.InitializeComponent();
        }
    }
}
