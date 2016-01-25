using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace Moon.PDFTemplate
{
    /// <summary>
    /// Basic element
    /// </summary>
	public abstract class Element
    {
        /// <summary>
        /// XML attr
        /// </summary>
		protected XmlAttributeCollection attrs;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="attrs"></param>
        public Element(XmlAttributeCollection attrs)
        {
            this.attrs = attrs;
        }

        /// <summary>
        /// Get XML attrs
        /// </summary>
        public XmlAttributeCollection Attributes
        {
            get { return attrs; }
        }
    }
}
