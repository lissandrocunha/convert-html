using ConvertHtml.NetCore.Core;
using ConvertHtml.NetCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertHtml.NetCore
{
    public sealed class Pdf
    {

        #region Variables



        #endregion

        #region Constructors


        #endregion

        #region Methods

        public static IDocument From(string html)
        {
            return DocumentBuilder.Create(html);
        }

        public static IDocument FromUrl(string url)
        {
            return DocumentBuilder.Create(null,
                                          url);
        }

        public static IDocument FromUrls(ICollection<string> urls)
        {
            return DocumentBuilder.Create(null,
                                          null,
                                          urls);
        }

        #endregion

    }
}
