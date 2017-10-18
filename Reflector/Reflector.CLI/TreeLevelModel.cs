using Reflector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflector.CLI
{
    public class TreeLevelModel
    {
        public TreeLevelModel(AssemblyInfo assembly)
        {
            _firstLevel = new TreeLevel(assembly);
            _currentLevel = _firstLevel;
            _sublevel = _currentLevel.Sublevel;
        }

        private TreeLevel _firstLevel { get; set; }
        public TreeLevel _currentLevel { get; set; }
        public Dictionary<string, TreeLevel> _sublevel { get; set; }

        private void ExpandLevel(TreeLevel currentLevel, string node, string previousNode)
        {
           string showableNode = node;
            if (currentLevel.IsExpanded == true && node!=previousNode)
            {
                showableNode = showableNode.TrimEnd(showableNode[showableNode.Length - 1]);
            }
            currentLevel.IsExpanded = true;


            if (currentLevel != _firstLevel)
            {
                for (int i = 0; i < showableNode.Length; i = i + 2)
                {
                    showableNode = showableNode.Insert(i, "|");
                }
            }
            else
            {
                showableNode = null;
            }

            foreach (var n in currentLevel.Sublevel)
            {
                Console.WriteLine($"{showableNode}|{n.Key}| {n.Value.Name}");
            }
        }

        /* private void ShrinkLevel(TreeLevel currentLevel, string node)
         {
             currentLevel.IsExpanded = false;
             if (node.Length > 1)
             {
                 string previousKey = node.TrimEnd(node[node.Length - 1]);
                 ExpandLevel(currentLevel, previousKey);
             }          

           if (currentLevel != _firstLevel)
             {
                 foreach (var s in currentLevel.Sublevel)
                 {
                     ShrinkLevel(s.Value, node);
                 }
             }

         } */

        public void ShowTree(TreeLevel currentLevel, string node, string previousNode, int iterator)
        {
            var sublevel = currentLevel.Sublevel;
            if (currentLevel == _firstLevel && node == "1")
            {
                ExpandLevel(currentLevel, node, previousNode);
            }
            else
            {


                if (iterator > 1)
                {
                    if (currentLevel.IsExpanded)
                    {
                     
                        ExpandLevel(currentLevel, node, previousNode);

                        foreach (var sublevelkey in sublevel.Keys)
                        {
                            currentLevel = sublevel[sublevelkey];
                            if (sublevelkey == node[node.Length - iterator].ToString())
                                ShowTree(currentLevel, node, previousNode, iterator-1);
                        }
                        iterator--;
                    }
                }

                if (iterator == 1)
                {
                    foreach (var finalLevelKey in currentLevel.Sublevel.Keys)
                    {
                        var finalLevel = currentLevel.Sublevel[finalLevelKey];
                        if (finalLevelKey == node[node.Length - 1].ToString())
                        {
                            ExpandLevel(currentLevel, node, previousNode);
                            ExpandLevel(finalLevel, node, previousNode);
                            Console.WriteLine("rozwijam wiersz o numerze " + node);
                        }
                    }
                }
            }
        }
    }
}