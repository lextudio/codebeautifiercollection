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
	/// Summarizes group information from Active Directory.
	/// <see href="ms-help://MS.MSDNQTR.2002JUL.1033/netdir/ad/group_object_user_interface_mapping.htm"/>
	/// </summary>
	[DebuggerDisplay( @"Name = {name}, SAM name = {samName}" )]
	public class ADGroupInfo
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="owner">The owner.</param>
		protected internal ADGroupInfo(
			ActiveDirectory owner )
		{
			this.owner = owner;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Guid
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
		/// SAM name
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
		/// Common name
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
		/// Name
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
		/// E-mail address
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
		/// Scope
		/// </summary>
		/// <value>The scope.</value>
		public string Scope
		{
			get
			{
				return scope;
			}
			set
			{
				scope = value;
			}
		}

		/// <summary>
		/// Type
		/// </summary>
		/// <value>The type.</value>
		public int Type
		{
			get
			{
				return type;
			}
			internal set
			{
				type = value;
			}
		}

		/// <summary>
		/// Description
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
		/// Returns a list of the direct(!) groups that a group belongs to.
		/// </summary>
		/// <value>The parent groups.</value>
		public ADGroupInfo[] ParentGroups
		{
			get
			{
				if ( parentGroups == null )
				{
					parentGroups = owner.GetGroupParentGroups( this );
				}

				return parentGroups;
			}
		}

		/// <summary>
		/// Returns a list of the direct(!) groups that belong to this group.
		/// </summary>
		/// <value>The child groups.</value>
		public ADGroupInfo[] ChildGroups
		{
			get
			{
				if ( childGroups == null )
				{
					childGroups = owner.GetGroupChildGroups( this );
				}

				return childGroups;
			}
		}

		/// <summary>
		/// Returns a list of the direct(!) groups that a group belongs to.
		/// </summary>
		/// <value>The child users.</value>
		public ADUserInfo[] ChildUsers
		{
			get
			{
				if ( childUsers == null )
				{
					childUsers = owner.GetGroupChildUsers( this );
				}

				return childUsers;
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
		private string samName;
		private string cn;
		private string name;
		private string email;
		private string scope;
		private int type;
		private string description;
		private string dn;

		/// <summary>
		/// Cached.
		/// </summary>
		private ADGroupInfo[] parentGroups;

		/// <summary>
		/// Cached.
		/// </summary>
		private ADGroupInfo[] childGroups;

		/// <summary>
		/// Cached.
		/// </summary>
		private ADUserInfo[] childUsers;

		/// <summary>
		/// 
		/// </summary>
		private ActiveDirectory owner;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}