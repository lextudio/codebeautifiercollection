/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/3/30
 * Time: 12:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.IO;

using Lextm.OpenTools;

namespace Lextm.WiseEditor.ProjectAid
{
	class Generator: IGenerator
	{
		public bool GenerateKey(string key)
		{
			if (!File.Exists(key))
			{
				string keyGen = Path.Combine(Path.Combine(Lextm.IO.SpecialFolders.DotNetSdkRoot, "Bin"), "sn.exe");
				if (!File.Exists(keyGen))
				{
					MessageService.Show("Corrupt .NET SDK 2.0 installation.");
					return false;
				}
				Process.Start(keyGen, string.Format("-k \"{0}\"", key));
			}
			return true;
		}
	}
}
