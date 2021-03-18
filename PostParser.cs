using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageFilterASP
{
    public class PostParser : IParser
    {
        ILogger _logger;
        PostParser(ILogger logger)
        {
            _logger = logger;
        }

        ILogger IParser._logger { set => throw new NotImplementedException(); }

        void Parse()
        {

        }
    }
}
