using Microsoft.Practices.Unity;
using Reflector.DataAccess;
using Reflector.DataAccess.Dll;
using Reflector.DataAccess.Xml;
using Reflector.GUI.Log;
using Reflector.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Linq;
using System.Windows;

namespace Reflector.GUI
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

            catalog.Catalogs.Add(new TypeCatalog(typeof(MainWindow)));
            Container = new CompositionContainer(catalog);
        }

        public static void ComposeDefault()
        {
            var catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new TypeCatalog(typeof(AssemblyDllReader)));
            catalog.Catalogs.Add(new TypeCatalog(typeof(AssemblyXmlSerializer)));
            catalog.Catalogs.Add(new TypeCatalog(typeof(DataAccessor)));
            catalog.Catalogs.Add(new TypeCatalog(typeof(MainWindow)));

            Container = new CompositionContainer(catalog);
        }

        private static bool ContainsMetadata(System.ComponentModel.Composition.Primitives.ComposablePartDefinition definition, string name, IEnumerable<string> values)
        {
            foreach (var value in values)
            {
                if (definition.ExportDefinitions.Any(e => (string)e.Metadata[name] == value))
                {
                    return true;
                }
            }
            return false;
        }
    }
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                Mef.ComposeFromConfigFile();
                Mef.Container.GetExportedValue<MainWindow>().Show();
            }
            catch (Exception ex)
            {
                Mef.ComposeDefault();
                MessageBox.Show("Aplication loaded from default configuration...", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                Console.WriteLine("Aplication loaded from default configuration...");
                Logger.log.Error(ex, $"Failed to initialize application from config file due to error: {ex}");
                Logger.log.Warn(ex, $"Loading default composition due to error: {ex}");
                Mef.Container.GetExportedValue<MainWindow>().Show();
            }
        }
    }
}
