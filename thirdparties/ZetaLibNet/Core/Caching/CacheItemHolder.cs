namespace ZetaLib.Core.Caching
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Holds items in the cache.
	/// </summary>
	/// <typeparam name="T">The type of the item to put 
	/// in the cache.</typeparam>
	public class CacheItemHolder<T>
	{
		#region Public constructors.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public CacheItemHolder()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="cache">The cache.</param>
		public CacheItemHolder(
			CacheManager cache )
		{
			this.cache = cache;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="value">The value.</param>
		public CacheItemHolder(
			T value )
		{
			this.Value = value;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="cache">The cache.</param>
		/// <param name="value">The value.</param>
		public CacheItemHolder(
			CacheManager cache,
			T value )
		{
			this.cache = cache;
			this.Value = value;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public CacheItemHolder(
			CacheItemInformation cacheItemInfo )
		{
			this.cacheItemInfo = cacheItemInfo;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="cache">The cache.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public CacheItemHolder(
			CacheManager cache,
			CacheItemInformation cacheItemInfo )
		{
			this.cache = cache;
			this.cacheItemInfo = cacheItemInfo;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public CacheItemHolder(
			T value,
			CacheItemInformation cacheItemInfo )
		{
			this.Value = value;
			this.cacheItemInfo = cacheItemInfo;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="cache">The cache.</param>
		/// <param name="value">The value.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public CacheItemHolder(
			CacheManager cache,
			T value,
			CacheItemInformation cacheItemInfo )
		{
			this.cache = cache;
			this.Value = value;
			this.cacheItemInfo = cacheItemInfo;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="cacheItemGroup">The cache item group.</param>
		public CacheItemHolder(
			CacheItemGroup cacheItemGroup )
		{
			this.cacheItemInfo.Group = cacheItemGroup;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="cache">The cache.</param>
		/// <param name="cacheItemGroup">The cache item group.</param>
		public CacheItemHolder(
			CacheManager cache,
			CacheItemGroup cacheItemGroup )
		{
			this.cache = cache;
			this.cacheItemInfo.Group = cacheItemGroup;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="cacheItemGroup">The cache item group.</param>
		public CacheItemHolder(
			T value,
			CacheItemGroup cacheItemGroup )
		{
			this.Value = value;
			this.cacheItemInfo.Group = cacheItemGroup;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="cache">The cache.</param>
		/// <param name="value">The value.</param>
		/// <param name="cacheItemGroup">The cache item group.</param>
		public CacheItemHolder(
			CacheManager cache,
			T value,
			CacheItemGroup cacheItemGroup )
		{
			this.cache = cache;
			this.Value = value;
			this.cacheItemInfo.Group = cacheItemGroup;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets the value from the cache or sets to the cache.
		/// </summary>
		/// <value>The value.</value>
		public T Value
		{
			get
			{
				if ( string.IsNullOrEmpty( cacheKey ) )
				{
					return default( T );
				}
				else
				{
					return (T)cache.Get( cacheKey );
				}
			}
			set
			{
				Remove();

				if ( value != null )
				{
					cacheKey = value.GetHashCode().ToString();
					cache.Set( cacheKey, value, cacheItemInfo );
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Removes this instance.
		/// </summary>
		private void Remove()
		{
			if ( !string.IsNullOrEmpty( cacheKey ) )
			{
				cache.Remove( cacheKey );
				cacheKey = null;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private string cacheKey;
		private CacheItemInformation cacheItemInfo =
			CacheItemInformation.Default;

		private CacheManager cache = defaultCacheManager;

		/// <summary>
		/// Gets the default cache manager.
		/// </summary>
		/// <value>The default cache manager.</value>
		private static CacheManager defaultCacheManager
		{
			get
			{
				return LibraryConfiguration.Current.Cache;
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}