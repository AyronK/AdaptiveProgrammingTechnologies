using Microsoft.Practices.Unity;
using Reflector.DataAccess;
using Reflector.DataAccess.Dll;
using Reflector.DataAccess.Xml;
using Reflector.Logic;
using System;
using System.Configuration;
using System.Reflection;
using System.Windows;

namespace Reflector.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //For now only magic string config - TODO MEF

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
            catch (ConfigurationErrorsException configExc)
            {
                Log.logger.Error(configExc, $"Unsuported reader type error: {configExc.Message}");
                MessageBox.Show(configExc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }
            catch (Exception exc)
            {
                Log.logger.Error(exc, $"Error during registering reader: {exc.Message}");
                MessageBox.Show(exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
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
            catch (ConfigurationErrorsException configExc)
            {
                Log.logger.Error(configExc, $"Unsuported writer type error: {configExc.Message}");
                MessageBox.Show(configExc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }
            catch (Exception exc)
            {
                Log.logger.Error(exc, $"Error during registering writer: {exc.Message}");
                MessageBox.Show(exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }

            container.RegisterType<IDataAccessor, DataAccessor>();
            Log.logger.Info("Application initialized successfully");
            container.Resolve<MainWindow>().Show();
        }
    }
}
