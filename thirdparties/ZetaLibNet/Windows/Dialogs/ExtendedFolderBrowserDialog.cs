namespace ZetaLib.Windows.Dialogs
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.ComponentModel;
	using System.Drawing.Design;
	using System.Runtime.InteropServices;
	using System.Security.Permissions;
	using System.Threading;
	using System.Windows.Forms;
	using ZetaLib.Windows.Common;

	// ----------------------------------------------------------------------
	#endregion
	
	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Prompts the user to select a folder. This class cannot be inherited.
	/// </summary>
	[
	DefaultEvent( @"HelpRequest" ),
	DefaultProperty( @"SelectedPath" )
	]
	public sealed class ExtendedFolderBrowserDialog :
		CommonDialog
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Windows.Forms.ExtendedFolderBrowserDialog"></see> class.
		/// </summary>
		public ExtendedFolderBrowserDialog()
		{
			this.Reset();
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Extendeds the folder browser dialog_ browse callback proc.
		/// </summary>
		/// <param name="hwnd">The HWND.</param>
		/// <param name="msg">The MSG.</param>
		/// <param name="lParam">The l param.</param>
		/// <param name="lpData">The lp data.</param>
		/// <returns></returns>
		private int ExtendedFolderBrowserDialog_BrowseCallbackProc(
			IntPtr hwnd,
			int msg,
			IntPtr lParam,
			IntPtr lpData )
		{
			switch ( msg )
			{
				case 1:
					if ( this.selectedPath.Length != 0 )
					{
						Win32.SendMessage(
							new HandleRef( null, hwnd ),
							Win32.BFFM_SETSELECTION,
							1,
							this.selectedPath );
					}
					break;

				case 2:
					{
						IntPtr ptr1 = lParam;
						if ( ptr1 != IntPtr.Zero )
						{
							IntPtr ptr2 = Marshal.AllocHGlobal( (int)(260 * Marshal.SystemDefaultCharSize) );
							bool flag1 = Win32.Shell32.SHGetPathFromIDList( ptr1, ptr2 );
							Marshal.FreeHGlobal( ptr2 );
							Win32.SendMessage(
								new HandleRef( null, hwnd ),
								Win32.BFFM_ENABLEOK,
								0,
								flag1 ? 1 : 0 );
						}
						break;
					}
			}
			return 0;
		}

		/// <summary>
		/// Gets the SH malloc.
		/// </summary>
		/// <returns></returns>
		private static Win32.IMalloc GetSHMalloc()
		{
			Win32.IMalloc[] mallocArray1 = new Win32.IMalloc[1];
			Win32.Shell32.SHGetMalloc( mallocArray1 );
			return mallocArray1[0];
		}

		/// <summary>
		/// Resets properties to their default values.
		/// </summary>
		public override void Reset()
		{
			this.rootFolder = Environment.SpecialFolder.Desktop;
			this.descriptionText = string.Empty;
			this.selectedPath = string.Empty;
			this.selectedPathNeedsCheck = false;
			this.showNewFolderButton = true;
			this.showEditBox = false;
		}

		/// <summary>
		/// Runs the dialog.
		/// </summary>
		/// <param name="hWndOwner">The h WND owner.</param>
		/// <returns></returns>
		protected override bool RunDialog(
			IntPtr hWndOwner )
		{
			IntPtr ptr1 = IntPtr.Zero;
			bool flag1 = false;
			Win32.Shell32.SHGetSpecialFolderLocation( hWndOwner, (int)this.rootFolder, ref ptr1 );
			if ( ptr1 == IntPtr.Zero )
			{
				Win32.Shell32.SHGetSpecialFolderLocation( hWndOwner, 0, ref ptr1 );
				if ( ptr1 == IntPtr.Zero )
				{
					throw new InvalidOperationException( "ExtendedFolderBrowserDialogNoRootFolder" );
				}
			}
			int flags = 0x40;
			if ( !this.showNewFolderButton )
			{
				flags += 0x200;
			}
			if ( this.showEditBox )
			{
				flags += Win32.BIF_EDITBOX;
			}
			if ( Control.CheckForIllegalCrossThreadCalls && (Application.OleRequired() != ApartmentState.STA) )
			{
				throw new ThreadStateException( "ThreadMustBeSTA" );
			}

			IntPtr ptr2 = IntPtr.Zero;
			IntPtr ptr3 = IntPtr.Zero;
			IntPtr ptr4 = IntPtr.Zero;
			try
			{
				Win32.BROWSEINFO browseinfo1 = new Win32.BROWSEINFO();
				ptr3 = Marshal.AllocHGlobal( (int)(260 * Marshal.SystemDefaultCharSize) );
				ptr4 = Marshal.AllocHGlobal( (int)(260 * Marshal.SystemDefaultCharSize) );
				Win32.BrowseCallbackProc proc1 = new Win32.BrowseCallbackProc(
					this.ExtendedFolderBrowserDialog_BrowseCallbackProc );
				browseinfo1.pidlRoot = ptr1;
				browseinfo1.hwndOwner = hWndOwner;
				browseinfo1.pszDisplayName = ptr3;
				browseinfo1.lpszTitle = this.descriptionText;
				browseinfo1.ulFlags = flags;
				browseinfo1.lpfn = proc1;
				browseinfo1.lParam = IntPtr.Zero;
				browseinfo1.iImage = 0;
				ptr2 = Win32.Shell32.SHBrowseForFolder( browseinfo1 );
				if ( ptr2 != IntPtr.Zero )
				{
					Win32.Shell32.SHGetPathFromIDList( ptr2, ptr4 );
					this.selectedPathNeedsCheck = true;
					this.selectedPath = Marshal.PtrToStringAuto( ptr4 );
					flag1 = true;
				}
			}
			finally
			{
				Win32.IMalloc malloc1 = GetSHMalloc();
				malloc1.Free( ptr1 );
				if ( ptr2 != IntPtr.Zero )
				{
					malloc1.Free( ptr2 );
				}
				if ( ptr4 != IntPtr.Zero )
				{
					Marshal.FreeHGlobal( ptr4 );
				}
				if ( ptr3 != IntPtr.Zero )
				{
					Marshal.FreeHGlobal( ptr3 );
				}
			}
			return flag1;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets or sets the descriptive text displayed above the tree view control in the dialog box.
		/// </summary>
		/// <value>The description.</value>
		/// <returns>The description to display. The default is an empty string ("").</returns>
		[
		Browsable( true ),
		Localizable( true ),
		DefaultValue( @"" )
		]
		public string Description
		{
			get
			{
				return this.descriptionText;
			}
			set
			{
				this.descriptionText = (value == null) ? string.Empty : value;
			}
		}

		/// <summary>
		/// Gets or sets the root folder where the browsing starts from.
		/// </summary>
		/// <value>The root folder.</value>
		/// <returns>One of the <see cref="T:System.Environment.SpecialFolder"></see> values. The default is Desktop.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Environment.SpecialFolder"></see> values. </exception>
		[
		Localizable( false ),
		DefaultValue( 0 ),
		Browsable( true )]
		public Environment.SpecialFolder RootFolder
		{
			get
			{
				return this.rootFolder;
			}
			set
			{
				if ( !Enum.IsDefined( typeof( Environment.SpecialFolder ), value ) )
				{
					throw new InvalidEnumArgumentException( "value", (int)value, typeof( Environment.SpecialFolder ) );
				}
				this.rootFolder = value;
			}
		}

		/// <summary>
		/// Gets or sets the path selected by the user.
		/// </summary>
		/// <value>The selected path.</value>
		/// <returns>The path of the folder first selected in the dialog box or the last folder selected by the user. The default is an empty string ("").</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[DefaultValue( @"" ), Browsable( true ),
		Editor( @"System.Windows.Forms.Design.SelectedPathEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
			typeof( UITypeEditor ) ),
		Localizable( true )]
		public string SelectedPath
		{
			get
			{
				if ( ((this.selectedPath != null) && (this.selectedPath.Length != 0)) && this.selectedPathNeedsCheck )
				{
					new FileIOPermission( FileIOPermissionAccess.PathDiscovery, this.selectedPath ).Demand();
				}
				return this.selectedPath;
			}
			set
			{
				this.selectedPath = (value == null) ? string.Empty : value;
				this.selectedPathNeedsCheck = false;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the New Folder button appears in the folder browser dialog box.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [show new folder button]; otherwise, <c>false</c>.
		/// </value>
		/// <returns>true if the New Folder button is shown in the dialog box; otherwise, false. The default is true.</returns>
		[Browsable( true ), DefaultValue( true ), Localizable( false )]
		public bool ShowNewFolderButton
		{
			get
			{
				return this.showNewFolderButton;
			}
			set
			{
				this.showNewFolderButton = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether [show edit box].
		/// </summary>
		/// <value><c>true</c> if [show edit box]; otherwise, <c>false</c>.</value>
		[Browsable( true ), DefaultValue( false ), Localizable( false )]
		public bool ShowEditBox
		{
			get
			{
				return this.showEditBox;
			}
			set
			{
				this.showEditBox = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private string descriptionText;
		private Environment.SpecialFolder rootFolder;
		private string selectedPath;
		private bool selectedPathNeedsCheck;
		private bool showNewFolderButton;
		private bool showEditBox;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}