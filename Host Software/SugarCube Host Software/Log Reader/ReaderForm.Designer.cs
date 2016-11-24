/* SugarCube Host Software - Log Reader
 * Copyright (c) 2014-2015 Chad Ullman
 */
namespace Me.ThreeDWares.SugarCube
{
	partial class ReaderForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReaderForm));
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.btnOpen = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tslModule = new System.Windows.Forms.ToolStripLabel();
			this.cbModuleChooser = new System.Windows.Forms.ToolStripComboBox();
			this.tslLogLevel = new System.Windows.Forms.ToolStripLabel();
			this.cbLogLevel = new System.Windows.Forms.ToolStripComboBox();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.gridView = new System.Windows.Forms.DataGridView();
			this.toolStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
			this.SuspendLayout();
			// 
			// toolStrip
			// 
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.btnOpen,
									this.toolStripSeparator1,
									this.tslModule,
									this.cbModuleChooser,
									this.tslLogLevel,
									this.cbLogLevel});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(728, 25);
			this.toolStrip.TabIndex = 0;
			this.toolStrip.Text = "toolStrip";
			// 
			// btnOpen
			// 
			this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(40, 22);
			this.btnOpen.Text = "Open";
			this.btnOpen.Click += new System.EventHandler(this.BtnOpenClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// tslModule
			// 
			this.tslModule.Name = "tslModule";
			this.tslModule.Size = new System.Drawing.Size(51, 22);
			this.tslModule.Text = "Module:";
			// 
			// cbModuleChooser
			// 
			this.cbModuleChooser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbModuleChooser.Items.AddRange(new object[] {
									"All"});
			this.cbModuleChooser.Name = "cbModuleChooser";
			this.cbModuleChooser.Size = new System.Drawing.Size(121, 25);
			this.cbModuleChooser.SelectedIndexChanged += new System.EventHandler(this.FilterChanged);
			// 
			// tslLogLevel
			// 
			this.tslLogLevel.Name = "tslLogLevel";
			this.tslLogLevel.Size = new System.Drawing.Size(60, 22);
			this.tslLogLevel.Text = "Log Level:";
			// 
			// cbLogLevel
			// 
			this.cbLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbLogLevel.Items.AddRange(new object[] {
									"All",
									"DEBUG",
									"INFO",
									"ERROR",
									"FATAL"});
			this.cbLogLevel.Name = "cbLogLevel";
			this.cbLogLevel.Size = new System.Drawing.Size(121, 25);
			this.cbLogLevel.SelectedIndexChanged += new System.EventHandler(this.FilterChanged);
			// 
			// openFileDialog
			// 
			this.openFileDialog.FileName = "openFileDialog1";
			// 
			// gridView
			// 
			this.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridView.Location = new System.Drawing.Point(0, 25);
			this.gridView.Name = "gridView";
			this.gridView.Size = new System.Drawing.Size(728, 529);
			this.gridView.TabIndex = 2;
			this.gridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.GridViewCellFormatting);
			// 
			// ReaderForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(728, 554);
			this.Controls.Add(this.gridView);
			this.Controls.Add(this.toolStrip);
			this.Name = "ReaderForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Log Reader";
			this.Load += new System.EventHandler(this.ReaderFormLoad);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.DataGridView gridView;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.ToolStripComboBox cbLogLevel;
		private System.Windows.Forms.ToolStripLabel tslLogLevel;
		private System.Windows.Forms.ToolStripComboBox cbModuleChooser;
		private System.Windows.Forms.ToolStripLabel tslModule;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btnOpen;
		private System.Windows.Forms.ToolStrip toolStrip;
	}
}
