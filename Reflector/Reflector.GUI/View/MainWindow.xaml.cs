using Microsoft.Practices.Unity;
using Reflector.GUI.Log;
using Reflector.GUI.ViewModels;
using Reflector.Logic;
using System.ComponentModel.Composition;
using System.Windows;

namespace Reflector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export]
    public partial class MainWindow : Window
    {
        [Import]
        public IDataAccessor DataAccessor
        {
            set { DataContext = new MainViewModel(value); }
        }

        public MainWindow()
        {
            InitializeComponent();
            Logger.log.Info("Main window successfully loaded");
        }
    }
}
