namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Data;
	using System.Data.OleDb;
	using System.IO;
	using System.Text.RegularExpressions;
	using ZetaLib.Core.Common;
	using System.Collections.Generic;
	using ZetaLib.Core.Properties;
	using ZetaLib.Core.Localization;
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
	public sealed class AdoNetOleDBHelper :
		AdoNetBaseHelper<
		OleDbCommand,
		OleDbCommandBuilder,
		OleDbConnection,
		OleDbDataAdapter,
		OleDbParameter,
		AdoNetOleDBParamCollection>
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
		public static AdoNetOleDBHelper Current
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
							current = new AdoNetOleDBHelper();
						}
					}
				}

				return current;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private static volatile AdoNetOleDBHelper current = null;

		// ------------------------------------------------------------------
		#endregion

		#region JET routines (JRO) for Microsoft Access databases.
		// ------------------------------------------------------------------

		/// <summary>
		/// Get a full connection string from a given JET file path to
		/// an ".mdb" file.
		/// </summary>
		/// <param name="databaseFilePath">The database file path.</param>
		/// <returns></returns>
		public SmartConnectionString GetJetFilePathConnectionString(
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
		public void CompactJetDatabase()
		{
			CompactJetDatabase( ConnectionString );
		}

		/// <summary>
		/// Compacts the Microsoft Access database of the current connection string.
		/// Only call if you are sure that the connection string contains
		/// a connection to a Microsoft Access database.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		public void CompactJetDatabase(
			SmartConnectionString connectionString )
		{
			if ( File.Exists( connectionString.ConnectionString ) )
			{
				throw new Exception(
					LocalizationHelper.Format(
					Resources.Str_AdoNetOleDBHelper_CompactJetDatabase_Error01,
					LocalizationHelper.CreatePair( @"ConnectionString", connectionString ) ) );
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
		public void CompactJetDatabase(
			FileInfo sourceFilePath )
		{
			if ( !sourceFilePath.Exists )
			{
				throw new ArgumentException(
					LocalizationHelper.Format(
					Resources.Str_AdoNetOleDBHelper_CompactJetDatabase_Error02,
					LocalizationHelper.CreatePair( @"SourceFilePath", sourceFilePath.FullName ) ),
					@"sourceFilePath" );
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
		public void CompactJetDatabase(
			FileInfo sourceFilePath,
			FileInfo destinationFilePath )
		{
			// see also Q306287:
			// "HOW TO: Compact a Microsoft Access Database by Using Visual Basic .NET".
			// http://support.microsoft.com/kb/306287/EN-US/.

			if ( !sourceFilePath.Exists )
			{
				throw new ArgumentException(
					LocalizationHelper.Format(
					Resources.Str_AdoNetOleDBHelper_CompactJetDatabase_Error02,
					LocalizationHelper.CreatePair( @"SourceFilePath", sourceFilePath.FullName ) ),
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
		public void RefreshJetCache()
		{
			RefreshJetCache( ConnectionString );
		}

		/// <summary>
		/// JET has a 3 second cache by default. This function flushes
		/// the cache, forcing immediate update.
		/// </summary>
		/// <param name="connectionString">The complete connection string.</param>
		public void RefreshJetCache(
			SmartConnectionString connectionString )
		{
			if ( File.Exists( connectionString.ConnectionString ) )
			{
				throw new Exception(
					LocalizationHelper.Format(
					Resources.Str_AdoNetOleDBHelper_CompactJetDatabase_Error03,
					LocalizationHelper.CreatePair( @"ConnectionString", connectionString ) ) );
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
		/// <param name="filePath">The file path.</param>
		public void RefreshJetCache(
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

		#region Schema routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Drops the column of a given table if both the table and the column exists.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		public void DropTableColumn(
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
		/// <param name="connectionString">The connection string.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		public void DropTableColumn(
			SmartConnectionString connectionString,
			string tableName,
			string columnName )
		{
			if ( ContainsTable( connectionString, tableName ) &&
				ContainsTableColumn( connectionString, tableName, columnName ) )
			{
				tableName = TrimQuotes( tableName );
				columnName = TrimQuotes( columnName );

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
		/// <param name="constraintName">Name of the constraint.</param>
		public void DropConstraint(
			string constraintName )
		{
			DropConstraint( ConnectionString, constraintName );
		}

		/// <summary>
		/// Check whether a constrained is present.
		/// For creating and dropping constraints, see http://support.microsoft.com/?scid=kb%3Ben-us%3B291539.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="constraintName">Name of the constraint.</param>
		public void DropConstraint(
			SmartConnectionString connectionString,
			string constraintName )
		{
			DropConstraint( connectionString, null, constraintName );
		}

		/// <summary>
		/// Check whether a constrained is present.
		/// For creating and dropping constraints, see http://support.microsoft.com/?scid=kb%3Ben-us%3B291539.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="constraintName">Name of the constraint.</param>
		public void DropConstraint(
			string tableName,
			string constraintName )
		{
			DropConstraint(
				ConnectionString,
				tableName,
				constraintName );
		}

		/// <summary>
		/// Check whether a constrained is present.
		/// For creating and dropping constraints, see http://support.microsoft.com/?scid=kb%3Ben-us%3B291539.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="constraintName">Name of the constraint.</param>
		public void DropConstraint(
			SmartConnectionString connectionString,
			string tableName,
			string constraintName )
		{
			SchemaConstraintInfo[] constraintInfos =
				GetConstraintInfos( connectionString );

			if ( constraintInfos != null && constraintInfos.Length > 0 )
			{
				constraintName = TrimQuotes( constraintName );
				tableName = TrimQuotes( tableName );

				foreach ( SchemaConstraintInfo constraintInfo in constraintInfos )
				{
					if ( string.Compare( constraintInfo.ConstraintName, constraintName, true ) == 0 &&
						!string.IsNullOrEmpty( constraintInfo.TableName ) )
					{
						if ( string.IsNullOrEmpty( tableName ) ||
							string.Compare( tableName, constraintInfo.TableName, true ) == 0 )
						{
							ExecuteNonQuery(
								connectionString,
								string.Format(
								@"ALTER TABLE [{0}] 
								DROP CONSTRAINT [{1}]",
								constraintInfo.TableName,
								constraintInfo.ConstraintName ) );

							// Avoid dropping the same multiple times.
							break;
						}
					}
				}
			}
		}

		/// <summary>
		/// Check whether a constrained is present.
		/// For creating and dropping constraints, see http://support.microsoft.com/?scid=kb%3Ben-us%3B291539.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		public void DropAllTableConstraints(
			string tableName )
		{
			DropAllTableConstraints(
				ConnectionString,
				tableName );
		}

		/// <summary>
		/// Check whether a constrained is present.
		/// For creating and dropping constraints, see http://support.microsoft.com/?scid=kb%3Ben-us%3B291539.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="tableName">Name of the table.</param>
		public void DropAllTableConstraints(
			SmartConnectionString connectionString,
			string tableName )
		{
			SchemaConstraintInfo[] constraintInfos =
				GetConstraintInfos( connectionString );

			if ( constraintInfos != null && constraintInfos.Length > 0 )
			{
				tableName = TrimQuotes( tableName );

				foreach ( SchemaConstraintInfo constraintInfo in constraintInfos )
				{
					if ( !string.IsNullOrEmpty( constraintInfo.TableName ) &&
						string.Compare( tableName, constraintInfo.TableName, true ) == 0 )
					{
						ExecuteNonQuery(
							connectionString,
							string.Format(
							@"ALTER TABLE [{0}] 
							DROP CONSTRAINT [{1}]",
							constraintInfo.TableName,
							constraintInfo.ConstraintName ) );

						// Avoid dropping the same multiple times.
						break;
					}
				}
			}
		}

		/// <summary>
		/// Check whether a constrained is present.
		/// For creating and dropping constraints, see http://support.microsoft.com/?scid=kb%3Ben-us%3B291539.
		/// </summary>
		/// <param name="constraintName">Name of the constraint.</param>
		/// <returns>
		/// 	<c>true</c> if the specified constraint name contains constraint; otherwise, <c>false</c>.
		/// </returns>
		public bool ContainsConstraint(
			string constraintName )
		{
			return ContainsConstraint( ConnectionString, constraintName );
		}

		/// <summary>
		/// Check whether a constrained is present.
		/// For creating and dropping constraints, see http://support.microsoft.com/?scid=kb%3Ben-us%3B291539.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="constraintName">Name of the constraint.</param>
		/// <returns>
		/// 	<c>true</c> if the specified connection string contains constraint; otherwise, <c>false</c>.
		/// </returns>
		public bool ContainsConstraint(
			SmartConnectionString connectionString,
			string constraintName )
		{
			SchemaConstraintInfo[] constraintInfos =
				GetConstraintInfos( connectionString );

			if ( constraintInfos == null || constraintInfos.Length <= 0 )
			{
				return false;
			}
			else
			{
				constraintName = TrimQuotes( constraintName );

				foreach ( SchemaConstraintInfo constraintInfo in constraintInfos )
				{
					if ( string.Compare( constraintInfo.ConstraintName, constraintName, true ) == 0 )
					{
						return true;
					}
				}

				return false;
			}
		}

		/// <summary>
		/// Read all the constraint names for a given table of the current connection string.
		/// </summary>
		/// <returns>
		/// Returns a list of constraint names or NULL if none.
		/// </returns>
		public SchemaConstraintInfo[] GetConstraintInfos()
		{
			return GetConstraintInfos( ConnectionString );
		}

		/// <summary>
		/// Read all the constraint names for a given table of the given connection string.
		/// </summary>
		/// <param name="connectionString">The connection string used to connect.</param>
		/// <returns>
		/// Returns a list of constraint names or NULL if none.
		/// </returns>
		public SchemaConstraintInfo[] GetConstraintInfos(
			SmartConnectionString connectionString )
		{
			using ( OleDbConnection conn = new OleDbConnection() )
			{
				conn.ConnectionString = connectionString.ConnectionString;
				conn.Open();

				List<SchemaConstraintInfo> result =
					new List<SchemaConstraintInfo>();

				// --
				// 1.)

				DataTable dt = conn.GetOleDbSchemaTable(
					OleDbSchemaGuid.Table_Constraints,
					new object[] 
					{
						null, 
						null, 
						null, 
						null, 
						null, 
						null, 
						null
					} );

				if ( dt != null && dt.Rows.Count > 0 )
				{
					foreach ( DataRow row in dt.Rows )
					{
						string constraintCatalogName;
						string constraintSchemaName;
						string constraintName;
						string constraintType;
						string tableCatalogName;
						string tableSchemaName;
						string tableName;

						DBHelper.ReadField( out constraintCatalogName, row[@"CONSTRAINT_CATALOG"] );
						DBHelper.ReadField( out constraintSchemaName, row[@"CONSTRAINT_SCHEMA"] );
						DBHelper.ReadField( out constraintName, row[@"CONSTRAINT_NAME"] );
						DBHelper.ReadField( out constraintType, row[@"CONSTRAINT_TYPE"] );
						DBHelper.ReadField( out tableCatalogName, row[@"TABLE_CATALOG"] );
						DBHelper.ReadField( out tableSchemaName, row[@"TABLE_SCHEMA"] );
						DBHelper.ReadField( out tableName, row[@"TABLE_NAME"] );

						result.Add( new SchemaConstraintInfo(
							constraintCatalogName,
							constraintSchemaName,
							constraintName,
							constraintType,
							tableCatalogName,
							tableSchemaName,
							tableName ) );
					}
				}

				// --
				// 2.)

				dt = conn.GetOleDbSchemaTable(
					OleDbSchemaGuid.Referential_Constraints,
					new object[] 
					{
						null, 
						null, 
						null
					} );

				if ( dt != null && dt.Rows.Count > 0 )
				{
					foreach ( DataRow row in dt.Rows )
					{
						string constraintCatalogName;
						string constraintSchemaName;
						string constraintName;

						DBHelper.ReadField( out constraintCatalogName, row[@"CONSTRAINT_CATALOG"] );
						DBHelper.ReadField( out constraintSchemaName, row[@"CONSTRAINT_SCHEMA"] );
						DBHelper.ReadField( out constraintName, row[@"CONSTRAINT_NAME"] );

						result.Add( new SchemaConstraintInfo(
							constraintCatalogName,
							constraintSchemaName,
							constraintName ) );
					}
				}

				// --

				if ( result.Count <= 0 )
				{
					return null;
				}
				else
				{
					// Sort by name.
					result.Sort(
						delegate( SchemaConstraintInfo a, SchemaConstraintInfo b )
						{
							if ( a.ConstraintName == b.ConstraintName )
							{
								return
									ConvertHelper.ToString( a.TableName, string.Empty ).CompareTo(
									ConvertHelper.ToString( b.TableName, string.Empty ) );
							}
							else
							{
								return
									a.ConstraintName.CompareTo(
									b.ConstraintName );
							}
						} );

					return result.ToArray();
				}
			}
		}

		/// <summary>
		/// Check whether a view is contained. in the database schema.
		/// </summary>
		/// <param name="viewName">Name of the view.</param>
		/// <returns>
		/// 	<c>true</c> if the specified view name contains view; otherwise, <c>false</c>.
		/// </returns>
		public bool ContainsView(
			string viewName )
		{
			return ContainsView( ConnectionString, viewName );
		}

		/// <summary>
		/// Check whether a view is contained. in the database schema.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="viewName">Name of the view.</param>
		/// <returns>
		/// 	<c>true</c> if the specified connection string contains view; otherwise, <c>false</c>.
		/// </returns>
		public bool ContainsView(
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
				viewName = TrimQuotes( viewName );

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
		/// Determines whether [contains table index] [the specified table name].
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="indexName">Name of the index.</param>
		/// <returns>
		/// 	<c>true</c> if [contains table index] [the specified table name]; otherwise, <c>false</c>.
		/// </returns>
		public bool ContainsTableIndex(
			string tableName,
			string indexName )
		{
			return ContainsTableIndex(
				ConnectionString,
				tableName,
				indexName );
		}

		/// <summary>
		/// Determines whether [contains table index] [the specified connection string].
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="indexName">Name of the index.</param>
		/// <returns>
		/// 	<c>true</c> if [contains table index] [the specified connection string]; otherwise, <c>false</c>.
		/// </returns>
		public bool ContainsTableIndex(
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
				tableName = TrimQuotes( tableName );
				indexName = TrimQuotes( indexName );

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
		/// Check whether a constrained is present.
		/// For creating and dropping constraints, see http://support.microsoft.com/?scid=kb%3Ben-us%3B291539.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="indexName">Name of the index.</param>
		public void DropIndex(
			string tableName,
			string indexName )
		{
			DropIndex(
				ConnectionString,
				tableName,
				indexName );
		}

		/// <summary>
		/// Check whether a constrained is present.
		/// For creating and dropping constraints, see http://support.microsoft.com/?scid=kb%3Ben-us%3B291539.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="indexName">Name of the index.</param>
		public void DropIndex(
			SmartConnectionString connectionString,
			string tableName,
			string indexName )
		{
			SchemaIndexInfo[] indexInfos =
				GetIndexInfos( connectionString );

			if ( indexInfos != null && indexInfos.Length > 0 )
			{
				indexName = TrimQuotes( indexName );
				tableName = TrimQuotes( tableName );

				foreach ( SchemaIndexInfo indexInfo in indexInfos )
				{
					if ( string.Compare( indexInfo.IndexName, indexName, true ) == 0 &&
						!string.IsNullOrEmpty( indexInfo.TableName ) )
					{
						if ( string.IsNullOrEmpty( tableName ) ||
							string.Compare( tableName, indexInfo.TableName, true ) == 0 )
						{
							ExecuteNonQuery(
								connectionString,
								string.Format(
								@"DROP INDEX [{0}]
								ON [{1}]",
								indexInfo.IndexName,
								indexInfo.TableName ) );

							// Avoid dropping the same multiple times.
							break;
						}
					}
				}
			}
		}

		/// <summary>
		/// Read all the view names of the current connection string.
		/// </summary>
		/// <returns>
		/// Returns a list of view names or NULL if none.
		/// </returns>
		public string[] GetViewNames()
		{
			return GetViewNames( ConnectionString );
		}

		/// <summary>
		/// Read all the view names of the given connection string.
		/// </summary>
		/// <param name="connectionString">The connection string used to connect.</param>
		/// <returns>
		/// Returns a list of view names or NULL if none.
		/// </returns>
		public string[] GetViewNames(
			SmartConnectionString connectionString )
		{
			using ( OleDbConnection conn = new OleDbConnection() )
			{
				conn.ConnectionString = connectionString.ConnectionString;
				conn.Open();

				DataTable dt = conn.GetOleDbSchemaTable(
					OleDbSchemaGuid.Views,
					new object[] 
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
			OleDbConnection connection,
			string tableName )
		{
			DatabaseType type =
				CSManager.CheckGet(
				new SmartConnectionString( connection.ConnectionString ) ).DatabaseType;

			switch ( type )
			{
				case DatabaseType.SqlServer:
					return string.Format(
						@"SELECT IDENT_CURRENT('{0}')",
						tableName );

				case DatabaseType.Access:
					return string.Format(
						@"SELECT @@IDENTITY" );

				default:
					throw new ArgumentException(
						LocalizationHelper.Format(
						Resources.Str_ZetaLib_Core_Data_AdoNetOleDBHelper_01,
						LocalizationHelper.CreatePair( @"Type", type ) ) );
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}