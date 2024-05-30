namespace test_2
{
    partial class loginFormN
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loginFormPanel = new DevExpress.XtraEditors.PanelControl();
            this.crtBtn = new DevExpress.XtraEditors.SimpleButton();
            this.loginBtn = new DevExpress.XtraEditors.SimpleButton();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.txtAccount = new DevExpress.XtraEditors.TextEdit();
            this.storeComboBox1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.loginFormPanel)).BeginInit();
            this.loginFormPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.storeComboBox1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // loginFormPanel
            // 
            this.loginFormPanel.Controls.Add(this.crtBtn);
            this.loginFormPanel.Controls.Add(this.loginBtn);
            this.loginFormPanel.Controls.Add(this.txtPassword);
            this.loginFormPanel.Controls.Add(this.txtAccount);
            this.loginFormPanel.Controls.Add(this.storeComboBox1);
            this.loginFormPanel.Controls.Add(this.pictureEdit1);
            this.loginFormPanel.Controls.Add(this.label3);
            this.loginFormPanel.Controls.Add(this.label2);
            this.loginFormPanel.Controls.Add(this.label1);
            this.loginFormPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loginFormPanel.Location = new System.Drawing.Point(0, 0);
            this.loginFormPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.loginFormPanel.Name = "loginFormPanel";
            this.loginFormPanel.Size = new System.Drawing.Size(774, 589);
            this.loginFormPanel.TabIndex = 0;
            // 
            // crtBtn
            // 
            this.crtBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.crtBtn.Appearance.Font = new System.Drawing.Font("新細明體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.crtBtn.Appearance.Options.UseFont = true;
            this.crtBtn.Location = new System.Drawing.Point(343, 377);
            this.crtBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.crtBtn.Name = "crtBtn";
            this.crtBtn.Size = new System.Drawing.Size(92, 32);
            this.crtBtn.TabIndex = 23;
            this.crtBtn.Text = "創建帳號";
            this.crtBtn.Click += new System.EventHandler(this.crtBtn_Click);
            // 
            // loginBtn
            // 
            this.loginBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.loginBtn.Appearance.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginBtn.Appearance.Options.UseFont = true;
            this.loginBtn.Location = new System.Drawing.Point(312, 308);
            this.loginBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.loginBtn.Name = "loginBtn";
            this.loginBtn.Size = new System.Drawing.Size(151, 52);
            this.loginBtn.TabIndex = 22;
            this.loginBtn.Text = "登入";
            this.loginBtn.Click += new System.EventHandler(this.loginBtn_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtPassword.Location = new System.Drawing.Point(268, 259);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Properties.Appearance.Options.UseFont = true;
            this.txtPassword.Size = new System.Drawing.Size(234, 31);
            this.txtPassword.TabIndex = 21;
            // 
            // txtAccount
            // 
            this.txtAccount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtAccount.Location = new System.Drawing.Point(268, 216);
            this.txtAccount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAccount.Name = "txtAccount";
            this.txtAccount.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAccount.Properties.Appearance.Options.UseFont = true;
            this.txtAccount.Size = new System.Drawing.Size(234, 31);
            this.txtAccount.TabIndex = 20;
            // 
            // storeComboBox1
            // 
            this.storeComboBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.storeComboBox1.EditValue = "請選擇門市";
            this.storeComboBox1.Location = new System.Drawing.Point(269, 176);
            this.storeComboBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.storeComboBox1.Name = "storeComboBox1";
            this.storeComboBox1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.storeComboBox1.Size = new System.Drawing.Size(233, 25);
            this.storeComboBox1.TabIndex = 19;
            this.storeComboBox1.SelectedIndexChanged += new System.EventHandler(this.storeComboBox1_SelectedIndexChanged);
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureEdit1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureEdit1.EditValue = global::test_2.Properties.Resources.mcdonaldsLogo;
            this.pictureEdit1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureEdit1.Location = new System.Drawing.Point(312, 30);
            this.pictureEdit1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pictureEdit1.Size = new System.Drawing.Size(161, 132);
            this.pictureEdit1.TabIndex = 18;
            this.pictureEdit1.EditValueChanged += new System.EventHandler(this.pictureEdit1_EditValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(214, 178);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 20);
            this.label3.TabIndex = 17;
            this.label3.Text = "門市";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(214, 262);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "密碼";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(214, 219);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 20);
            this.label1.TabIndex = 15;
            this.label1.Text = "帳號";
            // 
            // loginFormN
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(774, 589);
            this.Controls.Add(this.loginFormPanel);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "loginFormN";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登入";
            this.Load += new System.EventHandler(this.loginFormN_Load);
            ((System.ComponentModel.ISupportInitialize)(this.loginFormPanel)).EndInit();
            this.loginFormPanel.ResumeLayout(false);
            this.loginFormPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.storeComboBox1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl loginFormPanel;
        private DevExpress.XtraEditors.SimpleButton crtBtn;
        private DevExpress.XtraEditors.SimpleButton loginBtn;
        private DevExpress.XtraEditors.TextEdit txtPassword;
        private DevExpress.XtraEditors.TextEdit txtAccount;
        private DevExpress.XtraEditors.ComboBoxEdit storeComboBox1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;



    }
}