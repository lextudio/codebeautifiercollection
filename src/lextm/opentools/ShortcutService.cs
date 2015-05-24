// this is the shortcut array list class.
//      Some functions are ported from C#Goodies to do XML file reading and saving.
//		So the settings file format is the same with C#Goodies'.
// Copyright (C) 2005-2006  Lex Y. Li
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
namespace Lextm.OpenTools {

	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Xml;
	using Borland.Studio.ToolsAPI;
	using Lextm.Diagnostics;


	///<summary>
	///Shortcuts manager.
	///</summary>
	public sealed class ShortcutService
	{

		private IDictionary<string, Elements.ShortcutRecord> table = new Dictionary<string, Elements.ShortcutRecord>();
		/// <summary>
		/// dictionary inside.
		/// </summary>
		public IDictionary<string, Elements.ShortcutRecord> Table {
			get {
				return table;
			}
		}
		///<summary>
		///Adds a menu shortcut.
		///</summary>
		///<param name="item">Menu item</param>
		///<param name="shortcut">Shortcut</param>
		internal void Add( Borland.Studio.ToolsAPI.IOTAMenuItem item, int shortcut ) {
			if (item != null) {
				if (shortcut != 0) {
					if (String.IsNullOrEmpty(item.Name)) {
						LoggingService.Warn("noname menu");
					} else {
						// This.Add(new ShortcutRecord(item, shortcut));
						table.Add(item.Text, new Elements.ShortcutRecord(item, shortcut));
					}
				} else {
					LoggingService.Warn("zero shortcut");
				}
				//if (this.Count == 1) {
				if (table.Count == 1)
				{
					StartFileNotification();
				}
			} else {
				LoggingService.Warn("null menu");
			}
		}
		#region notification.
		private static void StartFileNotification( ) {
			IOTAService ideService = BeWise.Common.Utils.OtaUtils.GetService();
			ideService.FileNotification += new FileNotificationHandler(UpdateMenuItemShortcuts);
		}

		private static void UpdateMenuItemShortcuts( object sender, Borland.Studio.ToolsAPI.FileNotificationEventArgs args ) {
			if ((args.NotifyCode == OTAFileNotification.ofnPackageInstalled) ||
			    (args.NotifyCode == OTAFileNotification.ofnPackageUninstalled) ||
			    (args.NotifyCode == OTAFileNotification.ofnFileOpened)) {
				//foreach (ShortcutRecord record in getInstance()) {
				foreach (Elements.ShortcutRecord record in Instance.table.Values) {
					record.MenuItem.Shortcut = record.Shortcut;
				}
			}
		}
		#endregion
		
		///<summary>
		///Refreshes shortcuts.
		///</summary>
		public static void RefreshShortcuts( ) {

			LoggingService.EnterMethod();
			//foreach (ShortcutRecord record in getInstance()) {
			foreach (Elements.ShortcutRecord record in Instance.table.Values) {
				record.MenuItem.Shortcut = record.Shortcut;
				LoggingService.Info("the menu name is " + record.MenuItem.Name);
				LoggingService.Info("the shortcut is " + record.Shortcut);
			}

			LoggingService.LeaveMethod();

		}
		#region singleton
		private ShortcutService( ) {
		}

		private static ShortcutService instance;

		///<summary>
		///Gets singleton instance.
		///</summary>
		public static ShortcutService Instance {
			get {
				lock(typeof(ShortcutService)){
					if (instance == null)
					{
						instance = new ShortcutService();
					}
				}
				return instance;
			}
		}
		#endregion

		#region Load and save shortcuts

		/// <summary>
		/// Loads all shortcuts.
		/// </summary>
		/// <remarks>If the file is not valid, loads nothing.</remarks>
		public static void LoadShortcuts( ) {

			LoggingService.EnterMethod();
			if (Shortcuts.TryLoadFile()) {
				//foreach (ShortcutRecord record in getInstance()) {
				foreach (Elements.ShortcutRecord record in Instance.table.Values) {
					record.Shortcut =
						Shortcuts.GetShortcut(record.MenuItem.Name);
				}
			} else {
				LoggingService.Warn("no valid file, list is default.");
			}
			LoggingService.LeaveMethod();

		}

		/// <summary>
		/// Saves all shortcuts.
		/// </summary>
		/// <remarks>Overrides all shortcuts.</remarks>
		public static void SaveShortcuts( ) {

			LoggingService.EnterMethod();
			if (Shortcuts.TestBeforeSaveFile()) {
				//foreach (ShortcutRecord record in getInstance()) {
				foreach (Elements.ShortcutRecord record in Instance.table.Values) {
					Shortcuts.SaveShortcut(record.MenuItem.Name, record.Shortcut);
				}
				Shortcuts.SaveFile();
			} else {
				LoggingService.Warn("null doc");
			}
			LoggingService.LeaveMethod();

		}


		/// <remarks>
		/// A class to control shortcut configuration,
		/// which reads shortcuts from an XML file.
		/// </remarks>
		private sealed class Shortcuts {

			///<summary>
			///Tests before loading.
			///</summary>
			/// <returns>true if successfully loaded, false if not.</returns>
			internal static bool TryLoadFile( ) {
				XmlTextReader reader = null;
				bool result;
				if (File.Exists(FileName)) {
					
					try {
						// Load the XML file
						reader = new XmlTextReader(FileName);
						reader.Read();
						ShortcutsFile.Load(reader);
						// loads successfully
						result = true;
					}
					catch (XmlException)
					{
						LoggingService.Error("invalid file, so just empty body");
						ShortcutsFile.LoadXml("<?xml version='1.0' ?>" +
						                      "<Configuration />");
						result = false;
					}
					finally {
						if (reader != null) {
							reader.Close();
						}
					}
				} else {
					LoggingService.Warn("no file, so just empty body");
					ShortcutsFile.LoadXml("<?xml version='1.0' ?>" +
					                      "<Configuration />");
					result = false;
				}
				return result;
			}

			/// <summary>
			/// Tests before saving.
			/// </summary>
			/// <returns>true if file is valid, false if not.</returns>
			internal static bool TestBeforeSaveFile( ) {
				return (ShortcutsFile.DocumentElement != null);
			}

			/// <summary>
			/// Get the value for the desired setting.
			/// </summary>
			/// <param name="name">Name of setting value to retrieve. </param>
			/// <returns>String containing setting value. </returns>
			internal static int GetShortcut( string name ) {
				int result;      // Value to return
				string text = null;
				// Retrieve the appropriate Parameter node from the XML
				XmlNode node =
					ShortcutsFile.DocumentElement.SelectSingleNode(
						"descendant::Shortcut[@Name='" + name + "']");
				if ( node != null ) {
					// Retrieve the setting value from the Value attribute
					text = node.Attributes.GetNamedItem("Value").InnerText;
				}
				try {
					result = Convert.ToInt32(text, CultureInfo.InvariantCulture);
				} catch (ArgumentException) {
					result = 0;
				} catch (FormatException) {
					result = 0;
				} catch (OverflowException) {
					result = 0;
				}
				return result;
			}

			/// <summary>
			/// Save a new value for the desired setting.  If the setting
			/// does not already exist then it will be automatically created.
			/// </summary>
			/// <param name="name">Name of the setting. </param>
			/// <param name="shortcut">New setting value. </param>
			internal static void SaveShortcut( string name, int shortcut ) {
				// Get the value from the XML
				XmlNode node =
					ShortcutsFile.DocumentElement.SelectSingleNode(
						"descendant::Shortcut[@Name='" + name + "']");
				if ( node != null ) {
					// Set the new value of the Value attribute
					node.Attributes.GetNamedItem("Value").InnerText = shortcut.ToString(CultureInfo.InvariantCulture);
				}
				else {
					// Parameter does not exist so create it
					XmlElement setting =
						ShortcutsFile.CreateElement("Shortcut");
					setting.SetAttribute("Name", name);
					setting.SetAttribute("Value", shortcut.ToString(CultureInfo.InvariantCulture));
					ShortcutsFile.DocumentElement.AppendChild(setting);
				}
			}

			///<summary>
			///Saves file.
			///</summary>
			internal static void SaveFile( ) {
				ShortcutsFile.Save(FileName);
			}

			private const string Name = "Shortcuts.ota";
			// The configuration file name. The name is built using the
			// directory of the application. When in development this
			// is likely the \bin\Debug directory in the project folder.
			private readonly static string FileName =
				IO.Path.GetPreferencesFile(Name);
			// XML file containing the shortcuts
			private static XmlDocument ShortcutsFile = new XmlDocument();
			
			private Shortcuts( ) {
			}

		}
		#endregion
//	    /// <remarks>
//	    /// Attribute used to indicate if a property should be configured
//	    /// by the ShortcutConfigurator object.  It is valid only on Properties and
//	    /// only one attribute is allowed per property.
//	    /// </remarks>
//	    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
//	    public class ShortcutConfiguratorFieldAttribute : Attribute {
//	        // Internal Name value
//	        private string _FieldName = "";
//
//	        /// <value>Name of the data field</value>
//	        public string FieldName
//	        {
//	            get {
//	                return this._FieldName;
//	            }
//	            set {
//	                this._FieldName = value;
//	            }
//	        }
//
//	        // Internal InConfiguration value
//	        private bool _InConfiguration = false;
//	        /// <value>Indicator if the data field is in
//	        ///        configuration file</value>
//	        public bool InConfiguration
//	        {
//	            get {
//	                return this._InConfiguration;
//	            }
//	            set {
//	                this._InConfiguration = value;
//	            }
//	        }
//
//	        /// <summary>
//	        /// Constructor.
//	        /// </summary>
//	        /// <param name="FieldName">Name of the data field</param>
//	        /// <param name="InConfiguration">Indicator if data field in config</param>
//	        public ShortcutConfiguratorFieldAttribute(string FieldName, bool InConfiguration) {
//	            this._FieldName = FieldName;
//	            this._InConfiguration = InConfiguration;
//	        }
//	    }
	}
}
