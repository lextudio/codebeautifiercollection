namespace ZetaLib.Core.Logging
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Arguments to the log central event.
	/// </summary>
	public class LogCentralLogEventArgs :
		EventArgs
	{
		#region Constructors.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="loggerName">Name of the logger.</param>
		/// <param name="type">The type.</param>
		/// <param name="originalMessage">The original message.</param>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public LogCentralLogEventArgs(
			string loggerName,
			LogType type,
			string originalMessage,
			string message,
			Exception t,
			ILoggingUserDefinedInformation userDefinedInformation )
		{
			this.loggerName = loggerName;
			this.type = type;
			this.originalMessage = originalMessage;
			this.message = message;
			this.error = t;
			this.userDefinedInformation = userDefinedInformation;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="loggerName">Name of the logger.</param>
		/// <param name="type">The type.</param>
		/// <param name="originalMessage">The original message.</param>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		public LogCentralLogEventArgs(
			string loggerName,
			LogType type,
			string originalMessage,
			string message,
			Exception t )
		{
			this.loggerName = loggerName;
			this.type = type;
			this.originalMessage = originalMessage;
			this.message = message;
			this.error = t;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="loggerName">Name of the logger.</param>
		/// <param name="type">The type.</param>
		/// <param name="originalMessage">The original message.</param>
		/// <param name="message">The message.</param>
		public LogCentralLogEventArgs(
			string loggerName,
			LogType type,
			string originalMessage,
			string message )
		{
			this.loggerName = loggerName;
			this.type = type;
			this.originalMessage = originalMessage;
			this.message = message;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="loggerName">Name of the logger.</param>
		/// <param name="type">The type.</param>
		public LogCentralLogEventArgs(
			string loggerName,
			LogType type )
		{
			this.loggerName = loggerName;
			this.type = type;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="loggerName">Name of the logger.</param>
		public LogCentralLogEventArgs(
			string loggerName )
		{
			this.loggerName = loggerName;
			this.type = LogType.Unknown;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

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
		/// The type of log event.
		/// </summary>
		/// <value>The type.</value>
		public LogType Type
		{
			get
			{
				return type;
			}
		}

		/// <summary>
		/// The original message that is logged. You cannot modify the
		/// message in your event handler, because the logging
		/// already has occured.
		/// </summary>
		/// <value>The original message.</value>
		public string OriginalMessage
		{
			get
			{
				return originalMessage;
			}
		}

		/// <summary>
		/// The message that is logged. You cannot modify the
		/// message in your event handler, because the logging
		/// already has occured.
		/// </summary>
		/// <value>The message.</value>
		public string Message
		{
			get
			{
				return message;
			}
		}

		/// <summary>
		/// The exception that occured.
		/// </summary>
		/// <value>The error.</value>
		public Exception Error
		{
			get
			{
				return error;
			}
		}

		/// <summary>
		/// Provide optional additional user-defined information.
		/// </summary>
		/// <value>The user defined information.</value>
		public ILoggingUserDefinedInformation UserDefinedInformation
		{
			get
			{
				return userDefinedInformation;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Make them private for read-only.
		/// </summary>
		private string loggerName;
		private readonly LogType type;
		private readonly string originalMessage;
		private readonly string message;
		private readonly Exception error;
		private readonly ILoggingUserDefinedInformation userDefinedInformation;

		// ------------------------------------------------------------------
		#endregion
	}

	/// <summary>
	/// Delegate that is called upon logging from LogXxx().
	/// </summary>
	public delegate void LogCentralLogEventHandler(
		object sender,
		LogCentralLogEventArgs e );

	/// <summary>
	/// Delegate that is called upon searching for the configuration file
	/// path while logging.
	/// </summary>
	public delegate string LogCentralFindConfigurationFilePathEventHandler(
		object sender );

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Arguments to the event that tries to query additional information
	/// about the environment or the current session info.
	/// </summary>
	public sealed class LogCentralRequestMoreInformationEventArgs :
		LogCentralLogEventArgs
	{
		#region Constructors.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="loggerName">Name of the logger.</param>
		/// <param name="type">The type.</param>
		/// <param name="originalMessage">The original message.</param>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		/// <param name="userDefinedInformation">The user defined information.</param>
		public LogCentralRequestMoreInformationEventArgs(
			string loggerName,
			LogType type,
			string originalMessage,
			string message,
			Exception t,
			ILoggingUserDefinedInformation userDefinedInformation )
			:
			base( loggerName, type, originalMessage, message, t, userDefinedInformation )
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="loggerName">Name of the logger.</param>
		/// <param name="type">The type.</param>
		/// <param name="originalMessage">The original message.</param>
		/// <param name="message">The message.</param>
		/// <param name="t">The t.</param>
		public LogCentralRequestMoreInformationEventArgs(
			string loggerName,
			LogType type,
			string originalMessage,
			string message,
			Exception t )
			:
			base( loggerName, type, originalMessage, message, t )
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="loggerName">Name of the logger.</param>
		/// <param name="type">The type.</param>
		/// <param name="originalMessage">The original message.</param>
		/// <param name="message">The message.</param>
		public LogCentralRequestMoreInformationEventArgs(
			string loggerName,
			LogType type,
			string originalMessage,
			string message )
			:
			base( loggerName, type, originalMessage, message )
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="loggerName">Name of the logger.</param>
		/// <param name="type">The type.</param>
		public LogCentralRequestMoreInformationEventArgs(
			string loggerName,
			LogType type )
			:
			base( loggerName, type )
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="loggerName">Name of the logger.</param>
		public LogCentralRequestMoreInformationEventArgs(
			string loggerName )
			:
			base( loggerName )
		{
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// The event handler that gets this object passed can
		/// place additional information here.
		/// </summary>
		/// <value>The more information message.</value>
		public string MoreInformationMessage
		{
			get
			{
				return moreInformationMessage;
			}
			set
			{
				moreInformationMessage = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private member.
		// ------------------------------------------------------------------

		/// <summary>
		/// The event handler that gets this object passed can
		/// place additional information here.
		/// </summary>
		private string moreInformationMessage;

		// ------------------------------------------------------------------
		#endregion
	}

	/// <summary>
	/// Delegate that is called when the logging framework tries to query 
	/// additional information about the environment or the current session 
	/// info. Useful e.g. for passing infos about the currently logged in
	/// user or other things.
	/// </summary>
	public delegate void LogCentralRequestMoreInformationEventHandler(
		object sender,
		LogCentralRequestMoreInformationEventArgs e );

	/////////////////////////////////////////////////////////////////////////
}