/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 10/06/2013
 * Time: 11:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;

namespace TestApp.Data
{
	/// <summary>
	/// HashUtils.
	/// </summary>
	public static class HashUtils
	{
		
		static public string ToString( Hashtable data ){
			if(data == null)return null;
			string res = "";
			bool first = true;
			foreach( object key in data.Keys){
				if(first){
					first = false;
				}
				else{
					res += ", ";
				}
				res += key.ToString()+"="+data[key];
			}
			
			return res;
		}
	}
}
