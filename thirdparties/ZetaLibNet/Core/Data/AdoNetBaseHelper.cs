namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections;
	using System.Data;
	using System.Data.SqlClient;
	using System.Data.OleDb;
	using System.Data.Odbc;
	using System.Data.OracleClient;
	using System.Diagnostics;
	using System.Text;
	using System.Text.RegularExpressions;
	using ZetaLib.Core.Common;
	using System.Data.Common;
	using System.Collections.Generic;
	using ZetaLib.Core.Logging;
	using ZetaLib.Core.Properties;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Base class for helper classes.
	/// </summary>
	public abstract class AdoNetBaseHelper<
		TCommand,
		TCommandBuilder,
		TConnection,
		TDataAdapter,
		TParameter,
		TParamCollection>
		where TCommand : DbCommand, new()
		where TCommandBuilder : DbCommandBuilder, new()
		where TConnection : DbConnection, new()
		where TDataAdapter : DbDataAdapter, new()
		where TParameter : DbParameter, new()
		where TParamCollection : AdoNetBaseParamCollection<TParameter>
	{
		#region Executes for DataSets (direct SQL commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			string sql,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return Execute(
				ConnectionString,
				sql,
				cacheItemInfo );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			string sql )
		{
			return Execute( ConnectionString, sql );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			string sql,
			int startIndex,
			int maxCount,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return Execute(
				ConnectionString,
				sql,
				startIndex,
				maxCount,
				cacheItemInfo );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
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
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			string sql,
			int startIndex,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return Execute(
				ConnectionString,
				sql,
				startIndex,
				cacheItemInfo );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			string sql,
			int startIndex )
		{
			return Execute(
				ConnectionString,
				sql,
				startIndex,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			SmartConnectionString connectionString,
			string sql,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return Execute( connectionString, sql, -1, -1, cacheItemInfo );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			SmartConnectionString connectionString,
			string sql )
		{
			return Execute(
				connectionString,
				sql,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			SmartConnectionString connectionString,
			string sql,
			int startIndex,
			int maxCount,
			AdoNetCacheItemInformation cacheItemInfo )
		{
#if CACHESQL
			// Cache?
			DataSet cacheDataSet;
			if ( AdoNetCacheManager.Current == null )
			{
				cacheDataSet = null;
			}
			else
			{
				cacheDataSet = AdoNetCacheManager.Current.GetDataSetFromCache(
					connectionString,
					sql,
					startIndex,
					maxCount,
					cacheItemInfo );
			}

			if ( cacheDataSet != null )
			{
				return cacheDataSet;
			}
			else
#endif
			{
				DataSet ds = new DataSet();

				using ( TConnection conn = new TConnection() )
				{
					conn.ConnectionString = connectionString.ConnectionString;
					conn.Open();

					sql = CSManager.CheckGet(
						connectionString ).ReplaceQueryBoolean( sql );

					TCommand selectCommand = new TCommand();
					selectCommand.CommandText = sql;
					selectCommand.Connection = conn;

					TDataAdapter da = new TDataAdapter();
					da.SelectCommand = selectCommand;

					// Apply command timeouts, if any.
					CheckSetCommandTimeout( da.DeleteCommand );
					CheckSetCommandTimeout( da.InsertCommand );
					CheckSetCommandTimeout( da.SelectCommand );
					CheckSetCommandTimeout( da.UpdateCommand );

					TraceSql( sql, true, ds );
					try
					{
						/*string tableName = DetectTableName( sql );*/

						if ( startIndex == -1 && maxCount == -1 )
						{
							da.Fill(
								ds/*,
								tableName*/
											);
						}
						else
						{
							da.Fill(
								ds,
								startIndex,
								maxCount,
								@"Table1"/*,
								tableName*/
											);
						}
					}
					catch ( Exception x )
					{
						TraceSqlError( sql, x );
						throw;
					}
					TraceSql( sql, false, ds );

#if CACHESQL
					// Cache!(?)
					if ( AdoNetCacheManager.Current != null )
					{
						AdoNetCacheManager cacheManager =
							AdoNetCacheManager.Current;

						cacheManager.PutDataSetToCache(
							ds,
							connectionString,
							sql,
							startIndex,
							maxCount,
							cacheItemInfo );
					}
#endif
				}

				return ds;
			}
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
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
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			SmartConnectionString connectionString,
			string sql,
			int startIndex,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return Execute(
				connectionString,
				sql,
				startIndex,
				int.MaxValue,
				cacheItemInfo );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes for DataSets (stored procedure commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			string spName,
			TParamCollection spParameters,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return Execute(
				ConnectionString,
				spName,
				spParameters,
				cacheItemInfo );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			string spName,
			TParamCollection spParameters )
		{
			return Execute(
				ConnectionString,
				spName,
				spParameters );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			string spName,
			TParamCollection spParameters,
			int startIndex,
			int maxCount,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return Execute(
				ConnectionString,
				spName,
				spParameters,
				startIndex,
				maxCount,
				cacheItemInfo );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			string spName,
			TParamCollection spParameters,
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
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			string spName,
			TParamCollection spParameters,
			int startIndex,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return Execute(
				ConnectionString,
				spName,
				spParameters,
				startIndex,
				cacheItemInfo );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="startIndex">The start index.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			string spName,
			TParamCollection spParameters,
			int startIndex )
		{
			return Execute(
				ConnectionString,
				spName,
				spParameters,
				startIndex,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			SmartConnectionString connectionString,
			string spName,
			TParamCollection spParameters,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return Execute(
				connectionString,
				spName,
				spParameters,
				-1,
				-1,
				cacheItemInfo );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			SmartConnectionString connectionString,
			string spName,
			TParamCollection spParameters )
		{
			return Execute(
				connectionString,
				spName,
				spParameters,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			SmartConnectionString connectionString,
			string spName,
			TParamCollection spParameters,
			int startIndex,
			int maxCount )
		{
			return Execute(
				connectionString,
				spName,
				spParameters,
				startIndex,
				maxCount,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			SmartConnectionString connectionString,
			string spName,
			TParamCollection spParameters,
			int startIndex,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return Execute(
				connectionString,
				spName,
				spParameters,
				startIndex,
				int.MaxValue,
				cacheItemInfo );
		}

		/// <summary>
		/// Query for a DataSet.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataSet or NULL if no data is found.
		/// </returns>
		public DataSet Execute(
			SmartConnectionString connectionString,
			string spName,
			TParamCollection spParameters,
			int startIndex,
			int maxCount,
			AdoNetCacheItemInformation cacheItemInfo )
		{
#if CACHESQL
			// Cache?
			DataSet cacheDataSet;
			if ( AdoNetCacheManager.Current == null )
			{
				cacheDataSet = null;
			}
			else
			{
				cacheDataSet = AdoNetCacheManager.Current.GetDataSetFromCache(
					connectionString,
					spName,
					spParameters,
					startIndex,
					maxCount,
					cacheItemInfo );
			}

			if ( cacheDataSet != null )
			{
				return cacheDataSet;
			}
			else
#endif
			{
				DataSet ds = new DataSet();

				using ( TConnection conn = new TConnection() )
				{
					conn.ConnectionString = connectionString.ConnectionString;
					conn.Open();

					spName = CSManager.CheckGet(
						connectionString ).ReplaceQueryBoolean( spName );

					TCommand cmd = new TCommand();
					cmd.CommandText = spName;
					cmd.Connection = conn;
					cmd.CommandType = DetectCommandType( spName );

					// Apply command timeouts, if any.
					CheckSetCommandTimeout( cmd );

					if ( spParameters != null )
					{
						foreach ( TParameter parameter in spParameters )
						{
							cmd.Parameters.Add( parameter );
						}
					}

					TDataAdapter da = new TDataAdapter();
					da.SelectCommand = cmd;

					// Apply command timeouts, if any.
					CheckSetCommandTimeout( da.DeleteCommand );
					CheckSetCommandTimeout( da.InsertCommand );
					CheckSetCommandTimeout( da.SelectCommand );
					CheckSetCommandTimeout( da.UpdateCommand );

					TraceSql( cmd, true, ds );
					try
					{
						/*
						string tableName =
							QuickCreatePseudoSqlFromSP(
							cmd.CommandText,
							cmd.Parameters );
						*/
						string tableName = @"Table1";

						if ( startIndex == -1 && maxCount == -1 )
						{
							da.Fill(
								ds,
								tableName );
						}
						else
						{
							da.Fill(
								ds,
								startIndex,
								maxCount,
								tableName );
						}
					}
					catch ( Exception x )
					{
						TraceSqlError( cmd, x );
						throw;
					}
					TraceSql( cmd, false, ds );

#if CACHESQL
					// Cache!(?)
					if ( AdoNetCacheManager.Current != null )
					{
						AdoNetCacheManager cacheManager =
							AdoNetCacheManager.Current;

						cacheManager.PutDataSetToCache(
							ds,
							connectionString,
							spName,
							spParameters,
							startIndex,
							maxCount,
							cacheItemInfo );
					}
#endif
				}

				return ds;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes for DataTables (direct SQL commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			string sql,
			AdoNetCacheItemInformation
			cacheItemInfo )
		{
			return ExecuteTable(
				ConnectionString,
				sql,
				cacheItemInfo );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			string sql )
		{
			return ExecuteTable(
				ConnectionString,
				sql,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			string sql,
			int startIndex,
			int maxCount,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return ExecuteTable(
				ConnectionString,
				sql,
				startIndex,
				maxCount,
				cacheItemInfo );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			string sql,
			int startIndex,
			int maxCount )
		{
			return ExecuteTable(
				ConnectionString,
				sql,
				startIndex,
				maxCount,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			string sql,
			int startIndex,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return ExecuteTable(
				ConnectionString,
				sql,
				startIndex,
				cacheItemInfo );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			string sql,
			int startIndex )
		{
			return ExecuteTable(
				ConnectionString,
				sql,
				startIndex,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string sql,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			DataSet ds = Execute( connectionString, sql, cacheItemInfo );

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
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string sql )
		{
			return ExecuteTable(
				connectionString,
				sql,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string sql,
			int startIndex,
			int maxCount,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			DataSet ds = Execute(
				connectionString,
				sql,
				startIndex,
				maxCount,
				cacheItemInfo );

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
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
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
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string sql,
			int startIndex,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return ExecuteTable(
				connectionString,
				sql,
				startIndex,
				int.MaxValue,
				cacheItemInfo );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes for DataTables (stored procedure commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			string spName,
			TParamCollection spParameters,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return ExecuteTable(
				ConnectionString,
				spName,
				spParameters,
				cacheItemInfo );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			string spName,
			TParamCollection spParameters )
		{
			return ExecuteTable(
				ConnectionString,
				spName,
				spParameters,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			string spName,
			TParamCollection spParameters,
			int startIndex,
			int maxCount,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return ExecuteTable(
				ConnectionString,
				spName,
				spParameters,
				startIndex,
				maxCount,
				cacheItemInfo );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			string spName,
			TParamCollection spParameters,
			int startIndex,
			int maxCount )
		{
			return ExecuteTable(
				ConnectionString,
				spName,
				spParameters,
				startIndex,
				maxCount,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			string spName,
			TParamCollection spParameters,
			int startIndex,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return ExecuteTable(
				ConnectionString,
				spName,
				spParameters,
				startIndex,
				cacheItemInfo );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="startIndex">The start index.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			string spName,
			TParamCollection spParameters,
			int startIndex )
		{
			return ExecuteTable(
				ConnectionString,
				spName,
				spParameters,
				startIndex,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string spName,
			TParamCollection spParameters,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			DataSet ds = Execute(
				connectionString,
				spName,
				spParameters,
				cacheItemInfo );

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
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string spName,
			TParamCollection spParameters )
		{
			return ExecuteTable(
				connectionString,
				spName,
				spParameters,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string spName,
			TParamCollection spParameters,
			int startIndex,
			int maxCount,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			DataSet ds = Execute(
				connectionString,
				spName,
				spParameters,
				startIndex,
				maxCount,
				cacheItemInfo );

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
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="maxCount">The max count.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string spName,
			TParamCollection spParameters,
			int startIndex,
			int maxCount )
		{
			return ExecuteTable(
				connectionString,
				spName,
				spParameters,
				startIndex,
				maxCount,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Query for a DataTable.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataTable or NULL if no data is found.
		/// </returns>
		public DataTable ExecuteTable(
			SmartConnectionString connectionString,
			string spName,
			TParamCollection spParameters,
			int startIndex,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return ExecuteTable(
				connectionString,
				spName,
				spParameters,
				startIndex,
				int.MaxValue,
				cacheItemInfo );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes for DataRows (direct SQL commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for a DataRow.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataRow or NULL if no data is found.
		/// </returns>
		public DataRow ExecuteRow(
			string sql,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return ExecuteRow(
				ConnectionString,
				sql,
				cacheItemInfo );
		}

		/// <summary>
		/// Query for a DataRow.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <returns>
		/// Returns the DataRow or NULL if no data is found.
		/// </returns>
		public DataRow ExecuteRow(
			string sql )
		{
			return ExecuteRow(
				ConnectionString,
				sql );
		}

		/// <summary>
		/// Query for a DataRow.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataRow or NULL if no data is found.
		/// </returns>
		public DataRow ExecuteRow(
			SmartConnectionString connectionString,
			string sql,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			DataTable dt =
				ExecuteTable(
				connectionString,
				sql,
				0,
				1,
				cacheItemInfo );

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
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <returns>
		/// Returns the DataRow or NULL if no data is found.
		/// </returns>
		public DataRow ExecuteRow(
			SmartConnectionString connectionString,
			string sql )
		{
			return ExecuteRow(
				connectionString,
				sql,
				null as AdoNetCacheItemInformation );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes for DataRows (stored procedure commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for a DataRow.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataRow or NULL if no data is found.
		/// </returns>
		public DataRow ExecuteRow(
			string spName,
			TParamCollection spParameters,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return ExecuteRow(
				ConnectionString,
				spName,
				spParameters,
				cacheItemInfo );
		}

		/// <summary>
		/// Query for a DataRow.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <returns>
		/// Returns the DataRow or NULL if no data is found.
		/// </returns>
		public DataRow ExecuteRow(
			string spName,
			TParamCollection spParameters )
		{
			return ExecuteRow(
				ConnectionString,
				spName,
				spParameters );
		}

		/// <summary>
		/// Query for a DataRow.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the DataRow or NULL if no data is found.
		/// </returns>
		public DataRow ExecuteRow(
			SmartConnectionString connectionString,
			string spName,
			TParamCollection spParameters,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			DataTable dt = ExecuteTable(
				connectionString,
				spName,
				spParameters,
				0,
				1,
				cacheItemInfo );

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
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <returns>
		/// Returns the DataRow or NULL if no data is found.
		/// </returns>
		public DataRow ExecuteRow(
			SmartConnectionString connectionString,
			string spName,
			TParamCollection spParameters )
		{
			return ExecuteRow(
				connectionString,
				spName,
				spParameters,
				null as AdoNetCacheItemInformation );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes for values (direct SQL commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for a single value.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the value or NULL if no data is found.
		/// </returns>
		public object ExecuteValue(
			SmartConnectionString connectionString,
			string sql,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			DataRow row = ExecuteRow( connectionString, sql, cacheItemInfo );

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
		/// <param name="sql">The SQL.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the value or NULL if no data is found.
		/// </returns>
		public object ExecuteValue(
			string sql,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return ExecuteValue(
				ConnectionString,
				sql,
				cacheItemInfo );
		}

		/// <summary>
		/// Query for a single value.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <returns>
		/// Returns the value or NULL if no data is found.
		/// </returns>
		public object ExecuteValue(
			string sql )
		{
			return ExecuteValue(
				ConnectionString,
				sql );
		}

		/// <summary>
		/// Query for a single value.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <returns>
		/// Returns the value or NULL if no data is found.
		/// </returns>
		public object ExecuteValue(
			SmartConnectionString connectionString,
			string sql )
		{
			return ExecuteValue(
				connectionString,
				sql,
				null as AdoNetCacheItemInformation );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes for values (stored procedure commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for a single value.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the value or NULL if no data is found.
		/// </returns>
		public object ExecuteValue(
			SmartConnectionString connectionString,
			string spName,
			TParamCollection spParameters,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			DataRow row = ExecuteRow(
				connectionString,
				spName,
				spParameters,
				cacheItemInfo );

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
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		/// <returns>
		/// Returns the value or NULL if no data is found.
		/// </returns>
		public object ExecuteValue(
			string spName,
			TParamCollection spParameters,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return ExecuteValue(
				ConnectionString,
				spName,
				spParameters,
				cacheItemInfo );
		}

		/// <summary>
		/// Query for a single value.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <returns>
		/// Returns the value or NULL if no data is found.
		/// </returns>
		public object ExecuteValue(
			string spName,
			TParamCollection spParameters )
		{
			return ExecuteValue(
				ConnectionString,
				spName,
				spParameters );
		}

		/// <summary>
		/// Query for a single value.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <returns>
		/// Returns the value or NULL if no data is found.
		/// </returns>
		public object ExecuteValue(
			SmartConnectionString connectionString,
			string spName,
			TParamCollection spParameters )
		{
			return ExecuteValue(
				connectionString,
				spName,
				spParameters,
				null as AdoNetCacheItemInformation );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes without return value (direct SQL commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Execute a query without returning a value.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public void ExecuteNonQuery(
			string sql,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			ExecuteNonQuery(
				ConnectionString,
				sql,
				cacheItemInfo );
		}

		/// <summary>
		/// Execute a query without returning a value.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		public void ExecuteNonQuery(
			string sql )
		{
			ExecuteNonQuery(
				ConnectionString,
				sql,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Execute a query without returning a value.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		public void ExecuteNonQuery(
			SmartConnectionString connectionString,
			string sql )
		{
			ExecuteNonQuery(
				connectionString,
				sql,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Execute a query without returning a value.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public void ExecuteNonQuery(
			SmartConnectionString connectionString,
			string sql,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			using ( TConnection conn = new TConnection() )
			{
				conn.ConnectionString = connectionString.ConnectionString;
				conn.Open();

				sql = CSManager.CheckGet(
					connectionString ).ReplaceQueryBoolean( sql );

				TCommand cmd = new TCommand();
				cmd.CommandText = sql;
				cmd.Connection = conn;

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

#if CACHESQL
				// Cache.
				if ( IsModifyingQuery( sql ) )
				{
					if ( AdoNetCacheManager.Current != null )
					{
						AdoNetCacheManager.Current.RemoveAll( cacheItemInfo );
					}
				}
#endif
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes without return value (stored procedure commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Execute a query without returning a value.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public void ExecuteNonQuery(
			string spName,
			TParamCollection spParameters,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			ExecuteNonQuery(
				ConnectionString,
				spName,
				spParameters,
				cacheItemInfo );
		}

		/// <summary>
		/// Execute a query without returning a value.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		public void ExecuteNonQuery(
			string spName,
			TParamCollection spParameters )
		{
			ExecuteNonQuery(
				ConnectionString,
				spName,
				spParameters,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Execute a query without returning a value.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		public void ExecuteNonQuery(
			SmartConnectionString connectionString,
			string spName,
			TParamCollection spParameters )
		{
			ExecuteNonQuery(
				connectionString,
				spName,
				spParameters,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Execute a query without returning a value.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public void ExecuteNonQuery(
			SmartConnectionString connectionString,
			string spName,
			TParamCollection spParameters,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			using ( TConnection conn = new TConnection() )
			{
				conn.ConnectionString = connectionString.ConnectionString;
				conn.Open();

				spName = CSManager.CheckGet(
					connectionString ).ReplaceQueryBoolean( spName );

				TCommand cmd = new TCommand();
				cmd.CommandText = spName;
				cmd.Connection = conn;
				cmd.CommandType = DetectCommandType( spName );

				// Apply command timeouts, if any.
				CheckSetCommandTimeout( cmd );

				if ( spParameters != null )
				{
					foreach ( TParameter parameter in spParameters )
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
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes for updateable DataAdapters (direct SQL commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for an updatable DataAdapter.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <returns></returns>
		public TDataAdapter ExecuteForUpdate(
			string sql )
		{
			return ExecuteForUpdate(
				ConnectionString,
				sql );
		}

		/// <summary>
		/// Query for an updatable DataAdapter.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <returns></returns>
		public TDataAdapter ExecuteForUpdate(
			SmartConnectionString connectionString,
			string sql )
		{
			TConnection conn = new TConnection();
			conn.ConnectionString = connectionString.ConnectionString;

			sql = CSManager.CheckGet(
				connectionString ).ReplaceQueryBoolean( sql );

			TCommand selectCommand = new TCommand();
			selectCommand.CommandText = sql;
			selectCommand.Connection = conn;

			TDataAdapter da = new TDataAdapter();
			da.SelectCommand = selectCommand;

			// Apply command timeouts, if any.
			CheckSetCommandTimeout( da.DeleteCommand );
			CheckSetCommandTimeout( da.InsertCommand );
			CheckSetCommandTimeout( da.SelectCommand );
			CheckSetCommandTimeout( da.UpdateCommand );

			TCommandBuilder cb = new TCommandBuilder();
			cb.DataAdapter = da;

			if ( !string.IsNullOrEmpty( QuotePrefix ) )
			{
				cb.QuotePrefix = QuotePrefix;
			}
			if ( !string.IsNullOrEmpty( QuoteSuffix ) )
			{
				cb.QuoteSuffix = QuoteSuffix;
			}

			return da;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Executes for updateable DataAdapters (stored procedure commands).
		// ------------------------------------------------------------------

		/// <summary>
		/// Query for an updatable DataAdapter.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <returns></returns>
		public TDataAdapter ExecuteForUpdate(
			string spName,
			TParamCollection spParameters )
		{
			return ExecuteForUpdate(
				ConnectionString,
				spName,
				spParameters );
		}

		/// <summary>
		/// Query for an updatable DataAdapter.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <returns></returns>
		public TDataAdapter ExecuteForUpdate(
			SmartConnectionString connectionString,
			string spName,
			TParamCollection spParameters )
		{
			TConnection conn = new TConnection();
			conn.ConnectionString = connectionString.ConnectionString;

			spName = CSManager.CheckGet(
				connectionString ).ReplaceQueryBoolean( spName );

			TCommand cmd = new TCommand();
			cmd.CommandText = spName;
			cmd.Connection = conn;

			// Apply command timeouts, if any.
			CheckSetCommandTimeout( cmd );

			cmd.CommandType = DetectCommandType( spName );

			if ( spParameters != null )
			{
				foreach ( TParameter parameter in spParameters )
				{
					cmd.Parameters.Add( parameter );
				}
			}

			conn.Open();

			TDataAdapter da = new TDataAdapter();
			da.SelectCommand = cmd;

			// Apply command timeouts, if any.
			CheckSetCommandTimeout( da.DeleteCommand );
			CheckSetCommandTimeout( da.InsertCommand );
			CheckSetCommandTimeout( da.SelectCommand );
			CheckSetCommandTimeout( da.UpdateCommand );

			TCommandBuilder cb = new TCommandBuilder();
			cb.DataAdapter = da;

			if ( !string.IsNullOrEmpty( QuotePrefix ) )
			{
				cb.QuotePrefix = QuotePrefix;
			}
			if ( !string.IsNullOrEmpty( QuoteSuffix ) )
			{
				cb.QuoteSuffix = QuoteSuffix;
			}

			return da;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Identity routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Get the identity field value.
		/// </summary>
		/// <param name="tableName">The table name, as a hint to the retrieval function.</param>
		/// <returns>
		/// Returns the identity, if retrievable, ob 0 if none.
		/// </returns>
		public int GetIdentity(
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
		/// <returns>
		/// Returns the identity, if retrievable, ob 0 if none.
		/// </returns>
		public int GetIdentity(
			SmartConnectionString connectionString,
			string tableName )
		{
			using ( TConnection conn = new TConnection() )
			{
				conn.ConnectionString = connectionString.ConnectionString;
				return GetIdentity( conn, tableName );
			}
		}

		/// <summary>
		/// Get the identity field value.
		/// </summary>
		/// <param name="conn">The connection used to query
		/// for the identity value.</param>
		/// <param name="tableName">The table name, as a hint to the retrieval function.</param>
		/// <returns>
		/// Returns the identity, if retrievable, ob 0 if none.
		/// </returns>
		public int GetIdentity(
			TConnection conn,
			string tableName )
		{
			if ( conn.State == ConnectionState.Closed )
			{
				conn.Open();
			}

			// --

			string sql = GetIdentitySelectSql( conn, tableName );
			int result = 0;

			TraceSql( sql, true, null );
			try
			{
				sql = CSManager.CheckGet(
					connectionString ).ReplaceQueryBoolean( sql );

				TCommand cmd = new TCommand();
				cmd.CommandText = sql;
				cmd.Connection = conn;

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
			/// <param name="catalogName">Name of the catalog.</param>
			/// <param name="schemaName">Name of the schema.</param>
			/// <param name="indexName">Name of the index.</param>
			/// <param name="indexType">Type of the index.</param>
			/// <param name="tableName">Name of the table.</param>
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
			/// Gets the name of the catalog.
			/// </summary>
			/// <value>The name of the catalog.</value>
			public string CatalogName
			{
				get
				{
					return catalogName;
				}
			}

			/// <summary>
			/// Gets the name of the schema.
			/// </summary>
			/// <value>The name of the schema.</value>
			public string SchemaName
			{
				get
				{
					return schemaName;
				}
			}

			/// <summary>
			/// Gets the name of the index.
			/// </summary>
			/// <value>The name of the index.</value>
			public string IndexName
			{
				get
				{
					return indexName;
				}
			}

			/// <summary>
			/// Gets the type of the index.
			/// </summary>
			/// <value>The type of the index.</value>
			public SchemaIndexType IndexType
			{
				get
				{
					return indexType;
				}
			}

			/// <summary>
			/// Gets the name of the table.
			/// </summary>
			/// <value>The name of the table.</value>
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
			/// FxCop.
			/// </summary>
			None = 0,

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

		/// <summary>
		/// Information about a contraint.
		/// </summary>
		[DebuggerDisplay( @"ConstraintName = {constraintName}, TableName = {tableName}, ConstraintType = {constraintType}" )]
		public struct SchemaConstraintInfo
		{
			#region Public methods.

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="constraintCatalogName">Name of the constraint catalog.</param>
			/// <param name="constraintSchemaName">Name of the constraint schema.</param>
			/// <param name="constraintName">Name of the constraint.</param>
			/// <param name="constraintType">Type of the constraint.</param>
			/// <param name="tableCatalogName">Name of the table catalog.</param>
			/// <param name="tableSchemaName">Name of the table schema.</param>
			/// <param name="tableName">Name of the table.</param>
			public SchemaConstraintInfo(
				string constraintCatalogName,
				string constraintSchemaName,
				string constraintName,
				string constraintType,
				string tableCatalogName,
				string tableSchemaName,
				string tableName )
			{
				this.constraintCatalogName = constraintCatalogName;
				this.constraintSchemaName = constraintSchemaName;
				this.constraintName = constraintName;
				this.tableCatalogName = tableCatalogName;
				this.tableSchemaName = tableSchemaName;
				this.tableName = tableName;

				switch ( constraintType.ToLower() )
				{
					case @"check":
						this.constraintType = SchemaConstraintType.TableConstraintCheck;
						break;
					case @"foreign key":
						this.constraintType = SchemaConstraintType.TableConstraintForeignKey;
						break;
					case @"primary key":
						this.constraintType = SchemaConstraintType.TableConstraintPrimaryKey;
						break;
					case @"unique":
						this.constraintType = SchemaConstraintType.TableConstraintUnique;
						break;

					default:
						this.constraintType = SchemaConstraintType.Unknown;
						break;
				}
			}

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="constraintCatalogName">Name of the constraint catalog.</param>
			/// <param name="constraintSchemaName">Name of the constraint schema.</param>
			/// <param name="constraintName">Name of the constraint.</param>
			public SchemaConstraintInfo(
				string constraintCatalogName,
				string constraintSchemaName,
				string constraintName )
			{
				this.constraintCatalogName = constraintCatalogName;
				this.constraintSchemaName = constraintSchemaName;
				this.constraintName = constraintName;
				this.constraintType = SchemaConstraintType.ReferentialConstraint;
				this.tableCatalogName = null;
				this.tableSchemaName = null;
				this.tableName = null;
			}

			#endregion

			#region Public properties.

			/// <summary>
			/// Gets the name of the constraint catalog.
			/// </summary>
			/// <value>The name of the constraint catalog.</value>
			public string ConstraintCatalogName
			{
				get
				{
					return constraintCatalogName;
				}
			}

			/// <summary>
			/// Gets the name of the constraint schema.
			/// </summary>
			/// <value>The name of the constraint schema.</value>
			public string ConstraintSchemaName
			{
				get
				{
					return constraintSchemaName;
				}
			}

			/// <summary>
			/// Gets the name of the constraint.
			/// </summary>
			/// <value>The name of the constraint.</value>
			public string ConstraintName
			{
				get
				{
					return constraintName;
				}
			}

			/// <summary>
			/// Gets the type of the constraint.
			/// </summary>
			/// <value>The type of the constraint.</value>
			public SchemaConstraintType ConstraintType
			{
				get
				{
					return constraintType;
				}
			}

			/// <summary>
			/// Gets the name of the table catalog.
			/// </summary>
			/// <value>The name of the table catalog.</value>
			public string TableCatalogName
			{
				get
				{
					return tableCatalogName;
				}
			}

			/// <summary>
			/// Gets the name of the table schema.
			/// </summary>
			/// <value>The name of the table schema.</value>
			public string TableSchemaName
			{
				get
				{
					return tableSchemaName;
				}
			}

			/// <summary>
			/// Gets the name of the table.
			/// </summary>
			/// <value>The name of the table.</value>
			public string TableName
			{
				get
				{
					return tableName;
				}
			}

			#endregion

			#region Private variables.

			private string constraintCatalogName;
			private string constraintSchemaName;
			private string constraintName;
			private SchemaConstraintType constraintType;
			private string tableCatalogName;
			private string tableSchemaName;
			private string tableName;

			#endregion
		}

		/// <summary>
		/// The type of an constraint.
		/// </summary>
		public enum SchemaConstraintType
		{
			#region Enum members.

			/// <summary>
			/// 
			/// </summary>
			ReferentialConstraint = -1,

			/// <summary>
			/// 
			/// </summary>
			Unknown = 0,

			/// <summary>
			/// 
			/// </summary>
			TableConstraintUnique = 1,

			/// <summary>
			/// 
			/// </summary>
			TableConstraintPrimaryKey = 2,

			/// <summary>
			/// 
			/// </summary>
			TableConstraintForeignKey = 3,

			/// <summary>
			/// 
			/// </summary>
			TableConstraintCheck = 4

			#endregion
		}

		// ------------------------------------------------------------------
		#endregion

		#region Schema routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Dumps the schema information.
		/// </summary>
		/// <returns></returns>
		public string DumpSchemaInformation()
		{
			return DumpSchemaInformation( ConnectionString );
		}

		/// <summary>
		/// Dumps the schema information.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <returns></returns>
		public string DumpSchemaInformation(
			SmartConnectionString connectionString )
		{
			using ( TConnection conn = new TConnection() )
			{
				conn.ConnectionString = connectionString.ConnectionString;
				conn.Open();

				DataTable dts = conn.GetSchema();

				return DumpDataTable( dts );
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
		public DataColumnCollection GetTableSchema(
			string tableName )
		{
			return GetTableSchema( ConnectionString, tableName );
		}

		/// <summary>
		/// Read schema information for a given database table.
		/// Is especially useful for checking the size of a varchar column against
		/// the size of a string the user is about to store.
		/// </summary>
		/// <param name="connectionString">The connection string used to connect.</param>
		/// <param name="tableName">The name of the table.</param>
		/// <returns>Returns a collection of schema columns.</returns>
		/// <remarks>See also: http://google_de/search?q=cache:5AcY7whKMkgC:www.dotnet247.com/247reference/msgs/23/118660.aspx+ado.net+column+size. </remarks>
		public DataColumnCollection GetTableSchema(
			SmartConnectionString connectionString,
			string tableName )
		{
			tableName = TrimQuotes( tableName );

			using ( TConnection conn = new TConnection() )
			{
				conn.ConnectionString = connectionString.ConnectionString;
				conn.Open();

				TCommand selectCommand = new TCommand();
				selectCommand.CommandText =
					string.Format( @"SELECT TOP 1 * FROM [{0}]", tableName );
				selectCommand.Connection = conn;

				TDataAdapter da = new TDataAdapter();
				da.SelectCommand = selectCommand;
				da.MissingSchemaAction = MissingSchemaAction.AddWithKey;

				DataTable dt = new DataTable();
				da.Fill( dt );

				return dt.Columns;
			}
		}

		/// <summary>
		/// Check whether a table is contained.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns>
		/// 	<c>true</c> if [contains table column] [the specified table name]; otherwise, <c>false</c>.
		/// </returns>
		public bool ContainsTableColumn(
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
		/// <param name="connectionString">The connection string.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns>
		/// 	<c>true</c> if [contains table column] [the specified connection string]; otherwise, <c>false</c>.
		/// </returns>
		public bool ContainsTableColumn(
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
					columnName = TrimQuotes( columnName );

					return columns.Contains( columnName );
				}
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Check whether a table is contained. in the database schema.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <returns>
		/// 	<c>true</c> if the specified table name contains table; otherwise, <c>false</c>.
		/// </returns>
		public bool ContainsTable(
			string tableName )
		{
			return ContainsTable( ConnectionString, tableName );
		}

		/// <summary>
		/// Check whether a table is contained. in the database schema.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <returns>
		/// 	<c>true</c> if the specified connection string contains table; otherwise, <c>false</c>.
		/// </returns>
		public bool ContainsTable(
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
				tableName = TrimQuotes( tableName );

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
		/// Read all the table names of the current connection string.
		/// </summary>
		/// <returns>
		/// Returns a list of table names or NULL if none.
		/// </returns>
		public string[] GetTableNames()
		{
			return GetTableNames( ConnectionString );
		}

		/// <summary>
		/// Read all the table names of the given connection string.
		/// </summary>
		/// <param name="connectionString">The connection string used to connect.</param>
		/// <returns>
		/// Returns a list of table names or NULL if none.
		/// </returns>
		public virtual string[] GetTableNames(
			SmartConnectionString connectionString )
		{
			using ( TConnection conn = new TConnection() )
			{
				conn.ConnectionString = connectionString.ConnectionString;
				conn.Open();

				DataTable dt = conn.GetSchema(
					@"Tables",
					new string[] 
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
		/// Read all the index names for a given table of the current connection string.
		/// </summary>
		/// <returns>
		/// Returns a list of index names or NULL if none.
		/// </returns>
		public SchemaIndexInfo[] GetIndexInfos()
		{
			return GetIndexInfos( ConnectionString );
		}

		/// <summary>
		/// Read all the index names for a given table of the given connection string.
		/// </summary>
		/// <param name="connectionString">The connection string used to connect.</param>
		/// <returns>
		/// Returns a list of index names or NULL if none.
		/// </returns>
		public SchemaIndexInfo[] GetIndexInfos(
			SmartConnectionString connectionString )
		{
			using ( TConnection conn = new TConnection() )
			{
				conn.ConnectionString = connectionString.ConnectionString;
				conn.Open();

				DataTable dt = conn.GetSchema(
					@"Indexes",
					new string[] 
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
						// Sort by name.
						result.Sort(
							delegate( SchemaIndexInfo a, SchemaIndexInfo b )
							{
								if ( ConvertHelper.ToString( a.TableName, string.Empty ) ==
									ConvertHelper.ToString( b.TableName, string.Empty ) )
								{
									return
										ConvertHelper.ToString( a.IndexName, string.Empty ).CompareTo(
										ConvertHelper.ToString( b.IndexName, string.Empty ) );
								}
								else
								{
									return
										ConvertHelper.ToString( a.TableName, string.Empty ).CompareTo(
										ConvertHelper.ToString( b.TableName, string.Empty ) );
								}
							} );

						return result.ToArray();
					}
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Abstracts to override.
		// ------------------------------------------------------------------

		/// <summary>
		/// Must override, return NULL for none.
		/// </summary>
		/// <value>The quote prefix.</value>
		public abstract string QuotePrefix
		{
			get;
		}

		/// <summary>
		/// Must override, return NULL for none.
		/// </summary>
		/// <value>The quote suffix.</value>
		public abstract string QuoteSuffix
		{
			get;
		}

		/// <summary>
		/// Gets the identity select SQL.
		/// </summary>
		/// <param name="connection">The connection.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <returns></returns>
		protected abstract string GetIdentitySelectSql(
			TConnection connection,
			string tableName );

		// ------------------------------------------------------------------
		#endregion

		#region Miscellaneous properties, variables and routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// If the configuration file specifies a command timeout,
		/// set it now.
		/// </summary>
		/// <param name="command">The command.</param>
		public void CheckSetCommandTimeout(
			DbCommand command )
		{
			if ( wantSetCommandTimeout && command != null )
			{
				command.CommandTimeout = CommandTimeoutSeconds;
			}
		}

		/// <summary>
		/// The default connection string for the ExecuteXxx() routines.
		/// If you use an overload with no connection string, this value
		/// here is used.
		/// </summary>
		/// <value>The connection string.</value>
		/// <remarks>The "connectionString" appSettings value of the application
		/// configuration file is read automatically.</remarks>
		public SmartConnectionString ConnectionString
		{
			get
			{
				return connectionString;
			}
			set
			{
				connectionString = value;
			}
		}

		/// <summary>
		/// The type of the database.
		/// </summary>
		public enum DatabaseType
		{
			#region Enum members.

			/// <summary>
			/// Unknown type.
			/// </summary>
			Unknown,

			/// <summary>
			/// Microsoft SQL server.
			/// </summary>
			SqlServer,

			/// <summary>
			/// Microsoft Access.
			/// </summary>
			Access,

			/// <summary>
			/// Oracle.
			/// </summary>
			Oracle

			#endregion
		}

		/// <summary>
		/// Stores the current connection string.
		/// </summary>
		private SmartConnectionString connectionString =
			InitializeConnectionString();

		/// <summary>
		/// First-sets the connection string.
		/// </summary>
		/// <returns>
		/// Returns the connection string if found,
		/// or NULL if not found.
		/// </returns>
		public static SmartConnectionString InitializeConnectionString()
		{
			SmartConnectionString connectionString =
				new SmartConnectionString(
				LibraryConfiguration.Current.Database.ConnectionString );

			return connectionString;
		}

		/// <summary>
		/// Regular Expression.
		/// </summary>
		private static Regex rxIsModifyingQuery =
			new Regex(
			@"(\bUPDATE\b)|(\bINSERT\b)|(\bDELETE\b)",
			RegexOptions.IgnoreCase | RegexOptions.Compiled );

		/// <summary>
		/// Regular Expression.
		/// </summary>
		private static Regex rxDetectCommandType =
			new Regex(
			@"(\bSELECT\b)|(\bEXECUTE\b)|(\bUPDATE\b)|(\bINSERT\b)|(\bDELETE\b)",
			RegexOptions.IgnoreCase | RegexOptions.Compiled );

		/// <summary>
		/// Checks whether the query is a modifying query.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <returns>
		/// 	<c>true</c> if [is modifying query] [the specified SQL]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsModifyingQuery(
			string sql )
		{
			return rxIsModifyingQuery.IsMatch( sql );
		}

		/// <summary>
		/// Tries to detect whether the given command text is
		/// a stored procedure call or a normal text.
		/// </summary>
		/// <param name="commandText">The command text to check.</param>
		/// <returns>Returns the command type.</returns>
		public static CommandType DetectCommandType(
			string commandText )
		{
			if ( rxDetectCommandType.IsMatch( commandText ) )
			{
				return CommandType.Text;
			}
			else
			{
				return CommandType.StoredProcedure;
			}
		}

		/// <summary>
		/// Removes the quotes, if any.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		protected string TrimQuotes(
			string name )
		{
			if ( string.IsNullOrEmpty( name ) )
			{
				return name;
			}
			else
			{
				if ( !string.IsNullOrEmpty( QuotePrefix ) )
				{
					if ( name.StartsWith( QuotePrefix ) )
					{
						name = name.Substring( QuotePrefix.Length );
					}
				}

				if ( !string.IsNullOrEmpty( QuoteSuffix ) )
				{
					if ( name.EndsWith( QuoteSuffix ) )
					{
						name = name.Substring( 0, name.Length - QuoteSuffix.Length );
					}
				}

				return name;
			}
		}

		/// <summary>
		/// Command timeout.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [want set command timeout]; otherwise, <c>false</c>.
		/// </value>
		private bool wantSetCommandTimeout
		{
			get
			{
				return CommandTimeoutSeconds > 0;
			}
		}

		/// <summary>
		/// Command timeout.
		/// </summary>
		private int commandTimeoutSeconds =
			LibraryConfiguration.Current.Database.CommandTimeoutSeconds;

		/// <summary>
		/// Command timeout.
		/// </summary>
		/// <value>The command timeout seconds.</value>
		public int CommandTimeoutSeconds
		{
			get
			{
				return commandTimeoutSeconds;
			}
			set
			{
				commandTimeoutSeconds = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Trace functionality.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private static object typeLock = new object();

		/// <summary>
		/// Gets or sets a value indicating whether [want trace].
		/// </summary>
		/// <value><c>true</c> if [want trace]; otherwise, <c>false</c>.</value>
		public static bool WantTrace
		{
			get
			{
				return
					wantTrace &&
					LogCentral.Current.IsLoggingEnabled( LogType.Info );
			}
			set
			{
				lock ( typeLock )
				{
					wantTrace =
						value &&
						LogCentral.Current.IsLoggingEnabled( LogType.Info );
				}
			}
		}

		/// <summary>
		/// If turned on, logs a SQL statement.
		/// </summary>
		/// <param name="sql">The SQL statement to log.</param>
		/// <param name="before">Tell, whether before or after the execution of the
		/// SQL statement.</param>
		/// <param name="associatedDataSet">The associated data set.</param>
		[Conditional( @"TRACE" )]
		public static void TraceSql(
			string sql,
			bool before,
			DataSet associatedDataSet )
		{
			if ( WantTrace )
			{
				if ( before )
				{
					TraceSql( @"Executing query", sql, before, associatedDataSet );
				}
				else
				{
					TraceSql( @"Finished executing query", sql, before, associatedDataSet );
				}
			}
		}

		/// <summary>
		/// If turned on, logs a SQL statement.
		/// </summary>
		/// <param name="additionalText">Additional text that is traced.</param>
		/// <param name="sql">The SQL statement to log.</param>
		/// <param name="before">Tell, whether before or after the execution of the
		/// SQL statement.</param>
		/// <param name="associatedDataSet">The associated data set.</param>
		[Conditional( @"TRACE" )]
		public static void TraceSql(
			string additionalText,
			string sql,
			bool before,
			DataSet associatedDataSet )
		{
			if ( WantTrace )
			{
				string msg;

				int rowCount = 0;
				if ( associatedDataSet != null &&
					associatedDataSet.Tables.Count > 0 )
				{
					rowCount = associatedDataSet.Tables[0].Rows.Count;
				}

				if ( before )
				{
					msg = string.Format(
						@"[{0}.] SQL: {1} '{2}'.",
						traceCounter++,
						additionalText,
						sql );
				}
				else
				{
					msg = string.Format(
						@"/SQL: {0} '{1}' (number of returned rows: {2}).",
						additionalText,
						sql,
						rowCount );

					if ( rowCount == 1 && associatedDataSet.Tables[0].Columns.Count == 1 )
					{
						msg += string.Format(
							@" (returned value: {0}).",
							associatedDataSet.Tables[0].Rows[0][0] );
					}
				}

				// --

				if ( LogCentral.Current != null )
				{
					LogCentral.Current.LogInfo( msg );
				}
			}
		}

		/// <summary>
		/// If turned on, logs a SQL command (i.e. a stored procedure call).
		/// </summary>
		/// <param name="cmd">The SQL statement to log.</param>
		/// <param name="before">Tell, whether before or after the execution
		/// of the SQL statement.</param>
		/// <param name="associatedDataSet">The associated data set.</param>
		[Conditional( @"TRACE" )]
		public static void TraceSql(
			IDbCommand cmd,
			bool before,
			DataSet associatedDataSet )
		{
			if ( WantTrace )
			{
				string sql = CreatePseudoSqlFromSP(
					cmd.CommandText,
					cmd.Parameters );

				TraceSql( sql, before, associatedDataSet );
			}
		}

		/// <summary>
		/// If turned on, logs a SQL command (i.e. a stored procedure call).
		/// </summary>
		/// <param name="additionalText">Additional text that is traced.</param>
		/// <param name="cmd">The SQL statement to log.</param>
		/// <param name="before">Tell, whether before or after the execution
		/// of the SQL statement.</param>
		/// <param name="associatedDataSet">The associated data set.</param>
		[Conditional( @"TRACE" )]
		public static void TraceSql(
			string additionalText,
			IDbCommand cmd,
			bool before,
			DataSet associatedDataSet )
		{
			if ( WantTrace )
			{
				string sql = CreatePseudoSqlFromSP(
					cmd.CommandText,
					cmd.Parameters );

				TraceSql( additionalText, sql, before, associatedDataSet );
			}
		}

		/// <summary>
		/// Traces about an SQL error.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <param name="x">The x.</param>
		[Conditional( @"TRACE" )]
		public static void TraceSqlError(
			string sql,
			Exception x )
		{
			TraceSqlError(
				@"Error executing the SQL query",
				sql,
				x );
		}

		/// <summary>
		/// Traces about an SQL error.
		/// </summary>
		/// <param name="additionalText">The additional text.</param>
		/// <param name="sql">The SQL.</param>
		/// <param name="x">The x.</param>
		[Conditional( @"TRACE" )]
		public static void TraceSqlError(
			string additionalText,
			string sql,
			Exception x )
		{
			LogCentral.Current.LogError(
				string.Format(
				@"SQL: {0} '{1}'.",
				additionalText,
				sql ),
				x );
		}

		/// <summary>
		/// Traces about an SQL error.
		/// </summary>
		/// <param name="cmd">The CMD.</param>
		/// <param name="x">The x.</param>
		[Conditional( @"TRACE" )]
		public static void TraceSqlError(
			IDbCommand cmd,
			Exception x )
		{
			string sql;

			if ( cmd == null )
			{
				sql = @"(null)";
			}
			else
			{
				sql = CreatePseudoSqlFromSP(
					cmd.CommandText,
					cmd.Parameters );
			}

			TraceSqlError( sql, x );
		}

		/// <summary>
		/// Traces about an SQL error.
		/// </summary>
		/// <param name="additionalText">The additional text.</param>
		/// <param name="cmd">The CMD.</param>
		/// <param name="x">The x.</param>
		[Conditional( @"TRACE" )]
		public static void TraceSqlError(
			string additionalText,
			IDbCommand cmd,
			Exception x )
		{
			string sql;

			if ( cmd == null )
			{
				sql = @"(null)";
			}
			else
			{
				sql = CreatePseudoSqlFromSP(
					cmd.CommandText,
					cmd.Parameters );
			}

			TraceSqlError( additionalText, sql, x );
		}

		/// <summary>
		/// Puts the current content of a row to a string.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <returns></returns>
		public static string DumpDataTable(
			DataTable table )
		{
			if ( table == null || table.Rows == null || table.Rows.Count == 0 )
			{
				return string.Empty;
			}
			else
			{
				StringBuilder sb = new StringBuilder();

				sb.AppendFormat(
					@"====={0}",
					Environment.NewLine );

				sb.AppendFormat(
					@"Table '{0}'{1}",
					table.TableName,
					Environment.NewLine );
				sb.AppendFormat(
					@"{0} rows{1}",
					table.Rows.Count,
					Environment.NewLine );

				sb.AppendFormat(
					@"{0}",
					Environment.NewLine );

				int rowIndex = 0;
				foreach ( DataRow row in table.Rows )
				{
					sb.AppendFormat(
						@"Row {0}:{1}{2}{3}",
						rowIndex + 1,
						Environment.NewLine,
						DumpDataRow( row ),
						Environment.NewLine );

					rowIndex++;
				}

				sb.AppendFormat(
					@"====={0}",
					Environment.NewLine );

				return sb.ToString().Trim();
			}
		}

		/// <summary>
		/// Puts the current content of a row to a string.
		/// </summary>
		/// <param name="row">The row.</param>
		/// <returns></returns>
		public static string DumpDataRow(
			DataRow row )
		{
			if (
				row == null ||
				row.Table == null ||
				row.Table.Columns == null ||
				row.Table.Columns.Count <= 0 )
			{
				return string.Empty;
			}
			else
			{
				StringBuilder sb = new StringBuilder();

				sb.AppendFormat(
					@"-----{0}",
					Environment.NewLine );

				int index = 0;
				foreach ( DataColumn column in row.Table.Columns )
				{
					if ( index > 0 )
					{
						sb.AppendFormat(
							@",{0}",
							Environment.NewLine );
					}

					object o = row[column];

					if ( o == null || o == DBNull.Value )
					{
						sb.AppendFormat(
							@"[{0}] = null",
							column.ColumnName );
					}
					else
					{
						sb.AppendFormat(
							@"[{0}] = '{1}' ({2})",
							column.ColumnName,
							o,
							o.GetType() );
					}

					index++;
				}

				sb.AppendFormat(
					@"{0}-----",
					Environment.NewLine );

				return sb.ToString().Trim();
			}
		}

		/// <summary>
		/// Creates a string from the given stored procedure name
		/// and its spParameters. Useful when creating a caching key.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		/// <returns></returns>
		public static string QuickCreatePseudoSqlFromSP(
			string spName,
			ICollection spParameters )
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine( spName );

			if ( spParameters != null )
			{
				foreach ( IDbDataParameter param in spParameters )
				{
					if ( param != null )
					{
						sb.AppendLine( param.ParameterName );

						if ( param.Value == null )
						{
							sb.AppendLine( string.Empty );
						}
						else
						{
							sb.AppendLine( param.Value.ToString() );
						}
					}
				}
			}

			return sb.ToString().GetHashCode().ToString();
		}

		/// <summary>
		/// Creates a string from the given stored procedure name
		/// and its spParameters. Useful when creating a caching key.
		/// </summary>
		/// <param name="spName">The name of the stored procedure.</param>
		/// <param name="spParameters">A collection of SqlParameter objects.</param>
		/// <returns></returns>
		public static string CreatePseudoSqlFromSP(
			string spName,
			ICollection spParameters )
		{
			string sql = string.Format(
				@"EXECUTE [{0}]" + Environment.NewLine,
				spName == null ? @"(null)" : spName );

			if ( spParameters != null )
			{
				foreach ( IDbDataParameter param in spParameters )
				{
					if ( param != null )
					{
						bool isString1 =
							(param is SqlParameter) &&
							(
							((SqlDbType)param.DbType) == SqlDbType.UniqueIdentifier ||
							((SqlDbType)param.DbType) == SqlDbType.Char ||
							((SqlDbType)param.DbType) == SqlDbType.VarChar ||
							((SqlDbType)param.DbType) == SqlDbType.NChar ||
							((SqlDbType)param.DbType) == SqlDbType.NVarChar ||
							((SqlDbType)param.DbType) == SqlDbType.Text ||
							((SqlDbType)param.DbType) == SqlDbType.NText
							);

						bool isString2 =
							(param is OdbcParameter) &&
							(
							((OdbcType)param.DbType) == OdbcType.Char ||
							((OdbcType)param.DbType) == OdbcType.NChar ||
							((OdbcType)param.DbType) == OdbcType.NText ||
							((OdbcType)param.DbType) == OdbcType.NVarChar ||
							((OdbcType)param.DbType) == OdbcType.Text ||
							((OdbcType)param.DbType) == OdbcType.VarChar
							);

						bool isString3 =
							(param is TParameter) &&
							(
							((OleDbType)param.DbType) == OleDbType.Guid ||
							((OleDbType)param.DbType) == OleDbType.Char ||
							((OleDbType)param.DbType) == OleDbType.VarChar ||
							((OleDbType)param.DbType) == OleDbType.WChar ||
							((OleDbType)param.DbType) == OleDbType.VarWChar ||
							((OleDbType)param.DbType) == OleDbType.LongVarChar ||
							((OleDbType)param.DbType) == OleDbType.LongVarWChar ||
							((OleDbType)param.DbType) == OleDbType.BSTR
							);

						bool isString4 =
							param.DbType == DbType.AnsiString ||
							param.DbType == DbType.AnsiStringFixedLength ||
							param.DbType == DbType.String ||
							param.DbType == DbType.StringFixedLength ||
							param.DbType == DbType.Guid;

						bool isString5 =
							(param is OracleParameter) &&
							(
							((OracleType)param.DbType) == OracleType.NChar ||
							((OracleType)param.DbType) == OracleType.Char ||
							((OracleType)param.DbType) == OracleType.VarChar ||
							((OracleType)param.DbType) == OracleType.NVarChar ||
							((OracleType)param.DbType) == OracleType.LongVarChar
							);

						object value = null;

						if ( isString1 || isString2 || isString3 || isString4 || isString5 )
						{
							value = string.Format(
								@"'{0}'",
								param.Value );
						}
						else
						{
							value = param.Value;
						}

						if ( value == null || value == DBNull.Value )
						{
							value = @"(NULL)";
						}

						sql += string.Format(
							@"    {0} = {1}," + Environment.NewLine,
							param.ParameterName,
							value );
					}
				}
			}

			sql = sql.Trim();
			sql = sql.Trim( ',' );
			sql = sql.Trim();

			return sql;
		}

		/// <summary>
		/// Only trace if enabled.
		/// </summary>
		private static bool wantTrace =
			LibraryConfiguration.Current.Database.TraceSqlEnabled;

		/// <summary>
		/// 
		/// </summary>
		private static int traceCounter = 0;

		// ------------------------------------------------------------------
		#endregion

		#region Precalculating values based on the connection string.
		// ------------------------------------------------------------------

		/// <summary>
		/// Stores the cached connection string, as well as some more
		/// information.
		/// </summary>
		internal class ConnectionStringInfo
		{
			#region Public methods.

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="connectionString">The connection string.</param>
			public ConnectionStringInfo(
				SmartConnectionString connectionString )
			{
				this.connectionString = connectionString.Clone()
					as SmartConnectionString;

				if ( !SmartConnectionString.IsNullOrEmpty( connectionString ) )
				{
					// Log as two separate statements to even show the first
					// one if the second would fail due to security 
					// restrictions.
					LogCentral.Current.LogDebug(
						@"Using connection string: " );
					LogCentral.Current.LogDebug(
						connectionString.ConnectionString );

					// Normalized.
					string csn = connectionString.ConnectionString.ToLower();

					if ( csn.IndexOf( @"SQLOLEDB".ToLower() ) >= 0 ||
						csn.IndexOf( @"SQLOdbc".ToLower() ) >= 0 )
					{
						databaseType = DatabaseType.SqlServer;
						trueValue = @"1";
						falseValue = @"0";
					}
					else if ( csn.IndexOf( @"Microsoft.Jet.OLEDB".ToLower() ) >= 0 ||
						csn.IndexOf( @"Microsoft.Jet".ToLower() ) >= 0 )
					{
						databaseType = DatabaseType.Access;
						trueValue = @"TRUE";
						falseValue = @"FALSE";
					}
					else if ( csn.IndexOf( @"OraOLEDB.Oracle".ToLower() ) >= 0 )
					{
						databaseType = DatabaseType.Oracle;
						trueValue = @"1";
						falseValue = @"0";
					}

					LogCentral.Current.LogDebug(
						string.Format(
						@"Detected database type '{0}'.",
						databaseType ) );

					return;
				}

				// If here, not found.
				LogCentral.Current.LogInfo(
					string.Format(
					@"The database type cannot be detected from the passed connection string." ) );
			}

			/// <summary>
			/// Replaces the true and false placeholders with their correct
			/// values.
			/// </summary>
			/// <param name="sql">The SQL.</param>
			/// <returns></returns>
			public string ReplaceQueryBoolean(
				string sql )
			{
				if ( string.IsNullOrEmpty( sql ) )
				{
					return sql;
				}
				else
				{
					sql = sql.Replace( DBHelper.TruePlaceholder, trueValue );
					sql = sql.Replace( DBHelper.FalsePlaceholder, falseValue );

					return sql;
				}
			}

			#endregion

			#region Public properties.

			/// <summary>
			/// Gets the connection string.
			/// </summary>
			/// <value>The connection string.</value>
			public SmartConnectionString ConnectionString
			{
				get
				{
					return connectionString;
				}
			}

			/// <summary>
			/// Gets the type of the database.
			/// </summary>
			/// <value>The type of the database.</value>
			public DatabaseType DatabaseType
			{
				get
				{
					return databaseType;
				}
			}

			/// <summary>
			/// Gets the true value.
			/// </summary>
			/// <value>The true value.</value>
			public string TrueValue
			{
				get
				{
					return trueValue;
				}
			}

			/// <summary>
			/// Gets the false value.
			/// </summary>
			/// <value>The false value.</value>
			public string FalseValue
			{
				get
				{
					return falseValue;
				}
			}

			#endregion

			#region Private variables.

			private SmartConnectionString connectionString;
			private DatabaseType databaseType = DatabaseType.Unknown;
			private string trueValue = @"1";
			private string falseValue = @"0";

			#endregion
		}

		/// <summary>
		/// Manages the cached connection strings.
		/// </summary>
		internal class ConnectionStringManager
		{
			#region Public methods.

			/// <summary>
			/// Lookup and returns the values.
			/// </summary>
			/// <param name="connectionString">The connection string.</param>
			/// <returns></returns>
			public ConnectionStringInfo CheckGet(
				SmartConnectionString connectionString )
			{
				ConnectionStringInfo result;
				if ( cachedInfos.TryGetValue( connectionString, out result ) )
				{
					return result;
				}
				else
				{
					ConnectionStringInfo info = new ConnectionStringInfo(
						connectionString );

					cachedInfos[connectionString] = info;
					return info;
				}
			}

			#endregion

			#region Private variables.

			/// <summary>
			/// 
			/// </summary>
			private Dictionary<SmartConnectionString, ConnectionStringInfo>
				cachedInfos =
				new Dictionary<SmartConnectionString, ConnectionStringInfo>();

			#endregion
		}

		/// <summary>
		/// Manager.
		/// </summary>
		private static ConnectionStringManager csManager =
			new ConnectionStringManager();

		/// <summary>
		/// Gets the CS manager.
		/// </summary>
		/// <value>The CS manager.</value>
		internal static ConnectionStringManager CSManager
		{
			get
			{
				return csManager;
			}
		}

		/// <summary>
		/// Replaces the true and false placeholders with their correct
		/// values. Depends on the connection string.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sql">The SQL.</param>
		/// <returns></returns>
		public string ReplaceQueryBoolean(
			SmartConnectionString connectionString,
			string sql )
		{
			if ( string.IsNullOrEmpty( sql ) )
			{
				return sql;
			}
			else
			{
				return CSManager.CheckGet(
					connectionString ).ReplaceQueryBoolean( sql );
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}