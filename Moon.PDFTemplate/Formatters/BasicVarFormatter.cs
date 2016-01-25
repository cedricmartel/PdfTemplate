/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 04/06/2013
 * Time: 12:28
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;

namespace Moon.PDFTemplate.Formatters
{
	
	/// <summary>
	/// Default var formats. 
/// <para/>Registered: Text, Number, Boolean, DateTime, Custom
	/// </summary>
	public enum DefaultFormat {
		/// <summary>
		/// 
		/// </summary>
		Text,
		/// <summary>
		/// 
		/// </summary>
		Number,
		/// <summary>
		/// 
		/// </summary>
		Boolean,
		/// <summary>
		/// 
		/// </summary>
		DateTime,
		/// <summary>
		/// 
		/// </summary>
		Custom
	};
	
	
	
	/// <summary>
	/// BasicFormatter. Abstract class.
	/// </summary>
	public abstract class BasicVarFormatter : IVarFormatter
	{
		private DefaultFormat _format = DefaultFormat.Text;
		
		/// <summary>
		/// Gets var format.
		/// </summary>
		protected DefaultFormat VarFormat {
			get { return _format; }
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="format"></param>
		protected BasicVarFormatter( DefaultFormat format)	{
			_format = format;
		}
		
		
		private string _formatterParameters = null;
		/// <summary>
		/// Gets aditional formatter parameters.
		/// </summary>
		protected string FormatterParameters {
			get { return _formatterParameters; }
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="format"></param>
		/// <param name="aditionalParameters"></param>
		protected BasicVarFormatter( DefaultFormat format, string aditionalParameters)	{
			_format = format;
			this._formatterParameters = aditionalParameters;
		}
		
		#region abstract

		/// <summary>
		/// Gets formatted value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public abstract object GetFormatedValue(object value);
		
		/// <summary>
		/// Gets default value.
		/// </summary>
		/// <returns></returns>
		public abstract object GetDefaultValue();
		
		#endregion
		
		
		/// <summary>
		/// Convert string parameters to hash.
		/// </summary>
		/// <returns></returns>
		protected IDictionary<string,string> GetFormatterParametersData(){
			
			if(FormatterParameters == null)return null;
			
			try{
				
				string[] parameters = FormatterParameters.Split(';');
				
				Dictionary<string,string> res= new Dictionary<string,string>();
				
				foreach( string parameter in parameters ){
					string[] values = parameter.Split('=');
					if(values.Length != 2){
						Console.WriteLine(parameter +" not valid");
						continue;
					}
					
					res.Add( values[0], values[1] );
				}
				
				return res;
				
			}catch(Exception ex){
				Console.WriteLine(ex.ToString() );
				return null;
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("[BasicVarFormatter Format={0}]", Enum.GetName( typeof(DefaultFormat), _format ) );
		}

		
		#region static
		
		/// <summary>
		/// Convert string defaultformat to enum value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static DefaultFormat? Parse( string value ){
			if(value == null)return null;
			try{
				return (DefaultFormat)Enum.Parse( typeof( DefaultFormat ), value, true );
			}catch(Exception ex){
				Console.WriteLine(ex.ToString());
				return null;
			}
		}
		#endregion
	}
}
