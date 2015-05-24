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
	using ZetaLib.Core.Common;
	using ZetaLib.Core.Localization;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Functions for easy accessing the Active Directory.
	/// </summary>
	public class ActiveDirectory
	{
		#region Public methods.
		// ------------------------------------------------------------------
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		public ActiveDirectory(
			ActiveDirectoryConfiguration configuration )
		{
			this.configuration = configuration;

			if ( configuration.AllowForCaching )
			{
				LogCentral.Current.LogInfo(
					@"Cache active, creating cache items." );

				cacheFor_Users = new ActiveDirectoryUserCache();
				cacheFor_Groups = new ActiveDirectoryGroupCache();
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Get the configuration properties for this instance.
		/// </summary>
		/// <value>The configuration.</value>
		public ActiveDirectoryConfiguration Configuration
		{
			get
			{
				return configuration;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Read informations of a user, by its GUID.
		/// </summary>
		/// <param name="samName">Name of the sam.</param>
		/// <returns>
		/// Returns the information object or <c>null</c> if the
		/// user is not found.
		/// </returns>
		public ADUserInfo GetUserInfoBySamName(
			string samName )
		{
			if ( configuration.AllowForCaching )
			{
				ADUserInfo p =
					cacheFor_Users.FindBySamName( samName );
				if ( p != null )
				{
					LogCentral.Current.LogInfo(
						string.Format(
						@"Cache HIT for user by SAM name '{0}'.",
						samName ) );

					return p;
				}
			}

			// --

			string ldapString = LdapBaseString;
			string ldapFilter =
				string.Format(
				@"(&(objectCategory=person)(sAMAccountName={0}))",
				samName );

			LogCentral.Current.LogDebug(
				string.Format(
				@"GetUserInfo(): About to query LDAP server with LDAP string '{0}' and filter string '{1}'.",
				ldapString,
				ldapFilter ) );

			DirectoryEntry entry = GetDirectoryEntry( ldapString );

			DirectorySearcher searcher = new DirectorySearcher( entry );
			searcher.Filter = ldapFilter;
			searcher.SizeLimit = adSizeReturn;

			// --

			ADUserInfo info = GetUserInfoBySearcher( searcher );

			LogCentral.Current.LogDebug(
				string.Format(
				@"GetUserInfo(): Query SUCCEEDED on LDAP server with LDAP string '{0}' and filter string '{1}'.",
				ldapString,
				ldapFilter ) );

			// --

			if ( configuration.AllowForCaching )
			{
				cacheFor_Users.Add( info );
			}

			return info;
		}

		/// <summary>
		/// Read informations of a user, by its GUID.
		/// </summary>
		/// <param name="samName">Name of the sam.</param>
		/// <returns>
		/// Returns the information object or <c>null</c> if the
		/// user is not found.
		/// </returns>
		public ADGroupInfo GetGroupInfoBySamName(
			string samName )
		{
			if ( configuration.AllowForCaching )
			{
				ADGroupInfo p =
					cacheFor_Groups.FindBySamName( samName );
				if ( p != null )
				{
					LogCentral.Current.LogInfo(
						string.Format(
						@"Cache HIT for group by SAM name '{0}'.",
						samName ) );

					return p;
				}
			}

			// --

			string ldapString = LdapBaseString;
			string ldapFilter =
				string.Format(
				@"(&(objectCategory=group)(sAMAccountName={0}))",
				samName );

			LogCentral.Current.LogDebug(
				string.Format(
				@"GetGroupInfo(): About to query LDAP server with LDAP string '{0}' and filter string '{1}'.",
				ldapString,
				ldapFilter ) );

			DirectoryEntry entry = GetDirectoryEntry( ldapString );

			DirectorySearcher searcher = new DirectorySearcher( entry );
			searcher.Filter = ldapFilter;
			searcher.SizeLimit = adSizeReturn;

			// --

			ADGroupInfo info = GetGroupInfo( searcher );

			LogCentral.Current.LogDebug(
				string.Format(
				@"GetGroupInfo(): Query SUCCEEDED on LDAP server with LDAP string '{0}' and filter string '{1}'.",
				ldapString,
				ldapFilter ) );

			// --

			if ( configuration.AllowForCaching )
			{
				cacheFor_Groups.Add( info );
			}

			return info;
		}

		/// <summary>
		/// Read informations of a user, by its GUID.
		/// </summary>
		/// <param name="guid">The GUID of the user to read.</param>
		/// <returns>
		/// Returns the information object or <c>null</c> if the
		/// user is not found.
		/// </returns>
		public ADUserInfo GetUserInfoByGuid(
			Guid guid )
		{
			if ( configuration.AllowForCaching )
			{
				ADUserInfo p =
					cacheFor_Users.FindByGuid( guid );
				if ( p != null )
				{
					LogCentral.Current.LogInfo(
						string.Format(
						@"Cache HIT for user by GUID '{0}'.",
						guid ) );

					return p;
				}
			}

			// --

			byte[] guidBytes = guid.ToByteArray();
			string guidString = HexEscape( guidBytes );

			string ldapString = LdapBaseString;
			string ldapFilter =
				string.Format(
				@"(&(objectCategory=person)(objectGUID={0}))",
				guidString );

			LogCentral.Current.LogDebug(
				string.Format(
				@"GetUserInfo(): About to query LDAP server with LDAP string '{0}' and filter string '{1}'.",
				ldapString,
				ldapFilter ) );

			DirectoryEntry entry = GetDirectoryEntry( ldapString );

			DirectorySearcher searcher = new DirectorySearcher( entry );
			searcher.Filter = ldapFilter;
			searcher.SizeLimit = adSizeReturn;

			// --

			ADUserInfo info = GetUserInfoBySearcher( searcher );

			LogCentral.Current.LogDebug(
				string.Format(
				@"GetUserInfo(): Query SUCCEEDED on LDAP server with LDAP string '{0}' and filter string '{1}'.",
				ldapString,
				ldapFilter ) );

			// --

			if ( configuration.AllowForCaching )
			{
				cacheFor_Users.Add( info );
			}

			return info;
		}

		/// <summary>
		/// Read informations of a user, by its GUID.
		/// </summary>
		/// <param name="guid">The GUID of the user to read.</param>
		/// <returns>
		/// Returns the information object or <c>null</c> if the
		/// user is not found.
		/// </returns>
		public ADGroupInfo GetGroupInfoByGuid(
			Guid guid )
		{
			if ( configuration.AllowForCaching )
			{
				ADGroupInfo p =
					cacheFor_Groups.FindByGuid( guid );
				if ( p != null )
				{
					LogCentral.Current.LogInfo(
						string.Format(
						@"Cache HIT for group by GUID '{0}'.",
						guid ) );

					return p;
				}
			}

			// --

			byte[] guidBytes = guid.ToByteArray();
			string guidString = HexEscape( guidBytes );

			string ldapString = LdapBaseString;
			string ldapFilter =
				string.Format(
				@"(&(objectCategory=group)(objectGUID={0}))",
				guidString );

			LogCentral.Current.LogDebug(
				string.Format(
				@"GetGroupInfo(): About to query LDAP server with LDAP string '{0}' and filter string '{1}'.",
				ldapString,
				ldapFilter ) );

			DirectoryEntry entry = GetDirectoryEntry( ldapString );

			DirectorySearcher searcher = new DirectorySearcher( entry );
			searcher.Filter = ldapFilter;
			searcher.SizeLimit = adSizeReturn;

			// --

			ADGroupInfo info = GetGroupInfo( searcher );

			LogCentral.Current.LogDebug(
				string.Format(
				@"GetGroupInfo(): Query SUCCEEDED on LDAP server with LDAP string '{0}' and filter string '{1}'.",
				ldapString,
				ldapFilter ) );

			// --

			if ( configuration.AllowForCaching )
			{
				cacheFor_Groups.Add( info );
			}

			return info;
		}

		/// <summary>
		/// Modifies the information of an EXISTING user in
		/// the ActiveDirectory.
		/// </summary>
		/// <param name="info">The information of the existing user.
		/// The Guid field must be valid.</param>
		public void PutUserInfo(
			ADUserInfo info )
		{
			try
			{
				Impersonator impersonator = null;

				if ( configuration.DoImpersonate )
				{
					LogCentral.Current.LogDebug(
						string.Format(
						@"PutUserInfo(): About to impersonate with domain name '{0}' and user name '{1}'.",
						configuration.ImpersonationDomainName,
						configuration.ImpersonationUserName ) );

					impersonator = new Impersonator(
						configuration.ImpersonationUserName,
						configuration.ImpersonationDomainName,
						configuration.ImpersonationPassword );

					LogCentral.Current.LogDebug(
						string.Format(
						@"PutUserInfo(): Impersonation succeeded." ) );
				}

				try
				{
					byte[] guidBytes = info.Guid.ToByteArray();
					string guidString = HexEscape( guidBytes );

					string ldapString = LdapBaseString;
					string ldapFilter =
						string.Format(
						@"(&(objectCategory=person)(objectGUID={0}))",
						guidString );

					LogCentral.Current.LogDebug(
						string.Format(
						@"PutUserInfo(): About to query LDAP server with LDAP string '{0}' and filter string '{1}'.",
						ldapString,
						ldapFilter ) );

					DirectoryEntry entry = GetDirectoryEntry( ldapString );

					DirectorySearcher searcher = new DirectorySearcher( entry );
					searcher.Filter = ldapFilter;
					searcher.SizeLimit = adSizeReturn;

					// --

					// Use this call only to fill the req. fields.
					ADUserInfo currentInfo = GetUserInfoBySearcher( searcher );

					SearchResult result = searcher.FindOne();
					DirectoryEntry e = result.GetDirectoryEntry();
					PutUserInfo( e, currentInfo, info );

					// Actually store back.
					e.CommitChanges();
				}
				finally
				{
					if ( impersonator != null )
					{
						impersonator.Dispose();
						impersonator = null;
					}
				}
			}
			catch ( Exception x )
			{
				LogCentral.Current.LogError(
					string.Format(
					@"ActiveDirectory.PutUserInfo(): Exception occured for putting the user infos. The following values were set through SetWP(): '{0}'.",
					debugSettedWPValues ),
					x );


				throw;
			}
		}

		/// <summary>
		/// Modifies the information of an EXISTING user in
		/// the ActiveDirectory.
		/// </summary>
		/// <param name="info">The information of the existing user.
		/// The Guid field must be valid.</param>
		/// <param name="oldPassword">The old password.</param>
		/// <param name="newPassword">The new password.</param>
		public void ChangePassword(
			ADUserInfo info,
			string oldPassword,
			string newPassword )
		{
			try
			{
				Impersonator impersonator = null;

				if ( configuration.DoImpersonate )
				{
					LogCentral.Current.LogDebug(
						string.Format(
						@"ChangePassword(): About to impersonate with domain name '{0}' and user name '{1}'.",
						configuration.ImpersonationDomainName,
						configuration.ImpersonationUserName ) );

					impersonator = new Impersonator(
						configuration.ImpersonationUserName,
						configuration.ImpersonationDomainName,
						configuration.ImpersonationPassword );

					LogCentral.Current.LogDebug(
						string.Format(
						@"ChangePassword(): Impersonation succeeded." ) );
				}

				try
				{
					byte[] guidBytes = info.Guid.ToByteArray();
					string guidString = HexEscape( guidBytes );

					string ldapString = LdapBaseString;
					string ldapFilter =
						string.Format(
						@"(&(objectCategory=person)(objectGUID={0}))",
						guidString );

					LogCentral.Current.LogDebug(
						string.Format(
						@"ChangePassword(): About to query LDAP server with LDAP string '{0}' and filter string '{1}'.",
						ldapString,
						ldapFilter ) );

					DirectoryEntry entry = GetDirectoryEntry( ldapString );

					DirectorySearcher searcher =
						new DirectorySearcher( entry );
					searcher.Filter = ldapFilter;
					searcher.SizeLimit = adSizeReturn;

					// --

					// Use this call only to fill the req. fields.
					ADUserInfo currentInfo = GetUserInfoBySearcher( searcher );

					SearchResult result = searcher.FindOne();
					DirectoryEntry e = result.GetDirectoryEntry();

					// --

					e.Invoke( @"setPassword", new object[] { newPassword } );
					//e.Invoke( "changePassword", new object[] { oldPassword, newPassword } );

					// --

					// Actually store back.
					e.CommitChanges();
				}
				finally
				{
					if ( impersonator != null )
					{
						impersonator.Dispose();
						impersonator = null;
					}
				}
			}
			catch ( Exception x )
			{
				LogCentral.Current.LogError(
					string.Format(
					@"ActiveDirectory.ChangePassword(): Exception occured for putting the user infos. The following values were set through SetWP(): '{0}'.",
					debugSettedWPValues ),
					x );

				throw;
			}
		}

		/// <summary>
		/// Read informations of a user, by its name and password.
		/// </summary>
		/// <param name="samName">Name of the sam.</param>
		/// <param name="password">The password.</param>
		/// <returns>
		/// Returns the information object or <c>null</c>
		/// if the user is not found or
		/// the name and/or password is invalid.
		/// </returns>
		public ADUserInfo GetUserInfoByUserNameAndPassword(
			string samName,
			string password )
		{
			LogCentral.Current.LogInfo(
				@"++++++++++++++++++++++++++++++++++++++++++++++++" );
			LogCentral.Current.LogInfo(
				string.Format(
				@"+++ About to start core user authentication for user with name '{0}'. +++",
				samName ) );

			try
			{
				// Not empty and no wildcards allowed.
				if ( string.IsNullOrEmpty( samName ) )
				{
					LogCentral.Current.LogDebug(
						string.Format(
						@"GetUserInfo(u,p): userName is NULL or empty, exiting with NULL return value."
						) );

					return null;
				}
				else if ( samName.IndexOf( @"*" ) >= 0 )
				{
					LogCentral.Current.LogDebug(
						string.Format(
						@"GetUserInfo(u,p): userName is '*', exiting with NULL return value."
						) );

					return null;
				}
				else
				{
					LogCentral.Current.LogDebug(
						string.Format(
						@"GetUserInfo(u,p): userName is '{0}'.",
						samName
						) );

					if ( string.IsNullOrEmpty( configuration.LdapServer ) ||
						string.IsNullOrEmpty( configuration.LdapBaseDN ) )
					{
						throw new Exception(
							Resources.Str_ZetaLib_Core_Common_ActiveDirectory_01 );
					}
					else
					{
						string ldapUrl = LdapBaseString;
						string ldapFilter = string.Format(
							@"(&(objectCategory=person)(sAMAccountName={0}))",
							samName );

						// --

						// Support an alternative way.
						string directoryEntryLdapUserName =
							configuration.DirEntryUserNamePrefix +
							samName +
							configuration.DirEntryUserNameSuffix;

						LogCentral.Current.LogDebug(
							string.Format(
							@"GetUserInfo(): (A) about to create DirectorySearcher with directory entry LDAP user name '{0}'.",
							directoryEntryLdapUserName ) );

						DirectoryEntry entry = new DirectoryEntry(
							ldapUrl,
							directoryEntryLdapUserName,
							password,
							AuthenticationType );

						DirectorySearcher searcher =
							new DirectorySearcher( entry );
						searcher.Filter = ldapFilter;
						searcher.SizeLimit = adSizeReturn;

						// --

						ADUserInfo info = GetUserInfoBySearcher( searcher );
						return info;
					}
				}
			}
			finally
			{
				LogCentral.Current.LogInfo(
					string.Format(
					@"+++ /Finished core user authentication for user with name '{0}'. +++",
					samName ) );
				LogCentral.Current.LogInfo(
					@"++++++++++++++++++++++++++++++++++++++++++++++++" );
			}
		}

		/// <summary>
		/// If config-userName/password is available, use it.
		/// </summary>
		/// <param name="ldapUrl">The LDAP URL.</param>
		/// <returns></returns>
		public DirectoryEntry GetDirectoryEntry(
			string ldapUrl )
		{
			string ldapUserName = configuration.LdapUserName;
			string ldapPassword = configuration.LdapPassword;

			if ( ldapUserName != null && ldapUserName.Length > 0 )
			{
				LogCentral.Current.LogDebug(
					string.Format(
					@"GetDirectoryEntry(): Connecting to LDAP with background ldapUserName '{0}' and authentication type '{1}'.",
					ldapUserName,
					AuthenticationType ) );
				TraceThreadInfo();

				// Support an alternative way.
				string directoryEntryLdapUserName =
					configuration.DirEntryUserNamePrefix +
					ldapUserName +
					configuration.DirEntryUserNameSuffix;

				LogCentral.Current.LogDebug(
					string.Format(
					@"GetUserInfo(): (B) about to create DirectorySearcher with background LDAP user name '{0}'.",
					directoryEntryLdapUserName ) );

				DirectoryEntry de = new DirectoryEntry(
					ldapUrl,
					directoryEntryLdapUserName,
					ldapPassword,
					AuthenticationType );

				return de;
			}
			else
			{
				LogCentral.Current.LogDebug(
					string.Format(
					@"GetDirectoryEntry(): Connecting with empty ldapUserName and authentication type '{0}.'",
					AuthenticationType ) );
				TraceThreadInfo();

				DirectoryEntry de = new DirectoryEntry( ldapUrl );
				de.AuthenticationType = AuthenticationType;
				return de;
			}
		}

		/// <summary>
		/// Get information about all users in the Active Directory.
		/// </summary>
		/// <returns>
		/// Returns an array of information of all users.
		/// </returns>
		public ADUserInfo[] GetUserInfos()
		{
			List<ADUserInfo> list = new List<ADUserInfo>();

			// --

			// Loop to get past the 1000 items limit.
			for ( int i = 0; i < SliceLoopLimit; ++i )
			{
				string searchFilter = GetSliceLoopFilter( i );

				// --

				DirectoryEntry entry = GetDirectoryEntry( LdapBaseString );

				DirectorySearcher searcher = new DirectorySearcher( entry );
				searcher.Filter = string.Format(
					@"(&(objectCategory=person){0})",
					searchFilter ); // person=contact+user.

				LogCentral.Current.LogDebug(
					string.Format(
					@"Searching with search filter '{0}' and size limit {1}.",
					searcher.Filter,
					adSizeReturn ) );

				searcher.PropertiesToLoad.Add( @"objectGUID" );
				searcher.SizeLimit = adSizeReturn;

				SearchResultCollection results = searcher.FindAll();

				LogCentral.Current.LogDebug(
					string.Format(
					@"Searching returned {0} items.",
					results.Count ) );

				// --

				foreach ( SearchResult result in results )
				{
					Guid guid = new Guid(
						(byte[])result.Properties[@"objectGUID"][0] );

					list.Add( GetUserInfoByGuid( guid ) );
				}
			}

			// --

			if ( list.Count <= 0 )
			{
				return null;
			}
			else
			{
				return list.ToArray();
			}
		}

		/// <summary>
		/// Get information about all users in the Active Directory.
		/// </summary>
		/// <returns>
		/// Returns an array of information of all users.
		/// </returns>
		public ADGroupInfo[] GetGroupInfos()
		{
			List<ADGroupInfo> list = new List<ADGroupInfo>();

			// --

			// Loop to get past the 1000 items limit.
			for ( int i = 0; i < SliceLoopLimit; ++i )
			{
				string searchFilter = GetSliceLoopFilter( i );

				// --

				DirectoryEntry entry = GetDirectoryEntry( LdapBaseString );

				DirectorySearcher searcher = new DirectorySearcher( entry );
				searcher.Filter = string.Format(
					@"(&(objectCategory=group){0})",
					searchFilter ); // person=contact+user.

				LogCentral.Current.LogDebug(
					string.Format(
					@"Searching with search filter '{0}' and size limit {1}.",
					searcher.Filter,
					adSizeReturn ) );

				searcher.PropertiesToLoad.Add( @"objectGUID" );
				searcher.SizeLimit = adSizeReturn;

				SearchResultCollection results = searcher.FindAll();

				LogCentral.Current.LogDebug(
					string.Format(
					@"Searching returned {0} items.",
					results.Count ) );

				// --

				foreach ( SearchResult result in results )
				{
					Guid guid = new Guid(
						(byte[])result.Properties[@"objectGUID"][0] );
					list.Add( GetGroupInfoByGuid( guid ) );
				}
			}

			// --

			if ( list.Count <= 0 )
			{
				return null;
			}
			else
			{
				return list.ToArray();
			}
		}

		/// <summary>
		/// Get a list of all groups of a certain user.
		/// </summary>
		/// <param name="adInfo">The user to query the groups from.</param>
		/// <returns>
		/// Returns an array of all groups the given user is
		/// member of.
		/// </returns>
		/// <remarks>
		/// From: 
		/// http://72.14.221.104/search?q=cache:sywXEEFvZiYJ:www.411asp.net/func/goto%3Fid%3D5863210+memberOf+ad&hl=de&gl=de&ct=clnk&cd=2&client=firefox-a
		/// The property "MemberOf" does not supply the "Primary Group".
		/// </remarks>
		public ADGroupInfo[] GetUserParentGroups(
			ADUserInfo adInfo )
		{
			byte[] guidBytes = adInfo.Guid.ToByteArray();
			string guidString = HexEscape( guidBytes );

			DirectoryEntry entry = GetDirectoryEntry( LdapBaseString );

			DirectorySearcher searcher = new DirectorySearcher( entry );
			searcher.Filter =
				string.Format(
				@"(&(objectCategory=person)(objectGUID={0}))",
				guidString );

			searcher.PropertiesToLoad.Add( @"memberOf" );
			searcher.SizeLimit = adSizeReturn;

			SearchResult result = searcher.FindOne();
			if ( result == null )
			{
				LogCentral.Current.LogDebug(
					string.Format(
					@"GetUserGroups(): (B) searcher.FindOne() returned NULL, exiting with return value NULL."
					) );

				return null;
			}
			else if ( result.Properties[@"memberOf"] == null )
			{
				return null;
			}
			else
			{
				List<ADGroupInfo> list = new List<ADGroupInfo>();

				foreach ( object val in result.Properties[@"memberOf"] )
				{
					ADGroupInfo info = GetGroupInfoByDN(
						Convert.ToString( val ) );
					Debug.Assert( info != null );
					if ( info != null )
					{
						list.Add( info );
					}
				}

				return list.ToArray();
			}
		}

		/// <summary>
		/// Get a list of all groups of a certain user.
		/// </summary>
		/// <param name="adInfo">The user to query the groups from.</param>
		/// <returns>
		/// Returns an array of all groups the given user is
		/// member of.
		/// </returns>
		/// <remarks>
		/// From:
		/// http://72.14.221.104/search?q=cache:sywXEEFvZiYJ:www.411asp.net/func/goto%3Fid%3D5863210+memberOf+ad&hl=de&gl=de&ct=clnk&cd=2&client=firefox-a
		/// The property "MemberOf" does not supply the "Primary Group".
		/// See:
		/// http://dunnry.com/blog/EnumeratingTokenGroupsTokenGroupsInNET.aspx
		/// </remarks>
		public ADGroupInfo[] GetUserParentGroupsEx(
			ADUserInfo adInfo )
		{
			byte[] guidBytes = adInfo.Guid.ToByteArray();
			string guidString = HexEscape( guidBytes );

			DirectoryEntry entry = GetDirectoryEntry( LdapBaseString );

			DirectorySearcher searcher = new DirectorySearcher( entry );
			searcher.Filter =
				string.Format(
				@"(&(objectCategory=person)(objectGUID={0}))",
				guidString );

			searcher.PropertiesToLoad.Add( @"memberOf" );
			searcher.SizeLimit = adSizeReturn;

			SearchResult result = searcher.FindOne();
			if ( result == null )
			{
				LogCentral.Current.LogDebug(
					string.Format(
					@"GetUserGroups(): (B) searcher.FindOne() returned NULL, exiting with return value NULL."
					) );

				return null;
			}
			else
			{
				StringBuilder sb = new StringBuilder();

				//we are building an '|' clause
				sb.Append( @"(|" );

				using ( DirectoryEntry user = result.GetDirectoryEntry() )
				{
					//we must ask for this one first
					user.RefreshCache( new string[] { @"tokenGroups" } );

					PropertyValueCollection tokenGroups =
						user.Properties[@"tokenGroups"];

					if ( tokenGroups == null || tokenGroups.Count <= 0 )
					{
						LogCentral.Current.LogDebug(
							string.Format(
							@"GetUserGroups(): (C) No token groups for user '{0}' found, exiting with return value NULL.",
							adInfo.SamName
							) );

						return null;
					}

					foreach ( byte[] sid in tokenGroups )
					{
						//append each member into the filter
						sb.AppendFormat( @"(objectSid={0})", HexEscape( sid ) );
					}
				}

				//end our initial filter
				sb.Append( @")" );

				// --

				List<ADGroupInfo> list = new List<ADGroupInfo>();

				//now create and pull in one search
				using ( DirectorySearcher ds =
					new DirectorySearcher(
					GetDirectoryEntry( LdapBaseString ),
					sb.ToString(),
					new string[] { @"distinguishedName" } ) )
				{
					using ( SearchResultCollection srcs = ds.FindAll() )
					{
						foreach ( SearchResult src in srcs )
						{
							object val =
								src.Properties[@"distinguishedName"][0];

							ADGroupInfo info = GetGroupInfoByDN(
								Convert.ToString( val ) );

							Debug.Assert( info != null );
							if ( info != null )
							{
								list.Add( info );
							}
						}
					}
				}

				return list.ToArray();
			}
		}

		/// <summary>
		/// Gets the root groups.
		/// </summary>
		/// <returns></returns>
		public ADGroupInfo[] GetRootGroups()
		{
			List<ADGroupInfo> list = new List<ADGroupInfo>();

			// --

			// Loop to get past the 1000 items limit.
			for ( int i = 0; i < SliceLoopLimit; ++i )
			{
				string searchFilter = GetSliceLoopFilter( i );

				// --

				DirectoryEntry entry = GetDirectoryEntry( LdapBaseString );

				DirectorySearcher searcher = new DirectorySearcher( entry );
				searcher.Filter = string.Format(
					@"(&(objectCategory=group){0}(!(memberOf=*)))",
					searchFilter );

				LogCentral.Current.LogDebug(
					string.Format(
					@"Searching with search filter '{0}' and size limit {1}.",
					searcher.Filter,
					adSizeReturn ) );

				searcher.PropertiesToLoad.Add( @"objectGUID" );
				searcher.SizeLimit = adSizeReturn;

				SearchResultCollection results = searcher.FindAll();

				LogCentral.Current.LogDebug(
					string.Format(
					@"Searching returned {0} items.",
					results.Count ) );

				// --

				foreach ( SearchResult result in results )
				{
					Guid guid = new Guid(
						(byte[])result.Properties[@"objectGUID"][0] );
					list.Add( GetGroupInfoByGuid( guid ) );
				}
			}

			// --

			if ( list.Count <= 0 )
			{
				return null;
			}
			else
			{
				return list.ToArray();
			}
		}

		/// <summary>
		/// Gets the group child groups.
		/// </summary>
		/// <param name="parentGroup">The parent group.</param>
		/// <returns></returns>
		public ADGroupInfo[] GetGroupChildGroups(
			ADGroupInfo parentGroup )
		{
			List<ADGroupInfo> list = new List<ADGroupInfo>();

			// --

			// Loop to get past the 1000 items limit.
			for ( int i = 0; i < SliceLoopLimit; ++i )
			{
				string searchFilter = GetSliceLoopFilter( i );

				// --

				DirectoryEntry entry = GetDirectoryEntry( LdapBaseString );

				DirectorySearcher searcher = new DirectorySearcher( entry );
				searcher.Filter = string.Format(
					@"(&(objectCategory=group){0}(memberOf={1}))",
					searchFilter,
					parentGroup.DN );

				LogCentral.Current.LogDebug(
					string.Format(
					@"Searching with search filter '{0}' and size limit {1}.",
					searcher.Filter,
					adSizeReturn ) );

				searcher.PropertiesToLoad.Add( @"objectGUID" );
				searcher.SizeLimit = adSizeReturn;

				SearchResultCollection results = searcher.FindAll();

				LogCentral.Current.LogDebug(
					string.Format(
					@"Searching returned {0} items.",
					results.Count ) );

				// --

				foreach ( SearchResult result in results )
				{
					Guid guid = new Guid(
						(byte[])result.Properties[@"objectGUID"][0] );
					list.Add( GetGroupInfoByGuid( guid ) );
				}
			}

			// --

			if ( list.Count <= 0 )
			{
				return null;
			}
			else
			{
				return list.ToArray();
			}
		}

		/// <summary>
		/// Gets the group child users.
		/// </summary>
		/// <param name="parentGroup">The parent group.</param>
		/// <returns></returns>
		public ADUserInfo[] GetGroupChildUsers(
			ADGroupInfo parentGroup )
		{
			List<ADUserInfo> list = new List<ADUserInfo>();

			// --

			// Loop to get past the 1000 items limit.
			for ( int i = 0; i < SliceLoopLimit; ++i )
			{
				string searchFilter = GetSliceLoopFilter( i );

				// --

				DirectoryEntry entry = GetDirectoryEntry( LdapBaseString );

				DirectorySearcher searcher = new DirectorySearcher( entry );
				searcher.Filter = string.Format(
					@"(&(objectCategory=person){0}(memberOf={1}))",
					searchFilter,
					parentGroup.DN );

				LogCentral.Current.LogDebug(
					string.Format(
					@"Searching with search filter '{0}' and size limit {1}.",
					searcher.Filter,
					adSizeReturn ) );

				searcher.PropertiesToLoad.Add( @"objectGUID" );
				searcher.SizeLimit = adSizeReturn;

				SearchResultCollection results = searcher.FindAll();

				LogCentral.Current.LogDebug(
					string.Format(
					@"Searching returned {0} items.",
					results.Count ) );

				// --

				foreach ( SearchResult result in results )
				{
					Guid guid = new Guid(
						(byte[])result.Properties[@"objectGUID"][0] );
					list.Add( GetUserInfoByGuid( guid ) );
				}
			}

			// --

			if ( list.Count <= 0 )
			{
				return null;
			}
			else
			{
				return list.ToArray();
			}
		}

		/// <summary>
		/// Get a list of all groups of a certain group.
		/// </summary>
		/// <param name="adInfo">The group to query the groups from.</param>
		/// <returns>
		/// Returns an array of all groups the given group
		/// is member of.
		/// </returns>
		public ADGroupInfo[] GetGroupParentGroups(
			ADGroupInfo adInfo )
		{
			byte[] guidBytes = adInfo.Guid.ToByteArray();
			string guidString = HexEscape( guidBytes );

			DirectoryEntry entry = GetDirectoryEntry( LdapBaseString );

			DirectorySearcher searcher = new DirectorySearcher( entry );
			searcher.Filter =
				string.Format(
				@"(&(objectCategory=group)(objectGUID={0}))",
				guidString );

			searcher.PropertiesToLoad.Add( @"memberOf" );
			searcher.SizeLimit = adSizeReturn;

			SearchResult result = searcher.FindOne();
			if ( result == null )
			{
				LogCentral.Current.LogDebug(
					string.Format(
					@"GetGroupGroups(): (B) searcher.FindOne() returned NULL, exiting with return value NULL."
					) );

				return null;
			}
			else if ( result.Properties[@"memberOf"] == null )
			{
				return null;
			}
			else
			{
				List<ADGroupInfo> list = new List<ADGroupInfo>();

				foreach ( object val in result.Properties[@"memberOf"] )
				{
					ADGroupInfo info = GetGroupInfoByDN(
						Convert.ToString( val ) );
					if ( info != null )
					{
						list.Add( info );
					}
				}

				return list.ToArray();
			}
		}

		/// <summary>
		/// Gets information about a group by its DN (distinguished name).
		/// </summary>
		/// <param name="groupDN">The DN of the group. E.g.
		/// <c>CN=zeta Entwickler,OU=Gruppen,OU=zeta,DC=duernau,DC=zeta-software,DC=de</c></param>
		/// <returns>
		/// Returns the group information or <c>null</c>
		/// if not found.
		/// </returns>
		public ADGroupInfo GetGroupInfoByDN(
			string groupDN )
		{
			if ( configuration.AllowForCaching )
			{
				ADGroupInfo p =
					cacheFor_Groups.FindByCN( groupDN );
				if ( p != null )
				{
					LogCentral.Current.LogInfo(
						string.Format(
						@"Cache HIT for group by DN '{0}'.",
						groupDN ) );

					return p;
				}
			}

			// --

			string ldapString = string.Format(
				@"LDAP://{0}/{1}",
				configuration.LdapServer,
				groupDN );

			LogCentral.Current.LogInfo(
				string.Format(
				@"About to get group info for LDAP string '{0}'.",
				ldapString ) );

			DirectoryEntry entry = GetDirectoryEntry( ldapString );

			DirectorySearcher searcher = new DirectorySearcher( entry );
			searcher.SizeLimit = adSizeReturn;
			searcher.Filter = @"(objectCategory=group)";

			// --

			ADGroupInfo info = GetGroupInfo( searcher );

			// --

			if ( configuration.AllowForCaching )
			{
				cacheFor_Groups.Add( info );
			}

			return info;
		}

		/// <summary>
		/// Enable/disable a user in the Active Directory, based on the GUID
		/// of the user.
		/// </summary>
		/// <param name="userGuid">The GUID of the user to query.</param>
		/// <param name="enable">Whether to enable (=true) or
		/// disable (=false).</param>
		public void EnableUser(
			Guid userGuid,
			bool enable )
		{
			byte[] guidBytes = userGuid.ToByteArray();
			string guidString = HexEscape( guidBytes );

			DirectoryEntry entry = GetDirectoryEntry( LdapBaseString );

			DirectorySearcher searcher = new DirectorySearcher( entry );
			searcher.Filter =
				string.Format(
				@"(&(objectCategory=person)(objectGUID={0}))",
				guidString );
			searcher.SizeLimit = adSizeReturn;

			searcher.PropertiesToLoad.Add( @"userAccountControl" );

			// --

			SearchResult result = searcher.FindOne();

			if ( result == null )
			{
				throw new ApplicationException(
					LocalizationHelper.Format(
					Resources.Str_ZetaLib_Core_Common_ActiveDirectory_02,
					LocalizationHelper.CreatePair( @"Guid", userGuid ) ) );
			}
			else
			{
				// --
				// See http://groups.google.de/groups?q=disable+user+ldap&selm=uxwCpccnBHA.2084%40tkmsftngp04&rnum=3

				ADUserFlags flags =
					(ADUserFlags)Convert.ToUInt32( GetRP(
					result.Properties[@"userAccountControl"] ) );

				// Switch on or off.
				if ( enable )
				{
					flags &= ~(ADUserFlags.Accountdisable);
				}
				else
				{
					flags |= ADUserFlags.Accountdisable;
				}

				DirectoryEntry e = result.GetDirectoryEntry();
				e.Properties[@"userAccountControl"][0] = flags;

				// Actually store back.
				e.CommitChanges();
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private methods and properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets the group info.
		/// </summary>
		/// <param name="searcher">The searcher.</param>
		/// <returns></returns>
		private ADGroupInfo GetGroupInfo(
			DirectorySearcher searcher )
		{
			searcher.SizeLimit = adSizeReturn;

			searcher.PropertiesToLoad.Add( @"objectGUID" );
			searcher.PropertiesToLoad.Add( @"sAMAccountName" );
			searcher.PropertiesToLoad.Add( @"cn" );
			searcher.PropertiesToLoad.Add( @"name" );

			searcher.PropertiesToLoad.Add( @"mail" );
			searcher.PropertiesToLoad.Add( @"groupScope" );
			searcher.PropertiesToLoad.Add( @"groupType" );
			searcher.PropertiesToLoad.Add( @"description" );

			// --

			SearchResult result = searcher.FindOne();
			if ( result == null )
			{
				return null;
			}
			else
			{
				ADGroupInfo info = new ADGroupInfo( this );

				// Add relative DN.
				string dn = result.Path;
				string basePath =
					string.Format( @"LDAP://{0}",
					configuration.LdapServer );
				if ( dn.StartsWith( basePath, StringComparison.InvariantCultureIgnoreCase ) )
				{
					dn = dn.Substring( basePath.Length ).Trim( '/' );
				}
				info.DN = dn;

				byte[] guidBytes = (byte[])result.Properties[@"objectGUID"][0];

				info.Guid = new Guid( guidBytes );
				info.SamName = ReadFieldString( GetRP( result.Properties[@"sAMAccountName"] ) );
				info.CN = ReadFieldString( GetRP( result.Properties[@"cn"] ) );
				info.Name = ReadFieldString( GetRP( result.Properties[@"name"] ) );

				info.EMail = ReadFieldString( GetRP( result.Properties[@"mail"] ) );
				info.Scope = ReadFieldString( GetRP( result.Properties[@"groupScope"] ) );
				info.Type = ReadFieldInteger( GetRP( result.Properties[@"groupType"] ) );
				info.Description = ReadFieldString( GetRP( result.Properties[@"description"] ) );

				return info;
			}
		}

		/// <summary>
		/// Gets the type of the authentication.
		/// </summary>
		/// <value>The type of the authentication.</value>
		private AuthenticationTypes AuthenticationType
		{
			get
			{
				return AuthenticationTypes.Secure;
			}
		}

		/// <summary>
		/// Gets the user info by searcher.
		/// </summary>
		/// <param name="searcher">The searcher.</param>
		/// <returns></returns>
		private ADUserInfo GetUserInfoBySearcher(
			DirectorySearcher searcher )
		{
			searcher.SizeLimit = adSizeReturn;

			searcher.PropertiesToLoad.Add( @"userAccountControl" );

			searcher.PropertiesToLoad.Add( @"objectGUID" );
			searcher.PropertiesToLoad.Add( @"objectSid" );
			searcher.PropertiesToLoad.Add( @"sAMAccountName" );
			searcher.PropertiesToLoad.Add( @"cn" );
			searcher.PropertiesToLoad.Add( @"name" );

			searcher.PropertiesToLoad.Add( @"givenName" );
			searcher.PropertiesToLoad.Add( @"sn" );
			searcher.PropertiesToLoad.Add( @"initials" );
			searcher.PropertiesToLoad.Add( @"description" );

			searcher.PropertiesToLoad.Add( @"streetAddress" );
			searcher.PropertiesToLoad.Add( @"postOfficeBox" );
			searcher.PropertiesToLoad.Add( @"postalCode" );
			searcher.PropertiesToLoad.Add( @"c" );
			searcher.PropertiesToLoad.Add( @"st" );
			searcher.PropertiesToLoad.Add( @"l" );

			searcher.PropertiesToLoad.Add( @"telephoneNumber" );
			searcher.PropertiesToLoad.Add( @"mobile" );
			searcher.PropertiesToLoad.Add( @"facsimileTelephoneNumber" );
			searcher.PropertiesToLoad.Add( @"mail" );
			searcher.PropertiesToLoad.Add( @"wwwHomePage" );

			searcher.PropertiesToLoad.Add( @"title" );
			searcher.PropertiesToLoad.Add( @"department" );
			searcher.PropertiesToLoad.Add( @"company" );
			searcher.PropertiesToLoad.Add( @"manager" );
			searcher.PropertiesToLoad.Add( @"primaryGroupID" );

			// --

			SearchResult result = searcher.FindOne();
			if ( result == null )
			{
				LogCentral.Current.LogDebug(
					string.Format(
					@"GetUserInfo(): (A) searcher.FindOne() returned NULL, exiting with return value NULL (The LDAP query was '{0}').",
					searcher.Filter
					) );
				return null;
			}
			else
			{
				ADUserInfo info = new ADUserInfo( this );

				// Add relative DN.
				string dn = result.Path;
				string basePath =
					string.Format( @"LDAP://{0}",
					configuration.LdapServer );
				if ( dn.StartsWith( basePath, StringComparison.InvariantCultureIgnoreCase ) )
				{
					dn = dn.Substring( basePath.Length ).Trim( '/' );
				}
				info.DN = dn;

				// --

				byte[] guidBytes = GetPropertyValueByteArray( result.Properties[@"objectGUID"] );
				byte[] sidBytes = GetPropertyValueByteArray( result.Properties[@"objectSid"] );

				info.Guid = new Guid( guidBytes );
				info.Sid = ConvertByteToStringSid( sidBytes );
				info.AccountControlFlags = (ADUserFlags)Convert.ToUInt32( GetRP( result.Properties[@"userAccountControl"] ) );
				info.SamName = ReadFieldString( GetRP( result.Properties[@"sAMAccountName"] ) );
				info.CN = ReadFieldString( GetRP( result.Properties[@"cn"] ) );
				info.Name = ReadFieldString( GetRP( result.Properties[@"name"] ) );

				info.FirstName = ReadFieldString( GetRP( result.Properties[@"givenName"] ) );
				info.LastName = ReadFieldString( GetRP( result.Properties[@"sn"] ) );
				info.Initials = ReadFieldString( GetRP( result.Properties[@"initials"] ) );
				info.Description = ReadFieldString( GetRP( result.Properties[@"description"] ) );

				info.StreetAddress = ReadFieldString( GetRP( result.Properties[@"streetAddress"] ) );
				info.PostOfficeBox = ReadFieldString( GetRP( result.Properties[@"postOfficeBox"] ) );
				info.Zip = ReadFieldString( GetRP( result.Properties[@"postalCode"] ) );
				info.City = ReadFieldString( GetRP( result.Properties[@"l"] ) );
				info.State = ReadFieldString( GetRP( result.Properties[@"st"] ) );
				info.CountryCode = ReadFieldString( GetRP( result.Properties[@"c"] ) );

				info.TelephoneNumber = ReadFieldString( GetRP( result.Properties[@"telephoneNumber"] ) );
				info.MobileNumber = ReadFieldString( GetRP( result.Properties[@"mobile"] ) );
				info.FaxNumber = ReadFieldString( GetRP( result.Properties[@"facsimileTelephoneNumber"] ) );
				info.EMail = ReadFieldString( GetRP( result.Properties[@"mail"] ) );
				info.WwwHomePage = ReadFieldString( GetRP( result.Properties[@"wwwHomePage"] ) );

				info.Title = ReadFieldString( GetRP( result.Properties[@"title"] ) );
				info.Department = ReadFieldString( GetRP( result.Properties[@"department"] ) );
				info.Company = ReadFieldString( GetRP( result.Properties[@"company"] ) );
				info.Manager = ReadFieldString( GetRP( result.Properties[@"manager"] ) );
				info.PrimaryGroupID = ReadFieldLong( GetRP( result.Properties[@"primaryGroupID"] ) );

				// --

				return info;
			}
		}

		/// <summary>
		/// Internal helper.
		/// </summary>
		/// <param name="c">The c.</param>
		/// <returns></returns>
		private static byte[] GetPropertyValueByteArray(
			ResultPropertyValueCollection c )
		{
			if ( c == null || c.Count <= 0 || c[0] == null )
			{
				return null;
			}
			else
			{
				return (byte[])c[0];
			}
		}

		/// <summary>
		/// See http://www.codeproject.com/csharp/getusersid.asp.
		/// </summary>
		/// <param name="sidBytes">The sid bytes.</param>
		/// <returns></returns>
		private static string ConvertByteToStringSid(
			byte[] sidBytes )
		{
			if ( sidBytes == null )
			{
				return null;
			}
			else
			{
				StringBuilder strSid = new StringBuilder();
				strSid.Append( @"S-" );

				// Add SID revision.
				strSid.Append( sidBytes[0].ToString() );
				// Next six bytes are SID authority value.
				if ( sidBytes[6] != 0 || sidBytes[5] != 0 )
				{
					strSid.Append( @"-" );
					string strAuth = String.Format
						( @"0x{0:2x}{1:2x}{2:2x}{3:2x}{4:2x}{5:2x}",
						(Int16)sidBytes[1],
						(Int16)sidBytes[2],
						(Int16)sidBytes[3],
						(Int16)sidBytes[4],
						(Int16)sidBytes[5],
						(Int16)sidBytes[6] );
					strSid.Append( strAuth );
				}
				else
				{
					Int64 iVal = (Int32)(sidBytes[1]) +
						(Int32)(sidBytes[2] << 8) +
						(Int32)(sidBytes[3] << 16) +
						(Int32)(sidBytes[4] << 24);
					strSid.Append( @"-" );
					strSid.Append( iVal.ToString() );
				}

				// Get sub authority count...
				int iSubCount = Convert.ToInt32( sidBytes[7] );
				int idxAuth = 0;
				for ( int i = 0; i < iSubCount; i++ )
				{
					idxAuth = 8 + i * 4;
					UInt32 iSubAuth =
						BitConverter.ToUInt32( sidBytes, idxAuth );
					strSid.Append( @"-" );
					strSid.Append( iSubAuth.ToString() );
				}

				return strSid.ToString();
			}
		}

		/// <summary>
		/// Write back to AD, but only certain information, since not all
		/// is modifiable or intented to be so.
		/// </summary>
		/// <param name="e">The e.</param>
		/// <param name="curInfo">The cur info.</param>
		/// <param name="newInfo">The new info.</param>
		private void PutUserInfo(
			DirectoryEntry e,
			ADUserInfo curInfo,
			ADUserInfo newInfo )
		{
			// Reset for accumulating.
			debugSettedWPValues = string.Empty;

			if ( curInfo.CN != newInfo.CN )
			{
				SetWP( e.Properties, @"cn", newInfo.CN );
			}
			if ( curInfo.CN != newInfo.Name )
			{
				SetWP( e.Properties, @"name", newInfo.Name );
			}

			if ( curInfo.FirstName != newInfo.FirstName )
			{
				SetWP( e.Properties, @"givenName", newInfo.FirstName );
			}
			if ( curInfo.LastName != newInfo.LastName )
			{
				SetWP( e.Properties, @"sn", newInfo.LastName );
			}
			if ( curInfo.Initials != newInfo.Initials )
			{
				SetWP( e.Properties, @"initials", newInfo.Initials );
			}
			if ( curInfo.Description != newInfo.Description )
			{
				SetWP( e.Properties, @"description", newInfo.Description );
			}

			if ( curInfo.StreetAddress != newInfo.StreetAddress )
			{
				SetWP( e.Properties, @"streetAddress", newInfo.StreetAddress );
			}
			if ( curInfo.PostOfficeBox != newInfo.PostOfficeBox )
			{
				SetWP( e.Properties, @"postOfficeBox", newInfo.PostOfficeBox );
			}
			if ( curInfo.Zip != newInfo.Zip )
			{
				SetWP( e.Properties, @"postalCode", newInfo.Zip );
			}
			if ( curInfo.City != newInfo.City )
			{
				SetWP( e.Properties, @"l", newInfo.City );
			}
			if ( curInfo.State != newInfo.State )
			{
				SetWP( e.Properties, @"st", newInfo.State );
			}
			if ( curInfo.CountryCode != newInfo.CountryCode )
			{
				SetWP( e.Properties, @"c", newInfo.CountryCode );
			}

			if ( curInfo.TelephoneNumber != newInfo.TelephoneNumber )
			{
				SetWP( e.Properties, @"telephoneNumber", newInfo.TelephoneNumber );
			}
			if ( curInfo.MobileNumber != newInfo.MobileNumber )
			{
				SetWP( e.Properties, @"mobile", newInfo.MobileNumber );
			}
			if ( curInfo.FaxNumber != newInfo.FaxNumber )
			{
				SetWP( e.Properties, @"facsimileTelephoneNumber", newInfo.FaxNumber );
			}
			if ( curInfo.EMail != newInfo.EMail )
			{
				SetWP( e.Properties, @"mail", newInfo.EMail );
			}
			if ( curInfo.WwwHomePage != newInfo.WwwHomePage )
			{
				SetWP( e.Properties, @"wwwHomePage", newInfo.WwwHomePage );
			}

			if ( curInfo.Title != newInfo.Title )
			{
				SetWP( e.Properties, @"title", newInfo.Title );
			}
			if ( curInfo.Department != newInfo.Department )
			{
				SetWP( e.Properties, @"department", newInfo.Department );
			}
			if ( curInfo.Company != newInfo.Company )
			{
				SetWP( e.Properties, @"company", newInfo.Company );
			}
			if ( curInfo.PrimaryGroupID != newInfo.PrimaryGroupID )
			{
				SetWP( e.Properties, @"primaryGroupID", newInfo.PrimaryGroupID );
			}
			if ( curInfo.Manager != newInfo.Manager )
			{
				SetWP( e.Properties, @"manager", newInfo.Manager );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// For debugging purposes, only. Accumulating the setted values.
		/// </summary>
		private string debugSettedWPValues = string.Empty;

		/// <summary>
		/// The maximum number of returned items.
		/// </summary>
		private const int adSizeReturn = 1000;

		/// <summary>
		/// The configuration for this instance.
		/// </summary>
		private ActiveDirectoryConfiguration configuration;

		// ------------------------------------------------------------------
		#endregion

		#region Various helper functions.
		// ------------------------------------------------------------------

		/// <summary>
		/// Calculated LDAP base string.
		/// </summary>
		/// <value>The LDAP base string.</value>
		private string LdapBaseString
		{
			get
			{
				return string.Format(
					@"LDAP://{0}/{1}",
					configuration.LdapServer,
					configuration.LdapBaseDN );
			}
		}

		/// <summary>
		/// Gets the slice loop limit.
		/// </summary>
		/// <value>The slice loop limit.</value>
		private int SliceLoopLimit
		{
			get
			{
				if ( configuration.UseSlicedQueries )
				{
					return 28;
				}
				else
				{
					return 1;
				}
			}
		}

		/// <summary>
		/// Gets the slice loop filter.
		/// </summary>
		/// <param name="i">The i.</param>
		/// <returns></returns>
		private string GetSliceLoopFilter(
			int i )
		{
			if ( configuration.UseSlicedQueries )
			{
				char c1 = (char)(((int)'a') + i);

				if ( i < 26 )
				{
					return
						string.Format(
						@"(sAMAccountName={0}*)",
						c1 );
				}
				else if ( i == 26 )
				{
					return
						string.Format(
						@"(sAMAccountName<=a*)" );
				}
				else
				{
					return
						string.Format(
						@"(sAMAccountName>=z*)" );
				}
			}
			else
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Just dump some debugging information.
		/// </summary>
		private static void TraceThreadInfo()
		{
			IIdentity id = Thread.CurrentPrincipal.Identity;

			string s = string.Format(
				@"Information: The current thread (Thread.CurrentPrincipal.Identity) is running as user '{0}', with authentication type '{1}' and IsAuthenticated = '{2}'.",
				id.Name,
				id.AuthenticationType,
				id.IsAuthenticated
				);

			LogCentral.Current.LogDebug( s );
		}

		/// <summary>
		/// Gets the RP.
		/// </summary>
		/// <param name="rp">The rp.</param>
		/// <returns></returns>
		private object GetRP(
			ResultPropertyValueCollection rp )
		{
			if ( rp == null )
			{
				LogCentral.Current.LogDebug(
					string.Format(
					@"ActiveDirectory.GetRP(): rp is null, returning NULL."
					) );

				return null;
			}
			else if ( rp.Count <= 0 )
			{
				LogCentral.Current.LogDebug(
					string.Format(
					@"ActiveDirectory.GetRP(): rp is empty, returning NULL."
					) );

				return null;
			}
			{
				object o = rp[0];

				LogCentral.Current.LogDebug(
					string.Format(
					@"ActiveDirectory.GetRP(): rp is non-null, returning rp[0] with a value of '{0}'.",
					o
					) );

				return o;
			}
		}

		/// <summary>
		/// Sets the WP.
		/// </summary>
		/// <param name="pc">The pc.</param>
		/// <param name="key">The key.</param>
		/// <param name="val">The val.</param>
		private void SetWP(
			PropertyCollection pc,
			string key,
			object val )
		{
			try
			{
				if ( pc[key] == null )
				{
					LogCentral.Current.LogWarn(
						string.Format(
						@"ActiveDirectory.SetWP(): 'pc[key]', with key='{0}' is null. Not setting.",
						key
						) );
				}
				else if ( pc[key].Count <= 0 )
				{
					LogCentral.Current.LogDebug(
						string.Format(
						@"ActiveDirectory.SetWP(): 'pc[key].Count', with key='{0}' is zero. Adding new value '{1}'.",
						key,
						val
						) );

					if ( debugSettedWPValues.Length > 0 )
					{
						debugSettedWPValues += @", ";
					}
					debugSettedWPValues += string.Format(
						@"'{0}'='{1}'",
						key,
						val );

					pc[key].Add( val );
				}
				else
				{
					LogCentral.Current.LogDebug(
						string.Format(
						@"ActiveDirectory.SetWP(): Setting 'pc[key][0]', with key='{0}' and value='{1}'.",
						key,
						val
						) );

					if ( debugSettedWPValues.Length > 0 )
					{
						debugSettedWPValues += @", ";
					}
					debugSettedWPValues += string.Format(
						@"'{0}'='{1}'",
						key,
						val );

					pc[key][0] = val;
				}
			}
			catch ( Exception x )
			{
				LogCentral.Current.LogError(
					string.Format(
					@"ActiveDirectory.SetWP(): Exception occured for setting key='{0}', value='{1}'.",
					key,
					val
					),
					x );

				throw;
			}
		}

		/// <summary>
		/// Reads the field integer.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		private static int ReadFieldInteger(
			object fieldValue )
		{
			if ( fieldValue == null ||
				fieldValue == DBNull.Value ||
				!IsNumeric( fieldValue.ToString(), NumberStyles.Number ) )
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32( fieldValue );
			}
		}

		/// <summary>
		/// Reads the field long.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		private static int ReadFieldLong(
			object fieldValue )
		{
			if ( fieldValue == null ||
				fieldValue == DBNull.Value ||
				!IsNumeric( fieldValue.ToString(), NumberStyles.Number ) )
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32( fieldValue );
			}
		}

		/// <summary>
		/// Reads the field string.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <returns></returns>
		private static string ReadFieldString(
			object fieldValue )
		{
			return
				fieldValue == null ||
				fieldValue == DBNull.Value ?
				string.Empty :
				Convert.ToString( fieldValue );
		}

		/// <summary>
		/// Determines whether the specified STR is numeric.
		/// </summary>
		/// <param name="str">The STR.</param>
		/// <param name="styles">The styles.</param>
		/// <returns>
		/// 	<c>true</c> if the specified STR is numeric; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsNumeric(
			string str,
			NumberStyles styles )
		{
			if ( str == null )
			{
				return false;
			}
			else if ( string.IsNullOrEmpty( str ) )
			{
				return false;
			}
			else
			{
				double result;
				return double.TryParse(
					str,
					styles,
					Thread.CurrentThread.CurrentCulture.NumberFormat,
					out result );
			}
		}

		/// <summary>
		/// From:
		/// http://groups.google.de/groups?hl=de&lr=&ie=UTF-8&oe=UTF-8&threadm=uChbVByJAHA.244%40cppssbbsa04&rnum=3&prev=/groups%3Fq%3DLDAP%2Bfilter%2BobjectGUID%26hl%3Dde%26lr%3D%26ie%3DUTF-8%26oe%3DUTF-8%26selm%3DuChbVByJAHA.244%2540cppssbbsa04%26rnum%3D3
		/// </summary>
		/// <param name="bytes">The bytes.</param>
		/// <returns></returns>
		private static string HexEscape(
			byte[] bytes )
		{
			StringBuilder sb = new StringBuilder();

			for ( int i = 0; i < bytes.Length; i++ )
			{
				sb.AppendFormat( @"\{0}", bytes[i].ToString( @"X2" ) );
			}
			return sb.ToString();
		}

		// ------------------------------------------------------------------
		#endregion

		#region Caching - Special cache classes.
		// ------------------------------------------------------------------

		/// <summary>
		/// Special cache class.
		/// </summary>
		private class ActiveDirectoryUserCache
		{
			#region Public methods.

			/// <summary>
			/// Add to the cache.
			/// </summary>
			/// <param name="item">The item.</param>
			public void Add(
				ADUserInfo item )
			{
				cachedItems.Add( item );
			}

			/// <summary>
			/// Lookup, returns NULL if not found.
			/// </summary>
			/// <param name="samName">Name of the sam.</param>
			/// <returns></returns>
			public ADUserInfo FindBySamName(
				string samName )
			{
				return cachedItems.Find(
					delegate( ADUserInfo item )
					{
						return item != null && item.SamName == samName;
					} );
			}

			/// <summary>
			/// Lookup, returns NULL if not found.
			/// </summary>
			/// <param name="guid">The GUID.</param>
			/// <returns></returns>
			public ADUserInfo FindByGuid(
				Guid guid )
			{
				return cachedItems.Find(
					delegate( ADUserInfo item )
					{
						return item != null && item.Guid == guid;
					} );
			}

			#endregion

			#region Private variables.

			/// <summary>
			/// 
			/// </summary>
			private List<ADUserInfo> cachedItems = new List<ADUserInfo>();

			#endregion
		}

		/// <summary>
		/// Special cache class.
		/// </summary>
		private class ActiveDirectoryGroupCache
		{
			#region Public methods.

			/// <summary>
			/// Add to the cache.
			/// </summary>
			/// <param name="item">The item.</param>
			public void Add(
				ADGroupInfo item )
			{
				cachedItems.Add( item );
			}

			/// <summary>
			/// Lookup, returns NULL if not found.
			/// </summary>
			/// <param name="samName">Name of the sam.</param>
			/// <returns></returns>
			public ADGroupInfo FindBySamName(
				string samName )
			{
				return cachedItems.Find(
					delegate( ADGroupInfo item )
					{
						return item != null && item.SamName == samName;
					} );
			}

			/// <summary>
			/// Lookup, returns NULL if not found.
			/// </summary>
			/// <param name="guid">The GUID.</param>
			/// <returns></returns>
			public ADGroupInfo FindByGuid(
				Guid guid )
			{
				return cachedItems.Find(
					delegate( ADGroupInfo item )
					{
						return item != null && item.Guid == guid;
					} );
			}

			/// <summary>
			/// Lookup, returns NULL if not found.
			/// </summary>
			/// <param name="cn">The cn.</param>
			/// <returns></returns>
			public ADGroupInfo FindByCN(
				string cn )
			{
				return cachedItems.Find(
					delegate( ADGroupInfo item )
					{
						return item != null && item.CN == cn;
					} );
			}

			#endregion

			#region Private variables.

			/// <summary>
			/// 
			/// </summary>
			private List<ADGroupInfo> cachedItems = new List<ADGroupInfo>();

			#endregion
		}

		private ActiveDirectoryUserCache cacheFor_Users;
		private ActiveDirectoryGroupCache cacheFor_Groups;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}