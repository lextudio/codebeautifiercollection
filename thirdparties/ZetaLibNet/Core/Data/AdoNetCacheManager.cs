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
	using System.Text;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Base ADO.NET cache. Usable, but is replaced in the Web version.
	/// </summary>
	public class AdoNetCacheManager
	{
		#region Static methods and properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Access the singleton.
		/// </summary>
		/// <value>The current.</value>
		public static AdoNetCacheManager Current
		{
			get
			{
				if ( currentCacheManager == null )
				{
					// According to 
					// http://www.dofactory.com/Patterns/PatternSingleton.aspx,
					// it is sufficient to lock only the creation.
					// 
					// Quote:
					//		Support multithreaded applications through
					//		'Double checked locking' pattern which (once
					//		the instance exists) avoids locking each
					//		time the method is invoked 					
					//
					// http://geekswithblogs.net/akraus1/articles/90803.aspx
					// has the correct way of locking: declaring as "volatile".
					//
					// http://www.ibm.com/developerworks/java/library/j-dcl.html
					// has an in-deep discussion.
					lock ( typeLock )
					{
						if ( currentCacheManager == null )
						{
							currentCacheManager = new AdoNetCacheManager();
						}
					}
				}

				return currentCacheManager;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public AdoNetCacheManager()
		{
			cacheManager = new CacheManager();
		}

		/// <summary>
		/// Initialize the class.
		/// </summary>
		public void Initialize()
		{
			cacheManager.Initialize();
		}

		/// <summary>
		/// If you are UPDATING the database, you should call the
		/// ClearCache function
		/// to be sure that all values read are correct.
		/// </summary>
		public void RemoveAll()
		{
			if ( CacheBehaviour !=
				LibraryConfiguration.DatabaseConfiguration.
				DatabaseCacheSqlBehavior.Off )
			{
				if ( cacheManager != null )
				{
					// Only remove my prefixes.
					cacheManager.RemoveAll( prefixCacheItemGroup );
				}
			}
		}

		/// <summary>
		/// Clear more selectively.
		/// </summary>
		/// <param name="cacheItemInfo">Uses the 'cacheKeyPrefix' member,
		/// if given, to delete only the specified keys. If the
		/// 'cacheKeyPrefix' member is null, all ADO.NET entries
		/// are cleared.</param>
		public void RemoveAll(
			AdoNetCacheItemInformation cacheItemInfo )
		{
			if ( CheckCanClearCache( cacheItemInfo ) )
			{
				if ( cacheItemInfo != null && !cacheItemInfo.Group.IsEmpty )
				{
					cacheManager.RemoveAll(
						new CacheItemGroup(
						prefixCacheItemGroup.Name + cacheItemInfo.Group.Name ) );
				}
				else
				{
					cacheManager.RemoveAll(
						prefixCacheItemGroup );
				}
			}
		}

		/// <summary>
		/// Clears the cache if the SQL is modifying or the cache item
		/// info says so (AdoNetCacheDBOperation.Modifying).
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public void RemoveAll(
			string sql,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			bool canClear;

			// --

			if ( AdoNetSqlHelper.IsModifyingQuery( sql ) )
			{
				canClear = true;
			}
			else
			{
				if ( cacheItemInfo != null &&
					cacheItemInfo.Operation ==
					AdoNetCacheDBOperation.Modifying )
				{
					canClear = true;
				}
				else
				{
					canClear = false;
				}
			}

			// --

			if ( canClear )
			{
				RemoveAll( cacheItemInfo );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Caching DataSets.
		// ------------------------------------------------------------------

		/// <summary>
		/// Try to read a DataSet from the cache.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns NULL if not found or cache is deactivated.
		/// </returns>
		public DataSet GetDataSetFromCache(
			SmartConnectionString connectionString,
			string spName,
			ICollection spParameters,
			int startIndex,
			int maxCount,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			if ( CheckCanGetFromCache( cacheItemInfo ) )
			{
				string pseudoSql =
					AdoNetBaseHelper<
						OleDbCommand,
						OleDbCommandBuilder,
						OleDbConnection,
						OleDbDataAdapter,
						OleDbParameter,
						AdoNetOleDBParamCollection>.
					QuickCreatePseudoSqlFromSP(
					spName,
					spParameters );

				return GetDataSetFromCache(
					connectionString,
					pseudoSql,
					startIndex,
					maxCount,
					cacheItemInfo );
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Try to read a DataSet from the cache.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns NULL if not found or cache is deactivated.
		/// </returns>
		public DataSet GetDataSetFromCache(
			SmartConnectionString connectionString,
			string sql,
			int startIndex,
			int maxCount,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			DataSet result;

			if ( CheckCanGetFromCache( cacheItemInfo ) )
			{
				string computedKey = ComputeKey(
					@"ds",
					cacheItemInfo,
					connectionString,
					sql,
					startIndex,
					maxCount );

				result =
					DoGetDataSetFromCache( computedKey, cacheItemInfo );
			}
			else
			{
				result = null;
			}

			// --

			if ( result == null )
			{
				totalHitCount++;
				LogCentral.Current.LogDebug(
					string.Format(
					@"AdoNetCache DataSet CACHE MISS ({0} hits, {1} misses totally).",
					totalHitCount,
					totalMissCount ) );
			}
			else
			{
				totalHitCount++;
				LogCentral.Current.LogDebug(
					string.Format(
					@"AdoNetCache DataSet CACHE HIT ({0} hits, {1} misses totally).",
					totalHitCount,
					totalMissCount ) );
			}

			// --

			return result;
		}

		/// <summary>
		/// Try to put a DataSet into the cache.
		/// Does nothing if the cache is deactivated.
		/// </summary>
		/// <param name="ds">The ds.</param>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public void PutDataSetToCache(
			DataSet ds,
			SmartConnectionString connectionString,
			string spName,
			ICollection spParameters,
			int startIndex,
			int maxCount,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			if ( CheckPutToCache( cacheItemInfo ) )
			{
				string pseudoSql =
					AdoNetBaseHelper<
						OleDbCommand,
						OleDbCommandBuilder,
						OleDbConnection,
						OleDbDataAdapter,
						OleDbParameter,
						AdoNetOleDBParamCollection>.
					QuickCreatePseudoSqlFromSP(
					spName,
					spParameters );

				PutDataSetToCache(
					ds,
					connectionString,
					pseudoSql,
					startIndex,
					maxCount,
					cacheItemInfo );
			}
		}

		/// <summary>
		/// Try to put a DataSet into the cache.
		/// Does nothing if the cache is deactivated.
		/// </summary>
		/// <param name="ds">The ds.</param>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public void PutDataSetToCache(
			DataSet ds,
			SmartConnectionString connectionString,
			string sql,
			int startIndex,
			int maxCount,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			if ( CheckPutToCache( cacheItemInfo ) )
			{
				string computedKey = ComputeKey(
					@"ds",
					cacheItemInfo,
					connectionString,
					sql,
					startIndex,
					maxCount );

				lock ( thisLock )
				{
					// Clean before, if applicable.
					RemoveAll( sql, cacheItemInfo );

					// Put to cache.
					DoPutDataSetToCache( computedKey, ds, cacheItemInfo );
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Caching DataTables.
		// ------------------------------------------------------------------

		/// <summary>
		/// Try to read a DataTable from the cache.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns NULL if not found or cache is deactivated.
		/// </returns>
		public DataTable GetDataTableFromCache(
			SmartConnectionString connectionString,
			string spName,
			ICollection spParameters,
			int startIndex,
			int maxCount,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			if ( CheckCanGetFromCache( cacheItemInfo ) )
			{
				string pseudoSql =
					AdoNetBaseHelper<
						OleDbCommand,
						OleDbCommandBuilder,
						OleDbConnection,
						OleDbDataAdapter,
						OleDbParameter,
						AdoNetOleDBParamCollection>.
					QuickCreatePseudoSqlFromSP(
					spName,
					spParameters );

				return GetDataTableFromCache(
					connectionString,
					pseudoSql,
					startIndex,
					maxCount,
					cacheItemInfo );
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Try to read a DataTable from the cache.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns NULL if not found or cache is deactivated.
		/// </returns>
		public DataTable GetDataTableFromCache(
			SmartConnectionString connectionString,
			string sql,
			int startIndex,
			int maxCount,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			DataTable result;

			if ( CheckCanGetFromCache( cacheItemInfo ) )
			{
				string computedKey = ComputeKey(
					@"dt",
					cacheItemInfo,
					connectionString,
					sql,
					startIndex,
					maxCount );

				result =
					DoGetDataTableFromCache( computedKey, cacheItemInfo );
			}
			else
			{
				result = null;
			}

			// --

			if ( result == null )
			{
				totalMissCount++;
				LogCentral.Current.LogDebug(
					string.Format(
					@"AdoNetCache DataTable CACHE MISS ({0} hits, {1} misses totally).",
					totalHitCount,
					totalMissCount ) );
			}
			else
			{
				totalHitCount++;
				LogCentral.Current.LogDebug(
					string.Format(
					@"AdoNetCache DataTable CACHE HIT ({0} hits, {1} misses totally).",
					totalHitCount,
					totalMissCount ) );
			}

			// --

			return result;
		}

		/// <summary>
		/// Try to put a DataTable into the cache.
		/// Does nothing if the cache is deactivated.
		/// </summary>
		/// <param name="ds">The ds.</param>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public void PutDataTableToCache(
			DataTable ds,
			SmartConnectionString connectionString,
			string spName,
			ICollection spParameters,
			int startIndex,
			int maxCount,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			if ( CheckPutToCache( cacheItemInfo ) )
			{
				string pseudoSql =
					AdoNetBaseHelper<
						OleDbCommand,
						OleDbCommandBuilder,
						OleDbConnection,
						OleDbDataAdapter,
						OleDbParameter,
						AdoNetOleDBParamCollection>.
					QuickCreatePseudoSqlFromSP(
					spName,
					spParameters );

				PutDataTableToCache(
					ds,
					connectionString,
					pseudoSql,
					startIndex,
					maxCount,
					cacheItemInfo );
			}
		}

		/// <summary>
		/// Try to put a DataTable into the cache.
		/// Does nothing if the cache is deactivated.
		/// </summary>
		/// <param name="ds">The ds.</param>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public void PutDataTableToCache(
			DataTable ds,
			SmartConnectionString connectionString,
			string sql,
			int startIndex,
			int maxCount,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			if ( CheckPutToCache( cacheItemInfo ) )
			{
				string computedKey = ComputeKey(
					@"dt",
					cacheItemInfo,
					connectionString,
					sql,
					startIndex,
					maxCount );

				lock ( thisLock )
				{
					// Clean before, if applicable.
					RemoveAll( sql, cacheItemInfo );

					// Put to cache.
					DoPutDataTableToCache( computedKey, ds, cacheItemInfo );
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Caching DataRows.
		// ------------------------------------------------------------------

		/// <summary>
		/// Try to read a DataRow from the cache.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns NULL if not found or cache is deactivated.
		/// </returns>
		public DataRow GetDataRowFromCache(
			SmartConnectionString connectionString,
			string spName,
			ICollection spParameters,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			if ( CheckCanGetFromCache( cacheItemInfo ) )
			{
				string pseudoSql =
					AdoNetBaseHelper<
						OleDbCommand,
						OleDbCommandBuilder,
						OleDbConnection,
						OleDbDataAdapter,
						OleDbParameter,
						AdoNetOleDBParamCollection>.
					QuickCreatePseudoSqlFromSP(
					spName,
					spParameters );

				return GetDataRowFromCache(
					connectionString,
					pseudoSql,
					cacheItemInfo );
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Try to read a DataRow from the cache.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns NULL if not found or cache is deactivated.
		/// </returns>
		public DataRow GetDataRowFromCache(
			SmartConnectionString connectionString,
			string sql,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			DataRow result;

			if ( CheckCanGetFromCache( cacheItemInfo ) )
			{
				string computedKey = ComputeKey(
					@"dr",
					cacheItemInfo,
					connectionString,
					sql );

				result =
					DoGetDataRowFromCache( computedKey, cacheItemInfo );
			}
			else
			{
				result = null;
			}

			// --

			if ( result == null )
			{
				totalMissCount++;
				LogCentral.Current.LogDebug(
					string.Format(
					@"AdoNetCache DataRow CACHE MISS ({0} hits, {1} misses totally).",
					totalHitCount,
					totalMissCount ) );
			}
			else
			{
				totalHitCount++;
				LogCentral.Current.LogDebug(
					string.Format(
					@"AdoNetCache DataRow CACHE HIT ({0} hits, {1} misses totally).",
					totalHitCount,
					totalMissCount ) );
			}

			// --

			return result;
		}

		/// <summary>
		/// Try to put a DataRow into the cache.
		/// Does nothing if the cache is deactivated.
		/// </summary>
		/// <param name="ds">The ds.</param>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public void PutDataRowToCache(
			DataRow ds,
			SmartConnectionString connectionString,
			string spName,
			ICollection spParameters,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			if ( CheckPutToCache( cacheItemInfo ) )
			{
				string pseudoSql =
					AdoNetBaseHelper<
						OleDbCommand,
						OleDbCommandBuilder,
						OleDbConnection,
						OleDbDataAdapter,
						OleDbParameter,
						AdoNetOleDBParamCollection>.
					QuickCreatePseudoSqlFromSP(
					spName,
					spParameters );

				PutDataRowToCache(
					ds,
					connectionString,
					pseudoSql,
					cacheItemInfo );
			}
		}

		/// <summary>
		/// Try to put a DataRow into the cache.
		/// Does nothing if the cache is deactivated.
		/// </summary>
		/// <param name="ds">The ds.</param>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public void PutDataRowToCache(
			DataRow ds,
			SmartConnectionString connectionString,
			string sql,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			if ( CheckPutToCache( cacheItemInfo ) )
			{
				string computedKey = ComputeKey(
					@"dr",
					cacheItemInfo,
					connectionString,
					sql );

				lock ( thisLock )
				{
					// Clean before, if applicable.
					RemoveAll( sql, cacheItemInfo );

					// Put to cache.
					DoPutDataRowToCache( computedKey, ds, cacheItemInfo );
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Caching Data values.
		// ------------------------------------------------------------------

		/// <summary>
		/// Try to read a data value from the cache.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns NULL if not found or cache is deactivated.
		/// </returns>
		public object GetDataValueFromCache(
			SmartConnectionString connectionString,
			string spName,
			ICollection spParameters,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			if ( CheckCanGetFromCache( cacheItemInfo ) )
			{
				string pseudoSql =
					AdoNetBaseHelper<
						OleDbCommand,
						OleDbCommandBuilder,
						OleDbConnection,
						OleDbDataAdapter,
						OleDbParameter,
						AdoNetOleDBParamCollection>.
					QuickCreatePseudoSqlFromSP(
					spName,
					spParameters );

				return GetDataValueFromCache(
					connectionString,
					pseudoSql,
					cacheItemInfo );
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Try to read a data value from the cache.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns NULL if not found or cache is deactivated.
		/// </returns>
		public object GetDataValueFromCache(
			SmartConnectionString connectionString,
			string sql,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			object result;

			if ( CheckCanGetFromCache( cacheItemInfo ) )
			{
				string computedKey = ComputeKey(
					@"dv",
					cacheItemInfo,
					connectionString,
					sql );

				result =
					DoGetDataValueFromCache( computedKey, cacheItemInfo );
			}
			else
			{
				result = null;
			}

			// --

			if ( result == null )
			{
				totalMissCount++;
				LogCentral.Current.LogDebug(
					string.Format(
					@"AdoNetCache data value CACHE MISS ({0} hits, {1} misses totally).",
					totalHitCount,
					totalMissCount ) );
			}
			else
			{
				totalHitCount++;
				LogCentral.Current.LogDebug(
					string.Format(
					@"AdoNetCache data value CACHE HIT ({0} hits, {1} misses totally).",
					totalHitCount,
					totalMissCount ) );
			}

			// --

			return result;
		}

		/// <summary>
		/// Try to put a data value into the cache.
		/// Does nothing if the cache is deactivated.
		/// </summary>
		/// <param name="ds">The ds.</param>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public void PutDataValueToCache(
			object ds,
			SmartConnectionString connectionString,
			string spName,
			ICollection spParameters,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			if ( CheckPutToCache( cacheItemInfo ) )
			{
				string pseudoSql =
					AdoNetBaseHelper<
						OleDbCommand,
						OleDbCommandBuilder,
						OleDbConnection,
						OleDbDataAdapter,
						OleDbParameter,
						AdoNetOleDBParamCollection>.
					QuickCreatePseudoSqlFromSP(
					spName,
					spParameters );

				PutDataValueToCache(
					ds,
					connectionString,
					pseudoSql,
					cacheItemInfo );
			}
		}

		/// <summary>
		/// Try to put a data value into the cache.
		/// Does nothing if the cache is deactivated.
		/// </summary>
		/// <param name="ds">The ds.</param>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public void PutDataValueToCache(
			object ds,
			SmartConnectionString connectionString,
			string sql,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			if ( CheckPutToCache( cacheItemInfo ) )
			{
				string computedKey = ComputeKey(
					@"dv",
					cacheItemInfo,
					connectionString,
					sql );

				lock ( thisLock )
				{
					// Clean before, if applicable.
					RemoveAll( sql, cacheItemInfo );

					// Put to cache.
					DoPutDataValueToCache( computedKey, ds, cacheItemInfo );
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// Best practice, see C# MSDN documentation of the "lock" keyword.
		/// </summary>
		private object thisLock = new object();

		/// <summary>
		/// The reference to the actual value.
		/// </summary>
		protected static volatile AdoNetCacheManager currentCacheManager = null;

		/// <summary>
		/// 
		/// </summary>
		private static object typeLock = new object();

		/// <summary>
		/// Count hits and misses.
		/// </summary>
		private int totalHitCount = 0;
		private int totalMissCount = 0;

		// ------------------------------------------------------------------
		#endregion

		#region Protected methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Does the get data set from cache.
		/// </summary>
		/// <param name="computedKey">The computed key.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns></returns>
		protected virtual DataSet DoGetDataSetFromCache(
			string computedKey,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return cacheManager.Get( computedKey ) as DataSet;
		}

		/// <summary>
		/// Does the put data set to cache.
		/// </summary>
		/// <param name="computedKey">The computed key.</param>
		/// <param name="ds">The ds.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		protected virtual void DoPutDataSetToCache(
			string computedKey,
			DataSet ds,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			// Must add my base group name.
			if ( cacheItemInfo == null ||
				cacheItemInfo.Group.IsEmpty )
			{
				cacheItemInfo.Group = prefixCacheItemGroup;
			}
			else
			{
				cacheItemInfo.Group =
					new CacheItemGroup(
					prefixCacheItemGroup.Name + cacheItemInfo.Group.Name );
			}

			// Put into cache, with correct expiration values.
			cacheManager.Set( computedKey, ds, cacheItemInfo );
		}

		/// <summary>
		/// Does the get data table from cache.
		/// </summary>
		/// <param name="computedKey">The computed key.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns></returns>
		protected virtual DataTable DoGetDataTableFromCache(
			string computedKey,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return cacheManager.Get( computedKey ) as DataTable;
		}

		/// <summary>
		/// Does the put data table to cache.
		/// </summary>
		/// <param name="computedKey">The computed key.</param>
		/// <param name="ds">The ds.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		protected virtual void DoPutDataTableToCache(
			string computedKey,
			DataTable ds,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			// Must add my base group name.
			if ( cacheItemInfo == null ||
				cacheItemInfo.Group.IsEmpty )
			{
				cacheItemInfo.Group = prefixCacheItemGroup;
			}
			else
			{
				cacheItemInfo.Group =
					new CacheItemGroup(
					prefixCacheItemGroup.Name + cacheItemInfo.Group.Name );
			}

			// Put into cache, with correct expiration values.
			cacheManager.Set( computedKey, ds, cacheItemInfo );
		}

		/// <summary>
		/// Does the get data row from cache.
		/// </summary>
		/// <param name="computedKey">The computed key.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns></returns>
		protected virtual DataRow DoGetDataRowFromCache(
			string computedKey,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return cacheManager.Get( computedKey ) as DataRow;
		}

		/// <summary>
		/// Does the put data row to cache.
		/// </summary>
		/// <param name="computedKey">The computed key.</param>
		/// <param name="ds">The ds.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		protected virtual void DoPutDataRowToCache(
			string computedKey,
			DataRow ds,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			// Must add my base group name.
			if ( cacheItemInfo == null ||
				cacheItemInfo.Group.IsEmpty )
			{
				cacheItemInfo.Group = prefixCacheItemGroup;
			}
			else
			{
				cacheItemInfo.Group =
					new CacheItemGroup(
					prefixCacheItemGroup.Name + cacheItemInfo.Group.Name );
			}

			// Put into cache, with correct expiration values.
			cacheManager.Set( computedKey, ds, cacheItemInfo );
		}

		/// <summary>
		/// Does the get data value from cache.
		/// </summary>
		/// <param name="computedKey">The computed key.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns></returns>
		protected virtual object DoGetDataValueFromCache(
			string computedKey,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return cacheManager.Get( computedKey ) as object;
		}

		/// <summary>
		/// Does the put data value to cache.
		/// </summary>
		/// <param name="computedKey">The computed key.</param>
		/// <param name="ds">The ds.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		protected virtual void DoPutDataValueToCache(
			string computedKey,
			object ds,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			// Must add my base group name.
			if ( cacheItemInfo == null ||
				cacheItemInfo.Group.IsEmpty )
			{
				cacheItemInfo.Group = prefixCacheItemGroup;
			}
			else
			{
				cacheItemInfo.Group =
					new CacheItemGroup(
					prefixCacheItemGroup.Name + cacheItemInfo.Group.Name );
			}

			// Put into cache, with correct expiration values.
			cacheManager.Set( computedKey, ds, cacheItemInfo );
		}

		/// <summary>
		/// Checks whether the cache should be cleared.
		/// </summary>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns></returns>
		private bool CheckCanClearCache(
			AdoNetCacheItemInformation cacheItemInfo )
		{
			if ( cacheManager == null )
			{
				return false;
			}
			else
			{
				if ( CacheBehaviour ==
					LibraryConfiguration.DatabaseConfiguration.
					DatabaseCacheSqlBehavior.Off )
				{
					return false;
				}
				else
				{
					if ( cacheItemInfo == null )
					{
						return false;
					}
					else
					{
						if (
							cacheItemInfo.Usage == CacheUsage.DontClear ||
							cacheItemInfo.Usage == CacheUsage.IgnoreCache )
						{
							return false;
						}
						else
						{
							return true;
						}
					}
				}
			}
		}

		/// <summary>
		/// Checks whether something should be get from the cache.
		/// </summary>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns></returns>
		private bool CheckCanGetFromCache(
			AdoNetCacheItemInformation cacheItemInfo )
		{
			if ( cacheManager == null )
			{
				return false;
			}
			else
			{
				if ( CacheBehaviour ==
					LibraryConfiguration.DatabaseConfiguration.DatabaseCacheSqlBehavior.Off )
				{
					return false;
				}
				else
				{
					if ( CacheBehaviour ==
						LibraryConfiguration.DatabaseConfiguration.
						DatabaseCacheSqlBehavior.On )
					{
						if ( cacheItemInfo == null ||
							cacheItemInfo.Usage == CacheUsage.IgnoreCache )
						{
							return false;
						}
						else
						{
							return true;
						}
					}
					else
					{
						if ( cacheItemInfo != null &&
							cacheItemInfo.Usage == CacheUsage.UseCache )
						{
							return true;
						}
						else
						{
							return false;
						}
					}
				}
			}
		}

		/// <summary>
		/// Checks whether something should be get from the cache.
		/// </summary>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns></returns>
		protected bool CheckPutToCache(
			AdoNetCacheItemInformation cacheItemInfo )
		{
			if ( cacheManager == null )
			{
				return false;
			}
			else
			{
				if ( CacheBehaviour ==
					LibraryConfiguration.DatabaseConfiguration.
					DatabaseCacheSqlBehavior.Off )
				{
					return false;
				}
				else
				{
					if ( CacheBehaviour ==
						LibraryConfiguration.DatabaseConfiguration.
						DatabaseCacheSqlBehavior.On )
					{
						if ( cacheItemInfo == null ||
							cacheItemInfo.Usage == CacheUsage.IgnoreCache )
						{
							return false;
						}
						else
						{
							return true;
						}
					}
					else
					{
						if ( cacheItemInfo != null &&
							cacheItemInfo.Usage == CacheUsage.UseCache )
						{
							return true;
						}
						else
						{
							return false;
						}
					}
				}
			}
		}

		/// <summary>
		/// Makes a unique key from the given spParameters.
		/// </summary>
		/// <param name="prefix">The prefix.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <returns></returns>
		private static string ComputeKey(
			string prefix,
			AdoNetCacheItemInformation cacheItemInfo,
			SmartConnectionString connectionString,
			string sql,
			int startIndex,
			int maxCount )
		{
			StringBuilder keyComputer = new StringBuilder();

			keyComputer.Length = 0;

			keyComputer.Append( prefix );
			keyComputer.Append( connectionString );
			keyComputer.Append( sql );
			keyComputer.Append( startIndex );
			keyComputer.Append( @"-" );
			keyComputer.Append( maxCount );

			return StringHelper.GenerateHash( keyComputer.ToString() );
		}

		/// <summary>
		/// Makes a unique key from the given spParameters.
		/// </summary>
		/// <param name="prefix">The prefix.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <returns></returns>
		private static string ComputeKey(
			string prefix,
			AdoNetCacheItemInformation cacheItemInfo,
			SmartConnectionString connectionString,
			string sql )
		{
			StringBuilder keyComputer = new StringBuilder();

			keyComputer.Length = 0;

			keyComputer.Append( prefix );
			keyComputer.Append( connectionString );
			keyComputer.Append( sql );

			return StringHelper.GenerateHash( keyComputer.ToString() );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Protected properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Only cache if enabled.
		/// </summary>
		/// <value>The cache behaviour.</value>
		public LibraryConfiguration.DatabaseConfiguration.DatabaseCacheSqlBehavior
			CacheBehaviour
		{
			get
			{
				lock ( thisLock )
				{
					return cacheBehaviour;
				}
			}
			set
			{
				lock ( thisLock )
				{
					cacheBehaviour = value;
				}
			}
		}

		/// <summary>
		/// Only cache if enabled.
		/// </summary>
		private LibraryConfiguration.DatabaseConfiguration.DatabaseCacheSqlBehavior
			cacheBehaviour =
			LibraryConfiguration.Current.Database.CacheSqlBehavior;

		/// <summary>
		/// The prefix for the keys in the cache.
		/// </summary>
		protected static readonly CacheItemGroup prefixCacheItemGroup =
			new CacheItemGroup( @"AdoNetCache." );

		/// <summary>
		/// Use the web cache manager.
		/// </summary>
		private CacheManager internalCacheManager = null;

		/// <summary>
		/// Gets or sets the cache manager.
		/// </summary>
		/// <value>The cache manager.</value>
		protected CacheManager cacheManager
		{
			get
			{
				lock ( thisLock )
				{
					return internalCacheManager;
				}
			}
			set
			{
				lock ( thisLock )
				{
					internalCacheManager = value;
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}