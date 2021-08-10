
using ConvertHtml.NetCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConvertHtml.NetCore.Models
{
    public sealed class PaperSize
    {

        #region Variables

        private readonly Length _width;
        private readonly Length _height;

        #endregion

        #region Properties

        public string Width
        {
            get { return _width.SettingString; }
        }

        public string Height
        {
            get { return _height.SettingString; }
        }

        #endregion

        #region Constructors

        public PaperSize(Length width, Length height)
        {
            _width = width;
            _height = height;
        }

        #endregion

        #region Methods

        public static PaperSize A0 { get { return new PaperSize(841.Millimeters(), 1189.Millimeters()); } }
        public static PaperSize A1 { get { return new PaperSize(594.Millimeters(), 841.Millimeters()); } }
        public static PaperSize A2 { get { return new PaperSize(420.Millimeters(), 594.Millimeters()); } }
        public static PaperSize A3 { get { return new PaperSize(297.Millimeters(), 420.Millimeters()); } }
        public static PaperSize A4 { get { return new PaperSize(210.Millimeters(), 297.Millimeters()); } }
        public static PaperSize A5 { get { return new PaperSize(148.Millimeters(), 210.Millimeters()); } }
        public static PaperSize A6 { get { return new PaperSize(105.Millimeters(), 148.Millimeters()); } }
        public static PaperSize A7 { get { return new PaperSize(74.Millimeters(), 105.Millimeters()); } }
        public static PaperSize A8 { get { return new PaperSize(52.Millimeters(), 74.Millimeters()); } }
        public static PaperSize A9 { get { return new PaperSize(37.Millimeters(), 52.Millimeters()); } }
        public static PaperSize A10 { get { return new PaperSize(26.Millimeters(), 37.Millimeters()); } }
        public static PaperSize B0 { get { return new PaperSize(1000.Millimeters(), 1414.Millimeters()); } }
        public static PaperSize B1 { get { return new PaperSize(707.Millimeters(), 1000.Millimeters()); } }
        public static PaperSize B2 { get { return new PaperSize(500.Millimeters(), 707.Millimeters()); } }
        public static PaperSize B3 { get { return new PaperSize(353.Millimeters(), 500.Millimeters()); } }
        public static PaperSize B4 { get { return new PaperSize(250.Millimeters(), 353.Millimeters()); } }
        public static PaperSize B5 { get { return new PaperSize(176.Millimeters(), 250.Millimeters()); } }
        public static PaperSize B6 { get { return new PaperSize(125.Millimeters(), 176.Millimeters()); } }
        public static PaperSize B7 { get { return new PaperSize(88.Millimeters(), 125.Millimeters()); } }
        public static PaperSize B8 { get { return new PaperSize(62.Millimeters(), 88.Millimeters()); } }
        public static PaperSize B9 { get { return new PaperSize(44.Millimeters(), 62.Millimeters()); } }
        public static PaperSize B10 { get { return new PaperSize(31.Millimeters(), 44.Millimeters()); } }
        public static PaperSize C0 { get { return new PaperSize(917.Millimeters(), 1297.Millimeters()); } }
        public static PaperSize C1 { get { return new PaperSize(648.Millimeters(), 917.Millimeters()); } }
        public static PaperSize C2 { get { return new PaperSize(458.Millimeters(), 648.Millimeters()); } }
        public static PaperSize C3 { get { return new PaperSize(324.Millimeters(), 458.Millimeters()); } }
        public static PaperSize C4 { get { return new PaperSize(229.Millimeters(), 324.Millimeters()); } }
        public static PaperSize C5 { get { return new PaperSize(162.Millimeters(), 229.Millimeters()); } }
        public static PaperSize C6 { get { return new PaperSize(114.Millimeters(), 162.Millimeters()); } }
        public static PaperSize C7 { get { return new PaperSize(81.Millimeters(), 114.Millimeters()); } }
        public static PaperSize C8 { get { return new PaperSize(57.Millimeters(), 81.Millimeters()); } }
        public static PaperSize C9 { get { return new PaperSize(40.Millimeters(), 57.Millimeters()); } }
        public static PaperSize C10 { get { return new PaperSize(28.Millimeters(), 40.Millimeters()); } }
        public static PaperSize HalfLetter { get { return new PaperSize(5.5.Inches(), 8.5.Inches()); } }
        public static PaperSize Letter { get { return new PaperSize(8.5.Inches(), 11.Inches()); } }
        public static PaperSize JuniorLegal { get { return new PaperSize(5.Inches(), 8.Inches()); } }
        public static PaperSize Legal { get { return new PaperSize(8.5.Inches(), 14.Inches()); } }        
        public static PaperSize Ledger { get { return new PaperSize(11.Inches(), 17.Inches()); } }
        public static PaperSize Tabloid { get { return new PaperSize(11.Inches(), 17.Inches()); } }

        #endregion

    }
}
