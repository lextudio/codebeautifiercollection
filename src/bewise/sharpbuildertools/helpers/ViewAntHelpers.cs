using System;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI; 
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Tools;
using BeWise.SharpBuilderTools.Tools.Ant;

namespace BeWise.SharpBuilderTools.Helpers {

    public abstract class BaseViewAntHelper : BaseViewHelper {

        /**************************************************************/
        /*                      Constructor
        /**************************************************************/

        protected BaseViewAntHelper(BaseAntProject aAntProject, BaseTool aCurrentAnt) {
            fCurrentAnt = aCurrentAnt;
            fAntProject = aAntProject;
        }

        /**************************************************************/
        /*                      Private
        /**************************************************************/

        private BaseTool fCurrentAnt;
        private BaseAntProject fAntProject;

        /**************************************************************/
        /*                      Public
        /**************************************************************/

        public override int Compare(object x, object y) {
            BaseAntItem _AntItemX = (BaseAntItem) x;
            BaseAntItem _AntItemY = (BaseAntItem) y;

            return String.Compare(_AntItemX.Name, _AntItemY.Name);
        }

        public override void GoTo(object aItem) {
            BaseAntItem _AntItem = (BaseAntItem) aItem;
            int _LineNumber = _AntItem.LineNumber;

            if (_LineNumber > 0) {
                IOTAModuleInfo _ModuleInfo = OTAUtils.GetModuleInfoFromProject(OTAUtils.GetCurrentProject(), AntProject.FileName);
				if (_ModuleInfo == null) {
                    return;
                }
				IOTAModule _Module = _ModuleInfo.OpenModule();
                _Module.ShowFileName(AntProject.FileName);

                IOTASourceEditor _SourceEditor = OTAUtils.GetSourceEditor(_Module);
				if (_SourceEditor != null) {
					IOTAEditView _EditView = _SourceEditor.GetEditView(0);

					OTAUtils.GoToPosition(_SourceEditor, _LineNumber, (short) _AntItem.ColNumber);
					_EditView.MoveViewToCursor();
					_EditView.Paint();
                }
            }
        }

        public override bool ItemIsVisible(string aFilter, string aProjectFileName, object aItem) {
            BaseAntItem _AntItem = (BaseAntItem) aItem;
            return ((_AntItem.Name.ToUpperInvariant()).IndexOf(aFilter.ToUpperInvariant()) > -1);
        }

        /**************************************************************/
        /*                        Properties
        /**************************************************************/
        public BaseTool CurrentAnt {
            get {
                return fCurrentAnt;
            }
        }

        public BaseAntProject AntProject {
            get {
                return fAntProject;
            }
        }

    }

    public class ViewAllAntHelper  : BaseViewAntHelper {

        /**************************************************************/
        /*                      Constructor
        /**************************************************************/
        public ViewAllAntHelper(BaseAntProject aAntProject, BaseTool aCurrentAnt) : base (aAntProject, aCurrentAnt) {
        }

        /**************************************************************/
        /*                      Public
        /**************************************************************/

		protected override ListViewItem CreateListViewItem(object aItem, object[] Params) {
            BaseAntItem _AntItem = (BaseAntItem) aItem;

            ListViewItem _ListViewItem = new ListViewItem();

            _ListViewItem.Text = _AntItem.AntTypeName;
            _ListViewItem.SubItems.Add(_AntItem.Name);
            _ListViewItem.SubItems.Add(_AntItem.LineNumber.ToString());
            _ListViewItem.SubItems.Add(_AntItem.ColNumber.ToString());

            _ListViewItem.Tag = aItem;

            return _ListViewItem;
        }

        public override void Load() {
            if (AntProject != null) {
                Items.AddRange(AntProject.Targets);
                Items.AddRange(AntProject.Properties);
            }
        }
    }

    public class ViewAntTargetsHelper  : BaseViewAntHelper {

        /**************************************************************/
        /*                      Constructor
        /**************************************************************/

        public ViewAntTargetsHelper(BaseAntProject aAntProject, BaseTool aCurrentAnt) : base (aAntProject, aCurrentAnt) {
        }

        /**************************************************************/
        /*                      Public
        /**************************************************************/

		protected override ListViewItem CreateListViewItem(object aItem, object[] Params) {
            BaseAntItem _AntItem = (BaseAntItem) aItem;

            ListViewItem _ListViewItem = new ListViewItem();

            _ListViewItem.Text = _AntItem.Name;
            _ListViewItem.SubItems.Add(_AntItem.LineNumber.ToString());
            _ListViewItem.SubItems.Add(_AntItem.ColNumber.ToString());

            _ListViewItem.Tag = aItem;

            return _ListViewItem;
        }

        public override void Load() {
            if (AntProject != null) {
                Items.AddRange(AntProject.Targets);
            }
        }
    }

    public class ViewAntPropertiesHelper : BaseViewAntHelper {

        /**************************************************************/
        /*                      Constructor
        /**************************************************************/

        public ViewAntPropertiesHelper(BaseAntProject aAntProject, BaseTool aCurrentAnt) : base (aAntProject, aCurrentAnt) {
        }

        /**************************************************************/
        /*                      Public
        /**************************************************************/

        protected override ListViewItem CreateListViewItem(object aItem, object[] Params) {
            BaseAntItem _AntItem = (BaseAntItem) aItem;

            ListViewItem _ListViewItem = new ListViewItem();

            _ListViewItem.Text = _AntItem.Name;
            _ListViewItem.SubItems.Add(_AntItem.LineNumber.ToString());
            _ListViewItem.SubItems.Add(_AntItem.ColNumber.ToString());

            _ListViewItem.Tag = aItem;

            return _ListViewItem;
        }

        public override void Load() {
            if (AntProject != null) {
                Items.AddRange(AntProject.Properties);
            }
        }
    }
}
