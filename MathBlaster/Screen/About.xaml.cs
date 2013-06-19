using System.Reflection;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace MathBlaster.Screen
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
            var assembly = Assembly.GetExecutingAssembly().FullName;
            VersionNumber.Text = assembly.Split('=')[1].Split(',')[0];
        }

        private void EmailSupport_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var ect = new EmailComposeTask
            {
                To = "support@jerrelblankenship.com",
                Subject = "Math Cards App Feedback/Support"
            };

            ect.Show();
        }

        private void TangoLink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var task = new WebBrowserTask
            {
                URL = "http://tango.freedesktop.org/Tango_Desktop_Project"
            };

            task.Show();
        }

        private void IconLink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var task = new WebBrowserTask
            {
                URL = "http://twitter.com/dshultz"
            };

            task.Show();
        }
    }
}