namespace AstroDAM
{
    partial class DbConnectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbConnectionForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDataSource = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDatabaseFile = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rbSecurityYes = new System.Windows.Forms.RadioButton();
            this.rbSecurityNo = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.tbConnectionTimeout = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tbConnectionString = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblTestResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 92);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data Source";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 16.30189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 7);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(298, 38);
            this.label2.TabIndex = 4;
            this.label2.Text = "Connect to a database.";
            // 
            // tbDataSource
            // 
            this.tbDataSource.Location = new System.Drawing.Point(192, 89);
            this.tbDataSource.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbDataSource.Name = "tbDataSource";
            this.tbDataSource.Size = new System.Drawing.Size(407, 22);
            this.tbDataSource.TabIndex = 0;
            this.tbDataSource.TextChanged += new System.EventHandler(this.ParametersChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 124);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Initial Catalog";
            // 
            // tbDatabaseFile
            // 
            this.tbDatabaseFile.Location = new System.Drawing.Point(192, 121);
            this.tbDatabaseFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbDatabaseFile.Name = "tbDatabaseFile";
            this.tbDatabaseFile.Size = new System.Drawing.Size(407, 22);
            this.tbDatabaseFile.TabIndex = 1;
            this.tbDatabaseFile.TextChanged += new System.EventHandler(this.ParametersChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 156);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Integrated Security";
            // 
            // rbSecurityYes
            // 
            this.rbSecurityYes.AutoSize = true;
            this.rbSecurityYes.Location = new System.Drawing.Point(192, 153);
            this.rbSecurityYes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbSecurityYes.Name = "rbSecurityYes";
            this.rbSecurityYes.Size = new System.Drawing.Size(53, 21);
            this.rbSecurityYes.TabIndex = 2;
            this.rbSecurityYes.TabStop = true;
            this.rbSecurityYes.Text = "Yes";
            this.rbSecurityYes.UseVisualStyleBackColor = true;
            this.rbSecurityYes.CheckedChanged += new System.EventHandler(this.ParametersChanged);
            // 
            // rbSecurityNo
            // 
            this.rbSecurityNo.AutoSize = true;
            this.rbSecurityNo.Location = new System.Drawing.Point(260, 153);
            this.rbSecurityNo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbSecurityNo.Name = "rbSecurityNo";
            this.rbSecurityNo.Size = new System.Drawing.Size(47, 21);
            this.rbSecurityNo.TabIndex = 3;
            this.rbSecurityNo.TabStop = true;
            this.rbSecurityNo.Text = "No";
            this.rbSecurityNo.UseVisualStyleBackColor = true;
            this.rbSecurityNo.CheckedChanged += new System.EventHandler(this.ParametersChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 187);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Connection Timeout";
            // 
            // tbConnectionTimeout
            // 
            this.tbConnectionTimeout.Location = new System.Drawing.Point(192, 183);
            this.tbConnectionTimeout.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbConnectionTimeout.Name = "tbConnectionTimeout";
            this.tbConnectionTimeout.Size = new System.Drawing.Size(84, 22);
            this.tbConnectionTimeout.TabIndex = 4;
            this.tbConnectionTimeout.TextChanged += new System.EventHandler(this.ParametersChanged);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(23, 310);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(132, 28);
            this.btnTest.TabIndex = 6;
            this.btnTest.Text = "Test Connection";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.BtnTest_Click);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(577, 310);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 28);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 252);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 17);
            this.label6.TabIndex = 8;
            this.label6.Text = "Connection String";
            // 
            // tbConnectionString
            // 
            this.tbConnectionString.Location = new System.Drawing.Point(192, 249);
            this.tbConnectionString.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbConnectionString.Multiline = true;
            this.tbConnectionString.Name = "tbConnectionString";
            this.tbConnectionString.Size = new System.Drawing.Size(484, 53);
            this.tbConnectionString.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label7.Location = new System.Drawing.Point(19, 212);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(581, 33);
            this.label7.TabIndex = 8;
            this.label7.Text = "- or, define a connection string manually -";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label8.Location = new System.Drawing.Point(20, 52);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(581, 33);
            this.label8.TabIndex = 8;
            this.label8.Text = "You can define basic settings for a connection string...";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTestResult
            // 
            this.lblTestResult.AutoSize = true;
            this.lblTestResult.Location = new System.Drawing.Point(163, 316);
            this.lblTestResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTestResult.Name = "lblTestResult";
            this.lblTestResult.Size = new System.Drawing.Size(169, 17);
            this.lblTestResult.TabIndex = 9;
            this.lblTestResult.Text = "Awating connection test...";
            // 
            // DbConnectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 345);
            this.Controls.Add(this.lblTestResult);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.tbConnectionTimeout);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.rbSecurityNo);
            this.Controls.Add(this.rbSecurityYes);
            this.Controls.Add(this.tbConnectionString);
            this.Controls.Add(this.tbDatabaseFile);
            this.Controls.Add(this.tbDataSource);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "DbConnectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AstroDAM - Define Database Connection";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDataSource;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDatabaseFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rbSecurityYes;
        private System.Windows.Forms.RadioButton rbSecurityNo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbConnectionTimeout;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbConnectionString;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblTestResult;
    }
}