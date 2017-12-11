using System;

using SlotCarsGo.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SlotCarsGo.Models.Racing;
using SlotCarsGo.ViewModels;
using SlotCarsGo.Models.Manager;
using GalaSoft.MvvmLight.Ioc;
using SlotCarsGo.Services;

namespace SlotCarsGo.Views
{
    public sealed partial class RaceTypeSelectDetailControl : UserControl
    {
        public RaceTypeBase MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as RaceTypeBase; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrder), typeof(RaceTypeSelectDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public RaceTypeSelectDetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RaceTypeSelectDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }

        /// <summary>
        /// Toggles the selected Race Type's LapsNotDuration property to true.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectLaps_Checked(object sender, RoutedEventArgs e)
        {
            if (this.MasterMenuItem != null)
            {
                this.MasterMenuItem.LapsNotDuration = true;
            }
        }

        /// <summary>
        /// Toggles the selected Race Type's LapsNotDuration property to false.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectTime_Checked(object sender, RoutedEventArgs e)
        {
            if (this.MasterMenuItem != null)
            {
                this.MasterMenuItem.LapsNotDuration = false;
            }
        }

        /// <summary>
        /// Updates the race limit value text.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RaceLimitSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            this.RaceLimitValue.Text = e.NewValue.ToString();
            if (this.MasterMenuItem != null)
            {
                this.MasterMenuItem.RaceLimitValue = (int)this.RaceLimitSlider.Value;
            }
        }

        /// <summary>
        /// Selects the race type to create.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SimpleIoc.Default.GetInstance<RaceTypeSelectViewModel>().ProceedToDriverSetup(this.MasterMenuItem);
        }
    }
}
