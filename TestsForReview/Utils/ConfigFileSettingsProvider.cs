using System;
using System.Configuration;
using System.IO;

namespace TestsForReview.Utils
{
    public class ConfigFileSettingsProvider
    {
        public static string GetProfile()
        {
            var profile = ConfigurationManager.AppSettings.Get("profile")?.Trim();

            if (string.IsNullOrEmpty(profile))
            {
                var path = FsUtils.GetPath("./Configs/profile");
                profile = File.ReadAllText(path).Trim();
            }

            if (string.IsNullOrEmpty(profile))
                throw new Exception("Can't get profile, create file 'Configs/profile' or add app setting  'profile = release'");
            return profile;
        }
    }
}
