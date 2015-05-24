using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using PSTaskDialog;

namespace Lextm.Windows.Forms
{
	/// <summary>
	/// Message box factory.
	/// </summary>
	public sealed class MessageBoxFactory
	{
		private MessageBoxFactory() { }
		/// <summary>
		/// Shows debug.
		/// </summary>
		/// <param name="text"></param>
		[Conditional("DEBUG")]
		public static void Debug(object text)
		{
//            List<Vista_Api.CommandLink> links=new List<Vista_Api.CommandLink>();
//            Vista_Api.CommandLink cl = new Vista_Api.CommandLink(Vista_Api.CommandLink.DisplayStyle.Arrow, "Lorem Ipsum", "Lorem Ipsum");
//            cl = new Vista_Api.CommandLink(Vista_Api.CommandLink.DisplayStyle.Arrow, "Lorem Ipsum", "Lorem Ipsum");
//            cl.BackColor = SystemColors.Window;
//            cl.UseVisualStyleBackColor = false;
//            links.Add(cl);
//            cl = new Vista_Api.CommandLink(Vista_Api.CommandLink.DisplayStyle.Arrow, "Lorem Ipsum", "Lorem Ipsum");
//            cl.BackColor = SystemColors.Window;
//            cl.UseVisualStyleBackColor = false;
//            links.Add(cl);
//            cl = new Vista_Api.CommandLink(Vista_Api.CommandLink.DisplayStyle.Shield, "Lorem Ipsum", "Lorem Ipsum");
//            cl.BackColor = SystemColors.Window;
//                        cl.UseVisualStyleBackColor = false;
//            links.Add(cl);
//            Vista_Api.CommandDialog cd = new Vista_Api.CommandDialog(links);
//            cd.Title = "Lorem Ipsum";
//            cd.Description = "Lorem Ipsum";
//            cd.ShowDialog();			
			
//			List<Vista_Api.CommandLink> links=new List<Vista_Api.CommandLink>();
//			Vista_Api.CommandLink cl = new Vista_Api.CommandLink(Vista_Api.CommandLink.DisplayStyle.Arrow, "Lorem Ipsum", "Lorem Ipsum");
//			cl = new Vista_Api.CommandLink(Vista_Api.CommandLink.DisplayStyle.Arrow, "OK", "OK");
//			//cl.BackColor = SystemColors.Window;
//			cl.DialogResult = DialogResult.Yes;
//			links.Add(cl);
//			Vista_Api.CommandDialog cd = new Vista_Api.CommandDialog(links);
//			cd.Title = "Debug";
//			cd.Description = text.ToString();
//			cd.ShowDialog();
			MessageBox.Show(text.ToString(), 
                "Debug", 
                MessageBoxButtons.OK,
                MessageBoxIcon.Information, 
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
		}
		/// <summary>
		/// Shows info.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static void Info(object text)
		{
//			List<Vista_Api.CommandLink> links=new List<Vista_Api.CommandLink>();
//			Vista_Api.CommandLink cl = new Vista_Api.CommandLink(Vista_Api.CommandLink.DisplayStyle.Arrow, "Lorem Ipsum", "Lorem Ipsum");
//			cl = new Vista_Api.CommandLink(Vista_Api.CommandLink.DisplayStyle.Arrow, "OK");
//			cl.BackColor = SystemColors.Window;
//			cl.DialogResult = DialogResult.Yes;
//			links.Add(cl);
//			Vista_Api.CommandDialog cd = new Vista_Api.CommandDialog(links);
//			cd.Title = "Info";
//			cd.Description = text.ToString();
//			cd.ShowDialog();
			MessageBox.Show(text.ToString(),
			                "Info", 
			                MessageBoxButtons.OK,
			                MessageBoxIcon.Information,
			                MessageBoxDefaultButton.Button1,
			                MessageBoxOptions.DefaultDesktopOnly);
		}
		/// <summary>
		/// Shows warning.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static void Warn(object text)
		{
//			List<Vista_Api.CommandLink> links=new List<Vista_Api.CommandLink>();
//			Vista_Api.CommandLink cl = new Vista_Api.CommandLink(Vista_Api.CommandLink.DisplayStyle.Arrow, "Lorem Ipsum", "Lorem Ipsum");
//			cl = new Vista_Api.CommandLink(Vista_Api.CommandLink.DisplayStyle.Arrow, "OK");
//			cl.BackColor = SystemColors.Window;
//			cl.DialogResult = DialogResult.Yes;
//			links.Add(cl);
//			Vista_Api.CommandDialog cd = new Vista_Api.CommandDialog(links);
//			cd.Title = "Warn";
//			cd.Description = text.ToString();
//			cd.ShowDialog();
			MessageBox.Show(text.ToString(), 
			                "Warning", 
			                MessageBoxButtons.OK, 
			                MessageBoxIcon.Warning,
			                MessageBoxDefaultButton.Button1,
			                MessageBoxOptions.DefaultDesktopOnly);
		}
		/// <summary>
		/// Shows error.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static void Error(object text)
		{
//			List<Vista_Api.CommandLink> links=new List<Vista_Api.CommandLink>();
//			Vista_Api.CommandLink cl = new Vista_Api.CommandLink(Vista_Api.CommandLink.DisplayStyle.Arrow, "Lorem Ipsum", "Lorem Ipsum");
//			cl = new Vista_Api.CommandLink(Vista_Api.CommandLink.DisplayStyle.Arrow, "OK");
//			cl.BackColor = SystemColors.Window;
//			cl.DialogResult = DialogResult.Yes;
//			links.Add(cl);
//			Vista_Api.CommandDialog cd = new Vista_Api.CommandDialog(links);
//			cd.Title = "Error";
//			cd.Description = text.ToString();
//			cd.ShowDialog();
			MessageBox.Show(text.ToString(), 
			                "Error", 
			                MessageBoxButtons.OK, 
			                MessageBoxIcon.Error,
			                MessageBoxDefaultButton.Button1,
			                MessageBoxOptions.DefaultDesktopOnly);
		}
		/// <summary>
		/// Shows fatal.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static void Fatal(object text)
		{
//			List<Vista_Api.CommandLink> links=new List<Vista_Api.CommandLink>();
//			Vista_Api.CommandLink cl = new Vista_Api.CommandLink(Vista_Api.CommandLink.DisplayStyle.Arrow, "Lorem Ipsum", "Lorem Ipsum");
//			cl = new Vista_Api.CommandLink(Vista_Api.CommandLink.DisplayStyle.Arrow, "OK");
//			cl.BackColor = SystemColors.Window;
//			cl.DialogResult = DialogResult.Yes;
//			links.Add(cl);
//			Vista_Api.CommandDialog cd = new Vista_Api.CommandDialog(links);
//			cd.Title = "Fatal";
//			cd.Description = text.ToString();
//			cd.ShowDialog();
			MessageBox.Show(text.ToString(), 
			                "Fatal", 
			                MessageBoxButtons.OK, 
			                MessageBoxIcon.Stop,
			                MessageBoxDefaultButton.Button1,
			                MessageBoxOptions.DefaultDesktopOnly);
		}
		/// <summary>
		/// Shows confirmation.
		/// </summary>
		/// <param name="text"></param>
		/// <returns>DialogResult.Yes or DialogResult.No is returned.</returns>
		public static DialogResult Confirm(object text)
		{
//			List<Vista_Api.CommandLink> links=new List<Vista_Api.CommandLink>();
//			Vista_Api.CommandLink cl = new Vista_Api.CommandLink(Vista_Api.CommandLink.DisplayStyle.Arrow, "Lorem Ipsum", "Lorem Ipsum");
//			cl = new Vista_Api.CommandLink(Vista_Api.CommandLink.DisplayStyle.Arrow, "Yes");
//			cl.BackColor = SystemColors.Window;
//			cl.DialogResult = DialogResult.Yes;
//			links.Add(cl);
//			cl = new Vista_Api.CommandLink(Vista_Api.CommandLink.DisplayStyle.Arrow, "No");
//			cl.BackColor = SystemColors.Window;
//			cl.DialogResult = DialogResult.No;
//			links.Add(cl);
			//cl = new Vista_Api.CommandLink(Vista_Api.CommandLink.DisplayStyle.Shield, "Lorem Ipsum", "Lorem Ipsum");
			//cl.BackColor = SystemColors.Window;
			//links.Add(cl);
//			Vista_Api.CommandDialog cd = new Vista_Api.CommandDialog(links);
//			cd.Title = "Confirm";
//			cd.Description = text;
//			return cd.ShowDialog();
			cTaskDialog.ForceEmulationMode = true;
			cTaskDialog.UseToolWindowOnXP = true;
			//cTaskDialog.
			return cTaskDialog.MessageBox("Confirm", "Please confirm", text.ToString(), eTaskDialogButtons.YesNo, eSysIcons.Question);
			//return VDialog.Show(text.ToString(), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
			//return MessageBox.Show(text.ToString(), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);//TODO: finish
		}
	}
}
