/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 10/06/2013
 * Time: 9:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;

namespace TestApp
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		public enum Option {Template, Pdf, Debug, None};
		
		public const string OptionTemplate = "-t";
		public const string OptionDebug = "-d";
		public const string OptionPDFViewer = "-pdf";
		
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			
			MainForm mainF = new MainForm();
						
			//process args
			if(args != null){
				Option actualOption = Option.None;
				foreach( string act in args ){
					
					bool jump = false;
					switch( actualOption ){
						case Option.Template: 
							mainF.Template = act;							
							jump = true;
							break;
						case Option.Pdf:
							mainF.PDFViewer = act;
							jump = true;
							break;						
					}
					
					if(jump){
						actualOption = Option.None;
					}
					else{
						if(OptionDebug.Equals(act)){
							mainF.Debug = true;
						}
						else if (OptionTemplate.Equals(act)){
							actualOption = Option.Template;
						}
						else if(OptionPDFViewer.Equals(act)){
							actualOption = Option.Pdf;
						}
					}					
				}//end foreach
			}//end if
						
			Application.Run(mainF);
		}
		
	}
}
