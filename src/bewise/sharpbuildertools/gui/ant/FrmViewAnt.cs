using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using EV.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Tools.Ant;
using BeWise.SharpBuilderTools.Helpers;

namespace BeWise.SharpBuilderTools.Gui.Ant {
    /// <summary>
    /// View Ant mode.
    /// </summary>
	public enum ViewAntMode {
    	/// <summary>
    	/// All.
    	/// </summary>
		All = 1,
		/// <summary>
		/// Targets.
		/// </summary>
		Targets,
		/// <summary>
		/// Properties.
		/// </summary>
		Properties};

	class FrmViewAnt: Form {

        /**************************************************************/
        /*                    Constructor/ Destructor
        /**************************************************************/

		internal FrmViewAnt(BaseAntProject aAntProject, BaseAnt aCurrentAnt) {
            fAntProject = aAntProject;
            fCurrentAnt = aCurrentAnt;

            InitializeComponent();

            // Create the Sort Manager for lvAll
            new ListViewSortManager(lvAll,
                                    new Type[] {
                                        typeof(ListViewTextSort),
                                        typeof(ListViewTextSort),
                                        typeof(ListViewInt32Sort),
                                        typeof(ListViewInt32Sort)
                                    });

            // Create the Sort Manager for lvTargets
            new ListViewSortManager(lvTargets,
                                    new Type[] {
                                        typeof(ListViewTextSort),
                                        typeof(ListViewInt32Sort),
                                        typeof(ListViewInt32Sort)
                                    });

            // Create the Sort Manager for lvProperties
            new ListViewSortManager(lvProperties,
                                    new Type[] {
                                        typeof(ListViewTextSort),
                                        typeof(ListViewInt32Sort),
                                        typeof(ListViewInt32Sort)
                                    });
        }

        /**************************************************************/
        /*                         Private
        /**************************************************************/

        private BaseAntProject fAntProject;
        private BaseAnt fCurrentAnt;
        private ViewAllAntHelper fViewAllAntHelper;
        private ViewAntTargetsHelper fViewAntTargetsHelper;
        private ViewAntPropertiesHelper fViewAntPropertiesHelper;

        private System.Windows.Forms.Label lblAll;
        private System.Windows.Forms.TabPage tpAll;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TextBox txtAll;
        private System.Windows.Forms.ListView lvAll;
        private System.Windows.Forms.TabPage tpTargets;
        private System.Windows.Forms.ListView lvTargets;
        private System.Windows.Forms.Label lblTarget;
        private System.Windows.Forms.TextBox txtTargets;
        private System.Windows.Forms.ColumnHeader colAllType;
        private System.Windows.Forms.ColumnHeader colAllName;
        private System.Windows.Forms.ColumnHeader colTargetName;
        private System.Windows.Forms.ColumnHeader colAllLineNumber;
        private System.Windows.Forms.ColumnHeader colTargetLineNumber;
        private System.Windows.Forms.TabPage tpProperties;
        private System.Windows.Forms.TextBox txtProperties;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvProperties;
        private System.Windows.Forms.ColumnHeader colPropertyName;
        private System.Windows.Forms.ColumnHeader colPropertyLineNumber;
        private System.Windows.Forms.ColumnHeader colAllColNumber;
        private System.Windows.Forms.ColumnHeader colTargetColNumber;
        private System.Windows.Forms.ColumnHeader colPropertyColNumber;

        private void InitializeComponent()
		{
			this.btCancel = new System.Windows.Forms.Button();
			this.btOk = new System.Windows.Forms.Button();
			this.TabControl = new System.Windows.Forms.TabControl();
			this.tpAll = new System.Windows.Forms.TabPage();
			this.txtAll = new System.Windows.Forms.TextBox();
			this.lvAll = new System.Windows.Forms.ListView();
			this.colAllType = new System.Windows.Forms.ColumnHeader();
			this.colAllName = new System.Windows.Forms.ColumnHeader();
			this.colAllLineNumber = new System.Windows.Forms.ColumnHeader();
			this.colAllColNumber = new System.Windows.Forms.ColumnHeader();
			this.lblAll = new System.Windows.Forms.Label();
			this.tpTargets = new System.Windows.Forms.TabPage();
			this.txtTargets = new System.Windows.Forms.TextBox();
			this.lblTarget = new System.Windows.Forms.Label();
			this.lvTargets = new System.Windows.Forms.ListView();
			this.colTargetName = new System.Windows.Forms.ColumnHeader();
			this.colTargetLineNumber = new System.Windows.Forms.ColumnHeader();
			this.colTargetColNumber = new System.Windows.Forms.ColumnHeader();
			this.tpProperties = new System.Windows.Forms.TabPage();
			this.txtProperties = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lvProperties = new System.Windows.Forms.ListView();
			this.colPropertyName = new System.Windows.Forms.ColumnHeader();
			this.colPropertyLineNumber = new System.Windows.Forms.ColumnHeader();
			this.colPropertyColNumber = new System.Windows.Forms.ColumnHeader();
			this.TabControl.SuspendLayout();
			this.tpAll.SuspendLayout();
			this.tpTargets.SuspendLayout();
			this.tpProperties.SuspendLayout();
			this.SuspendLayout();
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Location = new System.Drawing.Point(643, 464);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(90, 23);
			this.btCancel.TabIndex = 2;
			this.btCancel.Text = "Cancel";
			// 
			// btOk
			// 
			this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOk.Location = new System.Drawing.Point(547, 464);
			this.btOk.Name = "btOk";
			this.btOk.Size = new System.Drawing.Size(90, 23);
			this.btOk.TabIndex = 1;
			this.btOk.Text = "Ok";
			this.btOk.Click += new System.EventHandler(this.btOk_Click);
			// 
			// TabControl
			// 
			this.TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.TabControl.Controls.Add(this.tpAll);
			this.TabControl.Controls.Add(this.tpTargets);
			this.TabControl.Controls.Add(this.tpProperties);
			this.TabControl.Location = new System.Drawing.Point(10, 8);
			this.TabControl.Name = "TabControl";
			this.TabControl.SelectedIndex = 0;
			this.TabControl.Size = new System.Drawing.Size(729, 448);
			this.TabControl.TabIndex = 0;
			this.TabControl.TabStop = false;
			this.TabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
			// 
			// tpAll
			// 
			this.tpAll.Controls.Add(this.txtAll);
			this.tpAll.Controls.Add(this.lvAll);
			this.tpAll.Controls.Add(this.lblAll);
			this.tpAll.Location = new System.Drawing.Point(4, 21);
			this.tpAll.Name = "tpAll";
			this.tpAll.Size = new System.Drawing.Size(721, 423);
			this.tpAll.TabIndex = 0;
			this.tpAll.Text = "All";
			// 
			// txtAll
			// 
			this.txtAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtAll.Location = new System.Drawing.Point(96, 8);
			this.txtAll.Name = "txtAll";
			this.txtAll.Size = new System.Drawing.Size(606, 21);
			this.txtAll.TabIndex = 0;
			this.txtAll.Text = "";
			this.txtAll.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtItem_KeyDown);
			this.txtAll.TextChanged += new System.EventHandler(this.txtItem_TextChanged);
			// 
			// lvAll
			// 
			this.lvAll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvAll.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
						this.colAllType,
						this.colAllName,
						this.colAllLineNumber,
						this.colAllColNumber});
			this.lvAll.FullRowSelect = true;
			this.lvAll.HideSelection = false;
			this.lvAll.Location = new System.Drawing.Point(10, 40);
			this.lvAll.Name = "lvAll";
			this.lvAll.Size = new System.Drawing.Size(701, 377);
			this.lvAll.TabIndex = 1;
			this.lvAll.View = System.Windows.Forms.View.Details;
			this.lvAll.DoubleClick += new System.EventHandler(this.ListView_DoubleClick);
			// 
			// colAllType
			// 
			this.colAllType.Text = "Type";
			this.colAllType.Width = 95;
			// 
			// colAllName
			// 
			this.colAllName.Text = "Name";
			this.colAllName.Width = 310;
			// 
			// colAllLineNumber
			// 
			this.colAllLineNumber.Text = "Line Number";
			this.colAllLineNumber.Width = 80;
			// 
			// colAllColNumber
			// 
			this.colAllColNumber.Text = "Col Number";
			this.colAllColNumber.Width = 80;
			// 
			// lblAll
			// 
			this.lblAll.Location = new System.Drawing.Point(10, 11);
			this.lblAll.Name = "lblAll";
			this.lblAll.Size = new System.Drawing.Size(120, 24);
			this.lblAll.TabIndex = 2;
			this.lblAll.Text = "Name";
			// 
			// tpTargets
			// 
			this.tpTargets.Controls.Add(this.txtTargets);
			this.tpTargets.Controls.Add(this.lblTarget);
			this.tpTargets.Controls.Add(this.lvTargets);
			this.tpTargets.Location = new System.Drawing.Point(4, 21);
			this.tpTargets.Name = "tpTargets";
			this.tpTargets.Size = new System.Drawing.Size(721, 423);
			this.tpTargets.TabIndex = 3;
			this.tpTargets.Text = "Targets";
			// 
			// txtTargets
			// 
			this.txtTargets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTargets.Location = new System.Drawing.Point(96, 8);
			this.txtTargets.Name = "txtTargets";
			this.txtTargets.Size = new System.Drawing.Size(615, 21);
			this.txtTargets.TabIndex = 0;
			this.txtTargets.Text = "";
			this.txtTargets.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtItem_KeyDown);
			this.txtTargets.TextChanged += new System.EventHandler(this.txtItem_TextChanged);
			// 
			// lblTarget
			// 
			this.lblTarget.Location = new System.Drawing.Point(10, 11);
			this.lblTarget.Name = "lblTarget";
			this.lblTarget.Size = new System.Drawing.Size(120, 22);
			this.lblTarget.TabIndex = 2;
			this.lblTarget.Text = "Target";
			// 
			// lvTargets
			// 
			this.lvTargets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvTargets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
						this.colTargetName,
						this.colTargetLineNumber,
						this.colTargetColNumber});
			this.lvTargets.FullRowSelect = true;
			this.lvTargets.HideSelection = false;
			this.lvTargets.Location = new System.Drawing.Point(10, 40);
			this.lvTargets.Name = "lvTargets";
			this.lvTargets.Size = new System.Drawing.Size(701, 377);
			this.lvTargets.TabIndex = 1;
			this.lvTargets.View = System.Windows.Forms.View.Details;
			this.lvTargets.DoubleClick += new System.EventHandler(this.ListView_DoubleClick);
			// 
			// colTargetName
			// 
			this.colTargetName.Text = "Name";
			this.colTargetName.Width = 405;
			// 
			// colTargetLineNumber
			// 
			this.colTargetLineNumber.Text = "Line Number";
			this.colTargetLineNumber.Width = 80;
			// 
			// colTargetColNumber
			// 
			this.colTargetColNumber.Text = "Col Number";
			this.colTargetColNumber.Width = 80;
			// 
			// tpProperties
			// 
			this.tpProperties.Controls.Add(this.txtProperties);
			this.tpProperties.Controls.Add(this.label1);
			this.tpProperties.Controls.Add(this.lvProperties);
			this.tpProperties.Location = new System.Drawing.Point(4, 21);
			this.tpProperties.Name = "tpProperties";
			this.tpProperties.Size = new System.Drawing.Size(721, 423);
			this.tpProperties.TabIndex = 4;
			this.tpProperties.Text = "Properties";
			// 
			// txtProperties
			// 
			this.txtProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProperties.Location = new System.Drawing.Point(96, 8);
			this.txtProperties.Name = "txtProperties";
			this.txtProperties.Size = new System.Drawing.Size(615, 21);
			this.txtProperties.TabIndex = 0;
			this.txtProperties.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 11);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 22);
			this.label1.TabIndex = 2;
			this.label1.Text = "Property";
			// 
			// lvProperties
			// 
			this.lvProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvProperties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
						this.colPropertyName,
						this.colPropertyLineNumber,
						this.colPropertyColNumber});
			this.lvProperties.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.lvProperties.FullRowSelect = true;
			this.lvProperties.HideSelection = false;
			this.lvProperties.Location = new System.Drawing.Point(10, 40);
			this.lvProperties.Name = "lvProperties";
			this.lvProperties.Size = new System.Drawing.Size(701, 377);
			this.lvProperties.TabIndex = 1;
			this.lvProperties.View = System.Windows.Forms.View.Details;
			this.lvProperties.DoubleClick += new System.EventHandler(this.ListView_DoubleClick);
			// 
			// colPropertyName
			// 
			this.colPropertyName.Text = "Name";
			this.colPropertyName.Width = 405;
			// 
			// colPropertyLineNumber
			// 
			this.colPropertyLineNumber.Text = "Line Number";
			this.colPropertyLineNumber.Width = 80;
			// 
			// colPropertyColNumber
			// 
			this.colPropertyColNumber.Text = "Col Number";
			this.colPropertyColNumber.Width = 80;
			// 
			// FrmViewAnt
			// 
			this.AcceptButton = this.btOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(748, 494);
			this.Controls.Add(this.TabControl);
			this.Controls.Add(this.btOk);
			this.Controls.Add(this.btCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "FrmViewAnt";
			this.Text = "Sharp Builder Tools - View Ant";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtItem_KeyDown);
			this.TabControl.ResumeLayout(false);
			this.tpAll.ResumeLayout(false);
			this.tpTargets.ResumeLayout(false);
			this.tpProperties.ResumeLayout(false);
			this.ResumeLayout(false);
		}

        private void OpenSelectedItems() {
            if (GetCurrentListView().SelectedItems.Count > 0) {
                for (int i = 0; i < GetCurrentListView().SelectedItems.Count; i++) {
                    GetCurrentViewHelper().GoTo(GetCurrentListView().SelectedItems[i].Tag);
                }
            }
        }

        private void btOpen_Click(object sender, System.EventArgs e) {
            OpenSelectedItems();
        }

        private void btOk_Click(object sender, System.EventArgs e) {
            OpenSelectedItems();

            Close();
        }

        private void ListView_DoubleClick(object sender, System.EventArgs e) {
            btOk_Click(sender, e);
        }

        private void txtItem_TextChanged(object sender, System.EventArgs e) {
            LoadItemsInListView();
        }

        private void txtItem_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
            ListViewHelper.SendKeyToListView(GetCurrentListView(), e);
        }

        private void TabControl_SelectedIndexChanged(object sender, System.EventArgs e) {
            LoadItemsInListView();
            GetCurrentTextBox().Focus();
        }

        /**************************************************************/
        /*                       Protected
        /**************************************************************/

        private BaseViewHelper GetCurrentViewHelper() {
            if (TabControl.SelectedTab == tpAll) {
                if (fViewAllAntHelper == null) {
                    fViewAllAntHelper = new ViewAllAntHelper(AntProject, CurrentAnt);
                    fViewAllAntHelper.Load();
                }

                return fViewAllAntHelper;
            } else if (TabControl.SelectedTab == tpTargets) {
                if (fViewAntTargetsHelper == null) {
                    fViewAntTargetsHelper = new ViewAntTargetsHelper(AntProject, CurrentAnt);
					fViewAntTargetsHelper.Load();
                }

                return fViewAntTargetsHelper;
            } else {
                if (fViewAntPropertiesHelper == null) {
                    fViewAntPropertiesHelper = new ViewAntPropertiesHelper(AntProject, CurrentAnt);
                    fViewAntPropertiesHelper.Load();
                }

                return fViewAntPropertiesHelper;
            }
        }

		private ListView GetCurrentListView() {
            if (TabControl.SelectedTab == tpAll) {
                return lvAll;
            } else if (TabControl.SelectedTab == tpTargets) {
                return lvTargets;
            } else {
                return lvProperties;
            }
        }

		private TextBox GetCurrentTextBox() {
            if (TabControl.SelectedTab == tpAll) {
                return txtAll;
            } else if (TabControl.SelectedTab == tpTargets) {
                return txtTargets;
            } else {
                return txtProperties;
            }
        }

        private void LoadItemsInListView() {
            IComparer _Comparer  = GetCurrentViewHelper() as IComparer;

            Items.Sort(_Comparer);
            ListView _ListView = GetCurrentListView();

            object _SelectedItem = null;
            try {
                _ListView.BeginUpdate();
                try {
                    if (_ListView.SelectedItems.Count > 0) {
                        _SelectedItem = _ListView.SelectedItems[0].Tag;
                    }

                    _ListView.Items.Clear();

                    for (int i = 0; i < Items.Count; i ++) {
                        if (GetCurrentViewHelper().ItemIsVisible(GetCurrentTextBox().Text, "", Items[i])) {
                            ListViewItem _Item = GetCurrentViewHelper().CreateListViewItem(Items[i]);
                            _ListView.Items.Add(_Item);
                        }
                    }
                } finally {
                    _ListView.EndUpdate();
                }
            } finally {
                if (_ListView.Items.Count > 0) {
                    _ListView.Items[0].Selected = true;

                    for (int j = 0; j < _ListView.Items.Count; j++) {

                        if (_ListView.Items[j].Tag == _SelectedItem) {
                            _ListView.Items[j].Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        /**************************************************************/
        /*                   Protected Properties
        /**************************************************************/

        private ArrayList Items {
            get {
                return GetCurrentViewHelper().Items;
            }
        }

        /**************************************************************/
        /*                         Public
        /**************************************************************/

        public ListView[] GetListViews() {
            ListView[] _ListViews = new ListView[] {lvAll,
                                                    lvTargets,
                                                    lvProperties};

            return _ListViews;
        }

        /**************************************************************/
        /*                     Properties
        /**************************************************************/

		private BaseAntProject AntProject {
            get {
                return fAntProject;
            }
        }

		private BaseAnt CurrentAnt {
            get {
                return fCurrentAnt;
            }
        }

        internal ViewAntMode ViewAntMode {
            get {
                if (TabControl.SelectedTab == tpAll) {
                    return ViewAntMode.All;
                } else if (TabControl.SelectedTab == tpTargets) {
                    return ViewAntMode.Targets;
                } else {
                    return ViewAntMode.Properties;
                }
            }

            set {
                if (value == ViewAntMode.All) {
                    TabControl.SelectedTab = tpAll;
                } else if (value == ViewAntMode.Targets) {
                    TabControl.SelectedTab = tpTargets;
                } else if (value == ViewAntMode.Properties) {
                    TabControl.SelectedTab = tpProperties;
                }

                LoadItemsInListView();
            }
        }

    }
}
