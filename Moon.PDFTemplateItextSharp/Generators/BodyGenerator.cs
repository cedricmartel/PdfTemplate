using System;
using System.Collections;
using System.Xml;
using Moon.PDFDraw;
using Moon.PDFTemplate;
using Moon.PDFTemplateItextSharp.Model;

namespace Moon.PDFTemplateItextSharp.Generators
{
    /// <summary>
    /// Class for converting body template to pdf output
    /// </summary>
    public class BodyGenerator
    {
        private readonly XmlNodeList bodyItems;
        private readonly XmlNode bodyNode;

        private readonly PDFTemplate.PDFTemplate pdfTemplate;


        /// <summary>
        /// generator for body tag
        /// </summary>
        public BodyGenerator(PDFTemplate.PDFTemplate template, XmlNode bodyNodeXml)
        {
            pdfTemplate = template;
            bodyNode = bodyNodeXml;
            if (bodyNode != null)
                bodyItems = bodyNode.SelectNodes(".//row|.//table");
        }

        /// <summary>
        /// draw row and table items inside body. 
        /// table items are treated as tables 
        /// </summary>
        public void DrawBody(Hashtable data, IPDFDraw drawer)
        {
            if (bodyItems == null)
                return;

            XmlAttributeCollection font = pdfTemplate.DefaultFontAttrs;
            if (bodyNode.FirstChild.Name == "font")
            {
                font = bodyNode.FirstChild.Attributes;
            }

            foreach (XmlNode itemNode in bodyItems)
            {
                if (!IsNodeVisible(itemNode, data))
                    continue;

                if (itemNode.Name == "row")
                {
                    RowGroup rowGroup = new RowGroup
                    {
                        Y = Helper.GetFloatAttributeValue("y", bodyNode.Attributes, -1)
                    };
                    rowGroup.AddRow(pdfTemplate._buildRow(itemNode, font));
                    pdfTemplate.DrawRowGroup(rowGroup, data, DocumentGroup.Body);
                }
                else if (itemNode.Name == "table")
                {
                    var tableGenerator = new TableGenerator(pdfTemplate, itemNode);
                    if (itemNode.Attributes == null)
                        continue;
                    var keyAttribute = itemNode.Attributes["var"];
                    if (keyAttribute == null || !data.ContainsKey(keyAttribute.Value))
                        continue;
                    var tableParameters = data[keyAttribute.Value];
                    if (!(tableParameters is TableData))
                        throw new Exception("table parameter must be of type TableData");

                    tableGenerator.DrawTable((TableData) (tableParameters), drawer);
                }
            }
        }


        /// <summary>
        /// row and table items in body can have attribute "condition" which can be set to a variable name
        /// lets say "condition"="{some_key"}"
        /// if bodyData["{some_key}"] == false, it won't be printed
        /// </summary>
        private bool IsNodeVisible(XmlNode node, Hashtable bodyData)
        {
            if (node.Attributes == null)
                return true;
            var conditionAttribute = node.Attributes["condition"];
            if (conditionAttribute == null)
                return true;
            var conditionKey = node.Attributes["condition"].Value;
            if (string.IsNullOrEmpty(conditionKey))
                return true;

            if (!bodyData.ContainsKey(conditionKey))
                return true;
            var data = bodyData[conditionKey];
            if (data == null)
                return false;
            return (data is bool && (bool)data);
        }

    }
}
