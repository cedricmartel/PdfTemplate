/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 10/06/2013
 * Time: 16:28
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Xml;
using iTextSharp.text;
using Moon.PDFDraw;
using Moon.PDFDrawItextSharp;

namespace Moon.PDFDrawItextSharp.StyleContainers
{
	/// <summary>
	/// Store the style to use in textbox and textblock backgrounds.
	/// </summary>
	public class BlockStyle : SingleTextBoxStyle
	{
		
		#region Constants
		
		/// <summary>
		/// Block style border color attribute constant
		/// </summary>
		public const string BorderColorAttributeConstant="bordercolor"; 
		
		/// <summary>
		/// Block style border width attribute constant
		/// </summary>
		public const string BorderWithAttributeConstant="borderwidth";
		
		/// <summary>
		/// Block style background attribute constant
		/// </summary>
		public const string FillBackgroundAttributeConstant="backgroundfill";
		
		/// <summary>
		/// Block style border attribute constant
		/// </summary>
		public const string BorderAttributeConstant="border";
		#endregion
		
		#region Properties
		
		
		private Color? _borderColor = null;
		/// <summary>
		/// Border color
		/// </summary>
		public Color? BorderColor {
			get { return _borderColor; }
			set { _borderColor = value; }
		}
		
		private float _borderSize = 1.0F;
		/// <summary>
		/// Border width
		/// </summary>
		public float BorderWidth {
			get { return _borderSize; }
			set { 
				if(value >= 0)_borderSize = value;
			}
		}
		
		private bool _fillBackground = false;
		/// <summary>
		/// Get or set if background color its only for the text (false) or fill all the area (true)
		/// </summary>
		public bool FillBackground {
			get { return _fillBackground; }
			set { _fillBackground = value; }
		}
		
		private int _borders = iTextSharp.text.Rectangle.NO_BORDER;
		/// <summary>
		/// Use iTextSharp.text.Rectangle. constants. Enables borders. By default NO_BORDER
		/// </summary>
		public int Borders {
			get { return _borders; }
			set { _borders = value; }
		}
		
		
		#endregion
		
		#region Builders
		
		/// <summary>
		/// Constructor
		/// </summary>
		public BlockStyle()
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="backgroundColor"></param>
		public BlockStyle(Color backgroundColor):base( backgroundColor ){			
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="borders"></param>
		public BlockStyle(int borders){
			this._borders = borders;
			if(this._borders != iTextSharp.text.Rectangle.NO_BORDER){
				this.BorderWidth = 1.0F;//defaults
				this.BorderColor = Color.Black;//defaults
			}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="borders"></param>
		/// <param name="borderColor"></param>
		/// <param name="borderSize"></param>
		public BlockStyle(int borders, Color borderColor, float borderSize){
			this._borders = borders;
			this._borderColor = borderColor;
			this.BorderWidth = borderSize;
		}
				
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="backgroundColor"></param>
		/// <param name="borderColor"></param>
		/// <param name="borderSize"></param>
		public BlockStyle(Color backgroundColor, Color borderColor, float borderSize):base( backgroundColor ){			
			this._borderColor = borderColor;
			this.BorderWidth = borderSize;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="attrs"></param>
		public BlockStyle(XmlAttributeCollection attrs):base( attrs )
		{
			if(attrs != null){
				           
				if( PDFDraw.XmlHelper.GetAttributeValue( BorderColorAttributeConstant , attrs, null ) != null ){
				   	BorderColor = PDFDraw.XmlHelper.GetAttributeColor(BorderColorAttributeConstant, attrs, "White");
				}
           		this.BorderWidth = PDFDraw.XmlHelper.GetFloatAttributeValue( BorderWithAttributeConstant, attrs, 1.0F );
				this.FillBackground = PDFDraw.XmlHelper.GetAttributeBoolean( FillBackgroundAttributeConstant, attrs, false );		

				this.Borders = PDFDrawItextSharpHelper.Border(	Moon.PDFDraw.XmlHelper.GetStringArray(BorderAttributeConstant, attrs));
				if(this.Borders != iTextSharp.text.Rectangle.NO_BORDER){
					if(BorderWidth <= 0)BorderWidth = 1.0F; //defaults
					if(BorderColor == null)BorderColor = Color.Black; //defaults.
				}
			}
		}		
		
		#endregion
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("[BlockStyle BackgroundColor={0}, BorderColor={1}, BorderSize={2}, FillBackground={3}, Borders={4}]", base.BackgroundColor, _borderColor, _borderSize, _fillBackground, _borders);
		}


	}
}
