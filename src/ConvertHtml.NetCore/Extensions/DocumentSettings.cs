using ConvertHtml.NetCore.Interfaces;
using ConvertHtml.NetCore.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ConvertHtml.NetCore.Exceptions
{
    public static class FluentSettings
    {
        #region Methods

        #region Global Options

        public static IDocument WithTitle(this IDocument pdfDocument,
                                          string title)
        {
            return pdfDocument.WithGlobalSetting("documentTitle", title);
        }

        public static IDocument Landscape(this IDocument pdfDocument)
        {
            return pdfDocument
                .WithGlobalSetting("orientation", "Landscape");
        }

        public static IDocument Portrait(this IDocument pdfDocument)
        {
            return pdfDocument
                .WithGlobalSetting("orientation", "Portrait");
        }

        public static IDocument GrayScale(this IDocument pdfDocument)
        {
            return pdfDocument.WithGlobalSetting("grayscale", "true");
        }

        public static IDocument LowQuality(this IDocument pdfDocument)
        {
            return pdfDocument.WithGlobalSetting("lowquality", "true");
        }

        public static IDocument WithMargins(this IDocument pdfDocument,
                                            Func<PaperMargins, PaperMargins> paperMargins)
        {
            return pdfDocument.WithMargins(paperMargins(PaperMargins.None()));
        }

        public static IDocument WithMargins(this IDocument pdfDocument,
                                            PaperMargins margins)
        {
            return pdfDocument.WithGlobalSetting("margin.bottom", margins.BottomSetting)
                              .WithGlobalSetting("margin.left", margins.LeftSetting)
                              .WithGlobalSetting("margin.right", margins.RightSetting)
                              .WithGlobalSetting("margin.top", margins.TopSetting);
        }

        public static IDocument PaperSize(this IDocument pdfDocument,
                                          PaperSize paperSize)
        {
            return pdfDocument.WithGlobalSetting("size.width", paperSize.Width)
                              .WithGlobalSetting("size.height", paperSize.Height);
        }

        public static IDocument EncodedWith(this IDocument pdfDocument, string encoding)
        {
            return pdfDocument.WithObjectSetting("web.defaultEncoding", encoding);
        }

        public static IDocument WithResolution(this IDocument pdfDocument, int dpi)
        {
            return pdfDocument
                .WithGlobalSetting("dpi", dpi.ToString(CultureInfo.InvariantCulture));
        }

        public static IDocument WithCopies(this IDocument pdfDocument, int copies)
        {
            return pdfDocument
                .WithGlobalSetting("copies", copies.ToString());
        }

        public static IDocument WithZoom(this IDocument pdfDocument, double zoomFactor)
        {
            return pdfDocument
                .WithGlobalSetting("zoom", zoomFactor.ToString());
        }

        public static IDocument Compressed(this IDocument pdfDocument)
        {
            return pdfDocument.WithGlobalSetting("useCompression", "true");
        }

        public static IDocument WithOutline(this IDocument pdfDocument)
        {
            return pdfDocument.WithGlobalSetting("outline", "true");
        }

        public static IDocument WithoutOutline(this IDocument pdfDocument)
        {
            return pdfDocument.WithGlobalSetting("outline", "false");
        }

        public static IDocument WithHeaderSpacing(this IDocument pdfDocument, double spaceInMilimiters)
        {
            return pdfDocument
                .WithGlobalSetting("header.spacing", spaceInMilimiters.ToString());
        }

        public static IDocument WithFooterSpacing(this IDocument pdfDocument, double spaceInMilimiters)
        {
            return pdfDocument
                .WithGlobalSetting("footer.spacing", spaceInMilimiters.ToString());
        }

        public static IDocument WithHeaderCustom(this IDocument pdfDocument, string customFooterArgs)
        {
            return pdfDocument.WithGlobalSetting("header.custom", customFooterArgs);
        }

        public static IDocument WithFooterCustom(this IDocument pdfDocument, string customFooterArgs)
        {
            return pdfDocument.WithGlobalSetting("footer.custom", customFooterArgs);
        }

        public static IDocument WithSmartShrink(this IDocument pdfDocument, bool enable = true)
        {
            return pdfDocument
                .WithGlobalSetting("smart.shrinking", enable.ToString()?.ToLower());
        }        

        #endregion

        #endregion
    }
}
