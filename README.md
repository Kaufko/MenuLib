# MenuLib
MenuLib is a dotnet library that is used to create an interactive menu in console.

---
## Features
1. Seamless menu changing and reloading
2. Advanced color customization
3. Menu navigation (Forward | Backward)
4. Keybinds with Windows Registry support

## Usage

<h3>Basic usage</h3>

First you need to make a main menu, which you will later pass as args into the Start() function.
Each menu is made from Options which have 3 overloads
```C#
Option(string text, Action action, ConsoleColor? textColor = null, ConsoleColor? textHighlightColor = null)
Option(string text, Action action, ConsoleColor? textHighlightColor = null)
Option(string text, Action action)
```

```C#
using MenuLibrary;

namespace Main
{
    public static class Program
    {
        static List<Option> mainMenu = new List<Option>
        {
            new Option("Option1", () => Console.ReadKey()),
            new Option("Option2", () => Console.ReadKey())
        };
    }
}
```

After that you are ready to use the interactive menu!
All you need to do is call the Start() function and pass the main menu as args

```C#
using MenuLibrary;

namespace Main
{
    public static class Program
    {
        static List<Option> mainMenu = new List<Option>
        {
            new Option("Option1", () => Console.ReadKey())
        };

        public static void Main(string[] args)
        {
            MenuLib.Start(mainMenu);
        }
    }
}
```

<h4>Changing menus</h4>

Changing menus is very easy. When creating an option use the function SelectSubMenu()

```C#
new Option("Menu change", () => SelectMenu(subMenu))
```

To change the controls simply call the ChangeKey() function which returns the new key that you can assign to one of the controls.
KeyChange has 2 overloads, both returning ConsoleKeyInfo

```C#
KeyChange(string keyName, ConsoleKeyInfo keyInfo)

KeyChange(string keyName, ConsoleKey key = ConsoleKey.None, bool shift = false, bool alt = false, bool ctrl = false)
```

<h4> Here is a list of all configurable keys and what they do </h4>

1. keyPress -> calls the function of an option
2. keyBack -> goes back in history
3. keyForward -> goes forward in history (only usable if you went back before)
4. keyQuit -> exits the interactive menu (not the program)
5. keyMainMenu -> goes back to main menu
6. keyUp -> goes up by 1
7. keyDown -> goes down by 1

These are the default keybinds that load everytime (unless registry keybinds are present)
1. { "Press", "13|0" },        // Enter
2. { "Back", "37|0" },         // LeftArrow
3. { "Forward", "39|0" },      // RightArrow
4. { "Quit", "27|0" },         // Escape
5. { "MainMenu", "32|0" },     // Spacebar
6. { "Up", "38|0" },           // UpArrow
7. { "Down", "40|0" },          // DownArrow

To manually load and save from and to the registry use LoadKeyFromRegistry & SaveKeyToRegistry

```C#
ConsoleKeyInfo LoadKeyFromRegistry(string keyName);

void SaveKeyToRegistry(string keyName, ConsoleKeyInfo keyBind);
```

<h4> Colors </h4>

You can change the default text color by changing 2 variables
1. defaultForegroundColor
2. defaultBackgroundColor

The foreground color is the text color and the background color is the text highlight color.
