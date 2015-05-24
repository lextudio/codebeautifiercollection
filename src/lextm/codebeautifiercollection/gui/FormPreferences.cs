// this is the form preferences class. Ported from SBT.
//  Some components are initialized elsewhere.
// Copyright (C) 2005-2006  Lex Y. Li
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

using Lextm.OpenTools.Gui;
using Lextm.OpenTools;
using Lextm.OpenTools.Elements;

namespace Lextm.CodeBeautifierCollection.Gui {  

    /// <summary>
    ///Preferences form.
    ///</summary>
	sealed class FormPreferences : System.Windows.Forms.Form, ITreeViewContainer {
        /// <summary>
        /// TreeView control.
        /// </summary>
        public Vista_Api.TreeView TreeView {
            get {
                return TreeView1;
            }
            set {
                TreeView1 = value;
            }
        }
        
        private FormPreferences() {
            InitializeComponent();
        }

        ///<summary>
        ///Cleans up any resources being used.
        ///</summary>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
#region Forms
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Panel panel1;
        private SplitContainer scPages;
        private System.ComponentModel.Container components = null;

        private void InitializeComponent()
		{
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.scPages = new System.Windows.Forms.SplitContainer();
            this.TreeView1 = new Vista_Api.TreeView();
            this.panel1.SuspendLayout();
            this.scPages.Panel1.SuspendLayout();
            this.scPages.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(592, 422);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(88, 25);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancel";
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(496, 422);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(88, 25);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "OK";
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.scPages);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(696, 414);
            this.panel1.TabIndex = 2;
            // 
            // scPages
            // 
            this.scPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scPages.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scPages.IsSplitterFixed = true;
            this.scPages.Location = new System.Drawing.Point(0, 0);
            this.scPages.Name = "scPages";
            // 
            // scPages.Panel1
            // 
            this.scPages.Panel1.Controls.Add(this.TreeView1);
            this.scPages.Size = new System.Drawing.Size(696, 414);
            this.scPages.SplitterDistance = 207;
            this.scPages.TabIndex = 0;
            // 
            // TreeView1
            // 
            this.TreeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeView1.Location = new System.Drawing.Point(0, 0);
            this.TreeView1.Name = "TreeView1";
            this.TreeView1.Size = new System.Drawing.Size(207, 414);
            this.TreeView1.TabIndex = 0;
            this.TreeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView1_AfterSelect);
            // 
            // FormPreferences
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(700, 458);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPreferences";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Code Beautifier Collection Preferences";
            this.Load += new System.EventHandler(this.FormPreferences_Load);
            this.panel1.ResumeLayout(false);
            this.scPages.Panel1.ResumeLayout(false);
            this.scPages.ResumeLayout(false);
            this.ResumeLayout(false);

		}
#endregion

        private Vista_Api.TreeView TreeView1;

        private void FormPreferences_Load(object sender, System.EventArgs e) {
            
            LoggingService.EnterMethod();

            TreeView1.ExpandAll();

            LoggingService.LeaveMethod();
           
        }

        private void btOK_Click(object sender, System.EventArgs e) {
           
            LoggingService.EnterMethod();

            foreach(CustomPage page in scPages.Panel2.Controls) {
				page.UIToPreferences();
                LoggingService.Info("preferences saved from UI.");
            }

            Collections.FeatureBuilder.SavePreferences();
            Collections.FeatureBuilder.Refresh();
            ShortcutService.RefreshShortcuts();

            LoggingService.LeaveMethod();
            
        }

        ///<summary>Panel control on FormPreferences</summary>
        public System.Windows.Forms.Panel Panel {
            get {
                return panel1;
            }
        }

        private static FormPreferences instance;

        ///<summary>
        ///Singleton instance of the form.
        ///</summary>
        internal static FormPreferences getInstance( ) {
            lock (typeof(FormPreferences)) {
                if (instance == null) {
                    instance = new FormPreferences();
                }
            }
            return instance;
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e) {
            if (currentPage != null) {
                // hide current page.
                currentPage.Visible = false;
			}
			
            TabPageProxy record = e.Node.Tag as TabPageProxy;

            if (record != null) {
				CustomPage page = record.Instance;

				if (!scPages.Panel2.Contains(page)) {
					scPages.Panel2.Controls.Add(page);
					page.Location = new Point(0, 0);
					//page.Size = pnlPages.Size;
					page.Dock = DockStyle.Fill;
					page.Visible = false;
					page.PreferencesToUI();
					LoggingService.Info("Preferences loaded into UI.");
				}
				// show a new page.
				page.Visible = true; 
				currentPage = page;
            }            
        }

        private CustomPage currentPage;

    }
}
