using Microsoft.Practices.Unity;
using Reflector.GUI;
using Reflector.GUI.ViewModels;
using Reflector.Logic;
using System.Windows;

namespace Reflector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [Dependency]
        public IDataAccessor DataAccessor
        {
            set { DataContext = new MainViewModel(value); }
        }

        public MainWindow()
        {
            InitializeComponent();
            Log.logger.Info("Main window successfully loaded");
        }
    }
}
