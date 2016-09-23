using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using TestRxUniversal.DataService;
using Windows.UI.Xaml.Controls;

namespace TestRxUniversal
{
    public sealed partial class RxPage : Page
    {
        public ViewModels.ViewModelReactive2 ViewModel { get; set; }

        public RxPage()
        {
            this.ViewModel = new ViewModels.ViewModelReactive2();
            this.InitializeComponent();

            //When testing ViewModelReactive instead of ViewModelReactive2, uncomment this:
            //DoReactive();
        }

        private void DoReactive()
        {
            var textChangedSequence =
                Observable.FromEventPattern<TextChangedEventArgs>(SearchBox, 
                nameof(SearchBox.TextChanged));

            var throttledTextChangedSequence =
                textChangedSequence.Select(x => ((TextBox)x.Sender).Text)
                      .DistinctUntilChanged()
                      .Throttle(TimeSpan.FromSeconds(.5));

            var result = (from qry in throttledTextChangedSequence
                          from images in SearchDataService.Search(qry)
                              .TakeUntil(throttledTextChangedSequence)
                          select images);

            result.ObserveOn(SynchronizationContext.Current).Subscribe(
                (e) => ViewModel.Results = e?.Select(a => a.thumb_url).ToList());
        }
    }
}
