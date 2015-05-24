using System;
using System.IO;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace Lextm.JcfExpert.Common {
    //-------------------------------------------------------------------------------
    public class Resources {
		public static ResourceManager MyRes;
        //---------------------------------------------------------------------------
        public static void InitResourceManager() {
            Resources.MyRes = new ResourceManager("Lextm.JcfExpertResource",
                                               Assembly.GetExecutingAssembly());
        }
    }  
}
