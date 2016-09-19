using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace testPDFTemplateItextSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            TestArStatementXML();
        }

        public static void TestArStatementXML()
        {

            // need to load CJK b4 create PDFTemplateItextSharp
            // itextsharp.dll 5.3.5 ok
            // itextsharp.dll 5.1.3 not ok
            //iTextSharp.text.pdf.BaseFont.AddToResourceSearch("iTextAsian.dll"); // iTextAsian.dll 2.1.0.0

            // istextsharp.dll 5.5.9 ok
            iTextSharp.text.io.StreamUtil.AddToResourceSearch("iTextAsian.dll"); // iTextAsian.dll 2.1.0.0

            Moon.PDFTemplateItextSharp.PDFTemplateItextSharp pdfTpl = 
                new Moon.PDFTemplateItextSharp.PDFTemplateItextSharp(@"file\ar_statement.xml");

            //// verdana font
            pdfTpl.RegisterFont(@"C:\Windows\Fonts\verdana.ttf", "verdana", "");

            //pdfTpl.RegisterFont(@"SimSun.ttf", "Simsun", "Identity-H");

            //// using CJK
            //// no need to give path for register CJK font
            pdfTpl.RegisterFont("", "STSong-Light", "UniGB-UCS2-H");
            //pdfTpl.RegisterFont("", "MHei-Medium", "UniCNS-UCS2-H");

            pdfTpl.RegisterFont(@"c:\Windows\Fonts\arial.ttf", "Arial", "Identity-H");
            //// enable to detect Arabic and Hebrew
            pdfTpl.DetectRightToLeft = true;

            Hashtable headerData = new Hashtable();

            headerData.Add("{short_name}", "shortName");
            headerData.Add("{name}", "Name");
            headerData.Add("{address1}", "Address1 '繁體'，'简体'");
            headerData.Add("{address2}", "Address2");
            headerData.Add("{tel}", "Tel");

            string arabic_welcome = "أهلا وسهلا";
            headerData.Add("{fax}", arabic_welcome);

            headerData.Add("{statement_date}", "statement date");

            headerData.Add("{cus_company_name}", "cus company name");
            headerData.Add("{cus_name}", "Name");
            headerData.Add("{cus_add1}", "Address1 '繁體'，'简体'");
            headerData.Add("{cus_add2}", "Address2 '繁體'，'简体'");
            headerData.Add("{cus_add3}", "Address3 '繁體'，'简体'");
            headerData.Add("{cus_add4}", "Address4 '繁體'，'简体'");
            headerData.Add("{cus_tel}", "Tel");
            headerData.Add("{cus_fax}", "Fax");
            headerData.Add("{cus_area}", "Area");

            List<Hashtable> loopData = new List<Hashtable>();

            for (int i = 0; i < 100; i++)
            {
                Hashtable data = new Hashtable();
                data.Add("{date}", "date" + i);
                data.Add("{due_date}", "due_date" + i);
                data.Add("{reference}", "reference");
                data.Add("{type}", "type");
                data.Add("{amount}", i.ToString());
                data.Add("{balance}", i.ToString());
                loopData.Add(data);

            }

            Hashtable bodyData = new Hashtable();
            Hashtable footerData = new Hashtable();
            footerData.Add("{tbalance}", "tbalance");

            pdfTpl.Draw(headerData, loopData, bodyData, footerData);
            //pdfTpl.Draw(headerData, loopData, bodyData, footerData, null, new List<Hashtable>(), null);

            System.IO.FileStream file = new System.IO.FileStream(@"Statement.pdf", System.IO.FileMode.Create);
            System.IO.MemoryStream mStream = pdfTpl.Close();
            byte[] bytes = mStream.ToArray();
            file.Write(bytes, 0, bytes.Length);
            file.Close();
            mStream.Close();

        }
    }
}
