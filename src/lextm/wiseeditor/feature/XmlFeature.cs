using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Gui;
using BeWise.SharpBuilderTools.Helpers;
using Borland.Studio.ToolsAPI;
using Lextm.OpenTools;
using System.Collections.Generic;

namespace Lextm.WiseEditor.Feature
{

	public class XmlFeature : CustomFeature
	{

		/**************************************************************/
		/*                     Protected
                    /**************************************************************/
		#region actions
		private static void AddXmlValidationMessage(string aMessage)
		{
			IOTAMessageService _MessageService = OtaUtils.GetMessageService();
			OTAMessageKind _OTAMessageKind = OTAMessageKind.otamkInfo;
			IntPtr _Out;

			_MessageService.ShowMessageView(null);

			_MessageService.AddCompilerMessage("",
			                                   aMessage,
			                                   "Xml / Xsd Validation",
			                                   _OTAMessageKind,
			                                   0,
			                                   0,
			                                   new IntPtr(0),
			                                   out _Out);
		}

		private static void AddXmlValidationMessage(string aFileName,
		                                            string aMessage,
		                                            XmlSeverityType aXmlSeverityType,
		                                            int aLineNumber,
		                                            int aLinePosition)
		{

			IOTAMessageService _MessageService = OtaUtils.GetMessageService();
			OTAMessageKind _OTAMessageKind = OTAMessageKind.otamkWarn;
			IntPtr _Out;

			if (aXmlSeverityType == XmlSeverityType.Error)
			{
				_OTAMessageKind = OTAMessageKind.otamkError;
			}

			_MessageService.ShowMessageView(null);

			_MessageService.AddCompilerMessage(aFileName,
			                                   aMessage,
			                                   "Xml / Xsd Validation",
			                                   _OTAMessageKind,
			                                   aLineNumber,
			                                   aLinePosition,
			                                   new IntPtr(0),
			                                   out _Out);
		}

		private static void HandleXmlException(Exception ex, string fileName)
		{
            XmlException xmlException = ex as XmlException; 
			if (xmlException != null)
			{
				AddXmlValidationMessage(fileName,
				                        xmlException.Message,
				                        XmlSeverityType.Error,
				                        xmlException.LineNumber,
				                        xmlException.LinePosition);
			}
			else
			{
				Lextm.Windows.Forms.MessageBoxFactory.Fatal(ex);
			}
		}

		private static void ValidationHandler(object sender, ValidationEventArgs e)
		{
			AddXmlValidationMessage(OtaUtils.GetCurrentEditorFileName(),
			                        e.Message,
			                        e.Severity,
			                        e.Exception.LineNumber,
			                        e.Exception.LinePosition);
		}

		private static void ValidateXsdFile(string fileName)
		{
			FileStream _FileStream = File.OpenRead(fileName);

			try
			{
				AddXmlValidationMessage("Validating Xsd [" + fileName + "]");
				XmlSchema.Read(_FileStream, new ValidationEventHandler(ValidationHandler));
			}
			catch (Exception e)
			{
				HandleXmlException(e, fileName);
			}
		}

		private static void ValidateXmlFile(string fileName, string xsdFileName)
		{
			FileStream _FileStream = File.OpenRead(fileName);

			XmlTextReader _XmlTextReader = new XmlTextReader(_FileStream);
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.ValidationType = ValidationType.Schema;
			if (String.IsNullOrEmpty(xsdFileName))
			{
				settings.Schemas.Add(null, xsdFileName);
			}
			settings.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);
			XmlReader _XmlValidatingReader = XmlReader.Create(_XmlTextReader, settings);
			try
			{
				if (String.IsNullOrEmpty(xsdFileName))
				{
					AddXmlValidationMessage("Validating Xml [" + fileName + "] with " + xsdFileName);
				}
				else
				{
					AddXmlValidationMessage("Validating Xml [" + fileName + "]");
				}
				while (_XmlValidatingReader.Read())
				{}
			}
			catch (Exception e)
			{
				HandleXmlException(e, fileName);
			}
		}

//		private static void ValidateXmlFile(string aFileName, string aXsdFileName)
//		{
//			FileStream _FileStream = File.OpenRead(aFileName);
//
//			XmlTextReader _XmlTextReader = new XmlTextReader(_FileStream);
//			XmlValidatingReader _XmlValidatingReader = new XmlValidatingReader(_XmlTextReader);
//			if (String.IsNullOrEmpty(aXsdFileName))
//			{
//				_XmlValidatingReader.Schemas.Add(null, new XmlTextReader(aXsdFileName));
//			}
//			_XmlValidatingReader.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);
//			try
//			{
//				if (String.IsNullOrEmpty(aXsdFileName))
//				{
//					AddXmlValidationMessage("Validating Xml [" + aFileName + "] with " + aXsdFileName);
//				}
//				else
//				{
//					AddXmlValidationMessage("Validating Xml [" + aFileName + "]");
//				}
//				while (_XmlValidatingReader.Read())
//				{}
//			}
//			catch (Exception e)
//			{
//				HandleXmlException(e, aFileName);
//			}
//		}

		private static void ValidateXmlFile(string fileName)
		{
			ValidateXmlFile(fileName, null);
			//            FileStream _FileStream = File.OpenRead(aFileName);
//
			//            XmlTextReader _XmlTextReader = new XmlTextReader(_FileStream);
			//            XmlValidatingReader _XmlValidatingReader = new XmlValidatingReader(_XmlTextReader);
			//            _XmlValidatingReader.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);
			//            try {
			//                AddXmlValidationMessage("Validating Xml [" + aFileName + "]");
			//                while (_XmlValidatingReader.Read()){
			//                }
			//            } catch (Exception e){
			//                HandleXmlException(e, aFileName);
			//            }
		}

		/**************************************************************/
		/*                     Protected
                    /**************************************************************/

		private void DoValidateXsd(object sender, EventArgs e)
		{
			if (!ValidationHelpers.ValidateCurrentModuleIsXmlFile())
			{
				return;
			}

			ValidateXsdFile(OtaUtils.GetCurrentEditorFileName());
		}

		private void DoValidateXml(object sender, EventArgs e)
		{
			if (!ValidationHelpers.ValidateCurrentModuleIsXmlFile())
			{
				return;
			}

			ValidateXmlFile(OtaUtils.GetCurrentEditorFileName());
		}

		public const string XsdFileExtension = ".xsd";

		private void DoValidateXmlWithXsd(object sender, EventArgs e)
		{
			if (!ValidationHelpers.ValidateCurrentModuleIsXmlFile() ||
			    !ValidationHelpers.ValidateCurrentProjectNotNull())
			{
				return;
			}

			IList<string> _FileNames = OtaUtils.GetProjectFileList(XsdFileExtension);

			if (_FileNames.Count == 0)
			{
				Lextm.Windows.Forms.MessageBoxFactory.Info("No xsd file found in the current project");
				return;
			}

			FrmSelectXsdFileName _Frm = new FrmSelectXsdFileName(_FileNames);

			if (_Frm.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(_Frm.FileName))
			{
				string _XsdFileName = _Frm.FileName;

				ValidateXmlFile(OtaUtils.GetCurrentEditorFileName(), _XsdFileName);
			}
		}

		private void DoViewXml(object sender, EventArgs e)
		{
			if (!ValidationHelpers.ValidateCurrentModuleIsXmlFile())
			{
				return;
			}
			// TODO: make it non-modal.
			FrmViewXmlFile _Frm = new FrmViewXmlFile(OtaUtils.GetCurrentEditorFileName());
			_Frm.ShowDialog();
		}

		/*
                    protected void DoGenerateClasses(object aSender, EventArgs e) {

                    }
		 */
		#endregion
		/**************************************************************/
		/*                     Public
                    /**************************************************************/
		private const string MenuXml = "XmlSubMenu";

		protected override void IdeRegisterMenus()
		{
			// Xml
			RegisterAction(CreateEmptyMenu(MenuItemLocation.Child,
                                           ShareUtils.MenuRootDefault,
			                               MenuXml,
			                               "XML"));
			// View Xml
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     MenuXml,
			                                     "ViewXmlMenu",
			                                     0,
			                                     "View Xml...",
			                                     new EventHandler(DoViewXml)));

			// Validate Xsd
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     MenuXml,
			                                     "ValidateXsdMenu",
			                                     0,
			                                     "Validate Xsd...",
			                                     new EventHandler(DoValidateXsd)));

			// Validate Xml
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     MenuXml,
			                                     "ValidateXmlMenu",
			                                     0,
			                                     "Validate Xml...",
			                                     new EventHandler(DoValidateXml)));

			// Validate Xml with Xsd
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     MenuXml,
			                                     "ValidateXmlWithXsdMenu",
			                                     0,
			                                     "Validate Xml with Xsd...",
			                                     new EventHandler(DoValidateXmlWithXsd)));

			// Generate Class from Xsd
			/*
                              RegisterAction(CreateOTACustomAction(OTAMenuItemLocation.otamlChild,
                                                                   MenuXml,
                                                                   "GenerateClassesMenu",
                                                                   0,
                                                                   "Generate Classes ...",
                                                                   new EventHandler(DoGenerateClasses)));
			 */

		}
	}
}
