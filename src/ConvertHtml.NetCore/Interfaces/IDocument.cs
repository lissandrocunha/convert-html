using System;
using System.Collections.Generic;
using System.Text;

namespace ConvertHtml.NetCore.Interfaces
{
    public interface IDocument
    {
        IDocument WithGlobalSetting(string key, string value);
        IDocument WithObjectSetting(string key, string value);
        byte[] Content();
    }
}
