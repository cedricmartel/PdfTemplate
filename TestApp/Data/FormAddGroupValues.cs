/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 10/06/2013
 * Time: 11:11
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TestApp.Data
{
	/// <summary>
	/// FormAddGroupValues.
	/// </summary>
	public partial class FormAddGroupValues : Form
	{
		List<Hashtable> _data = null;
		internal FormAddGroupValues()
		{			
			InitializeComponent();	
		}
		
		
		public FormAddGroupValues(List<Hashtable> data)
		{			
			InitializeComponent();	
			
			this._data = data;
			if(data == null)throw new ArgumentNullException("Data");
				
			LoadData();
		}
		
		
		protected void LoadData(){
			lstLines.Items.Clear();
			
			foreach( Hashtable act in _data){
				lstLines.Items.Add( new HashViewer( act ) );
			}
			
			lstLines.ClearSelected();
		}
		
		
		void BtnAddClick(object sender, EventArgs e)
		{
			Hashtable line = new Hashtable();
			FormAddValue addV = new FormAddValue( line );
			addV.ShowDialog();
			_data.Add( line );
			
			LoadData();
		}
		
		void BtnDelClick(object sender, EventArgs e)
		{
			HashViewer view = (HashViewer)lstLines.SelectedItem;
			if(view != null){
				_data.Remove( view.Data );
				LoadData();
			}			
		}
		
		void BtnCloneClick(object sender, EventArgs e)
		{
			HashViewer view = (HashViewer)lstLines.SelectedItem;
			if(view != null){
				Hashtable clone = (Hashtable)view.Data.Clone();
				_data.Add( clone );
				LoadData();
				lstLines.SelectedItem = view;
			}	
		}
		
		void BbtnExitClick(object sender, EventArgs e)
		{
			this.Close();
		}
		
				
		
		void LstLinesDoubleClick(object sender, EventArgs e)
		{
			//edition...
			HashViewer view = (HashViewer)lstLines.SelectedItem;
			if(view != null){
				_data.Remove( view.Data );
				
				FormDeleteValue fDel = new FormDeleteValue( view.Data );
				fDel.ShowDialog();
				
				_data.Add( view.Data );
				
				LoadData();
			}	
		}
		
		
		
		/// <summary>
		/// View Hash class.
		/// </summary>
		protected class HashViewer{
			
			private Hashtable _data = null;
			
			public Hashtable Data {
				get { return _data; }
			}
			
			public HashViewer( Hashtable data ){
				this._data = data;
			}
			
			public override string ToString()
			{
				return HashUtils.ToString( _data );
			}
			
		}
	}
}
