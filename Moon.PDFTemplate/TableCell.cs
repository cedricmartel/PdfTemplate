using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace Moon.PDFTemplate
{
	/// <summary>
	/// Table cell element. 
	/// Contains a list of DrawElements.
	/// 
	/// <para/>Attributes: backgroundcolor
	/// </summary>
	public class TableCell : Element
	{
		/// <summary>
		/// Table cell backcolor attr constant.
		/// </summary>
		public const string BackgroundColorAttributeConstant="backgroundcolor";
		
		
		
		/// <summary>
		/// Font attrs
		/// </summary>
		protected XmlAttributeCollection fontAttrs;
		
		/// <summary>
		/// Drawable elements into
		/// </summary>
		protected List<DrawElement> drawElements = new List<DrawElement>();

		/// <summary>
		/// Constructor
		/// </summary>
		public TableCell(XmlAttributeCollection attrs, XmlAttributeCollection fontAttrs)
			: base(attrs)
		{
			this.fontAttrs = fontAttrs;
		}

		/// <summary>
		/// Get font attrs
		/// </summary>
		public XmlAttributeCollection FontAttributes
		{
			get { return fontAttrs; }
		}

		/// <summary>
		/// Drawable elements into
		/// </summary>
		public List<DrawElement> DrawElements
		{
			get { return drawElements; }
		}

		//20130604 :: mellorasinxelas
		/// <summary>
		/// Add a new element
		/// </summary>
		/// <param name="drawElement"></param>
		public void AddDrawElement(DrawElement drawElement)
		{
			//null control.
			if(drawElement == null){
				Console.WriteLine("Draw element null. Not added into row element.");
				return;
			}
			//--
			drawElements.Add(drawElement);
		}
	}
}
