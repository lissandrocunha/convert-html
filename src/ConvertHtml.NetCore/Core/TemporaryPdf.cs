using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConvertHtml.NetCore.Core
{
    public static class TemporaryPdf
    {
        
        #region Methods

        public static string TemporaryFilePath()
        {
            return Path.Combine(Path.GetTempPath(), "Faepa","HtmlToX", TemporaryFilename());
        }

        private static string TemporaryFilename()
        {
            return Guid.NewGuid().ToString("N") + ".pdf";
        }

        public static byte[] ReadTemporaryFileContent(string temporaryFilename)
        {
            using (var temporaryFile = new FileStream(temporaryFilename, FileMode.Open, FileAccess.Read))
            {
                var content = new byte[temporaryFile.Length];

                temporaryFile.Read(content, 0, content.Length);

                return content;
            }
        }

        public static void DeleteTemporaryFile(string temporaryFilename)
        {
            try
            {
                if (File.Exists(temporaryFilename))
                    File.Delete(temporaryFilename);
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        #endregion

    }
}
