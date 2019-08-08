namespace AFC.WS.UI.UIRuleFileCreator
{
    /// <summary>
    /// 
    /// </summary>
    partial class TestForm
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.aToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBegin = new System.Windows.Forms.Button();
            this.txtNamespace = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDirecotry = new System.Windows.Forms.TextBox();
            this.clbTableName = new System.Windows.Forms.CheckedListBox();
            this.btnAllChoose = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtCondition = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aToolStripMenuItem,
            this.bToolStripMenuItem,
            this.dToolStripMenuItem,
            this.dToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(77, 92);
            // 
            // aToolStripMenuItem
            // 
            this.aToolStripMenuItem.Name = "aToolStripMenuItem";
            this.aToolStripMenuItem.Size = new System.Drawing.Size(76, 22);
            this.aToolStripMenuItem.Text = "a";
            // 
            // bToolStripMenuItem
            // 
            this.bToolStripMenuItem.Name = "bToolStripMenuItem";
            this.bToolStripMenuItem.Size = new System.Drawing.Size(76, 22);
            this.bToolStripMenuItem.Text = "b";
            // 
            // dToolStripMenuItem
            // 
            this.dToolStripMenuItem.Name = "dToolStripMenuItem";
            this.dToolStripMenuItem.Size = new System.Drawing.Size(76, 22);
            this.dToolStripMenuItem.Text = "d";
            // 
            // dToolStripMenuItem1
            // 
            this.dToolStripMenuItem1.Name = "dToolStripMenuItem1";
            this.dToolStripMenuItem1.Size = new System.Drawing.Size(76, 22);
            this.dToolStripMenuItem1.Text = "d";
            // 
            // btnBegin
            // 
            this.btnBegin.Location = new System.Drawing.Point(384, 95);
            this.btnBegin.Name = "btnBegin";
            this.btnBegin.Size = new System.Drawing.Size(75, 23);
            this.btnBegin.TabIndex = 1;
            this.btnBegin.Text = "开  始";
            this.btnBegin.UseVisualStyleBackColor = true;
            this.btnBegin.Click += new System.EventHandler(this.btnBegin_Click);
            // 
            // txtNamespace
            // 
            this.txtNamespace.Location = new System.Drawing.Point(384, 12);
            this.txtNamespace.Name = "txtNamespace";
            this.txtNamespace.Size = new System.Drawing.Size(220, 21);
            this.txtNamespace.TabIndex = 2;
            this.txtNamespace.Text = "AFC.WS.UI.BR.Data";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(313, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "名称空间：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(313, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "存放目录：";
            // 
            // txtDirecotry
            // 
            this.txtDirecotry.Location = new System.Drawing.Point(384, 53);
            this.txtDirecotry.Name = "txtDirecotry";
            this.txtDirecotry.Size = new System.Drawing.Size(220, 21);
            this.txtDirecotry.TabIndex = 5;
            this.txtDirecotry.Text = "D:\\Codes";
            // 
            // clbTableName
            // 
            this.clbTableName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbTableName.FormattingEnabled = true;
            this.clbTableName.Location = new System.Drawing.Point(3, 17);
            this.clbTableName.Name = "clbTableName";
            this.clbTableName.Size = new System.Drawing.Size(276, 292);
            this.clbTableName.TabIndex = 7;
            this.clbTableName.SelectedIndexChanged += new System.EventHandler(this.clbTableName_SelectedIndexChanged);
            // 
            // btnAllChoose
            // 
            this.btnAllChoose.Location = new System.Drawing.Point(303, 318);
            this.btnAllChoose.Name = "btnAllChoose";
            this.btnAllChoose.Size = new System.Drawing.Size(75, 23);
            this.btnAllChoose.TabIndex = 8;
            this.btnAllChoose.Text = "全  选";
            this.btnAllChoose.UseVisualStyleBackColor = true;
            this.btnAllChoose.Click += new System.EventHandler(this.btnAllChoose_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(303, 363);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "反  选";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtCondition
            // 
            this.txtCondition.Location = new System.Drawing.Point(53, 22);
            this.txtCondition.Name = "txtCondition";
            this.txtCondition.Size = new System.Drawing.Size(167, 21);
            this.txtCondition.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "搜索：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clbTableName);
            this.groupBox1.Location = new System.Drawing.Point(0, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 323);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据库表：";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(226, 20);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(49, 23);
            this.btnSearch.TabIndex = 13;
            this.btnSearch.Text = "确  定";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Controls.Add(this.txtCondition);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(1, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(281, 58);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "搜索条件：";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 398);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnAllChoose);
            this.Controls.Add(this.txtNamespace);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDirecotry);
            this.Controls.Add(this.btnBegin);
            this.Controls.Add(this.label2);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dToolStripMenuItem1;
        private System.Windows.Forms.Button btnBegin;
        private System.Windows.Forms.TextBox txtNamespace;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDirecotry;
        private System.Windows.Forms.CheckedListBox clbTableName;
        private System.Windows.Forms.Button btnAllChoose;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtCondition;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBox2;

    }
}