using Microsoft.Win32;
using Reflector.GUI.Log;
using Reflector.GUI.MVVMLight;
using Reflector.Logic;
using Reflector.Models;
using Reflector.Presentation;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Reflector.GUI.ViewModels
{
    public interface IMainViewModel
    {
        List<TreeViewNode> TreeView { get; }

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
        private List<TreeViewNode> _treeview = new List<TreeViewNode>();
        private AssemblyMetadata assemblyInfo;
        private OpenFileDialog fileDialog;

        private void ReadFile()
        {
            try
            {
                fileDialog.ShowDialog();
                assemblyInfo = dataAccessor.LoadAssembly("Reflector.Models.dll");
                NameText = assemblyInfo.Name;
                TreeView = new List<TreeViewNode>()
                {
                    new TreeViewNode(assemblyInfo)
                };
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "File read error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(exception.Message);
                Logger.log.Error(exception, $"Failed to read file {exception.Message}");
            }
        }

        private void SaveToXml()
        {
            if (assemblyInfo != null)
            {
                try
                {
                    dataAccessor.SaveAssembly(assemblyInfo);
                    MessageBox.Show("Assembly saved successfully", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    Logger.log.Info("Assembly saved successfully");
                }
                catch (Exception exception)
                {
                    Logger.log.Error(exception, $"Failed to save assembly :{exception.Message}");
                }
            }
            else
            {
                Logger.log.Warn("User tried to save non existing assembly");
            }
        }
        #endregion

        #region API
        public List<TreeViewNode> TreeView
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
