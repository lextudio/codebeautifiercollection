using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BeWise.SharpDevTools.Gui {

    public class FrmObjectInspector : System.Windows.Forms.Form
    {
        // *************************************************************************
        //                              Constructor
        // *************************************************************************

        public FrmObjectInspector() {
            InitializeComponent();
            cbObjectList.Items.Clear();
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
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.ComboBox cbObjectList;
        private ArrayList fList = new ArrayList();

        private void cbObjectList_SelectedIndexChanged(object sender, System.EventArgs e) {
            if (cbObjectList.SelectedIndex < 0 || cbObjectList.SelectedIndex >= fList.Count) {
                propertyGrid.SelectedObject = null;
                return;
            }

            object[] _Obj = (object[]) fList[cbObjectList.SelectedIndex];
            propertyGrid.SelectedObject = _Obj[0];
        }

        #region Windows Form Designer generated code
        private void InitializeComponent() {
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.cbObjectList = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                        | System.Windows.Forms.AnchorStyles.Left) 
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.CommandsVisibleIfAvailable = true;
            this.propertyGrid.LargeButtons = false;
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid.Location = new System.Drawing.Point(8, 32);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGrid.Size = new System.Drawing.Size(232, 480);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.Text = "propertyGrid";
            this.propertyGrid.ViewBackColor = System.Drawing.SystemColors.Window;
            this.propertyGrid.ViewForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // cbObjectList
            // 
            this.cbObjectList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbObjectList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObjectList.Items.AddRange(new object[] {
                        "UIManager"});
            this.cbObjectList.Location = new System.Drawing.Point(8, 8);
            this.cbObjectList.Name = "cbObjectList";
            this.cbObjectList.Size = new System.Drawing.Size(232, 21);
            this.cbObjectList.TabIndex = 1;
            this.cbObjectList.SelectedIndexChanged += new System.EventHandler(this.cbObjectList_SelectedIndexChanged);
            // 
            // FrmObjectInspector
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(248, 517);
            this.Controls.Add(this.cbObjectList);
            this.Controls.Add(this.propertyGrid);
            this.Name = "FrmObjectInspector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Object Inspector";
            this.ResumeLayout(false);
        }
        #endregion

        // *************************************************************************
        //                              Protected
        // *************************************************************************
        protected void LoadObjects() {
            cbObjectList.Items.Clear();
            foreach (object[] _Obj in fList) {
                cbObjectList.Items.Add(_Obj[1].ToString());
            }

            if (fList.Count > 0) {
                cbObjectList.SelectedIndex = 0;
            }
        }

        // *************************************************************************
        //                              Public
        // *************************************************************************
        public void AddObject(object aObject, string aName) {
            fList.Add(new object[] {aObject, aName});
            LoadObjects();
        }
    }
}
