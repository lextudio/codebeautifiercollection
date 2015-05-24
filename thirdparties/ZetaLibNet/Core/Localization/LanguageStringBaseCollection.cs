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
	/// A collection of languageCode languageStrings, with each languageCode string of 
	/// the collection usually repesents a string different languageCode.
	/// </summary>
	/// <remarks>
	/// This class usually is used when reading multi-lingual texts
	/// for an object in a database and constructing a C# class from this
	/// object. 
	/// In such a scenario, a LanguageStringBaseCollection-derived class
	/// would be one of multiple properties of this C# class representing
	/// the database object.
	/// </remarks>
	[Serializable]
	public abstract class LanguageStringBaseCollection :
		ICollection,
		ICollection<LanguageString>,
		ICloneable
	{
		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Assign the provider that is suitable for all instances of this
		/// class.
		/// </summary>
		/// <value>The languages provider.</value>
		public virtual ILanguageStringLanguagesProvider LanguagesProvider
		{
			get
			{
				Debug.Assert( 
					false,
					@"Must override LanguagesProvider property." );
				return null;
			}
		}

		/// <summary>
		/// Gets the strings.
		/// </summary>
		/// <value>The strings.</value>
		public LanguageString[] Strings
		{
			get
			{
				List<LanguageString> r = new List<LanguageString>();

				foreach ( LanguageString s in languageStrings )
				{
					r.Add( s );
				}

				return r.ToArray();
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Default constructor.
		/// </summary>
		public LanguageStringBaseCollection()
		{
			CreateDefaultLanguages();
		}

		/// <summary>
		/// Creates all available languages by default, if param set to true.
		/// </summary>
		/// <param name="createDefaultLanguage">if set to <c>true</c> [create default language].</param>
		public LanguageStringBaseCollection(
			bool createDefaultLanguage )
		{
			if ( createDefaultLanguage )
			{
				CreateDefaultLanguages();
			}
		}

		/// <summary>
		/// Initialize with these types.
		/// </summary>
		/// <param name="types">The types.</param>
		public LanguageStringBaseCollection(
			LanguageStringLanguageInfo[] types )
		{
			if ( types != null )
			{
				foreach ( LanguageStringLanguageInfo t in types )
				{
					SetString( t, null );
				}
			}
		}

		/// <summary>
		/// Access the languages by array.
		/// </summary>
		/// <value></value>
		public string this[LanguageStringLanguageInfo index]
		{
			get
			{
				return GetString( index );
			}
			set
			{
				SetString( index, value );
			}
		}

		/// <summary>
		/// Creates languageCode string items inside the collection
		/// with all different languageCode infos and empty texts.
		/// </summary>
		public virtual void CreateDefaultLanguages()
		{
			foreach ( LanguageStringLanguageInfo languageInfo
				in LanguagesProvider.LanguageInfos )
			{
				SetString( languageInfo, null );
			}
		}

		/// <summary>
		/// Get a string in the current languageCode.
		/// </summary>
		/// <param name="fallBack">The fall back.</param>
		/// <returns></returns>
		public string GetString(
			FallbackGetString fallBack )
		{
			return GetString(
				CurrentAutomaticLanguage,
				fallBack );
		}

		/// <summary>
		/// Get a string in the current languageCode.
		/// </summary>
		/// <returns></returns>
		public string GetString()
		{
			return GetString( CurrentAutomaticLanguage );
		}

		/// <summary>
		/// Get a string by specifying an explicit languageCode.
		/// </summary>
		/// <param name="languageStringLanguageInfo">The languageCode info
		/// to search for.</param>
		/// <returns></returns>
		public string GetString(
			LanguageStringLanguageInfo languageStringLanguageInfo )
		{
			return GetString(
				languageStringLanguageInfo,
				FallbackGetString.Default );
		}

		/// <summary>
		/// Get a string by specifying an explicit languageCode.
		/// </summary>
		/// <param name="languageStringLanguageInfo">The languageCode info
		/// to search for.</param>
		/// <param name="fallback">The fallback.</param>
		/// <returns></returns>
		public string GetString(
			LanguageStringLanguageInfo languageStringLanguageInfo,
			FallbackGetString fallback )
		{
			foreach ( LanguageString languageString in languageStrings )
			{
				if ( languageString.LanguageInfo == languageStringLanguageInfo )
				{
					string text = languageString.Text;

					if ( text == null || text.Length <= 0 )
					{
						if ( fallback == FallbackGetString.Default )
						{
							// Fall back to first.
							return ((LanguageString)languageStrings[0]).Text;
						}
						else
						{
							return null;
						}
					}
					else
					{
						return text;
					}
				}
			}

			if ( fallback == FallbackGetString.Default )
			{
				// Fall back to first.
				return ((LanguageString)languageStrings[0]).Text;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Get a string by specifying an explicit languageCode.
		/// </summary>
		/// <param name="symbolicName">The symbolic name of
		/// the languageCode to search for.</param>
		/// <returns></returns>
		public string GetString(
			string symbolicName )
		{
			return GetString(
				symbolicName,
				FallbackGetString.Default );
		}

		/// <summary>
		/// Get a string by specifying an explicit languageCode.
		/// </summary>
		/// <param name="symbolicName">The symbolic name of
		/// the languageCode to search for.</param>
		/// <param name="fallback">The fallback.</param>
		/// <returns></returns>
		public string GetString(
			string symbolicName,
			FallbackGetString fallback )
		{
			foreach ( LanguageString languageString in languageStrings )
			{
				if ( string.Compare(
					languageString.LanguageInfo.SymbolicName.ToString(),
					symbolicName, true ) == 0 )
				{
					string text = languageString.Text;

					if ( text == null || text.Length <= 0 )
					{
						if ( fallback == FallbackGetString.Default )
						{
							// Fall back to first.
							return ((LanguageString)languageStrings[0]).Text;
						}
						else
						{
							return null;
						}
					}
					else
					{
						return text;
					}
				}
			}

			// Not found.
			if ( languageStrings.Count > 0 )
			{
				if ( fallback == FallbackGetString.Default )
				{
					// Fall back to first.
					return ((LanguageString)languageStrings[0]).Text;
				}
				else
				{
					return null;
				}
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Assigns a languageCode string to this collection.
		/// If a languageCode string with the same languageCode info already exists,
		/// it is overwritten, otherwise the languageCode string is newly added.
		/// </summary>
		/// <param name="languageString">The languageCode string to assign.</param>
		public void SetString(
			LanguageString languageString )
		{
			foreach ( LanguageString itLanguageString in languageStrings )
			{
				if ( itLanguageString.LanguageInfo == languageString.LanguageInfo )
				{
					// Exists, just modify.
					itLanguageString.Text = languageString.Text;
					return;
				}
			}

			// Not found, just add.
			languageStrings.Add( languageString );
		}

		/// <summary>
		/// Assign a string with a given languageCode info.
		/// If a string for the languageCode info already exists, it is
		/// overwritten, otherwise the string with the languageCode info
		/// is newly added.
		/// </summary>
		/// <param name="languageInfo">The languageCode info to store.</param>
		/// <param name="s">The string to store.</param>
		public void SetString(
			LanguageStringLanguageInfo languageInfo,
			string s )
		{
			LanguageString st = new LanguageString();

			st.LanguageInfo = languageInfo;
			st.Text = s;

			SetString( st );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public enums.
		// ------------------------------------------------------------------

		/// <summary>
		/// How to behave if a a direct entry is not found.
		/// </summary>
		public enum FallbackGetString
		{
			#region Enum members.

			/// <summary>
			/// Fall back to first entry, if direct entry not found.
			/// </summary>
			Default,

			/// <summary>
			/// No fall back to first entry. If direct entry not found,
			/// return NULL.
			/// </summary>
			None

			#endregion
		}

		/// <summary>
		/// Configures the behavior if a missing column in a data row
		/// is detected.
		/// "Missing" means that a languageCode string with a certain 
		/// languageCode info exists in the current languageCode string collection,
		/// but the column for that languageCode is missing in the database row.
		/// </summary>
		public enum MissingLanguagColumnsBehavior
		{
			/// <summary>
			/// Ignore a missing column and simply don't read/write.
			/// </summary>
			Ignore,

			/// <summary>
			/// Fail the whole read/write operation when a column in a 
			/// certain languageCode is missing.
			/// </summary>
			Fail
		}

		// ------------------------------------------------------------------
		#endregion

		#region Loading and saving from/to a single DataRow.
		// ------------------------------------------------------------------

		/// <summary>
		/// Fill the current instance from a DataRow, overriding
		/// all current content in the current instance.
		/// </summary>
		/// <param name="row">The row.</param>
		/// <param name="columnBaseName">Name of the column base.</param>
		public void Load(
			DataRow row,
			string columnBaseName )
		{
			Load(
				row,
				columnBaseName,
				MissingLanguagColumnsBehavior.Ignore );
		}

		/// <summary>
		/// Fill the current instance from a DataRow, overriding
		/// all current content in the current instance.
		/// </summary>
		/// <param name="row">The row.</param>
		/// <param name="columnBaseName">Name of the column base.</param>
		/// <param name="missingLanguagColumnsBehavior">The missing languag columns behavior.</param>
		public virtual void Load(
			DataRow row,
			string columnBaseName,
			MissingLanguagColumnsBehavior missingLanguagColumnsBehavior )
		{
			CreateDefaultLanguages();

			foreach ( LanguageString languageString in this )
			{
				string columnName =
					columnBaseName +
					languageString.LanguageInfo.DBColumnPostfix;

				if ( row.Table.Columns.Contains( columnName ) ||
					missingLanguagColumnsBehavior == MissingLanguagColumnsBehavior.Fail )
				{
					string text;

					Data.DBHelper.ReadField(
						out text,
						row[columnName] );

					languageString.Text = text;
				}
			}
		}

		/// <summary>
		/// Initializes the current instance from a DataRow, overriding
		/// all current content in the current instance.
		/// </summary>
		/// <param name="columns">The columns.</param>
		/// <param name="columnBaseName">Name of the column base.</param>
		public void Initialize(
			DataColumnCollection columns,
			string columnBaseName )
		{
			Initialize(
				columns,
				columnBaseName,
				MissingLanguagColumnsBehavior.Ignore );
		}

		/// <summary>
		/// Initializes the current instance, overriding
		/// all current content in the current instance.
		/// </summary>
		/// <param name="columns">The columns.</param>
		/// <param name="columnBaseName">Name of the column base.</param>
		/// <param name="missingLanguagColumnsBehavior">The missing languag columns behavior.</param>
		public virtual void Initialize(
			DataColumnCollection columns,
			string columnBaseName,
			MissingLanguagColumnsBehavior missingLanguagColumnsBehavior )
		{
			CreateDefaultLanguages();

			foreach ( LanguageString languageString in this )
			{
				string columnName =
					columnBaseName +
					languageString.LanguageInfo.DBColumnPostfix;

				if ( columns.Contains( columnName ) ||
					missingLanguagColumnsBehavior == MissingLanguagColumnsBehavior.Fail )
				{
					// Only create, but do not fill.
					languageString.Text = null;
				}
			}
		}

		/// <summary>
		/// Stores the current instance into a DataRow.
		/// </summary>
		/// <param name="row">The row.</param>
		/// <param name="columnBaseName">Name of the column base.</param>
		public void Store(
			DataRow row,
			string columnBaseName )
		{
			Store(
				row,
				columnBaseName,
				MissingLanguagColumnsBehavior.Ignore );
		}

		/// <summary>
		/// Stores the current instance into a DataRow.
		/// </summary>
		/// <param name="row">The row.</param>
		/// <param name="columnBaseName">Name of the column base.</param>
		/// <param name="missingLanguagColumnsBehavior">The missing languag columns behavior.</param>
		public virtual void Store(
			DataRow row,
			string columnBaseName,
			MissingLanguagColumnsBehavior missingLanguagColumnsBehavior )
		{
			foreach ( LanguageString languageString in this )
			{
				string columnName =
					columnBaseName +
					languageString.LanguageInfo.DBColumnPostfix;

				if ( row.Table.Columns.Contains( columnName ) ||
					missingLanguagColumnsBehavior == MissingLanguagColumnsBehavior.Fail )
				{
					row[columnName] =
						Data.DBHelper.WriteField( languageString.Text );
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Use this class to provide context-dependent retrieval
		/// of the current set default languageCode.
		/// This could e.g. be the current browser languageCode or
		/// a languageCode that the user selected manually.
		/// </summary>
		/// <value>The current automatic language.</value>
		protected abstract LanguageStringLanguageInfo CurrentAutomaticLanguage
		{
			get;
		}

		/// <summary>
		/// 
		/// </summary>
		private List<LanguageString> languageStrings =
			new List<LanguageString>();

		// ------------------------------------------------------------------
		#endregion

		#region ICollection member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe).
		/// </summary>
		/// <value></value>
		/// <returns>true if access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe); otherwise, false.</returns>
		public bool IsSynchronized
		{
			get
			{
				return (languageStrings as ICollection).IsSynchronized;
			}
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.</returns>
		public int Count
		{
			get
			{
				return languageStrings.Count;
			}
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.ICollection"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in array at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">array is null. </exception>
		/// <exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"></see> cannot be cast automatically to the type of the destination array. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">array is multidimensional.-or- index is equal to or greater than the length of array.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"></see> is greater than the available space from index to the end of the destination array. </exception>
		public void CopyTo(
			Array array,
			int index )
		{
			(languageStrings as ICollection).CopyTo( array, index );
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.</returns>
		public object SyncRoot
		{
			get
			{
				return (languageStrings as ICollection).SyncRoot;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region IEnumerable member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator GetEnumerator()
		{
			return languageStrings.GetEnumerator();
		}

		// ------------------------------------------------------------------
		#endregion

		#region ICloneable member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// Returns a new object that is a copy of this instance.
		/// </returns>
		public abstract object Clone();

		/// <summary>
		/// Deep-copies the content of another object into
		/// the current instance.
		/// </summary>
		/// <param name="o">The object to copy from.</param>
		/// <remarks>My own function, not part of the ICloneable interface.</remarks>
		public void CopyFrom(
			object o )
		{
			LanguageStringBaseCollection src =
				o as LanguageStringBaseCollection;

			this.languageStrings =
				new List<LanguageString>(
				src.languageStrings.Count );

			foreach ( LanguageString s in src.languageStrings )
			{
				this.languageStrings.Add( s.Clone() as LanguageString );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region ICollection<LanguageString> members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public void Add( LanguageString item )
		{
			languageStrings.Add( item );
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
		public void Clear()
		{
			languageStrings.Clear();
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>
		/// true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
		/// </returns>
		public bool Contains( 
			LanguageString item )
		{
			return languageStrings.Contains( item );
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
		/// <exception cref="T:System.ArgumentNullException">array is null.</exception>
		/// <exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-Type T cannot be cast automatically to the type of the destination array.</exception>
		public void CopyTo(
			LanguageString[] array,
			int arrayIndex )
		{
			languageStrings.CopyTo( array, arrayIndex );
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>
		/// true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public bool Remove( 
			LanguageString item )
		{
			return languageStrings.Remove( item );
		}

		// ------------------------------------------------------------------
		#endregion

		#region IEnumerable<LanguageString> members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
		/// </returns>
		IEnumerator<LanguageString> IEnumerable<LanguageString>.GetEnumerator()
		{
			return languageStrings.GetEnumerator();
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}