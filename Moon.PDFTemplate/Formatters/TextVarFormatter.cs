/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 04/06/2013
 * Time: 12:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace Moon.PDFTemplate.Formatters
{
	/// <summary>
	/// TextFormateer. Do toString.<para/>
	/// <para/>Use 'defaultvalue' to define the default value ("" by default) before format.
	/// <para/>Use 'formatstring' to define string formatter (using Standard Format Strings). String.Format(...
	/// 
	/// </summary>
	public class TextVarFormatter : BasicVarFormatter
	{
		
		private const string FormatStringTag="formatstring";
		private const string DefaultValueTag="defaultvalue";
		
		private string _defaultValue = "";
		/// <summary>
		/// Default value
		/// </summary>
		protected string DefaultValue {
			get { return _defaultValue; }
		}
		
		private string _formatString = null;
		
		/// <summary>
		/// Get format string using Standard Format Strings
		/// </summary>
		protected  string FormatString {
			get { return _formatString; }
		}
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		public TextVarFormatter():base( DefaultFormat.Text, null )
		{
			
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parameters"></param>
		public TextVarFormatter(string parameters):base( DefaultFormat.Text, parameters )
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

			if( parameters.ContainsKey( DefaultValueTag ) ){
				_defaultValue = parameters[DefaultValueTag];
			}				
		}
		
		/// <summary>
		/// Get value formatted value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		protected virtual object Format( object value ){
			if(value == null)return null;
			
			if(FormatString == null)return value.ToString();
			else return String.Format( FormatString, value );
		}
		
		/// <summary>
		/// Gets text
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
		/// Gets default text
		/// </summary>
		/// <returns></returns>
		public override object GetDefaultValue()
		{
			return _defaultValue;
		}
	}
}
