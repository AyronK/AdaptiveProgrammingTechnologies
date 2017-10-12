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
            String name = "Reflector.DataAccess.dll";
            String path = System.IO.Directory.GetCurrentDirectory() + "\\" + name;
            AssemblyInfo assemblyInfo = dataAccessor.LoadAssembly(path);
            string nodeKey = "1";
            
            TreeLevelModel tree = new TreeLevelModel(assemblyInfo);
            tree._currentLevel.IsExpanded = true;

            do
            {
                Description();

                while (nodeKey != "Q")
                {
                    tree.ShowTree(tree._currentLevel, nodeKey, nodeKey.Length);
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
            Console.WriteLine("\nWrite unique code of the node or press \"Q\" to break the program");
            Console.WriteLine("#################################################################\n");
        }
    }

}