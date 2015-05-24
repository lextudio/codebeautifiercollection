using System;

using System.Collections.Generic;

namespace BeWise.Common.CommandLine {
	/// <summary>
	/// Command line manager.
	/// </summary>
    public class CommandLineManager {

        /**************************************************************/
        /*                          Private
        /**************************************************************/

        private IList<BaseCommandLineOption> commandLineOptions = new List<BaseCommandLineOption>();
        private string defaultMessage = "Invalid Command Line Options";

        /**************************************************************/
        /*                           Public
        /**************************************************************/
		/// <summary>
		/// Registers command line switch.
		/// </summary>
		/// <param name="commandLineOption">Command line option</param>
        public void RegisterCommandLineSwitch(BaseCommandLineOption commandLineOption) {
            commandLineOptions.Add(commandLineOption);
        }
		/// <summary>
		/// Gets command line option by switch.
		/// </summary>
		/// <param name="aSwitch">Switch</param>
		/// <returns>Command line option object.</returns>
        public BaseCommandLineOption GetCommandLineOptionBySwitch(string aSwitch) {
            foreach (BaseCommandLineOption _CommandLineOption in commandLineOptions) {
                if ((_CommandLineOption is ValueCommandLineOption) && aSwitch.StartsWith(_CommandLineOption.Switch, StringComparison.OrdinalIgnoreCase)) {
                    return _CommandLineOption;
                } else if (_CommandLineOption.Switch.ToUpperInvariant() == aSwitch.ToUpperInvariant()) {
                    return _CommandLineOption;
                }
            }

            return null;
        }
		/// <summary>
		/// Processes.
		/// </summary>
		/// <param name="commandLine">Command line</param>
		/// <returns>True if ok, false if not.</returns>
        public bool Process(string[] commandLine) {
            //Console.WriteLine("Processing command line...");

            foreach (string _Str in commandLine) {
                //Console.WriteLine(_Str);

                if (_Str.StartsWith("/", StringComparison.Ordinal) || _Str.StartsWith("-", StringComparison.Ordinal)) {
                    BaseCommandLineOption _CommandLineOption = GetCommandLineOptionBySwitch(_Str.Substring(1));

                    if (_CommandLineOption != null) {
                        //Console.WriteLine("1:" + _CommandLineOption.ToString());
                        if (!_CommandLineOption.Process(_Str.Substring(1))) {
                            return false;
                        }
                    } else {
                        return false;
                    }

                } else {
                    return false;
                }
            }

            return true;
        }

        /**************************************************************/
        /*                         Properties
        /**************************************************************/
		/// <summary>
		/// Command line options.
		/// </summary>
        public IList<BaseCommandLineOption> CommandLineOptions {
            get {
                return commandLineOptions;
            }
        }
		/// <summary>
		/// Default message.
		/// </summary>
        public string DefaultMessage {
            get {
                return defaultMessage;
            }

            set {
                defaultMessage = value;
            }
        }
    }
}
