using System;
using BeWise.Common.Utils;

namespace BeWise.Common.Info
{
    /// <summary>
    /// This class represents a Win32 expert.
    /// </summary>
	public class Win32Expert : ExpertInfo
	{
	    /// <summary>
	    /// General constructor.
	    /// </summary>
	    /// <param name="valueName">Value name.</param>
	    /// <param name="valueData">Value data.</param>
	    /// <param name="isActive">Active flag.</param>
	    /// <param name="version">IDE version.</param>
	    public Win32Expert(string valueName, string valueData, bool isActive, int version) : base(valueName, valueData, isActive, version) { }
	
	    /// <summary>
	    /// Deletes the expert key.
	    /// </summary>
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
	
	    /// <summary>
	    /// File name.
	    /// </summary>
	    public override string FileName
	    {
	        get { return ValueData; }
	    }
	
	    /// <summary>
	    /// Name.
	    /// </summary>
	    public override string Name
	    {
	        get { return ValueName; }
	    }
	}
}
