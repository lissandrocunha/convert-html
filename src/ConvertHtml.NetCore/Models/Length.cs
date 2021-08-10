using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ConvertHtml.NetCore.Models
{
    public sealed class Length
    {

        #region Variables

        private readonly double _length;
        private readonly object _unitOfLength;

        #endregion
        
        #region Properties

        public string SettingString
        {
            get { return string.Format("{0}{1}", _length.ToString("0.##", CultureInfo.InvariantCulture), _unitOfLength); }
        }

        #endregion

        #region Constructors

        private Length(double length, string unitOfLength)
        {
            _length = length;
            _unitOfLength = unitOfLength;
        }

        #endregion

        #region Methods

        public static Length Zero()
        {
            return new Length(0, "");
        }

        public static Length Millimeters(double length)
        {
            return new Length(length, "mm");
        }

        public static Length Centimeters(double length)
        {
            return new Length(length, "cm");
        }

        public static Length Inches(double length)
        {
            return new Length(length, "in");
        }

        #endregion

    }
}
