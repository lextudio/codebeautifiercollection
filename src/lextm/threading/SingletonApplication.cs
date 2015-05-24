// lextm: this is the singleton application class. 
// Copyright (C) 2006  Lex Mark
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
using System.Threading;

namespace Lextm.Threading
{
    /// <summary>
    /// SingletonApplication provides function to ensure only one instance of this 
    /// application can be created.
    /// </summary>
    public class SingletonApplication
    {
        private SingletonApplication()
        {}

        private static Mutex InstanceMutex;

        private const string MutexName = "772CB381-DB8D-4341-8255-A79382BB71F2";
		/// <summary>
		/// Indicates if there is an instance running.
		/// </summary>
		/// <returns></returns>
        public static bool IsRunning()
        {
            try
            {
                Mutex.OpenExisting(MutexName);
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                return false;
            }
            return true;
        }
		/// <summary>
		/// Marks an instance is running now.
		/// </summary>
        public static void MarkRunning()
        {
            InstanceMutex = new Mutex(true, MutexName);
        }
    }
}
