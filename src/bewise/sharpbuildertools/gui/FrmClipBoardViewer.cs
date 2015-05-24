using System;
using System.Drawing;

using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using Borland.Studio.ToolsAPI;
using BeWise.Common.Utils;
using System.Collections.Generic;

namespace BeWise.SharpBuilderTools.Gui {

	class FrmClipboardViewer : Form {

        /**************************************************************/
        /*                    Constructor/ Destructor
        /**************************************************************/

		internal FrmClipboardViewer() {
            InitializeComponent();
            InitializeListView(lvItems);
        }

        protected override void Dispose (bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /**************************************************************/
        /*                         Private
        /**************************************************************/

        private IList<string> fItems;
        private System.Windows.Forms.Button btCopy;
        private System.Windows.Forms.TextBox txtValue;

        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private Vista_Api.ListView lvItems;
        private System.Windows.Forms.ImageList imageList;

        private void txtItem_TextChanged(object sender, System.EventArgs e) {
            LoadItemsInListView();
        }

        private void FrmBaseView_Enter(object sender, System.EventArgs e) {
            lvItems.Focus();
        }

        private void lvItems_SelectedIndexChanged(object sender, System.EventArgs e) {
            if (lvItems.SelectedItems.Count > 0) {
                txtValue.Text = (string) lvItems.SelectedItems[0].Tag;
            } else {
                txtValue.Text = "";
            }
        }

        private void btCopy_Click(object sender, System.EventArgs e) {
			if (lvItems.SelectedItems.Count > 0) {
                Clipboard.SetDataObject(lvItems.SelectedItems[0].Tag);
            }

            Close();
        }

        private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmClipboardViewer));
            this.btCancel = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.lvItems = new Vista_Api.ListView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.btCopy = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCancel.Location = new System.Drawing.Point(432, 304);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "Cancel";
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(8, 8);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(504, 288);
            this.tabControl.TabIndex = 0;
            this.tabControl.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtValue);
            this.tabPage1.Controls.Add(this.lvItems);
            this.tabPage1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(496, 262);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Clipboard";
            // 
            // txtValue
            // 
            this.txtValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtValue.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValue.Location = new System.Drawing.Point(8, 144);
            this.txtValue.Multiline = true;
            this.txtValue.Name = "txtValue";
            this.txtValue.ReadOnly = true;
            this.txtValue.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtValue.Size = new System.Drawing.Size(480, 112);
            this.txtValue.TabIndex = 1;
            this.txtValue.TextChanged += new System.EventHandler(this.txtItem_TextChanged);
            // 
            // lvItems
            // 
            this.lvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvItems.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvItems.FullRowSelect = true;
            this.lvItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvItems.HideSelection = false;
            this.lvItems.Location = new System.Drawing.Point(8, 8);
            this.lvItems.MultiSelect = false;
            this.lvItems.Name = "lvItems";
            this.lvItems.Size = new System.Drawing.Size(480, 128);
            this.lvItems.SmallImageList = this.imageList;
            this.lvItems.TabIndex = 0;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.SelectedIndexChanged += new System.EventHandler(this.lvItems_SelectedIndexChanged);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            // 
            // btCopy
            // 
            this.btCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCopy.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btCopy.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCopy.Location = new System.Drawing.Point(352, 304);
            this.btCopy.Name = "btCopy";
            this.btCopy.Size = new System.Drawing.Size(75, 23);
            this.btCopy.TabIndex = 1;
            this.btCopy.Text = "Copy";
            this.btCopy.Click += new System.EventHandler(this.btCopy_Click);
            // 
            // FrmClipboardViewer
            // 
            this.AcceptButton = this.btCopy;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(520, 334);
            this.Controls.Add(this.btCopy);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btCancel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 350);
            this.Name = "FrmClipboardViewer";
            this.Enter += new System.EventHandler(this.FrmBaseView_Enter);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

		}

        /**************************************************************/
        /*                       Protected
        /**************************************************************/

		private static ListViewItem CreateListViewItem(object aItem) {
            ListViewItem _ListViewItem = new ListViewItem();
			string str = (string) aItem;
			StringReader  _Reader = new StringReader(str);
            string _Line = _Reader.ReadLine();

			if (str.Length > _Line.Length) {
				_Line += " [...]";
			}

			_ListViewItem.Text = OtaUtils.CreatePresentationLineFromDirtyString(_Line);
			_ListViewItem.ImageIndex = 0;
			_ListViewItem.Tag = str;

            return _ListViewItem;
        }

		protected virtual void InitializeListView(Vista_Api.ListView aListView) {
            ColumnHeader _Col = new ColumnHeader();
            _Col.Text = "Names";
            _Col.Width = lvItems.Width -4;

            lvItems.Columns.Add(_Col);
            lvItems.View = View.Details;
        }

        /**************************************************************/
        /*                         Public
        /**************************************************************/

        private void LoadItemsInListView() {
            lvItems.BeginUpdate();
            try {
                lvItems.Items.Clear();

                for (int i = 0; i < fItems.Count; i ++) {
                    ListViewItem _Item = CreateListViewItem(fItems[i]);
                    lvItems.Items.Add(_Item);
                }
            } finally {
                lvItems.EndUpdate();
            }
        }

        /**************************************************************/
        /*                       Properties
        /**************************************************************/

        internal IList<string> Items {
            get {
                return fItems;
            }

            set {
                fItems = value;
                LoadItemsInListView();
            }
        }
		
		private void btCancel_Click(object sender, System.EventArgs e)
		{
            Close();
		}
    }
}
