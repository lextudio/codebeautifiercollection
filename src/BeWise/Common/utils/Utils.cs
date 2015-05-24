using System;
using System.CodeDom;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;

namespace BeWise.Common.Utils {

    public class Utils {

        /**************************************************************/
        /*                     Clipboard
        /**************************************************************/

        public static string GetTextDataFromClipBoard() {
            IDataObject _Data = Clipboard.GetDataObject();

            if (_Data.GetDataPresent(DataFormats.Text)) {
                return _Data.GetData(DataFormats.Text).ToString();
            } else {
                return "";
            }
        }

        public static void SetTextDataToClipBoard(string aText) {
            Clipboard.SetDataObject(aText);
        }

        /**************************************************************/
        /*                         File
        /**************************************************************/

        public static string AddExtension(string aFileName, string aExtension) {
            if (!StringIsNullOrEmpty(aFileName) &&
                    !StringIsNullOrEmpty(aExtension)) {

                if (!aFileName.ToUpper().EndsWith(aExtension.ToUpper())) {
                    return aFileName + aExtension;
                }
            }

            return aFileName;
        }

        public static bool FileIsValid(string aFileName) {
            if (!File.Exists(aFileName)) {
                return false;
            } else {
                FileInfo _FileInfo = new FileInfo(aFileName);

                return (_FileInfo.Length > 0);
            }
        }

        public static bool GetFileIsReadOnly(string aFileName) {
            FileInfo _FileInfo = new FileInfo(aFileName);
            return ((_FileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) ;
        }

        public static void SetReadOnlyFile(string aFileName, bool aValue) {
            bool _ReadOnly = GetFileIsReadOnly(aFileName);

            if (_ReadOnly && !aValue) {
                File.SetAttributes(aFileName, File.GetAttributes(aFileName) ^ FileAttributes.ReadOnly);

            } else if (!_ReadOnly && aValue) {
                File.SetAttributes(aFileName, File.GetAttributes(aFileName) | FileAttributes.ReadOnly);
            }
        }

        /**************************************************************/
        /*                        String
        /**************************************************************/

        public static string GetLineValue(string aString, int aLineNumber) {
            if (aLineNumber < 1) {
                return null;
            }

            StringReader  _Reader = new StringReader(aString);

            string _Line = null;

            for (int i = 0; i < aLineNumber; i++) {
                _Line = _Reader.ReadLine();

                if (_Line == null) {
                    return null;
                }
            }

            return _Line;
        }

        public static string IncludeTraillingSlash(string aString) {
            if (!StringIsNullOrEmpty(aString)) {
                if (!aString.EndsWith(@"\")) {
                                      aString += @"\";
                                  }
                          }

                          return aString;
}

public static bool IsStrCaseIns(string aSource, int aPos, string aSubStr) {
        if (aPos + aSubStr.Length - 1 <= aSource.Length) {
            return aSubStr.ToUpper() == aSource.Substring(aPos, aSubStr.Length).ToUpper();
        }
        else {
            return false;
        }
    }

    public static string QuoteString(string aStr){
        if (aStr == null) {
            return null;
        }

        return @"""" + aStr + @"""";
    }

    public static bool StringIsNullOrEmpty(string aString){
        return (aString == null || aString == "");
    }
}
}
