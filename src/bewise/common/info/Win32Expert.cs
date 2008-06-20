using System;
using BeWise.Common.Utils;

namespace BeWise.Common.Info
{
	public class Win32Expert : ExpertInfo
	{
	    /// <summary>
	    /// General constructor.
	    /// </summary>
	    /// <param name="name">Name</param>
	    /// <param name="fileName">File name</param>
	    /// <param name="isActive">Is active</param>
	    /// <param name="version">IDE version</param>
	    public Win32Expert(string valueName, string valueData, bool isActive, int version) : base(valueName, valueData, isActive, version) { }
	
	    public override void Delete()
	    {
	        DeleteKey(Active ? OtaUtils.GetEnabledPackagesRegKey(Version) : OtaUtils.GetDisabledPackagesRegKey(Version));
	    }
	
	    /// <summary>
	    /// Activates an expert.
	    /// </summary>
	    public override void Activate()
	    {
	        if (Active)
	        {
	            return;
	        }
	        WriteKey(OtaUtils.GetEnabledPackagesRegKey(Version));
	        DeleteKey(OtaUtils.GetDisabledPackagesRegKey(Version));
	    }
	    /// <summary>
	    /// Deactivates an expert.
	    /// </summary>
	    public override void Deactivate()
	    {
	        if (!Active)
	        {
	            return;
	        }
	        WriteKey(OtaUtils.GetDisabledPackagesRegKey(Version));
	        DeleteKey(OtaUtils.GetEnabledPackagesRegKey(Version));
	    }
	
	    public override string FileName
	    {
	        get { return ValueData; }
	    }
	
	    public override string Name
	    {
	        get { return ValueName; }
	    }
	}
}
