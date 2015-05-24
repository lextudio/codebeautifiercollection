using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using PSTaskDialog;
using System.Globalization;
using Lextm.OpenTools;

namespace Lextm.Utilities.InDate
{

    /// <summary>
    /// Update list is downloaded.
    /// </summary>
    class ListReceived : UpdateStateBase
    {
		/// <summary>
		/// Constructor.
		/// </summary>
		internal ListReceived( )
		{}

		/// <summary>
		/// Gets tip.
		/// </summary>
		/// <returns></returns>
		internal override string GetTip( )
		{
			return "Update repository is connected.";
		}

		/// <summary>
		/// Handles.
		/// </summary>
		/// <param name="context">Context</param>
		internal override void Handle( UpdateContext context )
		{
            System.Threading.Thread.Sleep(1000);
			IList<PackageRecord> packages = GetZipLinks(UpdateContext.LocalListFileName);
			if (packages.Count != 0)
			{
				_record = GetLastestRecord(context, packages);
				if (_record != null )
				{
					_found = true;
					return;
				}
			}
			_found = false;
		}
		
		PackageRecord _record;
		bool _found;
		
		internal override void Transit(UpdateContext context)
		{
			if (_found)
			{
                cTaskDialog.ForceEmulationMode = true;
                cTaskDialog.UseToolWindowOnXP = true;
                cTaskDialog.EmulatedFormWidth = 450;

                bool confirmed = UpdateContext.AlwaysDownload ||
                    cTaskDialog.ShowCommandBox("Update Found",
                                               String.Format(CultureInfo.InvariantCulture, "A new version {0} is available.", _record.Version),
                                               "Do you want to download it now?",
                                               String.Format(CultureInfo.InvariantCulture, "The URL: {0}.", _record.Link),
                                               "Latest version contains bug fixes and new features.",
                                               "turn on auto download and install",
                                               "Yes, download it|No, thanks",
                                               false,
                                               eSysIcons.Question,
                                               eSysIcons.Information) == 0;

                PropertyRegistry.Set("InDateAutoInstall", cTaskDialog.VerificationChecked);
                if (confirmed)
                {
                    SetState(context, new DownloadAffirmed(_record.Link));
                }
                else
                {
                    SetState(context, new Canceled());
                }
			}
			else 
			{
				SetState(context, new Succeeded(
						 "No update is available at this moment."));
			}
		}

		private static PackageRecord GetLastestRecord(UpdateContext context,
											   IList<PackageRecord> packages)
		{
			Trace.Assert(packages.Count!= 0);
			PackageRecord result = packages[0];

			for (int i = 1; i < packages.Count; i++)
			{
				if (packages[i].Version > result.Version)
				{
					result = packages[i];
				}
			}
			if (context.IsUpdateAvailable(result.Version))
			{
				return result;
			}
			else
			{
				return null;
			}
		}

		private static Version GetVersion(Uri url)
		{		
			Version result;
			string verstring = Path.GetFileNameWithoutExtension(url.ToString());
			if (verstring.StartsWith("gv", StringComparison.Ordinal)) 
			{
				string ver = verstring.Substring(2);
				if ((UpdateContext.IncludeRcVersions) && (ver.EndsWith("rc", StringComparison.Ordinal)))
				{
					// RC version file name have a string "rc" at the end.
					ver = ver.Replace("rc", null);
				}

				try
				{
					result = new Version(ver);
				}
				catch (FormatException) {
					result = new Version("0.0.0.0");
				}
			} else {
				result = new Version("0.0.0.0");
			}
			return result;
		}

		private static IList<PackageRecord> GetZipLinks(string fileName)
		{
			Trace.Assert(File.Exists(fileName));
			string file = Lextm.IO.FileHelper.GetContentOf(fileName);
			string regular = "href\\s*=\\s*\"(?<1>\\S+.zip)\"";

			// Compile the regular expression.
			Regex r = new Regex(regular, RegexOptions.IgnoreCase |
								RegexOptions.Singleline |
								RegexOptions.Compiled);
			// Match the regular expression pattern against a text string.
			Match m;
			IList<PackageRecord> result = new List<PackageRecord>();

			for (m = r.Match(file); m.Success; m = m.NextMatch())
			{
				Uri link = UpdateContext.FillUri(m.Groups[1].Value);
				result.Add(new PackageRecord(GetVersion(link), link));
			}
			return result;
		}
	}
}









