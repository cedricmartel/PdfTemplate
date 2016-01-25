/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 10/06/2013
 * Time: 11:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Drawing;
using System.Security.Policy;
using System.Windows.Forms;

namespace TestApp.Data
{
	/// <summary>
	/// DeleteValue.
	/// </summary>
	public partial class FormDeleteValue : Form
	{
		
		private Hashtable _data = null;
		internal FormDeleteValue()
		{
			
			InitializeComponent();
			
		}
		
		
		public FormDeleteValue( Hashtable data)
		{
			
			InitializeComponent();
			_data = data;
			
			LoadCombo();			
		}
		
		
		private void LoadCombo(){
			//load combo
			if(_data != null){
				cmbValues.Items.Clear();				
				foreach( object key in _data.Keys ){
					cmbValues.Items.Add( key );
				}
			}
		}
		
		void BtnDeleteClick(object sender, EventArgs e)
		{
			object tag = cmbValues.SelectedItem;
			if(tag != null){
				_data.Remove( tag );
				LoadCombo();
			}			
		}
	}
}
