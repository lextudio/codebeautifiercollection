using System;
using System.Diagnostics;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common;
using BeWise.Common.Utils;

namespace BeWise.SharpBuilderTools.Wizard.Creator {

    public class WindowsServiceFileCreator : BaseModuleCreator {

        /**************************************************************/
        /*                     Constructor
        /**************************************************************/

        public WindowsServiceFileCreator(string aFileName, string aNamespace, string aClassName) {
            fFileName = aFileName;
            fNamespace = aNamespace;
            fClassName = aClassName;
        }

        /**************************************************************/
        /*                     Private
        /**************************************************************/
        private string fClassName;
        private string fFileName;
        private string fNamespace;

        /**************************************************************/
        /*                     Protected
        /**************************************************************/

        protected override string GetImplFileName() {
            return fFileName;
        }

        /**************************************************************/
        /*                   Protected Properties
        /**************************************************************/

        protected string ClassName {
            get {
                return fClassName;
            }
        }

        protected string FileName {
            get {
                return fFileName;
            }
        }

        protected string Namespace {
            get {
                return fNamespace;
            }
        }

        /**************************************************************/
        /*                     Public
        /**************************************************************/


        /*
        using System;
        using System.Collections;
        using System.ComponentModel;
        using System.Data;
        using System.Diagnostics;
        using System.ServiceProcess;
         
        namespace WindowsService
        {
            /// <summary>
            /// This is the class for my Service
            /// </summary>
            public class MyService : System.ServiceProcess.ServiceBase
            {
                public MyService()
                {
                    InitializeComponents();
         
                    // TODO: Add any further initialization code
                }
         
                private void InitializeComponents()
                {
                    this.ServiceName = "MyService";
                }
                
                /// <summary>
                /// This method starts the service.
                /// </summary>
                public static void Main()
                {
                    System.ServiceProcess.ServiceBase.Run(new System.ServiceProcess.ServiceBase[] {
                        new MyService() // To run more than one service you have to add them here
                    });
                }
         
                /// <summary>
                /// Clean up any resources being used.
                /// </summary>
                protected override void Dispose(bool disposing)
                {
                    // TODO: Add cleanup code here (if required)
                    base.Dispose(disposing);
                }
         
                /// <summary>
                /// Start this service.
                /// </summary>
                protected override void OnStart(string[] args)
                {
                    // TODO: Add start code here (if required)
                    //       to start your service.
                }
         
                /// <summary>
                /// Stop this service.
                /// </summary>
                protected override void OnStop()
                {
                    // TODO: Add tear-down code here (if required) 
                    //       to stop your service.
                }
            }
        }
         
        [RunInstaller(true)]
        public class ProjectInstaller : Installer
        {
            public ProjectInstaller()
            {
                ServiceProcessInstaller spi = new ServiceProcessInstaller();
                spi.Account = ServiceAccount.LocalSystem;
         
                ServiceInstaller si = new ServiceInstaller();
                si.ServiceName = "Hello Service Template";
                si.StartType = ServiceStartMode.Automatic;
                Installers.AddRange(new Installer[] {spi, si});
            }
        }
        */
		[]
        public override IOTAFile NewImplSource(string moduleIdent, string formIdent, string ancestorIdent) {
            string _Source = "";

            _Source += "using System;" + Environment.NewLine;
            _Source += "using System.Collections;" + Environment.NewLine;
            _Source += "using System.ComponentModel;" + Environment.NewLine;
            _Source += "using System.Data;" + Environment.NewLine;
            _Source += "using System.Diagnostics;" + Environment.NewLine;
            _Source += "using System.ServiceProcess;" + Environment.NewLine;


            /*
            if (!Utils.StringIsNullOrEmpty(fNamespace)) {
                _Source += "namespace " + fNamespace + " {" + Environment.NewLine;
            }

            _Source += "\tpublic class " + fClassName + " {" + Environment.NewLine + Environment.NewLine;
            _Source += "\t}" + Environment.NewLine;

            if (!Utils.StringIsNullOrEmpty(fNamespace)) {
                _Source += "}";
            }
            */
            //ici

            _Source += "\t[RunInstaller(true)]" + Environment.NewLine;
            _Source += "\tpublic class ProjectInstaller : Installer" + Environment.NewLine;
            _Source += "\t{" + Environment.NewLine;
            _Source += "\t\tpublic ProjectInstaller()" + Environment.NewLine;
            _Source += "\t\t{" + Environment.NewLine;
            _Source += "\t\t\tServiceProcessInstaller spi = new ServiceProcessInstaller();" + Environment.NewLine;
            _Source += "\t\t\tspi.Account = ServiceAccount.LocalSystem;" + Environment.NewLine + Environment.NewLine;

            _Source += "\t\t\tServiceInstaller si = new ServiceInstaller();" + Environment.NewLine;
            _Source += "\t\t\tsi.ServiceName = \"Hello Service Template\";" + Environment.NewLine;
            _Source += "\t\t\tsi.StartType = ServiceStartMode.Automatic;" + Environment.NewLine;
            _Source += "\t\t\tInstallers.AddRange(new Installer[] {spi, si});" + Environment.NewLine;
            _Source += "\t\t}" + Environment.NewLine;
            _Source += "\t}" + Environment.NewLine;

            return new CustomFile(_Source);
        }
    }
}
