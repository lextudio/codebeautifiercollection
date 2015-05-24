/*
_____________________________________
© Pedro Miguel C. Cardoso 2007.
All rights reserved.
http://pmcchp.com/

Redistribution and use in source and binary forms, with or without 
modification, are permitted provided that the following conditions are met:

1) Redistributions of source code must retain the above copyright notice, 
   this list of conditions and the following disclaimer. 
2) Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution. 
3) Neither the name of the ORGANIZATION nor the names of its contributors
   may be used to endorse or promote products derived from this software
   without specific prior written permission. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
THE POSSIBILITY OF SUCH DAMAGE.
*/
// Copyright (c) Sven Groot (Ookii.org) 2006

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using Vista_Api.Dialog.Native;

namespace Vista_Api
{
    /// <summary>
    /// Prompts the user to select a location for saving a file.
    /// </summary>
    /// <remarks>
    /// This class will use the Vista style save file dialog if possible, and automatically fall back to the old-style 
    /// dialog on versions of Windows older than Vista.
    /// </remarks>
    [Designer("System.Windows.Forms.Design.SaveFileDialogDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), System.Drawing.ToolboxBitmap(typeof(Vista_Api.SaveFileDialog), "SaveFileDialog.bmp"), Description("Prompts the user to open a file.")]
    public class SaveFileDialog : FileDialog
    {
        /// <summary>
        /// Creates a new instance of <see cref="SaveFileDialog" /> class.
        /// </summary>
        public SaveFileDialog()
            : this(false)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="SaveFileDialog" /> class.
        /// </summary>
        /// <param name="forceDownlevel">When true, the old style common file dialog will always be used even if the OS supports the Vista style.</param>
        public SaveFileDialog(bool forceDownlevel)
        {
            if (forceDownlevel || !NativeMethods.IsVistaOrLater)
                DownlevelDialog = new System.Windows.Forms.SaveFileDialog();
        }

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box prompts the user for permission to create a file if the 
        /// user specifies a file that does not exist.
        /// </summary>
        /// <value>
        /// true if the dialog box prompts the user before creating a file if the user specifies a file name that does not exist; 
        /// false if the dialog box automatically creates the new file without prompting the user for permission. The default 
        /// value is false.
        /// </value>
        [DefaultValue(false), Category("Behavior"), Description("A value indicating whether the dialog box prompts the user for permission to create a file if the user specifies a file that does not exist.")]
        public bool CreatePrompt
        {
            get
            {
                if( DownlevelDialog != null )
                    return ((System.Windows.Forms.SaveFileDialog)DownlevelDialog).CreatePrompt;
                return GetOption(NativeMethods.FOS.FOS_CREATEPROMPT);
            }
            set
            {
                if( DownlevelDialog != null )
                    ((System.Windows.Forms.SaveFileDialog)DownlevelDialog).CreatePrompt = value;
                else
                    SetOption(NativeMethods.FOS.FOS_CREATEPROMPT, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Save As dialog box displays a warning if the user 
        /// specifies a file name that already exists.
        /// </summary>
        /// <value>
        /// true if the dialog box prompts the user before overwriting an existing file if the user specifies a file 
        /// name that already exists; false if the dialog box automatically overwrites the existing file without 
        /// prompting the user for permission. The default value is true.
        /// </value>
        [Category("Behavior"), DefaultValue(true), Description("A value indicating whether the Save As dialog box displays a warning if the user specifies a file name that already exists.")]
        public bool OverwritePrompt
        {
            get
            {
                if( DownlevelDialog != null )
                    return ((System.Windows.Forms.SaveFileDialog)DownlevelDialog).OverwritePrompt;
                return GetOption(NativeMethods.FOS.FOS_OVERWRITEPROMPT);
            }
            set
            {
                if( DownlevelDialog != null )
                    ((System.Windows.Forms.SaveFileDialog)DownlevelDialog).OverwritePrompt = value;
                else
                    SetOption(NativeMethods.FOS.FOS_OVERWRITEPROMPT, value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Resets all properties to their default values.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            if( DownlevelDialog == null )
            {
                OverwritePrompt = true;
            }
        }

        /// <summary>
        /// Opens the file with read/write permission selected by the user.
        /// </summary>
        /// <returns>The read/write file selected by the user.</returns>
        public System.IO.Stream OpenFile()
        {
            if( DownlevelDialog != null )
                return ((System.Windows.Forms.SaveFileDialog)DownlevelDialog).OpenFile();
            else
            {
                string fileName = FileName;
                if( string.IsNullOrEmpty(fileName) )
                    throw new ArgumentNullException("FileName");
                return new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the <see cref="FileDialog.FileOk" /> event.
        /// </summary>
        /// <param name="e">A <see cref="System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>        
        protected override void OnFileOk(CancelEventArgs e)
        {
            // For reasons unknown, .Net puts the OFN_FILEMUSTEXIST and OFN_CREATEPROMPT flags on the save file dialog despite 
            // the fact that these flags only works on open file dialogs, and then prompts manually. Similarly, the 
            // FOS_CREATEPROMPT and FOS_FILEMUSTEXIST flags don't actually work on IFileSaveDialog, so we have to implement 
            // the prompt manually.
            if( DownlevelDialog == null )
            {
                if( CheckFileExists && !File.Exists(FileName) )
                {
                    PromptUser(ComDlgResources.FormatString(ComDlgResources.ComDlgResourceId.FileNotFound, Path.GetFileName(FileName)), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.Cancel = true;
                    return;
                }
                if( CreatePrompt && !File.Exists(FileName) )
                {
                    if( !PromptUser(ComDlgResources.FormatString(ComDlgResources.ComDlgResourceId.CreatePrompt, Path.GetFileName(FileName)), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) )
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            base.OnFileOk(e);
        }

        #endregion

        #region Internal Methods

        internal override IFileDialog CreateFileDialog()
        {
            return new NativeFileSaveDialog();
        }

        #endregion

    }
}
