namespace Lextm.Utilities.InDate
{

    /// <summary>
    /// Updating is successful.
    /// </summary>
    class Succeeded : UpdateStateBase
	{
		private string tip;

		internal Succeeded()
		{
			this.tip = "Updating is finished.";
		}

		internal Succeeded(string tip) {
            this.tip = tip;
		}

		/// <summary>
		/// Gets tip.
		/// </summary>
		/// <returns></returns>
		internal override string GetTip( )
		{
			return this.tip;
		}

        /// <summary>
        /// Handles.
        /// </summary>
        /// <param name="context">Context</param>
        internal override void Handle( UpdateContext context )
        {
            System.Threading.Thread.Sleep(1000);
        }
        
		internal override void Transit(UpdateContext context)
		{
            SetState(context, new Completed());
        }
    }
}




