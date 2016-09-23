using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestRxUniversal.DataService;

namespace TestRxUniversal.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        CancellationTokenSource cancellationTokenSource;

        private string _EnteredSearchQuery;

        public string EnteredSearchQuery
        {
            get { return _EnteredSearchQuery; }
            set
            {
                _EnteredSearchQuery = value;
                TryDoSearch();
            }
        }

        private List<string> _Results = new List<string>();

        public List<string> Results
        {
            get { return _Results; }
            set { _Results = value; RaisePropertyChanged(nameof(Results)); }
        }

        private void TryDoSearch()
        {
            Results = null;

            if (cancellationTokenSource != null)
                cancellationTokenSource.Cancel();

            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                DoSearch(cancellationTokenSource.Token);
            }
            catch(TaskCanceledException)
            {
            }
        }

        private async void DoSearch(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                
                await Task.Delay(500, token);

                var result = await SearchDataService.SearchAsync(EnteredSearchQuery, token);

                token.ThrowIfCancellationRequested();

                Results = result?.Select(a => a.thumb_url).ToList();

            }
            catch (TaskCanceledException)
            {
            }

        }

        public void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
