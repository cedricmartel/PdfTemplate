/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 10/06/2013
 * Time: 9:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using TestApp.Data;

namespace TestApp
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private bool debug = false;
		
		public bool Debug {
			get { return debug; }
			set { debug = value; }
		}
		
		public string Template{
			set{
				this.txtTemplate.Text = value;
			}
		}
		
		
		public string PDFViewer{
			set{
				this.txtViewer.Text = value;
			}
		}
		
		
		private Hashtable documentData = new Hashtable();
		private List<Hashtable> loopData= new List<Hashtable>();
		
		
		public MainForm()
		{			
			InitializeComponent();
			
			Console.SetOut( new TextWriterConsole( this.lstConsole ) );			
			Console.WriteLine(this.Text + " loaded .. ");		
			
			Console.WriteLine("Loading data ... ");	
			LoadData();		
			Console.Write("Loaded!");			
		}
		
		
		void Button1Click(object sender, EventArgs e)
		{
			//print
			
			try{
								
				if(this.txtTemplate.Text == null)throw new ArgumentNullException("Print Template");
				
				if(Debug){
					Console.WriteLine("Make PDF with template -> "+this.txtTemplate.Text);
				}
				
				//load
//				XmlDocument xmlDoc = new XmlDocument();
//				xmlDoc.LoadXml( this.txtTemplate.Text );
//				
				Moon.PDFTemplateItextSharp.PDFTemplateItextSharp pdfTemplate = new Moon.PDFTemplateItextSharp.PDFTemplateItextSharp( /*xmlDoc*/this.txtTemplate.Text );
			
				//FONTS
				//iTextSharp.text.pdf.BaseFont.AddToResourceSearch("iTextAsian.dll");
            	
				// verdana font
				pdfTemplate.RegisterFont(@"C:\Windows\Fonts\verdana.ttf", "verdana", "");

				// using CJK				
				//drawer.RegisterFont("", "STSong-Light", "UniGB-UCS2-H");
				pdfTemplate.RegisterFont("", "MHei-Medium", "UniCNS-UCS2-H");
				//---
				
			   //create
				pdfTemplate.Draw( documentData, loopData, documentData, documentData ,
				                 documentData, loopData, documentData               );
			
				
				string pdfFileName = Path.GetTempFileName();
				if(pdfFileName == null)throw new NullReferenceException("PDF File Name");
				
				//save PDF
				FileStream filePDF = new FileStream(pdfFileName, FileMode.Create );
				MemoryStream stream = pdfTemplate.Close();
				byte[] content = stream.ToArray();
				filePDF.Write( content, 0, content.Length );
				filePDF.Close();
				stream.Close();				
				
				//browse
				if(this.txtViewer.Text != null || this.txtViewer.Text.Trim() != ""){
					if(Debug){
						Console.WriteLine("Showing PDF -> "+ pdfFileName);
					}
					System.Diagnostics.Process.Start(this.txtViewer.Text , pdfFileName );
				}
				
			}catch(Exception ex){
				MessageBox.Show(ex.ToString());
			}
			
		}
		
		/// <summary>
		/// Load default data.
		/// 20130719 :: Add new data to template_v3
		/// </summary>
		protected void LoadData(){
			
			
			//load head data...
			this.documentData.Add("{concept}", "This is the text of the Concept VAR");
			this.documentData.Add("{notes }", "This is the text of Document Notes VAR");
			this.documentData.Add("{customer_name}", "Jimmy");
			this.documentData.Add("{customer_surname}", "Lee Jones");
			
			this.documentData.Add("{id_number}", "00000000A");
			this.documentData.Add("{customer_cp}", "32627");
			this.documentData.Add("{customer_address}", "Pedreiras 8 (Caldeliñas)");
			this.documentData.Add("{customer_town}", "Verín");
			this.documentData.Add("{customer_province}", "Ourense");
			
			this.documentData.Add("{number}", "0125/56");
			this.documentData.Add("{creation_date}", DateTime.Now );
			this.documentData.Add("{total_lines_amount}", 25.00 );
			this.documentData.Add("{taxable_amount}", 22.50);
			//this.documentData.Add("{total_document_amount}", 27.00);
			this.documentData.Add("{total_amount}", 27.00);
			this.documentData.Add("{vat_amount}", 4.50);
			this.documentData.Add("{vat_value}", 20);
			this.documentData.Add("{discount_value}", 10);
			this.documentData.Add("{discount_amount}", 2.50);
			
			//new document data
            documentData.Add("{short_name}", "shortName");
            documentData.Add("{name}", "Name");
            documentData.Add("{address1}", "Address1");
            documentData.Add("{address2}", "Address2");
            documentData.Add("{tel}", "Tel");
            documentData.Add("{fax}", "Fax");

            documentData.Add("{statement_date}", "statement date");

            documentData.Add("{cus_company_name}", "cus company name");
            documentData.Add("{cus_name}", "Name");
            documentData.Add("{cus_add1}", "Address1 繁體，简体");
            documentData.Add("{cus_add2}", "Address2 繁體，简体");
            documentData.Add("{cus_add3}", "Address3 繁體，简体");
            documentData.Add("{cus_add4}", "Address4 繁體，简体");
            documentData.Add("{cus_tel}", "Tel");
            documentData.Add("{cus_fax}", "Fax");
            documentData.Add("{cus_area}", "Area");
           
            documentData.Add("{tbalance}", "tbalance");
            //---            
			
			double totalR = 25;
			Random rDouble = new Random();
			Random rBool = new Random();
					
			int maxLines = 30;
			for(int i=0; i< maxLines; ++i){
				
				bool isFree = (rBool.Next() % 2) == 1;
				bool hasNotes = (rBool.Next() % 2) == 1;
				double amount = 0.0D;
				if(i < (maxLines -1)){
					amount = rDouble.NextDouble() % totalR;
					totalR -= amount;
				}
				else{
					amount = totalR;
				}
				
				Hashtable line = new Hashtable();
				if(isFree){
					line.Add( "{description}", "Free Line description" );					
				}
				else{
					line.Add( "{description}", "Line description" );
				}
				
				if(hasNotes)line.Add( "{notes}", "Line Notes" );
				line.Add( "{line_amount}", amount );
								
				line.Add( "{is_free_amount}", isFree );
				
				//new data
				line.Add("{date}", "date"+ i);
                line.Add("{due_date}", "due_date" + i);
                line.Add("{reference}", "reference");
                line.Add("{type}", "type");
                line.Add("{amount}", i.ToString());
                line.Add("{balance}", i.ToString());				
				//---
				
				this.loopData.Add( line );
			}
			
			 List<Hashtable> loopData = new List<Hashtable>();

			
		}
		
		void BtnDataDocClick(object sender, EventArgs e)
		{
			//common data
			new FormDocumentData( this.documentData ).ShowDialog();
		}
		
		void BtnDataLoopClick(object sender, EventArgs e)
		{
			//loop data
			new FormLoopData( this.loopData ).ShowDialog();
		}
		
		
		void LstConsoleDoubleClick(object sender, EventArgs e)
		{
			object data = null;
			
			try{
				data = this.lstConsole.Items[this.lstConsole.SelectedIndex];
			}catch(Exception){}
			
			if(data != null)MessageBox.Show( data.ToString() );
		}
		
		
		
		/// <summary>
		/// Console printer
		/// </summary>
		protected class TextWriterConsole : StringWriter{
			
			private ListBox _control = null; 
			public TextWriterConsole( ListBox control ){
				_control = control;
			}
						
			
			public override void WriteLine(object value)
			{
				//base.WriteLine(value);
				WriteToComponent( value );
			}
			
			public override void WriteLine(string value)
			{
				//base.WriteLine(value);
				WriteLine( (object)value );
			}
			
			public override void Write(object value)
			{
				//base.Write(value);
				WriteToComponent( value );
			}
			
			public override void Write(string value)
			{
				//base.Write(value);
				Write( (object)value );
			}
			
			
			private delegate void VoidStringDelegate( object data );
			
			protected void WriteToComponent(object data){
				if(this._control.InvokeRequired){
					this._control.Invoke( new VoidStringDelegate( WriteToComponent ), new Object[]{ data } );
				}
				else{
					if(data == null)data="";										
					this._control.Items.Add( data );
				}
			}
			
		}
		
		
	}
}
