using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using EV.Windows.Forms;

namespace BeWise.SharpDevTools.Gui {

    public class FrmVersionInspector : System.Windows.Forms.Form
    {
        // *************************************************************************
        //                              Constructor
        // *************************************************************************

        public FrmVersionInspector() {
            InitializeComponent();
            LoadClientFileList();

            new ListViewSortManager(lvClientFiles,
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

        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.ListView lvClientFiles;
        private System.Windows.Forms.Button butOk;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;

        #region Windows Form Designer generated code
        private void InitializeComponent() {
            this.lvClientFiles = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.butOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvClientFiles
            // 
            this.lvClientFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                        | System.Windows.Forms.AnchorStyles.Left) 
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvClientFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                        this.columnHeader1,
                        this.columnHeader2});
            this.lvClientFiles.Location = new System.Drawing.Point(8, 8);
            this.lvClientFiles.MultiSelect = false;
            this.lvClientFiles.Name = "lvClientFiles";
            this.lvClientFiles.Size = new System.Drawing.Size(328, 282);
            this.lvClientFiles.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvClientFiles.TabIndex = 0;
            this.lvClientFiles.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 165;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Version";
            this.columnHeader2.Width = 140;
            // 
            // butOk
            // 
            this.butOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butOk.Location = new System.Drawing.Point(256, 297);
            this.butOk.Name = "butOk";
            this.butOk.TabIndex = 3;
            this.butOk.Text = "Ok";
            // 
            // FrmVersionInspector
            // 
            this.AcceptButton = this.butOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(346, 327);
            this.Controls.Add(this.butOk);
            this.Controls.Add(this.lvClientFiles);
            this.Name = "FrmVersionInspector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Version Inspector";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmAbout_KeyDown);
            this.ResumeLayout(false);
        }
        #endregion

        private void AddAssemblyInfo(ListView aListView, string aAssemblyName, string aVersion) {
            ListViewItem _ListViewItem = new ListViewItem();
            _ListViewItem.Text = aAssemblyName;
            _ListViewItem.SubItems.Add(aVersion);
            aListView.Items.Add(_ListViewItem);
        }

        private void LoadClientFileList() {
            AddAssemblyInfo(lvClientFiles,
                            Assembly.GetExecutingAssembly().GetName().Name,
                            Assembly.GetExecutingAssembly().GetName().Version.ToString());


            AssemblyName[] _AssemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();

            foreach (AssemblyName _AssemblyName in _AssemblyNames) {
                AddAssemblyInfo(lvClientFiles, _AssemblyName.Name, _AssemblyName.Version.ToString());
            }
        }

        private void FrmAbout_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) {
                this.Close();
            }
        }
    }
}
