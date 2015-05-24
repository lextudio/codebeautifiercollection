namespace ZetaLib.Core.Common
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Collections.Specialized;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// 
	/// </summary>
	public class UrlParser
	{
		#region Public methods.
		// ----------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="uri">The URI to construct with.</param>
		public UrlParser(
			Uri uri )
		{
			this.uri = uri;
			this.parameters = ParseParameters( this.uri );
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="uri">The URI to construct with.</param>
		public UrlParser(
			string uri )
		{
			this.uri = new Uri( uri, UriKind.RelativeOrAbsolute );
			this.parameters = ParseParameters( this.uri );
		}

		/// <summary>
		/// Parses the parameters.
		/// </summary>
		/// <param name="uri">The URI.</param>
		/// <returns></returns>
		private static StringDictionary ParseParameters(
			Uri uri )
		{
			StringDictionary result = new StringDictionary();

			if ( !string.IsNullOrEmpty( uri.Query ) )
			{
				string query = uri.Query.Trim( '&', '?' );
				string[] pairs = query.Split( '&' );

				foreach ( string pair in pairs )
				{
					if ( pair.Contains( @"=" ) )
					{
						string[] ab = pair.Split( '=' );
						result[ab[0]] = ab[1];
					}
					else
					{
						result[pair] = pair;
					}
				}
			}

			return result;
		}

		// ----------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ----------------------------------------------------------------------

		/// <summary>
		/// The associated URI.
		/// </summary>
		/// <value>The URI.</value>
		public Uri Uri
		{
			get
			{
				return uri;
			}
		}

		/// <summary>
		/// The complete URL before the parameters.
		/// </summary>
		/// <value>The before URL.</value>
		public string BeforeUrl
		{
			get
			{
				if ( string.IsNullOrEmpty( uri.OriginalString ) )
				{
					return uri.OriginalString;
				}
				else
				{
					if ( uri.OriginalString.Contains( @"?" ) )
					{
						return uri.OriginalString.Substring(
							0,
							uri.OriginalString.IndexOf( '?' ) );
					}
					else
					{
						return uri.OriginalString;
					}
				}
			}
		}

		/// <summary>
		/// Gets the parameters.
		/// </summary>
		/// <value>The parameters.</value>
		public StringDictionary Parameters
		{
			get
			{
				return parameters;
			}
		}

		// ----------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ----------------------------------------------------------------------

		private StringDictionary parameters;
		private readonly Uri uri;

		// ----------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////

}