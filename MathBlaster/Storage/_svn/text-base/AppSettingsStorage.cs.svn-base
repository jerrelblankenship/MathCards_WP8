using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;

namespace MathBlaster
{
    public class AppSettingsStorage
    {
        // Our isolated storage settings
        private IsolatedStorageSettings isolatedStore;

        // The isolated storage key names of our settings
        private const string NumberOfProblems = "NumberOfProblems";
        private const string ProblemLevel = "ProblemLevel";
        private const string OperationToPerform = "OperationToPerform";
        
        // The default value of our settings
        private const decimal NumberOfProblemsSettingDefault = 10;
        private const decimal ProblemLevelSettingDefault = 1;
        private const OperatorEnum OperationToPerformSettingDefault = OperatorEnum.addition;
        
        /// Constructor that gets the application settings.
        public AppSettingsStorage()
        {
            try
            {
                // Get the settings for this application.
                isolatedStore = IsolatedStorageSettings.ApplicationSettings;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while using IsolatedStorageSettings: " + e);
            }
        }

        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        public bool AddOrUpdateValue(string Key, object value)
        {
            bool valueChanged = false;
            try
            {
                // if new value is different, set the new value.
                if (!ReferenceEquals(isolatedStore[Key], value))
                {
                    isolatedStore[Key] = value;
                    valueChanged = true;
                }
            }
            catch (KeyNotFoundException)
            {
                isolatedStore.Add(Key, value);
                valueChanged = true;
            }
            catch (ArgumentException)
            {
                isolatedStore.Add(Key, value);
                valueChanged = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while using IsolatedStorageSettings: " + e);
            }
            return valueChanged;
        }


        /// Get the current value of the setting, or if it is not found, set the
        /// setting to the default setting.
        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;
            try
            {
                value = (T)isolatedStore[Key];
            }
            catch (KeyNotFoundException)
            {
                value = defaultValue;
            }
            catch (ArgumentException)
            {
                value = defaultValue;
            }
            return value;
        }

        /// Save the settings.
        public void Save()
        {
            isolatedStore.Save();
        }

        /// Property to get and set a NumberOfProblems Setting Key.
        public decimal NumberOfProblemsSetting
        {
            get { return GetValueOrDefault(NumberOfProblems, NumberOfProblemsSettingDefault); }
            set
            {
                var result = value;
                if (result == 0)
                {
                    result = NumberOfProblemsSettingDefault;
                }

                AddOrUpdateValue(NumberOfProblems, result);
                Save();
            }
        }

        public OperatorEnum OperationToPerformSetting
        {
            get { return (OperatorEnum) GetValueOrDefault(OperationToPerform, OperationToPerformSettingDefault); }
            set
            {
                AddOrUpdateValue(OperationToPerform, value);
                Save();
            }
        }

        public decimal ProblemLevelSetting
        {
            get { return GetValueOrDefault(ProblemLevel, ProblemLevelSettingDefault); }
            set 
            { 
                AddOrUpdateValue(ProblemLevel, value);
                Save();
            }
        }
    }
}
