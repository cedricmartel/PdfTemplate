/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 06/06/2013
 * Time: 11:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Moon.PDFTemplate
{
	/// <summary>
	/// Foot group. Allows absolute mode that write it after document it's writed and change document margin pages.
	/// <para/>Attributes: absolute
	/// </summary>
	public class FooterGroup : RowGroup
	{		
		/// <summary>
		/// For footer element. Indicates if footer is absolute -> all pages and change margins or only last page. Values true|false
		/// </summary>
		public const string AbsoluteAttribute = "absolute";
		
		
		private bool _absolute = false;
		/// <summary>
		/// If is absolute, change document margins and write in all pages.
		/// </summary>
		public bool Absolute {
			get { return _absolute; }
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public FooterGroup()
		{
		}
		
		/// <summary>
		/// If is absolute, change document margins and write in all pages.
		/// </summary>
		public FooterGroup( bool absolute)
		{
			this._absolute = absolute;
		}
	}
}
