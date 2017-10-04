using Microsoft.Win32;
using Reflactor.DataAccess.Xml;
using Reflector.DataAccess;
using Reflector.DataAccess.Dll;
using Reflector.GUI.Model;
using Reflector.GUI.MVVMLight;
using Reflector.Models;
using System;
using System.Collections.ObjectModel;

namespace Reflector.GUI.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            FetchDataCommand = new RelayCommand(FetchData);
            SaveToXMLCommand = new RelayCommand(SaveToXml);
            _libraryPath = "\\";
        }
        #endregion

        #region Privates
        private IAssemblyReader assemblyReader;
        private IAssemblyWriter assemblyWriter;
        private string _libraryPath;
        private AssemblyViewModel _treeview;
        private AssemblyInfo assemblyInfo;

        private void FetchData()
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
                string path = fileDialog.FileName;
                if (path.Contains(".dll"))
                {
                    assemblyReader = new AssemblyDllReader(fileDialog.FileName);
                }
                else if (path.Contains(".xml"))
                {
                    assemblyReader = new AssemblyXmlDeserializer(fileDialog.FileName);
                }
                assemblyInfo = assemblyReader.Read();
                TreeView = new AssemblyViewModel(assemblyInfo);
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
        public AssemblyViewModel TreeView
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

        public RelayCommand FetchDataCommand
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

        //#region Unit test tools
        ///// <summary>
        ///// Gets or sets the data fetching delegate.
        ///// </summary>
        ///// <remarks>
        ///// Its purpose is allowing unit tests to override default File Dialog. Unit tests have internal access written to AssemblyInfo to allow them using this solution.
        ///// using <see cref="System.Runtime.CompilerServices.InternalsVisibleToAttribute"/>.
        ///// </remarks>
        //internal Func<DataContext> FetchDataDelegate { get; set; } = (() => new FileDialogData());
        //#endregion
    }
}
