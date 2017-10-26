using Microsoft.Win32;
using Reflector.GUI.MVVMLight;
using Reflector.Models;
using System;
using System.Windows;
using Reflector.Logic;
using Reflector.Presentation;
using System.Collections.Generic;

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
        private AssemblyInfo assemblyInfo;
        private OpenFileDialog fileDialog;

        private void ReadFile()
        {
            try
            {
                fileDialog.ShowDialog();
                assemblyInfo = dataAccessor.LoadAssembly(fileDialog.FileName);
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
                Log.logger.Error(exception, $"Failed to read file {exception.Message}");
            }
        }

        private void SaveToXml()
        {
            if (assemblyInfo != null)
            {
                try
                {
                    dataAccessor.SaveAssembly(assemblyInfo);
                    Log.logger.Info("Assembly saved successfully");
                }
                catch (Exception exception)
                {
                    Log.logger.Error(exception, $"Failed to save assembly :{exception.Message}");
                }
            }
            Log.logger.Warn("User tried to save non existing assembly");
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
