// lextm: this is timer class.
//		It is similar to C++ Boost Library's Timer class.
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

namespace Lextm.Diagnostics
{
	/// <summary>
	/// Timer class.
	/// </summary>
	/// <remarks>Reset before using.</remarks>
	public sealed class Stopwatch
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public Stopwatch()
		{
		}
		
		private int baseCount;
		private int lastElapseCount;

		/// <summary>
		/// Starts timing.
		/// </summary>
		public void Begin() {
			lock(typeof(Stopwatch)) {
				baseCount = Environment.TickCount;
				lastElapseCount = 0;
			}
		}
		/// <summary>
		/// Gets time passed, in milliseconds, from last Elapse call.
		/// </summary>
		/// <returns>In milliseconds.</returns>
		public int Elapse() {
			int temp;
			lock(typeof(Stopwatch)) {
				temp = (Environment.TickCount - baseCount) - lastElapseCount;
				lastElapseCount = (Environment.TickCount - baseCount);
			}
			return temp;
		}
		/// <summary>
		/// Stops and returns total time passed.
		/// </summary>
		/// <returns>In milliseconds.</returns>
		public int End() {
            int result;
			lock(typeof(Stopwatch)) {
				result = Environment.TickCount - baseCount;
				lastElapseCount = 0;
				baseCount = 0;
			}
			return result;
		}
	}
}
