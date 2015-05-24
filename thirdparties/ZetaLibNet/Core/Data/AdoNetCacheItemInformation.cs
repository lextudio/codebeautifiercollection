namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections;
	using System.Data;
	using System.Data.OleDb;
	using System.Diagnostics;
	using ZetaLib.Core.Common;
	using ZetaLib.Core.Caching;
	using ZetaLib.Core.Logging;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// This class can be passed to various functions for controlling about
	/// how to deal with the cache.
	/// </summary>
	public class AdoNetCacheItemInformation :
		CacheItemInformation
	{
		#region Constructors.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public AdoNetCacheItemInformation()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="usage">The usage.</param>
		public AdoNetCacheItemInformation(
			CacheUsage usage )
		{
			this.usage = usage;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="usage">The usage.</param>
		/// <param name="groupName">Name of the group.</param>
		public AdoNetCacheItemInformation(
			CacheUsage usage,
			string groupName )
		{
			this.usage = usage;
			this.Group = new CacheItemGroup( groupName );
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="groupName">Name of the group.</param>
		public AdoNetCacheItemInformation(
			string groupName )
		{
			this.Group = new CacheItemGroup( groupName );
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="operation">The operation.</param>
		public AdoNetCacheItemInformation(
			AdoNetCacheDBOperation operation )
		{
			this.operation = operation;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="operation">The operation.</param>
		/// <param name="usage">The usage.</param>
		public AdoNetCacheItemInformation(
			AdoNetCacheDBOperation operation,
			CacheUsage usage )
		{
			this.operation = operation;
			this.usage = usage;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="operation">The operation.</param>
		/// <param name="usage">The usage.</param>
		/// <param name="groupName">Name of the group.</param>
		public AdoNetCacheItemInformation(
			AdoNetCacheDBOperation operation,
			CacheUsage usage,
			string groupName )
		{
			this.operation = operation;
			this.usage = usage;
			this.Group = new CacheItemGroup( groupName );
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="operation">The operation.</param>
		/// <param name="groupName">Name of the group.</param>
		public AdoNetCacheItemInformation(
			AdoNetCacheDBOperation operation,
			string groupName )
		{
			this.operation = operation;
			this.Group = new CacheItemGroup( groupName );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Tell the caching routines when/whether to use the cache or to
		/// not use the cache.
		/// </summary>
		/// <value>The usage.</value>
		public CacheUsage Usage
		{
			get
			{
				return usage;
			}
			set
			{
				usage = value;
			}
		}

		/// <summary>
		/// Tells the cache what kind of operation was performed, to allow
		/// for decision about whether to clear the cache after the operation.
		/// </summary>
		/// <value>The operation.</value>
		public AdoNetCacheDBOperation Operation
		{
			get
			{
				return operation;
			}
			set
			{
				operation = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private CacheUsage usage =
			CacheUsage.Default;
		private AdoNetCacheDBOperation operation =
			AdoNetCacheDBOperation.Default;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}