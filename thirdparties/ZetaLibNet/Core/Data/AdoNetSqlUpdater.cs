namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Data;
	using System.Data.SqlClient;
	using System.Runtime.InteropServices;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Class that simplifies the adding/updating of recordsets with ADO.NET.
	/// See the description of the base class for usage details
	/// </summary>
	[ComVisible( false )]
	public sealed class AdoNetSqlUpdater :
		AdoNetBaseUpdater<
		SqlCommand,
		SqlCommandBuilder,
		SqlConnection,
		SqlDataAdapter,
		SqlParameter,
		AdoNetSqlParamCollection>
	{
		#region Constructors.
		// ------------------------------------------------------------------

		/// <summary>
		/// Default constructor.
		/// Uses the default connection string.
		/// </summary>
		public AdoNetSqlUpdater()
		{
		}

		/// <summary>
		/// Constructor that executes a SQL query.
		/// Uses the default connection string.
		/// </summary>
		/// <param name="query">The SQL query to execute.</param>
		public AdoNetSqlUpdater(
			string query )
		{
			Open( query );
		}

		/// <summary>
		/// Constructor that executes a SQL query.
		/// </summary>
		/// <param name="connectionString">The connection string to use.</param>
		/// <param name="query">The SQL query to execute.</param>
		public AdoNetSqlUpdater(
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
		public AdoNetSqlUpdater(
			string spName,
			AdoNetSqlParamCollection spParameters )
		{
			Open( spName, spParameters );
		}

		/// <summary>
		/// Constructor that executes a SQL stored procedure.
		/// </summary>
		/// <param name="connectionString">The connection string to use.</param>
		/// <param name="spName">Name of the sp.</param>
		/// <param name="spParameters">The sp parameters.</param>
		public AdoNetSqlUpdater(
			SmartConnectionString connectionString,
			string spName,
			AdoNetSqlParamCollection spParameters )
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

			AdoNetSqlHelper.TraceSql( query, true, dataSet );
			try
			{
				/*tableName = DetectTableName( query );*/

				query = AdoNetOleDBHelper.CSManager.CheckGet(
					connectionString ).ReplaceQueryBoolean( query );

				connection = new SqlConnection( connectionString.ConnectionString );
				dataAdapter = new SqlDataAdapter( query, connection );

				commandBuilder = new SqlCommandBuilder( dataAdapter );
				commandBuilder.QuotePrefix = AdoNetSqlHelper.Current.QuotePrefix;
				commandBuilder.QuoteSuffix = AdoNetSqlHelper.Current.QuoteSuffix;

				dataSet = new DataSet();
				dataAdapter.Fill( dataSet );
			}
			catch ( Exception x )
			{
				AdoNetSqlHelper.TraceSqlError( query, x );
				throw;
			}
			AdoNetSqlHelper.TraceSql( query, false, dataSet );
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
			AdoNetSqlParamCollection spParameters )
		{
			this.connectionString = connectionString;
			connection = new SqlConnection( connectionString.ConnectionString );

			spName = AdoNetOleDBHelper.CSManager.CheckGet(
				connectionString ).ReplaceQueryBoolean( spName );

			command = new SqlCommand( spName, connection );
			command.CommandType = AdoNetSqlHelper.DetectCommandType( spName );

			// Apply command timeouts, if any.
			AdoNetSqlHelper.Current.CheckSetCommandTimeout( command );

			if ( spParameters != null )
			{
				foreach ( SqlParameter parameter in spParameters )
				{
					command.Parameters.Add( parameter );
				}
			}

			connection.Open();
			dataAdapter = new SqlDataAdapter( command );

			commandBuilder = new SqlCommandBuilder( dataAdapter );
			commandBuilder.QuotePrefix = AdoNetSqlHelper.Current.QuotePrefix;
			commandBuilder.QuoteSuffix = AdoNetSqlHelper.Current.QuoteSuffix;

			// Apply command timeouts, if any.
			AdoNetSqlHelper.Current.CheckSetCommandTimeout( dataAdapter.DeleteCommand );
			AdoNetSqlHelper.Current.CheckSetCommandTimeout( dataAdapter.InsertCommand );
			AdoNetSqlHelper.Current.CheckSetCommandTimeout( dataAdapter.SelectCommand );
			AdoNetSqlHelper.Current.CheckSetCommandTimeout( dataAdapter.UpdateCommand );

			dataSet = new DataSet();
			AdoNetSqlHelper.TraceSql( command, true, dataSet );
			try
			{
				dataAdapter.Fill( dataSet );
			}
			catch ( Exception x )
			{
				AdoNetSqlHelper.TraceSqlError( command, x );
				throw;
			}
			AdoNetSqlHelper.TraceSql( command, false, dataSet );
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
			SqlConnection connection,
			string tableName )
		{
			return AdoNetSqlHelper.Current.GetIdentity(
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