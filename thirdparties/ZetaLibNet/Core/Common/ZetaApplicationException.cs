namespace ZetaLib.Core.Common
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Runtime.Serialization;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Enhanced base class for application exceptions.
	/// </summary>
	public class ZetaApplicationException :
		ApplicationException
	{
		#region Constructors.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public ZetaApplicationException() :
			base()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">The message.</param>
		public ZetaApplicationException(
			string message ) :
			base( message )
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		public ZetaApplicationException(
			SerializationInfo info,
			StreamingContext context ) :
			base( info, context )
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public ZetaApplicationException(
			string message,
			Exception innerException ) :
			base( message, innerException )
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="loggingBehaviour">The logging behaviour.</param>
		public ZetaApplicationException(
			LoggingBehaviourHint loggingBehaviour ) :
			base()
		{
			internalLoggingBehaviour = loggingBehaviour;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="loggingBehaviour">The logging behaviour.</param>
		public ZetaApplicationException(
			string message,
			LoggingBehaviourHint loggingBehaviour ) :
			base( message )
		{
			internalLoggingBehaviour = loggingBehaviour;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="info">The info.</param>
		/// <param name="context">The context.</param>
		/// <param name="loggingBehaviour">The logging behaviour.</param>
		public ZetaApplicationException(
			SerializationInfo info,
			StreamingContext context,
			LoggingBehaviourHint loggingBehaviour ) :
			base( info, context )
		{
			internalLoggingBehaviour = loggingBehaviour;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		/// <param name="loggingBehaviour">The logging behaviour.</param>
		public ZetaApplicationException(
			string message,
			Exception innerException,
			LoggingBehaviourHint loggingBehaviour ) :
			base( message, innerException )
		{
			internalLoggingBehaviour = loggingBehaviour;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public members.
		// ------------------------------------------------------------------

		/// <summary>
		/// The ways to behave when the exception is caught and the code
		/// must decide whether to log this exception or not.
		/// </summary>
		public enum LoggingBehaviourHint
		{
			#region Enum members.

			/// <summary>
			/// Do the default behavior. This depends on the
			/// code that actually catches the exception.
			/// </summary>
			Default,

			/// <summary>
			/// The catching code should log the exception.
			/// </summary>
			Log,

			/// <summary>
			/// The catching code should not log the exception.
			/// </summary>
			DontLog

			#endregion
		}

		/// <summary>
		/// The ways to behave when the exception is caught and the code
		/// must decide whether to log this exception or not.
		/// </summary>
		/// <value>The logging behaviour.</value>
		public LoggingBehaviourHint LoggingBehaviour
		{
			get
			{
				return internalLoggingBehaviour;
			}
			set
			{
				internalLoggingBehaviour = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Protected members.
		// ------------------------------------------------------------------

		/// <summary>
		/// The ways to behave when the exception is caught and the code
		/// must decide whether to log this exception or not.
		/// </summary>
		protected LoggingBehaviourHint internalLoggingBehaviour = 
			LoggingBehaviourHint.Default;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}