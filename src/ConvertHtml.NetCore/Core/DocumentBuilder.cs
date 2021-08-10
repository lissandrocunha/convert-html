using ConvertHtml.NetCore.Interfaces;
using ConvertHtml.NetCore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace ConvertHtml.NetCore.Core
{
    internal sealed class DocumentBuilder : IDocument
    {

        #region Variables

        private static ConversionSource _config;
        private static string _html;
        private static string _url;
        private static ICollection<string> _urls;
        private static IDictionary<string, string> _globalSettings;
        private static IDictionary<string, string> _objectSettings;

        #endregion

        #region Constructors

        private DocumentBuilder(string html,
                                string url,
                                ICollection<string> urls,
                                IDictionary<string, string> globalSettings,
                                IDictionary<string, string> objectSettings)
        {
            _html = html;
            _url = url;
            _urls = urls;
            _globalSettings = globalSettings;
            _objectSettings = objectSettings;
        }

        internal static DocumentBuilder Create(string html,
                                               string url = null,
                                               ICollection<string> urls = null)
        {
            if (_config == null)
                _config = new ConversionSource();

            if (!File.Exists(_config.WkhtmltoPdf))
                throw new ArgumentException($"Arquivo '{_config.WkhtmltoPdf}' não encontrado. Verifique se o aplication wkhtmltox está instalado antes de chamar este método.");

            if (!File.Exists(_config.WkhtmltoImg))
                throw new ArgumentException($"Arquivo '{_config.WkhtmltoImg}' não encontrado. Verifique se o aplication wkhtmltox está instalado antes de chamar este método.");

            return new DocumentBuilder(html,
                                       url,
                                       urls,
                                       new Dictionary<string, string>(),
                                       new Dictionary<string, string>());
        }

        #endregion

        #region Methods

        public IDocument WithGlobalSetting(string key, string value)
        {
            var globalSettings = _globalSettings.ToDictionary(e => e.Key, e => e.Value);

            globalSettings[key] = value;

            return new DocumentBuilder(_html,
                                       _url,
                                       _urls,
                                       globalSettings,
                                       _objectSettings);
        }

        public IDocument WithObjectSetting(string key, string value)
        {
            var objectSetting = _objectSettings.ToDictionary(e => e.Key, e => e.Value);

            objectSetting[key] = value;

            return new DocumentBuilder(_html,
                                       _url,
                                       _urls,
                                       _globalSettings,
                                       objectSetting);
        }

        public byte[] Content()
        {
            return ReadContentUsingTemporaryFile(TemporaryPdf.TemporaryFilePath());
        }

        private byte[] ReadContentUsingTemporaryFile(string temporaryFilename)
        {
            _globalSettings["out"] = temporaryFilename;
            _globalSettings["in"] = temporaryFilename.Replace(".pdf", ".html");

            HtmlToPdfConverterProcess.ConvertToPdf(_html,
                                                   _url,
                                                   _urls,
                                                   _globalSettings,
                                                   _objectSettings);

            var content = TemporaryPdf.ReadTemporaryFileContent(temporaryFilename);

            TemporaryPdf.DeleteTemporaryFile(temporaryFilename);
            TemporaryPdf.DeleteTemporaryFile(temporaryFilename.Replace(".pdf", ".html"));
            TemporaryPdf.DeleteTemporaryFile(temporaryFilename.Replace(".pdf", "_header.html"));
            TemporaryPdf.DeleteTemporaryFile(temporaryFilename.Replace(".pdf", "_content.html"));
            TemporaryPdf.DeleteTemporaryFile(temporaryFilename.Replace(".pdf", "_footer.html"));

            return content;
        }

        public override string ToString()
        {
            return _html;
        }

        #endregion

    }
}
