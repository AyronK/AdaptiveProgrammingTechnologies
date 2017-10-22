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

            string usersChoice = null;
            List<string> choices = new List<string>();

            TreeLevelModel tree = new TreeLevelModel(assemblyInfo);
            tree._currentLevel.IsExpanded = true;
         

            do
            {
                Description();
                
                //tree.ShowTree(tree._currentLevel, choices, iterator);
                //{
                ExpandLevel(tree, choices);
                DisplayLevel(tree._currentLevel);
                Console.Write("Klucz, klucz[...]: ");
                choices.Clear();
                usersChoice = Console.ReadLine();
                choices = usersChoice.Split(',').ToList();
                Console.Clear();
            }
            while (usersChoice != "Q");
            Console.WriteLine("Po wyjsciu z petli");
            Console.Read();
        }

        private void Description()
        {
            Console.WriteLine("\nWrite informations about the node or press \"Q\" to break the program\n");
            Console.WriteLine("#####################################################################\n");
        }


        public static void ExpandLevel(TreeLevelModel tree, List<string> choices)
        {
            TreeLevel currentLevel = tree._currentLevel;

            for (int choiceIndex = 0; choiceIndex < choices.Count; choiceIndex++)
            {
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