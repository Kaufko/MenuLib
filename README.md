# MenuLib
MenuLib is a dotnet library that is used to create an interactive menu in console.

---
## Features
1. Easily configurable controls
2. Advanced customization
3. Menu history
4. Multiple keybinds

## Usage

<h3>Basic usage</h3>

First you need to make a main menu, which you will later pass as args into the Start() function.

```C#
using MenuLibrary;

namespace Main
{
    public static class Program
    {
        static List<Menu> mainMenu = new List<Menu>
        {
            new Menu("Option1", () => Console.ReadKey()),
            new Menu("Option2", () => Console.ReadKey())
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
        static List<Menu> mainMenu = new List<Menu>
        {
            new Menu("Option1", () => Console.ReadKey())
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
new Menu("Submenu", () => SelectSubMenu(subMenu))
```

To change the controls simply call the ChangeKey() function which returns the new key that you can assign to one of the controls.

```C#
//Everything except the first argument is optional
keyPress = ChangeKey(ConsoleKey.Enter)
```

<h4> Here is a list of all configurable keys and what they do </h4>

1. keyPress -> "clicks" an option
2. keyBack -> goes back in history
3. keyForward -> goes forward in history (only usable if you went back before)
4. keyQuit -> exits the interactive menu (not the program)
5. keyMainMenu -> goes back to main menu
6. keyUp -> goes up by 1
7. keyDown -> goes down by 1
