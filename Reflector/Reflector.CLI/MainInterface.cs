using Reflector.Logic;
using Reflector.Models;
using Reflector.Presentation;
using System;
using System.Collections.Generic;
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
            if (dataAccessor != null)
            {
                Console.WriteLine("Successfully loaded data accessor");
            }
            else
            {
                Console.WriteLine("Error while loading data accessor");
            }

            //Console.WriteLine("Type the name of the file: ");
            //String name = Console.Read().ToString();
            // String name = "Reflector.DataAccess.dll";
            //String path = System.IO.Directory.GetCurrentDirectory() + "\\" + name;
            String path = "C://Users//Beata//Desktop//sem V//TPA//Reflector//RecursiveLibrary//bin//Debug//ABC.dll";
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
                    ExpandLevel(tree, choices);
                    DisplayLevel(tree);
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.Write("Klucz, klucz[...]: ");
                usersChoice = Console.ReadLine();

                var choiseKeys = usersChoice.Split(',').ToList();                
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
            Console.WriteLine("\nWrite informations about the node or press \"Q\" to break the program\n");
            Console.WriteLine("#####################################################################\n");
        }


        public static void ExpandLevel(TreeViewNode tree, IEnumerable<int> choices)
        {
            TreeViewNode currentLevel = tree;            
           foreach(var choise in choices)
            {                
                if (!currentLevel.IsExpanded)
                {
                    throw new IndexOutOfRangeException("Cannot expand that far. Submit nodes in correct order.");
                } else if (choise > choices.Count() || choise < 0)
                {
                    throw new IndexOutOfRangeException("Node does not exist.");
                }
                currentLevel = currentLevel.Sublevel[choise];
                                
            }
            currentLevel.IsExpanded = true;
        }

        public static void DisplayLevel(TreeViewNode treeLevel, int iterator = 0)
        {
            Console.WriteLine(treeLevel.Name);
            if (treeLevel.IsExpanded)
            {
                foreach (var child in treeLevel.Sublevel)
                {
                    for (int i = 0; i < iterator; i++)
                    {
                        Console.Write($" ");
                    }
                    Console.Write($"[{treeLevel.Sublevel.IndexOf(child)}] ");
                    DisplayLevel(child, iterator + 1);
                }
            }
        }
    }

}