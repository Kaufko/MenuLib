namespace MenuLib
{
    public static class MenuK
    {
        static List<Menu> activeMenu = new List<Menu>();
        static public int selectionIndex = 0;
        public static void Start(List<Menu> mainMenu)
        {
            activeMenu = mainMenu;
            ConsoleKeyInfo keyRead;
            do
            {
                Write(activeMenu, selectionIndex);
                keyRead = default;
                keyRead = Console.ReadKey();
                if (keyRead.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    activeMenu[selectionIndex].Action.Invoke();
                }
                else if (keyRead.Key == ConsoleKey.UpArrow && selectionIndex - 1 >= 0)
                {
                    selectionIndex--;
                }
                else if (keyRead.Key == ConsoleKey.DownArrow && selectionIndex + 1 < activeMenu.Count)
                {
                    selectionIndex++;
                }
                else if (keyRead.Key == ConsoleKey.Backspace)
                {
                    selectionIndex = 0;
                    activeMenu = mainMenu;
                }
            }
            while (keyRead.Key != ConsoleKey.Escape);
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
            selectionIndex = 0;
            activeMenu = menu;
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
