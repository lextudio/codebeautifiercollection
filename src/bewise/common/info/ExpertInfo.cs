using System;
using Microsoft.Win32;
using BeWise.Common.Utils;

namespace BeWise.Common.Info {
	/// <summary>
	/// Expert info.
	/// </summary>
    public class ExpertInfo {
        /**************************************************************/
        /*                     Constructor
        /**************************************************************/
		/// <summary>
		/// General constructor.
		/// </summary>
		/// <param name="name">Name</param>
		/// <param name="fileName">File name</param>
		/// <param name="isActive">Is active</param>
		/// <param name="version">IDE version</param>
		/// <param name="isDotNet">Is .NET</param>
        public ExpertInfo(string name, string fileName, bool isActive, int version, bool isDotNet) {
            this.fileName = fileName;
            this.isActive = isActive;
            this.name = name;
            this.version = version;
            this.isDotNet = isDotNet;
        }
        /// <summary>
        /// Construtor of .NET.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="fileName">File name</param>
        /// <param name="isActive">Is Active</param>
        /// <param name="version">IDE version</param>
        public ExpertInfo(string name, string fileName, bool isActive, int version) {
        	this.fileName = fileName;
            this.isActive = isActive;
            this.name = name;
            this.version = version;
            this.isDotNet = true;
        }

        /**************************************************************/
        /*                        Private
        /**************************************************************/

        private int version;
        private string fileName;
        private bool isActive;
        private string name;
        private bool isDotNet;

        private static void WriteKey(string keyName, string name, string value) {
            Registry.CurrentUser.CreateSubKey(keyName);

            RegistryKey _RegistryKey = Registry.CurrentUser.OpenSubKey(keyName, true);

            if (_RegistryKey != null) {
                _RegistryKey.SetValue(name, value);
            }
        }

        private static void DeleteKey(string aKey, string aAssemblyFileName) {
            RegistryKey _RegistryKey = Registry.CurrentUser.OpenSubKey(aKey, true);

            if (_RegistryKey != null) {
                _RegistryKey.DeleteValue(aAssemblyFileName);
            }
        }

        /**************************************************************/
        /*                        Public
        /**************************************************************/
		#region Lextm extensions
		/// <summary>
		/// Adds reg key.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <param name="name">Name</param>
		/// <param name="version">IDE version</param>
		/// <param name="isDotNet">IsDotNet flag</param>
        public static void Add(string fileName, string name, int version, bool isDotNet) {
			if (isDotNet) {
				WriteKey(OtaUtils.GetEnabledAssembliesRegKey(version), fileName, name);
			} else {
				WriteKey(OtaUtils.GetEnabledPackagesRegKey(version), name, fileName);
			}
        }
		/// <summary>
		/// Adds .NET.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <param name="name">Name</param>
		/// <param name="version">IDE version</param>
        public static void Add(string fileName, string name, int version) {
			Add(fileName, name, version, true);
        }
		/// <summary>
		/// Deletes .NET.
		/// </summary>
		/// <param name="isActive">Is active</param>
		/// <param name="fileName">File name</param>
		/// <param name="version">IDE version</param>
        public static void Delete(bool isActive, string fileName, int version) {
			Delete(isActive, fileName, version, true);
        }	
		/// <summary>
		/// Deletes.
		/// </summary>
		/// <param name="isActive">Is active</param>
		/// <param name="fileName">File name</param>
		/// <param name="version">IDE version</param>
		/// <param name="isDotNet">Is .NET</param>
        public static void Delete(bool isActive, string fileName, int version, bool isDotNet) {
            if (isActive) {
				if (isDotNet) {
					DeleteKey(OtaUtils.GetEnabledAssembliesRegKey(version), fileName);
				} else {
					DeleteKey(OtaUtils.GetEnabledPackagesRegKey(version), fileName);
				}
            } else {
				if (isDotNet) {
					DeleteKey(OtaUtils.GetDisabledAssembliesRegKey(version), fileName);
				} else {
					DeleteKey(OtaUtils.GetDisabledPackagesRegKey(version), fileName);
				}
            }
        }
		/// <summary>
		/// Activates an expert.
		/// </summary>
        public void Activate() {
            if (!isActive) {
				if (isDotNet) {
					WriteKey(OtaUtils.GetEnabledAssembliesRegKey(version), fileName, name);
				    DeleteKey(OtaUtils.GetDisabledAssembliesRegKey(version), fileName);
				} else {
					WriteKey(OtaUtils.GetEnabledPackagesRegKey(version), fileName, name);
					DeleteKey(OtaUtils.GetDisabledPackagesRegKey(version), fileName);
				}
            }
        }
		/// <summary>
		/// Deactivates an expert.
		/// </summary>
        public void Desactivate() {
            if (isActive) {
				if (isDotNet) {
					WriteKey(OtaUtils.GetDisabledAssembliesRegKey(version), fileName, name);
					DeleteKey(OtaUtils.GetEnabledAssembliesRegKey(version), fileName);
				} else {
					WriteKey(OtaUtils.GetDisabledPackagesRegKey(version), fileName, name);
					DeleteKey(OtaUtils.GetEnabledPackagesRegKey(version), fileName);
				}
            }
        }

        /**************************************************************/
        /*                        Properties
        /**************************************************************/
		/// <summary>
		/// IDE version.
		/// </summary>
        public int IdeVersion {
            get {
                return version;
            }
        }
        /// <summary>
        /// Is .NET or not.
        /// </summary>
        public bool IsDotNet {
        	get {
        		return isDotNet;
        	}
        }
#endregion
		/// <summary>
		/// File name.
		/// </summary>
        public string FileName {
            get {
                return fileName;
            }
        }
		/// <summary>
		/// Is active.
		/// </summary>
        public bool IsActive {
            get {
                return isActive;
            }
        }
		/// <summary>
		/// Name.
		/// </summary>
        public string Name {
            get {
                return name;
            }
        }
    }
}
