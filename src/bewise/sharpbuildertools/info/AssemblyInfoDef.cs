using System;
using System.Xml.Serialization;
using BeWise.Common.Utils;

namespace BeWise.SharpBuilderTools.Info {

	/// <summary>
	/// Assembly info definition.
	/// </summary>
    [Serializable]
    public class AssemblyInfoDef {
        
        ///<summary>Constructor</summary>        
        public AssemblyInfoDef() {
        }

        /**************************************************************/
        /*                     Public Field
        /**************************************************************/
        /// <summary>
        /// Company.
        /// </summary>
        public string AssemblyCompany;
        /// <summary>
        /// Configuration.
        /// </summary>
        public string AssemblyConfiguration;
        /// <summary>
        /// Copyright.
        /// </summary>
        public string AssemblyCopyright;
        /// <summary>
        /// Description.
        /// </summary>
        public string AssemblyDescription;
        /// <summary>
        /// Culture.
        /// </summary>
        public string AssemblyCulture;
        /// <summary>
        /// Delay sign.
        /// </summary>
        public bool AssemblyDelaySign;
        /// <summary>
        /// Key file.
        /// </summary>
        public string AssemblyKeyFile;
        /// <summary>
        /// Key name.
        /// </summary>
        public string AssemblyKeyName;
        /// <summary>
        /// Product.
        /// </summary>
        public string AssemblyProduct;
        /// <summary>
        /// Title.
        /// </summary>
        public string AssemblyTitle;
        /// <summary>
        /// Trademark.
        /// </summary>
        public string AssemblyTrademark;
        /// <summary>
        /// Version.
        /// </summary>
        public string AssemblyVersion;
        /// <summary>
        /// Index.
        /// </summary>
        public int Index;
        /// <summary>
        /// Output file name.
        /// </summary>
        public string OutputFileName;
		/// <summary>
		/// Input file name.
		/// </summary>
        [XmlIgnore]
        public string InputFileName;

        /**************************************************************/
        /*                        Public
        /**************************************************************/
		/// <summary>
		/// Deep copies.
		/// </summary>
		/// <param name="assemblyDef">Assembly definition</param>
        public void Assign(AssemblyInfoDef assemblyDef) {
            if (!String.IsNullOrEmpty(assemblyDef.AssemblyTitle)) {
                this.AssemblyTitle = assemblyDef.AssemblyTitle;
            }

            if (!String.IsNullOrEmpty(assemblyDef.AssemblyDescription)) {
                this.AssemblyDescription = assemblyDef.AssemblyDescription;
            }

            if (!String.IsNullOrEmpty(assemblyDef.AssemblyConfiguration)) {
                this.AssemblyConfiguration = assemblyDef.AssemblyConfiguration;
            }

            if (!String.IsNullOrEmpty(assemblyDef.AssemblyCompany)) {
                this.AssemblyCompany = assemblyDef.AssemblyCompany;
            }

            if (!String.IsNullOrEmpty(assemblyDef.AssemblyProduct)) {
                this.AssemblyProduct = assemblyDef.AssemblyProduct;
            }

            if (!String.IsNullOrEmpty(assemblyDef.AssemblyCopyright)) {
                this.AssemblyCopyright = assemblyDef.AssemblyCopyright;
            }

            if (!String.IsNullOrEmpty(assemblyDef.AssemblyTrademark)) {
                this.AssemblyTrademark = assemblyDef.AssemblyTrademark;
            }

            if (!String.IsNullOrEmpty(assemblyDef.AssemblyCulture)) {
                this.AssemblyCulture = assemblyDef.AssemblyCulture;
            }

            if (!String.IsNullOrEmpty(assemblyDef.AssemblyVersion)) {
                this.AssemblyVersion = assemblyDef.AssemblyVersion;
            }

            if (assemblyDef.AssemblyDelaySign) {
                this.AssemblyDelaySign = assemblyDef.AssemblyDelaySign;
            }

            if (!String.IsNullOrEmpty(assemblyDef.AssemblyKeyFile)) {
                this.AssemblyKeyFile = assemblyDef.AssemblyKeyFile;
            }

            if (!String.IsNullOrEmpty(assemblyDef.AssemblyKeyName)) {
                this.AssemblyKeyName = assemblyDef.AssemblyKeyName;
            }

            if (!String.IsNullOrEmpty(assemblyDef.OutputFileName)) {
                this.OutputFileName = assemblyDef.OutputFileName;
            }
        }
    }
}
