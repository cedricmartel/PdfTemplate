using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace Moon.PDFTemplate
{
	/// <summary>
	/// Line draw element
	/// <para/>Attributes: fontsize, x1,x2
	/// </summary>
	public class Line : DrawElement
	{
		/// <summary>
		/// Line font size  attr constant
		/// </summary>
		public const string FontSizeAttributeConstant="fontsize";
		
		/// <summary>
		/// Line x1(start position) attr constant
		/// </summary>
		public const string XInitAttributeConstant="x1";
		
		/// <summary>
    	/// Line x2(end position) attr constant
    	/// </summary>
		public const string XEndAttributeConstant="x2";
		
		
		private XmlAttributeCollection lineAttrs;
		private float x1 = -1;
		private float x2 = -1;

		//20130406 :: modify
		/// <summary>
		/// Gets start position
		/// </summary>
		protected float InitialX{
			get{
				return x1;
			}
		}
		
		/// <summary>
		/// Gets end position
		/// </summary>
		protected float FinalX{
			get{
				return x2;
			}
		}
		//----
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fontAttrs"></param>
		/// <param name="lineAttrs"></param>
		public Line(XmlAttributeCollection fontAttrs, XmlAttributeCollection lineAttrs):base(fontAttrs, lineAttrs, null)
		{
			this.lineAttrs = lineAttrs;
			//FontAttributes = fontAttrs;
			init();
		}

		private void init()
		{
			x1 = PDFDraw.Helper.GetFloatAttributeValue(XInitAttributeConstant, lineAttrs, -1);
			x2 = PDFDraw.Helper.GetFloatAttributeValue(XEndAttributeConstant, lineAttrs, -1);
			if (x1 == -1 && x2 == -1)
			{
				Width_percent = PDFDraw.Helper.GetAttributeWidthPercent(lineAttrs);
			}
		}

		/// <summary>
		/// Get XML Attrs
		/// </summary>
		public XmlAttributeCollection LineAttrs
		{
			get { return lineAttrs; }
		}


		/// <summary>
		/// Draws element
		/// </summary>
		/// <param name="pdfDraw"></param>
		/// <param name="data"></param>
		public override void Draw(PDFDraw.IPDFDraw pdfDraw, System.Collections.IDictionary data)
		{
			pdfDraw.DrawVerticalLine(this.x1, this.x2, lineAttrs);
		}
		//---

		/// <summary>
		/// Set X and W
		/// </summary>
		/// <param name="x"></param>
		/// <param name="width"></param>
		public override void SetXandWidth(float x, float width)
		{
//			this.x = x;
//			this.width = width;
			base.SetXandWidth(x,width);
			this.x1 = x;
			this.x2 = x + width;
		}

		/// <summary>
		/// Gets element height
		/// </summary>
		/// <returns></returns>
		public override float GetHeight()
		{
			return PDFDraw.Helper.GetFloatAttributeValue(FontSizeAttributeConstant, FontAttributes, 10);
		}
	}
}
