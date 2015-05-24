namespace ZetaLib.Windows.Misc
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Runtime.InteropServices;
	using System.Windows;
	using System.Windows.Forms;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Class that helps in detecting and establishing connections to the
	/// Internet.
	/// </summary>
	public sealed class InternetConnection
	{
		#region Public types.
		// ------------------------------------------------------------------

		/// <summary>
		/// The type of the Internet connection.
		/// </summary>
		public enum ConnectionType
		{
			#region Enum member.

			/// <summary>
			/// It cannot be determined whether connected or not.
			/// </summary>
			Unknown,

			/// <summary>
			/// No connection at all.
			/// </summary>
			None,

			/// <summary>
			/// Connected by modem.
			/// </summary>
			Modem,

			/// <summary>
			/// Connected through LAN.
			/// </summary>
			Lan,

			/// <summary>
			/// Connected through a proxy server.
			/// </summary>
			Proxy

			#endregion
		}

		/// <summary>
		/// Information about an Internet connection state.
		/// </summary>
		public sealed class ConnectionStateInfo
		{
			#region Public methods.

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="connectionType">Type of the connection.</param>
			/// <param name="isOnline">if set to <c>true</c> [is online].</param>
			/// <param name="isRasInstalled">if set to <c>true</c> [is ras installed].</param>
			public ConnectionStateInfo(
				ConnectionType connectionType,
				bool isOnline,
				bool isRasInstalled )
			{
				this.connectionType = connectionType;
				this.isOnline = isOnline;
				this.isRasInstalled = isRasInstalled;
			}

			#endregion

			#region Public properties.

			/// <summary>
			/// Get the type of the connection.
			/// </summary>
			/// <value>The type of the connection.</value>
			public ConnectionType ConnectionType
			{
				get
				{
					return connectionType;
				}
			}

			/// <summary>
			/// Check whether currently is online.
			/// </summary>
			/// <value><c>true</c> if this instance is online; otherwise, <c>false</c>.</value>
			public bool IsOnline
			{
				get
				{
					return isOnline;
				}
			}

			/// <summary>
			/// Check whether RAS is installed.
			/// </summary>
			/// <value>
			/// 	<c>true</c> if this instance is ras installed; otherwise, <c>false</c>.
			/// </value>
			public bool IsRasInstalled
			{
				get
				{
					return isRasInstalled;
				}
			}

			#endregion

			#region Private helper.

			private ConnectionType connectionType;
			private bool isOnline;
			private bool isRasInstalled;

			#endregion
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Query information about the current Internet connection state.
		/// </summary>
		/// <returns>
		/// Returns the information about the current Internet
		/// connection state.
		/// </returns>
		public static ConnectionStateInfo GetConnectionState()
		{
			InternetConnectionState flags = 0;

			bool isConnected = InternetGetConnectedState(
				ref flags,
				0 );

			// --

			if ( !isConnected )
			{
				return new ConnectionStateInfo(
					ConnectionType.None,
					false,
					false );
			}
			else
			{
				ConnectionType connectionType = ConnectionType.None;
				bool isOnline = false;
				bool isRasInstalled = false;

				// --

				if ( ((int)flags & 0xF) == 0 )
				{
					connectionType = ConnectionType.Unknown;
				}
				else if ( ((int)flags & 0xF) == (int)InternetConnectionState.ModemConnection )
				{
					connectionType = ConnectionType.Modem;
				}
				else if ( ((int)flags & 0xF) == (int)InternetConnectionState.LanConnection )
				{
					connectionType = ConnectionType.Lan;
				}
				else if ( ((int)flags & 0xF) == (int)InternetConnectionState.ProxyConnection )
				{
					connectionType = ConnectionType.Proxy;
				}

				if ( ((int)flags & (int)InternetConnectionState.Offline) == 0 )
				{
					isOnline = true;
				}
				else
				{
					isOnline = false;
				}

				if ( ((int)flags & (int)InternetConnectionState.RasInstalled) == 0 )
				{
					isRasInstalled = false;
				}
				else
				{
					isRasInstalled = true;
				}

				// --

				return new ConnectionStateInfo(
					connectionType,
					isOnline,
					isRasInstalled );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private helper.
		// ------------------------------------------------------------------

		/// <summary>
		/// Internets the state of the get connected.
		/// </summary>
		/// <param name="flags">The flags.</param>
		/// <param name="rReserved">The r reserved.</param>
		/// <returns></returns>
		[DllImport( @"wininet.dll", SetLastError = true )]
		static extern bool InternetGetConnectedState(
			ref InternetConnectionState flags,
			int rReserved );

		/// <summary>
		/// Flags that indicate the different states.
		/// </summary>
		[Flags]
		private enum InternetConnectionState : int
		{
			#region Enum members.

			ModemConnection = 0x1,
			LanConnection = 0x2,
			ProxyConnection = 0x4,
			RasInstalled = 0x10,
			Offline = 0x20,
			Configured = 0x40

			#endregion
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}