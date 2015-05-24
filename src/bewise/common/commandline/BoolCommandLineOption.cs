using System;

namespace BeWise.Common.CommandLine {
	/// <summary>
	/// Bool command line option.
	/// </summary>
    public class BoolCommandLineOption : BaseCommandLineOption {
        /**************************************************************/
        /*                     Contructor
        /**************************************************************/
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="aSwitch">Switch</param>
        public BoolCommandLineOption(string aSwitch) : base(aSwitch) {
        }

        /**************************************************************/
        /*                     Private
        /**************************************************************/

        private bool found;

        /**************************************************************/
        /*                       Public
        /**************************************************************/
		/// <summary>
		/// Processes.
		/// </summary>
		/// <param name="param">Parameters</param>
		/// <returns>Always true.</returns>
        public override bool Process(string param) {
            found = true;

            return true;
        }

        /**************************************************************/
        /*                     Properties
        /**************************************************************/
		/// <summary>
		/// Found or not.
		/// </summary>
        public bool Found {
            get {
                return found;
            }

            set {
                found = value;
            }
        }
    }
}
