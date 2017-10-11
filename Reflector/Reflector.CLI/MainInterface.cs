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
            if(dataAccessor != null)
            {
                Console.WriteLine("Successfully loaded data accessor");
            }
            else
            {
                Console.WriteLine("Error while loading data accessor");
            }

            Console.Read();

            //Console.WriteLine("Type the name of the file: ");
            //String name = Console.Read().ToString();
            String name = "Reflector.DataAccess.dll";
            String path = System.IO.Directory.GetCurrentDirectory() + "\\" + name;
            AssemblyInfo assemblyInfo = dataAccessor.LoadAssembly(path);

            Console.WriteLine("Write unique code of the node: ");
            string key = Console.ReadLine();
           



            var tree = new TreeLevel(assemblyInfo);

            Console.WriteLine($"o [0] {tree.Name}");
            do
            {
                if (Console.ReadLine() == "0")
                {
                    tree.IsExpanded = true;
                    foreach (var node in tree.Sublevel)
                    {
                        Console.WriteLine($"| o [{node.Key}] {node.Value}");
                    }
                }
            } while (Console.ReadLine() != "1");
        }
    }
}
