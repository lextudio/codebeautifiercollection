// lextm: this is the help wizard class. Ported from SBT.
//   Used to show a link to my blog, the manual, and so on.
// Copyright (C) 2005-2006  Lex Mark
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
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Wizard;
using Borland.Studio.ToolsAPI;
using Lextm.Diagnostics;
using Lextm.LeXDK;

namespace Lextm.Utilities.Feature
{

    /// <summary>
    /// Help wizards.
    /// </summary>
    public class HelpWizards: CustomFeature
    {
        /// <summary>
        /// Registers wizards.
        /// </summary>
        protected override void IDERegisterWizards()
        {
            base.IDERegisterWizards();
            AddHelpWizards();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public HelpWizards( )      { }

        private static void AddHelpWizards()
        {
            IOTAWizardService _WizardService = OTAUtils.GetWizardService();
            if (_WizardService != null)
            {
                _WizardService.AddWizard(
                    new WebsiteWizard("CBC.WebSite4",
                                      "Report A Bug of CBC",
									  new System.Uri("http://gforge.oss.org.cn/tracker/?func=add&group_id=39&atid=257"),
                                      "Report A Bug of CBC"));
                _WizardService.AddWizard(
                    new WebsiteWizard("CBC.WebSite1",
                                      "Lex Mark's Blog",
									  new System.Uri("http://lextm.blogspot.com/"),
                                      "Lex Mark's Blog"));
            	string homepage = (string)PropertyRegistry.Get("CBCHomepage", "http://gforge.oss.org.cn/projects/codebeautifiers/");
                _WizardService.AddWizard(
                    new WebsiteWizard("CBC.WebSite2",
                                      "Code Beautifier Collection Web Site",
                                      new System.Uri(homepage),
									  "Code Beautifier Collection Home Page"));

                _WizardService.AddWizard(
                    new HelpWizard("CBC.HelpWizard",
            		               LeXDK.IO.Path.GetDocFile("Manual.pdf"),
                                   "Code Beautifier Collection Help",
                                   "Code Beautifier Collection Help"));
                _WizardService.AddWizard(
                    new HelpWizard("CBC.HelpWizard2",
            		               LeXDK.IO.Path.GetDocFile("LeXDK.pdf"),
                                   "LeXDK Developer's Guide",
                                   "LeXDK Developer's Guide"));
                //_WizardService.AddWizard(new ShortCutHelpWizard());

            }
            else
            {
                LoggingService.Warn("null wizard service.");
            }
        }
    }
}


