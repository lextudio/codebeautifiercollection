using System;
using System.Windows.Forms;
using System.IO;
using Borland.Studio.ToolsAPI;
using BeWise.Common.Utils;
using Lextm.CBC.Gui;
using System.Xml;
using System.Text;

namespace Lextm.CBC {
public class OTAJediCodeFormat : BaseOTA {

		private static FrmCodeBeautifier fFrmCodeBeautifier = null;

        /**************************************************************/
        /*                        Protected
        /**************************************************************/
        
        protected void CodeBeautifyForCFile() {
            // Validations
            IOTAModule _Module = OTAUtils.GetCurrentModule();
            IOTAEditor _Editor = OTAUtils.GetEditorWithSourceEditor(_Module);
            FileInfo _FileInfo = new FileInfo(_Module.FileName);
            
			if ((Main.Configuration.AStylePath == "") || (!File.Exists(GetAStyleExe()))) {
                MessageBox.Show("Please configure correctly AStyle Executable!", Consts.MESSAGE_BOX_TITLE);
                return ;
            }
            if ((_FileInfo.Attributes & FileAttributes.ReadOnly) ==
                    FileAttributes.ReadOnly) {
                MessageBox.Show("Can't beautify read only file !");
                return ;
            }
			if (_Editor.IsModified) {
                MessageBox.Show("Save your file before applying the code beautifier !");
                return ;
            }
            
            IOTASourceEditor _SourceEditor = OTAUtils.GetCurrentSourceEditor();
            
            fFrmCodeBeautifier = new FrmCodeBeautifier();
            fFrmCodeBeautifier.Show();
            
            RunProcess _RP = new RunProcess();
            string[] _Arr = new string[2];
            
            Application.DoEvents();
            
            _Arr[0] = GetAStyleExe();
            _Arr[1] = Main.Configuration.AStyleParams + @" """ + _Module.FileName + @"""";
            
            _RP.OnRunCompleted += new EventHandler(DoRunCompleted);
            _RP.Run(_Arr, Path.GetDirectoryName(_Arr[0]));
            
            IOTAActionService _ActionService = (IOTAActionService) BorlandIDE.GetService(typeof(IOTAActionService));
            _ActionService.ReloadFile(_Module.FileName);
        }
        
        protected void CodeBeautifyForDelphiFile() {
            // Validations
            IOTAModule _Module = OTAUtils.GetCurrentModule();
            IOTAEditor _Editor = OTAUtils.GetEditorWithSourceEditor(_Module);
            FileInfo _FileInfo = new FileInfo(_Module.FileName);
            
			if ((Main.Configuration.JcfPath == "") || (!File.Exists(GetJcfExe()))) {
                MessageBox.Show("Please configure correctly JCF Executable!", Consts.MESSAGE_BOX_TITLE);
                return ;
            }
            if ((_FileInfo.Attributes & FileAttributes.ReadOnly) ==
                    FileAttributes.ReadOnly) {
                MessageBox.Show("Can't beautify read only file !");
                return ;
            }
			if (_Editor.IsModified) {
                MessageBox.Show("Save your file before applying the code beautifier !");
                return ;
            }
            
            IOTASourceEditor _SourceEditor = OTAUtils.GetCurrentSourceEditor();
            
            fFrmCodeBeautifier = new FrmCodeBeautifier();
            fFrmCodeBeautifier.Show();
            
            RunProcess _RP = new RunProcess();
            string[] _Arr = new string[2];
            
            Application.DoEvents();
            
            _Arr[0] = GetJcfExe();
            _Arr[1] = Main.Configuration.JcfParams + @" """ + _Module.FileName + @"""";
            
            _RP.OnRunCompleted += new EventHandler(DoRunCompleted);
            _RP.Run(_Arr, Path.GetDirectoryName(_Arr[0]));
            
            IOTAActionService _ActionService = (IOTAActionService) BorlandIDE.GetService(typeof(IOTAActionService));
            _ActionService.ReloadFile(_Module.FileName);
        }
        
        protected void CodeBeautifyForXmlFile() {
            // Validations
            IOTAModule _Module = OTAUtils.GetCurrentModule();
            IOTAEditor _Editor = OTAUtils.GetEditorWithSourceEditor(_Module);
            FileInfo _FileInfo = new FileInfo(_Module.FileName);
            
            if ((_FileInfo.Attributes & FileAttributes.ReadOnly) ==
                    FileAttributes.ReadOnly) {
                MessageBox.Show("Can't beautify read only file !");
                return ;
            }

            XmlDocument _XmlDocument = new XmlDocument();
            _XmlDocument.Load(OTAUtils.GetCurrentModule().FileName);
            MemoryStream _MemoryStream = new MemoryStream();
            try {
                XmlTextWriter _XmlTextWriter = new XmlTextWriter(_MemoryStream, new ASCIIEncoding());
                try {
                    _XmlTextWriter.Formatting = Formatting.Indented;
                    _XmlTextWriter.Indentation = Main.Configuration.XmlIndentation;
                    _XmlTextWriter.IndentChar = ' ';
                    _XmlDocument.Save(_XmlTextWriter);
                    _MemoryStream.Position = 0;
                    StreamReader _StreamReader = new StreamReader(_MemoryStream);
                    try {
                        string _Result = _StreamReader.ReadToEnd();
                        OTAUtils.FillBufferFromString(OTAUtils.GetCurrentSourceEditor(), _Result);
                    }
                    finally {
                        _StreamReader.Close();
                    }
                }
                finally {
                    _XmlTextWriter.Close();
                }
            }
            finally {
                _MemoryStream.Close();
            }
        }
        
        
        protected void DoCodeFormat(object aSender, EventArgs AEventArgs) {
            if (!( (OTAUtils.CurrentModuleIsDelphiFile()) ||
					(Lextm.JcfExpert.Utils.OTAUtils.CurrentModuleIsCFile() ||
                     (OTAUtils.CurrentModuleIsXmlFile())
                    ))) {
                MessageBox.Show("JEDI Code Format only apply to Delphi/C/C++/C#/XML files !", Consts.MESSAGE_BOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
			if (Lextm.JcfExpert.Utils.OTAUtils.CurrentModuleIsCFile()) {
                CodeBeautifyForCFile();
            }
            else if (OTAUtils.CurrentModuleIsDelphiFile()) {
                CodeBeautifyForDelphiFile();
            }
            else {
                CodeBeautifyForXmlFile();
            }
        }

		protected static void DoRunCompleted(object aSender, EventArgs e) {
            if (fFrmCodeBeautifier != null) {
                fFrmCodeBeautifier.Hide();
                fFrmCodeBeautifier = null;
            }
        }
        
        /**************************************************************/
        /*                        Public
        /**************************************************************/
        
        public override void IDERegister(out string[] aMenuNames, out int[] aMenuShortCuts) {
            IOTAMainMenuService _MenuService = null;
            IOTAMenuItem _MenuItem = null;
            
            _MenuService = (IOTAMainMenuService) BorlandIDE.GetService(typeof(IOTAMainMenuService));
            
            // Sep
            _MenuItem = _MenuService.AddMenuItem(Consts.CBC_MENU_NAME, OTAMenuItemLocation.otamlChild, "SepBeautifierMenu2", "-");
            
            // Code Beautifier
            _MenuItem = _MenuService.AddMenuItem(Consts.CBC_MENU_NAME, OTAMenuItemLocation.otamlChild, "JEDICodeFormatMenu", "Beautifiers");
            _MenuItem.Enabled = true;
            _MenuItem.Executed += new EventHandler(DoCodeFormat);
            
            aMenuNames = new string[] {"JEDICodeFormatMenu"
                                      };
            aMenuShortCuts = new int[] {
                                 16452
                             }
                             ; // Ctrl D
        }
        
        protected string GetJcfExe() {
            string _Path = Main.Configuration.JcfPath;
            
            if (_Path[_Path.Length - 1] != '\\') {
                _Path += "\\" ;
            }
            
            _Path += Consts.JCF_EXE_NAME;
            
            return _Path;
        }
        
        protected string GetAStyleExe() {
            string _Path = Main.Configuration.AStylePath;
            
            if (_Path[_Path.Length - 1] != '\\') {
                _Path += "\\" ;
            }
            
            _Path += Consts.ASTYLE_EXE_NAME;
            
            return _Path;
        }
    }
}
