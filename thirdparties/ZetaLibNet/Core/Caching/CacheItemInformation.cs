namespace ZetaLib.Core.Caching
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using ZetaLib.Core.Properties;
	using System.Diagnostics;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Controls the treatment of a cached item.
	/// </summary>
	[DebuggerDisplay( @"GroupName = {groupName}" )]
	public class CacheItemInformation
	{
		#region Public enums.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		public enum CacheExpirationMode
		{
			#region Enum members.

			/// <summary>
			/// The 'AbsoluteExpiration' member is valid.
			/// </summary>
			Absolute,

			/// <summary>
			/// The 'SlidingExpiration' member is valid.
			/// </summary>
			Sliding,

			/// <summary>
			/// Never expires.
			/// </summary>
			Never

			#endregion
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets or sets the absolute expiration.
		/// </summary>
		/// <value>The absolute expiration.</value>
		public DateTime AbsoluteExpiration
		{
			get
			{
				return absoluteExpiration;
			}
			set
			{
				absoluteExpiration = value;
			}
		}

		/// <summary>
		/// Gets or sets the sliding expiration.
		/// </summary>
		/// <value>The sliding expiration.</value>
		public TimeSpan SlidingExpiration
		{
			get
			{
				return slidingExpiration;
			}
			set
			{
				slidingExpiration = value;
			}
		}

		/// <summary>
		/// When the cache should expire.
		/// </summary>
		/// <value>The expiration mode.</value>
		public CacheExpirationMode ExpirationMode
		{
			get
			{
				if ( absoluteExpiration != NoAbsoluteExpiration )
				{
					return CacheExpirationMode.Absolute;
				}
				else if ( slidingExpiration != NoSlidingExpiration )
				{
					return CacheExpirationMode.Sliding;
				}
				else
				{
					return CacheExpirationMode.Never;
				}
			}
		}

		/// <summary>
		/// Creates a default item.
		/// </summary>
		/// <value>The default.</value>
		public static CacheItemInformation Default
		{
			get
			{
				CacheItemInformation info = new CacheItemInformation();
				return info;
			}
		}

		/// <summary>
		/// If this value is non-empty, this value is used to generate
		/// the key to store in the cache, or to delete from the cache.
		/// If this value is empty, the key is generated from the SQL
		/// query only.
		/// </summary>
		/// <value>The group.</value>
		public CacheItemGroup Group
		{
			get
			{
				return new CacheItemGroup( groupName );
			}
			set
			{
				// Cannot be empty.
				if ( value == null || value.IsEmpty )
				{
					throw new ArgumentException(
						Resources.Str_ZetaLib_Core_Caching_CacheItemInformation_01,
						@"Group" );
				}
				else
				{
					groupName = value.Name;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly DateTime NoAbsoluteExpiration =
			DateTime.MaxValue;

		/// <summary>
		/// 
		/// </summary>
		public static readonly TimeSpan NoSlidingExpiration =
			TimeSpan.Zero;

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private DateTime absoluteExpiration =
			DateTime.Now.Add( TimeSpan.FromMinutes( 20 ) );

		/// <summary>
		/// 
		/// </summary>
		private TimeSpan slidingExpiration =
			TimeSpan.FromMinutes( 20 );

		/// <summary>
		/// 
		/// </summary>
		private string groupName = null;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}