using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Moon.PDFDraw;
using Moon.PDFTemplate.XMAtributes;

namespace Moon.PDFTemplate
{
	/// <summary>
	/// Text box element. Basic text element.
	/// 
	/// <para/>20150203 :: melloraSinxelas :: Changed var visiblity. Modified properties. 
	/// <para/>Attributes: text, fontsize, varcontrol(any|no|all)
	/// </summary>
	public class TextBox : DrawElement
	{
		/// <summary>
		/// Variable values control. Allow to indicate conditions into the data.
		/// </summary>
		public enum VarValuesControl{
			/// <summary>
			/// Shows element only when all element vars have data.
			/// </summary>
			All,
			/// <summary>
			/// Shows element with built-in vars that have data.
			/// </summary>
			Yes,
			/// <summary>
			/// Show element with built-in vars with data and without data(shows var tag)
			/// </summary>
			No};
		
		/// <summary>
		/// Text box text attr constant
		/// </summary>
		public const string TextAttributeConstant = "text";
		
		/// <summary>
		/// Text box font size attr constant
		/// </summary>
		public const string FontSizeAttributeConstant="fontsize";
		
		/// <summary>
		/// Var control in text elements with vars. Values are any|no|all. If 'any' show data if there are any var value. When value is 'no' it's like as standard behaviour. If it's 'all', for show data it's required all vars with data.
		/// </summary>
		public const string VarControlAttributeConstant = "varcontrol";
		
		
		private string text = "";
		//private XmlAttributeCollection textAttrs;
		
		/// <summary>
		/// XML test attrs
		/// </summary>
		protected XmlAttributeCollection TextAttrs {
			get { return Attributes; }
		}
		
		
		private VarValuesControl _varControl = VarValuesControl.No;
		/// <summary>
		/// Var control status.
		/// </summary>
		protected TextBox.VarValuesControl VarControl {
			get { return _varControl; }
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="text"></param>
		/// <param name="fontAttrs"></param>
		/// <param name="textAttrs"></param>
		public TextBox(string text, XmlAttributeCollection fontAttrs, XmlAttributeCollection textAttrs):base( fontAttrs,  textAttrs, null)
		{
			Text = text;
//			FontAttributes = fontAttrs;
//			Attributes = textAttrs;
//			
			
			init();
		}

		//20130604 :: mellorasinxelas
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="text"></param>
		/// <param name="fontAttrs"></param>
		/// <param name="textAttrs"></param>
		/// <param name="vars"></param>
		public TextBox(string text,
		               XmlAttributeCollection fontAttrs,
		               XmlAttributeCollection textAttrs,        List<Variable> vars):base( fontAttrs,  textAttrs, vars)
		{
			Text = text;
			//FontAttributes = fontAttrs;
			//Attributes = textAttrs;
			
			//this.Vars = vars;

			init();
		}
		
		#region Modify 20130603
		
		/// <summary>
		/// Init and configure elemento. Loads varcontrol.
		/// </summary>
		private void init(){
			Width_percent = PDFDraw.XmlHelper.GetAttributeWidthPercent(Attributes);
			
			//load var control.
			if(Attributes != null){
				try{
					string vVarCtrl = PDFDraw.XmlHelper.GetAttributeValue( VarControlAttributeConstant, Attributes, null) ;
					if(vVarCtrl != null){
						this._varControl =
							(VarValuesControl)Enum.Parse( typeof( VarValuesControl), vVarCtrl, true);
					}
				}catch(Exception ex){
					Console.WriteLine(ex.ToString());
				}
			}
		}
		
		/// <summary>
		/// Draws this textbox
		/// </summary>
		/// <param name="pdfDraw"></param>
		/// <param name="data"></param>
		public override void Draw(PDFDraw.IPDFDraw pdfDraw, System.Collections.IDictionary data)
		{
			pdfDraw.DrawString( GetText(data), X, Width, FontAttributes, Attributes);
		}

		/// <summary>
		/// Gets text box text (processing vars).
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public virtual string GetText(System.Collections.IDictionary data)
		{
			
			StringBuilder txt = new StringBuilder(Text);
			
			bool anyHasValue = false;
			foreach (Variable act in VAR)
			{
				string var = act.Name;
				
				object dataV = data[var]; //null if not exists. Reduce HASH access.
				
				//var control...
				if(dataV == null && VarControl == VarValuesControl.All){
					return ""; //no value (required all).
				}				
				if(dataV != null){
					anyHasValue = true;
				}
				//---
				
				//first of all if its formatable. This system supports null values.
				if(act is FormatableVariable){
					object formatV = ((FormatableVariable)act).GetFormatedValue(dataV);
					txt.Replace(var, formatV != null? formatV.ToString() : null );
				}
				else if (dataV != null)
				{    //original
					txt.Replace(var, dataV.ToString() );
				}
				else if(PDFTemplate.UseOptionalTags && !act.Optional){
					//only NOT optional vars with OptionalTags ON are replaced with empty value.
					txt.Replace(var, "");
				}
			}
			
			if(!anyHasValue && VarControl == VarValuesControl.Yes){
				//no var values... (required one or more)
				return "";
			}
			
			return txt.ToString();
		}
		
		#endregion
		
//		/// <summary>
//		/// Sets X and W
//		/// </summary>
//		/// <param name="x"></param>
//		/// <param name="width"></param>
//		public override void SetXandWidth(float x, float width)
//		{
//			this.x = x;
//			this.width = width;
//		}

		/// <summary>
		/// Gets textbox height
		/// </summary>
		/// <returns></returns>
		public override float GetHeight()
		{
			return PDFDraw.XmlHelper.GetFloatAttributeValue( FontSizeAttributeConstant, FontAttributes, 10);
		}
		
		/// <summary>
		/// Gets / Sets textbox text.
		/// </summary>
		public virtual string Text
		{
			get { return text; }
			set { text = value; }
		}
		
		
		
	}
}
