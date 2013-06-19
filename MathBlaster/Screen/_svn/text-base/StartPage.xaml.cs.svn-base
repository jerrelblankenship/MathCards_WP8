using System;
using System.Windows;
using System.Windows.Controls;
using MathBlaster.Presenters;
using Microsoft.Devices;
using Microsoft.Phone.Controls;

namespace MathBlaster.Screen
{
    public interface IStartPage
    {
        
    }

    public partial class StartPage : PhoneApplicationPage, IStartPage
    {
        private StartPagePresenter presenter;

        public StartPage()
        {
            InitializeComponent();

            presenter = new StartPagePresenter(this, new AppSettingsStorage());
        }

        private void quiz_Click(object sender, RoutedEventArgs e)
        {
            var clicked = sender as Button;
            presenter.SaveOperationToPerform(clicked.Name);

            NavigationService.Navigate(new Uri("/Screen/ProblemPage.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Screen/AppSettings.xaml", UriKind.Relative));
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Screen/About.xaml", UriKind.Relative));
        }
    }
}