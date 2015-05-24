using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.Utilities.InDate
{
    class Completed: UpdateStateBase
    {
        internal override string GetTip()
        {
            return "InDate will close in 3 seconds.";
        }

        internal override void Handle(UpdateContext context)
        {            
        }

        internal override void Transit(UpdateContext context)
        {
        }
    }
}
