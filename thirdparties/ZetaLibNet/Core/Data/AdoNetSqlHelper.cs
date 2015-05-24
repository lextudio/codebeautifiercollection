namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Data;
	using System.Data.SqlClient;
	using System.Text.RegularExpressions;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;

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
	[ComVisible( false )]
	public sealed class AdoNetSqlHelper :
		AdoNetBaseHelper<
		SqlCommand,
		SqlCommandBuilder,
		SqlConnection,
		SqlDataAdapter,
		SqlParameter,
		AdoNetSqlParamCollection>
	{
		#region Singleton access to the one and only instance.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private static object typeLock = new object();

		/// <summary>
		/// Singleton access to the one and only instance.
		/// </summary>
		/// <value>The current.</value>
		public static AdoNetSqlHelper Current
		{
			get
			{
				if ( current == null )
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
						if ( current == null )
						{
							current = new AdoNetSqlHelper();
						}
					}
				}

				return current;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private static volatile AdoNetSqlHelper current = null;

		// ------------------------------------------------------------------
		#endregion

		#region Advanced queries.
		// ------------------------------------------------------------------

		/// <summary>
		/// Splits on "GO" statements.
		/// </summary>
		/// <param name="rawSql">The raw SQL.</param>
		/// <returns></returns>
		public static string[] SplitSqlStatementsOnGO(
			string rawSql )
		{
			string batchSeparator = @"GO";

			Regex regex = new Regex(
			   @"(\n\s*" + batchSeparator + @"\s*\n)(?!\s*\*\/)",
			   RegexOptions.IgnoreCase |
			   RegexOptions.Singleline |
			   RegexOptions.Compiled );

			string[] strings = regex.Split( rawSql );

			List<string> result = new List<string>();

			for ( int i = 0; i < strings.Length; i++ )
			{
				string s = strings[i];

				if ( (!regex.IsMatch( s )) && (!string.IsNullOrEmpty( s )) )
				{
					if ( s.Trim().Length > 0 )
					{
						result.Add( s.Trim() );
					}
				}
			}

			return result.ToArray();
		}

		/// <summary>
		/// Executes the non query batch.
		/// </summary>
		/// <param name="sqls">The SQLS.</param>
		public void ExecuteNonQueryBatch(
			string[] sqls )
		{
			ExecuteNonQueryBatch(
				ConnectionString,
				sqls );
		}

		/// <summary>
		/// Executes the non query batch.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sqls">The SQLS.</param>
		public void ExecuteNonQueryBatch(
			SmartConnectionString connectionString,
			string[] sqls )
		{
			ExecuteNonQueryBatch(
				connectionString,
				sqls,
				new AdoNetCacheItemInformation() );
		}

		/// <summary>
		/// Executes the non query batch.
		/// </summary>
		/// <param name="sqls">The SQLS.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public void ExecuteNonQueryBatch(
			string[] sqls,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			ExecuteNonQueryBatch(
				ConnectionString,
				sqls,
				cacheItemInfo );
		}

		/// <summary>
		/// Executes the non query batch.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="sqls">The SQLS.</param>
		/// <param name="cacheItemInfo">The cache item info.</param>
		public void ExecuteNonQueryBatch(
			SmartConnectionString connectionString,
			string[] sqls,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			if ( sqls != null && sqls.Length > 0 )
			{
				TraceSql(
					string.Format(
					@"Start executing {0} SQL statements in batch.",
					sqls.Length ),
					true, null );
				try
				{
					// Open connection once.
					using ( SqlConnection conn = new SqlConnection() )
					{
						conn.ConnectionString =
							connectionString.ConnectionString;
						conn.Open();

						// Iterate through all SQLs.
						foreach ( string sql in sqls )
						{
							SqlCommand cmd = new SqlCommand();
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
									AdoNetCacheManager.Current.RemoveAll(
										cacheItemInfo );
								}
							}
#endif
						}
					}
				}
				finally
				{
					TraceSql(
						string.Format(
						@"End executing {0} SQL statements in batch.",
						sqls.Length ),
						true, null );
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
		/// <value></value>
		public override string QuotePrefix
		{
			get
			{
				return @"[";
			}
		}

		/// <summary>
		/// Must override, return NULL for none.
		/// </summary>
		/// <value></value>
		public override string QuoteSuffix
		{
			get
			{
				return @"]";
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		protected override string GetIdentitySelectSql(
			SqlConnection connection,
			string tableName )
		{
			return string.Format(
				@"SELECT IDENT_CURRENT('{0}')",
				tableName );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Schema routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Read all the table names of the given connection string.
		/// </summary>
		/// <param name="connectionString">The connection string used to connect.</param>
		/// <returns>
		/// Returns a list of table names or NULL if none.
		/// </returns>
		public override string[] GetTableNames(
			SmartConnectionString connectionString )
		{
			DataTable table = ExecuteTable(
				connectionString,
				@"SELECT * 
				FROM INFORMATION_SCHEMA.TABLES
				WHERE TABLE_TYPE = 'BASE TABLE'
				ORDER BY TABLE_TYPE, TABLE_NAME" );

			if ( table == null )
			{
				return null;
			}
			else
			{
				List<string> result = new List<string>();

				foreach ( DataRow row in table.Rows )
				{
					string name;
					DBHelper.ReadField( out name, row[@"TABLE_NAME"] );

					result.Add( name );
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

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}