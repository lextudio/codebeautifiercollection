namespace ZetaLib.Core.DirectoryServices
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Globalization;
	using System.Security.Principal;
	using System.Threading;
	using System.Text;
	using ZetaLib.Core.Logging;
	using System.Collections.Generic;
	using System.DirectoryServices;
	using System.Diagnostics;
	using ZetaLib.Core.Properties;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Enumeration of flags that control an user account.
	/// <see href="ms-help://MS.MSDNQTR.2002JUL.1033/netdir/adsi/ads_user_flag_enum.htm"/>
	/// </summary>
	[Flags]
	public enum ADUserFlags
	{
		#region Flags.
		// ------------------------------------------------------------------

		/// <summary></summary>
		Script = 0x0001,
		/// <summary></summary>
		Accountdisable = 0x0002,
		/// <summary></summary>
		HomedirRequired = 0x0008,
		/// <summary></summary>
		Lockout = 0x0010,
		/// <summary></summary>
		PasswdNotreqd = 0x0020,
		/// <summary></summary>
		PasswdCantChange = 0x0040,
		/// <summary></summary>
		EncryptedTextPasswordAllowed = 0x0080,
		/// <summary></summary>
		TempDuplicateAccount = 0x0100,
		/// <summary></summary>
		NormalAccount = 0x0200,
		/// <summary></summary>
		InterdomainTrustAccount = 0x0800,
		/// <summary></summary>
		WorkstationTrustAccount = 0x1000,
		/// <summary></summary>
		ServerTrustAccount = 0x2000,
		/// <summary></summary>
		DontExpirePasswd = 0x10000,
		/// <summary></summary>
		MnsLogonAccount = 0x20000,
		/// <summary></summary>
		SmartcardRequired = 0x40000,
		/// <summary></summary>
		TrustedForDelegation = 0x80000,
		/// <summary></summary>
		NotDelegated = 0x100000,
		/// <summary></summary>
		UseDesKeyOnly = 0x200000,
		/// <summary></summary>
		DontRequirePreauth = 0x400000,
		/// <summary></summary>
		PasswordExpired = 0x800000,
		/// <summary></summary>
		TrustedToAuthenticateForDelegation = 0x1000000

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}