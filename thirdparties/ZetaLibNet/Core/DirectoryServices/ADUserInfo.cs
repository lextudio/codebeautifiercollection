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
	using ZetaLib.Core.Collections;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Summarizes user information from Active Directory.
	/// <see href="ms-help://MS.MSDNQTR.2002JUL.1033/netdir/ad/user_object_user_interface_mapping.htm" />
	/// </summary>
	[DebuggerDisplay( @"Name = {name}, SAM name = {samName}" )]
	public class ADUserInfo
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="owner">The owner.</param>
		public ADUserInfo(
			ActiveDirectory owner )
		{
			this.owner = owner;
		}

		/// <summary>
		/// Checks whether a user is member of a certain group.
		/// </summary>
		/// <param name="groupName">The name of the group to check for.</param>
		/// <returns>
		/// Returns <c>true</c> if the user is member of the group,
		/// <c>false</c> otherwise.
		/// </returns>
		public bool IsMemberOfGroup(
			string groupName )
		{
			ADGroupInfo[] groups = ParentGroupsEx;

			if ( groups == null )
			{
				return false;
			}
			else
			{
				Set<string> checkedGroupSamNames = new Set<string>();

				foreach ( ADGroupInfo group in groups )
				{
					if ( DoCheckIsMemberOfGroup(
						group,
						groupName,
						checkedGroupSamNames ) )
					{
						return true;
					}
				}

				return false;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets or sets the GUID.
		/// </summary>
		/// <value>The GUID.</value>
		public Guid Guid
		{
			get
			{
				return guid;
			}
			internal set
			{
				guid = value;
			}
		}

		/// <summary>
		/// The Security IDentifier.
		/// </summary>
		/// <value>The sid.</value>
		public string Sid
		{
			get
			{
				return sid;
			}
			internal set
			{
				sid = value;
			}
		}

		/// <summary>
		/// Gets or sets the account control flags.
		/// </summary>
		/// <value>The account control flags.</value>
		public ADUserFlags AccountControlFlags
		{
			get
			{
				return accountControlFlags;
			}
			internal set
			{
				accountControlFlags = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the sam.
		/// </summary>
		/// <value>The name of the sam.</value>
		public string SamName
		{
			get
			{
				return samName;
			}
			internal set
			{
				samName = value;
			}
		}

		/// <summary>
		/// Gets or sets the CN.
		/// </summary>
		/// <value>The CN.</value>
		public string CN
		{
			get
			{
				return cn;
			}
			internal set
			{
				cn = value;
			}
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the first.
		/// </summary>
		/// <value>The name of the first.</value>
		public string FirstName
		{
			get
			{
				return firstName;
			}
			set
			{
				firstName = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the last.
		/// </summary>
		/// <value>The name of the last.</value>
		public string LastName
		{
			get
			{
				return lastName;
			}
			set
			{
				lastName = value;
			}
		}

		/// <summary>
		/// Gets or sets the initials.
		/// </summary>
		/// <value>The initials.</value>
		public string Initials
		{
			get
			{
				return initials;
			}
			set
			{
				initials = value;
			}
		}

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				description = value;
			}
		}

		/// <summary>
		/// Gets or sets the street address.
		/// </summary>
		/// <value>The street address.</value>
		public string StreetAddress
		{
			get
			{
				return streetAddress;
			}
			set
			{
				streetAddress = value;
			}
		}

		/// <summary>
		/// Gets or sets the post office box.
		/// </summary>
		/// <value>The post office box.</value>
		public string PostOfficeBox
		{
			get
			{
				return postOfficeBox;
			}
			set
			{
				postOfficeBox = value;
			}
		}

		/// <summary>
		/// Gets or sets the zip.
		/// </summary>
		/// <value>The zip.</value>
		public string Zip
		{
			get
			{
				return zip;
			}
			set
			{
				zip = value;
			}
		}

		/// <summary>
		/// Gets or sets the city.
		/// </summary>
		/// <value>The city.</value>
		public string City
		{
			get
			{
				return city;
			}
			set
			{
				city = value;
			}
		}

		/// <summary>
		/// Gets or sets the state.
		/// </summary>
		/// <value>The state.</value>
		public string State
		{
			get
			{
				return state;
			}
			set
			{
				state = value;
			}
		}

		/// <summary>
		/// Gets or sets the country code.
		/// </summary>
		/// <value>The country code.</value>
		public string CountryCode
		{
			get
			{
				return countryCode;
			}
			set
			{
				countryCode = value;
			}
		}

		/// <summary>
		/// Gets or sets the telephone number.
		/// </summary>
		/// <value>The telephone number.</value>
		public string TelephoneNumber
		{
			get
			{
				return telephoneNumber;
			}
			set
			{
				telephoneNumber = value;
			}
		}

		/// <summary>
		/// Gets or sets the mobile number.
		/// </summary>
		/// <value>The mobile number.</value>
		public string MobileNumber
		{
			get
			{
				return mobileNumber;
			}
			set
			{
				mobileNumber = value;
			}
		}

		/// <summary>
		/// Gets or sets the fax number.
		/// </summary>
		/// <value>The fax number.</value>
		public string FaxNumber
		{
			get
			{
				return faxNumber;
			}
			set
			{
				faxNumber = value;
			}
		}

		/// <summary>
		/// Gets or sets the E mail.
		/// </summary>
		/// <value>The E mail.</value>
		public string EMail
		{
			get
			{
				return email;
			}
			set
			{
				email = value;
			}
		}

		/// <summary>
		/// Gets or sets the WWW home page.
		/// </summary>
		/// <value>The WWW home page.</value>
		public string WwwHomePage
		{
			get
			{
				return wwwHomePage;
			}
			set
			{
				wwwHomePage = value;
			}
		}

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				title = value;
			}
		}

		/// <summary>
		/// Gets or sets the department.
		/// </summary>
		/// <value>The department.</value>
		public string Department
		{
			get
			{
				return department;
			}
			set
			{
				department = value;
			}
		}

		/// <summary>
		/// Gets or sets the primary group ID.
		/// </summary>
		/// <value>The primary group ID.</value>
		public int PrimaryGroupID
		{
			get
			{
				return primaryGroupID;
			}
			internal set
			{
				primaryGroupID = value;
			}
		}

		/// <summary>
		/// Gets or sets the company.
		/// </summary>
		/// <value>The company.</value>
		public string Company
		{
			get
			{
				return company;
			}
			set
			{
				company = value;
			}
		}

		/// <summary>
		/// Gets or sets the manager.
		/// </summary>
		/// <value>The manager.</value>
		public string Manager
		{
			get
			{
				return manager;
			}
			set
			{
				manager = value;
			}
		}

		/// <summary>
		/// Checks, whether the user is enabled/disabled.
		/// </summary>
		/// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
		public bool IsActive
		{
			get
			{
				return
					(AccountControlFlags & ADUserFlags.Accountdisable) == 0;
			}
		}

		/// <summary>
		/// Returns a list of the direct(!) groups that a user belongs to.
		/// </summary>
		/// <value>The parent groups.</value>
		public ADGroupInfo[] ParentGroups
		{
			get
			{
				if ( parentGroups == null )
				{
					parentGroups = owner.GetUserParentGroups( this );
				}

				return parentGroups;
			}
		}

		/// <summary>
		/// Returns a list of the direct(!) groups that a user belongs to.
		/// </summary>
		/// <value>The parent groups ex.</value>
		public ADGroupInfo[] ParentGroupsEx
		{
			get
			{
				if ( parentGroupsEx == null )
				{
					parentGroupsEx = owner.GetUserParentGroupsEx( this );
				}

				return parentGroupsEx;
			}
		}

		/// <summary>
		/// Gets or sets the DN.
		/// </summary>
		/// <value>The DN.</value>
		protected internal string DN
		{
			get
			{
				return dn;
			}
			set
			{
				dn = value;
			}
		}

		/// <summary>
		/// The owning parent object.
		/// </summary>
		/// <value>The active directory.</value>
		public ActiveDirectory ActiveDirectory
		{
			get
			{
				return owner;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private Guid guid;
		private string sid;
		private ADUserFlags accountControlFlags;
		private string samName;
		private string cn;
		private string name;
		private string firstName;
		private string lastName;
		private string initials;
		private string description;
		private string streetAddress;
		private string postOfficeBox;
		private string zip;
		private string city;
		private string state;
		private string countryCode;
		private string telephoneNumber;
		private string mobileNumber;
		private string faxNumber;
		private string email;
		private string wwwHomePage;
		private string title;
		private string department;
		private string company;
		private string manager;
		private string dn;
		private int primaryGroupID;

		/// <summary>
		/// Cached.
		/// </summary>
		private ADGroupInfo[] parentGroups;
		private ADGroupInfo[] parentGroupsEx;

		/// <summary>
		/// 
		/// </summary>
		private ActiveDirectory owner;

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Helper.
		/// </summary>
		/// <param name="group">The group.</param>
		/// <param name="groupName">Name of the group.</param>
		/// <param name="checkedGroupSamNames">The checked group sam names.</param>
		/// <returns></returns>
		private bool DoCheckIsMemberOfGroup(
			ADGroupInfo group,
			string groupName,
			Set<string> checkedGroupSamNames )
		{
			if ( group == null )
			{
				return false;
			}
			else
			{
				// Only process once.
				if ( !checkedGroupSamNames.Contains( group.SamName ) )
				{
					checkedGroupSamNames.Add( group.SamName );

					LogCentral.Current.LogInfo(
						string.Format(
						@"[AD] About to check whether group with SAM name '{0}' equals group name '{1}'.",
						group.SamName,
						groupName
						) );

					if ( group.SamName.ToLower() == groupName.ToLower() )
					{
						return true;
					}

					// --
					// Recurse to parents.

					ADGroupInfo[] pgroups = group.ParentGroups;

					if ( pgroups != null )
					{
						foreach ( ADGroupInfo pgroup in pgroups )
						{
							if ( DoCheckIsMemberOfGroup(
								pgroup,
								groupName,
								checkedGroupSamNames ) )
							{
								return true;
							}
						}
					}
				}

				return false;
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}