// this is the form shortcuts class.
// Copyright (C) 2005-2006  Lex Y. Li
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
using System.Windows.Forms;
using Lextm.OpenTools;
using BeWise.Common.Utils;
using Lextm.OpenTools.Elements;
using System.Collections.Generic;

namespace Lextm.CodeBeautifierCollection.Gui {
	/// <summary>
	/// Shortcuts form.
	///</summary>
	sealed class FormShortcuts : System.Windows.Forms.Form {
		
		/// <summary>
		/// Constructor.
		/// </summary>
		internal FormShortcuts() {
			InitializeComponent();
			
			foreach (string name in table.Keys) {
				cbMenuNames.Items.Add(name);
			}

			textBox1.Visible = false;
			label2.Visible = false;
		}

		///<summary>
		///Cleans up any resources being used.
		///</summary>
		protected override void Dispose (bool disposing) {
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox textBox1;

		#region Windows Form Designer generated code
		///<summary>
		///Required method for Designer support - do not modify
		///the contents of this method with the code editor.
		///</summary>
		private void InitializeComponent()
		{
			this.btOK = new System.Windows.Forms.Button();
			this.cbMenuNames = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.gbShortcuts = new System.Windows.Forms.GroupBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btCancel = new System.Windows.Forms.Button();
			this.gbShortcuts.SuspendLayout();
			this.SuspendLayout();
			// 
			// btOK
			// 
			this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOK.Location = new System.Drawing.Point(72, 168);
			this.btOK.Name = "btOK";
			this.btOK.Size = new System.Drawing.Size(90, 23);
			this.btOK.TabIndex = 0;
			this.btOK.Text = "OK";
			this.btOK.Click += new System.EventHandler(this.btOKClick);
			// 
			// cbMenuNames
			// 
			this.cbMenuNames.Location = new System.Drawing.Point(150, 45);
			this.cbMenuNames.Name = "cbMenuNames";
			this.cbMenuNames.Size = new System.Drawing.Size(145, 21);
			this.cbMenuNames.TabIndex = 2;
			this.cbMenuNames.Text = "(choose)";
			this.cbMenuNames.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbValueKeyDown);
			this.cbMenuNames.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbValueKeyPress);
			this.cbMenuNames.SelectedIndexChanged += new System.EventHandler(this.CbMenuNamesSelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(58, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(85, 23);
			this.label1.TabIndex = 3;
			this.label1.Text = "Menu Name:";
			// 
			// gbShortcuts
			// 
			this.gbShortcuts.Controls.Add(this.textBox1);
			this.gbShortcuts.Controls.Add(this.label2);
			this.gbShortcuts.Controls.Add(this.cbMenuNames);
			this.gbShortcuts.Controls.Add(this.label1);
			this.gbShortcuts.Location = new System.Drawing.Point(14, 12);
			this.gbShortcuts.Name = "gbShortcuts";
			this.gbShortcuts.Size = new System.Drawing.Size(363, 140);
			this.gbShortcuts.TabIndex = 2;
			this.gbShortcuts.TabStop = false;
			this.gbShortcuts.Text = "Shortcuts Settings";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(150, 88);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(145, 21);
			this.textBox1.TabIndex = 5;
			this.textBox1.Text = "";
			this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbValueKeyDown);
			this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbValueKeyPress);
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(58, 93);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(85, 23);
			this.label2.TabIndex = 4;
			this.label2.Text = "Shortcut:";
			this.label2.Visible = false;
			// 
			// btCancel
			// 
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Location = new System.Drawing.Point(220, 168);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(90, 23);
			this.btCancel.TabIndex = 1;
			this.btCancel.Text = "Cancel";
			this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
			// 
			// FormShortcuts
			// 
			this.AcceptButton = this.btOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(391, 200);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.gbShortcuts);
			this.Controls.Add(this.btOK);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormShortcuts";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Code Beautifier Collection Shortcuts";
			this.TopMost = true;
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbValueKeyDown);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbValueKeyPress);
			this.gbShortcuts.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button btOK;
		private System.Windows.Forms.Button btCancel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox gbShortcuts;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbMenuNames;
		#endregion

		void btOKClick(object sender, System.EventArgs e)
		{
			ShortcutService.SaveShortcuts();
			ShortcutService.RefreshShortcuts();
			this.Hide();
		}

		void CbMenuNamesSelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cbMenuNames.SelectedIndex == -1) {
				// no selection
				textBox1.Visible = false;
				label2.Visible = false;
				Lextm.Windows.Forms.MessageBoxFactory.Warn(Text, "No selection", "Please select an item.");
				return;
			}
			if (table.ContainsKey(cbMenuNames.SelectedItem.ToString()))
			{
				textBox1.Text = OtaUtils.GetKeysTextFromShortcut(
					table[cbMenuNames.SelectedItem.ToString()].Shortcut
				);
				textBox1.Visible = true;
				label2.Visible = true;
			}
		}
		private	IDictionary<string, Lextm.OpenTools.Elements.ShortcutRecord> table =
			ShortcutService.Instance.Table;

		void TbValueKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			int tmp;
			// Determine whether one of the keys entered is the Alt/Ctrl/Shift.
			if (e.Alt || e.Control || e.Shift) {

				tmp = OtaUtils.GetShortcutFromKeys(e);
				table[cbMenuNames.SelectedItem.ToString()].Shortcut = tmp;
				textBox1.Text = OtaUtils.GetKeysTextFromShortcut(tmp);
			}
		}

		void TbValueKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			e.Handled = true;
		}
		
		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
			if (String.IsNullOrEmpty(textBox1.Text)) {
				(table[cbMenuNames.SelectedItem.ToString()]
				 as ShortcutRecord).Shortcut = 0;
				textBox1.Text = OtaUtils.GetKeysTextFromShortcut(0);
			}
		}
		
		private void btCancel_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

	}
}
