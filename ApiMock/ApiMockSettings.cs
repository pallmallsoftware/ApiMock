using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMock
{
    public class ApiMockSettings
    {
        public const string SectionName = "Settings";

        public string CacheFileName { get; set; }
    }

    public static class ApiMockSettingsExt
    {
        public static ApiMockSettings GetApiMockSettings(this IConfiguration conf)
        {
            var settings = new ApiMockSettings();

            conf.GetSection(ApiMockSettings.SectionName).Bind(settings);

            return settings;
        }
    }

}
