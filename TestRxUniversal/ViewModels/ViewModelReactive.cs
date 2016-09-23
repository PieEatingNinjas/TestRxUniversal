using System.Collections.Generic;
using System.ComponentModel;

namespace TestRxUniversal.ViewModels
{
    public class ViewModelReactive : INotifyPropertyChanged
    {
        public string EnteredSearchQuery
        {
            get;
            set;
        }

        private List<string> _Results = new List<string>();

        public List<string> Results
        {
            get { return _Results; }
            set { _Results = value; RaisePropertyChanged(nameof(Results)); }
        }

        public void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
