using Microsoft.Practices.Unity;
using Reflector.CLI.Log;
using Reflector.DataAccess;
using Reflector.DataAccess.Dll;
using Reflector.DataAccess.Xml;
using Reflector.Logic;
using System;
using System.Collections.Generic;
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
                var readersDll = ConfigurationManager.AppSettings["AssemblyReaders"].Split(',').Select(p => p.Trim()).ToList();
                var writersDll = ConfigurationManager.AppSettings["AssemblyWriters"].Split(',').Select(p => p.Trim()).ToList();
                var dataAccessorsDll = ConfigurationManager.AppSettings["DataAccessors"].Split(',').Select(p => p.Trim()).ToList();

                var catalog = new AggregateCatalog();

                var path = ConfigurationManager.AppSettings["ComponentsDirectoryPath"];
                var cat = new DirectoryCatalog(path);

                catalog.Catalogs.Add(new FilteredCatalog(cat, def => ContainsMetadata(def, "Name", readersDll)));
                catalog.Catalogs.Add(new FilteredCatalog(cat, def => ContainsMetadata(def, "Name", writersDll)));
                catalog.Catalogs.Add(new FilteredCatalog(cat, def => ContainsMetadata(def, "Name", dataAccessorsDll)));

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

            private static bool ContainsMetadata(System.ComponentModel.Composition.Primitives.ComposablePartDefinition definition, string name, IEnumerable<string> values)
            {
                foreach(var value in values)
                {
                    if (definition.ExportDefinitions.Any(e => (string)e.Metadata[name] == value))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        static void Main(string[] args)
        {
            try
            {
                Mef.ComposeFromConfigFile();
                Mef.Container.GetExportedValue<MainInterface>().Start();
            }
            catch (Exception e)
            {
                Mef.ComposeDefault();
                Console.WriteLine("Aplication loaded from default configuration...");
                Logger.log.Error(e, $"Failed to initialize application from config file due to error: {e}");
                Logger.log.Warn(e, $"Loading default composition due to error: {e}");
                Mef.Container.GetExportedValue<MainInterface>().Start();
            }
        }
    }

}
