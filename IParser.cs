﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageFilterASP
{
    interface IParser
    { 
        ILogger _logger {set;}
        void Parse(string data)
        {

        }
    }
}
