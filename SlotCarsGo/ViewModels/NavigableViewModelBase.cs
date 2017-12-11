using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using SlotCarsGo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace SlotCarsGo.ViewModels
{
    public abstract class NavigableViewModelBase : ViewModelBase, INavigable
    {
        protected NavigationServiceEx NavigationService;

        public NavigableViewModelBase()
        {
            this.NavigationService = SimpleIoc.Default.GetInstance<NavigationServiceEx>();
        }

        public abstract Task OnNavigatedToAsync(object parameter, NavigationMode mode);
        public abstract void OnNavigatingFrom(NavigatingCancelEventArgs e);
        public abstract void OnNavigatedFrom();
    }
}
