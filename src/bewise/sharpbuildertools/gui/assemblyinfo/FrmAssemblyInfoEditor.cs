using System;

using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Borland.Studio.ToolsAPI;
using EV.Windows.Forms;
using BeWise.Common;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;
using BeWise.SharpBuilderTools.Info;
using BeWise.SharpBuilderTools.Tools.AssemblyInfo;
using System.Globalization;
using System.Collections.Generic;

namespace BeWise.SharpBuilderTools.Gui.AssemblyInfo {

	class FrmAssemblyInfoEditor : System.Windows.Forms.Form {

        // *************************************************************************
        //                                Enum
        // *************************************************************************
        private enum ModeEnum {Default = 1, Edit, New}

        // *************************************************************************
        //                              Constructor
        // *************************************************************************
        internal FrmAssemblyInfoEditor()
        {
            InitializeComponent();

            new ListViewSortManager(lvFiles,
                                    new Type[] {
                                        typeof(ListViewTextSort),
                                        typeof(ListViewTextSort),
                                        typeof(ListViewInt32Sort)
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
        private ModeEnum fMode;

        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpEdition;
        private System.Windows.Forms.Button btGenerate;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label1;
        private Vista_Api.ListView lvFiles;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtConfiguration;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.NumericUpDown nuIndex;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btNew;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCopyright;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtTrademark;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtCulture;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtKeyFile;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.CheckBox chkDelaySign;
        private Vista_Api.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btEdit;
        private System.Windows.Forms.Button btCancelEdit;
        private Vista_Api.OpenFileDialog openFileDialog;
        private IList<AssemblyInfoDef> fList = new List<AssemblyInfoDef>();
        private System.Windows.Forms.Label lblInputFileName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;


        private void btAdd_Click(object sender, System.EventArgs e) {
            IOTAProject _Project = OtaUtils.GetCurrentProject();

            if (_Project != null) {
                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    LoadAssemblyInfoDefFromFile(openFileDialog.FileName);
                    _Project.AddFile(openFileDialog.FileName);
                    LoadAssemblyInfoDefsInListView();
                }
            }
        }

        private void btCancelEdit_Click(object sender, System.EventArgs e) {
            Mode = ModeEnum.Default;
            LoadAssemblyInfoDefInControls();
        }

        private void btDelete_Click(object sender, System.EventArgs e) {
            //OTAUtils.RemoveFromProject(null, CurrentAsemblyInfoDef.InputFileName);
            LoadAssemblyInfoDefsInListView();
        }

        private void btEdit_Click(object sender, System.EventArgs e) {
            Mode = ModeEnum.Edit;
            EnableControls();
        }

        private void btGenerate_Click(object sender, System.EventArgs e) {
            string[] _Files = new string[fList.Count];

            for(int i = 0; i < fList.Count; i++) {
                _Files[i] = fList[i].InputFileName;
            }

            Lextm.NFamily.Feature.AssemblyInfoEditorFeature.Generate(_Files);
        }

        private void btNew_Click(object sender, System.EventArgs e) {
            IOTAProject _Project = OtaUtils.GetCurrentProject();

            if (_Project != null) {
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    string _FileName = saveFileDialog.FileName;
                    
                    if (!_FileName.EndsWith(AID_FILE_EXTENSION, StringComparison.OrdinalIgnoreCase)) {
                        _FileName += AID_FILE_EXTENSION;
                    }

                    AssemblyInfoDef _AssemblyInfoDef = new AssemblyInfoDef();
                    fList.Add(_AssemblyInfoDef);
                    _Project.AddFile(_FileName);
                    SaveAssemblyInfoDefInFile(_FileName, _AssemblyInfoDef);
                    _AssemblyInfoDef.InputFileName = _FileName;
                    Mode = ModeEnum.New;
                    txtFileName.Text = _FileName;
                    LoadAssemblyInfoDefsInListView();
                    SelectListViewItem(_AssemblyInfoDef);
                }
            }
        }

        private void btSave_Click(object sender, System.EventArgs e) {
            Mode = ModeEnum.Default;
            SaveAssemblyDefFromControls();
            SaveAssemblyInfoDefInFile(CurrentAsemblyInfoDef.InputFileName, CurrentAsemblyInfoDef);
            EnableControls();
        }

        private void EnableControls(){
            btCancel.Enabled = true;
            tpEdition.Enabled = true;
            btOk.Enabled = true;
            lvFiles.Enabled = Mode == ModeEnum.Default;

            btGenerate.Enabled = (fList.Count > 0);

            txtTitle.Enabled = (Mode != ModeEnum.Default);
            txtDescription.Enabled = (Mode != ModeEnum.Default);
            txtFileName.Enabled = (Mode != ModeEnum.Default);
            txtConfiguration.Enabled = (Mode != ModeEnum.Default);
            txtCompany.Enabled = (Mode != ModeEnum.Default);
            txtProduct.Enabled = (Mode != ModeEnum.Default);
            nuIndex.Enabled = (Mode != ModeEnum.Default);
            btNew.Enabled = (Mode == ModeEnum.Default);
            txtCopyright.Enabled = (Mode != ModeEnum.Default);
            txtTrademark.Enabled = (Mode != ModeEnum.Default);
            txtCulture.Enabled = (Mode != ModeEnum.Default);
            txtVersion.Enabled = (Mode != ModeEnum.Default);
            txtKeyFile.Enabled = (Mode != ModeEnum.Default);
            txtKey.Enabled = (Mode != ModeEnum.Default);
            chkDelaySign.Enabled = (Mode != ModeEnum.Default);
            btSave.Enabled = (Mode != ModeEnum.Default);
            btCancelEdit.Enabled = (Mode != ModeEnum.Default);

            btAdd.Enabled = (Mode == ModeEnum.Default);
            btDelete.Enabled = (Mode == ModeEnum.Default);
            btEdit.Enabled = (Mode == ModeEnum.Default);
        }

        private void FrmAssemblyInfoEditor_Load(object sender, System.EventArgs e) {
            LoadAssemblyInfoDefsFromFiles();
            LoadAssemblyInfoDefsInListView();
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
		{
        	this.btCancel = new System.Windows.Forms.Button();
        	this.tabControl = new System.Windows.Forms.TabControl();
        	this.tpEdition = new System.Windows.Forms.TabPage();
        	this.lblInputFileName = new System.Windows.Forms.Label();
        	this.btCancelEdit = new System.Windows.Forms.Button();
        	this.btSave = new System.Windows.Forms.Button();
        	this.btEdit = new System.Windows.Forms.Button();
        	this.chkDelaySign = new System.Windows.Forms.CheckBox();
        	this.label13 = new System.Windows.Forms.Label();
        	this.txtKey = new System.Windows.Forms.TextBox();
        	this.label12 = new System.Windows.Forms.Label();
        	this.txtKeyFile = new System.Windows.Forms.TextBox();
        	this.label11 = new System.Windows.Forms.Label();
        	this.txtVersion = new System.Windows.Forms.TextBox();
        	this.label10 = new System.Windows.Forms.Label();
        	this.txtCulture = new System.Windows.Forms.TextBox();
        	this.label9 = new System.Windows.Forms.Label();
        	this.txtTrademark = new System.Windows.Forms.TextBox();
        	this.label8 = new System.Windows.Forms.Label();
        	this.txtCopyright = new System.Windows.Forms.TextBox();
        	this.groupBox1 = new System.Windows.Forms.GroupBox();
        	this.nuIndex = new System.Windows.Forms.NumericUpDown();
        	this.label7 = new System.Windows.Forms.Label();
        	this.label3 = new System.Windows.Forms.Label();
        	this.txtFileName = new System.Windows.Forms.TextBox();
        	this.btAdd = new System.Windows.Forms.Button();
        	this.btDelete = new System.Windows.Forms.Button();
        	this.btNew = new System.Windows.Forms.Button();
        	this.label6 = new System.Windows.Forms.Label();
        	this.txtProduct = new System.Windows.Forms.TextBox();
        	this.label5 = new System.Windows.Forms.Label();
        	this.txtCompany = new System.Windows.Forms.TextBox();
        	this.label4 = new System.Windows.Forms.Label();
        	this.txtConfiguration = new System.Windows.Forms.TextBox();
        	this.label2 = new System.Windows.Forms.Label();
        	this.txtDescription = new System.Windows.Forms.TextBox();
        	this.lvFiles = new Vista_Api.ListView();
        	this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
        	this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
        	this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
        	this.label1 = new System.Windows.Forms.Label();
        	this.txtTitle = new System.Windows.Forms.TextBox();
        	this.btGenerate = new System.Windows.Forms.Button();
        	this.btOk = new System.Windows.Forms.Button();
        	this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
        	this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        	this.openFileDialog = new Vista_Api.OpenFileDialog();
        	this.saveFileDialog = new Vista_Api.SaveFileDialog();
        	this.tabControl.SuspendLayout();
        	this.tpEdition.SuspendLayout();
        	this.groupBox1.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.nuIndex)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// btCancel
        	// 
        	this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        	this.btCancel.Location = new System.Drawing.Point(538, 551);
        	this.btCancel.Name = "btCancel";
        	this.btCancel.Size = new System.Drawing.Size(96, 26);
        	this.btCancel.TabIndex = 0;
        	this.btCancel.Text = "Cancel";
        	// 
        	// tabControl
        	// 
        	this.tabControl.Controls.Add(this.tpEdition);
        	this.tabControl.Location = new System.Drawing.Point(10, 9);
        	this.tabControl.Name = "tabControl";
        	this.tabControl.SelectedIndex = 0;
        	this.tabControl.Size = new System.Drawing.Size(739, 534);
        	this.tabControl.TabIndex = 1;
        	// 
        	// tpEdition
        	// 
        	this.tpEdition.Controls.Add(this.lblInputFileName);
        	this.tpEdition.Controls.Add(this.btCancelEdit);
        	this.tpEdition.Controls.Add(this.btSave);
        	this.tpEdition.Controls.Add(this.btEdit);
        	this.tpEdition.Controls.Add(this.chkDelaySign);
        	this.tpEdition.Controls.Add(this.label13);
        	this.tpEdition.Controls.Add(this.txtKey);
        	this.tpEdition.Controls.Add(this.label12);
        	this.tpEdition.Controls.Add(this.txtKeyFile);
        	this.tpEdition.Controls.Add(this.label11);
        	this.tpEdition.Controls.Add(this.txtVersion);
        	this.tpEdition.Controls.Add(this.label10);
        	this.tpEdition.Controls.Add(this.txtCulture);
        	this.tpEdition.Controls.Add(this.label9);
        	this.tpEdition.Controls.Add(this.txtTrademark);
        	this.tpEdition.Controls.Add(this.label8);
        	this.tpEdition.Controls.Add(this.txtCopyright);
        	this.tpEdition.Controls.Add(this.groupBox1);
        	this.tpEdition.Controls.Add(this.btAdd);
        	this.tpEdition.Controls.Add(this.btDelete);
        	this.tpEdition.Controls.Add(this.btNew);
        	this.tpEdition.Controls.Add(this.label6);
        	this.tpEdition.Controls.Add(this.txtProduct);
        	this.tpEdition.Controls.Add(this.label5);
        	this.tpEdition.Controls.Add(this.txtCompany);
        	this.tpEdition.Controls.Add(this.label4);
        	this.tpEdition.Controls.Add(this.txtConfiguration);
        	this.tpEdition.Controls.Add(this.label2);
        	this.tpEdition.Controls.Add(this.txtDescription);
        	this.tpEdition.Controls.Add(this.lvFiles);
        	this.tpEdition.Controls.Add(this.label1);
        	this.tpEdition.Controls.Add(this.txtTitle);
        	this.tpEdition.Location = new System.Drawing.Point(4, 22);
        	this.tpEdition.Name = "tpEdition";
        	this.tpEdition.Size = new System.Drawing.Size(731, 508);
        	this.tpEdition.TabIndex = 0;
        	this.tpEdition.Text = "Edit";
        	// 
        	// lblInputFileName
        	// 
        	this.lblInputFileName.Location = new System.Drawing.Point(10, 474);
        	this.lblInputFileName.Name = "lblInputFileName";
        	this.lblInputFileName.Size = new System.Drawing.Size(700, 26);
        	this.lblInputFileName.TabIndex = 35;
        	// 
        	// btCancelEdit
        	// 
        	this.btCancelEdit.Location = new System.Drawing.Point(624, 276);
        	this.btCancelEdit.Name = "btCancelEdit";
        	this.btCancelEdit.Size = new System.Drawing.Size(96, 26);
        	this.btCancelEdit.TabIndex = 34;
        	this.btCancelEdit.Text = "Cancel Edit";
        	this.btCancelEdit.Click += new System.EventHandler(this.btCancelEdit_Click);
        	// 
        	// btSave
        	// 
        	this.btSave.Location = new System.Drawing.Point(624, 241);
        	this.btSave.Name = "btSave";
        	this.btSave.Size = new System.Drawing.Size(96, 26);
        	this.btSave.TabIndex = 33;
        	this.btSave.Text = "Save";
        	this.btSave.Click += new System.EventHandler(this.btSave_Click);
        	// 
        	// btEdit
        	// 
        	this.btEdit.Location = new System.Drawing.Point(624, 207);
        	this.btEdit.Name = "btEdit";
        	this.btEdit.Size = new System.Drawing.Size(96, 26);
        	this.btEdit.TabIndex = 32;
        	this.btEdit.Text = "Edit";
        	this.btEdit.Click += new System.EventHandler(this.btEdit_Click);
        	// 
        	// chkDelaySign
        	// 
        	this.chkDelaySign.Checked = true;
        	this.chkDelaySign.CheckState = System.Windows.Forms.CheckState.Checked;
        	this.chkDelaySign.Location = new System.Drawing.Point(336, 422);
        	this.chkDelaySign.Name = "chkDelaySign";
        	this.chkDelaySign.Size = new System.Drawing.Size(154, 17);
        	this.chkDelaySign.TabIndex = 31;
        	this.chkDelaySign.Text = "Delay sign";
        	// 
        	// label13
        	// 
        	this.label13.Location = new System.Drawing.Point(307, 388);
        	this.label13.Name = "label13";
        	this.label13.Size = new System.Drawing.Size(58, 17);
        	this.label13.TabIndex = 30;
        	this.label13.Text = "Key name";
        	// 
        	// txtKey
        	// 
        	this.txtKey.Location = new System.Drawing.Point(394, 388);
        	this.txtKey.Name = "txtKey";
        	this.txtKey.Size = new System.Drawing.Size(120, 21);
        	this.txtKey.TabIndex = 29;
        	// 
        	// label12
        	// 
        	this.label12.Location = new System.Drawing.Point(307, 353);
        	this.label12.Name = "label12";
        	this.label12.Size = new System.Drawing.Size(58, 17);
        	this.label12.TabIndex = 28;
        	this.label12.Text = "Key file";
        	// 
        	// txtKeyFile
        	// 
        	this.txtKeyFile.Location = new System.Drawing.Point(394, 353);
        	this.txtKeyFile.Name = "txtKeyFile";
        	this.txtKeyFile.Size = new System.Drawing.Size(120, 21);
        	this.txtKeyFile.TabIndex = 27;
        	// 
        	// label11
        	// 
        	this.label11.Location = new System.Drawing.Point(307, 319);
        	this.label11.Name = "label11";
        	this.label11.Size = new System.Drawing.Size(58, 17);
        	this.label11.TabIndex = 26;
        	this.label11.Text = "Version";
        	// 
        	// txtVersion
        	// 
        	this.txtVersion.Location = new System.Drawing.Point(394, 319);
        	this.txtVersion.Name = "txtVersion";
        	this.txtVersion.Size = new System.Drawing.Size(120, 21);
        	this.txtVersion.TabIndex = 25;
        	// 
        	// label10
        	// 
        	this.label10.Location = new System.Drawing.Point(307, 284);
        	this.label10.Name = "label10";
        	this.label10.Size = new System.Drawing.Size(58, 18);
        	this.label10.TabIndex = 24;
        	this.label10.Text = "Culture";
        	// 
        	// txtCulture
        	// 
        	this.txtCulture.Location = new System.Drawing.Point(394, 284);
        	this.txtCulture.Name = "txtCulture";
        	this.txtCulture.Size = new System.Drawing.Size(120, 21);
        	this.txtCulture.TabIndex = 23;
        	// 
        	// label9
        	// 
        	this.label9.Location = new System.Drawing.Point(307, 250);
        	this.label9.Name = "label9";
        	this.label9.Size = new System.Drawing.Size(77, 17);
        	this.label9.TabIndex = 22;
        	this.label9.Text = "Trademark";
        	// 
        	// txtTrademark
        	// 
        	this.txtTrademark.Location = new System.Drawing.Point(394, 250);
        	this.txtTrademark.Name = "txtTrademark";
        	this.txtTrademark.Size = new System.Drawing.Size(220, 21);
        	this.txtTrademark.TabIndex = 21;
        	// 
        	// label8
        	// 
        	this.label8.Location = new System.Drawing.Point(307, 215);
        	this.label8.Name = "label8";
        	this.label8.Size = new System.Drawing.Size(77, 18);
        	this.label8.TabIndex = 20;
        	this.label8.Text = "Copyright";
        	// 
        	// txtCopyright
        	// 
        	this.txtCopyright.Location = new System.Drawing.Point(394, 215);
        	this.txtCopyright.Name = "txtCopyright";
        	this.txtCopyright.Size = new System.Drawing.Size(220, 21);
        	this.txtCopyright.TabIndex = 19;
        	// 
        	// groupBox1
        	// 
        	this.groupBox1.Controls.Add(this.nuIndex);
        	this.groupBox1.Controls.Add(this.label7);
        	this.groupBox1.Controls.Add(this.label3);
        	this.groupBox1.Controls.Add(this.txtFileName);
        	this.groupBox1.Location = new System.Drawing.Point(10, 129);
        	this.groupBox1.Name = "groupBox1";
        	this.groupBox1.Size = new System.Drawing.Size(710, 69);
        	this.groupBox1.TabIndex = 18;
        	this.groupBox1.TabStop = false;
        	// 
        	// nuIndex
        	// 
        	this.nuIndex.Location = new System.Drawing.Point(374, 26);
        	this.nuIndex.Name = "nuIndex";
        	this.nuIndex.Size = new System.Drawing.Size(144, 21);
        	this.nuIndex.TabIndex = 13;
        	// 
        	// label7
        	// 
        	this.label7.Location = new System.Drawing.Point(288, 29);
        	this.label7.Name = "label7";
        	this.label7.Size = new System.Drawing.Size(58, 17);
        	this.label7.TabIndex = 14;
        	this.label7.Text = "Index";
        	// 
        	// label3
        	// 
        	this.label3.Location = new System.Drawing.Point(10, 26);
        	this.label3.Name = "label3";
        	this.label3.Size = new System.Drawing.Size(115, 17);
        	this.label3.TabIndex = 6;
        	this.label3.Text = "Output File Name";
        	// 
        	// txtFileName
        	// 
        	this.txtFileName.Location = new System.Drawing.Point(134, 26);
        	this.txtFileName.Name = "txtFileName";
        	this.txtFileName.Size = new System.Drawing.Size(120, 21);
        	this.txtFileName.TabIndex = 5;
        	// 
        	// btAdd
        	// 
        	this.btAdd.Location = new System.Drawing.Point(624, 43);
        	this.btAdd.Name = "btAdd";
        	this.btAdd.Size = new System.Drawing.Size(96, 26);
        	this.btAdd.TabIndex = 17;
        	this.btAdd.Text = "Add";
        	this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
        	// 
        	// btDelete
        	// 
        	this.btDelete.Location = new System.Drawing.Point(624, 78);
        	this.btDelete.Name = "btDelete";
        	this.btDelete.Size = new System.Drawing.Size(96, 25);
        	this.btDelete.TabIndex = 16;
        	this.btDelete.Text = "Delete";
        	this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
        	// 
        	// btNew
        	// 
        	this.btNew.Location = new System.Drawing.Point(624, 9);
        	this.btNew.Name = "btNew";
        	this.btNew.Size = new System.Drawing.Size(96, 25);
        	this.btNew.TabIndex = 15;
        	this.btNew.Text = "New";
        	this.btNew.Click += new System.EventHandler(this.btNew_Click);
        	// 
        	// label6
        	// 
        	this.label6.Location = new System.Drawing.Point(19, 353);
        	this.label6.Name = "label6";
        	this.label6.Size = new System.Drawing.Size(58, 17);
        	this.label6.TabIndex = 12;
        	this.label6.Text = "Product";
        	// 
        	// txtProduct
        	// 
        	this.txtProduct.Location = new System.Drawing.Point(106, 353);
        	this.txtProduct.Name = "txtProduct";
        	this.txtProduct.Size = new System.Drawing.Size(192, 21);
        	this.txtProduct.TabIndex = 11;
        	// 
        	// label5
        	// 
        	this.label5.Location = new System.Drawing.Point(19, 319);
        	this.label5.Name = "label5";
        	this.label5.Size = new System.Drawing.Size(77, 17);
        	this.label5.TabIndex = 10;
        	this.label5.Text = "Company";
        	// 
        	// txtCompany
        	// 
        	this.txtCompany.Location = new System.Drawing.Point(106, 319);
        	this.txtCompany.Name = "txtCompany";
        	this.txtCompany.Size = new System.Drawing.Size(192, 21);
        	this.txtCompany.TabIndex = 9;
        	// 
        	// label4
        	// 
        	this.label4.Location = new System.Drawing.Point(19, 284);
        	this.label4.Name = "label4";
        	this.label4.Size = new System.Drawing.Size(87, 18);
        	this.label4.TabIndex = 8;
        	this.label4.Text = "Configuration";
        	// 
        	// txtConfiguration
        	// 
        	this.txtConfiguration.Location = new System.Drawing.Point(106, 284);
        	this.txtConfiguration.Name = "txtConfiguration";
        	this.txtConfiguration.Size = new System.Drawing.Size(192, 21);
        	this.txtConfiguration.TabIndex = 7;
        	// 
        	// label2
        	// 
        	this.label2.Location = new System.Drawing.Point(19, 250);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(77, 17);
        	this.label2.TabIndex = 4;
        	this.label2.Text = "Description";
        	// 
        	// txtDescription
        	// 
        	this.txtDescription.Location = new System.Drawing.Point(106, 250);
        	this.txtDescription.Name = "txtDescription";
        	this.txtDescription.Size = new System.Drawing.Size(192, 21);
        	this.txtDescription.TabIndex = 3;
        	// 
        	// lvFiles
        	// 
        	this.lvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
        	        	        	this.columnHeader1,
        	        	        	this.columnHeader2,
        	        	        	this.columnHeader3});
        	this.lvFiles.FullRowSelect = true;
        	this.lvFiles.HideSelection = false;
        	this.lvFiles.Location = new System.Drawing.Point(10, 9);
        	this.lvFiles.MultiSelect = false;
        	this.lvFiles.Name = "lvFiles";
        	this.lvFiles.Size = new System.Drawing.Size(604, 112);
        	this.lvFiles.TabIndex = 2;
        	this.lvFiles.UseCompatibleStateImageBehavior = false;
        	this.lvFiles.View = System.Windows.Forms.View.Details;
        	this.lvFiles.Click += new System.EventHandler(this.lvFiles_Click);
        	// 
        	// columnHeader1
        	// 
        	this.columnHeader1.Text = "Path";
        	this.columnHeader1.Width = 300;
        	// 
        	// columnHeader2
        	// 
        	this.columnHeader2.Text = "File name";
        	this.columnHeader2.Width = 120;
        	// 
        	// columnHeader3
        	// 
        	this.columnHeader3.Text = "Index";
        	this.columnHeader3.Width = 65;
        	// 
        	// label1
        	// 
        	this.label1.Location = new System.Drawing.Point(19, 215);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(58, 18);
        	this.label1.TabIndex = 1;
        	this.label1.Text = "Title";
        	// 
        	// txtTitle
        	// 
        	this.txtTitle.Location = new System.Drawing.Point(106, 215);
        	this.txtTitle.Name = "txtTitle";
        	this.txtTitle.Size = new System.Drawing.Size(192, 21);
        	this.txtTitle.TabIndex = 0;
        	// 
        	// btGenerate
        	// 
        	this.btGenerate.Location = new System.Drawing.Point(643, 551);
        	this.btGenerate.Name = "btGenerate";
        	this.btGenerate.Size = new System.Drawing.Size(96, 26);
        	this.btGenerate.TabIndex = 2;
        	this.btGenerate.Text = "Generate";
        	this.btGenerate.Click += new System.EventHandler(this.btGenerate_Click);
        	// 
        	// btOk
        	// 
        	this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
        	this.btOk.Location = new System.Drawing.Point(432, 551);
        	this.btOk.Name = "btOk";
        	this.btOk.Size = new System.Drawing.Size(96, 26);
        	this.btOk.TabIndex = 3;
        	this.btOk.Text = "Ok";
        	// 
        	// saveFileDialog1
        	// 
        	this.saveFileDialog1.Filter = "Assembly Info Def|*.aid";
        	// 
        	// openFileDialog1
        	// 
        	this.openFileDialog1.CheckFileExists = false;
        	this.openFileDialog1.Filter = "Assembly Info Def|*.aid";
        	// 
        	// openFileDialog
        	// 
        	this.openFileDialog.FileName = "openFileDialog2";
        	this.openFileDialog.Filter = null;
        	// 
        	// saveFileDialog
        	// 
        	this.saveFileDialog.Filter = null;
        	// 
        	// FrmAssemblyInfoEditor
        	// 
        	this.AcceptButton = this.btOk;
        	this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
        	this.CancelButton = this.btCancel;
        	this.ClientSize = new System.Drawing.Size(760, 583);
        	this.Controls.Add(this.btOk);
        	this.Controls.Add(this.tabControl);
        	this.Controls.Add(this.btCancel);
        	this.Controls.Add(this.btGenerate);
        	this.Font = new System.Drawing.Font("Tahoma", 8.25F);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        	this.MaximizeBox = false;
        	this.Name = "FrmAssemblyInfoEditor";
        	this.ShowInTaskbar = false;
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "Sharp Builder Tools - Assembly Info Editor / Runner";
        	this.Load += new System.EventHandler(this.FrmAssemblyInfoEditor_Load);
        	this.tabControl.ResumeLayout(false);
        	this.tpEdition.ResumeLayout(false);
        	this.tpEdition.PerformLayout();
        	this.groupBox1.ResumeLayout(false);
        	this.groupBox1.PerformLayout();
        	((System.ComponentModel.ISupportInitialize)(this.nuIndex)).EndInit();
        	this.ResumeLayout(false);
		}
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        #endregion


        private AssemblyInfoDef LoadAssemblyInfoDefFromFile(string aFileName) {
            if (Lextm.IO.FileHelper.FileIsValid(aFileName))
            {
                XmlSerializer _Serializer = new XmlSerializer(typeof(AssemblyInfoDef));

                FileStream _FileStream = File.OpenRead(aFileName);
                XmlReader _Reader = new XmlTextReader(_FileStream);
                try {
                    try {
                        AssemblyInfoDef _AssemblyInfoDef = (AssemblyInfoDef) _Serializer.Deserialize(_Reader);
                        if (_AssemblyInfoDef != null) {
                            _AssemblyInfoDef.InputFileName = aFileName;
                            fList.Add(_AssemblyInfoDef);
                        }
                        
                        return _AssemblyInfoDef;
                    } catch {
                       return null;
                    }
                } finally {
                    _Reader.Close();
                }
            }

            return null;
        }
        /// <summary>
        /// To be removed later.
        /// </summary>
		private const string AID_FILE_EXTENSION                                  = ".aid";

        private void LoadAssemblyInfoDefsFromFiles() {
            fList.Clear();
            foreach(string _File in OtaUtils.GetProjectFileList(AID_FILE_EXTENSION)) {
                LoadAssemblyInfoDefFromFile(_File);
            }
        }

        private void LoadAssemblyInfoDefInControls() {
            if (CurrentAsemblyInfoDef != null) {
                txtCompany.Text = CurrentAsemblyInfoDef.AssemblyCompany;
                txtConfiguration.Text = CurrentAsemblyInfoDef.AssemblyConfiguration;
                txtCopyright.Text = CurrentAsemblyInfoDef.AssemblyCopyright;
                txtCulture.Text = CurrentAsemblyInfoDef.AssemblyCulture;
                chkDelaySign.Checked = CurrentAsemblyInfoDef.AssemblyDelaySign;
                txtDescription.Text = CurrentAsemblyInfoDef.AssemblyDescription;
                txtFileName.Text = CurrentAsemblyInfoDef.OutputFileName;
                txtKeyFile.Text = CurrentAsemblyInfoDef.AssemblyKeyFile;
                txtKey.Text = CurrentAsemblyInfoDef.AssemblyKeyName;
                nuIndex.Value = CurrentAsemblyInfoDef.Index;
                txtProduct.Text = CurrentAsemblyInfoDef.AssemblyProduct;
                txtTitle.Text = CurrentAsemblyInfoDef.AssemblyTitle;
                txtTrademark.Text = CurrentAsemblyInfoDef.AssemblyTrademark;
                txtVersion.Text = CurrentAsemblyInfoDef.AssemblyVersion;
                lblInputFileName.Text = CurrentAsemblyInfoDef.InputFileName;
            } else {
                txtCompany.Text = "";
                txtConfiguration.Text = "";
                txtCopyright.Text = "";
                txtCulture.Text = "";
                chkDelaySign.Checked = false;
                txtDescription.Text = "";
                txtFileName.Text = "";
                txtKeyFile.Text = "";
                txtKey.Text = "";
                nuIndex.Value = 0;
                txtProduct.Text = "";
                txtTitle.Text = "";
                txtTrademark.Text = "";
                txtVersion.Text = "";
                lblInputFileName.Text = "";
            }
        }

        private void LoadAssemblyInfoDefsInListView() {
            lvFiles.Items.Clear();

            foreach(AssemblyInfoDef _AssemblyInfoDef in fList) {
                ListViewItem _ListViewItem = new ListViewItem();
                _ListViewItem.Text = Path.GetDirectoryName(_AssemblyInfoDef.InputFileName);
                _ListViewItem.SubItems.Add(Path.GetFileName(_AssemblyInfoDef.InputFileName));
                _ListViewItem.SubItems.Add(_AssemblyInfoDef.Index.ToString(CultureInfo.InvariantCulture));
                _ListViewItem.Tag = _AssemblyInfoDef; 

                lvFiles.Items.Add(_ListViewItem);
            }

            LoadAssemblyInfoDefInControls();
            EnableControls();
        }

        private void lvFiles_Click(object sender, System.EventArgs e) {
            LoadAssemblyInfoDefInControls();
        }

        private void SaveAssemblyDefFromControls() {
            CurrentAsemblyInfoDef.AssemblyCompany = txtCompany.Text;
            CurrentAsemblyInfoDef.AssemblyConfiguration = txtConfiguration.Text;
            CurrentAsemblyInfoDef.AssemblyCopyright = txtCopyright.Text;
            CurrentAsemblyInfoDef.AssemblyCulture = txtCulture.Text;
            CurrentAsemblyInfoDef.AssemblyDelaySign = chkDelaySign.Checked;
            CurrentAsemblyInfoDef.AssemblyDescription = txtDescription.Text;
            CurrentAsemblyInfoDef.OutputFileName = txtFileName.Text;
            CurrentAsemblyInfoDef.AssemblyKeyFile = txtKeyFile.Text;
            CurrentAsemblyInfoDef.AssemblyKeyName = txtKey.Text;
            CurrentAsemblyInfoDef.Index = Decimal.ToInt32(nuIndex.Value);
            CurrentAsemblyInfoDef.AssemblyProduct = txtProduct.Text;
            CurrentAsemblyInfoDef.AssemblyTitle = txtTitle.Text;
            CurrentAsemblyInfoDef.AssemblyTrademark = txtTrademark.Text;
            CurrentAsemblyInfoDef.AssemblyVersion = txtVersion.Text;
        }

        private static void SaveAssemblyInfoDefInFile(string aFileName, AssemblyInfoDef aAssemblyInfoDef){
            XmlSerializer _Serializer = new XmlSerializer(typeof(AssemblyInfoDef));

            // Create an XmlSerializerNamespaces object.
            Stream _FileStream = new FileStream(aFileName, FileMode.Create);

            XmlTextWriter _Writer = new XmlTextWriter(_FileStream, new UTF8Encoding());
            try {
                _Writer.Formatting  = Formatting.Indented;

                // Serialize using the XmlTextWriter.
                _Serializer.Serialize(_Writer, aAssemblyInfoDef);
            } finally {
                _Writer.Close();
            }
        }

        private void SelectListViewItem(AssemblyInfoDef aAssemblyInfoDef) {
            foreach(ListViewItem _ListViewItem in lvFiles.Items) {
                if (_ListViewItem.Tag == aAssemblyInfoDef) {
                    _ListViewItem.Selected = true;
                    
                    return;
                }
            }
        }

        // *************************************************************************
        //                              Properties
        // *************************************************************************
        private ModeEnum Mode {
            get {
                return fMode;
            }
            set {
                fMode = value;
                EnableControls();
            }
        }

        private AssemblyInfoDef CurrentAsemblyInfoDef {
            get {
                if (lvFiles.SelectedItems.Count > 0) {
                    return fList[lvFiles.SelectedItems[0].Index];
                } else {
                    return null;
                }
            }
        }

    }
}
