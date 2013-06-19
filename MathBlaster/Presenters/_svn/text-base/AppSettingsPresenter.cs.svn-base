using System.Collections.Generic;
using MathBlaster.Screen;

namespace MathBlaster.Presenters
{
    public class AppSettingsPresenter
    {
        private IAppSettings appSettingsView;
        private static readonly List<string> nonNumberValues = new List<string> { "*", "#", ",", "(", ")", "x", "-", "+", " ", "@", "." };
        private AppSettingsStorage storageSettings;

        public AppSettingsPresenter(IAppSettings view, AppSettingsStorage storage)
        {
            appSettingsView = view;
            storageSettings = storage;
        }

        public string RemoveInvalidNumericValues(string displayText)
        {
            nonNumberValues.ForEach(nonNumber => displayText = displayText.Replace(nonNumber, ""));
            return displayText;
        }

        public void SaveSettingValues()
        {
            storageSettings.NumberOfProblemsSetting = appSettingsView.NumberOfProblems.ToDecimal();
            storageSettings.ProblemLevelSetting = GetProblemLevel();
        }

        private decimal GetProblemLevel()
        {
            var level = 1;

            if (appSettingsView.Level2Checked)
            {
                level = 2;
            }
            else if (appSettingsView.Level3Checked)
            {
                level = 3;
            }

            return level;
        }

        public void LoadSettingsPage()
        {
            appSettingsView.NumberOfProblems = storageSettings.NumberOfProblemsSetting.ToString();
            SetupProblemLevel();
        }

        private void SetupProblemLevel()
        {
            if (storageSettings.ProblemLevelSetting == 2)
            {
                appSettingsView.Level2Checked = true;
            }
            else if (storageSettings.ProblemLevelSetting == 3)
            {
                appSettingsView.Level3Checked = true;
            }
            else
            {
                appSettingsView.Level1Checked = true;
            }
        }
    }
}
