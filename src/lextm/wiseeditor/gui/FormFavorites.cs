using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BeWise.Common.IconManagers;
using BeWise.Common.Utils;
using Borland.Studio.ToolsAPI;
using Lextm.OpenTools;

namespace Lextm.WiseEditor.Gui {

	class FormFavorites : Form {
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private Vista_Api.ListView lvFiles;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colPath;
        private System.Windows.Forms.ContextMenu cmFiles;
        private System.Windows.Forms.MenuItem mmView;
        private System.Windows.Forms.MenuItem mmLarge;
        private System.Windows.Forms.MenuItem mmSmall;
        private System.Windows.Forms.MenuItem mmList;
        private System.Windows.Forms.MenuItem mmDetails;
        private System.Windows.Forms.MenuItem mmSplitter2;
        private System.Windows.Forms.MenuItem mmNewFile;
        private System.Windows.Forms.MenuItem mmRemoveFile;
        private System.Windows.Forms.MenuItem mmAddFileToProject;
        private System.Windows.Forms.MenuItem mmOpenFile;
        private System.Windows.Forms.MenuItem mmOpenShell;
        private Vista_Api.TreeView tvFolders;
        private System.Windows.Forms.ContextMenu cmFolders;
        private System.Windows.Forms.MenuItem mmNewFolder;
        private System.Windows.Forms.MenuItem mmDeleteFolder;
        private System.Windows.Forms.MenuItem mmRenameFolder;
        private System.Windows.Forms.MainMenu menu;
        private System.Windows.Forms.MenuItem mmFile;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.MenuItem mmExit;
        private System.Windows.Forms.MenuItem mmSplitter3;

        // *********************************************************************
        //                           Private
        // *********************************************************************
        #region Windows Form Designer generated code
        private void InitializeComponent()
		{
        	this.components = new System.ComponentModel.Container();
        	this.panel1 = new System.Windows.Forms.Panel();
        	this.panel2 = new System.Windows.Forms.Panel();
        	this.lvFiles = new Vista_Api.ListView();
        	this.colName = new System.Windows.Forms.ColumnHeader();
        	this.colPath = new System.Windows.Forms.ColumnHeader();
        	this.cmFiles = new System.Windows.Forms.ContextMenu();
        	this.mmView = new System.Windows.Forms.MenuItem();
        	this.mmLarge = new System.Windows.Forms.MenuItem();
        	this.mmSmall = new System.Windows.Forms.MenuItem();
        	this.mmList = new System.Windows.Forms.MenuItem();
        	this.mmDetails = new System.Windows.Forms.MenuItem();
        	this.mmSplitter2 = new System.Windows.Forms.MenuItem();
        	this.mmOpenFile = new System.Windows.Forms.MenuItem();
        	this.mmAddFileToProject = new System.Windows.Forms.MenuItem();
        	this.mmOpenShell = new System.Windows.Forms.MenuItem();
        	this.mmSplitter3 = new System.Windows.Forms.MenuItem();
        	this.mmNewFile = new System.Windows.Forms.MenuItem();
        	this.mmRemoveFile = new System.Windows.Forms.MenuItem();
        	this.splitter1 = new System.Windows.Forms.Splitter();
        	this.tvFolders = new Vista_Api.TreeView();
        	this.cmFolders = new System.Windows.Forms.ContextMenu();
        	this.mmNewFolder = new System.Windows.Forms.MenuItem();
        	this.mmDeleteFolder = new System.Windows.Forms.MenuItem();
        	this.mmRenameFolder = new System.Windows.Forms.MenuItem();
        	this.menu = new System.Windows.Forms.MainMenu(this.components);
        	this.mmFile = new System.Windows.Forms.MenuItem();
        	this.mmExit = new System.Windows.Forms.MenuItem();
        	this.statusBar1 = new System.Windows.Forms.StatusBar();
        	this.panel1.SuspendLayout();
        	this.panel2.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// panel1
        	// 
        	this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        	        	        	| System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.panel1.Controls.Add(this.panel2);
        	this.panel1.Controls.Add(this.splitter1);
        	this.panel1.Controls.Add(this.tvFolders);
        	this.panel1.Location = new System.Drawing.Point(6, 9);
        	this.panel1.Name = "panel1";
        	this.panel1.Size = new System.Drawing.Size(714, 361);
        	this.panel1.TabIndex = 0;
        	// 
        	// panel2
        	// 
        	this.panel2.Controls.Add(this.lvFiles);
        	this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.panel2.Location = new System.Drawing.Point(199, 0);
        	this.panel2.Name = "panel2";
        	this.panel2.Size = new System.Drawing.Size(515, 361);
        	this.panel2.TabIndex = 3;
        	// 
        	// lvFiles
        	// 
        	this.lvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
        	        	        	this.colName,
        	        	        	this.colPath});
        	this.lvFiles.ContextMenu = this.cmFiles;
        	this.lvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.lvFiles.FullRowSelect = true;
        	this.lvFiles.HideSelection = false;
        	this.lvFiles.HoverSelection = true;
        	this.lvFiles.Location = new System.Drawing.Point(0, 0);
        	this.lvFiles.MultiSelect = false;
        	this.lvFiles.Name = "lvFiles";
        	this.lvFiles.Size = new System.Drawing.Size(515, 361);
        	this.lvFiles.TabIndex = 0;
        	this.lvFiles.UseCompatibleStateImageBehavior = false;
        	this.lvFiles.DoubleClick += new System.EventHandler(this.lvFiles_DoubleClick);
        	// 
        	// colName
        	// 
        	this.colName.Text = "Name";
        	this.colName.Width = 86;
        	// 
        	// colPath
        	// 
        	this.colPath.Text = "Path";
        	this.colPath.Width = 190;
        	// 
        	// cmFiles
        	// 
        	this.cmFiles.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
        	        	        	this.mmView,
        	        	        	this.mmSplitter2,
        	        	        	this.mmOpenFile,
        	        	        	this.mmAddFileToProject,
        	        	        	this.mmOpenShell,
        	        	        	this.mmSplitter3,
        	        	        	this.mmNewFile,
        	        	        	this.mmRemoveFile});
        	this.cmFiles.Popup += new System.EventHandler(this.cmFiles_Popup);
        	// 
        	// mmView
        	// 
        	this.mmView.Index = 0;
        	this.mmView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
        	        	        	this.mmLarge,
        	        	        	this.mmSmall,
        	        	        	this.mmList,
        	        	        	this.mmDetails});
        	this.mmView.Text = "View";
        	// 
        	// mmLarge
        	// 
        	this.mmLarge.Index = 0;
        	this.mmLarge.Text = "Large Icons";
        	this.mmLarge.Click += new System.EventHandler(this.mmLarge_Click);
        	// 
        	// mmSmall
        	// 
        	this.mmSmall.Index = 1;
        	this.mmSmall.Text = "Small Icons";
        	this.mmSmall.Click += new System.EventHandler(this.mmSmall_Click);
        	// 
        	// mmList
        	// 
        	this.mmList.Index = 2;
        	this.mmList.Text = "List";
        	this.mmList.Click += new System.EventHandler(this.mmList_Click);
        	// 
        	// mmDetails
        	// 
        	this.mmDetails.Index = 3;
        	this.mmDetails.Text = "Details";
        	this.mmDetails.Click += new System.EventHandler(this.mmDetails_Click);
        	// 
        	// mmSplitter2
        	// 
        	this.mmSplitter2.Index = 1;
        	this.mmSplitter2.Text = "-";
        	// 
        	// mmOpenFile
        	// 
        	this.mmOpenFile.Index = 2;
        	this.mmOpenFile.Text = "Open";
        	this.mmOpenFile.Click += new System.EventHandler(this.mmOpenFile_Click);
        	// 
        	// mmAddFileToProject
        	// 
        	this.mmAddFileToProject.Index = 3;
        	this.mmAddFileToProject.Text = "Add to project";
        	this.mmAddFileToProject.Click += new System.EventHandler(this.mmAddFileToProject_Click);
        	// 
        	// mmOpenShell
        	// 
        	this.mmOpenShell.Index = 4;
        	this.mmOpenShell.Text = "Open Shell";
        	this.mmOpenShell.Click += new System.EventHandler(this.mmOpenShell_Click);
        	// 
        	// mmSplitter3
        	// 
        	this.mmSplitter3.Index = 5;
        	this.mmSplitter3.Text = "-";
        	// 
        	// mmNewFile
        	// 
        	this.mmNewFile.Index = 6;
        	this.mmNewFile.Text = "New";
        	this.mmNewFile.Click += new System.EventHandler(this.mmNewFile_Click);
        	// 
        	// mmRemoveFile
        	// 
        	this.mmRemoveFile.Index = 7;
        	this.mmRemoveFile.Text = "Remove";
        	this.mmRemoveFile.Click += new System.EventHandler(this.mmRemoveFile_Click);
        	// 
        	// splitter1
        	// 
        	this.splitter1.Location = new System.Drawing.Point(192, 0);
        	this.splitter1.Name = "splitter1";
        	this.splitter1.Size = new System.Drawing.Size(7, 361);
        	this.splitter1.TabIndex = 1;
        	this.splitter1.TabStop = false;
        	// 
        	// tvFolders
        	// 
        	this.tvFolders.AllowDrop = true;
        	this.tvFolders.ContextMenu = this.cmFolders;
        	this.tvFolders.Dock = System.Windows.Forms.DockStyle.Left;
        	this.tvFolders.HideSelection = false;
        	this.tvFolders.LabelEdit = true;
        	this.tvFolders.Location = new System.Drawing.Point(0, 0);
        	this.tvFolders.Name = "tvFolders";
        	this.tvFolders.Size = new System.Drawing.Size(192, 361);
        	this.tvFolders.TabIndex = 0;
        	this.tvFolders.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.tvFolders_QueryContinueDrag);
        	this.tvFolders.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvFolders_DragDrop);
        	this.tvFolders.DragOver += new System.Windows.Forms.DragEventHandler(this.tvFolders_DragOver);
        	this.tvFolders.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvFolders_AfterLabelEdit);
        	this.tvFolders.AfterSelect += new TreeViewEventHandler(this.tvFolders_AfterSelect);
        	this.tvFolders.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvFolders_MouseUp);
        	this.tvFolders.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tvFolders_MouseMove);
        	this.tvFolders.BeforeSelect += new TreeViewCancelEventHandler(this.tvFolders_BeforeSelect);
        	this.tvFolders.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvFolders_MouseDown);
        	// 
        	// cmFolders
        	// 
        	this.cmFolders.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
        	        	        	this.mmNewFolder,
        	        	        	this.mmDeleteFolder,
        	        	        	this.mmRenameFolder});
        	this.cmFolders.Popup += new System.EventHandler(this.cmFolders_Popup);
        	// 
        	// mmNewFolder
        	// 
        	this.mmNewFolder.Index = 0;
        	this.mmNewFolder.Text = "New";
        	this.mmNewFolder.Click += new System.EventHandler(this.mmNewFolder_Click);
        	// 
        	// mmDeleteFolder
        	// 
        	this.mmDeleteFolder.Index = 1;
        	this.mmDeleteFolder.Text = "Delete";
        	this.mmDeleteFolder.Click += new System.EventHandler(this.mmDeleteFolder_Click);
        	// 
        	// mmRenameFolder
        	// 
        	this.mmRenameFolder.Index = 2;
        	this.mmRenameFolder.Text = "Rename";
        	this.mmRenameFolder.Click += new System.EventHandler(this.mmRenameFolder_Click);
        	// 
        	// menu
        	// 
        	this.menu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
        	        	        	this.mmFile});
        	// 
        	// mmFile
        	// 
        	this.mmFile.Index = 0;
        	this.mmFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
        	        	        	this.mmExit});
        	this.mmFile.Text = "&File";
        	// 
        	// mmExit
        	// 
        	this.mmExit.Index = 0;
        	this.mmExit.Text = "&Exit";
        	this.mmExit.Click += new System.EventHandler(this.mmExit_Click);
        	// 
        	// statusBar1
        	// 
        	this.statusBar1.Location = new System.Drawing.Point(0, 395);
        	this.statusBar1.Name = "statusBar1";
        	this.statusBar1.ShowPanels = true;
        	this.statusBar1.Size = new System.Drawing.Size(729, 23);
        	this.statusBar1.TabIndex = 0;
        	this.statusBar1.Text = "statusBar";
        	// 
        	// FormFavorites
        	// 
        	this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
        	this.ClientSize = new System.Drawing.Size(729, 418);
        	this.Controls.Add(this.statusBar1);
        	this.Controls.Add(this.panel1);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
        	this.MaximizeBox = false;
        	this.Menu = this.menu;
        	this.MinimizeBox = false;
        	this.Name = "FormFavorites";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "Favourites";
        	this.Closed += new System.EventHandler(this.FormFavorites_Closed);
        	this.Load += new System.EventHandler(this.FrmFavorites_Load);
        	this.panel1.ResumeLayout(false);
        	this.panel2.ResumeLayout(false);
        	this.ResumeLayout(false);
		}
        #endregion

        internal FormFavorites() {
            InitializeComponent();

            lvFiles.LargeImageList = fFileIconManager.ImageListLarge;
            lvFiles.SmallImageList = fFileIconManager.ImageListSmall;
            tvFolders.ImageList = fFileIconManager.ImageListSmall;
            tvFolders.ImageIndex = fFileIconManager.GetImageIndex(Environment.GetFolderPath(Environment.SpecialFolder.System));
            tvFolders.SelectedImageIndex = fFileIconManager.GetImageIndex(Environment.GetFolderPath(Environment.SpecialFolder.System), true);

            table.Add("bdsproj", ExtensionType.Project);
            table.Add("csproj", ExtensionType.Project);
            table.Add("vbproj", ExtensionType.Project);
            table.Add("dproj", ExtensionType.Project);
            table.Add("cbproj", ExtensionType.Project);
            table.Add("bdsgroup", ExtensionType.ProjectGroup);
            table.Add("groupproj", ExtensionType.ProjectGroup);
            table.Add("html", ExtensionType.Web);
            table.Add("htm", ExtensionType.Web);
            table.Add("cs", ExtensionType.CSharp);
            table.Add("resources", ExtensionType.Resource);
            table.Add("resx", ExtensionType.Resource);
            table.Add("txt", ExtensionType.Text);
            table.Add("aspx", ExtensionType.Web);
            table.Add("ascx", ExtensionType.Web);
            table.Add("Global.asax", ExtensionType.XML);
            table.Add("asmx", ExtensionType.CSharp);
            table.Add("App.config", ExtensionType.XML);
            table.Add("Web.config", ExtensionType.XML);
            table.Add("xml", ExtensionType.XML);
            table.Add("dll", ExtensionType.Executable);
            table.Add("exe", ExtensionType.Executable);
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

        private enum ExtensionType {Project = 1, ProjectGroup, Web, CSharp, Resource, Text, XML, Executable};

        private TreeNode fRightClickNode;
        private Rectangle fDragRectangle = Rectangle.Empty;
        private Point fScreenOffset;
        private FileIconManager fFileIconManager = new FileIconManager();
        private IDictionary<string, ExtensionType> table = new Dictionary<string, ExtensionType>();

        private void cmFiles_Popup(object sender, System.EventArgs e) {
            mmAddFileToProject.Text = "Add to project";
            mmAddFileToProject.Enabled = true;
            mmOpenFile.Enabled = true;
            mmOpenShell.Enabled = true;
            if (lvFiles.SelectedItems.Count > 0) {
                switch (GetFileExtensionType(lvFiles.SelectedItems[0].SubItems[1].Text)) {
                    case ExtensionType.Project:
                        mmAddFileToProject.Text = "Add to project group";
                        mmOpenShell.Enabled = false;
                        break;
                    case ExtensionType.Web:
                    case ExtensionType.CSharp:
                    case ExtensionType.Text:
                    case ExtensionType.XML:
                        break;
                    case ExtensionType.Executable:
                    case ExtensionType.Resource:
                        mmOpenFile.Enabled = false;
                        break;
                    case ExtensionType.ProjectGroup:
                        mmAddFileToProject.Enabled = false;
                        mmOpenShell.Enabled = false;
                        mmAddFileToProject.Enabled = false;
                        break;
                    default:
                        break;
                }
            }
            else {
                mmAddFileToProject.Text = "Add to project";
                mmAddFileToProject.Enabled = false;
                mmOpenFile.Enabled = false;
                mmOpenShell.Enabled = false;
            }
            mmNewFile.Enabled = (tvFolders.SelectedNode != null);
            mmRemoveFile.Enabled = (lvFiles.SelectedItems.Count > 0);
        }

        private void cmFolders_Popup(object sender, System.EventArgs e) {
            mmDeleteFolder.Enabled = (tvFolders.SelectedNode != null);
            mmRenameFolder.Enabled = (tvFolders.SelectedNode != null);
            mmNewFolder.Enabled = true;
        }

        private void DoAddFolder() {
            TreeNode _TreeNode =  new TreeNode("New Folder");

            if (fRightClickNode != null) {
                fRightClickNode.Nodes.Add(_TreeNode);
            } else {
                tvFolders.Nodes.Add(_TreeNode);
            }
            tvFolders.SelectedNode = _TreeNode;
            DoRenameFolder(_TreeNode);
        }

        private static void DoDeleteFolder(TreeNode folderNode) {
            if (folderNode != null) {
                folderNode.Remove();
            }
        }

        private static void DoRenameFolder(TreeNode folderNode) {
            if ((folderNode != null) && (!folderNode.IsEditing)) {
                folderNode.BeginEdit();
            }
        }

		private void FrmFavorites_Load(object sender, System.EventArgs e) {
			if (!DesignMode) {
				LoadFavorites();
                MyView = lvFiles.View;
            }
        }

        private ExtensionType GetFileExtensionType(string aFile) {
			string _Extentions = Path.GetExtension(aFile);
            if (_Extentions.Length > 0) {
                _Extentions = _Extentions.Substring(1, _Extentions.Length-1);
                if (table.ContainsKey(_Extentions)) {
                    return table[_Extentions];
                }
            }
			return ExtensionType.CSharp;
        }

        private TreeNode GetRightClickOrSelectedNode() {
            return fRightClickNode != null ? fRightClickNode : tvFolders.SelectedNode;
        }

		private void LoadFavorites() {
            tvFolders.Nodes.Clear();
			Feature.FavoriteRecords _Favorites =
				Feature.FavoritesFeature.GetOptions();
			LoadCategories(_Favorites.FavoriteCategories, null);
			tvFolders.Sorted = true;

			// Set Selected Node
			string _Path =
				_Favorites.SelectedFavoriteCategoryPath;
			if (!String.IsNullOrEmpty(_Path)) {
				string[] _StringNodes = _Favorites.SelectedFavoriteCategoryPath.Split(new char[]{tvFolders.PathSeparator[0]});
				TreeNode _SelectedNode = null;
				TreeNodeCollection _Nodes = tvFolders.Nodes;
				foreach (string _string in _StringNodes) {
					foreach (TreeNode _Node in _Nodes) {
						if (_Node.Text == _string) {
							_SelectedNode = _Node;
							_Nodes = _Node.Nodes;
							break;
						}
					}
				}
				tvFolders.SelectedNode = _SelectedNode;
			}
		}

		private void LoadCategories(Feature.FavoriteCategory[] aFavoriteCategories, TreeNode aTreeNode) {
			TreeNodeCollection _Nodes = aTreeNode == null ? tvFolders.Nodes : aTreeNode.Nodes;
			if (aFavoriteCategories != null) {
				foreach (Feature.FavoriteCategory _FavoriteCategory in aFavoriteCategories) {
					TreeNode _TreeNode = new TreeNode();
					_Nodes.Add(_TreeNode);
					_TreeNode.Text = _FavoriteCategory.Name;
					LoadCategories(_FavoriteCategory.FavoriteCategories, _TreeNode);
					_TreeNode.Tag = _FavoriteCategory.FavoriteFiles;
					if (_FavoriteCategory.Expanded) {
						_TreeNode.Expand();
					}
				}
			}
		}

		private void mmAddFileToProject_Click(object sender, System.EventArgs e) {
			if (mmAddFileToProject.Text == "Add to project group") {
				IOTAActionService _IOTAActionService = OtaUtils.GetActionService();
				if (_IOTAActionService != null) {
					foreach (ListViewItem _ListViewItem in lvFiles.SelectedItems) {
						_IOTAActionService.OpenProject(_ListViewItem.SubItems[1].Text, false);
					}
				}
			} else {
				IOTAProject _IOTAProject = OtaUtils.GetCurrentProject();
				if (_IOTAProject != null) {
					foreach (ListViewItem _ListViewItem in lvFiles.SelectedItems) {
						_IOTAProject.AddFile(_ListViewItem.SubItems[1].Text);
					}
				}
			}
		}

		private void mmDeleteFolder_Click(object sender, System.EventArgs e) {
			DoDeleteFolder(GetRightClickOrSelectedNode());
		}

		private void mmDetails_Click(object sender, System.EventArgs e) {
			MyView = View.Details;
		}

		private void mmExit_Click(object sender, System.EventArgs e) {
			this.Close();
		}

        const string filter = "Borland Developer Studio File (*.dproj;*.cbproj;*.vbproj;*.csproj;*.groupproj;*.resx;App.config;*.bdsproj;*.bdsgroup;*.html;*.htm;*.cs;*.resources;*.txt;*.aspx;*.ascx;Global.asax;*.asmx;Web.config)|*.dproj;*.cbproj;*.vbproj;*.csproj;*.groupproj;*.resx;App.config;*.bdsproj;*.bdsgroup;*.html;*.htm;*.cs;*.resources;*.txt;*.aspx;*.ascx;Global.asax;*.asmx;Web.config"
            + "|Borland Developer Studio Project (*.dproj;*.cbproj;*.vbproj;*.csproj;*.bdsproj)|*.dproj;*.cbproj;*.vbproj;*.csproj;*.bdsproj"
            + "|Borland Developer Studio Project Group (*.groupproj;*.bdsgroup)|*.groupproj;*.bdsgroup"
            + "|HTML file (*.html;*.htm)|*.html;*.htm"
            + "|C# file (*.cs)|*.cs"
            + "|XML file (*.xml)|*.xml"
            + "|Assemblies (*.dll)|*.dll"
            + "|Executables (*.exe)|*.exe"
            + "|Resources (*.resx;*.resources)|*.resx;*.resources"
            + "|Text file (*.txt)|*.txt"
            + "|ASP.NET page (*.aspx)|*.aspx"
            + "|ASP.NET User control (*.ascx)|*.ascx"
            + "|Global.asax (Global.asax)|Global.asax"
            + "|ASP.NET Web Services (*.asmx)|*.asmx"
            + "|Web.config (Web.config)|Web.config"
            + "|App.config (App.config)|App.config"
            + "|Any file (*.*)|*.*";

		private void mmNewFile_Click(object sender, System.EventArgs e) {
			Vista_Api.OpenFileDialog _OpenFileDialog = new Vista_Api.OpenFileDialog();

			_OpenFileDialog.CheckFileExists = true;
            _OpenFileDialog.Filter = filter;
            _OpenFileDialog.FilterIndex = 0 ;
            IOTAProject _IOTAProject = OtaUtils.GetCurrentProject();
            string _Dir = string.Empty;
            if (_IOTAProject != null) {
                _Dir = Path.GetDirectoryName(_IOTAProject.FileName);
            }
            if (string.IsNullOrEmpty(_Dir)) {
                _Dir = Directory.GetCurrentDirectory();
            }
            _OpenFileDialog.InitialDirectory = _Dir;
            _OpenFileDialog.Multiselect = true;
            _OpenFileDialog.ShowReadOnly = false;
            _OpenFileDialog.Title = "Add to Favorites";

            if(_OpenFileDialog.ShowDialog() == DialogResult.OK) {
                foreach (string _FileName in _OpenFileDialog.FileNames) {
                    ListViewItem _ListViewItem = lvFiles.Items.Add(Path.GetFileNameWithoutExtension(_FileName));
                    _ListViewItem.SubItems.Add(_FileName);
                    _ListViewItem.ImageIndex = fFileIconManager.GetImageIndex(_FileName);
                }
            }
        }

        private void mmNewFolder_Click(object sender, System.EventArgs e) {
            DoAddFolder();
        }

        private void mmOpenFile_Click(object sender, System.EventArgs e) {
			IOTAActionService _IOTAActionService = OtaUtils.GetActionService();
            if (_IOTAActionService != null) {
                foreach (ListViewItem _ListViewItem in lvFiles.SelectedItems) {
                    _IOTAActionService.OpenFile(_ListViewItem.SubItems[1].Text);
                }
            }
        }

        private void mmOpenShell_Click(object sender, System.EventArgs e) {
            foreach (ListViewItem _ListViewItem in lvFiles.SelectedItems) {
                System.Diagnostics.Process.Start(_ListViewItem.SubItems[1].Text);
            }
        }

        private void mmLarge_Click(object sender, System.EventArgs e) {
            MyView = View.LargeIcon;
        }

        private void mmList_Click(object sender, System.EventArgs e) {
            MyView = View.List;
        }

        private void mmRemoveFile_Click(object sender, System.EventArgs e) {
            foreach (ListViewItem _ListViewItem in lvFiles.SelectedItems) {
                _ListViewItem.Remove();
            }
        }

        private void mmRenameFolder_Click(object sender, System.EventArgs e) {
            DoRenameFolder(GetRightClickOrSelectedNode());
        }

        private void mmSmall_Click(object sender, System.EventArgs e) {
            MyView = View.SmallIcon;
        }

		private Feature.FavoriteCategory[] SaveCategories(TreeNode aTreeNode) {
            TreeNodeCollection _Nodes = aTreeNode == null ? tvFolders.Nodes : aTreeNode.Nodes;
			Feature.FavoriteCategory[] FavoriteCategories =
				new Feature.FavoriteCategory[_Nodes.Count];
            for (int i = 0; i < _Nodes.Count; i++) {
				FavoriteCategories[i] = new Feature.FavoriteCategory();
                FavoriteCategories[i].Name = _Nodes[i].Text;
                FavoriteCategories[i].Expanded = _Nodes[i].IsExpanded;
                FavoriteCategories[i].FavoriteCategories = SaveCategories(_Nodes[i]);
				FavoriteCategories[i].FavoriteFiles = (Feature.FavoriteFile[])_Nodes[i].Tag;
            }
            return FavoriteCategories;
        }

		private void SaveFavorites() {
			tvFolders_BeforeSelect(null, null);
			Feature.FavoritesFeature.GetOptions().FavoriteCategories = SaveCategories(null);
			Feature.FavoritesFeature.GetOptions().SelectedFavoriteCategoryPath = tvFolders.SelectedNode != null? tvFolders.SelectedNode.FullPath : "";
			ILoadableFeature feature = FeatureRegistry.GetFeature("Lextm.WiseEditor.Feature.FavoritesFeature");
			if (feature != null) {
				feature.SavePreferences();
            }
        }

        private void tvFolders_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e) {
            if (e.Label != null) {
                if (e.Label.Length == 0) {
                    e.CancelEdit = true;
                    Lextm.Windows.Forms.MessageBoxFactory.Warn(Text, "Invalid folder name", "The name cannot be empty");
                    e.Node.BeginEdit();
                }
            }
        }

        private void tvFolders_AfterSelect(object sender, TreeViewEventArgs e) {
            TreeNode _TreeNode = e.Node;
			if (_TreeNode.Tag is Feature.FavoriteFile[]) {
				Feature.FavoriteFile[] _FavoriteFiles =
				    (Feature.FavoriteFile[])_TreeNode.Tag;
				foreach (Feature.FavoriteFile _FavoriteFile in _FavoriteFiles) {
                    ListViewItem _ListViewItem = new ListViewItem();
                    _ListViewItem.Text = _FavoriteFile.Name;
                    _ListViewItem.SubItems.Add(_FavoriteFile.FileName);
                    _ListViewItem.ImageIndex = fFileIconManager.GetImageIndex(_FavoriteFile.FileName);
                    lvFiles.Items.Add(_ListViewItem);
                }
            }
        }

        private void tvFolders_BeforeSelect(object sender, TreeViewCancelEventArgs e) {
            TreeNode _TreeNode = tvFolders.SelectedNode;
            if (_TreeNode != null) {
				Feature.FavoriteFile[] _FavoriteFiles =
				    new Feature.FavoriteFile[lvFiles.Items.Count];
                for (int i = 0; i < lvFiles.Items.Count; i++) {
					Feature.FavoriteFile _FavoriteFile =
					    new Feature.FavoriteFile();
                    _FavoriteFile.Name = lvFiles.Items[i].Text;
                    _FavoriteFile.FileName = lvFiles.Items[i].SubItems[1].Text;
                    _FavoriteFiles[i] = _FavoriteFile;
                }
                _TreeNode.Tag = _FavoriteFiles;
            }
            lvFiles.Items.Clear();
        }

        private void tvFolders_DragDrop(object sender, System.Windows.Forms.DragEventArgs e) {
            if (e.Data.GetDataPresent(typeof(TreeNode))) {
                TreeNode _DragNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
                Point _Point = tvFolders.PointToClient(new Point(e.X, e.Y));
                TreeNode _TreeNode = tvFolders.GetNodeAt(_Point.X, _Point.Y);
                if (_TreeNode != _DragNode) {
                    _DragNode.Remove();
                    if (_TreeNode == null) {
                        tvFolders.Nodes.Add(_DragNode);
                    } else {
                        _TreeNode.Nodes.Add(_DragNode);
                    }
                    _TreeNode.Expand();
                }
            }
        }

        private void tvFolders_DragOver(object sender, System.Windows.Forms.DragEventArgs e) {
            if (e.Data.GetDataPresent(typeof(TreeNode))) {
                e.Effect = DragDropEffects.None;
                TreeNode _DragNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
                Point _Point = tvFolders.PointToClient(new Point(e.X, e.Y));
                TreeNode _TreeNode = tvFolders.GetNodeAt(_Point.X, _Point.Y);
                // not over a node, allow move to root
                if (_TreeNode == null) {
                    if (_DragNode.Parent != null) {
                        e.Effect = DragDropEffects.Move;
                    }
                } else {
                    if ((_TreeNode != _DragNode.Parent) && (_TreeNode != _DragNode)) {
                        while (_TreeNode.Parent != null) {
                            if (_TreeNode.Parent == _DragNode) {
                                return;
                            } else {
                                _TreeNode = _TreeNode.Parent;
                            }
                        }
                        e.Effect = DragDropEffects.Move;
                    }
                }
            }
        }

        private void tvFolders_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
            fRightClickNode = tvFolders.GetNodeAt(e.X, e.Y);
            if (fRightClickNode != null) {
                // Remember the point where the mouse down occurred. The DragSize indicates
                // the size that the mouse can move before a drag event should be started.
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                fDragRectangle = new Rectangle(new Point(e.X - (dragSize.Width /2),
                                                         e.Y - (dragSize.Height /2)), dragSize);
            } else {
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                fDragRectangle = Rectangle.Empty;
            }
        }

        private void tvFolders_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                // If the mouse moves outside the rectangle, start the drag.
                if (fDragRectangle != Rectangle.Empty &&
                    !fDragRectangle.Contains(e.X, e.Y)) {
                    // The fScreenOffset is used to account for any desktop bands
                    // that may be at the top or left side of the screen when
                    // determining when to cancel the drag drop operation.
                    fScreenOffset = SystemInformation.WorkingArea.Location;

                    // Proceed with the drag and drop, passing in the list item.
                    tvFolders.DoDragDrop(fRightClickNode, DragDropEffects.Move);
                }
            }
        }

        private void tvFolders_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
            // Reset the drag rectangle when the mouse button is raised.
            fDragRectangle = Rectangle.Empty;
        }

        private void tvFolders_QueryContinueDrag(object sender, System.Windows.Forms.QueryContinueDragEventArgs e) {
            // Cancel the drag if the mouse moves off the form.
            Vista_Api.TreeView _TreeView = sender as Vista_Api.TreeView;
            if (_TreeView != null) {
                Form _Form = _TreeView.FindForm();
                // Cancel the drag if the mouse moves off the form. The fScreenOffset
                // takes into account any desktop bands that may be at the top or left
                // side of the screen.
                if (((Control.MousePosition.X - fScreenOffset.X) < _Form.DesktopBounds.Left) ||
                    ((Control.MousePosition.X - fScreenOffset.X) > _Form.DesktopBounds.Right) ||
                    ((Control.MousePosition.Y - fScreenOffset.Y) < _Form.DesktopBounds.Top) ||
                    ((Control.MousePosition.Y - fScreenOffset.Y) > _Form.DesktopBounds.Bottom)) {
                    e.Action = DragAction.Cancel;
                }
            }
        }

        // *********************************************************************
        //                           Private Properties
        // *********************************************************************
        private View MyView {
            set {
                lvFiles.View = value;
                mmLarge.Checked = View.LargeIcon == value;
                mmSmall.Checked = View.SmallIcon == value;
                mmList.Checked = View.List == value;
                mmDetails.Checked = View.Details == value;
            }
        }

        // *********************************************************************
        //                           Public
        // *********************************************************************
        //public Vista_Api.ListView[] GetListViews() {
        //    return new Vista_Api.ListView[] {lvFiles};
        //}
		
		private void lvFiles_DoubleClick(object sender, System.EventArgs e)
		{
            mmOpenFile_Click(null, null);
		}
		
		private void FormFavorites_Closed(object sender, System.EventArgs e)
		{
			if (!DesignMode) {
				SaveFavorites();
			}
		}
    }
}
