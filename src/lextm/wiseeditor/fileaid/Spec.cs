using System;
using BeWise.Common.Info;

namespace Lextm.WiseEditor.FileAid
{
	/// <summary>
	/// Specification for files.
	/// </summary>
	internal abstract class Spec
	{
		internal Spec()	{}

		internal abstract bool IsSatisfiedBy(ModuleFileInfo file);
	}
}
