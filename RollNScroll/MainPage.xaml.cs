using System.Windows;
using Microsoft.Phone.Controls;

namespace Fusao.RollNScroll
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ScrollViewModel scrollViewModel = new ScrollViewModel();

        // Constructor
        public MainPage()
        {
            DataContext = scrollViewModel;
            InitializeComponent();
        }

        private void btnRoll_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(scrollViewModel.GetNavigationUri());
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            lstpkForeground.SelectedItem = lstpkForeground.Items[0];
            lstpkBackground.SelectedItem = lstpkBackground.Items[5];
        }
    }
}