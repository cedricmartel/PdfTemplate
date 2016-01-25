/*
 * Created by SharpDevelop.
 * User: jaime.lopez
 * Date: 10/06/2013
 * Time: 9:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace TestApp
{
	partial class MainForm
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
			this.txtViewer = new System.Windows.Forms.TextBox();
			this.txtTemplate = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnPrint = new System.Windows.Forms.Button();
			this.btnDataDoc = new System.Windows.Forms.Button();
			this.btnDataLoop = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lstConsole = new System.Windows.Forms.ListBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtViewer
			// 
			this.txtViewer.Location = new System.Drawing.Point(24, 94);
			this.txtViewer.Name = "txtViewer";
			this.txtViewer.Size = new System.Drawing.Size(323, 20);
			this.txtViewer.TabIndex = 2;
			// 
			// txtTemplate
			// 
			this.txtTemplate.Location = new System.Drawing.Point(24, 40);
			this.txtTemplate.Name = "txtTemplate";
			this.txtTemplate.Size = new System.Drawing.Size(323, 20);
			this.txtTemplate.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(26, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 19);
			this.label1.TabIndex = 2;
			this.label1.Text = "Template";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 78);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "PDFViewer";
			// 
			// btnPrint
			// 
			this.btnPrint.Location = new System.Drawing.Point(272, 147);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(75, 23);
			this.btnPrint.TabIndex = 0;
			this.btnPrint.Text = "Print";
			this.btnPrint.UseVisualStyleBackColor = true;
			this.btnPrint.Click += new System.EventHandler(this.Button1Click);
			// 
			// btnDataDoc
			// 
			this.btnDataDoc.Location = new System.Drawing.Point(24, 16);
			this.btnDataDoc.Name = "btnDataDoc";
			this.btnDataDoc.Size = new System.Drawing.Size(75, 23);
			this.btnDataDoc.TabIndex = 3;
			this.btnDataDoc.Text = "Document";
			this.btnDataDoc.UseVisualStyleBackColor = true;
			this.btnDataDoc.Click += new System.EventHandler(this.BtnDataDocClick);
			// 
			// btnDataLoop
			// 
			this.btnDataLoop.Location = new System.Drawing.Point(105, 16);
			this.btnDataLoop.Name = "btnDataLoop";
			this.btnDataLoop.Size = new System.Drawing.Size(75, 23);
			this.btnDataLoop.TabIndex = 4;
			this.btnDataLoop.Text = "Loop";
			this.btnDataLoop.UseVisualStyleBackColor = true;
			this.btnDataLoop.Click += new System.EventHandler(this.BtnDataLoopClick);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnDataDoc);
			this.groupBox1.Controls.Add(this.btnDataLoop);
			this.groupBox1.Location = new System.Drawing.Point(24, 131);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(205, 48);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Data";
			// 
			// lstConsole
			// 
			this.lstConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.lstConsole.FormattingEnabled = true;
			this.lstConsole.HorizontalScrollbar = true;
			this.lstConsole.Location = new System.Drawing.Point(366, 12);
			this.lstConsole.Name = "lstConsole";
			this.lstConsole.ScrollAlwaysVisible = true;
			this.lstConsole.Size = new System.Drawing.Size(476, 173);
			this.lstConsole.TabIndex = 6;
			this.lstConsole.DoubleClick += new System.EventHandler(this.LstConsoleDoubleClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(854, 191);
			this.Controls.Add(this.lstConsole);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnPrint);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtTemplate);
			this.Controls.Add(this.txtViewer);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "TestApp";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ListBox lstConsole;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnDataLoop;
		private System.Windows.Forms.Button btnDataDoc;
		private System.Windows.Forms.Button btnPrint;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtTemplate;
		private System.Windows.Forms.TextBox txtViewer;
	}
}
