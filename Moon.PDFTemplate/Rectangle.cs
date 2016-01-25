/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 04/06/2013
 * Time: 18:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Xml;

namespace Moon.PDFTemplate
{
	/// <summary>
	/// Rectangle.
	/// You cant define fill="yes|no" if you want that cursor jump to end or continue in the last position.
	/// 
	/// TODO en desarrollo. NO USAR.
	/// </summary>
	public class Rectangle : DrawElement
	{
		public const string FillTagConstant="fill";
		
		public const string InitialXConstant="x1";
		public const string FinalXConstant="x2";
		
		public const string HeightConstant="height";
		
		
		private XmlAttributeCollection _rectangleAttrs;
		
		protected  XmlAttributeCollection RectangleAttrs {
			get { return _rectangleAttrs; }
		}
		
        private float x1 = -1;
        private float x2 = -1;
        protected float InitialX{
        	get{
        		return x1;
        	}
		}
                
        protected float FinalX{
        	get{
        		return x2;
        	}
		}
        
		private bool _fill = false;
		
		protected bool Fill {
			get { return _fill; }
		}
		
		private float _height = -1;
		
		protected float Height {
			get { return _height; }
		}
				
		
        
        public Rectangle(XmlAttributeCollection fontAttrs, XmlAttributeCollection rectAttrs)
        {
            this._rectangleAttrs = rectAttrs;
            FontAttributes = fontAttrs;
            
            init();
        }

        
        private void init()
        {
            x1 = PDFDraw.Helper.GetFloatAttributeValue("x1", RectangleAttrs, -1);
            x2 = PDFDraw.Helper.GetFloatAttributeValue("x2", RectangleAttrs, -1);
            if (x1 == -1 && x2 == -1)
            {
                Width_percent = PDFDraw.Helper.GetAttributeWidthPercent(RectangleAttrs);
            }
            
            _height = PDFDraw.Helper.GetFloatAttributeValue(HeightConstant, RectangleAttrs, -1);
            
            _fill = PDFDraw.Helper.GetBooleanValue( PDFDraw.Helper.GetAttributeValue( FillTagConstant, RectangleAttrs, "false" ), false );
        }

      
        
        public override void Draw(PDFDraw.IPDFDraw pdfDraw, System.Collections.Hashtable data)
        {
            pdfDraw.DrawHorizontalLine(this.x1, this.x2, RectangleAttrs);           
        }
        

        public override void SetXandWidth(float x, float width)
        {
            base.x = x;
            base.width = width;
            this.x1 = x;
            this.x2 = x + width;
        }

        
        public override float GetHeight()
        {
             
            float lineH = PDFDraw.Helper.GetFloatAttributeValue("fontsize", FontAttributes, 10);
            if(Fill){
            	return lineH * 2;
            }
            else{
            	return lineH;
            }
        }
		
	}
}
