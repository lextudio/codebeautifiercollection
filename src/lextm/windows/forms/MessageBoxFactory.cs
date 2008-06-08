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
			MessageBox.Show(text.ToString(),
			                "Debug",
			                MessageBoxButtons.OK,
			                MessageBoxIcon.Information,
			                MessageBoxDefaultButton.Button1,
			                0);
		}
		/// <summary>
		/// Shows info.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="text"></param>
		/// <param name="detail"></param>
		/// <returns></returns>
		public static void Info(string title, string text, string detail)
		{
			cTaskDialog.ForceEmulationMode = true;
			cTaskDialog.UseToolWindowOnXP = true;
			cTaskDialog.MessageBox(title, text, detail, eTaskDialogButtons.OK, eSysIcons.Information);
		}
		/// <summary>
		/// Shows warning.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="text"></param>
		/// <param name="detail"></param>
		/// <returns></returns>
		public static void Warn(string title, string text, string detail)
		{
			cTaskDialog.ForceEmulationMode = true;
			cTaskDialog.UseToolWindowOnXP = true;
			cTaskDialog.MessageBox(title, text, detail, eTaskDialogButtons.OK, eSysIcons.Warning);
		}
		/// <summary>
		/// Shows error.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="text"></param>
		/// <param name="detail"></param>
		/// <returns></returns>
		public static void Error(string title, string text, string detail)
		{
			cTaskDialog.ForceEmulationMode = true;
			cTaskDialog.UseToolWindowOnXP = true;
			cTaskDialog.MessageBox(title, text, detail, eTaskDialogButtons.OK, eSysIcons.Error);
		}
		/// <summary>
		/// Shows fatal.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="ex"></param>
		/// <returns></returns>
		public static void Fatal(string title, Exception ex)
		{
			cTaskDialog.ForceEmulationMode = true;
			cTaskDialog.UseToolWindowOnXP = true;
			cTaskDialog.MessageBox(title, ex.GetType().Name, ex.ToString(), eTaskDialogButtons.OK, eSysIcons.Error);
		}
		/// <summary>
		/// Shows confirmation.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="text"></param>
		/// <param name="detail"></param>
		/// <returns>DialogResult.Yes or DialogResult.No is returned.</returns>
		public static DialogResult Confirm(string title, string text, string detail)
		{
			cTaskDialog.ForceEmulationMode = true;
			cTaskDialog.UseToolWindowOnXP = true;
			return cTaskDialog.MessageBox(title, text, detail, eTaskDialogButtons.YesNo, eSysIcons.Question);
		}
	}
}
