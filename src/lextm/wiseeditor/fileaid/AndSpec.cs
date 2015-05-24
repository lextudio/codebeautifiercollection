using System;
using BeWise.Common.Info;

namespace Lextm.WiseEditor.FileAid
{
	/// <summary>
	/// Description of AndSpec.
	/// </summary>
	class AndSpec : Spec
	{
		private Spec spec1;
		private Spec spec2;
		internal AndSpec(Spec spec1, Spec spec2)
		{
			this.spec1 = spec1;
			this.spec2 = spec2;
		}
		
		internal override bool IsSatisfiedBy(ModuleFileInfo file) {
			return spec1.IsSatisfiedBy(file) && spec2.IsSatisfiedBy(file);
		}
	}
}
