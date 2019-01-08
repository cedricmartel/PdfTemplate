using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Moon.PDFTemplate.XMAtributes;

namespace Moon.PDFTemplate
{
	/// <summary>
	/// Text block element. It's a Text box widh aditional features.
	/// 
	/// <para/>Attributes: height and 'TextBox' attributes
	/// 
	/// </summary>
	public class TextBlock : TextBox
	{
		//20130605 :: mellorasinxelas
		
		/// <summary>
		/// Text block height attr constant
		/// </summary>
		public const string HeightAttributeConstant="height";
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="text"></param>
		/// <param name="fontAttrs"></param>
		/// <param name="textAttrs"></param>
		public TextBlock(string text, XmlAttributeCollection fontAttrs, XmlAttributeCollection textAttrs):base( text, fontAttrs, textAttrs )
		{
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
		public TextBlock(string text,
		                 XmlAttributeCollection fontAttrs,
		                 XmlAttributeCollection textAttrs, List<Variable> vars):base( text, fontAttrs, textAttrs, vars )
		{
			init();
		}

		private void init()
		{
			//nothing
		}

		//20130604 :: mellorasinxelas
		/// <summary>
		/// Draws the textblock
		/// </summary>
		/// <param name="pdfDraw"></param>
		/// <param name="data"></param>
		public override void Draw(PDFDraw.IPDFDraw pdfDraw, System.Collections.IDictionary data)
		{
			//            StringBuilder txt = new StringBuilder(Text);
			//            foreach (Variable act in VAR)
			//            {
//					string var = act.Name;
//
//					object dataV = data[var]; //null if not exists. Reduce HASH access.
//
//					//first of all if its formatable. This system supports null values.
//					if(act is FormatableVariable){
//						object formatV = ((FormatableVariable)act).GetFormatedValue(dataV);
//						txt.Replace(var, formatV != null? formatV.ToString() : null );
//					}
//					else if (dataV != null)
//					{    //original
//						txt.Replace(var, dataV.ToString() );
//					}
//					else if(PDFTemplate.UseOptionalTags && !act.Optional){
//						//only NOT optional vars with OptionalTags ON are replaced with empty value.
//						txt.Replace(var, "");
//					}
			//            }
			//change to super gettext.
			pdfDraw.DrawBlockString(GetText( data ), X, Width, FontAttributes, TextAttrs);
		}


		//        public override void SetXandWidth(float x, float width)
		//        {
		//            this.x = x;
		//            this.width = width;
		//        }

		/// <summary>
		/// Gets textblock height
		/// </summary>
		/// <returns></returns>
		public override float GetHeight()
		{
			float height = PDFDraw.XmlHelper.GetFloatAttributeValue(HeightAttributeConstant, TextAttrs, -1);
			if (height == -1)
			{
				height = PDFDraw.XmlHelper.GetFloatAttributeValue(HeightAttributeConstant, FontAttributes, 10);
			}
			return height;
		}
		
		//        public string Text
		//        {
		//            get { return text; }
		//            set { text = value; }
		//        }
		//        public XmlAttributeCollection TextAttrs
		//        {
		//            get { return textAttrs; }
		//        }
	}
}
