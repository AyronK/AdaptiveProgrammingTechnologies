using Microsoft.Practices.Unity;
using Reflector.DataAccess;
using Reflector.DataAccess.Dll;
using Reflector.DataAccess.Xml;
using Reflector.Logic;
using System;
using System.Configuration;

namespace Reflector.CLI
{
    class MainConsole
    {
        static void Main(string[] args)
        {
            var readerType = ConfigurationManager.AppSettings["AssemblyReader"];
            var writerType = ConfigurationManager.AppSettings["AssemblyWriter"];

            IUnityContainer container = new UnityContainer();

            // Readers
            try
            {
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
            }
            catch (ConfigurationErrorsException e)
            {
                Log.logger.Error(e, $"Unsuported reader type error: {e.Message}");
                throw;
            }
            catch (Exception e)
            {
                Log.logger.Error(e, $"Error during registering reader: {e.Message}");
                throw;
            }


            // Writers
            try
            {
                switch (writerType)
                {
                    case "XML":
                        container.RegisterType<IAssemblyWriter, AssemblyXmlSerializer>();
                        break;
                    default:
                        throw new ConfigurationErrorsException("Not supported writer");
                }
            }
            catch (ConfigurationErrorsException e)
            {
                Log.logger.Error(e, $"Unsuported writer type error: {e.Message}");
                throw;
            }
            catch (Exception e)
            {
                Log.logger.Error(e, $"Error during registering writer: {e.Message}");
                throw;
            }

            container.RegisterType<IDataAccessor, DataAccessor>();
            Log.logger.Info("Application initialized successfully");
            container.Resolve<MainInterface>().Start();

        }
    }
}
