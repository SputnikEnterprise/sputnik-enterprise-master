namespace DevExpress.Metro.Navigation {
    partial class _Layout {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_Layout));
      this.header = new DevExpress.XtraEditors.PanelControl();
      this.title = new DevExpress.XtraEditors.LabelControl();
      this.back = new System.Windows.Forms.PictureBox();
      this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
      ((System.ComponentModel.ISupportInitialize)(this.header)).BeginInit();
      this.header.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.back)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
      this.SuspendLayout();
      // 
      // header
      // 
      this.header.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
      this.header.Controls.Add(this.title);
      this.header.Controls.Add(this.back);
      this.header.Dock = System.Windows.Forms.DockStyle.Top;
      this.header.Location = new System.Drawing.Point(10, 0);
      this.header.Name = "header";
      this.header.Size = new System.Drawing.Size(872, 60);
      this.header.TabIndex = 1;
      // 
      // title
      // 
      this.title.AllowHtmlString = true;
      this.title.Appearance.Font = new System.Drawing.Font("Segoe UI", 24F);
      this.title.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
      this.title.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
      this.title.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
      this.title.Location = new System.Drawing.Point(70, 8);
      this.title.Name = "title";
      this.title.Size = new System.Drawing.Size(758, 45);
      this.title.TabIndex = 1;
      this.title.Text = "[]";
      // 
      // back
      // 
      this.back.Image = ((System.Drawing.Image)(resources.GetObject("back.Image")));
      this.back.Location = new System.Drawing.Point(0, 0);
      this.back.Name = "back";
      this.back.Size = new System.Drawing.Size(64, 57);
      this.back.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
      this.back.TabIndex = 0;
      this.back.TabStop = false;
      // 
      // panelControl1
      // 
      this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelControl1.Location = new System.Drawing.Point(10, 60);
      this.panelControl1.Name = "panelControl1";
      this.panelControl1.Size = new System.Drawing.Size(872, 466);
      this.panelControl1.TabIndex = 2;
      // 
      // _Layout
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.panelControl1);
      this.Controls.Add(this.header);
      this.LookAndFeel.SkinName = "Office 2010 Black";
      this.LookAndFeel.UseDefaultLookAndFeel = true;
      this.LookAndFeel.UseWindowsXPTheme = false;
      this.Name = "_Layout";
      this.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
      this.Size = new System.Drawing.Size(892, 536);
      this.Load += new System.EventHandler(this._Layout_Load);
      ((System.ComponentModel.ISupportInitialize)(this.header)).EndInit();
      this.header.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.back)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
      this.ResumeLayout(false);

        }

        #endregion

        private XtraEditors.PanelControl header;
        private XtraEditors.LabelControl title;
        private System.Windows.Forms.PictureBox back;
        private XtraEditors.PanelControl panelControl1;
    }
}
