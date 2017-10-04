using Microsoft.Win32;
using Reflector.DataAccess;
using Reflector.DataAccess.File;
using Reflector.GUI.Model;
using Reflector.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Reflector.GUI.ViewModel
{
    class FileDialogData : DataContext
    {
        public AssemblyInfo AssemblyInfoData { get; private set; }
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
                var reader = new AssemblyDllReader(LibraryPath);
                AssemblyInfoData = reader.Read();
            }
            else if (LibraryPath.Contains(".xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AssemblyInfo));
                XmlReader reader = new XmlTextReader(LibraryPath);
                AssemblyInfoData = (AssemblyInfo)serializer.Deserialize(reader);
            }
            else
            {
                throw new FormatException("Extention of selected file is not supported");
            }
               
        }

        public override void SerializeToXML(String path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AssemblyInfo));
            XmlWriter writer = new XmlTextWriter(path, Encoding.Unicode);
            serializer.Serialize(writer, AssemblyInfoData);
            writer.Close();
        }

        public override ObservableCollection<TreeViewNode> ToTree()
        {
            ObservableCollection<TreeViewNode> tree = new ObservableCollection<TreeViewNode>();
            foreach (NamespaceModel namespaces in AssemblyInfoData.Namespaces)
            {
                TreeViewNode n = new TreeViewNode(namespaces);
                tree.Add(n);
            }
            return tree;
        }
    }
}
