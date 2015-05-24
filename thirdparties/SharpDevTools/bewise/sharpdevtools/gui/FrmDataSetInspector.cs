using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace BeWise.SharpDevTools.Gui {

    public class FrmDataSetInspector : System.Windows.Forms.Form {

        // *************************************************************************
        //                              Constructor
        // *************************************************************************
        public FrmDataSetInspector() {
            InitializeComponent();
            LoadDataViewRowStateInCombo();
            LoadTables();
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
        private const string NO_FILTER_VALUE = "No Filter (Current Rows)";

        private DataSet fDataSet;
        private DataView fDataRows;
        private DataViewRowState fDataViewRowState = DataViewRowState.CurrentRows;
        private string fSelect = "";

        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.DataGrid dataGrid;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.Panel pnl;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem mnuSaveToFile;
        private System.Windows.Forms.MenuItem mnuLoadFromFile;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem mnuInspectDataSet;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TextBox txtSelect;
        private System.Windows.Forms.Button btSelect;
        private System.Windows.Forms.ComboBox cbFilter;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.StatusBar statusBar;
        private System.Windows.Forms.StatusBarPanel statusBarPanel1;
        private System.Windows.Forms.StatusBarPanel statusBarPanel2;

        private void ApplyFilter() {
            ApplyFilter(GetCurrentDataViewRowState());
        }

        private void ApplyFilter(DataViewRowState aDataViewRowState){
            if (CurrentDataTable == null) {
                return;
            }

            fDataRows = new DataView(CurrentDataTable, "", "", aDataViewRowState);
            dataGrid.DataSource = fDataRows;
            fDataViewRowState = aDataViewRowState;
            fSelect = "";

            SetStatusBar();
        }

        private void btFilter_Click(object sender, System.EventArgs e) {
            ApplyFilter();
        }

        private void btSelect_Click(object sender, System.EventArgs e) {
            if (CurrentDataTable == null) {
                return;
            }

            try {
                fDataRows = new DataView(CurrentDataTable, txtSelect.Text, "", DataViewRowState.CurrentRows);
                dataGrid.DataSource = fDataRows;
                fSelect = txtSelect.Text;
                SetStatusBar();
            } catch {
                MessageBox.Show("Invalid Select Parameter");
            }
        }


        #region Windows Form Designer generated code
        private void InitializeComponent() {
            this.dataGrid = new System.Windows.Forms.DataGrid();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.mnuLoadFromFile = new System.Windows.Forms.MenuItem();
            this.mnuSaveToFile = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.mnuInspectDataSet = new System.Windows.Forms.MenuItem();
            this.listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.pnl = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.txtSelect = new System.Windows.Forms.TextBox();
            this.btSelect = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanel2 = new System.Windows.Forms.StatusBarPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.pnl.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGrid
            // 
            this.dataGrid.ContextMenu = this.contextMenu;
            this.dataGrid.DataMember = "";
            this.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid.Location = new System.Drawing.Point(179, 0);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.Size = new System.Drawing.Size(373, 360);
            this.dataGrid.TabIndex = 0;
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                        this.mnuLoadFromFile,
                        this.mnuSaveToFile,
                        this.menuItem3,
                        this.mnuInspectDataSet});
            // 
            // mnuLoadFromFile
            // 
            this.mnuLoadFromFile.Index = 0;
            this.mnuLoadFromFile.Text = "Load from File ...";
            // 
            // mnuSaveToFile
            // 
            this.mnuSaveToFile.Index = 1;
            this.mnuSaveToFile.Text = "Save to File ...";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.Text = "-";
            // 
            // mnuInspectDataSet
            // 
            this.mnuInspectDataSet.Index = 3;
            this.mnuInspectDataSet.Text = "Inspect DataSet ...";
            this.mnuInspectDataSet.Click += new System.EventHandler(this.mnuInspectDataSet_Click);
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                        this.columnHeader1});
            this.listView.Dock = System.Windows.Forms.DockStyle.Left;
            this.listView.FullRowSelect = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(176, 360);
            this.listView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView.TabIndex = 1;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Table Name";
            this.columnHeader1.Width = 158;
            // 
            // pnl
            // 
            this.pnl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                        | System.Windows.Forms.AnchorStyles.Left) 
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl.Controls.Add(this.dataGrid);
            this.pnl.Controls.Add(this.splitter1);
            this.pnl.Controls.Add(this.listView);
            this.pnl.Location = new System.Drawing.Point(8, 16);
            this.pnl.Name = "pnl";
            this.pnl.Size = new System.Drawing.Size(552, 360);
            this.pnl.TabIndex = 2;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(176, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 360);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                        | System.Windows.Forms.AnchorStyles.Left) 
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.splitter2);
            this.groupBox1.Controls.Add(this.pnl);
            this.groupBox1.Location = new System.Drawing.Point(8, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(568, 384);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(3, 16);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 365);
            this.splitter2.TabIndex = 4;
            this.splitter2.TabStop = false;
            // 
            // txtSelect
            // 
            this.txtSelect.Location = new System.Drawing.Point(200, 16);
            this.txtSelect.Name = "txtSelect";
            this.txtSelect.Size = new System.Drawing.Size(320, 20);
            this.txtSelect.TabIndex = 4;
            this.txtSelect.Text = "";
            // 
            // btSelect
            // 
            this.btSelect.Location = new System.Drawing.Point(528, 14);
            this.btSelect.Name = "btSelect";
            this.btSelect.Size = new System.Drawing.Size(48, 23);
            this.btSelect.TabIndex = 5;
            this.btSelect.Text = "Select";
            this.btSelect.Click += new System.EventHandler(this.btSelect_Click);
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.Location = new System.Drawing.Point(8, 16);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(184, 21);
            this.cbFilter.TabIndex = 6;
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 439);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
                        this.statusBarPanel1,
                        this.statusBarPanel2});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(584, 22);
            this.statusBar.TabIndex = 8;
            this.statusBar.Text = "statusBar";
            // 
            // statusBarPanel1
            // 
            this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusBarPanel1.MinWidth = 120;
            this.statusBarPanel1.Width = 120;
            // 
            // statusBarPanel2
            // 
            this.statusBarPanel2.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusBarPanel2.Width = 448;
            // 
            // FrmDataSetInspector
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.cbFilter);
            this.Controls.Add(this.btSelect);
            this.Controls.Add(this.txtSelect);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmDataSetInspector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataSet Inspector";
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.pnl.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).EndInit();
            this.ResumeLayout(false);
        }
        #endregion

        private void listView_SelectedIndexChanged(object sender, System.EventArgs e) {
            dataGrid.DataSource = DataSet;

            // change the member
            if (dataGrid.DataSource is DataSet) {
                if (listView.SelectedItems.Count > 0) {
                    bool _Filtered  = IsFiltered;

                    dataGrid.DataMember = listView.SelectedItems[0].Text;
                    fSelect = "";

                    if (IsFiltered) {
                        ApplyFilter(fDataViewRowState);
                    }
                } else {
                    dataGrid.DataMember = "";
                }
            }

            SetStatusBar();
        }

        private void SetStatusBar(){
            if (IsFiltered) {
                statusBar.Panels[0].Text = "Filtered [" + fDataViewRowState.ToString() + "]";
            } else {
                statusBar.Panels[0].Text = "";
            }

            if (fSelect != "") {
                statusBar.Panels[1].Text = "Select [" + fSelect + "]";
            }
        }

        // *************************************************************************
        //                              Protected
        // *************************************************************************

        protected DataViewRowState GetCurrentDataViewRowState(){
            if (cbFilter.Text == "" || cbFilter.Text == NO_FILTER_VALUE) {
                return DataViewRowState.CurrentRows;
            }

            return (DataViewRowState) Enum.Parse(typeof(DataViewRowState), cbFilter.Text);
        }

        protected bool IsFiltered {
            get {
                return (fDataViewRowState != DataViewRowState.CurrentRows);
            }
        }

        protected void LoadDataViewRowStateInCombo(){
            cbFilter.Items.Clear();

            cbFilter.Items.Add(NO_FILTER_VALUE);

            foreach (string _DataViewRowState  in Enum.GetNames(typeof(DataViewRowState))) {
                cbFilter.Items.Add(_DataViewRowState.ToString());
            }

            cbFilter.SelectedIndex = 0;
        }

        protected void LoadTables(){
            listView.Items.Clear();

            if (DataSet != null) {
                foreach(DataTable _Table in DataSet.Tables) {
                    listView.Items.Add(_Table.TableName);
                }
            }
        }

        // *************************************************************************
        //                              Public
        // *************************************************************************
        public DataTable CurrentDataTable {
            get {
                if (DataSet == null || listView.SelectedItems.Count == 0) {
                    return null;
                }

                return DataSet.Tables[listView.SelectedItems[0].Text];
            }
        }

        public DataSet DataSet {
            get {
                return fDataSet;
            } set {
                fDataSet = value;
                dataGrid.DataSource = fDataSet;
                LoadTables();
            }
        }
        
        private void mnuInspectDataSet_Click(object sender, System.EventArgs e) {
            FrmObjectInspector _FrmObjectInspector = new FrmObjectInspector();
            _FrmObjectInspector.AddObject(DataSet, "DataSet");
            _FrmObjectInspector.Show();
        }
        
        private void cbFilter_SelectedIndexChanged(object sender, System.EventArgs e) {
            ApplyFilter();
        }
    }
}
