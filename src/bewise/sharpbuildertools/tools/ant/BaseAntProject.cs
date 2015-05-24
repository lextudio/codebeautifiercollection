using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;

namespace BeWise.SharpBuilderTools.Tools.Ant {

    public abstract class BaseAntProject {
        // *************************************************************************
        //                           Constructor
        // *************************************************************************

        protected BaseAntProject(string aFileName) {
            if (!File.Exists(aFileName)) {
                throw new Exception("Expected valid FileName: " + aFileName);
            }

            fFileName = aFileName;

            LoadDocument(aFileName);
        }

        // *************************************************************************
        //                               Private
        // *************************************************************************

        private string fDefault;
        private string fFileName;
        private string fProjectName;
        private BaseAntProperty[] fProperties;
        private BaseAntTarget[] fTargets;
        //private XmlDocument fXml;

        private void LoadDocument(string aFileName) {
            ArrayList _TargetList = new ArrayList();
            ArrayList _PropertyList = new ArrayList();
            XmlTextReader _Reader = null;
            try {
                // Load the reader with the data file and ignore all white space nodes.
                _Reader = new XmlTextReader(File.OpenRead(aFileName));
                _Reader.WhitespaceHandling = WhitespaceHandling.None;

                // Parse the file and display each of the nodes.
                while (_Reader.Read()) {
                    switch (_Reader.NodeType) {
                    case XmlNodeType.Element:
                        if (_Reader.Name == "project") {
                            fProjectName = _Reader.GetAttribute("name");
                            fDefault = _Reader.GetAttribute("default");
                        } else if (_Reader.Name == "target") {
                            BaseAntItem _Target = CreateAntTarget(_Reader.GetAttribute("name"), _Reader.GetAttribute("description"));
                            _Target.ColNumber = _Reader.LinePosition;
                            _Target.LineNumber = _Reader.LineNumber;
                            _TargetList.Add(_Target);
                        } else if (_Reader.Name == "property") {
                            BaseAntItem _Property = CreateAntProperty(_Reader.GetAttribute("name"), _Reader.Value);
                            _Property.ColNumber = _Reader.LinePosition;
                            _Property.LineNumber = _Reader.LineNumber;
                            _PropertyList.Add(_Property);
                        }
                        break;
                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.ProcessingInstruction:
                    case XmlNodeType.Comment:
                    case XmlNodeType.XmlDeclaration:
                    case XmlNodeType.Document:
                    case XmlNodeType.DocumentType:
                    case XmlNodeType.EntityReference:
                    case XmlNodeType.EndElement:
                        break;
                    }
                }

                if (String.IsNullOrEmpty(fProjectName)) {
                    Lextm.Windows.Forms.MessageBoxFactory.Info("Invalid project !");
                }
            } finally {
                if (_Reader!=null)
                {    _Reader.Close();}
            }

            fTargets = (BaseAntTarget[]) _TargetList.ToArray(typeof(BaseAntTarget));
            fProperties = (BaseAntProperty[]) _PropertyList.ToArray(typeof(BaseAntProperty));
        }
        // *************************************************************************
        //                               Protected
        // *************************************************************************

        /*
        protected virtual BaseAntProperty BuildAntProperty(XmlNode aNode) {
            string _Name = "";
            string _Value = "";
            XmlAttribute _Attr = null;

            _Attr = (XmlAttribute) aNode.Attributes.GetNamedItem("name");

            if (_Attr != null) {
                _Name = _Attr.Value; 
            } else {
                _Name = "Undefined";
            }

            _Attr = (XmlAttribute) aNode.Attributes.GetNamedItem("value");

            if (_Attr != null) {
                _Value = _Attr.Value; 
            } 

            BaseAntProperty _result = CreateAntProperty(_Name, _Value);

            return _result;
        }

        protected virtual BaseAntProperty[] BuildAntProperties() {
            ArrayList _ls = new ArrayList();
            BaseAntProperty[] _result;

            XmlNode _Root = null;

            _Root = fXml.DocumentElement;

            for (int i = 0; i < _Root.ChildNodes.Count; i++) {
                if (_Root.ChildNodes[i].Name == "property") {
                    _ls.Add(BuildAntProperty(_Root.ChildNodes[i]));
                }
            }

            _result = new BaseAntProperty[_ls.Count];
            for (int j = 0; j < _ls.Count; j++) {
                _result[j] = _ls[j] as BaseAntProperty;
            }
            
            return _result;
        }
        */
        protected virtual BaseAntTarget BuildAntTarget(string aName, string aDescription) {
            if (aName == null) {
                throw new Exception("Expected target name");
            }

            BaseAntTarget _Result = CreateAntTarget(aName, aDescription);

            return _Result;
        }

        /*
        protected virtual BaseAntTarget BuildAntTarget(XmlNode aNode) {
            string _Description = "";
            string _Name = "";
            XmlAttribute _Attr = null;

            _Attr = (XmlAttribute) aNode.Attributes.GetNamedItem("name");

            if (_Attr != null) {
                _Name = _Attr.Value;
            } else {
                throw new Exception("Expected target name");
            }

            _Attr = (XmlAttribute) aNode.Attributes.GetNamedItem("description");

            if (_Attr != null) {
                _Description = _Attr.Value;
            }

            //BaseAntTarget _result = CreateAntTarget(_Name, _Description, aNode.OuterXml);

            //return _result;
            return null;
        }

        protected virtual BaseAntTarget[] BuildAntTargets() {
            ArrayList _ls = new ArrayList();
            BaseAntTarget[] _result;
            XmlNode _Root = null;

            _Root = fXml.DocumentElement;

            for (int i = 0; i < _Root.ChildNodes.Count; i++) {
                if (_Root.ChildNodes[i].Name == "target") {
                    _ls.Add(BuildAntTarget(_Root.ChildNodes[i]));
                }
            }

            _result = new BaseAntTarget[_ls.Count];
            for (int j = 0; j < _ls.Count; j++) {
                _result[j] = _ls[j] as BaseAntTarget;
            }
            
            return _result;
        }

        protected virtual void FillAntProject() {
            XmlNode  _Node = null;
            XmlAttribute _Attr = null;

            if (fXml.DocumentElement != null) {               
                _Node = fXml.DocumentElement;

                if (_Node.Name == "project") {
                    // Set the Name value
                    _Attr = (XmlAttribute) _Node.Attributes.GetNamedItem("name");

                    if (_Attr != null) {
                        fProjectName = _Attr.Value;                     
                    }

                    // Set the Default value
                    _Attr = (XmlAttribute) _Node.Attributes.GetNamedItem("default");

                    if (_Attr != null) {
                        fDefault = _Attr.Value;                     
                    }

                    // Build the Target
                    fTargets = BuildAntTargets();
                    fProperties = BuildAntProperties();
                } else {
                    Message/Box.Show("Invalid project");
                }

            }
        }
        */

		protected abstract BaseAntProperty CreateAntProperty(string aName, string aValue);

        protected abstract BaseAntTarget CreateAntTarget(string aName, string aDescription);

        // *************************************************************************
        //                                Properties
        // *************************************************************************

        public string Default {
            get {
                return fDefault;
            }
        }

		internal string FileName {
            get {
                return fFileName;
            }
            set {
                fFileName = value;
            }
        }

        public string ProjectName {
            get {
                return fProjectName;
            }
        }

		[Browsable(false)]
		internal BaseAntProperty[] Properties {
            get {
                return fProperties;
            }
        }

        [Browsable(false)]
		internal BaseAntTarget[] Targets {
			get {
                return fTargets;
            }
        }

        /*
        [Browsable(false)]
        public XmlDocument Xml {
            get {
                return fXml;
            }
        }
        */
    }
}
