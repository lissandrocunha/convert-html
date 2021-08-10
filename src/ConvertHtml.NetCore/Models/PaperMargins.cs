using System;
using System.Collections.Generic;
using System.Text;

namespace ConvertHtml.NetCore.Models
{
    public sealed class PaperMargins
    {

        #region Variables

        private static Length _top;
        private static Length _right;
        private static Length _bottom;
        private static Length _left;

        #endregion

        #region Properties

        public string TopSetting
        {
            get { return _top?.SettingString; }
        }

        public string RightSetting
        {
            get { return _right?.SettingString; }
        }

        public string BottomSetting
        {
            get { return _bottom?.SettingString; }
        }

        public string LeftSetting
        {
            get { return _left?.SettingString; }
        }

        #endregion

        #region Constructors

        private PaperMargins(Length allMargins)
            : this(allMargins, allMargins, allMargins, allMargins)
        {
        }

        private PaperMargins(Length top, Length right, Length bottom, Length left)
        {
            _top = top;
            _right = right;
            _bottom = bottom;
            _left = left;
        }

        #endregion

        #region Methods

        public static PaperMargins All(Length allMargins)
        {
            return new PaperMargins(allMargins);
        }

        public static PaperMargins None()
        {
            return new PaperMargins(Length.Zero());
        }

        public static PaperMargins Top(Length top)
        {
            return new PaperMargins(top, _right, _bottom, _left);
        }

        public static PaperMargins Botton(Length botton)
        {
            return new PaperMargins(_top, _right, botton, _left);
        }

        public static PaperMargins Right(Length right)
        {
            return new PaperMargins(_top, right, _bottom, _left);
        }

        public static PaperMargins Left(Length left)
        {
            return new PaperMargins(_top, _right, _bottom, left);
        }

        public static PaperMargins Custom(Length top, Length botton, Length left, Length right)
        {
            return new PaperMargins(top, right, botton, left);
        }

        public static implicit operator PaperMargins(Length allMargins)
        {
            return new PaperMargins(allMargins);
        }

        #endregion

    }
}
