using ConvertHtml.NetCore.Exceptions;
using ConvertHtml.NetCore.Models;
using ConvertHtml.NetCore.Test.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ConvertHtml.NetCore.Test
{
    public class UnitTest1
    {

        #region Variables

        private const string _htmlDocumentFormat = "<!DOCTYPE html><html><head><meta charset='UTF-8'><title>Title</title></head><body>{0}</body></html>";
        private const int _210mmInPostScriptPoints = 595;// 595,276
        private const int _297mmInPostScriptPoints = 842;// 841,89

        #endregion

        #region Methods

        [Fact(DisplayName = "Conteúdo do Documento")]
        [Trait("Category", "PDF")]
        public void Pdf_document_content()
        {
            // Arrange
            string expect = "Expected document content";

            // Act
            var html = string.Format(_htmlDocumentFormat, expect);

            var result = Pdf.From(html)
                            .Content();

            TextAssert.AreEqual(expect, PdfDocumentReader.ToText(result));
        }

        [Fact(DisplayName = "Conteúdo do Documento com Header")]
        [Trait("Category", "PDF")]
        public void Pdf_document_header_content()
        {
            // Arrange
            string expect = "Expected document content" + System.Environment.NewLine + "Header of Document";

            // Act
            var html = string.Format(_htmlDocumentFormat, "<header><div>Header of Document</div></header>Expected document content");

            var result = Pdf.From(html)
                            .Content();

            TextAssert.AreEqual(expect, PdfDocumentReader.ToText(result));
        }

        [Fact(DisplayName = "Conteúdo do Documento com Footer")]
        [Trait("Category", "PDF")]
        public void Pdf_document_footer_content()
        {
            // Arrange
            string expect = "Expected document content" + System.Environment.NewLine + "Footer of Document";

            // Act
            var html = string.Format(_htmlDocumentFormat, "Expected document content<footer><div>Footer of Document</div></footer>");

            var result = Pdf.From(html)
                            .Content();

            TextAssert.AreEqual(expect, PdfDocumentReader.ToText(result));
        }

        [Fact(DisplayName = "Conteúdo do Documento com Header e Footer")]
        [Trait("Category", "PDF")]
        public void Pdf_document_header_content_footer()
        {
            // Arrange
            string expect = "Expected document content" + System.Environment.NewLine + "Header of Document" + System.Environment.NewLine + "Footer of Document";

            // Act
            var html = string.Format(_htmlDocumentFormat, "<header><div>Header of Document</div></header>Expected document content<footer><div>Footer of Document</div></footer>");

            var result = Pdf.From(html)
                            .Content();

            TextAssert.AreEqual(expect, PdfDocumentReader.ToText(result));
        }

        [Fact(DisplayName = "Documento Encoding")]
        [Trait("Category", "PDF")]
        public void Pdf_document_encoding()
        {
            // Arrange
            const string expect = "Äöåõ";

            // Act
            var html = string.Format(_htmlDocumentFormat, expect);

            var result = Pdf.From(html)
                            .EncodedWith("utf-8")
                            .Content();

            TextAssert.AreEqual(expect, PdfDocumentReader.ToText(result));
        }

        [Fact(DisplayName = "Titulo")]
        [Trait("Category", "PDF")]
        public void Document_title()
        {
            // Arrange
            const string expect = "Expected title";
            const string expectedDocumentContent = "Expected document content";

            // Act
            var html = string.Format(_htmlDocumentFormat, expectedDocumentContent);

            var result = Pdf.From(html)
                            .WithTitle(expect)
                            .Content();

            Assert.Equal(expect, PdfDocumentReader.Title(result));
            TextAssert.AreEqual(expectedDocumentContent, PdfDocumentReader.ToText(result));
        }

        [Fact(DisplayName = "Tamanho da Página")]
        [Trait("Category", "PDF")]
        public void Page_size()
        {
            // Arrange
            const string expect = "Expected paper size of document content";

            // Act
            var html = string.Format(_htmlDocumentFormat, expect);

            var result = Pdf.From(html)
                            .PaperSize(PaperSize.A4)
                            .Content();

            TextAssert.AreEqual(expect, PdfDocumentReader.ToText(result));
            Assert.Equal(_210mmInPostScriptPoints + 1, PdfDocumentReader.WidthOfFirstPage(result));
            Assert.Equal(_297mmInPostScriptPoints, PdfDocumentReader.HeightOfFirstPage(result));
        }

        [Fact(DisplayName = "Página em Retrato")]
        [Trait("Category", "PDF")]
        public void Portrait()
        {
            // Arrange
            const string expect = "Expected portrait document content";

            // Act
            var html = string.Format(_htmlDocumentFormat, expect);

            var result = Pdf.From(html).Portrait().Content();

            TextAssert.AreEqual(expect, PdfDocumentReader.ToText(result));
            Assert.Equal(_210mmInPostScriptPoints, PdfDocumentReader.WidthOfFirstPage(result));
            Assert.Equal(_297mmInPostScriptPoints, PdfDocumentReader.HeightOfFirstPage(result));
        }

        [Fact(DisplayName = "Página em Paisagem")]
        [Trait("Category", "PDF")]
        public void Landscape()
        {
            // Arrange
            const string expect = "Expected landscape document content";

            // Act
            var html = string.Format(_htmlDocumentFormat, expect);

            var result = Pdf.From(html).Landscape().Content();

            TextAssert.AreEqual(expect, PdfDocumentReader.ToText(result));
            Assert.Equal(_297mmInPostScriptPoints, PdfDocumentReader.WidthOfFirstPage(result));
            Assert.Equal(_210mmInPostScriptPoints, PdfDocumentReader.HeightOfFirstPage(result));
        }

        [Fact(DisplayName = "Margens")]
        [Trait("Category", "PDF")]
        public void Margins()
        {
            // Arrange
            const string expect = "Expected margins document content";

            // Act
            var html = string.Format(_htmlDocumentFormat, expect);

            var result = Pdf.From(html)
                            .WithMargins(1.25.Centimeters())
                            .Content();

            TextAssert.AreEqual(expect, PdfDocumentReader.ToText(result));
        }

        [Fact(DisplayName = "Removendo Arquivos temporários")]
        [Trait("Category", "PDF")]
        public void No_temporary_files_are_left_behind()
        {
            // Arrange
            const string expect = "Expected NO document content in temporary folder";

            // Act
            var html = string.Format(_htmlDocumentFormat, expect);

            Pdf.From(html).Content();

            Assert.True(Directory.GetFiles(Path.Combine(Path.GetTempPath(), "Faepa", "HtmlToX"), "*.pdf").Count() == 0);
        }

        [Fact(DisplayName = "Compressão de dados")]
        [Trait("Category", "PDF")]
        public void Compressed()
        {
            // Arrange
            const string expect = "Expected compressed document content";

            // Act
            var html = string.Format(_htmlDocumentFormat, expect);

            var result = Pdf.From(html)
                            .Compressed()
                            .Content();

            TextAssert.AreEqual(expect, PdfDocumentReader.ToText(result));
        }

        [Fact(DisplayName = "Com linha no rodapé")]
        [Trait("Category", "PDF")]
        public void With_outline()
        {
            // Arrange
            const string expect = "Expected with outline compressed document content";

            // Act
            var html = string.Format(_htmlDocumentFormat, expect);

            var result = Pdf.From(html)
                            .WithOutline()
                            .Content();

            TextAssert.AreEqual(expect, PdfDocumentReader.ToText(result));
        }

        [Fact(DisplayName = "Sem linha no rodapé")]
        [Trait("Category", "PDF")]
        public void Without_outline()
        {
            // Arrange
            const string expect = "Expected without outline compressed document content";

            // Act
            var html = string.Format(_htmlDocumentFormat, expect);

            var result = Pdf.From(html)
                            .WithoutOutline()
                            .Content();

            TextAssert.AreEqual(expect, PdfDocumentReader.ToText(result));
        }


        [Fact(DisplayName = "Massa de Dados")]
        [Trait("Category", "PDF")]
        public void Convert_massive_number_of_documents()
        {
            // Arrange
            const string expect = "Expected massive documents contents";
            const int documentCount = 100;

            // Act
            var html = string.Format(_htmlDocumentFormat, expect);
            var tasks = new List<Task<byte[]>>();

            for (var i = 0; i < documentCount; i++)
                tasks.Add(Task.Run(() => Pdf.From(html).Content()));

            Task.WaitAll(tasks.OfType<Task>().ToArray());

            foreach (var task in tasks)
                TextAssert.AreEqual(expect, PdfDocumentReader.ToText(task.Result));
        }

        [Fact(DisplayName = "Converter Multiplos arquivos em paralelo")]
        [Trait("Category", "PDF")]
        public void Convert_multiple_documents_concurrently()
        {
            // Arrange
            const string expect = "Expected multiple documents";
            const int documentCount = 10;

            // Act
            var html = string.Format(_htmlDocumentFormat, expect);
            var tasks = new List<Task<byte[]>>();

            for (var i = 0; i < documentCount; i++)
                tasks.Add(Task.Run(() => Pdf.From(html).Content()));

            Task.WaitAll(tasks.OfType<Task>().ToArray());

            foreach (var task in tasks)
                TextAssert.AreEqual(expect, PdfDocumentReader.ToText(task.Result));
        }

        [Fact(DisplayName = "Converter Multiplos arquivos em sequência")]
        [Trait("Category", "PDF")]
        public void Convert_multiple_documents_sequently()
        {
            // Arrange
            const string expect1 = "Expected document content 1";
            const string expect2 = "Expected document content 2";
            const string expect3 = "Expected document content 3";

            // Act
            var html1 = string.Format(_htmlDocumentFormat, expect1);
            var html2 = string.Format(_htmlDocumentFormat, expect2);
            var html3 = string.Format(_htmlDocumentFormat, expect3);

            var first = Pdf.From(html1).Content();
            var second = Pdf.From(html2).Content();
            var third = Pdf.From(html3).Content();

            TextAssert.AreEqual(expect1, PdfDocumentReader.ToText(first));
            TextAssert.AreEqual(expect2, PdfDocumentReader.ToText(second));
            TextAssert.AreEqual(expect3, PdfDocumentReader.ToText(third));
        }


        #endregion        

        //[TestMethod]
        //public void Is_directory_agnostic()
        //{
        //    const string expectedDocumentContent = "Expected document content";
        //    var html = string.Format(HtmlDocumentFormat, expectedDocumentContent);

        //    Directory.SetCurrentDirectory(@"c:\");
        //    var result = Pdf.From(html).Content();

        //    TextAssert.AreEqual(expectedDocumentContent, PdfDocumentReader.ToText(result));
        //}

    }
}
