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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TestApp.Data
{
	/// <summary>
	///FormLoopData.
	/// </summary>
	public partial class FormLoopData : FormAddGroupValues
	{
		
		internal FormLoopData()
		{			
			InitializeComponent();			
		}
		
		
		public FormLoopData( List<Hashtable> data):base( data )
		{			
			InitializeComponent();			
		}
	}
}
