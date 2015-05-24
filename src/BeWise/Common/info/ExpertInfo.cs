using System;
using Microsoft.Win32;
using BeWise.Common;
using BeWise.Common.Utils;

namespace BeWise.Common.Info {

    public class ExpertInfo {
        /**************************************************************/
        /*                     Constructor
        /**************************************************************/

        public ExpertInfo(string aName, string aFileName, bool aIsActive, BDSVersion aBDSVersion) {
            fFileName = aFileName;
            fIsActive = aIsActive;
            fName = aName;
            fBDSVersion = aBDSVersion;
        }

        /**************************************************************/
        /*                        Private
        /**************************************************************/

        private BDSVersion fBDSVersion;
        private string fFileName;
        private bool fIsActive;
        private string fName;

        private static void WriteKey(string aKey, string aAssemblyFileName, string aName) {
            Registry.CurrentUser.CreateSubKey(aKey);

            RegistryKey _RegistryKey = Registry.CurrentUser.OpenSubKey(aKey, true);

            if (_RegistryKey != null) {
                _RegistryKey.SetValue(aAssemblyFileName, aName);
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

        public static void Add(string aFileName, string aName, BDSVersion aBDSVersion) {
            WriteKey(OTAUtils.GetEnabledAssembliesRegKey(aBDSVersion), aFileName, aName);
        }

        public static void Delete(bool aIsActive, string aFileName, BDSVersion aBDSVersion) {
            if (aIsActive) {
                DeleteKey(OTAUtils.GetEnabledAssembliesRegKey(aBDSVersion), aFileName);
            } else {
                DeleteKey(OTAUtils.GetDisabledAssembliesRegKey(aBDSVersion), aFileName);
            }
        }

        public void Activate() {
            if (!IsActive) {
                WriteKey(OTAUtils.GetEnabledAssembliesRegKey(fBDSVersion), fFileName, fName);
                DeleteKey(OTAUtils.GetDisabledAssembliesRegKey(fBDSVersion), fFileName);
            }
        }

        public void Desactivate() {
            if (IsActive) {
                WriteKey(OTAUtils.GetDisabledAssembliesRegKey(fBDSVersion), fFileName, fName);
                DeleteKey(OTAUtils.GetEnabledAssembliesRegKey(fBDSVersion), fFileName);
            }
        }

        /**************************************************************/
        /*                        Properties
        /**************************************************************/

        public BDSVersion BDSVersion {
            get {
                return fBDSVersion;
            }
        }

        public string FileName {
            get {
                return fFileName;
            }
        }

        public bool IsActive {
            get {
                return fIsActive;
            }
        }

        public string Name {
            get {
                return fName;
            }
        }
    }
}
