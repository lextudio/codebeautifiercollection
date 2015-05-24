namespace ZetaLib.Core.Caching
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Threading;
	using ZetaLib.Core.Common;
	using System.Configuration;
	using ZetaLib.Core.Logging;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Cache that cleans up asynchronously in a background thread.
	/// </summary>
	/// <remarks>
	/// Configure externally through the configuration app setting values:
	/// - "asyncThreadCleanupInterval": 
	///     The interval in milliseconds. Default is 60,000 (60 seconds).
	/// - "disableAsyncThreadCleanup":
	///     Disables the asynchron handling, behaves synchron. 
	///     Default is FALSE.
	/// </remarks>
	public class AsyncCleanupCacheBackend :
		SimpleCacheBackend,
		IDisposable
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public AsyncCleanupCacheBackend()
		{
			if ( isAsyncEnabled )
			{
				LogCentral.Current.LogDebug(
					string.Format(
					@"[Cache] Constructing asynchron cache backend with 'isAsyncEnabled' set to {0}.",
					isAsyncEnabled ) );

				TimerCallback timerDelegate =
					new TimerCallback( DoAsyncCleanup );

				// Create new timer to regularily execute the cleanup.
				asyncTimer = new Timer(
					DoAsyncCleanup,
					null,
					timerIntervalMilliSeconds,
					timerIntervalMilliSeconds
					);
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Function that is being called after a value is being set.
		/// The default checks syncronously for old items and deletes them.
		/// Override to provide your own implementation.
		/// </summary>
		protected override void OnAfterValueSet()
		{
			if ( isAsyncEnabled )
			{
				base.OnAfterValueSet();
			}
			else
			{
				// Do NOT call the base class here to prevent
				// syncronous update.
			}
		}

		/// <summary>
		/// Does the async cleanup.
		/// </summary>
		/// <param name="state">The state.</param>
		private void DoAsyncCleanup(
			object state )
		{
			lock ( thisLock )
			{
				DateTime d1 = DateTime.Now;
				LogCentral.Current.LogDebug(
					string.Format(
					@"[Cache] Starting asnychron cleanup of cache at '{0}'.",
					d1 ) );

				int removedCount = CleanupOldEntries();

				DateTime d2 = DateTime.Now;
				LogCentral.Current.LogDebug(
					string.Format(
					@"[Cache] Finished asnychron cleanup of cache at '{0}', removed {1} items, took '{2}'.",
					d2,
					removedCount,
					d2 - d1 ) );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// Best practice, see C# MSDN documentation of the "lock" keyword.
		/// </summary>
		private object thisLock = new object();

		/// <summary>
		/// The timer that regularily executes the cleanup.
		/// </summary>
		private Timer asyncTimer;

		/// <summary>
		/// Once a minute by default, changeable via config.
		/// </summary>
		private static readonly int timerIntervalMilliSeconds =
			ConvertHelper.ToInt32(
			ConfigurationManager.AppSettings[@"asyncThreadCleanupInterval"],
			60 * 1000 );

		/// <summary>
		/// Flag to disable.
		/// </summary>
		private static readonly bool isAsyncEnabled =
			!ConvertHelper.ToBoolean(
			ConfigurationManager.AppSettings[@"disableAsyncThreadCleanup"],
			false );

		// ------------------------------------------------------------------
		#endregion

		#region IDisposable members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Performs application-defined tasks associated with freeing,
		/// releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if ( asyncTimer != null )
			{
				Timer t = asyncTimer;
				asyncTimer = null;

				t.Dispose();
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}