namespace test_2
{
    partial class signUpForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.storeComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.textEditAcc = new DevExpress.XtraEditors.TextEdit();
            this.textEditPaswrd = new DevExpress.XtraEditors.TextEdit();
            this.textEditPaswrdA = new DevExpress.XtraEditors.TextEdit();
            this.signUpSimpleBtn = new DevExpress.XtraEditors.SimpleButton();
            this.returnBtn = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.storeComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditAcc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPaswrd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPaswrdA.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(372, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 28);
            this.label1.TabIndex = 3;
            this.label1.Text = "帳號";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(372, 206);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 28);
            this.label2.TabIndex = 4;
            this.label2.Text = "密碼";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(227, 264);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(215, 28);
            this.label3.TabIndex = 5;
            this.label3.Text = "再輸入一次密碼";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(372, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 28);
            this.label4.TabIndex = 8;
            this.label4.Text = "門市";
            // 
            // storeComboBoxEdit
            // 
            this.storeComboBoxEdit.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.storeComboBoxEdit.EditValue = "請選擇門市";
            this.storeComboBoxEdit.Location = new System.Drawing.Point(448, 94);
            this.storeComboBoxEdit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.storeComboBoxEdit.Name = "storeComboBoxEdit";
            this.storeComboBoxEdit.Properties.Appearance.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.storeComboBoxEdit.Properties.Appearance.Options.UseFont = true;
            this.storeComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.storeComboBoxEdit.Size = new System.Drawing.Size(249, 42);
            this.storeComboBoxEdit.TabIndex = 10;
            this.storeComboBoxEdit.SelectedIndexChanged += new System.EventHandler(this.storeComboBoxEdit_SelectedIndexChanged);
            // 
            // textEditAcc
            // 
            this.textEditAcc.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textEditAcc.Location = new System.Drawing.Point(448, 145);
            this.textEditAcc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textEditAcc.Name = "textEditAcc";
            this.textEditAcc.Properties.Appearance.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textEditAcc.Properties.Appearance.Options.UseFont = true;
            this.textEditAcc.Size = new System.Drawing.Size(344, 42);
            this.textEditAcc.TabIndex = 11;
            // 
            // textEditPaswrd
            // 
            this.textEditPaswrd.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textEditPaswrd.Location = new System.Drawing.Point(448, 201);
            this.textEditPaswrd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textEditPaswrd.Name = "textEditPaswrd";
            this.textEditPaswrd.Properties.Appearance.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textEditPaswrd.Properties.Appearance.Options.UseFont = true;
            this.textEditPaswrd.Size = new System.Drawing.Size(344, 42);
            this.textEditPaswrd.TabIndex = 12;
            // 
            // textEditPaswrdA
            // 
            this.textEditPaswrdA.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textEditPaswrdA.Location = new System.Drawing.Point(448, 259);
            this.textEditPaswrdA.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textEditPaswrdA.Name = "textEditPaswrdA";
            this.textEditPaswrdA.Properties.Appearance.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textEditPaswrdA.Properties.Appearance.Options.UseFont = true;
            this.textEditPaswrdA.Size = new System.Drawing.Size(344, 42);
            this.textEditPaswrdA.TabIndex = 13;
            // 
            // signUpSimpleBtn
            // 
            this.signUpSimpleBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.signUpSimpleBtn.Appearance.Font = new System.Drawing.Font("新細明體", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.signUpSimpleBtn.Appearance.Options.UseFont = true;
            this.signUpSimpleBtn.Location = new System.Drawing.Point(494, 326);
            this.signUpSimpleBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.signUpSimpleBtn.Name = "signUpSimpleBtn";
            this.signUpSimpleBtn.Size = new System.Drawing.Size(203, 56);
            this.signUpSimpleBtn.TabIndex = 14;
            this.signUpSimpleBtn.Text = "創建";
            this.signUpSimpleBtn.Click += new System.EventHandler(this.signUpSimpleBtn_Click);
            // 
            // returnBtn
            // 
            this.returnBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.returnBtn.Appearance.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.returnBtn.Appearance.Options.UseFont = true;
            this.returnBtn.Location = new System.Drawing.Point(540, 415);
            this.returnBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.returnBtn.Name = "returnBtn";
            this.returnBtn.Size = new System.Drawing.Size(112, 32);
            this.returnBtn.TabIndex = 15;
            this.returnBtn.Text = "取消";
            this.returnBtn.Click += new System.EventHandler(this.returnBtn_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.textEditPaswrd);
            this.panelControl1.Controls.Add(this.returnBtn);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.signUpSimpleBtn);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.textEditPaswrdA);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Controls.Add(this.textEditAcc);
            this.panelControl1.Controls.Add(this.storeComboBoxEdit);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1167, 615);
            this.panelControl1.TabIndex = 16;
            // 
            // signUpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1167, 615);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "signUpForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "signUpForm";
            this.Load += new System.EventHandler(this.signUpForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.storeComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditAcc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPaswrd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPaswrdA.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ComboBoxEdit storeComboBoxEdit;
        private DevExpress.XtraEditors.TextEdit textEditAcc;
        private DevExpress.XtraEditors.TextEdit textEditPaswrd;
        private DevExpress.XtraEditors.TextEdit textEditPaswrdA;
        private DevExpress.XtraEditors.SimpleButton signUpSimpleBtn;
        private DevExpress.XtraEditors.SimpleButton returnBtn;
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}