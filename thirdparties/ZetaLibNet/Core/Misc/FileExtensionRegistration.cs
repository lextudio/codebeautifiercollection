namespace ZetaLib.Core.Misc
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections;
	using System.Configuration;
	using System.Globalization;
	using System.Diagnostics;
	using System.IO;
	using System.Net;
	using System.Runtime.InteropServices;
	using System.Threading;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Xml;
	using Microsoft.Win32;
	using ZetaLib.Core.Base;
	using ZetaLib.Core.Common;
	using System.Collections.Generic;
	using ZetaLib.Core.Properties;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Class for registering ("associating") a file extension with 
	/// a given application.
	/// </summary>
	/// <remarks>For various implementations, see 
	/// http://www.google.com/search?q=Associate+File+Extension at Google.</remarks>
	public sealed class FileExtensionRegistration
	{
		#region Information class for registering.
		// ------------------------------------------------------------------
		/// <summary>
		/// Information class for registering.
		/// </summary>
		public class RegistrationInformation
		{
			#region Public properties.

			/// <summary>
			/// The file extension to register.
			/// </summary>
			/// <value>The extension.</value>
			/// <example>For example ".cad".</example>
			public string Extension
			{
				get
				{
					return extension;
				}
				set
				{
					extension = value;
				}
			}

			/// <summary>
			/// The class name of the application to register.
			/// </summary>
			/// <value>The name of the class.</value>
			/// <example>For example "MyCadDoc".</example>
			public string ClassName
			{
				get
				{
					return className;
				}
				set
				{
					className = value;
				}
			}

			/// <summary>
			/// The description of the file type.
			/// </summary>
			/// <value>The description.</value>
			/// <example>For example "CAD document".</example>
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
			/// The application that handles the file extension.
			/// </summary>
			/// <value>The application file path.</value>
			/// <example>For example "C:\Cad\MyCad.exe".</example>
			public string ApplicationFilePath
			{
				get
				{
					return applicationFilePath;
				}
				set
				{
					applicationFilePath = value;
				}
			}

			#endregion

			#region Private variables.

			private string extension;
			private string className;
			private string description;
			private string applicationFilePath;

			#endregion
		}

		// ------------------------------------------------------------------
		#endregion

		#region Static routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Check whether a file extension is registered.
		/// See
		/// http://groups.google.de/groups?hl=de&lr=&selm=953pic%24l7m%241%40nnrp1.deja.com.
		/// </summary>
		/// <param name="extension">The extension.</param>
		/// <returns>
		/// 	<c>true</c> if [is file extension registered] [the specified extension]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsFileExtensionRegistered(
			string extension )
		{
			bool result;
			if ( cacheFor_IsFileExtensionRegistered.TryGetValue(
				extension, out result ) )
			{
				return result;
			}
			else
			{
				string lcExtension =
					@"." +
					extension.ToLowerInvariant().Trim( '.', '*' );

				Microsoft.Win32.RegistryKey key =
					Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(
					lcExtension,
					RegistryKeyPermissionCheck.Default,
					System.Security.AccessControl.RegistryRights.ReadKey );

				result = key != null;

				lock ( typeLock )
				{
					cacheFor_IsFileExtensionRegistered[extension] = result;
				}

				return result;
			}
		}

		/// <summary>
		/// Registers the given extension with the given application.
		/// </summary>
		/// <param name="info">Information about the extension to
		/// register. All properties of the information class must have
		/// been filled before passing to this function.</param>
		public static void Register(
			RegistrationInformation info )
		{
			if ( info.Extension == null ||
				info.Extension.Length <= 0 )
			{
				throw new ArgumentException(
					Resources.Str_ZetaLib_Core_Common_Misc_FileExtensionRegistration_01,
					@"info.Extension" );
			}
			else if ( info.ClassName == null ||
				info.ClassName.Length <= 0 )
			{
				throw new ArgumentException(
					Resources.Str_ZetaLib_Core_Common_Misc_FileExtensionRegistration_02,
					@"info.ClassName" );
			}
			else if ( info.Description == null ||
				info.Description.Length <= 0 )
			{
				throw new ArgumentException(
					Resources.Str_ZetaLib_Core_Common_Misc_FileExtensionRegistration_03,
					@"info.Description" );
			}
			else if ( info.ApplicationFilePath == null ||
				info.ApplicationFilePath.Length <= 0 )
			{
				throw new ArgumentException(
					Resources.Str_ZetaLib_Core_Common_Misc_FileExtensionRegistration_04,
					@"info.ApplicationFilePath" );
			}
			else
			{
				// Everything passed correctly.

				// Add dot if missing.
				if ( info.Extension[0] != '.' )
				{
					info.Extension = '.' + info.Extension;
				}

				bool hasSomethingChanged = false;

				// Create a new registry key under HKEY_CLASSES_ROOT.
				if ( !new List<string>( Registry.ClassesRoot.GetSubKeyNames()
					).Contains( info.Extension ) )
				{
					hasSomethingChanged = true;
				}
				RegistryKey key = Registry.ClassesRoot.CreateSubKey(
					info.Extension );

				// Create a value for this key that contains the class name.
				if ( key.GetValue( null ) == null ||
					key.GetValue( null ).ToString() != info.ClassName )
				{
					hasSomethingChanged = true;
				}
				key.SetValue(
					null,
					info.ClassName );

				// Create a new key for the class name.
				if ( !new List<string>( Registry.ClassesRoot.GetSubKeyNames()
					).Contains( info.ClassName ) )
				{
					hasSomethingChanged = true;
				}
				key = Registry.ClassesRoot.CreateSubKey(
					info.ClassName + @"\Shell\Open\Command" );

				// Set its value to the command line.
				string s = string.Format(
					@"""{0}"" ""%1""",
					info.ApplicationFilePath );
				if ( key.GetValue( null ) == null ||
					key.GetValue( null ).ToString() != s )
				{
					hasSomethingChanged = true;
				}
				key.SetValue( null, s );

				// TODO: Use the description.

				// TODO: Optionally set an icon as described at
				// http://support.microsoft.com/kb/247529/.

				if ( hasSomethingChanged )
				{
					// Notify Windows that file associations have changed.
					SHChangeNotify(
						SHCNE_ASSOCCHANGED,
						SHCNF_IDLIST,
						0,
						0 );

					// Flush.
					lock ( typeLock )
					{
						cacheFor_IsFileExtensionRegistered.Clear();
					}
				}
			}
		}

		/// <summary>
		/// Removes a previously registered file extension.
		/// </summary>
		/// <param name="info">Information about the extension to
		/// unregister. All properties of the information class must have
		/// been filled before passing to this function.</param>
		public static void Unregister(
			RegistrationInformation info )
		{
			// TODO.
			throw new Exception( "Not yet implemented." );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private helpers.
		// ------------------------------------------------------------------

		private const int SHCNE_ASSOCCHANGED = 0x8000000;
		private const int SHCNF_IDLIST = 0;

		/// <summary>
		/// P/Invoke.
		/// </summary>
		/// <param name="wEventId">The w event id.</param>
		/// <param name="uFlags">The u flags.</param>
		/// <param name="dwItem1">The dw item1.</param>
		/// <param name="dwItem2">The dw item2.</param>
		[DllImport( @"shell32.dll" )]
		static extern void SHChangeNotify(
			int wEventId,
			uint uFlags,
			int dwItem1,
			int dwItem2 );

		/// <summary>
		/// 
		/// </summary>
		private static object typeLock = new object();

		/// <summary>
		/// 
		/// </summary>
		private static  Dictionary<string, bool> cacheFor_IsFileExtensionRegistered =
			new Dictionary<string, bool>();

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}