/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 10/06/2013
 * Time: 11:10
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace TestApp.Data
{
	partial class FormAddValue
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
			this.txtName = new System.Windows.Forms.TextBox();
			this.txtValue = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.rdText = new System.Windows.Forms.RadioButton();
			this.rdNumber = new System.Windows.Forms.RadioButton();
			this.rdDateTime = new System.Windows.Forms.RadioButton();
			this.rdBool = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(66, 27);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(166, 20);
			this.txtName.TabIndex = 0;
			// 
			// txtValue
			// 
			this.txtValue.Location = new System.Drawing.Point(66, 62);
			this.txtValue.Name = "txtValue";
			this.txtValue.Size = new System.Drawing.Size(240, 20);
			this.txtValue.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Name";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 65);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(34, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Value";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rdBool);
			this.groupBox1.Controls.Add(this.rdDateTime);
			this.groupBox1.Controls.Add(this.rdNumber);
			this.groupBox1.Controls.Add(this.rdText);
			this.groupBox1.Location = new System.Drawing.Point(12, 102);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(366, 92);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Data type";
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(303, 208);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 5;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.BtnAddClick);
			// 
			// rdText
			// 
			this.rdText.Checked = true;
			this.rdText.Location = new System.Drawing.Point(18, 25);
			this.rdText.Name = "rdText";
			this.rdText.Size = new System.Drawing.Size(104, 24);
			this.rdText.TabIndex = 0;
			this.rdText.TabStop = true;
			this.rdText.Text = "Text";
			this.rdText.UseVisualStyleBackColor = true;
			// 
			// rdNumber
			// 
			this.rdNumber.Location = new System.Drawing.Point(18, 55);
			this.rdNumber.Name = "rdNumber";
			this.rdNumber.Size = new System.Drawing.Size(104, 24);
			this.rdNumber.TabIndex = 1;
			this.rdNumber.Text = "Number";
			this.rdNumber.UseVisualStyleBackColor = true;
			// 
			// rdDateTime
			// 
			this.rdDateTime.Location = new System.Drawing.Point(150, 55);
			this.rdDateTime.Name = "rdDateTime";
			this.rdDateTime.Size = new System.Drawing.Size(104, 24);
			this.rdDateTime.TabIndex = 2;
			this.rdDateTime.Text = "Date Time";
			this.rdDateTime.UseVisualStyleBackColor = true;
			// 
			// rdBool
			// 
			this.rdBool.Location = new System.Drawing.Point(150, 25);
			this.rdBool.Name = "rdBool";
			this.rdBool.Size = new System.Drawing.Size(104, 24);
			this.rdBool.TabIndex = 3;
			this.rdBool.Text = "Boolean";
			this.rdBool.UseVisualStyleBackColor = true;
			// 
			// FormAddValue
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(390, 243);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtValue);
			this.Controls.Add(this.txtName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAddValue";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormAddValue";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.RadioButton rdText;
		private System.Windows.Forms.RadioButton rdNumber;
		private System.Windows.Forms.RadioButton rdDateTime;
		private System.Windows.Forms.RadioButton rdBool;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtValue;
		private System.Windows.Forms.TextBox txtName;
	}
}
