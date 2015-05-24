namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Configuration;
	using System.Collections;
	using System.Data;
	using System.Data.SqlClient;
	using System.Data.SqlTypes;
	using System.Data.OleDb;
	using System.Data.Odbc;
	using System.Data.OracleClient;
	using System.Diagnostics;
	using System.IO;
	using System.Security.Cryptography;
	using System.Text;
	using System.Text.RegularExpressions;

	using ZetaLib.Core.Common;
	using System.Data.Common;
	using System.Collections.Generic;

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
		where TCommand : DbCommand
		where TCommandBuilder : DbCommandBuilder
		where TConnection : DbConnection
		where TDataAdapter : DbDataAdapter
		where TParameter : DbParameter, new()
		where TParamCollection : AdoNetBaseParamCollection<TParameter>
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
				using ( TConnection conn = new TConnection() )
				{
					conn.ConnectionString = connectionString.ConnectionString;
					conn.Open();

					TDataAdapter da = new TDataAdapter( sql, conn );

					// Apply command timeouts, if any.
					CheckSetCommandTimeout( da.DeleteCommand );
					CheckSetCommandTimeout( da.InsertCommand );
					CheckSetCommandTimeout( da.SelectCommand );
					CheckSetCommandTimeout( da.UpdateCommand );

					DataSet ds = new DataSet();
					TraceSql( sql, true, ds );
					try
					{
						string tableName = 
							AdoNetBaseUpdater<TCommand, TCommandBuilder, TConnection, TDataAdapter, TParameter, TParamCollection>.
							DetectTableName( sql );

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

		#region Miscellaneous properties, variables and routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// The default connection string for the ExecuteXxx() routines.
		/// If you use an overload with no connection string, this value
		/// here is used.
		/// </summary>
		/// <remarks>The "connectionString" appSettings value of the application
		/// configuration file is read automatically.</remarks>
		public static SmartConnectionString ConnectionString
		{
			get
			{
				lock ( typeof( AdoNetBaseHelper ) )
				{
					return connectionString;
				}
			}
			set
			{
				lock ( typeof( AdoNetBaseHelper ) )
				{
					connectionString = value;
				}
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
		/// Tries to detect a database type from the connection string.
		/// </summary>
		/// <param name="connectionString">The connection string used to detect
		/// the database type.</param>
		/// <returns>Returns the database type or throws exception if not
		/// detectable.</returns>
		public static DatabaseType
			DetectDatabaseTypeFromConnectionString(
			SmartConnectionString connectionString )
		{
			if ( !SmartConnectionString.IsNullOrEmpty( connectionString ) )
			{
				// Cached.
				if ( cachedConnectionStringTypes.ContainsKey( connectionString ) )
				{
					return cachedConnectionStringTypes[connectionString];
				}
				else
				{
					SmartConnectionString originalConnectionString =
						connectionString.Clone() as SmartConnectionString;
					DatabaseType result;

					if ( connectionString.ConnectionString.ToLower().IndexOf( @"SQLOLEDB".ToLower() ) >= 0 ||
						connectionString.ConnectionString.ToLower().IndexOf( @"SQLOdbc".ToLower() ) >= 0 )
					{
						result = DatabaseType.SqlServer;
					}
					else if ( connectionString.ConnectionString.ToLower().IndexOf( @"Microsoft.Jet.OLEDB".ToLower() ) >= 0 ||
						connectionString.ConnectionString.ToLower().IndexOf( @"Microsoft.Jet".ToLower() ) >= 0 )
					{
						result = DatabaseType.Access;
					}
					else if ( connectionString.ConnectionString.ToLower().IndexOf( @"OraOLEDB.Oracle".ToLower() ) >= 0 )
					{
						result = DatabaseType.Oracle;
					}
					else
					{
						result = DatabaseType.Unknown;
					}

					LogCentral.Current.LogDebug(
						string.Format(
						@"Detected database type '{0}'.",
						DatabaseType.SqlServer ) );

					cachedConnectionStringTypes[originalConnectionString] = result;
					return result;
				}
			}

			// If here, not found.
			LogCentral.Current.LogInfo(
				string.Format(
				@"The database type cannot be detected from the passed connection string." ) );
			return DatabaseType.Unknown;
		}

		/// <summary>
		/// Stores the current connection string.
		/// </summary>
		private static SmartConnectionString connectionString =
			InitializeConnectionString();

		/// <summary>
		/// First-sets the connection string.
		/// </summary>
		/// <returns>Returns the connection string if found,
		/// or NULL if not found.</returns>
		private static SmartConnectionString InitializeConnectionString()
		{
			SmartConnectionString connectionString =
				new SmartConnectionString(
				LibraryConfiguration.Current.Database.ConnectionString );

			return connectionString;
		}

		/// <summary>
		/// Checks whether the query is a modifying query.
		/// </summary>
		protected static bool IsModifyingQuery(
			string sql )
		{
			return StringHelper.RXTest(
				sql,
				@"(\bUPDATE\b)|(\bINSERT\b)|(\bDELETE\b)", @"i" );
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
			if ( StringHelper.RXTest(
				commandText,
				@"(\bSELECT\b)|(\bEXECUTE\b)|(\bUPDATE\b)|(\bINSERT\b)|(\bDELETE\b)", @"i" ) )
			{
				return CommandType.Text;
			}
			else
			{
				return CommandType.StoredProcedure;
			}
		}

		/// <summary>
		/// TCommand timeout.
		/// </summary>
		protected static bool WantSetCommandTimeout =
			LibraryConfiguration.Current.Database.CommandTimeoutSeconds > 0;

		/// <summary>
		/// TCommand timeout.
		/// </summary>
		protected static int CommandTimeoutSeconds =
			LibraryConfiguration.Current.Database.CommandTimeoutSeconds;

		// ------------------------------------------------------------------
		#endregion

		#region Trace functionality.
		// ------------------------------------------------------------------

		/// <summary>
		/// If turned on, logs a SQL statement.
		/// </summary>
		/// <param name="sql">The SQL statement to log.</param>
		/// <param name="before">Tell, whether before or after the execution of the
		/// SQL statement.</param>
		/// <param name="associatedDataSet"></param>
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
		/// <param name="associatedDataSet"></param>
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
						@"SQL: {0} '{1}'.",
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
		/// <param name="before">Tell, whether before or after the execution of the
		/// SQL statement.</param>
		/// <param name="associatedDataSet"></param>
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
		/// <param name="before">Tell, whether before or after the execution of the
		/// SQL statement.</param>
		/// <param name="associatedDataSet"></param>
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
		/// <param name="sql"></param>
		/// <param name="x"></param>
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
		/// <param name="additionalText"></param>
		/// <param name="sql"></param>
		/// <param name="x"></param>
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
		/// <param name="cmd"></param>
		/// <param name="x"></param>
		public static void TraceSqlError(
			IDbCommand cmd,
			Exception x )
		{
			string sql;

			if ( cmd == null )
			{
				sql = "(null)";
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
		/// <param name="additionalText"></param>
		/// <param name="cmd"></param>
		/// <param name="x"></param>
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
		/// <param name="row"></param>
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
		/// <param name="spName">The name of the stored procedure.</param>
		/// <param name="spParameters">A collection of SqlParameter objects.</param>
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
							(param is OleDbParameter) &&
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
							"\t{0} = {1}," + Environment.NewLine,
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
		private static bool WantTrace =
			LibraryConfiguration.Current.Database.TraceSqlEnabled;

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// Cache to speed up detection.
		/// </summary>
		private static Dictionary<SmartConnectionString, DatabaseType> cachedConnectionStringTypes =
			new Dictionary<SmartConnectionString, DatabaseType>();

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Base class for parameter collection classes.
	/// </summary>
	public class AdoNetBaseParamCollection<P> :
		ICollection<P>,
		IEnumerable<P>,
		IList<P>,
		ICollection,
		IEnumerable,
		IList
		where P : DbParameter, new()
	{
		#region Static methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Creates a parameter.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="dataType"></param>
		/// <returns></returns>
		public static P CreateParameter(
			string name,
			object value,
			DbType dataType )
		{
			P p = new P();
			p.ParameterName = name;
			p.Value = value;
			p.DbType = dataType;
			return p;
		}

		/// <summary>
		/// Creates a parameter.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="dataType"></param>
		/// <param name="nullBehaviour"></param>
		/// <returns></returns>
		public static P CreateParameter(
			string name,
			object value,
			DbType dataType,
			NullBehaviour nullBehaviour )
		{
			P p = new P();
			p.ParameterName = name;
			p.Value = ApplyNullBehaviourToValue( value, nullBehaviour );
			p.DbType = dataType;
			return p;
		}

		/// <summary>
		/// Creates a parameter.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static P CreateParameter(
			string name,
			object value )
		{
			P p = new P();
			p.ParameterName = name;
			p.Value = value;
			return p;
		}

		/// <summary>
		/// Creates a parameter.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nullBehaviour"></param>
		/// <returns></returns>
		public static P CreateParameter(
			string name,
			object value,
			NullBehaviour nullBehaviour )
		{
			P p = new P();
			p.ParameterName = name;
			p.Value = ApplyNullBehaviourToValue( value, nullBehaviour );
			return p;
		}

		/// <summary>
		/// Creates a parameter.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="dataType"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static P CreateParameter(
			string name,
			object value,
			DbType dataType,
			int size )
		{
			P p = new P();
			p.ParameterName = name;
			p.Value = value;
			p.DbType = dataType;
			p.Size = size;
			return p;
		}

		/// <summary>
		/// Creates a parameter.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="dataType"></param>
		/// <param name="size"></param>
		/// <param name="nullBehaviour"></param>
		/// <returns></returns>
		public static P CreateParameter(
			string name,
			object value,
			DbType dataType,
			int size,
			NullBehaviour nullBehaviour )
		{
			P p = new P();
			p.ParameterName = name;
			p.Value = ApplyNullBehaviourToValue( value, nullBehaviour );
			p.DbType = dataType;
			p.Size = size;
			return p;
		}

		/// <summary>
		/// Creates a parameter.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static P CreateParameter(
			string name,
			object value,
			int size )
		{
			P p = new P();
			p.ParameterName = name;
			p.Value = value;
			p.Size = size;
			return p;
		}

		/// <summary>
		/// Creates a parameter.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="size"></param>
		/// <param name="nullBehaviour"></param>
		/// <returns></returns>
		public static P CreateParameter(
			string name,
			object value,
			int size,
			NullBehaviour nullBehaviour )
		{
			P p = new P();
			p.ParameterName = name;
			p.Value = ApplyNullBehaviourToValue( value, nullBehaviour );
			p.Size = size;
			return p;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public constructors.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public AdoNetBaseParamCollection()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public AdoNetBaseParamCollection(
			params P[] parameters )
		{
			if ( parameters != null )
			{
				foreach ( P parameter in parameters )
				{
					Add( parameter );
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region ICollection member.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				return (parameterList as ICollection).IsSynchronized;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Count
		{
			get
			{
				return parameterList.Count;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(
			Array array,
			int index )
		{
			(parameterList as ICollection).CopyTo( array, index );
		}

		/// <summary>
		/// 
		/// </summary>
		public object SyncRoot
		{
			get
			{
				return (parameterList as ICollection).SyncRoot;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region IEnumerable member.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return parameterList.GetEnumerator();
		}

		// ------------------------------------------------------------------
		#endregion

		#region IList member.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		public bool IsReadOnly
		{
			get
			{
				return (parameterList as IList).IsReadOnly;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public object this[int index]
		{
			get
			{
				return parameterList[index];
			}
			set
			{
				parameterList[index] = value as P;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt( int index )
		{
			parameterList.RemoveAt( index );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void Insert( int index, object value )
		{
			(parameterList as IList).Insert( index, value );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		public void Remove( object value )
		{
			(parameterList as IList).Remove( value );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Contains( object value )
		{
			return (parameterList as IList).Contains( value );
		}

		/// <summary>
		/// 
		/// </summary>
		public void Clear()
		{
			parameterList.Clear();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int IndexOf(
			object value )
		{
			return (parameterList as IList).IndexOf( value );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int Add(
			object value )
		{
			if ( value == null )
			{
				throw new ArgumentNullException( "value" );
			}
			else if ( !(value is P) )
			{
				throw new ArgumentException(
					"Parameter value must be of type P",
					"value" );
			}
			else
			{
				return (parameterList as IList).Add( value );
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int Add(
			P value )
		{
			if ( value == null )
			{
				throw new ArgumentNullException( "value" );
			}
			else
			{
				return (parameterList as IList).Add( value );
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsFixedSize
		{
			get
			{
				return (parameterList as IList).IsFixedSize;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region ICollection<P> members.
		// ------------------------------------------------------------------

		void ICollection<P>.Add( P item )
		{
			parameterList.Add( item );
		}

		public bool Contains( P item )
		{
			return parameterList.Contains( item );
		}

		public void CopyTo( P[] array, int arrayIndex )
		{
			parameterList.CopyTo( array, arrayIndex );
		}

		public bool Remove( P item )
		{
			return parameterList.Remove( item );
		}

		// ------------------------------------------------------------------
		#endregion

		#region IEnumerable<P> members.
		// ------------------------------------------------------------------

		IEnumerator<P> IEnumerable<P>.GetEnumerator()
		{
			return parameterList.GetEnumerator();
		}

		// ------------------------------------------------------------------
		#endregion

		#region IList<P> members.
		// ------------------------------------------------------------------

		public int IndexOf( P item )
		{
			return parameterList.IndexOf( item );
		}

		public void Insert( int index, P item )
		{
			Insert( index, item );
		}

		P IList<P>.this[int index]
		{
			get
			{
				return parameterList[index];
			}
			set
			{
				parameterList[index] = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Returns a modified version of the given value depending on
		/// the NullBehaviour.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="nullBehaviour"></param>
		/// <returns></returns>
		protected static object ApplyNullBehaviourToValue(
			object value,
			NullBehaviour nullBehaviour )
		{
			if ( value == null )
			{
				if ( nullBehaviour == NullBehaviour.ConvertNullToDBNull )
				{
					return DBNull.Value;
				}
				else if ( nullBehaviour == NullBehaviour.ConvertNullToEmptyString )
				{
					return string.Empty;
				}
				else
				{
					return value;
				}
			}
			else if ( (value is string) && value.ToString().Length <= 0 )
			{
				if ( nullBehaviour == NullBehaviour.ConvertEmptyStringToDBNull )
				{
					return DBNull.Value;
				}
				else
				{
					return value;
				}
			}
			else
			{
				return value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private List<P> parameterList = new List<P>();

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// How the CreateParameter()-functions behave when passing null-values.
	/// </summary>
	public enum NullBehaviour
	{
		#region Enum members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Convert NULL values to DBNull.Value.
		/// </summary>
		ConvertNullToDBNull,

		/// <summary>
		/// Just pass on the parameters with no auto-conversion.
		/// </summary>
		NoConversion,

		/// <summary>
		/// Convert a null value to string.Empty.
		/// </summary>
		ConvertNullToEmptyString,

		/// <summary>
		/// Convert a string.Empty to DBNull.Value.
		/// </summary>
		ConvertEmptyStringToDBNull

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Base class for updater classes.
	/// </summary>
	public abstract class AdoNetBaseUpdater<
		TCommand,
		TCommandBuilder,
		TConnection,
		TDataAdapter,
		TParameter,
		TParamCollection> :
		IDisposable
		where TCommand : DbCommand
		where TCommandBuilder : DbCommandBuilder
		where TConnection : DbConnection
		where TDataAdapter : DbDataAdapter
		where TParameter : DbParameter, new()
		where TParamCollection : AdoNetBaseParamCollection<TParameter>
	{
		#region Identity control type.
		// ------------------------------------------------------------------

		/// <summary>
		/// Tells what to do during an update with identity fields.
		/// </summary>
		public enum IdentityControl
		{
			/// <summary>
			/// Read the identity field after the table got updated.
			/// </summary>
			Get,

			/// <summary>
			/// Don't read the identity field after the update.
			/// </summary>
			DontGet
		}

		// ------------------------------------------------------------------
		#endregion

		#region Open methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Open the given SQL query.
		/// Uses the default connection string.
		/// </summary>
		/// <param name="query">The SQL query to execute.</param>
		public void Open(
			string query )
		{
			Open( ConnectionString, query );
		}

		/// <summary>
		/// Open the given SQL query.
		/// </summary>
		/// <param name="connectionString">The connection string to use.</param>
		/// <param name="query">The SQL query to execute.</param>
		public abstract void Open(
			SmartConnectionString connectionString,
			string query );

		/// <summary>
		/// Opens the given SQL stored procedure.
		/// Uses the default connection string.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		public void Open(
			string spName,
			TParamCollection spParameters )
		{
			Open(
				ConnectionString,
				spName,
				spParameters );
		}

		/// <summary>
		/// Opens the given SQL stored procedure.
		/// </summary>
		/// <param name="connectionString">The connection string to use.</param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		public abstract void Open(
			SmartConnectionString connectionString,
			string spName,
			TParamCollection spParameters );

		// ------------------------------------------------------------------
		#endregion

		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Creates a new row in memory.
		/// Only works if the Open function was called or
		/// a query was specified in the constructor.
		/// </summary>
		/// <returns>Returns the new row. The row is now added yet.
		/// You must call AddRow( row ) by yourself.</returns>
		public DataRow NewRow()
		{
			return Table.NewRow();
		}

		/// <summary>
		/// Adds a row in memory.
		/// Only works if the Open function was called or
		/// a query was specified in the constructor.
		/// </summary>
		/// <param name="row">The row, previously created with NewRow(),
		/// to add in memory.</param>
		public void AddRow(
			DataRow row )
		{
			Table.Rows.Add( row );
		}

		/// <summary>
		/// Adds a row in memory and returns the new row.
		/// This function combines the NewRow() and AddRow( row )
		/// functions.
		/// Only works if the Open function was called or
		/// a query was specified in the constructor.
		/// </summary>
		/// <returns>Returns the new row. The row is now added yet.
		/// You must call AddRow( row ) by yourself.</returns>
		public DataRow AddNewRow()
		{
			DataRow row = Table.NewRow();
			AddRow( row );
			return row;
		}

		/// <summary>
		/// Either returns the current row, if available,
		/// or adds a new row if not available.
		/// Only works if the Open function was called or
		/// a query was specified in the constructor.
		/// </summary>
		/// <returns>Returns the row.</returns>
		public DataRow CheckGetOrAddNewRow()
		{
			if ( HasRow )
			{
				return Row;
			}
			else
			{
				return AddNewRow();
			}
		}

		/// <summary>
		/// Close the connection.
		/// </summary>
		public void Close()
		{
			if ( connection != null )
			{
				connection.Close();
			}
		}

		/// <summary>
		/// Tries to parse the name of a table from a given SQL query.
		/// </summary>
		/// <param name="sqlQuery">The SQL query to parse.</param>
		/// <returns>Returns the name of the table if found,
		/// returns NULL if not found.</returns>
		public static string DetectTableName(
			string sqlQuery )
		{
			string pattern = @"\b(EXEC|EXECUTE|FROM|INTO)\s+([^\s]+)";
			RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Singleline;
			Regex rx = new Regex( pattern, options );

			Match m = rx.Match( sqlQuery );
			if ( m != null && m.Groups.Count > 2 )
			{
				string s = m.Groups[2].Value;
				return s.Trim( '[', ']' );
			}

			// --

			return null;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Updating methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Write any changes to database.
		/// </summary>
		/// <param name="identityControl">Specify whether you want to try to get the 
		/// identity value (if any) of the inserted row.</param>
		/// <returns>Returns the identity (if requested and possible) 
		/// or 0 if unavailable or not retrievable.</returns>
		public int Update(
			IdentityControl identityControl )
		{
			return Update(
				identityControl,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Write any changes to database.
		/// </summary>
		/// <param name="identityControl">Specify whether you want to try to get the 
		/// identity value (if any) of the inserted row.</param>
		/// <param name="hintTableName">If the table name could not be automatically
		/// detected, use this parameter to specify the table name
		/// to get the identity from (e.g. useful when this class was filled
		/// by a stored procedure.</param>
		/// <returns>Returns the identity (if requested and possible) 
		/// or 0 if unavailable or not retrievable.</returns>
		public int Update(
			string hintTableName,
			IdentityControl identityControl )
		{
			return Update(
				identityControl,
				hintTableName,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Write any changes to database.
		/// </summary>
		/// <param name="identityControl">Specify whether you want to try to get the 
		/// identity value (if any) of the inserted row.</param>
		/// <param name="hintTableName">If the table name could not be automatically
		/// detected, use this parameter to specify the table name
		/// to get the identity from (e.g. useful when this class was filled
		/// by a stored procedure.</param>
		/// <returns>Returns the identity (if requested and possible) 
		/// or 0 if unavailable or not retrievable.</returns>
		public int Update(
			IdentityControl identityControl,
			string hintTableName )
		{
			return Update(
				identityControl,
				hintTableName,
				null as AdoNetBaseCacheControl );
		}

		/// <summary>
		/// Write any changes to database.
		/// </summary>
		/// <param name="cacheControl">Specifies how the cache should
		/// be handled.</param>
		/// <returns>Returns the identity (if requested and possible) 
		/// or 0 if unavailable or not retrievable.</returns>
		public int Update(
			AdoNetBaseCacheControl cacheControl )
		{
			return Update(
				IdentityControl.Get,
				cacheControl );
		}

		/// <summary>
		/// Write any changes to database.
		/// </summary>
		/// <param name="hintTableName">If the table name could not be automatically
		/// detected, use this parameter to specify the table name
		/// to get the identity from (e.g. useful when this class was filled</param>
		/// <param name="cacheControl">Specifies how the cache should
		/// be handled.</param>
		/// <returns>Returns the identity (if requested and possible) 
		/// or 0 if unavailable or not retrievable.</returns>
		public int Update(
			string hintTableName,
			AdoNetBaseCacheControl cacheControl )
		{
			return Update(
				IdentityControl.Get,
				hintTableName,
				cacheControl );
		}

		/// <summary>
		/// Write any changes to database.
		/// </summary>
		/// <param name="identityControl">Specify whether you want to try to get the 
		/// identity value (if any) of the inserted row.</param>
		/// <param name="hintTableName">If the table name could not be automatically
		/// detected, use this parameter to specify the table name
		/// to get the identity from (e.g. useful when this class was filled
		/// by a stored procedure.</param>
		/// <param name="cacheControl">Specifies how the cache should
		/// be handled.</param>
		/// <returns>Returns the identity (if requested and possible) 
		/// or 0 if unavailable or not retrievable.</returns>
		public int Update(
			IdentityControl identityControl,
			string hintTableName,
			AdoNetBaseCacheControl cacheControl )
		{
			try
			{
				dataAdapter.Update( dataSet );
			}
			catch ( Exception x )
			{
				if ( query == null )
				{
					TraceUpdateSqlError( command, x );
				}
				else
				{
					TraceUpdateSqlError( query, x );
				}
				throw;
			}
			TraceUpdateSql( @"Updated successfully.", false );

			// Ensure the cache is clear.
			if ( cacheControl.Usage != AdoNetCacheUsage.DontClear )
			{
				AdoNetBaseCacheManager.Current.ClearCache( cacheControl );
			}

			if ( identityControl == IdentityControl.Get )
			{
				AdoNetOleDBHelper.DatabaseType type =
					AdoNetOleDBHelper.DetectDatabaseTypeFromConnectionString(
					new SmartConnectionString( Connection.ConnectionString ) );

				// Special handling for MS Access.
				if ( type == AdoNetOleDBHelper.DatabaseType.Access )
				{
					// Was filled inside the event.
					return lastIdentityValue;
				}
				else
				{
					return GetIdentity( hintTableName );
				}
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// Write any changes to database.
		/// </summary>
		/// <param name="identityControl">Specify whether you want to try to get the 
		/// identity value (if any) of the inserted row.</param>
		/// <param name="cacheControl">Specifies how the cache should
		/// be handled.</param>
		/// <returns>Returns the identity (if requested and possible) 
		/// or 0 if unavailable or not retrievable.</returns>
		public int Update(
			IdentityControl identityControl,
			AdoNetBaseCacheControl cacheControl )
		{
			return Update(
				identityControl,
				tableName,
				cacheControl );
		}

		/// <summary>
		/// Write any changes to database.
		/// </summary>
		/// <returns>Returns the identity (if requested and possible) 
		/// or 0 if unavailable or not retrievable.</returns>
		public int Update()
		{
			return Update(
				IdentityControl.Get );
		}

		/// <summary>
		/// Write any changes to database.
		/// </summary>
		/// <param name="hintTableName">If the table name could not be automatically
		/// detected, use this parameter to specify the table name
		/// to get the identity from (e.g. useful when this class was filled
		/// by a stored procedure.</param>
		/// <returns>Returns the identity (if requested and possible) 
		/// or 0 if unavailable or not retrievable.</returns>
		public int Update(
			string hintTableName )
		{
			return Update(
				IdentityControl.Get,
				hintTableName );
		}

		/// <summary>
		/// Write any changes to database.
		/// </summary>
		/// <param name="hintTableName">If the table name could not be automatically
		/// detected, use this parameter to specify the table name
		/// to get the identity from (e.g. useful when this class was filled
		/// by a stored procedure.</param>
		/// <returns>Returns the identity (if requested and possible) 
		/// or 0 if unavailable or not retrievable.</returns>
		public void Update(
			ref int id,
			string hintTableName )
		{
			if ( id == 0 )
			{
				// Not yet set, read now.
				id = Update(
					IdentityControl.Get,
					hintTableName );
			}
			else
			{
				// Already set, not again.
				Update(
					IdentityControl.DontGet,
					hintTableName );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Identity methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Detectes the identity. Call this function after
		/// the Update() function returned successfully.
		/// </summary>
		/// <returns>Returns the identity, if any.</returns>
		public int GetIdentity()
		{
			return GetIdentity( connection, tableName );
		}

		/// <summary>
		/// Detectes the identity. Call this function after
		/// the Update() function returned successfully.
		/// </summary>
		/// <param name="conn">The connection used to query
		/// for the identity value.</param>
		/// <returns>Returns the identity, if any.</returns>
		public int GetIdentity(
			TConnection connection )
		{
			return GetIdentity( connection, tableName );
		}

		/// <summary>
		/// Detectes the identity. Call this function after
		/// the Update() function returned successfully.
		/// </summary>
		/// <param name="tableName">The table name, as a hint,
		/// if auto-detection of the table name fails. This parameter usually
		/// is required if the SQL query that was used to open the database,
		/// was an EXECUTE of a stored procedure.</param>
		/// <returns>Returns the identity, if any.</returns>
		public int GetIdentity(
			string tableName )
		{
			return GetIdentity( connection, tableName );
		}

		/// <summary>
		/// Detectes the identity. Call this function after
		/// the Update() function returned successfully.
		/// </summary>
		/// <param name="conn">The connection used to query
		/// for the identity value.</param>
		/// <param name="tableName">The table name, as a hint,
		/// if auto-detection of the table name fails. This parameter usually
		/// is required if the SQL query that was used to open the database,
		/// was an EXECUTE of a stored procedure.</param>
		/// <returns>Returns the identity, if any.</returns>
		public abstract int GetIdentity(
			TConnection connection,
			string tableName );

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Access a ROW of the current queried table (if any).
		/// Only works if the Open function was called or
		/// a query was specified in the constructor.
		/// </summary>
		public DataRow this[int index]
		{
			get
			{
				return Table.Rows[index];
			}
		}

		/// <summary>
		/// The underlying DataTable object.
		/// </summary>
		public DataTable Table
		{
			get
			{
				if ( dataSet == null || dataSet.Tables == null || dataSet.Tables.Count <= 0 )
				{
					return null;
				}
				else
				{
					return dataSet.Tables[0];
				}
			}
		}

		/// <summary>
		/// Returns the first row of the current table.
		/// Usually, you only need the first row.
		/// Only works if the Open function was called or
		/// a query was specified in the constructor.
		/// </summary>
		public DataRow Row
		{
			get
			{
				if ( Table == null || Table.Rows == null || Table.Rows.Count <= 0 )
				{
					return null;
				}
				else
				{
					return Table.Rows[0];
				}
			}
		}

		/// <summary>
		/// Check whether a row is present.
		/// </summary>
		public bool HasRow
		{
			get
			{
				return Row != null;
			}
		}

		/// <summary>
		/// Returns the rows of the current table.
		/// Only works if the Open function was called or
		/// a query was specified in the constructor.
		/// </summary>
		public DataRowCollection Rows
		{
			get
			{
				if ( Table == null || Table.Rows == null )
				{
					return null;
				}
				else
				{
					return Table.Rows;
				}
			}
		}

		/// <summary>
		/// The connection string that is used to query
		/// the database.
		/// The default value is automatically read from the
		/// "connectionString" app value of the config file,
		/// if present.
		/// </summary>
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
		/// The underlying TConnection object.
		/// </summary>
		public TConnection Connection
		{
			get
			{
				return connection;
			}
			set
			{
				connection = value;
			}
		}

		/// <summary>
		/// The underlying TDataAdapter object.
		/// </summary>
		public TDataAdapter DataAdapter
		{
			get
			{
				return dataAdapter;
			}
			set
			{
				dataAdapter = value;
			}
		}

		/// <summary>
		/// The underlying DataSet object.
		/// </summary>
		public DataSet DataSet
		{
			get
			{
				return dataSet;
			}
			set
			{
				dataSet = value;
			}
		}

		/// <summary>
		/// The underlying TCommandBuilder object.
		/// </summary>
		public TCommandBuilder CommandBuilder
		{
			get
			{
				return commandBuilder;
			}
			set
			{
				commandBuilder = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region IDisposable members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Disposes the object.
		/// </summary>
		public void Dispose()
		{
			if ( dataSet != null )
			{
				dataSet.Dispose();
				dataSet = null;
			}

			if ( command != null )
			{
				command.Dispose();
				command = null;
			}

			if ( dataAdapter != null )
			{
				dataAdapter.Dispose();
				dataAdapter = null;
			}

			if ( commandBuilder != null )
			{
				commandBuilder.Dispose();
				commandBuilder = null;
			}

			if ( connection != null )
			{
				connection.Dispose();
				connection = null;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// The name of the table.
		/// </summary>
		protected string tableName;

		/// <summary>
		/// Remembered query in Open() for logging errors in Update().
		/// </summary>
		protected string query = null;

		/// <summary>
		/// The connection string that is used to query
		/// the database.
		/// The default value is automatically read from the
		/// "connectionString" app value of the config file,
		/// if present.
		/// </summary>
		protected SmartConnectionString connectionString =
			AdoNetBaseHelper.ConnectionString;

		/// <summary>
		/// The underlying DataSet object.
		/// </summary>
		protected DataSet dataSet;

		/// <summary>
		/// The underlying TConnection object.
		/// </summary>
		protected TConnection connection;

		/// <summary>
		/// Remembered comand in Open() for logging errors in Update().
		/// </summary>
		protected TCommand command = null;

		/// <summary>
		/// The underlying TDataAdapter object.
		/// </summary>
		protected TDataAdapter dataAdapter;

		/// <summary>
		/// The underlying TCommandBuilder object.
		/// </summary>
		protected TCommandBuilder commandBuilder;

		/// <summary>
		/// Used for MS Access databases.
		/// </summary>
		protected int lastIdentityValue = 0;

		// ------------------------------------------------------------------
		#endregion

		#region Tracing methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Trace helper.
		/// </summary>
		private void TraceUpdateSql(
			string query,
			bool before )
		{
			AdoNetBaseHelper.TraceSql(
				string.Format(
				@"UPDATE of row '{0}' based upon query",
				AdoNetBaseHelper.DumpDataRow( Row ) ),
				query,
				before,
				dataSet );
		}

		/// <summary>
		/// Trace helper.
		/// </summary>
		private void TraceUpdateSqlError(
			string query,
			Exception x )
		{
			AdoNetBaseHelper.TraceSqlError(
				string.Format(
				@"UPDATE of row '{0}' based upon query",
				AdoNetBaseHelper.DumpDataRow( Row ) ),
				query,
				x );
		}

		/// <summary>
		/// Trace helper.
		/// </summary>
		private void TraceUpdateSql(
			TCommand cmd,
			bool before )
		{
			AdoNetBaseHelper.TraceSql(
				string.Format(
				@"UPDATE of row '{0}' based upon SP",
				AdoNetBaseHelper.DumpDataRow( Row ) ),
				cmd,
				before,
				dataSet );
		}

		/// <summary>
		/// Trace helper.
		/// </summary>
		private void TraceUpdateSqlError(
			TCommand cmd,
			Exception x )
		{
			string s = string.Format(
				@"UPDATE of row '{0}' based upon SP",
				AdoNetBaseHelper.DumpDataRow( Row ) );

			AdoNetBaseHelper.TraceSqlError(
				s,
				cmd,
				x );
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}