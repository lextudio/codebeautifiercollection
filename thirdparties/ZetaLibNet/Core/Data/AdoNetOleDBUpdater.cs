namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Data;
	using System.Data.OleDb;
	using System.Runtime.InteropServices;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Class that simplifies the adding/updating of recordsets with ADO.NET.
	/// See the description of the base class for usage details
	/// </summary>
	[ComVisible( false )]
	public sealed class AdoNetOleDBUpdater :
		AdoNetBaseUpdater<
		OleDbCommand,
		OleDbCommandBuilder,
		OleDbConnection,
		OleDbDataAdapter,
		OleDbParameter,
		AdoNetOleDBParamCollection>
	{
		#region Constructors.
		// ------------------------------------------------------------------

		/// <summary>
		/// Default constructor.
		/// Uses the default connection string.
		/// </summary>
		public AdoNetOleDBUpdater()
		{
		}

		/// <summary>
		/// Constructor that executes a SQL query.
		/// Uses the default connection string.
		/// </summary>
		/// <param name="query">The SQL query to execute.</param>
		public AdoNetOleDBUpdater(
			string query )
		{
			Open( query );
		}

		/// <summary>
		/// Constructor that executes a SQL query.
		/// </summary>
		/// <param name="connectionString">The connection string to use.</param>
		/// <param name="query">The SQL query to execute.</param>
		public AdoNetOleDBUpdater(
			SmartConnectionString connectionString,
			string query )
		{
			Open( connectionString, query );
		}

		/// <summary>
		/// Constructor that executes a SQL stored procedure.
		/// Uses the default connection string.
		/// </summary>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		public AdoNetOleDBUpdater(
			string spName,
			AdoNetOleDBParamCollection spParameters )
		{
			Open( spName, spParameters );
		}

		/// <summary>
		/// Constructor that executes a SQL stored procedure.
		/// </summary>
		/// <param name="connectionString">The connection string to use.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		public AdoNetOleDBUpdater(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters )
		{
			Open( connectionString, spName, spParameters );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Open methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Open the given SQL query.
		/// </summary>
		/// <param name="connectionString">The connection string to use.</param>
		/// <param name="query">The SQL query to execute.</param>
		public override void Open(
			SmartConnectionString connectionString,
			string query )
		{
			// Remember query, for logging error in Update().
			this.connectionString = connectionString;
			this.query = query;

			AdoNetOleDBHelper.TraceSql( query, true, dataSet );
			try
			{
				/*tableName = DetectTableName( query );*/
				query = AdoNetOleDBHelper.CSManager.CheckGet(
					connectionString ).ReplaceQueryBoolean( query );

				connection = new OleDbConnection( connectionString.ConnectionString );
				dataAdapter = new OleDbDataAdapter( query, connection );

				dataAdapter.RowUpdated +=
					new OleDbRowUpdatedEventHandler(
					dataAdapter_RowUpdated );

				commandBuilder = new OleDbCommandBuilder( dataAdapter );
				commandBuilder.QuotePrefix = QuotePrefix;
				commandBuilder.QuoteSuffix = QuoteSuffix;

				dataSet = new DataSet();
				dataAdapter.Fill( dataSet );
			}
			catch ( Exception x )
			{
				AdoNetOleDBHelper.TraceSqlError( query, x );
				throw;
			}
			AdoNetOleDBHelper.TraceSql( query, false, dataSet );
		}

		/// <summary>
		/// Opens the given SQL stored procedure.
		/// </summary>
		/// <param name="connectionString">The connection string to use.</param>
		/// <param name="spName"></param>
		/// <param name="spParameters"></param>
		public override void Open(
			SmartConnectionString connectionString,
			string spName,
			AdoNetOleDBParamCollection spParameters )
		{
			this.connectionString = connectionString;
			connection = new OleDbConnection( connectionString.ConnectionString );

			spName = AdoNetOleDBHelper.CSManager.CheckGet(
				connectionString ).ReplaceQueryBoolean( spName );

			command = new OleDbCommand( spName, connection );
			command.CommandType = AdoNetOleDBHelper.DetectCommandType( spName );

			// Apply command timeouts, if any.
			AdoNetOleDBHelper.Current.CheckSetCommandTimeout( command );

			if ( spParameters != null )
			{
				foreach ( OleDbParameter parameter in spParameters )
				{
					command.Parameters.Add( parameter );
				}
			}

			connection.Open();
			dataAdapter = new OleDbDataAdapter( command );

			dataAdapter.RowUpdated +=
				new OleDbRowUpdatedEventHandler(
				dataAdapter_RowUpdated );

			commandBuilder = new OleDbCommandBuilder( dataAdapter );
			commandBuilder.QuotePrefix = QuotePrefix;
			commandBuilder.QuoteSuffix = QuoteSuffix;

			// Apply command timeouts, if any.
			AdoNetOleDBHelper.Current.CheckSetCommandTimeout( dataAdapter.DeleteCommand );
			AdoNetOleDBHelper.Current.CheckSetCommandTimeout( dataAdapter.InsertCommand );
			AdoNetOleDBHelper.Current.CheckSetCommandTimeout( dataAdapter.SelectCommand );
			AdoNetOleDBHelper.Current.CheckSetCommandTimeout( dataAdapter.UpdateCommand );

			dataSet = new DataSet();
			AdoNetOleDBHelper.TraceSql( command, true, dataSet );
			try
			{
				dataAdapter.Fill( dataSet );
			}
			catch ( Exception x )
			{
				AdoNetOleDBHelper.TraceSqlError( command, x );
				throw;
			}
			AdoNetOleDBHelper.TraceSql( command, false, dataSet );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Identity methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Detectes the identity. Call this function after
		/// the Update() function returned successfully.
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="tableName">The table name, as a hint,
		/// if auto-detection of the table name fails. This parameter usually
		/// is required if the SQL query that was used to open the database,
		/// was an EXECUTE of a stored procedure.</param>
		/// <returns>Returns the identity, if any.</returns>
		public override int GetIdentity(
			OleDbConnection connection,
			string tableName )
		{
			return AdoNetOleDBHelper.Current.GetIdentity(
				connection,
				tableName );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Updating methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// See http://www.codeproject.com/cs/database/relationaladonet.asp.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Data.OleDb.OleDbRowUpdatedEventArgs"/>
		/// instance containing the event data.</param>
		private void dataAdapter_RowUpdated(
			object sender,
			OleDbRowUpdatedEventArgs e )
		{
			AdoNetOleDBHelper.DatabaseType type =
				AdoNetOleDBHelper.CSManager.CheckGet(
				new SmartConnectionString( Connection.ConnectionString ) ).DatabaseType;

			if ( identityControlForNextUpdate == IdentityControl.Get )
			{
				// Special handling for MS Access.
				if ( type == AdoNetOleDBHelper.DatabaseType.Access )
				{
					OleDbCommand cmd = new OleDbCommand(
						@"SELECT @@IDENTITY",
						e.Command.Connection );

					object o = cmd.ExecuteScalar();

					lastIdentityValue = 0;
					if ( o != null && o != DBNull.Value )
					{
						lastIdentityValue = Convert.ToInt32( o );
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

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}