using Microsoft.Practices.Unity;
using Reflector.DataAccess;
using Reflector.DataAccess.Dll;
using Reflector.DataAccess.Xml;
using Reflector.Logic;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Reflector.CLI
{
    class MainConsole
    {
        public static class Mef
        {
            public static CompositionContainer Container { get; set; }

            public static void ComposeFromConfigFile()
            {
                var readerDll = ConfigurationManager.AppSettings["AssemblyReader"];
                var writerDll = ConfigurationManager.AppSettings["AssemblyWriter"];
                var dataAccessorDll = ConfigurationManager.AppSettings["DataAccessor"];

                var catalog = new AggregateCatalog();

                var path = Directory.GetCurrentDirectory() + "\\Components";
                var cat = new DirectoryCatalog(path);

                catalog.Catalogs.Add(new FilteredCatalog(cat, def => ContainsMetadata(def, "Name", readerDll)));
                catalog.Catalogs.Add(new FilteredCatalog(cat, def => ContainsMetadata(def, "Name", writerDll)));
                catalog.Catalogs.Add(new FilteredCatalog(cat, def => ContainsMetadata(def, "Name", dataAccessorDll)));

                catalog.Catalogs.Add(new TypeCatalog(typeof(MainInterface)));
                Container = new CompositionContainer(catalog);
            }

            public static void ComposeDefault()
            {                
                var catalog = new AggregateCatalog();
             
                catalog.Catalogs.Add(new TypeCatalog(typeof(AssemblyDllReader)));
                catalog.Catalogs.Add(new TypeCatalog(typeof(AssemblyXmlSerializer)));
                catalog.Catalogs.Add(new TypeCatalog(typeof(DataAccessor)));
                catalog.Catalogs.Add(new TypeCatalog(typeof(MainInterface)));

                Container = new CompositionContainer(catalog);
            }

            private static bool ContainsMetadata(System.ComponentModel.Composition.Primitives.ComposablePartDefinition definition, string name, string value)
            {
                return definition.ExportDefinitions.Any(e => (string)e.Metadata[name] == value);
            }
        }

        static void Main(string[] args)
        {
            //var readerType = ConfigurationManager.AppSettings["AssemblyReader"];
            //var writerType = ConfigurationManager.AppSettings["AssemblyWriter"];

            //IUnityContainer container = new UnityContainer();

            //// Readers
            //try
            //{
            //    switch (readerType)
            //    {
            //        case "DLL":
            //            container.RegisterType<IAssemblyReader, AssemblyDllReader>();
            //            break;
            //        case "XML":
            //            container.RegisterType<IAssemblyReader, AssemblyXmlDeserializer>();
            //            break;
            //        default:
            //            throw new ConfigurationErrorsException("Not supported reader");
            //    }
            //}
            //catch (ConfigurationErrorsException e)
            //{
            //    Log.logger.Error(e, $"Unsuported reader type error: {e.Message}");
            //    Console.Read();
            //    return;
            //}
            //catch (Exception e)
            //{
            //    Log.logger.Error(e, $"Error during registering reader: {e.Message}");
            //    Console.Read();
            //    return;
            //}


            //// Writers
            //try
            //{
            //    switch (writerType)
            //    {
            //        case "XML":
            //            container.RegisterType<IAssemblyWriter, AssemblyXmlSerializer>();
            //            break;
            //        default:
            //            throw new ConfigurationErrorsException("Not supported writer");
            //    }
            //}
            //catch (ConfigurationErrorsException e)
            //{
            //    Log.logger.Error(e, $"Unsuported writer type error: {e.Message}");
            //    Console.Read();
            //    return;
            //}
            //catch (Exception e)
            //{
            //    Log.logger.Error(e, $"Error during registering writer: {e.Message}");
            //    Console.Read();
            //    return;
            //}

            //container.RegisterType<IDataAccessor, DataAccessor>();
            //Log.logger.Info("Application initialized successfully");
            //container.Resolve<MainInterface>().Start();

            try
            {
                Mef.ComposeFromConfigFile();
                Mef.Container.GetExportedValue<MainInterface>().Start();
            }
            catch (Exception e)
            {
                Mef.ComposeDefault();
                Console.WriteLine("Aplication loaded from default configuration...");
                Log.logger.Error(e, $"Failed to initialize application from config file due to error: {e}");
                Log.logger.Warn(e, $"Loading default composition due to error: {e}");
                Mef.Container.GetExportedValue<MainInterface>().Start();
            }
        }
    }

}
