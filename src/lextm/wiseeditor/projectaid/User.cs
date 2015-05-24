/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/3/30
 * Time: 12:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using Lextm.Windows.Forms;

namespace Lextm.WiseEditor.ProjectAid
{
	class User: IUser
	{
		public string ProvideKeyFile()
		{
			string result = null;
			if (MessageBoxFactory.Confirm("Do you have a key file?") == DialogResult.Yes)
			{
				Vista_Api.OpenFileDialog dialog = new Vista_Api.OpenFileDialog();
				dialog.Filter = "Key Files (*.snk)|*.snk";
				dialog.FilterIndex = 0;
				dialog.Multiselect = false;
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					result = dialog.FileName;
				}
			}
			return result;
		}
	}
}
