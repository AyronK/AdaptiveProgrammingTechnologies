﻿using Reflector.Logic;
using Reflector.Models;
using Reflector.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflector.CLI
{
    [Export]
    public class MainInterface
    {
        //[Import]
        IDataAccessor dataAccessor;

        [ImportingConstructor]
        public MainInterface(IDataAccessor dataAccessor)
        {
            this.dataAccessor = dataAccessor;
        }

        public void Start()
        {
            Log.logger.Info("Application started successfully");
            //Console.WriteLine("Type the name of the file: ");
            //String name = Console.Read().ToString();
            // String name = "Reflector.DataAccess.dll";
            //String path = System.IO.Directory.GetCurrentDirectory() + "\\" + name;
            String path = Directory.GetCurrentDirectory();
            path += "/Reflector.Models.dll";

            AssemblyMetadata assemblyInfo = null;

            try
            {
                assemblyInfo = dataAccessor.LoadAssembly(path);
            }
            catch (Exception e)
            {
                Log.logger.Error(e, $"Failed to load assembly: {e.Message}");
            }

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

                if(usersChoice == "S")
                {
                    if (assemblyInfo != null)
                    {
                        Save(assemblyInfo);
                        Console.Read();
                    }
                    else
                    {
                        Log.logger.Warn("User tried to save non existing assembly");
                    }                    
                }
                else
                {
                    IEnumerable<string> choiseKeys = usersChoice.Split(',').ToList();
                    choices = choiseKeys.Select(c =>
                    {
                        int parseResult;
                        Int32.TryParse(c, out parseResult);
                        return parseResult;
                    });
                }
                Console.Clear();
            }
            while (usersChoice != "Q");
            Log.logger.Info("Application closed by user request");
            Console.Read();
        }

        private void Description()
        {
            Console.WriteLine("\nWrite sequence to expand the node \nPress \"Q\" to break the program\nPress \"S\" to save\n");
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

        private void Save(AssemblyMetadata assemblyInfo)
        {
             try
            {
                dataAccessor.SaveAssembly(assemblyInfo);
                Log.logger.Info("Assembly saved successfully");
            }
            catch (Exception exception)
            {
                Log.logger.Error(exception, $"Failed to save assembly :{exception.Message}");
            }
        }
    }
}