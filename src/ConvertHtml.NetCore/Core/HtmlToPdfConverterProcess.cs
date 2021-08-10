using ConvertHtml.NetCore.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ConvertHtml.NetCore.Core
{
    internal static class HtmlToPdfConverterProcess
    {

        #region Constructors

        public static void ConvertToPdf(string html,
                                        string url,
                                        ICollection<string> urls,
                                        IDictionary<string, string> globalSettings,
                                        IDictionary<string, string> objectSettings)
        {
            Convert(ToConversionSource(html,
                                       url,
                                       urls,
                                       globalSettings,
                                       objectSettings));
        }

        #endregion

        #region Methods

        private static ConversionSource ToConversionSource(string html,
                                                           string url,
                                                           ICollection<string> urls,
                                                           IDictionary<string, string> globalSettings,
                                                           IDictionary<string, string> objectSettings)
        {
            var conversionSource = new ConversionSource(html,
                                                        url,
                                                        urls,
                                                        globalSettings,
                                                        objectSettings);
            return conversionSource;
        }

        private static ProcessStartInfo GetProcessStartInfo(ConversionSource conversionSource)
        {
            return new ProcessStartInfo()
            {
                FileName = conversionSource.WkhtmltoPdf,
                RedirectStandardInput = conversionSource.ProcessOptionRedirectStandardInput,
                RedirectStandardOutput = conversionSource.ProcessOptionRedirectStandardOutput,
                RedirectStandardError = conversionSource.ProcessOptionRedirectStandardError,
                UseShellExecute = conversionSource.ProcessOptionUseShellExecute,
                CreateNoWindow = conversionSource.ProcessOptionCreateNoWindow,
                Arguments = conversionSource.ProcessOptionCommands
            };
        }

        private static void Convert(ConversionSource conversionSource)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(conversionSource.Html);
            var htmlNodesDoc = htmlDoc.DocumentNode.ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "html");
            var bodyNode = htmlNodesDoc?.ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "body");

            if (bodyNode != null
             && (bodyNode.ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "header") != null
             || bodyNode.ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "footer") != null))
                ConvertPartitionedDocument(conversionSource);
            else
                ConvertSingleDocument(conversionSource);

            // start process
            using (var process = new Process())
            {
                ProcessStartInfo startInfo = GetProcessStartInfo(conversionSource);
                process.StartInfo = startInfo;
                process.Start();

                // Preparing input File
                //process.StandardInput.Write(SerializeToBase64EncodedString(conversionSource));
                //process.StandardInput.Close();

                // Preparing output File
                //var output = process.StandardOutput.ReadToEnd();


                if (!process.WaitForExit(conversionSource.ProcessOptionExecutionTimeout))
                    throw new TimeoutException($"wkhtmltopdf tempo de execução decorrido {conversionSource.ProcessOptionExecutionTimeout} ms.");

                // check to make sure the generated file exists and the process didn't error
                if (!File.Exists(conversionSource.GlobalSettings["out"]))
                {
                    if (process.ExitCode != 0)
                    {
                        var error = startInfo.RedirectStandardError ?
                            process.StandardError.ReadToEnd() :
                            $"WkHTMLToPdf exited with code {process.ExitCode}.";
                        throw new InvalidDataException($"WkHTMLToPdf conversion of HTML data failed. Output: \r\n{error}");
                    }

                    throw new InvalidDataException($"WkHTMLToPdf a conversão do HTML falhou. Output file '{conversionSource.GlobalSettings["out"]}' not found.");
                }

            }

        }

        private static void ConvertSingleDocument(ConversionSource conversionSource)
        {
            string temporaryFile = Path.GetDirectoryName(conversionSource.GlobalSettings["in"]);

            if (!Directory.Exists(temporaryFile))
                Directory.CreateDirectory(temporaryFile);

            // Html File
            using (FileStream fs = new FileStream(conversionSource.GlobalSettings["in"], FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(conversionSource.Html);
                }
            }
        }

        private static void ConvertPartitionedDocument(ConversionSource conversionSource)
        {
            string argumments = "";
            string temporaryFolder = Path.GetDirectoryName(conversionSource.GlobalSettings["in"]);
            string temporaryFile = Path.GetFileNameWithoutExtension(conversionSource.GlobalSettings["in"]);

            if (!Directory.Exists(temporaryFolder))
                Directory.CreateDirectory(temporaryFolder);

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(conversionSource.Html);
            var htmlNodesDoc = htmlDoc.DocumentNode.ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "html");
            var bodyNode = htmlNodesDoc?.ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "body");

            #region Header

            if (bodyNode.ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "header") != null)
            {
                HtmlDocument htmlHeaderDoc = new HtmlDocument();
                htmlHeaderDoc.LoadHtml(conversionSource.Html);

                htmlHeaderDoc.DocumentNode.ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "html")?
                                          .ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "body")?
                                          .RemoveAllChildren();

                foreach (var node in bodyNode.ChildNodes.Where(n => n.Name.ToLower() == "header"))
                {
                    htmlHeaderDoc.DocumentNode.ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "html")?
                                              .ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "body")?
                                              .ChildNodes.Add(node);
                }

                using (FileStream fs = new FileStream(Path.Combine(temporaryFolder, temporaryFile + "_header.html"), FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        htmlHeaderDoc.Save(w);
                    }
                }
                
                argumments += " --header-html " + Path.Combine(temporaryFolder, temporaryFile + "_header.html ");
            }

            #endregion

            #region Footer

            if (bodyNode.ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "footer") != null)
            {
                HtmlDocument htmlFooterDoc = new HtmlDocument();
                htmlFooterDoc.LoadHtml(conversionSource.Html);

                htmlFooterDoc.DocumentNode.ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "html")?
                                          .ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "body")?
                                          .RemoveAllChildren();

                foreach (var node in bodyNode.ChildNodes.Where(n => n.Name.ToLower() == "footer"))
                {
                    htmlFooterDoc.DocumentNode.ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "html")?
                                              .ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "body")?
                                              .ChildNodes.Add(node);
                }

                using (FileStream fs = new FileStream(Path.Combine(temporaryFolder, temporaryFile + "_footer.html"), FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        htmlFooterDoc.Save(w);
                    }
                }

                argumments += " --footer-html " + Path.Combine(temporaryFolder, temporaryFile + "_footer.html ");
            }

            #endregion

            #region Content

            HtmlDocument htmlContentDoc = new HtmlDocument();
            htmlContentDoc.LoadHtml(conversionSource.Html);

            htmlContentDoc.DocumentNode.ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "html")?
                                       .ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "body")?
                                       .RemoveAllChildren();

            foreach (var node in bodyNode.ChildNodes.Where(n => n.Name.ToLower() != "header" && n.Name.ToLower() != "footer"))
            {
                htmlContentDoc.DocumentNode.ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "html")?
                                           .ChildNodes.FirstOrDefault(n => n.Name.ToLower() == "body")?
                                           .ChildNodes.Add(node);
            }

            using (FileStream fs = new FileStream(Path.Combine(temporaryFolder, temporaryFile + "_content.html"), FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    htmlContentDoc.Save(w);
                }
            }


            argumments += Path.Combine(temporaryFolder, temporaryFile + "_content.html ");

            #endregion            

            conversionSource.AddHeaderAndFooterArgument(argumments);
        }

        private static string SerializeToBase64EncodedString(ConversionSource conversionSource)
        {
            var result = SerializeToJson(conversionSource);
            return System.Convert.ToBase64String(Encoding.UTF8.GetBytes(result));
        }

        private static string SerializeToJson(ConversionSource conversionSource)
        {
            var inputFile = new InputFile()
            {
                Html = conversionSource.Html,
                GlobalSettings = conversionSource.GlobalSettings,
                ObjectSettings = conversionSource.ObjectSettings
            };

            using (var stream = new MemoryStream())
            {
                new DataContractJsonSerializer(typeof(InputFile)).WriteObject(stream, inputFile);
                stream.Position = 0;
                return new StreamReader(stream).ReadToEnd();
            }
        }

        #endregion

    }
}
