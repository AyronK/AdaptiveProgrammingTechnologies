using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using MVVMProject.ViewModel;
using MVVMProject.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MVVMProject.UnitTest
{
    [TestClass]
    public class MainViewModelUnitTest
    {
        [TestMethod]
        public void CreatorTestMethod()
        {
            MainViewModel vm = new MainViewModel();
            Assert.IsNotNull(vm.FetchDataCommand);
            Assert.IsNull(vm.TreeView);
        }        

        [TestMethod]
        public void FetchDataCommandMethod()
        {
            MainViewModel vm = new MainViewModel();
            MockFetchDataCommand dataContext = new MockFetchDataCommand();
            vm.FetchDataDelegate = delegate { return dataContext; };
            Assert.IsTrue(vm.FetchDataCommand.CanExecute(null));
            
            Assert.IsNull(vm.TreeView);

            vm.FetchDataCommand.Execute(null);
            
            Assert.AreEqual("Test Path", vm.LibraryPathText);
            Assert.IsNotNull(vm.TreeView);
            Assert.AreSame(dataContext, vm.DataContext);
            Assert.AreEqual(dataContext.ToTree().Count, vm.TreeView.Count);
        }                
    }

    class MockFetchDataCommand : DataContext
    {
        public MockFetchDataCommand()
        {
            LibraryPath = "Test Path";
        }

        public override void SerializeToXML(string path)
        {
            Debug.Print("Serialized!");
        }

        public override ObservableCollection<TreeViewNode> ToTree()
        {
            ObservableCollection<TreeViewNode> tree = new ObservableCollection<TreeViewNode>();

            return tree;
        }
    }
}
