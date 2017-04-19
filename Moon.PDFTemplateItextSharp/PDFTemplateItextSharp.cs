using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Moon.PDFDraw;
using Moon.PDFDrawItextSharp;
using Moon.PDFTemplate;
using Moon.PDFTemplate.Model;
using Moon.PDFTemplateItextSharp.Generators;
using Moon.PDFTemplateItextSharp.Model;
using Moon.Utils;
using Image = iTextSharp.text.Image;
using Version = System.Version;

namespace Moon.PDFTemplateItextSharp
{
    /// <summary>
    /// PDF builder using XML templates.
    /// </summary>
    public class PDFTemplateItextSharp : PDFTemplate.PDFTemplate
    {

        #region Class VARS

        private Rectangle pageSize;
        private MemoryStream stream = new MemoryStream();
        private string fontpath = "";

        #endregion

        #region Builders

        /// <summary>
        /// Constructor
        /// 
        /// </summary>
        /// <param name="xmlFile"></param>
        public PDFTemplateItextSharp(string xmlFile) : base(xmlFile)
        {
            Init();
        }

        /// <summary>
        /// Constructor
        /// 
        /// </summary>
        /// <param name="xmldoc"></param>
        public PDFTemplateItextSharp(XmlDocument xmldoc) : base(xmldoc)
        {
            Init();
        }

        /// <summary>
        /// Constructor
        /// 
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <param name="fontpath"></param>
        public PDFTemplateItextSharp(string xmlFile, string fontpath) : base(xmlFile)
        {
            this.fontpath = fontpath;
            Init();
        }

        /// <summary>
        /// Constructor
        /// 
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <param name="fontpath"></param>
        public PDFTemplateItextSharp(XmlDocument xmldoc, string fontpath) : base(xmldoc)
        {
            this.fontpath = fontpath;
            Init();
        }

        #endregion



        private void Init()
        {

            //20130604 :: mellorasinxelas
            //iTextSharp control.
            Version iTextSharpVersion = AssembliesTypesLoader.GetAssemblyVersion(typeof(Image));
            if (iTextSharpVersion == new Version(0, 0, 0, 0)) throw new DllNotFoundException("iTextSharp DLL not loaded!");
            if (iTextSharpVersion < new Version(5, 0))
            {
                throw new NotSupportedException(GetType().Name + " requires iTextSharp 5.0 or higher!");
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
            if (PdfDrawer == null) throw new NullReferenceException("PDF Drawer");
            //---

            //20130607
            if (PageDefinition.BackgroundImage != null)
            {
                //adds background image.
                PdfDrawer.SetBackgroundImage(PageDefinition.BackgroundImage);
            }
            //----

            //20130606 :: Absolute Footer
            if (PageDefinition.Footer != null && PageDefinition.Footer.RowGroup is FooterGroup && ((FooterGroup)PageDefinition.Footer.RowGroup).Absolute)
            {
                //reserve space to absolute footer...
                ((PDFDrawItextSharp.PDFDrawItextSharp)PdfDrawer).ReserveSpaceToFooter(PageDefinition.Footer.RowGroup.Y);
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
            Rectangle size,
            float marginLeft,
            float marginRight, float marginTop,
            float marginBottom, Stream dataStream, string fontPath)
        {

            if (!string.IsNullOrEmpty(fontpath))
            {
                return new PDFDrawItextSharp.PDFDrawItextSharp(
                    pageSize,
                    PageDefinition.Margin_left,
                    PageDefinition.Margin_right,
                    PageDefinition.Margin_top,
                    PageDefinition.Margin_bottom,
                    stream,
                    fontpath);
            }
            return new PDFDrawItextSharp.PDFDrawItextSharp(
                pageSize,
                PageDefinition.Margin_left,
                PageDefinition.Margin_right,
                PageDefinition.Margin_top,
                PageDefinition.Margin_bottom,
                stream);
        }

        #endregion

        /// <summary>
        /// Internal PDF stream.
        /// </summary>
        internal MemoryStream Stream
        {
            get { return stream; }
        }

        /// <summary>
        /// Set document properties stored in the output document.
        /// </summary>
        /// <param name="docProperties"></param>
        public void SetDocumentProperties(DocumentProperties docProperties)
        {
            ((PDFDrawItextSharp.PDFDrawItextSharp)PdfDrawer).SetDocumentProperties(docProperties);
        }
        
        /// <summary>
        /// This object is used to exclusively lock usage fvor Close method bellow, to prevent concurrency errors on loading fonts 
        /// </summary>
        public static Object ClosingFileLocker = new object();

        /// <summary>
        /// Close the underlaying PDFDraw and return the closed memory stream
        /// </summary>
        /// <returns></returns>
        public MemoryStream Close()
        {
            lock (ClosingFileLocker)
            {
                PDFDrawItextSharp.PDFDrawItextSharp p = (PDFDrawItextSharp.PDFDrawItextSharp) PdfDrawer;
                Font font = null;
                if (PageNumberBoxes != null && PageNumberBoxes.Count > 0)
                {
                    font = p.CreateFontFromAttribute(PageNumberBoxes[0].FontAttributes);
                }
                p.Close();
                Debug();

                if (PageNumberBoxes != null && PageNumberBoxes.Count > 0)
                {
                    DrawPageNumber(font);
                }
                return stream;
            }
        }




        private void DrawPageNumber(Font font)
        {
#if DEBUG
            //Console.WriteLine("pageNumberBox: " + pageNumberBox.X + ", " + pageNumberBox.Y);
#endif
            PdfReader reader = new PdfReader(stream.ToArray());
            MemoryStream _stream = new MemoryStream();
            PdfStamper stamper = new PdfStamper(reader, _stream);

            int currentPage = 1;
            int totalPage = 0;
            int drawPage = 1;

            PDFDrawItextSharp.PDFDrawItextSharp pDraw = (PDFDrawItextSharp.PDFDrawItextSharp)PdfDrawer;
            FontSelector selector = pDraw.FontSelector(font);

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
                    if (PageNumberBoxes.Count < currentPage)
                        break;
                    var pageNumberBox = PageNumberBoxes[currentPage - 1];

                    Hashtable data = new Hashtable
                    {
                        {"{__PAGE__}", currentPage}, 
                        {"{__TOTALPAGE__}", totalPage}
                    };

                    PdfContentByte cb = stamper.GetOverContent(drawPage);
                    //iTextSharp.text.Phrase phrase = new iTextSharp.text.Chunk(pageNumberBox.GetText(data), font);
                    Phrase phrase = selector.Process(pageNumberBox.GetText(data));

                    //Console.WriteLine("chunk.Content: " + chunk.Content);
                    //iTextSharp.text.pdf.ColumnText column = new iTextSharp.text.pdf.ColumnText(cb);
                    int align = PDFDrawItextSharpHelper.Align(
                        Helper.GetAttributeValue(
                            "align",
                            pageNumberBox.Attributes, "Left"));

                    //20130610 :: mellorasinxelas :: change to static class
                    //20130721 :: cmwong :: change to PDFDrawItextSharp.DrawString()
                    pDraw.DrawString(cb, phrase, pageNumberBox.X, pageNumberBox.Y, pageNumberBox.Width, align, null);

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
        protected override void SetPageDefWidthHeight(Orientation orientation)
        {
            // this will call from PDFTemplate._buildPageDef()
            if (pageSize == null)
                pageSize = PDFDrawItextSharpHelper.PageSize(Helper.GetAttributeValue("pagesize", PageDefinition.PageDefAttrs, "A4"));

            if (orientation != CurrentOrientation)
            {
                this.CurrentOrientation = orientation;

                pageSize = pageSize.Rotate();
                PdfDrawer?.RotatePage();
            }

            PageDefinition.Width = pageSize.Width;
            PageDefinition.Height = pageSize.Height;
        }

        //---------------------------------------------------------------------

        // table
        private TableGenerator principalTableGenerator;
        private BodyGenerator bodyGenerator;


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

            if (tableNode != null)
                principalTableGenerator = new TableGenerator(this, tableNode);

            XmlNode bodyNode = elmRoot.SelectSingleNode("//body");
            if (tableNode != null)
                bodyGenerator = new BodyGenerator(this, bodyNode);
        }

        /// <summary>
        /// Draw PDF using the template with minimal variables, enabling body multiple tables 
        /// </summary>
        public void Draw(Hashtable header, Hashtable body, Hashtable footer)
        {
            Draw(header, null, body, footer, null, null, null);
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

            var tableData = new TableData
            {
                HeadData = tableHeadData,
                LoopData = tableLoopData,
                FootData = tableFootData
            };

            if (DrawCallCounter > 0)
            {
                // how many times the Draw() hv been call! if it was call b4, this time we draw to a new page!
                NextPage();
            }

            DrawHeader();

            if (principalTableGenerator != null && tableData.LoopData != null)
                principalTableGenerator.DrawTable(tableData, PdfDrawer);

            if (loopData != null)
            {
                _drawLoop(loopData);
            }

            if (bodyGenerator != null)
                bodyGenerator.DrawBody(bodyData, PdfDrawer);

            _drawFooter();

            EachPageCount.Add(PageCount);
            DrawCallCounter++;
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
            ((PDFDrawItextSharp.PDFDrawItextSharp)PdfDrawer).RegisterFont(_fontpath, _fontName, _encoding);
        }

        #endregion

        /// <summary>
        /// Get/set if the class use auto right to left font detect, into the PDF drawer.
        /// </summary>
        public bool DetectRightToLeft
        {
            get { return ((PDFDrawItextSharp.PDFDrawItextSharp)PdfDrawer).DetectRightToLeft; }
            set { ((PDFDrawItextSharp.PDFDrawItextSharp)PdfDrawer).DetectRightToLeft = value; }
        }

    }
}
