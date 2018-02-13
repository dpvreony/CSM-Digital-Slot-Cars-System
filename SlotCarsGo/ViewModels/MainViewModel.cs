using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Ioc;
using SlotCarsGo.Services;
using SlotCarsGo.Models.Comms;

namespace SlotCarsGo.ViewModels
{
    public class MainViewModel : NavigableViewModelBase
    {

        private bool isPowerbaseConnected;

        public MainViewModel()
        {
//            this.isPowerbaseConnected = SimpleIoc.Default.GetInstance<Powerbase>().IsPowerbaseConnected;
        }

        public override void OnNavigatedFrom()
        {
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode)
        {
            return Task.CompletedTask;
        }

        public override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }

        public void RaceTypeSelectClicked()
        {
            SimpleIoc.Default.GetInstance<NavigationServiceEx>().Navigate(typeof(RaceTypeSelectViewModel).FullName);
        }
    }
}
