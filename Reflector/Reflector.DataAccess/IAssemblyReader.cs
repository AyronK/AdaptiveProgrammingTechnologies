﻿using Reflector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflector.DataAccess
{
    public interface IAssemblyReader
    {
        AssemblyMetadata Read(string name);
    }
}
