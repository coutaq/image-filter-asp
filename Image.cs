using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageFilterASP
{
    public class Image
    {
        string type { get; set; }
        string data { get; set; }
        public Image(string data)
        {
            this.data = data;
        }

        public string getData()
        {
            return data;
        }
    }
}
