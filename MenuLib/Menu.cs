using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MenuLibrary
{
    public static class MenuLib
    {
        //default keybinds
        static private readonly Dictionary<string, string> defaultKeyBindings = new Dictionary<string, string>
        {
            { "Press", "13|0" },        // Enter
            { "Back", "37|0" },         // LeftArrow
            { "Forward", "39|0" },      // RightArrow
            { "Quit", "27|0" },         // Escape
            { "MainMenu", "32|0" },     // Spacebar
            { "Up", "38|0" },           // UpArrow
            { "Down", "40|0" }          // DownArrow
        };

        #region Registry
        //default registry path
        static private string registryKeyPath = @"HKEY_CURRENT_USER\Software\Kaufko\MenuLib";
        //loads in format ascii/bool/bool/bool
        static ConsoleKeyInfo LoadKeyFromRegistry(string keyName)
        {
            //retrieves value from registry
            string registryValue; // Initialize the registry value to null
            // Check if the current platform is Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // If on Windows, try to access the registry
                registryValue = Registry.GetValue(registryKeyPath, keyName, null) as string;

                // If registry value is null or empty, fall back to default
                if (string.IsNullOrEmpty(registryValue))
                {
                    registryValue = defaultKeyBindings[keyName]; // Default value
                }
            }
            else
            {
                // For non-Windows platforms, directly use the default keybindings
                registryValue = defaultKeyBindings[keyName]; // Default value
            }


            // Split the string into parts (key and modifiers)
            int[] regVal = registryValue.Split("|").Select(int.Parse).ToArray();
            //stores the key modifiers
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
            return new ConsoleKeyInfo('\0',ConvertNumberToConsoleKey(regVal[0]), modifiers[0], modifiers[1], modifiers[2]);
        }
        #endregion
        //saves keybind
        static void SaveKeyToRegistry(string keyName, ConsoleKeyInfo keyBind)
        {
        //checks if OS is windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Registry.SetValue(registryKeyPath, keyName, $"{(int)keyBind.Key}|{(int)keyBind.Modifiers}");
            }
            //saves value to registry
            
        }
        //decodes ascii to consolekey 
        static ConsoleKey ConvertNumberToConsoleKey(int number)
        {
            if (Enum.IsDefined(typeof(ConsoleKey), number))
            {
                // Check if the number matches a special key (e.g., Enter, Escape)
                return (ConsoleKey)number;
            }
            else
            {
                //if it's not a special key, just convert to a char
                return (ConsoleKey)Convert.ToChar(number);
            }
        }
    
        #region  Vars
        static public List<Option> activeMenu = new List<Option>(); // Current menu being displayed

        static public int selectionIndex = 0; // Current selection index in the menu
        

        static private Stack<List<Option>> backHistory = new Stack<List<Option>>(); // Stack to track previous menus for back navigation
        static private Stack<List<Option>> forwardHistory = new Stack<List<Option>>(); // Stack to track menus for forward navigation
        #endregion

        #region Keys
        static public ConsoleKeyInfo keyPress; //Key to select option
        static public ConsoleKeyInfo keyBack; //Key to move back to the last menu
        static public ConsoleKeyInfo keyForward; // Key to move forward to the next menu    
        static public ConsoleKeyInfo keyQuit; // Key to quit the menu
        static public ConsoleKeyInfo keyMainMenu; // Key to return to the main menu
        static public ConsoleKeyInfo keyUp; // Key to move selection up
        static public ConsoleKeyInfo keyDown; // Key to move selection down

        #endregion


        //changes keybind and saves to registry (Windows only)
        public static ConsoleKeyInfo KeyChange(string keyName, ConsoleKey key = ConsoleKey.None, bool shift = false, bool alt = false, bool ctrl = false)
        {
            //temporary value to pass TryParse out
            char keyChar;
            //checks if key is a valid character or not
            //check if this is even needed, shouldn't it be inverted?
            if(char.TryParse(key.ToString(), out keyChar))
            {
                //sets keyChar to "null"
                keyChar = '\0';
            }
            //builds the ConsoleKeyInfo
            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo(keyChar, key, shift, alt, ctrl); // Create custom key mapping

            // Save the key binding to the registry after creating the key mapping
            SaveKeyToRegistry(keyName, keyInfo);

            return keyInfo;
        }

        //main function of the library
        public static void Start(List<Option> mainMenu)
        {
            //loads keys from the registry
            keyBack = LoadKeyFromRegistry("Back");
            keyDown = LoadKeyFromRegistry("Down");
            keyForward = LoadKeyFromRegistry("Forward");
            keyMainMenu = LoadKeyFromRegistry("MainMenu");
            keyPress = LoadKeyFromRegistry("Press");
            keyQuit = LoadKeyFromRegistry("Quit");
            keyUp = LoadKeyFromRegistry("Up");


            activeMenu = mainMenu; // Set the initial menu
            ConsoleKeyInfo keyRead;
            do
            {
                Write(); // Display the current menu
                keyRead = Console.ReadKey(true); // Read user input without disaplying it
                if (keyRead.Key == keyPress.Key)
                {
                    Console.Clear();
                    activeMenu[selectionIndex].Action.Invoke(); // Execute the action associated with the selected option
                }
                else if (keyRead.Key == keyUp.Key && selectionIndex - 1 >= 0)
                {
                    selectionIndex--; // Move the selection up
                }
                else if (keyRead.Key == keyDown.Key && selectionIndex + 1 < activeMenu.Count)
                {
                    selectionIndex++; // Move the selection down
                }
                else if (keyRead.Key == keyMainMenu.Key)
                {
                    selectionIndex = 0; // Reset selection to the top of the main menu
                    activeMenu = mainMenu; // Go back to the main menu
                    backHistory.Clear(); // Clear navigation history
                    forwardHistory.Clear();
                }
                else if (keyRead.Key == keyBack.Key)
                {
                    GoBack(); // Navigate to the previous menu
                }
                else if (keyRead.Key == keyForward.Key)
                {
                    GoForwards(); // Navigate to the next menu
                }
            }
            while (keyRead.Key != keyQuit.Key); // Exit the loop when quit key is pressed
        }

        //writes out the active menu
        public static void Write()
        {
            Console.Clear();

            foreach (Option option in activeMenu)
            {
                Console.ForegroundColor = option.TextColor; // Set text color
                Console.BackgroundColor = option.TextHighlightColor; // Set background color
                try
                {
                    if (activeMenu[selectionIndex] == option)
                    {
                        Console.WriteLine(">" + option.Text); // Highlight the selected option
                    }
                    else
                    {
                        Console.WriteLine(" " + option.Text); // Display unselected options
                    }
                }
                catch
                {
                    selectionIndex = 0; // Reset selection if out of bounds
                }
                Console.ResetColor(); // Reset console colors
            }
        }

        public static void SelectSubMenu(List<Option> menu)
        {
            backHistory.Push(activeMenu); // Save current menu to back history
            forwardHistory.Clear(); // Clear forward navigation history
            selectionIndex = 0; // Reset selection index for the submenu
            activeMenu = menu; // Set the new menu as active
        }

        private static void GoBack()
        {
            if (backHistory.Count > 0)
            {
                forwardHistory.Push(activeMenu); // Save current menu to forward history
                activeMenu = backHistory.Pop(); // Retrieve the last menu from back history
                selectionIndex = 0; // Reset selection index
            }
        }

        private static void GoForwards()
        {
            if (forwardHistory.Count > 0)
            {
                backHistory.Push(activeMenu); // Save current menu to back history
                activeMenu = forwardHistory.Pop(); // Retrieve the next menu from forward history
                selectionIndex = 0; // Reset selection index
            }
        }
    }

    public class Option
    {
        public string Text { get; set; } // Menu option text
        public Action Action { get; set; } // Action executed when the option is selected
        public ConsoleColor TextColor { get; set; } // Text color of the option
        public ConsoleColor TextHighlightColor { get; set; } // Highlight color for the selected option

        public Option(string text, Action action, ConsoleColor textColor = ConsoleColor.White, ConsoleColor textHighlightColor = ConsoleColor.Black)
        {
            Text = text;
            Action = action;
            TextColor = textColor;
            TextHighlightColor = textHighlightColor;
        }
    }
}

