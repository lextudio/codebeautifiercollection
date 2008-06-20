using System;
using BeWise.Common.Utils;

namespace BeWise.Common.Info
{
	public class DotNetExpert : ExpertInfo
	{
	    /// <summary>
	    /// General constructor.
	    /// </summary>
	    /// <param name="name">Name</param>
	    /// <param name="fileName">File name</param>
	    /// <param name="isActive">Is active</param>
	    /// <param name="version">IDE version</param>
	    public DotNetExpert(string valueName, string valueData, bool isActive, int version) : base(valueName, valueData, isActive, version) { }
	
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
	
	    public override string FileName
	    {
	        get { return ValueName; }
	    }
	
	    public override string Name
	    {
	        get { return ValueData; }
	    }
	}
}
