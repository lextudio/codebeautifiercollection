using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace BeWise.SharpDevTools.Component {

    public class SplashScreenControl : Control
    {
        // *************************************************************************
        //                              Constructor
        // *************************************************************************
        public SplashScreenControl(System.ComponentModel.IContainer container) : this()
        {
            container.Add(this);
        }

        public SplashScreenControl()
        {
            InitializeComponent();
            fTimer.Enabled = false;
            fTimer.Interval = 500;
            fTimer.Tick += new System.EventHandler(this.OnTick);
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
        private bool fHideSplashScreen;
        private System.Windows.Forms.Timer fTimer = new System.Windows.Forms.Timer();

        #region Component Designer generated code
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        private void OnTick(object sender,EventArgs e) {
            if (fHideSplashScreen == true) {
                fTimer.Enabled = false;
                SplashScreenManager.SplashScreen.Close();
            }
        }

        public void Start(){
            fTimer.Enabled = true;
        }

        // *************************************************************************
        //                              Properties
        // *************************************************************************

        public bool HideSplashScreen {
            get {
                lock(this) {
                   return fHideSplashScreen;
                }
             }
             set {
                lock(this) {
                   fHideSplashScreen = value;
                }
             }
        }
    }

    public class SplashScreenManager : object
    {
        // *************************************************************************
        //                              Constructor
        // *************************************************************************

        public SplashScreenManager()
        {
        }

        // *************************************************************************
        //                              Private
        // *************************************************************************
        private static Form fSplashScreen;
        private static SplashScreenControl fSplashScreenControl = new SplashScreenControl();
        private static Thread fWorkerThread;

        private static void DoShow() {
            fSplashScreenControl.Start();
            SplashScreen.ShowDialog();
        }

        // *************************************************************************
        //                              Public
        // *************************************************************************
        public static void CloseSplashScreen() {
            if (fSplashScreen == null || fSplashScreenControl.HideSplashScreen) {
                return;
            }

            fSplashScreenControl.HideSplashScreen = true;

            if (fWorkerThread != null && fWorkerThread.IsAlive && fWorkerThread != Thread.CurrentThread) {
                fWorkerThread.Join();
            }
        }

        public static void ShowSplashScreen(Form aForm) {
            if (fWorkerThread != null) {
                throw new Exception("Splash Screen already visible");
            }

            fSplashScreen = aForm;
            fSplashScreenControl.Parent = aForm;
            fSplashScreen.Controls.Add(fSplashScreenControl);
            ThreadStart _ThreadStart = new ThreadStart(DoShow);
            fWorkerThread = new Thread(_ThreadStart);
            fWorkerThread.Start();
        }

        // *************************************************************************
        //                              Properties
        // *************************************************************************
        public static Form SplashScreen {
            get {
                return fSplashScreen;
            }
        }

        public static Thread WorkerThread {
            get {
                return fWorkerThread;
            }
        }

    }
}
