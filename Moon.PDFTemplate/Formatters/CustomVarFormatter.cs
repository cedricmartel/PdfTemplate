/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 04/06/2013
 * Time: 13:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using Moon.Utils;

namespace Moon.PDFTemplate.Formatters
{
	/// <summary>
	/// Custom Var Formatter. (formatter="custom") <para/>
	/// <para/><para/>Use 'classname' tag into parameters to define custom class (formatterparameters="classname=full_class_name"). Class must be IVarFormatter.<para/>
	/// <para/>When class create the format object, try  first create with one parameters string with FormatParameters. <para/>
	/// <para/>If crashes it try to create with anonymous constructor.
	/// </summary>
	public class CustomVarFormatter : BasicVarFormatter
	{
		/// <summary>
		/// Custom var formatter class name attr constant
		/// </summary>
		public const string ClassNameTag = "classname";
		
		private IVarFormatter _customFormatter = null;
		
		/// <summary>
		/// Gets custom formatter
		/// </summary>
		protected IVarFormatter CustomFormatter {
			get { return _customFormatter; }
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parameters"></param>
		public CustomVarFormatter( string parameters ):base( DefaultFormat.Custom, parameters )
		{
			LoadCustomClass();			
		}
		
		
		/// <summary>
		/// Load class defined into the string parameters.
		/// Use 'classname' tag to define class and try to pass string parameters to configure it. Class must be IVarFormatter.
		/// </summary>
		/// <returns></returns>
		protected virtual void LoadCustomClass(){
			
			IDictionary<string,string> parameters = GetFormatterParametersData();
			if(parameters == null)throw new ArgumentNullException("Class Name");
			
			if(!parameters.ContainsKey(ClassNameTag)){
				throw new ArgumentNullException("Class Name");
			}
			
			string className = parameters[ClassNameTag];
			Type tLoad = AssembliesTypesLoader.GetType( className );
			if(tLoad == null)throw new Exception("Class '"+className+"' not found!");
			
			object objLoad = null;
			try{
				objLoad = Activator.CreateInstance( tLoad, new object[]{ FormatterParameters } );//string constructor first
			}catch(Exception){
				//if crashes use anonymous constructor..
				try{
					objLoad = Activator.CreateInstance( tLoad, new object[]{} );
				}catch(Exception){	}
			}
			
			if(objLoad == null) throw new Exception("Object invocation failed!");
			
			if(!(objLoad is IVarFormatter))throw new InvalidCastException("Object isn't IVarFormatter");
			
			this._customFormatter = (IVarFormatter)objLoad;	
		}
		
		
		/// <summary>
		/// Gets formatted text. Uses custom formatter
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object GetFormatedValue(object value)
		{
			if(CustomFormatter == null)throw new NullReferenceException("Custom Formatter");
			return CustomFormatter.GetFormatedValue( value );
		}
		
		/// <summary>
		/// Gets default value. Uses custom formatter
		/// </summary>
		/// <returns></returns>
		public override object GetDefaultValue()
		{
			if(CustomFormatter == null)throw new NullReferenceException("Custom Formatter");
			return CustomFormatter.GetDefaultValue();
		}
	}
}
