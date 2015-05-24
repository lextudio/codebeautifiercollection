using System;
using System.Windows.Forms;
using BeWise.Common.Utils;
using Lextm.LeXDK;
using Lextm.Windows.Forms;

namespace ArtCSB
{
	/// <summary>
	/// Summary description for Class.
	/// </summary>
	class Utils_AutoSave
	{
		private static Timer FTimer = new Timer();
		private static bool FTimerTickAssigned = false;
		private static bool FTimerBusy = false;
		
		internal static void SetTimer(){
			if (FTimer.Enabled){
				FTimer.Enabled = false;
			}
			if ((bool)PropertyRegistry.Get("AutoSave")){
				if ((bool)PropertyRegistry.Get("AutoSave_DelayInMinutes")){
					FTimer.Interval = (int)PropertyRegistry.Get("AutoSave_Delay") * 1000 * 60;
				}else{
					FTimer.Interval = (int)PropertyRegistry.Get("AutoSave_Delay") * 1000;
				}
				if (!FTimerTickAssigned){
					FTimer.Tick += new EventHandler(OnTimer);
					FTimerTickAssigned = true;
				}
				FTimer.Start();
			}
		}

		private static void OnTimer(object sender, EventArgs e) {
			if (FTimerBusy){
				return;
			}
			FTimerBusy = true;
			try{ 
				if ( OTAUtils.GetCurrentEditActions() != null ){
					try{
						if ((bool)PropertyRegistry.Get("AutoSave_PlaySound")){
							if ((bool)PropertyRegistry.Get("AutoSave_Beep")){
								WinAPI.Beep((uint)PropertyRegistry.Get("AutoSave_BeepFrequency"),
								            (uint)PropertyRegistry.Get("AutoSave_BeepDurationMs"));
							}else{
								WinAPI.PlaySound(PropertyRegistry.Get("AutoSave_Wav") as string);
							}
						}
					}catch{}
					OTAUtils.GetCurrentEditActions().SaveAll();
				}
			}catch{
				MessageBoxFactory.Error("There was error during saving project.");
			}
			FTimerBusy = false;
		}
		private Utils_AutoSave() {}

	}
}
