namespace MenuLib
{
    public static class MenuK
    {
        #region 
        static public List<Menu> activeMenu = new List<Menu>();
        static public int selectionIndex = 0;
        static public int currentHistoryIndex = 10;
        static public List<KeyValuePair<int, Menu>> menuHistory = new List<KeyValuePair<int, Menu>>();
        #endregion

        #region Keys
        static public ConsoleKeyInfo keyPress = ConsoleKey.Enter;
        static public ConsoleKeyInfo keyBack = ConsoleKey.LeftArrow;
        static public ConsoleKeyInfo keyForward = ConoleKey.RightArrow;
        static public ConsoleKeyInfo keyQuit = ConsoleKey.Escape;
        static public ConsoleKeyInfo keyMainMenu = ConsoleKey.Space;
        static public ConsoleKeyInfo keyUp = ConsoleKey.UpArrow;
        static public ConsoleKeyInfo keyDown = ConsolKey.DownArrow;
        #endregion

        public static void Start(List<Menu> mainMenu)
        {
            activeMenu = mainMenu;
            ConsoleKeyInfo keyRead;
            do
            {
                Write(activeMenu, selectionIndex);
                keyRead = default;
                keyRead = Console.ReadKey();
                if (keyRead.Key == keyPress)
                {
                    Console.Clear();
                    activeMenu[selectionIndex].Action.Invoke();
                }
                else if (keyRead.Key == keyUp && selectionIndex - 1 >= 0)
                {
                    selectionIndex--;
                }
                else if (keyRead.Key == keyDown && selectionIndex + 1 < activeMenu.Count)
                {
                    selectionIndex++;
                }
                else if (keyRead.Key == keyMainMenu)
                {
                    selectionIndex = 0;
                    activeMenu = mainMenu;
                }
                else if(keyRead.Key == keyBack)
                { 
                    GoBack()
                }
                else if(keyRead.Key == keyForward)
                {
                    GoForwards()
                }
            }
            while (keyRead.Key != keyQuit);
        }

        public static void Write(List<Menu> activeMenu, int selectionIndex)
        {
            Console.Clear();

            foreach (Menu menu in activeMenu)
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

        public static void SelectSubMenu(List<Menu> menu)
        {
            menuHistory.Remove(10);
            menuHistory.Insert(0, KeyValuePair<selectionIndex, activeMenu>);
            currentHistoryIndex = 10;
            selectionIndex = 0;
            activeMenu = menu;
        }

        public static void GoBack()
        {
            if(currentHistoryIndex > 0)
            {
                currentHistoryIndex--;
                selectionIndex = menuHistory[currentHistoryIndex].Key;
                activeMenu = menuHistory[currentHistoryIndex].Value;
            }
        }

        public static void GoForwards()
        {
            if(currentHistoryIndex < 10)
            {
                currentHistoryIndex++;
                selectionIndex = menuHistory[currentHistoryIndex].Key;
                activeMenu = menuHistory[currentHistoryIndex].Value;
            }
        }
    }

    public class Menu
    {
        public string Name { get; }
        public Action Action { get; }

        public Menu(string name, Action action)
        {
            Name = name;
            Action = action;
        }
    }
}
