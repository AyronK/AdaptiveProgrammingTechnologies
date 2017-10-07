using Reflector.Logic;
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
        }
    }
}
