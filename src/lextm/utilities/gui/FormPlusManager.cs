// this is Plus manager form.
// Copyright (C) 2006  Lex Y. Li
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using Lextm.Diagnostics;
using BeWise.Common.Utils;
using Lextm.OpenTools.Plus;
using System.Globalization;
using Lextm.Windows.Forms;

namespace Lextm.Utilities.Gui
{
    /// <summary>
    /// Form Plus Manager.
    /// </summary>
    public class FormPlusManager : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox gbFeature;

        TreeNode rootNode = new TreeNode("Pluses");

        private const string MaskIDE = "Version {0}.0 ({1})";
        private System.Windows.Forms.RichTextBox tbFeatureInfo;
        private System.Windows.Forms.TextBox tbPlus;
        private System.Windows.Forms.RichTextBox tbPlusInfo;
        private ToolStrip toolStrip1;
        private ToolStripButton tsbtnSave;
        private ToolStripLabel tslblIDE;
        private Crad.Windows.Forms.Actions.ActionList alPlusManager;
        private Crad.Windows.Forms.Actions.Action actSave;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripComboBox tscbIdeVersion;
        private SplitContainer splitContainer1;
        private TableLayoutPanel tableLayoutPanel1;
        private ImageList imageList1;
        private IContainer components;
        private ZetaLib.Windows.Controls.TriStateCheckBoxesTreeView tvPlusList;
        private System.Windows.Forms.Label label1;
        /// <summary>
        /// Construtor.
        /// </summary>
        public FormPlusManager()
        {
            InitializeComponent();

            list.LoadPluses();
            tvPlusList.Nodes.Clear();
            tvPlusList.Nodes.Add(rootNode);

            rootNode.StateImageIndex = 0;
            rootNode.ImageIndex = 0;
            rootNode.SelectedImageIndex = 0;

            tscbIdeVersion.Sorted = false;
            foreach (IdeVersionInfo info in OtaUtils.IdeVersionInfoCollection)
            {
                tscbIdeVersion.Items.Insert(info.Version - 1, String.Format(CultureInfo.InvariantCulture, MaskIDE, info.Version, info.Name));
            }

            tscbIdeVersion.SelectedIndex = tscbIdeVersion.Items.Count - 1;

            modified = false;
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPlusManager));
            this.tbPlusInfo = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPlus = new System.Windows.Forms.TextBox();
            this.gbFeature = new System.Windows.Forms.GroupBox();
            this.tbFeatureInfo = new System.Windows.Forms.RichTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tslblIDE = new System.Windows.Forms.ToolStripLabel();
            this.tscbIdeVersion = new System.Windows.Forms.ToolStripComboBox();
            this.alPlusManager = new Crad.Windows.Forms.Actions.ActionList();
            this.actSave = new Crad.Windows.Forms.Actions.Action();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvPlusList = new ZetaLib.Windows.Controls.TriStateCheckBoxesTreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gbFeature.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.alPlusManager)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbPlusInfo
            // 
            this.tbPlusInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbPlusInfo.Location = new System.Drawing.Point(3, 66);
            this.tbPlusInfo.Name = "tbPlusInfo";
            this.tbPlusInfo.ReadOnly = true;
            this.tbPlusInfo.Size = new System.Drawing.Size(388, 74);
            this.tbPlusInfo.TabIndex = 12;
            this.tbPlusInfo.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(388, 18);
            this.label1.TabIndex = 11;
            this.label1.Text = "Plus Information:";
            // 
            // tbPlus
            // 
            this.tbPlus.BackColor = System.Drawing.SystemColors.Control;
            this.tbPlus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbPlus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbPlus.Location = new System.Drawing.Point(3, 3);
            this.tbPlus.Multiline = true;
            this.tbPlus.Name = "tbPlus";
            this.tbPlus.Size = new System.Drawing.Size(388, 39);
            this.tbPlus.TabIndex = 9;
            // 
            // gbFeature
            // 
            this.gbFeature.Controls.Add(this.tbFeatureInfo);
            this.gbFeature.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFeature.Location = new System.Drawing.Point(3, 146);
            this.gbFeature.Name = "gbFeature";
            this.gbFeature.Size = new System.Drawing.Size(388, 277);
            this.gbFeature.TabIndex = 2;
            this.gbFeature.TabStop = false;
            this.gbFeature.Text = "Feature";
            // 
            // tbFeatureInfo
            // 
            this.tbFeatureInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbFeatureInfo.Location = new System.Drawing.Point(3, 17);
            this.tbFeatureInfo.Name = "tbFeatureInfo";
            this.tbFeatureInfo.ReadOnly = true;
            this.tbFeatureInfo.Size = new System.Drawing.Size(382, 257);
            this.tbFeatureInfo.TabIndex = 1;
            this.tbFeatureInfo.Text = "";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnSave,
            this.toolStripSeparator1,
            this.tslblIDE,
            this.tscbIdeVersion});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(558, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnSave
            // 
            this.alPlusManager.SetAction(this.tsbtnSave, this.actSave);
            this.tsbtnSave.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnSave.Image")));
            this.tsbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSave.Name = "tsbtnSave";
            this.tsbtnSave.Size = new System.Drawing.Size(55, 22);
            this.tsbtnSave.Text = "Save";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tslblIDE
            // 
            this.tslblIDE.Name = "tslblIDE";
            this.tslblIDE.Size = new System.Drawing.Size(28, 22);
            this.tslblIDE.Text = "IDE";
            // 
            // tscbIdeVersion
            // 
            this.tscbIdeVersion.Name = "tscbIdeVersion";
            this.tscbIdeVersion.Size = new System.Drawing.Size(280, 25);
            this.tscbIdeVersion.Text = "(Select)";
            this.tscbIdeVersion.SelectedIndexChanged += new System.EventHandler(this.cbIDEVersion_SelectedIndexChanged);
            // 
            // alPlusManager
            // 
            this.alPlusManager.Actions.Add(this.actSave);
            this.alPlusManager.ContainerControl = this;
            // 
            // actSave
            // 
            this.actSave.Image = ((System.Drawing.Image)(resources.GetObject("actSave.Image")));
            this.actSave.Text = "Save";
            this.actSave.ToolTipText = "Save";
            this.actSave.Update += new System.EventHandler(this.actSave_Update);
            this.actSave.Execute += new System.EventHandler(this.actSave_Execute);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvPlusList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(558, 426);
            this.splitContainer1.SplitterDistance = 160;
            this.splitContainer1.TabIndex = 5;
            // 
            // tvPlusList
            // 
            this.tvPlusList.AutoCheckChilds = true;
            this.tvPlusList.AutoCheckParents = true;
            this.tvPlusList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvPlusList.ImageIndex = 0;
            this.tvPlusList.ImageList = this.imageList1;
            this.tvPlusList.IndeterminateToChecked = true;
            this.tvPlusList.Location = new System.Drawing.Point(0, 0);
            this.tvPlusList.Name = "tvPlusList";
            this.tvPlusList.SelectedImageIndex = 0;
            this.tvPlusList.Size = new System.Drawing.Size(160, 426);
            this.tvPlusList.TabIndex = 0;
            this.tvPlusList.UseTriStateCheckBoxes = true;
            this.tvPlusList.AfterTriStateCheck += new ZetaLib.Windows.Controls.TriStateCheckBoxesTreeView.AfterTriStateCheckEventHandler(this.tvPlusList_AfterTriStateCheck);
            this.tvPlusList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvPlusList_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "computer.png");
            this.imageList1.Images.SetKeyName(1, "folder.png");
            this.imageList1.Images.SetKeyName(2, "text-x-generic.png");
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tbPlus, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gbFeature, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbPlusInfo, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(394, 426);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // FormPlusManager
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(558, 451);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPlusManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Plus Manager";
            this.Load += new System.EventHandler(this.FormPlusManager_Load);
            this.Closed += new System.EventHandler(this.FormPlusManager_Closed);
            this.gbFeature.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.alPlusManager)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private bool modified;
 
        private void DescribePlus(TreeNode node)
        {
            Plus2 plus = node.Tag as Plus2;

            System.Reflection.Assembly assembly =
                Plus2.LoadAssembly(plus.ModuleName);

            tbPlus.Text = Lextm.Reflection.AssemblyHelper.GetProduct(assembly)
                + " Version: " + Lextm.Reflection.AssemblyHelper.GetVersion(assembly)
                + " " + Lextm.Reflection.AssemblyHelper.GetCopyright(assembly);

            tbPlusInfo.Text = Lextm.Reflection.AssemblyHelper.GetDescription(assembly);

            ShowInfo(true);
            gbFeature.Visible = false;
        }

        private void DescribeFeature(TreeNode node)
        {
            Feature2 feature = node.Tag as Feature2;
            DescribePlus(node.Parent);
            gbFeature.Visible = true;
            tbFeatureInfo.Text = feature.Description;
        }
 
        #region Show plus and feature info
        private void tvPlusList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is Feature2)
            {
                DescribeFeature(e.Node);
            }
            else if (e.Node.Tag is Plus2)
            {
                DescribePlus(e.Node);
            }
            else
            {
                ShowInfo(false);
            }
        }

        private void ShowInfo(bool visible)
        {
            label1.Visible = visible;
            tbPlus.Visible = visible;
            tbPlusInfo.Visible = visible;
            gbFeature.Visible = visible;
        }
        #endregion

        private int GetIdeVersion()
        {
            return tscbIdeVersion.SelectedIndex + 1;
        }

        private void cbIDEVersion_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            PersistState();
            UpdateTreeView();
            modified = false;
        }

        private Plus2Registry list = Plus2Registry.Instance;

        private void UpdateTreeView()
        {

            tvPlusList.BeginUpdate();
            rootNode.Nodes.Clear();

            foreach (Plus2 plus in list.Plus2Collection)
            {
                TreeNode plusNode = new TreeNode(plus.Name);
                plusNode.Tag = plus;
                tvPlusList.Nodes[0].Nodes.Add(plusNode);
                plusNode.StateImageIndex = 0;
                plusNode.ImageIndex = 1;
                plusNode.SelectedImageIndex = 1;
                foreach (Feature2 feature in plus.Features)
                {
                    string[] tmp = feature.Name.Split('.');
                    TreeNode featureNode = new TreeNode(tmp[tmp.GetUpperBound(0)]);
                    featureNode.Tag = feature;
                    plusNode.Nodes.Add(featureNode);
                    featureNode.StateImageIndex = 0;
                    featureNode.ImageIndex = 2;
                    featureNode.SelectedImageIndex = 2;
                    tvPlusList.SetNodeCheckState(featureNode,
                        feature.IsEnabledFor(GetIdeVersion()) ? CheckState.Checked : CheckState.Unchecked);
                }
                tvPlusList.ChangeNodesCheckStates(plusNode, ZetaLib.Windows.Controls.TriStateCheckBoxesTreeView.NodesCheckState.UpdateStateFromChilds);
            }
            tvPlusList.ChangeNodesCheckStates(rootNode, ZetaLib.Windows.Controls.TriStateCheckBoxesTreeView.NodesCheckState.UpdateStateFromChilds);
            tvPlusList.EndUpdate();
            ShowInfo(false);
        }

        private void FormPlusManager_Load(object sender, System.EventArgs e)
        {
            this.tvPlusList.ExpandAll();
            this.tvPlusList.SelectedNode = this.rootNode;
        }

        private void FormPlusManager_Closed(object sender, System.EventArgs e)
        {
            PersistState();
        }

        private void PersistState()
        {
            if (modified && MessageBoxFactory.Confirm("Do you want to apply your changes?") == DialogResult.Yes)
            {
                UpdateTag(rootNode);
                Plus2Registry.GeneratePlus2Files();
                LoggingService.Info("modified so save pluses");
                modified = false;
            }
        }

        private void UpdateTag(TreeNode node)
        {
            System.Diagnostics.Trace.Assert(node != null);
            if (node.Tag is Feature2)
            {
                (node.Tag as Feature2).SetEnabled(GetIdeVersion(),
                    tvPlusList.GetNodeCheckState(node) == CheckState.Checked);
            }
            if (node.Nodes.Count == 0)
            {
                return;
            }
            foreach (TreeNode child in node.Nodes)
            {
                UpdateTag(child);
            }
        }

        private void actSave_Update(object sender, EventArgs e)
        {
            actSave.Enabled = modified;
        }

        private void actSave_Execute(object sender, EventArgs e)
        {
            PersistState();
        }

        private void tvPlusList_AfterTriStateCheck(object sender, ZetaLib.Windows.Controls.TriStateCheckBoxesTreeView.TreeViewTriStateEventArgs args)
        {
            modified = true;
        }
    }
}