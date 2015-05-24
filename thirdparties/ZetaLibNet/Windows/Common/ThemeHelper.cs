namespace ZetaLib.Windows.Common
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Diagnostics;
	using System.Windows.Forms;
	using ZetaLib.Core.Common;
	using ZetaLib.Core.Collections;
	using System.Collections.Generic;
	using ZetaLib.Windows.Properties;
	using System.Runtime.InteropServices;
	using System.Drawing;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Helper class for theming support.
	/// See also http://www.vbaccelerator.com/home/net/Code/Controls/Explorer_Bar/ExplorerBar_Control_Source_Code_zip_acclExplorerBar_XpThemeAPI_cs.asp
	/// </summary>
	public class ThemeHelper :
		IDisposable
	{
		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Check whether theming is possible on the current platform.
		/// </summary>
		/// <value><c>true</c> if [want theming]; otherwise, <c>false</c>.</value>
		public static bool WantTheming
		{
			get
			{
				PlatformID platformId = Environment.OSVersion.Platform;

				Version version = Environment.OSVersion.Version;
				Version targetVersion = new Version( @"5.1.2600.0" );

				bool isCompatibleOS =
					((version >= targetVersion) &&
					(platformId == PlatformID.Win32NT));

				return isCompatibleOS && IsThemeActive() != 0;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region High-level routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Get a certain theme color.
		/// See http://windowssdk.msdn.microsoft.com/en-us/library/ms649930.aspx
		/// </summary>
		/// <param name="part">The part.</param>
		/// <param name="state">One of the UxThemeXXXState enums.</param>
		/// <param name="prop">The prop.</param>
		/// <returns></returns>
		public Color GetThemeColor(
			UxThemeWindowParts part,
			int state,
			UxThemeProp prop )
		{
			Debug.Assert( WantTheming );

			Control c = new Control();
			IntPtr hTheme = OpenThemeData(
				c.Handle,
				@"button" );
			Debug.Assert(
				hTheme != IntPtr.Zero,
				@"OpenThemeData() returned NULL pointer." );
			try
			{
				COLORREF cr = new COLORREF();
				int result = GetThemeColor(
					hTheme,
					(int)part,
					(int)state,
					(int)prop,
					ref cr );

				Debug.Assert( result == 0 );
				return cr.Color;
			}
			finally
			{
				CloseThemeData( hTheme );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Low-level theming helper classes.
		// ------------------------------------------------------------------

		/// <summary>
		/// Helper struct for theming support.
		/// </summary>
		[StructLayout( LayoutKind.Explicit )]
		public struct RECT
		{
			#region Public variables.

			[FieldOffset( 0 )]
			public int Left;

			[FieldOffset( 4 )]
			public int Top;

			[FieldOffset( 8 )]
			public int Right;

			[FieldOffset( 12 )]
			public int Bottom;

			#endregion

			#region Public methods.

			/// <summary>
			/// Initializes a new instance of the <see cref="RECT"/> class.
			/// </summary>
			/// <param name="left">The left.</param>
			/// <param name="top">The top.</param>
			/// <param name="right">The right.</param>
			/// <param name="bottom">The bottom.</param>
			public RECT( int left, int top, int right, int bottom )
			{
				Left = left;
				Top = top;
				Right = right;
				Bottom = bottom;
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="RECT"/> class.
			/// </summary>
			/// <param name="rect">The rect.</param>
			public RECT( Rectangle rect )
			{
				Left = rect.Left;
				Top = rect.Top;
				Right = rect.Right;
				Bottom = rect.Bottom;
			}

			/// <summary>
			/// Toes the rectangle.
			/// </summary>
			/// <returns></returns>
			public Rectangle ToRectangle()
			{
				return new Rectangle( Left, Top, Right, Bottom - 1 );
			}

			#endregion
		}

		/// <summary>
		/// Helper struct for theming support.
		/// </summary>
		[StructLayout( LayoutKind.Sequential )]
		public struct COLORREF
		{
			#region Public variables.

			/// <summary>
			/// 
			/// </summary>
			public uint ColorDWORD;

			#endregion

			#region Public methods.

			/// <summary>
			/// Initializes a new instance of the <see cref="COLORREF"/> class.
			/// </summary>
			/// <param name="color">The color.</param>
			public COLORREF(
				Color color )
			{
				ColorDWORD =
					(uint)color.R +
					(((uint)color.G) << 8) +
					(((uint)color.B) << 16);
			}

			#endregion

			#region Public properties.

			/// <summary>
			/// Gets or sets the color.
			/// </summary>
			/// <value>The color.</value>
			public Color Color
			{
				get
				{
					return Color.FromArgb(
						(int)(0x000000FFU & ColorDWORD),
						(int)(0x0000FF00U & ColorDWORD) >> 8,
						(int)(0x00FF0000U & ColorDWORD) >> 16 );
				}
				set
				{
					ColorDWORD =
						(uint)value.R +
						(((uint)value.G) << 8) +
						(((uint)value.B) << 16);
				}
			}

			#endregion
		}

		// ------------------------------------------------------------------
		#endregion

		#region Low-level theming enums.
		// ------------------------------------------------------------------

		/// <summary>
		///  "Window" (i.e., non-client) Parts
		/// </summary>
		public enum UxThemeWindowParts : int
		{
			/// <summary>Caption</summary>
			WP_CAPTION = 1,
			/// <summary>Small Caption</summary>
			WP_SMALLCAPTION = 2,
			/// <summary>Minimised Caption</summary>
			WP_MINCAPTION = 3,
			/// <summary>Small minimised Caption</summary>
			WP_SMALLMINCAPTION = 4,
			/// <summary>Maximised Caption</summary>
			WP_MAXCAPTION = 5,
			/// <summary>Small maximised Caption</summary>
			WP_SMALLMAXCAPTION = 6,
			/// <summary>Frame left</summary>
			WP_FRAMELEFT = 7,
			/// <summary>Frame right</summary>
			WP_FRAMERIGHT = 8,
			/// <summary>Frame bottom</summary>
			WP_FRAMEBOTTOM = 9,
			/// <summary>Small frame left</summary>
			WP_SMALLFRAMELEFT = 10,
			/// <summary>Small frame right</summary>
			WP_SMALLFRAMERIGHT = 11,
			/// <summary>Small frame bottom</summary>
			WP_SMALLFRAMEBOTTOM = 12,
			/// <summary>System button</summary>
			WP_SYSBUTTON = 13,
			/// <summary>MDI System button</summary>
			WP_MDISYSBUTTON = 14,
			/// <summary>Min button</summary>
			WP_MINBUTTON = 15,
			/// <summary>MDI Min button</summary>
			WP_MDIMINBUTTON = 16,
			/// <summary>Max button</summary>
			WP_MAXBUTTON = 17,
			/// <summary>Close button</summary>
			WP_CLOSEBUTTON = 18,
			/// <summary>Small close button</summary>
			WP_SMALLCLOSEBUTTON = 19,
			/// <summary>MDI close button</summary>
			WP_MDICLOSEBUTTON = 20,
			/// <summary>Restore button</summary>
			WP_RESTOREBUTTON = 21,
			/// <summary>MDI Restore button</summary>
			WP_MDIRESTOREBUTTON = 22,
			/// <summary>Help button</summary>
			WP_HELPBUTTON = 23,
			/// <summary>MDI Help button</summary>
			WP_MDIHELPBUTTON = 24,
			/// <summary>Horizontal scroll bar</summary>
			WP_HORZSCROLL = 25,
			/// <summary>Horizontal scroll thumb</summary>
			WP_HORZTHUMB = 26,
			/// <summary>Vertical scroll bar</summary>
			WP_VERTSCROLL = 27,
			/// <summary>Vertical scroll thumb</summary>
			WP_VERTTHUMB = 28,
			/// <summary>Dialog</summary>
			WP_DIALOG = 29,
			/// <summary>Caption sizing hittest template</summary>
			WP_CAPTIONSIZINGTEMPLATE = 30,
			/// <summary>Small caption sizing hittest template</summary>
			WP_SMALLCAPTIONSIZINGTEMPLATE = 31,
			/// <summary>Frame left sizing hittest template</summary>
			WP_FRAMELEFTSIZINGTEMPLATE = 32,
			/// <summary>Small frame left sizing hittest template</summary>
			WP_SMALLFRAMELEFTSIZINGTEMPLATE = 33,
			/// <summary>Frame right sizing hittest template</summary>
			WP_FRAMERIGHTSIZINGTEMPLATE = 34,
			/// <summary>Small frame right sizing hittest template</summary>
			WP_SMALLFRAMERIGHTSIZINGTEMPLATE = 35,
			/// <summary>Frame button sizing hittest template</summary>
			WP_FRAMEBOTTOMSIZINGTEMPLATE = 36,
			/// <summary>Small frame bottom sizing hittest template</summary>
			WP_SMALLFRAMEBOTTOMSIZINGTEMPLATE = 37
		}

		/// <summary>
		/// Frame states
		/// </summary>
		public enum UxThemeFrameStates : int
		{
			/// <summary>Active frame</summary>
			FS_ACTIVE = 1,
			/// <summary>Inactive frame</summary>
			FS_INACTIVE = 2
		}

		/// <summary>
		/// Caption states
		/// </summary>
		public enum UxThemeCaptionStates : int
		{
			/// <summary>Active caption</summary>
			CS_ACTIVE = 1,
			/// <summary>Inactive caption</summary>
			CS_INACTIVE = 2,
			/// <summary>Disabled caption</summary>
			CS_DISABLED = 3
		}

		/// <summary>
		/// Maximised caption states
		/// </summary>
		public enum UxThemeMaxCaptionStates : int
		{
			/// <summary>Max Active caption</summary>
			MXCS_ACTIVE = 1,
			/// <summary>Max inactive caption</summary>
			MXCS_INACTIVE = 2,
			/// <summary>Max disabled caption</summary>
			MXCS_DISABLED = 3
		}

		/// <summary>
		/// Minimised caption states
		/// </summary>
		public enum UxThemeMinCaptionStates : int
		{
			/// <summary>Minimised active caption</summary>
			MNCS_ACTIVE = 1,
			/// <summary>Minimised inactive caption</summary>
			MNCS_INACTIVE = 2,
			/// <summary>Minimised disabled caption</summary>
			MNCS_DISABLED = 3
		}

		/// <summary>
		/// Horizontal scroll states
		/// </summary>
		public enum UxThemeHorzScrollStates : int
		{
			/// <summary>Normal</summary>
			HSS_NORMAL = 1,
			/// <summary>Hot</summary>
			HSS_HOT = 2,
			/// <summary>Pushed</summary>
			HSS_PUSHED = 3,
			/// <summary>Disabled</summary>
			HSS_DISABLED = 4
		}

		/// <summary>
		/// Horizontal thumb states
		/// </summary>
		public enum UxThemeHorzThumbStates : int
		{
			/// <summary>Normal</summary>
			HTS_NORMAL = 1,
			/// <summary>Hot</summary>
			HTS_HOT = 2,
			/// <summary>Pushed</summary>
			HTS_PUSHED = 3,
			/// <summary>Disabled</summary>
			HTS_DISABLED = 4
		}

		/// <summary>
		/// Vertical scroll states
		/// </summary>
		public enum UxThemeVertScrollStates : int
		{
			/// <summary>Normal</summary>
			VSS_NORMAL = 1,
			/// <summary>Hot</summary>
			VSS_HOT = 2,
			/// <summary>Pushed</summary>
			VSS_PUSHED = 3,
			/// <summary>Disabled</summary>
			VSS_DISABLED = 4
		}

		/// <summary>
		/// Vertical thumb states
		/// </summary>
		public enum UxThemeVertThumbStates : int
		{
			/// <summary>Normal</summary>
			VTS_NORMAL = 1,
			/// <summary>Hot</summary>
			VTS_HOT = 2,
			/// <summary>Pushed</summary>
			VTS_PUSHED = 3,
			/// <summary>Disabled</summary>
			VTS_DISABLED = 4
		}

		/// <summary>
		/// System Button states
		/// </summary>
		public enum UxThemeSysButtonStates : int
		{
			/// <summary>Normal</summary>
			SBS_NORMAL = 1,
			/// <summary>Hot</summary>
			SBS_HOT = 2,
			/// <summary>Pushed</summary>
			SBS_PUSHED = 3,
			/// <summary>Disabled</summary>
			SBS_DISABLED = 4
		}

		/// <summary>
		/// Min Button states
		/// </summary>
		public enum UxThemeMinButtonStates : int
		{
			/// <summary>Normal</summary>
			MINBS_NORMAL = 1,
			/// <summary>Hot</summary>
			MINBS_HOT = 2,
			/// <summary>Pushed</summary>
			MINBS_PUSHED = 3,
			/// <summary>Disabled</summary>
			MINBS_DISABLED = 4
		}

		/// <summary>
		/// Max Button states
		/// </summary>
		public enum UxThemeMaxButtonStates : int
		{
			/// <summary>Normal</summary>
			MAXBS_NORMAL = 1,
			/// <summary>Hot</summary>
			MAXBS_HOT = 2,
			/// <summary>Pushed</summary>
			MAXBS_PUSHED = 3,
			/// <summary>Disabled</summary>
			MAXBS_DISABLED = 4
		}

		/// <summary>
		/// Restore button states
		/// </summary>
		public enum UxThemeRestoreButtonStates : int
		{
			/// <summary>Normal</summary>
			RBS_NORMAL = 1,
			/// <summary>Hot</summary>
			RBS_HOT = 2,
			/// <summary>Pushed</summary>
			RBS_PUSHED = 3,
			/// <summary>Disabled</summary>
			RBS_DISABLED = 4
		}

		/// <summary>
		/// Help button states
		/// </summary>
		public enum UxThemeHelpButtonStates : int
		{
			/// <summary>Normal</summary>
			HBS_NORMAL = 1,
			/// <summary>Hot</summary>
			HBS_HOT = 2,
			/// <summary>Pushed</summary>
			HBS_PUSHED = 3,
			/// <summary>Disabled</summary>
			HBS_DISABLED = 4,
		}

		/// <summary>
		/// Closed button states
		/// </summary>
		public enum UxThemeCloseButtonStates : int
		{
			/// <summary>Normal</summary>
			CBS_NORMAL = 1,
			/// <summary>Hot</summary>
			CBS_HOT = 2,
			/// <summary>Pushed</summary>
			CBS_PUSHED = 3,
			/// <summary>Disabled</summary>
			CBS_DISABLED = 4
		}


		/// <summary>
		///  "Button" Parts
		/// </summary>
		public enum UxThemeButtonParts : int
		{
			/// <summary>Push Button</summary>
			BP_PUSHBUTTON = 1,
			/// <summary>Radio Button</summary>
			BP_RADIOBUTTON = 2,
			/// <summary>Check box</summary>
			BP_CHECKBOX = 3,
			/// <summary>Group box</summary>
			BP_GROUPBOX = 4,
			/// <summary>User button</summary>
			BP_USERBUTTON = 5
		}

		/// <summary>
		/// Push Button states
		/// </summary>
		public enum UxThemePushButtonStates : int
		{
			/// <summary>Normal</summary>
			PBS_NORMAL = 1,
			/// <summary>Hot</summary>
			PBS_HOT = 2,
			/// <summary>Pressed</summary>
			PBS_PRESSED = 3,
			/// <summary>Disabled</summary>
			PBS_DISABLED = 4,
			/// <summary>Defaulted</summary>
			PBS_DEFAULTED = 5
		}

		/// <summary>
		/// Radio button states
		/// </summary>
		public enum UxThemeRadioButtonStates : int
		{
			/// <summary>Unchecked Normal</summary>
			RBS_UNCHECKEDNORMAL = 1,
			/// <summary>Unchecked Hot</summary>
			RBS_UNCHECKEDHOT = 2,
			/// <summary>Unchecked Pressed</summary>
			RBS_UNCHECKEDPRESSED = 3,
			/// <summary>Unchecked Disabled</summary>
			RBS_UNCHECKEDDISABLED = 4,
			/// <summary>Checked Normal</summary>
			RBS_CHECKEDNORMAL = 5,
			/// <summary>Checked Hot</summary>
			RBS_CHECKEDHOT = 6,
			/// <summary>Checked Pressed</summary>
			RBS_CHECKEDPRESSED = 7,
			/// <summary>Checked Disabled</summary>
			RBS_CHECKEDDISABLED = 8
		}

		/// <summary>
		/// Check box states
		/// </summary>
		public enum UxThemeCheckBoxStates : int
		{
			/// <summary>Unchecked Normal</summary>
			CBS_UNCHECKEDNORMAL = 1,
			/// <summary>Unchecked Hot</summary>
			CBS_UNCHECKEDHOT = 2,
			/// <summary>Unchecked Pressed</summary>
			CBS_UNCHECKEDPRESSED = 3,
			/// <summary>Unchecked Disabled</summary>
			CBS_UNCHECKEDDISABLED = 4,
			/// <summary>Checked Normal</summary>
			CBS_CHECKEDNORMAL = 5,
			/// <summary>Checked Hot</summary>
			CBS_CHECKEDHOT = 6,
			/// <summary>Checked Pressed</summary>
			CBS_CHECKEDPRESSED = 7,
			/// <summary>Checked Disabled</summary>
			CBS_CHECKEDDISABLED = 8,
			/// <summary>Mixed Normal</summary>
			CBS_MIXEDNORMAL = 9,
			/// <summary>Mixed Hot</summary>
			CBS_MIXEDHOT = 10,
			/// <summary>Mixed Pressed</summary>
			CBS_MIXEDPRESSED = 11,
			/// <summary>Mixed Disabled</summary>
			CBS_MIXEDDISABLED = 12
		}

		/// <summary>
		/// Group box states
		/// </summary>
		public enum UxThemeGroupBoxStates : int
		{
			/// <summary>Normal</summary>
			GBS_NORMAL = 1,
			/// <summary>Disabled</summary>
			GBS_DISABLED = 2
		}


		/// <summary>
		/// "Rebar" Parts
		/// </summary>
		public enum UxThemeRebarParts : int
		{
			/// <summary>Gripper</summary>
			RP_GRIPPER = 1,
			/// <summary>Vertical Gripper</summary>
			RP_GRIPPERVERT = 2,
			/// <summary>Band</summary>
			RP_BAND = 3,
			/// <summary>Chevron</summary>
			RP_CHEVRON = 4,
			/// <summary>Vertical Chevron</summary>
			RP_CHEVRONVERT = 5
		}

		/// <summary>
		/// Chevron states
		/// </summary>
		public enum UxThemeChevronStates : int
		{
			/// <summary>Normal</summary>
			CHEVS_NORMAL = 1,
			/// <summary>Hot</summary>
			CHEVS_HOT = 2,
			/// <summary>Pressed</summary>
			CHEVS_PRESSED = 3
		}


		/// <summary>
		///  "Toolbar" Parts
		/// </summary>
		public enum UxThemeToolBarParts : int
		{
			/// <summary>Button</summary>
			TP_BUTTON = 1,
			/// <summary>Drop-Down Button</summary>
			TP_DROPDOWNBUTTON = 2,
			/// <summary>Split Button</summary>
			TP_SPLITBUTTON = 3,
			/// <summary>Split drop-Down Button</summary>
			TP_SPLITBUTTONDROPDOWN = 4,
			/// <summary>Separator</summary>
			TP_SEPARATOR = 5,
			/// <summary>Vertical Separator</summary>
			TP_SEPARATORVERT = 6
		}


		/// <summary>
		/// Tool bar states
		/// </summary>
		public enum UxThemeToolBarStates : int
		{
			/// <summary>Normal</summary>
			TS_NORMAL = 1,
			/// <summary>Hot</summary>
			TS_HOT = 2,
			/// <summary>Pressed</summary>
			TS_PRESSED = 3,
			/// <summary>Disabled</summary>
			TS_DISABLED = 4,
			/// <summary>Checked</summary>
			TS_CHECKED = 5,
			/// <summary>Checked and Hot</summary>
			TS_HOTCHECKED = 6
		}


		/// <summary> "Status" Parts </summary>      
		public enum UxThemeStatusParts : int
		{
			/// <summary>Pane</summary>
			SP_PANE = 1,
			/// <summary>Gripper Pane</summary>
			SP_GRIPPERPANE = 2,
			/// <summary>Gripper</summary>
			SP_GRIPPER = 3
		}

		/// <summary> "Menu" Parts </summary>summary>
		public enum UxThemeMenuParts : int
		{
			/// <summary>Menu Item</summary>
			MP_MENUITEM = 1,
			/// <summary>Menu Drop-Down</summary>
			MP_MENUDROPDOWN = 2,
			/// <summary>Menu Bar Item</summary>
			MP_MENUBARITEM = 3,
			/// <summary>Menu Bar Drop-DOwn</summary>
			MP_MENUBARDROPDOWN = 4,
			/// <summary>Chevron</summary>
			MP_CHEVRON = 5,
			/// <summary>Separator</summary>
			MP_SEPARATOR = 6
		}
		/// <summary>Menu States</summary>
		public enum UxThemeMenuStates : int
		{
			/// <summary>Normal</summary>
			MS_NORMAL = 1,
			/// <summary>Selected</summary>
			MS_SELECTED = 2,
			/// <summary>Demoted</summary>
			MS_DEMOTED = 3
		}

		/// <summary> "ListView" Parts</summary>
		public enum UxThemeLISTVIEWParts : int
		{
			/// <summary>List item</summary>
			LVP_LISTITEM = 1,
			/// <summary>List Group</summary>
			LVP_LISTGROUP = 2,
			/// <summary>List Detail</summary>
			LVP_LISTDETAIL = 3,
			/// <summary>List Sorted Detail</summary>
			LVP_LISTSORTEDDETAIL = 4,
			/// <summary>List Empty text</summary>
			LVP_EMPTYTEXT = 5
		}

		/// <summary>List Item States</summary>
		public enum UxThemeLISTITEMStates : int
		{
			/// <summary>Normal</summary>      
			LIS_NORMAL = 1,
			/// <summary>Hot</summary>
			LIS_HOT = 2,
			/// <summary>Selected</summary>      
			LIS_SELECTED = 3,
			/// <summary>Disabled</summary>      
			LIS_DISABLED = 4,
			/// <summary>Selected no focus</summary>      
			LIS_SELECTEDNOTFOCUS = 5
		}

		/// <summary> "Header" Parts</summary>
		public enum UxThemeHEADERParts : int
		{
			/// <summary>Header Item</summary>
			HP_HEADERITEM = 1,
			/// <summary>Left Header Item</summary>
			HP_HEADERITEMLEFT = 2,
			/// <summary>Right Header Item</summary>
			HP_HEADERITEMRIGHT = 3,
			/// <summary>Sort Arrow</summary>
			HP_HEADERSORTARROW = 4
		}

		/// <summary>Header Item States</summary>
		public enum UxThemeHEADERITEMStates : int
		{
			/// <summary>Normal</summary>      
			HIS_NORMAL = 1,
			/// <summary>Hot</summary>
			HIS_HOT = 2,
			/// <summary>Pressed</summary>      
			HIS_PRESSED = 3
		}

		/// <summary>Left Header Item States</summary>
		public enum UxThemeHEADERITEMLEFTStates : int
		{
			/// <summary>Normal</summary>      
			HILS_NORMAL = 1,
			/// <summary>Hot</summary>
			HILS_HOT = 2,
			/// <summary>Pressed</summary>      
			HILS_PRESSED = 3
		}

		/// <summary>Right Header Item States</summary>
		public enum UxThemeHEADERITEMRIGHTStates : int
		{
			/// <summary>Normal</summary>      
			HIRS_NORMAL = 1,
			/// <summary>Hot</summary>
			HIRS_HOT = 2,
			/// <summary>Pressed</summary>      
			HIRS_PRESSED = 3
		}

		/// <summary>Header Sort Arrow States</summary>
		public enum UxThemeHEADERSORTARROWStates : int
		{
			/// <summary>Up</summary>
			HSAS_SORTEDUP = 1,
			/// <summary>Down</summary>
			HSAS_SORTEDDOWN = 2
		}

		/// <summary>
		///  Progress Parts
		/// </summary>
		public enum UxThemePROGRESSParts : int
		{
			/// <summary>Bar</summary>      
			PP_BAR = 1,
			/// <summary>Vertical Bar</summary>      
			PP_BARVERT = 2,
			/// <summary>Chunks</summary>      
			PP_CHUNK = 3,
			/// <summary>Vertical chunks</summary>      
			PP_CHUNKVERT = 4
		}

		/// <summary>
		///  Tab Parts
		/// </summary>
		public enum UsxThemeTABParts : int
		{
			/// <summary>Tab</summary>      
			TABP_TABITEM = 1,
			/// <summary>Tab left edge</summary>      
			TABP_TABITEMLEFTEDGE = 2,
			/// <summary>Tab right edge</summary>      
			TABP_TABITEMRIGHTEDGE = 3,
			/// <summary>Tab both edge</summary>      
			TABP_TABITEMBOTHEDGE = 4,
			/// <summary>Top tab item</summary>      
			TABP_TOPTABITEM = 5,
			/// <summary>Top tab item left edge</summary>      
			TABP_TOPTABITEMLEFTEDGE = 6,
			/// <summary>Top tab item right edge</summary>      
			TABP_TOPTABITEMRIGHTEDGE = 7,
			/// <summary>Top tab item both edge</summary>      
			TABP_TOPTABITEMBOTHEDGE = 8,
			/// <summary>Tab pane</summary>      
			TABP_PANE = 9,
			/// <summary>Tab body</summary>      
			TABP_BODY = 10
		}

		/// <summary>
		/// Tab Item States
		/// </summary>
		public enum UxThemeTABITEMStates : int
		{
			/// <summary>Normal</summary>      
			TIS_NORMAL = 1,
			/// <summary>Hot</summary>      
			TIS_HOT = 2,
			/// <summary>Selected</summary>      
			TIS_SELECTED = 3,
			/// <summary>Disabled</summary>      
			TIS_DISABLED = 4,
			/// <summary>Focused</summary>      
			TIS_FOCUSED = 5
		}

		/// <summary>
		/// Tab item left edge states
		/// </summary>
		public enum UxThemeTABITEMLEFTEDGEStates : int
		{
			/// <summary>Normal</summary>      
			TILES_NORMAL = 1,
			/// <summary>Hot</summary>      
			TILES_HOT = 2,
			/// <summary>Selected</summary>      
			TILES_SELECTED = 3,
			/// <summary>Disabled</summary>      
			TILES_DISABLED = 4,
			/// <summary>Focused</summary>      
			TILES_FOCUSED = 5
		}

		/// <summary>
		/// Tab item right edge states
		/// </summary>
		public enum UxThemeTABITEMRIGHTEDGEStates : int
		{
			/// <summary>Normal</summary>      
			TIRES_NORMAL = 1,
			/// <summary>Hot</summary>      
			TIRES_HOT = 2,
			/// <summary>Selected</summary>      
			TIRES_SELECTED = 3,
			/// <summary>Disabled</summary>      
			TIRES_DISABLED = 4,
			/// <summary>Focused</summary>      
			TIRES_FOCUSED = 5
		}

		/// <summary>
		/// Tab item both edge states
		/// </summary>
		public enum UxThemeTABITEMBOTHEDGESStates : int
		{
			/// <summary>Normal</summary>      
			TIBES_NORMAL = 1,
			/// <summary>Hot</summary>      
			TIBES_HOT = 2,
			/// <summary>Selected</summary>      
			TIBES_SELECTED = 3,
			/// <summary>Disabled</summary>      
			TIBES_DISABLED = 4,
			/// <summary>Focused</summary>      
			TIBES_FOCUSED = 5
		}

		/// <summary>
		/// Top tab item states
		/// </summary>
		public enum UxThemeTOPTABITEMStates : int
		{
			/// <summary>Normal</summary>      
			TTIS_NORMAL = 1,
			/// <summary>Hot</summary>      
			TTIS_HOT = 2,
			/// <summary>Selected</summary>      
			TTIS_SELECTED = 3,
			/// <summary>Disabled</summary>      
			TTIS_DISABLED = 4,
			/// <summary>Focused</summary>      
			TTIS_FOCUSED = 5
		}

		/// <summary>
		/// Top tab item left edge states
		/// </summary>
		public enum UxThemeTOPTABITEMLEFTEDGEStates : int
		{
			/// <summary>Normal</summary>      
			TTILES_NORMAL = 1,
			/// <summary>Hot</summary>      
			TTILES_HOT = 2,
			/// <summary>Selected</summary>      
			TTILES_SELECTED = 3,
			/// <summary>Disabled</summary>      
			TTILES_DISABLED = 4,
			/// <summary>Focused</summary>      
			TTILES_FOCUSED = 5
		}

		/// <summary>
		/// Top tab item right edge states
		/// </summary>
		public enum UxThemeTOPTABITEMRIGHTEDGEStates : int
		{
			/// <summary>Normal</summary>      
			TTIRES_NORMAL = 1,
			/// <summary>Hot</summary>      
			TTIRES_HOT = 2,
			/// <summary>Selected</summary>      
			TTIRES_SELECTED = 3,
			/// <summary>Disabled</summary>      
			TTIRES_DISABLED = 4,
			/// <summary>Focused</summary>      
			TTIRES_FOCUSED = 5
		}

		/// <summary>
		/// Top tab item both edge states
		/// </summary>
		public enum UxThemeTOPTABITEMBOTHEDGESStates : int
		{
			/// <summary>Normal</summary>      
			TTIBES_NORMAL = 1,
			/// <summary>Hot</summary>      
			TTIBES_HOT = 2,
			/// <summary>Selected</summary>      
			TTIBES_SELECTED = 3,
			/// <summary>Disabled</summary>      
			TTIBES_DISABLED = 4,
			/// <summary>Focused</summary>      
			TTIBES_FOCUSED = 5
		}

		/// <summary> "Trackbar" Parts</summary>
		public enum UxThemeTRACKBARParts : int
		{
			/// <summary>Track</summary>      
			TKP_TRACK = 1,
			/// <summary>Vertical Track</summary>      
			TKP_TRACKVERT = 2,
			/// <summary>Thumb</summary>      
			TKP_THUMB = 3,
			/// <summary>Thumb Button</summary>      
			TKP_THUMBBOTTOM = 4,
			/// <summary>Thumb Top</summary>      
			TKP_THUMBTOP = 5,
			/// <summary>Vertical Thumb</summary>      
			TKP_THUMBVERT = 6,
			/// <summary>Thumb left</summary>      
			TKP_THUMBLEFT = 7,
			/// <summary>Thumb right</summary>      
			TKP_THUMBRIGHT = 8,
			/// <summary>Track tic marks</summary>      
			TKP_TICS = 9,
			/// <summary>Vertical track tic marks</summary>      
			TKP_TICSVERT = 10
		}

		/// <summary>
		/// Track Bar states
		/// </summary>
		public enum UxThemeTRACKBARStates : int
		{
			/// <summary>Normal</summary>      
			TKS_NORMAL = 1
		}

		/// <summary>
		/// Track states
		/// </summary>
		public enum UxThemeTRACKStates : int
		{
			/// <summary>Normal</summary>      
			TRS_NORMAL = 1
		}

		/// <summary>
		/// Vertical track bar states
		/// </summary>
		public enum UxThemeTRACKVERTStates : int
		{
			/// <summary>Normal</summary>      
			TRVS_NORMAL = 1
		}

		/// <summary>
		/// Thumb states
		/// </summary>
		public enum UxThemeTHUMBStates : int
		{
			/// <summary>Normal</summary>      
			TUS_NORMAL = 1,
			/// <summary>Hot</summary>      
			TUS_HOT = 2,
			/// <summary>Pressed</summary>      
			TUS_PRESSED = 3,
			/// <summary>Focused</summary>      
			TUS_FOCUSED = 4,
			/// <summary>Disabled</summary>      
			TUS_DISABLED = 5
		}

		/// <summary>Thumb Bottom states</summary>
		public enum UxThemeTHUMBBOTTOMStates : int
		{
			/// <summary>Normal</summary>      
			TUBS_NORMAL = 1,
			/// <summary>Hot</summary>      
			TUBS_HOT = 2,
			/// <summary>Pressed</summary>      
			TUBS_PRESSED = 3,
			/// <summary>Focused</summary>      
			TUBS_FOCUSED = 4,
			/// <summary>Disabled</summary>      
			TUBS_DISABLED = 5
		}

		/// <summary>Thumb Top states</summary>
		public enum UxThemeTHUMBTOPStates : int
		{
			/// <summary>Normal</summary>      
			TUTS_NORMAL = 1,
			/// <summary>Hot</summary>      
			TUTS_HOT = 2,
			/// <summary>Pressed</summary>      
			TUTS_PRESSED = 3,
			/// <summary>Focused</summary>      
			TUTS_FOCUSED = 4,
			/// <summary>Disabled</summary>      
			TUTS_DISABLED = 5
		}

		/// <summary>Vertical thumb states</summary>
		public enum UxThemeTHUMBVERTStates : int
		{
			/// <summary>Normal</summary>      
			TUVS_NORMAL = 1,
			/// <summary>Hot</summary>      
			TUVS_HOT = 2,
			/// <summary>Pressed</summary>      
			TUVS_PRESSED = 3,
			/// <summary>Focused</summary>      
			TUVS_FOCUSED = 4,
			/// <summary>Disabled</summary>      
			TUVS_DISABLED = 5
		}

		/// <summary>Vertical thumb left states</summary>
		public enum UxThemeTHUMBLEFTStates : int
		{
			/// <summary>Normal</summary>      
			TUVLS_NORMAL = 1,
			/// <summary>Hot</summary>      
			TUVLS_HOT = 2,
			/// <summary>Pressed</summary>      
			TUVLS_PRESSED = 3,
			/// <summary>Focused</summary>      
			TUVLS_FOCUSED = 4,
			/// <summary>Disabled</summary>      
			TUVLS_DISABLED = 5
		}

		/// <summary>Vertical thumb right states</summary>
		public enum UxThemeTHUMBRIGHTStates : int
		{
			/// <summary>Normal</summary>      
			TUVRS_NORMAL = 1,
			/// <summary>Hot</summary>      
			TUVRS_HOT = 2,
			/// <summary>Pressed</summary>      
			TUVRS_PRESSED = 3,
			/// <summary>Focused</summary>      
			TUVRS_FOCUSED = 4,
			/// <summary>Disabled</summary>      
			TUVRS_DISABLED = 5
		}

		/// <summary>Thumb states</summary>
		public enum UxThemeTICSStates : int
		{
			/// <summary>Normal</summary>      
			TSS_NORMAL = 1
		}

		/// <summary>Vertical thumb tics states</summary>
		public enum UxThemeTICSVERTStates : int
		{
			/// <summary>Normal</summary>      
			TSVS_NORMAL = 1
		}

		/// <summary> "Tooltips" Parts</summary>
		public enum UxThemeTOOLTIPParts : int
		{
			/// <summary>Standard</summary>      
			TTP_STANDARD = 1,
			/// <summary>Standard with title</summary>      
			TTP_STANDARDTITLE = 2,
			/// <summary>Balloon</summary>      
			TTP_BALLOON = 3,
			/// <summary>Balloon with title</summary>      
			TTP_BALLOONTITLE = 4,
			/// <summary>Close</summary>      
			TTP_CLOSE = 5
		}

		/// <summary>Tool tip Close states</summary>
		public enum UxThemeCLOSEStates : int
		{
			/// <summary>Normal</summary>      
			TTCS_NORMAL = 1,
			/// <summary>Hot</summary>      
			TTCS_HOT = 2,
			/// <summary>Pressed</summary>      
			TTCS_PRESSED = 3
		}

		/// <summary>
		/// Standard Tool Tip states
		/// </summary>
		public enum UxThemeSTANDARDStates : int
		{
			/// <summary>Normal</summary>      
			TTSS_NORMAL = 1,
			/// <summary>Link</summary>      
			TTSS_LINK = 2
		}

		/// <summary>
		/// Balloon tool tip states
		/// </summary>
		public enum UxThemeBALLOONStates : int
		{
			/// <summary>Normal</summary>      
			TTBS_NORMAL = 1,
			/// <summary>Link</summary>      
			TTBS_LINK = 2
		}

		/// <summary> "TreeView" Parts</summary>
		public enum UxThemeTREEVIEWParts : int
		{
			/// <summary>Tree Item</summary>      
			TVP_TREEITEM = 1,
			/// <summary>Glyph</summary>      
			TVP_GLYPH = 2,
			/// <summary>Branch</summary>      
			TVP_BRANCH = 3
		}

		/// <summary>Tree Item States</summary>      
		public enum UxThemeTREEITEMStates : int
		{
			/// <summary>Normal</summary>      
			TREIS_NORMAL = 1,
			/// <summary>Hot</summary>      
			TREIS_HOT = 2,
			/// <summary>Selected</summary>      
			TREIS_SELECTED = 3,
			/// <summary>Disabled</summary>      
			TREIS_DISABLED = 4,
			/// <summary>Selected no focus</summary>      
			TREIS_SELECTEDNOTFOCUS = 5
		}

		/// <summary>Glyph states</summary>      
		public enum UxThemeGLYPHStates : int
		{
			/// <summary>Closed</summary>      
			GLPS_CLOSED = 1,
			/// <summary>Opened</summary>      
			GLPS_OPENED = 2
		}

		/// <summary> "Spin" Parts</summary>
		public enum UxThemeSPINStates : int
		{
			/// <summary>Spin up</summary>      
			SPNP_UP = 1,
			/// <summary>Spin down</summary>      
			SPNP_DOWN = 2,
			/// <summary>Spin up horizontal</summary>      
			SPNP_UPHORZ = 3,
			/// <summary>Spin down horizontal</summary>      
			SPNP_DOWNHORZ = 4
		}

		/// <summary>Spin up states</summary>
		public enum UxThemeUPStates : int
		{
			/// <summary>Normal</summary>      
			UPS_NORMAL = 1,
			/// <summary>Hot</summary>      
			UPS_HOT = 2,
			/// <summary>Pressed</summary>      
			UPS_PRESSED = 3,
			/// <summary>Disabled</summary>      
			UPS_DISABLED = 4
		}

		/// <summary>Spin down states</summary>
		public enum UxThemeDOWNStates : int
		{
			/// <summary>Normal</summary>      
			DNS_NORMAL = 1,
			/// <summary>Hot</summary>      
			DNS_HOT = 2,
			/// <summary>Pressed</summary>      
			DNS_PRESSED = 3,
			/// <summary>Disabled</summary>      
			DNS_DISABLED = 4
		}

		/// <summary>Horizontal spin up states</summary>
		public enum UxThemeUPHORZStates : int
		{
			/// <summary>Normal</summary>      
			UPHZS_NORMAL = 1,
			/// <summary>Hot</summary>      
			UPHZS_HOT = 2,
			/// <summary>Pressed</summary>      
			UPHZS_PRESSED = 3,
			/// <summary>Disabled</summary>      
			UPHZS_DISABLED = 4
		}

		/// <summary>Horizontal spin down states</summary>
		public enum UxThemeDOWNHORZStates : int
		{
			/// <summary>Normal</summary>      
			DNHZS_NORMAL = 1,
			/// <summary>Hot</summary>      
			DNHZS_HOT = 2,
			/// <summary>Pressed</summary>      
			DNHZS_PRESSED = 3,
			/// <summary>Disabled</summary>      
			DNHZS_DISABLED = 4
		}

		/// <summary> "Page" Parts.Pager uses same states as Spin</summary>
		public enum UxThemePageParts : int
		{
			/// <summary>Up</summary>
			PGRP_UP = 1,
			/// <summary>Down</summary>
			PGRP_DOWN = 2,
			/// <summary>Horizontal Up</summary>
			PGRP_UPHORZ = 3,
			/// <summary>Horizontal Down</summary>
			PGRP_DOWNHORZ = 4
		}


		/// <summary> "Scrollbar" Parts</summary>
		public enum UxThemeSCROLLBARParts : int
		{
			/// <summary>Arrow button</summary>
			SBP_ARROWBTN = 1,
			/// <summary>Horizontal thumb button</summary>
			SBP_THUMBBTNHORZ = 2,
			/// <summary>Verical thumb button</summary>
			SBP_THUMBBTNVERT = 3,
			/// <summary>Horizontal lower track</summary>
			SBP_LOWERTRACKHORZ = 4,
			/// <summary>Horizontal upper track</summary>
			SBP_UPPERTRACKHORZ = 5,
			/// <summary>Vertical lower track</summary>
			SBP_LOWERTRACKVERT = 6,
			/// <summary>Vertical upper track</summary>
			SBP_UPPERTRACKVERT = 7,
			/// <summary>Horizontal gripper</summary>
			SBP_GRIPPERHORZ = 8,
			/// <summary>Vertical gripper</summary>
			SBP_GRIPPERVERT = 9,
			/// <summary>Size box</summary>
			SBP_SIZEBOX = 10
		}


		/// <summary>
		/// Scroll Arrow Button states
		/// </summary>
		public enum UxThemeARROWBTNStates : int
		{
			/// <summary>Up Normal</summary>
			ABS_UPNORMAL = 1,
			/// <summary>Up Hot</summary>
			ABS_UPHOT = 2,
			/// <summary>Up Pressed</summary>
			ABS_UPPRESSED = 3,
			/// <summary>Up Disabled</summary>
			ABS_UPDISABLED = 4,
			/// <summary>Down Normal</summary>
			ABS_DOWNNORMAL = 5,
			/// <summary>Down Hot</summary>
			ABS_DOWNHOT = 6,
			/// <summary>Down Pressed</summary>
			ABS_DOWNPRESSED = 7,
			/// <summary>Down Disabled</summary>
			ABS_DOWNDISABLED = 8,
			/// <summary>Left Normal</summary>
			ABS_LEFTNORMAL = 9,
			/// <summary>Left Hot</summary>
			ABS_LEFTHOT = 10,
			/// <summary>Left Pressed</summary>
			ABS_LEFTPRESSED = 11,
			/// <summary>Left Disabled</summary>
			ABS_LEFTDISABLED = 12,
			/// <summary>Right Normal</summary>
			ABS_RIGHTNORMAL = 13,
			/// <summary>Right Hot</summary>
			ABS_RIGHTHOT = 14,
			/// <summary>Right Pressed</summary>
			ABS_RIGHTPRESSED = 15,
			/// <summary>Right Disabled</summary>
			ABS_RIGHTDISABLED = 16
		}

		/// <summary>
		/// Scroll bar states
		/// </summary>
		public enum UxThemeSCROLLBARStates : int
		{
			/// <summary>Normal</summary>      
			SCRBS_NORMAL = 1,
			/// <summary>Hot</summary>      
			SCRBS_HOT = 2,
			/// <summary>Pressed</summary>      
			SCRBS_PRESSED = 3,
			/// <summary>Disabled</summary>      
			SCRBS_DISABLED = 4
		}

		/// <summary>
		/// Size box states
		/// </summary>
		public enum UxThemeSIZEBOXStates : int
		{
			/// <summary>Right Align</summary>
			SZB_RIGHTALIGN = 1,
			/// <summary>Left Align</summary>
			SZB_LEFTALIGN = 2
		}

		/// <summary> "Edit" Parts</summary>
		public enum UxThemeEDITParts : int
		{
			/// <summary>Text</summary>
			EP_EDITTEXT = 1,
			/// <summary>Caret</summary>
			EP_CARET = 2
		}

		/// <summary>
		/// Edit states
		/// </summary>
		public enum UxThemeEDITTEXTStates : int
		{
			/// <summary>Normal</summary>      
			ETS_NORMAL = 1,
			/// <summary>Hot</summary>      
			ETS_HOT = 2,
			/// <summary>Selected</summary>      
			ETS_SELECTED = 3,
			/// <summary>Disabled</summary>      
			ETS_DISABLED = 4,
			/// <summary>Focused</summary>      
			ETS_FOCUSED = 5,
			/// <summary>Read only</summary>      
			ETS_READONLY = 6,
			/// <summary>Assist</summary>      
			ETS_ASSIST = 7
		}

		/// <summary> "ComboBox" Parts</summary>
		public enum UxThemeComboBoxParts : int
		{
			/// <summary>Drop-down button</summary>      
			CP_DROPDOWNBUTTON = 1
		}

		/// <summary>
		/// Combo box states
		/// </summary>
		public enum UxThemeComboBoxStates : int
		{
			/// <summary>Normal</summary>      
			CBXS_NORMAL = 1,
			/// <summary>Hot</summary>      
			CBXS_HOT = 2,
			/// <summary>Pressed</summary>      
			CBXS_PRESSED = 3,
			/// <summary>Disabled</summary>      
			CBXS_DISABLED = 4
		}

		/// <summary> "Taskbar Clock" Parts</summary>
		public enum UxThemeCLOCKParts : int
		{
			/// <summary>Time</summary>      
			CLP_TIME = 1
		}

		/// <summary>
		/// Clock states
		/// </summary>
		public enum UxThemeCLOCKStates : int
		{
			/// <summary>Normal</summary>      
			CLS_NORMAL = 1
		}

		/// <summary> "Tray Notify" Parts</summary>
		public enum UxThemeTRAYNOTIFYParts : int
		{
			/// <summary>Background</summary>
			TNP_BACKGROUND = 1,
			/// <summary>Animation Background</summary>
			TNP_ANIMBACKGROUND = 2
		}

		/// <summary> "TaskBar" Parts</summary>
		public enum UxThemeTASKBARParts : int
		{
			/// <summary>Background bottom</summary>
			TBP_BACKGROUNDBOTTOM = 1,
			/// <summary>Background right</summary>
			TBP_BACKGROUNDRIGHT = 2,
			/// <summary>Background top</summary>
			TBP_BACKGROUNDTOP = 3,
			/// <summary>Background left</summary>
			TBP_BACKGROUNDLEFT = 4,
			/// <summary>Sizing bar bottom</summary>         
			TBP_SIZINGBARBOTTOM = 5,
			/// <summary>Sizing bar right</summary>
			TBP_SIZINGBARRIGHT = 6,
			/// <summary>Sizing bar top</summary>         
			TBP_SIZINGBARTOP = 7,
			/// <summary>Sizing bar left</summary>         
			TBP_SIZINGBARLEFT = 8
		}

		/// <summary> "TaskBand" Parts</summary>
		public enum UxThemeTASKBANDParts : int
		{
			/// <summary>Group count</summary>         
			TDP_GROUPCOUNT = 1,
			/// <summary>Flash button</summary>         
			TDP_FLASHBUTTON = 2,
			/// <summary>Flash button group menu</summary>         
			TDP_FLASHBUTTONGROUPMENU = 3
		}

		/// <summary> "StartPanel" Parts</summary>
		public enum UxThemeSTARTPANELParts : int
		{
			/// <summary>User pane</summary>         
			SPP_USERPANE = 1,
			/// <summary>More programs</summary>         
			SPP_MOREPROGRAMS = 2,
			/// <summary>More programs arrow</summary>         
			SPP_MOREPROGRAMSARROW = 3,
			/// <summary>Program list</summary>         
			SPP_PROGLIST = 4,
			/// <summary>Program list separator</summary>         
			SPP_PROGLISTSEPARATOR = 5,
			/// <summary>Places list</summary>         
			SPP_PLACESLIST = 6,
			/// <summary>Places list separator</summary>         
			SPP_PLACESLISTSEPARATOR = 7,
			/// <summary>Log off</summary>         
			SPP_LOGOFF = 8,
			/// <summary>Log off buttons</summary>         
			SPP_LOGOFFBUTTONS = 9,
			/// <summary>User picture</summary>         
			SPP_USERPICTURE = 10,
			/// <summary>Preview</summary>         
			SPP_PREVIEW = 11
		}

		/// <summary>
		/// More programs arrow states
		/// </summary>
		public enum UxThemeMOREPROGRAMSARROWStates : int
		{
			/// <summary>Normal</summary>      
			SPS_NORMAL = 1,
			/// <summary>Hot</summary>      
			SPS_HOT = 2,
			/// <summary>Pressed</summary>      
			SPS_PRESSED = 3
		}

		/// <summary>
		/// Log off button states
		/// </summary>
		public enum UxThemeLOGOFFBUTTONSStates : int
		{
			/// <summary>Normal</summary>      
			SPLS_NORMAL = 1,
			/// <summary>Hot</summary>      
			SPLS_HOT = 2,
			/// <summary>Pressed</summary>      
			SPLS_PRESSED = 3
		}

		/// <summary> "ExplorerBar" Parts</summary>
		public enum UxThemeEXPLORERBARParts : int
		{
			/// <summary>Header background</summary>         
			EBP_HEADERBACKGROUND = 1,
			/// <summary>Header close</summary>         
			EBP_HEADERCLOSE = 2,
			/// <summary>Header pin</summary>         
			EBP_HEADERPIN = 3,
			/// <summary>Header IE Bar menu</summary>         
			EBP_IEBARMENU = 4,
			/// <summary>Normal group background</summary>         
			EBP_NORMALGROUPBACKGROUND = 5,
			/// <summary>Normal group collapse</summary>         
			EBP_NORMALGROUPCOLLAPSE = 6,
			/// <summary>Normal group expand</summary>         
			EBP_NORMALGROUPEXPAND = 7,
			/// <summary>Normal group head</summary>         
			EBP_NORMALGROUPHEAD = 8,
			/// <summary>Special group background</summary>         
			EBP_SPECIALGROUPBACKGROUND = 9,
			/// <summary>Special group collapse</summary>         
			EBP_SPECIALGROUPCOLLAPSE = 10,
			/// <summary>Special group expand</summary>         
			EBP_SPECIALGROUPEXPAND = 11,
			/// <summary>Special group header</summary>         
			EBP_SPECIALGROUPHEAD = 12
		}

		/// <summary>
		/// Header close states
		/// </summary>
		public enum UxThemeHEADERCLOSEStates
		{
			/// <summary>Normal</summary>      
			EBHC_NORMAL = 1,
			/// <summary>Hot</summary>      
			EBHC_HOT = 2,
			/// <summary>Pressed</summary>      
			EBHC_PRESSED = 3
		}

		/// <summary>
		/// Header Pin states
		/// </summary>
		public enum UxThemeHEADERPINStates
		{
			/// <summary>Normal</summary>      
			EBHP_NORMAL = 1,
			/// <summary>Hot</summary>      
			EBHP_HOT = 2,
			/// <summary>Pressed</summary>      
			EBHP_PRESSED = 3,
			/// <summary>Selected normal</summary>      
			EBHP_SELECTEDNORMAL = 4,
			/// <summary>Selected hot</summary>      
			EBHP_SELECTEDHOT = 5,
			/// <summary>Selected pressed</summary>      
			EBHP_SELECTEDPRESSED = 6
		}

		/// <summary>
		/// IE Bar Menu states
		/// </summary>
		public enum UxThemeIEBARMENUStates
		{
			/// <summary>Normal</summary>      
			EBM_NORMAL = 1,
			/// <summary>Hot</summary>      
			EBM_HOT = 2,
			/// <summary>Pressed</summary>      
			EBM_PRESSED = 3
		}

		/// <summary>
		/// Normal group collapse states
		/// </summary>
		public enum UxThemeNORMALGROUPCOLLAPSEStates
		{
			/// <summary>Normal</summary>      
			EBNGC_NORMAL = 1,
			/// <summary>Hot</summary>      
			EBNGC_HOT = 2,
			/// <summary>Pressed</summary>      
			EBNGC_PRESSED = 3
		}

		/// <summary>
		/// Normal group expand states
		/// </summary>
		public enum UxThemeNORMALGROUPEXPANDStates
		{
			/// <summary>Normal</summary>      
			EBNGE_NORMAL = 1,
			/// <summary>Hot</summary>      
			EBNGE_HOT = 2,
			/// <summary>Pressed</summary>      
			EBNGE_PRESSED = 3
		}

		/// <summary>
		/// Special group collapse states
		/// </summary>
		public enum UxThemeSPECIALGROUPCOLLAPSEStates
		{
			/// <summary>Normal</summary>      
			EBSGC_NORMAL = 1,
			/// <summary>Hot</summary>      
			EBSGC_HOT = 2,
			/// <summary>Pressed</summary>      
			EBSGC_PRESSED = 3
		}

		/// <summary>
		/// Special group expand states
		/// </summary>
		public enum UxThemeSPECIALGROUPEXPANDStates
		{
			/// <summary>Normal</summary>      
			EBSGE_NORMAL = 1,
			/// <summary>Hot</summary>      
			EBSGE_HOT = 2,
			/// <summary>Pressed</summary>      
			EBSGE_PRESSED = 3
		}

		/// <summary> "TaskBand" Parts</summary>
		public enum UxThemeMENUBANDParts : int
		{
			/// <summary>New application button</summary>
			MDP_NEWAPPBUTTON = 1,
			/// <summary>Separator</summary>
			MDP_SEPARATOR = 2
		}

		/// <summary> "TaskBand" States</summary>
		public enum UxThemeMENUBANDStates : int
		{
			/// <summary>Normal</summary>      
			MDS_NORMAL = 1,
			/// <summary>Hot</summary>      
			MDS_HOT = 2,
			/// <summary>Pressed</summary>      
			MDS_PRESSED = 3,
			/// <summary>Disabled</summary>      
			MDS_DISABLED = 4,
			/// <summary>Checked</summary>      
			MDS_CHECKED = 5,
			/// <summary>Checked and Hot</summary>
			MDS_HOTCHECKED = 6
		}
		/// <summary>
		/// 
		/// </summary>
		public enum UxThemeProp : int
		{
			/// <summary> color of borders for BorderFill</summary>
			TMT_BORDERCOLOR = 3801,
			/// <summary> color of bg fill</summary>
			TMT_FILLCOLOR = 3802,
			/// <summary> color text is drawn in</summary>
			TMT_TEXTCOLOR = 3803,
			/// <summary> edge color</summary>
			TMT_EDGELIGHTCOLOR = 3804,
			/// <summary> edge color</summary>
			TMT_EDGEHIGHLIGHTCOLOR = 3805,
			/// <summary> edge color</summary>
			TMT_EDGESHADOWCOLOR = 3806,
			/// <summary> edge color</summary>
			TMT_EDGEDKSHADOWCOLOR = 3807,
			/// <summary> edge color</summary>
			TMT_EDGEFILLCOLOR = 3808,
			/// <summary> color of pixels that are treated as transparent (not drawn)</summary>
			TMT_TRANSPARENTCOLOR = 3809,
			/// <summary> first color in gradient</summary>
			TMT_GRADIENTCOLOR1 = 3810,
			/// <summary> second color in gradient</summary>
			TMT_GRADIENTCOLOR2 = 3811,
			/// <summary> third color in gradient</summary>
			TMT_GRADIENTCOLOR3 = 3812,
			/// <summary> forth color in gradient</summary>
			TMT_GRADIENTCOLOR4 = 3813,
			/// <summary> fifth color in gradient</summary>
			TMT_GRADIENTCOLOR5 = 3814,
			/// <summary> color of text shadow</summary>
			TMT_SHADOWCOLOR = 3815,
			/// <summary> color of glow produced by DrawThemeIcon</summary>
			TMT_GLOWCOLOR = 3816,
			/// <summary> color of text border</summary>
			TMT_TEXTBORDERCOLOR = 3817,
			/// <summary> color of text shadow</summary>
			TMT_TEXTSHADOWCOLOR = 3818,
			/// <summary> color that font-based glyph is drawn with</summary>
			TMT_GLYPHTEXTCOLOR = 3819,
			/// <summary> color of transparent pixels in GlyphImageFile</summary>
			TMT_GLYPHTRANSPARENTCOLOR = 3820,
			/// <summary> hint about fill color used (for custom controls)</summary>
			TMT_FILLCOLORHINT = 3821,
			/// <summary> hint about border color used (for custom controls)</summary>
			TMT_BORDERCOLORHINT = 3822,
			/// <summary> hint about accent color used (for custom controls)</summary>
			TMT_ACCENTCOLORHINT = 3823
		}

		// ------------------------------------------------------------------
		#endregion

		#region Low-level theming functions.
		// ------------------------------------------------------------------

		/// <summary>
		/// Get a known color.
		/// </summary>
		/// <param name="hTheme">The h theme.</param>
		/// <param name="iPartId">The i part id.</param>
		/// <param name="iStateId">The i state id.</param>
		/// <param name="iPropId">The i prop id.</param>
		/// <param name="pColor">Color of the p.</param>
		/// <returns></returns>
		[DllImport( @"uxtheme.dll", CharSet = CharSet.Auto )]
		public static extern int GetThemeColor(
			IntPtr hTheme,
			int iPartId,
			int iStateId,
			int iPropId,
			ref COLORREF pColor );

		/// <summary>
		/// Tests if a visual style for the current application is active.
		/// </summary>
		/// <returns></returns>
		[DllImport( @"uxtheme.dll" )]
		public static extern int IsThemeActive();

		/// <summary>
		/// Opens the theme data for a window and its associated class.
		/// </summary>
		/// <param name="hWnd">The h WND.</param>
		/// <param name="classList">The class list.</param>
		/// <returns></returns>
		[DllImport( @"uxtheme.dll" )]
		public static extern IntPtr OpenThemeData(
			IntPtr hWnd,
			[MarshalAs( UnmanagedType.LPTStr )] string classList );

		/// <summary>
		/// Closes the theme data handle.
		/// </summary>
		/// <param name="hTheme">The h theme.</param>
		/// <remarks>The CloseThemeData function should be called when a
		/// window that has a visual style applied is destroyed.</remarks>
		[DllImport( @"uxtheme.dll" )]
		public static extern void CloseThemeData(
			IntPtr hTheme );

		/// <summary>
		/// Draws the background image defined by the visual
		/// style for the specified control part.
		/// </summary>
		/// <param name="hTheme">The h theme.</param>
		/// <param name="hDC">The h DC.</param>
		/// <param name="partId">The part id.</param>
		/// <param name="stateId">The state id.</param>
		/// <param name="rect">The rect.</param>
		/// <param name="clipRect">The clip rect.</param>
		[DllImport( @"uxtheme.dll" )]
		public static extern void DrawThemeBackground(
			IntPtr hTheme,
			IntPtr hDC,
			int partId,
			int stateId,
			ref RECT rect,
			ref RECT clipRect );

		/// <summary>
		/// Draws one or more edges defined by the visual style
		/// of a rectangle.
		/// </summary>
		/// <param name="hTheme">The h theme.</param>
		/// <param name="hDC">The h DC.</param>
		/// <param name="partId">The part id.</param>
		/// <param name="stateId">The state id.</param>
		/// <param name="destRect">The dest rect.</param>
		/// <param name="edge">The edge.</param>
		/// <param name="flags">The flags.</param>
		/// <param name="contentRect">The content rect.</param>
		[DllImport( @"uxtheme.dll" )]
		public static extern void DrawThemeEdge(
			IntPtr hTheme,
			IntPtr hDC,
			int partId,
			int stateId,
			ref RECT destRect,
			uint edge,
			uint flags,
			ref RECT contentRect );

		/// <summary>
		/// Draws an image from an image list with the icon effect
		/// defined by the visual style.
		/// </summary>
		/// <param name="hTheme">The h theme.</param>
		/// <param name="hDC">The h DC.</param>
		/// <param name="partId">The part id.</param>
		/// <param name="stateId">The state id.</param>
		/// <param name="rect">The rect.</param>
		/// <param name="hIml">The h iml.</param>
		/// <param name="imageIndex">Index of the image.</param>
		[DllImport( @"uxtheme.dll" )]
		public static extern void DrawThemeIcon(
			IntPtr hTheme,
			IntPtr hDC,
			int partId,
			int stateId,
			ref RECT rect,
			IntPtr hIml,
			int imageIndex );

		/// <summary>
		/// Draws text using the color and font defined
		/// by the visual style.
		/// </summary>
		/// <param name="hTheme">The h theme.</param>
		/// <param name="hDC">The h DC.</param>
		/// <param name="partId">The part id.</param>
		/// <param name="stateId">The state id.</param>
		/// <param name="text">The text.</param>
		/// <param name="charCount">The char count.</param>
		/// <param name="textFlags">The text flags.</param>
		/// <param name="textFlags2">The text flags2.</param>
		/// <param name="rect">The rect.</param>
		[DllImport( @"uxtheme.dll" )]
		public static extern void DrawThemeText(
			IntPtr hTheme,
			IntPtr hDC,
			int partId,
			int stateId,
			[MarshalAs( UnmanagedType.LPTStr )] string text,
			int charCount,
			uint textFlags,
			uint textFlags2,
			ref RECT rect );

		/// <summary>
		/// Draws the part of a parent control that is covered by
		/// a partially-transparent or alpha-blended child control.
		/// </summary>
		/// <param name="hWnd">The h WND.</param>
		/// <param name="hDC">The h DC.</param>
		/// <param name="rect">The rect.</param>
		[DllImport( @"uxtheme.dll" )]
		public static extern void DrawThemeParentBackground(
			IntPtr hWnd,
			IntPtr hDC,
			ref RECT rect );

		/// <summary>
		/// Causes a window to use a different set of visual style
		/// information than its class normally uses.
		/// </summary>
		/// <param name="hWnd">The h WND.</param>
		/// <param name="subAppName">Name of the sub app.</param>
		/// <param name="subIdList">The sub id list.</param>
		[DllImport( @"uxtheme.dll" )]
		public static extern void SetWindowTheme(
			IntPtr hWnd,
			string subAppName,
			string subIdList );

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
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}