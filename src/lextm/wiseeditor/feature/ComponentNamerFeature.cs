using System;

using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Text;
using System.Windows.Forms;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;
using Borland.Studio.ToolsAPI;
using Lextm.OpenTools;
using Lextm.Diagnostics;
using Lextm.Xml;
using System.Collections.Generic;
using System.Collections;

namespace Lextm.WiseEditor.Feature {
    /// <summary>
    /// Rename Components feature.
    /// </summary>
    public class ComponentNamerFeature : CustomFeature {

		private const string Name = "Component Namer";

        ///<summary>
        ///Registers tabs.
        ///</summary>
        ///<remarks>
        /// Used to configure tabs on FormPreferences.
        ///</remarks>
        protected override void IdeRegisterTabs() {
            base.IdeRegisterTabs();
			RegisterTab(CreateTabNode(Name, typeof(Gui.ComponentNamerPage)));
        }

        /**************************************************************/
        /*                     Constructor
        /**************************************************************/

        public ComponentNamerFeature() {
            // binds custom handler to IDE.
            IOTAService _Service = OtaUtils.GetService();
            _Service.FileNotification += new FileNotificationHandler(OpenCloseFileNotificationHandler);

			//RegisterOptionsType(typeof(Preferences));
            //StartFileNotification();
        }

        /**************************************************************/
        /*                     Private
        /**************************************************************/

        //private static FileNotificationHandler fMenuShortCutNotification;

        private static string GetControlPrefix(Type aType) {
            string _TypeName = aType.Name;

			foreach (ControlPrefix _ControlPrefix in options.ControlPrefixes) {
                if (_ControlPrefix.ControlTypeName == _TypeName) {
                    return _ControlPrefix.Prefix;
                }
            }

            return null;
        }

        private static string GetCandidateName(Type type) {
            string typeName = type.Name;
            StringBuilder prefix = new StringBuilder();
            // removes a, e, i, o, u, y from the string.
            foreach (char charIn in typeName) {
                char lower = Char.ToUpperInvariant(charIn);
                if ((lower == 'A') || (lower == 'E') || (lower == 'I')
                        || (lower == 'O') || (lower == 'U') || (lower == 'Y')) {
                    continue;
                } else {
                    prefix.Append(lower);
                }
            }
			if (Array.IndexOf(options.ControlPrefixes, prefix.ToString()) > -1) {
                return null;
            } else {
                return prefix.ToString();
            }
        }

        private static void SetValidControlName(Control aControl, INameCreationService aNameCreationService, string aName)
        {
            LoggingService.EnterMethod();

            System.Diagnostics.Trace.Assert(aNameCreationService != null);
            string _Name = aName;
            PropertyDescriptor _PropertyDescriptor = TypeDescriptor.GetProperties(aControl)["Name"];
            System.Diagnostics.Trace.Assert(_PropertyDescriptor != null);
            if (aNameCreationService.IsValidName(_Name))
            {
                try
                {
                    _PropertyDescriptor.SetValue(aControl, _Name);
                    LoggingService.Info("The name is " + _Name + " finally.");
                    string after = _PropertyDescriptor.GetValue(aControl) as string;
                    int j = 0;
                    while (after != _Name)
                    {
                        LoggingService.Info("Naming fails, try again.");
                        _PropertyDescriptor.SetValue(aControl, _Name);
                        after = _PropertyDescriptor.GetValue(aControl) as string;
                        j++;
                        if (j > 10)
                        {
                            break;
                        }
                    }
                    LoggingService.LeaveMethod();

                    return;
                }
                catch (Exception ex)
                {
                    LoggingService.Error(ex);
                }
            }
            else
            {
                LoggingService.Warn("Name (" + _Name + ") is not valid.");
            }

            if (_Name.EndsWith("1", StringComparison.Ordinal))
            {
                _Name = _Name.TrimEnd(new char[] { '1' });
                LoggingService.Info("1 is removed from name.");
            }
            else
            {
                LoggingService.Warn("There is not 1 in the name.");
            }

            int i = 1;

            while (true)
            {
                try
                {
                    _PropertyDescriptor.SetValue(aControl, _Name + i);
                    LoggingService.Info("The name is " + _Name + i + " finally.");
                    string after = _PropertyDescriptor.GetValue(aControl) as string;
                    int j = 0;
                    while (after != _Name + i)
                    {
                        LoggingService.Info("Naming fails, try again.");
                        _PropertyDescriptor.SetValue(aControl, _Name + i);
                        after = _PropertyDescriptor.GetValue(aControl) as string;
                        j++;
                        if (j > 10)
                        {
                            break;
                        }
                    }
                    LoggingService.LeaveMethod();

                    return;
                }
                catch (Exception ex)
                {
                    LoggingService.Error(ex);
                }

                i++;

                // To be sure that we don't have an infinit loop for an
                // invalid name (Prefix)
                if (i > 100)
                {
                    LoggingService.Debug("i > 100, exit.");
                    LoggingService.LeaveMethod();

                    return;
                }
            }
        }

        private void OpenCloseFileNotificationHandler(object aSender, FileNotificationEventArgs aArgs) {
            LoggingService.Debug("enter handler");
            try {
                if (OtaUtils.IsCSFile(aArgs.FileName) &&
                        aArgs.NotifyCode == OTAFileNotification.ofnFileOpened) {

                    IOTAModule _Module = OtaUtils.GetModuleServices().FindModule(aArgs.FileName);

                    if (_Module != null) {
                        IDesignerHost _Designer = OtaUtils.GetDesignerForModule(_Module);

                        if (_Designer != null) {
                            // this module has a designer, so it is Control, UserControl, or Form.
                            IComponentChangeService _ChangeService = (IComponentChangeService) _Designer.GetService(typeof(IComponentChangeService));
                            // drops default handler and installs custom handler.
                            _ChangeService.ComponentAdded -= new ComponentEventHandler(OnComponentAdded);
                            _ChangeService.ComponentAdded += new ComponentEventHandler(OnComponentAdded);
                            LoggingService.Info("component added");
                        } else {
                            LoggingService.Warn("component not added");
                        }
                    }
                } else if (OtaUtils.IsCSFile(aArgs.FileName) && aArgs.NotifyCode == OTAFileNotification.ofnFileClosing) {
                    IOTAModule _Module = OtaUtils.GetModuleServices().FindModule(aArgs.FileName);

                    if (_Module != null) {
                        // Remove the Auto Rename Control Handler
                        IDesignerHost _Designer = OtaUtils.GetDesignerForModule(_Module);

                        if (_Designer != null) {
                            IComponentChangeService _ChangeService = (IComponentChangeService) _Designer.GetService(typeof(IComponentChangeService));

                            _ChangeService.ComponentAdded -= new ComponentEventHandler(OnComponentAdded);
                            LoggingService.Info("component removed");
                        }
                    }
                }
            } catch (Exception ex) {
                Lextm.Windows.Forms.MessageBoxFactory.Fatal(Name, ex);
            }
        }

        private static IDesignerHost fDesignerHost;
        private static IList<Control> fCreatedControls = new List<Control>();
        private static Timer fTimerComponentAdded = new Timer();

		private void OnRenameComponent(object o, EventArgs e) {
          
			LoggingService.EnterMethod();
			fTimerComponentAdded.Enabled = false;

			if (fDesignerHost == null) {
				LoggingService.Warn("null desinger cache.");
				LoggingService.LeaveMethod();
               
                return;
            }


            foreach(Control _Control in fCreatedControls) {
                string _Prefix = GetControlPrefix(_Control.GetType());
                if (String.IsNullOrEmpty(_Prefix)) {
					LoggingService.Info("This is not a prefix in list.");
                    _Prefix = GetNewPrefix(_Control.GetType());

                    if (!String.IsNullOrEmpty(_Prefix)) {
                        // if not in the list, add this prefix to preferences.
						LoggingService.Info("New prefix is adding, " + _Prefix);
						AddNewPrefix(_Prefix, _Control.GetType().Name);
					} else {
						// do not touch this control.
						LoggingService.Warn("This type is not added. Ignore this control and break.");
						break;
					}
				} 
				// names the component.
				NameComponent(_Control, _Prefix); 
            }
            fCreatedControls.Clear();
			fDesignerHost = null;
			LoggingService.LeaveMethod();
		
        }

        // whenever a component is added.
        private void OnComponentAdded(object sender, ComponentEventArgs ce) {
        	if (!(bool)PropertyRegistry.Get("EnableAutoRenameControls", false)) {
                return;
            }

            Control _Control = ce.Component as Control;

            if (_Control != null) {
                IDesignerHost _Designer = OtaUtils.GetDesignerForModule(OtaUtils.GetCurrentModule());

                if (_Designer != null && (fDesignerHost == null || _Designer == fDesignerHost) && !_Designer.Loading) {
                    fDesignerHost = _Designer;
                    fCreatedControls.Add(_Control);
                    fTimerComponentAdded.Interval = 500;
                    fTimerComponentAdded.Tick -= new EventHandler(OnRenameComponent);
                    fTimerComponentAdded.Tick += new EventHandler(OnRenameComponent);
                    fTimerComponentAdded.Enabled = true;
                }
            }
        }

#region Preferences
        /**************************************************************/
        /*                     Public
        /**************************************************************/
		private static Preferences options;

		public static Preferences GetOptions() {
			return options;
		}

		public override void LoadPreferences() {
			base.LoadPreferences();
			options = (Preferences)SerializationService.Load(
				Lextm.OpenTools.IO.Path.GetPreferencesFile(this.GetType()),
				typeof(Preferences));
		}

		public override void SavePreferences() {
			base.SavePreferences();
			SerializationService.Save(
				Lextm.OpenTools.IO.Path.GetPreferencesFile(this.GetType()),
				options);
        }
		/// <summary>
        /// Sets default preferences.
        /// </summary>
		public override void SetDefaultPreferences() {
        	base.SetDefaultPreferences();
			if (options == null)
			{
				options = new Preferences();
				options.ControlPrefixes = new ControlPrefix[0];
			}

			if (options.ControlPrefixes == null
					|| options.ControlPrefixes.Length == 0) {
                ArrayList _List = new ArrayList();

                _List.Add(new ControlPrefix("bt", "Button"));
                _List.Add(new ControlPrefix("chk", "CheckBox"));
                _List.Add(new ControlPrefix("cb", "ComboBox"));
                _List.Add(new ControlPrefix("lv", "Vista_Api.ListView"));
                _List.Add(new ControlPrefix("txt", "TextBox"));

				options.ControlPrefixes =  (ControlPrefix[]) _List.ToArray(typeof(ControlPrefix));
            }
        }

            ///<summary>
            ///All configuration.
            ///</summary>
            /// <remarks>
            /// It should be a Serializable class with public constructor so as to be serialized and deserialized.
            /// </remarks>
            [System.Serializable]
            public sealed class Preferences {

                private ControlPrefix[] controlPrefixes;
    
                public ControlPrefix[] ControlPrefixes {
                    get {
                        return controlPrefixes;
                    }
                    set {
                        controlPrefixes = value;
                    }
                }   

            }

#endregion
		private static string GetNewPrefix( Type type ) {
            Gui.FormNewPrefix form = new Gui.FormNewPrefix();
			form.lblControlName.Text = type.Name;
            
            form.txtPrefix.Text = GetCandidateName(type);
            if (form.ShowDialog() == DialogResult.OK) {
                // gets a new prefix
                return form.txtPrefix.Text;
            } else {
				return null;
			}
        }

		private static string GetComponentName( string _Prefix, Control _Control ) {
			Gui.FormComponentName formName = new Gui.FormComponentName();
			formName.txtName.Text = _Prefix + _Control.Name;
            formName.ShowDialog();
            return formName.txtName.Text;
		}

		private static void NameComponent( Control _Control, string _Prefix ) {            
			LoggingService.EnterMethod();
			if (!_Control.Name.StartsWith(_Prefix, StringComparison.Ordinal)) {
				//DesignerTransaction _DesignerTransaction = fDesignerHost.CreateTransaction("RenameComponents");
				//using (_DesignerTransaction) {
					INameCreationService service = (INameCreationService) fDesignerHost.GetService(typeof(INameCreationService));
                    SetValidControlName(_Control, service, (GetComponentName(_Prefix, _Control)));
					// Complete the designer transaction.
					//_DesignerTransaction.Commit();
				//}
			} else {
                LoggingService.Warn("Control " + _Control.Name + " has the prefix already.");
            }
			LoggingService.LeaveMethod();			
		}

		private void AddNewPrefix( string prefix, string typeName ) {
			if (Array.IndexOf(options.ControlPrefixes, prefix) == -1) {
				ArrayList list = new ArrayList();
				foreach (ControlPrefix controlPrefix in options.ControlPrefixes) {
					list.Add(controlPrefix);
				}
				list.Add(new ControlPrefix(prefix, typeName));

				options.ControlPrefixes = null; // releases the resources.
				options.ControlPrefixes = (ControlPrefix[]) list.ToArray(typeof(ControlPrefix));

				this.SavePreferences();
			} else {
                LoggingService.Warn("The prefix is already there.");
            }
		}
	}
}
