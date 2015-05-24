using System;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Xml.Serialization;
using Borland.Studio.ToolsAPI;
using System.Drawing;
using Lextm.JcfExpert.Common;

namespace Lextm.JcfExpert {
    public class Main {
        /**************************************************************/
        /*                     Private
        /**************************************************************/
        private static Configuration fConfiguration;
        private static ArrayList fMenuNames = new ArrayList();
        private static ArrayList fMenuShortCuts = new ArrayList();
        private static Timer fTimer = new Timer();
        private static int fTimerRetry = 20;
        private static bool fShortcutEnabled = false;

        /**************************************************************/
        /*                     Protected
        /**************************************************************/

        protected static void CreateJcfExpertMenu() {
            IOTAMainMenuService _MenuService = null;
            IOTAMenuItem _MenuItem = null;

            _MenuService = (IOTAMainMenuService) BorlandIDE.GetService(typeof(IOTAMainMenuService));
            _MenuItem = _MenuService.AddMenuItem("ToolsMenu",
                                                 OTAMenuItemLocation.otamlBefore,
                                                 Consts.JCF_Expert_MENU_NAME,
                                                 "JCFExpert");

            _MenuItem.Enabled = true;

            _MenuItem.Enabled = true;
        }

        protected static void DoSetShortCut(object sender, EventArgs e) {
            if (!fShortcutEnabled) {
                fShortcutEnabled = true;

                IOTAMainMenuService _MenuService = null;
                IOTAMenuItem _MenuItem = null;

                _MenuService = (IOTAMainMenuService) BorlandIDE.GetService(typeof(IOTAMainMenuService));

                _MenuItem = _MenuService.GetMenuItem(Consts.JCF_Expert_MENU_NAME);
                _MenuItem.Enabled = true;

                for (int i = 0; i < fMenuNames.Count; i++) {
                    _MenuItem = _MenuService.GetMenuItem((string) fMenuNames[i]);

                    if ((_MenuItem != null) && ((int) fMenuShortCuts[i] != 0)) {
                        _MenuItem.Shortcut = (int) fMenuShortCuts[i];
                    }
                }
            }
        }

        protected static void RegisterMenus(string[] aMenuNames, int[] aMenuShortCuts) {
            for (int i = 0; i < aMenuNames.Length; i++) {
                fMenuNames.Add(aMenuNames[i]);
                fMenuShortCuts.Add(aMenuShortCuts[i]);
            }
        }

        protected static void DoTimer(object sender, EventArgs e) {
            fTimerRetry--;

            // Stop the timer
            if (fTimerRetry <= 0) {
                fTimer.Enabled = false;
            }

            DoSetShortCut(null, null);
        }

		protected static void RegisterAboutBox() {
			IOTAAboutBoxService _AboutBoxService = (IOTAAboutBoxService) BorlandIDE.GetService(typeof(IOTAAboutBoxService));
			Bitmap aBmp = ((Bitmap)(Resources.MyRes.GetObject("JCF36.bmp")));
            aBmp.MakeTransparent(Color.White);
            _AboutBoxService.AddPluginInfo(
				"JCF Integration Expert 1.0 for Delphi 2005",//Title
				" JCF Integration Expert 1.0 (2005/March/16) - \r\n"+
                " add-in for Borland Delphi 2005\r\n"+
                " from Lex Mark (lextm)\r\n :)",//Description
                (aBmp).GetHbitmap(),
                false,//isUnregistered
                "",//LicenseStatus
                "");//SkuName
        }

        protected static void RegisterSplashScreen() {
            IOTASplashScreenService splashServ = BorlandIDE.SplashScreenService;
			Bitmap sBmp = ((Bitmap)(Resources.MyRes.GetObject("JCF24.bmp")));
            sBmp.MakeTransparent(Color.White);

            splashServ.AddPluginBitmap(
				"JCF Integration Expert 1.0",
                (sBmp).GetHbitmap(),
                false,//isUnregistered
                "",//licenseStatus
                ""); //skuName
        }

        /**************************************************************/
        /*                       Public
        /**************************************************************/

        public static void IDERegister() {
            try {
				LoadConfiguration();
				CreateJcfExpertMenu();
                Resources.InitResourceManager();
                RegisterSplashScreen();
                RegisterAboutBox();

                string[] _MenuNames;
                int[] _MenuShortCuts;

                new OTAJediCodeFormat().IDERegister(out _MenuNames, out _MenuShortCuts);
                RegisterMenus(_MenuNames, _MenuShortCuts);

                new OTAJcfExpertOptions().IDERegister(out _MenuNames, out _MenuShortCuts);
                RegisterMenus(_MenuNames, _MenuShortCuts);

                fTimer.Interval = 1000;
                fTimer.Tick += new EventHandler(DoTimer);
                fTimer.Enabled = true;
            } catch (Exception e) {
                MessageBox.Show(e.Message + "--" + e.StackTrace);
            }
        }

        public static void LoadConfiguration() {
            fConfiguration = null;

            if (File.Exists(OptionFileName)) {
                XmlSerializer _Serializer = new XmlSerializer(typeof(Configuration));

                FileStream _FileStream = new FileStream(OptionFileName, FileMode.Open);
                XmlReader _Reader = new XmlTextReader(_FileStream);

                fConfiguration = (Configuration) _Serializer.Deserialize(_Reader);
            } else {
                fConfiguration = new Configuration();
            }

            SetDefaultConfiguration();
        }

        public static void SaveConfiguration() {
            SetDefaultConfiguration();

            XmlSerializer _Serializer = new XmlSerializer(typeof(Configuration));

            // Create an XmlSerializerNamespaces object.
            Stream _FileStream = new FileStream(OptionFileName, FileMode.Create);
            XmlWriter _Writer = new XmlTextWriter(_FileStream, new UTF8Encoding());

            // Serialize using the XmlTextWriter.
            _Serializer.Serialize(_Writer, fConfiguration);
            _Writer.Close();
        }

        public static void SetDefaultConfiguration() {
            if (fConfiguration.JcfParams == "") {
                fConfiguration.JcfParams = Consts.DEFAULT_JCF_PARAMS;
            }
        }

        /**************************************************************/
        /*                     Properties
        /**************************************************************/

        public static Configuration Configuration {
            get {
                return fConfiguration;
            }
        }

        public static string OptionFileName {
            get {
                string _Path  = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";

                return _Path + "JcfExpert.xml";
            }
        }

    }
}
