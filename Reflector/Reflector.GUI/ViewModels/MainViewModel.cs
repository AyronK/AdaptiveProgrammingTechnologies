using Microsoft.Win32;
using Reflactor.DataAccess.Xml;
using Reflector.DataAccess;
using Reflector.DataAccess.Dll;
using Reflector.GUI.MVVMLight;
using Reflector.Models;
using System;
using System.Windows;

namespace Reflector.GUI.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ReadFileCommand = new RelayCommand(ReadFile);
            SaveToXMLCommand = new RelayCommand(SaveToXml);
            _libraryPath = string.Empty;
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
        private IAssemblyReader assemblyReader;
        private IAssemblyWriter assemblyWriter;
        private string _libraryPath;
        private AssemblyTreeModel _treeview;
        private AssemblyInfo assemblyInfo;
        private OpenFileDialog fileDialog;

        private void ReadFile()
        {
            try
            {
                fileDialog.ShowDialog();
                LibraryPathText = fileDialog.FileName;

                if (LibraryPathText.Contains(".dll"))
                {
                    assemblyReader = new AssemblyDllReader(LibraryPathText);
                }
                else if (LibraryPathText.Contains(".xml"))
                {
                    assemblyReader = new AssemblyXmlDeserializer(LibraryPathText);
                }
                else
                {
                    throw new Exception("Incorrect file extension");
                }
                assemblyInfo = assemblyReader.Read();
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
            assemblyWriter = new AssemblyXmlSerializer();
            assemblyWriter.Write(assemblyInfo);
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

        public string LibraryPathText
        {
            get { return _libraryPath; }
            set
            {
                _libraryPath = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand ReadFileCommand
        {
            get;
            private set;
        }

        public RelayCommand SaveToXMLCommand
        {
            get;
            private set;
        }
        #endregion
    }
}
