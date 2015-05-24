// this is the property registry class which manages simple type data used in most features.
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
using System.Reflection;
using System.IO;

using Lextm.Diagnostics;
using Lextm.Xml;
using System.Collections;

namespace Lextm.OpenTools
{
    /// <summary>
    /// Registry for all properties.
    /// </summary>
    public sealed class PropertyRegistry
    {
        static string fileName;
        
        static string FileName 
        {
            get {
                if (String.IsNullOrEmpty(fileName))
                {
                    fileName = IO.Path.GetPreferencesFile("all.ota");
                }
                return fileName;
            }
        }
		
        private static Stopwatch globalOne;

        static PropertyRegistry()
        {
        	try
        	{
        		table = SerializationService.Load(FileName) as Hashtable;
        	} catch (Exception ex) {
                //TODO: make it more specific
        		LoggingService.Error(ex);
        	}
            if (table == null)
            {
                table = new Hashtable();
            }
            globalOne = new Stopwatch();
            globalOne.Start();
            globalOne.Suspend();
        }
        private PropertyRegistry() {}

        private static Hashtable table;
        private static bool autoFlush = true;
#region interface
		/// <summary>
		/// Gets a property.
		/// </summary>
		/// <param name="key">Key</param>
		/// <returns></returns>
        public static object Get(string key)
        {
            return Get(key, null);
        }
		/// <summary>
		/// Gets a property with default value.
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="defaultValue">Default value</param>
		/// <returns></returns>
		/// <remarks>If the value does not exist, create it with the default value.</remarks>
        public static object Get(string key, object defaultValue)
        {
            if (table.Contains(key))
            {
                return table[key];
            }
            else if (defaultValue != null)
            {
                table.Add(key, defaultValue);
                if (AutoFlush)
                {
                	FlushToDisk();
                }
                return defaultValue;
            }
            return null;
        }
		/// <summary>
		/// Sets a property.
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="value">Value</param>
        public static void Set(string key, object value)
        {
            if (table.Contains(key))
            {
                table[key] = value;
            }
            else
            {
                table.Add(key, value);
            }
            if (AutoFlush)
            {
            	FlushToDisk();
            }
        }
		/// <summary>
		/// AutoFlushes to disk.
		/// </summary>
		/// <remarks>If set to true, after every changes the settings are flushed to disk. 
		/// If set to false, call Flush() when you need a saving.</remarks>
        public static bool AutoFlush
        {
            get
            {
                return autoFlush;
            }
            set
            {
                autoFlush = value;
            }
        }
		/// <summary>
		/// Flushes to disk.
		/// </summary>
        public static void Flush()
        {
        	if (!AutoFlush) {
        		FlushToDisk();
        	}
        }
        private static void FlushToDisk()
        {
        	LoggingService.EnterMethod();
        	SerializationService.Save(table, FileName);
			LoggingService.LeaveMethod();
        }

#endregion

    }
}




