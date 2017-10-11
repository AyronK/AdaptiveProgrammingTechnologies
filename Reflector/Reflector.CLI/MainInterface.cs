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

            //Console.WriteLine("Type the name of the file: ");
            //String name = Console.Read().ToString();
            String name = "Reflector.DataAccess.dll";
            String path = System.IO.Directory.GetCurrentDirectory() + "\\" + name;
            AssemblyInfo assemblyInfo = dataAccessor.LoadAssembly(path);
            string nodeKey = "1";

            do
            {
                Description();

                while (nodeKey != "Q")
                {
                    var tree = new TreeLevel(assemblyInfo);

                    tree.IsExpanded = true;
                    foreach (var node in tree.Sublevel)
                    {
                        Console.WriteLine($"[{node.Key}] {node.Value.Name}");
                        if (node.Key == nodeKey[0].ToString())
                        {
                            node.Value.IsExpanded = true;
                            foreach (var nd in node.Value.Sublevel)
                            {
                                Console.WriteLine($"[{node.Key}][{nd.Key}] {nd.Value.Name}");
                                string id = node.Key + nd.Key;
                                if (id == nodeKey) 
                                {
                                    nd.Value.IsExpanded = true;
                                    foreach (var nod in nd.Value.Sublevel)
                                    {
                                        Console.WriteLine($"[{node.Key}][{nd.Key}][{nod.Key}] {nod.Value.Name}");
                                    }
                                }
                            }
                        }
                    }
                   // Console.ReadKey();
                    break;
                }

                nodeKey = Console.ReadLine();
                Console.Clear();
            }
            while (nodeKey != "Q");

            Console.WriteLine("Po wyjsciu z petli");
            Console.Read();
        }

        private void Description()
        {
            Console.WriteLine("\nWrite unique code of the node or press \"Q\" to break the program ");
            Console.WriteLine("#######################################################################");
        }
    }
}
