using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Borland.Studio.ToolsAPI;
using System.Collections.Generic;

namespace BeWise.SharpBuilderTools.Gui {

	class FrmSaveFiles : Form {
        // *************************************************************************
        //                              Constructor
        // *************************************************************************
		internal FrmSaveFiles() {
            InitializeComponent();
        }

        protected override void Dispose (bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        // *************************************************************************
        //                              Private
        // *************************************************************************
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.CheckedListBox chklbModules;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btCancel;
        private IList<IOTAModule> fModules = new List<IOTAModule>();
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox1;

#region Windows Form Designer generated code

        private void InitializeComponent()
		{
			this.btSave = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.chklbModules = new System.Windows.Forms.CheckedListBox();
			this.lblSubTitle = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.SuspendLayout();
			// 
			// btSave
			// 
			this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btSave.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btSave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btSave.Location = new System.Drawing.Point(368, 341);
			this.btSave.Name = "btSave";
			this.btSave.TabIndex = 1;
			this.btSave.Text = "Save";
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btCancel.Location = new System.Drawing.Point(448, 341);
			this.btCancel.Name = "btCancel";
			this.btCancel.TabIndex = 2;
			this.btCancel.Text = "Cancel";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(0, 325);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(536, 8);
			this.groupBox2.TabIndex = 6;
			this.groupBox2.TabStop = false;
			// 
			// chklbModules
			// 
			this.chklbModules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.chklbModules.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chklbModules.Location = new System.Drawing.Point(8, 72);
			this.chklbModules.Name = "chklbModules";
			this.chklbModules.Size = new System.Drawing.Size(512, 244);
			this.chklbModules.TabIndex = 0;
			// 
			// lblSubTitle
			// 
			this.lblSubTitle.BackColor = System.Drawing.Color.White;
			this.lblSubTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSubTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSubTitle.ForeColor = System.Drawing.Color.Black;
			this.lblSubTitle.Location = new System.Drawing.Point(0, 24);
			this.lblSubTitle.Name = "lblSubTitle";
			this.lblSubTitle.Size = new System.Drawing.Size(528, 32);
			this.lblSubTitle.TabIndex = 4;
			this.lblSubTitle.Text = "               Check the file that you want to save";
			// 
			// lblTitle
			// 
			this.lblTitle.BackColor = System.Drawing.Color.White;
			this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.ForeColor = System.Drawing.Color.Black;
			this.lblTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(528, 24);
			this.lblTitle.TabIndex = 3;
			this.lblTitle.Text = "    Save Files";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(0, 52);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(536, 8);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			// 
			// FrmSaveFiles
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(528, 371);
			this.Controls.Add(this.lblSubTitle);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.chklbModules);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.btSave);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "FrmSaveFiles";
			this.Load += new System.EventHandler(this.FrmSaveFiles_Load);
			this.ResumeLayout(false);
		}
#endregion

        private void FrmSaveFiles_Load(object sender, System.EventArgs e) {
            chklbModules.Items.Clear();

            foreach (IOTAModule _Module in Modules) {
                SaveModuleInfo _SaveModuleInfo = new SaveModuleInfo(_Module);
                chklbModules.Items.Add(_SaveModuleInfo, true);
            }
        }

        // *************************************************************************
        //                              Public
        // *************************************************************************
        internal IOTAModule[] GetModuleToSave() {
            IOTAModule[] _List = new IOTAModule[chklbModules.CheckedItems.Count];

            for (int i = 0; i < chklbModules.CheckedItems.Count; i ++) {
                SaveModuleInfo _SaveModuleInfo = (SaveModuleInfo) chklbModules.CheckedItems[i];
                _List[i] = _SaveModuleInfo.module;
            }

            return _List;
        }

        // *************************************************************************
        //                              Properties
        // *************************************************************************
		internal IList<IOTAModule> Modules {
            get {
				return fModules;
            }
        }

        // *************************************************************************
        //                              Classes
        // *************************************************************************
        private class SaveModuleInfo {
            // *************************************************************************
            //                              Constructor
            // *************************************************************************

			internal SaveModuleInfo (IOTAModule aModule) {
                module = aModule;
            }

            // *************************************************************************
            //                             Public
            // *************************************************************************

			public override string ToString() {
                return module.FileName;
            }

            internal IOTAModule module;
        }

    }
}
