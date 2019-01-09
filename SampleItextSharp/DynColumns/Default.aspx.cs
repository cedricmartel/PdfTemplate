using Moon.PDFTemplateItextSharp.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace fr.cedricmartel.SampleItextSharp.DynColumns
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly Hashtable headerData = new Hashtable();
        private readonly Hashtable bodyData = new Hashtable();
        private readonly Hashtable footerData = new Hashtable();

        private Random randonGen;

        protected void GenerationPdf(object sender, EventArgs e)
        {
            randonGen = new Random();
            // templates load
            var template = Server.MapPath("test.xml");
            var pdfTemplate = new Moon.PDFTemplateItextSharp.PDFTemplateItextSharp(template);
            // TODO fonts externes 

            //  parameters load
            headerData.Add("{titreDocument}", "DOCUMENT \nTITLE");
            headerData.Add("{logoUrl}", Server.MapPath("LogoPdf.jpg"));
            footerData.Add("{titreDocument}", "Document Title");

            var nbDynamicColumns = randonGen.Next(2, 6);

            var percent = new List<int> { 25, 50, 75, 100 };

            // data load
            var firstTable = new TableData
            {
                DynamicColumns = new List<DynamicColumnDefinition>(),
                HeadData = new Hashtable(),
                LoopData = new List<Hashtable>(),
                FootData = new Hashtable()
            };
            for (var i = 1; i <= nbDynamicColumns; i++)
            {
                firstTable.DynamicColumns.Add(new DynamicColumnDefinition
                {
                    CellWidth = 1,
                    HeaderTemplate = "<tablecell border=\"Top, Bottom, Right\" backgroundcolor=\"#9BCFF9\"><textbox text=\"Column " + i + "\"></textbox></tablecell>",
                    DataTemplate = "<tablecell border=\"Bottom, Right\"><textbox text=\"{Val" + i + "}\"><var name=\"{Val" + i + "}\" /></textbox></tablecell>"
                });


            }

            DateTime debut = new DateTime(2016, 1, 1);
            for (int i = 0; i < 10; i++)
            {
                var donnees1 = new Hashtable { { "{Title}", "Line number " + i } };
                for (var j = 1; j <= nbDynamicColumns; j++)
                    donnees1.Add("{Val" + j + "}", RandomData());
                firstTable.LoopData.Add(donnees1);
            }
            bodyData.Add("{FirstTable}", firstTable);

            // pdf generation
            pdfTemplate.Draw(headerData, bodyData, footerData);

            // save file locally
            string fileDirectory = Server.MapPath("../Output/");
            string fileName = "MultipleTables-" + string.Format("{0:yyyyMMdd-HHmmss}", DateTime.Now) + ".pdf";
            using (var filePdf = new FileStream(fileDirectory + fileName, FileMode.Create))
            {
                using (MemoryStream stream = pdfTemplate.Close())
                {
                    byte[] content = stream.ToArray();
                    filePdf.Write(content, 0, content.Length);
                }
            }

            Resulat.Text = "Generated PDF: <a href='../Output/" + fileName + "'>" + fileName + "</a><br/><br/><iframe src='../Output/" + fileName + "' width='1024' height='600' />";
        }

        private string RandomData()
        {
            var hasValue = randonGen.Next(0, 3) == 1;
            if (!hasValue)
                return "";
            return randonGen.Next(100) + "%";
        }

    }
}