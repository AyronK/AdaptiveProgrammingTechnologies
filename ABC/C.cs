using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveNamespace
{
    public class C
    {
        public A a;
        public B B { get; set; }
        public C getInstance() { return new C(); }
    }
}
