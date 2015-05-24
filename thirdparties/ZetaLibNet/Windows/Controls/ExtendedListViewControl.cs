namespace ZetaLib.Windows.Controls
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Runtime.InteropServices;
	using System.Windows.Forms;
	using System.Drawing;
	using ZetaLib.Windows.Base;
	using ZetaLib.Windows.Properties;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Extended list view control.
	/// Supports, beside other, sorting.
	/// </summary>
	public partial class ExtendedListViewControl :
		TriStateCheckBoxesListView
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public ExtendedListViewControl()
		{
		}

		/// <summary>
		/// Moves the specified node up by one,
		/// staying in the same level.
		/// Wraps if at the top.
		/// </summary>
		/// <param name="item">The item.</param>
		public void MoveItemUpByOne(
			ListViewItem item )
		{
			this.SuspendLayout();
			try
			{
				int itemIndex = item.Index;

				Items.RemoveAt( itemIndex );

				int newItemIndex = itemIndex - 1;
				if ( newItemIndex < 0 )
				{
					Items.Add( item );
				}
				else
				{
					Items.Insert( newItemIndex, item );
				}
			}
			finally
			{
				this.SuspendLayout();
			}
		}

		/// <summary>
		/// Moves the specified node down by one,
		/// staying in the same level.
		/// Wraps if at the top.
		/// </summary>
		/// <param name="item">The item.</param>
		public void MoveItemDownByOne(
			ListViewItem item )
		{
			this.SuspendLayout();
			try
			{
				int itemIndex = item.Index;

				Items.RemoveAt( itemIndex );

				int newItemIndex = itemIndex + 1;
				if ( newItemIndex > Items.Count )
				{
					Items.Insert( 0, item );
				}
				else if ( newItemIndex == Items.Count )
				{
					Items.Add( item );
				}
				else
				{
					Items.Insert( newItemIndex, item );
				}
			}
			finally
			{
				this.SuspendLayout();
			}
		}

		/// <summary>
		/// Ensures the items order positions set.
		/// </summary>
		public void EnsureItemsOrderPositionsSet()
		{
			int previousOrderPosition = -1;

			// Take the current item order of the items in the
			// list and ensure the order position is
			// ascending (not naturally immediate following numbers).
			int itemIndex = 0;
			while ( itemIndex < this.Items.Count )
			{
				ListViewItem listViewItem = this.Items[itemIndex];

				IOrderPosition obj = listViewItem.Tag as IOrderPosition;
				Debug.Assert( obj != null );

				int currentOrderPosition = obj.OrderPosition;

				// Must adjust.
				if ( currentOrderPosition <= previousOrderPosition )
				{
					// Increment.
					int newCurrentOrderPosition = previousOrderPosition + 1;

					if ( obj.OrderPosition != newCurrentOrderPosition )
					{
						// New order position.
						obj.OrderPosition = newCurrentOrderPosition;

						// Mark as modified, but do no display update,
						// since nothing VISUAL has changed.
						obj.StoreOrderPosition();
					}

					// Remember for next turn.
					previousOrderPosition = newCurrentOrderPosition;
				}
				else
				{
					// Remember for next turn.
					previousOrderPosition = currentOrderPosition;
				}

				itemIndex++;
			}
		}

		/// <summary>
		/// Removes the selected items.
		/// </summary>
		public void RemoveSelectedItems()
		{
			BeginUpdate();
			try
			{
				while ( SelectedItems.Count > 0 )
				{
					Items.Remove( SelectedItems[0] );
				}
			}
			finally
			{
				EndUpdate();
			}
		}

		/// <summary>
		/// Selects all.
		/// </summary>
		public void SelectAll()
		{
			BeginUpdate();
			try
			{
				foreach ( ListViewItem item in Items )
				{
					item.Selected = true;
				}
			}
			finally
			{
				EndUpdate();
			}
		}

		/// <summary>
		/// Selects the none.
		/// </summary>
		public void SelectNone()
		{
			BeginUpdate();
			try
			{
				SelectedItems.Clear();
			}
			finally
			{
				EndUpdate();
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private static bool useNativeArrows = ComCtlDllSupportsArrows;

		private bool onCreateControlCalled = false;

		private ImageList arrowImageList;

		/// <summary>
		/// http://www.codeproject.com/cs/miscctrl/extlistviewarticle.asp?msg=944292#xx944292xx.
		/// </summary>
		private bool gridLines = false;

		/// <summary>
		/// http://www.codeproject.com/cs/miscctrl/extlistviewarticle.asp?msg=944292#xx944292xx.
		/// </summary>
		private string noItemsMessage =
			Resources.Str_ExtendedListViewControl_NoItemsMessage;

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Handle GridLines on our own because base.GridLines has to be switched on
		/// and off depending on the amount of items in the ListView.
		/// http://www.codeproject.com/cs/miscctrl/extlistviewarticle.asp?msg=944292#xx944292xx.
		/// </summary>
		/// <value></value>
		/// <returns>true if grid lines are drawn around items and subitems; otherwise, false. The default is false.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[DefaultValue( false )]
		public new bool GridLines
		{
			get
			{
				return gridLines;
			}
			set
			{
				gridLines = value;
				Invalidate();
			}
		}

		/// <summary>
		/// If this property is set to a non-null/non-empty string, the string
		/// is draw if the list view contains no items.
		/// </summary>
		/// <value>The no items message.</value>
		/// <remarks>
		/// To be able to localize the message it must not be hardcoded.
		/// http://www.codeproject.com/cs/miscctrl/extlistviewarticle.asp?msg=944292#xx944292xx.
		/// </remarks>
		[DefaultValue( "There are no items to show in this view" )]
		public string NoItemsMessage
		{
			get
			{
				return noItemsMessage;
			}
			set
			{
				noItemsMessage = value;
				Invalidate();
			}
		}

		/// <summary>
		/// Checks whether to draw a message if no items are present.
		/// </summary>
		/// <value><c>true</c> if [draw no items message]; otherwise, <c>false</c>.</value>
		public bool DrawNoItemsMessage
		{
			get
			{
				if ( UserControlBase.IsDesignMode( this ) )
				{
					// Never draw in design mode.
					return false;
				}
				else
				{
					return !string.IsNullOrEmpty( NoItemsMessage );
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region P/Invoke structs and other unsafe stuff.
		// ------------------------------------------------------------------

		/// <summary>
		/// http://www.codeproject.com/cs/miscctrl/extlistviewarticle.asp?msg=944292#xx944292xx.
		/// </summary>
		private static readonly int WM_PAINT = 0x0F;

		/// <summary>
		/// 
		/// </summary>
		[StructLayout( LayoutKind.Sequential )]
		private struct NMHDR
		{
			public IntPtr hwndFrom;
			public IntPtr idFrom;
			public int code;
		}

		/// <summary>
		/// 
		/// </summary>
		[StructLayout( LayoutKind.Sequential )]
		private struct NMLISTVIEW
		{
			public NMHDR hdr;
			public int iItem;
			public int iSubItem;
			public int uNewState;
			public int uOldState;
			public int uChanged;
			public IntPtr lParam;
		}

		/// <summary>
		/// 
		/// </summary>
		private int LVN_FIRST = -100;

		/// <summary>
		/// WMs the notify.
		/// </summary>
		/// <param name="m">The message.</param>
		protected void WMNotify(
			ref Message m )
		{
			NMHDR nm = (NMHDR)m.GetLParam( typeof( NMHDR ) );

			// LVN_ITEMCHANGED.
			if ( nm.code == -101 )
			{
				NMLISTVIEW nmPtr1 = (NMLISTVIEW)m.GetLParam( typeof( NMLISTVIEW ) );

				LvnItemChanged( nmPtr1 );
			}
			else if ( nm.code == LVN_FIRST - 2 )
			{
				NMLISTVIEW nmPtr1 = (NMLISTVIEW)m.GetLParam( typeof( NMLISTVIEW ) );

				LvnInsertItem( nmPtr1 );
			}
			else if ( nm.code == LVN_FIRST - 3 )
			{
				NMLISTVIEW nmPtr1 = (NMLISTVIEW)m.GetLParam( typeof( NMLISTVIEW ) );

				LvnDeleteItem( nmPtr1 );
			}
			else if ( nm.code == LVN_FIRST - 4 )
			{
				NMLISTVIEW nmPtr1 = (NMLISTVIEW)m.GetLParam( typeof( NMLISTVIEW ) );

				LvnDeleteItem( nmPtr1 );
			}
		}

		/// <summary>
		/// Provide the events that MS has "forgotten" to implement.
		/// </summary>
		/// <param name="nmPtr">The nm PTR.</param>
		private void LvnItemChanged(
			NMLISTVIEW nmPtr )
		{
			if ( !(
				(this.Items.Count == 0) ||
				(this.Items[this.Items.Count - 1] != null)
				) )
			{
				this.OnSelectedIndexChanged( EventArgs.Empty );
			}
		}

		/// <summary>
		/// LVNs the insert item.
		/// </summary>
		/// <param name="nmPtr">The nm PTR.</param>
		private void LvnInsertItem(
			NMLISTVIEW nmPtr )
		{
			this.OnItemInserted( EventArgs.Empty );
		}

		/// <summary>
		/// LVNs the delete item.
		/// </summary>
		/// <param name="nmPtr">The nm PTR.</param>
		private void LvnDeleteItem(
			NMLISTVIEW nmPtr )
		{
			this.OnItemDeleted( EventArgs.Empty );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public events.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		public event EventHandler<EventArgs> ItemInserted;

		/// <summary>
		/// 
		/// </summary>
		public event EventHandler<EventArgs> ItemDeleted;

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Overrides <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)"></see>.
		/// </summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message"></see> to process.</param>
		protected override void WndProc(
			ref Message m )
		{
			try
			{
				base.WndProc( ref m );
			}
			catch ( NullReferenceException )
			{
				// 2006-12-04: Strange. I got an exception inside this call.
				// Ignore for now. 
				// This happened once when called "private unsafe void LvnItemChanged()".
				// Probably because of accessing the Items collection from within.
				// TODO: FIX! (maybe Post again to myself).
			}

			// WM_NOTIFY or "WM_REFLECT_NOTIFY".
			int msgID = m.Msg;
			if ( msgID == 0x204E || msgID == 0x004E )
			{
				WMNotify( ref m );
			}
			else if ( msgID == 0x1008 )
			{
				// LVM_DELETEITEM.
				LvnItemChanged( new NMLISTVIEW() );
			}
			else if ( msgID == 0x1009 )
			{
				// LVM_DELETEALLITEMS.
				LvnItemChanged( new NMLISTVIEW() );
			}
			else if ( m.Msg == WM_PAINT )
			{
				// http://www.codeproject.com/cs/miscctrl/extlistviewarticle.asp?msg=944292#xx944292xx.
				#region Handle drawing of "no items" message

				// UK: only draw if enabled.
				if ( DrawNoItemsMessage )
				{
					if ( Items.Count == 0 && Columns.Count > 0 )
					{
						if ( !UserControlBase.IsDesignMode( this ) )
						{
							if ( this.GridLines )
							{
								base.GridLines = false;
							}

							int w = 0;
							foreach ( ColumnHeader h in this.Columns )
							{
								w += h.Width;
							}

							StringFormat sf = new StringFormat();
							sf.Alignment = StringAlignment.Center;

							double addTop =
								this.HeaderStyle == ColumnHeaderStyle.None ?
								0 :
								1.0;
							Rectangle rc = new Rectangle(
								0,
								(int)(this.Font.Height * (1.5 + addTop)),
								w,
								this.Height );

							// This seems to "fail" (i.e. have no effect), if
							// double-buffering is enabled, because the text here is drawn on the
							// default graphic context (the screen directly), but the double-buffer
							// seems to overwrite again).
							using ( Graphics g = this.CreateGraphics() )
							{
								g.FillRectangle( SystemBrushes.Window, 0, 0, this.Width, this.Height );
								g.DrawString( NoItemsMessage, this.Font, SystemBrushes.ControlText, rc, sf );
							}
						}
					}
					else
					{
						base.GridLines = this.GridLines;
					}
				}

				#endregion
			}
		}

		/// <summary>
		/// Raises the <see cref="M:System.Windows.Forms.Control.CreateControl"></see> method.
		/// </summary>
		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			if ( !onCreateControlCalled )
			{
				onCreateControlCalled = true;

				DoubleBuffered = true;

				// http://www.codeproject.com/cs/miscctrl/extlistviewarticle.asp?msg=944292#xx944292xx.
				SetStyle( ControlStyles.ResizeRedraw, true );

				if ( !useNativeArrows )
				{
					arrowImageList = new ImageList();
					arrowImageList.ImageSize = new Size( 8, 8 );
					arrowImageList.TransparentColor = System.Drawing.Color.Magenta;

					arrowImageList.Images.Add( GetArrowBitmap( ArrowType.Ascending ) );		// Add ascending arrow
					arrowImageList.Images.Add( GetArrowBitmap( ArrowType.Descending ) );		// Add descending arrow

					SetHeaderImageList( arrowImageList );
				}
			}
		}

		/// <summary>
		/// Raises the <see cref="E:ItemInserted"/> event.
		/// </summary>
		/// <param name="eventArgs">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnItemInserted(
			EventArgs eventArgs )
		{
			if ( ItemInserted != null )
			{
				ItemInserted( this, eventArgs );
			}
		}

		/// <summary>
		/// Raises the <see cref="E:ItemDeleted"/> event.
		/// </summary>
		/// <param name="eventArgs">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnItemDeleted(
			EventArgs eventArgs )
		{
			if ( ItemDeleted != null )
			{
				ItemDeleted( this, eventArgs );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Graphics.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private enum ArrowType
		{
			#region Enum members.

			/// <summary>
			/// 
			/// </summary>
			Ascending,

			/// <summary>
			/// 
			/// </summary>
			Descending

			#endregion
		}

		/// <summary>
		/// Gets the arrow bitmap.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		private Bitmap GetArrowBitmap(
			ArrowType type )
		{
			Bitmap bmp = new Bitmap( 8, 8 );
			Graphics gfx = Graphics.FromImage( bmp );

			Pen lightPen = SystemPens.ControlLightLight;
			Pen shadowPen = SystemPens.ControlDark;

			gfx.FillRectangle( System.Drawing.Brushes.Magenta, 0, 0, 8, 8 );

			if ( type == ArrowType.Ascending )
			{
				gfx.DrawLine( lightPen, 0, 7, 7, 7 );
				gfx.DrawLine( lightPen, 7, 7, 4, 0 );
				gfx.DrawLine( shadowPen, 3, 0, 0, 7 );
			}

			else if ( type == ArrowType.Descending )
			{
				gfx.DrawLine( lightPen, 4, 7, 7, 0 );
				gfx.DrawLine( shadowPen, 3, 7, 0, 0 );
				gfx.DrawLine( shadowPen, 0, 0, 7, 0 );
			}

			gfx.Dispose();

			return bmp;
		}

		[StructLayout( LayoutKind.Sequential )]
		private struct HDITEM
		{
			public int mask;
			public int cxy;
			[MarshalAs( UnmanagedType.LPTStr )]
			public String pszText;
			public IntPtr hbm;
			public int cchTextMax;
			public int fmt;
			public int lParam;
			public int iImage;
			public int iOrder;
		};

		/// <summary>
		/// Sends the message.
		/// </summary>
		/// <param name="Handle">The handle.</param>
		/// <param name="msg">The MSG.</param>
		/// <param name="wParam">The w param.</param>
		/// <param name="lParam">The l param.</param>
		/// <returns></returns>
		[DllImport( @"user32" )]
		private static extern IntPtr SendMessage(
			IntPtr Handle,
			int msg,
			IntPtr wParam,
			IntPtr lParam );

		/// <summary>
		/// Sends the message2.
		/// </summary>
		/// <param name="Handle">The handle.</param>
		/// <param name="msg">The MSG.</param>
		/// <param name="wParam">The w param.</param>
		/// <param name="lParam">The l param.</param>
		/// <returns></returns>
		[DllImport( @"user32", EntryPoint = @"SendMessage" )]
		private static extern IntPtr SendMessage2(
			IntPtr Handle,
			int msg,
			IntPtr wParam,
			ref HDITEM lParam );

		private const int HDI_WIDTH = 0x0001;
		private const int HDI_HEIGHT = HDI_WIDTH;
		private const int HDI_TEXT = 0x0002;
		private const int HDI_FORMAT = 0x0004;
		private const int HDI_LPARAM = 0x0008;
		private const int HDI_BITMAP = 0x0010;
		private const int HDI_IMAGE = 0x0020;
		private const int HDI_DI_SETITEM = 0x0040;
		private const int HDI_ORDER = 0x0080;
		private const int HDI_FILTER = 0x0100;		// 0x0500

		private const int HDF_LEFT = 0x0000;
		private const int HDF_RIGHT = 0x0001;
		private const int HDF_CENTER = 0x0002;
		private const int HDF_JUSTIFYMASK = 0x0003;
		private const int HDF_RTLREADING = 0x0004;
		private const int HDF_OWNERDRAW = 0x8000;
		private const int HDF_STRING = 0x4000;
		private const int HDF_BITMAP = 0x2000;
		private const int HDF_BITMAP_ON_RIGHT = 0x1000;
		private const int HDF_IMAGE = 0x0800;
		private const int HDF_SORTUP = 0x0400;		// 0x0501
		private const int HDF_SORTDOWN = 0x0200;		// 0x0501

		private const int LVM_FIRST = 0x1000;		// DropDownList messages
		private const int LVM_GETHEADER = LVM_FIRST + 31;

		private const int HDM_FIRST = 0x1200;		// Header messages
		private const int HDM_SETIMAGELIST = HDM_FIRST + 8;
		private const int HDM_GETIMAGELIST = HDM_FIRST + 9;
		private const int HDM_GETITEM = HDM_FIRST + 11;
		private const int HDM_SETITEM = HDM_FIRST + 12;

		/// <summary>
		/// Shows the header icon.
		/// </summary>
		/// <param name="columnIndex">Index of the column.</param>
		/// <param name="sortOrder">The sort order.</param>
		protected void ShowHeaderIcon(
			int columnIndex,
			SortOrder sortOrder )
		{
			if ( columnIndex >= 0 && columnIndex < this.Columns.Count )
			{
				IntPtr hHeader =
					SendMessage(
					this.Handle,
					LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero );

				ColumnHeader colHdr = this.Columns[columnIndex];

				HDITEM hd = new HDITEM();
				hd.mask = HDI_FORMAT;

				HorizontalAlignment align = colHdr.TextAlign;

				if ( align == HorizontalAlignment.Left )
				{
					hd.fmt = HDF_LEFT | HDF_STRING | HDF_BITMAP_ON_RIGHT;
				}
				else if ( align == HorizontalAlignment.Center )
				{
					hd.fmt = HDF_CENTER | HDF_STRING | HDF_BITMAP_ON_RIGHT;
				}
				else
				{
					Debug.Assert( align == HorizontalAlignment.Right );
					hd.fmt = HDF_RIGHT | HDF_STRING;
				}

				if ( useNativeArrows )
				{
					if ( sortOrder == SortOrder.Ascending )
					{
						hd.fmt |= HDF_SORTUP;
					}
					else if ( sortOrder == SortOrder.Descending )
					{
						hd.fmt |= HDF_SORTDOWN;
					}
				}
				else
				{
					hd.mask |= HDI_IMAGE;

					if ( sortOrder != SortOrder.None )
					{
						hd.fmt |= HDF_IMAGE;
					}
					hd.iImage = (int)sortOrder - 1;
				}

				SendMessage2(
					hHeader,
					HDM_SETITEM,
					new IntPtr( columnIndex ),
					ref hd );
			}
		}

		/// <summary>
		/// Sets the header image list.
		/// </summary>
		/// <param name="imgList">The img list.</param>
		private void SetHeaderImageList(
			ImageList imgList )
		{
			IntPtr hHeader = SendMessage( this.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero );
			SendMessage( hHeader, HDM_SETIMAGELIST, IntPtr.Zero, imgList.Handle );
		}

		// ------------------------------------------------------------------
		#endregion

		#region ComCtrl information.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		[StructLayout( LayoutKind.Sequential )]
		private struct DLLVERSIONINFO
		{
			public int cbSize;
			public int dwMajorVersion;
			public int dwMinorVersion;
			public int dwBuildNumber;
			public int dwPlatformID;
		}

		/// <summary>
		/// Loads the library.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <returns></returns>
		[DllImport( @"kernel32.dll" )]
		private static extern IntPtr LoadLibrary(
			string fileName );

		/// <summary>
		/// Gets the proc address.
		/// </summary>
		/// <param name="hModule">The h module.</param>
		/// <param name="procName">Name of the proc.</param>
		/// <returns></returns>
		[DllImport( @"kernel32.dll" )]
		private static extern IntPtr GetProcAddress(
			IntPtr hModule,
			string procName );

		/// <summary>
		/// Frees the library.
		/// </summary>
		/// <param name="hModule">The h module.</param>
		/// <returns></returns>
		[DllImport( @"kernel32.dll" )]
		private static extern bool FreeLibrary(
			IntPtr hModule );

		/// <summary>
		/// DLLs the get version.
		/// </summary>
		/// <param name="pdvi">The pdvi.</param>
		/// <returns></returns>
		[DllImport( @"comctl32.dll" )]
		private static extern int DllGetVersion(
			ref DLLVERSIONINFO pdvi );

		/// <summary>
		/// Gets a value indicating whether [COM CTL DLL supports arrows].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [COM CTL DLL supports arrows]; otherwise, <c>false</c>.
		/// </value>
		public static bool ComCtlDllSupportsArrows
		{
			get
			{
				return Application.RenderWithVisualStyles;
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////}
}