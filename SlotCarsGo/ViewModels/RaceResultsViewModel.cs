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
    public class RaceResultsViewModel : NavigableViewModelBase
    {
        private RaceSession session;
        private SampleOrder _selected;

        public SampleOrder Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();

        public RaceResultsViewModel()
        {
        }

        public async Task LoadDataAsync(MasterDetailsViewState viewState)
        {
            SampleItems.Clear();

            var data = await SampleDataService.GetSampleModelDataAsync();

            foreach (var item in data)
            {
                SampleItems.Add(item);
            }

            if (viewState == MasterDetailsViewState.Both)
            {
                Selected = SampleItems.First();
            }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode)
        {
            this.session = parameter as RaceSession;

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
