using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Text.Json;
using System.Text.Json.Serialization;
using static SFML.Window.Keyboard;
using System.Security.Cryptography;
using System.Diagnostics;

namespace KeyOverlay
{
    public class ConfigFile {
        [JsonPropertyName("keyAmount")]
        public int KeyAmount { get; set; }

        [JsonPropertyName("key1")]
        public string Key1 { get; set; }

        [JsonPropertyName("key2")]
        public string Key2 { get; set; }

        [JsonPropertyName("key3")]
        public string Key3 { get; set; }

        [JsonPropertyName("key4")]
        public string Key4 { get; set; }

        [JsonPropertyName("key5")]
        public string Key5 { get; set; }

        [JsonPropertyName("key6")]
        public string Key6 { get; set; }

        [JsonPropertyName("key7")]
        public string Key7 { get; set; }

        [JsonPropertyName("displayKey1")]
        public string DisplayKey1 { get; set; }

        [JsonPropertyName("displayKey2")]
        public string DisplayKey2 { get; set; }

        [JsonPropertyName("displayKey3")]
        public string DisplayKey3 { get; set; }

        [JsonPropertyName("displayKey4")]
        public string DisplayKey4 { get; set; }

        [JsonPropertyName("displayKey5")]
        public string DisplayKey5 { get; set; }

        [JsonPropertyName("displayKey6")]
        public string DisplayKey6 { get; set; }

        [JsonPropertyName("displayKey7")]
        public string DisplayKey7 { get; set; }

        [JsonPropertyName("keyOrder")]
        public string keyOrder { get; set; }

        [JsonPropertyName("keyCounter")]
        public bool KeyCounter { get; set; }

        [JsonPropertyName("windowHeight")]
        public int WindowHeight { get; set; }

        [JsonPropertyName("windowWidth")]
        public int WindowWidth { get; set; }

        [JsonPropertyName("keySize")]
        public int KeySize { get; set; }

        [JsonPropertyName("barSpeed")]
        public int BarSpeed { get; set; }

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

        [JsonPropertyName("maxFPS")]
        public int MaxFPS { get; set; }

        [JsonPropertyName("showErrors")]
        public bool ShowErrors { get; set; }

        [JsonPropertyName("showKeyLock")]
        public bool ShowKeyLock { get; set; }
    }

    public class AppWindow
    {
        private readonly RenderWindow _window;
        private readonly List<Key> _keyList = new();
        private readonly List<RectangleShape> _squareList;
        private readonly float _barSpeed;
        private readonly float _ratioX;
        private readonly float _ratioY;
        private readonly int _outlineThickness;
        private readonly Color _backgroundColor;
        private readonly Color _keyBackgroundColor;
        private readonly Color _barColor;
        private readonly Color _fontColor;
        private readonly Color _pressFontColor;
        private readonly Sprite _background;
        private readonly bool _fading;
        private readonly bool _counter;
        private readonly List<Drawable> _staticDrawables = new();
        private readonly List<Text> _keyText = new();
        private readonly uint _maxFPS;
        private Clock _clock = new();
        private readonly bool _showErrors;
        private readonly int _keyAmount;
        private readonly int[] _keyOrder;
        private readonly bool _showKeyLock;

        public AppWindow(string configFileName)
        {
            var config = ReadConfig(configFileName);
            var windowWidth = config["windowWidth"];
            var windowHeight = config["windowHeight"];
            _window = new RenderWindow(new VideoMode(uint.Parse(windowWidth!), uint.Parse(windowHeight!)),
                "KeyOverlay", Styles.Default);

            //calculate screen ratio relative to original program size for easy resizing
            _ratioX = float.Parse(windowWidth) / 480f;
            _ratioY = float.Parse(windowHeight) / 960f;

            _barSpeed = float.Parse(config["barSpeed"], CultureInfo.InvariantCulture);
            _outlineThickness = int.Parse(config["outlineThickness"]);
            _backgroundColor = CreateItems.CreateColor(config["backgroundColor"]);
            _keyBackgroundColor = CreateItems.CreateColor(config["keyColor"]);
            _barColor = CreateItems.CreateColor(config["barColor"]);
            _maxFPS = uint.Parse(config["maxFPS"]);

            //get background image if in config
            if (config["backgroundImage"] != "")
                _background = new Sprite(new Texture(
                    Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Resources",
                        config["backgroundImage"]))));

            //create keys which will be used to create the squares and text
            var keyAmount = int.Parse(config["keyAmount"]);
            for (var i = 1; i <= keyAmount; i++)
                try
                {
                    var key = new Key(config[$"key" + i]);
                    if (config.ContainsKey($"displayKey" + i))
                        if (config[$"displayKey" + i] != "")
                            key.KeyLetter = config[$"displayKey" + i];
                    _keyList.Add(key);
                }
                catch (InvalidOperationException e)
                {
                    //invalid key
                    Console.WriteLine(e.Message);
                    using var sw = new StreamWriter("keyErrorMessage.txt");
                    sw.WriteLine(e.Message);
                }

            //create squares and add them to _staticDrawables list
            var outlineColor = CreateItems.CreateColor(config["borderColor"]);
            var keySize = int.Parse(config["keySize"]);
            var margin = int.Parse(config["margin"]);
            _squareList = CreateItems.CreateKeys(keyAmount, _outlineThickness, keySize, _ratioX, _ratioY, margin,
                _window, _keyBackgroundColor, outlineColor);
            foreach (var square in _squareList) _staticDrawables.Add(square);

            //create text and add it ti _staticDrawables list
            _fontColor = CreateItems.CreateColor(config["fontColor"]);
            _pressFontColor = CreateItems.CreateColor(config["pressFontColor"]);
            for (var i = 0; i < keyAmount; i++)
            {
                var text = CreateItems.CreateText(_keyList.ElementAt(i).KeyLetter, _squareList.ElementAt(i),
                    _fontColor, false);
                _keyText.Add(text);
                _staticDrawables.Add(text);
            }

            _fading = bool.Parse(config["fading"]);
            _counter = bool.Parse(config["keyCounter"]);
            _showErrors = bool.Parse(config["showErrors"]);
            _keyAmount = int.Parse(config["keyAmount"]);
            _showKeyLock = bool.Parse(config["showKeyLock"]);

            _keyOrder = config["keyOrder"].Split(',').Select(int.Parse).ToArray();
        }

        private Dictionary<string, string> ReadConfig(string configFileName)
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //var objectDict = new Dictionary<string, string>();
            //var file = configFileName == null ?
            //    File.ReadLines(Path.Combine(assemblyPath ?? "", "config.txt")).ToArray() :
            //    File.ReadLines(Path.Combine(assemblyPath ?? "", configFileName)).ToArray();
            //foreach (var s in file) objectDict.Add(s.Split("=")[0], s.Split("=")[1]);

            ConfigFile cfg = JsonSerializer.Deserialize<ConfigFile>(File.ReadAllText(Path.Combine(assemblyPath ?? "", "config.json")));

            Dictionary<string, string> cfgDict = new Dictionary<string, string> {
                {"keyAmount", cfg.KeyAmount.ToString()},
                {"key1", cfg.Key1},
                {"key2", cfg.Key2},
                {"key3", cfg.Key3},
                {"key4", cfg.Key4},
                {"key5", cfg.Key5},
                {"key6", cfg.Key6},
                {"key7", cfg.Key7},
                {"displayKey1", cfg.DisplayKey1},
                {"displayKey2", cfg.DisplayKey2},
                {"displayKey3", cfg.DisplayKey3},
                {"displayKey4", cfg.DisplayKey4},
                {"displayKey5", cfg.DisplayKey5},
                {"displayKey6", cfg.DisplayKey6},
                {"displayKey7", cfg.DisplayKey7},
                {"keyOrder", cfg.keyOrder},
                {"keyCounter", cfg.KeyCounter.ToString()},
                {"windowHeight", cfg.WindowHeight.ToString()},
                {"windowWidth", cfg.WindowWidth.ToString()},
                {"keySize", cfg.KeySize.ToString()},
                {"barSpeed", cfg.BarSpeed.ToString()},
                {"margin", cfg.Margin.ToString()},
                {"outlineThickness", cfg.OutlineThickness.ToString()},
                {"fading", cfg.Fading.ToString()},
                {"backgroundColor", cfg.BackgroundColor.ToString()},
                {"keyColor", cfg.KeyColor.ToString()},
                {"borderColor", cfg.BorderColor.ToString()},
                {"barColor", cfg.BarColor.ToString()},
                {"fontColor", cfg.FontColor.ToString()},
                {"pressFontColor", cfg.PressFontColor.ToString()},
                {"backgroundImage", cfg.BackgroundImage.ToString()},
                {"maxFPS", cfg.MaxFPS.ToString()},
                {"showErrors", cfg.ShowErrors.ToString() },
                {"showKeyLock", cfg.ShowErrors.ToString() }
            };

            return cfgDict;
        }

        private void OnClose(object sender, EventArgs e)
        {
            _window.Close();
        }

        public void Run()
        {
            _window.Closed += OnClose;
            _window.SetFramerateLimit(_maxFPS);

            //Creating a sprite for the fading effect
            var fadingList = Fading.GetBackgroundColorFadingTexture(_backgroundColor, _window.Size.X, _ratioY);
            var fadingTexture = new RenderTexture(_window.Size.X, (uint)(255 * 2 * _ratioY));
            fadingTexture.Clear(Color.Transparent);
            if (_fading)
                foreach (var sprite in fadingList)
                    fadingTexture.Draw(sprite);
            fadingTexture.Display();
            var fadingSprite = new Sprite(fadingTexture.Texture);

            int keyListRot = 0;
            while (_window.IsOpen)
            {
                _window.Clear(_backgroundColor);
                _window.DispatchEvents();

                //if no keys are being held fill the square with bg color
                foreach (var square in _squareList) square.FillColor = _keyBackgroundColor;
                //if a key is being held, change the key bg and increment hold variable of key
                int keyIndex = 0;
                foreach (var key in _keyList)
                {
                    if (key.isKey && Keyboard.IsKeyPressed(key.KeyboardKey) ||
                        !key.isKey && Mouse.IsButtonPressed(key.MouseButton))
                    {
                        if (!key.Held) {
                            key.Held = true;

                            if (keyListRot != _keyOrder[keyIndex]) {
                                Debug.WriteLine(string.Format("error: {0}", key.KeyLetter));
                                key.Error = true;
                                keyListRot = _keyOrder[keyIndex];
                            }

                            //Debug.WriteLine(keyListRot.ToString());
                            keyListRot++;
                        }
                        
                        key.Hold++;
                        if (_keyText.ElementAt(_keyList.IndexOf(key)).FillColor != _pressFontColor)
                            _keyText.ElementAt(_keyList.IndexOf(key)).FillColor = _pressFontColor;
                        {
                            Color finalBarColor = _barColor;

                            if (key.Error)
                                finalBarColor = Color.Red;

                            // NOTE: This kinda an assumption on the key layout. probably should do this better later.
                            if (_keyAmount == 4 && _showKeyLock) {
                                _squareList.ElementAt(_keyList.IndexOf(key)).FillColor = finalBarColor;

                                if ((_keyList[0].Held && _keyList[2].Held) && (keyIndex == 0 || keyIndex == 2)) {
                                    finalBarColor = Color.Yellow;
                                }
                                if ((_keyList[1].Held && _keyList[3].Held) && (keyIndex == 1 || keyIndex == 3)) {
                                    finalBarColor = Color.Yellow;
                                }
                            }

                            _squareList.ElementAt(_keyList.IndexOf(key)).FillColor = finalBarColor;
                        }

                    } else
                    {
                        if (_keyText.ElementAt(_keyList.IndexOf(key)).FillColor != _fontColor)
                            _keyText.ElementAt(_keyList.IndexOf(key)).FillColor = _fontColor;
                        key.Hold = 0;
                        key.Held = false;
                        key.Error = false;
                    }

                    if (keyListRot >= _keyAmount) {
                        keyListRot = 0;
                    }

                    keyIndex++;
                }

                MoveBars(_keyList, _squareList);

                //draw bg from image if not null

                if (_background is not null)
                    _window.Draw(_background);
                foreach (var staticDrawable in _staticDrawables) _window.Draw(staticDrawable);

                foreach (var key in _keyList)
                {
                    if (_counter)
                    {
                        var text = CreateItems.CreateText(Convert.ToString(key.Counter),
                            _squareList.ElementAt(_keyList.IndexOf(key)),
                            _fontColor, true);
                        _window.Draw(text);
                    }

                    foreach (var bar in key.BarList)
                        _window.Draw(bar);
                }

                _window.Draw(fadingSprite);

                _window.Display();
            }
        }

        /// <summary>
        /// if a key is a new input create a new bar, if it is being held stretch it and move all bars up
        /// </summary>
        private void MoveBars(List<Key> keyList, List<RectangleShape> squareList)
        {
            var moveDist = _clock.Restart().AsSeconds() * _barSpeed;

            foreach (var key in keyList)
            {
                if (key.Hold == 1)
                {
                    var rect = CreateItems.CreateBar(squareList.ElementAt(keyList.IndexOf(key)), _outlineThickness,
                        moveDist);
                    key.BarList.Add(rect);
                    key.Counter++;
                }
                else if (key.Hold > 1)
                {
                    var rect = key.BarList.Last();
                    rect.Size = new Vector2f(rect.Size.X, rect.Size.Y + moveDist);
                }

                foreach (var rect in key.BarList)
                    rect.Position = new Vector2f(rect.Position.X, rect.Position.Y - moveDist);
                if (key.BarList.Count > 0 && key.BarList.First().Position.Y + key.BarList.First().Size.Y < 0)
                    key.BarList.RemoveAt(0);
            }
        }
    }
}