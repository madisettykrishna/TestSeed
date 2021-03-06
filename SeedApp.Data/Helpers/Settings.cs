﻿// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace SeedApp.Data.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        private const string UserNameKey = "UserName_Key";
        private static readonly string UserNameDefault = string.Empty;

        private const string PasswordKey = "Password_Key";
        private static readonly string PasswordDefault = string.Empty;

        #endregion

        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(SettingsKey, value);
            }
        }

        public static string UserName
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(UserNameKey, UserNameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(UserNameKey, value);
            }
        }

        public static string Passowrd
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(PasswordKey, PasswordDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(PasswordKey, value);
            }
        }
    }
}