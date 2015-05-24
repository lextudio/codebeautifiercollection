// this is idle state class.
// Copyright (C) 2007  Lex Y. Li
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
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307
using System;

namespace Lextm.Diagnostics {

	/// <summary>
	/// The idle state.
	/// </summary>
	/// <remarks>The stopwatch is in this state between <see cref="Suspend"/> and 
	/// <see cref="Resume"/>. So <see cref="Start"/> and <see cref="Suspend"/> calls 
	/// are ignored.</remarks>
	internal class Idle : CustomState {

		public override void Start( Stopwatch context ) {

		}

		public override void Stop( Stopwatch context ) {
			SetState(context, new Dead());
		}

		public override void Suspend( Stopwatch context ) {

		}

		public override void Resume( Stopwatch context ) {
			if (thisInterval == 0) {
				SetState(context, new Working(valueBeforeIdle));
			} else {
				SetState(context, new Working(valueBeforeIdle, intervalBeforeIdle));
			}
		}

		public override int GetValue( ) {
			return valueBeforeIdle;
		}

		public override int GetInterval( ) {
			int result = thisInterval;
			thisInterval -= thisInterval;
			return result;
		}

		private readonly int valueBeforeIdle;
		private readonly int intervalBeforeIdle;
		private int thisInterval;

		internal Idle( int value, int interval ) {
			valueBeforeIdle = value;
			intervalBeforeIdle = interval;
			thisInterval = interval;
		}
	}
}
