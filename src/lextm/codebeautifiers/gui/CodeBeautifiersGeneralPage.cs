// This is CB General tab page.
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
using System;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Lextm.OpenTools;


namespace Lextm.CodeBeautifiers.Gui {
	/// <summary>
	/// General tab page for CB Plus.
	/// </summary>
	public class CodeBeautifiersGeneralPage : Lextm.OpenTools.Gui.CustomPage
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckBox cbFormatAfterOpen;
		private System.Windows.Forms.GroupBox groupBox1;
		/// <summary>
		/// Constructor.
		/// </summary>
		public CodeBeautifiersGeneralPage()
		{
			// This call is required by the Windows.Forms Designer.
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{	components.Dispose();}
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
			this.cbFormatAfterOpen = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// cbFormatAfterOpen
			// 
			this.cbFormatAfterOpen.Location = new System.Drawing.Point(16, 24);
			this.cbFormatAfterOpen.Name = "cbFormatAfterOpen";
			this.cbFormatAfterOpen.Size = new System.Drawing.Size(136, 24);
			this.cbFormatAfterOpen.TabIndex = 1;
			this.cbFormatAfterOpen.Text = "Format after open";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cbFormatAfterOpen);
			this.groupBox1.Location = new System.Drawing.Point(16, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(176, 64);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Options";
			// 
			// CodeBeautifiersGeneralPage
			// 
			this.Controls.Add(this.groupBox1);
			this.Name = "CodeBeautifiersGeneralPage";
			this.Size = new System.Drawing.Size(432, 232);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion

		/// <summary>
		/// Sets preferences to UI.
        /// </summary>
        public override void PreferencesToUI( ) {
            base.PreferencesToUI();

            this.cbFormatAfterOpen.Checked = (bool)PropertyRegistry.Get("FormatAfterOpen", false);
		}
        /// <summary>
        /// Sets UI to preferences.
        /// </summary>
        public override void UIToPreferences( ) {
            base.UIToPreferences();

            PropertyRegistry.Set("FormatAfterOpen", cbFormatAfterOpen.Checked);


		}
	}
}
