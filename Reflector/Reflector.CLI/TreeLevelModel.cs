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


        private void ExpandLevel(TreeLevel currentLevel, string node)
        {
            currentLevel.IsExpanded = true;


            if (currentLevel != _firstLevel)
            {
                for (int i = 0; i <= node.Length; i = i + 2)
                {
                    node = node.Insert(i, "|");
                }
            }
            else
            {
                node = null;
            }

            foreach (var n in currentLevel.Sublevel)
            {
                Console.WriteLine($"{node}|{n.Key}| {n.Value.Name}");
            }
        }

        private void ShrinkLevel(TreeLevel currentLevel, string node)
        {
            currentLevel.IsExpanded = false;
            if (node.Length > 1)
            {
                string previousKey = node.TrimEnd(node[node.Length - 1]);
                ExpandLevel(currentLevel, previousKey);
            }          
        }

        public void ShowTree(TreeLevel currentLevel, string node, int iterator)
        {
            if(iterator>1)
            {
                if (currentLevel.IsExpanded)
                {
                    //0 poziom jest, każdy kolejny expand + wyswietl w tej metodzie
                    ExpandLevel(currentLevel, node);

                    foreach (var sublevelkey in _sublevel.Keys)
                    {
                        currentLevel = _sublevel[sublevelkey];
                        if(sublevelkey == node)
                      //  currentLevel.IsExpanded = true;
                        ShowTree(currentLevel, node, iterator);

                    }

                }
                iterator--;
            }

               


                if (iterator == 1)
                {

                    //ostatnie przejscie, porównać node z kluczem sublevelu i tylko ten expandnąć
                    foreach(var finalLevelKey in currentLevel.Sublevel.Keys)
                    {
                        var finalLevel = currentLevel.Sublevel[finalLevelKey];
                        if (finalLevelKey == node[node.Length-1].ToString())
                        {
                            if (finalLevel.IsExpanded == true)
                            {
                                ShrinkLevel(finalLevel, node);
                                Console.WriteLine("chowam wiersz o numerze " + node);
                            }
                            else
                            {
                                ExpandLevel(finalLevel, node);
                                Console.WriteLine("rozwijam wiersz o numerze " + node);
                            }
                        } 
                    }
                }

            }

    }


}
