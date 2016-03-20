using System;
using System.Collections;
using System.Xml;

namespace Moon.PDFDraw
{
	/// <summary>
	/// XML data helper.
	/// <para/>20141203 :: Secure code.
	/// </summary>
	public class Helper
	{
		/// <summary>
		/// Gets int value from XML attribute
		/// </summary>
		/// <param name="name"></param>
		/// <param name="attrs"></param>
		/// <param name="value_default"></param>
		/// <returns></returns>
		public static int GetIntAttributeValue(string name, XmlAttributeCollection attrs, int value_default)
		{
			int r = value_default;
			try{
				XmlNode attr = attrs.GetNamedItem(name);
				if (attr != null)
				{
					r = Convert.ToInt32(attr.Value);
				}
			}catch(Exception ex){
				Console.WriteLine("Error 'GetIntAttributeValue' ->  ["+name+"]"+ex.Message );
			}
			return r;
		}
		
		/// <summary>
		/// Gets float value from XML attribute
		/// </summary>
		/// <param name="name"></param>
		/// <param name="attrs"></param>
		/// <param name="value_default"></param>
		/// <returns></returns>
		public static float GetFloatAttributeValue(string name, XmlAttributeCollection attrs, float value_default)
		{
			float r = value_default;
			try{
				XmlNode attr = attrs.GetNamedItem(name);
				if (attr != null)
				{
					r = (float)Convert.ToDouble(attr.Value);
				}
			}catch(Exception ex){
				Console.WriteLine("Error 'GetFloatAttributeValue' ["+name+"]-> "+ex.Message );
			}
			return r;
		}
		
		/// <summary>
		/// Gets string value from XML attribute
		/// </summary>
		/// <param name="name"></param>
		/// <param name="attrs"></param>
		/// <param name="value_default"></param>
		/// <returns></returns>
		public static string GetAttributeValue(string name, XmlAttributeCollection attrs, string value_default)
		{
			string r = value_default;
			try{
				XmlNode attr = attrs.GetNamedItem(name);
				if (attr != null)
				{
					r = attr.Value;
					//Console.WriteLine(name + ": " + r);
				}
			}catch(Exception ex){
				Console.WriteLine("Error 'GetAttributeValue'  ["+name+"]-> "+ex.Message );
			}
			return r;
		}


        /// <summary>
        /// Gets Color value from XML attribute
        /// </summary>
	    public static System.Drawing.Color GetAttributeColor(string name, XmlAttributeCollection attrs, string defaultValue)
        {
            return GetAttributeColor(name, attrs, defaultValue, null);
            ;
	    }

		/// <summary>
		/// Gets Color value from XML attribute, using vars 
		/// </summary>
        public static System.Drawing.Color GetAttributeColor(string name, XmlAttributeCollection attrs, string defaultValue, IDictionary data)
		{
            if (string.IsNullOrEmpty(defaultValue))
                defaultValue = "Black";

		    var val = GetAttributeValue(name, attrs, defaultValue);
		    if (data != null)
		    {
		        var dataVal = (string)data[val];
		        if (!string.IsNullOrEmpty(dataVal))
		            val = dataVal;
		        else if (val.Contains("{"))
		            val = defaultValue;
		    }

            return System.Drawing.ColorTranslator.FromHtml(val);
		}
		
		/// <summary>
		/// Gets width percent value from XML attribute
		/// </summary>
		/// <param name="attrs"></param>
		/// <returns></returns>
		public static float GetAttributeWidthPercent(XmlAttributeCollection attrs)
		{
			float r = 0;
			try{
				string widthPercent = GetAttributeValue("width", attrs, "0%");
				if (widthPercent.Contains("%"))
				{
					string _txt = widthPercent.Substring(0, widthPercent.IndexOf("%"));
					r = (float)Convert.ToDouble(_txt);
					r /= 100;
				}
			}catch(Exception ex){
				Console.WriteLine("Error 'GetAttributeWidthPercent'  [width]-> "+ex.Message );
			}
			return r;
		}
		
		/// <summary>
		/// Gets float[] value from XML attribute
		/// </summary>
		/// <param name="name"></param>
		/// <param name="attrs"></param>
		/// <returns></returns>
		public static float[] GetFloatArray(string name, XmlAttributeCollection attrs)
		{

			string[] textArray = GetStringArray(name, attrs);
			System.Collections.ArrayList alist = new System.Collections.ArrayList();
			foreach (string t in textArray)
			{
				alist.Add((float)Convert.ToDouble(t));
			}
			return (float[]) alist.ToArray(typeof(float));
		}
		
		/// <summary>
		/// Gets string[] value from XML attribute
		/// </summary>
		/// <param name="name"></param>
		/// <param name="attrs"></param>
		/// <returns></returns>
		public static string[] GetStringArray(string name, XmlAttributeCollection attrs)
		{
			string text = GetAttributeValue(name, attrs, "");
			string[] r = text.Split(new char[] { ',' });
			for (int i = 0; i < r.Length; i++)
			{
				r[i] = r[i].Trim();
			}
			return r;
		}
		
		
		/// <summary>
		/// Gets boolean value.
		/// <para/>20141203 :: Add string parse code (true|false 1|0).
		/// </summary>
		/// <param name="name"></param>
		/// <param name="attrs"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static bool GetAttributeBoolean(string name, XmlAttributeCollection attrs, bool defaultValue)
		{
			object vObject = GetAttributeValue( name, attrs, null );
			
			if(vObject == null){
				return defaultValue;
			}
			
			try{
				if(vObject is string){
					if( ((string)vObject).Equals("yes", StringComparison.InvariantCultureIgnoreCase) ||
					   ((string)vObject).Equals("true", StringComparison.InvariantCultureIgnoreCase) ||
					   ((string)vObject).Equals("1", StringComparison.InvariantCultureIgnoreCase)
					  ){
						return true;
					}
					else if( ((string)vObject).Equals("no", StringComparison.InvariantCultureIgnoreCase) ||
					        ((string)vObject).Equals("false", StringComparison.InvariantCultureIgnoreCase) ||
					        ((string)vObject).Equals("0", StringComparison.InvariantCultureIgnoreCase)
					       ){
						return false;
					}
					else{
						return defaultValue;
					}
				}
				else{
					return Boolean.Parse( vObject.ToString() );
				}
			}catch(Exception ex){
				Console.WriteLine("Error 'GetAttributeBoolean'  ["+name+"]-> "+ex.Message );
				return defaultValue;
			}
			
		}
	}
}
