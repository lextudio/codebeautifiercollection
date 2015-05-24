namespace ZetaLib.Core.Caching
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections.Generic;
	using ZetaLib.Core.Logging;
	using System.Diagnostics;
	using ZetaLib.Core.Common;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// A simple implementation of the the cache backend interface.
	/// Stores in-memory.
	/// </summary>
	public class SimpleCacheBackend :
		ICacheBackend
	{
		#region ICacheBackend members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Best practice, see C# MSDN documentation of the "lock" keyword.
		/// </summary>
		private object thisLock = new object();

		/// <summary>
		/// Initialize the cache.
		/// </summary>
		public void Initialize()
		{
		}

		/// <summary>
		/// Removes all items from the cache with
		/// a certain prefix in the key.
		/// </summary>
		/// <param name="group">The group name of the keys to remove. If the
		/// prefix is NULL or empty, the complete cache is emptied.</param>
		public void RemoveAll(
			CacheItemGroup group )
		{
			lock ( thisLock )
			{
				if ( CacheItemGroup.IsNullOrEmpty( group ) )
				{
					int count = inMemoryCache.Count;

					if ( count > 0 )
					{
						LogCentral.Current.LogDebug(
							string.Format(
							@"[Cache] About to removed {0} cache items at '{1}', " +
							@"ignoring their keys. The items follow:",
							count,
							DateTime.Now ) );

						int index = 1;
						foreach ( string key in inMemoryCache.Keys )
						{
							LogCentral.Current.LogDebug(
								string.Format(
								@"[Cache] Item {0} to remove: key = '{1}', " +
								@"value = '{2}'.",
								index + 1,
								key,
								inMemoryCache[key].Dump() ) );

							index++;
						}
					}

					// Cleaning all.
					inMemoryCache.Clear();

					if ( count > 0 )
					{
						LogCentral.Current.LogDebug(
							string.Format(
							@"[Cache] Removed {0} cache items, ignoring their keys.",
							count ) );
					}
				}
				else
				{
					List<string> keysToRemove = new List<string>();

					string groupName = group.Name;
					foreach ( string key in inMemoryCache.Keys )
					{
						SimpleCacheItemInformation itemInfo =
							inMemoryCache[key];

						if ( !CacheItemGroup.IsNullOrEmpty( group ) &&
							itemInfo != null )
						{
							CacheItemGroup g = itemInfo.Group;

							if ( !CacheItemGroup.IsNullOrEmpty( g ) &&
								g.Name.StartsWith( groupName,
								StringComparison.InvariantCultureIgnoreCase ) )
							{
								keysToRemove.Add( key );
							}
						}
					}

					foreach ( string key in keysToRemove )
					{
						LogCentral.Current.LogDebug(
							string.Format(
							@"[Cache] About to remove cache item at '{0}' with: " +
							@"key = '{1}', value = '{2}'.",
							DateTime.Now,
							key,
							inMemoryCache[key].Dump() ) );

						inMemoryCache.Remove( key );
					}
				}
			}
		}

		/// <summary>
		/// Removes the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public object Remove(
			string key )
		{
			lock ( thisLock )
			{
				if ( inMemoryCache.ContainsKey( key ) )
				{
					LogCentral.Current.LogDebug(
						string.Format(
						@"[Cache] About to remove cache item at '{0}' with: " +
						@"key = '{1}', value = '{2}'.",
						DateTime.Now,
						key,
						inMemoryCache[key].Dump() ) );

					object o = inMemoryCache[key].Value;
					inMemoryCache.Remove( key );

					return o;
				}
				else
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Gets the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Returns NULL if not found.</returns>
		public object Get(
			string key )
		{
			lock ( thisLock )
			{
				if ( inMemoryCache.ContainsKey( key ) )
				{
					return inMemoryCache[key].Value;
				}
				else
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Sets the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="itemInfo">Pass NULL if want to use default
		/// values.</param>
		public void Set(
			string key,
			object value,
			CacheItemInformation itemInfo )
		{
			lock ( thisLock )
			{
				OnBeforeValueSet();

				inMemoryCache[key] =
					new SimpleCacheItemInformation(
					key,
					value,
					itemInfo );

				LogCentral.Current.LogDebug(
					string.Format(
					@"[Cache] Added cache item at '{0}' with: " +
					@"key = '{1}', value = '{2}'.",
					DateTime.Now,
					key,
					inMemoryCache[key].Dump() ) );

				OnAfterValueSet();
			}
		}

		/// <summary>
		/// Returns the number of cached elements.
		/// </summary>
		/// <value>The count.</value>
		public int Count
		{
			get
			{
				lock ( thisLock )
				{
					return inMemoryCache.Count;
				}
			}
		}

		/// <summary>
		/// Determines whether [contains] [the specified key].
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>
		/// 	<c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(
			string key )
		{
			return Get( key ) != null;
		}

		/// <summary>
		/// Returns whether the cache contains any elements.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is empty; otherwise,
		/// <c>false</c>.
		/// </value>
		public bool IsEmpty
		{
			get
			{
				return Count <= 0;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Helper class for managing expiration.
		// ------------------------------------------------------------------

		/// <summary>
		/// Storing extended information.
		/// </summary>
		[DebuggerDisplay( @"Key = {key}, CacheInfo = {cacheInfo}" )]
		private class SimpleCacheItemInformation :
			IDumpable
		{
			#region Public methods.

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="key">The key.</param>
			/// <param name="value">The value.</param>
			/// <param name="cacheInfo">The cache info.</param>
			public SimpleCacheItemInformation(
				string key,
				object value,
				CacheItemInformation cacheInfo )
			{
				this.key = key;
				this.value = value;
				this.cacheInfo = cacheInfo;
			}

			/// <summary>
			/// Checks whether removable now.
			/// </summary>
			/// <returns></returns>
			public bool CheckIfRemove()
			{
				if ( cacheInfo == null )
				{
					return false;
				}
				else
				{
					return EffectivelyExpiresAfter <= DateTime.Now;
				}
			}

			#endregion

			#region Public properties.

			/// <summary>
			/// Gets the value.
			/// </summary>
			/// <value>The value.</value>
			public object Value
			{
				get
				{
					lastAccessed = DateTime.Now;
					return value;
				}
			}

			/// <summary>
			/// Calculates the real date this item expires.
			/// </summary>
			/// <value>The effectively expires after.</value>
			public DateTime EffectivelyExpiresAfter
			{
				get
				{
					if ( cacheInfo == null )
					{
						return DateTime.MaxValue;
					}
					else
					{
						switch ( cacheInfo.ExpirationMode )
						{
							case CacheItemInformation.CacheExpirationMode.Absolute:
								return cacheInfo.AbsoluteExpiration;
							case CacheItemInformation.CacheExpirationMode.Sliding:
								return lastAccessed.Add( cacheInfo.SlidingExpiration );
							default:
								return DateTime.MaxValue;
						}
					}
				}
			}

			/// <summary>
			/// Gets the group.
			/// </summary>
			/// <value>The group.</value>
			public CacheItemGroup Group
			{
				get
				{
					return cacheInfo.Group;
				}
			}

			#endregion

			#region Private variables.

			private string key;
			private object value;
			private CacheItemInformation cacheInfo;

			private DateTime dateAdded = DateTime.Now;
			private DateTime lastAccessed = DateTime.Now;

			#endregion

			#region IDumpable members.

			/// <summary>
			/// Dumps the content to a string.
			/// </summary>
			/// <param name="indentLevel">How many tabs to indent.</param>
			/// <param name="deep">Whether to dump child objects.</param>
			/// <returns>
			/// Returns the textual representation of the dump.
			/// </returns>
			/// <remarks>
			/// Use the DumpBuilder class like this:
			/// "DumpBuilder db = new DumpBuilder( indentLevel, deep, GetType() );".
			/// </remarks>
			public string Dump(
				int indentLevel,
				bool deep )
			{
				DumpBuilder d = new DumpBuilder(
					indentLevel,
					deep,
					GetType() );

				d.AddLine( @"Date added", this.dateAdded );
				d.AddLine( @"Key", this.key );
				d.AddLine( @"Value", this.value );
				d.AddLine( @"Date last accessed", this.lastAccessed );
				d.AddLine( @"Expires after", this.EffectivelyExpiresAfter );

				return d.ToString();
			}

			/// <summary>
			/// Dumps the content to a string.
			/// </summary>
			/// <returns>
			/// Returns the textual representation of the dump.
			/// </returns>
			/// <remarks>
			/// Usually you just redirect this overload to "return Dump( 0, true );".
			/// </remarks>
			public string Dump()
			{
				return Dump( 0, false );
			}

			#endregion
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Function that is being called after a value is being set.
		/// The default checks syncronously for old items and deletes them.
		/// Override to provide your own implementation.
		/// </summary>
		protected virtual void OnBeforeValueSet()
		{
			CleanupOldEntries();
		}

		/// <summary>
		/// Function that is being called after a value is being set.
		/// The default checks syncronously for old items and deletes them.
		/// Override to provide your own implementation.
		/// </summary>
		protected virtual void OnAfterValueSet()
		{
		}

		/// <summary>
		/// Walks the stack and removes expired items.
		/// </summary>
		/// <returns>
		/// Returns the total number of items removed from
		/// the cache.
		/// </returns>
		protected int CleanupOldEntries()
		{
			lock ( thisLock )
			{
				int countBefore = inMemoryCache.Count;

				List<string> keysToRemove = new List<string>();

				// Collect the keys to remove.
				foreach ( string key in inMemoryCache.Keys )
				{
					SimpleCacheItemInformation info = inMemoryCache[key];

					if ( info.CheckIfRemove() )
					{
						keysToRemove.Add( key );
					}
				}

				// --

				// Actually remove.
				foreach ( string key in keysToRemove )
				{
					inMemoryCache.Remove( key );

					LogCentral.Current.LogDebug(
						string.Format(
						@"[Cache] Removed old cache item with key '{0}'.",
						key ) );
				}

				int countAfter = inMemoryCache.Count;

				return countAfter - countBefore;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// Simply store in-memory.
		/// </summary>
		private Dictionary<string, SimpleCacheItemInformation> inMemoryCache =
			new Dictionary<string, SimpleCacheItemInformation>();

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}