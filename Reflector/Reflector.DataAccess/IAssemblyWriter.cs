using Reflector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflector.DataAccess
{
    public interface IAssemblyWriter
    {
        AssemblyInfo Write();
    }
}
