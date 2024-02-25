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
using static SFML.Window.Keyboard;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Runtime.InteropServices;
using osu.Framework.Input;
using osu.Framework.Input.Bindings;
using osu.Game.Database;
using osu.Game.Input.Bindings;
using osu.Game.Rulesets;
using Realms;

namespace KeyOverlay
{
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
        private ConfigFile _config;
        private FileSystemWatcher clientReamWatcher;
        private Key _reloadKey;

        private string ConvertToAlias(string str) {
            string alias = str;

            if (_config.KeyNameAliases.ContainsKey(str)) {
                alias = _config.KeyNameAliases[str];
            }

            return alias;
        }

        private void ReloadClientRealm() {
            Debug.WriteLine("Reloading client realm....");

            RealmConfiguration clientRealm = new RealmConfiguration(Path.Combine(Path.Combine(_config.LazerInstall, "client.realm")));
            clientRealm.SchemaVersion = 40;

            List<Key> leftKeys = new List<Key>();
            List<Key> rightKeys = new List<Key>();
            using (Realm realm = Realm.GetInstance(clientRealm)) {
                foreach (RealmKeyBinding action in realm.All<RealmKeyBinding>().Where(kb => kb.RulesetName == "osu")) {
                    ReadableKeyCombinationProvider rkcp = new ReadableKeyCombinationProvider();
                    string keyString = rkcp.GetReadableString(action.KeyCombination);
                    if (action.Action.ToString() == "0") {
                        Key k = new Key(keyString);
                        k.KeyLetter = ConvertToAlias(keyString);
                        leftKeys.Add(k);
                    }
                    if (action.Action.ToString() == "1") {
                        Key k = new Key(keyString);
                        k.KeyLetter = ConvertToAlias(keyString);
                        rightKeys.Add(k);
                    }
                }
            }

            _keyList.Clear();
            _keyList.Add(leftKeys[0]);
            _keyList.Add(rightKeys[0]);
            _keyList.Add(leftKeys[1]);
            _keyList.Add(rightKeys[1]);


            if (_keyText != null && _staticDrawables != null && _squareList != null) {
                _keyText.Clear();
                _staticDrawables.Clear();
                foreach (var square in _squareList) _staticDrawables.Add(square);

                for (var i = 0; i < _config.KeyAmount; i++) {
                    var text = CreateItems.CreateText(_keyList.ElementAt(i).KeyLetter, _squareList.ElementAt(i),
                        _fontColor, false);
                    _keyText.Add(text);
                    _staticDrawables.Add(text);
                }
            }
        }

        public AppWindow(string configFileName)
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _config = JsonSerializer.Deserialize<ConfigFile>(File.ReadAllText(Path.Combine(assemblyPath ?? "", "config.json")));

            if (_config.UseKeyboardHooks) {
                KeyboardHook.CreateHook();
            }

            //clientReamWatcher = new FileSystemWatcher();
            //clientReamWatcher.Path = _config.LazerInstall;
            //clientReamWatcher.Filter = "client.realm";
            //clientReamWatcher.NotifyFilter = NotifyFilters.LastWrite;
            //clientReamWatcher.Changed += ReloadClientRealm;
            //clientReamWatcher.EnableRaisingEvents = true;

            _window = new RenderWindow(new VideoMode(_config.WindowWidth, _config.WindowHeight),
                "KeyOverlay", Styles.Default);

            //calculate screen ratio relative to original program size for easy resizing
            _ratioX = (float)(_config.WindowWidth) / 480f;
            _ratioY = (float)(_config.WindowHeight) / 960f;

            _barSpeed = _config.BarSpeed;
            _outlineThickness = _config.OutlineThickness;
            _backgroundColor = CreateItems.CreateColor(_config.BackgroundColor);
            _keyBackgroundColor = CreateItems.CreateColor(_config.KeyColor);
            _barColor = CreateItems.CreateColor(_config.BarColor);
            _maxFPS = _config.MaxFPS;

            //get background image if in config
            if (_config.BackgroundImage != "")
                _background = new Sprite(new Texture(
                    Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Resources", _config.BackgroundImage))));

            _reloadKey = new Key(_config.ReloadKey);
            _reloadKey.KeyLetter = ConvertToAlias(_config.ReloadKey);

            if (_config.UseLazerKeyConfig) {
                ReloadClientRealm();
            } else {
                Key k1 = new Key(_config.Key1);
                Key k2 = new Key(_config.Key2);
                Key k3 = new Key(_config.Key3);
                Key k4 = new Key(_config.Key4);

                k1.KeyLetter = ConvertToAlias(_config.Key1);
                k2.KeyLetter = ConvertToAlias(_config.Key2);
                k3.KeyLetter = ConvertToAlias(_config.Key3);
                k4.KeyLetter = ConvertToAlias(_config.Key4);

                _keyList.Add(k1);
                _keyList.Add(k2);
                _keyList.Add(k3);
                _keyList.Add(k4);
            }

            //create squares and add them to _staticDrawables list
            var outlineColor = CreateItems.CreateColor(_config.BorderColor);
            var keySize = _config.KeySize;
            var margin = _config.Margin;
            _squareList = CreateItems.CreateKeys(_config.KeyAmount, _outlineThickness, keySize, _ratioX, _ratioY, margin,
                _window, _keyBackgroundColor, outlineColor);
            foreach (var square in _squareList) _staticDrawables.Add(square);

            //create text and add it ti _staticDrawables list
            _fontColor = CreateItems.CreateColor(_config.FontColor);
            _pressFontColor = CreateItems.CreateColor(_config.PressFontColor);
            for (var i = 0; i < _config.KeyAmount; i++)
            {
                var text = CreateItems.CreateText(_keyList.ElementAt(i).KeyLetter, _squareList.ElementAt(i),
                    _fontColor, false);
                _keyText.Add(text);
                _staticDrawables.Add(text);
            }

            _fading = _config.Fading;
            _counter = _config.KeyCounter;
            _showErrors = _config.ShowErrors;
            _keyAmount = _config.KeyAmount;
            _showKeyLock = _config.ShowKeyLock;

            _keyOrder = _config.keyOrder.Split(',').Select(int.Parse).ToArray();
        }

        private void OnClose(object sender, EventArgs e)
        {
            _window.Close();
        }

        private bool GetKeyDown(Key k) {
            if (_config.UseKeyboardHooks) {
                return KeyboardHook.KeyStates[k.VKCode];
            }

            return Keyboard.IsKeyPressed(k.KeyboardKey);
        }

        public void Run()
        {
            _window.Closed += OnClose;

            if (!_config.UncappedFPS) { 
                _window.SetFramerateLimit(_maxFPS);
            }

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
            Stopwatch reloadCounter = new Stopwatch();
            reloadCounter.Start();
            while (_window.IsOpen)
            {
                if (reloadCounter.ElapsedMilliseconds >= 1000 && GetKeyDown(_reloadKey) && _config.UseLazerKeyConfig) {
                    ReloadClientRealm();
                    reloadCounter.Restart();
                }
                _window.Clear(_backgroundColor);
                _window.DispatchEvents();

                //if no keys are being held fill the square with bg color
                foreach (var square in _squareList) square.FillColor = _keyBackgroundColor;
                //if a key is being held, change the key bg and increment hold variable of key
                int keyIndex = 0;
                foreach (var key in _keyList)
                {
                    //if (key.isKey && Keyboard.IsKeyPressed(key.KeyboardKey) ||
                    //    !key.isKey && Mouse.IsButtonPressed(key.MouseButton))

                    //if (key.isKey && Keyboard.IsKeyPressed(key.KeyboardKey))
                    if (key.isKey && GetKeyDown(key))
                    {
                        if (!key.Held) {
                            key.Held = true;

                            if (keyListRot != _keyOrder[keyIndex]) {
                                Debug.WriteLine(string.Format("hit error: {0}", key.KeyLetter));
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

                    if (keyListRot >= _keyOrder.Length) {
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