using System;
using Microsoft.Extensions.Configuration;
using TestsForReview.Models;

namespace TestsForReview.Utils
{
    internal static class Config
    {
        public static readonly Uri HomePageUrl;
        public static readonly Uri AdminPageUrl;

        public static readonly int ElementLoadTimeout;
        public static readonly TimeSpan ElementLoadTimeoutTimeSpan;
        public static readonly string ScreenShotPath;
        public static readonly bool IsHeadless;
        public static readonly Uri WdHubUrl;
        public static readonly Account DefaultAccount;
        public static readonly Account AdminAccount;
        static Config()
        {
            var configsPath = FsUtils.FindPath("./Configs");
            var profile = ConfigFileSettingsProvider.GetProfile();
            var config = ConfigBuilderHelper.SetUpBuilder(configsPath, false, profile).Build();

            HomePageUrl = new Uri(config.GetValue<string>("Site:Url"));
            AdminPageUrl = Helper.UrlCombine(config.GetValue<string>("AdminSite:RelativeUrl"));

            var wdHubUrlStr = config.GetValue<string>("WdHubUrl", null);
            WdHubUrl = string.IsNullOrEmpty(wdHubUrlStr) ? null : new Uri(wdHubUrlStr, UriKind.Absolute);

            ElementLoadTimeout = config.GetValue<int>("ElementLoadTimeout");
            ElementLoadTimeoutTimeSpan = TimeSpan.FromSeconds(ElementLoadTimeout);

            ScreenShotPath = config.GetValue<string>("ScreenShotPath");

            IsHeadless = config.GetValue("Headless", false);
            DefaultAccount = new Account
            {
                UserName = config.GetValue<string>("Account:username"),
                Password = config.GetValue<string>("Account:password"),
                FirstName = config.GetValue<string>("Account:firstName"),
                LastName = config.GetValue<string>("Account:lastName")
            };
            AdminAccount = new Account
            {
                UserName = config.GetValue<string>("AdminSite:Account:username"),
                Password = config.GetValue<string>("AdminSite:Account:password"),
                IsAdmin = config.GetValue("AdminSite:Account:isAdmin", true)
            };
        }
    }
}
