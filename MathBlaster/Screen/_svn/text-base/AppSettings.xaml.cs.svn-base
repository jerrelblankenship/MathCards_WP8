using System;
using System.Windows.Input;
using MathBlaster.Presenters;
using Microsoft.Phone.Controls;

namespace MathBlaster.Screen
{
    public interface IAppSettings
    {
        string NumberOfProblems { get; set; }
        bool Level1Checked { get; set; } 
        bool Level2Checked { get; set; }
        bool Level3Checked { get; set; } 
    }

    public partial class AppSettings : PhoneApplicationPage, IAppSettings
    {
        private AppSettingsPresenter presenter;

        #region View_Properties

        public string NumberOfProblems
        {
            get { return numberOfProblems.Text; }
            set { numberOfProblems.Text = value; }
        }

        public bool Level1Checked
        {
            get { return difLevel1.IsChecked != null && difLevel1.IsChecked.Value; }
            set { difLevel1.IsChecked = value; }
        }

        public bool Level2Checked
        {
            get { return difLevel2.IsChecked != null && difLevel2.IsChecked.Value; }
            set { difLevel2.IsChecked = value; }
        }

        public bool Level3Checked
        {
            get { return difLevel3.IsChecked != null && difLevel3.IsChecked.Value; }
            set { difLevel3.IsChecked = value; }
        }

        #endregion

        public AppSettings()
        {
            InitializeComponent();
            presenter = new AppSettingsPresenter(this, new AppSettingsStorage());
            presenter.LoadSettingsPage();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            presenter.SaveSettingValues();
            NavigationService.GoBack();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void numberOfProblems_KeyUp(object sender, KeyEventArgs e)
        {
            numberOfProblems.Text = presenter.RemoveInvalidNumericValues(numberOfProblems.Text);
            numberOfProblems.SelectionStart = numberOfProblems.Text.Length;
        }

        
    }
}