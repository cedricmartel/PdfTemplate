/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 07/06/2013
 * Time: 12:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;


namespace Moon.PDFTemplate
{
	/// <summary>
	/// Document background image.
	/// </summary>
	public class BackgroundImageDefinition 
	{
		/// <summary>
		/// Image alignment
		/// </summary>
		public enum Alignment {
			/// <summary>
			/// 
			/// </summary>
			Center,
			/// <summary>
			/// 
			/// </summary>
			TopLeft,
			/// <summary>
			/// 
			/// </summary>
			TopCenter,
			/// <summary>
			/// 
			/// </summary>
			TopRight };
				
		private string _source = null;
		/// <summary>
		/// Image source
		/// </summary>
		public string Source {
			get { return _source; }
		}
		
		
		private bool _useDocMargins = true;
		/// <summary>
		/// If use doc margins
		/// </summary>
		public bool UseDocMargins {
			get { return _useDocMargins; }
			set { _useDocMargins = value; }
		}
		
		
		private Alignment _imageAligment = Alignment.TopLeft;
		/// <summary>
		/// Image alignment
		/// </summary>
		public BackgroundImageDefinition.Alignment ImageAligment {
			get { return _imageAligment; }
			set { _imageAligment = value; }
		}
		
		private bool _scaleToFit = true;
		/// <summary>
		/// If scale image.
		/// </summary>
		public bool ScaleToFit {
			get { return _scaleToFit; }
			set { _scaleToFit = value; }
		}
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="src">Image source</param>
		public BackgroundImageDefinition( string src ):base(  )
		{
			if(src== null)throw new ArgumentNullException("Image Source");
			_source = src;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("[BackgroundImageDefinition Source={0}, UseDocMargins={1}, ImageAligment={2}, ScaleToFit={3}]", _source, _useDocMargins, _imageAligment, _scaleToFit);
		}

		
	}
	
	
	
}
