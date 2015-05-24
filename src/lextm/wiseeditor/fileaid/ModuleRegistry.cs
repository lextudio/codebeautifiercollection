using System;
using BeWise.Common.Info;
using BeWise.Common.Utils;
using Borland.Studio.ToolsAPI;
using System.Collections.Generic;

namespace Lextm.WiseEditor.FileAid
{
	/// <summary>
	/// Description of ModuleRegistry.
	/// </summary>
	class ModuleRegistry
	{
        private ModuleRegistry() { }

        static ModuleRegistry() {
			RefreshList();
		}
		private static IDictionary<string, ModuleFileInfo> list = new Dictionary<string, ModuleFileInfo>();
		// Workaround for Octane
		private static bool IsValidModuleInfo(IOTAModuleInfo aModuleInfo){
			try {
				if (!String.IsNullOrEmpty(aModuleInfo.FileName)){
					return true;
				} else {
					return false;
				}
			} catch {
				return false;
			}
		}

		internal static void RefreshList()
		{
			list.Clear();

            IOTAProjectGroup _ProjectGroup = OtaUtils.GetCurrentProjectGroup();

            if (_ProjectGroup != null) {
                for (int j= 0; j < _ProjectGroup.ProjectCount; j++) {
                    IOTAProject _Project = _ProjectGroup[j];

                    for (int k = 0; k < _Project.ModuleCount; k++) {
                        IOTAModuleInfo _ModuleInfo = _Project.GetModuleInfo(k);
                        // TODO: what if the file is required by a certain project? not only the first project including it.
                        if (!IsValidModuleInfo(_ModuleInfo)) {
                            continue;
                        }
                        string key = _ModuleInfo.FileName.ToUpperInvariant();
                        if (list.ContainsKey(key))
                        {
                            list[key].ProjectFileName += ";" + _Project.FileName;
                        }
                        else
                        {
                            ModuleFileInfo _ModuleFileInfo = new ModuleFileInfo();
                            _ModuleFileInfo.ProjectFileName = _Project.FileName;
                            //_ModuleFileInfo.IsPartOfActiveProject = _Project == _ProjectGroup.ActiveProject;
                            _ModuleFileInfo.Tag = _ModuleInfo;
                            list.Add(_ModuleInfo.FileName.ToUpperInvariant(), _ModuleFileInfo);
                        }
                    }
                }
            }
		}
		
		internal static IEnumerable<ModuleFileInfo> GetList() {
			return list.Values;
		}
	}
}
