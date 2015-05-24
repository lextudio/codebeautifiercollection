using System;
using System.IO;
using Lextm.OpenTools;

namespace Lextm.Utilities.InDate
{

	/// <summary>
	/// Update context.
	/// </summary>
	/// <remarks>The context is used in the UI. It hides all the details.</remarks>
	class UpdateContext
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="version">Installed version</param>
		internal UpdateContext( Version version )
		{
			this.installedVersion = version;
			this.state = new Initialized();
		}

		/// <summary>
		/// Verifies that update is ended.
		/// </summary>
		/// <returns>true if state internal is failed or successful.</returns>
		internal bool Completed
		{
			get {
                return state is Completed;				
			}
		}

		private const string localListName = "list.htm";
		private const string localPackageName = "package.zip";
		/// <summary>
		/// List file name.
		/// </summary>
		internal static string LocalListFileName
		{
			get {
				// the file is save in the same folder.
				return Path.Combine(Path.GetTempPath(), localListName);
			}
		}
		
		internal static string LocalPackageFileName
		{
			get {
				return Path.Combine(Path.GetTempPath(), localPackageName);
			}
		}

		/// <summary>
		/// GetTip wrapper of state.
		/// </summary>
		/// <returns></returns>
		internal string GetTip ()
		{
			return state.GetTip();
		}

		/// <summary>
		/// Verifies that if update is available.
		/// </summary>
		/// <param name="latest">Latest version</param>
		/// <returns>true if the latest versions is not installed,
		/// and false if otherwise.</returns>
		internal bool IsUpdateAvailable(Version latest)
		{
			return latest > installedVersion;
		}

		/// <summary>
		/// Runs forward and keeps changing the state.
		/// </summary>
		internal void RunForward( )
		{
			state.Handle(this);
		}
		
		internal void Transit()
		{
			state.Transit(this);
		}

		/// <summary>
		/// Sets state.
		/// </summary>
		/// <param name="newState">New state</param>
		internal void SetState( UpdateStateBase newState )
		{
			state = newState;
		}

		/// <summary>
		/// A value that indicates whether downloading update.
		/// </summary>
		internal static bool AlwaysDownload
		{
			get
			{
				return (bool)PropertyRegistry.Get("InDateAutoInstall", false);
			}
		}

		/// <summary>
		/// A value that indicates whether installing update after downloading.
		/// </summary>
		internal static bool AlwaysInstall
		{
			get
			{
				return (bool)PropertyRegistry.Get("InDateAutoInstall", false);
			}
		}

		/// <summary>
		/// A value that indicates whether RC versions are included.
		/// </summary>
		internal static bool IncludeRcVersions
		{
			get
			{
				return (bool)PropertyRegistry.Get("InDateIncludeRC", false);
			}
		}
		
		internal static Uri FillUri(string path) {
			if (Lextm.StringHelper.IsHttpUri(path)) {
				return new Uri(path);
			} else {
				UriBuilder link = new UriBuilder(BaseUri);
				link.Path = path;
				return link.Uri;
			}
		}
		/// <summary>
		/// File list link.
		/// </summary>
		internal static System.Uri PackageListUri
		{
			get
			{
				return new Uri((string)PropertyRegistry.Get("InDateListUri",
				                                            "http://code.google.com/p/lextudio/downloads/list"
				                                           ));
			}
		}

		private static System.Uri baseUri;

		private static System.Uri BaseUri
		{
			get
			{
				if (baseUri == null)
				{
					UriBuilder builder = new UriBuilder(PackageListUri);
					builder.Path = String.Empty;
					builder.Query = String.Empty;
					builder.Fragment = String.Empty;
					baseUri = builder.Uri;
				}
				return baseUri;
			}
		}

		private System.Version installedVersion;
		private UpdateStateBase state;
	}
}



