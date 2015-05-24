using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lextm.Utilities.InDate
{
    class Canceled: UpdateStateBase
    {
        internal override string GetTip()
        {
            return "Update process is canceled.";
        }

        internal override void Handle(UpdateContext context)
        {
            Thread.Sleep(1000);
        }

        internal override void Transit(UpdateContext context)
        {
            SetState(context, new Completed());
        }
    }
}
