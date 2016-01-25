/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 04/06/2013
 * Time: 11:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Moon.PDFTemplate.XMAtributes
{
	/// <summary>
	/// Encapsules a VAR into a DrawElement.
	/// </summary>
	public class Variable
	{
		/// <summary>
		/// Configures default value.
		/// </summary>
		public static bool OptionalByDefault = false;
		
		
		
		/// <summary>
		/// Optional var tag. Values yes|no
		/// </summary>
		public const string OptionalAttributeConstant = "optional";
		
		/// <summary>
		/// Var name attribute
		/// </summary>
		public const string NameAttributeConstant = "name";
		
		
		
		
		#region Properties
		
		
		private string _name = null;
		/// <summary>
		/// Var name.
		/// </summary>
		public string Name {
			get { return _name; }
		}
		private bool _optional = OptionalByDefault;
		/// <summary>
		/// Gets if var its optional.
		/// </summary>
		public bool Optional {
			get { return _optional; }
		}
		
		#endregion
		
		
		#region Constructors
		
		/// <summary>
		/// Constructor with defaults.
		/// </summary>
		/// <param name="name"></param>
		public Variable(string name)
		{
			this._name = name;
			this._optional = OptionalByDefault;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name"></param>
		/// <param name="optional"></param>
		public Variable(string name, bool optional)
		{
			this._name = name;
			this._optional = optional;
		}
		
		#endregion
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("[Variable Name={0}, Optional={1}]", _name, _optional);
		}

	}
}
