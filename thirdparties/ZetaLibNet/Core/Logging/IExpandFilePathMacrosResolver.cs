namespace ZetaLib.Core.Logging
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Optional interface for the LogCentral.ExpandFilePathMacros() function.
	/// </summary>
	public interface IExpandFilePathMacrosResolver
	{
		#region Interface member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Resolves a macro. If you do nothing in the function, the behaviour
		/// is the built-in default behaviour. If you modify the value of
		/// 'macroValueToReplaceWith', this modified value is used instead.
		/// </summary>
		/// <param name="macroName">The name of the macro to expand.</param>
		/// <param name="macroValueToReplaceWith">The current value being used
		/// to replace the macro. Modify this value to you own needs.</param>
		void Resolve(
			string macroName,
			ref string macroValueToReplaceWith );

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}