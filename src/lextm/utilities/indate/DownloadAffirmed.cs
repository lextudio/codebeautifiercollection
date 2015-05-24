using System;
using System.Net;
using Lextm.OpenTools;
using Lextm.Diagnostics;

namespace Lextm.Utilities.InDate
{

	/// <summary>
	/// User has affirmed downloading the package.
	/// </summary>
	class DownloadAffirmed : UpdateStateBase
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="link">Packager link</param>
		internal DownloadAffirmed( Uri link )
		{
			_link = link;
		}

		/// <summary>
		/// Gets tip.
		/// </summary>
		internal override string GetTip( )
		{
			return "Downloading update package.";
		}

		/// <summary>
		/// Handles.
		/// </summary>
		/// <param name="context">Context</param>
		internal override void Handle( UpdateContext context )
		{
			try
			{
				(new WebClient()).DownloadFile(
					_link.ToString(), UpdateContext.LocalPackageFileName);
				_successful = true;
			}
			catch (WebException ex)
			{
				_error = ex.Message;
				_successful = false;
			}
		}
		
		internal override void Transit(UpdateContext context)
		{
			if (_successful) {
				SetState(context, new PackageReceived());
			}
			else 
			{
				SetState(context, new Failed(_error));
			}
		}

		private Uri _link;
		bool _successful;
		string _error;
	}
}



