using System.Net;
using System;
using System.IO;
using Lextm.Diagnostics;
using Lextm.OpenTools;

namespace Lextm.Utilities.InDate
{

    /// <summary>
    /// updating is started.
    /// </summary>
    class Initialized : UpdateStateBase
    {
		internal Initialized()
		{}

		/// <summary>
		/// Gets tip.
		/// </summary>
		/// <returns></returns>
		internal override string GetTip( )
		{
			return "Downloading the update list.";
		}

		/// <summary>
		/// Handles.
		/// </summary>
		/// <param name="context">Context</param>
		internal override void Handle( UpdateContext context )
		{
			try
			{
				// download the blog entry first.
				MessageService.Debug("Download " + UpdateContext.PackageListUri 
				                    + " to " + UpdateContext.LocalListFileName);
				(new WebClient()).DownloadFile(UpdateContext.PackageListUri.ToString(), 
				                               UpdateContext.LocalListFileName);
				_successful = true;
			}
			catch (WebException ex)
			{
				_successful = false;
				_error = ex.Message;
			}
		}
		
		internal override void Transit(UpdateContext context)
		{
			if (_successful) {
				SetState(context, new ListReceived());
			}
			else
			{
				SetState(context, new Failed(_error));
			}
		}
		
		bool _successful;
		string _error;
    }
}



