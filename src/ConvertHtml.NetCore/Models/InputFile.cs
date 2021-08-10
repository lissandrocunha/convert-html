using System;
using System.Collections.Generic;
using System.Text;

namespace ConvertHtml.NetCore.Models
{
    [Serializable]
    internal class InputFile
    {
        public string Html { get; set; }
        public IDictionary<string, string> GlobalSettings { get; set; }
        public IDictionary<string, string> ObjectSettings { get; set; }
    }
}
