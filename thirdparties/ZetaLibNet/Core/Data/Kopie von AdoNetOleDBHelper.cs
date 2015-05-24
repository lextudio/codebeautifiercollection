namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Configuration;
	using System.Collections;
	using System.Data;
	using System.Data.OleDb;
	using System.Diagnostics;
	using System.IO;
	using System.Security.Cryptography;
	using System.Text.RegularExpressions;

	using ZetaLib.Core.Common;
	using System.Collections.Generic;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Helper class with statics, dealing with databases.
	/// </summary>
	/// <remarks>Parameters in the application configuration file (e.g. "web.config)":
	/// - "connectionString": The connection string for connecting to the database.
	/// - "traceSqlEnabled": Turn tracing of SQL statements to LOG4NET on/off.
	/// - "cacheSqlEnabled": Turn caching of SQL statements and their result inside
	/// the ASP.NET web cache on/off.
	/// - "commandTimeoutSeconds": Define an optional timeout for a command to execute.
	/// Please note that this parameter is different from the connection timeout which
	/// can be defined inside the connection string.</remarks>
	public sealed class AdoNetOleDBHelper :
		AdoNetBaseHelper<
		OleDbCommand,
		OleDbCommandBuilder,
		OleDbConnection,
		OleDbDataAdapter,
		OleDbParameter,
		AdoNetOleDBParamCollection>
	{
		#region Executes for DataSets (direct SQL commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			string sql,
			AdoNetBaseCacheControl cacheControl )
		{
			return Execute(
				ConnectionString,
				sql,
				cacheControl );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="sql"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			string sql )
		{
			return Execute( ConnectionString, sql );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="startIndex"></param>
		/// <param name="maxCount"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			string sql,
			int startIndex,
			int maxCount,
			AdoNetBaseCacheControl cacheControl )
		{
			return Execute(
				ConnectionString,
				sql,
				startIndex,
				maxCount,
				cacheControl );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="startIndex"></param>
		/// <param name="maxCount"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			string sql,
			int startIndex,
			int maxCount )
		{
			return Execute(
				ConnectionString,
				sql,
				startIndex,
				maxCount );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="startIndex"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			string sql,
			int startIndex,
			AdoNetBaseCacheControl cacheControl )
		{
			return Execute(
				ConnectionString,
				sql,
				startIndex,
				cacheControl );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="startIndex"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			string sql,
			int startIndex )
		{
			return Execute(
				ConnectionString,
				sql,
				startIndex,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="sql"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			SmartConnectionString connectionString,
			string sql,
			AdoNetBaseCacheControl cacheControl )
		{
			return Execute( connectionString, sql, -1, -1, cacheControl );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="sql"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			SmartConnectionString connectionString,
			string sql )
		{
			return Execute( connectionString, sql, null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="sql"></param>
		/// <param name="startIndex"></param>
		/// <param name="maxCount"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			SmartConnectionString connectionString,
			string sql,
			int startIndex,
			int maxCount,
			AdoNetBaseCacheControl cacheControl )
		{
			// Cache?
			DataSet cacheDataSet;
			if ( AdoNetBaseCacheManager.Current == null )
			{
				cacheDataSet = null;
			}
			else
			{
				cacheDataSet = AdoNetBaseCacheManager.Current.GetDatasetFromCache(
					connectionString,
					sql,
					startIndex,
					maxCount,
					cacheControl );
			}

			if ( cacheDataSet != null )
			{
				return cacheDataSet;
			}
			else
			{
				using ( OleDbConnection conn = new OleDbConnection( connectionString.ConnectionString ) )
				{
					conn.Open();

					OleDbDataAdapter da = new OleDbDataAdapter( sql, conn );

					// Apply command timeouts, if any.
					CheckSetCommandTimeout( da.DeleteCommand );
					CheckSetCommandTimeout( da.InsertCommand );
					CheckSetCommandTimeout( da.SelectCommand );
					CheckSetCommandTimeout( da.UpdateCommand );

					DataSet ds = new DataSet();
					TraceSql( sql, true, ds );
					try
					{
						if ( startIndex == -1 && maxCount == -1 )
						{
							da.Fill(
								ds,
								AdoNetOleDBUpdater.DetectTableName( sql ) );
						}
						else
						{
							da.Fill(
								ds,
								startIndex,
								maxCount,
								AdoNetOleDBUpdater.DetectTableName( sql ) );
						}
					}
					catch ( Exception x )
					{
						TraceSqlError( sql, x );
						throw;
					}
					TraceSql( sql, false, ds );

					// Cache!(?)
					if ( AdoNetBaseCacheManager.Current != null )
					{
						AdoNetBaseCacheManager.Current.PutDatasetToCache(
							ds,
							connectionString,
							sql,
							startIndex,
							maxCount,
							cacheControl );
					}

					return ds;
				}
			}
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="sql"></param>
		/// <param name="startIndex"></param>
		/// <param name="maxCount"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			SmartConnectionString connectionString,
			string sql,
			int startIndex,
			int maxCount )
		{
			return Execute(
				connectionString,
				sql,
				startIndex,
				maxCount,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="sql"></param>
		/// <param name="startIndex"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			SmartConnectionString connectionString,
			string sql,
			int startIndex,
			AdoNetBaseCacheControl cacheControl )
		{
			return Execute(
				connectionString,
				sql,
				startIndex,
				int.MaxValue,
				cacheControl );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes for DataSets (stored procedure commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			string spName,
			AdoNetOleDBParamCollection spParameters,
			AdoNetBaseCacheControl cacheControl )
		{
			return Execute(
				ConnectionString,
				spName,
				spParameters,
				cacheControl );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			string spName,
			AdoNetOleDBParamCollection spParameters )
		{
			return Execute(
				ConnectionString,
				spName,
				spParameters );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="startIndex"></param>
		/// <param name="maxCount"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			string spName,
			AdoNetOleDBParamCollection spParameters,
			int startIndex,
			int maxCount,
			AdoNetBaseCacheControl cacheControl )
		{
			return Execute(
				ConnectionString,
				spName,
				spParameters,
				startIndex,
				maxCount,
				cacheControl );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="startIndex"></param>
		/// <param name="maxCount"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			string spName,
			AdoNetOleDBParamCollection spParameters,
			int startIndex,
			int maxCount )
		{
			return Execute(
				ConnectionString,
				spName,
				spParameters,
				startIndex,
				maxCount );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="startIndex"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			string spName,
			AdoNetOleDBParamCollection spParameters,
			int startIndex,
			AdoNetBaseCacheControl cacheControl )
		{
			return Execute(
				ConnectionString,
				spName,
				spParameters,
				startIndex,
				cacheControl );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="startIndex"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			string spName,
			AdoNetOleDBParamCollection spParameters,
			int startIndex )
		{
			return Execute(
				ConnectionString,
				spName,
				spParameters,
				startIndex,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters,
			AdoNetBaseCacheControl cacheControl )
		{
			return Execute(
				connectionString,
				spName,
				spParameters,
				-1,
				-1,
				cacheControl );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters )
		{
			return Execute(
				connectionString,
				spName,
				spParameters,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="startIndex"></param>
		/// <param name="maxCount"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters,
			int startIndex,
			int maxCount )
		{
			return Execute(
				connectionString,
				spName,
				spParameters,
				startIndex,
				maxCount,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="startIndex"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters,
			int startIndex,
			AdoNetBaseCacheControl cacheControl )
		{
			return Execute(
				connectionString,
				spName,
				spParameters,
				startIndex,
				int.MaxValue,
				cacheControl );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="startIndex"></param>
		/// <param name="maxCount"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataSet or NULL if no data is found.</returns>
		public static DataSet Execute(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters,
			int startIndex,
			int maxCount,
			AdoNetBaseCacheControl cacheControl )
		{
			// Cache?
			DataSet cacheDataSet;
			if ( AdoNetBaseCacheManager.Current == null )
			{
				cacheDataSet = null;
			}
			else
			{
				cacheDataSet = AdoNetBaseCacheManager.Current.GetDatasetFromCache(
					connectionString,
					spName,
					spParameters,
					startIndex,
					maxCount,
					cacheControl );
			}

			if ( cacheDataSet != null )
			{
				return cacheDataSet;
			}
			else
			{
				using ( OleDbConnection conn = new OleDbConnection( connectionString.ConnectionString ) )
				{
					OleDbCommand cmd = new OleDbCommand( spName, conn );
					cmd.CommandType = DetectCommandType( spName );

					// Apply command timeouts, if any.
					CheckSetCommandTimeout( cmd );

					if ( spParameters != null )
					{
						foreach ( OleDbParameter parameter in spParameters )
						{
							cmd.Parameters.Add( parameter );
						}
					}

					conn.Open();

					OleDbDataAdapter da = new OleDbDataAdapter( cmd );

					// Apply command timeouts, if any.
					CheckSetCommandTimeout( da.DeleteCommand );
					CheckSetCommandTimeout( da.InsertCommand );
					CheckSetCommandTimeout( da.SelectCommand );
					CheckSetCommandTimeout( da.UpdateCommand );

					DataSet ds = new DataSet();
					TraceSql( cmd, true, ds );
					try
					{
						if ( startIndex == -1 && maxCount == -1 )
						{
							da.Fill(
								ds,
								AdoNetOleDBUpdater.DetectTableName(
								CreatePseudoSqlFromSP(
								cmd.CommandText,
								cmd.Parameters ) ) );
						}
						else
						{
							da.Fill(
								ds,
								startIndex,
								maxCount,
								AdoNetOleDBUpdater.DetectTableName(
								CreatePseudoSqlFromSP(
								cmd.CommandText,
								cmd.Parameters ) ) );
						}
					}
					catch ( Exception x )
					{
						TraceSqlError( cmd, x );
						throw;
					}
					TraceSql( cmd, false, ds );

					// Cache!(?)
					if ( AdoNetBaseCacheManager.Current != null )
					{
						AdoNetBaseCacheManager.Current.PutDatasetToCache(
							ds,
							connectionString,
							spName,
							spParameters,
							startIndex,
							maxCount,
							cacheControl );
					}

					return ds;
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes for DataTables (direct SQL commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			string sql,
			AdoNetBaseCacheControl
			cacheControl )
		{
			return ExecuteTable(
				ConnectionString,
				sql,
				cacheControl );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="sql"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			string sql )
		{
			return ExecuteTable(
				ConnectionString,
				sql,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="startIndex"></param>
		/// <param name="maxCount"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			string sql,
			int startIndex,
			int maxCount,
			AdoNetBaseCacheControl cacheControl )
		{
			return ExecuteTable(
				ConnectionString,
				sql,
				startIndex,
				maxCount,
				cacheControl );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="startIndex"></param>
		/// <param name="maxCount"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			string sql,
			int startIndex,
			int maxCount )
		{
			return ExecuteTable(
				ConnectionString,
				sql,
				startIndex,
				maxCount,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="startIndex"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			string sql,
			int startIndex,
			AdoNetBaseCacheControl cacheControl )
		{
			return ExecuteTable(
				ConnectionString,
				sql,
				startIndex,
				cacheControl );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="startIndex"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			string sql,
			int startIndex )
		{
			return ExecuteTable(
				ConnectionString,
				sql,
				startIndex,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="sql"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string sql,
			AdoNetBaseCacheControl cacheControl )
		{
			DataSet ds = Execute( connectionString, sql, cacheControl );

			// Only return if has a table AND if the table has rows at all.
			if ( ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 )
			{
				return ds.Tables[0];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="sql"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string sql )
		{
			return ExecuteTable(
				connectionString,
				sql,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="sql"></param>
		/// <param name="startIndex"></param>
		/// <param name="maxCount"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string sql,
			int startIndex,
			int maxCount,
			AdoNetBaseCacheControl cacheControl )
		{
			DataSet ds = Execute(
				connectionString,
				sql,
				startIndex,
				maxCount,
				cacheControl );

			// Only return if has a table AND if the table has rows at all.
			if ( ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 )
			{
				return ds.Tables[0];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="sql"></param>
		/// <param name="startIndex"></param>
		/// <param name="maxCount"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string sql,
			int startIndex,
			int maxCount )
		{
			return ExecuteTable(
				connectionString,
				sql,
				startIndex,
				maxCount,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="sql"></param>
		/// <param name="startIndex"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string sql,
			int startIndex,
			AdoNetBaseCacheControl cacheControl )
		{
			return ExecuteTable(
				connectionString,
				sql,
				startIndex,
				int.MaxValue,
				cacheControl );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes for DataTables (stored procedure commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			string spName,
			AdoNetOleDBParamCollection spParameters,
			AdoNetBaseCacheControl cacheControl )
		{
			return ExecuteTable(
				ConnectionString,
				spName,
				spParameters,
				cacheControl );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			string spName,
			AdoNetOleDBParamCollection spParameters )
		{
			return ExecuteTable(
				ConnectionString,
				spName,
				spParameters,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="startIndex"></param>
		/// <param name="maxCount"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			string spName,
			AdoNetOleDBParamCollection spParameters,
			int startIndex,
			int maxCount,
			AdoNetBaseCacheControl cacheControl )
		{
			return ExecuteTable(
				ConnectionString,
				spName,
				spParameters,
				startIndex,
				maxCount,
				cacheControl );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="startIndex"></param>
		/// <param name="maxCount"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			string spName,
			AdoNetOleDBParamCollection spParameters,
			int startIndex,
			int maxCount )
		{
			return ExecuteTable(
				ConnectionString,
				spName,
				spParameters,
				startIndex,
				maxCount,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="startIndex"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			string spName,
			AdoNetOleDBParamCollection spParameters,
			int startIndex,
			AdoNetBaseCacheControl cacheControl )
		{
			return ExecuteTable(
				ConnectionString,
				spName,
				spParameters,
				startIndex,
				cacheControl );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="startIndex"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			string spName,
			AdoNetOleDBParamCollection spParameters,
			int startIndex )
		{
			return ExecuteTable(
				ConnectionString,
				spName,
				spParameters,
				startIndex,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters,
			AdoNetBaseCacheControl cacheControl )
		{
			DataSet ds = Execute(
				connectionString,
				spName,
				spParameters,
				cacheControl );

			// Only return if has a table AND if the table has rows at all.
			if ( ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 )
			{
				return ds.Tables[0];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters )
		{
			return ExecuteTable(
				connectionString,
				spName,
				spParameters,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="startIndex"></param>
		/// <param name="maxCount"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters,
			int startIndex,
			int maxCount,
			AdoNetBaseCacheControl cacheControl )
		{
			DataSet ds = Execute(
				connectionString,
				spName,
				spParameters,
				startIndex,
				maxCount,
				cacheControl );

			// Only return if has a table AND if the table has rows at all.
			if ( ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 )
			{
				return ds.Tables[0];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="startIndex"></param>
		/// <param name="maxCount"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters,
			int startIndex,
			int maxCount )
		{
			return ExecuteTable(
				connectionString,
				spName,
				spParameters,
				startIndex,
				maxCount,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="startIndex"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataTable or NULL if no data is found.</returns>
		public static DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters,
			int startIndex,
			AdoNetBaseCacheControl cacheControl )
		{
			return ExecuteTable(
				connectionString,
				spName,
				spParameters,
				startIndex,
				int.MaxValue,
				cacheControl );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes for DataRows (direct SQL commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for a DataRow.
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataRow or NULL if no data is found.</returns>
		public static DataRow ExecuteRow(
			string sql,
			AdoNetBaseCacheControl cacheControl )
		{
			return ExecuteRow(
				ConnectionString,
				sql,
				cacheControl );
		}

		/// <summary>
		/// Query for a DataRow.
		/// </summary>
		/// <param name="sql"></param>
		/// <returns>Returns the DataRow or NULL if no data is found.</returns>
		public static DataRow ExecuteRow(
			string sql )
		{
			return ExecuteRow(
				ConnectionString,
				sql );
		}

		/// <summary>
		/// Query for a DataRow.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="sql"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataRow or NULL if no data is found.</returns>
		public static DataRow ExecuteRow(
			SmartConnectionString connectionString,
			string sql,
			AdoNetBaseCacheControl cacheControl )
		{
			DataTable dt = ExecuteTable( connectionString, sql, 0, 1, cacheControl );

			if ( dt != null && dt.Rows.Count > 0 )
			{
				return dt.Rows[0];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Query for a DataRow.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="sql"></param>
		/// <returns>Returns the DataRow or NULL if no data is found.</returns>
		public static DataRow ExecuteRow(
			SmartConnectionString connectionString,
			string sql )
		{
			return ExecuteRow( connectionString, sql, null as AdoNetBaseCacheControl );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes for DataRows (stored procedure commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for a DataRow.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataRow or NULL if no data is found.</returns>
		public static DataRow ExecuteRow(
			string spName,
			AdoNetOleDBParamCollection spParameters,
			AdoNetBaseCacheControl cacheControl )
		{
			return ExecuteRow(
				ConnectionString,
				spName,
				spParameters,
				cacheControl );
		}

		/// <summary>
		/// Query for a DataRow.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <returns>Returns the DataRow or NULL if no data is found.</returns>
		public static DataRow ExecuteRow(
			string spName,
			AdoNetOleDBParamCollection spParameters )
		{
			return ExecuteRow(
				ConnectionString,
				spName,
				spParameters );
		}

		/// <summary>
		/// Query for a DataRow.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the DataRow or NULL if no data is found.</returns>
		public static DataRow ExecuteRow(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters,
			AdoNetBaseCacheControl cacheControl )
		{
			DataTable dt = ExecuteTable(
				connectionString,
				spName,
				spParameters,
				0,
				1,
				cacheControl );

			if ( dt != null && dt.Rows.Count > 0 )
			{
				return dt.Rows[0];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Query for a DataRow.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <returns>Returns the DataRow or NULL if no data is found.</returns>
		public static DataRow ExecuteRow(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters )
		{
			return ExecuteRow(
				connectionString,
				spName,
				spParameters,
				null as AdoNetBaseCacheControl );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes for values (direct SQL commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for a single value.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="sql"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the value or NULL if no data is found.</returns>
		public static object ExecuteValue(
			SmartConnectionString connectionString,
			string sql,
			AdoNetBaseCacheControl cacheControl )
		{
			DataRow row = ExecuteRow( connectionString, sql, cacheControl );

			if ( row != null )
			{
				if ( row[0] == null || row[0] == DBNull.Value )
				{
					return null;
				}
				else
				{
					return row[0];
				}
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Query for a single value.
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the value or NULL if no data is found.</returns>
		public static object ExecuteValue(
			string sql,
			AdoNetBaseCacheControl cacheControl )
		{
			return ExecuteValue(
				ConnectionString,
				sql,
				cacheControl );
		}

		/// <summary>
		/// Query for a single value.
		/// </summary>
		/// <param name="sql"></param>
		/// <returns>Returns the value or NULL if no data is found.</returns>
		public static object ExecuteValue(
			string sql )
		{
			return ExecuteValue(
				ConnectionString,
				sql );
		}

		/// <summary>
		/// Query for a single value.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="sql"></param>
		/// <returns>Returns the value or NULL if no data is found.</returns>
		public static object ExecuteValue(
			SmartConnectionString connectionString,
			string sql )
		{
			return ExecuteValue(
				connectionString,
				sql,
				null as AdoNetBaseCacheControl );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes for values (stored procedure commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for a single value.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the value or NULL if no data is found.</returns>
		public static object ExecuteValue(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters,
			AdoNetBaseCacheControl cacheControl )
		{
			DataRow row = ExecuteRow(
				connectionString,
				spName,
				spParameters,
				cacheControl );

			if ( row != null )
			{
				if ( row[0] == null || row[0] == DBNull.Value )
				{
					return null;
				}
				else
				{
					return row[0];
				}
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Query for a single value.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="cacheControl"></param>
		/// <returns>Returns the value or NULL if no data is found.</returns>
		public static object ExecuteValue(
			string spName,
			AdoNetOleDBParamCollection spParameters,
			AdoNetBaseCacheControl cacheControl )
		{
			return ExecuteValue(
				ConnectionString,
				spName,
				spParameters,
				cacheControl );
		}

		/// <summary>
		/// Query for a single value.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <returns>Returns the value or NULL if no data is found.</returns>
		public static object ExecuteValue(
			string spName,
			AdoNetOleDBParamCollection spParameters )
		{
			return ExecuteValue(
				ConnectionString,
				spName,
				spParameters );
		}

		/// <summary>
		/// Query for a single value.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <returns>Returns the value or NULL if no data is found.</returns>
		public static object ExecuteValue(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters )
		{
			return ExecuteValue(
				connectionString,
				spName,
				spParameters,
				null as AdoNetBaseCacheControl );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes without return value (direct SQL commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Execute a query without returning a value.
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="cacheControl"></param>
		public static void ExecuteNonQuery(
			string sql,
			AdoNetBaseCacheControl cacheControl )
		{
			ExecuteNonQuery(
				ConnectionString,
				sql,
				cacheControl );
		}

		/// <summary>
		/// Execute a query without returning a value.
		/// </summary>
		/// <param name="sql"></param>
		public static void ExecuteNonQuery(
			string sql )
		{
			ExecuteNonQuery(
				ConnectionString,
				sql,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Execute a query without returning a value.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="sql"></param>
		public static void ExecuteNonQuery(
			SmartConnectionString connectionString,
			string sql )
		{
			ExecuteNonQuery(
				connectionString,
				sql,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Execute a query without returning a value.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="sql"></param>
		/// <param name="cacheControl"></param>
		public static void ExecuteNonQuery(
			SmartConnectionString connectionString,
			string sql,
			AdoNetBaseCacheControl cacheControl )
		{
			using ( OleDbConnection conn = new OleDbConnection( connectionString.ConnectionString ) )
			{
				conn.Open();

				OleDbCommand cmd = new OleDbCommand( sql, conn );

				// Apply command timeouts, if any.
				CheckSetCommandTimeout( cmd );

				TraceSql( sql, true, null );
				try
				{
					cmd.ExecuteNonQuery();
				}
				catch ( Exception x )
				{
					TraceSqlError( sql, x );
					throw;
				}
				TraceSql( sql, false, null );

				// Cache.
				if ( IsModifyingQuery( sql ) )
				{
					AdoNetBaseCacheManager.Current.ClearCache( cacheControl );
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes without return value (stored procedure commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Execute a query without returning a value.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="cacheControl"></param>
		public static void ExecuteNonQuery(
			string spName,
			AdoNetOleDBParamCollection spParameters,
			AdoNetBaseCacheControl cacheControl )
		{
			ExecuteNonQuery(
				ConnectionString,
				spName,
				spParameters,
				cacheControl );
		}

		/// <summary>
		/// Execute a query without returning a value.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		public static void ExecuteNonQuery(
			string spName,
			AdoNetOleDBParamCollection spParameters )
		{
			ExecuteNonQuery(
				ConnectionString,
				spName,
				spParameters,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Execute a query without returning a value.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		public static void ExecuteNonQuery(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters )
		{
			ExecuteNonQuery(
				connectionString,
				spName,
				spParameters,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Execute a query without returning a value.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <param name="cacheControl"></param>
		public static void ExecuteNonQuery(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters,
			AdoNetBaseCacheControl cacheControl )
		{
			using ( OleDbConnection conn = new OleDbConnection( connectionString.ConnectionString ) )
			{
				conn.Open();

				OleDbCommand cmd = new OleDbCommand( spName, conn );
				cmd.CommandType = DetectCommandType( spName );

				// Apply command timeouts, if any.
				CheckSetCommandTimeout( cmd );

				if ( spParameters != null )
				{
					foreach ( OleDbParameter parameter in spParameters )
					{
						cmd.Parameters.Add( parameter );
					}
				}

				TraceSql( cmd, true, null );
				try
				{
					cmd.ExecuteNonQuery();
				}
				catch ( Exception x )
				{
					TraceSqlError( cmd, x );
					throw;
				}
				TraceSql( cmd, false, null );

				// Cache.
				/*
				if ( IsModifyingQuery( sql ) )
				{
					AdoNetBaseCacheManager.Current.ClearCache( cacheControl );
				}
				*/
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes for updateable DataAdapters (direct SQL commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for an updatable DataAdapter.
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		public static OleDbDataAdapter ExecuteForUpdate(
			string sql )
		{
			return ExecuteForUpdate(
				ConnectionString,
				sql );
		}

		/// <summary>
		/// Query for an updatable DataAdapter.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="sql"></param>
		/// <returns></returns>
		public static OleDbDataAdapter ExecuteForUpdate(
			SmartConnectionString connectionString,
			string sql )
		{
			OleDbConnection conn = new OleDbConnection( connectionString.ConnectionString );
			OleDbDataAdapter da = new OleDbDataAdapter( sql, conn );

			// Apply command timeouts, if any.
			CheckSetCommandTimeout( da.DeleteCommand );
			CheckSetCommandTimeout( da.InsertCommand );
			CheckSetCommandTimeout( da.SelectCommand );
			CheckSetCommandTimeout( da.UpdateCommand );

			OleDbCommandBuilder cb = new OleDbCommandBuilder( da );
			cb.QuotePrefix = @"[";
			cb.QuoteSuffix = @"]";

			return da;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes for updateable DataAdapters (stored procedure commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for an updatable DataAdapter.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <returns></returns>
		public static OleDbDataAdapter ExecuteForUpdate(
			string spName,
			AdoNetOleDBParamCollection spParameters )
		{
			return ExecuteForUpdate(
				ConnectionString,
				spName,
				spParameters );
		}

		/// <summary>
		/// Query for an updatable DataAdapter.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		/// <returns></returns>
		public static OleDbDataAdapter ExecuteForUpdate(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters )
		{
			OleDbConnection conn = new OleDbConnection( connectionString.ConnectionString );
			OleDbCommand cmd = new OleDbCommand( spName, conn );

			// Apply command timeouts, if any.
			CheckSetCommandTimeout( cmd );

			cmd.CommandType = DetectCommandType( spName );

			if ( spParameters != null )
			{
				foreach ( OleDbParameter parameter in spParameters )
				{
					cmd.Parameters.Add( parameter );
				}
			}

			conn.Open();

			OleDbDataAdapter da = new OleDbDataAdapter( cmd );

			// Apply command timeouts, if any.
			CheckSetCommandTimeout( da.DeleteCommand );
			CheckSetCommandTimeout( da.InsertCommand );
			CheckSetCommandTimeout( da.SelectCommand );
			CheckSetCommandTimeout( da.UpdateCommand );

			OleDbCommandBuilder cb = new OleDbCommandBuilder( da );
			cb.QuotePrefix = @"[";
			cb.QuoteSuffix = @"]";

			return da;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Schema routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Drops the column of a given table if both the table and the column exists.
		/// </summary>
		public static void DropTableColumn(
			string tableName,
			string columnName )
		{
			DropTableColumn(
				ConnectionString,
				tableName,
				columnName );
		}

		/// <summary>
		/// Drops the column of a given table if both the table and the column exists.
		/// </summary>
		public static void DropTableColumn(
			SmartConnectionString connectionString,
			string tableName,
			string columnName )
		{
			if ( ContainsTable( connectionString, tableName ) &&
				ContainsTableColumn( connectionString, tableName, columnName ) )
			{
				tableName = tableName.Trim( '[', ']' );
				columnName = columnName.Trim( '[', ']' );

				ExecuteNonQuery(
					connectionString,
					string.Format(
					@"ALTER TABLE [{0}] DROP COLUMN [{1}]",
					tableName,
					columnName ) );
			}
		}

		/// <summary>
		/// Check whether a constrained is present.
		/// For creating and dropping constraints, see http://support.microsoft.com/?scid=kb%3Ben-us%3B291539.
		/// </summary>
		public static bool ContainsConstraint(
			string constraintName )
		{
			return ContainsConstraint( ConnectionString, constraintName );
		}

		/// <summary>
		/// Check whether a constrained is present.
		/// For creating and dropping constraints, see http://support.microsoft.com/?scid=kb%3Ben-us%3B291539.
		/// </summary>
		public static bool ContainsConstraint(
			SmartConnectionString connectionString,
			string constraintName )
		{
			string[] constraintNames = GetConstraintNames( connectionString );
			if ( constraintNames == null || constraintNames.Length <= 0 )
			{
				return false;
			}
			else
			{
				constraintName = constraintName.Trim( '[', ']' );

				foreach ( string constraintName_ in constraintNames )
				{
					if ( string.Compare( constraintName_, constraintName, true ) == 0 )
					{
						return true;
					}
				}

				return false;
			}
		}

		/// <summary>
		/// Check whether a constrained is present.
		/// For creating and dropping constraints, see http://support.microsoft.com/?scid=kb%3Ben-us%3B291539.
		/// </summary>
		public static string[] GetConstraintNames()
		{
			return GetConstraintNames( ConnectionString );
		}

		/// <summary>
		/// Check whether a constrained is present.
		/// For creating and dropping constraints, see http://support.microsoft.com/?scid=kb%3Ben-us%3B291539.
		/// </summary>
		public static string[] GetConstraintNames(
			SmartConnectionString connectionString )
		{
			using ( OleDbConnection conn =
				new OleDbConnection( connectionString.ConnectionString ) )
			{
				conn.Open();

				DataTable dt = conn.GetOleDbSchemaTable(
					OleDbSchemaGuid.Referential_Constraints,
					new Object[] 
					{
						null, 
						null, 
						null
					} );

				if ( dt == null || dt.Rows.Count <= 0 )
				{
					return null;
				}
				else
				{
					List<string> result = new List<string>();

					foreach ( DataRow row in dt.Rows )
					{
						string tn = row[@"CONSTRAINT_NAME"].ToString();
						result.Add( tn );
					}

					if ( result.Count <= 0 )
					{
						return null;
					}
					else
					{
						return result.ToArray();
					}
				}
			}
		}

		/// <summary>
		/// Check whether a table is contained. in the database schema.
		/// </summary>
		public static bool ContainsTable(
			string tableName )
		{
			return ContainsTable( ConnectionString, tableName );
		}

		/// <summary>
		/// Check whether a table is contained. in the database schema.
		/// </summary>
		public static bool ContainsTable(
			SmartConnectionString connectionString,
			string tableName )
		{
			string[] tableNames = GetTableNames( connectionString );
			if ( tableName == null || tableName.Length <= 0 )
			{
				return false;
			}
			else
			{
				tableName = tableName.Trim( '[', ']' );

				foreach ( string tableName_ in tableNames )
				{
					if ( string.Compare( tableName_, tableName, true ) == 0 )
					{
						return true;
					}
				}

				return false;
			}
		}

		/// <summary>
		/// Check whether a view is contained. in the database schema.
		/// </summary>
		public static bool ContainsView(
			string viewName )
		{
			return ContainsView( ConnectionString, viewName );
		}

		/// <summary>
		/// Check whether a view is contained. in the database schema.
		/// </summary>
		public static bool ContainsView(
			SmartConnectionString connectionString,
			string viewName )
		{
			string[] viewNames = GetViewNames( connectionString );
			if ( viewName == null || viewName.Length <= 0 )
			{
				return false;
			}
			else
			{
				viewName = viewName.Trim( '[', ']' );

				foreach ( string viewName_ in viewNames )
				{
					if ( string.Compare( viewName_, viewName, true ) == 0 )
					{
						return true;
					}
				}

				return false;
			}
		}

		/// <summary>
		/// Read schema information for a given database table.
		/// Is especially useful for checking the size of a varchar column against
		/// the size of a string the user is about to store.
		/// </summary>
		/// <param name="tableName">The name of the table.</param>
		/// <returns>Returns a collection of schema columns.</returns>
		/// <remarks>See also: http://google_de/search?q=cache:5AcY7whKMkgC:www.dotnet247.com/247reference/msgs/23/118660.aspx+ado.net+column+size. </remarks>
		public static DataColumnCollection GetTableSchema(
			string tableName )
		{
			return GetTableSchema( ConnectionString, tableName );
		}

		/// <summary>
		/// Read schema information for a given database table.
		/// Is especially useful for checking the size of a varchar column against
		/// the size of a string the user is about to store.
		/// </summary>
		/// <param name="tableName">The name of the table.</param>
		/// <param name="connectionString">The connection string used to connect.</param>
		/// <returns>Returns a collection of schema columns.</returns>
		/// <remarks>See also: http://google_de/search?q=cache:5AcY7whKMkgC:www.dotnet247.com/247reference/msgs/23/118660.aspx+ado.net+column+size. </remarks>
		public static DataColumnCollection GetTableSchema(
			SmartConnectionString connectionString,
			string tableName )
		{
			tableName = tableName.Trim( '[', ']' );

			using ( OleDbConnection conn = new OleDbConnection( connectionString.ConnectionString ) )
			{
				OleDbDataAdapter da = new OleDbDataAdapter(
					string.Format( @"SELECT TOP 1 * FROM [{0}]", tableName ), conn );
				da.MissingSchemaAction = MissingSchemaAction.AddWithKey;

				DataTable dt = new DataTable();
				da.Fill( dt );

				return dt.Columns;
			}
		}

		/// <summary>
		/// Check whether a table is contained.
		/// </summary>
		public static bool ContainsTableColumn(
			string tableName,
			string columnName )
		{
			return ContainsTableColumn(
				ConnectionString,
				tableName,
				columnName );
		}

		/// <summary>
		/// Check whether a table contains a given column.
		/// </summary>
		public static bool ContainsTableColumn(
			SmartConnectionString connectionString,
			string tableName,
			string columnName )
		{
			if ( ContainsTable( connectionString, tableName ) )
			{
				DataColumnCollection columns =
					GetTableSchema( connectionString, tableName );

				if ( columns == null )
				{
					return false;
				}
				else
				{
					columnName = columnName.Trim( '[', ']' );

					return columns.Contains( columnName );
				}
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="indexName"></param>
		/// <returns></returns>
		public static bool ContainsTableIndex(
			string tableName,
			string indexName )
		{
			return ContainsTableIndex(
				ConnectionString,
				tableName,
				indexName );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="tableName"></param>
		/// <param name="indexName"></param>
		/// <returns></returns>
		public static bool ContainsTableIndex(
			SmartConnectionString connectionString,
			string tableName,
			string indexName )
		{
			SchemaIndexInfo[] infos = GetIndexInfos( connectionString );

			if ( infos == null )
			{
				return false;
			}
			else
			{
				tableName = tableName.Trim( '[', ']' );
				indexName = indexName.Trim( '[', ']' );

				foreach ( SchemaIndexInfo info in infos )
				{
					if ( string.Compare( tableName, info.TableName, true ) == 0 &&
						string.Compare( indexName, info.IndexName, true ) == 0 )
					{
						return true;
					}
				}

				return false;
			}
		}

		/// <summary>
		/// Read all the index names for a given table of the current connection string.
		/// </summary>
		/// <returns>Returns a list of index names or NULL if none.</returns>
		public static SchemaIndexInfo[] GetIndexInfos()
		{
			return GetIndexInfos( ConnectionString );
		}

		/// <summary>
		/// Read all the index names for a given table of the given connection string.
		/// </summary>
		/// <param name="connectionString">The connection string used to connect.</param>
		/// <returns>Returns a list of index names or NULL if none.</returns>
		public static SchemaIndexInfo[] GetIndexInfos(
			SmartConnectionString connectionString )
		{
			using ( OleDbConnection conn =
				new OleDbConnection( connectionString.ConnectionString ) )
			{
				conn.Open();

				DataTable dt = conn.GetOleDbSchemaTable(
					OleDbSchemaGuid.Indexes,
					new Object[] 
					{
						null, 
						null, 
						null, 
						null, 
						null
					} );


				if ( dt == null || dt.Rows.Count <= 0 )
				{
					return null;
				}
				else
				{
					List<SchemaIndexInfo> result = new List<SchemaIndexInfo>();

					foreach ( DataRow row in dt.Rows )
					{
						string catalogName;
						string schemaName;
						string indexName;
						int indexType;
						string tableName;

						DBHelper.ReadField( out catalogName, row[@"TABLE_CATALOG"] );
						DBHelper.ReadField( out schemaName, row[@"TABLE_SCHEMA"] );
						DBHelper.ReadField( out indexName, row[@"INDEX_NAME"] );
						DBHelper.ReadField( out indexType, row[@"TYPE"] );
						DBHelper.ReadField( out tableName, row[@"TABLE_NAME"] );

						result.Add( new SchemaIndexInfo(
							catalogName,
							schemaName,
							indexName,
							(SchemaIndexType)indexType,
							tableName ) );
					}

					if ( result.Count <= 0 )
					{
						return null;
					}
					else
					{
						return result.ToArray();
					}
				}
			}
		}

		/// <summary>
		/// Read all the table names of the current connection string.
		/// </summary>
		/// <returns>Returns a list of table names or NULL if none.</returns>
		public static string[] GetTableNames()
		{
			return GetTableNames( ConnectionString );
		}

		/// <summary>
		/// Read all the table names of the given connection string.
		/// </summary>
		/// <param name="connectionString">The connection string used to connect.</param>
		/// <returns>Returns a list of table names or NULL if none.</returns>
		public static string[] GetTableNames(
			SmartConnectionString connectionString )
		{
			using ( OleDbConnection conn = new OleDbConnection( connectionString.ConnectionString ) )
			{
				conn.Open();

				DataTable dt = conn.GetOleDbSchemaTable(
					OleDbSchemaGuid.Tables,
					new Object[] 
					{
						null, 
						null, 
						null, 
						@"TABLE"
					} );


				if ( dt == null || dt.Rows.Count <= 0 )
				{
					return null;
				}
				else
				{
					List<string> result = new List<string>();

					foreach ( DataRow row in dt.Rows )
					{
						string tn = row[@"TABLE_NAME"].ToString();
						result.Add( tn );
					}

					if ( result.Count <= 0 )
					{
						return null;
					}
					else
					{
						return result.ToArray();
					}
				}
			}
		}

		/// <summary>
		/// Read all the view names of the current connection string.
		/// </summary>
		/// <returns>Returns a list of view names or NULL if none.</returns>
		public static string[] GetViewNames()
		{
			return GetViewNames( ConnectionString );
		}

		/// <summary>
		/// Read all the view names of the given connection string.
		/// </summary>
		/// <param name="connectionString">The connection string used to connect.</param>
		/// <returns>Returns a list of view names or NULL if none.</returns>
		public static string[] GetViewNames(
			SmartConnectionString connectionString )
		{
			using ( OleDbConnection conn = new OleDbConnection( connectionString.ConnectionString ) )
			{
				conn.Open();

				DataTable dt = conn.GetOleDbSchemaTable(
					OleDbSchemaGuid.Views,
					new Object[] 
					{
						null, 
						null, 
						null
					} );


				if ( dt == null || dt.Rows.Count <= 0 )
				{
					return null;
				}
				else
				{
					List<string> result = new List<string>();

					foreach ( DataRow row in dt.Rows )
					{
						string tn = row[@"TABLE_NAME"].ToString();
						result.Add( tn );
					}

					if ( result.Count <= 0 )
					{
						return null;
					}
					else
					{
						return result.ToArray();
					}
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Schema types.
		// ------------------------------------------------------------------

		/// <summary>
		/// Information about an index.
		/// </summary>
		[DebuggerDisplay( @"TableName = {tableName}, IndexName = {indexName}, IndexType = {indexType}" )]
		public struct SchemaIndexInfo
		{
			#region Public methods.

			/// <summary>
			/// Constructor.
			/// </summary>
			public SchemaIndexInfo(
				string catalogName,
				string schemaName,
				string indexName,
				SchemaIndexType indexType,
				string tableName )
			{
				this.catalogName = catalogName;
				this.schemaName = schemaName;
				this.indexName = indexName;
				this.indexType = indexType;
				this.tableName = tableName;
			}

			#endregion

			#region Public properties.

			/// <summary>
			/// 
			/// </summary>
			public string CatalogName
			{
				get
				{
					return catalogName;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public string SchemaName
			{
				get
				{
					return schemaName;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public string IndexName
			{
				get
				{
					return indexName;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public SchemaIndexType IndexType
			{
				get
				{
					return indexType;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public string TableName
			{
				get
				{
					return tableName;
				}
			}

			#endregion

			#region Private variables.

			private string catalogName;
			private string schemaName;
			private string indexName;
			private SchemaIndexType indexType;
			private string tableName;

			#endregion
		}

		/// <summary>
		/// The type of an index.
		/// </summary>
		public enum SchemaIndexType
		{
			#region Enum members.

			/// <summary>
			/// 
			/// </summary>
			BTree = 1,

			/// <summary>
			/// 
			/// </summary>
			Hash = 2,

			/// <summary>
			/// 
			/// </summary>
			Content = 3,

			/// <summary>
			/// 
			/// </summary>
			Other = 4

			#endregion
		}

		// ------------------------------------------------------------------
		#endregion

		#region Miscellaneous routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// If the configuration file specifies a command timeout,
		/// set it now.
		/// </summary>
		public static void CheckSetCommandTimeout(
			OleDbCommand command )
		{
			if ( WantSetCommandTimeout && command != null )
			{
				command.CommandTimeout = CommandTimeoutSeconds;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region JET routines (JRO) for Microsoft Access databases.
		// ------------------------------------------------------------------

		/// <summary>
		/// Get a full connection string from a given JET file path to
		/// an ".mdb" file.
		/// </summary>
		public static SmartConnectionString GetJetFilePathConnectionString(
			FileInfo databaseFilePath )
		{
			return new SmartConnectionString(
				string.Format(
				@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}",
				databaseFilePath.FullName ) );
		}

		/// <summary>
		/// Compacts the Microsoft Access database of the current connection string.
		/// Only call if you are sure that the connection string contains
		/// a connection to a Microsoft Access database.
		/// </summary>
		public static void CompactJetDatabase()
		{
			CompactJetDatabase( ConnectionString );
		}

		/// <summary>
		/// Compacts the Microsoft Access database of the current connection string.
		/// Only call if you are sure that the connection string contains
		/// a connection to a Microsoft Access database.
		/// </summary>
		public static void CompactJetDatabase(
			SmartConnectionString connectionString )
		{
			if ( File.Exists( connectionString.ConnectionString ) )
			{
				throw new Exception(
					string.Format(
					"You passed a file-name-only ('{0}') as the connection string to CompactJetDatabase(). Please use the FileInfo-overload instead.",
					connectionString ) );
			}
			else
			{
				// Extract the data source.
				Match m = Regex.Match(
					connectionString.ConnectionString,
					@"\bData source\s*=\s*(.+?)(;|$)",
					RegexOptions.IgnoreCase );

				if ( m != null && m.Success && m.Groups.Count > 1 )
				{
					string sourceFilePath = m.Groups[1].Value.Trim();
					CompactJetDatabase( new FileInfo( sourceFilePath ) );
				}
			}
		}

		/// <summary>
		/// Compacts a Microsoft Access database at a given path.
		/// </summary>
		/// <param name="sourceFilePath">The file path to the Microsoft Access
		/// database file (.MDB). The original database file will be overwritten
		/// with the compacted database file.</param>
		/// <remarks>Be sure that the database file to compact is closed and
		/// no other user is accessing it during compacting.</remarks>
		public static void CompactJetDatabase(
			FileInfo sourceFilePath )
		{
			if ( !sourceFilePath.Exists )
			{
				throw new ArgumentException(
					string.Format(
					"The specified sourceFilePath '{0}' path does not exists.",
					sourceFilePath.FullName
					),
					"sourceFilePath" );
			}
			else
			{
				string destinationFilePath = Path.Combine(
					sourceFilePath.DirectoryName,
					Guid.NewGuid().ToString( @"N" ) +
					sourceFilePath.Extension );

				try
				{
					// Convert to (temporary) destination.
					CompactJetDatabase(
						sourceFilePath,
						new FileInfo( destinationFilePath ) );

					// And copy back to sourceConnectionString, 
					// overwriting the sourceConnectionString.
					File.Copy(
						destinationFilePath,
						sourceFilePath.FullName,
						true );
				}
				finally
				{
					File.Delete( destinationFilePath );
				}
			}
		}

		/// <summary>
		/// Compacts a Microsoft Access database at a given path and stores
		/// the newly compacted database at a new location.
		/// </summary>
		/// <param name="sourceFilePath">The file path to the Microsoft Access
		/// database file (.MDB). The file will not be modified.</param>
		/// <param name="destinationFilePath">The file path of the new database file 
		/// that will be created.</param>
		/// <remarks>Be sure that the database file to compact is closed and
		/// no other user is accessing it during compacting.</remarks>
		public static void CompactJetDatabase(
			FileInfo sourceFilePath,
			FileInfo destinationFilePath )
		{
			// see also Q306287:
			// "HOW TO: Compact a Microsoft Access Database by Using Visual Basic .NET".
			// http://support.microsoft.com/kb/306287/EN-US/.

			if ( !sourceFilePath.Exists )
			{
				throw new ArgumentException(
					string.Format(
					"The specified sourceFilePath '{0}' path does not exists.",
					sourceFilePath.FullName
					),
					@"sourceFilePath" );
			}
			else
			{
				// Build connection strings, as expected for the call.
				SmartConnectionString sourceJetConnectionString =
					GetJetFilePathConnectionString(
					sourceFilePath );

				SmartConnectionString destinationJetConnectionString =
					GetJetFilePathConnectionString(
					destinationFilePath );

				// Actually call.
				JRO.JetEngine jet = new JRO.JetEngineClass();
				jet.CompactDatabase(
					sourceJetConnectionString.ConnectionString,
					destinationJetConnectionString.ConnectionString );
			}
		}

		/// <summary>
		/// JET has a 3 second cache by default. This function flushes
		/// the cache, forcing immediate update.
		/// </summary>
		public static void RefreshJetCache()
		{
			RefreshJetCache( ConnectionString );
		}

		/// <summary>
		/// JET has a 3 second cache by default. This function flushes
		/// the cache, forcing immediate update.
		/// </summary>
		/// <param name="connectionString">The complete connection string.</param>
		public static void RefreshJetCache(
			SmartConnectionString connectionString )
		{
			if ( File.Exists( connectionString.ConnectionString ) )
			{
				throw new Exception(
					string.Format(
					"You passed a file-name-only ('{0}') as the connection string to RefreshJetCache(). Please use the FileInfo-overload instead.",
					connectionString ) );
			}
			else
			{
				// Extract the data source.
				Match m = Regex.Match(
					connectionString.ConnectionString,
					@"\bData source\s*=\s*(.+?)(;|$)",
					RegexOptions.IgnoreCase );

				if ( m != null && m.Success && m.Groups.Count > 1 )
				{
					string sourceFilePath = m.Groups[1].Value.Trim();
					RefreshJetCache( new FileInfo( sourceFilePath ) );
				}
			}
		}

		/// <summary>
		/// JET has a 3 second cache by default. This function flushes
		/// the cache, forcing immediate update.
		/// </summary>
		public static void RefreshJetCache(
			FileInfo filePath )
		{
			SmartConnectionString jetConnectionString =
				GetJetFilePathConnectionString(
				filePath );

			ADODB.Connection connection = new ADODB.ConnectionClass();
			try
			{
				connection.Open(
					jetConnectionString.ConnectionString,
					string.Empty,
					string.Empty,
					0 );

				JRO.JetEngine jet = new JRO.JetEngineClass();
				jet.RefreshCache( connection );
			}
			finally
			{
				connection.Close();
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Identity routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Get the identity field value.
		/// </summary>
		/// <param name="tableName">The table name, as a hint to the retrieval function.</param>
		/// <returns>Returns the identity, if retrievable, ob 0 if none.</returns>
		public static int GetIdentity(
			string tableName )
		{
			return GetIdentity(
				ConnectionString,
				tableName );
		}

		/// <summary>
		/// Get the identity field value.
		/// </summary>
		/// <param name="connectionString">The connection string being used to 
		/// query for the identity value.</param>
		/// <param name="tableName">The table name, as a hint to the retrieval function.</param>
		/// <returns>Returns the identity, if retrievable, ob 0 if none.</returns>
		public static int GetIdentity(
			SmartConnectionString connectionString,
			string tableName )
		{
			using ( OleDbConnection conn = new OleDbConnection( connectionString.ConnectionString ) )
			{
				return GetIdentity( conn, tableName );
			}
		}

		/// <summary>
		/// Get the identity field value.
		/// </summary>
		/// <param name="conn">The connection used to query
		/// for the identity value.</param>
		/// <param name="tableName">The table name, as a hint to the retrieval function.</param>
		/// <returns>Returns the identity, if retrievable, ob 0 if none.</returns>
		public static int GetIdentity(
			OleDbConnection conn,
			string tableName )
		{
			if ( conn.State == ConnectionState.Closed )
			{
				conn.Open();
			}

			// --

			DatabaseType type =
				DetectDatabaseTypeFromConnectionString(
				new SmartConnectionString( conn.ConnectionString ) );

			string sql;

			switch ( type )
			{
				case AdoNetOleDBHelper.DatabaseType.SqlServer:
					sql = string.Format(
						@"SELECT IDENT_CURRENT('{0}')",
						tableName );
					break;

				case AdoNetOleDBHelper.DatabaseType.Access:
					sql = string.Format(
						@"SELECT @@IDENTITY" );
					break;

				default:
					throw new ArgumentException(
						string.Format(
						"Unknown DatabaseType: '{0}'.",
						type ) );
			}

			// --

			int result = 0;

			TraceSql( sql, true, null );
			try
			{
				OleDbCommand cmd = new OleDbCommand( sql, conn );

				// Apply command timeouts, if any.
				CheckSetCommandTimeout( cmd );

				object o = cmd.ExecuteScalar();

				if ( o != null && o != DBNull.Value )
				{
					result = Convert.ToInt32( o );
				}
			}
			catch ( Exception x )
			{
				TraceSqlError( sql, x );
				throw;
			}

			TraceSql( sql, false, null );
			return result;
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Because OleDbParameterCollection cannot be instantiated,
	/// this class mimics the required operations of OleDbParameterCollection.
	/// </summary>
	public class AdoNetOleDBParamCollection :
		AdoNetBaseParamCollection<OleDbParameter>
	{
		#region Public constructors.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public AdoNetOleDBParamCollection()
			:
			base()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public AdoNetOleDBParamCollection(
			params OleDbParameter[] parameters )
			:
			base( parameters )
		{
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}