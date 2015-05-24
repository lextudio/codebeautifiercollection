// this is the ota features class.
// Copyright (C) 2005-2006  Lex Y. Li
// 
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
namespace Lextm.CodeBeautifierCollection.Collections {

	using System;
	using System.IO;
	using BeWise.Common.Utils;
	using Lextm.Diagnostics;
	using Lextm.OpenTools;
	using Lextm.OpenTools.Elements;
    using System.Globalization;
    using System.Collections.Generic;
	///<summary>
	///OTA creator.
	/// </summary>
	/// <version>1</version>
	/// <since>2006-1-17</since>
	/// <author>lextm</author>
	sealed class FeatureBuilder {

#region configuration
		///<summary>
		///Refreshes OTA preferences.
		///</summary>
		private void RefreshPreferences( ) {
			foreach( ILoadableFeature _OTA in features) {
				_OTA.RefreshPreferences();
			}
		}

		///<summary>
		///Sets OTA default preferences.
		///</summary>
		private void SetDefaultPreferences( ) {
			foreach (ILoadableFeature _OTA in features) {
				_OTA.SetDefaultPreferences();
			}
		}
		///<summary>
		///Loads OTA preferences.
		///</summary>
		private void LoadPreferences( ) {
		
			LoggingService.EnterMethod();

			foreach (ILoadableFeature feature in features) {
				feature.LoadPreferences();
			}
			LoggingService.LeaveMethod();						
		
		}
		///<summary>
		///Saves OTA preferences.
		///</summary>
		internal static void SavePreferences( ) {
		
			LoggingService.EnterMethod();
			
			getInstance().SetDefaultPreferences();
			foreach (ILoadableFeature feature in getInstance().features) {
				feature.SavePreferences();
			}
			PropertyRegistry.Flush();
			LoggingService.LeaveMethod();					
		
		}
		internal static void Refresh() {
			getInstance().RefreshPreferences();
		}
		///<summary>Reloads configuration.</summary>		
		internal static void ReloadPreferences( ) {
			getInstance().LoadPreferences();
			getInstance().SetDefaultPreferences();
			getInstance().RefreshPreferences();
		}
#endregion
		///<summary>
		///Registers.
		///</summary>
		internal static void Register() {
			Stopwatch watch = new Stopwatch();
			watch.Start();
			getInstance().DefineFeatures();
			LoggingService.Debug(String.Format(CultureInfo.InvariantCulture, "Last step ends at {0}-ms.", watch.Interval));
			getInstance().BuildFeatures();
			//getInstance().registered = true;
			LoggingService.Debug(String.Format(CultureInfo.InvariantCulture, "Last step ends at {0}-ms.", watch.Interval));
			watch.Stop();
		}
		
//		private static Mode mode = Mode.Integrated;
//		/// <summary>
//		/// Mode.
//		/// </summary>
//		internal static Mode Mode {
//            get {
//				return mode;
//			}
//			set {
//				mode = value;
//            }
//        }

		private void BuildFeatures( ) {

			theMenuTree.Build( );	

            theTabTree.Build();
		}

		private void DefineFeatures( ) {
		
			LoggingService.EnterMethod();
			foreach (ILoadableFeature ota in features) {
				if (ota != null) {
					RegisterFeature(ota);
				}
			}	
			
			LoggingService.LeaveMethod();
		
		}

		private void RegisterFeature( ILoadableFeature feature ) {
		
			LoggingService.EnterMethod();
			
			feature.Register();
			RegisterMenus(feature);
			RegisterTabs(feature);
			
			LoggingService.LeaveMethod();
        
		}

		private void RegisterTabs( ILoadableFeature feature ) {
			foreach (TabNode node in feature.TabNodes) {
				theTabTree.AddNode( node );
			}
		}

		private void RegisterMenus( ILoadableFeature feature ) {
		
			LoggingService.EnterMethod();
			
			foreach (CustomMenuNode action in feature.Menus) {
				if (action != null) {
					theMenuTree.AddNode( action );
				} else {
					LoggingService.Warn("null action");
				}
			}
			
			LoggingService.LeaveMethod();
		
		}

		private TabBuilder theTabTree = TabBuilder.getInstance();
		private MenuBuilder theMenuTree = MenuBuilder.getInstance();
#region singleton		
		private FeatureBuilder( ) {
		
			LoggingService.EnterMethod();
			
			AddFramework();			
			AddPlus();

			LoggingService.LeaveMethod();
	
		}

		private IList<ILoadableFeature> features = new List<ILoadableFeature>();

		private void AddFramework() {
		
			LoggingService.EnterMethod();
			
			// should be the first item.
			features.Add(new Feature.Framework());
						
			LoggingService.LeaveMethod();
		
		}	
		
		private void AddPlus( ) {
		
			LoggingService.EnterMethod();
			
			FeatureRegistry.Initiate(OtaUtils.CurrentIdeVersion);
			
			foreach(ILoadableFeature feature in
					FeatureRegistry.Features) {
				features.Add(feature);
			}
		
			LoggingService.LeaveMethod();
	
		}

		private static FeatureBuilder instance;

		///<summary>
		///Gets singleton instance.
		///</summary>
		/// <returns>
		///Singleton instance.
		///</returns>
		static FeatureBuilder getInstance( ) {
			lock(typeof(FeatureBuilder)){
				if (instance == null)
				{
					instance = new FeatureBuilder();
				}
			}
			return instance;
		}
#endregion
	}
}
