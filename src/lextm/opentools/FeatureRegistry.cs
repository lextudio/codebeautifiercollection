// this is the feature registry class.
// Copyright (C) 2006  Lex Y. Li
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;


namespace Lextm.OpenTools
{
	/// <summary>
	/// Registry of feature instances.
	/// </summary>
	/// <remarks><para>Use <see cref="Initiate"></see> to initiate it for a specific IDE version.</para>
	/// <para>Use <see cref="Features"></see> to get all features, and use <see cref="GetFeature"/> to find a specific feature.</para>
	/// </remarks>
	public sealed class FeatureRegistry
	{       
		private FeatureRegistry() {	}
		 
		private readonly static IDictionary<string, ILoadableFeature> table = new SortedList<string, ILoadableFeature>();
        /// <summary>
        /// Initiates.
        /// </summary>
        /// <param name="version">IDE version</param>
        public static void Initiate(int version) {
        	table.Clear();
        	// in different version, feature list is different.
        	Plus.Plus2Registry.AddFeaturesTo(table, version);
        }
        /// <summary>g
        /// Gets all features.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ILoadableFeature> Features {
            get
            {
                return table.Values;
            }
        }
        /// <summary>
        /// Gets a feature.
        /// </summary>
        /// <param name="name">Feature name</param>
        /// <remarks>Send a full qualified name as <paramref name="name"/>.</remarks>
        public static ILoadableFeature GetFeature(string name) {
        	if (table.ContainsKey(name)) {
        		return table[name] as ILoadableFeature;
        	} else {
        		return null;
        	}
        }
	}
}
