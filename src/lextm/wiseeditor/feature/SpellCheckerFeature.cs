using System;
using System.IO;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;
using Borland.Studio.ToolsAPI;
using Lextm.OpenTools;

namespace Lextm.WiseEditor.Feature {

	public class SpellcheckerFeature : CustomFeature {

        /**************************************************************/
        /*                        Private
        /**************************************************************/

        private NetSpell.SpellChecker.Spelling spelling = new NetSpell.SpellChecker.Spelling();

        private void SpellingDoubledWord(object sender, NetSpell.SpellChecker.SpellingEventArgs args) {
            // Nothing to do
        }

        private void SpellingEndOfText(object sender, System.EventArgs args) {
            IOTASourceEditor _SourceEditor = OtaUtils.GetCurrentSourceEditor();

            IOTAEditView _EditView = OtaUtils.GetCurrentEditView();

            OTAEditPos _StartEditPos = _EditView.CursorPos;
            OtaUtils.ReplaceSelectedText(_SourceEditor, spelling.Text);
            OTAEditPos _EndEditPos = _EditView.CursorPos;

            _EditView.CursorPos = _StartEditPos;

            _EditView.Block.BeginBlock();
            _EditView.Block.Style = OTABlockType.btNonInclusive;

            _EditView.CursorPos = _EndEditPos;
            _EditView.MoveViewToCursor();
            _EditView.Block.EndBlock();

        }

        private void SpellingMisspelledWord(object sender, NetSpell.SpellChecker.SpellingEventArgs args) {
            // Nothing to do
        }

        /**************************************************************/
        /*                        Protected
        /**************************************************************/

        private void DoSpellCheck(object aSender, EventArgs AEventArgs) {
        	//TODO: spend time on the file type checker structure. It may be a pattern.
        	if (!ValidationHelpers.ValidateCurrentModuleIsSourceFile())
        	{
        		return;
        	}
            if (ValidationHelpers.ValidateSelectionExists()) {
        		spelling.SpellCheck(OtaUtils.GetSelectedText(OtaUtils.GetCurrentModule()));
        	} else {
        		spelling.SpellCheck(OtaUtils.GetCurrentSourceEditorText());            	
        	}
        }

        /**************************************************************/
        /*                     Public
        /**************************************************************/

        protected override void IdeRegisterMenus() {

			string _Dir = OpenTools.IO.Path.DataFolder;

            spelling.IgnoreAllCapsWords = true;
            spelling.IgnoreHtml = true;
            spelling.IgnoreWordsWithDigits = false;
            spelling.MaxSuggestions = 25;
            spelling.Dictionary.DictionaryFolder = _Dir;
            spelling.Dictionary.DictionaryFile= "en-US.dic";
            spelling.Dictionary.UserFile = "user.dic";

            spelling.MisspelledWord += new NetSpell.SpellChecker.Spelling.MisspelledWordEventHandler(this.SpellingMisspelledWord);
            spelling.EndOfText += new NetSpell.SpellChecker.Spelling.EndOfTextEventHandler(this.SpellingEndOfText);
            spelling.DoubledWord += new NetSpell.SpellChecker.Spelling.DoubledWordEventHandler(this.SpellingDoubledWord);

            // Spell Check
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
                                                 ShareUtils.MenuRootDefault,
                                                 "SpellCheckMenu",
                                                 0,
                                                 "Spell Check...",
                                                 new EventHandler(DoSpellCheck)));
        }
    }
}
