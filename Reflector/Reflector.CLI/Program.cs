using Microsoft.Practices.Unity;
using Reflector.DataAccess;
using Reflector.DataAccess.Dll;
using Reflector.DataAccess.Xml;
using Reflector.Logic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflector.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var readerType = ConfigurationManager.AppSettings["AssemblyReader"];
            var writerType = ConfigurationManager.AppSettings["AssemblyWriter"];

            IUnityContainer container = new UnityContainer();

            // Readers
            switch (readerType)
            {
                case "DLL":
                    container.RegisterType<IAssemblyReader, AssemblyDllReader>();
                    break;
                case "XML":
                    container.RegisterType<IAssemblyReader, AssemblyXmlDeserializer>();
                    break;
                default:
                    throw new ConfigurationErrorsException("Not supported reader");
            }

            // Writers
            switch (writerType)
            {
                case "XML":
                    container.RegisterType<IAssemblyWriter, AssemblyXmlSerializer>();
                    break;
                default:
                    throw new ConfigurationErrorsException("Not supported reader");
            }

            container.RegisterType<IDataAccessor, DataAccessor>();
            container.Resolve<MainInterface>().Start();
        }
    }
}
