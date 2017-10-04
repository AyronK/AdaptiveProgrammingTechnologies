using Reflector.DataAccess;
using Reflector.GUI.Model;
using Reflector.GUI.MVVMLight;
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
        private DataContext _dataContext;
        private ObservableCollection<TreeViewNode> _treeview;

        private void FetchData()
        {           
            try
            {
                DataContext = FetchDataDelegate();
                LibraryPathText = DataContext.LibraryPath;
                TreeView = DataContext.ToTree();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void SaveToXml()
        {
            //if(DataContext != null)
            //    DataContext.SerializeToXML("DataContext.xml");
        }
        #endregion

        #region API
        public DataContext DataContext
        {
            get
            {
                return _dataContext;
            }

            private set
            {
                _dataContext = value;
            }
        }

        public ObservableCollection<TreeViewNode> TreeView
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

        #region Unit test tools
        /// <summary>
        /// Gets or sets the data fetching delegate.
        /// </summary>
        /// <remarks>
        /// Its purpose is allowing unit tests to override default File Dialog. Unit tests have internal access written to AssemblyInfo to allow them using this solution.
        /// using <see cref="System.Runtime.CompilerServices.InternalsVisibleToAttribute"/>.
        /// </remarks>
        internal Func<DataContext> FetchDataDelegate { get; set; } = (() => new FileDialogData());
        #endregion
    }
}
