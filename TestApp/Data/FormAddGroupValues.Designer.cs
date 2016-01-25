/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 10/06/2013
 * Time: 11:11
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace TestApp.Data
{
	partial class FormAddGroupValues
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.lstLines = new System.Windows.Forms.ListBox();
			this.bbtnExit = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnDel = new System.Windows.Forms.Button();
			this.btnClone = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lstLines
			// 
			this.lstLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.lstLines.FormattingEnabled = true;
			this.lstLines.Location = new System.Drawing.Point(12, 12);
			this.lstLines.Name = "lstLines";
			this.lstLines.Size = new System.Drawing.Size(507, 199);
			this.lstLines.TabIndex = 0;
			this.lstLines.DoubleClick += new System.EventHandler(this.LstLinesDoubleClick);
			// 
			// bbtnExit
			// 
			this.bbtnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bbtnExit.Location = new System.Drawing.Point(444, 227);
			this.bbtnExit.Name = "bbtnExit";
			this.bbtnExit.Size = new System.Drawing.Size(75, 23);
			this.bbtnExit.TabIndex = 1;
			this.bbtnExit.Text = "Exit";
			this.bbtnExit.UseVisualStyleBackColor = true;
			this.bbtnExit.Click += new System.EventHandler(this.BbtnExitClick);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd.Location = new System.Drawing.Point(12, 227);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 2;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.BtnAddClick);
			// 
			// btnDel
			// 
			this.btnDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDel.Location = new System.Drawing.Point(93, 227);
			this.btnDel.Name = "btnDel";
			this.btnDel.Size = new System.Drawing.Size(75, 23);
			this.btnDel.TabIndex = 3;
			this.btnDel.Text = "Delete";
			this.btnDel.UseVisualStyleBackColor = true;
			this.btnDel.Click += new System.EventHandler(this.BtnDelClick);
			// 
			// btnClone
			// 
			this.btnClone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClone.Location = new System.Drawing.Point(174, 227);
			this.btnClone.Name = "btnClone";
			this.btnClone.Size = new System.Drawing.Size(75, 23);
			this.btnClone.TabIndex = 4;
			this.btnClone.Text = "Clone";
			this.btnClone.UseVisualStyleBackColor = true;
			this.btnClone.Click += new System.EventHandler(this.BtnCloneClick);
			// 
			// FormAddGroupValues
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(531, 262);
			this.Controls.Add(this.btnClone);
			this.Controls.Add(this.btnDel);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.bbtnExit);
			this.Controls.Add(this.lstLines);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAddGroupValues";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormAddGroupValues";
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button btnClone;
		private System.Windows.Forms.Button btnDel;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button bbtnExit;
		private System.Windows.Forms.ListBox lstLines;
	}
}
