// this is expert manager form.
//  it is ported from SBT. Win32 supports are added.
// Copyright (C) 2006  Lex Y. Li
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
using Lextm.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using BeWise.Common;
using BeWise.Common.Info;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools;
using BeWise.SharpBuilderTools.Helpers;
using EV.Windows.Forms;
using Lextm.Diagnostics;
using Microsoft.Win32;
using Lextm.Utilities.ExpertManager;

namespace Lextm.Utilities.Gui
{

	/// <summary>
	/// Expert Manager Form.
	/// </summary>
	public class FormExpertManager : Form
	{
		private const string MaskIDE = "Version {0}.0 ({1})";
		/// <summary>
		/// Constructor.
		/// </summary>
		public FormExpertManager()
		{
			InitializeComponent();

			new ListViewSortManager(lvExperts,
			                        new Type[] {
			                        	typeof(ListViewTextSort),
			                        	typeof(ListViewTextSort),
			                        	typeof(ListViewTextSort)
			                        });
		}
		ExpertRegistry registry;
		//private IList<ExpertInfo> fExpertInfoList = new List<ExpertInfo>();
		private Vista_Api.ListView lvExperts;
		private System.Windows.Forms.MenuItem menuFile;
		private System.Windows.Forms.MenuItem menuExit;
		private System.Windows.Forms.MenuItem menuHelp;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colFileName;
		private Vista_Api.OpenFileDialog openFileDialog;
		private System.Windows.Forms.MenuItem menuAbout;
		private ToolStrip toolStrip1;
		private ToolStripButton tsbtnEnable;
		private ToolStripButton tsbtnDisable;
		private ToolStripButton tsbtnRefresh;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripButton tsbtnAdd;
		private ToolStripButton tsbtnDelete;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripLabel toolStripLabel1;
		private ToolStripComboBox tscbIdeVersion;
		private StatusStrip statusStrip1;
		private MenuStrip menuStrip1;
		private ToolStripMenuItem fileToolStripMenuItem;
		private ToolStripMenuItem aboutToolStripMenuItem;
		private ToolStripMenuItem exitToolStripMenuItem;
		private Crad.Windows.Forms.Actions.ActionList alExpertManager;
		private Crad.Windows.Forms.Actions.Action actEnable;
		private Crad.Windows.Forms.Actions.Action actDisable;
		private Crad.Windows.Forms.Actions.Action actRefresh;
		private Crad.Windows.Forms.Actions.Action actAdd;
		private Crad.Windows.Forms.Actions.Action actDelete;
		private System.Windows.Forms.ColumnHeader colStatus;

		private int IDEVersion
		{
			get
			{
				return tscbIdeVersion.SelectedIndex + 1;
			}
		}

		private void btActivate_Click(object sender, System.EventArgs e)
		{
			ListViewItem item = lvExperts.SelectedItems[0];
			((ExpertInfo)item.Tag).Activate();
			item.Text = "[True]";
		}

		private void btAdd_Click(object sender, System.EventArgs e)
		{
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			string fileName = openFileDialog.FileName;
			if (registry.Registered(fileName))
			{
				ValidationHelpers.ShowWarning("The selected IDE Assembly is already registered !");
				return;
			}
			ExpertInfo expert = registry.CreateFromFile(fileName);
			AppendTo(lvExperts, expert);
		}
		
		void AppendTo(ListView list, ExpertInfo expert)
		{
			ListViewItem result = new ListViewItem();
			result.Text = "[" + expert.IsActive + "]";
			result.SubItems.Add(expert.Name);
			result.SubItems.Add(expert.FileName);
			result.Tag = expert;
			list.Items.Add(result);
		}

		private void btDelete_Click(object sender, System.EventArgs e)
		{
			if (Lextm.Windows.Forms.MessageBoxFactory.Confirm(Text, "Do you really want to remove this IDE assembly ?", "Click Yes and the assembly will be removed. Click No if you want to cancel.") != DialogResult.Yes)
			{
				return;
			}
			ListViewItem item = lvExperts.SelectedItems[0];
			ExpertInfo expert = (ExpertInfo)item.Tag;
			registry.Delete(expert.Name);
			item.Remove();
		}

		private void btDesactivate_Click(object sender, System.EventArgs e)
		{
			ListViewItem item = lvExperts.SelectedItems[0];
			((ExpertInfo)item.Tag).Deactivate();
			item.Text = "[False]";
		}

		private void btRefresh_Click(object sender, System.EventArgs e)
		{
            registry = new ExpertRegistry(IDEVersion);
			UpdateView(lvExperts, registry);
		}

		private void cbIDEVersion_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			registry = new ExpertRegistry(IDEVersion);
			UpdateView(lvExperts, registry);
		}

		private void FrmMain_Load(object sender, System.EventArgs e)
		{
			tscbIdeVersion.Sorted = false;

			foreach (IdeVersionInfo info in OtaUtils.IdeVersionInfoCollection)
			{
				tscbIdeVersion.Items.Insert(info.Version - 1, String.Format(CultureInfo.InvariantCulture, MaskIDE, info.Version, info.Name));
			}

			tscbIdeVersion.SelectedIndex = tscbIdeVersion.Items.Count - 1;
		}

		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExpertManager));
			this.lvExperts = new Vista_Api.ListView();
			this.colStatus = new System.Windows.Forms.ColumnHeader();
			this.colName = new System.Windows.Forms.ColumnHeader();
			this.colFileName = new System.Windows.Forms.ColumnHeader();
			this.menuFile = new System.Windows.Forms.MenuItem();
			this.menuExit = new System.Windows.Forms.MenuItem();
			this.menuHelp = new System.Windows.Forms.MenuItem();
			this.menuAbout = new System.Windows.Forms.MenuItem();
			this.openFileDialog = new Vista_Api.OpenFileDialog();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsbtnEnable = new System.Windows.Forms.ToolStripButton();
			this.tsbtnDisable = new System.Windows.Forms.ToolStripButton();
			this.tsbtnRefresh = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbtnAdd = new System.Windows.Forms.ToolStripButton();
			this.tsbtnDelete = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.tscbIdeVersion = new System.Windows.Forms.ToolStripComboBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.alExpertManager = new Crad.Windows.Forms.Actions.ActionList();
			this.actEnable = new Crad.Windows.Forms.Actions.Action();
			this.actDisable = new Crad.Windows.Forms.Actions.Action();
			this.actRefresh = new Crad.Windows.Forms.Actions.Action();
			this.actAdd = new Crad.Windows.Forms.Actions.Action();
			this.actDelete = new Crad.Windows.Forms.Actions.Action();
			this.toolStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.alExpertManager)).BeginInit();
			this.SuspendLayout();
			// 
			// lvExperts
			// 
			this.lvExperts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			                                	this.colStatus,
			                                	this.colName,
			                                	this.colFileName});
			this.lvExperts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvExperts.FullRowSelect = true;
			this.lvExperts.HideSelection = false;
			this.lvExperts.Location = new System.Drawing.Point(0, 50);
			this.lvExperts.MultiSelect = false;
			this.lvExperts.Name = "lvExperts";
			this.lvExperts.Size = new System.Drawing.Size(734, 332);
			this.lvExperts.TabIndex = 0;
			this.lvExperts.UseCompatibleStateImageBehavior = false;
			this.lvExperts.View = System.Windows.Forms.View.Details;
			//this.lvExperts.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ListView_KeyUp);
			//this.lvExperts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListView_KeyDown);
			//this.lvExperts.Click += new System.EventHandler(this.ListView_Click);
			// 
			// colStatus
			// 
			this.colStatus.Text = "Status";
			// 
			// colName
			// 
			this.colName.Text = "Name";
			this.colName.Width = 142;
			// 
			// colFileName
			// 
			this.colFileName.Text = "FileName";
			this.colFileName.Width = 395;
			// 
			// menuFile
			// 
			this.menuFile.Index = -1;
			this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			                                 	this.menuExit});
			this.menuFile.Text = "File";
			// 
			// menuExit
			// 
			this.menuExit.Index = 0;
			this.menuExit.Text = "Exit";
			this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
			// 
			// menuHelp
			// 
			this.menuHelp.Index = -1;
			this.menuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			                                 	this.menuAbout});
			this.menuHelp.Text = "Help";
			// 
			// menuAbout
			// 
			this.menuAbout.Index = 0;
			this.menuAbout.Text = "About ...";
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "IDE Experts (*.dll)|*.dll";
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			                               	this.tsbtnEnable,
			                               	this.tsbtnDisable,
			                               	this.tsbtnRefresh,
			                               	this.toolStripSeparator2,
			                               	this.tsbtnAdd,
			                               	this.tsbtnDelete,
			                               	this.toolStripSeparator1,
			                               	this.toolStripLabel1,
			                               	this.tscbIdeVersion});
			this.toolStrip1.Location = new System.Drawing.Point(0, 25);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(734, 25);
			this.toolStrip1.TabIndex = 8;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// tsbtnEnable
			// 
			this.alExpertManager.SetAction(this.tsbtnEnable, this.actEnable);
			this.tsbtnEnable.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnEnable.Image")));
			this.tsbtnEnable.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbtnEnable.Name = "tsbtnEnable";
			this.tsbtnEnable.Size = new System.Drawing.Size(67, 22);
			this.tsbtnEnable.Text = "Enable";
			// 
			// tsbtnDisable
			// 
			this.alExpertManager.SetAction(this.tsbtnDisable, this.actDisable);
			this.tsbtnDisable.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnDisable.Image")));
			this.tsbtnDisable.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbtnDisable.Name = "tsbtnDisable";
			this.tsbtnDisable.Size = new System.Drawing.Size(71, 22);
			this.tsbtnDisable.Text = "Disable";
			// 
			// tsbtnRefresh
			// 
			this.alExpertManager.SetAction(this.tsbtnRefresh, this.actRefresh);
			this.tsbtnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnRefresh.Image")));
			this.tsbtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbtnRefresh.Name = "tsbtnRefresh";
			this.tsbtnRefresh.Size = new System.Drawing.Size(72, 22);
			this.tsbtnRefresh.Text = "Refresh";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// tsbtnAdd
			// 
			this.alExpertManager.SetAction(this.tsbtnAdd, this.actAdd);
			this.tsbtnAdd.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnAdd.Image")));
			this.tsbtnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbtnAdd.Name = "tsbtnAdd";
			this.tsbtnAdd.Size = new System.Drawing.Size(52, 22);
			this.tsbtnAdd.Text = "Add";
			// 
			// tsbtnDelete
			// 
			this.alExpertManager.SetAction(this.tsbtnDelete, this.actDelete);
			this.tsbtnDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnDelete.Image")));
			this.tsbtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbtnDelete.Name = "tsbtnDelete";
			this.tsbtnDelete.Size = new System.Drawing.Size(65, 22);
			this.tsbtnDelete.Text = "Delete";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(28, 22);
			this.toolStripLabel1.Text = "IDE";
			// 
			// tscbIdeVersion
			// 
			this.tscbIdeVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.tscbIdeVersion.Name = "tscbIdeVersion";
			this.tscbIdeVersion.Size = new System.Drawing.Size(300, 25);
			this.tscbIdeVersion.SelectedIndexChanged += new System.EventHandler(this.cbIDEVersion_SelectedIndexChanged);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 382);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
			this.statusStrip1.Size = new System.Drawing.Size(734, 22);
			this.statusStrip1.TabIndex = 9;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			                               	this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(734, 25);
			this.menuStrip1.TabIndex = 10;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			                                                  	this.aboutToolStripMenuItem,
			                                                  	this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
			this.aboutToolStripMenuItem.Text = "About";
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.menuExit_Click);
			// 
			// alExpertManager
			// 
			this.alExpertManager.Actions.Add(this.actEnable);
			this.alExpertManager.Actions.Add(this.actDisable);
			this.alExpertManager.Actions.Add(this.actRefresh);
			this.alExpertManager.Actions.Add(this.actAdd);
			this.alExpertManager.Actions.Add(this.actDelete);
			this.alExpertManager.ContainerControl = this;
			// 
			// actEnable
			// 
			this.actEnable.Image = ((System.Drawing.Image)(resources.GetObject("actEnable.Image")));
			this.actEnable.Text = "Enable";
			this.actEnable.ToolTipText = "Enable";
			this.actEnable.Update += new System.EventHandler(this.actEnable_Update);
			this.actEnable.Execute += new System.EventHandler(this.btActivate_Click);
			// 
			// actDisable
			// 
			this.actDisable.Image = ((System.Drawing.Image)(resources.GetObject("actDisable.Image")));
			this.actDisable.Text = "Disable";
			this.actDisable.ToolTipText = "Disable";
			this.actDisable.Update += new System.EventHandler(this.actDisable_Update);
			this.actDisable.Execute += new System.EventHandler(this.btDesactivate_Click);
			// 
			// actRefresh
			// 
			this.actRefresh.Image = ((System.Drawing.Image)(resources.GetObject("actRefresh.Image")));
			this.actRefresh.Text = "Refresh";
			this.actRefresh.ToolTipText = "Refresh";
			this.actRefresh.Execute += new System.EventHandler(this.btRefresh_Click);
			// 
			// actAdd
			// 
			this.actAdd.Image = ((System.Drawing.Image)(resources.GetObject("actAdd.Image")));
			this.actAdd.Text = "Add";
			this.actAdd.ToolTipText = "Add";
			this.actAdd.Execute += new System.EventHandler(this.btAdd_Click);
			// 
			// actDelete
			// 
			this.actDelete.Image = ((System.Drawing.Image)(resources.GetObject("actDelete.Image")));
			this.actDelete.Text = "Delete";
			this.actDelete.ToolTipText = "Delete";
			this.actDelete.Update += new System.EventHandler(this.actDelete_Update);
			this.actDelete.Execute += new System.EventHandler(this.btDelete_Click);
			// 
			// FormExpertManager
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(734, 404);
			this.Controls.Add(this.lvExperts);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.menuStrip1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(750, 440);
			this.Name = "FormExpertManager";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Expert Manager";
			this.Load += new System.EventHandler(this.FrmMain_Load);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.alExpertManager)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void menuExit_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void UpdateView(ListView list, ExpertRegistry registry)
		{
			LoggingService.EnterMethod();
            list.SuspendLayout();
			list.Items.Clear();
			foreach (ExpertInfo expert in registry.Experts)
			{				
				AppendTo(list, expert);
			}
            list.ResumeLayout();
			LoggingService.LeaveMethod();
		}

		private void actDelete_Update(object sender, EventArgs e)
		{
			actDelete.Enabled = lvExperts.SelectedItems.Count == 1;
		}

		private void actDisable_Update(object sender, EventArgs e)
		{
			actDisable.Enabled = lvExperts.SelectedItems.Count == 1 && lvExperts.SelectedItems[0].Text == "[True]";
		}

		private void actEnable_Update(object sender, EventArgs e)
		{
			actEnable.Enabled = lvExperts.SelectedItems.Count == 1 && lvExperts.SelectedItems[0].Text == "[False]";
		}
	}
}