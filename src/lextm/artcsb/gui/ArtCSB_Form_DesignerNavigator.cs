using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;//IToolboxService
using System.Windows.Forms;
using BeWise.Common.Utils;
using Lextm.Diagnostics;
using Lextm.OpenTools;

namespace ArtCSB{
	//-------------------------------------------------------------------------------
	/// <summary>
	/// Designer navigator form.
	/// </summary>
	class WinForm_DesignerNavigator : System.Windows.Forms.Form{
		//---------------------------------------------------------------------------
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolBar toolBar_Designer;
		private System.Windows.Forms.ImageList imageList_TOOLBAR;
		private System.Windows.Forms.ImageList imageList_TOOLPALETTE;
		private System.Windows.Forms.ToolBarButton toolBarButton_REFRESH;
		private System.Windows.Forms.ToolBarButton toolBarButton_OPTIONS;
		private System.Windows.Forms.ToolBarButton toolBarButton_SEPARATOR1;
		private System.Windows.Forms.Panel panel_TreeViews;
		private System.Windows.Forms.TabControl tabControl_Designer;
		private System.Windows.Forms.TabPage tabPage_Controls;
		private System.Windows.Forms.TabPage tabPage_Components;
		private Vista_Api.TreeView TreeView_Controls;
		private Vista_Api.TreeView TreeView_Components;
		private System.Windows.Forms.StatusBar statusBar_Designer;
		private System.Windows.Forms.StatusBarPanel statusBarPanel_Module;
		private System.Windows.Forms.ToolBarButton toolBarButton_SEPARATOR2;
		private System.Windows.Forms.ToolBarButton toolBarButton_ABOUT;
		private System.Windows.Forms.ToolBarButton toolBarButton_SEPARATOR3;
		private System.Windows.Forms.ToolBarButton toolBarButton_VIEW_TYPE;
		private System.Windows.Forms.ToolBarButton toolBarButton_DEL_CMP;
		private System.Windows.Forms.ToolBarButton toolBarButton_SEPARATOR4;
		//---------------------------------------------------------------------------
		private ToolboxItemCollection  toolboxItemCollection;
		//is form in active state (some nuances with TreeView behavior)
		private bool ActiveFlag;
        private IList<ISelectionService> AL = new List<ISelectionService>();
		private string RefreshFileName = String.Empty;
		private static bool BlockSelectionRefresh;

		private bool ViewBaseClass;
		private System.Windows.Forms.ContextMenu contextMenu_OPTIONS;
		private System.Windows.Forms.MenuItem menuItem_StayOnTop;
		private System.Windows.Forms.MenuItem menuItem_Transparent;
		private System.Windows.Forms.MenuItem menuItem_ShowInTaskBar;
		private System.Windows.Forms.MenuItem menuItem_ExpandTree;

		//private OTASnap Snap = new OTASnap();

		//---------------------------------------------------------------------------
		internal WinForm_DesignerNavigator(){
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		}
		//---------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose (bool disposing){
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		//---------------------------------------------------------------------------

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WinForm_DesignerNavigator));
			this.imageList_TOOLPALETTE = new System.Windows.Forms.ImageList(this.components);
			this.toolBar_Designer = new System.Windows.Forms.ToolBar();
			this.toolBarButton_REFRESH = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton_SEPARATOR1 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton_OPTIONS = new System.Windows.Forms.ToolBarButton();
			this.contextMenu_OPTIONS = new System.Windows.Forms.ContextMenu();
			this.menuItem_StayOnTop = new System.Windows.Forms.MenuItem();
			this.menuItem_Transparent = new System.Windows.Forms.MenuItem();
			this.menuItem_ShowInTaskBar = new System.Windows.Forms.MenuItem();
			this.menuItem_ExpandTree = new System.Windows.Forms.MenuItem();
			this.toolBarButton_SEPARATOR2 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton_VIEW_TYPE = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton_SEPARATOR3 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton_DEL_CMP = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton_SEPARATOR4 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton_ABOUT = new System.Windows.Forms.ToolBarButton();
			this.imageList_TOOLBAR = new System.Windows.Forms.ImageList(this.components);
			this.panel_TreeViews = new System.Windows.Forms.Panel();
			this.tabControl_Designer = new System.Windows.Forms.TabControl();
			this.tabPage_Controls = new System.Windows.Forms.TabPage();
			this.TreeView_Controls = new Vista_Api.TreeView();
			this.tabPage_Components = new System.Windows.Forms.TabPage();
			this.TreeView_Components = new Vista_Api.TreeView();
			this.statusBar_Designer = new System.Windows.Forms.StatusBar();
			this.statusBarPanel_Module = new System.Windows.Forms.StatusBarPanel();
			this.panel_TreeViews.SuspendLayout();
			this.tabControl_Designer.SuspendLayout();
			this.tabPage_Controls.SuspendLayout();
			this.tabPage_Components.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel_Module)).BeginInit();
			this.SuspendLayout();
			// 
			// imageList_TOOLPALETTE
			// 
			this.imageList_TOOLPALETTE.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList_TOOLPALETTE.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// toolBar_Designer
			// 
			this.toolBar_Designer.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBar_Designer.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
						this.toolBarButton_REFRESH,
						this.toolBarButton_SEPARATOR1,
						this.toolBarButton_OPTIONS,
						this.toolBarButton_SEPARATOR2,
						this.toolBarButton_VIEW_TYPE,
						this.toolBarButton_SEPARATOR3,
						this.toolBarButton_DEL_CMP,
						this.toolBarButton_SEPARATOR4,
						this.toolBarButton_ABOUT});
			this.toolBar_Designer.DropDownArrows = true;
			this.toolBar_Designer.ImageList = this.imageList_TOOLBAR;
			this.toolBar_Designer.Location = new System.Drawing.Point(0, 0);
			this.toolBar_Designer.Name = "toolBar_Designer";
			this.toolBar_Designer.ShowToolTips = true;
			this.toolBar_Designer.Size = new System.Drawing.Size(364, 28);
			this.toolBar_Designer.TabIndex = 0;
			this.toolBar_Designer.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_Designer_ButtonClick);
			// 
			// toolBarButton_REFRESH
			// 
			this.toolBarButton_REFRESH.ImageIndex = 0;
			this.toolBarButton_REFRESH.Tag = "0";
			this.toolBarButton_REFRESH.ToolTipText = "Refresh Designer Navigator (Ctrl+R)";
			// 
			// toolBarButton_SEPARATOR1
			// 
			this.toolBarButton_SEPARATOR1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButton_OPTIONS
			// 
			this.toolBarButton_OPTIONS.DropDownMenu = this.contextMenu_OPTIONS;
			this.toolBarButton_OPTIONS.ImageIndex = 1;
			this.toolBarButton_OPTIONS.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
			this.toolBarButton_OPTIONS.Tag = "3";
			this.toolBarButton_OPTIONS.ToolTipText = "Options";
			// 
			// contextMenu_OPTIONS
			// 
			this.contextMenu_OPTIONS.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
						this.menuItem_StayOnTop,
						this.menuItem_Transparent,
						this.menuItem_ShowInTaskBar,
						this.menuItem_ExpandTree});
			// 
			// menuItem_StayOnTop
			// 
			this.menuItem_StayOnTop.Index = 0;
			this.menuItem_StayOnTop.Text = "Stay on Top     (Ctrl+T)";
			this.menuItem_StayOnTop.Click += new System.EventHandler(this.menuItem_StayOnTop_Click);
			// 
			// menuItem_Transparent
			// 
			this.menuItem_Transparent.Index = 1;
			this.menuItem_Transparent.Text = "Transparent     (Ctrl+O)";
			this.menuItem_Transparent.Click += new System.EventHandler(this.menuItem_Transparent_Click);
			// 
			// menuItem_ShowInTaskBar
			// 
			this.menuItem_ShowInTaskBar.Checked = true;
			this.menuItem_ShowInTaskBar.Index = 2;
			this.menuItem_ShowInTaskBar.Text = "Show on taskbar";
			this.menuItem_ShowInTaskBar.Click += new System.EventHandler(this.menuItem_ShowInTaskBar_Click);
			// 
			// menuItem_ExpandTree
			// 
			this.menuItem_ExpandTree.Checked = true;
			this.menuItem_ExpandTree.Index = 3;
			this.menuItem_ExpandTree.Text = "Expand all tree branches";
			this.menuItem_ExpandTree.Click += new System.EventHandler(this.menuItem_ExpandTree_Click);
			// 
			// toolBarButton_SEPARATOR2
			// 
			this.toolBarButton_SEPARATOR2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButton_VIEW_TYPE
			// 
			this.toolBarButton_VIEW_TYPE.ImageIndex = 3;
			this.toolBarButton_VIEW_TYPE.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButton_VIEW_TYPE.ToolTipText = "View object types (Ctrl+Y)";
			// 
			// toolBarButton_SEPARATOR3
			// 
			this.toolBarButton_SEPARATOR3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButton_DEL_CMP
			// 
			this.toolBarButton_DEL_CMP.ImageIndex = 4;
			this.toolBarButton_DEL_CMP.ToolTipText = "Delete selected components";
			// 
			// toolBarButton_SEPARATOR4
			// 
			this.toolBarButton_SEPARATOR4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButton_ABOUT
			// 
			this.toolBarButton_ABOUT.ImageIndex = 2;
			this.toolBarButton_ABOUT.ToolTipText = "About";
			// 
			// imageList_TOOLBAR
			// 
			this.imageList_TOOLBAR.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList_TOOLBAR.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList_TOOLBAR.ImageStream")));
			this.imageList_TOOLBAR.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// panel_TreeViews
			// 
			this.panel_TreeViews.Controls.Add(this.tabControl_Designer);
			this.panel_TreeViews.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel_TreeViews.Location = new System.Drawing.Point(0, 28);
			this.panel_TreeViews.Name = "panel_Vista_Api.TreeViews";
			this.panel_TreeViews.Size = new System.Drawing.Size(364, 338);
			this.panel_TreeViews.TabIndex = 16;
			// 
			// tabControl_Designer
			// 
			this.tabControl_Designer.Controls.Add(this.tabPage_Controls);
			this.tabControl_Designer.Controls.Add(this.tabPage_Components);
			this.tabControl_Designer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl_Designer.Location = new System.Drawing.Point(0, 0);
			this.tabControl_Designer.Name = "tabControl_Designer";
			this.tabControl_Designer.SelectedIndex = 0;
			this.tabControl_Designer.Size = new System.Drawing.Size(364, 338);
			this.tabControl_Designer.TabIndex = 0;
			// 
			// tabPage_Controls
			// 
			this.tabPage_Controls.Controls.Add(this.TreeView_Controls);
			this.tabPage_Controls.ForeColor = System.Drawing.Color.Black;
			this.tabPage_Controls.Location = new System.Drawing.Point(4, 22);
			this.tabPage_Controls.Name = "tabPage_Controls";
			this.tabPage_Controls.Size = new System.Drawing.Size(356, 312);
			this.tabPage_Controls.TabIndex = 0;
			this.tabPage_Controls.Text = "Controls tree";
			// 
			// TreeView_Controls
			// 
			this.TreeView_Controls.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.TreeView_Controls.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TreeView_Controls.ForeColor = System.Drawing.Color.Black;
			this.TreeView_Controls.ImageList = this.imageList_TOOLPALETTE;
			this.TreeView_Controls.Location = new System.Drawing.Point(0, 0);
			this.TreeView_Controls.Name = "Vista_Api.TreeView_Controls";
			this.TreeView_Controls.Size = new System.Drawing.Size(356, 312);
			this.TreeView_Controls.Sorted = true;
			this.TreeView_Controls.TabIndex = 0;
			this.TreeView_Controls.Click += new System.EventHandler(this.TreeView_Controls_Click);
			this.TreeView_Controls.AfterSelect += new TreeViewEventHandler(this.TreeView_ControlsAndComponents_AfterSelect);
			// 
			// tabPage_Components
			// 
			this.tabPage_Components.Controls.Add(this.TreeView_Components);
			this.tabPage_Components.ForeColor = System.Drawing.Color.Black;
			this.tabPage_Components.Location = new System.Drawing.Point(4, 22);
			this.tabPage_Components.Name = "tabPage_Components";
			this.tabPage_Components.Size = new System.Drawing.Size(356, 312);
			this.tabPage_Components.TabIndex = 1;
			this.tabPage_Components.Text = "Components";
			// 
			// TreeView_Components
			// 
			this.TreeView_Components.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.TreeView_Components.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TreeView_Components.ForeColor = System.Drawing.Color.Black;
			this.TreeView_Components.ImageList = this.imageList_TOOLPALETTE;
			this.TreeView_Components.Location = new System.Drawing.Point(0, 0);
			this.TreeView_Components.Name = "Vista_Api.TreeView_Components";
			this.TreeView_Components.Size = new System.Drawing.Size(356, 312);
			this.TreeView_Components.Sorted = true;
			this.TreeView_Components.TabIndex = 17;
			this.TreeView_Components.Click += new System.EventHandler(this.TreeView_Components_Click);
			this.TreeView_Components.AfterSelect += new TreeViewEventHandler(this.TreeView_ControlsAndComponents_AfterSelect);
			// 
			// statusBar_Designer
			// 
			this.statusBar_Designer.Location = new System.Drawing.Point(0, 366);
			this.statusBar_Designer.Name = "statusBar_Designer";
			this.statusBar_Designer.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
						this.statusBarPanel_Module});
			this.statusBar_Designer.ShowPanels = true;
			this.statusBar_Designer.Size = new System.Drawing.Size(364, 24);
			this.statusBar_Designer.TabIndex = 17;
			// 
			// statusBarPanel_Module
			// 
			this.statusBarPanel_Module.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel_Module.Width = 348;
			// 
			// WinForm_DesignerNavigator
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(364, 390);
			this.Controls.Add(this.panel_TreeViews);
			this.Controls.Add(this.statusBar_Designer);
			this.Controls.Add(this.toolBar_Designer);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Location = new System.Drawing.Point(50, 50);
			this.Name = "WinForm_DesignerNavigator";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ArtCSB Designer Navigator";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WinForm_DesignerNavigator_KeyDown);
			this.Click += new System.EventHandler(this.WinForm_DesignerNavigator_Click);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.WinForm_DesignerNavigator_Closing);
			this.Load += new System.EventHandler(this.WinForm_DesignerNavigator_Load);
			this.Activated += new System.EventHandler(this.WinForm_DesignerNavigator_Activated);
			this.Deactivate += new System.EventHandler(this.WinForm_DesignerNavigator_Deactivate);
			this.panel_TreeViews.ResumeLayout(false);
			this.tabControl_Designer.ResumeLayout(false);
			this.tabPage_Controls.ResumeLayout(false);
			this.tabPage_Components.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel_Module)).EndInit();
			this.ResumeLayout(false);
		}
		#endregion //Windows Form Designer generated code

		//---------------------------------------------------------------------------
		private void WinForm_DesignerNavigator_Activated(object sender,
				System.EventArgs e) {
			ActiveFlag = true;
			MyTreeRefresh();
			SelectionRefresh();
		}
		//---------------------------------------------------------------------------
		private void WinForm_DesignerNavigator_Deactivate(object sender,
				System.EventArgs e) {
			ActiveFlag = false;
		}
		//---------------------------------------------------------------------------
		private void WinForm_DesignerNavigator_Load(object sender,
				System.EventArgs e) {
			SkinRefresh();
			ViewBaseClass = false;
			try{
				toolboxItemCollection = OtaUtils.GetToolboxService().GetToolboxItems();
				imageList_TOOLPALETTE.Images.Clear();
				Icon ico = Lextm.ArtCSB.ArtCSB_MyResource.UnknownImage16x16_ico;//((Icon)(Common.MyRes.GetObject("UnknownImage16x16.ico")));
				imageList_TOOLPALETTE.Images.Add(ico);

				for (int i = 0; i < toolboxItemCollection.Count; i++){
					imageList_TOOLPALETTE.Images.Add(toolboxItemCollection[i].Bitmap);
				}

				this.ShowInTaskbar = (bool)PropertyRegistry.Get("DesignerNavigator_ShowInTaskBar", true);

				//if (RefreshFileName != Utils_OTAPI.CurrentEditorFileName){
					SkinRefresh();
					MyTreeRefresh();
					SelectionRefresh();
				//}

			}catch{
            	LoggingService.Error("ArtCSB: Fails to load icons for controls.");
			}
		}
		//---------------------------------------------------------------------------
		//also for TreeView_Components
		private void TreeView_ControlsAndComponents_AfterSelect(object sender,
				TreeViewEventArgs e){
			try{
				if (ActiveFlag){
					Object[] objArray = new Object[1] { ((MyTreeNode)e.Node).obj };
					OtaUtils.GetSelectionService().SetSelectedComponents(objArray);
					SelectionRefresh();
				}
				((Vista_Api.TreeView)sender).SelectedNode = null;
			}catch{}
		}
		//---------------------------------------------------------------------------
		private void toolBar_Designer_ButtonClick(
				object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e){
			if (e.Button == toolBarButton_REFRESH){
				SkinRefresh();
				MyTreeRefresh();
				SelectionRefresh();
				return;
			}
			if (e.Button == toolBarButton_DEL_CMP){

				DialogResult result = Lextm.Windows.Forms.MessageBoxFactory.Confirm("Selected components will be deleted!");
				if (result == DialogResult.Yes)
				{
					this.Activate();
					BlockSelectionRefresh = true;
					try{
						DelSelCmp(TreeView_Controls.Nodes);
						DelSelCmp(TreeView_Components.Nodes);
					}catch{}

					BlockSelectionRefresh = false;

					MyTreeRefresh();
					SelectionRefresh();
					return;
				}
			}
			if (e.Button == toolBarButton_VIEW_TYPE){
				switch (toolBarButton_VIEW_TYPE.Pushed){
					case true:
						ViewBaseClass = true;
						break;
					case false:
						ViewBaseClass = false;
						break;
				}
				MyTreeRefresh();
				SelectionRefresh();
				return;
			}
			if (e.Button == toolBarButton_OPTIONS){
            /*TODO : Add options later*/
//				WinForm_Options  dlgOpt =
//					new WinForm_Options(OptionsKind.Designer);
//				if (this.TopMost) {dlgOpt.TopMost = true;}
//				if (dlgOpt.ShowDialog() == DialogResult.OK){
//					SkinRefresh();
//				}
//				dlgOpt.Dispose();
				return;
			}
			if (e.Button == toolBarButton_ABOUT){
				WinForm_About  dlgAbt = new WinForm_About();
				if (this.TopMost) {dlgAbt.TopMost = true;}
				dlgAbt.ShowDialog();
				dlgAbt.Dispose();
				return;
			}
		}
		//---------------------------------------------------------------------------
		class MyTreeNode: TreeNode{
			private bool _MySelected;
			private Color _UnselectedColor;
			private Color _SelectedColor = Color.FromArgb(255,192,255);
			internal MyTreeNode(string st):base(st){
				_UnselectedColor = this.BackColor;
			}
			internal object obj;
			internal bool MySelected{
				get{
					return _MySelected;
				}
				set{
					if (BlockSelectionRefresh){ return; }
					if (value == _MySelected){ return; }
					if (value == true){
						this.BackColor = _SelectedColor;
						if ( !((TreeNode)this).IsVisible ){
							((TreeNode)this).EnsureVisible();
						}
					}
					else{
						this.BackColor = _UnselectedColor;
					}
					_MySelected = value;
				}
			}
		}
		//---------------------------------------------------------------------------
		private void DelSelCmp(TreeNodeCollection Nodes){
			if ( Nodes == null) { return; }
			int i = 0;
			while (i < Nodes.Count){

				DelSelCmp(Nodes[i].Nodes);

				if ( ((MyTreeNode)Nodes[i]).MySelected ){
					if (  ((MyTreeNode)Nodes[i]).obj is System.Web.UI.Control ){
					}else{
						IComponent cmp = (IComponent)(((MyTreeNode)Nodes[i]).obj);
						if (cmp.Site != null){
							if (cmp.Site.Container != null){
								cmp.Site.Container.Remove(cmp);
								cmp.Dispose();
							}
						}
					}
					Nodes.RemoveAt(i);
				}else{
					i++;
				}
			}//while
		}
		//---------------------------------------------------------------------------
		private bool FindNodeByObj(TreeNodeCollection Nodes,object obj,
				out MyTreeNode node){
			node = null;
			for (int i = 0; i < Nodes.Count; i++){
				if (obj == ((MyTreeNode)Nodes[i]).obj){
					node = (MyTreeNode)Nodes[i];
					return true;
				}else{
					if (FindNodeByObj(Nodes[i].Nodes, obj, out node)){
						return true;
					}
				}
			}//for
			return false;
		}
		//---------------------------------------------------------------------------
		private void UnselectAll(TreeNodeCollection Nodes){
			for (int i = 0; i < Nodes.Count; i++){
				((MyTreeNode)Nodes[i]).MySelected = false;
				UnselectAll(Nodes[i].Nodes);
			}
		}
		//---------------------------------------------------------------------------
		private void SkinRefresh(){

			switch ((bool)PropertyRegistry.Get("DesignerNavigator_StayOnTop", false)){
				case true:
					TopMost = true;
					break;
				case false:
					TopMost = false;
					break;
			}
			switch ((bool)PropertyRegistry.Get("DesignerNavigator_Transparent", false)){
				case true:
					this.Opacity = (double)PropertyRegistry.Get("DesignerNavigator_OpacityValue", 0.5);
					break;
				case false:
					this.Opacity = 1;
					break;
			}
			ShowInTaskbar = (bool)PropertyRegistry.Get("DesignerNavigator_ShowInTaskBar", true);

			menuItem_StayOnTop.Checked = (bool)PropertyRegistry.Get("DesignerNavigator_StayOnTop", false);
			menuItem_Transparent.Checked = (bool)PropertyRegistry.Get("DesignerNavigator_Transparent", false);
			menuItem_ShowInTaskBar.Checked = (bool)PropertyRegistry.Get("DesignerNavigator_ShowInTaskBar", true);
			menuItem_ExpandTree.Checked = (bool)PropertyRegistry.Get("DesignerNavigator_ExpandTree", true);
		}
		//---------------------------------------------------------------------------
		private void NodeProcessing(TreeNodeCollection nodes,IComponent cmp){
			if (cmp.Site == null){//for example, in NumericUpDown.Controls[0]
				return;
			}
			string nodeSt = cmp.Site.Name;
			if (ViewBaseClass){
				nodeSt += " : " + cmp.GetType().FullName;
			}
			MyTreeNode node = new MyTreeNode(nodeSt);
			node.obj = cmp;
			node.Checked = false;

			bool findType = false;
			for (int i = 0; i< toolboxItemCollection.Count; i++){
				if (toolboxItemCollection[i].TypeName == cmp.GetType().FullName){
					node.ImageIndex = i+1;
					node.SelectedImageIndex = i+1;
					findType = true;
					break;
				}
			 }
			 if (!findType){
				node.ImageIndex = 0;
				node.SelectedImageIndex = 0;
				LoggingService.Warn("ArtCSB: Type is not found.");
			 }

			nodes.Add(node);
            System.Windows.Forms.Control cntrl = cmp as System.Windows.Forms.Control;
			if (cntrl != null){				
				for (int i = 0; i < cntrl.Controls.Count; i++){
					NodeProcessing(node.Nodes, cntrl.Controls[i]);
				}
			}
            System.Web.UI.Control wtrl = cmp as System.Web.UI.Control;
			if (wtrl != null){				
				for (int i = 0; i < wtrl.Controls.Count; i++){
					NodeProcessing(node.Nodes, wtrl.Controls[i]);
				}
			}
		}
		//---------------------------------------------------------------------------
		private void MyTreeRefresh(){
			try{
				RefreshFileName = OtaUtils.GetCurrentEditorFileName();

				if (!OtaUtils.GetCurrentDotNetModule().HasDesignableType){
					TreeView_Controls.Nodes.Clear();
					TreeView_Components.Nodes.Clear();
					statusBarPanel_Module.Text = "";
					statusBarPanel_Module.ToolTipText = "";
					return;
				}

				ISelectionService selServ = OtaUtils.GetSelectionService();
				if ( !AL.Contains(selServ) ){
					selServ.SelectionChanged +=
						new EventHandler(OnDesignerSelectionChanged);
					AL.Add(selServ);
				}

				statusBarPanel_Module.Text =
					OtaUtils.GetModuleServices().CurrentModule.FileName;
				statusBarPanel_Module.ToolTipText =
					OtaUtils.GetModuleServices().CurrentModule.FileName;

				TreeView_Controls.Nodes.Clear();
				IComponent cmp = OtaUtils.GetCurrentDotNetModule().DesignerHost.RootComponent;
				NodeProcessing(TreeView_Controls.Nodes, cmp);

				TreeView_Components.Nodes.Clear();
				for (int i = 0;
					 i < OtaUtils.GetCurrentDotNetModule().DesignerHost.Container.Components.Count;
					 i++)
				{
					cmp = OtaUtils.GetCurrentDotNetModule().DesignerHost.Container.Components[i];

					if ( !(
							(cmp is System.Windows.Forms.Control)
							||
							(cmp is System.Web.UI.Control)
						  )
					   )
					{
						string nodeSt = cmp.Site.Name;
						if (ViewBaseClass){
							nodeSt += " : " + cmp.GetType().FullName;
						}
						MyTreeNode node = new MyTreeNode(nodeSt);
						node.obj = cmp;
						node.Checked = false;

						bool findType = false;
						for (int j = 0; j< toolboxItemCollection.Count; j++){
							if (toolboxItemCollection[j].TypeName ==
								cmp.GetType().FullName)
							{
								node.ImageIndex = j+1;
								node.SelectedImageIndex = j+1;
								findType = true;
								break;
							}
						}
						if (!findType){
							node.ImageIndex = 0;
							node.SelectedImageIndex = 0;
						}
						TreeView_Components.Nodes.Add(node);
					}
				}//for

				if ((bool)PropertyRegistry.Get("DesignerNavigator_ExpandTree", true)){
					TreeView_Controls.ExpandAll();
					TreeView_Components.ExpandAll();
				}

				if (TreeView_Controls.Nodes.Count > 0){
					TreeView_Controls.Nodes[0].EnsureVisible();
				}

			}catch{}
			try{
				if (OtaUtils.GetCurrentDotNetModule().DesignerHost.RootComponent is System.Web.UI.Control){
					toolBarButton_DEL_CMP.Visible = false;
					toolBarButton_SEPARATOR4.Visible = false;
				}else{
					toolBarButton_DEL_CMP.Visible = true;
					toolBarButton_SEPARATOR4.Visible = true;
				}
			}catch{}
		}
		//---------------------------------------------------------------------------
		private void SelectionRefresh(){
			if (BlockSelectionRefresh){ return; }
			try{
				MyTreeNode node;
				ISelectionService selServ = OtaUtils.GetSelectionService();
				int n = selServ.GetSelectedComponents().Count;
				Object[] objectArray = new Object[n];
				selServ.GetSelectedComponents().CopyTo(objectArray, 0);
				UnselectAll(TreeView_Controls.Nodes);
				UnselectAll(TreeView_Components.Nodes);
				for (int i=0; i < n; i++){
					if ( (objectArray[i] is System.Windows.Forms.Control)
						 ||
						 (objectArray[i] is System.Web.UI.Control)
					   )
					{
						if (FindNodeByObj(TreeView_Controls.Nodes,
							objectArray[i], out node))
						{
							if (tabControl_Designer.SelectedTab != tabPage_Controls){
								tabControl_Designer.SelectedTab = tabPage_Controls;
							}
							node.MySelected = true;
						}
					}
					else{
						if (objectArray[i] is System.ComponentModel.Component){
							if (FindNodeByObj(TreeView_Components.Nodes,
								objectArray[i], out node))
							{
								if (tabControl_Designer.SelectedTab !=
									tabPage_Components)
								{
									tabControl_Designer.SelectedTab = tabPage_Components;
								}
								node.MySelected = true;
							}
						}
					}
				}//for
			}catch{}
		}
		//---------------------------------------------------------------------------
		private void OnDesignerSelectionChanged(object sender, EventArgs args){
			//MessageB/ox.Show("!");
			SelectionRefresh();
		}
		//---------------------------------------------------------------------------
		private void WinForm_DesignerNavigator_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			try{
				for (int i = 0; i < AL.Count; i++){
					((ISelectionService)AL[i]).SelectionChanged -= new EventHandler(OnDesignerSelectionChanged);
				}
			}catch{}
		}
		//---------------------------------------------------------------------------
		private void TreeView_Controls_Click(object sender, System.EventArgs e) {
			if (RefreshFileName != OtaUtils.GetCurrentEditorFileName()){
				MyTreeRefresh();
				SelectionRefresh();
			}
		}
		//---------------------------------------------------------------------------
		private void TreeView_Components_Click(object sender, System.EventArgs e) {
			if (RefreshFileName != OtaUtils.GetCurrentEditorFileName()){
				MyTreeRefresh();
				SelectionRefresh();
			}
		}
		//---------------------------------------------------------------------------
		private void WinForm_DesignerNavigator_Click(object sender, System.EventArgs e) {
			if (RefreshFileName != OtaUtils.GetCurrentEditorFileName()){
				MyTreeRefresh();
				SelectionRefresh();
			}
		}
		//---------------------------------------------------------------------------
		private void WinForm_DesignerNavigator_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			if (e.KeyCode == Keys.Escape){
				OtaUtils.GetCurrentEditor().Show();
				this.Close();
				OtaUtils.GetCurrentEditView().Paint();
				return;
			}
			if (e.KeyCode == Keys.R && e.Control){
				SkinRefresh();
				MyTreeRefresh();
				SelectionRefresh();
				return;
			}
			if (e.KeyCode == Keys.Y && e.Control){
				switch (toolBarButton_VIEW_TYPE.Pushed){
					case true:
						toolBarButton_VIEW_TYPE.Pushed = false;
						ViewBaseClass = false;
						break;
					case false:
						toolBarButton_VIEW_TYPE.Pushed = true;
						ViewBaseClass = true;
						break;
				}
				MyTreeRefresh();
				SelectionRefresh();
				return;
			}
			if (e.KeyCode == Keys.T && e.Control){

				PropertyRegistry.Set("DesignerNavigator_StayOnTop",
									!(bool)PropertyRegistry.Get("DesignerNavigator_StayOnTop", false));
				SkinRefresh();
				return;
			}
			if (e.KeyCode == Keys.O && e.Control){

				PropertyRegistry.Set("DesignerNavigator_Transparent",
									!(bool)PropertyRegistry.Get("DesignerNavigator_Transparent", false));
				SkinRefresh();
				return;
			}
		}
		//---------------------------------------------------------------------------
		#region contextMenu_OPTIONS implementation
		private void menuItem_StayOnTop_Click(object sender, System.EventArgs e)
		{
			menuItem_StayOnTop.Checked = !menuItem_StayOnTop.Checked;

			PropertyRegistry.Set("DesignerNavigator_StayOnTop", menuItem_StayOnTop.Checked);
			//FeatureRegistry.GetFeature("Lextm.ArtCSB.Feature.DesignerNavigatorFeature").SavePreferences();
			PropertyRegistry.Flush();
			SkinRefresh();
		}
		//---------------------------------------------------------------------------
		private void menuItem_Transparent_Click(object sender, System.EventArgs e)
		{
			menuItem_Transparent.Checked = !menuItem_Transparent.Checked;
			
			PropertyRegistry.Set("DesignerNavigator_Transparent", menuItem_Transparent.Checked);
			//FeatureRegistry.GetFeature("Lextm.ArtCSB.Feature.DesignerNavigatorFeature").SavePreferences();
			PropertyRegistry.Flush();
			SkinRefresh();
		}
		//---------------------------------------------------------------------------
		private void menuItem_ShowInTaskBar_Click(object sender, System.EventArgs e)
		{
			menuItem_ShowInTaskBar.Checked = !menuItem_ShowInTaskBar.Checked;

			PropertyRegistry.Set("DesignerNavigator_ShowInTaskBar", menuItem_ShowInTaskBar.Checked);
			//FeatureRegistry.GetFeature("Lextm.ArtCSB.Feature.DesignerNavigatorFeature").SavePreferences();
			PropertyRegistry.Flush();
			SkinRefresh();
		}
		//---------------------------------------------------------------------------
		private void menuItem_ExpandTree_Click(object sender, System.EventArgs e)
		{
			menuItem_ExpandTree.Checked = !menuItem_ExpandTree.Checked;
			PropertyRegistry.Set("DesignerNavigator_ExpandTree", menuItem_ExpandTree.Checked);
			//FeatureRegistry.GetFeature("Lextm.ArtCSB.Feature.DesignerNavigatorFeature").SavePreferences();
			PropertyRegistry.Flush();
		}
		#endregion

//		private ArtCSB.Options options = Lextm.ArtCSB.Feature.DesignerNavigatorFeature.getInstance().Options
//		    as ArtCSB.Options; 
		//---------------------------------------------------------------------------

	}
	//-------------------------------------------------------------------------------

}
