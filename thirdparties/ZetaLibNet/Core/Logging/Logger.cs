namespace ZetaLib.Core.Logging
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections;
	using System.Diagnostics;
	using System.Globalization;
	using System.Text.RegularExpressions;
	using System.Collections.Generic;
	using System.Reflection;
	using ZetaLib.Core.Common;
	using System.Text;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// A logger is a unit that is able to log.
	/// You do NOT create any instances of this class by yourself.
	/// </summary>
	public class Logger
	{
		#region Enhanced logging.
		// ------------------------------------------------------------------

		/// <summary>
		/// The event that tries to request more information. This event
		/// is raised before a message is actually being logged. Use this
		/// event to add your own handler to provide more detailed information
		/// to the message being logged.
		/// </summary>
		public event LogCentralRequestMoreInformationEventHandler
			RequestMoreInformation
		{
			add
			{
				if ( requestMoreInformationEvents == null )
				{
					requestMoreInformationEvents =
						new List<LogCentralRequestMoreInformationEventHandler>();
				}

				requestMoreInformationEvents.Add( value );
			}
			remove
			{
				if ( requestMoreInformationEvents != null )
				{
					if ( requestMoreInformationEvents.IndexOf( value ) >= 0 )
					{
						requestMoreInformationEvents.Remove( value );
					}
				}
			}
		}

		/// <summary>
		/// Collect the events.
		/// </summary>
		private List<LogCentralRequestMoreInformationEventHandler>
			requestMoreInformationEvents;

		/// <summary>
		/// Logging event. This event is raised after
		/// a call to one of the LogDebug, LogError, etc. functions occured.
		/// </summary>
		public event LogCentralLogEventHandler Log;

		// ------------------------------------------------------------------
		#endregion

		#region Log - Debug.
		// ------------------------------------------------------------------

		/// <summary>
		/// Log a Debug message.
		/// </summary>
		/// <param name="message">The message.</param>
		[Conditional( @"DEBUG" )]
		public void LogDebug(
			string message )
		{
			DoPerformLog(
				message,
				null,
				null,
				LogType.Debug );
		}

		/// <summary>
		/// Log a Debug message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		[Conditional( @"DEBUG" )]
		public void LogDebug(
			string message,
			Exception t )
		{
			DoPerformLog(
				message,
				t,
				null,
				LogType.Debug );
		}

		/// <summary>
		/// Log a Debug message.
		/// </summary>
		/// <param name="t">The t.</param>
		[Conditional( @"DEBUG" )]
		public void LogDebug(
			Exception t )
		{
			DoPerformLog(
				null,
				t,
				null,
				LogType.Debug );
		}

		/// <summary>
		/// Log a Debug message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		[Conditional( @"DEBUG" )]
		public void LogDebug(
			string message,
			Exception t,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				message,
				t,
				userDefinedInformation,
				LogType.Debug );
		}

		/// <summary>
		/// Log a Debug message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		[Conditional( @"DEBUG" )]
		public void LogDebug(
			string message,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				message,
				null,
				userDefinedInformation,
				LogType.Debug );
		}

		/// <summary>
		/// Log a Debug message.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		[Conditional( @"DEBUG" )]
		public void LogDebug(
			Exception t,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				null,
				t,
				userDefinedInformation,
				LogType.Debug );
		}

		/// <summary>
		/// Log a Debug message.
		/// </summary>
		/// <param name="userDefinedInformation">The user defined information.</param>
		[Conditional( @"DEBUG" )]
		public void LogDebug(
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				null,
				null,
				userDefinedInformation,
				LogType.Debug );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Log - Fatal.
		// ------------------------------------------------------------------

		/// <summary>
		/// Log a Fatal message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void LogFatal(
			string message )
		{
			DoPerformLog(
				message,
				null,
				null,
				LogType.Fatal );
		}

		/// <summary>
		/// Log a Fatal message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		public void LogFatal(
			string message,
			Exception t )
		{
			DoPerformLog(
				message,
				t,
				null,
				LogType.Fatal );
		}

		/// <summary>
		/// Log a Fatal message.
		/// </summary>
		/// <param name="t">The t.</param>
		public void LogFatal(
			Exception t )
		{
			DoPerformLog(
				null,
				t,
				null,
				LogType.Fatal );
		}

		/// <summary>
		/// Log a Fatal message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogFatal(
			string message,
			Exception t,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				message,
				t,
				userDefinedInformation,
				LogType.Fatal );
		}

		/// <summary>
		/// Log a Fatal message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogFatal(
			string message,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				message,
				null,
				userDefinedInformation,
				LogType.Fatal );
		}

		/// <summary>
		/// Log a Fatal message.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogFatal(
			Exception t,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				null,
				t,
				userDefinedInformation,
				LogType.Fatal );
		}

		/// <summary>
		/// Log a Fatal message.
		/// </summary>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogFatal(
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				null,
				null,
				userDefinedInformation,
				LogType.Fatal );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Log - Error.
		// ------------------------------------------------------------------

		/// <summary>
		/// Log a Error message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void LogError(
			string message )
		{
			DoPerformLog(
				message,
				null,
				null,
				LogType.Error );
		}

		/// <summary>
		/// Log a Error message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		public void LogError(
			string message,
			Exception t )
		{
			DoPerformLog(
				message,
				t,
				null,
				LogType.Error );
		}

		/// <summary>
		/// Log a Error message.
		/// </summary>
		/// <param name="t">The t.</param>
		public void LogError(
			Exception t )
		{
			DoPerformLog(
				null,
				t,
				null,
				LogType.Error );
		}

		/// <summary>
		/// Log a Error message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogError(
			string message,
			Exception t,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				message,
				t,
				userDefinedInformation,
				LogType.Error );
		}

		/// <summary>
		/// Log a Error message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogError(
			string message,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				message,
				null,
				userDefinedInformation,
				LogType.Error );
		}

		/// <summary>
		/// Log a Error message.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogError(
			Exception t,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				null,
				t,
				userDefinedInformation,
				LogType.Error );
		}

		/// <summary>
		/// Log a Error message.
		/// </summary>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogError(
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				null,
				null,
				userDefinedInformation,
				LogType.Error );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Log - Info.
		// ------------------------------------------------------------------

		/// <summary>
		/// Log a Info message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void LogInfo(
			string message )
		{
			DoPerformLog(
				message,
				null,
				null,
				LogType.Info );
		}

		/// <summary>
		/// Log a Info message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		public void LogInfo(
			string message,
			Exception t )
		{
			DoPerformLog(
				message,
				t,
				null,
				LogType.Info );
		}

		/// <summary>
		/// Log a Info message.
		/// </summary>
		/// <param name="t">The t.</param>
		public void LogInfo(
			Exception t )
		{
			DoPerformLog(
				null,
				t,
				null,
				LogType.Info );
		}

		/// <summary>
		/// Log a Info message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogInfo(
			string message,
			Exception t,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				message,
				t,
				userDefinedInformation,
				LogType.Info );
		}

		/// <summary>
		/// Log a Info message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogInfo(
			string message,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				message,
				null,
				userDefinedInformation,
				LogType.Info );
		}

		/// <summary>
		/// Log a Info message.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogInfo(
			Exception t,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				null,
				t,
				userDefinedInformation,
				LogType.Info );
		}

		/// <summary>
		/// Log a Info message.
		/// </summary>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogInfo(
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				null,
				null,
				userDefinedInformation,
				LogType.Info );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Log - Warn.
		// ------------------------------------------------------------------

		/// <summary>
		/// Log a Warn message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void LogWarn(
			string message )
		{
			DoPerformLog(
				message,
				null,
				null,
				LogType.Warn );
		}

		/// <summary>
		/// Log a Warn message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		public void LogWarn(
			string message,
			Exception t )
		{
			DoPerformLog(
				message,
				t,
				null,
				LogType.Warn );
		}

		/// <summary>
		/// Log a Warn message.
		/// </summary>
		/// <param name="t">The t.</param>
		public void LogWarn(
			Exception t )
		{
			DoPerformLog(
				null,
				t,
				null,
				LogType.Warn );
		}

		/// <summary>
		/// Log a Warn message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogWarn(
			string message,
			Exception t,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				message,
				t,
				userDefinedInformation,
				LogType.Warn );
		}

		/// <summary>
		/// Log a Warn message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogWarn(
			string message,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				message,
				null,
				userDefinedInformation,
				LogType.Warn );
		}

		/// <summary>
		/// Log a Warn message.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogWarn(
			Exception t,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				null,
				t,
				userDefinedInformation,
				LogType.Warn );
		}

		/// <summary>
		/// Log a Warn message.
		/// </summary>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogWarn(
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				null,
				null,
				userDefinedInformation,
				LogType.Warn );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Log - Todo.
		// ------------------------------------------------------------------

		/// <summary>
		/// Log a Todo message.
		/// Special overload for TODO only: no text at all.
		/// </summary>
		public void LogTodo()
		{
			DoPerformLog(
				null,
				null,
				null,
				LogType.Todo );
		}

		/// <summary>
		/// Log a Todo message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void LogTodo(
			string message )
		{
			DoPerformLog(
				message,
				null,
				null,
				LogType.Todo );
		}

		/// <summary>
		/// Log a Todo message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		public void LogTodo(
			string message,
			Exception t )
		{
			DoPerformLog(
				message,
				t,
				null,
				LogType.Todo );
		}

		/// <summary>
		/// Log a Todo message.
		/// </summary>
		/// <param name="t">The t.</param>
		public void LogTodo(
			Exception t )
		{
			DoPerformLog(
				null,
				t,
				null,
				LogType.Todo );
		}

		/// <summary>
		/// Log a Todo message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogTodo(
			string message,
			Exception t,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				message,
				t,
				userDefinedInformation,
				LogType.Todo );
		}

		/// <summary>
		/// Log a Todo message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogTodo(
			string message,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				message,
				null,
				userDefinedInformation,
				LogType.Todo );
		}

		/// <summary>
		/// Log a Todo message.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogTodo(
			Exception t,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				null,
				t,
				userDefinedInformation,
				LogType.Todo );
		}

		/// <summary>
		/// Log a Todo message.
		/// </summary>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogTodo(
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				null,
				null,
				userDefinedInformation,
				LogType.Todo );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Log - Typed.
		// ------------------------------------------------------------------

		/// <summary>
		/// Log a message.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="message">The message.</param>
		public void LogTyped(
			LogType type,
			string message )
		{
			DoPerformLog(
				message,
				null,
				null,
				type );
		}

		/// <summary>
		/// Log a message.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		public void LogTyped(
			LogType type,
			string message,
			Exception t )
		{
			DoPerformLog(
				message,
				t,
				null,
				type );
		}

		/// <summary>
		/// Log a message.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="t">The t.</param>
		public void LogTyped(
			LogType type,
			Exception t )
		{
			DoPerformLog(
				null,
				t,
				null,
				type );
		}

		/// <summary>
		/// Log a message.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogTyped(
			LogType type,
			string message,
			Exception t,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				message,
				t,
				userDefinedInformation,
				type );
		}

		/// <summary>
		/// Log a message.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="message">The message.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogTyped(
			LogType type,
			string message,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				message,
				null,
				userDefinedInformation,
				type );
		}

		/// <summary>
		/// Log a message.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="t">The t.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogTyped(
			LogType type,
			Exception t,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				null,
				t,
				userDefinedInformation,
				type );
		}

		/// <summary>
		/// Log a message.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public void LogTyped(
			LogType type,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			DoPerformLog(
				null,
				null,
				userDefinedInformation,
				type );
		}

		// ------------------------------------------------------------------
		#endregion

		#region More logging.
		// ------------------------------------------------------------------

		/// <summary>
		/// Check whether logging is enabled for a certain type.
		/// </summary>
		/// <param name="logType">Type of the log.</param>
		/// <returns>
		/// 	<c>true</c> if [is logging enabled] [the specified log type]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsLoggingEnabled(
			LogType logType )
		{
			if ( logImplementation == null )
			{
				return false;
			}
			else
			{
				switch ( logType )
				{
					case LogType.Debug:
						return logImplementation.IsDebugEnabled;
					case LogType.Error:
						return logImplementation.IsErrorEnabled;
					case LogType.Fatal:
						return logImplementation.IsFatalEnabled;
					case LogType.Info:
						return logImplementation.IsInfoEnabled;
					case LogType.Warn:
						return logImplementation.IsWarnEnabled;
					case LogType.Todo:
						// Map TODO to WARN.
						return logImplementation.IsWarnEnabled;

					default:
					case LogType.Unknown:
						Debug.Assert(
							false,
							string.Format( @"Unknown log type '{0}'.", logType ) );
						return logImplementation.IsDebugEnabled;
				}
			}
		}

		/// <summary>
		/// Central function for actually logging to the targets.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		/// <param name="logType">Type of the log.</param>
		private void DoPerformLog(
			string message,
			Exception t,
			ILoggingUserDefinedInformation userDefinedInformation,
			LogType logType )
		{
			// Only log if enabled at all.
			if ( IsLoggingEnabled( logType ) )
			{
				// Avoid endless recursion in certain circumstances.
				// No "lock" required, since already done in the calling function.
				if ( !CheckSetIsInsideLogging() )
				{
					isInsideLogging = true;
					try
					{
						// Never allow passwords to be displayed, 
						// no matter where.
						message = ProtectPasswords( message );

						// Prefix for TODO.
						if ( logType == LogType.Todo )
						{
							if ( string.IsNullOrEmpty( message ) )
							{
								message = todoPrefix;
							}
							else
							{
								message = todoPrefix + @" " + message;
							}
						}

						// --
						// 2006-11-03. Add at top to ease reading.

						if ( string.IsNullOrEmpty( message ) )
						{
							message = string.Empty;
						}
						else
						{
							message += Environment.NewLine + Environment.NewLine;
						}

						message +=
							/*MakeInnerExceptionsTrace( t, true ) +*/
							MakeTraceMessage( t ) +
							Environment.NewLine;

						// --

						string originalMessage = message == null ? null : message;

						message = DispatchRequestMoreInformationEvents(
							logType,
							originalMessage,
							message,
							t,
							userDefinedInformation );

						// --

						switch ( logType )
						{
							case LogType.Debug:
								logImplementation.Debug( message, t );
								break;
							case LogType.Error:
								logImplementation.Error( message, t );
								break;
							case LogType.Fatal:
								logImplementation.Fatal( message, t );
								break;
							case LogType.Info:
								logImplementation.Info( message, t );
								break;
							case LogType.Warn:
								logImplementation.Warn( message, t );
								break;
							case LogType.Todo:
								// Map TODO to WARN.
								logImplementation.Warn( message, t );
								break;

							default:
							case LogType.Unknown:
								Debug.Assert(
									false,
									string.Format(
									@"Unknown log type '{0}'.",
									logType ) );
								logImplementation.Debug( message, t );
								break;
						}

						// --

						string traceMessage =
							MakeTraceMessage( message, t );

						Debug.WriteLine( traceMessage );
						Trace.WriteLine( traceMessage );

						if ( Log != null )
						{
							Log(
								this,
								new LogCentralLogEventArgs(
								loggerName,
								logType,
								originalMessage,
								traceMessage,
								t,
								userDefinedInformation ) );
						}
					}
					finally
					{
						isInsideLogging = false;
					}
				}
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="loggerName">The name of the logger. Pass NULL
		/// or empty string for the root logger.</param>
		internal Logger(
			string loggerName )
		{
			if ( loggerName == null || loggerName.Trim().Length <= 0 )
			{
				this.loggerName = null;
			}
			else
			{
				this.loggerName = loggerName.Trim();
			}

			// --

			if ( this.loggerName == null )
			{
				// Global.
				logImplementation = log4net.LogManager.GetLogger(
					MethodBase.GetCurrentMethod().
					DeclaringType );
			}
			else
			{
				// Child.
				logImplementation = log4net.LogManager.GetLogger(
					loggerName );
			}

			// --

			lock ( typeLock )
			{
				if ( loggerName != null )
				{
					// Add to static map.
					loggers[this.loggerName] = this;
				}
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="loggerName">Name of the logger.</param>
		/// <param name="logImplementation">The log implementation.</param>
		private Logger(
			string loggerName,
			log4net.ILog logImplementation )
		{
			this.loggerName = loggerName;
			this.logImplementation = logImplementation;
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~Logger()
		{
			lock ( typeLock )
			{
				if ( loggerName != null )
				{
					if ( loggers.ContainsKey( loggerName ) )
					{
						loggers.Remove( loggerName );
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private static object typeLock = new object();

		/// <summary>
		/// Get a certain logger by a specified name.
		/// </summary>
		/// <param name="loggerName">Name of the logger.</param>
		/// <returns></returns>
		protected static Logger GetLoggerByName(
			string loggerName )
		{
			if ( string.IsNullOrEmpty( loggerName ) )
			{
				return null;
			}
			else
			{
				lock ( typeLock )
				{
					if ( loggers.ContainsKey( loggerName ) )
					{
						return loggers[loggerName];
					}
					else
					{
						log4net.ILog logImplementation =
							log4net.LogManager.GetLogger(
							loggerName );

						if ( logImplementation == null )
						{
							return null;
						}
						else
						{
							Logger logger = new Logger(
								loggerName,
								logImplementation );

							// Add to static map.
							loggers[loggerName] = logger;

							return logger;
						}
					}
				}
			}
		}

		/// <summary>
		/// Stores all loggers.
		/// </summary>
		private static Dictionary<string, Logger> loggers =
			new Dictionary<string, Logger>();

		/// <summary>
		/// The actual connection to LOG4NET.
		/// </summary>
		protected log4net.ILog logImplementation = null;

		/// <summary>
		/// By using this flag, we avoid stack overflow inside the called
		/// event handlers during logging.
		/// </summary>
		private bool _isInsideLogging = false;

		/// <summary>
		/// My own lock.
		/// Best practice, see C# MSDN documentation of the "lock" keyword.
		/// </summary>
		private object thisInsideLoggingLock = new object();

		/// <summary>
		/// Checks the set is inside logging.
		/// </summary>
		/// <returns></returns>
		private bool CheckSetIsInsideLogging()
		{
			lock ( thisInsideLoggingLock )
			{
				if ( _isInsideLogging )
				{
					return true;
				}
				else
				{
					_isInsideLogging = true;
					return false;
				}
			}
		}

		/// <summary>
		/// By using this flag, we avoid stack overflow inside the called
		/// event handlers during logging.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is inside logging; otherwise, <c>false</c>.
		/// </value>
		private bool isInsideLogging
		{
			set
			{
				lock ( thisInsideLoggingLock )
				{
					_isInsideLogging = value;
				}
			}
		}

		/// <summary>
		/// The name of the logger.
		/// </summary>
		private string loggerName;

		/// <summary>
		/// The name of the logger.
		/// </summary>
		/// <value>The name of the logger.</value>
		public string LoggerName
		{
			get
			{
				return loggerName;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private const string todoPrefix = @"[TODO]";

		/// <summary>
		/// Combines the more information strings and the regular
		/// message string.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="moreInformationMessage">The more information message.</param>
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
						Environment.NewLine +
						Environment.NewLine +
						Indent( moreInformationMessage.Trim() );
				}
			}
		}

		/// <summary>
		/// Dispatches the RequestMoreInformation event to
		/// all connected event handlers (if any).
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="originalMessage">The original message.</param>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		/// <returns></returns>
		private string DispatchRequestMoreInformationEvents(
			LogType type,
			string originalMessage,
			string message,
			Exception t,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			if ( requestMoreInformationEvents == null ||
				requestMoreInformationEvents.Count <= 0 )
			{
				return message;
			}
			else
			{
				string cumulatedMessage = message;

				foreach ( LogCentralRequestMoreInformationEventHandler evt
					in requestMoreInformationEvents )
				{
					if ( evt != null )
					{
						LogCentralRequestMoreInformationEventArgs args =
							new LogCentralRequestMoreInformationEventArgs(
							loggerName,
							type,
							originalMessage,
							message,
							t,
							userDefinedInformation );

						evt( this, args );

						// Yes, add additional information.
						cumulatedMessage =
							CombineMoreInformationMessage(
							cumulatedMessage,
							args.MoreInformationMessage );
					}
				}

				return cumulatedMessage;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Helper for converting exceptions to formatted strings.
		// ------------------------------------------------------------------

		/// <summary>
		/// Unwind all inner exceptions and add up their Message properties.
		/// </summary>
		/// <param name="t">The exception.</param>
		/// <returns>Returns the complete string.</returns>
		public static string MakeUserReadableExceptionMessage(
			Exception t )
		{
			return MakeUserReadableExceptionMessage( t, @" " );
		}

		/// <summary>
		/// Unwind all inner exceptions and add up their Message properties.
		/// </summary>
		/// <param name="t">The exception.</param>
		/// <param name="innerExceptionMessageSeparator">The string to
		/// concatenate two inner exceptions with.</param>
		/// <returns>Returns the complete string.</returns>
		public static string MakeUserReadableExceptionMessage(
			Exception t,
			string innerExceptionMessageSeparator )
		{
			StringBuilder result = new StringBuilder();

			while ( t != null )
			{
				if ( result.Length > 0 )
				{
					result.Append( innerExceptionMessageSeparator );
				}

				result.Append( t.Message );

				t = t.InnerException;
			}

			string r = result.ToString().Trim();

			if ( r.Length <= 0 )
			{
				return null;
			}
			else
			{
				return r;
			}
		}

		/// <summary>
		/// Make a string to trace from a given message and exception.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <returns></returns>
		public static string MakeTraceMessage(
			Exception t )
		{
			return MakeTraceMessage( null, t );
		}

		/// <summary>
		/// Make a string to trace from a given message and exception.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="x">The x.</param>
		/// <returns></returns>
		public static string MakeTraceMessage(
			string message,
			Exception x )
		{
			if ( string.IsNullOrEmpty( message ) )
			{
				if ( x == null )
				{
					return string.Empty;
				}
				else
				{
					StringBuilder s = new StringBuilder();
					s.AppendFormat(
						CultureInfo.CurrentCulture,
						@"{1}{0}{0}-----------------{0}{0}{2}{0}{0}-----------------{0}{0}{3}{0}{0}{4}",
						Environment.NewLine,
						x.GetType() == null || x.GetType().FullName == null ? string.Empty : x.GetType().FullName,
						x.Message == null ? string.Empty : x.Message.Trim(),
						x.StackTrace == null ? string.Empty : x.StackTrace.Trim(),
						DumpBuilder.Dump( x ) );

					string i = MakeInnerExceptionsTrace( x );

					if ( i != null && i.Trim().Length > 0 )
					{
						s.AppendFormat(
							"\r\n\t-->{0}",
							i );
					}

					// 2007-01-30.
					Exception xi = x.InnerException;
					while ( xi != null )
					{
						s.Append( MakeTraceMessage( null, xi ) );
						xi = xi.InnerException;
					}

					return s.ToString();
				}
			}
			else
			{
				if ( x == null )
				{
					return message.ToString();
				}
				else
				{
					// Comment.
					StringBuilder s = new StringBuilder();
					s.AppendFormat(
						CultureInfo.CurrentCulture,
						@"{1}{0}{0}{2}{0}{0}-----------------{0}{0}{3}{0}{0}-----------------{0}{0}{4}{0}{0}{5}",
						Environment.NewLine,
						message,
						x.GetType() == null || x.GetType().FullName == null ? string.Empty : x.GetType().FullName,
						x.Message == null ? string.Empty : x.Message.Trim(),
						x.StackTrace == null ? string.Empty : x.StackTrace.Trim(),
						DumpBuilder.Dump( x ) );

					string i = MakeInnerExceptionsTrace( x );

					if ( !string.IsNullOrEmpty( i ) )
					{
						s.AppendFormat(
							"\r\n\t-->{0}",
							i );
					}

					// 2007-01-30.
					Exception xi = x.InnerException;
					while ( xi != null )
					{
						s.Append( MakeTraceMessage( null, xi ) );
						xi = xi.InnerException;
					}

					return s.ToString();
				}
			}
		}

		/// <summary>
		/// Make a string to trace from a given message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns></returns>
		public static string MakeTraceMessage(
			string message )
		{
			return message == null ? string.Empty : message.ToString();
		}

		/// <summary>
		/// Never allow passwords to be displayed, no matter where.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns></returns>
		public static string ProtectPasswords(
			string message )
		{
			if ( message == null || message.ToString().Length <= 0 ||
				LibraryConfiguration.Current.DisableLoggingPasswordProtection )
			{
				return message;
			}
			else
			{
				bool hasMatch = false;

				string checkMessage = message.ToLowerInvariant();

				// Do a first quick check without RX, for speed.
				foreach ( string s in pwProtectQuickPatterns )
				{
					if ( checkMessage.Contains( s ) )
					{
						hasMatch = true;
						break;
					}
				}

				if ( hasMatch || Regex.IsMatch(
					message.ToString(),
					pwProtectPattern,
					RegexOptions.IgnoreCase |
					RegexOptions.Singleline |
					RegexOptions.Compiled ) )
				{
					return string.Format(
						@"##### Due to security restrictions (illegal words " +
						@"contained), the message to log has been discarded. " +
						@"The message contained {0} characters.",
						message.ToString().Length );
				}
				else
				{
					return message;
				}
			}
		}

		/// <summary>
		/// Indents block of lines.
		/// </summary>
		/// <param name="textToIndent">The textToIndent to indent.</param>
		/// <returns>Returns the indented textToIndent.</returns>
		public static string Indent(
			string textToIndent )
		{
			return Indent( textToIndent, @"    " );
		}

		/// <summary>
		/// Indents block of lines.
		/// </summary>
		/// <param name="textToIndent">The textToIndent to indent.</param>
		/// <param name="linePrefix">The prefix to add before every
		/// found line.</param>
		/// <returns>Returns the indented textToIndent.</returns>
		public static string Indent(
			string textToIndent,
			string linePrefix )
		{
			if ( textToIndent == null )
			{
				return textToIndent;
			}
			else
			{
				textToIndent = textToIndent.Replace(
					@"" + Environment.NewLine, "\n" );
				textToIndent = textToIndent.Replace( '\r', '\n' );

				if ( textToIndent.IndexOf( '\n' ) < 0 )
				{
					return linePrefix + textToIndent;
				}
				else
				{
					string[] lines = textToIndent.Split( '\n' );

					StringBuilder result = new StringBuilder();

					foreach ( string line in lines )
					{
						if ( result.Length > 0 )
						{
							result.Append( Environment.NewLine );
						}

						result.Append( linePrefix );
						result.Append( line );
					}

					return result.ToString();
				}
			}
		}

		/// <summary>
		/// Add up the inner exception texts.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <returns></returns>
		private static string MakeInnerExceptionsTrace(
			Exception x )
		{
			return MakeInnerExceptionsTrace( x, false );
		}

		/// <summary>
		/// Add up the inner exception texts.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="includeExceptionItself">if set to <c>true</c> [include exception itself].</param>
		/// <returns></returns>
		private static string MakeInnerExceptionsTrace(
			Exception x,
			bool includeExceptionItself )
		{
			if ( x == null )
			{
				return null;
			}
			else
			{
				if ( !includeExceptionItself )
				{
					x = x.InnerException;
				}

				// --

				if ( x == null )
				{
					return null;
				}
				else
				{
					string s1 = x.Message;
					string s2 = MakeInnerExceptionsTrace( x );

					if ( s2 != null && s2.Length > 0 )
					{
						s1 += string.Format(
							@"--> {0}",
							s2 );
					}

					return s1;
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// For a first check of password, without the need for RX.
		/// </summary>
		private static readonly string[] pwProtectQuickPatterns =
			new string[] 
			{ 
				@"pass",
				@"kennwort",
				@"pwd"
			};

		/// <summary>
		/// For protecting logged messages against passwords being made
		/// visible.
		/// </summary>
		private const string pwProtectPattern =
			@"\b(?:password|pass|passwort|kennwort|pwd)\b";

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}