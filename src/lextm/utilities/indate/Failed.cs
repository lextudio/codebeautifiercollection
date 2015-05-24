using Lextm.Diagnostics;
using PSTaskDialog;
using Lextm.OpenTools;

namespace Lextm.Utilities.InDate
{

    /// <summary>
    /// Updating has failed
    /// </summary>
    class Failed : UpdateStateBase
    {
		/// <summary>
		/// Constructor.
		/// </summary>
		internal Failed( )
		{
			_error = "Unknown error.";
		}

		/// <summary>
		/// Construtor.
		/// </summary>
		/// <param name="error">Error</param>
		internal Failed( string error )
		{
			_error = error;
		}

		/// <summary>
		/// Gets tip.
		/// </summary>
		/// <returns></returns>
		internal override string GetTip( )
		{
			LoggingService.Error(_error);
			return _error;
		}

		/// <summary>
		/// Handles.
		/// </summary>
		/// <param name="context">Context</param>
		internal override void Handle( UpdateContext context )
		{
            System.Threading.Thread.Sleep(1000);
        }
		/// <summary>
		/// Transites.
		/// </summary>
		/// <param name="context"></param>
        internal override void Transit(UpdateContext context)
        {
            cTaskDialog.ForceEmulationMode = true;
            cTaskDialog.UseToolWindowOnXP = true;
            cTaskDialog.EmulatedFormWidth = 450;
            //cTaskDialog.VerificationChecked = !(bool)PropertyRegistry.Get("InDateCheckAtStartup", true);
            int result = cTaskDialog.ShowCommandBox("Oops!",
                                                    "Update process is not completed.",
                                                    "Go to homepage to check for latest updates?",
                                                    "Error message: " + _error,
                                                    "Homepage URL: http://code.google.com/p/lextudio",
                                                    "turn off auto update",
                                                    "Yes, take me there|No, thanks",
                                                    false,
                                                    eSysIcons.Error,
                                                    eSysIcons.Information);
            PropertyRegistry.Set("InDateCheckAtStartup", !cTaskDialog.VerificationChecked);
            if (result == 0)
            {
                ShellHelper.Execute(PropertyRegistry.Get("InDateListUri").ToString());
            }
            SetState(context, new Completed());
        }

		private string _error;
    }
}

