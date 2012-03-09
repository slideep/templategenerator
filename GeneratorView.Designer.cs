//using TemplateGenerator.Kontrollit;

//namespace TemplateGenerator
//{
//    partial class GeneratorView
//    {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary> 
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (!Controls.Contains(tekstiEditori))
//            {
//                tekstiEditori.Dispose();
//            }
//            if (!Controls.Contains(xmlTekstiEditori))
//            {
//                xmlTekstiEditori.Dispose();
//            }
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }

//            base.Dispose(disposing);
//        }

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            this.cmValinnat = new System.Windows.Forms.ContextMenuStrip(this.components);
//            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
//            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
//            this.valitseKaikkiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
//            this.pnlYlaosa = new System.Windows.Forms.Panel();
//            this.chkKopioi = new System.Windows.Forms.CheckBox();
//            this.lblLuokat = new System.Windows.Forms.Label();
//            this.lblMallipohjat = new System.Windows.Forms.Label();
//            this.cboLuokkaPohjat = new System.Windows.Forms.ComboBox();
//            this.btnGeneroi = new System.Windows.Forms.Button();
//            this.cboLuokat = new System.Windows.Forms.ComboBox();
//            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
//            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
//            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
//            this.tsSisalto = new System.Windows.Forms.ToolStripContainer();
//            this.ssTilapalkki = new System.Windows.Forms.StatusStrip();
//            this.tsslTila = new System.Windows.Forms.ToolStripStatusLabel();
//            this.tcGeneroitu = new System.Windows.Forms.TabControl();
//            this.tpMallipohja = new System.Windows.Forms.TabPage();
//            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
//            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
//            this.tvMallipohja = new XmlExplorer.Controls.XPathNavigatorTreeView();
//            this.xmlTekstiEditori = new TemplateGenerator.Kontrollit.XmlTekstiEditori();
//            this.groupBox2 = new System.Windows.Forms.GroupBox();
//            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
//            this.label3 = new System.Windows.Forms.Label();
//            this.cbHakuehto = new System.Windows.Forms.ComboBox();
//            this.btnHaeSanomasta = new System.Windows.Forms.Button();
//            this.tpGeneroitu = new System.Windows.Forms.TabPage();
//            this.tekstiEditori = new TemplateGenerator.Kontrollit.TekstiEditori();
//            this.cmValinnat.SuspendLayout();
//            this.pnlYlaosa.SuspendLayout();
//            this.tsSisalto.BottomToolStripPanel.SuspendLayout();
//            this.tsSisalto.ContentPanel.SuspendLayout();
//            this.tsSisalto.SuspendLayout();
//            this.ssTilapalkki.SuspendLayout();
//            this.tcGeneroitu.SuspendLayout();
//            this.tpMallipohja.SuspendLayout();
//            this.tableLayoutPanel2.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
//            this.splitContainer1.Panel1.SuspendLayout();
//            this.splitContainer1.Panel2.SuspendLayout();
//            this.splitContainer1.SuspendLayout();
//            this.groupBox2.SuspendLayout();
//            this.tableLayoutPanel4.SuspendLayout();
//            this.tpGeneroitu.SuspendLayout();
//            this.SuspendLayout();
//            // 
//            // cmValinnat
//            // 
//            this.cmValinnat.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
//            this.toolStripMenuItem2,
//            this.toolStripMenuItem1,
//            this.valitseKaikkiToolStripMenuItem});
//            this.cmValinnat.Name = "cmValinnat";
//            this.cmValinnat.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
//            this.cmValinnat.Size = new System.Drawing.Size(223, 70);
//            // 
//            // toolStripMenuItem2
//            // 
//            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
//            this.toolStripMenuItem2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
//            this.toolStripMenuItem2.Size = new System.Drawing.Size(222, 22);
//            this.toolStripMenuItem2.Text = "Tallenna";
//            // 
//            // toolStripMenuItem1
//            // 
//            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
//            this.toolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F12)));
//            this.toolStripMenuItem1.Size = new System.Drawing.Size(222, 22);
//            this.toolStripMenuItem1.Text = "&Tallenna nimellä...";
//            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
//            // 
//            // valitseKaikkiToolStripMenuItem
//            // 
//            this.valitseKaikkiToolStripMenuItem.Name = "valitseKaikkiToolStripMenuItem";
//            this.valitseKaikkiToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
//            this.valitseKaikkiToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
//            this.valitseKaikkiToolStripMenuItem.Text = "Valitse kaikki";
//            this.valitseKaikkiToolStripMenuItem.Click += new System.EventHandler(this.valitseKaikkiToolStripMenuItem_Click);
//            // 
//            // pnlYlaosa
//            // 
//            this.pnlYlaosa.Controls.Add(this.chkKopioi);
//            this.pnlYlaosa.Controls.Add(this.lblLuokat);
//            this.pnlYlaosa.Controls.Add(this.lblMallipohjat);
//            this.pnlYlaosa.Controls.Add(this.cboLuokkaPohjat);
//            this.pnlYlaosa.Controls.Add(this.btnGeneroi);
//            this.pnlYlaosa.Controls.Add(this.cboLuokat);
//            this.pnlYlaosa.Dock = System.Windows.Forms.DockStyle.Top;
//            this.pnlYlaosa.Location = new System.Drawing.Point(0, 0);
//            this.pnlYlaosa.Name = "pnlYlaosa";
//            this.pnlYlaosa.Size = new System.Drawing.Size(900, 33);
//            this.pnlYlaosa.TabIndex = 9;
//            // 
//            // chkKopioi
//            // 
//            this.chkKopioi.AutoSize = true;
//            this.chkKopioi.Checked = true;
//            this.chkKopioi.CheckState = System.Windows.Forms.CheckState.Checked;
//            this.chkKopioi.Location = new System.Drawing.Point(634, 8);
//            this.chkKopioi.Name = "chkKopioi";
//            this.chkKopioi.Size = new System.Drawing.Size(116, 17);
//            this.chkKopioi.TabIndex = 3;
//            this.chkKopioi.Text = "Siirrä &hakemistoihin";
//            this.chkKopioi.UseVisualStyleBackColor = true;
//            this.chkKopioi.CheckedChanged += new System.EventHandler(this.chkKopioi_CheckedChanged);
//            // 
//            // lblLuokat
//            // 
//            this.lblLuokat.AutoSize = true;
//            this.lblLuokat.Location = new System.Drawing.Point(11, 10);
//            this.lblLuokat.Name = "lblLuokat";
//            this.lblLuokat.Size = new System.Drawing.Size(61, 13);
//            this.lblLuokat.TabIndex = 11;
//            this.lblLuokat.Text = "&Descriptions:";
//            // 
//            // lblMallipohjat
//            // 
//            this.lblMallipohjat.AutoSize = true;
//            this.lblMallipohjat.Location = new System.Drawing.Point(355, 10);
//            this.lblMallipohjat.Name = "lblMallipohjat";
//            this.lblMallipohjat.Size = new System.Drawing.Size(60, 13);
//            this.lblMallipohjat.TabIndex = 10;
//            this.lblMallipohjat.Text = "Templates:";
//            // 
//            // cboLuokkaPohjat
//            // 
//            this.cboLuokkaPohjat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//            this.cboLuokkaPohjat.FormattingEnabled = true;
//            this.cboLuokkaPohjat.Location = new System.Drawing.Point(436, 6);
//            this.cboLuokkaPohjat.Name = "cboLuokkaPohjat";
//            this.cboLuokkaPohjat.Size = new System.Drawing.Size(191, 21);
//            this.cboLuokkaPohjat.TabIndex = 2;
//            // 
//            // btnGeneroi
//            // 
//            this.btnGeneroi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnGeneroi.DialogResult = System.Windows.Forms.DialogResult.OK;
//            this.btnGeneroi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
//            this.btnGeneroi.Location = new System.Drawing.Point(773, 5);
//            this.btnGeneroi.Name = "btnGeneroi";
//            this.btnGeneroi.Size = new System.Drawing.Size(111, 23);
//            this.btnGeneroi.TabIndex = 4;
//            this.btnGeneroi.Text = "&Generate";
//            this.btnGeneroi.UseVisualStyleBackColor = true;
//            this.btnGeneroi.Click += new System.EventHandler(this.btnGeneroi_Click);
//            // 
//            // cboLuokat
//            // 
//            this.cboLuokat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//            this.cboLuokat.FormattingEnabled = true;
//            this.cboLuokat.Location = new System.Drawing.Point(78, 6);
//            this.cboLuokat.Name = "cboLuokat";
//            this.cboLuokat.Size = new System.Drawing.Size(250, 21);
//            this.cboLuokat.TabIndex = 1;
//            this.cboLuokat.SelectedIndexChanged += new System.EventHandler(this.cboLuokat_SelectedIndexChanged);
//            // 
//            // openFileDialog1
//            // 
//            this.openFileDialog1.FileName = "openFileDialog1";
//            // 
//            // tsSisalto
//            // 
//            // 
//            // tsSisalto.BottomToolStripPanel
//            // 
//            this.tsSisalto.BottomToolStripPanel.Controls.Add(this.ssTilapalkki);
//            // 
//            // tsSisalto.ContentPanel
//            // 
//            this.tsSisalto.ContentPanel.Controls.Add(this.tcGeneroitu);
//            this.tsSisalto.ContentPanel.Size = new System.Drawing.Size(900, 479);
//            this.tsSisalto.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tsSisalto.Location = new System.Drawing.Point(0, 33);
//            this.tsSisalto.Name = "tsSisalto";
//            this.tsSisalto.Size = new System.Drawing.Size(900, 526);
//            this.tsSisalto.TabIndex = 10;
//            this.tsSisalto.Text = "toolStripContainer1";
//            // 
//            // ssTilapalkki
//            // 
//            this.ssTilapalkki.Dock = System.Windows.Forms.DockStyle.None;
//            this.ssTilapalkki.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
//            this.tsslTila});
//            this.ssTilapalkki.Location = new System.Drawing.Point(0, 0);
//            this.ssTilapalkki.Name = "ssTilapalkki";
//            this.ssTilapalkki.Size = new System.Drawing.Size(900, 22);
//            this.ssTilapalkki.TabIndex = 8;
//            this.ssTilapalkki.Text = "statusStrip1";
//            // 
//            // tsslTila
//            // 
//            this.tsslTila.Name = "tsslTila";
//            this.tsslTila.Size = new System.Drawing.Size(885, 17);
//            this.tsslTila.Spring = true;
//            this.tsslTila.Text = "Valmis";
//            this.tsslTila.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
//            // 
//            // tcGeneroitu
//            // 
//            this.tcGeneroitu.Controls.Add(this.tpMallipohja);
//            this.tcGeneroitu.Controls.Add(this.tpGeneroitu);
//            this.tcGeneroitu.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tcGeneroitu.Location = new System.Drawing.Point(0, 0);
//            this.tcGeneroitu.Name = "tcGeneroitu";
//            this.tcGeneroitu.SelectedIndex = 0;
//            this.tcGeneroitu.Size = new System.Drawing.Size(900, 479);
//            this.tcGeneroitu.TabIndex = 9;
//            // 
//            // tpMallipohja
//            // 
//            this.tpMallipohja.Controls.Add(this.tableLayoutPanel2);
//            this.tpMallipohja.Location = new System.Drawing.Point(4, 22);
//            this.tpMallipohja.Name = "tpMallipohja";
//            this.tpMallipohja.Padding = new System.Windows.Forms.Padding(3);
//            this.tpMallipohja.Size = new System.Drawing.Size(892, 453);
//            this.tpMallipohja.TabIndex = 1;
//            this.tpMallipohja.Text = "Mallipohja";
//            this.tpMallipohja.UseVisualStyleBackColor = true;
//            // 
//            // tableLayoutPanel2
//            // 
//            this.tableLayoutPanel2.ColumnCount = 3;
//            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
//            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
//            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
//            this.tableLayoutPanel2.Controls.Add(this.splitContainer1, 0, 1);
//            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 0, 0);
//            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
//            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
//            this.tableLayoutPanel2.RowCount = 2;
//            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
//            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
//            this.tableLayoutPanel2.Size = new System.Drawing.Size(886, 447);
//            this.tableLayoutPanel2.TabIndex = 0;
//            // 
//            // splitContainer1
//            // 
//            this.tableLayoutPanel2.SetColumnSpan(this.splitContainer1, 3);
//            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.splitContainer1.Location = new System.Drawing.Point(3, 75);
//            this.splitContainer1.Name = "splitContainer1";
//            // 
//            // splitContainer1.Panel1
//            // 
//            this.splitContainer1.Panel1.Controls.Add(this.tvMallipohja);
//            // 
//            // splitContainer1.Panel2
//            // 
//            this.splitContainer1.Panel2.Controls.Add(this.xmlTekstiEditori);
//            this.splitContainer1.Size = new System.Drawing.Size(880, 427);
//            this.splitContainer1.SplitterDistance = 142;
//            this.splitContainer1.TabIndex = 5;
//            // 
//            // tvMallipohja
//            // 
//            this.tvMallipohja.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tvMallipohja.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
//            this.tvMallipohja.HideSelection = false;
//            this.tvMallipohja.Location = new System.Drawing.Point(0, 0);
//            this.tvMallipohja.Name = "tvMallipohja";
//            this.tvMallipohja.Navigator = null;
//            this.tvMallipohja.Size = new System.Drawing.Size(142, 427);
//            this.tvMallipohja.TabIndex = 0;
//            // 
//            // xmlTekstiEditori
//            // 
//            this.xmlTekstiEditori.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.xmlTekstiEditori.IndentStyle = ICSharpCode.TextEditor.Document.IndentStyle.Auto;
//            this.xmlTekstiEditori.IsIconBarVisible = false;
//            this.xmlTekstiEditori.Location = new System.Drawing.Point(0, 0);
//            this.xmlTekstiEditori.Name = "xmlTekstiEditori";
//            this.xmlTekstiEditori.ShowInvalidLines = false;
//            this.xmlTekstiEditori.Size = new System.Drawing.Size(734, 427);
//            this.xmlTekstiEditori.TabIndent = 3;
//            this.xmlTekstiEditori.TabIndex = 6;
//            this.xmlTekstiEditori.UseAntiAliasFont = true;
//            // 
//            // groupBox2
//            // 
//            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
//            this.tableLayoutPanel2.SetColumnSpan(this.groupBox2, 3);
//            this.groupBox2.Controls.Add(this.tableLayoutPanel4);
//            this.groupBox2.Location = new System.Drawing.Point(3, 3);
//            this.groupBox2.Name = "groupBox2";
//            this.groupBox2.Size = new System.Drawing.Size(880, 66);
//            this.groupBox2.TabIndex = 6;
//            this.groupBox2.TabStop = false;
//            this.groupBox2.Text = " Haku ";
//            // 
//            // tableLayoutPanel4
//            // 
//            this.tableLayoutPanel4.ColumnCount = 3;
//            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.47195F));
//            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.52805F));
//            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
//            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 0);
//            this.tableLayoutPanel4.Controls.Add(this.cbHakuehto, 1, 0);
//            this.tableLayoutPanel4.Controls.Add(this.btnHaeSanomasta, 2, 0);
//            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
//            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
//            this.tableLayoutPanel4.RowCount = 2;
//            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
//            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
//            this.tableLayoutPanel4.Size = new System.Drawing.Size(874, 47);
//            this.tableLayoutPanel4.TabIndex = 0;
//            // 
//            // label3
//            // 
//            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
//            this.label3.AutoSize = true;
//            this.label3.Location = new System.Drawing.Point(3, 5);
//            this.label3.Name = "label3";
//            this.label3.Size = new System.Drawing.Size(139, 13);
//            this.label3.TabIndex = 0;
//            this.label3.Text = "Hae:";
//            // 
//            // cbHakuehto
//            // 
//            this.cbHakuehto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
//            this.cbHakuehto.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
//            this.cbHakuehto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
//            this.cbHakuehto.FlatStyle = System.Windows.Forms.FlatStyle.System;
//            this.cbHakuehto.FormattingEnabled = true;
//            this.cbHakuehto.Location = new System.Drawing.Point(148, 3);
//            this.cbHakuehto.Name = "cbHakuehto";
//            this.cbHakuehto.Size = new System.Drawing.Size(597, 17);
//            this.cbHakuehto.TabIndex = 1;
//            // 
//            // btnHaeSanomasta
//            // 
//            this.btnHaeSanomasta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnHaeSanomasta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
//            this.btnHaeSanomasta.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
//            this.btnHaeSanomasta.Location = new System.Drawing.Point(751, 3);
//            this.btnHaeSanomasta.Name = "btnHaeSanomasta";
//            this.tableLayoutPanel4.SetRowSpan(this.btnHaeSanomasta, 2);
//            this.btnHaeSanomasta.Size = new System.Drawing.Size(120, 41);
//            this.btnHaeSanomasta.TabIndex = 5;
//            this.btnHaeSanomasta.Text = "&Hae";
//            this.btnHaeSanomasta.UseVisualStyleBackColor = true;
//            // 
//            // tpGeneroitu
//            // 
//            this.tpGeneroitu.Controls.Add(this.tekstiEditori);
//            this.tpGeneroitu.Location = new System.Drawing.Point(4, 22);
//            this.tpGeneroitu.Name = "tpGeneroitu";
//            this.tpGeneroitu.Size = new System.Drawing.Size(892, 453);
//            this.tpGeneroitu.TabIndex = 2;
//            this.tpGeneroitu.Text = "Generoitu";
//            this.tpGeneroitu.UseVisualStyleBackColor = true;
//            // 
//            // tekstiEditori
//            // 
//            this.tekstiEditori.BracketMatchingStyle = ICSharpCode.TextEditor.Document.BracketMatchingStyle.Before;
//            this.tekstiEditori.CausesValidation = false;
//            this.tekstiEditori.ContextMenuStrip = this.cmValinnat;
//            this.tekstiEditori.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tekstiEditori.IndentStyle = ICSharpCode.TextEditor.Document.IndentStyle.Auto;
//            this.tekstiEditori.IsIconBarVisible = false;
//            this.tekstiEditori.LineViewerStyle = ICSharpCode.TextEditor.Document.LineViewerStyle.FullRow;
//            this.tekstiEditori.Location = new System.Drawing.Point(0, 0);
//            this.tekstiEditori.Name = "tekstiEditori";
//            this.tekstiEditori.ShowInvalidLines = false;
//            this.tekstiEditori.Size = new System.Drawing.Size(892, 453);
//            this.tekstiEditori.TabIndent = 3;
//            this.tekstiEditori.TabIndex = 0;
//            this.tekstiEditori.UseAntiAliasFont = true;
//            // 
//            // HeveGeneroijaNakyma
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(900, 559);
//            this.Controls.Add(this.tsSisalto);
//            this.Controls.Add(this.pnlYlaosa);
//            this.Name = "HeveGeneroijaNakyma";
//            this.Text = "Luokkageneroija";
//            this.Load += new System.EventHandler(this.TogetherNakyma_Load);
//            this.cmValinnat.ResumeLayout(false);
//            this.pnlYlaosa.ResumeLayout(false);
//            this.pnlYlaosa.PerformLayout();
//            this.tsSisalto.BottomToolStripPanel.ResumeLayout(false);
//            this.tsSisalto.BottomToolStripPanel.PerformLayout();
//            this.tsSisalto.ContentPanel.ResumeLayout(false);
//            this.tsSisalto.ResumeLayout(false);
//            this.tsSisalto.PerformLayout();
//            this.ssTilapalkki.ResumeLayout(false);
//            this.ssTilapalkki.PerformLayout();
//            this.tcGeneroitu.ResumeLayout(false);
//            this.tpMallipohja.ResumeLayout(false);
//            this.tableLayoutPanel2.ResumeLayout(false);
//            this.splitContainer1.Panel1.ResumeLayout(false);
//            this.splitContainer1.Panel2.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
//            this.splitContainer1.ResumeLayout(false);
//            this.groupBox2.ResumeLayout(false);
//            this.tableLayoutPanel4.ResumeLayout(false);
//            this.tableLayoutPanel4.PerformLayout();
//            this.tpGeneroitu.ResumeLayout(false);
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private System.Windows.Forms.Panel pnlYlaosa;
//        private System.Windows.Forms.CheckBox chkKopioi;
//        private System.Windows.Forms.Label lblLuokat;
//        private System.Windows.Forms.Label lblMallipohjat;
//        private System.Windows.Forms.ComboBox cboLuokkaPohjat;
//        private System.Windows.Forms.Button btnGeneroi;
//        private System.Windows.Forms.ComboBox cboLuokat;
//        private System.Windows.Forms.OpenFileDialog openFileDialog1;
//        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
//        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
//        private System.Windows.Forms.ContextMenuStrip cmValinnat;
//        private System.Windows.Forms.ToolStripMenuItem valitseKaikkiToolStripMenuItem;
//        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
//        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
//        private System.Windows.Forms.ToolStripContainer tsSisalto;
//        private System.Windows.Forms.TabControl tcGeneroitu;
//        private System.Windows.Forms.TabPage tpMallipohja;
//        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
//        private System.Windows.Forms.SplitContainer splitContainer1;
//        private XmlExplorer.Controls.XPathNavigatorTreeView tvMallipohja;
//        private XmlTekstiEditori xmlTekstiEditori;
//        private System.Windows.Forms.GroupBox groupBox2;
//        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
//        private System.Windows.Forms.Label label3;
//        private System.Windows.Forms.ComboBox cbHakuehto;
//        private System.Windows.Forms.Button btnHaeSanomasta;
//        private System.Windows.Forms.TabPage tpGeneroitu;
//        private TekstiEditori tekstiEditori;
//        private System.Windows.Forms.StatusStrip ssTilapalkki;
//        private System.Windows.Forms.ToolStripStatusLabel tsslTila;
//    }
//}

