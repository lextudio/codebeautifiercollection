// this is dead state class.
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
namespace Lextm.Diagnostics {

	/// <summary>
	/// The dead state. 
	/// </summary>
	/// <remarks>A stopwatch is in this state before <see cref="Start"/> and after 
	/// <see cref="Stop"></see>. As a result <see cref="Resume"/>, <see cref="Suspend"/>,
	/// <see cref="Stop"/> calls will be ignored.</remarks>
	internal class Dead : CustomState {

		public override void Suspend( Lextm.Diagnostics.Stopwatch context ) {
		}

		public override void Resume( Lextm.Diagnostics.Stopwatch context ) {
		}

		public override void Stop( Lextm.Diagnostics.Stopwatch context ) {
		}

		public override int GetInterval( ) {
			return 0;
		}

		public override void Start( Lextm.Diagnostics.Stopwatch context ) {
			SetState(context, new Working());
		}

		public override int GetValue( ) {
			return 0;
		}
	}
}
