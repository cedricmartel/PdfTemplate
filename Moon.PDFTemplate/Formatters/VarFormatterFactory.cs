/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 04/06/2013
 * Time: 12:52
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.CodeDom.Compiler;

namespace Moon.PDFTemplate.Formatters
{
	/// <summary>
	/// Formatter factory
	/// </summary>
	public static class VarFormatterFactory
	{
		/// <summary>
		/// Builds var formatter
		/// </summary>
		/// <param name="format"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static IVarFormatter Build( DefaultFormat format, string parameters ){
			
			switch(  format ){
				case DefaultFormat.Text:
					return new TextVarFormatter(parameters);
				case DefaultFormat.Boolean:
					return new BooleanVarFormatter(parameters);
				case DefaultFormat.Custom:
					return new CustomVarFormatter( parameters );
				case DefaultFormat.DateTime:
					return new DateTimeVarFormatter(parameters);
				case DefaultFormat.Number:
					return new NumberVarFormatter(parameters);
				default:
					return new TextVarFormatter(parameters);
			}
		}
		
		/// <summary>
		/// Builds var formatter
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		public static IVarFormatter Build( DefaultFormat format ){
			return Build( format, null);
		}
		
		
		
	}
}
