// this is the feature2 class.
// Copyright (C) 2006  Lex Y. Li
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
using System.Reflection;
using Lextm.Diagnostics;

namespace Lextm.OpenTools.Plus {
	///<summary>
	/// Feature record.
	/// </summary>
	[Serializable]
	public class Feature2 {

		/// <summary>
		/// Constructor.
		/// </summary>
		public Feature2() { }

		private EnabledRecord[] enabledRecords;
		private string name;
		private string description;
		/// <summary>
		/// Enabled.
		/// </summary>
		public EnabledRecord[] EnabledRecords {
			get {
				return enabledRecords;
			}
			set {
				enabledRecords = value;
			}
		}
		/// <summary>
		/// Name.
		/// </summary>
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}
		/// <summary>
		/// Description.
		/// </summary>
		public string Description {
			get {
				return description;
			}
			set {
				description = value;
			}
		}
		/// <summary>
		/// Gets enabled.
		/// </summary>
		/// <param name="version">IDE version</param>
		/// <returns></returns>
		public bool IsEnabledFor(int version) {
			foreach (EnabledRecord  r in enabledRecords)
			{
				if (version == r.Version)
				{
					return r.Enabled;
				}
			}
			// no record, set false by default;
			return false;
		}
		/// <summary>
		/// Sets enabled.
		/// </summary>
		/// <param name="version">IDE version</param>
		/// <param name="enabled">Enabled</param>
		public void SetEnabled(int version, bool enabled) {
			foreach (EnabledRecord r in enabledRecords)
			{
				if (version == r.Version)
				{
					r.Enabled = enabled;
				}
			}
		}

		/// <summary>Loads feature object.</summary>
		internal static ILoadableFeature LoadFeature(string fullName, Assembly assembly) {
			ILoadableFeature result = null;
			object temp = null;
			Type type = assembly.GetType(fullName);

			if (type != null) {
				try {
					//MethodInfo method = type.GetMethod("getInstance");
					temp = Activator.CreateInstance(type);//method.Invoke(null, null);
				}
				catch (TargetInvocationException ex)
				{
					LoggingService.Error(ex.ToString());
					MessageService.Show("Invoke exception so instance is not loaded: "
					                    + assembly.GetName() + ";" + fullName);
				}
				catch (MethodAccessException ex)
				{
					LoggingService.Error(ex.ToString());
					MessageService.Show("Invoke exception so instance is not loaded: "
					                    + assembly.GetName() + ";" + fullName);
				}
				catch (MemberAccessException ex)
				{
					LoggingService.Error(ex.ToString());
					MessageService.Show("Invoke exception so instance is not loaded: "
					                    + assembly.GetName() + ";" + fullName);
				}
				catch (TypeLoadException ex)
				{
					LoggingService.Error(ex.ToString());
					MessageService.Show("Invoke exception so instance is not loaded: "
					                    + assembly.GetName() + ";" + fullName);
				}
				finally {
					result = temp as ILoadableFeature;
					if (result == null) {
						MessageService.Show("ILoadableFeature is not implemented, feature is disabled: "
						                    + assembly.GetName() + ";" + fullName);
					}
				}
			}
			return result;
		}
	}
}
