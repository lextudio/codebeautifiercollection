namespace ZetaLib.Core.Logging
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Diagnostics;
	using System.Globalization;
	using System.IO;
	using System.Reflection;
	using System.Text.RegularExpressions;
	using System.Xml;
	using ZetaLib.Core.Properties;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Central base for logging, so that even library functions
	/// have access to logging functionality.
	/// </summary>
	public sealed class LogCentral :
		Logger
	{
		#region Static member.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private static object typeLock = new object();

		/// <summary>
		/// Access the current instance of the class.
		/// </summary>
		/// <value>The current.</value>
		public static LogCentral Current
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
							current = new LogCentral();
							current.ConfigureLogging();
						}
					}
				}

				return current;
			}
		}

		/// <summary>
		/// Can be static here, even for a web application, because
		/// no session-/user-dependent information is stored inside
		/// this class.
		/// </summary>
		private static volatile LogCentral current = null;

		// ------------------------------------------------------------------
		#endregion

		#region Configuring logging.
		// ------------------------------------------------------------------

		/// <summary>
		/// Configures logging through the given stream with
		/// XML configuration data.
		/// </summary>
		/// <param name="configStream">The config stream.</param>
		public void ConfigureLogging(
			Stream configStream )
		{
			lock ( thisLock )
			{
				log4net.Config.XmlConfigurator.Configure(
					ConvertConfigToXml( configStream ) );
				configurationFilePath = null;
			}
		}

		/// <summary>
		/// Configures logging through the given file with
		/// XML configuration data.
		/// </summary>
		/// <param name="configFilePath">The config file path.</param>
		public void ConfigureLogging(
			string configFilePath )
		{
			lock ( thisLock )
			{
				log4net.Config.XmlConfigurator.Configure(
					ConvertConfigToXml( new FileInfo( configFilePath ) ) );
				this.configurationFilePath = configFilePath;
			}
		}

		/// <summary>
		/// Configures logging through the given file with
		/// XML configuration data.
		/// </summary>
		/// <param name="configElement">The config element.</param>
		public void ConfigureLogging(
			XmlElement configElement )
		{
			lock ( thisLock )
			{
				log4net.Config.XmlConfigurator.Configure(
					ConvertConfigToXml( configElement ) );
				configurationFilePath = null;
			}
		}

		/// <summary>
		/// Helper for searching.
		/// </summary>
		public static event LogCentralFindConfigurationFilePathEventHandler
			FindConfigurationFilePath;

		/// <summary>
		/// Automatically configures logging. Searches for possible
		/// configuration files.
		/// This function is called automatically, too.
		/// </summary>
		public void ConfigureLogging()
		{
			lock ( thisLock )
			{
				bool configured = false;

				// --

				// Also, try custom configuration path.
				if ( !configured )
				{
					if ( FindConfigurationFilePath != null )
					{
						string path = FindConfigurationFilePath( this );
						if ( File.Exists( path ) )
						{
							log4net.Config.XmlConfigurator.Configure(
								ConvertConfigToXml( new FileInfo( path ) ) );
							configurationFilePath = path;
							configured = true;
							Trace.WriteLine( string.Format(
								@".NET-logging configured for config file '{0}'.",
								configurationFilePath ) );
							Debug.WriteLine( string.Format(
								@".NET-logging configured for config file '{0}'.",
								configurationFilePath ) );
						}
					}
				}

				// --

				if ( !configured )
				{
					// Tells the logging system the correct path.
					Assembly[] assembliesToTry = new Assembly[]
						{
							Assembly.GetEntryAssembly(),
							Assembly.GetExecutingAssembly(),
							Assembly.GetCallingAssembly(),
							Assembly.GetAssembly( typeof( LogCentral ) )
						};

					foreach ( Assembly a in assembliesToTry )
					{
						if ( !configured )
						{
							if ( a != null && a.Location != null )
							{
								// Try multiple paths.
								// Be sure to KEEP the order!
								string[] pathsToTry = new string[]
								{
									Path.Combine(
										Path.GetDirectoryName( a.Location ), 
									Path.GetFileNameWithoutExtension( a.Location ) + @".logging.config" ),
									a.Location + @".logging.config",
									Path.Combine( 
										Path.GetDirectoryName( a.Location ), 
									Path.GetFileNameWithoutExtension( a.Location ) + @".config" ),
									a.Location + @".config",
								};

								foreach ( string path in pathsToTry )
								{
									if ( File.Exists( path ) )
									{
										log4net.Config.XmlConfigurator.Configure(
											ConvertConfigToXml( new FileInfo( path ) ) );
										configurationFilePath = path;
										configured = true;
										Trace.WriteLine( string.Format(
											@".NET-logging configured for config file '{0}'.",
											configurationFilePath ) );
										Debug.WriteLine( string.Format(
											@".NET-logging configured for config file '{0}'.",
											configurationFilePath ) );
										break;
									}
								}
							}
						}
					}

					if ( !configured )
					{
						foreach ( Assembly a in assembliesToTry )
						{
							if ( !configured )
							{
								if ( a != null && a.Location != null )
								{
									string path = FindConfigInPath( Path.GetDirectoryName( a.Location ) );
									if ( File.Exists( path ) )
									{
										log4net.Config.XmlConfigurator.Configure(
											ConvertConfigToXml( new FileInfo( path ) ) );
										configurationFilePath = path;
										configured = true;
										Trace.WriteLine( string.Format(
											@".NET-logging configured for config file '{0}'.",
											configurationFilePath ) );
										Debug.WriteLine( string.Format(
											@".NET-logging configured for config file '{0}'.",
											configurationFilePath ) );
										break;
									}
								}
							}
						}
					}
				}

				// --

				if ( !configured && FindConfigurationFilePath == null )
				{
					throw new ApplicationException(
						Resources.Str_LogCentral_Error01 );
				}

				ConfigureTracing();
			}
		}

		/// <summary>
		/// Configures the tracing.
		/// </summary>
		private void ConfigureTracing()
		{
			// Add my own trace listener.
			Debug.Listeners.Add( new LogCentralTraceListener() );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Configuration helper.
		// ------------------------------------------------------------------

		/// <summary>
		/// Helper function for centralized expanding of "${xxx}"-placeholders.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="resolver">The resolver.</param>
		/// <returns></returns>
		public static string ExpandFilePathMacros(
			string input,
			IExpandFilePathMacrosResolver resolver )
		{
			return ExpandMacroInString( input, resolver );
		}

		/// <summary>
		/// Helper function for centralized expanding of "${xxx}"-placeholders.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		public static string ExpandFilePathMacros(
			string input )
		{
			return ExpandMacroInString( input, null );
		}

		/// <summary>
		/// Expand "${xxx}"-placeholders.
		/// Returns original string with expanded macros (if any).
		/// If a macro could not be expanded, it is replaced by an empty string.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="resolver">The resolver.</param>
		/// <returns></returns>
		private static string ExpandMacroInString(
			string text,
			IExpandFilePathMacrosResolver resolver )
		{
			if ( string.IsNullOrEmpty( text ) || text.IndexOf( @"${" ) < 0 )
			{
				return text;
			}
			else
			{
				// First pass, "${Xxx}".
				MatchCollection matches = Regex.Matches(
					text,
					@"\$\{[^}]+\}",
					RegexOptions.IgnoreCase | RegexOptions.Compiled );

				if ( matches != null && matches.Count > 0 )
				{
					foreach ( Match match in matches )
					{
						text = text.Replace(
							match.Value,
							ExpandMacro( match.Value, resolver ) );
					}
				}

				// --

				// Second pass, "$(Xxx)".
				matches = Regex.Matches(
					text,
					@"\$\([^)]+\)",
					RegexOptions.IgnoreCase | RegexOptions.Compiled );

				if ( matches != null && matches.Count > 0 )
				{
					foreach ( Match match in matches )
					{
						text = text.Replace(
							match.Value,
							ExpandMacro( match.Value, resolver ) );
					}
				}

				// --

				return text;
			}
		}

		/// <summary>
		/// Expand "${xxx}"-placeholders.
		/// Returns empty string if not found.
		/// </summary>
		/// <param name="macroName">Name of the macro.</param>
		/// <param name="resolver">The resolver.</param>
		/// <returns></returns>
		private static string ExpandMacro(
			string macroName,
			IExpandFilePathMacrosResolver resolver )
		{
			if ( macroName != null && macroName.Length > 0 )
			{
				// Remove the placeholder-surroundings.
				macroName = macroName.Trim( '$', '{', '}' );

				bool hasExpanded = false;

				// Special folders.
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.AppData",
					Environment.GetFolderPath(
					Environment.SpecialFolder.ApplicationData ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.CommonAppData",
					Environment.GetFolderPath(
					Environment.SpecialFolder.CommonApplicationData ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.Cookies",
					Environment.GetFolderPath(
					Environment.SpecialFolder.Cookies ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.History",
					Environment.GetFolderPath(
					Environment.SpecialFolder.History ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.InternetCache",
					Environment.GetFolderPath(
					Environment.SpecialFolder.InternetCache ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.LocalAppData",
					Environment.GetFolderPath(
					Environment.SpecialFolder.LocalApplicationData ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.MyPictures",
					Environment.GetFolderPath(
					Environment.SpecialFolder.MyPictures ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.Personal",
					Environment.GetFolderPath(
					Environment.SpecialFolder.Personal ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.ProgramFiles",
					Environment.GetFolderPath(
					Environment.SpecialFolder.ProgramFiles ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.ProgramFilesCommon",
					Environment.GetFolderPath(
					Environment.SpecialFolder.CommonProgramFiles ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.System",
					Environment.GetFolderPath(
					Environment.SpecialFolder.System ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.Temporary",
					Path.GetTempPath().TrimEnd( '\\' ) );

				// Assembly.
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.CallingAssembly",
					Assembly.GetCallingAssembly() == null ?
					string.Empty :
					Path.GetDirectoryName(
					Assembly.GetCallingAssembly().Location ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.EntryAssembly",
					Assembly.GetEntryAssembly() == null ?
					string.Empty :
					Path.GetDirectoryName(
					Assembly.GetEntryAssembly().Location ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.ExecutingAssembly",
					Assembly.GetExecutingAssembly() == null ?
					string.Empty :
					Path.GetDirectoryName(
					Assembly.GetExecutingAssembly().Location ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.CallingModule",
					Assembly.GetCallingAssembly() == null ?
					string.Empty :
					Path.GetDirectoryName(
					Assembly.GetCallingAssembly().Location ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.EntryModule",
					Assembly.GetEntryAssembly() == null ?
					string.Empty :
					Path.GetDirectoryName(
					Assembly.GetEntryAssembly().Location ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.ExecutingModule",
					Assembly.GetExecutingAssembly() == null ?
					string.Empty :
					Path.GetDirectoryName(
					Assembly.GetExecutingAssembly().Location ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFolder.ConfigurationFilePath",
					LogCentral.Current.ConfigurationFilePath == null ?
					string.Empty :
					Path.GetDirectoryName(
					LogCentral.Current.ConfigurationFilePath ) );

				// Assembly.
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFile.CallingAssembly",
					Assembly.GetCallingAssembly() == null ?
					string.Empty :
					Assembly.GetCallingAssembly().Location );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFile.EntryAssembly",
					Assembly.GetEntryAssembly() == null ?
					string.Empty :
					Assembly.GetEntryAssembly().Location );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFile.ExecutingAssembly",
					Assembly.GetExecutingAssembly() == null ?
					string.Empty :
					Assembly.GetExecutingAssembly().Location );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFile.CallingModule",
					Assembly.GetCallingAssembly() == null ?
					string.Empty :
					Assembly.GetCallingAssembly().Location );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFile.EntryModule",
					Assembly.GetEntryAssembly() == null ?
					string.Empty :
					Assembly.GetEntryAssembly().Location );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFile.ExecutingModule",
					Assembly.GetExecutingAssembly() == null ?
					string.Empty :
					Assembly.GetExecutingAssembly().Location );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"SpecialFile.ConfigurationFilePath",
					LogCentral.Current.ConfigurationFilePath == null ?
					string.Empty :
					LogCentral.Current.ConfigurationFilePath );

				// Date specific.
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"Date.Now",
					DateTime.Now.ToString( @"yyyy-MM-dd HH_mm_ss" ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"Date.NowDate",
					DateTime.Now.ToString( @"yyyy-MM-dd" ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"Date.NowTime",
					DateTime.Now.ToString( @"HH_mm_ss" ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"Date.NowYear",
					DateTime.Now.ToString( @"yyyy" ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"Date.NowMonth",
					DateTime.Now.ToString( @"MM" ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"Date.NowDay",
					DateTime.Now.ToString( @"dd" ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"Date.NowHour",
					DateTime.Now.ToString( @"HH" ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"Date.NowMinute",
					DateTime.Now.ToString( @"mm" ) );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"Date.NowSecond",
					DateTime.Now.ToString( @"ss" ) );

				// Environment variables.
				CheckExpandMacro(
					resolver,
					ref hasExpanded, ref macroName,
					@"Environment.CurrentDirectory",
					Environment.CurrentDirectory );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"Environment.SystemDirectory",
					Environment.SystemDirectory );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"Environment.MachineName",
					Environment.MachineName );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"Environment.UserDomainName",
					Environment.UserDomainName );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"Environment.UserName",
					Environment.UserName );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"Environment.Version",
					Environment.Version.ToString() );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"Environment.OSVersion",
					Environment.OSVersion.ToString() );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"Environment.ProcessorCount",
					Environment.ProcessorCount.ToString() );
				CheckExpandMacro(
					resolver,
					ref hasExpanded,
					ref macroName,
					@"Environment.TickCount",
					Environment.TickCount.ToString() );

				if ( !hasExpanded && !string.IsNullOrEmpty( macroName ) )
				{
					CheckExpandMacro(
						resolver,
						ref hasExpanded,
						ref macroName,
						macroName,
						string.IsNullOrEmpty( macroName ) ?
						string.Empty :
						Environment.GetEnvironmentVariable( macroName ) );
				}
				if ( !hasExpanded && !string.IsNullOrEmpty( macroName ) )
				{
					CheckExpandMacro(
						resolver,
						ref hasExpanded,
						ref macroName,
						@"Environment.Variable." +
						(string.IsNullOrEmpty( macroName ) ? string.Empty : macroName),
						string.IsNullOrEmpty( macroName ) ?
						string.Empty :
						Environment.GetEnvironmentVariable( macroName ) );
				}
			}

			if ( macroName == null )
			{
				macroName = string.Empty;
			}
			return macroName;
		}

		/// <summary>
		/// Helper.
		/// </summary>
		/// <param name="resolver">The resolver.</param>
		/// <param name="hasExpanded">if set to <c>true</c> [has expanded].</param>
		/// <param name="macroName">Name of the macro.</param>
		/// <param name="macroNameToCompareWith">The macro name to compare with.</param>
		/// <param name="macroValueToReplaceWith">The macro value to replace with.</param>
		private static void CheckExpandMacro(
			IExpandFilePathMacrosResolver resolver,
			ref bool hasExpanded,
			ref string macroName,
			string macroNameToCompareWith,
			string macroValueToReplaceWith )
		{
			if ( !hasExpanded && macroName != null && macroName.Length > 0 )
			{
				if ( string.Compare( macroName, macroNameToCompareWith, true ) == 0 )
				{
					if ( resolver != null )
					{
						resolver.Resolve(
							macroName,
							ref macroValueToReplaceWith );
					}

					macroName = macroValueToReplaceWith;
					hasExpanded = true;
				}
			}
		}

		/// <summary>
		/// Loads configuration and expands any macros.
		/// </summary>
		/// <param name="config">The config.</param>
		/// <returns></returns>
		private XmlElement ConvertConfigToXml(
			Stream config )
		{
			XmlDocument doc = new XmlDocument();
			doc.Load( config );

			return ConvertConfigToXml(
				doc.GetElementsByTagName( @"log4net" )[0] as XmlElement );
		}

		/// <summary>
		/// Loads configuration and expands any macros.
		/// </summary>
		/// <param name="config">The config.</param>
		/// <returns></returns>
		private XmlElement ConvertConfigToXml(
			FileInfo config )
		{
			XmlDocument doc = new XmlDocument();
			doc.Load( config.FullName );

			return ConvertConfigToXml(
				doc.GetElementsByTagName( @"log4net" )[0] as XmlElement );
		}

		/// <summary>
		/// Loads configuration and expands any macros.
		/// </summary>
		/// <param name="config">The config.</param>
		/// <returns></returns>
		private XmlElement ConvertConfigToXml(
			XmlElement config )
		{
			if ( config != null )
			{
				// Expand macros.
				if ( config.Attributes != null )
				{
					foreach ( XmlAttribute a in config.Attributes )
					{
						if ( a != null )
						{
							a.Value = ExpandMacroInString( a.Value, null );
						}
					}
				}

				// Recurse.
				if ( config.ChildNodes != null )
				{
					foreach ( XmlNode n in config.ChildNodes )
					{
						if ( n != null )
						{
							XmlElement e = n as XmlElement;

							if ( e != null )
							{
								ConvertConfigToXml( e );
							}
						}
					}
				}
			}

			return config;
		}

		/// <summary>
		/// Best practice, see C# MSDN documentation of the "lock" keyword.
		/// </summary>
		private object thisLock = new object();

		// ------------------------------------------------------------------
		#endregion

		#region Logging.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		private LogCentral()
			:
			base( null )
		{
			// Add default handlers.
			this.RequestMoreInformation +=
				new LogCentralRequestMoreInformationEventHandler(
				LogCentral_RequestMoreInformationNetworkEnvironment );
			this.RequestMoreInformation +=
				new LogCentralRequestMoreInformationEventHandler(
				LogCentral_RequestMoreInformationEnvironment );
			this.RequestMoreInformation +=
				new LogCentralRequestMoreInformationEventHandler(
				LogCentral_RequestMoreInformationAssembly );
		}

		/// <summary>
		/// Check whether logging is enabled for a certain type.
		/// </summary>
		/// <param name="logType"></param>
		/// <returns></returns>
		public new bool IsLoggingEnabled(
			LogType logType )
		{
			return GetRootLogger().IsLoggingEnabled( logType );
		}

		/// <summary>
		/// Check whether logging is enabled for a certain type.
		/// </summary>
		/// <param name="loggerName">Name of the logger.</param>
		/// <param name="logType">Type of the log.</param>
		/// <returns>
		/// 	<c>true</c> if [is logging enabled] [the specified logger name]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsLoggingEnabled(
			string loggerName,
			LogType logType )
		{
			return GetLoggerByName( loggerName ).IsLoggingEnabled( logType );
		}

		/// <summary>
		/// Get a certain logger by a specified name.
		/// Returns the root logger if the specified logger was not found.
		/// </summary>
		/// <value></value>
		public Logger this[
			string loggerName]
		{
			get
			{
				return GetLoggerByName( loggerName );
			}
		}

		/// <summary>
		/// Get a certain logger by a specified name.
		/// Returns the root logger if the specified logger was not found.
		/// </summary>
		/// <param name="loggerName">Name of the logger.</param>
		/// <returns></returns>
		public new Logger GetLoggerByName(
			string loggerName )
		{
			if ( string.IsNullOrEmpty( loggerName ) )
			{
				return GetRootLogger();
			}
			else
			{
				Logger logger = Logger.GetLoggerByName( loggerName );
				if ( logger == null )
				{
					return GetRootLogger();
				}
				else
				{
					return logger;
				}
			}
		}

		/// <summary>
		/// Get the root logger.
		/// </summary>
		/// <returns></returns>
		private Logger GetRootLogger()
		{
			return this;
		}

		/// <summary>
		/// Get the full path to the configuration file of this
		/// application. Usually a file with ".CONFIG" extension.
		/// Can be NULL when configured through a stream or through
		/// an XML element.
		/// </summary>
		/// <value>The configuration file path.</value>
		public string ConfigurationFilePath
		{
			get
			{
				lock ( thisLock )
				{
					return configurationFilePath;
				}
			}
		}

		/// <summary>
		/// Check whether a configuration file exists.
		/// Can be NULL when configured through a stream or through
		/// an XML element.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has configuration file; otherwise, <c>false</c>.
		/// </value>
		public bool HasConfigurationFile
		{
			get
			{
				string filePath = ConfigurationFilePath;

				return
					!string.IsNullOrEmpty( ConfigurationFilePath ) &&
					File.Exists( filePath );
			}
		}

		/// <summary>
		/// Searches for a configuration file in the given path.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		private string FindConfigInPath(
			string path )
		{
			string[] files = Directory.GetFiles( path, @"*.config" );

			if ( files != null && files.Length > 0 )
			{
				// Two passes.

				foreach ( string file in files )
				{
					string fileName = Path.GetFileName( file ).ToLower();

					int pos = fileName.IndexOf( @".logging.config" );
					if ( pos + @".logging.config".Length == fileName.Length )
					{
						Trace.WriteLine( string.Format( @"Found .NET-logging config file '{0}'.", file ) );
						Debug.WriteLine( string.Format( @"Found .NET-logging config file '{0}'.", file ) );
						return file;
					}
				}

				foreach ( string file in files )
				{
					string fileName = Path.GetFileName( file ).ToLower();

					if ( string.Compare(
						Path.GetExtension( file ).Trim( '.' ),
						@"config",
						true ) == 0 )
					{
						Trace.WriteLine( string.Format( @"Found .NET-logging config file '{0}'.", file ) );
						Debug.WriteLine( string.Format( @"Found .NET-logging config file '{0}'.", file ) );
						return file;
					}
				}
			}

			// Not found.
			return string.Empty;
		}

		/// <summary>
		/// The path where the configuration is read from.
		/// This value is set upon a call to ConfigureLogging().
		/// </summary>
		private string configurationFilePath;

		/// <summary>
		/// Combines the more information strings and the regular
		/// message string.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="moreInformationMessage"></param>
		/// <returns></returns>
		private string CombineMoreInformationMessage(
			string message,
			string moreInformationMessage )
		{
			if ( moreInformationMessage == null )
			{
				return message;
			}
			else
			{
				if ( message == null )
				{
					return Indent( moreInformationMessage );
				}
				else
				{
					return message.ToString().TrimEnd() +
						Environment.NewLine + Environment.NewLine +
						Indent( moreInformationMessage.Trim() );
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Information provider.
		// ------------------------------------------------------------------

		/// <summary>
		/// Provide my own handler for basic information.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="ZetaLib.Core.Logging.LogCentralRequestMoreInformationEventArgs"/> instance containing the event data.</param>
		/// <remarks>
		/// This handler is called when the logging framework wants more
		/// information about the current environment.
		/// </remarks>
		private void LogCentral_RequestMoreInformationAssembly(
			object sender,
			LogCentralRequestMoreInformationEventArgs e )
		{
			if ( e.Type == LogType.Error ||
				e.Type == LogType.Fatal ||
				e.Type == LogType.Warn )
			{
				e.MoreInformationMessage =
					Environment.NewLine +
					Environment.NewLine +
					@"------------->" +
					Environment.NewLine +

					LoggingInformation.Assemblies +

					Environment.NewLine +
					@"<-------------" +
					Environment.NewLine +
					Environment.NewLine;
			}
		}

		/// <summary>
		/// Provide my own handler for basic information.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="ZetaLib.Core.Logging.LogCentralRequestMoreInformationEventArgs"/> instance containing the event data.</param>
		/// <remarks>
		/// This handler is called when the logging framework wants more
		/// information about the current environment.
		/// </remarks>
		private void LogCentral_RequestMoreInformationEnvironment(
			object sender,
			LogCentralRequestMoreInformationEventArgs e )
		{
			if ( e.Type == LogType.Error ||
				e.Type == LogType.Fatal ||
				e.Type == LogType.Warn )
			{
				e.MoreInformationMessage =
					Environment.NewLine +
					Environment.NewLine +
					@"------------->" +
					Environment.NewLine +

					LoggingInformation.WindowsEnvironment +

					Environment.NewLine +
					@"<-------------" +
					Environment.NewLine +
					Environment.NewLine;
			}
		}

		/// <summary>
		/// Provide my own handler for basic information.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="ZetaLib.Core.Logging.LogCentralRequestMoreInformationEventArgs"/> instance containing the event data.</param>
		/// <remarks>
		/// This handler is called when the logging framework wants more
		/// information about the current environment.
		/// </remarks>
		private void LogCentral_RequestMoreInformationNetworkEnvironment(
			object sender,
			LogCentralRequestMoreInformationEventArgs e )
		{
			if ( e.Type == LogType.Error ||
				e.Type == LogType.Fatal ||
				e.Type == LogType.Warn )
			{
				e.MoreInformationMessage =
					Environment.NewLine +
					Environment.NewLine +
					@"------------->" +
					Environment.NewLine +

					LoggingInformation.NetworkEnvironment +

					Environment.NewLine +
					@"<-------------" +
					Environment.NewLine +
					Environment.NewLine;
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}