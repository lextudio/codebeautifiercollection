namespace ZetaLib.Core.Base
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using ZetaLib.Core.Logging;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Generic base class for applications.
	/// </summary>
	public class GenericBase
	{
		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private static object typeLock = new object();

		/// <summary>
		/// Gets or sets the application environment.
		/// </summary>
		/// <value>The application environment.</value>
		public static IApplicationEnvironment ApplicationEnvironment
		{
			get
			{
				lock ( typeLock )
				{
					return applicationEnvironment;
				}
			}
			set
			{
				lock ( typeLock )
				{
					applicationEnvironment = value;
				}
			}
		}

		/// <summary>
		/// Gets the magic key.
		/// </summary>
		/// <value>The magic key.</value>
		public static string MagicKey
		{
			get
			{
				return @"DCBE5938EFC54967ABDDE79BA639128B";
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public GenericBase()
		{
			// Install a global handler.
			AppDomain.CurrentDomain.UnhandledException +=
				new UnhandledExceptionEventHandler( UnhandledException );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Handles the application error.
		/// </summary>
		/// <param name="e">The e.</param>
		protected virtual void HandleApplicationError(
			Exception e )
		{
			LogCentral.Current.LogError(
				@"Application error occured.",
				e );
		}

		/// <summary>
		/// Unhandleds the exception.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.UnhandledExceptionEventArgs"/> 
		/// instance containing the event data.</param>
		private void UnhandledException(
			object sender,
			UnhandledExceptionEventArgs e )
		{
			try
			{
				Exception x = e.ExceptionObject as Exception;
				HandleApplicationError( x );
			}
			catch ( Exception )
			{
				// Do nothing.
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private static IApplicationEnvironment
			applicationEnvironment;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}