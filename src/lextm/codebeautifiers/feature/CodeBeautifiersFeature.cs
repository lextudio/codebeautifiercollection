// This is the ota codebeautifiers class. Ported from SBT.
//  JCF and XMLLex supports are added. Implementation is changed.
// Copyright (C) 2005-2006  Lex Y. Li
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
using System;
using System.IO;
using System.Windows.Forms;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;
using Borland.Studio.ToolsAPI;
using Lextm.OpenTools;
using Lextm.Diagnostics;
using System.Globalization;
using Lextm.OpenTools.Elements;

namespace Lextm.CodeBeautifiers.Feature
{

    /// <summary>
    ///Beautifiers feature.
    /// </summary>
    /// <remarks>
    /// This serves as a demo of my OTA model - an improved SBT version.
    /// Every feature in this model should be derived from CustomOtaFeature class.
    /// You can choose a unique name of your OTA.
    /// Normally this name is used as file name of preferences file of your class.
    /// </remarks>
    public sealed class CodeBeautifiersFeature : CustomFeature
    {

        private const int DefaultFileShortcut = ShareUtils.Ctrl + (int)Keys.W;

        private const string MenuBeautify = "CBCExpertBeautifyMenu";
        private const string MenuBeautifyFile = "CodeBeautifiersFileMenu";
        //private const string MenuBeautifyGroup = "CodeBeautifiersGroupMenu";
        //private const string MenuBeautifyProject = "CodeBeautifiersProjectMenu";
        private const string MenuLeXtudio = "CBCLeXtudioMenu";

        private const string MenuTextBeautify = "Beautify";
        private const string MenuTextBeautifyFile = "Document";
        //private const string MenuTextBeautifyGroup = "Group";
        //private const string MenuTextBeautifyProject = "Project";

        private const string TabAStyle = "C/C++/C#";
        private const string TabJcf = "Object Pascal";
        private const string TabCBGeneral = "Beautifiers";

        #region Preferences
        ///<summary>
        ///Sets default preferences.
        ///</summary>
        public override void SetDefaultPreferences()
        {

            LoggingService.EnterMethod();

            base.SetDefaultPreferences();
            PropertyRegistry.Get("AStyleStyle", AStyleStyle.KernighanRitchie);
            PropertyRegistry.Get("AStyleParams", DefaultAStyleParams);
            PropertyRegistry.Get("AStyleConvertTabs", true);
            PropertyRegistry.Get("AStyleIndentNamespace", true);
            PropertyRegistry.Get("AStyleIndentPreprocessor", true);
            PropertyRegistry.Get("JcfParams", DefaultJcfParams);
            PropertyRegistry.Get("JcfSettingsFileName", DefaultSettingsFileName);
            PropertyRegistry.Get("JcfStyle", JcfStyle.Borland);

            if (String.IsNullOrEmpty(PropertyRegistry.Get("AStyleParams") as string))
            {
                PropertyRegistry.Set("AStyleParams", DefaultAStyleParams);
            }

            if (String.IsNullOrEmpty(PropertyRegistry.Get("JcfParams") as string))
            {
                PropertyRegistry.Set("JcfParams", DefaultJcfParams);
            }

            if (String.IsNullOrEmpty((string)PropertyRegistry.Get("JcfSettingsFileName")))
            {
                PropertyRegistry.Set("JcfSettingsFileName",
                                    DefaultSettingsFileName);
            }
            PropertyRegistry.Flush();
            LoggingService.LeaveMethod();

        }
        private const string DefaultAStyleParams = "--style=kr --indent-namespaces --convert-tabs --indent-preprocessor";
        private readonly static string DefaultJcfParams =
            String.Format(CultureInfo.InvariantCulture, "-config={0} -clarify -inplace -f -y", //"-config={0} -clarify -backup -f -y",
                          StringHelper.QuoteString(
                            Lextm.OpenTools.IO.Path.DefaultConfigFile));
        private readonly static string DefaultSettingsFileName =
            Lextm.OpenTools.IO.Path.DefaultConfigFile;
        #endregion
        ///<summary>
        ///Registers tabs.
        ///</summary>
        ///<remarks>
        /// Used to configure tabs on FormPreferences.
        ///</remarks>
        protected override void IdeRegisterTabs()
        {
            base.IdeRegisterTabs();
            // parent tab should be coming first.
            //RegisterTab(CreateTabNode(TabCBGeneral, typeof(Gui.CodeBeautifiersGeneralPage)));
            RegisterTab(new TabNode(TabCBGeneral));
            RegisterTab(CreateTabNode(TabCBGeneral, TabAStyle, typeof(Gui.AStylePage)));
            RegisterTab(CreateTabNode(TabCBGeneral, TabJcf, typeof(Gui.JcfPage)));
        }

        /// <summary>
        /// Registers actions.
        /// </summary>
        /// <remarks>
        /// Used to configure actions.
        /// Normally actions are translated to menus by CBC.
        /// </remarks>
        protected override void IdeRegisterMenus()
        {
            base.IdeRegisterMenus();
            // Beautify
            RegisterMenu(CreateEmptyMenu(
                             MenuItemLocation.Child,
                             MenuLeXtudio,
                             MenuBeautify,
                             MenuTextBeautify));

            // Project group
            //            RegisterMenu(CreateActionMenu(MenuItemLocation.Child,
            //                                          MenuBeautify,
            //                                          MenuBeautifyGroup,
            //                                          0,
            //                                          MenuTextBeautifyGroup,
            //                                          new EventHandler(DoCodeBeautifyProjectGroup)));
            // Project
            //            RegisterMenu(CreateActionMenu(MenuItemLocation.Child,
            //										  MenuBeautify,
            //										  MenuBeautifyProject,
            //										  0,
            //                                          MenuTextBeautifyProject,
            //										  new EventHandler(DoCodeBeautifyProject)));
            // File
            RegisterMenu(CreateActionMenu(MenuItemLocation.Child,
                                          MenuBeautify,
                                          MenuBeautifyFile,
                                          DefaultFileShortcut,
                                          MenuTextBeautifyFile,
                                          new EventHandler(DoCodeBeautifyFile)));

        }

        //
        // This region contains function related to actions.
        //
        #region RegisterActions
        //        private void DoCodeBeautifyProjectGroup(object sender, EventArgs e) {
        //          
        //            LoggingService.EnterMethod();
        //
        //            if (!ValidationHelpers.ValidateCurrentProjectGroupNotNull()) {
        //				LoggingService.LeaveMethod();
        //                return ;
        //            }
        //
        //            Gui.FormCodeBeautifiers.SetProgress(0, 1);
        //            Gui.FormCodeBeautifiers.getInstance().Show();
        //
        //            IOTAProjectGroup _Group = OTAUtils.GetCurrentProjectGroup();
        //            for (int j = 0; j < _Group.ProjectCount; j++) {
        //                IOTAProject _Project = _Group[j];
        //				for (int i = 0; i < _Project.ModuleCount; i++) {
        //                    IOTAModuleInfo _Info = _Project.GetModuleInfo(i);
        //                    
        //					//IO.CustomFileCreator.BeautifyFile(_Info.FileName);
        //                    // CodeBeautifyOneFile(_Project.GetModuleFileName(i)); //unavailable in ToolsAPI 7.1.
        //                }
        //                // use the percentage of completed projects.
        //				Gui.FormCodeBeautifiers.SetProgress(j, _Group.ProjectCount);
        //            }
        //
        //            Gui.FormCodeBeautifiers.getInstance().Hide();
        //
        //            LoggingService.LeaveMethod();
        //          
        //        }

        //        private void DoCodeBeautifyProject(object sender, EventArgs e) {
        //          
        //            LoggingService.EnterMethod();
        //
        //            if (!ValidationHelpers.ValidateCurrentProjectNotNull()) {
        //                LoggingService.LeaveMethod();
        //               
        //                return ;
        //            }
        //
        //			Gui.FormCodeBeautifiers.SetProgress(0, 1);
        //            Gui.FormCodeBeautifiers.getInstance().Show();
        //
        //            IOTAProject _Project = OTAUtils.GetCurrentProject();
        //            for (int i = 0; i < _Project.ModuleCount; i++) {
        //                IOTAModuleInfo _Info = _Project.GetModuleInfo(i);
        //                
        //				//IO.CustomFileCreator.BeautifyFile(_Info.FileName);
        //                // use percentage of completed files.
        //				Gui.FormCodeBeautifiers.SetProgress(i, _Project.ModuleCount);
        //				// CodeBeautifyOneFile(_Project.GetModuleFileName(i)); // unavailable only in ToolsAPI 7.1.
        //            }
        //
        //            Gui.FormCodeBeautifiers.getInstance().Hide();
        //
        //            LoggingService.LeaveMethod();
        //          
        //        }

        private void DoCodeBeautifyFile(object sender, EventArgs e)
        {

            LoggingService.EnterMethod();

            IOTAModule _Module = OtaUtils.GetCurrentModule();
            IOTAEditor _Editor = OtaUtils.GetEditorWithSourceEditor(_Module);

            if (!ValidationHelpers.ValidateCustom(!OtaUtils.GetDesignerIsActive(_Module),
                                                  "Can't execute the code beautifier with the Form Designer!") ||
                    !ValidationHelpers.ValidateCustom((_Editor != null),
                                                      "No current file !"))
            {
                LoggingService.LeaveMethod();

                return;
            }
            FormatModule();
            //            try {
            //                string tip = "Ctrl + W is the shortcut.";
            //                Lextm.Windows.Forms.AlertForm alert = new Lextm.Windows.Forms.AlertForm(tip);
            //
            //                alert.Show();
            //            } catch (Exception ex) {
            //                MessageBo/x.Show(ex.Message + " : " + ex.StackTrace);
            //            }

            LoggingService.LeaveMethod();

        }
        #endregion
        //
        // This OTA constructor should be public.
        //
        /// <summary>
        /// Constructor.
        /// </summary>
        public CodeBeautifiersFeature() { }

        private static void FormatModule()
        {
            Gui.FormCodeBeautifiers.SetProgress(0, 1);
            Gui.FormCodeBeautifiers.getInstance().Show();

            IOTASourceEditor _SourceEditor = OtaUtils.GetCurrentSourceEditor();
            string _SourceFile = OtaUtils.GetCurrentEditorFileName();
            string _TempFile = Path.Combine(Path.GetTempPath(), Path.GetFileName(_SourceFile));
            OtaUtils.CreateFileFromBuffer(_SourceEditor, _TempFile);

            // this percentage is meaningless.
            Gui.FormCodeBeautifiers.SetProgress(1, 2);
            BaseLanguageCodeHelper helper = LanguageCodeHelperFactory.GetLanguageCodeHelper(_TempFile);
            helper.BeautifyFile(_TempFile);

            //IO.CustomFileCreator.BeautifyFile(_TempFile);
            OtaUtils.FillBufferFromFile(_SourceEditor, _TempFile);

            Gui.FormCodeBeautifiers.SetProgress(1, 1);
            System.IO.File.Delete(_TempFile);
            System.IO.File.Delete(_TempFile + ".orig");
            Gui.FormCodeBeautifiers.getInstance().Hide();
        }
    }
}