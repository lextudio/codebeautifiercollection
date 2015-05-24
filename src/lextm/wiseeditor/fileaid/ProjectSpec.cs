using System;
using BeWise.Common.Info;

namespace Lextm.WiseEditor.FileAid {
    /// <summary>
    /// Summary description for Class.
    /// </summary>
	class ProjectSpec : Spec {

		private string project;

		internal ProjectSpec(string project) {
			// three kinds of input: Any, Active, *.bdsproj
			this.project = project;
		}

		internal override bool IsSatisfiedBy(ModuleFileInfo file) {
			if (project == AnyProject) {
				return true;
			} else if (project == ActiveProject) {
				return file.IsPartOfActiveProject;
            } else {
				return file.ProjectFileName.Contains(project);
            }
		}
		internal const string AnyProject = "Any";
		internal const string ActiveProject = "Active";
	}
}


