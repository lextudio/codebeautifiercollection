using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BeWise.Common.Utils;
using Lextm.OpenTools;
using Lextm.OpenTools.CodeDom;

namespace Lextm.BagPlug.Gui
{
	/// <summary>
	/// Summary description for WinForm.
	/// </summary>
	class FormSourceNavigator : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ComboBox cbMembers;
		private System.Windows.Forms.ComboBox cbTypes;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.PictureBox pbMethodType;
		private System.Windows.Forms.PictureBox pbTypeType;
		private System.Windows.Forms.Button btTransparent;

		internal FormSourceNavigator()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            if (Environment.OSVersion.Version.Major >= 6)
            {
                MinimumSize = new Size(400, 58);
                Height = 58;
            } 
            else 
            {
                MinimumSize = new Size(400, 48);
                Height = 48;
            }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSourceNavigator));
            this.pbMethodType = new System.Windows.Forms.PictureBox();
            this.pbTypeType = new System.Windows.Forms.PictureBox();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.btTransparent = new System.Windows.Forms.Button();
            this.cbMembers = new System.Windows.Forms.ComboBox();
            this.cbTypes = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbMethodType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTypeType)).BeginInit();
            this.SuspendLayout();
            // 
            // pbMethodType
            // 
            this.pbMethodType.Location = new System.Drawing.Point(160, 2);
            this.pbMethodType.Name = "pbMethodType";
            this.pbMethodType.Size = new System.Drawing.Size(16, 16);
            this.pbMethodType.TabIndex = 2;
            this.pbMethodType.TabStop = false;
            // 
            // pbTypeType
            // 
            this.pbTypeType.Location = new System.Drawing.Point(0, 2);
            this.pbTypeType.Name = "pbTypeType";
            this.pbTypeType.Size = new System.Drawing.Size(16, 16);
            this.pbTypeType.TabIndex = 3;
            this.pbTypeType.TabStop = false;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            this.imageList.Images.SetKeyName(3, "");
            this.imageList.Images.SetKeyName(4, "");
            this.imageList.Images.SetKeyName(5, "");
            this.imageList.Images.SetKeyName(6, "");
            this.imageList.Images.SetKeyName(7, "");
            this.imageList.Images.SetKeyName(8, "");
            this.imageList.Images.SetKeyName(9, "");
            this.imageList.Images.SetKeyName(10, "");
            this.imageList.Images.SetKeyName(11, "");
            this.imageList.Images.SetKeyName(12, "");
            this.imageList.Images.SetKeyName(13, "");
            this.imageList.Images.SetKeyName(14, "");
            this.imageList.Images.SetKeyName(15, "");
            this.imageList.Images.SetKeyName(16, "");
            this.imageList.Images.SetKeyName(17, "");
            this.imageList.Images.SetKeyName(18, "");
            this.imageList.Images.SetKeyName(19, "");
            this.imageList.Images.SetKeyName(20, "");
            this.imageList.Images.SetKeyName(21, "");
            this.imageList.Images.SetKeyName(22, "");
            this.imageList.Images.SetKeyName(23, "");
            this.imageList.Images.SetKeyName(24, "");
            this.imageList.Images.SetKeyName(25, "");
            this.imageList.Images.SetKeyName(26, "");
            this.imageList.Images.SetKeyName(27, "");
            this.imageList.Images.SetKeyName(28, "");
            // 
            // btTransparent
            // 
            this.btTransparent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btTransparent.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btTransparent.Location = new System.Drawing.Point(448, 2);
            this.btTransparent.Name = "btTransparent";
            this.btTransparent.Size = new System.Drawing.Size(16, 16);
            this.btTransparent.TabIndex = 4;
            this.btTransparent.Text = "T";
            this.btTransparent.Click += new System.EventHandler(this.btTransparent_Click);
            // 
            // cbMembers
            // 
            this.cbMembers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMembers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbMembers.Location = new System.Drawing.Point(176, 0);
            this.cbMembers.Name = "cbMembers";
            this.cbMembers.Size = new System.Drawing.Size(272, 22);
            this.cbMembers.Sorted = true;
            this.cbMembers.TabIndex = 5;
            this.cbMembers.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbMembers_DrawItem);
            this.cbMembers.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.cbMembers_MeasureItem);
            this.cbMembers.SelectedIndexChanged += new System.EventHandler(this.cbMethods_SelectedIndexChanged);
            // 
            // cbTypes
            // 
            this.cbTypes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbTypes.Location = new System.Drawing.Point(16, 0);
            this.cbTypes.Name = "cbTypes";
            this.cbTypes.Size = new System.Drawing.Size(144, 22);
            this.cbTypes.Sorted = true;
            this.cbTypes.TabIndex = 6;
            this.cbTypes.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbMembers_DrawItem);
            this.cbTypes.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.cbMembers_MeasureItem);
            this.cbTypes.SelectedIndexChanged += new System.EventHandler(this.cbClasses_SelectedIndexChanged);
            // 
            // FormSourceNavigator
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(464, 22);
            this.Controls.Add(this.cbTypes);
            this.Controls.Add(this.cbMembers);
            this.Controls.Add(this.btTransparent);
            this.Controls.Add(this.pbTypeType);
            this.Controls.Add(this.pbMethodType);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 60);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 58);
            this.Name = "FormSourceNavigator";
            this.ShowInTaskbar = false;
            this.Text = "Source Navigator";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormSourceNavigator_Load);
            this.Activated += new System.EventHandler(this.FormSourceNavigator_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.pbMethodType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTypeType)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void FormSourceNavigator_Activated(object sender, System.EventArgs e)
		{
			cbTypes.Items.Clear();
			classList.Clear();
			CodeDomProvider.LoadTypeInfoInto(classList, OtaUtils.GetCurrentModule());
			foreach(TypeInfo type in classList) {
				cbTypes.Items.Add(new ComboBoxItemAdapter(type));
            }

			cbMembers.Items.Clear();
			methodList.Clear();
			CodeDomProvider.LoadMethodInfoInto(methodList, OtaUtils.GetCurrentModule());
			CodeDomProvider.LoadFieldInfoInto(methodList, OtaUtils.GetCurrentModule());
			CodeDomProvider.LoadEventInfoInto(methodList, OtaUtils.GetCurrentModule());
			foreach(CodeDomInfo c in methodList) {
				cbMembers.Items.Add(new ComboBoxItemAdapter(c));
			}
		}

        IList<CodeDomInfo> classList = new List<CodeDomInfo>();
        IList<CodeDomInfo> methodList = new List<CodeDomInfo>();
		private void cbMethods_SelectedIndexChanged(object sender, System.EventArgs e)
		{
        	// TODO: use ArtCSB implementation of Goto instead.
			(cbMembers.SelectedItem as ComboBoxItemAdapter).Item.Goto();
			pbMethodType.Image =
				imageList.Images[(cbMembers.SelectedItem as ComboBoxItemAdapter).IconIndex];
		}
		
		private void cbClasses_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			(cbTypes.SelectedItem as ComboBoxItemAdapter).Item.Goto();
			pbTypeType.Image = imageList.Images[(cbTypes.SelectedItem as ComboBoxItemAdapter).IconIndex];
            LoadMethodsFor((cbTypes.SelectedItem as ComboBoxItemAdapter).Item.FullName);       
		}

        private void LoadMethodsFor(string className)
        {
            cbMembers.Items.Clear();
            foreach (CodeDomInfo item in methodList)
            {
                if (item.FullName.StartsWith(className, StringComparison.Ordinal))
                {
                    cbMembers.Items.Add(new ComboBoxItemAdapter(item));
                }
            }
        }
		// font - has to be static - don't create on each draw
		static Font font = new Font("Arial", 8.25f);
		static StringFormat drawStringFormat = new StringFormat(StringFormatFlags.NoWrap);
		
		private void cbMembers_MeasureItem(object sender, System.Windows.Forms.MeasureItemEventArgs e)
		{ 
			ComboBox comboBox = (ComboBox)sender;
			if (e.Index >= 0) {
				ComboBoxItemAdapter item = (ComboBoxItemAdapter)comboBox.Items[e.Index];
				SizeF size = e.Graphics.MeasureString(item.ToString(), font);
				e.ItemWidth  = (int)size.Width;

				e.ItemHeight = (int)Math.Max(size.Height, imageList.ImageSize.Height);
			}
		}
		
		private void cbMembers_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			e.DrawBackground();
			
			if (e.Index >= 0) {
				ComboBoxItemAdapter item = (ComboBoxItemAdapter)comboBox.Items[e.Index];
				
				e.Graphics.DrawImageUnscaled(imageList.Images[item.IconIndex],
											 new Point(e.Bounds.X, e.Bounds.Y + (e.Bounds.Height - imageList.ImageSize.Height) / 2));
				Rectangle drawingRect = new Rectangle(e.Bounds.X + imageList.ImageSize.Width,
				                                      e.Bounds.Y,
				                                      e.Bounds.Width - imageList.ImageSize.Width,
				                                      e.Bounds.Height);
				
				Brush drawItemBrush = SystemBrushes.WindowText;
				if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) {
					drawItemBrush = SystemBrushes.HighlightText;
				} else {
					drawItemBrush = SystemBrushes.WindowText;
				}
				e.Graphics.DrawString(item.ToString(),
				                      font,
				                      drawItemBrush,
				                      drawingRect,
				                      drawStringFormat);
			}
			e.DrawFocusRectangle();
		}

		class ComboBoxItemAdapter// : System.IComparable
		{
			CodeDomInfo item;
			
			internal int IconIndex {
				get {
					return item.ImageIndex;
				}
			}
			
			internal CodeDomInfo Item {
				get {
					return item;
				}
			}

			internal ComboBoxItemAdapter(CodeDomInfo item)
			{
				this.item = item;
			}

			string cachedString;
			
			public override string ToString()
			{
				// ambience lookups can be expensive when the return type is
				// resolved on the fly.
				// Therefore, we need to cache the generated string because it is used
				// very often for the sorting.
				if (cachedString == null)
					cachedString = ToStringInternal();
				return cachedString;
			}
			
			string ToStringInternal()
			{
				return item.FullName;
			}
			
//			#region System.IComparable interface implementation
//			public int CompareTo(object obj)
//			{
//				return string.Compare(ToString(), obj.ToString(), StringComparison.OrdinalIgnoreCase);
//			}
//			#endregion
			
		}

		private void btTransparent_Click(object sender, System.EventArgs e)
		{
			PropertyRegistry.Set("SourceNavigatorTransparent",
				!(bool)PropertyRegistry.Get("SourceNavigatorTransparent"));
			SetOpacity((bool)PropertyRegistry.Get("SourceNavigatorTransparent"));
		}
		
		private void FormSourceNavigator_Load(object sender, System.EventArgs e)
		{
			SetOpacity(((bool)PropertyRegistry.Get("SourceNavigatorTransparent", false)));
		}

		private void SetOpacity(bool transparent) {
			if (transparent)
			{
				this.Opacity = 0.5;
			} else {
				this.Opacity = 1.0;
			}
		}
	}
}
