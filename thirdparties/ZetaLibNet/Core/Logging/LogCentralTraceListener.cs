namespace ZetaLib.Core.Logging
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Diagnostics;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Listener to redirect the Debug.Assert + co. to the logging classes.
	/// </summary>
	internal class LogCentralTraceListener :
		TraceListener
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Emits an error message to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener"></see> class.
		/// </summary>
		/// <param name="message">A message to emit.</param>
		public override void Fail(
			string message )
		{
			base.Fail( message );

			DoFail( message, null );
		}

		/// <summary>
		/// Emits an error message and a detailed error message to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener"></see> class.
		/// </summary>
		/// <param name="message">A message to emit.</param>
		/// <param name="detailMessage">A detailed message to emit.</param>
		public override void Fail(
			string message,
			string detailMessage )
		{
			base.Fail( message, detailMessage );

			DoFail( message, detailMessage );
		}

		/// <summary>
		/// When overridden in a derived class, writes the specified message to the listener you create in the derived class.
		/// </summary>
		/// <param name="message">A message to write.</param>
		public override void Write(
			string message )
		{
			DoWrite( message );
		}

		/// <summary>
		/// When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.
		/// </summary>
		/// <param name="message">A message to write.</param>
		public override void WriteLine(
			string message )
		{
			DoWrite( message );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Best practice, see C# MSDN documentation of the "lock" keyword.
		/// </summary>
		private object thisLock = new object();

		/// <summary>
		/// Does the fail.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="detailMessage">The detail message.</param>
		private void DoFail(
			string message,
			string detailMessage )
		{
			lock ( thisLock )
			{
				if ( !isInsideLog )
				{
					isInsideLog = true;
					try
					{
						string logMessage = message;

						if ( string.IsNullOrEmpty( logMessage ) )
						{
							logMessage = @"(no assertion message was provided)";
						}

						if ( !string.IsNullOrEmpty( detailMessage ) )
						{
							logMessage += string.Format( @",{0}", detailMessage );
						}

						logMessage = logMessage.TrimEnd( '.' ) + @".";

						LogCentral.Current.LogDebug(
							string.Format(
							@"Assertion failed: {0}",
							logMessage ) );

						LogCentral.Current.LogDebug(
							string.Format(
							@"Stack trace of the assertion: {0}",
							Environment.StackTrace ) );
					}
					finally
					{
						isInsideLog = false;
					}
				}
			}
		}

		/// <summary>
		/// Does the write.
		/// </summary>
		/// <param name="message">The message.</param>
		private void DoWrite(
			string message )
		{
			lock ( thisLock )
			{
				if ( !isInsideLog )
				{
					isInsideLog = true;
					try
					{
						LogCentral.Current.LogInfo( message );
					}
					finally
					{
						isInsideLog = false;
					}
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private bool isInsideLog = false;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}