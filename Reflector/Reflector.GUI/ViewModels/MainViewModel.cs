using Microsoft.Win32;
using Reflactor.DataAccess.Xml;
using Reflector.DataAccess;
using Reflector.DataAccess.Dll;
using Reflector.GUI.MVVMLight;
using Reflector.Models;
using System;

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
            _libraryPath = String.Empty;
        }
        #endregion

        #region Privates
        private IAssemblyReader assemblyReader;
        private IAssemblyWriter assemblyWriter;
        private string _libraryPath;
        private AssemblyTreeModel _treeview;
        private AssemblyInfo assemblyInfo;

        private void ReadFile()
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.FileName = ""; // Default file name
                //fileDialog.Filter = "All files (*.*)|*.*|DLL files (.dll)|*.dll|XML files (.xml)|*.xml"; // Filter files by extension
                fileDialog.Filter = "All files (*.*)|*.*|DLL files (.dll)|*.dll"; // Filter files by extension
                fileDialog.InitialDirectory = @System.IO.Directory.GetCurrentDirectory();
                // Show open file dialog box
                Nullable<bool> result = fileDialog.ShowDialog();
                LibraryPathText = fileDialog.FileName;
                if (LibraryPathText.Contains(".dll"))
                {
                    assemblyReader = new AssemblyDllReader(LibraryPathText);
                }
                else if (LibraryPathText.Contains(".xml"))
                {
                    assemblyReader = new AssemblyXmlDeserializer(LibraryPathText);
                }
                assemblyInfo = assemblyReader.Read();
                TreeView = new AssemblyTreeModel(assemblyInfo);
            }
            catch (Exception exception)
            {
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
