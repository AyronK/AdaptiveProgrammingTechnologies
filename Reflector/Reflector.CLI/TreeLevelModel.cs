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
        }

        private TreeLevel _firstLevel { get; set; }
        public TreeLevel _currentLevel { get; set; }
                
      
    }
}