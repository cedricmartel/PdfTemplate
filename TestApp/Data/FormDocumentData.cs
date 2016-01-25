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
	/// FormDocumentData.
	/// </summary>
	public partial class FormDocumentData : Form
	{
		
		private Hashtable _data = null;
		
		internal FormDocumentData()
		{
			
			InitializeComponent();
			
		}		
		
		
		public FormDocumentData( Hashtable data)
		{
			
			InitializeComponent();
			_data = data;
			
			LoadData();
		}
		
		
		protected void LoadData(){
			this.txtData.Text = HashUtils.ToString( _data );
		}
		
		void BtnExitClick(object sender, EventArgs e)
		{
			this.Close();
		}
		
		void BtnDelClick(object sender, EventArgs e)
		{
			new FormDeleteValue( _data ).ShowDialog();
			LoadData();
		}
		
		void BtnAddClick(object sender, EventArgs e)
		{
			new FormAddValue( _data ).ShowDialog();
			LoadData();
		}
	}
}
