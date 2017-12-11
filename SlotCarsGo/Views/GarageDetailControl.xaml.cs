using System;

using SlotCarsGo.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SlotCarsGo.Views
{
    public sealed partial class GarageDetailControl : UserControl
    {
        public SampleOrder MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SampleOrder; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrder), typeof(GarageDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public GarageDetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as GarageDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
