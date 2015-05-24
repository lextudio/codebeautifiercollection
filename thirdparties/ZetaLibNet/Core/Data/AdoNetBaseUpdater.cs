namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Data;
	using System.Text.RegularExpressions;
	using System.Data.Common;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Class that simplifies the adding/updating of recordsets with ADO.NET.
	/// Use one of the derivation of this class like in the following description:
	/// 
	/// Usage:
	/// 
	///		using ( AdoNetOleDBUpdater upd = new AdoNetOleDBUpdater( Settings.HostDBString ) )
	///		{
	///			DataRow row;
	///			if ( wantToAddNew )
	///			{
	/// 			upd.Open( "SELECT TOP 1 * FROM MyTable" );
	/// 			row = upd.AddNewRow();
	///			}
	///			else
	///			{
	/// 			upd.Open( "SELECT TOP 1 * FROM MyTable WHERE ID=" + someIDOfMine );
	/// 			row = upd.Row;
	///			}
	///		
	///			row["xxx"] = yyy;
	///			row["xxx"] = yyy;
	///			...
	/// 
	///			int myID = upd.Update();
	///		}
	///		
	/// </summary>
	/// <remarks>Parameters in the application configuration file (e.g. "web.config)":
	/// - "connectionString": The connection string for connecting to the database.
	/// - "traceSqlEnabled": Turn tracing of SQL statements to LOG4NET on/off.
	/// - "cacheSqlEnabled": Turn caching of SQL statements and their result inside
	/// the ASP.NET web cache on/off.</remarks>
	public abstract class AdoNetBaseUpdater<
		TCommand,
		TCommandBuilder,
		TConnection,
		TDataAdapter,
		TParameter,
		TParamCollection> :
		IDisposable
		where TCommand : DbCommand, new()
		where TCommandBuilder : DbCommandBuilder, new()
		where TConnection : DbConnection, new()
		where TDataAdapter : DbDataAdapter, new()
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
			#region Enum members.

			/// <summary>
			/// Read the identity field after the table got updated.
			/// </summary>
			Get,

			/// <summary>
			/// Don't read the identity field after the update.
			/// </summary>
			DontGet

			#endregion
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
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
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
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
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
		/// <returns>
		/// Returns the new row. The row is now added yet.
		/// You must call AddRow( row ) by yourself.
		/// </returns>
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
		/// <returns>
		/// Returns the new row. The row is now added yet.
		/// You must call AddRow( row ) by yourself.
		/// </returns>
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

		// ------------------------------------------------------------------
		#endregion

		#region Updating methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Write any changes to database.
		/// </summary>
		/// <param name="identityControl">Specify whether you want to try to get the
		/// identity value (if any) of the inserted row.</param>
		/// <returns>
		/// Returns the identity (if requested and possible)
		/// or 0 if unavailable or not retrievable.
		/// </returns>
		public int Update(
			IdentityControl identityControl )
		{
			return Update(
				identityControl,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Write any changes to database.
		/// </summary>
		/// <param name="hintTableName">If the table name could not be automatically
		/// detected, use this parameter to specify the table name
		/// to get the identity from (e.g. useful when this class was filled
		/// by a stored procedure.</param>
		/// <param name="identityControl">Specify whether you want to try to get the
		/// identity value (if any) of the inserted row.</param>
		/// <returns>
		/// Returns the identity (if requested and possible)
		/// or 0 if unavailable or not retrievable.
		/// </returns>
		public int Update(
			string hintTableName,
			IdentityControl identityControl )
		{
			return Update(
				identityControl,
				hintTableName,
				null as AdoNetCacheItemInformation );
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
		/// <returns>
		/// Returns the identity (if requested and possible)
		/// or 0 if unavailable or not retrievable.
		/// </returns>
		public int Update(
			IdentityControl identityControl,
			string hintTableName )
		{
			return Update(
				identityControl,
				hintTableName,
				null as AdoNetCacheItemInformation );
		}

		/// <summary>
		/// Write any changes to database.
		/// </summary>
		/// <param name="cacheItemInfo">Specifies how the cache should
		/// be handled.</param>
		/// <returns>
		/// Returns the identity (if requested and possible)
		/// or 0 if unavailable or not retrievable.
		/// </returns>
		public int Update(
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return Update(
				IdentityControl.Get,
				cacheItemInfo );
		}

		/// <summary>
		/// Write any changes to database.
		/// </summary>
		/// <param name="hintTableName">If the table name could not be automatically
		/// detected, use this parameter to specify the table name
		/// to get the identity from (e.g. useful when this class was filled</param>
		/// <param name="cacheItemInfo">Specifies how the cache should
		/// be handled.</param>
		/// <returns>
		/// Returns the identity (if requested and possible)
		/// or 0 if unavailable or not retrievable.
		/// </returns>
		public int Update(
			string hintTableName,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return Update(
				IdentityControl.Get,
				hintTableName,
				cacheItemInfo );
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
		/// <param name="cacheItemInfo">Specifies how the cache should
		/// be handled.</param>
		/// <returns>
		/// Returns the identity (if requested and possible)
		/// or 0 if unavailable or not retrievable.
		/// </returns>
		public int Update(
			IdentityControl identityControl,
			string hintTableName,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			try
			{
				this.identityControlForNextUpdate = identityControl;
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
			if ( cacheItemInfo != null &&
				cacheItemInfo.Usage != CacheUsage.DontClear )
			{
				AdoNetCacheManager.Current.RemoveAll( cacheItemInfo );
			}

			if ( identityControl == IdentityControl.Get )
			{
				AdoNetOleDBHelper.DatabaseType type =
					AdoNetOleDBHelper.CSManager.CheckGet(
					new SmartConnectionString( Connection.ConnectionString ) ).DatabaseType;

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
		/// <param name="cacheItemInfo">Specifies how the cache should
		/// be handled.</param>
		/// <returns>
		/// Returns the identity (if requested and possible)
		/// or 0 if unavailable or not retrievable.
		/// </returns>
		public int Update(
			IdentityControl identityControl,
			AdoNetCacheItemInformation cacheItemInfo )
		{
			return Update(
				identityControl,
				tableName,
				cacheItemInfo );
		}

		/// <summary>
		/// Write any changes to database.
		/// </summary>
		/// <returns>
		/// Returns the identity (if requested and possible)
		/// or 0 if unavailable or not retrievable.
		/// </returns>
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
		/// <returns>
		/// Returns the identity (if requested and possible)
		/// or 0 if unavailable or not retrievable.
		/// </returns>
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
		/// <param name="id">The id.</param>
		/// <param name="hintTableName">If the table name could not be automatically
		/// detected, use this parameter to specify the table name
		/// to get the identity from (e.g. useful when this class was filled
		/// by a stored procedure.</param>
		public void Update(
			ref int id,
			string hintTableName )
		{
			if ( id <= 0 )
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
		/// <param name="connection">The connection.</param>
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
		/// <param name="connection">The connection.</param>
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
		/// Call after Open().
		/// Set to zero for maximum allowed size.
		/// Set to 1 to disable batch updating.
		/// http://davidhayden.com/blog/dave/archive/2006/01/05/2665.aspx
		/// </summary>
		/// <value>The size of the update batch.</value>
		public int UpdateBatchSize
		{
			get
			{
				return DataAdapter.UpdateBatchSize;
			}
			set
			{
				DataAdapter.UpdateBatchSize = value;
			}
		}

		/// <summary>
		/// Access a ROW of the current queried table (if any).
		/// Only works if the Open function was called or
		/// a query was specified in the constructor.
		/// </summary>
		/// <value></value>
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
		/// <value>The table.</value>
		public DataTable Table
		{
			get
			{
				if ( dataSet == null ||
					dataSet.Tables == null ||
					dataSet.Tables.Count <= 0 )
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
		/// <value>The row.</value>
		public DataRow Row
		{
			get
			{
				DataTable table = Table;

				if ( table == null || 
					table.Rows == null || 
					table.Rows.Count <= 0 )
				{
					return null;
				}
				else
				{
					return table.Rows[0];
				}
			}
		}

		/// <summary>
		/// Check whether a row is present.
		/// </summary>
		/// <value><c>true</c> if this instance has row; otherwise, <c>false</c>.</value>
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
		/// <value>The rows.</value>
		public DataRowCollection Rows
		{
			get
			{
				DataTable table = Table;

				if ( table == null || table.Rows == null )
				{
					return null;
				}
				else
				{
					return table.Rows;
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
		/// <value>The connection string.</value>
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
		/// <value>The connection.</value>
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
		/// <value>The data adapter.</value>
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
		/// <value>The data set.</value>
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
		/// <value>The command builder.</value>
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
			AdoNetBaseHelper<
			TCommand, 
			TCommandBuilder, 
			TConnection, 
			TDataAdapter, 
			TParameter, 
			TParamCollection>.InitializeConnectionString();

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

		/// <summary>
		/// Speed up when using MS Access and getting identity.
		/// </summary>
		protected IdentityControl identityControlForNextUpdate =
			IdentityControl.Get;

		// ------------------------------------------------------------------
		#endregion

		#region Private helper.
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
		/// Removes the quotes, if any.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		private string TrimQuotes(
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

		// ------------------------------------------------------------------
		#endregion

		#region Tracing methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Trace helper.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <param name="before">if set to <c>true</c> [before].</param>
		private void TraceUpdateSql(
			string query,
			bool before )
		{
			AdoNetBaseHelper<TCommand, TCommandBuilder, TConnection, TDataAdapter, TParameter, TParamCollection>.TraceSql(
				string.Format(
				@"UPDATE of row '{0}' based upon query",
				AdoNetBaseHelper<TCommand, TCommandBuilder, TConnection, TDataAdapter, TParameter, TParamCollection>.DumpDataRow( Row ) ),
				query,
				before,
				dataSet );
		}

		/// <summary>
		/// Trace helper.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <param name="x">The x.</param>
		private void TraceUpdateSqlError(
			string query,
			Exception x )
		{
			AdoNetBaseHelper<TCommand, TCommandBuilder, TConnection, TDataAdapter, TParameter, TParamCollection>.TraceSqlError(
				string.Format(
				@"UPDATE of row '{0}' based upon query",
				AdoNetBaseHelper<TCommand, TCommandBuilder, TConnection, TDataAdapter, TParameter, TParamCollection>.DumpDataRow( Row ) ),
				query,
				x );
		}

		/// <summary>
		/// Trace helper.
		/// </summary>
		/// <param name="cmd">The CMD.</param>
		/// <param name="before">if set to <c>true</c> [before].</param>
		private void TraceUpdateSql(
			TCommand cmd,
			bool before )
		{
			AdoNetBaseHelper<TCommand, TCommandBuilder, TConnection, TDataAdapter, TParameter, TParamCollection>.
				TraceSql(
				string.Format(
				@"UPDATE of row '{0}' based upon SP",
				AdoNetBaseHelper<TCommand, TCommandBuilder, TConnection, TDataAdapter, TParameter, TParamCollection>.
				DumpDataRow( Row ) ),
				cmd,
				before,
				dataSet );
		}

		/// <summary>
		/// Trace helper.
		/// </summary>
		/// <param name="cmd">The CMD.</param>
		/// <param name="x">The x.</param>
		private void TraceUpdateSqlError(
			TCommand cmd,
			Exception x )
		{
			string s = string.Format(
				@"UPDATE of row '{0}' based upon SP",
				AdoNetBaseHelper<TCommand, TCommandBuilder, TConnection, TDataAdapter, TParameter, TParamCollection>.
				DumpDataRow( Row ) );

			AdoNetBaseHelper<TCommand, TCommandBuilder, TConnection, TDataAdapter, TParameter, TParamCollection>.
				TraceSqlError(
				s,
				cmd,
				x );
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}