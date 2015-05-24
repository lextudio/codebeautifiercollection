using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BeWise.SharpBuilderTools.Tools.Ant {

    public abstract class BaseAntItem {

        // *************************************************************************
        //                           Constructor
        // *************************************************************************

        protected BaseAntItem(string aName) {
            fName = aName;
        }

        // *************************************************************************
        //                               Private
        // *************************************************************************

        private int fColNumber;
        private int fLineNumber;
        private string fName;

        // *************************************************************************
        //                                Protected
        // *************************************************************************

		protected abstract string GetAntTypeName();

        // *************************************************************************
        //                                Properties
        // *************************************************************************

		internal string AntTypeName {
            get {
				return GetAntTypeName();
            }
        }

		internal int ColNumber {
            get {
                return fColNumber;
            }
            set {
                fColNumber = value;
            }
        }

		internal int LineNumber {
            get {
                return fLineNumber;
            }
            set {
                fLineNumber = value;
            }
        }

		internal string Name {
            get {
                return fName;
            }
        }
    }
}
