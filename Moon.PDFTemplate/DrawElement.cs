using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Moon.PDFTemplate.XMAtributes;

namespace Moon.PDFTemplate
{
	
	/// <summary>
	/// PDF template draw element.
	/// <para/>
	/// Modify to use optional VARS ([] var enclousure). When it's a optional var, if data doesnt found, doesnt touch anything(xml original value it's right),
	/// otherwise if var it's not optional, var value it's replaced to empty string value.
	/// 
	/// <para/>20150203 :: melloraSinxelas :: Changed var visiblity. Modified properties. Add parameterized constructor
	/// </summary>
	public abstract class DrawElement
	{
		
		/// <summary>
		/// Width percent
		/// </summary>
		private float _width_percent = -1;
		
		/// <summary>
		/// X position
		/// </summary>
		private float _x = -1;
		
		/// <summary>
		/// Absolute width
		/// </summary>
		private float _width = -1;
		

		//20130604 :: mellorasinxelas
		private List<Variable> _vars = new List<Variable>();
		/// <summary>
		/// Vars into this element.
		/// </summary>
		public List<Variable> Vars {
			get { return _vars; }
			protected set { _vars = value; }
		}
		
		//-----
		

		private System.Xml.XmlAttributeCollection _fontAttributes;

		//20130603 :: Compatibility
		private System.Xml.XmlAttributeCollection _attributes;
		
		/// <summary>
		/// XML element Attributes
		/// </summary>
		public System.Xml.XmlAttributeCollection Attributes {
			get {
				return _attributes;
			}
//			protected set{
//				_attributes = value;
//			}

		}
		//-----
		/// <summary>
		/// XML font attr
		/// </summary>
		public System.Xml.XmlAttributeCollection FontAttributes
		{
			get { return _fontAttributes; }
//			protected set { _fontAttributes = value; }
		}

		/// <summary>
		/// Gets/set width percent
		/// </summary>
		public float Width_percent
		{
			get { return _width_percent; }
			protected set { _width_percent = value; }
		}
		
		/// <summary>
		/// Gets/set X position
		/// </summary>
		public float X
		{
			get { return _x; }
//			protected set{ _x = value; }
		}
		
		/// <summary>
		/// Gets/set Absolute Width
		/// </summary>
		public float Width
		{
			get { return _width; }
//			protected set{_width = value;}
		}
		
		/// <summary>
		/// Gets/set the list of Variable
		/// </summary>
		public List<Variable> VAR
		{
			get { return _vars; }
		}

		#region Constructors

		
//		/// <summary>
//		/// Constructor
//		/// </summary>
//		/// <param name="fontAttrs"></param>
//		/// <param name="elementAttrs"></param>
//		protected DrawElement(XmlAttributeCollection fontAttrs, XmlAttributeCollection elementAttrs)
//		{
//			_fontAttributes = fontAttrs;
//			_attributes = elementAttrs;
//		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fontAttrs"></param>
		/// <param name="elementAttrs"></param>
		/// <param name="vars"></param>
		public DrawElement(  XmlAttributeCollection fontAttrs, XmlAttributeCollection elementAttrs, List<Variable> vars)
		{
			_fontAttributes = fontAttrs;
			_attributes = elementAttrs;
			if(vars!=null)_vars.AddRange( vars );
		}
		
		#endregion
		
		
		
		/// <summary>
		/// Sets X position and width.
		/// 20150203 :: Changed to virtual. Set internal vars.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="width"></param>
		public virtual void SetXandWidth(float x, float width){
			_x = x;
			_width = width;
		}
		
		
		
		#region Abstract
		
		/// <summary>
		/// Draws the PDF element
		/// </summary>
		/// <param name="pdfDraw"></param>
		/// <param name="data"></param>
		public abstract void Draw(Moon.PDFDraw.IPDFDraw pdfDraw, IDictionary data);
		
		
		
		/// <summary>
		/// Gets the height of this element.
		/// </summary>
		/// <returns></returns>
		public abstract float GetHeight();
		
		
		
		#endregion
	}
	
}
