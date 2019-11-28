namespace AstroDAM
{
    partial class frmPreferences
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
            this.components = new System.ComponentModel.Container();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBbCollection = new System.Windows.Forms.Button();
            this.btnBbObjectTitle = new System.Windows.Forms.Button();
            this.btnBbDateTime = new System.Windows.Forms.Button();
            this.tbEndNodeFormat = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cbPlaySplashClip = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnTestTree = new System.Windows.Forms.Button();
            this.cbNodeGrouping = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 16.30189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 35);
            this.label2.TabIndex = 5;
            this.label2.Text = "Preferences";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(18, 47);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(643, 365);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.treeView1);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.tbEndNodeFormat);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(635, 339);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "EndNode Naming";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(9, 70);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(414, 263);
            this.treeView1.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnTestTree);
            this.groupBox1.Controls.Add(this.btnBbCollection);
            this.groupBox1.Controls.Add(this.btnBbObjectTitle);
            this.groupBox1.Controls.Add(this.btnBbDateTime);
            this.groupBox1.Location = new System.Drawing.Point(429, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 263);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Building Blocks";
            // 
            // btnBbCollection
            // 
            this.btnBbCollection.Location = new System.Drawing.Point(6, 97);
            this.btnBbCollection.Name = "btnBbCollection";
            this.btnBbCollection.Size = new System.Drawing.Size(188, 33);
            this.btnBbCollection.TabIndex = 0;
            this.btnBbCollection.Text = "Catalogue";
            this.toolTip1.SetToolTip(this.btnBbCollection, "Inserts either collection Id {c|id}, short name {c|sn} \r\nor long name {c|ln}.\r\n");
            this.btnBbCollection.UseVisualStyleBackColor = true;
            this.btnBbCollection.Click += new System.EventHandler(this.btnBbCollection_Click);
            // 
            // btnBbObjectTitle
            // 
            this.btnBbObjectTitle.Location = new System.Drawing.Point(6, 58);
            this.btnBbObjectTitle.Name = "btnBbObjectTitle";
            this.btnBbObjectTitle.Size = new System.Drawing.Size(188, 33);
            this.btnBbObjectTitle.TabIndex = 0;
            this.btnBbObjectTitle.Text = "Object";
            this.toolTip1.SetToolTip(this.btnBbObjectTitle, "Inserts the Object field Id {o|id} or title {o|n}.");
            this.btnBbObjectTitle.UseVisualStyleBackColor = true;
            this.btnBbObjectTitle.Click += new System.EventHandler(this.btnBbObject_Click);
            // 
            // btnBbDateTime
            // 
            this.btnBbDateTime.Location = new System.Drawing.Point(6, 19);
            this.btnBbDateTime.Name = "btnBbDateTime";
            this.btnBbDateTime.Size = new System.Drawing.Size(188, 33);
            this.btnBbDateTime.TabIndex = 0;
            this.btnBbDateTime.Text = "Time";
            this.toolTip1.SetToolTip(this.btnBbDateTime, "Insert information from the CaptureDateTime field \r\nof a collection according to " +
        "the .NET DateTime formatting rules.");
            this.btnBbDateTime.UseVisualStyleBackColor = true;
            this.btnBbDateTime.Click += new System.EventHandler(this.btnBbDateTime_Click);
            // 
            // tbEndNodeFormat
            // 
            this.tbEndNodeFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbEndNodeFormat.Location = new System.Drawing.Point(9, 40);
            this.tbEndNodeFormat.Name = "tbEndNodeFormat";
            this.tbEndNodeFormat.Size = new System.Drawing.Size(620, 24);
            this.tbEndNodeFormat.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(383, 30);
            this.label1.TabIndex = 7;
            this.label1.Text = "Construct the wanted end-node format from the given building blocks.\r\nHover over " +
    "the buttons to see multiple options.";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cbNodeGrouping);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(635, 339);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Node Grouping";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Label according to:";
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSave.Location = new System.Drawing.Point(586, 418);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(505, 418);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "What\'s that?";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.cbPlaySplashClip);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(635, 339);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Graphics";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // cbPlaySplashClip
            // 
            this.cbPlaySplashClip.AutoSize = true;
            this.cbPlaySplashClip.Location = new System.Drawing.Point(6, 6);
            this.cbPlaySplashClip.Name = "cbPlaySplashClip";
            this.cbPlaySplashClip.Size = new System.Drawing.Size(202, 19);
            this.cbPlaySplashClip.TabIndex = 0;
            this.cbPlaySplashClip.Text = "Play animation in splash screen.";
            this.toolTip1.SetToolTip(this.cbPlaySplashClip, "Disabling this option will reduce RAM usage.\r\nGood for lower-end machines and if " +
        "you dislike\r\nthe opening sequence for some reason.");
            this.cbPlaySplashClip.UseVisualStyleBackColor = true;
            // 
            // btnTestTree
            // 
            this.btnTestTree.Location = new System.Drawing.Point(6, 224);
            this.btnTestTree.Name = "btnTestTree";
            this.btnTestTree.Size = new System.Drawing.Size(188, 33);
            this.btnTestTree.TabIndex = 1;
            this.btnTestTree.Text = "Test Tree";
            this.toolTip1.SetToolTip(this.btnTestTree, "Tests the tree.");
            this.btnTestTree.UseVisualStyleBackColor = true;
            this.btnTestTree.Click += new System.EventHandler(this.btnTestTree_Click);
            // 
            // cbNodeGrouping
            // 
            this.cbNodeGrouping.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNodeGrouping.FormattingEnabled = true;
            this.cbNodeGrouping.Items.AddRange(new object[] {
            "Capture Date",
            "Catalogue Name",
            "Object Title"});
            this.cbNodeGrouping.Location = new System.Drawing.Point(120, 15);
            this.cbNodeGrouping.Name = "cbNodeGrouping";
            this.cbNodeGrouping.Size = new System.Drawing.Size(121, 21);
            this.cbNodeGrouping.TabIndex = 2;
            // 
            // frmPreferences
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(673, 450);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label2);
            this.Name = "frmPreferences";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preferences";
            this.Load += new System.EventHandler(this.frmPreferences_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBbDateTime;
        private System.Windows.Forms.TextBox tbEndNodeFormat;
        private System.Windows.Forms.Button btnBbObjectTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button btnBbCollection;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox cbPlaySplashClip;
        private System.Windows.Forms.Button btnTestTree;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox cbNodeGrouping;
    }
}