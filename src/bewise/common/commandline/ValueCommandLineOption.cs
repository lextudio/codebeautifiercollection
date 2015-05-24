using System;
using System.Collections.Generic;

namespace BeWise.Common.CommandLine {
	/// <summary>
	/// Value command line option.
	/// </summary>
    public class ValueCommandLineOption : BaseCommandLineOption {
        /**************************************************************/
        /*                     Contructor
        /**************************************************************/
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="aSwitch">Switch</param>
        public ValueCommandLineOption(string aSwitch) : base(aSwitch) {
        }

        /**************************************************************/
        /*                     Private
        /**************************************************************/

        private IList<string> valueList = new List<string>();

        /**************************************************************/
        /*                       Public
        /**************************************************************/
		/// <summary>
		/// Processes.
		/// </summary>
		/// <param name="param">Parameters</param>
		/// <returns></returns>
        public override bool Process(string param) {
            string _Value = null;

            if (param.Length > Switch.Length + 1) {
                _Value = param.Substring(Switch.Length);
				valueList.Add(_Value);
            }

            return _Value != null;
        }

        /**************************************************************/
        /*                     Properties
        /**************************************************************/
        ///// <summary>
        ///// Values.
        ///// </summary>
        //public string[] Values {
        //    get {
        //        return (string[]) valueList.ToArray(typeof(string));
        //    }
        //}
		/// <summary>
		/// Values.
		/// </summary>
        public IList<string> Values {
            get {
                return valueList;
            }
        }

    }
}
