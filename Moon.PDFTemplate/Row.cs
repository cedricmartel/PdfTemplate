using System;
using System.Collections.Generic;
 
using System.Text;

namespace Moon.PDFTemplate
{
    /// <summary>
    /// Row element.
    /// 
    /// <para/>Attributes: NONE
    /// </summary>
	public class Row
    {
        private float x1;
        private float x2;
        private float height = 0;
        
        private List<DrawElement> drawElement = new List<DrawElement>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        public Row(float x1, float x2)
        {
            this.x1 = x1;
            this.x2 = x2;
        }
        /// <summary>
        /// Start position
        /// </summary>
        public float X1 { get { return x1; } }
        
        /// <summary>
        /// End position
        /// </summary>
        public float X2 { get { return x2; } }
        
        /// <summary>
        /// Row height
        /// </summary>
        public float Height
        {
            get { return height; }
        }
        
        /// <summary>
        /// List of drawable elements
        /// </summary>
        public List<DrawElement> DrawElements
        {
            get { return drawElement; }
        }
        // 20130604 :: mellorasinxelas
        /// <summary>
        /// Add a new element
        /// </summary>
        /// <param name="drawElement"></param>
        public void AddDrawElement(DrawElement drawElement)
        {
            //null control.
            if(drawElement == null){
            	Console.WriteLine("Draw element null. Not added into row element.");
            	return;
            }
            //--
        	DrawElements.Add(drawElement);
        }
        
        /// <summary>
        /// Gets row width
        /// </summary>
        public void CalculateWidth()
        {
            float width = X2 - X1;
            float start_x = X1;
            foreach (DrawElement drawElement in DrawElements)
            {
                if (drawElement is Line)
                {
                    if (drawElement.Width_percent != -1)
                    {
                        float _width = width * drawElement.Width_percent;
                        drawElement.SetXandWidth(start_x, _width);
                        //Console.WriteLine("Line width: " + _width);
                    }
                }
                else if (drawElement.Width_percent != -1)
                {
                    // ignore width_percent == -1
                    // width
                    float _width = width * drawElement.Width_percent;
                    drawElement.SetXandWidth(start_x, _width);
                    start_x = start_x + _width;
                }
                else if (drawElement.Width != -1)
                {
                    drawElement.SetXandWidth(start_x, drawElement.Width);
                    start_x = start_x + drawElement.Width;
                }
                // height
                if (drawElement.GetHeight() > height)
                {
                    height = drawElement.GetHeight();
                }
            }
        }
    }
}
