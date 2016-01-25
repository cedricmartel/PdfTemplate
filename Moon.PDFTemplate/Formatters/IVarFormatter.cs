/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 04/06/2013
 * Time: 12:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Moon.PDFTemplate.Formatters
{	
	
	
	/// <summary>
	/// Defines formatter methods.
	/// </summary>
	public interface IVarFormatter
	{
		/// <summary>
		/// Gets formatted text value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		System.Object GetFormatedValue( System.Object value );
		
		
		/// <summary>
		/// Gets default value.
		/// </summary>
		/// <returns></returns>
		System.Object GetDefaultValue();
		
	}
}
