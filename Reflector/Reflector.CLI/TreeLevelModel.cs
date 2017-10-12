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
            currentLevel = _firstLevel;
        }

        private TreeLevel _firstLevel { get; set; }
        public TreeLevel currentLevel { get; set; }


        private void ExpandLevel(TreeLevel currentLevel, string currentLevelKey)
        {
            currentLevel.IsExpanded = true;
            foreach (var node in currentLevel.Sublevel)
            {
                Console.WriteLine($"{currentLevelKey}|{node.Key}| {node.Value.Name}");
            }
        }

        private void ShrinkLevel(TreeLevel currentLevel, string currentLevelKey)
        {
            currentLevel.IsExpanded = false;
            if (currentLevelKey.Length > 1)
            {
                string previousKey = currentLevelKey.TrimEnd(currentLevelKey[currentLevelKey.Length - 1]);
                ExpandLevel(currentLevel, previousKey);
            }            
        }

        public void ShowTree(TreeLevel currentLevel, string currentLevelKey)
        {
            if (!currentLevel.IsExpanded)
            {
                if (currentLevel != _firstLevel)
                {
                    for (int i = 0; i <= currentLevelKey.Length; i = i + 2)
                    {
                        currentLevelKey.Insert(i, "/|");
                    }
                }
                else
                {
                    currentLevelKey = null;
                }

                ExpandLevel(currentLevel, currentLevelKey);

            }
            else ShrinkLevel(currentLevel, currentLevelKey);

        }

    }


}
