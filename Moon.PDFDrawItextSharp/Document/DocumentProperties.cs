/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 14/06/2013
 * Time: 9:29
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Moon.PDFDrawItextSharp
{
	/// <summary>
	/// DocumentProperties. Allows define document properties to store in the output pdf file.
	/// </summary>
	public class DocumentProperties
	{
		/// <summary>
		/// Creator tag constant
		/// </summary>
		public const string CreatorConstant = "PDFTemplateItextSharp";
		
		#region Properties
		
		private string _author = null;
		/// <summary>
		/// Author of document.
		/// </summary>
		public string Author {
			get { return _author; }
		}
		private string _title = null;
		/// <summary>
		/// Title of document.
		/// </summary>
		public string Title {
			get { return _title; }
		}
		
		private bool _includeCreationDate = true;
		
		/// <summary>
		/// With true value sets creation date in the pdf document.
		/// </summary>
		public bool IncludeCreationDate {
			get { return _includeCreationDate; }
		}
		
		
		private string _keywords = null;
		/// <summary>
		/// Keywords
		/// </summary>
		public string Keywords {
			get { return _keywords; }
			set { _keywords = value; }
		}
		
		private string _creator = CreatorConstant;
		/// <summary>
		/// Creator APP
		/// </summary>
		public string Creator {
			get { return _creator; }
		}
		
		private string _subject = null;
		/// <summary>
		/// Gets subject
		/// </summary>
		public string Subject {
			get { return _subject; }
		}
		
		#endregion
		
		#region Builder 
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="title"></param>
		/// <param name="author"></param>
		public DocumentProperties(string title, string author)
		{
			CommonConstructor( title, author, true, null,null);
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="title"></param>
		/// <param name="author"></param>
		/// <param name="includeCreationDate"></param>
		public DocumentProperties(string title, string author, bool includeCreationDate)
		{
			CommonConstructor( title, author, includeCreationDate, null,null );
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="title"></param>
		/// <param name="author"></param>
		/// <param name="includeCreationDate"></param>
		/// <param name="subject"></param>
		/// <param name="keywords"></param>
		public DocumentProperties(string title, string author, bool includeCreationDate, string subject,  string keywords)
		{
			CommonConstructor( title, author, includeCreationDate, subject, keywords );
		}
		
		
		private void CommonConstructor(string title, string author, bool includeCreationDate, string subject, string keywords){
			
			this._title = title;
			this._author = author;
				
			this._includeCreationDate = includeCreationDate;			
			
			this._keywords = keywords;
			this._subject = subject;
		}
		
		#endregion
		
		
		
		

	}
	
	
	
}
