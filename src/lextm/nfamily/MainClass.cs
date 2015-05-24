using System;
using BeWise.Common.CommandLine;
using BeWise.SharpBuilderTools.Tools.AssemblyInfo;
using Lextm.OpenTools;

namespace Lextm.NFamily {

    public sealed class MainClass {

        private MainClass() { }
        /**************************************************************/
        /*                         Private
        /**************************************************************/

        private static void MessageEvent(object aSender, string aMessage) {
            Console.WriteLine(aMessage);
        }

        /**************************************************************/
        /*                     Consts
        /**************************************************************/
        const string DEFAULT_MESSAGE =
            "This program is used to create an assemblyinfo.cs /  assemblyinfo.pas file with many xml files.\r\n" +
            "Useful when you want to create many assemblies with the same version but not the same description/title\r\n" +
            "\r\n" +
            "usage: assemblyinfobuilder [outputfilename] <inputfilename> <inputfilename> ...\r\n" +
            "    -language:       -l:{filename}     specified the output language {Cs / Delphi}\r\n" +
            "    -outputfilename: -o:{filename}     specified a output cs file to create with the input xml files (optional, could be specified in the xml file)\r\n" +
            "    -inputfilename : -i:{filename}     specied the xml file for the create (*.aid)\r\n";

        const string INPUT_FILE_SWITCH   = "i:";
        const string LANGUAGE_SWITCH     = "l:";
        const string OUTPUT_FILE_SWITCH  = "o:";

        const string LANGUAGE_CS         = "Cs";
        const string LANGUAGE_DELPHI     = "Delphi";

        /**************************************************************/
        /*                     Public
        /**************************************************************/

        private static void Run(CommandLineManager aCommandLineManager) {
            ValueCommandLineOption _InputOptions = (ValueCommandLineOption) aCommandLineManager.GetCommandLineOptionBySwitch(INPUT_FILE_SWITCH);
            ValueCommandLineOption _OutputOption = (ValueCommandLineOption) aCommandLineManager.GetCommandLineOptionBySwitch(OUTPUT_FILE_SWITCH);
            ValueCommandLineOption _LanguageOptions = (ValueCommandLineOption) aCommandLineManager.GetCommandLineOptionBySwitch(LANGUAGE_SWITCH);

            if (_InputOptions == null) {
                Console.WriteLine("No input files specified");
                return;
            }

            if (_LanguageOptions == null) {
                Console.WriteLine("Language not specified");
                return;
            } else if (_LanguageOptions.Values.Count > 1){
                Console.WriteLine("Too many Language specified");
                return;
            }

            string[] _Files = new string[_InputOptions.Values.Count];
            for(int i = 0; i < _InputOptions.Values.Count; i++) {
                _Files[i] = _InputOptions.Values[i];
            }

            AssemblyInfoCreator _AssemblyInfoCreator = new AssemblyInfoCreator();
            _AssemblyInfoCreator.MessageEvent += new AssemblyInfoCreator.AssemblyInfoCreatorMessageEvent(MessageEvent);

            // Get the Language
            Language _Language;

            if (_LanguageOptions.Values[0].ToUpperInvariant() == LANGUAGE_CS.ToUpperInvariant()) {
                _Language = Language.CSharp;
            } else if (_LanguageOptions.Values[0].ToUpperInvariant() == LANGUAGE_DELPHI.ToUpperInvariant()) {
                _Language = Language.Delphi;
            } else {
                Console.WriteLine("Invalid language specified");
                return;
            }

            if (_OutputOption != null && _OutputOption.Values.Count > 0) {
                if (_OutputOption.Values.Count > 1) {
                    Console.WriteLine("Too many output specified");
                    return;
                }

                _AssemblyInfoCreator.CreateAssemblyInfo(_Language, _Files, _OutputOption.Values[0]);
            } else {
                _AssemblyInfoCreator.CreateAssemblyInfo(_Language, _Files);
            }
        }

        [STAThread]
        public static void Main(string[] aArgs) {
            Console.WriteLine("Assembly Info Builder 3.0.0.0 Copyright (C) 2004 David Hervieux\r\n");

            CommandLineManager _CommandLineManager = new CommandLineManager();
            _CommandLineManager.DefaultMessage = DEFAULT_MESSAGE;

            ValueCommandLineOption _OutputCommandLineOption = new ValueCommandLineOption(OUTPUT_FILE_SWITCH);
            ValueCommandLineOption _InputCommandLineOption = new ValueCommandLineOption(INPUT_FILE_SWITCH);
            ValueCommandLineOption _LanguageCommandLineOption = new ValueCommandLineOption(LANGUAGE_SWITCH);

            _CommandLineManager.RegisterCommandLineSwitch(_OutputCommandLineOption);
            _CommandLineManager.RegisterCommandLineSwitch(_InputCommandLineOption);
            _CommandLineManager.RegisterCommandLineSwitch(_LanguageCommandLineOption);

            if (aArgs.Length != 0 && _CommandLineManager.Process(aArgs)) {
                Run(_CommandLineManager);
            } else {
                Console.WriteLine(_CommandLineManager.DefaultMessage);
            }
        }
    }
}
