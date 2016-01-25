using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Moon.PDFTemplate;

namespace Moon.PDFDraw
{

	/// <summary>
	/// Document groups for filter operations.
	/// </summary>
	public enum DocumentGroup {
		/// <summary>
		/// Document header
		/// </summary>
		Header,
		/// <summary>
		/// Document main body
		/// </summary>
		Body,
		/// <summary>
		/// Document footer
		/// </summary>
		Footer,
		/// <summary>
		/// Document table data
		/// </summary>
		Table,
		/// <summary>
		/// Document loop region
		/// </summary>
		Loop
	};

	/// <summary>
	/// PDF drawer available operations.
	/// <para/>20150914 :: Update interface. 
	/// </summary>
	public interface IPDFDraw
	{
		
		/// <summary>
		/// Draws a string
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="width"></param>
		/// <param name="fontAttrs"></param>
		/// <param name="textAttrs"></param>
		void DrawString(string txt, float width, XmlAttributeCollection fontAttrs, XmlAttributeCollection textAttrs);
		
		/// <summary>
		/// Draws a string
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="x"></param>
		/// <param name="width"></param>
		/// <param name="fontAttrs"></param>
		/// <param name="textAttrs"></param>
		void DrawString(string txt, float x, float width, XmlAttributeCollection fontAttrs, XmlAttributeCollection textAttrs);
		
		/// <summary>
		/// Draws a string
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="fontAttrs"></param>
		/// <param name="textAttrs"></param>
		void DrawString(string txt, float x, float y, float width, XmlAttributeCollection fontAttrs, XmlAttributeCollection textAttrs);
		
		/// <summary>
		/// Draws a vertical line
		/// </summary>
		/// <param name="x_start"></param>
		/// <param name="x_end"></param>
		/// <param name="lineAttrs"></param>
		void DrawVerticalLine(float x_start, float x_end, XmlAttributeCollection lineAttrs);
		
		/// <summary>
		/// Draws a block string
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="x"></param>
		/// <param name="width"></param>
		/// <param name="fontAttrs"></param>
		/// <param name="textAttrs"></param>
		void DrawBlockString(string txt, float x, float width,XmlAttributeCollection fontAttrs, XmlAttributeCollection textAttrs);

		
		/// <summary>
		/// draw at x and CurrentY
		/// </summary>
		/// <param name="x"></param>
		/// <param name="imageAttrs"></param>
		/// 
		void DrawImage(float x, XmlAttributeCollection imageAttrs);
		/// <summary>
		/// draw at absolute position x, y
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="imageAttrs"></param>
		void DrawImage(float x, float y, XmlAttributeCollection imageAttrs);
		
		/// <summary>
		/// Draws a image
		/// </summary>
		/// <param name="x"></param>
		/// <param name="src"></param>
		/// <param name="imageAttrs"></param>
		void DrawImage(float x, string src, XmlAttributeCollection imageAttrs);

		/// <summary>
		/// Creates a new page
		/// </summary>
		void NextPage();
		
		/// <summary>
		/// Creates a new row
		/// </summary>
		/// <param name="group"></param>
		void NextRow( DocumentGroup group);
		
		/// <summary>
		/// Creates a new row
		/// </summary>
		/// <param name="height"></param>
		/// <param name="group"></param>
		void NextRow(float height, DocumentGroup group);
		
		/// <summary>
		/// Gets current Y
		/// </summary>
		/// <returns></returns>
		float CurrentY();
		
		/// <summary>
		/// Gets if has more Y space
		/// </summary>
		/// <param name="height"></param>
		/// <param name="group"></param>
		/// <returns></returns>
		bool isNoMoreY(float height, DocumentGroup group);
		
		/// <summary>
		/// Sets Y
		/// </summary>
		/// <param name="y"></param>
		void SetY(float y);
		
		/// <summary>
		/// Allow set background image.
		/// </summary>
		/// <param name="backgroundImage"></param>
		void SetBackgroundImage( BackgroundImageDefinition backgroundImage );
	}

}
