namespace ZetaLib.Core.Caching
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Diagnostics;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// This is basically only an encapsulated string to perform certain
	/// caching operations.
	/// </summary>
	[DebuggerDisplay( @"Name = {name}" )]
	public class CacheItemGroup
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Shortcut for checking whether invalid.
		/// </summary>
		/// <param name="group">The group.</param>
		/// <returns>
		/// 	<c>true</c> if [is null or empty] [the specified group]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNullOrEmpty(
			CacheItemGroup group )
		{
			return group == null || group.IsEmpty;
		}

		/// <summary>
		/// Constructor with empty name.
		/// </summary>
		public CacheItemGroup()
		{
		}

		/// <summary>
		/// Constructor with the given gorup name.
		/// </summary>
		/// <param name="name">The name.</param>
		public CacheItemGroup(
			string name )
		{
			this.name = name;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Get the name of the cache group.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get
			{
				return name;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is empty.
		/// </summary>
		/// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
		public bool IsEmpty
		{
			get
			{
				return string.IsNullOrEmpty( name );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private string name;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}