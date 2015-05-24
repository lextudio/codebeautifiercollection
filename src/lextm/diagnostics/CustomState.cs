// this is custom state class.
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
	/// Base class for stopwatch states.
	/// </summary>
	internal abstract class CustomState {

		public abstract int GetInterval( );
		public abstract void Suspend( Lextm.Diagnostics.Stopwatch context );
		public abstract void Resume( Lextm.Diagnostics.Stopwatch context );
		public abstract void Stop( Lextm.Diagnostics.Stopwatch context );
		public abstract void Start( Lextm.Diagnostics.Stopwatch context );
		public abstract int GetValue( );

		public static void SetState( Stopwatch context, CustomState state ) {
            context.SetState(state);
		}
	}
}
