using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Microsoft.Toolkit.Uwp.UI.Controls;

using SlotCarsGo.Models;
using SlotCarsGo.Services;
using Windows.UI.Xaml.Navigation;
using SlotCarsGo.Models.Racing;
using GalaSoft.MvvmLight.Ioc;

namespace SlotCarsGo.ViewModels
{
    public class RaceResultsViewModel : NavigableViewModelBase
    {
        private RaceSession session;
        private DriverResult _selected;

        public DriverResult Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ObservableCollection<DriverResult> Results { get; private set; } 

        public RaceResultsViewModel()
        {
        }

        public async Task LoadDataAsync(MasterDetailsViewState viewState)
        {
            if (this.session != null)
            {
                var Results = this.session.DriverResults.Values.ToList().OrderByDescending(d => d.Position);

                if (viewState == MasterDetailsViewState.Both)
                {
                    Selected = Results.First();
                }
            }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode)
        {
            if (parameter == null)
            {
                SimpleIoc.Default.GetInstance<NavigationServiceEx>().Navigate(typeof(MainViewModel).FullName);
            }
            else
            {
                this.session = parameter as RaceSession;
            }

            return Task.CompletedTask;
        }

        public override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }

        public override void OnNavigatedFrom()
        {
        }
    }
}
