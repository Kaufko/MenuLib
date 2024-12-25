using MenuLibrary;
using System.Runtime.ConstrainedExecution;

class Program
{
    // Global main menu definition
    static List<Option> mainMenu = new List<Option>
    {
        new Option("Go to Submenu 1", () => MenuLib.SelectSubMenu(SubMenu1())),
        new Option("Go to Submenu 2", () => MenuLib.SelectSubMenu(SubMenu2())),
        new Option("View Controls", ViewControls),     // Option to view current key bindings
        new Option("Quit", () => Environment.Exit(0))  // Option to quit
    };

    static void Main()
    {
        //MenuLib.Start(mainMenu); // Start the menu system with the global main menu
        while (true)
        {
            MenuLib.Start(mainMenu);

        }

    }

    static List<Option> SubMenu1()
    {
        return new List<Option>
        {
            new Option("Option 1.1", () => Console.WriteLine("You selected Option 1.1!")),
            new Option("Option 1.2", () => Console.WriteLine("You selected Option 1.2!")),
            new Option("Back to Main Menu", () => MenuLib.SelectSubMenu(mainMenu)),
            new Option("Go to Submenu 1.3", () => MenuLib.SelectSubMenu(SubMenu1_3())),
            new Option("Quit", () => Environment.Exit(0))
        };
    }

    static List<Option> SubMenu2()
    {
        return new List<Option>
        {
            new Option("Option 2.1", () => Console.WriteLine("You selected Option 2.1!")),
            new Option("Option 2.2", () => Console.WriteLine("You selected Option 2.2!")),
            new Option("Option 2.3", () => Console.WriteLine("You selected Option 2.3!")),
            new Option("Back to Main Menu", () => MenuLib.SelectSubMenu(mainMenu)),
            new Option("Quit", () => Environment.Exit(0))
        };
    }

    static List<Option> SubMenu1_3()
    {
        return new List<Option>
        {
            new Option("Option 1.3.1", () => Console.WriteLine("You selected Option 1.3.1!")),
            new Option("Option 1.3.2", () => Console.WriteLine("You selected Option 1.3.2!")),
            new Option("Go Back", () => MenuLib.SelectSubMenu(SubMenu1())),
            new Option("Back to Main Menu", () => MenuLib.SelectSubMenu(mainMenu)),
            new Option("Quit", () => Environment.Exit(0))
        };
    }

    static void ViewControls()
    {
        Console.Clear();
        Console.WriteLine($"Current Controls:\n" +
                          $"Up: {MenuLib.keyUp.Key}\n" +
                          $"Down: {MenuLib.keyDown.Key}\n" +
                          $"Select: {MenuLib.keyPress.Key}\n" +
                          $"Back: {MenuLib.keyBack.Key}\n" +
                          $"Forward: {MenuLib.keyForward.Key}\n" +
                          $"Quit: {MenuLib.keyQuit.Key}\n" +
                          $"Main Menu: {MenuLib.keyMainMenu.Key}");
        Console.ReadKey();
    }
}
