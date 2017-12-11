using System;

using SlotCarsGo_GUI.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SlotCarsGo_GUI.Views
{
    public sealed partial class RaceResultsDetailControl : UserControl
    {
        public SampleOrder MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SampleOrder; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrder), typeof(RaceResultsDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

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
