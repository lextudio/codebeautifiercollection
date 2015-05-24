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
	/// The cache manager. It is allowed (and often necessary) to derive
	/// a custom class from this class.
	/// See ZetaLib.Web.Caching.WebCacheManager for an example.
	/// </summary>
	[DebuggerDisplay( @"Count = {Count}, Backend = {backend}" )]
	public class CacheManager
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Best practice, see C# MSDN documentation of the "lock" keyword.
		/// </summary>
		private object thisLock = new object();

		/// <summary>
		/// Initialize the class.
		/// </summary>
		public virtual void Initialize()
		{
			lock ( thisLock )
			{
				if ( backend != null )
				{
					backend.Initialize();
				}
			}
		}

		/// <summary>
		/// Empties the complete cache.
		/// </summary>
		public virtual void RemoveAll()
		{
			lock ( thisLock )
			{
				if ( backend != null )
				{
					backend.RemoveAll( null );
				}
			}
		}

		/// <summary>
		/// Removes all items from the cache with
		/// a certain prefix in the key.
		/// </summary>
		/// <param name="group">The group.</param>
		public virtual void RemoveAll(
			CacheItemGroup group )
		{
			lock ( thisLock )
			{
				if ( backend != null )
				{
					backend.RemoveAll( group );
				}
			}
		}

		/// <summary>
		/// Removes the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public virtual object Remove(
			string key )
		{
			lock ( thisLock )
			{
				if ( backend == null )
				{
					return null;
				}
				else
				{
					return backend.Remove( key );
				}
			}
		}

		/// <summary>
		/// Read from the cache.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public virtual object Get(
			string key )
		{
			lock ( thisLock )
			{
				if ( backend == null )
				{
					return null;
				}
				else
				{
					return backend.Get( key );
				}
			}
		}

		/// <summary>
		/// Write to the cache.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public virtual void Set(
			string key,
			object value )
		{
			Set( key, value, CacheItemInformation.Default );
		}

		/// <summary>
		/// Write to the cache.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="itemInfo">The item info.</param>
		public virtual void Set(
			string key,
			object value,
			CacheItemInformation itemInfo )
		{
			lock ( thisLock )
			{
				if ( backend != null )
				{
					backend.Set( key, value, itemInfo );
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Read from and write to the cache.
		/// </summary>
		/// <value></value>
		public virtual object this[string key]
		{
			get
			{
				return Get( key );
			}
			set
			{
				Set( key, value );
			}
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public virtual int Count
		{
			get
			{
				lock ( thisLock )
				{
					if ( backend == null )
					{
						return 0;
					}
					else
					{
						return backend.Count;
					}
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is empty.
		/// </summary>
		/// <value><c>true</c> if this instance is empty; otherwise,
		/// <c>false</c>.</value>
		public bool IsEmpty
		{
			get
			{
				return Count <= 0;
			}
		}

		/// <summary>
		/// The backend that actually caches.
		/// </summary>
		/// <value>The backend.</value>
		public ICacheBackend Backend
		{
			get
			{
				lock ( thisLock )
				{
					return backend;
				}
			}
			set
			{
				lock ( thisLock )
				{
					backend = value;

					// Set the default backend if none.
					if ( backend == null )
					{
						backend = new SimpleCacheBackend();
					}
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// The backend that actually caches.
		/// </summary>
		private ICacheBackend backend =
			new SimpleCacheBackend();

		/// <summary>
		/// The reference to the actual cache manager.
		/// </summary>
		protected static volatile CacheManager currentCacheManager = null;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}