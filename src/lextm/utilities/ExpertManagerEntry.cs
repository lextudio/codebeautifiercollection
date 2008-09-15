using System;
using System.Windows.Forms;
using Lextm.Diagnostics;
using Lextm.OpenTools;
using Lextm.OpenTools.IO;
using UnhandledExceptionManager;
using Zayko.Dialogs.UnhandledExceptionDlg;

namespace Lextm.Utilities
{
    /// <summary>
    /// Executable entry.
    /// </summary>
    public static class ExpertManagerEntry
    {
        private static readonly string configuration =
            Lextm.OpenTools.IO.Path.GetDataFile("cbc2.exe.config");
        /// <summary>
        /// Entry for Utilities Plus.
        /// </summary>
        /// <param name="args">Arguments</param>
        [STAThread]
        public static void Main()
        {
            IntPtr prevHandle;
            if (SingletonApp.SingletonAppliation.PreviousInstanceDetected(out prevHandle))
            {
                SingletonApp.SingletonAppliation.ShowPreviousInstance(prevHandle);
                return;
            }

            TExceptionManager.DefaultInfoManagerType = typeof(UnhandledExDlgForm);
            TExceptionManager.Initialize();
            TExceptionManager.UserContinue = true;
            ToolStripManager.Renderer = new Office2007Renderer.Office2007Renderer();
            LoggingService.Start(configuration);
            if (!ShareUtils.InstalledForCurrentUser)
            {
                string fileName = System.IO.Path.Combine(Path.ProgramFilesFolder, "installforcurrentuser.exe");
                ShellHelper.Execute(fileName, null, null, true);
            }
            Gui.FormExpertManager _Frm = new Gui.FormExpertManager();
            Application.Run(_Frm);
        }
    }
}
