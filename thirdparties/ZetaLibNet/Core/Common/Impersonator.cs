namespace ZetaLib.Core.Common
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Security;
	using System.Security.Principal;
	using System.Runtime.InteropServices;
	using System.ComponentModel;
	using ZetaLib.Core.Logging;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Impersonation of a user. Allows to execute code under another
	/// user context.
	/// Please note that the account that instantiates the Impersonator class
	/// needs to have the 'Act as part of operating system' privilege set.
	/// </summary>
	/// <remarks>	
	/// This class is based on the information in the Microsoft knowledge base
	/// article http://support.microsoft.com/default.aspx?scid=kb;en-us;Q306158
	/// 
	/// Encapsulate an instance into a using-directive like e.g.:
	/// 
	///		...
	///		using ( new Impersonator( "myUsername", "myDomainname", "myPassword" ) )
	///		{
	///			...
	///			[code that executes under the new context]
	///			...
	///		}
	///		...
	/// </remarks>
	public class Impersonator :
		IDisposable
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor. Starts the impersonation with the given credentials.
		/// Please note that the account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		public Impersonator(
			string userName,
			string domainName,
			string password )
		{
			ImpersonateValidUser(
				userName,
				domainName,
				password,
				LoginType.Interactive );
		}

		/// <summary>
		/// Constructor. Starts the impersonation with the given credentials.
		/// Please note that the account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">The login type.</param>
		public Impersonator(
			string userName,
			string domainName,
			string password,
			LoginType loginType )
		{
			ImpersonateValidUser(
				userName,
				domainName,
				password,
				loginType );
		}

		/// <summary>
		/// Constructor. Starts the impersonation with the given credentials.
		/// Please note that the account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		public Impersonator(
			string userName,
			string domainName,
			SecureString password )
		{
			ImpersonateValidUser(
				userName,
				domainName,
				password,
				LoginType.Interactive );
		}

		/// <summary>
		/// Constructor. Starts the impersonation with the given credentials.
		/// Please note that the account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">The login type.</param>
		public Impersonator(
			string userName,
			string domainName,
			SecureString password,
			LoginType loginType )
		{
			ImpersonateValidUser(
				userName,
				domainName,
				password,
				loginType );
		}

		// ------------------------------------------------------------------
		#endregion

		#region IDisposable member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Performs application-defined tasks associated with freeing,
		/// releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			UndoImpersonation();
			GC.SuppressFinalize( this );
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="Impersonator"/> is reclaimed by garbage collection.
		/// </summary>
		~Impersonator()
		{
			UndoImpersonation();
		}

		// ------------------------------------------------------------------
		#endregion

		#region P/Invoke.
		// ------------------------------------------------------------------

		/// <summary>
		/// Logons the user.
		/// </summary>
		/// <param name="lpszUserName">Name of the LPSZ user.</param>
		/// <param name="lpszDomain">The LPSZ domain.</param>
		/// <param name="lpszPassword">The LPSZ password.</param>
		/// <param name="dwLogonType">Type of the dw logon.</param>
		/// <param name="dwLogonProvider">The dw logon provider.</param>
		/// <param name="phToken">The ph token.</param>
		/// <returns></returns>
		[DllImport( @"advapi32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		private static extern int LogonUser(
			string lpszUserName,
			string lpszDomain,
			string lpszPassword,
			int dwLogonType,
			int dwLogonProvider,
			ref IntPtr phToken );

		/// <summary>
		/// Logons the user2.
		/// </summary>
		/// <param name="lpszUserName">Name of the LPSZ user.</param>
		/// <param name="lpszDomain">The LPSZ domain.</param>
		/// <param name="Password">The password.</param>
		/// <param name="dwLogonType">Type of the dw logon.</param>
		/// <param name="dwLogonProvider">The dw logon provider.</param>
		/// <param name="phToken">The ph token.</param>
		/// <returns></returns>
		[DllImport( @"advapi32.dll", EntryPoint = @"LogonUser", CharSet = CharSet.Unicode, SetLastError = true )]
		private static extern int LogonUser2(
			string lpszUserName,
			string lpszDomain,
			IntPtr Password,
			int dwLogonType,
			int dwLogonProvider,
			ref IntPtr phToken );

		/// <summary>
		/// Duplicates the token.
		/// </summary>
		/// <param name="hToken">The h token.</param>
		/// <param name="impersonationLevel">The impersonation level.</param>
		/// <param name="hNewToken">The h new token.</param>
		/// <returns></returns>
		[DllImport( @"advapi32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		private static extern int DuplicateToken(
			IntPtr hToken,
			int impersonationLevel,
			ref IntPtr hNewToken );

		/// <summary>
		/// Reverts to self.
		/// </summary>
		/// <returns></returns>
		[DllImport( @"advapi32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		private static extern bool RevertToSelf();

		/// <summary>
		/// Closes the handle.
		/// </summary>
		/// <param name="handle">The handle.</param>
		/// <returns></returns>
		[DllImport( @"kernel32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		private static extern bool CloseHandle(
			IntPtr handle );

		private const int LOGON32_LOGON_INTERACTIVE = 2;
		private const int LOGON32_PROVIDER_DEFAULT = 0;

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Does the actual impersonation.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		private void ImpersonateValidUser(
			string userName,
			string domainName,
			string password,
			LoginType loginType )
		{
			LogCentral.Current.LogInfo(
				string.Format(
				@"[Impersonation] About to impersonate as domain '{0}', user '{1}'.",
				domainName,
				userName ) );

			try
			{
				if ( domainName != null || domainName.Length <= 0 )
				{
					domainName = null;
				}

				WindowsIdentity tempWindowsIdentity = null;
				IntPtr token = IntPtr.Zero;
				IntPtr tokenDuplicate = IntPtr.Zero;

				try
				{
					if ( RevertToSelf() )
					{
						if ( LogonUser(
							userName,
							domainName,
							password,
							(int)loginType,
							LOGON32_PROVIDER_DEFAULT,
							ref token ) != 0 )
						{
							if ( DuplicateToken( token, 2, ref tokenDuplicate ) != 0 )
							{
								tempWindowsIdentity = new WindowsIdentity( tokenDuplicate );
								impersonationContext = tempWindowsIdentity.Impersonate();
							}
							else
							{
								int le = Marshal.GetLastWin32Error();
								throw new Win32Exception( le );
							}
						}
						else
						{
							int le = Marshal.GetLastWin32Error();
							throw new Win32Exception( le );
						}
					}
					else
					{
						int le = Marshal.GetLastWin32Error();
						throw new Win32Exception( le );
					}
				}
				finally
				{
					if ( token != IntPtr.Zero )
					{
						CloseHandle( token );
					}
					if ( tokenDuplicate != IntPtr.Zero )
					{
						CloseHandle( tokenDuplicate );
					}
				}
			}
			catch ( Exception x )
			{
				LogCentral.Current.LogError(
					string.Format(
					@"[Impersonation] Error impersonating as domain '{0}', user '{1}'.",
					domainName,
					userName ),
					x );

				throw;
			}

			LogCentral.Current.LogInfo(
				string.Format(
				@"[Impersonation] Successfully impersonated as domain '{0}', user '{1}'.",
				domainName,
				userName ) );
		}


		/// <summary>
		/// Does the actual impersonation.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		private void ImpersonateValidUser(
			string userName,
			string domainName,
			SecureString password,
			LoginType loginType )
		{
			LogCentral.Current.LogInfo(
				string.Format(
				@"[Impersonation] About to impersonate as domain '{0}', user '{1}'.",
				domainName,
				userName ) );

			try
			{
				if ( domainName != null || domainName.Length <= 0 )
				{
					domainName = null;
				}

				WindowsIdentity tempWindowsIdentity = null;
				IntPtr token = IntPtr.Zero;
				IntPtr tokenDuplicate = IntPtr.Zero;
				IntPtr passwordPtr = IntPtr.Zero;

				try
				{
					if ( RevertToSelf() )
					{
						// Marshal the SecureString to unmanaged memory.
						passwordPtr = 
							Marshal.SecureStringToGlobalAllocUnicode( password );

						if ( LogonUser2(
							userName,
							domainName,
							passwordPtr,
							(int)loginType,
							LOGON32_PROVIDER_DEFAULT,
							ref token ) != 0 )
						{
							if ( DuplicateToken( token, 2, ref tokenDuplicate ) != 0 )
							{
								tempWindowsIdentity = new WindowsIdentity( tokenDuplicate );
								impersonationContext = tempWindowsIdentity.Impersonate();
							}
							else
							{
								int le =Marshal.GetLastWin32Error();
								throw new Win32Exception( le );
							}
						}
						else
						{
							int le = Marshal.GetLastWin32Error();
							throw new Win32Exception( le );
						}
					}
					else
					{
						int le = Marshal.GetLastWin32Error();
						throw new Win32Exception( le );
					}
				}
				finally
				{
					if ( token != IntPtr.Zero )
					{
						CloseHandle( token );
					}
					if ( tokenDuplicate != IntPtr.Zero )
					{
						CloseHandle( tokenDuplicate );
					}

					// Zero-out and free the unmanaged string reference.
					Marshal.ZeroFreeGlobalAllocUnicode( passwordPtr );
				}
			}
			catch ( Exception x )
			{
				LogCentral.Current.LogError(
					string.Format(
					@"[Impersonation] Error impersonating as domain '{0}', user '{1}'.",
					domainName,
					userName ),
					x );

				throw;
			}

			LogCentral.Current.LogInfo(
				string.Format(
				@"[Impersonation] Successfully impersonated as domain '{0}', user '{1}'.",
				domainName,
				userName ) );
		}

		/// <summary>
		/// Reverts the impersonation.
		/// </summary>
		private void UndoImpersonation()
		{
			if ( impersonationContext != null )
			{
				LogCentral.Current.LogInfo(
					string.Format(
					@"[Impersonation] About to undo impersonation." ) );

				try
				{
					impersonationContext.Undo();
					impersonationContext = null;
				}
				catch ( Exception x )
				{
					LogCentral.Current.LogError(
						string.Format(
						@"[Impersonation] Error undoing impersonation." ),
						x );

					throw;
				}

				LogCentral.Current.LogInfo(
					string.Format(
					@"[Impersonation] Successfully undone impersonation." ) );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private WindowsImpersonationContext impersonationContext = null;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// How to log in the user.
	/// </summary>
	public enum LoginType
	{
		#region Enum members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Interactive. This is the default.
		/// </summary>
		Interactive = 2,

		/// <summary>
		/// 
		/// </summary>
		Batch = 4,

		/// <summary>
		/// 
		/// </summary>
		Network = 3,

		/// <summary>
		/// 
		/// </summary>
		NetworkClearText = 0,

		/// <summary>
		/// 
		/// </summary>
		Service = 5,

		/// <summary>
		/// 
		/// </summary>
		Unlock = 7

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}