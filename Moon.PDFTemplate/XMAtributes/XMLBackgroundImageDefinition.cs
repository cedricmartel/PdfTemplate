/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 07/06/2013
 * Time: 13:20
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Xml;
using Moon.PDFDraw;

namespace Moon.PDFTemplate.XMAtributes
{
	/// <summary>
	/// BackgroundImageDefinition with XML definition. <para/>
	/// Parameters:<para/>
	/// backgroundimagealigment=Center|TopLeft|TopCenter|TopRight<para/>
	/// backgroundimagemargins=yes|no<para/>
	/// backgroundimagescaletofit=yes|no<para/>
	/// </summary>
	public class XMLBackgroundImageDefinition : BackgroundImageDefinition
	{
		/// <summary>
		/// Background image alignment attr constant
		/// </summary>
		public const string AlignmentAttributeConstant = "backgroundimagealignment";
		
		
		/// <summary>
		/// Background margin  attr constant
		/// </summary>
		public const string MarginsAttributeConstant = "backgroundimagemargins";
		
		/// <summary>
		/// Background scale to fit attr constant
		/// </summary>
		public const string ScaleToFitAttributeConstant = "backgroundimagescaletofit";
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="source"></param>
		/// <param name="attrs"></param>
		public XMLBackgroundImageDefinition( string source, XmlAttributeCollection attrs):base(source)
		{			
			//process attrs...
			if(attrs != null){
				
				this.ScaleToFit = XmlHelper.GetAttributeBoolean( ScaleToFitAttributeConstant, attrs, false );
				this.UseDocMargins = XmlHelper.GetAttributeBoolean( MarginsAttributeConstant, attrs, false );
								
				string alignment = XmlHelper.GetAttributeValue( AlignmentAttributeConstant, attrs, null );
				if(alignment != null){
					try{
						object oParse = Enum.Parse( typeof( BackgroundImageDefinition.Alignment ), alignment, true );
						if(oParse != null){
							base.ImageAligment = (BackgroundImageDefinition.Alignment)oParse;
						}
					}catch(Exception ex){
						Console.WriteLine(ex.ToString());
					}
				}
				
			}			
		}
	}
}
