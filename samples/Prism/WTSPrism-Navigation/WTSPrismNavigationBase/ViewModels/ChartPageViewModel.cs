using System.Collections.ObjectModel;
using Prism.Windows.Mvvm;
using WTSPrismNavigationBase.Services;
using WTSPrismNavigationBase.Models;

namespace WTSPrismNavigationBase.ViewModels
{
    public class ChartPageViewModel : ViewModelBase
    {
        private readonly ISampleDataService sampleDataService;

        public ChartPageViewModel(ISampleDataService sampleDataService)
        {
            this.sampleDataService = sampleDataService;
        }

        public ObservableCollection<DataPoint> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return sampleDataService.GetChartSampleData();
            }
        }
    }
}
