using ICSharpCode.SharpZipLib;
using System;
using System.Diagnostics;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Lextm.Diagnostics;

namespace Lextm.Utilities.InDate
{
    /// <summary>
    /// Package is downloaded.
    /// </summary>
    class PackageReceived : UpdateStateBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        internal PackageReceived( )
        {}
        /// <summary>
        /// Gets tip.
        /// </summary>
        /// <returns></returns>
        internal override string GetTip( )
        {
            return "Extracting update package.";
        }

        /// <summary>
        /// Handles.
        /// </summary>
        /// <param name="context">Context</param>
        internal override void Handle( UpdateContext context )
        {
            FastZip zipper = new FastZip();
            string fileName = UpdateContext.LocalPackageFileName;
            try
            {
                zipper.ExtractZip(fileName,
            	                  Path.GetDirectoryName(fileName),
                                  String.Empty);
                _extracted = true;
            }
            catch (SharpZipBaseException ex) 
            {
            	LoggingService.Fatal(ex);
            	_extracted = false;
            }
        }
        
		internal override void Transit(UpdateContext context)
		{
			if (_extracted)
            {
                string installerName = Path.Combine(Path.GetDirectoryName(UpdateContext.LocalPackageFileName),
                                                   "setup.exe");
                if (!File.Exists(installerName))
                {
                    SetState(context, new Failed("The installer is not there."));
                }
                else
                {
                    while (IdeIsRunning)
                    {
                        Lextm.Windows.Forms.MessageBoxFactory.Info("Please close CodeGear RAD Studio before installing the update.");
                    }
                    Lextm.Diagnostics.ShellHelper.Execute(installerName);
                    SetState(context, new Succeeded());
                }
			} 
			else 
			{
				SetState(context, new Failed("Bad package."));			
			}
		}
		
		bool _extracted;

        private static bool IdeIsRunning
        {
            get
            {
                return Process.GetProcessesByName("bds").Length > 0;
            }
        }
	}
}




