/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 10/06/2013
 * Time: 11:10
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace TestApp.Data
{
	/// <summary>
	/// FormAddValue.
	/// </summary>
	public partial class FormAddValue : Form
	{
		public enum DataType {Text, Number, DateTime,Boolean};
		
		private Hashtable _data = null;
		
		internal FormAddValue()
		{
			
			InitializeComponent();
						
			
		}
		
		public FormAddValue( Hashtable data)
		{
			
			InitializeComponent();
			_data = data;
			if(_data == null)throw new ArgumentNullException("Data");
		}
		
		
		void BtnAddClick(object sender, EventArgs e)
		{
			
			try{
				
				Check( "Name", this.txtName.Text );
				Check( "Text", this.txtValue.Text );
				
				string name = this.txtName.Text;
				string strValue = this.txtValue.Text;
				
				
				object value = null;
				DataType type = GetDataType();
				switch( type ){
					case DataType.Text:
						value = strValue;						
						break;
						
					case DataType.Number:
						value = Double.Parse(strValue);
						break;
					case DataType.DateTime:
						value = DateTime.Parse(strValue);
						break;
						
					case DataType.Boolean:
						value = Boolean.Parse(strValue);
						break;						
				}
				
				
				if(value != null){
					_data.Add( name, value );					
					ClearFields();
				}
				
			}catch(Exception ex){
				MessageBox.Show(ex.ToString());
			}
			
		}
		
		protected void ClearFields(){
			this.txtName.Text ="";
			this.txtValue.Text = "";
			rdText.Checked = true;
		}
		
		protected void Check( string tag, string data ){
			if(data == null || data.Trim().Length == 0)throw new Exception(""+tag+" empty!");
		}
		
		
		protected DataType GetDataType(){
			
			if(rdText.Checked){
				return DataType.Text;
			}
			else if(rdBool.Checked){
				return DataType.Boolean;
			}
			else if(rdDateTime.Checked){
				return DataType.DateTime;
			}
			else if(rdNumber.Checked){
				return DataType.Number;
			}
			
			throw new Exception("Data Type not selected!");
		}
		
	}
}
