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
	/// One information item about a single languageCode.
	/// </summary>
	[Serializable]
	public sealed class LanguageStringLanguageInfo
	{
		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Get or set the symbolic name.
		/// This is either e.g. an integer or a string that
		/// uniquely represents the languageCode. Usually this is something like
		/// "en" for english or "de" for german.
		/// </summary>
		/// <value>The name of the symbolic.</value>
		public object SymbolicName
		{
			get
			{
				return symbolicName;
			}
			set
			{
				symbolicName = value;
			}
		}

		/// <summary>
		/// Get or set the database column postfix.
		/// This is the postfix to append to columns when reading
		/// from the database. E.g. nothing (empty string) for english, since
		/// this is often the default, or e.g. "_de" for german.
		/// </summary>
		/// <value>The DB column postfix.</value>
		public string DBColumnPostfix
		{
			get
			{
				return dbColumnPostfix;
			}
			set
			{
				dbColumnPostfix = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// Either e.g. an integer or a string that
		/// uniquely represents the languageCode.
		/// </summary>
		private object symbolicName;

		/// <summary>
		/// The postfix to append to columns when reading
		/// from the database.
		/// </summary>
		private string dbColumnPostfix;

		// ------------------------------------------------------------------
		#endregion

		#region (Operator-) overloading.
		// ------------------------------------------------------------------

		/// <summary>
		/// Overload for comparing the symbolic names.
		/// </summary>
		/// <param name="aa">The aa.</param>
		/// <param name="bb">The bb.</param>
		/// <returns></returns>
		public static bool operator ==(
			LanguageStringLanguageInfo aa,
			LanguageStringLanguageInfo bb )
		{
			if ( aa == null || bb == null )
			{
				return false;
			}
			else
			{
				return string.Compare(
					aa.SymbolicName.ToString(),
					bb.SymbolicName.ToString(), true ) == 0;
			}
		}

		/// <summary>
		/// Overload for comparing the symbolic names.
		/// </summary>
		/// <param name="aa">The aa.</param>
		/// <param name="bb">The bb.</param>
		/// <returns></returns>
		public static bool operator !=(
			LanguageStringLanguageInfo aa,
			LanguageStringLanguageInfo bb )
		{
			if ( aa == null && bb == null )
			{
				return false;
			}
			else
			{
				if ( aa == null || bb == null )
				{
					return true;
				}
				else
				{
					return string.Compare(
						aa.SymbolicName.ToString(),
						bb.SymbolicName.ToString(), true ) != 0;
				}
			}
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"></see> 
		/// is equal to the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"></see> 
		/// to compare with the current <see cref="T:System.Object"></see>.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"></see> 
		/// is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
		/// </returns>
		public override bool Equals(
			object obj )
		{
			return this == (obj as LanguageStringLanguageInfo);
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override int GetHashCode()
		{
			return 
				base.GetHashCode() +
				SymbolicName.ToString().ToLower().GetHashCode();
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}