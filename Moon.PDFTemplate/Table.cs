using System;
using System.Collections.Generic;
 
using System.Text;
using System.Xml;

namespace Moon.PDFTemplate
{
    /// <summary>
    /// Table element
    /// <para/>Attributes: NONE
    /// </summary>
	public class Table : Element
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="attrs"></param>
		public Table(XmlAttributeCollection attrs) : base(attrs) { }

    }
}
