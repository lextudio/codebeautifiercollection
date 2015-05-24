using System;

using System.IO;
using System.Windows.Forms;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Gui;
using BeWise.SharpBuilderTools.Helpers;
using Borland.Studio.ToolsAPI;
using Lextm.OpenTools;
using System.Collections.Generic;
using System.Text;
using Lextm.Diagnostics;

namespace Lextm.WiseEditor.Feature {

	public class ClipboardWatcherFeature : CustomFeature {
		
		public ClipboardWatcherFeature() {}
		
#region hook
		private class ClipboardHook : NativeWindow, IDisposable {

			internal ClipboardHook() {}
			
			internal void Create() {
				CreateParams _CP = new CreateParams();

				if (System.Environment.OSVersion.Version.Major >= 5)
					_CP.Parent = (IntPtr)HWND_MESSAGE;

				CreateHandle(_CP);
			}

			/**************************************************************/
			/*                       Destructor
                     /**************************************************************/

			protected void Dispose(bool disposing) {
				if (disposing)
					DestroyHandle();
			}


			~ClipboardHook() {
				Dispose(false);
			}

			public void Dispose() {
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			/**************************************************************/
			/*                       Private
                     /**************************************************************/

			// Win32 functions Mapping
			private const int WM_CHANGECBCHAIN = 0x030D;
			private const int WM_DRAWCLIPBOARD = 0x0308;
			private const int HWND_MESSAGE = -3;

			private IntPtr _NextClipboardWindow;
			private EventHandler fChanged;

			/**************************************************************/
			/*                       Protected
                     /**************************************************************/

			protected override void WndProc(ref Message m) {
				if (m.Msg == WM_CHANGECBCHAIN) {
					if (_NextClipboardWindow == m.WParam)
						_NextClipboardWindow = m.LParam;
					else
						WinAPI.SendMessage(_NextClipboardWindow, WM_CHANGECBCHAIN, m.WParam, m.LParam);
				} else if (m.Msg == WM_DRAWCLIPBOARD) {
					try {
						OnChanged();
					} finally {
						WinAPI.SendMessage(_NextClipboardWindow, WM_DRAWCLIPBOARD, m.WParam, m.LParam);
					}
				} else
					base.WndProc(ref m);
			}

			private void OnChanged() {
				if (fChanged != null)
					fChanged(this, new EventArgs());
			}

			/**************************************************************/
			/*                       Public
                     /**************************************************************/

			public override void CreateHandle(CreateParams aCp) {
				base.CreateHandle(aCp);

				_NextClipboardWindow = WinAPI.SetClipboardViewer(Handle);
			}

			public override void DestroyHandle() {
				WinAPI.ChangeClipboardChain(Handle, _NextClipboardWindow);
				base.DestroyHandle();
			}

			/**************************************************************/
			/*                       Events
                     /**************************************************************/

			internal event EventHandler Changed {
				add {
					fChanged += value;
				}
				remove {
					fChanged -= value;
				}
			}
		}
#endregion

#region actions
		private IList<string> fClipboardList = new List<string>();
		private ClipboardHook fClipboardHook;

		private void ClipboardChanged(object aSender, EventArgs e) {
			IDataObject _Data = Clipboard.GetDataObject();

			if (_Data.GetDataPresent(DataFormats.Text)) {
				fClipboardList.Insert(0, _Data.GetData(DataFormats.Text).ToString());

				if (fClipboardList.Count > (int)(PropertyRegistry.Get("ClipboardStackSize", 10))) {
					fClipboardList.RemoveAt((int)(PropertyRegistry.Get("ClipboardStackSize", 10)));
				}
			}
		}

		/**************************************************************/
		/*                        Protected
              /**************************************************************/

		private void DoAppendToClipboard(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateSelectionExists()) {
				return;
			}

			MiscUtils.SetTextDataToClipboard(OtaUtils.GetSelectedText(OtaUtils.GetCurrentModule()) +
			                             MiscUtils.GetTextDataFromClipboard());
		}

		private void DoCopyFileNameToClipboard(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentSourceEditorNotNull()) {
				return;
			}

			MiscUtils.SetTextDataToClipboard(OtaUtils.GetCurrentEditorFileName());
		}

		private void DoCopyFilePathToClipboard(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentSourceEditorNotNull()) {
				return;
			}

			MiscUtils.SetTextDataToClipboard(Path.GetDirectoryName(OtaUtils.GetCurrentEditorFileName()));
		}

		private	FrmClipboardViewer _Frm;

		private void DoOpenClipboardViewer(object aSender, EventArgs AEventArgs) {
            try
			{
				if ((_Frm == null) || (_Frm.IsDisposed))
				{
					_Frm = new FrmClipboardViewer();
				}
				_Frm.Items = fClipboardList;
				_Frm.Show();
				_Frm.BringToFront();
            } catch (Exception ex) {
                Lextm.Windows.Forms.MessageBoxFactory.Fatal(ex);
			}
		}

        private void DoPasteAsString(object aSender, EventArgs AEventArgs)
        {
            if (!ValidationHelpers.ValidateCurrentModuleIsSourceFile())
            {
                return;
            }
            IOTAEditView _EditView = OtaUtils.GetCurrentEditView();
            if (_EditView == null)
            {
                LoggingService.Debug("null edit view");
                return;
            }
            IDataObject _Data = Clipboard.GetDataObject();
            if (!_Data.GetDataPresent(DataFormats.Text))
            {
                LoggingService.Debug("invalid content");
                return;
            }
            string _Text = _Data.GetData(DataFormats.Text).ToString();
            // Split in lines
            IList<string> _Lines = StringHelper.GetLinesFromString(_Text);
            BaseLanguageCodeHelper _LanguageCodeHelper = LanguageCodeHelperFactory.GetLanguageCodeHelper(OtaUtils.GetCurrentEditorFileName());
            _Lines = _LanguageCodeHelper.CreateString(_Lines);
            StringBuilder text = new StringBuilder();
            string _Prefix = new String(' ', _EditView.CursorPos.Col - 1);
            // Recreate the String
            for (int i = 0; i < _Lines.Count; i++)
            {
                if (i == 0)
                {
                    text.AppendLine(_Lines[i]);
                }
                else if (i == _Lines.Count - 1)
                {
                    text.Append(_Prefix + _Lines[i]);
                }
                else
                {
                    text.AppendLine(_Prefix + _Lines[i]);
                }
            }
            OtaUtils.InsertText(OtaUtils.GetCurrentSourceEditor(), text.ToString());
        }

        private void DoPasteCommented(object aSender, EventArgs AEventArgs)
        {
            if (!ValidationHelpers.ValidateCurrentModuleIsSourceFile())
            {
                return;
            }
            IOTAEditView _EditView = OtaUtils.GetCurrentEditView();
            if (_EditView == null)
            {
                LoggingService.Debug("null edit view");
                return;
            }
            IDataObject _Data = Clipboard.GetDataObject();
            if (!_Data.GetDataPresent(DataFormats.Text))
            {
                LoggingService.Debug("invalid content");
                return;
            }
            string _Text = _Data.GetData(DataFormats.Text).ToString();
            BaseLanguageCodeHelper _LanguageCodeHelper = LanguageCodeHelperFactory.GetLanguageCodeHelper(OtaUtils.GetCurrentEditorFileName());
            OtaUtils.InsertText(OtaUtils.GetCurrentSourceEditor(), _LanguageCodeHelper.CommentLine(_Text));
        }

        private void DoPasteKeepSelected(object aSender, EventArgs AEventArgs)
        {
            if (!ValidationHelpers.ValidateCurrentModuleIsSourceFile())
            {
                return;
            }
            IOTAEditView _EditView = OtaUtils.GetCurrentEditView();
            if (_EditView == null)
            {
                LoggingService.Debug("null edit view");
                return;
            }
            IDataObject _Data = Clipboard.GetDataObject();
            if (!_Data.GetDataPresent(DataFormats.Text))
            {
                LoggingService.Debug("invalid content");
                return;
            }
            IOTASourceEditor _SourceEditor = OtaUtils.GetCurrentSourceEditor();
            OTAEditPos _StartPos = OtaUtils.GetCurrentEditView().CursorPos;
            _EditView.Position.Paste();
            OTAEditPos _EndPos = OtaUtils.GetCurrentEditView().CursorPos;
            OtaUtils.SelectTextFromPosToPos(_SourceEditor, _StartPos, _EndPos);
        }
#endregion
		private const string MenuClipboard = "ClipboardMenu";
		
		protected override void IdeRegisterMenus() {
			// Clipboard  Viewer
			RegisterAction(CreateEmptyMenu(MenuItemLocation.Child,
			                               Lextm.OpenTools.ShareUtils.MenuRootDefault,
			                               MenuClipboard,
			                               "Clipboard"));
			RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                MenuClipboard,
			                                "ClipboardViewMenu",
			                                     0,
			                                     "Clipboard Viewer...",
			                                     new EventHandler(DoOpenClipboardViewer)));

			// Separator
			RegisterAction(CreateSeparatorMenu(MenuItemLocation.Child,
			                                      MenuClipboard));

			// Paste Commented code
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     MenuClipboard,
			                                     "PasteKeepSelectedMenu",
			                                     0,
			                                     "Paste Keep Selected",
			                                     new EventHandler(DoPasteKeepSelected)));

			// Paste Commented code
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     MenuClipboard,
			                                     "PasteCommentedMenu",
			                                     24662, // Ctrl Shift P
			                                     "Paste Commented",
			                                     new EventHandler(DoPasteCommented)));

			// Paste Commented code
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     MenuClipboard,
			                                     "PasteAsStringMenu",
			                                     0,
			                                     "Paste As String",
			                                     new EventHandler(DoPasteAsString)));

			// Send file path to clipboard
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     MenuClipboard,
			                                     "CopyFilePathToClipboardMenu",
			                                     0,
			                                     "Send folder path to clipboard",
			                                     new EventHandler(DoCopyFilePathToClipboard)));

			// Send file name to clipboard
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     MenuClipboard,
			                                     "CopyFileNameToClipboardMenu",
			                                     0,
			                                     "Send file path to clipboard",
			                                     new EventHandler(DoCopyFileNameToClipboard)));

			// Append selection to clipboard
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     MenuClipboard,
			                                     "AppendToClipboardMenu",
			                                     0,
			                                     "Append selection to clipboard",
			                                     new EventHandler(DoAppendToClipboard)));

			fClipboardHook = new ClipboardHook();
			fClipboardHook.Changed += new EventHandler(ClipboardChanged);
			fClipboardHook.Create();
		}

	}
}
