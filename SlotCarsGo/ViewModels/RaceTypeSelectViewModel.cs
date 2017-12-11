using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;

using Microsoft.Toolkit.Uwp.UI.Controls;

using SlotCarsGo.Models;
using SlotCarsGo.Services;
using SlotCarsGo.Models.Racing;
using SlotCarsGo.Models.Manager;
using GalaSoft.MvvmLight.Ioc;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Views;
using SlotCarsGo.Views;

namespace SlotCarsGo.ViewModels
{
    public class RaceTypeSelectViewModel : NavigableViewModelBase
    {
        private RaceTypeBase _selected;

        public RaceTypeBase Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ObservableCollection<RaceTypeBase> RaceTypeItems { get; private set; } = new ObservableCollection<RaceTypeBase>();

        public RaceTypeSelectViewModel()
        {
        }

        public async Task LoadDataAsync(MasterDetailsViewState viewState)
        {
            RaceTypeItems.Clear();

            var data = await SelectRaceTypeService.GetRaceTypesDataAsync();

            foreach (var item in data)
            {
                RaceTypeItems.Add(item);
            }

            if (viewState == MasterDetailsViewState.Both)
            {
                Selected = RaceTypeItems.First();
            }
        }

        public void ProceedToDriverSetup(RaceTypeBase configuredRaceType)
        {
            SimpleIoc.Default.GetInstance<NavigationServiceEx>().Navigate(typeof(GridConfirmationViewModel).FullName, configuredRaceType);
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
