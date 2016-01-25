using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Moon.PDFDraw;
using Moon.PDFTemplate.XMAtributes;

namespace Moon.PDFTemplate
{
    /// <summary>
    /// Image draw element
    /// <para/>Attributes: height, width, src
    /// <para/>20150203 :: melloraSinxelas :: Changed var visiblity. Modified properties. 
    /// <para/>20150914 :: melloraSinxelas :: Layouts: none, zoom, stretch. Supports absolute X,Y.
    /// </summary>
	public class Image : DrawElement
    {
        
    	/// <summary>
    	/// Image height attr constant
    	/// </summary>
		public const string HeightAttributeConstant="height";
    	
		/// <summary>
    	/// Image width attr constant
    	/// </summary>
		public const string WidthAttributeConstant="width";
    	
		/// <summary>
    	/// Image source attr constant
    	/// </summary>
		public const string SrcAttributeConstant="src";
		
		/// <summary>
    	/// Image absolute X
    	/// </summary>
		public const string AbsoluteXAttributeConstant="x";
		
		/// <summary>
    	/// Image absolute Y
    	/// </summary>
		public const string AbsoluteYAttributeConstant="y";
		
		
		#region 20150914 :: Image layout.
		
    	/// <summary>
    	/// Image layout attr constant. Values: none, strech,zoom
    	/// </summary>
		public const string ImageLayoutAttributeConstant="layout";
		
		/// <summary>
    	/// Image layout attr value constant. 
    	/// </summary>
		public const string NoneImageLayoutValueAttributeConstant="none";
		
		/// <summary>
    	/// Image layout attr value constant. 
    	/// </summary>
		public const string StrechImageLayoutValueAttributeConstant="stretch";
		
		/// <summary>
    	/// Image layout attr value constant. 
    	/// </summary>
		public const string ZoomImageLayoutValueAttributeConstant="zoom";
		
		/// <summary>
    	/// Image align attr constant. Supports: center, none.
    	/// </summary>
		public const string AlignAttributeConstant="align";
		
		#endregion
		
		
    	//private XmlAttributeCollection imgAttrs;
        //20130604 :: mellorasinxelas
    	/// <summary>
    	/// Constructor
    	/// </summary>
    	/// <param name="fontAttrs"></param>
    	/// <param name="imgAttrs"></param>
    	/// <param name="vars"></param>
    	public Image(XmlAttributeCollection fontAttrs, XmlAttributeCollection imgAttrs, List<Variable> vars):base( fontAttrs, imgAttrs, vars )
        {
//            FontAttributes = fontAttrs;
//            //this.imgAttrs = imgAttrs;
//            Attributes = imgAttrs;
//            
//            this.Vars = vars;
//           
            _init();
        }

        private void _init()
        {
            //this.Width = PDFDraw.Helper.GetFloatAttributeValue(WidthAttributeConstant, Attributes, 50);
            //20150914 :: If attributes have x attribute, attribute value overwrite default X in nexts steps.
            SetXandWidth(X, PDFDraw.Helper.GetFloatAttributeValue(WidthAttributeConstant, Attributes, 50) );
        }

        
		public override void SetXandWidth(float x, float width)
		{
			base.SetXandWidth(x, width);
		}
        
		//20130604 :: mellorasinxelas       
        
		/// <summary>
        /// Draws built-in image
        /// </summary>
        /// <param name="pdfDraw"></param>
        /// <param name="data"></param>
		public override void Draw(PDFDraw.IPDFDraw pdfDraw,IDictionary data)
        {
			//NOTE: Dont support format.           
        	StringBuilder src = new StringBuilder(Attributes[SrcAttributeConstant].Value);
            foreach (Variable act in VAR)
            {
                string var = act.Name;
            	if (data.Contains(var))
                {
                    src.Replace(var, data[var].ToString());
                }
                else if(PDFTemplate.UseOptionalTags && !act.Optional){
                	//only NOT optional vars with OptionalTags ON are replaced with empty value.
            		src.Replace(var, "");
                }   
            }                 

            
            //20150914 :: supports absolute X,Y.
            float absX = Moon.PDFDraw.Helper.GetFloatAttributeValue(AbsoluteXAttributeConstant, Attributes, -1);
			float absY = Moon.PDFDraw.Helper.GetFloatAttributeValue(AbsoluteYAttributeConstant, Attributes, -1);
			if (absX != -1 && absY != -1)
			{//new
				pdfDraw.DrawImage(absX, absY, /*src.ToString(),*/ Attributes);
			}
			else{//classic				
            	pdfDraw.DrawImage(X, src.ToString(), Attributes );
			}
			//---
        }
        //----

//        /// <summary>
//        /// Sets X and W to draw the image
//        /// </summary>
//        /// <param name="x"></param>
//        /// <param name="width"></param>
//        public override void SetXandWidth(float x, float width)
//        {
//            this.x = x;
//            this.width = width;
//        }

        /// <summary>
        /// Gets Height from image
        /// </summary>
        /// <returns></returns>
        public override float GetHeight()
        {
            return PDFDraw.Helper.GetFloatAttributeValue(HeightAttributeConstant, Attributes, 0);
        }
    }
}
