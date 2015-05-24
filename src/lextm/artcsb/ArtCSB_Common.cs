using System.Reflection;
using System.Resources;

namespace ArtCSB
{
	internal class Common
	{
		internal static ResourceManager MyRes;
		//---------------------------------------------------------------------------
		internal static void InitResourceManager(){
			Common.MyRes = new ResourceManager("ArtCSB.src.lextm.artcsb.ArtCSB_MyResource",
				Assembly.GetExecutingAssembly());
		}
		private Common () {} 
	}
}
