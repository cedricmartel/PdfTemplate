using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using Moon.PDFTemplateItextSharp.Model;

namespace fr.cedricmartel.SampleItextSharp.MultipleTables
{
    public partial class Default : Page
    {
        private readonly Hashtable headerData = new Hashtable();
        private readonly Hashtable bodyData = new Hashtable();
        private readonly Hashtable footerData = new Hashtable();

        protected void GenerationPdf(object sender, EventArgs e)
        {
            // templates load
            var template = Server.MapPath("test.xml");
            var pdfTemplate = new Moon.PDFTemplateItextSharp.PDFTemplateItextSharp(template);
            // TODO fonts externes 

            //  parameters load
            headerData.Add("{titreDocument}", "SERVICE DEPARTEMENTAL D'INCENDIE \nET DE SECOURS DES DEUX-SEVRES");
            headerData.Add("{logoUrl}", Server.MapPath("LogoPdf.jpg"));
            footerData.Add("{titreDocument}", "Titre du document");

            // data load
            var firstTable = new TableData
            {
                HeadData = new Hashtable(),
                LoopData = new List<Hashtable>(),
                FootData = new Hashtable()
            };
            DateTime debut = new DateTime(2016, 1, 1);
            for (int i = 0; i < 100; i++)
            {
                var donnees1 = new Hashtable
                {
                    {"{Date}", debut.AddDays(i)},
                    {"{Centre}", "Centre 1"},
                    {"{Frais}", 5},
                    {"{Nombre}", "200,00"},
                    {"{Base}", "5,00"},
                    {"{Montant}", i}
                };
                firstTable.LoopData.Add(donnees1);
            }
            firstTable.FootData.Add("{Total}", 250.5);
            bodyData.Add("{FirstTable}", firstTable);
            bodyData.Add("{SecondTable}", firstTable);

            // pdf generation
            pdfTemplate.Draw(headerData, bodyData, footerData);

            // save file locally
            string fileDirectory = Server.MapPath("../Output/");
            string fileName = "SimpleTest-" + String.Format("{0:yyyyMMdd-HHmmss}", DateTime.Now) + ".pdf";
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
    }
}