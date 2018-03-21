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

namespace SlotCarsGo.ViewModels
{
    public class GarageViewModel : NavigableViewModelBase
    {
        private Car _selected;

        public Car Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ObservableCollection<Car> CarsInGarage { get; private set; } = new ObservableCollection<Car>();

        public GarageViewModel()
        {
        }

        public async Task LoadDataAsync(MasterDetailsViewState viewState)
        {
            CarsInGarage.Clear();

            var data = await CarsInGarageDataService.GetCarsInGarageAsync();

            foreach (var car in data)
            {
                CarsInGarage.Add(car);
            }

            if (viewState == MasterDetailsViewState.Both && Selected != null)
            {
                Selected = CarsInGarage.First();
            }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode)
        {
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
