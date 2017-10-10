using Microsoft.Win32;
using Reflector.GUI.MVVMLight;
using Reflector.Models;
using System;
using System.Windows;
using Reflector.Logic;

namespace Reflector.GUI.ViewModels
{
    public interface IMainViewModel
    {
        AssemblyTreeModel TreeView { get; }

        string NameText { get; set; }

        RelayCommand ReadFileCommand { get; }

        RelayCommand SaveCommand { get; }
    }

    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        #region Constructors
        public MainViewModel(IDataAccessor dataAccessor)
        {
            this.dataAccessor = dataAccessor;
            ReadFileCommand = new RelayCommand(ReadFile);
            SaveCommand = new RelayCommand(SaveToXml);
            _name = string.Empty;
            InitFileDialog();
        }

        private void InitFileDialog()
        {
            fileDialog = new OpenFileDialog();
            fileDialog.FileName = "";
            fileDialog.Filter = "All files (*.*)|*.*|DLL files (.dll)|*.dll|XML files (.xml)|*.xml"; // Filter files by extension
            fileDialog.InitialDirectory = @System.IO.Directory.GetCurrentDirectory();
        }
        #endregion

        #region Privates
        private IDataAccessor dataAccessor;
        private string _name;
        private AssemblyTreeModel _treeview;
        private AssemblyInfo assemblyInfo;
        private OpenFileDialog fileDialog;

        private void ReadFile()
        {
            try
            {
                fileDialog.ShowDialog();                                
                assemblyInfo = dataAccessor.LoadAssembly(fileDialog.FileName);
                NameText = assemblyInfo.Name;
                TreeView = new AssemblyTreeModel(assemblyInfo);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "File read error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(exception.Message);
            }
        }

        private void SaveToXml()
        {
            if (assemblyInfo != null)
            {
                dataAccessor.SaveAssembly(assemblyInfo);
            }
        }
        #endregion

        #region API
        public AssemblyTreeModel TreeView
        {
            get { return _treeview; }
            private set
            {
                _treeview = value;
                RaisePropertyChanged();
            }
        }

        public string NameText
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand ReadFileCommand
        {
            get;
            private set;
        }

        public RelayCommand SaveCommand
        {
            get;
            private set;
        }
        #endregion
    }
}
