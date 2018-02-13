using System;

using SlotCarsGo.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SlotCarsGo.Models.Racing;

namespace SlotCarsGo.Views
{
    public sealed partial class RaceResultsDetailControl : UserControl
    {
        public DriverResult MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as DriverResult; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(DriverResult), typeof(RaceResultsDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public RaceResultsDetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RaceResultsDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
