using System;

namespace Lextm.JcfExpert
{
	public abstract class BaseOTA
	{

		// *************************************************************************
		//                              Public
		// *************************************************************************

		public abstract void IDERegister(out string[] aMenuNames, out int[] aMenuShortCuts);
	}
}
