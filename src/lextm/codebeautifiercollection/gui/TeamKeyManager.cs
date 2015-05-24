using System;
using System.ComponentModel;

using System.Diagnostics;
using System.Windows.Forms;
using BeWise.SharpDevTools.Component;

namespace Lextm.CodeBeautifierCollection.Gui
{
	/// <summary>
	/// Summary description for Component.
	/// </summary>
    [CLSCompliantAttribute(false)]
	public class TeamKeyManager : SecretKeyManager {

    	public TeamKeyManager(System.ComponentModel.IContainer container)
		{
			container.Add(this);
		}
    	
    	public TeamKeyManager() { }

    	public override void ShowHelp() {
			DavidTheCreator david = new DavidTheCreator();
            david.ShowDialog();
        }

	}
}
