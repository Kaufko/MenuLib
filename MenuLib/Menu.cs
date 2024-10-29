using System.Runtime.InteropServices;

namespace MenuLibrary
{
    public static class MenuLib
    {
        #region 
        static public List<Option> activeMenu = new List<Option>();
        static public int selectionIndex = 0;
        static public int currentHistoryIndex = 0;
        static public List<KeyValuePair<int, List<Option>>> menuHistory = new List<KeyValuePair<int, List<Option>>>();
        #endregion

        #region Keys
        static public ConsoleKeyInfo keyPress = KeyChange(ConsoleKey.Enter);
        static public ConsoleKeyInfo keyBack = KeyChange(ConsoleKey.LeftArrow);
        static public ConsoleKeyInfo keyForward = KeyChange(ConsoleKey.RightArrow);
        static public ConsoleKeyInfo keyQuit = KeyChange(ConsoleKey.Escape);
        static public ConsoleKeyInfo keyMainMenu = KeyChange(ConsoleKey.Spacebar);
        static public ConsoleKeyInfo keyUp = KeyChange(ConsoleKey.UpArrow);
        static public ConsoleKeyInfo keyDown = KeyChange(ConsoleKey.DownArrow);
        #endregion

        public static ConsoleKeyInfo KeyChange(ConsoleKey key, bool shift = false, bool alt = false, bool ctrl = false)
        {
            return new ConsoleKeyInfo('\0', key, shift, alt, ctrl);
        }

        public static void Start(List<Option> mainMenu)
        {
            activeMenu = mainMenu;
            ConsoleKeyInfo keyRead;
            do
            {
                Write(activeMenu, selectionIndex);
                keyRead = Console.ReadKey();
                if (keyRead.Key == keyPress.Key)
                {
                    Console.Clear();
                    activeMenu[selectionIndex].Action.Invoke();
                }
                else if (keyRead.Key == keyUp.Key && selectionIndex - 1 >= 0)
                {
                    selectionIndex--;
                }
                else if (keyRead.Key == keyDown.Key && selectionIndex + 1 < activeMenu.Count)
                {
                    selectionIndex++;
                }
                else if (keyRead.Key == keyMainMenu.Key)
                {
                    selectionIndex = 0;
                    activeMenu = mainMenu;
                }
                else if (keyRead.Key == keyBack.Key)
                {
                    GoBack();
                }
                else if (keyRead.Key == keyForward.Key)
                {
                    GoForwards();
                }
            }
            while (keyRead.Key != keyQuit.Key);
        }

        public static void Write(List<Option> activeMenu, int selectionIndex)
        {
            Console.Clear();

            foreach (Option menu in activeMenu)
            {
                if (activeMenu[selectionIndex] == menu)
                {
                    Console.WriteLine(">" + menu.Name);
                }
                else
                {
                    Console.WriteLine(" " + menu.Name);
                }
            }

        }

        public static void SelectSubMenu(List<Option> menu)
        {
            if (menuHistory.Count > 10)
            {
                menuHistory.RemoveAt(10);
            }
            menuHistory.Insert(0, new KeyValuePair<int, List<Option>>(selectionIndex, new List<Option>(activeMenu)));
            //currentHistoryIndex = menuHistory.Count;
            selectionIndex = 0;
            activeMenu = menu;
        }

        public static void GoForwards()
        {
            if (currentHistoryIndex > 0)
            {
                currentHistoryIndex--;
                selectionIndex = menuHistory[currentHistoryIndex].Key;
                activeMenu = menuHistory[currentHistoryIndex].Value;
            }
        }

        public static void GoBack()
        {
            if (currentHistoryIndex < menuHistory.Count - 1)
            {
                currentHistoryIndex++;
                selectionIndex = menuHistory[currentHistoryIndex].Key;
                activeMenu = menuHistory[currentHistoryIndex].Value;
            }
        }
    }

    public class Option
    {
        public string Name { get; }
        public Action Action { get; }

        public Option(string name, Action action)
        {
            Name = name;
            Action = action;
        }
    }
}
