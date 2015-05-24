using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace BeWise.SharpDevTools.Component {

    public class SecretKeyManager : System.ComponentModel.Component
    {

        // *************************************************************************
        //                                  Class
        // *************************************************************************
        public class HiddenKeys {
        
            // *************************************************************************
            //                              Constructor
            // *************************************************************************
            public HiddenKeys (string aKeys, string aHelp, EventHandler aExecuteHandler) {
                fKeys = aKeys;
                fHelp = aHelp;
                
                ExecuteHandler += aExecuteHandler;
            }

            // *************************************************************************
            //                              Private
            // *************************************************************************
            private string fHelp;
            private string fKeys;
            
            // *************************************************************************
            //                              Public
            // *************************************************************************
            public void Execute() {
                if (ExecuteHandler != null) {
                    ExecuteHandler(null, null);
                }
            }

            // *************************************************************************
            //                              Properties
            // *************************************************************************
            public string Help {
                get {
                    return fHelp;
                }
            }

            public string Keys {
                get {
                    return fKeys;
                }
            }

            public event EventHandler ExecuteHandler;

        }

        // *************************************************************************
        //                              Constructor
        // *************************************************************************
        public SecretKeyManager(System.ComponentModel.IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        public SecretKeyManager()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        // *************************************************************************
        //                              Private
        // *************************************************************************
        private System.ComponentModel.Container components = null;
        private string fLastKeyPress;
        private DateTime fLastKeyTime;
        private ArrayList fHiddenKeysList = new ArrayList();

        #region Component Designer generated code
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        // *************************************************************************
        //                              Protected
        // *************************************************************************
        protected virtual void ProcessHiddenKeysPressed(string aHiddenKeysPressed) {
            foreach(HiddenKeys _HiddenKeys in fHiddenKeysList) {
                if (_HiddenKeys.Keys.ToUpper() == aHiddenKeysPressed.ToUpper()) {
                    _HiddenKeys.Execute();
                    return;
                }
            }
        }

        protected ArrayList HiddenKeysList {
            get {
                return fHiddenKeysList;
            }
        }

        // *************************************************************************
        //                              Public
        // *************************************************************************
        public void RegisterKeySequence(HiddenKeys aHiddenKeys){
            fHiddenKeysList.Add(aHiddenKeys);
        }

        public void SendKey(char aKey) {
            System.TimeSpan _Diff = DateTime.Now - fLastKeyTime;

            if ((fLastKeyTime == DateTime.MinValue) || (_Diff.TotalMilliseconds > 1000)) {
                fLastKeyPress = aKey.ToString();
            } else {
                fLastKeyPress += aKey;
            }

            fLastKeyTime = DateTime.Now;

            if (fLastKeyPress != null && fLastKeyPress != String.Empty) {
                ProcessHiddenKeysPressed(fLastKeyPress);
            }
        }

        public void SendKey(Keys aKeyCode) {
            if ((int) aKeyCode > 64 && (int) aKeyCode < 91) {
                int _Key = (int) aKeyCode;
                SendKey((char)_Key);
            } else {
                Reset();
            }
        }

        public void Reset(){
            fLastKeyPress = "";
            fLastKeyTime = DateTime.MinValue;
        }

        public virtual void ShowHelp() {
            string _Msg = "Help: \r\n\r\n";

            ArrayList _List = new ArrayList();

            foreach(HiddenKeys _HiddenKeys in HiddenKeysList) {
                _List.Add("  " + _HiddenKeys.Keys + ": " + _HiddenKeys.Help + "\r\n\r\n");
            }

            _List.Sort();

            foreach(string _Str in _List) {
                if (!_Str.StartsWith("HELP")) {
                    _Msg += _Str;
                }
            }

            MessageBox.Show(_Msg);
        }
    }
}
