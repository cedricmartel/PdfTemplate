/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 04/06/2013
 * Time: 13:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace Moon.PDFTemplate.Formatters
{
	/// <summary>
	/// DateTime Var Formatter.
	/// <para/><para/>Use 'formatstring' to define date time formatter (using Standard Date and Time Format Strings).
	/// <para/>Example - > formatstring="d" for short date formatter.
	/// <para/>Default value is null.
	/// </summary>
	public class DateTimeVarFormatter : BasicVarFormatter
	{
		
		private const string FormatStringTag="formatstring";
		
		private string _formatString = null;
		
		/// <summary>
		/// Get format string using Standard Date and Time Format Strings
		/// </summary>
		protected  string FormatString {
			get { return _formatString; }
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public DateTimeVarFormatter():base( DefaultFormat.DateTime, null )
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parameters"></param>
		public DateTimeVarFormatter(string parameters):base( DefaultFormat.DateTime, parameters )
		{
			LoadParameters();
		}
		
		/// <summary>
		/// Load formatter and other configurations
		/// </summary>
		/// <returns></returns>
		protected virtual void LoadParameters(){
			
			IDictionary<string,string> parameters = GetFormatterParametersData();				
			if(parameters == null)return;
			
			if( parameters.ContainsKey( FormatStringTag ) ){
				_formatString = parameters[FormatStringTag];
			}
			
		}
		
		/// <summary>
		/// Get value formatted value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		protected virtual object Format( object value ){
			if(value == null)return null;
			
			DateTime temp;
			if(value is DateTime){
				temp = (DateTime)value;
			}
			else{
				temp = DateTime.Parse( value.ToString() );
			}
			
			if(FormatString == null)return temp;
			else return temp.ToString( FormatString );
		}
		
		/// <summary>
		/// Gets date time formatted text
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object GetFormatedValue(object value)
		{								
			if(value == null)value = GetDefaultValue();
			
			try{
				
				return Format( value );
				
			}catch(Exception ex){
				Console.WriteLine(ex.ToString());
				return GetDefaultValue(); //return default value.
			}			
		}
		
		/// <summary>
		/// Gets default date time value.
		/// </summary>
		/// <returns></returns>
		public override object GetDefaultValue()
		{
			return null;			
		}
	}
}
