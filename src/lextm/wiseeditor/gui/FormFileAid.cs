using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using BeWise.Common.Info;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;
using Borland.Studio.ToolsAPI;
using EV.Windows.Forms;
using Lextm.WiseEditor.FileAid;

namespace Lextm.WiseEditor.Gui
{
	/// <summary>
	/// Summary description for WinForm.
	/// </summary>
	public class FormFileAid : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbProjects;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbAll;
		private System.Windows.Forms.RadioButton rbForms;
		private System.Windows.Forms.RadioButton rbSource;
		private System.Windows.Forms.RadioButton rbAssemblies;
		private System.Windows.Forms.RadioButton rbOpened;
		private System.Windows.Forms.Button btOpen;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button btCancel;
		private Vista_Api.ListView ListView;
		private System.Windows.Forms.ColumnHeader chType;
		private System.Windows.Forms.ColumnHeader chFileName;
		private System.Windows.Forms.ColumnHeader chPath;
		private System.Windows.Forms.ImageList imageList;

		public FormFileAid()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			new ListViewSortManager(ListView,
									new Type[] {
										typeof(ListViewTextSort),
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
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormFileAid));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cbProjects = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbOpened = new System.Windows.Forms.RadioButton();
			this.rbAssemblies = new System.Windows.Forms.RadioButton();
			this.rbSource = new System.Windows.Forms.RadioButton();
			this.rbForms = new System.Windows.Forms.RadioButton();
			this.rbAll = new System.Windows.Forms.RadioButton();
			this.btOpen = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btCancel = new System.Windows.Forms.Button();
			this.ListView = new Vista_Api.ListView();
			this.chType = new System.Windows.Forms.ColumnHeader();
			this.chFileName = new System.Windows.Forms.ColumnHeader();
			this.chPath = new System.Windows.Forms.ColumnHeader();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(248, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(16, 16);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(88, 6);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(152, 21);
			this.txtName.TabIndex = 1;
			this.txtName.Text = "";
			this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
			this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(57, 17);
			this.label1.TabIndex = 2;
			this.label1.Text = "File name:";
			// 
			// cbProjects
			// 
			this.cbProjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbProjects.Location = new System.Drawing.Point(336, 6);
			this.cbProjects.Name = "cbProjects";
			this.cbProjects.Size = new System.Drawing.Size(264, 21);
			this.cbProjects.TabIndex = 3;
			this.cbProjects.Text = "(select here)";
			this.cbProjects.SelectedIndexChanged += new System.EventHandler(this.cbProjects_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(280, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(43, 17);
			this.label2.TabIndex = 4;
			this.label2.Text = "Project:";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.rbOpened);
			this.groupBox1.Controls.Add(this.rbAssemblies);
			this.groupBox1.Controls.Add(this.rbSource);
			this.groupBox1.Controls.Add(this.rbForms);
			this.groupBox1.Controls.Add(this.rbAll);
			this.groupBox1.Location = new System.Drawing.Point(8, 32);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(592, 48);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "File type";
			// 
			// rbOpened
			// 
			this.rbOpened.Location = new System.Drawing.Point(504, 16);
			this.rbOpened.Name = "rbOpened";
			this.rbOpened.Size = new System.Drawing.Size(80, 24);
			this.rbOpened.TabIndex = 4;
			this.rbOpened.Text = "Open Tabs";
			this.rbOpened.Visible = false;
			this.rbOpened.CheckedChanged += new System.EventHandler(this.rbOpened_CheckedChanged);
			// 
			// rbAssemblies
			// 
			this.rbAssemblies.Location = new System.Drawing.Point(368, 16);
			this.rbAssemblies.Name = "rbAssemblies";
			this.rbAssemblies.Size = new System.Drawing.Size(88, 24);
			this.rbAssemblies.TabIndex = 3;
			this.rbAssemblies.Text = "Assemblies";
			this.rbAssemblies.CheckedChanged += new System.EventHandler(this.rbOpened_CheckedChanged);
			// 
			// rbSource
			// 
			this.rbSource.Location = new System.Drawing.Point(216, 16);
			this.rbSource.Name = "rbSource";
			this.rbSource.TabIndex = 2;
			this.rbSource.Text = "Source Files";
			this.rbSource.CheckedChanged += new System.EventHandler(this.rbOpened_CheckedChanged);
			// 
			// rbForms
			// 
			this.rbForms.Location = new System.Drawing.Point(112, 16);
			this.rbForms.Name = "rbForms";
			this.rbForms.Size = new System.Drawing.Size(56, 24);
			this.rbForms.TabIndex = 1;
			this.rbForms.Text = "Forms";
			this.rbForms.CheckedChanged += new System.EventHandler(this.rbOpened_CheckedChanged);
			// 
			// rbAll
			// 
			this.rbAll.Checked = true;
			this.rbAll.Location = new System.Drawing.Point(16, 16);
			this.rbAll.Name = "rbAll";
			this.rbAll.Size = new System.Drawing.Size(48, 24);
			this.rbAll.TabIndex = 0;
			this.rbAll.TabStop = true;
			this.rbAll.Text = "All";
			this.rbAll.CheckedChanged += new System.EventHandler(this.rbOpened_CheckedChanged);
			// 
			// btOpen
			// 
			this.btOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOpen.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOpen.Location = new System.Drawing.Point(432, 288);
			this.btOpen.Name = "btOpen";
			this.btOpen.TabIndex = 7;
			this.btOpen.Text = "Open";
			this.btOpen.Click += new System.EventHandler(this.btOpen_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Location = new System.Drawing.Point(0, 272);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(608, 8);
			this.groupBox2.TabIndex = 8;
			this.groupBox2.TabStop = false;
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Location = new System.Drawing.Point(520, 288);
			this.btCancel.Name = "btCancel";
			this.btCancel.TabIndex = 9;
			this.btCancel.Text = "Cancel";
			// 
			// Vista_Api.ListView
			// 
			this.ListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
						this.chType,
						this.chFileName,
						this.chPath});
			this.ListView.FullRowSelect = true;
			this.ListView.HideSelection = false;
			this.ListView.Location = new System.Drawing.Point(8, 88);
			this.ListView.Name = "Vista_Api.ListView";
			this.ListView.Size = new System.Drawing.Size(592, 184);
			this.ListView.SmallImageList = this.imageList;
			this.ListView.TabIndex = 10;
			this.ListView.View = System.Windows.Forms.View.Details;
			this.ListView.DoubleClick += new System.EventHandler(this.ListView_DoubleClick);
			// 
			// chType
			// 
			this.chType.Text = "Type";
			// 
			// chFileName
			// 
			this.chFileName.Text = "File name";
			this.chFileName.Width = 174;
			// 
			// chPath
			// 
			this.chPath.Text = "Path";
			this.chPath.Width = 341;
			// 
			// imageList
			// 
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// FormFileAid
			// 
			this.AcceptButton = this.btOpen;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(608, 318);
			this.Controls.Add(this.ListView);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.btOpen);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.cbProjects);
			this.Controls.Add(this.pictureBox1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "FormFileAid";
			this.Text = "File Aid";
			this.Load += new System.EventHandler(this.WinForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion

		private void btOpen_Click(object sender, System.EventArgs e)
		{
            OpenSelectedItems();
		}

		private void OpenSelectedItems() {
			if (this.ListView.SelectedItems.Count > 0) {
                for (int i = 0; i < this.ListView.SelectedItems.Count; i++) {
					NavigateTo(this.ListView.SelectedItems[i].Tag as ModuleFileInfo);
				}
			}
		}

		private static void NavigateTo(ModuleFileInfo item) {
			IOTAModuleInfo _ModuleInfo = (IOTAModuleInfo)item.Tag;
            IOTAModule _Module = _ModuleInfo.OpenModule();

            if (_Module != null) {
                _Module.ShowFileName(_ModuleInfo.FileName);
			}
        }
		
		private void WinForm_Load(object sender, System.EventArgs e)
		{
            ModuleRegistry.RefreshList();
			LoadProjectsInCombo();
			//LoadItemsInVista_Api.ListView();
		}

		Bitmap GetBackGroundBitmap() {
			Bitmap bmp = new Bitmap(ListView.ClientSize.Width, ListView.ClientSize.Height);
            Graphics gfx = Graphics.FromImage(bmp);

			Rectangle rect = new Rectangle (0,0,ListView.ClientSize.Width,
				ListView.ClientSize.Height);

            Brush _brush;

            _brush = new LinearGradientBrush(rect, Color.LightBlue, Color.Blue, LinearGradientMode.Vertical);
            gfx.FillRectangle(_brush,rect);

            return bmp;
		}

		private static ListViewItem CreateListViewItem(ModuleFileInfo item) {
			ModuleFileInfo _ModuleFileInfo = item;
            IOTAModuleInfo _ModuleInfo = (IOTAModuleInfo) _ModuleFileInfo.Tag;

            ListViewItem _ListViewItem = new ListViewItem();

            _ListViewItem.Text = Path.GetExtension(_ModuleInfo.FileName);
            _ListViewItem.SubItems.Add(Path.GetFileName(_ModuleInfo.FileName));
            _ListViewItem.SubItems.Add(Path.GetDirectoryName(_ModuleInfo.FileName));
            _ListViewItem.ImageIndex = _ModuleFileInfo.ImageIndex;
            _ListViewItem.Tag = item;

            return _ListViewItem;
		}
		private string GetProjectSpecString() {
			if (cbProjects.SelectedIndex == 0)
			{
				return ProjectSpec.AnyProject;
			} else if (cbProjects.SelectedIndex == 1) {
				return ProjectSpec.ActiveProject;
			} else {
				return cbProjects.Text;
			}			
		}
		
		private ModuleType GetTypeSpecEnum() {
			if (rbAll.Checked) {
				return ModuleType.All;
			} else if (rbAssemblies.Checked) {
				return ModuleType.Assemblies;
			} else if (rbForms.Checked) {
				return ModuleType.Forms;
			} else {//if (rbSource.Checked) {
				return ModuleType.Sources;
			}
		}
		
		private void LoadItemsInListView() {
			// prepare the specification
			string projSpec = GetProjectSpecString();

            Spec spec = new AndSpec(new ProjectSpec(projSpec), 
			                        new AndSpec(new FileNameSpec(txtName.Text),
			                                    new TypeSpec(GetTypeSpecEnum())));
			// find and add
			this.ListView.BeginUpdate();
			this.ListView.Items.Clear();
			foreach (ModuleFileInfo file in ModuleFinder.SelectModules(spec)) {
				this.ListView.Items.Add(CreateListViewItem(file));
			}
			this.ListView.EndUpdate();
		}
		
		private void LoadProjectsInCombo( ) {
			this.cbProjects.Items.Clear();
			this.cbProjects.Items.Add("Any");
			this.cbProjects.Items.Add("Active Project");
			IOTAProjectGroup _ProjectGroup = OtaUtils.GetCurrentProjectGroup();
			if (_ProjectGroup != null) {
				for (int j= 0; j < _ProjectGroup.ProjectCount; j++) {
                    IOTAProject _Project = _ProjectGroup[j];

					this.cbProjects.Items.Add(_Project.FileName);
                }
            }
			cbProjects.SelectedIndex = 0;
		}
		
		private void txtName_TextChanged(object sender, System.EventArgs e)
		{
			LoadItemsInListView();
		}
		
		private void rbOpened_CheckedChanged(object sender, System.EventArgs e)
		{
            LoadItemsInListView();
			cbProjects.Enabled = (sender != this.rbOpened);
            this.txtName.Focus();			
		}
		
		private void cbProjects_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            LoadItemsInListView();
		}

		private void txtName_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			ListViewHelper.SendKeyToListView(this.ListView, e);
		}
		
		private void ListView_DoubleClick(object sender, System.EventArgs e)
		{
			btOpen_Click(sender, e);
			Close();
		}
	}
}
