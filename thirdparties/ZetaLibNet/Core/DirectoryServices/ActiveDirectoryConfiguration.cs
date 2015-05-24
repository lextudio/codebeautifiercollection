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
	/// Class to configure an instance of the active directory class.
	/// </summary>
	public class ActiveDirectoryConfiguration
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public ActiveDirectoryConfiguration()
		{
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Configures the LDAP connection string for accessing the LDAP
		/// server.
		/// <example><c>MyServer</c></example>
		/// </summary>
		/// <value>The LDAP server.</value>
		public string LdapServer
		{
			get
			{
				return ldapServer;
			}
			set
			{
				ldapServer = value;
			}
		}

		/// <summary>
		/// Configures the LDAP connection base DN (distinguished name)
		/// string for accessing the LDAP server.
		/// <example><c>DC=duernau,DC=zeta-software,DC=de</c></example>
		/// </summary>
		/// <value>The LDAP base DN.</value>
		public string LdapBaseDN
		{
			get
			{
				return ldapBaseDN;
			}
			set
			{
				ldapBaseDN = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether [do impersonate].
		/// </summary>
		/// <value><c>true</c> if [do impersonate]; otherwise, <c>false</c>.</value>
		public bool DoImpersonate
		{
			get
			{
				return doImpersonate;
			}
			set
			{
				doImpersonate = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the impersonation user.
		/// </summary>
		/// <value>The name of the impersonation user.</value>
		public string ImpersonationUserName
		{
			get
			{
				return impersonationUserName;
			}
			set
			{
				impersonationUserName = value;
			}
		}

		/// <summary>
		/// Gets or sets the impersonation password.
		/// </summary>
		/// <value>The impersonation password.</value>
		public string ImpersonationPassword
		{
			get
			{
				return impersonationPassword;
			}
			set
			{
				impersonationPassword = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the impersonation domain.
		/// </summary>
		/// <value>The name of the impersonation domain.</value>
		public string ImpersonationDomainName
		{
			get
			{
				return impersonationDomainName;
			}
			set
			{
				impersonationDomainName = value;
			}
		}

		/// <summary>
		/// Gets or sets the dir entry user name prefix.
		/// </summary>
		/// <value>The dir entry user name prefix.</value>
		public string DirEntryUserNamePrefix
		{
			get
			{
				return dirEntryUserNamePrefix;
			}
			set
			{
				dirEntryUserNamePrefix = value;
			}
		}

		/// <summary>
		/// Gets or sets the dir entry user name suffix.
		/// </summary>
		/// <value>The dir entry user name suffix.</value>
		public string DirEntryUserNameSuffix
		{
			get
			{
				return dirEntryUserNameSuffix;
			}
			set
			{
				dirEntryUserNameSuffix = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the LDAP user.
		/// </summary>
		/// <value>The name of the LDAP user.</value>
		public string LdapUserName
		{
			get
			{
				return ldapUserName;
			}
			set
			{
				ldapUserName = value;
			}
		}

		/// <summary>
		/// Gets or sets the LDAP password.
		/// </summary>
		/// <value>The LDAP password.</value>
		public string LdapPassword
		{
			get
			{
				return ldapPassword;
			}
			set
			{
				ldapPassword = value;
			}
		}

		/// <summary>
		/// To get over the 1000-result-entries limit,
		/// set this to TRUE. Makes the processing slower, though.
		/// </summary>
		/// <value><c>true</c> if [use sliced queries]; otherwise, <c>false</c>.</value>
		public bool UseSlicedQueries
		{
			get
			{
				return useSlicedQueries;
			}
			set
			{
				useSlicedQueries = value;
			}
		}

		/// <summary>
		/// Enables caching for reading items.
		/// </summary>
		/// <value><c>true</c> if [allow for caching]; otherwise, <c>false</c>.</value>
		public bool AllowForCaching
		{
			get
			{
				return allowForCaching;
			}
			set
			{
				allowForCaching = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private string ldapServer;

		/// <summary>
		/// 
		/// </summary>
		private string ldapBaseDN;

		/// <summary>
		/// 
		/// </summary>
		private bool doImpersonate;

		/// <summary>
		/// 
		/// </summary>
		private string impersonationUserName;

		/// <summary>
		/// 
		/// </summary>
		private string impersonationPassword;

		/// <summary>
		/// 
		/// </summary>
		private string impersonationDomainName;

		/// <summary>
		/// 
		/// </summary>
		private string dirEntryUserNamePrefix;
		private string dirEntryUserNameSuffix;

		/// <summary>
		/// 
		/// </summary>
		private string ldapUserName;
		private string ldapPassword;

		/// <summary>
		/// 
		/// </summary>
		private bool useSlicedQueries = false;

		/// <summary>
		/// 
		/// </summary>
		private bool allowForCaching = true;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}