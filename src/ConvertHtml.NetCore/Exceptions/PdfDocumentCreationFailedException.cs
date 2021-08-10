using System;
using System.Collections.Generic;
using System.Text;

namespace ConvertHtml.NetCore.Exceptions
{
    internal sealed class PdfDocumentCreationFailedException : Exception
    {
        public PdfDocumentCreationFailedException(string error)
            : base(error)
        {
        }
    }
}
