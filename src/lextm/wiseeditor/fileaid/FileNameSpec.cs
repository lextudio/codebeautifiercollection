using System;
using System.IO;
using BeWise.Common.Info;

namespace Lextm.WiseEditor.FileAid
{
	/// <summary>
	/// Summary description for Class.
	/// </summary>
	class FileNameSpec : Spec
	{
		private string name;

		internal FileNameSpec(string name)
		{
			this.name = name;
		}

		internal override bool IsSatisfiedBy(ModuleFileInfo file) {
			if (String.IsNullOrEmpty(name))
			{
				return true;
			} else {
				return Path.GetFileName(file.FileName).IndexOf(name, StringComparison.OrdinalIgnoreCase) > -1;
            }
        }
	}
}
