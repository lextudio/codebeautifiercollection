using System;
using System.Diagnostics;
using System.Reflection;
//using Lextm.CodeBeautifierCollection.Features;

namespace Lextm.CodeBeautifierCollection.Collections {
	///<summary>
	/// Feature record.
	/// </summary>
	[Serializable]
	public class Feature2 {

		/// <summary>
		/// Constructor.
		/// </summary>
		public Feature2() { }

		private EnabledRecord[] enabledRecords;
		private string name = "noname";
		private string description = "nohint";
		/// <summary>
		/// Enabled.
		/// </summary>
		public EnabledRecord[] EnabledRecords {
			get {
				return enabledRecords;
			}
			set {
                enabledRecords = value;
			}
		}                   
		/// <summary>
		/// Name.
		/// </summary>
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}
		/// <summary>
		/// Description.
		/// </summary>
		public string Description {
            get {
				return description;
			}
			set {
                description = value;
            }
        }
		/// <summary>
		/// Gets enabled.
		/// </summary>
		/// <param name="version">BDS version</param>
		/// <returns></returns>
		public bool GetEnabled(int version) {
			Array.Sort(enabledRecords);
			int index = Array.BinarySearch(enabledRecords, version);
			if (index > -1) {
				return enabledRecords[index].Enabled;
			} else {
            	// no record, set false by default;
				return false;
            }
		}
		/// <summary>
		/// Sets enabled.
		/// </summary>
		/// <param name="version">BDS version</param>
		/// <param name="enabled">Enabled</param>
		public void SetEnabled(int version, bool enabled) {
			Array.Sort(enabledRecords);
			int index = Array.BinarySearch(enabledRecords, version);
			if (index > -1)
			{
				enabledRecords[index].Enabled = enabled;
			}
        }	

//        /// <summary>Loads feature object.</summary>
//		public static CustomFeature LoadFeature(string fullName, Assembly assembly) {
//			object result = null;
//
//			Type type = assembly.GetType(fullName);
//
//            if (type != null) {
//                try {
//                    MethodInfo method = type.GetMethod("getInstance");
//					result = method.Invoke(null, null);
//				}
//                catch (Exception ex) {
//                    Debug.WriteLine(ex);
//                    Debug.WriteLine("exception so instance is not loaded.");
//                }
//                finally {
//					if (!(result is CustomFeature)) {
//						Debug.WriteLine("invalid object, feature is disabled.");
//						result = null;
//                    }
//                }
//			}
//			else {
//                Debug.WriteLine("null type");
//            }
//			return result as CustomFeature;
//		}
	}
}
