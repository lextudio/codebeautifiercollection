using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using EV.Windows.Forms;

namespace BeWise.SharpDevTools.Gui {

    public class FrmFormInspector : System.Windows.Forms.Form
    {
        // *************************************************************************
        //                              Constructor
        // *************************************************************************

        public FrmFormInspector() {
            InitializeComponent();
        }

        public FrmFormInspector(Control aRoot) : this () {
            fRoot = aRoot;
            LoadInTreeView(tvControls, fRoot);
            LoadInListView(lvControls, fRoot);

            new ListViewSortManager(lvControls,
                                    new Type[] {
                                        typeof(ListViewTextSort),
                                        typeof(ListViewTextSort)
                                    });

        }

        protected override void Dispose (bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        // *************************************************************************
        //                              Private
        // *************************************************************************
        private Hashtable fHashtable = new Hashtable();

        private Control fRoot;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.TreeView tvControls;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpTree;
        private System.Windows.Forms.TabPage tpList;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ListView lvControls;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;

        private int GetImageIndex(Control aControl) {
            string _Key = aControl.GetType().Name;

            if (fHashtable.Contains(_Key)) {
                return (int) fHashtable[_Key];
            } else {
                return AddImage(_Key, ToolboxBitmapAttribute.Default.GetImage(aControl.GetType()));
            }
        }

        #region Windows Form Designer generated code
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.tvControls = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpTree = new System.Windows.Forms.TabPage();
            this.tpList = new System.Windows.Forms.TabPage();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.lvControls = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tabControl.SuspendLayout();
            this.tpTree.SuspendLayout();
            this.tpList.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.CommandsVisibleIfAvailable = true;
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.LargeButtons = false;
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid.Location = new System.Drawing.Point(379, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGrid.Size = new System.Drawing.Size(206, 449);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.Text = "propertyGrid";
            this.propertyGrid.ViewBackColor = System.Drawing.SystemColors.Window;
            this.propertyGrid.ViewForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // tvControls
            // 
            this.tvControls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                        | System.Windows.Forms.AnchorStyles.Left) 
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvControls.ImageList = this.imageList;
            this.tvControls.Location = new System.Drawing.Point(8, 8);
            this.tvControls.Name = "tvControls";
            this.tvControls.Size = new System.Drawing.Size(352, 408);
            this.tvControls.TabIndex = 1;
            this.tvControls.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvForm_AfterSelect);
            // 
            // imageList
            // 
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpTree);
            this.tabControl.Controls.Add(this.tpList);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(376, 449);
            this.tabControl.TabIndex = 2;
            // 
            // tpTree
            // 
            this.tpTree.Controls.Add(this.tvControls);
            this.tpTree.Location = new System.Drawing.Point(4, 22);
            this.tpTree.Name = "tpTree";
            this.tpTree.Size = new System.Drawing.Size(368, 423);
            this.tpTree.TabIndex = 0;
            this.tpTree.Text = "Tree";
            // 
            // tpList
            // 
            this.tpList.Controls.Add(this.txtFilter);
            this.tpList.Controls.Add(this.lvControls);
            this.tpList.Location = new System.Drawing.Point(4, 22);
            this.tpList.Name = "tpList";
            this.tpList.Size = new System.Drawing.Size(368, 423);
            this.tpList.TabIndex = 1;
            this.tpList.Text = "List";
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.Location = new System.Drawing.Point(8, 10);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(352, 20);
            this.txtFilter.TabIndex = 3;
            this.txtFilter.Text = "";
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // lvControls
            // 
            this.lvControls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                        | System.Windows.Forms.AnchorStyles.Left) 
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvControls.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                        this.columnHeader1,
                        this.columnHeader2});
            this.lvControls.FullRowSelect = true;
            this.lvControls.HideSelection = false;
            this.lvControls.LargeImageList = this.imageList;
            this.lvControls.Location = new System.Drawing.Point(8, 40);
            this.lvControls.MultiSelect = false;
            this.lvControls.Name = "lvControls";
            this.lvControls.Size = new System.Drawing.Size(352, 368);
            this.lvControls.SmallImageList = this.imageList;
            this.lvControls.TabIndex = 0;
            this.lvControls.View = System.Windows.Forms.View.Details;
            this.lvControls.SelectedIndexChanged += new System.EventHandler(this.lvControls_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 185;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 145;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                        | System.Windows.Forms.AnchorStyles.Left) 
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.propertyGrid);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.tabControl);
            this.panel1.Location = new System.Drawing.Point(7, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(585, 449);
            this.panel1.TabIndex = 3;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(376, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 449);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // FrmFormInspector
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(600, 462);
            this.Controls.Add(this.panel1);
            this.Name = "FrmFormInspector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form Inspector";
            this.tabControl.ResumeLayout(false);
            this.tpTree.ResumeLayout(false);
            this.tpList.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        #endregion

        private bool IsFiltered(Control aControl, string aFilter){
            if (aFilter == "" || aFilter == null) {
                return false;
            }

            if (aControl.Name == "" || aControl.Name == null) {
                return true;
            }

            return (aControl.Name.IndexOf(aFilter) < 0);
        }

        private void LoadListViewItems(ListView aListView, Control aControl, string aFilter) {
            if (!IsFiltered(aControl, aFilter)) {
                ListViewItem _ListViewItem = new ListViewItem();
                _ListViewItem.Tag = aControl;

                if (aControl.Name == null || aControl.Name == "") {
                    _ListViewItem.Text = "(" + aControl.GetType().Name + ")";
                } else {
                    _ListViewItem.Text = aControl.Name;
                }

                _ListViewItem.SubItems.Add(aControl.GetType().Name);

                _ListViewItem.ImageIndex = GetImageIndex(aControl);
                aListView.Items.Add(_ListViewItem);
            }
            
            foreach (Control _Child in aControl.Controls) {
                LoadListViewItems(aListView, _Child, aFilter);
            }
        }

        private TreeNode LoadTreeNode(Control aControl, TreeView aTreeView) {
            TreeNode _TreeNode = new TreeNode();
            _TreeNode.Tag = aControl;

            if (aControl.Name == null || aControl.Name == "") {
                _TreeNode.Text = "(" + aControl.GetType().Name + ")";
            } else {
                _TreeNode.Text = aControl.Name;
            }

            _TreeNode.ImageIndex = GetImageIndex(aControl);
            _TreeNode.SelectedImageIndex = _TreeNode.ImageIndex;

            foreach (Control _Child in aControl.Controls) {
                TreeNode _ChildTreeNode = LoadTreeNode(_Child, aTreeView);

                _TreeNode.Nodes.Add(_ChildTreeNode);
            }

            return _TreeNode;
        }

        private void lvControls_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lvControls.SelectedItems.Count == 0) {
                propertyGrid.SelectedObjects = null;
            } else {
                propertyGrid.SelectedObject = lvControls.SelectedItems[0].Tag;
            }

        }

        private void txtFilter_TextChanged(object sender, System.EventArgs e)
        {
            LoadInListView(lvControls, fRoot, txtFilter.Text);
        }

        private void tvForm_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e) {
            if (tvControls.SelectedNode == null) {
                propertyGrid.SelectedObjects = null;
            } else {
                propertyGrid.SelectedObject = tvControls.SelectedNode.Tag;
            }
        }

        /**************************************************************/
        /*                      Protected
        /**************************************************************/
        protected int AddImage(string aKey, Image aImage) {
            if (fHashtable.Contains(aKey)) {
                throw new Exception("Key " + aKey + " is already loaded");
            } else {
                imageList.Images.Add(aImage);
                int _Index = imageList.Images.Count - 1;
                fHashtable.Add(aKey, _Index);
                return _Index;
            }
        }

        /**************************************************************/
        /*                      Public
        /**************************************************************/

        public void LoadInTreeView(TreeView aTreeView, Control aControl) {
            aTreeView.Nodes.Clear();

            if (aControl != null) {
                Control _Root = aControl;
                TreeNode _TreeNode = LoadTreeNode(_Root, aTreeView);
                aTreeView.Nodes.Add(_TreeNode);
            }
        }

        public void LoadInListView(ListView aListView, Control aControl) {
            LoadInListView(aListView, aControl, "");
        }

        public void LoadInListView(ListView aListView, Control aControl, string aFilter) {
            aListView.Items.Clear();

            if (aControl != null) {
                Control _Root = aControl;
                LoadListViewItems(aListView, _Root, aFilter);
            }
        }

        /**************************************************************/
        /*                      Properties
        /**************************************************************/
        public Control Root {
            get {
                return fRoot;
            }
        }
    }
}
