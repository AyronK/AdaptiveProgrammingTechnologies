using Reflector.Logic;
using Reflector.Models;
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
            List<string> choices = new List<string>();

            TreeLevel tree = new TreeLevel(assemblyInfo);
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
                choices.Clear();
                usersChoice = Console.ReadLine();
                choices = usersChoice.Split(',').ToList();
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


        public static void ExpandLevel(TreeLevel tree, List<string> choices)
        {
            TreeLevel currentLevel = tree;            
            for (int choiceIndex = 0; choiceIndex < choices.Count; choiceIndex++)
            {
                if (!currentLevel.IsExpanded)
                {
                    throw new IndexOutOfRangeException("Cannot expand that far. Submit nodes in correct order.");
                }
                currentLevel = currentLevel.Sublevel[choices[choiceIndex]];
                                
            }
            currentLevel.IsExpanded = true;
        }

        public static void DisplayLevel(TreeLevel treeLevel, int iterator = 0)
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
                    Console.Write($"[{child.Key}] ");
                    DisplayLevel(child.Value, iterator + 1);
                }
            }
        }
    }

}