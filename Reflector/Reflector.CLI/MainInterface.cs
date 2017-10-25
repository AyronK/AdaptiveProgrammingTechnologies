using Reflector.Logic;
using Reflector.Models;
using Reflector.Presentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflector.CLI
{
    public class MainInterface
    {
        IDataAccessor dataAccessor;
        public MainInterface(IDataAccessor dataAccessor)
        {
            this.dataAccessor = dataAccessor;
        }

        public void Start()
        {
            //Console.WriteLine("Type the name of the file: ");
            //String name = Console.Read().ToString();
            // String name = "Reflector.DataAccess.dll";
            //String path = System.IO.Directory.GetCurrentDirectory() + "\\" + name;
            String path = Directory.GetCurrentDirectory();
            path += "/Reflector.Models.dll";
            AssemblyInfo assemblyInfo = dataAccessor.LoadAssembly(path);

            string usersChoice = String.Empty;
            IEnumerable<int> choices = Enumerable.Empty<int>();

            TreeViewNode tree = new TreeViewNode(assemblyInfo);
            tree.IsExpanded = true;

            do
            {
                Description();
                try
                {
                    ToogleLevel(tree, choices);
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                    Console.WriteLine();
                }
                DisplayLevel(tree);
                usersChoice = Console.ReadLine();

                IEnumerable<string> choiseKeys = usersChoice.Split(',').ToList();
                choices = choiseKeys.Select(c =>
                {
                    int parseResult;
                    Int32.TryParse(c, out parseResult);
                    return parseResult;
                }
                );

                Console.Clear();
            }
            while (usersChoice != "Q");
            Console.Read();
        }

        private void Description()
        {
            Console.WriteLine("\nWrite sequence to expand the node or press \"Q\" to break the program\n");
            Console.WriteLine("Sequence format: key,key,key [...]\n");
            Console.WriteLine("#####################################################################\n");
        }


        public static void ToogleLevel(TreeViewNode tree, IEnumerable<int> choices)
        {
            if (choices.Count() <= 0)
            {
                return;
            }

            TreeViewNode currentLevel = tree;
            foreach (var choise in choices)
            {
                if (!currentLevel.IsExpanded)
                {
                    throw new IndexOutOfRangeException("Cannot expand that far. Submit nodes in correct order.");
                }
                else if (choise >= currentLevel.Sublevels.Count() || choise < 0)
                {
                    throw new IndexOutOfRangeException("Node does not exist.");
                }
                currentLevel = currentLevel.Sublevels[choise];
            }

            currentLevel.IsExpanded = !currentLevel.IsExpanded;
        }

        public static void DisplayLevel(TreeViewNode treeLevel, int iterator = 0)
        {
            Console.WriteLine(treeLevel.Name);
            if (treeLevel.IsExpanded)
            {
                foreach (var child in treeLevel.Sublevels)
                {
                    for (int i = 0; i < iterator; i++)
                    {
                        Console.Write($" ");
                    }
                    Console.Write($"[{treeLevel.Sublevels.IndexOf(child)}] ");
                    DisplayLevel(child, iterator + 1);
                }
            }
        }
    }

}