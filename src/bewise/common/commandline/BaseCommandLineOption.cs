using System;

namespace BeWise.Common.CommandLine {
	
	/// <summary>
	/// Base command line option.
	/// </summary>
	public abstract class BaseCommandLineOption {

		///<summary>Contructor
		/// </summary>
		/// <param name="aswitch">Switch</param>
        protected BaseCommandLineOption(string aswitch) {
            fSwitch = aswitch;
        }

        /**************************************************************/
        /*                     Private
        /**************************************************************/

        private string fSwitch = String.Empty;

        /**************************************************************/
        /*                       Public
        /**************************************************************/
		/// <summary>
		/// Processes.
		/// </summary>
		/// <param name="param">Parameter</param>
		/// <returns>Sucessful or not.</returns>
        public abstract bool Process(string param);

        /**************************************************************/
        /*                        Properties
        /**************************************************************/
		/// <summary>
		/// Switch.
		/// </summary>
        public string Switch {
            get {
                return fSwitch;
            }
        }
    }

}
