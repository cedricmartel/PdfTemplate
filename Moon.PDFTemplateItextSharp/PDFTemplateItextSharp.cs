using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using iTextSharp.text;
using Moon.PDFDraw;
using Moon.PDFDrawItextSharp;
using Moon.PDFDrawItextSharp.StyleContainers;
using Moon.PDFTemplate;
using Moon.Utils;

namespace Moon.PDFTemplateItextSharp
{
	/// <summary>
	/// PDF builder using XML templates.
	/// </summary>
	public class PDFTemplateItextSharp : PDFTemplate.PDFTemplate
	{
		
		#region Class VARS
		
		private iTextSharp.text.Rectangle pageSize;
		private System.IO.MemoryStream stream = new System.IO.MemoryStream();
		private string fontpath = "";

		private System.Collections.IDictionary tableHeadData = new Hashtable();
		private System.Collections.IDictionary tableFootData = new Hashtable();

		#endregion
		
		
		#region Builders
		
		/// <summary>
		/// Constructor
		/// 
		/// </summary>
		/// <param name="xmlFile"></param>
		public PDFTemplateItextSharp(string xmlFile)
			: base(xmlFile)
		{
			init();
		}
		
		/// <summary>
		/// Constructor
		/// 
		/// </summary>
		/// <param name="xmldoc"></param>
		public PDFTemplateItextSharp(XmlDocument xmldoc)
			: base(xmldoc)
		{
			init();
		}
		
		/// <summary>
		/// Constructor
		/// 
		/// </summary>
		/// <param name="xmlFile"></param>
		/// <param name="fontpath"></param>
		public PDFTemplateItextSharp(string xmlFile, string fontpath)
			: base(xmlFile)
		{
			this.fontpath = fontpath;
			init();
		}
		
		/// <summary>
		/// Constructor
		/// 
		/// </summary>
		/// <param name="xmldoc"></param>
		/// <param name="fontpath"></param>
		public PDFTemplateItextSharp(XmlDocument xmldoc, string fontpath)
			: base(xmldoc)
		{
			this.fontpath = fontpath;
			init();
		}
		
		#endregion
		
		
		
		private void init()
		{
			
			//20130604 :: mellorasinxelas
			//iTextSharp control.
			System.Version iTextSharpVersion = AssembliesTypesLoader.GetAssemblyVersion( typeof( iTextSharp.text.Image ) );
			if(iTextSharpVersion == new System.Version(0,0,0,0))throw new DllNotFoundException("iTextSharp DLL not loaded!");
			if(iTextSharpVersion < new System.Version(5,0)){
				throw new NotSupportedException( this.GetType().Name + " requires iTextSharp 5.0 or higher!");
			}
			//----
			
			//20130607 :: mellorasinxelas :: add background image
			//            if (fontpath != string.Empty)
			//            {
			//                PdfDrawer = new Moon.PDFDrawItextSharp.PDFDrawItextSharp(
			//                    pageSize,
			//                    PageDefinition.Margin_left,
			//                    PageDefinition.Margin_right,
			//                    PageDefinition.Margin_top,
			//                    PageDefinition.Margin_bottom,
			//                    stream,
			//                    fontpath);
			//            }
			//            else
			//            {
			//                PdfDrawer = new Moon.PDFDrawItextSharp.PDFDrawItextSharp(
			//                    pageSize,
			//                    PageDefinition.Margin_left,
			//                    PageDefinition.Margin_right,
			//                    PageDefinition.Margin_top,
			//                    PageDefinition.Margin_bottom,
			//                    stream);
			//            }
			
			//20150914: customizable builder.
			PdfDrawer = BuildPDFDrawer(pageSize,
			                           PageDefinition.Margin_left,
			                           PageDefinition.Margin_right,
			                           PageDefinition.Margin_top,
			                           PageDefinition.Margin_bottom,
			                           stream,
			                           fontpath);
			if(PdfDrawer == null)throw new NullReferenceException("PDF Drawer");
			//---
			
			//20130607
			if(PageDefinition.BackgroundImage != null){
				//adds background image.
				PdfDrawer.SetBackgroundImage( PageDefinition.BackgroundImage );
			}
			//----
			
			//20130606 :: Absolute Footer
			if(PageDefinition.Footer != null && PageDefinition.Footer is FooterGroup && ((FooterGroup)PageDefinition.Footer).Absolute){
				//reserve space to absolute footer...
				((Moon.PDFDrawItextSharp.PDFDrawItextSharp)PdfDrawer).ReserveSpaceToFooter( PageDefinition.Footer.Y );
			}
			//----
			
		}
		
		
		
		#region Build builder
		
		/// <summary>
		/// Builds internal PDF Drawer. By default PDFDrawItextSharp.
		/// </summary>
		/// <param name="size">Page size</param>
		/// <param name="marginLeft">Left margin</param>
		/// <param name="marginRight">Right margin</param>
		/// <param name="marginTop">Top margin</param>
		/// <param name="marginBottom">Bottom margin</param>
		/// <param name="dataStream">stream target to pdf content</param>
		/// <param name="fontPath">Optional</param>
		/// <returns></returns>
		protected virtual IPDFDraw BuildPDFDrawer(
			iTextSharp.text.Rectangle size,
			float marginLeft,
			float marginRight, float marginTop,
			float marginBottom, Stream dataStream, string fontPath){
			
			if(!string.IsNullOrEmpty( fontpath)){
				return new Moon.PDFDrawItextSharp.PDFDrawItextSharp(
					pageSize,
					PageDefinition.Margin_left,
					PageDefinition.Margin_right,
					PageDefinition.Margin_top,
					PageDefinition.Margin_bottom,
					stream,
					fontpath);
			}
			else
			{
				return new Moon.PDFDrawItextSharp.PDFDrawItextSharp(
					pageSize,
					PageDefinition.Margin_left,
					PageDefinition.Margin_right,
					PageDefinition.Margin_top,
					PageDefinition.Margin_bottom,
					stream);
			}
		}
	
	
	
	
	#endregion
	
	/// <summary>
	/// Internal PDF stream.
	/// </summary>
	internal System.IO.MemoryStream Stream
	{
		get { return stream; }
	}
	
	/// <summary>
	/// Set document properties stored in the output document.
	/// </summary>
	/// <param name="docProperties"></param>
	public void SetDocumentProperties( DocumentProperties docProperties ){
		((Moon.PDFDrawItextSharp.PDFDrawItextSharp)PdfDrawer).SetDocumentProperties( docProperties );
	}
	
	
	/// <summary>
	/// Close the underlaying PDFDraw and return the closed memory stream
	/// </summary>
	/// <returns></returns>
	public System.IO.MemoryStream Close()
	{
		Moon.PDFDrawItextSharp.PDFDrawItextSharp p = (Moon.PDFDrawItextSharp.PDFDrawItextSharp)PdfDrawer;
		iTextSharp.text.Font font = null;
		if (this.PageNumberBox != null)
		{
			font = p.CreateFontFromAttribute(PageNumberBox.FontAttributes);
		}
		p.Close();
		Debug();

		if (this.PageNumberBox != null)
		{
			DrawPageNumber(font);
		}
		return stream;
	}
	
	
	
	
	private void DrawPageNumber(iTextSharp.text.Font font)
	{
		#if DEBUG
		//Console.WriteLine("pageNumberBox: " + pageNumberBox.X + ", " + pageNumberBox.Y);
		#endif
		iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(stream.ToArray());
		System.IO.MemoryStream _stream = new System.IO.MemoryStream();
		iTextSharp.text.pdf.PdfStamper stamper = new iTextSharp.text.pdf.PdfStamper(reader, _stream);

		int currentPage = 1;
		int totalPage = 0;
		int drawPage = 1;

		Moon.PDFDrawItextSharp.PDFDrawItextSharp pDraw = (Moon.PDFDrawItextSharp.PDFDrawItextSharp)PdfDrawer;
		iTextSharp.text.pdf.FontSelector selector = pDraw.FontSelector(font);

		foreach (int eachCount in EachPageCount)
		{
			if (drawPage == 1)
			{
				currentPage = 1;
				totalPage = eachCount;
			}
			else
			{
				currentPage = 1;
				totalPage = eachCount - totalPage;
			}
			while (drawPage <= eachCount)
			{
				#if DEBUG
				//Console.WriteLine("PDFTemplateItextSharp.DrawPageNumber: " + currentPage + "/" + totalPage);
				#endif
				System.Collections.Hashtable data = new System.Collections.Hashtable();
				data.Add("{__PAGE__}", currentPage);
				data.Add("{__TOTALPAGE__}", totalPage);

				iTextSharp.text.pdf.PdfContentByte cb = stamper.GetOverContent(drawPage);
				//iTextSharp.text.Phrase phrase = new iTextSharp.text.Chunk(pageNumberBox.GetText(data), font);
				iTextSharp.text.Phrase phrase = selector.Process(PageNumberBox.GetText(data));

				//Console.WriteLine("chunk.Content: " + chunk.Content);
				//iTextSharp.text.pdf.ColumnText column = new iTextSharp.text.pdf.ColumnText(cb);
				int align = Moon.PDFDrawItextSharp.PDFDrawItextSharpHelper.Align(
					Moon.PDFDraw.Helper.GetAttributeValue(
						"align",
						PageNumberBox.Attributes, "Left"));
				
				//20130610 :: mellorasinxelas :: change to static class
				//20130721 :: cmwong :: change to PDFDrawItextSharp.DrawString()
				pDraw.DrawString(cb, phrase, PageNumberBox.X, PageNumberBox.Y, PageNumberBox.Width, align, null);

				currentPage++;
				drawPage++;
			}
		}
		stamper.Close();
		stream = _stream;
	}
	
	/// <summary>
	/// implement for PDFTemplate
	/// this will call when initialize PDFTemplate object!
	/// </summary>
	protected override void SetPageDefWidthHeight()
	{
		// this will call from PDFTemplate._buildPageDef()
		pageSize = PDFDrawItextSharp.PDFDrawItextSharpHelper.PageSize(
			PDFDraw.Helper.GetAttributeValue("pagesize", PageDefinition.PageDefAttrs, "A4"));

		if (PDFDraw.Helper.GetAttributeValue(
			"pageorientation",
			PageDefinition.PageDefAttrs,
			"landscape").ToUpper() == "LANDSCAPE")
		{
			pageSize = pageSize.Rotate();
		}
		
		PageDefinition.Width = pageSize.Width;
		PageDefinition.Height = pageSize.Height;
	}

	//---------------------------------------------------------------------
	
	// table
	private PDFTemplate.TableCell _buildTableCell(XmlNode node, XmlAttributeCollection fontAttrs)
	{
		PDFTemplate.TableCell tableCell = new PDFTemplate.TableCell(node.Attributes, fontAttrs);
		_buildTableCellChildElement(tableCell, node, fontAttrs);

		return tableCell;
	}
	
	/// <summary>
	/// 
	/// 20130612 :: Add custom element.
	/// </summary>
	/// <param name="tableCell"></param>
	/// <param name="tableCellNode"></param>
	/// <param name="fontAttrs"></param>
	private void _buildTableCellChildElement(
		PDFTemplate.TableCell tableCell,
		XmlNode tableCellNode,
		XmlAttributeCollection fontAttrs)
	{
		foreach (XmlNode child in tableCellNode.ChildNodes)
		{
			switch (child.Name)
			{
				case "textbox":
					tableCell.AddDrawElement(_buildTextBox(child, fontAttrs));
					break;
				case "image":
					tableCell.AddDrawElement(_buildImage(child, fontAttrs));
					break;
					//20130612 :: mellorasinxelas :: custom
				case CustomElementConstant:
					tableCell.AddDrawElement(_buildCustomElement(child, fontAttrs));
					break;
					
			}
			if (child.Name == "font" && child.HasChildNodes)
			{
				_buildTableCellChildElement(tableCell, child, child.Attributes);
			}
		}
	}
	
	private void _buildTableRowElement(PDFTemplate.TableRow tableRow, XmlNode tableRowNode, XmlAttributeCollection fontAttrs)
	{
		foreach (XmlNode child in tableRowNode)
		{
			switch (child.Name)
			{
				case "tablecell":
					tableRow.AddTableCell(_buildTableCell(child, fontAttrs));
					break;
			}
			if (child.Name == "font" && child.HasChildNodes)
			{
				_buildTableRowElement(tableRow, child, child.Attributes);
			}
		}
	}
	private PDFTemplate.TableRow _buildTableRow(XmlNode tableRowNode, XmlAttributeCollection fontAttrs)
	{
		PDFTemplate.TableRow tableRow = new PDFTemplate.TableRow();
		if (tableRowNode.HasChildNodes)
			_buildTableRowElement(tableRow, tableRowNode, fontAttrs);

		return tableRow;
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="node">tablehead, tableloop, tablefoot</param>
	/// <returns></returns>
	private PDFTemplate.TableRowGroup _buildTableRowGroup(XmlNode node)
	{
		PDFTemplate.TableRowGroup tableRowGroup = new PDFTemplate.TableRowGroup();
		XmlAttributeCollection _font = DefaultFontAttrs;
		if (node.FirstChild.Name == "font")
		{
			_font = node.FirstChild.Attributes;
		}

		XmlNodeList tableRowNodes = node.SelectNodes(".//tablerow");
		foreach (XmlNode tableRowNode in tableRowNodes)
		{
			tableRowGroup.AddTableRow(_buildTableRow(tableRowNode, _font));
		}
		return tableRowGroup;
	}
	
	private PDFTemplate.Table tableElement;
	private PDFTemplate.TableRowGroup tableRowGroupHead = new PDFTemplate.TableRowGroup();
	private PDFTemplate.TableRowGroup tableRowGroupLoop = new PDFTemplate.TableRowGroup();
	private PDFTemplate.TableRowGroup tableRowGroupFoot = new PDFTemplate.TableRowGroup();
	/// <summary>
	/// override base _buildPageDef but still call it first
	/// implement table here
	/// </summary>
	protected override void _buildPageDef()
	{
		// call base to build footer, loop, body, footer from xml
		base._buildPageDef();
		//Console.WriteLine("call _buildPageDef in PDFTemplateItextSharp");

		// here, we build the table from xml
		XmlElement elmRoot = XMLTemplate.DocumentElement;
		XmlNode tableNode = elmRoot.SelectSingleNode("//table");
		if (tableNode == null)
		{
			return;
		}
		tableElement = new PDFTemplate.Table(tableNode.Attributes);

		XmlNode tableHeadNode = tableNode.SelectSingleNode(".//tablehead");
		if (tableHeadNode != null)
		{
			tableRowGroupHead = _buildTableRowGroup(tableHeadNode);
		}
		XmlNode tableLoopNode = tableNode.SelectSingleNode(".//tableloop");
		if (tableLoopNode != null)
		{
			tableRowGroupLoop = _buildTableRowGroup(tableLoopNode);
		}
		XmlNode tableFootNode = tableNode.SelectSingleNode(".//tablefoot");
		if (tableFootNode != null)
		{
			tableRowGroupFoot = _buildTableRowGroup(tableFootNode);
		}
		#if DEBUG
		Console.WriteLine("tableRowGroupHead.Count: " + tableRowGroupHead.TableRows.Count);
		Console.WriteLine("tableRowGroupLoop.Count: " + tableRowGroupLoop.TableRows.Count);
		Console.WriteLine("tableRowGroupFoot.Count: " + tableRowGroupFoot.TableRows.Count);
		#endif

	}
	
	/// <summary>
	/// Draw PDF using the template and this data.
	/// </summary>
	/// <param name="headerData"></param>
	/// <param name="loopData"></param>
	/// <param name="bodyData"></param>
	/// <param name="footerData"></param>
	/// <param name="tableHeadData"></param>
	/// <param name="tableLoopData"></param>
	/// <param name="tableFootData"></param>
	public void Draw(
		Hashtable headerData,
		List<Hashtable> loopData,
		Hashtable bodyData,
		Hashtable footerData,
		Hashtable tableHeadData,
		List<Hashtable> tableLoopData,
		Hashtable tableFootData)
	{
		if (headerData != null)
		{
			this.headerData = headerData;
		}
		if (bodyData != null)
		{
			this.bodyData = bodyData;
		}
		if (footerData != null)
		{
			this.footerData = footerData;
		}

		if (tableHeadData != null)
		{
			this.tableHeadData = tableHeadData;
		}
		if (this.tableFootData != null)
		{
			this.tableFootData = tableFootData;
		}

		if (DrawCallCounter > 0)
		{
			// how many times the Draw() hv been call! if it was call b4, this time we draw to a new page!
			NextPage();
		}

		_drawHeader();

		if (tableElement != null)
		{
			
			//Console.WriteLine("got tableElement!");
			iTextSharp.text.pdf.PdfPTable table = _drawTableHead();
			if (tableLoopData != null)
			{
				_drawTableLoop(ref table, tableLoopData);
			}
			_drawTableFoot(ref table);

			if (table.Rows.Count > 0)
			{
				PDFDrawItextSharp.PDFDrawItextSharp _pdfDraw = (PDFDrawItextSharp.PDFDrawItextSharp)PdfDrawer;
				PdfDrawer.NextRow(1, DocumentGroup.Table);
				_pdfDraw.DrawTable(table, _pdfDraw.Current_x, _pdfDraw.Current_y);
				_pdfDraw.NextRow(table.TotalHeight, DocumentGroup.Table);
			}
		}
		
		if (loopData != null)
		{
			_drawLoop(loopData);
		}
		
		_drawRowGroup(PageDefinition.Body, bodyData, DocumentGroup.Table);
		_drawFooter();

		EachPageCount.Add(PageCount);
		DrawCallCounter++;
	}
	
	
	/// <summary>
	/// Add a row of pdfpcell to table.
	/// return True, current page have enought size for that row
	/// return False, current page not enought size for that row, row will be remove and need to draw on
	/// next page
	/// 20130606 :: jaimelopez :: mellorasinxelas to support absolute footer.
	/// </summary>
	/// <param name="table"></param>
	/// <param name="tableRowElement"></param>
	/// <param name="data"></param>
	/// <returns></returns>
	protected bool _drawTableRow(
		iTextSharp.text.pdf.PdfPTable table,
		PDFTemplate.TableRow tableRowElement, System.Collections.IDictionary data)
	{
		bool enoughSpace = true;
		PDFDrawItextSharp.PDFDrawItextSharp _pdfDraw = (PDFDrawItextSharp.PDFDrawItextSharp)PdfDrawer;
		foreach (PDFTemplate.TableCell tableCell in tableRowElement.TableCells)
		{
			iTextSharp.text.pdf.PdfPCell cell = _pdfDraw.CreateTableCell(tableCell.Attributes);
			foreach (PDFTemplate.DrawElement drawElement in tableCell.DrawElements)
			{
				if (drawElement is PDFTemplate.TextBox)
				{
					//iTextSharp.text.Phrase phrase = _pdfDraw.CreatePhrase(
					//    ((PDFTemplate.TextBox)drawElement).GetText(data), drawElement.FontAttributes);
					iTextSharp.text.Paragraph paragraph = _pdfDraw.CreateParagraph(((PDFTemplate.TextBox)drawElement).GetText(data), drawElement.FontAttributes);
					paragraph.Alignment = Moon.PDFDrawItextSharp.PDFDrawItextSharpHelper.Align(Moon.PDFDraw.Helper.GetAttributeValue("align", drawElement.Attributes, "Left"));

					cell.AddElement(paragraph);
				}
				else if (drawElement is PDFTemplate.Image)
				{
					iTextSharp.text.Image image = _pdfDraw.CreateImageFromAttribute(drawElement.Attributes);
					image.Alignment = Moon.PDFDrawItextSharp.PDFDrawItextSharpHelper.Align(Moon.PDFDraw.Helper.GetAttributeValue("align", drawElement.Attributes, "Left"));

					cell.AddElement(image);
				}
			}
			table.AddCell(cell);
		}
		table.CompleteRow();
		
		//if(table.row
		//fixme need to check if any row span
		if (PdfDrawer.isNoMoreY(table.TotalHeight, DocumentGroup.Table))
		{
			enoughSpace = false;
			table.DeleteLastRow();
		}

		return enoughSpace;
	}
	
	/// <summary>
	/// Draws table row group
	/// 
	/// </summary>
	/// <param name="table"></param>
	/// <param name="tableRowGroup"></param>
	/// <param name="data"></param>
	protected void _drawTableRowGroup(
		ref iTextSharp.text.pdf.PdfPTable table,
		PDFTemplate.TableRowGroup tableRowGroup,
		System.Collections.IDictionary data)
	{
		int count = 0;
		foreach (PDFTemplate.TableRow tableRow in tableRowGroup.TableRows)
		{
			
			bool haveSize = _drawTableRow(table, tableRow, data);
			//Console.WriteLine("_drawTableRowGroup count: " + count + " haveSize: " + haveSize);
			if (!haveSize)
			{
				//Console.WriteLine("_drawTableRowGroup in no more size");
				//1. draw the current table to pdf
				PdfDrawer.NextRow(1, DocumentGroup.Table);
				PDFDrawItextSharp.PDFDrawItextSharp _pdfDraw = (PDFDrawItextSharp.PDFDrawItextSharp)PdfDrawer;
				_pdfDraw.DrawTable(table, _pdfDraw.Current_x, _pdfDraw.Current_y);
				//2. move pdf to next page
				NextPage();
				_drawHeader();
				//Console.WriteLine("Call nextPage, _drawHeader()");
				//3. recreate table with the header
				table = _drawTableHead();
				//Console.WriteLine("call _drawTableHead(), table.totalheight: " + table.TotalHeight);
				//4. add the current row to the new table
				_drawTableRow(table, tableRow, data);
			}
			count++;
		}
	}
	
	/// <summary>
	/// Draws table header
	/// </summary>
	/// <returns></returns>
	protected iTextSharp.text.pdf.PdfPTable _drawTableHead()
	{
		PDFDrawItextSharp.PDFDrawItextSharp _pdfDraw = (PDFDrawItextSharp.PDFDrawItextSharp)PdfDrawer;
		iTextSharp.text.pdf.PdfPTable table = _pdfDraw.CreateTable(tableElement.Attributes);
		float widthpercentage = Moon.PDFDraw.Helper.GetAttributeWidthPercent(tableElement.Attributes);
		if (widthpercentage != 0)
		{
			table.TotalWidth = (PageDefinition.Width - PageDefinition.Margin_left - PageDefinition.Margin_right) * widthpercentage;
		}

		_drawTableRowGroup(ref table, tableRowGroupHead, tableHeadData);
		return table;
	}
	
	/// <summary>
	/// Draws table loop
	/// </summary>
	/// <param name="table"></param>
	/// <param name="tableLoopData"></param>
	protected void _drawTableLoop(ref iTextSharp.text.pdf.PdfPTable table, List<Hashtable> tableLoopData)
	{
		if (tableLoopData == null)
		{
			return;
		}
		foreach (Hashtable data in tableLoopData)
		{
			_drawTableRowGroup(ref table, tableRowGroupLoop, data);
		}
	}
	
	/// <summary>
	/// Draws table footer
	/// </summary>
	/// <param name="table"></param>
	protected void _drawTableFoot(ref iTextSharp.text.pdf.PdfPTable table)
	{
		_drawTableRowGroup(ref table, tableRowGroupFoot, tableFootData);
	}
	
	
	
	#region Register fonts
	
	/// <summary>
	/// Register font to print it correctly.
	/// <example>
	/// pdfTemplate.RegisterFont(@"C:\Windows\Fonts\verdana.ttf", "verdana", "");
	/// drawer.RegisterFont("", "STSong-Light", "UniGB-UCS2-H");
	///	pdfTemplate.RegisterFont("", "MHei-Medium", "UniCNS-UCS2-H");
	/// </example>
	/// 
	/// @since 20130719
	/// </summary>
	/// <param name="_fontpath"></param>
	/// <param name="_fontName"></param>
	/// <param name="_encoding"></param>
	public void RegisterFont(string _fontpath, string _fontName, string _encoding)
	{
		((Moon.PDFDrawItextSharp.PDFDrawItextSharp)this.PdfDrawer).RegisterFont( _fontpath, _fontName, _encoding );
	}
	
	#endregion

	/// <summary>
	/// Get/set if the class use auto right to left font detect, into the PDF drawer.
	/// </summary>
	public bool DetectRightToLeft
	{
		get { return ((Moon.PDFDrawItextSharp.PDFDrawItextSharp)this.PdfDrawer).DetectRightToLeft; }
		set { ((Moon.PDFDrawItextSharp.PDFDrawItextSharp)this.PdfDrawer).DetectRightToLeft = value; }
	}

}
}
