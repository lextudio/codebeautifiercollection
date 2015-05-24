using System;
using BeWise.Common.Info;
using System.Collections.Generic;

namespace Lextm.WiseEditor.FileAid
{
	/// <summary>
	/// Summary description for Class.
	/// </summary>
	sealed class ModuleFinder
	{
		ModuleFinder()
		{}

		internal static IEnumerable<ModuleFileInfo> SelectModules(Spec spec) {
			List<ModuleFileInfo> result = new List<ModuleFileInfo>();
			foreach(ModuleFileInfo file in ModuleRegistry.GetList()) {
				if (spec.IsSatisfiedBy(file)) {
					result.Add(file);
				}
			}
			result.Sort(ModuleInfoComparer.Instance);
			return result;
		}
		
		private class ModuleInfoComparer : IComparer<ModuleFileInfo> {
            ModuleInfoComparer() { }

            static ModuleInfoComparer instance;

            internal static ModuleInfoComparer Instance
            {
                get
                {
                    lock (typeof(ModuleInfoComparer))
                    {
                        if (instance == null)
                        {
                            instance = new ModuleInfoComparer();
                        }
                    }
                    return instance;
                }
            }

			public int Compare(ModuleFileInfo x, ModuleFileInfo y) {
				return String.Compare(x.FileName,
					y.FileName, StringComparison.OrdinalIgnoreCase);
			}
        }
	}
}
