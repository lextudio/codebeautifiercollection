// this is stopwatch class.
// Copyright (C) 2006-2007  Lex Y. Li
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
    /// Stopwatch class.
    /// </summary>
    public class Stopwatch {

        /// <summary>
        /// Construtor.
        /// </summary>
		public Stopwatch( ) {
			internalState = new Dead();
		}
        /// <summary>
        /// Starts the timing.
        /// </summary>
        /// <seealso cref="Stop"/>
        /// <seealso cref="Suspend"/>
        /// <seealso cref="Value"/>
        /// <seealso cref="Interval"/>
        /// <remarks>If it is working, it will ignore other <see cref="Start"/> calls.</remarks>
        public void Start( ) {
			internalState.Start(this);
        }
        /// <summary>
        /// Stops the timing.
        /// </summary>
        /// <remarks>Once stopped, you have to <see cref="Start"></see> it.</remarks>
        /// <seealso cref="Start"/>
        public void Stop( ) {
			internalState.Stop(this);
        }
        /// <summary>
        /// Resumes a timing.
        /// </summary>
        /// <seealso cref="Suspend"/>
        public void Resume( ) {
			internalState.Resume(this);
        }
        /// <summary>
        /// Suspends timing.
        /// </summary>
        /// <seealso cref="Resume"/>
        public void Suspend( ) {
            internalState.Suspend(this);
        }
        /// <summary>
        /// Count in milliseconds.
        /// </summary>
        /// <remarks>This counts from the beginning of timing.</remarks>
        /// <seealso cref="Interval"/>
        public int Value {
            get {
                return internalState.GetValue();
            }
        }
        /// <summary>
        /// Interval in milliseconds.
        /// </summary>
        /// <seealso cref="Value"/>
        /// <remarks>This returns a count of interval since last call of this function.</remarks>
        public int Interval {
            get {
                return internalState.GetInterval();
            }
        }

        private Lextm.Diagnostics.CustomState internalState;

		internal void SetState( CustomState state ) {
        	this.internalState = state;
		}
	}
}
