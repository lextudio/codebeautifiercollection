using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Lextm.OpenTools
{
	/// <summary>
	/// OpenTools core exception.
	/// </summary>
	[Serializable]
    public class CoreException: Exception
    {
    	/// <summary>
    	/// Constructor.
    	/// </summary>
    	public CoreException() { }
    	/// <summary>
    	/// Constructor.
    	/// </summary>
    	/// <param name="message">Message</param>
        public CoreException(string message) : base(message) { }
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">Message</param>
		/// <param name="ex">Inner exception</param>
        public CoreException(string message, Exception ex) : base(message, ex) { }   
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="si">Serialization info</param>
		/// <param name="sc">Streaming context</param>
        protected CoreException(SerializationInfo si, StreamingContext sc) : base(si, sc) { }
    }
}
