using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflector.GUI.Log
{
    class Logger
    {
        public static NLog.Logger log = LogManager.GetCurrentClassLogger();
    }
}
