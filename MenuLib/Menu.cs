using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace MenuLibrary
{
    public static class MenuLib
    {
        #region KeyBinds
        static public ConsoleKeyInfo keyPress; // Key to interact with option   
        static public ConsoleKeyInfo keyBack; // Key to move backward to the previous menu
        static public ConsoleKeyInfo keyForward; // Key to move forward to the next menu    
        static public ConsoleKeyInfo keyQuit; // Key to quit the menu
        static public ConsoleKeyInfo keyMainMenu; // Key to return to the main menu
        static public ConsoleKeyInfo keyUp; // Key to move selection up
        static public ConsoleKeyInfo keyDown; // Key to move selection down

        static private readonly Dictionary<string, string> defaultKeyBindings = new Dictionary<string, string>
        {
            { "Press", "13|0" },        // Enter
            { "Back", "37|0" },         // LeftArrow
            { "Forward", "39|0" },      // RightArrow
            { "Quit", "27|0" },         // Escape
            { "MainMenu", "32|0" },     // Spacebar
            { "Up", "38|0" },           // UpArrow
            { "Down", "40|0" },          // DownArrow
        };

        static ConsoleKey ConvertNumberToConsoleKey(int number)
        {
            if (Enum.IsDefined(typeof(ConsoleKey), number)) // Check if the number matches a special key (e.g., Enter, Escape)
            {
                return (ConsoleKey)number;
            }
            else
            {
                return (ConsoleKey)Convert.ToChar(number); //else just convert to char
            }
        }

        #endregion

        #region Registry
        static private string registryKeyPath = @"HKEY_CURRENT_USER\Software\Kaufko\MenuLib"; //loads in format ascii/bool/bool/bool

        static ConsoleKeyInfo LoadKeyFromRegistry(string keyName)
        {
            string registryValue;


            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) // Checks if the current platform is Windows
            {
                registryValue = Registry.GetValue(registryKeyPath, keyName, null) as string;

                if (string.IsNullOrEmpty(registryValue)) //checks if keybinds have been changed
                {
                    registryValue = defaultKeyBindings[keyName];
                }
            }
            else
            {
                registryValue = defaultKeyBindings[keyName]; // For non-Windows platforms, directly use the default keybinds
            }

            //checks for modifiers in console key info
            int[] regVal = registryValue.Split("|").Select(int.Parse).ToArray();
            bool[] modifiers = new bool[3];
            if (regVal[1] >= 4)
            {
                modifiers[0] = true;
                regVal[1] -= 4;
            }
            if (regVal[1] >= 2)
            {
                modifiers[1] = true;
                regVal[1] -= 2;
            }
            if (regVal[1] >= 1)
            {
                modifiers[2] = true;
                regVal[1] -= 1;
            }
            return new ConsoleKeyInfo('\0', ConvertNumberToConsoleKey(regVal[0]), modifiers[0], modifiers[1], modifiers[2]);
        }

        public static void SaveKeyToRegistry(string keyName, ConsoleKeyInfo keyBind)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Registry.SetValue(registryKeyPath, keyName, $"{(int)keyBind.Key}|{(int)keyBind.Modifiers}");
            }
        }

        #endregion

        #region  Variables


        static public int selectionIndex = 0;

        static internal ConsoleColor defaultForegroundColor;
        static internal ConsoleColor defaultBackgroundColor;
        static public List<Option> activeMenu = new List<Option>();

      static private LinkedList<List<Option>> navigationHistory = new();
        #endregion


        public static ConsoleKeyInfo KeyChange(string keyName, ConsoleKey key = ConsoleKey.None, bool shift = false, bool alt = false, bool ctrl = false)
        {
            char keyChar;
            if (char.TryParse(key.ToString(), out keyChar))
            {
                keyChar = '\0';
            }
            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo(keyChar, key, shift, alt, ctrl); // Create custom key mapping

            SaveKeyToRegistry(keyName, keyInfo); // Save the key binding to the registry after creating the key mapping

            return keyInfo;
        }

        public static ConsoleKeyInfo KeyChange(string keyName, ConsoleKeyInfo keyInfo)
        {
            SaveKeyToRegistry(keyName, keyInfo);
            return keyInfo;
        }

        public static void Start(List<Option> mainMenu)
        {
            defaultForegroundColor = Console.ForegroundColor;
            defaultBackgroundColor = Console.BackgroundColor;

            LoadKeybinds();

            activeMenu = mainMenu; // Set the initial menu
            ConsoleKeyInfo keyRead;
            do
            {
                Write(); // Display the current menu
                keyRead = Console.ReadKey(true); // Read user input without disaplying it
                if (keyRead.Key == keyPress.Key && keyRead.Modifiers == keyPress.Modifiers)
                {
                    Console.Clear();
                    activeMenu[selectionIndex].Action.Invoke(); // Execute the action associated with the selected option
                }
                else if (keyRead.Key == keyUp.Key && keyRead.Modifiers == keyUp.Modifiers && selectionIndex - 1 >= 0)
                {
                    selectionIndex--; // Move the selection up
                }
                else if (keyRead.Key == keyDown.Key && keyRead.Modifiers == keyDown.Modifiers && selectionIndex + 1 < activeMenu.Count)
                {
                    selectionIndex++; // Move the selection down
                }
                else if (keyRead.Key == keyMainMenu.Key && keyRead.Modifiers == keyMainMenu.Modifiers)
                {
                    selectionIndex = 0; // Reset selection to the top of the main menu
                    activeMenu = mainMenu; // Go back to the main menu
                }
                else if (keyRead.Key == keyBack.Key && keyRead.Modifiers == keyBack.Modifiers)
                {
                    GoBack(); // Navigate to the previous menu
                }
                else if (keyRead.Key == keyForward.Key && keyRead.Modifiers == keyForward.Modifiers)
                {
                    GoForwards(); // Navigate to the next menu
                }
            }
            while (keyRead.Key != keyQuit.Key); // Exit the loop when quit key is pressed
        }

        public static void Write()
        {
            Console.Clear();

            foreach (Option option in activeMenu)
            {
                if (option.TextColor != null)
                {
                    Console.ForegroundColor = (ConsoleColor)option.TextColor;
                }
                if (option.TextHighlightColor != null)
                {
                    Console.BackgroundColor = (ConsoleColor)option.TextHighlightColor;
                }
                string finalText = option.Text;
                try
                {
                    if (activeMenu[selectionIndex] == option)
                    {
                        Console.WriteLine(">" + finalText); // Highlight the selected option
                    }
                    else
                    {
                        Console.WriteLine(" " + finalText); // Display unselected options
                    }
                }
                catch
                {
                    selectionIndex = 0; // Reset selection if out of bounds
                }
                Console.ResetColor(); // Reset console colors
            }
        }

        if (currentNode != null)
{
    while (currentNode.Next != null)
    {
        navigationHistory.Remove(currentNode.Next);
    }
}


        private static void GoBack()
{
    if (currentNode != null && currentNode.Previous != null)
    {
        // Move to the previous menu in the history
        currentNode = currentNode.Previous;
        activeMenu = currentNode.Value;  // Assign the menu from the previous node to activeMenu
        selectionIndex = 0;  // Optionally reset the selection index
    }
}

private static void GoForwards()
{
    if (currentNode != null && currentNode.Next != null)
    {
        // Move to the next menu in the history
        currentNode = currentNode.Next;
        activeMenu = currentNode.Value;  // Assign the menu from the next node to activeMenu
        selectionIndex = 0;  // Optionally reset the selection index
    }
}

        private static void LoadKeybinds(bool resetKeybinds = false)
        {
            keyBack = LoadKeyFromRegistry("Back");
            keyDown = LoadKeyFromRegistry("Down");
            keyForward = LoadKeyFromRegistry("Forward");
            keyMainMenu = LoadKeyFromRegistry("MainMenu");
            keyPress = LoadKeyFromRegistry("Press");
            keyQuit = LoadKeyFromRegistry("Quit");
            keyUp = LoadKeyFromRegistry("Up");
        }
    }// End of MenuLib class
    public class Option
    {
        public string Text { get; set; }
        public Action Action { get; set; } // Is executed on option selection
        public ConsoleColor? TextColor { get; set; }
        public ConsoleColor? TextHighlightColor { get; set; }


        public Option(string text, Action action, ConsoleColor? textColor = null, ConsoleColor? textHighlightColor = null)
        {
            Text = text;
            Action = action;
            TextColor = textColor;
            TextHighlightColor = textHighlightColor;
        }

        public Option(string text, Action action, ConsoleColor? textHighlightColor = null)
        {
            Text = text;
            Action = action;
            TextColor = null;
            TextHighlightColor = textHighlightColor;
        }

        public Option(string text, Action action)
        {
            Text = text;
            Action = action;
            TextColor = null;
            TextHighlightColor = null;
        }
    }
}