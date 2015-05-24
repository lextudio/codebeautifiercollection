namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Data;
	using System.Data.OracleClient;
	using System.Runtime.InteropServices;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Class that simplifies the adding/updating of recordsets with ADO.NET.
	/// See the description of the base class for usage details
	/// </summary>
	[ComVisible( false )]
	public sealed class AdoNetOracleUpdater :
		AdoNetBaseUpdater<
		OracleCommand,
		OracleCommandBuilder,
		OracleConnection,
		OracleDataAdapter,
		OracleParameter,
		AdoNetOracleParamCollection>
	{
		#region Constructors.
		// ------------------------------------------------------------------

		/// <summary>
		/// Default constructor.
		/// Uses the default connection string.
		/// </summary>
		public AdoNetOracleUpdater()
		{
		}

		/// <summary>
		/// Constructor that executes a SQL query.
		/// Uses the default connection string.
		/// </summary>
		/// <param name="query">The SQL query to execute.</param>
		public AdoNetOracleUpdater( 
			string query )
		{
			Open( query );
		}

		/// <summary>
		/// Constructor that executes a SQL query.
		/// </summary>
		/// <param name="connectionString">The connection string to use.</param>
		/// <param name="query">The SQL query to execute.</param>
		public AdoNetOracleUpdater( 
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
		public AdoNetOracleUpdater( 
			string spName,
			AdoNetOracleParamCollection spParameters )
		{
			Open( spName, spParameters );
		}

		/// <summary>
		/// Constructor that executes a SQL stored procedure.
		/// </summary>
		/// <param name="connectionString">The connection string to use.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		public AdoNetOracleUpdater( 
			SmartConnectionString connectionString, 
			string spName,
			AdoNetOracleParamCollection spParameters )
		{
			Open( connectionString, spName, spParameters );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Open routines.
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

			AdoNetOracleHelper.TraceSql( query, true, dataSet );
			try
			{
				/*tableName = DetectTableName( query );*/
				query = AdoNetOleDBHelper.CSManager.CheckGet(
					connectionString ).ReplaceQueryBoolean( query );

				connection = new OracleConnection( connectionString.ConnectionString );
				dataAdapter = new OracleDataAdapter( query, connection );

				commandBuilder = new OracleCommandBuilder( dataAdapter );
				/*CommandBuilder.QuotePrefix = "[";
				CommandBuilder.QuoteSuffix = "]";*/

				dataSet = new DataSet();
				dataAdapter.Fill( dataSet );
			}
			catch ( Exception x )
			{
				AdoNetOracleHelper.TraceSqlError( query, x );
				throw;
			}
			AdoNetOracleHelper.TraceSql( query, false, dataSet );
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
			AdoNetOracleParamCollection spParameters )
		{
			this.connectionString = connectionString;
			connection = new OracleConnection( connectionString.ConnectionString );

			spName = AdoNetOleDBHelper.CSManager.CheckGet(
				connectionString ).ReplaceQueryBoolean( spName );

			command = new OracleCommand( spName, connection );
			command.CommandType = AdoNetBaseHelper<OracleCommand, OracleCommandBuilder, OracleConnection, OracleDataAdapter, OracleParameter, AdoNetOracleParamCollection>.
				DetectCommandType( spName );

			// Apply command timeouts, if any.
			AdoNetOracleHelper.Current.CheckSetCommandTimeout( command );

			if ( spParameters!=null )
			{
				foreach ( OracleParameter parameter in spParameters )
				{
					command.Parameters.Add( parameter );
				}
			}

			connection.Open();
			dataAdapter = new OracleDataAdapter( command );

			CommandBuilder = new OracleCommandBuilder( dataAdapter );
			/*CommandBuilder.QuotePrefix = "[";
			CommandBuilder.QuoteSuffix = "]";*/

			// Apply command timeouts, if any.
			AdoNetOracleHelper.Current.CheckSetCommandTimeout( dataAdapter.DeleteCommand );
			AdoNetOracleHelper.Current.CheckSetCommandTimeout( dataAdapter.InsertCommand );
			AdoNetOracleHelper.Current.CheckSetCommandTimeout( dataAdapter.SelectCommand );
			AdoNetOracleHelper.Current.CheckSetCommandTimeout( dataAdapter.UpdateCommand );

			dataSet = new DataSet();
			AdoNetOracleHelper.TraceSql( command, true, dataSet );
			try
			{
				dataAdapter.Fill( dataSet );
			}
			catch ( Exception x )
			{
				AdoNetOracleHelper.TraceSqlError( command, x );
				throw;
			}
			AdoNetOracleHelper.TraceSql( command, false, dataSet );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Identity routines.
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
			OracleConnection connection,
			string tableName )
		{
			return AdoNetOracleHelper.Current.GetIdentity(
				connection, 
				tableName );
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
				return null;
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
				return null;
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}