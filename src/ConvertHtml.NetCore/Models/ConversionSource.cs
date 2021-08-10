using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ConvertHtml.NetCore.Models
{
    internal sealed class ConversionSource
    {

        #region Variables

        private readonly string _html;
        private readonly string _url;
        private readonly ICollection<string> _urls;
        private readonly IDictionary<string, string> _globalSettings;
        private readonly IDictionary<string, string> _objectSettings;

        #endregion

        #region Properties

        internal string WkhtmltoxPath => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "wkhtmltox");
        internal string WkhtmltoPdf => Path.Combine(WkhtmltoxPath, "wkhtmltopdf.exe");
        internal string WkhtmltoImg => Path.Combine(WkhtmltoxPath, "wkhtmltoimage.exe");

        public bool ProcessOptionRedirectStandardInput { get; internal set; } = true;
        public bool ProcessOptionRedirectStandardOutput { get; internal set; } = true;
        public bool ProcessOptionRedirectStandardError { get; internal set; } = true;
        public bool ProcessOptionUseShellExecute { get; internal set; } = false;
        public bool ProcessOptionCreateNoWindow { get; internal set; } = true;
        public int ProcessOptionExecutionTimeout { get; internal set; } = 300000;
        public string ProcessOptionCommands { get => BuildOptions(); }

        public string Html { get => _html; }
        public string Url { get => _url; }
        public ICollection<string> Urls { get => _urls; }
        public IDictionary<string, string> GlobalSettings { get => _globalSettings; }
        public IDictionary<string, string> ObjectSettings { get => _objectSettings; }

        #endregion

        #region Constructors

        public ConversionSource() { }

        public ConversionSource(string html,
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

        #endregion

        #region Methods

        internal void AddHeaderAndFooterArgument(string args)
        {
            _globalSettings.Add("internal-args", args);
        }

        private string BuildOptions()
        {
            StringBuilder options = new StringBuilder();

            #region Global Options

            if (_globalSettings != null
             && _globalSettings.Count > 0)
            {
                if (_globalSettings.ContainsKey("documentTitle")) options.AppendFormat("--title \"{0}\" ", _globalSettings["documentTitle"].Replace("\"", ""));
                if (_globalSettings.ContainsKey("orientation")) options.AppendFormat("--orientation {0} ", _globalSettings["orientation"]);                
                if (_globalSettings.ContainsKey("size.width")) options.AppendFormat("--page-width {0} ", _globalSettings["size.width"]);
                if (_globalSettings.ContainsKey("size.height")) options.AppendFormat("--page-height {0} ", _globalSettings["size.height"]);
                if (_globalSettings.ContainsKey("web.defaultEncoding")) options.AppendFormat("--encoding {0} ", _globalSettings["web.defaultEncoding"]);

                if (_globalSettings.ContainsKey("grayscale") && _globalSettings["grayscale"] == "true") options.Append("--grayscale ");
                if (_globalSettings.ContainsKey("lowquality") && _globalSettings["lowquality"] == "true") options.Append("--lowquality ");
                if (_globalSettings.ContainsKey("dpi")) options.AppendFormat("--dpi {0} ", _globalSettings["dpi"]);

                if (!_globalSettings.ContainsKey("useCompression") || _globalSettings["useCompression"] == "false") options.Append("--no-pdf-compression ");
                if (_globalSettings.ContainsKey("copies") && int.Parse(_globalSettings["copies"]) > 1) options.AppendFormat("--copies {0} ", _globalSettings["copies"]);
                if (_globalSettings.ContainsKey("zoom") && int.Parse(_globalSettings["zoom"]) > 1) options.AppendFormat("--zoom {0} ", _globalSettings["zoom"]);

                if (_globalSettings.ContainsKey("outline") && _globalSettings["outline"] == "true") options.Append("--outline ");
                if (_globalSettings.ContainsKey("outline") && _globalSettings["outline"] == "false") options.Append("--no-outline ");                

                if (_globalSettings.ContainsKey("smart.shrinking") && _globalSettings["smart.shrinking"] == "true") options.Append("--enable-smart-shrinking ");
                if (_globalSettings.ContainsKey("smart.shrinking") && _globalSettings["smart.shrinking"] == "false") options.Append("--disable-smart-shrinking ");

                if (_globalSettings.ContainsKey("margin.top") && !string.IsNullOrWhiteSpace(_globalSettings["margin.top"])) options.AppendFormat("--margin-top {0} ", _globalSettings["margin.top"]);
                if (_globalSettings.ContainsKey("margin.bottom") && !string.IsNullOrWhiteSpace(_globalSettings["margin.bottom"])) options.AppendFormat("--margin-bottom {0} ", _globalSettings["margin.bottom"]);
                if (_globalSettings.ContainsKey("margin.left") && !string.IsNullOrWhiteSpace(_globalSettings["margin.left"])) options.AppendFormat("--margin-left {0} ", _globalSettings["margin.left"]);
                if (_globalSettings.ContainsKey("margin.right") && !string.IsNullOrWhiteSpace(_globalSettings["margin.right"])) options.AppendFormat("--margin-right {0} ", _globalSettings["margin.right"]);

                if (_globalSettings.ContainsKey("header.spacing") && int.Parse(_globalSettings["header.spacing"]) > 0) options.AppendFormat("--header-spacing {0} ", _globalSettings["header.spacing"]);
                if (_globalSettings.ContainsKey("header.custom") && !string.IsNullOrWhiteSpace(_globalSettings["header.custom"])) options.AppendFormat("{0} ", _globalSettings["header.custom"]);
                if (_globalSettings.ContainsKey("footer.spacing") && int.Parse(_globalSettings["footer.spacing"]) > 0) options.AppendFormat("--footer-spacing {0} ", _globalSettings["footer.spacing"]);
                if (_globalSettings.ContainsKey("footer.custom") && !string.IsNullOrWhiteSpace(_globalSettings["footer.custom"])) options.AppendFormat("{0} ", _globalSettings["footer.custom"]);

            }
            else
            {
                options.Append(" ");
            }

            #endregion

            #region Object Options

            if (_objectSettings != null
             && _objectSettings.Count > 0)
            {

            }

            #endregion

            // Set Input and Output
            if (_globalSettings.ContainsKey("internal-args"))
                options.AppendFormat("{0} {1}", _globalSettings["internal-args"], _globalSettings["out"]);
            else
                options.AppendFormat("{0} {1}", _globalSettings["in"], _globalSettings["out"]);

            return options.ToString();
        }


        #endregion

    }
}
