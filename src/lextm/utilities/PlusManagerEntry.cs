using System;
using System.Windows.Forms;
using UnhandledExceptionManager;
using Lextm.Diagnostics;
using Lextm.OpenTools.IO;
using Lextm.OpenTools;
using Zayko.Dialogs.UnhandledExceptionDlg;

namespace Lextm.Utilities
{
    /// <summary>
    /// Executable entry.
    /// </summary>
    public sealed class PlusManagerEntry
    {
        private PlusManagerEntry()
        {
        }
        private static readonly string configuration =
            Lextm.OpenTools.IO.Path.GetDataFile("cbc2.exe.config");
        /// <summary>
        /// Entry for Utilities Plus.
        /// </summary>
        /// <param name="args">Arguments</param>
        [STAThread]
        public static void Main(string[] args)
        {
            IntPtr prevHandle;
            if (SingletonApp.SingletonAppliation.PreviousInstanceDetected(out prevHandle))
            {
                SingletonApp.SingletonAppliation.ShowPreviousInstance(prevHandle);
                return;
            }

            ToolStripManager.Renderer = new Office2007Renderer.Office2007Renderer();

            TExceptionManager.DefaultInfoManagerType = typeof(UnhandledExDlgForm);
            TExceptionManager.Initialize();
            TExceptionManager.UserContinue = true;
            LoggingService.Start(configuration);

            if (!ShareUtils.InstalledForCurrentUser)
            {
                string fileName = System.IO.Path.Combine(Path.ProgramFilesFolder, "installforcurrentuser.exe");
                ShellHelper.Execute(fileName, null, null, true);
            }

            Gui.FormPlusManager _Frm = new Gui.FormPlusManager();
            Application.Run(_Frm);
        }
    }
}
