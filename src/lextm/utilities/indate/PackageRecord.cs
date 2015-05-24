using System;

namespace Lextm.Utilities.InDate
{
	/// <summary>
	/// Update package record.
	/// </summary>
	class PackageRecord
	{
		/// <summary>Contructor.</summary>
		/// <param name="version">Version</param>
		/// <param name="link">Url</param>
		internal PackageRecord(Version version, Uri link)
		{
			this.version = version;
			this.link = link;
		}

        Uri link;
        Version version;
		/// <summary>
		/// Url.
		/// </summary>
        internal Uri Link
        {
            get
            {
                return link;
            }
        }
		/// <summary>
		/// Package version.
		/// </summary>
        internal Version Version
        {
            get
            {
                return version;
            }
        }
	}
}
