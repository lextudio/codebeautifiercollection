namespace ZetaLib.Windows
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Runtime;
	using System.Reflection;
	using System.Collections;
	using System.Configuration;
	using System.Globalization;
	using System.Diagnostics;
	using System.IO;
	using System.Net;
	using System.Threading;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Xml;
	using ZetaLib.Core.Base;
	using ZetaLib.Core.Common;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Central class for managing all configuration aspects of the library.
	/// </summary>
	public class LibraryConfiguration :
		LibraryConfigurationBase,
		ILibraryConfiguration
	{
		#region Static routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private static object typeLock = new object();

		/// <summary>
		/// Singleton access to the library configuratin.
		/// </summary>
		/// <value>The current.</value>
		public static LibraryConfiguration Current
		{
			get
			{
				if ( current == null )
				{
					// According to  
					// http://www.dofactory.com/Patterns/PatternSingleton.aspx,
					// it is sufficient to lock only the creation.
					// 
					// Quote:
					//		Support multithreaded applications through
					//		'Double checked locking' pattern which (once
					//		the instance exists) avoids locking each
					//		time the method is invoked 					
					//
					// http://geekswithblogs.net/akraus1/articles/90803.aspx
					// has the correct way of locking: declaring as "volatile".
					//
					// http://www.ibm.com/developerworks/java/library/j-dcl.html
					// has an in-deep discussion.
					lock ( typeLock )
					{
						if ( current == null )
						{
							// Please not that the following line does NOT return a
							// fresh instance every time being called, but rather
							// caches and return the same instance in subsequent calls.
							XmlNode section = ConfigurationManager.GetSection(
								@"zetaLibWindows" ) as XmlNode;

							LibraryConfiguration result = new LibraryConfiguration();
							result.Initialize();
							result.LoadFromXml( section );

							current = result;
						}
					}
				}

				return current;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private static volatile LibraryConfiguration current = null;

		// ------------------------------------------------------------------
		#endregion

		#region Public routines.
		// ------------------------------------------------------------------

		/// <summary>
		/// Initialize this library.
		/// Please ensure to call this function once at the very start of your
		/// application, before doing any logging functions, if you are
		/// running a Windows Forms application. A good place would be inside
		/// the Main() method.
		/// </summary>
		public void Initialize()
		{
			if ( libraryInitialisator == null )
			{
				ZetaLib.Core.LibraryConfiguration.Current.Initialize();
				libraryInitialisator = new LibraryInitialisator();

				lock ( typeLock )
				{
					if ( automaticDesignModeDetection )
					{
						isDesignMode = false;
					}
				}
			}
		}

		/// <summary>
		/// Loads this class form the given configuration node.
		/// </summary>
		/// <param name="node">The node.</param>
		public void LoadFromXml(
			XmlNode node )
		{
			if ( node != null )
			{
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Configuration properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Global flag to check whether the application is in design mode
		/// (in terms of the Windows Forms designer).
		/// The default behaviour is to indicate a TRUE when the library
		/// has not yet been initialized via "Initialize()" and to indicate
		/// FALSE if it has been intitialized via "Initialize()".
		/// You can override the default behaviour by explicitely setting
		/// a value.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is design mode; otherwise, <c>false</c>.
		/// </value>
		/// <remarks>
		/// Must be a static property to avoid implicit library initialization.
		/// </remarks>
		public static bool IsDesignMode
		{
			get
			{
				lock ( typeLock )
				{
					return isDesignMode;
				}
			}
			set
			{
				lock ( typeLock )
				{
					isDesignMode = value;
					automaticDesignModeDetection = false;
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// Doing new initialization.
		/// </summary>
		private static LibraryInitialisator libraryInitialisator = null;

		/// <summary>
		/// Helper class for initialising the library.
		/// </summary>
		private class LibraryInitialisator
		{
			#region Public methods.

			/// <summary>
			/// Constructor.
			/// </summary>
			public LibraryInitialisator()
			{
			}

			#endregion
		}

		private static bool isDesignMode = true;
		private static bool automaticDesignModeDetection = true;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Read the configuration.
	/// See http://support.microsoft.com/?kbid=309045.
	/// </summary>
	internal sealed class LibraryConfigurationSectionHandler :
		IConfigurationSectionHandler
	{
		#region IConfigurationSectionHandler member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Creates an instance of this class.
		/// </summary>
		/// <param name="parent">Parent object.</param>
		/// <param name="configContext">Configuration context object.</param>
		/// <param name="section">Section XML node.</param>
		/// <returns>The created section handler object.</returns>
		public object Create(
			object parent,
			object configContext,
			XmlNode section )
		{
			return section;
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}