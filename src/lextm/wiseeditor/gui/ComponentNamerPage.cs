using System;

using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using EV.Windows.Forms;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;
using Lextm.OpenTools;
using System.Collections;

namespace Lextm.WiseEditor.Gui
{
	/// <summary>
	/// Summary description for UserControl.
	/// </summary>
	public class ComponentNamerPage : Lextm.OpenTools.Gui.CustomPage
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Button btUpdateComponentPrefix;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtControlPrefixTypeName;
		private System.Windows.Forms.TextBox txtControlPrefixPrefix;
		private System.Windows.Forms.CheckBox chkEnableAutoRenameControls;
		private System.Windows.Forms.Button btRemoveComponentPrefix;
		private System.Windows.Forms.Button btAddComponentPrefix;
		private Vista_Api.ListView lvComponentPrefix;
		private System.Windows.Forms.ColumnHeader colPrefix;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ColumnHeader colType;

		public ComponentNamerPage()
		{
			// This call is required by the Windows.Forms Designer.
			InitializeComponent();

			new ListViewSortManager(lvComponentPrefix,
						new Type[] {
							typeof(ListViewTextSort),
							typeof(ListViewTextSort)
						});
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
					components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.ListViewItem ListViewItem11 = new System.Windows.Forms.ListViewItem("allo");
			this.label11 = new System.Windows.Forms.Label();
			this.btUpdateComponentPrefix = new System.Windows.Forms.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.txtControlPrefixTypeName = new System.Windows.Forms.TextBox();
			this.txtControlPrefixPrefix = new System.Windows.Forms.TextBox();
			this.chkEnableAutoRenameControls = new System.Windows.Forms.CheckBox();
			this.btRemoveComponentPrefix = new System.Windows.Forms.Button();
			this.btAddComponentPrefix = new System.Windows.Forms.Button();
			this.colType = new System.Windows.Forms.ColumnHeader();
			this.lvComponentPrefix = new Vista_Api.ListView();
			this.colPrefix = new System.Windows.Forms.ColumnHeader();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(176, 32);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(32, 16);
			this.label11.TabIndex = 22;
			this.label11.Text = "Type";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btUpdateComponentPrefix
			// 
			this.btUpdateComponentPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btUpdateComponentPrefix.Location = new System.Drawing.Point(224, 232);
			this.btUpdateComponentPrefix.Name = "btUpdateComponentPrefix";
			this.btUpdateComponentPrefix.Size = new System.Drawing.Size(72, 24);
			this.btUpdateComponentPrefix.TabIndex = 21;
			this.btUpdateComponentPrefix.Text = "Update";
			this.btUpdateComponentPrefix.Click += new System.EventHandler(this.btUpdateComponentPrefix_Click);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 32);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(48, 16);
			this.label10.TabIndex = 20;
			this.label10.Text = "Prefix";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtControlPrefixTypeName
			// 
			this.txtControlPrefixTypeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtControlPrefixTypeName.Location = new System.Drawing.Point(216, 32);
			this.txtControlPrefixTypeName.Name = "txtControlPrefixTypeName";
			this.txtControlPrefixTypeName.Size = new System.Drawing.Size(224, 21);
			this.txtControlPrefixTypeName.TabIndex = 19;
			this.txtControlPrefixTypeName.Text = "";
			// 
			// txtControlPrefixPrefix
			// 
			this.txtControlPrefixPrefix.Location = new System.Drawing.Point(56, 32);
			this.txtControlPrefixPrefix.Name = "txtControlPrefixPrefix";
			this.txtControlPrefixPrefix.Size = new System.Drawing.Size(112, 21);
			this.txtControlPrefixPrefix.TabIndex = 18;
			this.txtControlPrefixPrefix.Text = "";
			// 
			// chkEnableAutoRenameControls
			// 
			this.chkEnableAutoRenameControls.Location = new System.Drawing.Point(16, 16);
			this.chkEnableAutoRenameControls.Name = "chkEnableAutoRenameControls";
			this.chkEnableAutoRenameControls.Size = new System.Drawing.Size(280, 24);
			this.chkEnableAutoRenameControls.TabIndex = 17;
			this.chkEnableAutoRenameControls.Text = "Enable auto rename controls";
			// 
			// btRemoveComponentPrefix
			// 
			this.btRemoveComponentPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btRemoveComponentPrefix.Location = new System.Drawing.Point(120, 232);
			this.btRemoveComponentPrefix.Name = "btRemoveComponentPrefix";
			this.btRemoveComponentPrefix.Size = new System.Drawing.Size(72, 24);
			this.btRemoveComponentPrefix.TabIndex = 16;
			this.btRemoveComponentPrefix.Text = "Remove";
			this.btRemoveComponentPrefix.Click += new System.EventHandler(this.btRemoveComponentPrefix_Click);
			// 
			// btAddComponentPrefix
			// 
			this.btAddComponentPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btAddComponentPrefix.Location = new System.Drawing.Point(16, 232);
			this.btAddComponentPrefix.Name = "btAddComponentPrefix";
			this.btAddComponentPrefix.Size = new System.Drawing.Size(72, 24);
			this.btAddComponentPrefix.TabIndex = 15;
			this.btAddComponentPrefix.Text = "Add";
			this.btAddComponentPrefix.Click += new System.EventHandler(this.btAddComponentPrefix_Click);
			// 
			// colType
			// 
			this.colType.Text = "Control Type";
			this.colType.Width = 374;
			// 
			// lvComponentPrefix
			// 
			this.lvComponentPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvComponentPrefix.AutoArrange = false;
			this.lvComponentPrefix.CausesValidation = false;
			this.lvComponentPrefix.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
						this.colPrefix,
						this.colType});
			this.lvComponentPrefix.FullRowSelect = true;
			this.lvComponentPrefix.HideSelection = false;
			this.lvComponentPrefix.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
						ListViewItem11});
			this.lvComponentPrefix.Location = new System.Drawing.Point(16, 40);
			this.lvComponentPrefix.MultiSelect = false;
			this.lvComponentPrefix.Name = "lvComponentPrefix";
			this.lvComponentPrefix.Size = new System.Drawing.Size(408, 184);
			this.lvComponentPrefix.TabIndex = 14;
			this.lvComponentPrefix.View = System.Windows.Forms.View.Details;
			this.lvComponentPrefix.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvComponentPrefix_KeyDown);
			this.lvComponentPrefix.Click += new System.EventHandler(this.lvComponentPrefix_Click);
			// 
			// colPrefix
			// 
			this.colPrefix.Text = "Prefix";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.txtControlPrefixTypeName);
			this.groupBox1.Controls.Add(this.txtControlPrefixPrefix);
			this.groupBox1.Location = new System.Drawing.Point(16, 280);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(448, 72);
			this.groupBox1.TabIndex = 23;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Prefix properties";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.lvComponentPrefix);
			this.groupBox2.Controls.Add(this.chkEnableAutoRenameControls);
			this.groupBox2.Controls.Add(this.btUpdateComponentPrefix);
			this.groupBox2.Controls.Add(this.btRemoveComponentPrefix);
			this.groupBox2.Controls.Add(this.btAddComponentPrefix);
			this.groupBox2.Location = new System.Drawing.Point(16, 8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(448, 264);
			this.groupBox2.TabIndex = 24;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Prefix list";
			// 
			// ComponentNamerPage
			// 
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "ComponentNamerPage";
			this.Size = new System.Drawing.Size(568, 408);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion
		private static Feature.ComponentNamerFeature.Preferences preferences =
			Feature.ComponentNamerFeature.GetOptions();

		/// <summary>
		/// Preferences to UI.
		/// </summary>
		public override void PreferencesToUI()
		{
			base.PreferencesToUI();
			
			chkEnableAutoRenameControls.Checked = (bool)PropertyRegistry.Get("EnableAutoRenameControls", false);

            LoadControlPrefixesInListView();
		
		}
		/// <summary>
		/// UI to preferences.
		/// </summary>
		public override void UIToPreferences()
		{
			base.UIToPreferences();

			PropertyRegistry.Set("EnableAutoRenameControls",
			                     chkEnableAutoRenameControls.Checked);
			SaveControlPrefixesFromListView();

		}

#region Event handlers
        		private void btAddComponentPrefix_Click(object sender, System.EventArgs e) {
    			if (ControlPrefixIsValid()) {
    				ListViewItem _ListViewItem = new ListViewItem();
    				ControlPrefix _ControlPrefix = new ControlPrefix();
    
    				SaveControlPrefixFromControls(_ControlPrefix);
                    lvComponentPrefix.Items.Add(_ListViewItem);
    				LoadControlPrefixInListViewItem(_ControlPrefix, _ListViewItem);
    				lvComponentPrefix.SelectedItems.Clear();
    				_ListViewItem.Selected = true;
    			}
    
    			LoadControlPrefixInControl();
    		}
    
    		private void btRemoveComponentPrefix_Click(object sender, System.EventArgs e) {
				if (lvComponentPrefix.SelectedItems.Count > 0) {
    				lvComponentPrefix.Items.Remove(lvComponentPrefix.SelectedItems[0]);
    			}
    
    			LoadControlPrefixInControl();
    		}
    
    		private void btUpdateComponentPrefix_Click(object sender, System.EventArgs e) {
    			if (ControlPrefixIsValid()) {
    				SaveControlPrefixFromControls(GetSelectedControlPrefix());
    				LoadControlPrefixesInListView();
    			}
    		}
    		private void lvComponentPrefix_Click(object sender, System.EventArgs e) {
    			LoadControlPrefixInControl();
    		}
    
    		private void lvComponentPrefix_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
    			LoadControlPrefixInControl();
    		}
#endregion
		private void LoadControlPrefixesInListView() {
			lvComponentPrefix.Items.Clear();

			foreach(ControlPrefix _ControlPrefix in preferences.ControlPrefixes) {
				ListViewItem _ListViewItem = new ListViewItem();
				lvComponentPrefix.Items.Add(_ListViewItem);

				LoadControlPrefixInListViewItem(_ControlPrefix, _ListViewItem);
			}
		}

		private static void LoadControlPrefixInListViewItem(ControlPrefix controlPrefix, ListViewItem listViewItem) {
			listViewItem.SubItems.Clear();

			listViewItem.Text = controlPrefix.Prefix;
			listViewItem.SubItems.Add(controlPrefix.ControlTypeName);
			listViewItem.Tag = controlPrefix;
		}

		private void LoadControlPrefixInControl() {
			ControlPrefix _ControlPrefix = GetSelectedControlPrefix();

			if (_ControlPrefix != null) {
				txtControlPrefixPrefix.Text = _ControlPrefix.Prefix;
				txtControlPrefixTypeName.Text = _ControlPrefix.ControlTypeName;
			} else {
				txtControlPrefixPrefix.Text = "";
				txtControlPrefixTypeName.Text = "";
			}
		}
		private ListViewItem GetSelectedControlPrefixListViewItem() {
			if (lvComponentPrefix.SelectedItems.Count > 0) {
				return lvComponentPrefix.SelectedItems[0];
			}

			return null;
		}

		private void SaveControlPrefixFromControls(ControlPrefix aControlPrefix) {
			aControlPrefix.Prefix = txtControlPrefixPrefix.Text;
			aControlPrefix.ControlTypeName = txtControlPrefixTypeName.Text;
		}

		private void SaveControlPrefixesFromListView() {
			ArrayList _List = new ArrayList();

			foreach(ListViewItem _ListViewItem in lvComponentPrefix.Items) {
				_List.Add(_ListViewItem.Tag);
			}

			preferences.ControlPrefixes = (ControlPrefix[]) _List.ToArray(typeof(ControlPrefix));
		}

		private ControlPrefix GetSelectedControlPrefix() {
			ListViewItem _ListViewItem = GetSelectedControlPrefixListViewItem();
			if (_ListViewItem != null) {
				return (ControlPrefix) _ListViewItem.Tag;
			}

			return null;
		}

		private bool ControlPrefixIsValid(){
			if (String.IsNullOrEmpty(txtControlPrefixPrefix.Text)) {
				Lextm.Windows.Forms.MessageBoxFactory.Info(null, "Empty input", "You must enter a prefix");

				return false;
			}

			if (String.IsNullOrEmpty(txtControlPrefixTypeName.Text)) {
				Lextm.Windows.Forms.MessageBoxFactory.Info(null, "Empty input", "You must enter a type name");

				return false;
			}

			return true;
		}

		
	}
}
