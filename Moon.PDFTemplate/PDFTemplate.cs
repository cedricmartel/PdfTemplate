using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Diagnostics;
using Moon.PDFDraw;
using Moon.PDFTemplate.CustomElements;
using Moon.PDFTemplate.Formatters;
using Moon.PDFTemplate.Model;
using Moon.PDFTemplate.Utils;
using Moon.PDFTemplate.XMAtributes;
using Moon.Utils;

namespace Moon.PDFTemplate
{
    /// <summary>
    /// PDF template base class. 
    /// Definition and base code to use with PDF writer implementations.
    /// 
    /// </summary>
    public abstract class PDFTemplate
    {
        #region Static


        //20130603 :: mellorasinxelas
        /// <summary>
        /// Var to use optional tags(true) or original mode(false). When you use optional tags,
        /// if a var has 'optional' var name with true value, behaviour as like original behaviour. example:
        /// <example>
        ///   var name="x" optional="yes"/
        /// result: if x value found, replace but if not found write 'x'.
        ///  var name="x" optional="no"/
        ///  var name="x" />
        /// result: if x value found, replace but if not found write ''.
        /// 
        /// </example>
        /// </summary>
        public static bool UseOptionalTags = false;

        #endregion

        #region Constants

        /// <summary>
        /// PDF template var element attr constant
        /// </summary>
        public const string VarElementConstant = "var";

        /// <summary>
        /// Custom element tag. Needs classname attribute to configure correctly
        /// </summary>
        public const string CustomElementConstant = "custom";


        #endregion

        #region Vars and properties

        private XmlDocument _xmlDoc = new XmlDocument();

        /// <summary>
        /// XML doc with PDF template structure.
        /// </summary>
        public XmlDocument XMLTemplate
        {
            get { return _xmlDoc; }
        }

        private PageDef _pageDef;
        /// <summary>
        /// PDF page definition
        /// </summary>
        public PageDef PageDefinition
        {
            get { return _pageDef; }
            protected set
            {
                _pageDef = value;
            }
        }

        private PDFDraw.IPDFDraw _pdfDraw;
        /// <summary>
        /// PDF drawer.
        /// </summary>
        protected Moon.PDFDraw.IPDFDraw PdfDrawer
        {
            get { return _pdfDraw; }
            set { _pdfDraw = value; }
        }

        private XmlAttributeCollection _defaultFontAttrs;

        /// <summary>
        /// Document font attrs 
        /// </summary>
        public XmlAttributeCollection DefaultFontAttrs
        {
            get { return _defaultFontAttrs; }
        }

        /// <summary>
        /// Header data 
        /// </summary>
        protected IDictionary headerData = new Hashtable();

        /// <summary>
        /// Footer data
        /// </summary>
        protected IDictionary footerData = new Hashtable();

        /// <summary>
        /// Body data
        /// </summary>
        protected IDictionary bodyData = new Hashtable();

        private int _pageCount = 1;
        /// <summary>
        /// Page counter
        /// </summary>
        protected int PageCount
        {
            get { return _pageCount; }
        }

        private int _drawCallCounter = 0;

        /// <summary>
        /// Number of Draw() calls. 
        /// </summary>
        public int DrawCallCounter
        {
            get { return _drawCallCounter; }
            set { _drawCallCounter = value; }
        }


        private List<int> _eachPageCount = new List<int>();
        /// <summary>
        /// Page store counters.
        /// </summary>
        protected List<int> EachPageCount
        {
            get { return _eachPageCount; }
        }

        /// <summary>
        /// The page number box.
        /// </summary>
        protected List<PageNumber> PageNumberBoxes = new List<PageNumber>();

        protected Orientation CurrentOrientation = Orientation.Portrait;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="xmlFile"></param>
        public PDFTemplate(string xmlFile)
        {
            _xmlDoc.Load(xmlFile);
            init();
            _buildPageDef();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="xmldoc"></param>
        public PDFTemplate(XmlDocument xmldoc)
        {
            this._xmlDoc = xmldoc;
            init();
            _buildPageDef();
        }

        #endregion


        #region Abstracts

        /// <summary>
        /// implement set width and height value to PageDef
        /// </summary>
        protected abstract void SetPageDefWidthHeight(Orientation orientation);


        #endregion

        private void init()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<font fontsize='10' fonttype='Arial' fontstyle='regular' fontcolor='black'></font>");
            _defaultFontAttrs = doc.DocumentElement.Attributes;
        }

        /// <summary>
        /// Builds the page definition element.
        /// _build template object
        ///20130606 :: mellorasinxelas to write footer in two modes.
        /// </summary>
        protected virtual void _buildPageDef()
        {
            // TODO it would be usefull to check xml document against xsd

            // build the header, loop, body, footer from xml!
            XmlElement elmRoot = XMLTemplate.DocumentElement;
            if (elmRoot.Name.ToUpper() != "PAGEDEF")
            {
                throw new ArgumentException("Element Root not pagedef! pls check ur xml file!");
            }
            _pageDef = new PageDef(elmRoot.Attributes);

            var orientation = CurrentOrientation;
            if (XmlHelper.GetAttributeValue("pageorientation", PageDefinition.PageDefAttrs, "landscape").ToUpper() == "LANDSCAPE")
                orientation = Orientation.Landscape;

            SetPageDefWidthHeight(orientation);
            //Console.WriteLine("PageDef widthHeight: " + pageDef.Width + ", " + pageDef.Height);

            XmlNodeList header_nodes = elmRoot.SelectNodes("//header");
            if (header_nodes.Count > 0)
            {
                PageDefinition.Header = _buildDynamicRowGroup(header_nodes[0]);
            }
            XmlNodeList loop_nodes = elmRoot.SelectNodes("//loop");
            if (loop_nodes.Count > 0)
            {
                PageDefinition.Loop = _buildRowGroup(loop_nodes[0]);
            }
            XmlNodeList body_nodes = elmRoot.SelectNodes("//body");
            if (body_nodes.Count > 0)
            {
                PageDefinition.Body = _buildRowGroup(body_nodes[0]);
            }
            XmlNodeList footer_nodes = elmRoot.SelectNodes("//footer");
            if (footer_nodes.Count > 0)
            {
                //20130606 :: jaimelopez --> special elemet for footer.
                PageDefinition.Footer = _buildDynamicRowGroup(footer_nodes[0], true);
                //---
            }
            //Console.WriteLine("call _buildPageDef in PDFTemplate");
        }

        //20130603 :: mellorasinxelas
        /// <summary>
        /// Builds a text box element
        /// </summary>
        /// <param name="node"></param>
        /// <param name="fontAttrs"></param>
        /// <returns></returns>
        public TextBox BuildTextBox(XmlNode node, XmlAttributeCollection fontAttrs)
        {
            string text = XmlHelper.GetAttributeValue(TextBox.TextAttributeConstant, node.Attributes, "");

            List<Variable> vars = new List<Variable>();

            XmlNodeList varNodes = node.SelectNodes(VarElementConstant);
            foreach (XmlNode varNode in varNodes)
            {
                string name = XmlHelper.GetAttributeValue(Variable.NameAttributeConstant, varNode.Attributes, "");

                if (name != string.Empty)
                {

                    //load formatter
                    DefaultFormat? formatter = null;
                    string formatterParams = null;
                    formatter = BasicVarFormatter.Parse(XmlHelper.GetAttributeValue(FormatableVariable.FormatterAttributeConstant, varNode.Attributes, null));
                    if (formatter != null)
                    {
                        formatterParams = XmlHelper.GetAttributeValue(FormatableVariable.FormatterParametersAttributeConstant, varNode.Attributes, null);
                    }

                    if (UseOptionalTags)
                    {
                        bool optional = XmlHelper.GetAttributeBoolean(Variable.OptionalAttributeConstant, varNode.Attributes, false);

                        if (formatter == null)
                        {
                            vars.Add(new Variable(name, optional));
                        }
                        else
                        {
                            vars.Add(new FormatableVariable(name, optional, formatter.Value, formatterParams));
                        }
                    }
                    else
                    {
                        if (formatter == null)
                        {
                            vars.Add(new Variable(name));
                        }
                        else
                        {
                            vars.Add(new FormatableVariable(name, formatter.Value, formatterParams));
                        }
                    }
                }
            }

            TextBox textBox = new TextBox(text, fontAttrs, node.Attributes, vars);

            return textBox;
        }


        //20130406 :: mellorasinxelas
        /// <summary>
        /// Build programmer custom element.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="fontAttrs"></param>
        /// <returns></returns>
        public DrawElement BuildCustomElement(XmlNode node, XmlAttributeCollection fontAttrs)
        {

            //load object name 
            string classname = XmlHelper.GetAttributeValue(DefaultCustomElement.ClassNameAttributeCustomElement, node.Attributes, null);
            if (classname == null) throw new NullReferenceException("Custom elements needs Class Name!");

            //load VARS

            List<Variable> vars = new List<Variable>();

            XmlNodeList varNodes = node.SelectNodes(VarElementConstant);
            foreach (XmlNode varNode in varNodes)
            {
                string name = XmlHelper.GetAttributeValue(Variable.NameAttributeConstant, varNode.Attributes, "");

                if (name != string.Empty)
                {

                    //load formatter
                    DefaultFormat? formatter = null;
                    string formatterParams = null;
                    formatter = BasicVarFormatter.Parse(XmlHelper.GetAttributeValue(FormatableVariable.FormatterAttributeConstant, varNode.Attributes, null));
                    if (formatter != null)
                    {
                        formatterParams = XmlHelper.GetAttributeValue(FormatableVariable.FormatterParametersAttributeConstant, varNode.Attributes, null);
                    }

                    if (UseOptionalTags)
                    {
                        bool optional = XmlHelper.GetAttributeBoolean(Variable.OptionalAttributeConstant, varNode.Attributes, false);
                        if (formatter == null)
                        {
                            vars.Add(new Variable(name, optional));
                        }
                        else
                        {
                            vars.Add(new FormatableVariable(name, optional, formatter.Value, formatterParams));
                        }
                    }
                    else
                    {
                        if (formatter == null)
                        {
                            vars.Add(new Variable(name));
                        }
                        else
                        {
                            vars.Add(new FormatableVariable(name, formatter.Value, formatterParams));
                        }
                    }
                }
            }

            //load object

            Type tLoad = null;
            try
            {
                tLoad = AssembliesTypesLoader.GetType(classname);
            }
            catch (Exception ex1)
            {
                Console.WriteLine(ex1.ToString());
            }

            if (tLoad == null) throw new Exception("Custom element '" + classname + "' type don't found!");

            object objLoad = null;
            try
            {
                objLoad = Activator.CreateInstance(tLoad, new object[] { fontAttrs, node.Attributes, vars });
            }
            catch (Exception ex2)
            {
                try
                {
                    objLoad = Activator.CreateInstance(tLoad, new object[] { fontAttrs, node.Attributes });
                }
                catch (Exception ex3)
                {
                    Console.WriteLine("Custom element must have (fontAttr, textAttr) builder or (fontAttr, textAttr, List<Variables>)");
                    Console.WriteLine(ex2.ToString());
                    Console.WriteLine(ex3.ToString());
                }
            }
            if (objLoad == null) throw new NullReferenceException("Object '" + classname + "' don't loaded!");
            if (!(objLoad is DrawElement))
            {
                throw new Exception("Object '" + classname + "' type doesn't match!. DrawElement is required!");
            }

            return (DrawElement)objLoad;
        }
        //----

        //---

        //20130603 :: mellorasinxelas
        /// <summary>
        /// Builds a page number element
        /// </summary>
        /// <param name="node"></param>
        /// <param name="fontAttrs"></param>
        /// <returns></returns>
        private PageNumber _buildPageNumber(XmlNode node, XmlAttributeCollection fontAttrs)
        {
            string text = XmlHelper.GetAttributeValue(TextBox.TextAttributeConstant, node.Attributes, "");

            List<Variable> vars = new List<Variable>();

            XmlNodeList varNodes = node.SelectNodes(VarElementConstant);
            foreach (XmlNode varNode in varNodes)
            {
                string name = XmlHelper.GetAttributeValue(Variable.NameAttributeConstant, varNode.Attributes, "");

                if (name != string.Empty)
                {

                    //load formatter
                    DefaultFormat? formatter = null;
                    string formatterParams = null;
                    formatter = BasicVarFormatter.Parse(XmlHelper.GetAttributeValue(FormatableVariable.FormatterAttributeConstant, varNode.Attributes, null));
                    if (formatter != null)
                    {
                        formatterParams = XmlHelper.GetAttributeValue(FormatableVariable.FormatterParametersAttributeConstant, varNode.Attributes, null);
                    }

                    if (UseOptionalTags)
                    {
                        bool optional = XmlHelper.GetAttributeBoolean(Variable.OptionalAttributeConstant, varNode.Attributes, false);
                        if (formatter == null)
                        {
                            vars.Add(new Variable(name, optional));
                        }
                        else
                        {
                            vars.Add(new FormatableVariable(name, optional, formatter.Value, formatterParams));
                        }
                    }
                    else
                    {
                        if (formatter == null)
                        {
                            vars.Add(new Variable(name));
                        }
                        else
                        {
                            vars.Add(new FormatableVariable(name, formatter.Value, formatterParams));
                        }
                    }
                }
            }

            PageNumber _pageNumberBox = new PageNumber(text, fontAttrs, node.Attributes, vars);
            PageNumberBoxes.Add(_pageNumberBox);
            return _pageNumberBox;
        }
        //--


        //20130603 :: mellorasinxelas
        /// <summary>
        /// Builds a text block element
        /// </summary>
        /// <param name="node"></param>
        /// <param name="fontAttrs"></param>
        /// <returns></returns>
        private TextBlock _buildTextBlock(XmlNode node, XmlAttributeCollection fontAttrs)
        {
            string text = XmlHelper.GetAttributeValue(TextBlock.TextAttributeConstant, node.Attributes, "");
            List<Variable> vars = new List<Variable>();

            XmlNodeList varNodes = node.SelectNodes(VarElementConstant);
            foreach (XmlNode varNode in varNodes)
            {
                string name = XmlHelper.GetAttributeValue(Variable.NameAttributeConstant, varNode.Attributes, "");

                if (name != string.Empty)
                {

                    //load formatter
                    DefaultFormat? formatter = null;
                    string formatterParams = null;
                    formatter = BasicVarFormatter.Parse(XmlHelper.GetAttributeValue(FormatableVariable.FormatterAttributeConstant, varNode.Attributes, null));
                    if (formatter != null)
                    {
                        formatterParams = XmlHelper.GetAttributeValue(FormatableVariable.FormatterParametersAttributeConstant, varNode.Attributes, null);
                    }

                    if (UseOptionalTags)
                    {
                        bool optional = XmlHelper.GetAttributeBoolean(Variable.OptionalAttributeConstant, varNode.Attributes, false);
                        if (formatter == null)
                        {
                            vars.Add(new Variable(name, optional));
                        }
                        else
                        {
                            vars.Add(new FormatableVariable(name, optional, formatter.Value, formatterParams));
                        }
                    }
                    else
                    {
                        if (formatter == null)
                        {
                            vars.Add(new Variable(name));
                        }
                        else
                        {
                            vars.Add(new FormatableVariable(name, formatter.Value, formatterParams));
                        }
                    }
                }
            }
            TextBlock textBlock = new TextBlock(text, fontAttrs, node.Attributes, vars);

            return textBlock;
        }
        //--

        /// <summary>
        /// Builds a line
        /// </summary>
        /// <param name="node"></param>
        /// <param name="fontAttrs"></param>
        /// <returns></returns>
        private Line _buildLine(XmlNode node, XmlAttributeCollection fontAttrs)
        {
            Line line = new Line(fontAttrs, node.Attributes);

            return line;
        }

        //20130603 :: mellorasinxelas
        /// <summary>
        /// Builds a image
        /// </summary>
        /// <param name="node"></param>
        /// <param name="fontAttrs"></param>
        /// <returns></returns>
        public Image BuildImage(XmlNode node, XmlAttributeCollection fontAttrs)
        {
            //NOT support format.
            List<Variable> vars = new List<Variable>();

            XmlNodeList varNodes = node.SelectNodes(VarElementConstant);
            foreach (XmlNode varNode in varNodes)
            {
                string name = XmlHelper.GetAttributeValue(Variable.NameAttributeConstant, varNode.Attributes, "");
                if (name != string.Empty)
                {
                    if (UseOptionalTags)
                    {
                        vars.Add(new Variable(name,
                                               XmlHelper.GetAttributeBoolean(Variable.OptionalAttributeConstant, varNode.Attributes, false)
                                              )
                                );
                    }
                    else
                    {
                        vars.Add(new Variable(name));
                    }
                }
            }

            return new Image(fontAttrs, node.Attributes, vars);
        }
        //---
        /// <summary>
        /// Add to the row all XML elements defined in node.
        /// Supports: textbox, line,image,textblock,pagenumber and customelement
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="node"></param>
        /// <param name="font"></param>
        private void _buildDrawElement(Row row, XmlNode node, XmlAttributeCollection font)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                switch (child.Name)
                {
                    case "textbox":
                        row.AddDrawElement(BuildTextBox(child, font));
                        break;
                    case "line":
                        row.AddDrawElement(_buildLine(child, font));
                        break;
                    case "image":
                        row.AddDrawElement(BuildImage(child, font));
                        break;
                    case "textblock":
                        row.AddDrawElement(_buildTextBlock(child, font));
                        break;
                    case "pagenumber":
                        row.AddDrawElement(_buildPageNumber(child, font));
                        break;
                    case CustomElementConstant:
                        row.AddDrawElement(BuildCustomElement(child, font));
                        break;
                }
                if (child.Name == "font" && child.HasChildNodes)
                {
                    _buildDrawElement(row, child, child.Attributes);
                }
            }
        }
        /// <summary>
        /// Builds a PDF row.
        /// </summary>
        /// <param name="node">node of xml row</param>
        /// <param name="font"></param>
        /// <returns></returns>
        public Row _buildRow(XmlNode node, XmlAttributeCollection font)
        {
            Row row = new Row(PageDefinition.Margin_left, PageDefinition.Width - PageDefinition.Margin_right);
            _buildDrawElement(row, node, font);
            row.CalculateWidth();

            return row;
        }

        //20130606 :: jaimelopez
        /// <summary>
        /// Builds a PDF row group
        /// Original function.
        /// </summary>
        /// <param name="node">node of xml header, body, footer, loop</param>
        /// <returns></returns>
        public RowGroup _buildRowGroup(XmlNode node)
        {
            return _buildRowGroup(node, false);
        }

        public DynamicRowGroup _buildDynamicRowGroup(XmlNode node)
        {
            return new DynamicRowGroup
            {
                XmlNode = node,
                RowGroup = _buildRowGroup(node, false)
            };
        }

        public DynamicRowGroup _buildDynamicRowGroup(XmlNode node, bool estFooter)
        {
            return new DynamicRowGroup
            {
                XmlNode = node,
                RowGroup = _buildRowGroup(node, estFooter)
            };
        }

        /// <summary>
        /// </summary>
        /// 
        /// <param name="node">node of xml header, body, footer, loop</param>
        /// <param name="footer">True if its a footer page element</param>
        /// <returns></returns>
        public RowGroup _buildRowGroup(XmlNode node, bool footer)
        {
            //20130606 :: jaimelopez
            RowGroup rowGroup = null;

            if (footer)
            {
                rowGroup = new FooterGroup(XmlHelper.GetAttributeBoolean(FooterGroup.AbsoluteAttribute, node.Attributes, false));
            }
            else
            {
                rowGroup = new RowGroup();
            }

            //--

            rowGroup.Y = XmlHelper.GetFloatAttributeValue("y", node.Attributes, -1);

            XmlAttributeCollection _font = DefaultFontAttrs;
            if (node.FirstChild.Name == "font")
            {
                _font = node.FirstChild.Attributes;
            }

            XmlNodeList rowNodes = node.SelectNodes(".//row");
            foreach (XmlNode rowNode in rowNodes)
            {
                if(rowNode.Name == "row")
                    rowGroup.AddRow(_buildRow(rowNode, _font));
            }

            return rowGroup;
        }
        //--

        //--------------------------------------------------------------------
        // _draw to pdf

        /// <summary>
        /// draw a row of drawElement to the pdf
        /// </summary>
        /// <param name="drawElements"></param>
        /// <param name="data"></param>
        public void DrawRow(List<DrawElement> drawElements, IDictionary data)
        {
            foreach (DrawElement drawElement in drawElements)
            {

#if DEBUG
                //Console.WriteLine(drawElement.GetType());
#endif
                // skip PageNumber element
                if (drawElement is PageNumber)
                {
                    ((PageNumber)drawElement).SetY(PdfDrawer.CurrentY());
                    continue;
                }
                drawElement.Draw(PdfDrawer, data);
            }
        }


        /// <summary>
        /// Draw rowGroup to the Pdf, when goto nextPage, repeat the header!
        /// 201306060 :: mellorasinxelas to write absolute groups
        /// </summary>
        /// <param name="rowGroup"></param>
        /// <param name="data"></param>
        /// <param name="dGroup"></param>
        public void DrawRowGroup(RowGroup rowGroup, System.Collections.IDictionary data, DocumentGroup dGroup)
        {
            bool absolute = false;
            if (rowGroup is Moon.PDFTemplate.FooterGroup)
            {
                absolute = ((Moon.PDFTemplate.FooterGroup)rowGroup).Absolute;
            }

            float oldY = -1;
            bool ignoreAddRow = false;
            if (absolute)
            {
                oldY = PdfDrawer.CurrentY();
                PdfDrawer.SetY(PageDefinition.Margin_bottom + ((rowGroup.Y != -1.0F) ? rowGroup.Y : rowGroup.Height));//20130610 :: Use sY if is stored, otherwise uses Height.
                ignoreAddRow = true; //position has been stored.
            }

            foreach (Row row in rowGroup.Rows)
            {
                if (!absolute)
                {
                    if (PdfDrawer.isNoMoreY(row.Height, dGroup))
                    {
                        NextPage();
                        DrawHeader();
                    }
                }

                if (ignoreAddRow)
                {
                    ignoreAddRow = false;
                }
                else
                {
                    PdfDrawer.NextRow(row.Height, dGroup);
                }

                DrawRow(row.DrawElements, data);
            }

            //returns to old Y value.
            if (absolute)
            {
                PdfDrawer.SetY(oldY);
            }
        }

        /// <summary>
        /// Draws the document header
        /// </summary>
        public void DrawHeader()
        {
            RowGroup rowGroup = PageDefinition.Header.RowGroup;

            foreach (Row row in rowGroup.Rows)
            {
                PdfDrawer.NextRow(row.Height, DocumentGroup.Header);
                DrawRow(row.DrawElements, headerData);
            }
        }

        /// <summary>
        /// Draws the document loop
        /// </summary>
        /// <param name="loopData"></param>
        protected void _drawLoop(List<Hashtable> loopData)
        {
            RowGroup rowGroup = PageDefinition.Loop;
            foreach (Hashtable data in loopData)
            {
                DrawRowGroup(rowGroup, data, DocumentGroup.Loop);
            }
        }

        //jaimelopez :: 20130606
        /// <summary>
        /// Draws the document footer.
        /// </summary>
        protected void _drawFooter()
        {
            RowGroup rowGroup = PageDefinition.Footer.RowGroup;
            //			if(rowGroup is FooterGroup){
            //				if( ((FooterGroup)rowGroup).Absolute) return; //print int NextPage operation.
            //			}

            if (!(rowGroup is FooterGroup) || !((FooterGroup)rowGroup).Absolute)
            {
                if (rowGroup.Y != -1)
                {
                    if (PdfDrawer.CurrentY() < rowGroup.Y)
                    {
                        NextPage();
                        DrawHeader();
                    }
                    PdfDrawer.SetY(rowGroup.Y);
                }
            }

            DrawRowGroup(rowGroup, footerData, DocumentGroup.Footer);

        }


        /// <summary>
        /// Draws the document. Generate a new document.
        /// </summary>
        /// <param name="headerData"></param>
        /// <param name="loopData"></param>
        /// <param name="bodyData"></param>
        /// <param name="footerData"></param>
        public void Draw(
            Hashtable headerData,
            List<Hashtable> loopData,
            Hashtable bodyData,
            Hashtable footerData)
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
            if (DrawCallCounter > 0)
            {
                // how many times the Draw() hv been call! if it was call b4, this time we draw to a new page!
                NextPage();
            }

            DrawHeader();
            if (loopData != null)
            {
                _drawLoop(loopData);
            }
            DrawRowGroup(PageDefinition.Body, bodyData, DocumentGroup.Body);
            _drawFooter();

            EachPageCount.Add(PageCount);
            DrawCallCounter++;
        }

        /// <summary>
        /// Create a next document page.
        /// </summary>
        public void NextPage()
        {
            NextPage(CurrentOrientation);
        }

        /// <summary>
        /// Create a next document page with param orientation.
        /// </summary>
        public void NextPage(Orientation orientation)
        {
            //201306060 :: jaimelopez :: absolute footer
            if (PageDefinition.Footer.RowGroup is FooterGroup && ((FooterGroup)PageDefinition.Footer.RowGroup).Absolute)
            {
                //process footer in each page...
                DrawRowGroup(PageDefinition.Footer.RowGroup, footerData, DocumentGroup.Footer);
            }

            SetPageDefWidthHeight(orientation);
            
            // we regenerate footer each time, because with may vary with landscape / portrait
            PageDefinition.Header.RowGroup = _buildRowGroup(PageDefinition.Header.XmlNode);
            PageDefinition.Footer.RowGroup = _buildRowGroup(PageDefinition.Footer.XmlNode, true);

            PdfDrawer.NextPage();
            _pageCount++;
            //Console.WriteLine("PDFTemplate.NextPage() pageCount: " + pageCount);
        }

        /// <summary>
        /// 
        /// </summary>
        [Conditional("DEBUG")]
        protected void Debug()
        {
            foreach (int num in EachPageCount)
            {
                Console.WriteLine("eachPageCount: " + num);
            }
        }
    }
}
