namespace MenuLib
{
    public class MenuK
    {
        List<Menu> activeMenu = new List<Menu>();
        int selectionIndex = 0;
        void Start(List<Menu> mainMenu)
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

        void Write(List<Menu> activeMenu, int selectionIndex)
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

        static void CwControls()
        {
            Console.Clear();
            Console.WriteLine("Zmáčkni klávesu Enter pro spuštění vybrané položky");
            Console.WriteLine("Zmáčkni šipku dolů a nahorů pro vybýrání položek");
            Console.WriteLine("Zmáčkni Backspace pro návrat do hlavního menu");
            Console.WriteLine("Zmáčkni Escape pro ukončení aplikace");
            Console.WriteLine("\nZmáčkni jakoukoli klávesu pro pokračování");
            Console.ReadKey();
        }

        void SelectSubMenu(List<Menu> menu)
        {
            selectionIndex = 0;
            activeMenu = menu;
        }
    }

    public class Menu
    {
        public string Name { get; }
        public bool WaitForKey { get; }
        public Action Action { get; }

        public Menu(string name, bool waitForKey, Action action)
        {
            Name = name;
            WaitForKey = waitForKey;
            Action = action;
        }
    }
}
