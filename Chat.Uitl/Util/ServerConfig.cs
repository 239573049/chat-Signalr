using CZGL.SystemInfo.Linux;
using CZGL.SystemInfo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Chat.Uitl.Util
{
    public class ServerConfig
    {
        /// <summary>
        /// 读取内存信息
        /// </summary>
        /// <returns></returns>
        public static MemInfo ReadMemInfo()
        {
            MemInfo memInfo = new MemInfo();
            const string CPU_FILE_PATH = "/proc/meminfo";
            var mem_file_info = File.ReadAllText(CPU_FILE_PATH);
            var lines = mem_file_info.Split(new[] { '\n' });
            int count = 0;
            foreach (var item in lines) {
                if (item.StartsWith("MemTotal:")) {
                    count++;
                    var tt = item.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    var total = tt[1].Trim().Split(" ")[0];
                    memInfo.Total= string.IsNullOrEmpty(total)?0: int.Parse(total) / 1024;
                }
                else if (item.StartsWith("MemAvailable:")) {
                    count++;
                    var tt = item.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    var availble = tt[1].Trim().Split(" ")[0];
                    memInfo.Available = string.IsNullOrEmpty(availble) ? 0 : int.Parse(availble)/1024;
                }
                if (count >= 2) break;
            }
            memInfo.Usage = Convert.ToInt32((memInfo.Total - memInfo.Available) / memInfo.Total * 100);
            return memInfo;
        }
        /// <summary>
        /// 读取CPU使用率信息
        /// </summary>
        /// <returns></returns>
        public static int ReadCpuUsage()
        {
            float value = 0f;
            var process = new Process
            {
                StartInfo = new ProcessStartInfo("top", "-b -n1")
            };
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            var cpuInfo = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();

            var lines = cpuInfo.Split('\n');
            bool flags = false;
            foreach (var item in lines) {
                if (!flags) {
                    if (item.Contains("PID USER")) {
                        flags = true;
                    }
                }
                else {
                    var li = item.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < li.Length; i++) {
                        if (li[i] == "R" || li[i] == "S") {
                            value += float.Parse(li[i + 1]);
                            break;
                        }
                    }
                }
            }
            int r = (int)(value / 4f);
            if (r > 100) r = 100;
            return r;
        }

        /// <summary>
        /// 获取服务器运行时信息
        /// </summary>
        /// <returns></returns>
        public static ServerInfo GetServerInfo()
        {
            var serverInfo = new ServerInfo
            {
                MemInfo = ReadMemInfo(),
            };
            serverInfo.PacketCount = 0;
            serverInfo.SessionCount = 0;
            return serverInfo;
        }
    }
    public class MemInfo
    {
        /// <summary>
        /// 总计内存大小
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// 可用内存大小
        /// </summary>
        public decimal Available { get; set; }
        public int Usage { get; set; }
    }

    public class ServerInfo
    {
        /// <summary>
        /// 内存
        /// </summary>
        public MemInfo MemInfo { get; set; }

        /// <summary>
        /// 接包数据
        /// </summary>
        public long PacketCount { get; set; }

        /// <summary>
        /// 当前会话连接数
        /// </summary>
        public int SessionCount { get; set; }
    }
    public class ConfigFile
    {

        /// <summary>
        /// Gets the groups found in the configuration file.
        /// </summary>
        public Dictionary<string, SettingsGroup> SettingGroups { get; private set; }
        /// <summary>
        /// Loads a configuration file.
        /// </summary>
        /// <param name="file">The filename where the configuration file can be found.</param>
        public ConfigFile(string file)
        {
            Load(file);
        }
        /// <summary>
        /// Loads the configuration from a file.
        /// </summary>
        /// <param name="file">The file from which to load the configuration.</param>
        public void Load(string file)
        {
            using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read)) {
                Load(stream);
            }
        }

        /// <summary>
        /// Loads the configuration from a stream.
        /// </summary>
        /// <param name="stream">The stream from which to read the configuration.</param>
        public void Load(Stream stream)
        {
            //track line numbers for exceptions
            int lineNumber = 0;

            //groups found
            List<SettingsGroup> groups = new List<SettingsGroup>();

            //current group information
            string currentGroupName = null;
            List<Setting> settings = null;

            using (StreamReader reader = new StreamReader(stream)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    lineNumber++;

                    //strip out comments
                    if (line.Contains("#")) {
                        if (line.IndexOf("#") == 0)
                            continue;

                        line = line.Substring(0, line.IndexOf("#"));
                    }

                    //trim off any extra whitespace
                    line = line.Trim();

                    //try to match a group name
                    Match match = Regex.Match(line, "\\[[a-zA-Z\\d\\s]+\\]");

                    //found group name
                    if (match.Success) {
                        //if we have a current group we're on, we save it
                        if (settings != null && currentGroupName != null)
                            groups.Add(new SettingsGroup(currentGroupName, settings));

                        //make sure the name exists
                        if (match.Value.Length == 2)
                            throw new Exception(string.Format("Group must have name (line {0})", lineNumber));

                        //set our current group information
                        currentGroupName = match.Value.Substring(1, match.Length - 2);
                        settings = new List<Setting>();
                    }

                    //no group name, check for setting with equals sign
                    else if (line.Contains("=")) {
                        //split the line
                        string[] parts = line.Split(new[] { '=' }, 2);

                        //if we have any more than 2 parts, we have a problem
                        if (parts.Length != 2)
                            throw new Exception(string.Format("Settings must be in the format 'name = value' (line {0})", lineNumber));

                        //trim off whitespace
                        parts[0] = parts[0].Trim();
                        parts[1] = parts[1].Trim();

                        //figure out if we have an array or not
                        bool isArray = false;
                        bool inString = false;

                        //go through the characters
                        foreach (char c in parts[1]) {
                            //any comma not in a string makes us creating an array
                            if (c == ',' && !inString)
                                isArray = true;

                            //flip the inString value each time we hit a quote
                            else if (c == '"')
                                inString = !inString;
                        }

                        //if we have an array, we have to trim off whitespace for each item and
                        //do some checking for boolean values.
                        if (isArray) {
                            //split our value array
                            string[] pieces = parts[1].Split(',');

                            //need to build a new string
                            StringBuilder builder = new StringBuilder();

                            for (int i = 0; i < pieces.Length; i++) {
                                //trim off whitespace
                                string s = pieces[i].Trim();

                                //convert to lower case
                                string t = s.ToLower();

                                //check for any of the true values
                                if (t == "on" || t == "yes" || t == "true")
                                    s = "true";

                                //check for any of the false values
                                else if (t == "off" || t == "no" || t == "false")
                                    s = "false";

                                //append the value
                                builder.Append(s);

                                //if we are not on the last value, add a comma
                                if (i < pieces.Length - 1)
                                    builder.Append(",");
                            }

                            //save the built string as the value
                            parts[1] = builder.ToString();
                        }

                        //if not an array
                        else {
                            //make sure we are not working with a string value
                            if (!parts[1].StartsWith("\"")) {
                                //convert to lower
                                string t = parts[1].ToLower();

                                //check for any of the true values
                                if (t == "on" || t == "yes" || t == "true")
                                    parts[1] = "true";

                                //check for any of the false values
                                else if (t == "off" || t == "no" || t == "false")
                                    parts[1] = "false";
                            }
                        }

                        //add the setting to our list making sure, once again, we have stripped
                        //off the whitespace
                        settings.Add(new Setting(parts[0].Trim(), parts[1].Trim(), isArray));
                    }
                }
            }

            //make sure we save off the last group
            if (settings != null && currentGroupName != null)
                groups.Add(new SettingsGroup(currentGroupName, settings));

            //create our new group dictionary
            SettingGroups = new Dictionary<string, SettingsGroup>();

            //add each group to the dictionary
            foreach (SettingsGroup group in groups) {
                SettingGroups.Add(group.Name, group);
            }
        }


        /// <summary>
        /// Saves the configuration to a stream.
        /// </summary>
        /// <param name="stream">The stream to which the configuration will be saved.</param>
        public void Save(Stream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream)) {
                foreach (KeyValuePair<string, SettingsGroup> groupValue in SettingGroups) {
                    writer.WriteLine("[{0}]", groupValue.Key);
                    foreach (KeyValuePair<string, Setting> settingValue in groupValue.Value.Settings) {
                        writer.WriteLine("{0}={1}", settingValue.Key, settingValue.Value.RawValue);
                    }
                    writer.WriteLine();
                }
            }
        }
    }
    public class Setting
    {
        /// <summary>
        /// Gets the name of the setting.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the raw value of the setting.
        /// </summary>
        public string RawValue { get; private set; }

        /// <summary>
        /// Gets whether or not the setting is an array.
        /// </summary>
        public bool IsArray { get; private set; }

        internal Setting(string name)
        {
            Name = name;
            RawValue = string.Empty;
            IsArray = false;
        }

        internal Setting(string name, string value, bool isArray)
        {
            Name = name;
            RawValue = value;
            IsArray = isArray;
        }

        /// <summary>
        /// Attempts to return the setting's value as an integer.
        /// </summary>
        /// <returns>An integer representation of the value</returns>
        public int GetValueAsInt()
        {
            return int.Parse(RawValue, CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// Attempts to return the setting's value as a float.
        /// </summary>
        /// <returns>A float representation of the value</returns>
        public float GetValueAsFloat()
        {
            return float.Parse(RawValue, CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// Attempts to return the setting's value as a bool.
        /// </summary>
        /// <returns>A bool representation of the value</returns>
        public bool GetValueAsBool()
        {
            return bool.Parse(RawValue);
        }

        /// <summary>
        /// Attempts to return the setting's value as a string.
        /// </summary>
        /// <returns>A string representation of the value</returns>
        public string GetValueAsString()
        {
            ;

            return RawValue;
        }

        /// <summary>
        /// Attempts to return the setting's value as an array of integers.
        /// </summary>
        /// <returns>An integer array representation of the value</returns>
        public int[] GetValueAsIntArray()
        {
            string[] parts = RawValue.Split(',');

            int[] valueParts = new int[parts.Length];

            for (int i = 0; i < parts.Length; i++)
                valueParts[i] = int.Parse(parts[i], CultureInfo.InvariantCulture.NumberFormat);

            return valueParts;
        }

        /// <summary>
        /// 尝试以浮点数数组的形式返回设置值。  
        /// </summary>
        /// <returns>An float array representation of the value</returns>
        public float[] GetValueAsFloatArray()
        {
            string[] parts = RawValue.Split(',');

            float[] valueParts = new float[parts.Length];

            for (int i = 0; i < parts.Length; i++)
                valueParts[i] = float.Parse(parts[i], CultureInfo.InvariantCulture.NumberFormat);

            return valueParts;
        }

        /// <summary>
        /// Attempts to return the setting's value as an array of bools.
        /// </summary>
        /// <returns>An bool array representation of the value</returns>
        public bool[] GetValueAsBoolArray()
        {
            string[] parts = RawValue.Split(',');

            bool[] valueParts = new bool[parts.Length];

            for (int i = 0; i < parts.Length; i++)
                valueParts[i] = bool.Parse(parts[i]);

            return valueParts;
        }

        /// <summary>
        /// Attempts to return the setting's value as an array of strings.
        /// </summary>
        /// <returns>An string array representation of the value</returns>
        public string[] GetValueAsStringArray()
        {
            Match match = Regex.Match(RawValue, "[\\\"][^\\\"]*[\\\"][,]*");

            List<string> values = new List<string>();

            while (match.Success) {
                string value = match.Value;
                if (value.EndsWith(","))
                    value = value.Substring(0, value.Length - 1);

                value = value.Substring(1, value.Length - 2);
                values.Add(value);
                match = match.NextMatch();
            }

            return values.ToArray();
        }

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="value">The new value to store.</param>
        public void SetValue(int value)
        {
            RawValue = value.ToString(CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="value">The new value to store.</param>
        public void SetValue(float value)
        {
            RawValue = value.ToString(CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="value">The new value to store.</param>
        public void SetValue(bool value)
        {
            RawValue = value.ToString();
        }

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="value">The new value to store.</param>
        public void SetValue(string value)
        {
            RawValue = assertStringQuotes(value);
        }

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="value">The new values to store.</param>
        public void SetValue(params int[] values)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < values.Length; i++) {
                builder.Append(values[i].ToString(CultureInfo.InvariantCulture.NumberFormat));
                if (i < values.Length - 1)
                    builder.Append(",");
            }

            RawValue = builder.ToString();
        }

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="value">The new values to store.</param>
        public void SetValue(params float[] values)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < values.Length; i++) {
                builder.Append(values[i].ToString(CultureInfo.InvariantCulture.NumberFormat));
                if (i < values.Length - 1)
                    builder.Append(",");
            }

            RawValue = builder.ToString();
        }

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="value">The new values to store.</param>
        public void SetValue(params bool[] values)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < values.Length; i++) {
                builder.Append(values[i]);
                if (i < values.Length - 1)
                    builder.Append(",");
            }

            RawValue = builder.ToString();
        }

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="value">The new values to store.</param>
        public void SetValue(params string[] values)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < values.Length; i++) {
                builder.Append(assertStringQuotes(values[i]));
                if (i < values.Length - 1)
                    builder.Append(",");
            }

            RawValue = builder.ToString();
        }

        private static string assertStringQuotes(string value)
        {
            return value;
        }

        public string GetValue()
        {
            return RawValue;
        }
    }
    public class SettingsGroup
    {
        /// <summary>
        /// Gets the name of the group.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the settings found in the group.
        /// </summary>
        public Dictionary<string, Setting> Settings { get; private set; }

        internal SettingsGroup(string name)
        {
            Name = name;
            Settings = new Dictionary<string, Setting>();
        }

        internal SettingsGroup(string name, List<Setting> settings)
        {
            Name = name;
            Settings = new Dictionary<string, Setting>();

            foreach (Setting setting in settings)
                Settings.Add(setting.Name, setting);
        }

        /// <summary>
        /// Adds a setting to the group.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The value of the setting.</param>
        public void AddSetting(string name, int value)
        {
            Setting setting = new Setting(name);
            setting.SetValue(value);
            Settings.Add(name, setting);
        }

        /// <summary>
        /// Adds a setting to the group.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The value of the setting.</param>
        public void AddSetting(string name, float value)
        {
            Setting setting = new Setting(name);
            setting.SetValue(value);
            Settings.Add(name, setting);
        }

        /// <summary>
        /// Adds a setting to the group.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The value of the setting.</param>
        public void AddSetting(string name, bool value)
        {
            Setting setting = new Setting(name);
            setting.SetValue(value);
            Settings.Add(name, setting);
        }

        /// <summary>
        /// Adds a setting to the group.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The value of the setting.</param>
        public void AddSetting(string name, string value)
        {
            Setting setting = new Setting(name);
            setting.SetValue(value);
            Settings.Add(name, setting);
        }

        /// <summary>
        /// Adds a setting to the group.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The values of the setting.</param>
        public void AddSetting(string name, params int[] values)
        {
            Setting setting = new Setting(name);
            setting.SetValue(values);
            Settings.Add(name, setting);
        }

        /// <summary>
        /// Adds a setting to the group.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The values of the setting.</param>
        public void AddSetting(string name, params float[] values)
        {
            Setting setting = new Setting(name);
            setting.SetValue(values);
            Settings.Add(name, setting);
        }

        /// <summary>
        /// Adds a setting to the group.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The values of the setting.</param>
        public void AddSetting(string name, params bool[] values)
        {
            Setting setting = new Setting(name);
            setting.SetValue(values);
            Settings.Add(name, setting);
        }

        /// <summary>
        /// Adds a setting to the group.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The values of the setting.</param>
        public void AddSetting(string name, params string[] values)
        {
            Setting setting = new Setting(name);
            setting.SetValue(values);
            Settings.Add(name, setting);
        }

        /// <summary>
        /// Deletes a setting from the group.
        /// </summary>
        /// <param name="name">The name of the setting to delete.</param>
        public void DeleteSetting(string name)
        {
            Settings.Remove(name);
        }
    }
}
