/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 22/07/2009
 * Time: 10:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Reflection;
using System.Collections.Generic;


namespace Moon.Utils
{
	/// <summary>
	/// Load object instances in all assemblies loaded.
	/// Copied from EasyWork.Core.Utils 
	/// Carga instancias de objetos en todos los ensamblados cargados.
	/// </summary>
	public static class AssembliesTypesLoader
	{
		
		/// <summary>
		/// Intenta obtener el tipo del objeto indicado en los ensamblados correspondientes.
		/// </summary>
		/// <param name="className"></param>
		/// <returns></returns>
		public static Type GetType(string className){
			
			if(className == null){
				if(className == null)throw new ArgumentNullException("Class Name");				
			}
			
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			
			List<Type> types = new List<Type>();
			
			foreach (Assembly assembly in assemblies)
			{
				
				Type type = null;
				
				try{
					type = assembly.GetType(className);
				}catch(Exception){}
				
				if (type != null){
					return type;
				}				
			}

			Console.WriteLine("Class '"+className+"' not found!");
			
			return null;
		}
		
		/// <summary>
		/// Obtiene la información de un ensamblado dado su nombre.
		/// NOTA:Se cargan los ensamblados de la solución. Es posible que no se encuentre. Devuelve NULL.
		/// 
		/// @since 20121010
		/// </summary>
		/// <param name="assemblyName"></param>
		/// <returns>NULL si no se ha encontrado</returns>
		public static AssemblyName GetAssemblyInformation( string assemblyName ){
			try{
				if(assemblyName == null)throw new ArgumentNullException("Assembly Name");
				
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				foreach(Assembly act in assemblies ){
					if( (act.GetName().Name).Equals(assemblyName ) ){
					   	return act.GetName();
					   }
					}
				//no encontrado
				return null;
			}catch(Exception ex){
				Console.WriteLine(ex.ToString());
				return null;
			}
		}
		
		/// <summary>
		/// Obtiene la versión del ensamblado del tipo indicado.
		/// 
		/// @since 20121010
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static Version GetAssemblyVersion( Type type ){
			try{
				return type.Assembly.GetName().Version;
			}catch(Exception e){
				Console.WriteLine(e.ToString());
				return new Version(0,0,0);
			}
		}
		
	}
}
