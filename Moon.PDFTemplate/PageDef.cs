using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Moon.PDFTemplate.XMAtributes;

namespace Moon.PDFTemplate
{
	/// <summary>
	/// PDF Page definition.
	/// 20130607 :: mellorasinxelas :: Add backgroundimage with src in value.
	/// <para/>Attributes: backgroundimage, margin_left, margin_right, margin_top, margin_bottom, width, height
	/// </summary>
	public class PageDef
	{
		/// <summary>
		/// Page background image attr constant
		/// </summary>
		public const string BackgroundImageAttributeConstant = "backgroundimage";
		
		/// <summary>
		/// Page margin left attr constant
		/// </summary>
		public const string MarginLeftAttributeConstant = "margin_left";
		
		/// <summary>
		/// Page margin right attr constant
		/// </summary>
		public const string MarginRightAttributeConstant = "margin_right";
		
		/// <summary>
		/// Page margin top attr constant
		/// </summary>
		public const string MarginTopAttributeConstant = "margin_top";
		
		/// <summary>
		/// Page margin bottom attr constant
		/// </summary>
		public const string MarginBottonAttributeConstant = "margin_bottom";
		
		/// <summary>
		/// Page width left attr constant
		/// </summary>
		public const string WidthAttributeConstant = "width";
		
		/// <summary>
		/// Page height attr constant
		/// </summary>
		public const string HeightAttributeConstant = "height";
		
		
		private XmlAttributeCollection pageDefAttrs;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="pageDefAttrs"></param>
		public PageDef(XmlAttributeCollection pageDefAttrs)
		{
			this.pageDefAttrs = pageDefAttrs;
			init();
		}

		private void init()
		{
			margin_left = PDFDraw.Helper.GetFloatAttributeValue(MarginLeftAttributeConstant, pageDefAttrs, 20);
			margin_right = PDFDraw.Helper.GetFloatAttributeValue(MarginRightAttributeConstant, pageDefAttrs, 20);
			margin_top = PDFDraw.Helper.GetFloatAttributeValue(MarginTopAttributeConstant, pageDefAttrs, 20);
			margin_bottom = PDFDraw.Helper.GetFloatAttributeValue(MarginBottonAttributeConstant, pageDefAttrs, 20);
			width = PDFDraw.Helper.GetFloatAttributeValue(WidthAttributeConstant, pageDefAttrs, -1);
			height = PDFDraw.Helper.GetFloatAttributeValue(HeightAttributeConstant, pageDefAttrs, -1);
			
			//20130607
			string biPath = PDFDraw.Helper.GetAttributeValue(BackgroundImageAttributeConstant, pageDefAttrs, null );
			if(biPath != null){
				_backgroundImage = new XMLBackgroundImageDefinition( biPath, pageDefAttrs ); //auto load attrs.
			}
			//--
		}
		
		private float width = -1;
		private float height = -1;
		
		private float margin_left = 20;
		private float margin_right = 20;
		private float margin_top = 20;
		private float margin_bottom = 20;
		
		private RowGroup header = new RowGroup();
		private RowGroup loop = new RowGroup();
		private RowGroup footer = new RowGroup();
		private RowGroup body = new RowGroup();

		//20130607
		private BackgroundImageDefinition _backgroundImage = null;
		/// <summary>
		/// Gets background image if exists.
		/// </summary>
		public BackgroundImageDefinition BackgroundImage {
			get { return _backgroundImage; }
		}
		//----
		
		//-----------------------------------------------------------
		
		/// <summary>
		/// Page XM attrs
		/// </summary>
		public XmlAttributeCollection PageDefAttrs
		{
			get { return pageDefAttrs; }
		}
		
		/// <summary>
		/// Page width
		/// </summary>
		public float Width
		{
			get { return width; }
			set { width = value; }
		}
		
		/// <summary>
		/// Page height
		/// </summary>
		public float Height
		{
			get { return height; }
			set { height = value; }
		}
		
		/// <summary>
		/// Page margin left
		/// </summary>
		public float Margin_left
		{
			get { return margin_left; }
			set { margin_left = value; }
		}
		
		/// <summary>
		/// Page margin right
		/// </summary>
		public float Margin_right
		{
			get { return margin_right; }
			set { margin_right = value; }
		}
		
		/// <summary>
		/// Page margin top
		/// </summary>
		public float Margin_top
		{
			get { return margin_top; }
			set { margin_top = value; }
		}
		
		/// <summary>
		/// Page margin bottom
		/// </summary>
		public float Margin_bottom
		{
			get { return margin_bottom; }
			set { margin_bottom = value; }
		}
		
		/// <summary>
		/// Header group
		/// </summary>
		public RowGroup Header
		{
			get { return header; }
			set { header = value; }
		}
		
		/// <summary>
		/// Row group
		/// </summary>
		public RowGroup Loop
		{
			get { return loop; }
			set { loop = value; }
		}
		
		/// <summary>
		/// Footer group
		/// </summary>
		public RowGroup Footer
		{
			get { return footer; }
			set { footer = value; }
		}
		
		/// <summary>
		/// Body group
		/// </summary>
		public RowGroup Body
		{
			get { return body; }
			set { body = value; }
		}
	}
}
