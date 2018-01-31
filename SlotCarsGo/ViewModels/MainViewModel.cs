using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Ioc;
using SlotCarsGo.Services;

namespace SlotCarsGo.ViewModels
{
    public class MainViewModel : NavigableViewModelBase
    {
        public MainViewModel()
        {
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
