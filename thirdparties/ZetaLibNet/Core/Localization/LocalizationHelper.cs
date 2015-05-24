namespace ZetaLib.Core.Localization
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections.Generic;
	using System.Text;
	using ZetaLib.Core.Common;

	// ----------------------------------------------------------------------
	#endregion
	
	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Helper for easier localization.
	/// </summary>
	public static class LocalizationHelper
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Creates the pair.
		/// </summary>
		/// <param name="placeholder">The placeholder.</param>
		/// <param name="replacement">The replacement.</param>
		/// <returns></returns>
		public static ReplacementPair CreatePair(
			string placeholder,
			object replacement )
		{
			placeholder = @"{" + placeholder.Trim( '{', '}' ) + @"}";

			return new ReplacementPair(
				placeholder,
				replacement );
		}

		/// <summary>
		/// Servers as a better "string.Format" by allowing not only to
		/// replace by number but by name.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="replacements">The replacements.</param>
		/// <returns></returns>
		public static string Format(
			string text,
			params ReplacementPair[] replacements )
		{
			if ( string.IsNullOrEmpty( text ) )
			{
				return text;
			}
			else
			{
				foreach ( ReplacementPair replacement in replacements )
				{
					text = text.Replace(
						replacement.First,
						ConvertHelper.ToString( replacement.Second ) );
				}

				return text;
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}