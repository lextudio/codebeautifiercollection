using System;
using System.IO;
using BeWise.Common.Utils;
using Microsoft.Win32;

namespace BeWise.Common.Info {

	/// <summary>
	/// Expert info.
	/// </summary>
    public abstract class ExpertInfo
    {
        int version;
        string valueName;
        bool isActive;
        string valueData;

        public ExpertInfo(string valueName, string valueData, bool active, int version)
        {
            this.valueName = valueName;
            this.valueData = valueData;
            this.isActive = active;
            this.version = version;
        }

        public int Version
        {
            get
            {
                return version;
            }
        }

        public bool Active
        {
            get
            {
                return isActive;
            }
        }

        public bool Exists
        {
            get
            {
                return File.Exists(FileName);
            }
        }

        internal void WriteKey(string key)
        {
            Registry.CurrentUser.CreateSubKey(key);
            RegistryKey _RegistryKey = Registry.CurrentUser.OpenSubKey(key, true);
            if (_RegistryKey != null)
            {
                _RegistryKey.SetValue(valueName, valueData);
            }
        }

        internal void DeleteKey(string key)
        {
            RegistryKey _RegistryKey = Registry.CurrentUser.OpenSubKey(key, true);
            if (_RegistryKey != null)
            {
                _RegistryKey.DeleteValue(valueName);
            }
        }

        /// <summary>
        /// Adds reg key.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="name">Name</param>
        /// <param name="version">IDE version</param>
        /// <param name="isDotNet">IsDotNet flag</param>
        public void Persist()
        {
            WriteKey(this is DotNetExpert ? OtaUtils.GetEnabledAssembliesRegKey(version) : OtaUtils.GetEnabledPackagesRegKey(version));
        }

        public abstract void Delete();
        /// <summary>
        /// Activates an expert.
        /// </summary>
        public abstract void Activate();

        public abstract void Deactivate();
        /// <summary>
        /// IDE version.
        /// </summary>
        public int IdeVersion
        {
            get
            {
                return version;
            }
        }

        /// <summary>
        /// File name.
        /// </summary>
        public abstract string FileName
        {
            get;
        }

        protected string ValueName
        {
            get
            {
                return valueName;
            }
        }
        /// <summary>
        /// Is active.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return isActive;
            }
        }
        /// <summary>
        /// Name.
        /// </summary>
        public abstract string Name
        {
            get;
        }

        protected string ValueData
        {
            get
            {
                return valueData;
            }
        }
    }
}
