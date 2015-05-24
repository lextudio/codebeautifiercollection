// lextm: this is the file wizards class. Ported from SBT.
//   Used to add a few custom file wizards.
using System;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Wizard;
using Borland.Studio.ToolsAPI;
using Lextm.Diagnostics;
using Lextm.LeXDK;

namespace Lextm.WiseEditor.Feature {

    /// <summary>
    /// File wizards.
    /// </summary>
    /// <remarks>Some wizards are not valid now which will be fixd later.</remarks>
	public class FileWizardsFeature: CustomFeature {

        private static void AddFileWizards() {
          
            LoggingService.EnterMethod();
            try
            {
				int result;
				/*TODO : using some smart mechanism to do this. For example some foreach loop will be easy.*/
				IOTAGalleryCategoryManager _GalleryCategoryManager = OTAUtils.GetGalleryCategoryManager();
				if (_GalleryCategoryManager != null) {
					// not implemented in BDS 1.0
					//IOTAGalleryCategory _Cat = _GalleryCategoryManager.FindCategory(OTAGalleryCategories.sCategoryRoot);
					//LoggingService.AddDebug("root of gallery is " + _Cat.IDString);

					//IOTAGalleryCategory _Cat = _GalleryCategoryManager.FindCategory("Borland.Root");
					//IOTAGalleryCategory _HelpersCat = _GalleryCategoryManager.AddCategory(_Cat, "#Helpers", "#Helpers", IntPtr.Zero);
					
					IOTAGalleryCategory _HelpersCat = _GalleryCategoryManager.AddCategory("#Helpers", "#Helpers", IntPtr.Zero);
					if (_HelpersCat != null)
					{
						
						IOTAGalleryCategory _ProjectCat = _GalleryCategoryManager.AddCategory(_HelpersCat, "#Project.Helpers", "#Projects", IntPtr.Zero);
						IOTAGalleryCategory _FileCat = _GalleryCategoryManager.AddCategory(_HelpersCat, "#File.Helpers", "#Files", IntPtr.Zero);
						
						IOTAWizardService _WizardService = OTAUtils.GetWizardService();
						if (_WizardService != null) {
							// Empty Project
							EmptyProjectWizard _EmptyProjectWizard = new EmptyProjectWizard(_ProjectCat);
							result = _WizardService.AddWizard(_EmptyProjectWizard);
							//LoggingService.AddDebug(result);
							
							// Expert Project
							ExpertProjectWizard _ExpertProjectWizard = new ExpertProjectWizard(_ProjectCat);
							result = _WizardService.AddWizard(_ExpertProjectWizard);
							//LoggingService.AddDebug(result);
							
							// Windows Services Project
							WindowsServiceProjectWizard _WindowsServiceProjectWizard = new WindowsServiceProjectWizard(_ProjectCat);
							result = _WizardService.AddWizard(_WindowsServiceProjectWizard);
							//LoggingService.AddDebug(result);
							
							// Cs File
							CsFileWizard _CsFileWizard = new CsFileWizard(_FileCat);
							result = _WizardService.AddWizard(_CsFileWizard);
							//LoggingService.AddDebug(result);
							
							// Strong Name File
							StrongNameFileWizard _StrongNameFileWizard = new StrongNameFileWizard(_FileCat);
							result = _WizardService.AddWizard(_StrongNameFileWizard);
							//LoggingService.AddDebug(result);
							
							// SQL File
							SqlFileWizard _SqlFileWizard = new SqlFileWizard(_FileCat);
							result = _WizardService.AddWizard(_SqlFileWizard);
							//LoggingService.AddDebug(result);
							
							// StyleSheet File
							StyleSheetFileWizard _StyleSheetFileWizard = new StyleSheetFileWizard(_FileCat);
							result = _WizardService.AddWizard(_StyleSheetFileWizard);
							//LoggingService.AddDebug(result);
							
							// Xml File
							XmlFileWizard _XmlFileWizard = new XmlFileWizard(_FileCat);
							result = _WizardService.AddWizard(_XmlFileWizard);
							//LoggingService.AddDebug(result);
							
							// Xsd File
							XsdFileWizard _XsdFileWizard = new XsdFileWizard(_FileCat);
							result = _WizardService.AddWizard(_XsdFileWizard);
							//LoggingService.AddDebug(result);
							
							// Xslt File
							XsltFileWizard _XsltFileWizard = new XsltFileWizard(_FileCat);
							result = _WizardService.AddWizard(_XsltFileWizard);
							//LoggingService.AddDebug(result);
						}
						else {
							LoggingService.Warn("null wizard service");
						}
					} else {
						LoggingService.Warn("Fails to add a category");
					}
				}
				else {
					LoggingService.Warn("null gallery service");
				}                        	
            }
            catch
            {
				LoggingService.Debug("fail to add file wizards");
            }
			
            
            LoggingService.LeaveMethod();
           
        }
        
        /// <summary>
        /// Registers wizards.
        /// </summary>
        protected override void IDERegisterWizards() {
            base.IDERegisterWizards();
            AddFileWizards();
        }
    }
}

