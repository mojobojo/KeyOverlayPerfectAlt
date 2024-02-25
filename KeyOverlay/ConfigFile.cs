using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Diagnostics;

namespace KeyOverlay {
    public class ConfigFile {
        [JsonPropertyName("keyAmount")]
        public int KeyAmount { get; set; }
        [JsonPropertyName("reloadKey")]
        public string ReloadKey { get; set; }

        [JsonPropertyName("key1")]
        public string Key1 { get; set; }

        [JsonPropertyName("key2")]
        public string Key2 { get; set; }

        [JsonPropertyName("key3")]
        public string Key3 { get; set; }

        [JsonPropertyName("key4")]
        public string Key4 { get; set; }

        //[JsonPropertyName("key5")]
        //public string Key5 { get; set; }

        //[JsonPropertyName("key6")]
        //public string Key6 { get; set; }

        //[JsonPropertyName("key7")]
        //public string Key7 { get; set; }

        //[JsonPropertyName("displayKey1")]
        //public string DisplayKey1 { get; set; }

        //[JsonPropertyName("displayKey2")]
        //public string DisplayKey2 { get; set; }

        //[JsonPropertyName("displayKey3")]
        //public string DisplayKey3 { get; set; }

        //[JsonPropertyName("displayKey4")]
        //public string DisplayKey4 { get; set; }

        //[JsonPropertyName("displayKey5")]
        //public string DisplayKey5 { get; set; }

        //[JsonPropertyName("displayKey6")]
        //public string DisplayKey6 { get; set; }

        //[JsonPropertyName("displayKey7")]
        //public string DisplayKey7 { get; set; }

        [JsonPropertyName("keyOrder")]
        public string keyOrder { get; set; }

        [JsonPropertyName("keyCounter")]
        public bool KeyCounter { get; set; }

        [JsonPropertyName("windowHeight")]
        public uint WindowHeight { get; set; }

        [JsonPropertyName("windowWidth")]
        public uint WindowWidth { get; set; }

        [JsonPropertyName("keySize")]
        public int KeySize { get; set; }

        [JsonPropertyName("barSpeed")]
        public float BarSpeed { get; set; }

        [JsonPropertyName("margin")]
        public int Margin { get; set; }

        [JsonPropertyName("outlineThickness")]
        public int OutlineThickness { get; set; }

        [JsonPropertyName("fading")]
        public bool Fading { get; set; }

        [JsonPropertyName("backgroundColor")]
        public string BackgroundColor { get; set; }

        [JsonPropertyName("keyColor")]
        public string KeyColor { get; set; }

        [JsonPropertyName("borderColor")]
        public string BorderColor { get; set; }

        [JsonPropertyName("barColor")]
        public string BarColor { get; set; }

        [JsonPropertyName("fontColor")]
        public string FontColor { get; set; }

        [JsonPropertyName("pressFontColor")]
        public string PressFontColor { get; set; }

        [JsonPropertyName("backgroundImage")]
        public string BackgroundImage { get; set; }

        [JsonPropertyName("uncappedFPS")]
        public bool UncappedFPS { get; set; }

        [JsonPropertyName("maxFPS")]
        public uint MaxFPS { get; set; }

        [JsonPropertyName("showErrors")]
        public bool ShowErrors { get; set; }

        [JsonPropertyName("showKeyLock")]
        public bool ShowKeyLock { get; set; }

        [JsonPropertyName("keyNameAliases")]
        public Dictionary<string, string> KeyNameAliases { get; set; }

        [JsonPropertyName("osuInstall")]
        public string OsuInstall { get; set; }

        [JsonPropertyName("lazerInstall")]
        public string LazerInstall { get; set; }

        [JsonPropertyName("useLazerKeyConfig")]
        public bool UseLazerKeyConfig { get; set; }

        [JsonPropertyName("useKeyboardHooks")]
        public bool UseKeyboardHooks { get; set; }
    }
    public class ConfigLoader {
        private string configPath;
        private string configPathLegacy;

        public ConfigLoader() {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            configPathLegacy = Path.Combine(assemblyPath ?? "", "config.txt");
            configPath = Path.Combine(assemblyPath ?? "", "config.json");
        }

        public ConfigLoader(string path) {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            configPathLegacy = Path.Combine(assemblyPath ?? "", "config.txt");
            configPath = path;
        }

        public ConfigFile Load() {
            ConfigFile config = null;
            try {
                if (File.Exists(configPathLegacy)) {
                    // TODO: Convert legacy config file into json
                }

                config = JsonSerializer.Deserialize<ConfigFile>(File.ReadAllText(configPath));
            } catch (Exception e) {
                Debug.WriteLine(e);
            }

            return config;
        }
    }
}
