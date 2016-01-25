/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 04/06/2013
 * Time: 12:57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace Moon.PDFTemplate.Formatters
{
	/// <summary>
	/// BooleanVarFormatter.
	/// <para/>For modify behaviour: Parameters: true, false - > Text for true or false: example - >  true=Yes;false=No
	/// </summary>
	public class BooleanVarFormatter : BasicVarFormatter
	{
				
		private const string DefaultValueTag="defaultvalue";
		
		/// <summary>
		/// Boolean var formatter text for true parameter attr constant
		/// </summary>
		public const string TextForTtrueParameter = "true";
		
		/// <summary>
		/// Boolean var formatter text for false parameter attr constant
		/// </summary>
		public const string TextForFalseParameter = "false";
		
		private string _textTrue = "True";
		
		/// <summary>
		/// Text for true value
		/// </summary>
		protected string TextTrue {
			get { return _textTrue; }
		}
		private string _textFalse = "False";
		
		
		/// <summary>
		/// Text for false value
		/// </summary>
		protected string TextFalse {
			get { return _textFalse; }
			set { _textFalse = value; }
		}
				
		private bool? _defaultValue = null;
		/// <summary>
		/// Default value
		/// </summary>
		protected Nullable<bool> DefaultValue {
			get { return _defaultValue; }
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public BooleanVarFormatter():base( DefaultFormat.Boolean, null )
		{
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameters">Text for true or false: example - >  true=Yes;false=No</param>
		public BooleanVarFormatter(string parameters):base( DefaultFormat.Boolean, parameters )
		{
			LoadParameters();
		}
		
		/// <summary>
		/// Loader
		/// </summary>
		protected virtual void LoadParameters(){
			
			IDictionary<string,string> parameters = GetFormatterParametersData();				
			if(parameters == null)return;
			
			if( parameters.ContainsKey( TextForTtrueParameter ) ){
				_textTrue = parameters[TextForTtrueParameter];
			}
			
			if ( parameters.ContainsKey( TextForFalseParameter ) ){
				_textFalse =  parameters[TextForFalseParameter];
			}
			
			if ( parameters.ContainsKey( DefaultValueTag ) ){
				_defaultValue =  Boolean.Parse(parameters[DefaultValueTag]);
			}
		}
		
		/// <summary>
		/// Gets bool formatted value.
		/// <para/>20141203 :: ADD string parse code.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object GetFormatedValue(object value)
		{
			if(value == null){
				value = GetDefaultValue();				
			}
			
			if(value is Boolean){
				return ((Boolean)value)?TextTrue: TextFalse;
			}
			else if(value is string){
				
				//empty
				if(string.IsNullOrEmpty((string)value)) return GetDefaultValue();
								
				if( ((string)value).Equals("yes", StringComparison.InvariantCultureIgnoreCase) ||
				   ((string)value).Equals("true", StringComparison.InvariantCultureIgnoreCase) ||
				   ((string)value).Equals("1", StringComparison.InvariantCultureIgnoreCase)
				  ){
					return true;
				}
				else if( ((string)value).Equals("no", StringComparison.InvariantCultureIgnoreCase) ||
				        ((string)value).Equals("false", StringComparison.InvariantCultureIgnoreCase) ||
				        ((string)value).Equals("0", StringComparison.InvariantCultureIgnoreCase)
				       ){
					return false;
				}
				
			}
			
			
			try{
				bool parsed = Boolean.Parse( value.ToString() );
				return parsed?TextTrue: TextFalse;
			}catch(Exception ex){
				Console.WriteLine("Parsing error 'GetFormatedValue' ["+value+"] -> "+ ex.ToString());
				return GetDefaultValue();
			}
						
		}
		
		/// <summary>
		/// Gets default boolean value.
		/// </summary>
		/// <returns></returns>
		public override object GetDefaultValue()
		{
			return DefaultValue;
		}
	}
}
