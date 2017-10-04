using Microsoft.Win32;
using MVVMProject.Model;
using MVVMProject.MVVMLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MVVMProject.ViewModel
{
    class FileDialogData : DataContext
    {
        public AssemblyModel AssemblyModelData { get; private set; }
        OpenFileDialog fileDialog;

        public FileDialogData()
        {
            fileDialog = new OpenFileDialog();
            fileDialog.FileName = ""; // Default file name
            fileDialog.Filter = "All files (*.*)|*.*|DLL files (.dll)|*.dll|XML files (.xml)|*.xml"; // Filter files by extension
            fileDialog.InitialDirectory = @System.IO.Directory.GetCurrentDirectory();
            // Show open file dialog box
            Nullable<bool> result = fileDialog.ShowDialog();

            LibraryPath = fileDialog.FileName;

            if (LibraryPath.Contains(".dll"))
            {
                AssemblyModelData = new AssemblyModel(LibraryPath);
            }
            else if (LibraryPath.Contains(".xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AssemblyModel));
                XmlReader reader = new XmlTextReader(LibraryPath);
                AssemblyModelData = (AssemblyModel)serializer.Deserialize(reader);
            }
            else
            {
                throw new FormatException("Extention of selected file is not supported");
            }
               
        }

        public override void SerializeToXML(String path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AssemblyModel));
            XmlWriter writer = new XmlTextWriter(path, Encoding.Unicode);
            serializer.Serialize(writer, AssemblyModelData);
            writer.Close();
        }

        public override ObservableCollection<TreeViewNode> ToTree()
        {
            ObservableCollection<TreeViewNode> tree = new ObservableCollection<TreeViewNode>();
            foreach (NamespaceModel namespaces in AssemblyModelData.Namespaces)
            {
                TreeViewNode n = new TreeViewNode(namespaces);
                tree.Add(n);
            }
            return tree;
        }
    }
}
