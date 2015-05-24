namespace ZetaLib.Core.Localization
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections;
	using System.Data;
	using System.Diagnostics;
	using System.Collections.Generic;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// This plug-in interface is used by a LanguageString to actually
	/// query information about the languages supported.
	/// </summary>
	public interface ILanguageStringLanguagesProvider
	{
		#region Interface member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Returns the list of all supported languages.
		/// The first entry in the array is the fallback languageCode.
		/// </summary>
		/// <value>The language infos.</value>
		LanguageStringLanguageInfo[] LanguageInfos
		{
			get;
		}

		/// <summary>
		/// Lookup a languageCode string languageCode info by its symbolic name.
		/// </summary>
		/// <param name="symbolicName">The symbolic name to look up.
		/// The name is caseINsensitive.</param>
		/// <returns>
		/// Returns the found languageCode info or NULL if none found.
		/// </returns>
		LanguageStringLanguageInfo GetLanguageInfoBySymbolicName(
			string symbolicName );

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}