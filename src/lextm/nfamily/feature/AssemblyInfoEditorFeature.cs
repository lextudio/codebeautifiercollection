using System;
using System.Windows.Forms;
using BeWise.SharpBuilderTools.Gui.AssemblyInfo;
using BeWise.SharpBuilderTools.Tools.AssemblyInfo;
using Lextm.OpenTools;
using System.Collections.Generic;

namespace Lextm.NFamily.Feature {

	public class AssemblyInfoEditorFeature : CustomFeature {
		/// <summary>
		/// Constructor.
		/// </summary>
		public AssemblyInfoEditorFeature() {		}

        private static List<string> fMessages = new List<string>();

        private static void MessageEvent(object aSender, string aMessage) {
            fMessages.Add(aMessage);
        }

        /**************************************************************/
        /*                       Protected
        /**************************************************************/
		private void DoAssemblyInfoEditor(object aSender, EventArgs aEventArgs) {
            FrmAssemblyInfoEditor _Frm = new FrmAssemblyInfoEditor();
            _Frm.ShowDialog();
        }

        /**************************************************************/
        /*                        Public
        /**************************************************************/

        internal static void Generate(string[] aFiles) {
            FrmAssemblyInfoBuilderRunner _Frm = new FrmAssemblyInfoBuilderRunner();

            if (_Frm.ShowDialog() == DialogResult.OK) {
                AssemblyInfoCreator _AssemblyInfoCreator = new AssemblyInfoCreator();
                _AssemblyInfoCreator.MessageEvent += new AssemblyInfoCreator.AssemblyInfoCreatorMessageEvent(MessageEvent);

                fMessages.Clear();
                _AssemblyInfoCreator.CreateAssemblyInfo(_Frm.Language, aFiles);
                FrmAssemblyInfoMessage _FrmAssemblyInfoMessage = new FrmAssemblyInfoMessage(fMessages);
                _FrmAssemblyInfoMessage.ShowDialog();
            }
        }

        protected override void IdeRegisterMenus() {
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
                                                 ShareUtils.MenuRootDefault,
                                                 "AssemblyInfoEditorMenu",
                                                 0,
                                                 "AssemblyInfo Editor...",
                                                 new EventHandler(DoAssemblyInfoEditor)));
        }
    }
}
