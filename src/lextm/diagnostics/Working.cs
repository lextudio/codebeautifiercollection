// this is working state class.
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
	/// The working state.
	/// </summary>
	/// <remarks><see cref="Start"/> and <see cref="Resume"/> calls are ignored.</remarks>
    internal class Working : CustomState {

        private readonly int lastTick;
        private int lastValueBeforeInterval;

        public override void Suspend( Lextm.Diagnostics.Stopwatch context ) {
            int valueBeforeSleep = Environment.TickCount - lastTick;
            int lastInterval = GetInterval();
            SetState(context, new Idle(valueBeforeSleep, lastInterval));
        }

        public override void Resume( Lextm.Diagnostics.Stopwatch context ) {
        }

        public override void Stop( Lextm.Diagnostics.Stopwatch context ) {
            SetState(context, new Dead());
        }

        public override int GetInterval( ) {
            int result = (Environment.TickCount - lastTick) - lastValueBeforeInterval;
            lastValueBeforeInterval = (Environment.TickCount - lastTick);
            return result;
        }

        public override void Start( Lextm.Diagnostics.Stopwatch context ) {}

        public override int GetValue( ) {
            return Environment.TickCount - lastTick;
        }

        internal Working( ) :
				this(0, 0) {}

		internal Working( int value ) :
				this(value, 0) {}

		internal Working( int value, int interval ) {
            lastTick = Environment.TickCount - value;
            lastValueBeforeInterval = value - interval;
        }
    }
}


