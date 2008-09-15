using System;
using BeWise.Common.Utils;

namespace BeWise.Common.Info
{
    /// <summary>
    /// This class represents a .NET expert.
    /// </summary>
	public class DotNetExpert : ExpertInfo
	{
	    /// <summary>
	    /// General constructor.
	    /// </summary>
	    /// <param name="valueData">Value data.</param>
	    /// <param name="valueName">Value name.</param>
	    /// <param name="isActive">Active flag.</param>
	    /// <param name="version">IDE version</param>
	    public DotNetExpert(string valueName, string valueData, bool isActive, int version) : base(valueName, valueData, isActive, version) { }
	
	    /// <summary>
	    /// Deletes the value pair.
	    /// </summary>
	    public override void Delete()
	    {
	        DeleteKey(Active ? OtaUtils.GetEnabledAssembliesRegKey(Version) : OtaUtils.GetDisabledAssembliesRegKey(Version));
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
	        WriteKey(OtaUtils.GetEnabledAssembliesRegKey(Version));
	        DeleteKey(OtaUtils.GetDisabledAssembliesRegKey(Version));
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
	        WriteKey(OtaUtils.GetDisabledAssembliesRegKey(Version));
	        DeleteKey(OtaUtils.GetEnabledAssembliesRegKey(Version));
	    }
	
	    /// <summary>
	    /// File name.
	    /// </summary>
	    public override string FileName
	    {
	        get { return ValueName; }
	    }
	
	    /// <summary>
	    /// Name.
	    /// </summary>
	    public override string Name
	    {
	        get { return ValueData; }
	    }
	}
}
