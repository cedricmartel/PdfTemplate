/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 10/06/2013
 * Time: 19:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Xml;
using Moon.PDFDraw;

namespace Moon.PDFDrawItextSharp.StyleContainers
{
	/// <summary>
	/// TextBox Style. Only support backcolor.
	/// </summary>
	public class SingleTextBoxStyle : IStyleContainer
	{
		/// <summary>
		/// Text box style backcolor attr constant
		/// </summary>
		public const string BackgroundColorAttributeConstant="backgroundcolor";
		
				
		private Color? _backgroundColor = null;
		/// <summary>
		/// Background color.
		/// </summary>
		public Nullable<Color> BackgroundColor {
			get { return _backgroundColor; }
			set { _backgroundColor = value; }
		}
		
				
		/// <summary>
		/// Constructor
		/// </summary>
		public SingleTextBoxStyle()
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="backgroundColor"></param>
		public SingleTextBoxStyle(Color backgroundColor){
			this._backgroundColor = backgroundColor;
		}
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="attrs"></param>
		public SingleTextBoxStyle( XmlAttributeCollection attrs ){
			
			if(attrs != null){
				if( PDFDraw.Helper.GetAttributeValue( BackgroundColorAttributeConstant , attrs, null ) != null ){
				   	BackgroundColor = PDFDraw.Helper.GetAttributeColor(BackgroundColorAttributeConstant, attrs, "White");
				}           
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("[SingleTextBoxStyle BackgroundColor={0}]", _backgroundColor);
		}

	}
}
