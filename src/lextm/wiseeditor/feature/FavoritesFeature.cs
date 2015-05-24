using System;

using System.Windows.Forms;
using BeWise.Common.Utils;
using Lextm.OpenTools;
using Lextm.Xml;
using System.Collections;

namespace Lextm.WiseEditor.Feature {

	public class FavoritesFeature : CustomFeature {

		private static FavoriteRecords options;

		internal static FavoriteRecords GetOptions() {
			return options;
		}

		public override void LoadPreferences() {
			base.LoadPreferences();
			options = (FavoriteRecords)SerializationService.Load(
				Lextm.OpenTools.IO.Path.GetPreferencesFile(this.GetType()),
				typeof(FavoriteRecords));
		}

		public override void SavePreferences() {
			base.SavePreferences();
			SerializationService.Save(
				Lextm.OpenTools.IO.Path.GetPreferencesFile(this.GetType()),
				options);
		}
		/**************************************************************/
		/*                        Protected
		/**************************************************************/
		private Gui.FormFavorites _FormFavorites;

		private void DoFavorites(object aSender, EventArgs AEventArgs) {
			if ((_FormFavorites == null) || (_FormFavorites.IsDisposed) )
			{
				_FormFavorites = new Gui.FormFavorites();
			}
			_FormFavorites.Show();
			_FormFavorites.BringToFront();
		}

		/**************************************************************/
		/*                     Private
        /**************************************************************/
//		private void AddFavoriteCategory(ArrayList aCategoryList, string aName) {
//			FavoriteCategory _FavoriteCategory = new FavoriteCategory();
//			_FavoriteCategory.Name = aName;
//			aCategoryList.Add(_FavoriteCategory);
//		}

		private static void AddFavoriteFile(ArrayList aFileList, string aName, string aFileName) {
			FavoriteFile _FavoriteFile = new FavoriteFile();
			_FavoriteFile.Name = aName;
			_FavoriteFile.FileName = aFileName;
			aFileList.Add(_FavoriteFile);
		}

		private static void AddFrameworkFile(ArrayList aFileList, string aName) {
			string _FrameworkPath = OtaUtils.GetDotNetFrameworkInstallationPath();
			AddFavoriteFile(aFileList, aName,
			                System.IO.Path.Combine(_FrameworkPath, aName));
		}

		/**************************************************************/
		/*                     Public
        /**************************************************************/

		protected override void IdeRegisterMenus() {
			// Favorites
			RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                ShareUtils.MenuRootDefault,
			                                "FavoritesMenu",
			                                OtaUtils.Ctrl + OtaUtils.Alt + (int)Keys.F,
			                                "Favorites...",
			                                new EventHandler(DoFavorites)));
		}

		public override void SetDefaultPreferences() {
			base.SetDefaultPreferences();
			if (options == null)
			{
				options = new FavoriteRecords();
			}
			if (options.FavoriteCategories == null ||
			    options.FavoriteCategories.Length == 0) {
				ArrayList _CategoryList = new ArrayList();

				FavoriteCategory _FavoriteCategory;

				// Files
				_FavoriteCategory = new FavoriteCategory();
				_FavoriteCategory.Name = "Files";
				_CategoryList.Add(_FavoriteCategory);
				
				// Projects
				_FavoriteCategory = new FavoriteCategory();
				_FavoriteCategory.Name = "Projects";
				_CategoryList.Add(_FavoriteCategory);

				// References
				_FavoriteCategory = new FavoriteCategory();
				_FavoriteCategory.Name = "References";
				_CategoryList.Add(_FavoriteCategory);

				string _FrameworkPath = OtaUtils.GetDotNetFrameworkInstallationPath();
				if (!String.IsNullOrEmpty(_FrameworkPath)) {
					ArrayList _FileList = new ArrayList();

					AddFrameworkFile(_FileList, "System.dll");
					AddFrameworkFile(_FileList, "System.Data.dll");
					AddFrameworkFile(_FileList, "System.Data.OracleClient.dll");
					AddFrameworkFile(_FileList, "System.Design.dll");
					AddFrameworkFile(_FileList, "System.DirectoryServices.dll");
					AddFrameworkFile(_FileList, "System.Drawing.dll");
					AddFrameworkFile(_FileList, "System.EnterpriseServices.dll");
					AddFrameworkFile(_FileList, "System.Messaging.dll");
					AddFrameworkFile(_FileList, "System.Runtime.Remoting.dll");
					AddFrameworkFile(_FileList, "System.Runtime.Serialization.Formatters.Soap.dll");
					AddFrameworkFile(_FileList, "System.Security.dll");
					AddFrameworkFile(_FileList, "System.ServiceProcess.dll");
					AddFrameworkFile(_FileList, "System.Web.dll");
					AddFrameworkFile(_FileList, "System.Web.RegularExpressions.dll");
					AddFrameworkFile(_FileList, "System.Web.Services.dll");
					AddFrameworkFile(_FileList, "System.Windows.Forms.dll");
					AddFrameworkFile(_FileList, "System.XML.dll");

					_FavoriteCategory.FavoriteFiles = (FavoriteFile[]) _FileList.ToArray(typeof(FavoriteFile));
				}
				
				// Set the default Categories
				options.FavoriteCategories = (FavoriteCategory[]) _CategoryList.ToArray(typeof(FavoriteCategory));

			}
		}
	}
	/// <summary>
	/// Favorite records.
	/// </summary>
	[Serializable]
	public class FavoriteRecords {

		// *************************************************************************
		//                            Public Fields
		// *************************************************************************
		
		public FavoriteCategory[] FavoriteCategories;
		public string SelectedFavoriteCategoryPath;
		public string[] SelectedFilePath;
	}
	
	[Serializable]
	public class FavoriteCategory {
		
		// *************************************************************************
		//                            Public Fields
		// *************************************************************************
		
		public string Name;
		public bool Expanded;
		public FavoriteCategory[] FavoriteCategories;
		public FavoriteFile[] FavoriteFiles;
	}
	
	[Serializable]
	public class FavoriteFile {
		
		// *************************************************************************
		//                            Public Fields
		// *************************************************************************
		
		public string Name;
		public string FileName;
	}
}
