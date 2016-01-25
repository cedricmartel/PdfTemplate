/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 04/06/2013
 * Time: 12:34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Moon.PDFTemplate.Formatters;

namespace Moon.PDFTemplate.XMAtributes
{
	/// <summary>
	/// Formatable variable.
	/// </summary>
	public class FormatableVariable : Variable, IVarFormatter
	{
		/// <summary>
		/// Formatter var tag. Values text|number|boolean|datetime|custom
		/// </summary>
		public const string FormatterAttributeConstant = "formatter";
		
		/// <summary>
		/// Formatter parameters var tag. Free values. Some examples: formatstring, defaultvalue, true, false, classname, ... 
		/// </summary>
		public const string FormatterParametersAttributeConstant = "formatterparameters";
		
		
		
		private IVarFormatter _formatter = null;
		
		/// <summary>
		/// Var formatter
		/// </summary>
		protected IVarFormatter Formatter {
			get { return _formatter; }
		}
		
		/// <summary>
		/// Create a default TEXT variable.
		/// </summary>
		/// <param name="name"></param>
		public FormatableVariable( string name ):base( name )
		{
			_formatter = new TextVarFormatter();
		}
		
		
		/// <summary>
		/// Create custom formatable var.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="formatter"></param>
		public FormatableVariable( string name, IVarFormatter formatter):base( name )
		{
			_formatter = formatter;
		}
		
		
		/// <summary>
		/// Create custom formatable var.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="format"></param>
		/// <param name="formatParameters"></param>
		public FormatableVariable( string name, DefaultFormat format, string formatParameters):base( name )
		{			
			_formatter = GetFormatter( format, formatParameters );
		}
		
		
		/// <summary>
		/// Create a default TEXT variable.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="optional"></param>
		public FormatableVariable( string name , bool optional):base( name, optional )
		{
			_formatter = new TextVarFormatter();
		}
		
		
		/// <summary>
		/// Create custom formatable var.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="optional"></param>
		/// <param name="formatter"></param>
		public FormatableVariable( string name,  bool optional, IVarFormatter formatter):base( name,optional )
		{
			_formatter = formatter;
		}
		
		
		/// <summary>
		/// Create custom formatable var.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="optional"></param>
		/// <param name="format"></param>
		/// <param name="formatParameters"></param>
		public FormatableVariable( string name, bool optional, DefaultFormat format, string formatParameters):base( name, optional )
		{			
			_formatter = GetFormatter( format, formatParameters );
		}
		
		
		/// <summary>
		/// Get formatter by enum.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		protected IVarFormatter GetFormatter( DefaultFormat format, string parameters ){
			return VarFormatterFactory.Build(format, parameters);
		}
		
		
		
		#region IVarFormatter
		
		/// <summary>
		/// Gets formatted text value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public object GetFormatedValue(object value)
		{
			if(Formatter == null)return value;
			
			return Formatter.GetFormatedValue( value );
		}
		
		/// <summary>
		/// Gets default value
		/// </summary>
		/// <returns></returns>
		public object GetDefaultValue()
		{
			if(Formatter == null)return null;
			
			return Formatter.GetDefaultValue();
		}
		
		#endregion
	}
}
