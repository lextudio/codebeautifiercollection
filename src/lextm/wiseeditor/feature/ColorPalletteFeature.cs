using System;
using System.Windows.Forms;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;
using Borland.Studio.ToolsAPI;
using Lextm.OpenTools;

namespace Lextm.WiseEditor.Feature {

    public class ColorPalletteFeature : CustomFeature {
		
        private void DoColorCodeWizard(object aSender, EventArgs e) {
            if (ValidationHelpers.ValidateCurrentModuleIsCSFile())
            {
                ColorDialog _ColorDialog = new ColorDialog();

                if (_ColorDialog.ShowDialog() == DialogResult.OK)
                {
                    string _Color;

                    if (_ColorDialog.Color.IsKnownColor)
                    {
                        _Color = "Color." + _ColorDialog.Color.ToKnownColor();
                    }
                    else
                    {
                        _Color = "Color.FromArgb(((byte)(" + _ColorDialog.Color.R +
                                  ")), ((byte)(" + _ColorDialog.Color.G +
                                  ")), ((byte)(" + _ColorDialog.Color.B +
                                  ")));";
                    }

                    IOTASourceEditor _SourceEditor = OtaUtils.GetCurrentSourceEditor();
                    OtaUtils.ReplaceSelectedText(_SourceEditor, _Color);

                    IOTAEditView _EditView = OtaUtils.GetEditView(_SourceEditor);
                    _EditView.MoveViewToCursor();
                    _EditView.Paint();
                }
            }
        }

        protected override void IdeRegisterMenus() {
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
                               ShareUtils.MenuRootDefault,
                               "ColorCodeWizardMenu",
                               0,
                               "Color Pallette Helper",
                               new EventHandler(DoColorCodeWizard)));        
        }

    }
}
