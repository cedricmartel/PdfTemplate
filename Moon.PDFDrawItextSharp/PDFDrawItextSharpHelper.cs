using System;
using System.Collections.Generic;
using System.Text;

namespace Moon.PDFDrawItextSharp
{
    /// <summary>
    /// ITextSharp types converter.
    /// </summary>
	public class PDFDrawItextSharpHelper
    {
        /// <summary>
        /// Gets itextsharp font from string
        /// </summary>
        /// <param name="fonttype"></param>
        /// <returns></returns>
		public static string FontType(string fonttype)
        {
            // itextsharp basefont on a few type
            //("Courier", "Helvetica", "Times New Roman", "Symbol" or "ZapfDingbats"). 
            fonttype = fonttype.ToUpper();
            string r = iTextSharp.text.FontFactory.COURIER;
            switch (fonttype)
            {
                case "COURIER":
                    //r = "COURIER";
                    break;
                case "HELVETICA":
                    r = iTextSharp.text.FontFactory.HELVETICA;
                    break;
                case "TIMES ROMAN":
                    r = iTextSharp.text.FontFactory.TIMES_ROMAN;
                    break;
            }
            return r;
        }

		/// <summary>
		/// Gets itextsharp aligment from string
		/// </summary>
		/// <param name="alignment"></param>
		/// <returns></returns>
        public static int Align(string alignment)
        {
            alignment = alignment.ToUpper();
            int r = iTextSharp.text.Element.ALIGN_LEFT;
            switch (alignment)
            {
                case "LEFT":
                    break;
                case "RIGHT":
                    r = iTextSharp.text.Element.ALIGN_RIGHT;
                    break;
                case "CENTER":
                    r = iTextSharp.text.Element.ALIGN_CENTER;
                    break;
                case "TOP":
                    r = iTextSharp.text.Element.ALIGN_TOP;
                    break;
                case "BOTTOM":
                    r = iTextSharp.text.Element.ALIGN_BOTTOM;
                    break;
                    //20130610 :: mellorasinxelas :: add justified
                case "JUSTIFIED":
                    r = iTextSharp.text.Element.ALIGN_JUSTIFIED;
                    break;
                case "JUSTIFIEDALL":
                    r = iTextSharp.text.Element.ALIGN_JUSTIFIED_ALL;
                    break;
                    //---
            }
            return r;
        }
        
        /// <summary>
        /// Gets itextsharp vertical alignment from string
        /// </summary>
        /// <param name="align"></param>
        /// <returns></returns>
        public static int AlignVertical(string align)
        {
            align = align.ToUpper();
            int r = iTextSharp.text.Element.ALIGN_BOTTOM;
            switch (align)
            {
                case "BOTTOM":
                    break;
                case "MIDDLE":
                    r = iTextSharp.text.Element.ALIGN_MIDDLE;
                    break;
                case "TOP":
                    r = iTextSharp.text.Element.ALIGN_TOP;
                    break;
            }
            return r;
        }
        
        /// <summary>
        /// Gets itextsharp font style from string
        /// </summary>
        /// <param name="fontstyle"></param>
        /// <returns></returns>
        public static int FontStyle(string fontstyle)
        {
            fontstyle = fontstyle.ToUpper();
            int r = iTextSharp.text.Font.NORMAL;
            switch (fontstyle)
            {
                case "REGULAR":
                    break;
                case "BOLD":
                    r = iTextSharp.text.Font.BOLD;
                    break;
                case "BOLDITALIC":
                    r = iTextSharp.text.Font.BOLDITALIC;
                    break;
                case "ITALIC":
                    r = iTextSharp.text.Font.ITALIC;
                    break;
                case "UNDERLINE":
                    r = iTextSharp.text.Font.UNDERLINE;
                    break;
                case "STRIKEOUT":
                    r = iTextSharp.text.Font.STRIKETHRU;
                    break;
            }
            return r;
        }
        
        /// <summary>
        /// Gets itextsharp border from string
        /// </summary>
        /// <param name="border"></param>
        /// <returns></returns>
        public static int Border(string border)
        {
            border = border.ToUpper();
            int r = iTextSharp.text.Rectangle.NO_BORDER;
            switch (border)
            {
                case "LEFT":
                    r = iTextSharp.text.Rectangle.LEFT_BORDER;
                    break;
                case "TOP":
                    r = iTextSharp.text.Rectangle.TOP_BORDER;
                    break;
                case "RIGHT":
                    r = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    break;
                case "BOTTOM":
                    r = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    break;
            }
            return r;
        }
        
        /// <summary>
        /// Gets itextsharp border from string[]
        /// </summary>
        /// <param name="borders"></param>
        /// <returns></returns>
        public static int Border(string[] borders)
        {
            int[] r = new int[borders.Length];
            for(int i=0; i<borders.Length; i++)
            {
                r[i] = Border(borders[i]);
            }

            int r1 = 0;
            for (int i = 0; i < r.Length; i++)
            {
                r1 |= r[i];
            }
            return r1;
        }
        
        /// <summary>
        /// Gets itextsharp rectangle from string
        /// </summary>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static iTextSharp.text.Rectangle PageSize(string pagesize)
        {
            iTextSharp.text.Rectangle r = iTextSharp.text.PageSize.GetRectangle(pagesize);
            return r;
        }
    }
}
