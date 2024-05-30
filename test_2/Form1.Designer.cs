namespace test_2
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dataCheckBtn = new DevExpress.XtraEditors.SimpleButton();
            this.addDataBtn = new DevExpress.XtraEditors.SimpleButton();
            this.logOutBtn = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelControl1.Appearance.Font = new System.Drawing.Font("新細明體", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(271, 101);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(490, 48);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "歡迎進入資料儲存系統";
            // 
            // dataCheckBtn
            // 
            this.dataCheckBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dataCheckBtn.Appearance.Font = new System.Drawing.Font("新細明體", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataCheckBtn.Appearance.Options.UseFont = true;
            this.dataCheckBtn.Location = new System.Drawing.Point(216, 218);
            this.dataCheckBtn.Name = "dataCheckBtn";
            this.dataCheckBtn.Size = new System.Drawing.Size(205, 68);
            this.dataCheckBtn.TabIndex = 5;
            this.dataCheckBtn.Text = "資料查詢";
            this.dataCheckBtn.Click += new System.EventHandler(this.dataCheckBtn_Click);
            // 
            // addDataBtn
            // 
            this.addDataBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.addDataBtn.Appearance.Font = new System.Drawing.Font("新細明體", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addDataBtn.Appearance.Options.UseFont = true;
            this.addDataBtn.Location = new System.Drawing.Point(620, 218);
            this.addDataBtn.Name = "addDataBtn";
            this.addDataBtn.Size = new System.Drawing.Size(205, 68);
            this.addDataBtn.TabIndex = 6;
            this.addDataBtn.Text = "資料新增";
            this.addDataBtn.Click += new System.EventHandler(this.addDataBtn_Click);
            // 
            // logOutBtn
            // 
            this.logOutBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.logOutBtn.Appearance.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.logOutBtn.Appearance.Options.UseFont = true;
            this.logOutBtn.Location = new System.Drawing.Point(440, 342);
            this.logOutBtn.Name = "logOutBtn";
            this.logOutBtn.Size = new System.Drawing.Size(167, 49);
            this.logOutBtn.TabIndex = 7;
            this.logOutBtn.Text = "登出";
            this.logOutBtn.Click += new System.EventHandler(this.logOutBtn_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.dataCheckBtn);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.addDataBtn);
            this.panelControl1.Controls.Add(this.logOutBtn);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1017, 569);
            this.panelControl1.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 569);
            this.Controls.Add(this.panelControl1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton dataCheckBtn;
        private DevExpress.XtraEditors.SimpleButton addDataBtn;
        private DevExpress.XtraEditors.SimpleButton logOutBtn;
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}

