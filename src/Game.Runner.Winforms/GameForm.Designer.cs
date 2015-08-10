namespace Game.Running.Winforms
{
    partial class GameForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.boardControl1 = new Game.Running.Winforms.BoardControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ScoreLabel = new System.Windows.Forms.Label();
            this.UnitsRemainingLabel = new System.Windows.Forms.Label();
            this.SolutionBox = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.ButtonSe = new System.Windows.Forms.Button();
            this.ButtonSw = new System.Windows.Forms.Button();
            this.ButtonE = new System.Windows.Forms.Button();
            this.ButtonW = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CWButton = new System.Windows.Forms.Button();
            this.CCWButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.CCWButton);
            this.splitContainer1.Panel2.Controls.Add(this.CWButton);
            this.splitContainer1.Panel2.Controls.Add(this.SolutionBox);
            this.splitContainer1.Panel2.Controls.Add(this.button2);
            this.splitContainer1.Panel2.Controls.Add(this.ButtonSe);
            this.splitContainer1.Panel2.Controls.Add(this.ButtonSw);
            this.splitContainer1.Panel2.Controls.Add(this.ButtonE);
            this.splitContainer1.Panel2.Controls.Add(this.ButtonW);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Size = new System.Drawing.Size(714, 671);
            this.splitContainer1.SplitterDistance = 502;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.boardControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer2.Size = new System.Drawing.Size(714, 502);
            this.splitContainer2.SplitterDistance = 506;
            this.splitContainer2.TabIndex = 0;
            // 
            // boardControl1
            // 
            this.boardControl1.Board = null;
            this.boardControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boardControl1.GameState = null;
            this.boardControl1.GraphicsEngine = null;
            this.boardControl1.Location = new System.Drawing.Point(0, 0);
            this.boardControl1.Name = "boardControl1";
            this.boardControl1.Size = new System.Drawing.Size(502, 498);
            this.boardControl1.TabIndex = 0;
            this.boardControl1.Text = "boardControl1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ScoreLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.UnitsRemainingLabel, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 498);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Score";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Units Remaining";
            // 
            // ScoreLabel
            // 
            this.ScoreLabel.AutoSize = true;
            this.ScoreLabel.Location = new System.Drawing.Point(103, 0);
            this.ScoreLabel.Name = "ScoreLabel";
            this.ScoreLabel.Size = new System.Drawing.Size(35, 13);
            this.ScoreLabel.TabIndex = 2;
            this.ScoreLabel.Text = "label3";
            // 
            // UnitsRemainingLabel
            // 
            this.UnitsRemainingLabel.AutoSize = true;
            this.UnitsRemainingLabel.Location = new System.Drawing.Point(103, 20);
            this.UnitsRemainingLabel.Name = "UnitsRemainingLabel";
            this.UnitsRemainingLabel.Size = new System.Drawing.Size(35, 13);
            this.UnitsRemainingLabel.TabIndex = 3;
            this.UnitsRemainingLabel.Text = "label4";
            // 
            // SolutionBox
            // 
            this.SolutionBox.Location = new System.Drawing.Point(352, 14);
            this.SolutionBox.Name = "SolutionBox";
            this.SolutionBox.Size = new System.Drawing.Size(259, 137);
            this.SolutionBox.TabIndex = 6;
            this.SolutionBox.Text = "";
            this.SolutionBox.SelectionChanged += new System.EventHandler(this.SolutionBox_SelectionChanged);
            this.SolutionBox.TextChanged += new System.EventHandler(this.SolutionBox_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(4, 33);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Solve";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ButtonSe
            // 
            this.ButtonSe.Location = new System.Drawing.Point(234, 43);
            this.ButtonSe.Name = "ButtonSe";
            this.ButtonSe.Size = new System.Drawing.Size(35, 23);
            this.ButtonSe.TabIndex = 4;
            this.ButtonSe.Text = "SE";
            this.ButtonSe.UseVisualStyleBackColor = true;
            this.ButtonSe.Click += new System.EventHandler(this.ButtonSe_Click);
            // 
            // ButtonSw
            // 
            this.ButtonSw.Location = new System.Drawing.Point(193, 43);
            this.ButtonSw.Name = "ButtonSw";
            this.ButtonSw.Size = new System.Drawing.Size(35, 23);
            this.ButtonSw.TabIndex = 3;
            this.ButtonSw.Text = "SW";
            this.ButtonSw.UseVisualStyleBackColor = true;
            this.ButtonSw.Click += new System.EventHandler(this.ButtonSw_Click);
            // 
            // ButtonE
            // 
            this.ButtonE.Location = new System.Drawing.Point(234, 14);
            this.ButtonE.Name = "ButtonE";
            this.ButtonE.Size = new System.Drawing.Size(20, 23);
            this.ButtonE.TabIndex = 2;
            this.ButtonE.Text = "E";
            this.ButtonE.UseVisualStyleBackColor = true;
            this.ButtonE.Click += new System.EventHandler(this.ButtonE_Click);
            // 
            // ButtonW
            // 
            this.ButtonW.Location = new System.Drawing.Point(208, 14);
            this.ButtonW.Name = "ButtonW";
            this.ButtonW.Size = new System.Drawing.Size(20, 23);
            this.ButtonW.TabIndex = 1;
            this.ButtonW.Text = "W";
            this.ButtonW.UseVisualStyleBackColor = true;
            this.ButtonW.Click += new System.EventHandler(this.ButtonW_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Spawn Piece";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(714, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // CWButton
            // 
            this.CWButton.Location = new System.Drawing.Point(260, 14);
            this.CWButton.Name = "CWButton";
            this.CWButton.Size = new System.Drawing.Size(35, 23);
            this.CWButton.TabIndex = 7;
            this.CWButton.Text = "CW";
            this.CWButton.UseVisualStyleBackColor = true;
            this.CWButton.Click += new System.EventHandler(this.CWButton_Click);
            // 
            // CCWButton
            // 
            this.CCWButton.Location = new System.Drawing.Point(167, 14);
            this.CCWButton.Name = "CCWButton";
            this.CCWButton.Size = new System.Drawing.Size(35, 23);
            this.CCWButton.TabIndex = 8;
            this.CCWButton.Text = "CCW";
            this.CCWButton.UseVisualStyleBackColor = true;
            this.CCWButton.Click += new System.EventHandler(this.CCWButton_Click);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 695);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "GameForm";
            this.Text = "ICFP 2015";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private BoardControl boardControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ScoreLabel;
        private System.Windows.Forms.Label UnitsRemainingLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button ButtonSe;
        private System.Windows.Forms.Button ButtonSw;
        private System.Windows.Forms.Button ButtonE;
        private System.Windows.Forms.Button ButtonW;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox SolutionBox;
        private System.Windows.Forms.Button CCWButton;
        private System.Windows.Forms.Button CWButton;
    }
}

