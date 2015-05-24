// this is the expert checker class.
// Copyright (C) 2005-2006  Lex Y. Li
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
using System;
using System.Diagnostics;
using BeWise.Common.Utils;
using Lextm.Diagnostics;

namespace Lextm.OpenTools
{
	/// <summary>
	/// ExpertChecker class.
	/// </summary>
	/// <remarks>This class checks whether certain experts are installed.</remarks>
	public sealed class ExpertChecker
	{
		private ExpertChecker()
		{
		}
		
        private const string RegKeyCastalia = @"\Known IDE Packages";
		/// <summary>
		/// Validates if Castalia is installed.
		/// </summary>
		/// <returns></returns>
        public static bool CastaliaInstalled() {

			LoggingService.EnterMethod();
        	bool result = false;
        	Microsoft.Win32.RegistryKey key = 
        		Microsoft.Win32.Registry.CurrentUser.OpenSubKey(OtaUtils.IdeRegKey + RegKeyCastalia);
        	if (key != null) {
        		LoggingService.Info("subkey number is " + key.SubKeyCount);
        		foreach(string name in key.GetValueNames()) {
        			LoggingService.Info("this key is " + name);
        			if (name.IndexOf("Castalia", StringComparison.Ordinal) != -1) {
        				result = true;
        				LoggingService.Info("found castalia");
        				break;
        			} //else {
        				//LoggingService.Warn("not found castalia");
        			//}        			
        		}        		
        	} else {
        		LoggingService.Warn("null key");
        	}
        	LoggingService.LeaveMethod();						

        	return result;
        }
        
		private const string MenuCNPack = "CnPackMenu";
		/// <summary>
		/// Validates if CnWizards is installed.
		/// </summary>
		/// <returns></returns>
		public static bool CNPackInstalled() {
			return OtaUtils.MenuExists(MenuCNPack);
		}		
	}
}
