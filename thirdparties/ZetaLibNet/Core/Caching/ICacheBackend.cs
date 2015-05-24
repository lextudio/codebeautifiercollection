namespace ZetaLib.Core.Caching
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// An interface that is used by the cache manager to actually store
	/// and retrieve values.
	/// </summary>
	public interface ICacheBackend
	{
		#region Interface methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Initialize the cache.
		/// </summary>
		void Initialize();

		/// <summary>
		/// Removes all items from the cache with
		/// a certain prefix in the key.
		/// </summary>
		/// <param name="group">The group name of the keys to remove. If the
		/// prefix is NULL or empty, the complete cache is emptied.</param>
		void RemoveAll(
			CacheItemGroup group );

		/// <summary>
		/// Removes the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		object Remove(
			string key );

		/// <summary>
		/// Gets the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Returns NULL if not found.</returns>
		object Get(
			string key );

		/// <summary>
		/// Determines whether [contains] [the specified key].
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>
		/// 	<c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.
		/// </returns>
		bool Contains(
			string key );

		/// <summary>
		/// Sets the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="itemInfo">Pass NULL if want to use default
		/// values.</param>
		void Set(
			string key,
			object value,
			CacheItemInformation itemInfo );

		// ------------------------------------------------------------------
		#endregion

		#region Interface properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Returns the number of cached elements.
		/// </summary>
		/// <value>The count.</value>
		int Count
		{
			get;
		}

		/// <summary>
		/// Returns whether the cache contains any elements.
		/// </summary>
		/// <value><c>true</c> if this instance is empty; otherwise,
		/// <c>false</c>.</value>
		bool IsEmpty
		{
			get;
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}