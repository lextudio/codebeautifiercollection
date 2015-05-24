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
	/// A string class that encapsulates a normal text string, together
	/// with additional information about the languageCode that this string
	/// is associated with.
	/// </summary>
	[Serializable]
	public sealed class LanguageString :
		ICloneable
	{
		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Procides detailed information of the languageCode that this
		/// string represents.
		/// </summary>
		/// <value>The language info.</value>
		public LanguageStringLanguageInfo LanguageInfo
		{
			get
			{
				return language;
			}
			set
			{
				language = value;
			}
		}

		/// <summary>
		/// The actual text value that is stored inside this string.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			return Text;
		}

		// ------------------------------------------------------------------
		#endregion

		#region ICloneable Member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		public object Clone()
		{
			return this.MemberwiseClone();
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private LanguageStringLanguageInfo language;

		/// <summary>
		/// 
		/// </summary>
		private string text;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}