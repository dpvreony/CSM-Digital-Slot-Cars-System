namespace SlotCarsGo.ViewModels
{
    using System.Threading.Tasks;
    using Windows.UI.Xaml.Navigation;

    /* Enables override of page activation methods, /* Extracted from:  https://marcominerva.wordpress.com/2017/10/17/how-to-handle-navigation-events-in-mvvm-using-windows-template-studio/ */
    interface INavigable
    {
        Task OnNavigatedToAsync(object parameter, NavigationMode mode);

        void OnNavigatingFrom(NavigatingCancelEventArgs e);

        void OnNavigatedFrom();
    }
}
