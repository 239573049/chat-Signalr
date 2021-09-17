using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Uitl.Util
{
    public class AppSettingsUtil
    {

        private static IConfiguration Configuration
        {
            get;
            set;
        }

        private static string ContentPath
        {
            get;
            set;
        }

        public AppSettingsUtil(IConfiguration configuration, string path = "appsettings.json")
        {
            Configuration = configuration;
        }
        public static void SetValue(string value,params string[] sections)
        {

            try {
                if (sections.Any()) {
                    Configuration[string.Join(":", sections)]= value;
                }
            }
            catch (Exception) { }
        }
        public static string App(params string[] sections)
        {
            try {
                if (sections.Any()) {
                    return Configuration[string.Join(":", sections)];
                }
            }
            catch (Exception) {}

            return "";
        }

        public static object GetValue<T>(string sections)
        {
            var data = Configuration.GetValue<string>(sections);
            try {
                return Convert.ToInt32(data);
            }
            catch (FormatException) {
            }
            return data;
        }
    }
}
