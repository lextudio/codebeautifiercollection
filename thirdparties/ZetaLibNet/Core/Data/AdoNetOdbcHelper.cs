namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Data.Odbc;
	using ZetaLib.Core.Properties;
	using ZetaLib.Core.Localization;
	using System.Runtime.InteropServices;

	// ----------------------------------------------------------------------
	#endregion

	////////////////////////////////////////////////////////////////////////////

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
	public sealed class AdoNetOdbcHelper :
		AdoNetBaseHelper<
		OdbcCommand,
		OdbcCommandBuilder,
		OdbcConnection,
		OdbcDataAdapter,
		OdbcParameter,
		AdoNetOdbcParamCollection>
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
		public static AdoNetOdbcHelper Current
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
							current = new AdoNetOdbcHelper();
						}
					}
				}

				return current;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private static volatile AdoNetOdbcHelper current = null;

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
			OdbcConnection connection,
			string tableName )
		{
			DatabaseType type = CSManager.CheckGet(
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
						Resources.Str_ZetaLib_Core_Data_AdoNetOdbcHelper_01,
						LocalizationHelper.CreatePair( @"Type", type ) ) );
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	////////////////////////////////////////////////////////////////////////////
}