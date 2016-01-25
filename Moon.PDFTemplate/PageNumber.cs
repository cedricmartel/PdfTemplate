using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Moon.PDFTemplate.XMAtributes;

namespace Moon.PDFTemplate
{
    /// <summary>
    /// Page number PDF element.
    /// NOTE: Only allow one page number per page.
    /// <para/>Attributes: NONE
    /// </summary>
	public class PageNumber : TextBox
    {
        private float y;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fontAttrs"></param>
        /// <param name="textAttrs"></param>
        public PageNumber(string text, XmlAttributeCollection fontAttrs, XmlAttributeCollection textAttrs) :
            base(text, fontAttrs, textAttrs)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fontAttrs"></param>
        /// <param name="textAttrs"></param>
        /// <param name="vars"></param>
        public PageNumber(string text,
            XmlAttributeCollection fontAttrs,
            XmlAttributeCollection textAttrs,
            List<Variable> vars) :       base(text, fontAttrs, textAttrs, vars)
        { }
        
        /// <summary>
        /// Get / set Y position
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        
        /// <summary>
        /// Sets Y position
        /// </summary>
        /// <param name="y"></param>
        public void SetY(float y)
        {
            this.y = y;
        }
    }
}
