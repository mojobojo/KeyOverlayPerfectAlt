
# KeyOverlay
 A simple key overlay for 4 key players on lazer. This is customized for a 4 key players needs, no guarantees for capabilities for anyother other than 4 key players.

# config.json properties
- **keyAmount**: (Integer) The number of keys to be displayed.
- **key1, key2, key3, key4**: (String) You can either provied an ASCII value like "A", " ", ";", etc or the program should still work the previous format of the [sfml keys](https://www.sfml-dev.org/documentation/2.5.1/classsf_1_1Keyboard.php#acb4cacd7cc5802dec45724cf3314a142)
- **keyOrder**: (String) The alternating pattern to enforce. Default is left to right (0, 1, 2, 3) = (k1, k2, k3, k4)
- **keyCounter**: (Boolean) Enable or disable a counter for each key.
- **windowHeight**: (Integer) Height of the application window.
- **windowWidth**: (Integer) Width of the application window.
- **keySize**: (Integer) The size of the displayed key (excluding border)
- **barSpeed**: (Float) The speed at which the bars are travelling upwards.
- **margin**: (Integer) The margin of the keys from the sides.
- **outlineThickness**: (Integer) The thickness of the square border
- **fading**: (Boolean) Enable or disable fading effect.
- **backgroundColor**: (String) RGBA values for the background color.
- **keyColor**: (String) RGBA values for the color of keys.
- **borderColor**: (String) RGBA values for the color of borders around keys.
- **barColor**: (String) RGBA values for the color of the moving bar.
- **fontColor**: (String) RGBA values for the color of the text.
- **pressFontColor**: (String) RGBA values for the color of the text when pressed.
- **backgroundImage**: (String) Lets you set a background. Put the image into Resources directory and then put the filename into this property ex. "bg.png". Makes sure the background is the same resolution as your window and if you want transparency on your background you have to put the transparency on the image itself.
- **maxFPS**: (Integer) The target FPS for the program to run
- **showErrors**: (Boolean) Show a red bar when you do not alternate properly.
- **showKeyLock**: (Boolean) Show a yellow bar when you press together k1 + k3 or k2 + k4 at the same time.
- **keyNameAliases**: (Dictionary) Custom aliases for key names.

### Key Name Aliases

Left hand side is the key binding, right hand side is what you would like to display in the application.

- " " : "Spc"
- "Space" : "Spc"
- "LShift" : "LSh"

# Example Preview

![](https://i.imgur.com/rwW2gba.gif)

# Future plans and ideas
- Live display showing a list of errors.
- Reload key config off of lazer and stable when you change keys.
- Uhh, I think I had some other ideas but as I am writing this I cant remember so I typed this.

# Additional notes
- I commented out mouse stuff when I did the keyboard hooks. Nobody I know uses them and I just couldnt be bothered at the moment. Maybe later.
- Forked from [Blondazz](https://github.com/Blondazz/KeyOverlay)
