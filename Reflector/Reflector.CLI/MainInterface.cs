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
            string nodeKey = "1", previousNode = "-1";
            
            TreeLevelModel tree = new TreeLevelModel(assemblyInfo);
            tree._currentLevel.IsExpanded = true;
          

            do
            {
                Description();

                if(previousNode!=nodeKey) tree.ShowTree(tree._currentLevel, nodeKey, previousNode, nodeKey.Length);

                nodeKey = Console.ReadLine();
                Console.Clear();
            }
            while (nodeKey != "Q");
            previousNode = nodeKey;
            Console.WriteLine("Po wyjsciu z petli");
            Console.Read();
        }

        private void Description()
        {
            Console.WriteLine("\nWrite unique code of the node\n" +
                                "Press \"Q\" to break the program\n");
            Console.WriteLine("#################################################################\n");
        }
    }

}