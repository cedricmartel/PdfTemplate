/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 04/06/2013
 * Time: 17:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Xml;
using Moon.PDFTemplate.XMAtributes;

namespace Moon.PDFTemplate.CustomElements
{
	/// <summary>
	/// Basic implementation of CustomElement for design user pdf elements. This clases will be charged from external assemblies.
	/// <para/>XML format:  /custom  classname="full_classname" .... /custom\
	/// <para/>20150203 :: melloraSinxelas :: Changed var visiblity. Modified properties. 
	/// Moon.PDFTemplate.CustomElements.DefaultCustomElement
	/// </summary>
	public abstract class DefaultCustomElement : DrawElement
	{
		/// <summary>
		/// Custom class name for custom element.
		/// </summary>
		public const string ClassNameAttributeCustomElement = "classname";
		
		
		/// <summary>
		/// Element XML attributes . Alias of 'Attributes'
		/// </summary>
		protected XmlAttributeCollection ElementAttrs {
			get { /*return _elementAttrs;*/return base.Attributes; }
		}
        
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fontAttrs"></param>
		/// <param name="elementAttrs"></param>
		public DefaultCustomElement(XmlAttributeCollection fontAttrs, XmlAttributeCollection elementAttrs):base(fontAttrs, elementAttrs, null)
        {           
//            FontAttributes = fontAttrs;
//            //this._elementAttrs = elementAttrs;        
//			base.Attributes = elementAttrs;            
        }

       /// <summary>
       /// Constructor
       /// </summary>
       /// <param name="fontAttrs"></param>
       /// <param name="elementAttrs"></param>
       /// <param name="vars"></param>
       public DefaultCustomElement(  XmlAttributeCollection fontAttrs, XmlAttributeCollection elementAttrs, List<Variable> vars):base(fontAttrs, elementAttrs, vars)
        {
//        	FontAttributes = fontAttrs;
//           // this._elementAttrs = elementAttrs;
//           base.Attributes = elementAttrs;       
//            base.Vars = vars;
        }
	}
}
