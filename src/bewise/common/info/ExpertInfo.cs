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

        /// <summary>
        /// Initiates an <see cref="ExpertInfo"/> instance.
        /// </summary>
        /// <param name="valueName">Value name.</param>
        /// <param name="valueData">Value data.</param>
        /// <param name="active">Active flag.</param>
        /// <param name="version">Version number.</param>
        public ExpertInfo(string valueName, string valueData, bool active, int version)
        {
            this.valueName = valueName;
            this.valueData = valueData;
            this.isActive = active;
            this.version = version;
        }

        /// <summary>
        /// Version number.
        /// </summary>
        public int Version
        {
            get
            {
                return version;
            }
        }

        /// <summary>
        /// Active flag.
        /// </summary>
        public bool Active
        {
            get
            {
                return isActive;
            }
        }

        /// <summary>
        /// Indicates that if this expert file exists.
        /// </summary>
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
        /// Persists the expert to registry.
        /// </summary>
        public void Persist()
        {
            WriteKey(this is DotNetExpert ? OtaUtils.GetEnabledAssembliesRegKey(version) : OtaUtils.GetEnabledPackagesRegKey(version));
        }

        /// <summary>
        /// Deletes the expert key.
        /// </summary>
        public abstract void Delete();
        
        /// <summary>
        /// Activates an expert.
        /// </summary>
        public abstract void Activate();
        
        /// <summary>
        /// Deactives an expert.
        /// </summary>
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

        /// <summary>
        /// Value name.
        /// </summary>
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

        /// <summary>
        /// Value data.
        /// </summary>
        protected string ValueData
        {
            get
            {
                return valueData;
            }
        }
    }
}
