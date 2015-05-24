namespace ZetaLib.Core
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Configuration;
	using System.Globalization;
	using System.Diagnostics;
	using System.IO;
	using System.Net;
	using System.Threading;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Xml;
	using System.Net.Mail;
	using ZetaLib.Core.Base;
	using ZetaLib.Core.Common;
	using System.Reflection;
	using System.Collections.Generic;
	using ZetaLib.Core.Data;
	using ZetaLib.Core.Caching;
	using ZetaLib.Core.Logging;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Central class for managing all configuration aspects of the library.
	/// </summary>
	public class LibraryConfiguration :
		LibraryConfigurationBase,
		ILibraryConfiguration
	{
		#region Static routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Singleton access to the library configuratin.
		/// </summary>
		/// <value>The current.</value>
		public static LibraryConfiguration Current
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
							// Please not that the following line does NOT return 
							// a fresh instance every time being called, but 
							// rather caches and return the same instance in 
							// subsequent calls.
							XmlNode section = ConfigurationManager.GetSection(
								@"zetaLibCore" ) as XmlNode;

							LibraryConfiguration result =
								new LibraryConfiguration();
							result.Initialize();
							result.LoadFromXml( section );

							current = result;
						}
					}
				}

				return current;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Initialize this library.
		/// Please ensure to call this function once at the very start of your
		/// application, before doing any logging functions. A good place
		/// would be inside the Main() method or GLOBAL.ASAX file.
		/// </summary>
		public void Initialize()
		{
			if ( libraryInitialisator == null )
			{
				libraryInitialisator = new object();

				AdoNetCacheManager.Current.Initialize();
			}
		}

		/// <summary>
		/// Loads this class form the given configuration node.
		/// </summary>
		/// <param name="node">The node.</param>
		public void LoadFromXml(
			XmlNode node )
		{
			if ( node != null )
			{
				XmlHelper.ReadAttribute(
					out applicationRegistryKeyName,
					node.Attributes[@"applicationRegistryKeyName"] );
				XmlHelper.ReadAttribute(
					out administratorEMailAddress,
					node.Attributes[@"administratorEMailAddress"] );
				XmlHelper.ReadAttribute(
					out disableLoggingPasswordProtection,
					node.Attributes[@"disableLoggingPasswordProtection"],
					false );
				XmlHelper.ReadAttribute(
					out smtpServer,
					node.Attributes[@"smtpServer"] );
				XmlHelper.ReadAttribute(
					out smtpServerPort,
					node.Attributes[@"smtpServerPort"] );
				XmlHelper.ReadAttribute(
					out smtpServerUserName,
					node.Attributes[@"smtpServerUserName"] );
				XmlHelper.ReadAttribute(
					out smtpServerPassword,
					node.Attributes[@"smtpServerPassword"] );

				string s;
				XmlHelper.ReadAttribute(
					out s,
					node.Attributes[@"deletedFilesFolderPath"] );

				if ( !string.IsNullOrEmpty( s ) )
				{
					deletedFilesFolderPath = new DirectoryInfo( s );
				}

				// --

				XmlNode databaseNode =
					node.SelectSingleNode( @"database" );

				if ( databaseNode != null )
				{
					string connectionStringString = null;
					int commandTimeoutSeconds = 0;
					bool traceSqlEnabled = false;
					string cacheSqlBehaviorText = null;
					DatabaseConfiguration.DatabaseCacheSqlBehavior
						cacheSqlBehavior =
						DatabaseConfiguration.DatabaseCacheSqlBehavior.Partially;

					XmlHelper.ReadAttribute(
						out connectionStringString,
						databaseNode.Attributes[@"connectionString"] );
					XmlHelper.ReadAttribute(
						out commandTimeoutSeconds,
						databaseNode.Attributes[@"commandTimeoutSeconds"] );
					XmlHelper.ReadAttribute(
						out cacheSqlBehaviorText,
						databaseNode.Attributes[@"cacheSqlBehavior"] );

					if ( !string.IsNullOrEmpty( cacheSqlBehaviorText ) )
					{
						cacheSqlBehavior =
							(DatabaseConfiguration.DatabaseCacheSqlBehavior)
							Enum.Parse(
							typeof( DatabaseConfiguration.DatabaseCacheSqlBehavior ),
							cacheSqlBehaviorText,
							true );
					}

					XmlHelper.ReadAttribute(
						out traceSqlEnabled,
						databaseNode.Attributes[@"traceSqlEnabled"] );

					database = new DatabaseConfiguration(
						connectionStringString,
						commandTimeoutSeconds,
						cacheSqlBehavior,
						traceSqlEnabled );
				}

				// --

				XmlNode webNode =
					node.SelectSingleNode( @"web" );

				if ( webNode != null )
				{
					bool useServerSideViewState = false;
					string replaceTildeFallback = null;
					string replaceTildeCompleteFallback = null;
					bool useCustomErrors = false;

					XmlHelper.ReadAttribute(
						out useServerSideViewState,
						webNode.Attributes[@"useServerSideViewState"] );
					XmlHelper.ReadAttribute(
						out replaceTildeFallback,
						webNode.Attributes[@"replaceTildeFallback"] );
					XmlHelper.ReadAttribute(
						out replaceTildeCompleteFallback,
						webNode.Attributes[@"replaceTildeCompleteFallback"] );
					XmlHelper.ReadAttribute(
						out useCustomErrors,
						webNode.Attributes[@"useCustomErrors"] );

					web = new WebConfiguration(
						useServerSideViewState,
						replaceTildeFallback,
						replaceTildeCompleteFallback,
						useCustomErrors );
				}

				// --

				/*
				<webProxy
					address=""
					bypassProxyOnLocal="" >
					<credentials 
						domain=""
						userName=""
						password="" />
					<bypassList>
						<bypass rx="" />
						<bypass rx="" />
						<bypass rx="" />
						<bypass rx="" />
					</bypassList>
				</webProxy>
				*/

				XmlNode webProxyNode =
					node.SelectSingleNode( @"webProxy" );

				if ( webProxyNode != null )
				{
					bool hasEnabled = false;
					bool enabled = false;
					string address = null;
					bool hasBypassProxyOnLocal = false;
					bool bypassProxyOnLocal = false;

					hasEnabled =
						webProxyNode.Attributes[@"enabled"] != null;
					XmlHelper.ReadAttribute(
						out enabled,
						webProxyNode.Attributes[@"enabled"] );
					XmlHelper.ReadAttribute(
						out address,
						webProxyNode.Attributes[@"address"] );
					hasBypassProxyOnLocal =
						webProxyNode.Attributes[@"bypassProxyOnLocal"] != null;
					XmlHelper.ReadAttribute(
						out bypassProxyOnLocal,
						webProxyNode.Attributes[@"bypassProxyOnLocal"] );

					// If no address is given, ignore everything else.
					if ( hasEnabled && enabled &&
						!string.IsNullOrEmpty( address ) )
					{
						WebProxy webProxy = new WebProxy();
						webProxy.Address = new Uri( address );

						if ( hasBypassProxyOnLocal )
						{
							webProxy.BypassProxyOnLocal = bypassProxyOnLocal;
						}

						XmlNode credentialsNode =
							webProxyNode.SelectSingleNode( @"credentials" );

						if ( credentialsNode != null )
						{
							string domain = null;
							string userName = null;
							string password = null;

							XmlHelper.ReadAttribute(
								out domain,
								credentialsNode.Attributes[@"domain"] );
							XmlHelper.ReadAttribute(
								out userName,
								credentialsNode.Attributes[@"userName"] );
							XmlHelper.ReadAttribute(
								out password,
								credentialsNode.Attributes[@"password"] );

							// If no user name is given, ignore the credentials.
							if ( userName != null && userName.Length > 0 )
							{
								NetworkCredential credentials =
									new NetworkCredential();

								credentials.UserName = userName;
								credentials.Password = password;
								credentials.Domain = domain;

								webProxy.Credentials = credentials;
							}
						}

						XmlNodeList bypassNodes =
							webProxyNode.SelectNodes( @"bypassList/bypass" );

						if ( bypassNodes != null && bypassNodes.Count > 0 )
						{
							List<string> bypassRXs = new List<string>();

							foreach ( XmlNode bypassNode in bypassNodes )
							{
								string rx = null;

								XmlHelper.ReadAttribute(
									out rx,
									bypassNode.Attributes[@"rx"] );

								if ( rx != null && rx.Length > 0 )
								{
									bypassRXs.Add( rx );
								}
							}

							if ( bypassRXs.Count > 0 )
							{
								webProxy.BypassList = bypassRXs.ToArray();
							}
						}

						this.webProxy = webProxy;
					}
				}
			}
		}

		/// <summary>
		/// Central point to send an e-mail message.
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="body">The body.</param>
		public void SendEMailMessage(
			string from,
			string to,
			string subject,
			string body )
		{
			MailMessage message = new MailMessage();

			message.IsBodyHtml = false;
			message.From = new MailAddress( from );
			message.Subject = subject;
			message.Body = body;
			message.To.Add( new MailAddress( to ) );

			SendEMailMessage( message );
		}

		/// <summary>
		/// Central point to send an e-mail message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void SendEMailMessage(
			MailMessage message )
		{
			string smtpServer =
				ZetaLib.Core.LibraryConfiguration.Current.SmtpServer;

			try
			{
				LogCentral.Current.LogDebug(
					string.Format(
					@"Sending e-mail message. SMTP server: '{0}' (IP '{1}'), " +
					@"sender: '{2}', receiver: '{3}', subject: '{4}', body: '{5}'.",
					smtpServer,
					SafeResolveIPAddress( smtpServer ),
					message.From,
					message.To,
					message.Subject,
					message.Body ) );

				SmtpClient client;

				if ( ZetaLib.Core.LibraryConfiguration.Current.SmtpServerPort != 0 )
				{
					client = new SmtpClient(
						smtpServer,
						ZetaLib.Core.LibraryConfiguration.Current.SmtpServerPort );
				}
				else
				{
					client = new SmtpClient(
						smtpServer );
				}

				client.Send( message );
			}
			catch ( Exception x )
			{
				LogCentral.Current.LogError(
					string.Format(
					@"Error sending e-mail message. SMTP server: " +
					@"'{0}' (IP '{1}'), sender: '{2}', receiver: '{3}', " +
					@"subject: '{4}', body: '{5}'.",
					smtpServer,
					SafeResolveIPAddress( smtpServer ),
					message.From,
					message.To,
					message.Subject,
					message.Body ),
					x );

				throw;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Access the default cache manager.
		/// </summary>
		/// <value>The cache.</value>
		public CacheManager Cache
		{
			get
			{
				if ( cacheManager == null )
				{
					cacheManager = new CacheManager();
					cacheManager.Initialize();
				}

				return cacheManager;
			}
			set
			{
				cacheManager = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Configuration subclasses - database.
		// ------------------------------------------------------------------

		/// <summary>
		/// Configuration of the database.
		/// </summary>
		public class DatabaseConfiguration
		{
			#region Public enums.

			/// <summary>
			/// How caching of SQL statements is being handled.
			/// </summary>
			public enum DatabaseCacheSqlBehavior
			{
				#region Enum members.

				/// <summary>
				/// The cache is active, even if not explicitely requested
				/// by a caller.
				/// </summary>
				On,

				/// <summary>
				/// The cache is inactive, even if explicitely requested
				/// by a caller.
				/// </summary>
				Off,

				/// <summary>
				/// The cache is partially active, when being explicitely
				/// requested by a caller.
				/// </summary>
				Partially

				#endregion
			}

			#endregion

			#region Public methods.

			/// <summary>
			/// Constructor. Creates empty.
			/// </summary>
			public DatabaseConfiguration()
			{
				CheckGetDefaultConnectionString();
			}

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="connectionString">The connection string.</param>
			/// <param name="commandTimeoutSeconds">The command timeout seconds.</param>
			/// <param name="cacheSqlBehavior">The cache SQL behavior.</param>
			/// <param name="traceSqlEnabled">if set to <c>true</c> [trace SQL enabled].</param>
			public DatabaseConfiguration(
				string connectionString,
				int commandTimeoutSeconds,
				DatabaseCacheSqlBehavior cacheSqlBehavior,
				bool traceSqlEnabled )
			{
				this.connectionString = connectionString;
				this.commandTimeoutSeconds = commandTimeoutSeconds;
				this.cacheSqlBehavior = cacheSqlBehavior;
				this.traceSqlEnabled = traceSqlEnabled;

				CheckGetDefaultConnectionString();
			}

			#endregion

			#region Public properties.

			/// <summary>
			/// The connection string to the database.
			/// </summary>
			/// <value>The connection string.</value>
			public SmartConnectionString ConnectionString
			{
				get
				{
					return new SmartConnectionString( connectionString );
				}
			}

			/// <summary>
			/// The timeout for commands in seconds.
			/// </summary>
			/// <value>The command timeout seconds.</value>
			public int CommandTimeoutSeconds
			{
				get
				{
					return commandTimeoutSeconds;
				}
			}

			/// <summary>
			/// Whether SQL-statements and their results
			/// should be cached.
			/// </summary>
			/// <value>The cache SQL behavior.</value>
			public DatabaseCacheSqlBehavior CacheSqlBehavior
			{
				get
				{
					return cacheSqlBehavior;
				}
			}

			/// <summary>
			/// Whether SQL-statements should be traced.
			/// </summary>
			/// <value><c>true</c> if [trace SQL enabled]; otherwise, <c>false</c>.</value>
			public bool TraceSqlEnabled
			{
				get
				{
					return
						traceSqlEnabled &&
						LogCentral.Current.IsLoggingEnabled( LogType.Info );
				}
			}

			#endregion

			#region Private methods.

			/// <summary>
			/// Try to get a standard connection string if none
			/// is provided.
			/// </summary>
			private void CheckGetDefaultConnectionString()
			{
				if ( string.IsNullOrEmpty( connectionString ) )
				{
					// Entry 0 seems to be a default entry.
					if ( ConfigurationManager.ConnectionStrings.Count > 1 )
					{
						// Get the first one.
						connectionString =
							ConfigurationManager.ConnectionStrings[1].
							ConnectionString;
					}
				}
			}

			#endregion

			#region Private variables.

			private string connectionString = null;
			private int commandTimeoutSeconds = 0;
			private DatabaseCacheSqlBehavior cacheSqlBehavior =
				DatabaseCacheSqlBehavior.Partially;
			private bool traceSqlEnabled = false;

			#endregion
		}

		// ------------------------------------------------------------------
		#endregion

		#region Configuration subclasses - web.
		// ------------------------------------------------------------------

		/// <summary>
		/// Configuration of the web.
		/// </summary>
		public class WebConfiguration
		{
			#region Public methods.

			/// <summary>
			/// Constructor. Creates empty.
			/// </summary>
			public WebConfiguration()
			{
			}

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="useServerSideViewState">if set to <c>true</c> [use server side view state].</param>
			/// <param name="replaceTildeFallback">The replace tilde fallback.</param>
			/// <param name="replaceTildeCompleteFallback">The replace tilde complete fallback.</param>
			/// <param name="useCustomErrors">if set to <c>true</c> [use custom errors].</param>
			public WebConfiguration(
				bool useServerSideViewState,
				string replaceTildeFallback,
				string replaceTildeCompleteFallback,
				bool useCustomErrors )
			{
				this.useServerSideViewState = useServerSideViewState;
				this.replaceTildeFallback = replaceTildeFallback;
				this.replaceTildeCompleteFallback =
					replaceTildeCompleteFallback;
				this.useCustomErrors = useCustomErrors;
			}

			#endregion

			#region Public properties.

			/// <summary>
			/// Whether to use a server-side viewstate instead
			/// of a client-side viewstate.
			/// </summary>
			/// <value>
			/// 	<c>true</c> if [use server side view state]; otherwise, <c>false</c>.
			/// </value>
			public bool UseServerSideViewState
			{
				get
				{
					return useServerSideViewState;
				}
			}

			/// <summary>
			/// The string that is used when replacing a tilde
			/// and the result cannot be resolved.
			/// </summary>
			/// <value>The replace tilde fallback.</value>
			public string ReplaceTildeFallback
			{
				get
				{
					return replaceTildeFallback;
				}
			}

			/// <summary>
			/// The string that is used when replacing a tilde
			/// and the result cannot be resolved.
			/// </summary>
			/// <value>The replace tilde complete fallback.</value>
			public string ReplaceTildeCompleteFallback
			{
				get
				{
					return replaceTildeCompleteFallback;
				}
			}

			/// <summary>
			/// Whether to display custom errors on
			/// web pages.
			/// </summary>
			/// <value><c>true</c> if [use custom errors]; otherwise, <c>false</c>.</value>
			public bool UseCustomErrors
			{
				get
				{
					return useCustomErrors;
				}
			}

			#endregion

			#region Private variables.

			private bool useServerSideViewState = false;
			private bool useCustomErrors = false;
			private string replaceTildeFallback = null;
			private string replaceTildeCompleteFallback = null;

			#endregion
		}

		// ------------------------------------------------------------------
		#endregion

		#region Configuration properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// The name of the application for storing values in the registry.
		/// Use the format "MyApplicationName".
		/// </summary>
		/// <value>The name of the application registry key.</value>
		public string ApplicationRegistryKeyName
		{
			get
			{
				if ( string.IsNullOrEmpty( applicationRegistryKeyName ) )
				{
					return Assembly.GetEntryAssembly().GetName().Name;
				}
				else
				{
					return applicationRegistryKeyName;
				}
			}
			set
			{
				applicationRegistryKeyName = value;
			}
		}

		/// <summary>
		/// The database configuration.
		/// </summary>
		/// <value>The database.</value>
		public DatabaseConfiguration Database
		{
			get
			{
				return database;
			}
		}

		/// <summary>
		/// The web configuration.
		/// </summary>
		/// <value>The web.</value>
		public WebConfiguration Web
		{
			get
			{
				return web;
			}
		}

		/// <summary>
		/// Configure to turn off protection of passwords
		/// stored inside the logfile.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [disable logging password protection]; otherwise, <c>false</c>.
		/// </value>
		public bool DisableLoggingPasswordProtection
		{
			get
			{
				return disableLoggingPasswordProtection;
			}
		}

		/// <summary>
		/// The e-mail address of an administrator.
		/// Can be NULL.
		/// </summary>
		/// <value>The administrator E mail address.</value>
		public string AdministratorEMailAddress
		{
			get
			{
				if ( administratorEMailAddress == null ||
					administratorEMailAddress.IndexOf( ';' ) < 0 )
				{
					return administratorEMailAddress;
				}
				else
				{
					return administratorEMailAddress.Split( ';' )[0];
				}
			}
		}

		/// <summary>
		/// The e-mail address of an administrator.
		/// Can be NULL.
		/// </summary>
		/// <value>The administrator E mail addresses.</value>
		public string[] AdministratorEMailAddresses
		{
			get
			{
				if ( administratorEMailAddress == null )
				{
					return null;
				}
				else
				{
					if ( administratorEMailAddress.IndexOf( ';' ) < 0 )
					{
						return new string[] { administratorEMailAddress };
					}
					else
					{
						return administratorEMailAddress.Split( ';' );
					}
				}
			}
		}

		/// <summary>
		/// The server used to send e-mail messages via SMTP.
		/// Can be NULL.
		/// </summary>
		/// <value>The SMTP server.</value>
		public string SmtpServer
		{
			get
			{
				return smtpServer;
			}
			set
			{
				smtpServer = value;
			}
		}

		/// <summary>
		/// The server port used to send e-mail messages via SMTP.
		/// Is zero if not assigned.
		/// </summary>
		/// <value>The SMTP server port.</value>
		public int SmtpServerPort
		{
			get
			{
				return smtpServerPort;
			}
			set
			{
				smtpServerPort = value;
			}
		}

		/// <summary>
		/// The user name used to send e-mail messages via SMTP.
		/// Is null/empty if not assigned.
		/// </summary>
		/// <value>The name of the SMTP server user.</value>
		public string SmtpServerUserName
		{
			get
			{
				return smtpServerUserName;
			}
		}

		/// <summary>
		/// The user name used to send e-mail messages via SMTP.
		/// Is null/empty if not assigned.
		/// </summary>
		/// <value>The SMTP server password.</value>
		public string SmtpServerPassword
		{
			get
			{
				return smtpServerPassword;
			}
		}

		/// <summary>
		/// Get the web proxy settings. If no web proxy is configured
		/// in the configuration file, the default Internet Explorer
		/// proxy settings are returned.
		/// </summary>
		/// <value>The web proxy.</value>
		public IWebProxy WebProxy
		{
			get
			{
				if ( webProxy == null )
				{
					return WebRequest.DefaultWebProxy;
				}
				else
				{
					return webProxy;
				}
			}
		}

		/// <summary>
		/// For the "FileDeleteHelper" class.
		/// </summary>
		/// <value>The deleted files folder path.</value>
		public DirectoryInfo DeletedFilesFolderPath
		{
			get
			{
				return deletedFilesFolderPath;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Helper.
		/// </summary>
		/// <param name="hostName">Name of the host.</param>
		/// <returns></returns>
		private string SafeResolveIPAddress(
			string hostName )
		{
			if ( hostName == null || hostName.Length <= 0 )
			{
				return string.Empty;
			}
			else
			{
				IPHostEntry entry =
					Dns.GetHostEntry( hostName );

				if ( entry == null ||
					entry.AddressList == null ||
					entry.AddressList.Length < 0 )
				{
					return string.Empty;
				}
				else
				{
					return entry.AddressList[0].ToString();
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private string applicationRegistryKeyName;
		private string administratorEMailAddress;
		private bool disableLoggingPasswordProtection;
		private string smtpServer;
		private int smtpServerPort;
		private string smtpServerUserName;
		private string smtpServerPassword;
		private DirectoryInfo deletedFilesFolderPath;
		private IWebProxy webProxy;

		private DatabaseConfiguration database =
			new DatabaseConfiguration();
		private WebConfiguration web =
			new WebConfiguration();

		/// <summary>
		/// Doing new initialization.
		/// </summary>
		private static object libraryInitialisator;

		/// <summary>
		/// 
		/// </summary>
		private static object typeLock = new object();

		/// <summary>
		/// The default cache manager.
		/// </summary>
		private CacheManager cacheManager;

		/// <summary>
		/// 
		/// </summary>
		private static volatile LibraryConfiguration current;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Read the configuration.
	/// See http://support.microsoft.com/?kbid=309045.
	/// </summary>
	internal sealed class LibraryConfigurationSectionHandler :
		IConfigurationSectionHandler
	{
		#region IConfigurationSectionHandler member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Creates an instance of this class.
		/// </summary>
		/// <param name="parent">Parent object.</param>
		/// <param name="configContext">Configuration context object.</param>
		/// <param name="section">Section XML node.</param>
		/// <returns>The created section handler object.</returns>
		public object Create(
			object parent,
			object configContext,
			XmlNode section )
		{
			return section;
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}