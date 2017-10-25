using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reflector.Models;
using Reflector.Presentation;

namespace ReflectorUnitTest.Presentation
{
    [TestClass]
    public class PresentationTest
    {
        private static void ExpandTreeNode(TreeViewNode tree)
        {
            tree.IsExpanded = true;
        }

        private static void ShrinkTreeNode(TreeViewNode tree)
        {
            tree.IsExpanded = false;
        }

        [TestMethod]
        public void ExpandTest()
        {
            IExpandable assembly = new AssemblyInfo(GetType().Assembly);
            TreeViewNode tree = new TreeViewNode(assembly);

            Assert.IsTrue(tree.Sublevels.Count == 0);
            ExpandTreeNode(tree);
            Assert.IsFalse(tree.Sublevels.Count == 0);
        }

        [TestMethod]
        public void DontRebuildOnShrinkTest()
        {
            IExpandable assembly = new AssemblyInfo(GetType().Assembly);
            TreeViewNode tree = new TreeViewNode(assembly);

            ExpandTreeNode(tree);

            var sublevelAfterInitExpand = tree.Sublevels;
            ShrinkTreeNode(tree);

            Assert.AreSame(sublevelAfterInitExpand, tree.Sublevels);
        }

        [TestMethod]
        public void DontRebuildOnExpandAgainTest()
        {
            IExpandable assembly = new AssemblyInfo(GetType().Assembly);
            TreeViewNode tree = new TreeViewNode(assembly);

            ExpandTreeNode(tree);

            var sublevelAfterInitExpand = tree.Sublevels;

            ShrinkTreeNode(tree);
            ExpandTreeNode(tree);

            Assert.AreSame(sublevelAfterInitExpand, tree.Sublevels);
        }
    }
}
