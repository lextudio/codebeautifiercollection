namespace ZetaLib.Windows.Common
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Diagnostics;
	using System.Windows;
	using System.Windows.Forms;
	using ZetaLib.Core.Common;
	using System.Threading;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// A class to use like MFC's CWaitCursor.
	/// </summary>
	/// <example>using ( new WaitCursor( this ) ) { ... }</example>
	public sealed class WaitCursor :
		IDisposable
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor. Uses the current active form.
		/// </summary>
		public WaitCursor()
		{
			Init( Form.ActiveForm, WaitCursorOption.Standard );
		}

		/// <summary>
		/// Constructor. Uses the current active form.
		/// </summary>
		/// <param name="options">The options.</param>
		public WaitCursor(
			WaitCursorOption options )
		{
			Init( Form.ActiveForm, options );
		}

		/// <summary>
		/// Constructor. Uses the given form.
		/// </summary>
		/// <param name="owner">The form to set/reset the cursor.</param>
		public WaitCursor(
			Form owner )
		{
			Init( owner, WaitCursorOption.Standard );
		}

		/// <summary>
		/// Constructor. Uses the given form.
		/// </summary>
		/// <param name="owner">The form to set/reset the cursor.</param>
		/// <param name="options">The options.</param>
		public WaitCursor(
			Form owner,
			WaitCursorOption options )
		{
			Init( owner, options );
		}

		/// <summary>
		/// Constructor. Uses the given form.
		/// </summary>
		/// <param name="owner">The form to set/reset the cursor.</param>
		public WaitCursor(
			Control owner )
		{
			Init( owner, WaitCursorOption.Standard );
		}

		/// <summary>
		/// Constructor. Uses the given form.
		/// </summary>
		/// <param name="owner">The form to set/reset the cursor.</param>
		/// <param name="options">The options.</param>
		public WaitCursor(
			Control owner,
			WaitCursorOption options )
		{
			Init( owner, options );
		}

		/// <summary>
		/// Constructor. Uses the given form.
		/// </summary>
		/// <param name="owner">The form to set/reset the cursor.</param>
		public WaitCursor(
			IWin32Window owner )
		{
			Debug.Assert(
				owner is Form ||
				owner is Control,
				@"The passed IWin32Window is not a Form or Control." );

			Init( owner as Form, WaitCursorOption.Standard );
		}

		/// <summary>
		/// Constructor. Uses the given form.
		/// </summary>
		/// <param name="owner">The form to set/reset the cursor.</param>
		/// <param name="options">The options.</param>
		public WaitCursor(
			IWin32Window owner,
			WaitCursorOption options )
		{
			Debug.Assert(
				owner is Form ||
				owner is Control,
				@"The passed IWin32Window is not a Form or Control." );

			Init( owner as Form, options );
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="WaitCursor"/> is reclaimed by garbage collection.
		/// </summary>
		~WaitCursor()
		{
			FreeResources();
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Inits the specified owner.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="options">The options.</param>
		private void Init(
			Control owner,
			WaitCursorOption options )
		{
			if ( owner != null )
			{
				if ( !(owner is Form) )
				{
					Form form = owner.FindForm();
					if ( form != null )
					{
						owner = form;
					}
				}

				startDate = DateTime.Now;

				this.owner = owner;
				this.previousCursor = this.owner.Cursor;
				this.owner.Cursor = Cursors.WaitCursor;

				if ( (options & WaitCursorOption.ShortSleep) != 0 )
				{
					Thread.Sleep( 100 );
				}
				if ( (options & WaitCursorOption.DoEvents) != 0 )
				{
					Application.DoEvents();
				}
			}
		}

		/// <summary>
		/// Frees the resources.
		/// </summary>
		private void FreeResources()
		{
			endDate = DateTime.Now;

			// TODO: Implement tip from:
			// http://www.codeproject.com/script/comments/forums.asp?msg=1100778&forumid=1649#xx1100778xx

			if ( owner != null )
			{
				owner.Cursor = previousCursor;
				owner = null;
			}
		}

		private Cursor previousCursor = null;
		private Control owner = null;

		private DateTime startDate;
		private DateTime endDate;

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
			FreeResources();
			GC.SuppressFinalize( this );
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}