using System;
using System.Collections.Generic;
using System.Drawing;
using iTextSharp.text;
using Moon.PDFDraw;
using System.Xml;
using Moon.PDFDrawItextSharp.StyleContainers;
using Moon.PDFTemplate;

namespace Moon.PDFDrawItextSharp
{
	/// <summary>
	/// PDF drawer using ItextSharp library
	/// </summary>
	public class PDFDrawItextSharp : IPDFDraw
	{
		
		
		//20130605 :: mellorasinxelas
		/// <summary>
		/// Align XML attribute constant
		/// </summary>
		public const string AlignAttributeConstant="align";
		
		/// <summary>
		/// Color XML attribute constant
		/// </summary>
		public const string ColorAttributeConstant="color";
		
		/// <summary>
		/// Backcolor XML attribute constant
		/// </summary>
		public const string BackgroundColorAttributeConstant="backgroundcolor";
		
		/// <summary>
		/// Border XML attribute constant
		/// </summary>
		public const string BorderAttributeConstant="border";
		
		/// <summary>
		/// Line thinckness XML attribute constant
		/// </summary>
		public const string LineThicknessAttributeConstant="linethickness";
		//----
		
		#region melloraSinxelas 20150914
		
			/// <summary>
    	/// Image absolute X
    	/// </summary>
		public const string AbsoluteXAttributeConstant="x";
		
		/// <summary>
    	/// Image absolute Y
    	/// </summary>
		public const string AbsoluteYAttributeConstant="y";
		
		
		/// <summary>
    	/// Layout attr constant.
    	/// </summary>
		public const string LayoutAttributeConstant="layout";
		
			/// <summary>
    	/// Height attr constant
    	/// </summary>
		public const string HeightAttributeConstant="height";
    	
		/// <summary>
    	/// Width attr constant
    	/// </summary>
		public const string WidthAttributeConstant="width";
		
		#endregion
		
		/// <summary>
		/// Only build first time the background image. Use the same information to fit it.
		/// @since 20130607
		/// </summary>
		public static bool UseCachedBackgroundImage = true;
		
		
		private iTextSharp.text.Document _pdfDoc;
		
		internal iTextSharp.text.Document PdfDoc {
			get { return _pdfDoc; }
		}

		private float margin_top = 20;
		private float margin_bottom = 20;
		private float margin_left = 20;
		private float margin_right = 20;
		private float absolute_footer_height = 0;//for absolute footer control.
		/// <summary>
		/// Space reserved to footer. 0 by default.
		/// </summary>
		public float AbsoluteFooterHeight {
			get { return absolute_footer_height; }
		}
		
		private float current_y;
		private float current_x;
		private string fontpath = "";
		
		//20130607
		private BackgroundImageDefinition _backgroundImage = null;
		//-----
		
		private iTextSharp.text.pdf.BaseFont baseFont;
		
		
		private iTextSharp.text.Font font;
		private iTextSharp.text.Rectangle pageSize;
		private iTextSharp.text.pdf.PdfWriter _iPDFWriter;
		
		/// <summary>
		/// Internal PDF writer.
		/// </summary>
		protected iTextSharp.text.pdf.PdfWriter PDFWriter {
			get { return _iPDFWriter; }
		}
		private iTextSharp.text.pdf.PdfContentByte iPDFContent;


		
		#region constructor and initializators
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="pageSize"></param>
		/// <param name="marginLeft"></param>
		/// <param name="marginRight"></param>
		/// <param name="marginTop"></param>
		/// <param name="marginBottom"></param>
		/// <param name="stream"></param>
		/// <param name="fontpath"></param>
		public PDFDrawItextSharp(iTextSharp.text.Rectangle pageSize, float marginLeft, float marginRight, float marginTop, float marginBottom, System.IO.Stream stream, string fontpath)
		{
			CommonConstructor( pageSize, marginLeft, marginRight, marginTop, marginBottom, stream, fontpath, null );
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="pageSize"></param>
		/// <param name="marginLeft"></param>
		/// <param name="marginRight"></param>
		/// <param name="marginTop"></param>
		/// <param name="marginBottom"></param>
		/// <param name="stream"></param>
		public PDFDrawItextSharp(iTextSharp.text.Rectangle pageSize, float marginLeft, float marginRight, float marginTop, float marginBottom,  System.IO.Stream stream)
		{
			CommonConstructor( pageSize, marginLeft, marginRight, marginTop, marginBottom, stream, null, null );
		}
		
		
		private void CommonConstructor( iTextSharp.text.Rectangle pageSize, float marginLeft, float marginRight, float marginTop, float marginBottom, System.IO.Stream stream, string fontpath, BackgroundImageDefinition bImageDefinition  ){
			
			if(this.fontpath != null)this.fontpath = fontpath;
			
			this.margin_left = marginLeft;
			this.margin_right = marginRight;
			this.margin_top = marginTop;
			this.margin_bottom = marginBottom;
			this.pageSize = pageSize;
			
			this._backgroundImage = bImageDefinition;
			
			_pdfDoc = new iTextSharp.text.Document(pageSize, margin_left, margin_right, margin_top, margin_bottom);
			_iPDFWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(PdfDoc, stream);
			
			PdfDoc.Open();
			iPDFContent = PDFWriter.DirectContent;
			
			_init();
			initRow();
		}
		
		private void _init()
		{
			if (fontpath != null && fontpath != string.Empty)
			{
				baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(fontpath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED);
			}
			else
			{
				baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.COURIER, iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.EMBEDDED);
			}
			
			font = new iTextSharp.text.Font(baseFont, 10);
			iPDFContent.SetFontAndSize(font.BaseFont, font.Size);

			
			//20130607 :: add backgroung image
			if(_backgroundImage != null){
				DrawBackgroundImage(); // image for first page.
			}
			
			
			//20130613 :: set doc properties by default...
			try{PdfDoc.AddCreator( DocumentProperties.CreatorConstant );}catch(Exception){}
			
		}

		#endregion
		
		
		#region 20130606 :: Add to absolute footer.
		
		/// <summary>
		/// Allow reserve space in pages for footer.
		/// </summary>
		/// <param name="height"></param>
		public void ReserveSpaceToFooter( float height ){
			if(height < 0)return;
			
			absolute_footer_height = height;
		}
		
		#endregion
		
		/// <summary>
		/// Set document properties stored in the PDF document.
		/// </summary>
		/// <param name="docProperties"></param>
		public void SetDocumentProperties( DocumentProperties docProperties ){
			if(docProperties == null)return;
			try{
				if(docProperties.Author != null)PdfDoc.AddAuthor( docProperties.Author );
				
				PdfDoc.AddCreationDate();
				PdfDoc.AddCreator( docProperties.Creator );
				
				if(docProperties.Keywords != null)PdfDoc.AddKeywords( docProperties.Keywords );
				if(docProperties.Title != null)PdfDoc.AddTitle( docProperties.Title );
				
				if(docProperties.Subject != null)PdfDoc.AddSubject( docProperties.Subject );
			}catch(Exception ex){
				Console.WriteLine(ex);
			}
		}
		
		/// <summary>
		/// Closes PDF
		/// </summary>
		public void Close()
		{
			PdfDoc.Close();
		}
		
		/// <summary>
		/// Get/set current Y
		/// </summary>
		public float Current_y
		{
			get { return current_y; }
			set { current_y = value; }
		}
		
		/// <summary>
		/// Get/set current X
		/// </summary>
		public float Current_x
		{
			get { return current_x; }
			set { current_x = value; }
		}
		
		/// <summary>
		/// Get/set margin top
		/// </summary>
		public float Margin_top
		{
			get { return margin_top; }
			set { margin_top = value; }
		}

		/// <summary>
		/// Get/set margin bottom
		/// </summary>
		public float Margin_bottom
		{
			get { return margin_bottom; }
			set { margin_bottom = value; }
		}

		/// <summary>
		/// Get/set margin left
		/// </summary>
		public float Margin_left
		{
			get { return margin_left; }
			set { margin_left = value; }
		}

		/// <summary>
		/// Get/set margin right
		/// </summary>
		public float Margin_right
		{
			get { return margin_right; }
			set { margin_right = value; }
		}
		// --------------------------------------------------------------------
		private void MoveX(float width)
		{
			Current_x += width;
		}
		/// <summary>
		/// move y to next row with default font height
		/// </summary>
		public void NextRow(DocumentGroup group)
		{
			NextRow(font.Size + 1, group);
		}
		
		/// <summary>
		/// move y to next row with height
		/// 20130606 :: Modified to absolute footer.
		/// </summary>
		/// <param name="height"></param>
		/// <param name="group"></param>
		public void NextRow(float height, DocumentGroup group)
		{
			Current_y -= height + 1;
			Current_x = Margin_left;
			
			if(group != DocumentGroup.Footer){
				if(Current_y < Margin_bottom)
				{
					NextPage();
				}
			}
			#if DEBUG
			//Console.WriteLine("NextRow() Current_y: " + Current_y);
			#endif
		}
		/// <summary>
		/// check is no more space for next row with default font height
		/// </summary>
		/// <returns></returns>
		public bool isNoMoreY( DocumentGroup group)
		{
			return isNoMoreY(font.Size, group);
		}
		
		/// <summary>
		/// check is no more space for next row with height
		/// true, no more spare
		/// false, still got space
		/// 
		/// 20130606 :: mellorasinxelas to abs footer
		/// </summary>
		/// <param name="height"></param>
		/// <param name="group"></param>
		/// <returns></returns>
		public bool isNoMoreY(float height, DocumentGroup group)
		{
			
			switch(group){
				case DocumentGroup.Body:
				case DocumentGroup.Header:
				case DocumentGroup.Loop:
				case DocumentGroup.Table:
					return (Current_y - height <= (Margin_bottom + AbsoluteFooterHeight) );//control with footer
				case DocumentGroup.Footer:
					return (Current_y - height <= Margin_bottom );//without footer h.
					default: return false;
			}
			
			
		}
		/// <summary>
		/// is the Current_x already reach to the right side
		/// </summary>
		/// <returns></returns>
		public bool isNoMoreX( DocumentGroup group)
		{
			bool r = false;
			if (Current_x >= pageSize.Width - Margin_right)
			{
				r = true;
			}
			return r;
		}
		
		
		/// <summary>
		/// from the current_x, can it fit a width?
		/// </summary>
		/// <param name="width"></param>
		/// <param name="group"></param>
		/// <returns></returns>
		public bool canFitWidth(float width, DocumentGroup group)
		{
			bool r = true;
			if (Current_x + width > pageSize.Width - Margin_right)
			{
				r = false;
			}
			return r;
		}
		
		/// <summary>
		/// Creates next page
		/// </summary>
		public void NextPage()
		{
			PdfDoc.NewPage();
			
			//20130607 :: Add backgroun image control.
			if(_backgroundImage != null){
				DrawBackgroundImage( );
			}
			//----
			
			initRow();
		}
		
		/// <summary>
		/// Gets current Y
		/// </summary>
		/// <returns></returns>
		public float CurrentY()
		{
			return Current_y;
		}
		
		/// <summary>
		/// Sets current Y
		/// </summary>
		/// <param name="y"></param>
		public void SetY(float y)
		{
			Current_y = y;
		}
		/// <summary>
		/// Add a blank page (without background image)
		/// </summary>
		public void BlankPage()
		{
			PdfDoc.Add(new iTextSharp.text.Chunk(""));
		}
		/// <summary>
		/// set x, y to the top of the page with margin top and margin left
		/// </summary>
		private void initRow()
		{
			// set cursor to upper left of page
			Current_y = pageSize.Height - margin_top;
			Current_x = margin_left;
		}
		



		//---------------------------------------------------------------------
		// implement interface from IPDFDraw
		/// <summary>
		/// Tested
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="fontAttrs"></param>
		/// <param name="textAttrs"></param>
		public void DrawString(string txt, XmlAttributeCollection fontAttrs, XmlAttributeCollection textAttrs)
		{
			//only calc width
			iTextSharp.text.Font font = CreateFontFromAttribute(fontAttrs);
//			int align = PDFDrawItextSharpHelper.Align(Moon.PDFDraw.Helper.GetAttributeValue(AlignAttributeConstant, textAttrs, "Near"));
			iTextSharp.text.pdf.FontSelector selector = FontSelector(font);
			iTextSharp.text.Phrase phrase = selector.Process(txt);
			
			float width = 0;
			foreach (iTextSharp.text.Chunk chunk in phrase.Chunks)
			{
				width += chunk.GetWidthPoint();
			}

			//SingleTextBoxStyle textBoxStyle = new SingleTextBoxStyle( textAttrs );
			
			//_DrawString(phrase, Current_x, null, width, align, textBoxStyle );
			_DrawString( txt, Current_x, null, width, fontAttrs,  textAttrs);
		}
		
		/// <summary>
		/// Tested. Interface.
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="width"></param>
		/// <param name="fontAttrs"></param>
		/// <param name="textAttrs"></param>
		public void DrawString(string txt, float width, XmlAttributeCollection fontAttrs, XmlAttributeCollection textAttrs)
		{
			//DrawString(txt, Current_x, width, fontAttrs, textAttrs);
			_DrawString( txt,  Current_x, null,  width,  fontAttrs,  textAttrs);
		}

		
		/// <summary>
		/// Tested. Interface.
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="x"></param>
		/// <param name="width"></param>
		/// <param name="fontAttrs"></param>
		/// <param name="textAttrs"></param>
		public void DrawString(string txt, float x, float width, XmlAttributeCollection fontAttrs, XmlAttributeCollection textAttrs)
		{
//			// this was call from TextBox
//			//fontsize, fontstyle, fontcolor
//			iTextSharp.text.Font font = CreateFontFromAttribute(fontAttrs);
//			int align = PDFDrawItextSharpHelper.Align(Moon.PDFDraw.Helper.GetAttributeValue(AlignAttributeConstant, textAttrs, "Left"));
//			iTextSharp.text.pdf.FontSelector selector = FontSelector(font);
//			iTextSharp.text.Phrase phrase = selector.Process(txt);
//
//			SingleTextBoxStyle textBoxStyle = new SingleTextBoxStyle( textAttrs );
//
//			DrawString(phrase, x, null, width, align, textBoxStyle );
			_DrawString( txt,  x, null,  width,  fontAttrs,  textAttrs);
		}
		
		/// <summary>
		/// Tested. Interface.
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="fontAttrs"></param>
		/// <param name="textAttrs"></param>
		public void DrawString(string txt, float x, float y, float width, XmlAttributeCollection fontAttrs, XmlAttributeCollection textAttrs)
		{
			_DrawString( txt,  x,  y,  width,  fontAttrs,  textAttrs);
		}
		
		/// <summary>
		/// Common string drawer
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="fontAttrs"></param>
		/// <param name="textAttrs"></param>
		protected void _DrawString(string txt, float x, float? y, float width, XmlAttributeCollection fontAttrs, XmlAttributeCollection textAttrs)
		{
			//fontsize, fontstyle, fontcolor
			iTextSharp.text.Font font = CreateFontFromAttribute(fontAttrs);
			int align = PDFDrawItextSharpHelper.Align(Moon.PDFDraw.Helper.GetAttributeValue(AlignAttributeConstant, textAttrs, "Left"));
			iTextSharp.text.pdf.FontSelector selector = FontSelector(font);
			iTextSharp.text.Phrase phrase = selector.Process(txt);

			SingleTextBoxStyle textBoxStyle = new SingleTextBoxStyle( textAttrs );
			
			//change to here to reduce functions
			if(y == null || !y.HasValue) y = Current_y;
			
			//PDFDrawItextSharpUtils.DrawString(iPDFContent, phrase, x, y.Value, width, align, textBoxStyle);
			DrawString(iPDFContent, phrase, x, y.Value, width, align, textBoxStyle);
			MoveX(width);
		}

		/// <summary>
		/// Tested. Main draw operation.
		/// 
		/// 20130718 :: Add Arabic support
		/// </summary>
		/// <param name="cb"></param>
		/// <param name="phrase"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="ELEMENT_ALIGN"></param>
		/// <param name="textBoxStyle"></param>
		public void DrawString(iTextSharp.text.pdf.PdfContentByte cb, iTextSharp.text.Phrase phrase,
		                       float x, float y, float width, int ELEMENT_ALIGN, IStyleContainer textBoxStyle)
		{
			iTextSharp.text.pdf.ColumnText column = new iTextSharp.text.pdf.ColumnText(cb);

			//add style, only SingleTextBoxStyle supported
			if (textBoxStyle != null && textBoxStyle is SingleTextBoxStyle)
			{
				Color? bckCol = ((SingleTextBoxStyle)textBoxStyle).BackgroundColor;
				if (bckCol != null && bckCol.HasValue)
				{
					if (phrase != null)
					{
						BaseColor color = new BaseColor(bckCol.Value);
						foreach (iTextSharp.text.Chunk act in phrase.Chunks)
						{
							act.SetBackground(color);//sets the same backcolor to the text.
						}
					}
				}
			}

			//column.SetSimpleColumn(x, y, x + width, (y + chunk.Font.Size + (chunk.Font.Size / 2) + 2));
			column.SetSimpleColumn(x, y - 2, (x + width), (y + 18));

			column.AddText(phrase);
			column.Alignment = ELEMENT_ALIGN;

			//20130718 :: Arabic support
			if (DetectRightToLeft && IsArabicHebrewText(phrase.Content))
			{
				column.RunDirection = iTextSharp.text.pdf.PdfWriter.RUN_DIRECTION_RTL;
			}
			//---

			column.Go();
		}
		
		


		/// <summary>
		/// Draws a block string
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="width"></param>
		/// <param name="fontAttrs"></param>
		/// <param name="textAttrs"></param>
		public void DrawBlockString(string txt, float width,
		                            XmlAttributeCollection fontAttrs, XmlAttributeCollection textAttrs)
		{
			DrawBlockString(txt, Current_x, width, fontAttrs, textAttrs);
		}
		
		
		/// <summary>
		/// Draws a block string
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="x"></param>
		/// <param name="width"></param>
		/// <param name="fontAttrs"></param>
		/// <param name="textAttrs"></param>
		public void DrawBlockString(string txt, float x, float width,
		                            XmlAttributeCollection fontAttrs, XmlAttributeCollection textAttrs)
		{
			iTextSharp.text.Font font = CreateFontFromAttribute(fontAttrs);
			int align = PDFDrawItextSharpHelper.Align(
				Moon.PDFDraw.Helper.GetAttributeValue(AlignAttributeConstant, textAttrs, "Near"));
			
			iTextSharp.text.Chunk chunk = new iTextSharp.text.Chunk(txt, font);
			
			//20130605 :: mellorasinxelas
			BlockStyle backgroundDef = new BlockStyle( textAttrs );//load text background values.
			
			if(backgroundDef.BackgroundColor != null){
				chunk.SetBackground( new BaseColor(backgroundDef.BackgroundColor.Value) );//sets the same backcolor to the text.
			}
			//----
			float height = Moon.PDFDraw.Helper.GetFloatAttributeValue( "height", textAttrs, font.Size + (font.Size / 2));

			DrawBlockString(chunk, x, width, align, height, backgroundDef);
		}
		

		
		/// <summary>
		/// 20130610 :: Add TextBackground modifiers to paint backgrounds.
		/// 20130718 :: Add arabic support
		/// </summary>
		/// <param name="chunk"></param>
		/// <param name="x"></param>
		/// <param name="width"></param>
		/// <param name="ELEMENT_ALIGN"></param>
		/// <param name="height"></param>
		/// <param name="txtBackgroundAttrs"></param>
		public void DrawBlockString(iTextSharp.text.Chunk chunk, float x, float width, int ELEMENT_ALIGN, float height, BlockStyle txtBackgroundAttrs )
		{
			//20130610 :: mellorasinxelas
			iTextSharp.text.pdf.ColumnText column = new iTextSharp.text.pdf.ColumnText(iPDFContent);
			
			float offsetTextInit = 0.0F;
			if(txtBackgroundAttrs != null && (txtBackgroundAttrs.BackgroundColor != null || txtBackgroundAttrs.BorderColor != null)){
				
				iPDFContent.SaveState();
				
				bool hasBorder = false;
				bool usePath = false;
				bool hasBackground = false;
				
				/*
				public const int TOP_BORDER = 1;
				public const int BOTTOM_BORDER = 2;
				public const int LEFT_BORDER = 4;
				public const int RIGHT_BORDER = 8;
				public const int NO_BORDER = 0;
				 */
				
				//iTextSharp.text.Rectangle rec = new iTextSharp.text.Rectangle( x, current_y, width, height ); -> no runs
				
				//process border
				if(txtBackgroundAttrs.BorderColor != null && txtBackgroundAttrs.Borders != iTextSharp.text.Rectangle.NO_BORDER){
					iPDFContent.SetColorStroke( new BaseColor( txtBackgroundAttrs.BorderColor.Value ) );
					if(txtBackgroundAttrs.BorderWidth > 0.0F){
						iPDFContent.SetLineWidth( txtBackgroundAttrs.BorderWidth ); //add border width
						//rec.BorderWidth = txtBackgroundAttrs.BorderWidth;	-> no runs
						offsetTextInit = txtBackgroundAttrs.BorderWidth + 1; //add offset.
						hasBorder = true;
					}
					
					//process each border..
					if(txtBackgroundAttrs.Borders != 15){
						usePath = true; //paint border with lines..
					}
				}
				
				//process background
				if(txtBackgroundAttrs.BackgroundColor != null && txtBackgroundAttrs.FillBackground){
					iPDFContent.SetColorFill( new BaseColor( txtBackgroundAttrs.BackgroundColor.Value ) );
					//rec.BackgroundColor = new BaseColor( txtBackgroundAttrs.BackgroundColor.Value ); -> no runs
					hasBackground = true;
				}
				
				//iPDFContent.Rectangle( rec );//draw rectangle -> no runs
				if ((hasBorder && !usePath) || hasBackground){
					//put rectangle
					iPDFContent.Rectangle( x, current_y, width, height );//draw rectangle
				}
				
				if( (hasBorder && !usePath ) && hasBackground){
					iPDFContent.ClosePathFillStroke();
				}
				else if(hasBackground){
					iPDFContent.Fill();
				}
				else if(hasBorder && !usePath){
					iPDFContent.ClosePathStroke();
				}
				else{
					//nothing. If it has border color but hasn't border.
				}
				
				if(usePath){
					//need paint each border
					
					//first top
					if((txtBackgroundAttrs.Borders & iTextSharp.text.Rectangle.TOP_BORDER) == iTextSharp.text.Rectangle.TOP_BORDER ){
						iPDFContent.MoveTo(x, Current_y + height);
						iPDFContent.LineTo( x + width, Current_y + height);
						iPDFContent.Stroke();
					}
					//left
					if((txtBackgroundAttrs.Borders & iTextSharp.text.Rectangle.LEFT_BORDER) == iTextSharp.text.Rectangle.LEFT_BORDER ){
						iPDFContent.MoveTo(x, Current_y);
						iPDFContent.LineTo( x, Current_y + height);
						iPDFContent.Stroke();
					}
					//right
					if((txtBackgroundAttrs.Borders & iTextSharp.text.Rectangle.RIGHT_BORDER) == iTextSharp.text.Rectangle.RIGHT_BORDER ){
						iPDFContent.MoveTo(x + width, Current_y);
						iPDFContent.LineTo( x + width, Current_y + height);
						iPDFContent.Stroke();
					}
					//botton
					if((txtBackgroundAttrs.Borders & iTextSharp.text.Rectangle.BOTTOM_BORDER) == iTextSharp.text.Rectangle.BOTTOM_BORDER ){
						iPDFContent.MoveTo(x, Current_y);
						iPDFContent.LineTo( x + width, Current_y);
						iPDFContent.Stroke();
					}
				}
				
				iPDFContent.RestoreState();
			}
			//----
			
			//paint text column. Add offset
			column.SetSimpleColumn( x + offsetTextInit			, current_y ,
			                       x + width - offsetTextInit	, current_y + height
			                      );//add offset
			column.AddText(chunk);
			column.Alignment = ELEMENT_ALIGN;
			column.SetLeading(0, 1);
			
			//20130718 :: Add arabic support
			if (_detectRightToLeft && IsArabicHebrewText(chunk.Content))
			{
				column.RunDirection = iTextSharp.text.pdf.PdfWriter.RUN_DIRECTION_RTL;
			}
			//----
			
			column.Go();
		}
		// implement interface from IPDFDraw
		
		/// <summary>
		/// Draws a vertical line
		/// </summary>
		/// <param name="x_start"></param>
		/// <param name="x_end"></param>
		/// <param name="lineAttrs"></param>
		public void DrawVerticalLine(float x_start, float x_end, XmlAttributeCollection lineAttrs)
		{
			// attribute
			// color, linethickness
			iTextSharp.text.BaseColor color = new iTextSharp.text.BaseColor(Moon.PDFDraw.Helper.GetAttributeColor(ColorAttributeConstant, lineAttrs, "Black"));
			float lineThickness = Moon.PDFDraw.Helper.GetFloatAttributeValue(LineThicknessAttributeConstant,lineAttrs,  1f);

			DrawVerticalLine(x_start, x_end, lineThickness, color);
		}
		/// <summary>
		/// Draw a vertical line on the current y with default line width of 1
		/// </summary>
		/// <param name="x_start"></param>
		/// <param name="x_end"></param>
		public void DrawVerticalLine(float x_start, float x_end)
		{
			DrawVerticalLine(x_start, x_end, 1);
		}
		
		/// <summary>
		/// Draws a vertical line
		/// </summary>
		/// <param name="x_start"></param>
		/// <param name="x_end"></param>
		/// <param name="lineThickness"></param>
		public void DrawVerticalLine(float x_start, float x_end, float lineThickness)
		{
			// default black color
			DrawVerticalLine(x_start, x_end, lineThickness, new iTextSharp.text.BaseColor(0, 0, 0));
		}
		
		/// <summary>
		/// Draws a vertical line
		/// </summary>
		/// <param name="x_start"></param>
		/// <param name="x_end"></param>
		/// <param name="lineWidth"></param>
		/// <param name="color"></param>
		public void DrawVerticalLine(float x_start, float x_end, float lineWidth, iTextSharp.text.BaseColor color)
		{
			iPDFContent.SaveState();
			iPDFContent.SetLineWidth(lineWidth);
			iPDFContent.SetColorStroke(color);
			iPDFContent.MoveTo(x_start, Current_y);
			iPDFContent.LineTo(x_end, Current_y);
			iPDFContent.Stroke();
			iPDFContent.RestoreState();
		}
		
		
		
		/// <summary>
		/// Draw on CurrentY
		/// </summary>
		/// <param name="x"></param>
		/// <param name="imageAttrs"></param>
		public void DrawImage(float x, XmlAttributeCollection imageAttrs)
		{
			iTextSharp.text.Image img = CreateImageFromAttribute(imageAttrs);
			_PreITextDrawImage(img, x, Current_y, imageAttrs);
		}
		
		/// <summary>
		/// Draws image on CurrentY. Use X position parameter value and Current_Y position.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="src"></param>
		/// <param name="imageAttrs"></param>
		public void DrawImage(float x, string src, XmlAttributeCollection imageAttrs)
		{
			iTextSharp.text.Image img = CreateImageFromAttribute(src, imageAttrs);
			_PreITextDrawImage(img, x, Current_y , imageAttrs);
		}
		
		/// <summary>
		/// Draw at absolute position x, y
		/// <para/>Doesn't support alignment.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="imageAttrs"></param>
		public void DrawImage(float x, float y, XmlAttributeCollection imageAttrs)
		{
			iTextSharp.text.Image img = CreateImageFromAttribute(imageAttrs);
			DrawImage(img, x, y);
		}
		
		
		/// <summary>
		/// Draws image. Main method. Sets absolute position. 
		/// 
		/// </summary>
		/// <param name="img"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="imageAttrs">optional</param>
		private  void _PreITextDrawImage(iTextSharp.text.Image img, float x, float y, XmlAttributeCollection imageAttrs){
			
			if(img != null && imageAttrs != null){
				string attrAlign = Helper.GetAttributeValue( AlignAttributeConstant, imageAttrs, "none" );				
				float attrWidth = Helper.GetFloatAttributeValue( WidthAttributeConstant, imageAttrs, -1 );
				if(attrWidth != -1 && string.Equals( attrAlign, "center", StringComparison.InvariantCultureIgnoreCase) ){
					//get img size and calc new x position.
					float recalcS =  attrWidth - img.ScaledWidth;
					if(recalcS > 0){
						//only if its small than reserved size.
						x += recalcS / 2; //half excess.
					}
				}
			}
			
			DrawImage( img, x, y ); //draw absolute.
		}
		
		/// <summary>
		/// Draws image. Main method. Sets absolute position. 
		/// 
		/// </summary>
		/// <param name="img"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void DrawImage(iTextSharp.text.Image img, float x, float y)
		{
			//20130603 :: mellorasinxelas
			if(img == null){
				Console.WriteLine("Image its null. Don't draw it.");
				return;
			}
			//----
			
			img.SetAbsolutePosition(x, y);			
			iPDFContent.AddImage(img);
		}
		//---------------------------------------------------------------------
		
		#region Single elements internal creation
		
		/// <summary>
		/// Creates a chunk
		/// </summary>
		/// <param name="text"></param>
		/// <param name="fontAttrs"></param>
		/// <returns></returns>
		public iTextSharp.text.Chunk CreateChunk(string text, XmlAttributeCollection fontAttrs)
		{
			iTextSharp.text.Font font = CreateFontFromAttribute(fontAttrs);
			return CreateChunk(text, font);
		}
		
		/// <summary>
		/// Creates a chunk
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <returns></returns>
		public virtual  iTextSharp.text.Chunk CreateChunk(string text, iTextSharp.text.Font font)
		{
			return new iTextSharp.text.Chunk(text, font);
		}
		
		/// <summary>
		/// Creates a phrase
		/// </summary>
		/// <param name="text"></param>
		/// <param name="fontAttrs"></param>
		/// <returns></returns>
		public iTextSharp.text.Phrase CreatePhrase(string text, XmlAttributeCollection fontAttrs)
		{
			iTextSharp.text.Font font = CreateFontFromAttribute(fontAttrs);

			return CreatePhrase(text, font);
		}
		
		/// <summary>
		/// Creates a phrase
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <returns></returns>
		public virtual  iTextSharp.text.Phrase CreatePhrase(string text, iTextSharp.text.Font font)
		{
			iTextSharp.text.pdf.FontSelector selector = FontSelector(font);
			iTextSharp.text.Phrase phrase = selector.Process(text);
			phrase.Leading = font.Size;

			return phrase;
		}
		
		/// <summary>
		/// Creates a paragraph
		/// </summary>
		/// <param name="text"></param>
		/// <param name="fontAttrs"></param>
		/// <returns></returns>
		public iTextSharp.text.Paragraph CreateParagraph(string text, XmlAttributeCollection fontAttrs)
		{
			iTextSharp.text.Font font = CreateFontFromAttribute(fontAttrs);

			return CreateParagraph(text, font);
		}
		
		
		/// <summary>
		/// Creates a paragraph
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <returns></returns>
		public virtual iTextSharp.text.Paragraph CreateParagraph(string text, iTextSharp.text.Font font)
		{
			iTextSharp.text.pdf.FontSelector selector = FontSelector(font);
			iTextSharp.text.Phrase phrase = selector.Process(text);
			phrase.Leading = font.Size;
			iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(phrase);

			return paragraph;
		}
		
		
		/// <summary>
		/// Creates a image
		/// </summary>
		/// <param name="imgAttrs"></param>
		/// <returns></returns>
		public iTextSharp.text.Image CreateImageFromAttribute(XmlAttributeCollection imgAttrs)
		{
			string src = Moon.PDFDraw.Helper.GetAttributeValue("src", imgAttrs, "");
			return CreateImageFromAttribute(src, imgAttrs);
		}
		
		/// <summary>
		/// Creates a image
		/// </summary>
		/// <param name="src"></param>
		/// <param name="imgAttrs"></param>
		/// <returns></returns>
		public virtual iTextSharp.text.Image CreateImageFromAttribute(string src, XmlAttributeCollection imgAttrs)
		{
			try{
				//size
				float width = Moon.PDFDraw.Helper.GetFloatAttributeValue(WidthAttributeConstant, imgAttrs, -1);
				float height = Moon.PDFDraw.Helper.GetFloatAttributeValue(HeightAttributeConstant, imgAttrs, -1);
				//absolute position
				float x = Moon.PDFDraw.Helper.GetFloatAttributeValue(AbsoluteXAttributeConstant, imgAttrs, -1);
				float y = Moon.PDFDraw.Helper.GetFloatAttributeValue(AbsoluteYAttributeConstant, imgAttrs, -1);
				
				//20150914 :: layout
				string layout = Moon.PDFDraw.Helper.GetAttributeValue( LayoutAttributeConstant, imgAttrs, null);
				//
				
				iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(src);
				
				//20150914 :: layouts
				//image resize or crop.
				if (width != -1 && height != -1)
				{
					if(string.IsNullOrEmpty(layout)){//default. compatibility. old behaviour.
						img.ScaleToFit(width, height);//aspect ratio
					}
					else if(string.Equals(layout, "zoom", StringComparison.InvariantCultureIgnoreCase )){
						//zoom mode. scale to fit
						img.ScaleToFit(width, height);//aspect ratio						
					}
					else if(string.Equals(layout, "stretch", StringComparison.InvariantCultureIgnoreCase )){
						//strech mode. absolute size							
						img.ScaleAbsolute( width, height );						
					}
					else if(string.Equals(layout, "none", StringComparison.InvariantCultureIgnoreCase )){
						//none
					}
					
				}
				//------
				
				//mellorasinxelas: BUG? is it used? overwrited in next steps? Now Image object has new object to absolute position.  @see:Image
				if (x != -1 && y != -1)
				{
					img.SetAbsolutePosition(x, y);
				}
				//----
				return img;
				
				//20130603 :: mellorasinxelas
			}catch(Exception ex){
				Console.WriteLine(ex);
				return null;
			}
		}
		
		/// <summary>
		/// Create a font by XML attr
		/// </summary>
		/// <param name="fontAttrs"></param>
		/// <returns></returns>
		public virtual iTextSharp.text.Font CreateFontFromAttribute(XmlAttributeCollection fontAttrs)
		{
			iTextSharp.text.Font font = CreateFont(
				Moon.PDFDraw.Helper.GetAttributeValue("fonttype", fontAttrs, "Courier"));

			font.Size = Moon.PDFDraw.Helper.GetFloatAttributeValue("fontsize", fontAttrs, 10f);
			font.SetStyle(PDFDrawItextSharpHelper.FontStyle(
				Moon.PDFDraw.Helper.GetAttributeValue("fontstyle", fontAttrs, "REGULAR")));
			font.Color = new iTextSharp.text.BaseColor(
				Moon.PDFDraw.Helper.GetAttributeColor("fontcolor", fontAttrs, "Black"));

			return font;
		}
		
		/// <summary>
		/// Create a font by name. 
		/// </summary>
		/// <param name="fontFamilyName"></param>
		/// <returns></returns>
		public virtual  iTextSharp.text.Font CreateFont(string fontFamilyName)
		{
			iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER);
			switch (fontFamilyName.ToUpper())
			{
				case "COURIER":
					//font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER);
					break;
				case "HELVETICA":
					font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA);
					break;
				case "SYMBOL":
					font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.SYMBOL);
					break;
				case "TIMES-ROMAN":
					font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN);
					break;
				case "ZAPFDINGBATS":
					font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.ZAPFDINGBATS);
					break;
				default:
					// 20130713 :: cmwong :: Please see GetFont
					font = GetFont(fontFamilyName);
					break;
			}
			return font;
		}
		
		#endregion
		
		
		/// <summary>
		/// add all the font that was RegisterFont by programmer in the order that font type from xml is the first font to use with
		/// </summary>
		/// <param name="font"></param>
		/// <returns></returns>
		public iTextSharp.text.pdf.FontSelector FontSelector(iTextSharp.text.Font font)
		{
			// add the font that define in xml to selector
			iTextSharp.text.pdf.FontSelector selector = new iTextSharp.text.pdf.FontSelector();
			
			selector.AddFont(font);

			// 20130713 :: cmwong :: add the font that was register by the programmer
			foreach (string fontName in _selectorFontNames)
			{
				if (fontName != font.Familyname)
				{
					iTextSharp.text.Font f = GetFont(fontName);
					f.Color = font.Color;
					f.SetStyle(font.Style);
					f.Size = font.Size;
					selector.AddFont(f);
				}
			}
			//-----
			
			return selector;
			
		}
		
		
		/// <summary>
		/// using iTextSharp FontFactory to get the font
		/// </summary>
		/// <param name="fontFamilyName"></param>
		/// <returns></returns>
		private iTextSharp.text.Font GetFont(string fontFamilyName)
		{
			// itextsharp default behavier is using Helvetica if font not register!!
			iTextSharp.text.Font font = null;
			if (_fontEncoding.ContainsKey(fontFamilyName))
			{
				font = iTextSharp.text.FontFactory.GetFont(
					fontFamilyName, _fontEncoding[fontFamilyName]);
			}
			else
			{
				font = iTextSharp.text.FontFactory.GetFont(fontFamilyName);
			}
			return font;
		}
		
		
		
		//---------------------------------------------------------------------
		// table
		
		/// <summary>
		/// Creates the table
		/// </summary>
		/// <param name="table"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void DrawTable(iTextSharp.text.pdf.PdfPTable table, float x, float y)
		{
			table.WriteSelectedRows(0, -1, x, y, iPDFContent);
			
		}
		
		
		/// <summary>
		/// Creates the table
		/// </summary>
		/// <param name="width"></param>
		/// <param name="cellPerRow"></param>
		/// <param name="cellWidth"></param>
		/// <returns></returns>
		public iTextSharp.text.pdf.PdfPTable CreateTable(float width, int cellPerRow, float[] cellWidth)
		{
			iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(cellPerRow);
			table.TotalWidth = width;
			table.LockedWidth = true;
			table.SetWidths(cellWidth);
			
			return table;
		}
		
		/// <summary>
		/// Creates the table
		/// </summary>
		/// <param name="attrs"></param>
		/// <returns></returns>
		public iTextSharp.text.pdf.PdfPTable CreateTable(XmlAttributeCollection attrs)
		{
			float tableWidth = Moon.PDFDraw.Helper.GetFloatAttributeValue("tablewidth", attrs, -1);
			int cellPerRow = Moon.PDFDraw.Helper.GetIntAttributeValue("cellperrow", attrs, -1);
			float[] cellWidth = Moon.PDFDraw.Helper.GetFloatArray("cellwidth", attrs);
			

			return CreateTable(tableWidth, cellPerRow, cellWidth);
		}
		
		/// <summary>
		/// Creates a table cell
		/// </summary>
		/// <param name="colspan"></param>
		/// <param name="enableBroder_RECTANGLE_BORDER">iTextSharp.text.Rectangle.BOTTOM_BORDER
		///        | iTextSharp.text.Rectangle.RIGHT_BORDER
		///        | iTextSharp.text.Rectangle.LEFT_BORDER
		///        | iTextSharp.text.Rectangle.TOP_BORDER</param>
		/// <param name="horizontal_ELEMENT_ALIGN"></param>
		/// <param name="borderColor"></param>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <returns></returns>
		public iTextSharp.text.pdf.PdfPCell CreateTableCell(
			int colspan,
			int enableBroder_RECTANGLE_BORDER,
			int horizontal_ELEMENT_ALIGN,
			iTextSharp.text.BaseColor borderColor,
			string text,
			iTextSharp.text.Font font)
		{
			iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell();
			if (colspan > 1)
			{
				cell.Colspan = colspan;
			}
			cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
			cell.EnableBorderSide(enableBroder_RECTANGLE_BORDER);
			cell.HorizontalAlignment = horizontal_ELEMENT_ALIGN;
			cell.BorderColor = borderColor;
			cell.Phrase = new iTextSharp.text.Phrase(text, font);
			cell.UseAscender = true;
			cell.UseDescender = false;

			return cell;
		}

        /// <summary>
        /// Creates a table cell
        /// </summary>
	    public iTextSharp.text.pdf.PdfPCell CreateTableCell(XmlAttributeCollection cellAttrs)
        {
            return CreateTableCell(cellAttrs, null);
        }

	    /// <summary>
		/// Creates a table cell
		/// </summary>
		public iTextSharp.text.pdf.PdfPCell CreateTableCell(XmlAttributeCollection cellAttrs, System.Collections.IDictionary data)
		{
			int colspan = Moon.PDFDraw.Helper.GetIntAttributeValue("colspan", cellAttrs, 0);
			int horizontalalign = PDFDrawItextSharpHelper.Align(
				Moon.PDFDraw.Helper.GetAttributeValue("horizontalalign", cellAttrs, "Left"));
			int border = PDFDrawItextSharpHelper.Border(
				Moon.PDFDraw.Helper.GetStringArray(BorderAttributeConstant, cellAttrs));
			iTextSharp.text.BaseColor borderColor = new iTextSharp.text.BaseColor(
				Moon.PDFDraw.Helper.GetAttributeColor("bordercolor", cellAttrs, "Black"));
			
			#if DEBUG
			//Console.WriteLine("PDFDrawItextSharp.CreateTableCell Border: " + border);
			#endif

			iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell();
			//iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("xx"));
			if (colspan > 1)
			{
				cell.Colspan = colspan;
			}
			
			float borderWidth = Moon.PDFDraw.Helper.GetFloatAttributeValue("borderwidth", cellAttrs, -1);
			float borderWidthLeft = Moon.PDFDraw.Helper.GetFloatAttributeValue("borderwidthleft", cellAttrs, -1);
			float borderWidthTop = Moon.PDFDraw.Helper.GetFloatAttributeValue("borderwidthtop", cellAttrs, -1);
			float borderWidthRight = Moon.PDFDraw.Helper.GetFloatAttributeValue("borderwidthright", cellAttrs, -1);
			float borderWidthBottom = Moon.PDFDraw.Helper.GetFloatAttributeValue("borderwidthbottom", cellAttrs, -1);
			if (borderWidth != -1) cell.BorderWidth = borderWidth;
			if (borderWidthLeft != -1) cell.BorderWidthLeft = borderWidthLeft;
			if (borderWidthTop != -1) cell.BorderWidthTop = borderWidthTop;
			if (borderWidthRight != -1) cell.BorderWidthRight = borderWidthRight;
			if (borderWidthBottom != -1) cell.BorderWidthBottom = borderWidthBottom;

			cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
			cell.EnableBorderSide(border);
			cell.HorizontalAlignment = horizontalalign;
			
			//cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
			cell.BorderColor = borderColor;

			//20130610 :: Add background color
			if( Moon.PDFDraw.Helper.GetAttributeValue(BackgroundColorAttributeConstant, cellAttrs, null) != null ){
				cell.BackgroundColor = new BaseColor(Helper.GetAttributeColor(BackgroundColorAttributeConstant, cellAttrs, "White", data));
			}
			//---
			
			cell.UseAscender = true;
			cell.UseDescender = false;

			return cell;
		}
		
		/// <summary>
		/// Creates one table cell
		/// </summary>
		/// <param name="attrs"></param>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <returns></returns>
		public iTextSharp.text.pdf.PdfPCell CreateTableCell(
			XmlAttributeCollection attrs,
			string text,
			iTextSharp.text.Font font)
		{
			iTextSharp.text.pdf.PdfPCell cell = CreateTableCell(attrs);
			cell.AddElement(new iTextSharp.text.Phrase(text, font));

			return cell;
		}
		
		
		private iTextSharp.text.Image _backgroundImageCache = null;
		
		/// <summary>
		/// Draw page background image.
		/// </summary>
		internal void DrawBackgroundImage()
		{
			if(_backgroundImage == null)return;
			try{
				iTextSharp.text.Image imageToDraw = null;
				
				if(_backgroundImageCache == null){
					
					iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(_backgroundImage.Source);
					if(img == null){
						Console.WriteLine("Image its null. Don't draw it.");
						return;
					}
					
					float docAvailableW = (pageSize.Width - (_backgroundImage.UseDocMargins?(Margin_right + Margin_left):0F) );
					float docAvailableH = (pageSize.Height - (_backgroundImage.UseDocMargins?(Margin_top + Margin_bottom):0F) );
					
					//process dim if scale to fit
					if( (img.Width > docAvailableW ||img.Height > docAvailableH) && _backgroundImage.ScaleToFit ){
						
						float imgAspectRatio = img.Width / img.Height; //aspect ratio for resize
						float docAspectRatio = docAvailableW / docAvailableH; //document aspect ratio
						
						//control
						if(imgAspectRatio > docAspectRatio){
							// width it's the reference
							img.ScaleToFit(docAvailableW , (docAvailableW / imgAspectRatio )); //included footer, mantain aspect ratio
						}
						else{
							// height it's the reference
							img.ScaleToFit((docAvailableH * imgAspectRatio), docAvailableH); //included footer, mantain aspect ratio
						}
					}
					
					//to debug
//					img.Border= iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
//					img.BorderColor = BaseColor.BLACK;
//					img.BorderWidth = 2;
					img.Alignment = iTextSharp.text.Image.ALIGN_TOP | iTextSharp.text.Image.ALIGN_LEFT;
					
					//position
					// SetAbsolute uses x document value without margins and botton y diff value.
					switch(_backgroundImage.ImageAligment){
							
						case BackgroundImageDefinition.Alignment.Center:
							img.SetAbsolutePosition( ((pageSize.Width  - img.ScaledWidth) /2) , (docAvailableH - img.ScaledHeight) /2 );
							break;
							
						case BackgroundImageDefinition.Alignment.TopLeft:
							img.SetAbsolutePosition( (_backgroundImage.UseDocMargins?Margin_left:0F), docAvailableH - img.ScaledHeight);
							break;
							
						case BackgroundImageDefinition.Alignment.TopRight:
							img.SetAbsolutePosition( pageSize.Width - (_backgroundImage.UseDocMargins?Margin_right:0F) - img.ScaledWidth , docAvailableH - img.ScaledHeight );
							break;
							
						case BackgroundImageDefinition.Alignment.TopCenter:
							//float docWMiddle = (pageSize.Width - Margin_right - Margin_left) / 2;
							img.SetAbsolutePosition(  (((pageSize.Width  - img.ScaledWidth ) / 2) /*+  (_backgroundImage.UseDocMargins?Margin_left:0F)*/ ) , (docAvailableH - img.ScaledHeight) );
							break;
							
						default:
							img.SetAbsolutePosition( 0, 0 );
							break;
					}
					
					// common attributes
					img.Alignment = iTextSharp.text.Image.UNDERLYING; //behind
					
					if(UseCachedBackgroundImage){
						_backgroundImageCache = img;
					}
					
					
					imageToDraw = img;
					
				}
				else{
					imageToDraw = _backgroundImageCache;
				}
				
				
				if(imageToDraw != null){
					iPDFContent.AddImage(imageToDraw);
				}
				
			}catch(Exception ex){
				Console.WriteLine(ex.ToString());
			}
		}
		
		
		/// <summary>
		/// Store background image.
		/// </summary>
		/// <param name="backgroundImage"></param>
		public void SetBackgroundImage(BackgroundImageDefinition backgroundImage)
		{
			this._backgroundImage = backgroundImage;
			DrawBackgroundImage();
		}
		
		
		#region Register Font support
		
		// 20130713 :: cmwong :: to hold the fontName and encoding by RegisterFont
		// _fontEncoding fontName => encoding
		// for use in CreateFont() to create font
		private Dictionary<string, string> _fontEncoding = new Dictionary<string, string>();

		// 20130713 :: cmwong :: the register fontName that will be add to FontSelector in sequence!
		private List<string> _selectorFontNames = new List<string>();
		
		
		
		/// <summary>
		/// RegisterFont allow you to use your own fonttype in the xml template.
		/// </summary>
		/// <example>
		/// 20130713 :: cmwong :: for programmer to register font and font name for them to be use
		/// in the xml. This allow more font choose for them and they can use their own font.
		/// iTextAsian.dll is supported, add it before initialize PDFTemplateItextSharp
		/// iTextSharp.text.pdf.BaseFont.AddToResourceSearch("iTextAsian.dll");
		/// drawer.RegisterFont("", "STSong-Light", "UniGB-UCS2-H"); // regiter the CJK font name with the correct
		/// encoding, ignore the path.
		/// register true type font.
		/// drawer.RegisterFont(@"C:\Windows\Fonts\verdana.ttf", "verdana", "");
		/// then the font name verdana can be use in your xml template.
		/// </example>
		/// 
		/// <param name="fontpath">font file path</param>
		/// <param name="fontName">alias font name to use in xml</param>
		/// <param name="encoding">ttf font usually use Identity-H</param>
		public void RegisterFont(string fontpath, string fontName, string encoding)
		{
			if (!string.IsNullOrEmpty(fontpath))
			{
				iTextSharp.text.FontFactory.Register(fontpath, fontName);
			}
			if (!string.IsNullOrEmpty(encoding))
			{
				_fontEncoding.Add(fontName, encoding);
			}
			_selectorFontNames.Add(fontName);
		}

		
		// 20130713 :: cmwong :: whether to detect Right to Left character or not!
		private bool _detectRightToLeft = false;
		/// <summary>
		/// whether to detect Right to Left character or not!
		/// </summary>
		public bool DetectRightToLeft
		{
			get { return _detectRightToLeft; }
			set { _detectRightToLeft = value; }
		}
		
		
		#endregion
		// 20130713 :: cmwong :: regex to detect arabic and hebrew Right to Left character
		private System.Text.RegularExpressions.Regex _arabicHebrew =
			new System.Text.RegularExpressions.Regex(
				@"\p{IsHebrew}|\p{IsArabic}",
				System.Text.RegularExpressions.RegexOptions.IgnoreCase
			);


		/// <summary>
		/// Check if text is arabic o hebrew.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		protected bool IsArabicHebrewText(string text)
		{
			return _arabicHebrew.IsMatch(text);
		}
		
		
	}
}
