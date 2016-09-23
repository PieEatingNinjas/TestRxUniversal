using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using TestRxUniversal.DataService;

namespace TestRxUniversal.ViewModels
{
    public class ViewModelReactive2 : INotifyPropertyChanged
    {
        private string _EnteredSearchQuery;

        public string EnteredSearchQuery
        {
            get { return _EnteredSearchQuery; }
            set
            {
                _EnteredSearchQuery = value;
                RaisePropertyChanged(nameof(EnteredSearchQuery));
            }
        }

        private List<string> _Results = new List<string>();

        public List<string> Results
        {
            get { return _Results; }
            set { _Results = value; RaisePropertyChanged(nameof(Results)); }
        }

        public ViewModelReactive2()
        {
            #region scenario 1

            //var textChangedSequence = Observable.FromEventPattern<PropertyChangedEventArgs>(this, 
            //    nameof(PropertyChanged));

            //var throttledTextChangedSequence = textChangedSequence
            //    .Where(a => a.EventArgs.PropertyName == nameof(EnteredSearchQuery))
            //    .Select(a => EnteredSearchQuery)
            //    .DistinctUntilChanged()
            //    .Throttle(TimeSpan.FromSeconds(.5));

            #endregion

            #region scenario 2

            var throttledTextChangedSequence = this.GetPropertyAsObservable(() => EnteredSearchQuery)
                      .DistinctUntilChanged()
                      .Throttle(TimeSpan.FromSeconds(.5));

            #endregion

            var result = from qry in throttledTextChangedSequence
                         from images in SearchDataService.Search(qry).TakeUntil(throttledTextChangedSequence)
                         select images?.Select(a => a.thumb_url);

            throttledTextChangedSequence.ObserveOn(
                SynchronizationContext.Current).Subscribe(e => UpdateList(null));

            result.ObserveOn(
                SynchronizationContext.Current).Subscribe(UpdateList);

        }

        private void UpdateList(IEnumerable<string> items)
        {
            Results = items?.ToList();
        }

        public void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public static class ReactiveExtensionMethods
    {
        public static IObservable<EventPattern<PropertyChangedEventArgs>> GetPropertyAsObservable(this INotifyPropertyChanged @class, string property)
        {
            return Observable.FromEventPattern<PropertyChangedEventArgs>(@class, nameof(@class.PropertyChanged)).Where(a => a.EventArgs.PropertyName == property);
        }

        public static IObservable<T> GetPropertyAsObservable<T>(this INotifyPropertyChanged @class, 
            Expression<Func<T>> property)
        {
            var propName = (property.Body as MemberExpression).Member.Name;

            return Observable.FromEventPattern<PropertyChangedEventArgs>(@class,
                nameof(@class.PropertyChanged))
                .Where(a => a.EventArgs.PropertyName == propName)
                .Select(a => property.Compile().Invoke());
        }
    }
}
