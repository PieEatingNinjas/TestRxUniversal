using Windows.UI.Xaml.Controls;

namespace TestRxUniversal
{
    public sealed partial class MainPage : Page
    {
        public ViewModels.MainPageViewModel ViewModel { get; set; }
        public MainPage()
        {
            this.ViewModel = new ViewModels.MainPageViewModel();

            this.InitializeComponent();
        }
    }
}
