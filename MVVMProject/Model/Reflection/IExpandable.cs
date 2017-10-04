using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMProject.Model.Reflection
{
    public interface IExpandable
    {
        IEnumerable<IExpandable> Expand();
    }
}
