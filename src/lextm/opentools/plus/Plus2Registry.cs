// this is plus array list class.
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

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Lextm.Diagnostics;
using Lextm.Xml;

namespace Lextm.OpenTools.Plus {

	/// <summary>
	/// Plus list.
	/// </summary>
	public class Plus2Registry {

		IList<Plus2> collection = new List<Plus2>();
		/// <summary>
		/// Plus collection.
		/// </summary>
		public IEnumerable<Plus2> Plus2Collection
		{
			get
			{
				return collection;
			}
		}

		private bool loaded;

		private Plus2Registry( ) {	}
		/// <summary>Loads .plus2 files.</summary>
		public void LoadPluses() {
			if (!loaded)
			{
				foreach(string fileName in Plus2.GetPlus2Files()) {
					Plus2 plus = (Plus2)SerializationService.Load(fileName, typeof(Plus2));
					plus.ModuleName = Path.Combine(
						Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
						Path.GetFileNameWithoutExtension(fileName));
					collection.Add(plus);
				}
				loaded = true;
			} else {
				// .plus2 files loads only once.
				LoggingService.Warn("Plus list is already loaded.");
			}
		}
		//private const string MaskPlus2 = "{0}.plus2";
		/// <summary>
		/// Generates .plus2 files.
		/// </summary>
		public static void GeneratePlus2Files() {
			foreach(Plus2 plus in Instance.collection) {
				SerializationService.Save(
					OpenTools.IO.Path.GetPreferencesFile(Path.GetFileName(plus.ModuleName) + ".plus2"), plus);
			}
		}

		internal static void AddFeaturesTo(IDictionary<string, ILoadableFeature> table, int version) {
			
			Plus2Registry.Instance.LoadPluses();

			foreach(Plus2 plus in Plus2Registry.Instance.collection) {
				if (!plus.IsEnabledFor(version)) {
					MessageService.Show("plus is disabled: " + plus.Name);
					continue;
				}
				
				Assembly assembly = Plus2.LoadAssembly(plus.ModuleName);
				if (assembly == null) {
					MessageService.Show("No module is available: " + plus.ModuleName);
				}
				else {
					//Console.WriteLine(assembly);
					
					foreach(Feature2 feature in plus.Features) {
						//Console.WriteLine("the feature is : " + feature.Name);
						if (feature.IsEnabledFor(version)) {
							//Console.WriteLine("the feature is enabled");
							ILoadableFeature featureObject = Feature2.LoadFeature(feature.Name, assembly);
							if (featureObject != null) {
								Console.WriteLine(featureObject);
								if (!table.ContainsKey(feature.Name)) {
									table.Add(feature.Name, featureObject);
								} else {
									LoggingService.Warn(feature.Name + " is already there.");
								}
							}
							else {
								MessageService.Show("Feature object cannot be loaded: " + feature.Name);
							}
						}
						else {
							//Console.WriteLine("the feature is disabled");
						}
					}
				}
			}
		}

		#region Singleton
		private static Plus2Registry instance;
		/// <summary>
		/// Gets singleton instance.
		/// </summary>
		/// <returns>Singleton instance.</returns>
		public static Plus2Registry Instance {
			get {
				lock (typeof(Plus2Registry)) {
					if (instance == null) {
						instance = new Plus2Registry();
					}
				}
				return instance;
			}
		}
		#endregion
	}
}
