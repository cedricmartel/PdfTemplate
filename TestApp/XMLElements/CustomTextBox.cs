/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 12/06/2013
 * Time: 13:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Xml;
using Moon.PDFTemplate;
using Moon.PDFTemplate.XMAtributes;

namespace TestApp.XMLElements
{
	/// <summary>
	/// Custom text box to check custom elements.
	/// Has modified constructor.
	/// 
	/// TestApp.XMLElements.CustomTextBox
	/// </summary>
	public class CustomTextBox : TextBox
	{
		
		
		public CustomTextBox(XmlAttributeCollection fontAttrs, XmlAttributeCollection textAttrs):base("", fontAttrs, textAttrs )
		{
			LoadTextAttr();
		}

		
		public CustomTextBox(
		               XmlAttributeCollection fontAttrs,
		               XmlAttributeCollection textAttrs,        List<Variable> vars):base( "", fontAttrs, textAttrs, vars )
		{
			LoadTextAttr();
		}
		
		protected void LoadTextAttr(){
			base.Text = Moon.PDFDraw.XmlHelper.GetAttributeValue(TextBox.TextAttributeConstant, TextAttrs, "");
		}
		
		/// <summary>
		/// If it's a free line and amount is ZERO, returns "".
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public override string GetText(System.Collections.IDictionary data)
		{
			
			if(data != null){
				
				bool special = false;
				object isFree = data["is_free_line"];
				if(isFree != null)special = Boolean.Parse( isFree.ToString());
				
				if(special){
					double? amount = null;
					try{
						amount = (double?)data[ "line_amount" ];
					}catch(Exception ex){
						Console.WriteLine(ex);
					}
					if(amount == null || !amount.HasValue || amount.Value == 0.0D)return "";// devolvemos que NO HAY TEXTO
				}
			}
			
			return base.GetText(data);
		}
		
		public override string ToString()
		{
			return base.ToString();
		}

	}
}
