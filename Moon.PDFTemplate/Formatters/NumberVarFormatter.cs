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
	/// Number Var Formatter.
	/// <para/>Use 'defaultvalue' to define the default value (null by default) before format. Use 'formatstring' to define number formatter (using Standard Numeric Format Strings).
	/// <para/>Example - > defaultvalue="";formatstring="C" for currency formatter and default empty
	/// 
	/// </summary>
	public class NumberVarFormatter : BasicVarFormatter
	{
		private const string DefaultValueTag="defaultvalue";
		private const string FormatStringTag="formatstring";
		
		private Double? _defaultValue = null;
		/// <summary>
		/// Default value
		/// </summary>
		protected Nullable<double> DefaultValue {
			get { return _defaultValue; }
		}
		
		
		private string _formatString = null;
		
		/// <summary>
		/// Get format string using Standard Numeric Format Strings
		/// </summary>
		protected  string FormatString {
			get { return _formatString; }
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public NumberVarFormatter():base( DefaultFormat.Number, null )
		{			
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parameters"></param>
		public NumberVarFormatter(string parameters):base( DefaultFormat.Number, parameters )
		{
			LoadParameters();
		}
		
		/// <summary>
		/// Load formatter and other configurations
		/// </summary>
		/// <returns></returns>
		protected virtual void LoadParameters(){
			
			//load string parameters.
			IDictionary<string,string> parameters = GetFormatterParametersData();				
			if(parameters == null)return;
			
			if( parameters.ContainsKey( DefaultValueTag ) ){
				_defaultValue = double.Parse(parameters[DefaultValueTag]);
			}
			
			if ( parameters.ContainsKey( FormatStringTag ) ){
				_formatString =  parameters[FormatStringTag];
			}
		}
		
		/// <summary>
		/// Get value formatted value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		protected virtual object Format( object value ){
			if(value == null)return null;
			
			double temp;
			if(value is Double){
				temp = (Double)value;
			}
			else{
				temp = Double.Parse( value.ToString() );
			}
			
			if(FormatString == null)return temp;
			else return temp.ToString( FormatString );
		}
		
		/// <summary>
		/// Gets number formatted text
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object GetFormatedValue(object value)
		{
			if(value == null){
				value = GetDefaultValue();
			}
						
			try{				
				return Format( value );
				
			}catch(Exception ex){
				Console.WriteLine(ex.ToString());
				return GetDefaultValue(); //return default value.
			}			
		}
		
		/// <summary>
		/// Gets default value
		/// </summary>
		/// <returns></returns>
		public override object GetDefaultValue()
		{
			if(DefaultValue == null)return null;
			else return DefaultValue.Value;
		}
	}
}
