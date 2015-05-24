// this is the registry helper class.
// Copyright (C) 2005-2006  Lex Y. Li
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
using Microsoft.Win32;

namespace Lextm.Win32
{
    /// <summary>
    /// Base key.
    /// </summary>
    public enum BaseKey { 
        /// <summary>
        /// Default, current user.
        /// </summary>
        Default = 0, 
        /// <summary>
        /// Local machine.
        /// </summary>
        LocalMachine 
    };
	/// <summary>
	/// Registry helper.
	/// </summary>
	public sealed class RegistryHelper
	{
		private RegistryHelper(){}
		/// <summary>
		/// Gets value from a registry key under base key.
		/// </summary>
        /// <param name="keyBase">Base key</param>
		/// <param name="keyName">Key name</param>
		/// <param name="valueName">Value name</param>
		/// <param name="defaultValue">Default value</param>
		/// <returns></returns>
		public static object GetValueFromRegKey(BaseKey keyBase, string keyName, string valueName, object defaultValue) {
			
            RegistryKey _Key = (keyBase == BaseKey.Default) ?
                Registry.CurrentUser.OpenSubKey(keyName) : Registry.LocalMachine.OpenSubKey(keyName);

			if ( _Key != null && Array.IndexOf(_Key.GetValueNames(), valueName) > -1) {  
				object value =  _Key.GetValue(valueName);
				if (value != null) {
					return value;
				}
			} 
			return defaultValue;
		}
        /// <summary>
        /// Gets value from a registry key under CurrentUser.
        /// </summary>
        /// <param name="keyName">Key name</param>
        /// <param name="valueName">Value name</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns></returns>
        public static object GetValueFromRegKey(string keyName, string valueName, object defaultValue)
        {
            return GetValueFromRegKey(BaseKey.Default, keyName, valueName, defaultValue);
        }
        /// <summary>
        /// Sets value to a registry key under base key.
        /// </summary>
        /// <param name="keyBase">Base key</param>
        /// <param name="keyName">Key name</param>
        /// <param name="valueName">Value name</param>
        /// <param name="newValue">New value</param>
        public static void SetValueToRegKey(BaseKey keyBase, string keyName, string valueName, object newValue)
        {
            if (null == newValue)
            {
                throw new ArgumentException("new value cannot be null");
            }
            RegistryKey _Key = (keyBase == BaseKey.Default) ? 
                Registry.CurrentUser.OpenSubKey(keyName, true) : Registry.LocalMachine.OpenSubKey(keyName, true);

            if (_Key == null)
            {
                _Key = (keyBase == BaseKey.Default) ? 
                    Registry.CurrentUser.CreateSubKey(keyName) : Registry.LocalMachine.CreateSubKey(keyName);
            }
            _Key.SetValue(valueName, newValue);   
        }
        /// <summary>
        /// Sets value to a registry key under CurrentUser.
        /// </summary>
        /// <param name="keyName">Key name</param>
        /// <param name="valueName">Value name</param>
        /// <param name="newValue">New value</param>
        public static void SetValueToRegKey(string keyName, string valueName, object newValue)
        {
            SetValueToRegKey(BaseKey.Default, keyName, valueName, newValue);
        }
        /// <summary>
        /// Removes a value from a registry key under base key.
        /// </summary>
        /// <param name="keyBase">Base key</param>
        /// <param name="keyName">Key name</param>
        /// <param name="valueName">Value name</param>
        public static void RemoveValueFromRegKey(BaseKey keyBase, string keyName, string valueName)
        {
            RegistryKey _Key = (keyBase == BaseKey.Default) ?
                Registry.CurrentUser.OpenSubKey(keyName, true) : Registry.LocalMachine.OpenSubKey(keyName, true);

            if (_Key != null && Array.IndexOf(_Key.GetValueNames(), valueName) > -1)
            {
                _Key.DeleteValue(valueName, false);
            }
        }
        /// <summary>
        /// Removes a value from a registry key under CurrentUser.
        /// </summary>
        /// <param name="keyName">Key name</param>
        /// <param name="valueName">Value name</param>
        public static void RemoveValueFromRegKey(string keyName, string valueName)
        {
            RemoveValueFromRegKey(BaseKey.Default, keyName, valueName);
        }
    }
}
