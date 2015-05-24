namespace Lextm.Utilities.InDate
{

    /// <summary>
    /// Base type of an update state.
    /// </summary>
    abstract class UpdateStateBase
    {
		/// <summary>
		/// Constructor.
		/// </summary>
		protected UpdateStateBase()
		{}

		/// <summary>
		/// Sets state of a context.
		/// </summary>
		/// <param name="context">Update context</param>
		/// <param name="newState">New state</param>
		protected static void SetState( UpdateContext context,
								 UpdateStateBase newState)
		{
			context.SetState(newState);
		}
		/// <summary>
		/// Gets a tip.
		/// </summary>
		/// <returns></returns>
		internal abstract string GetTip();
		/// <summary>
		/// Handles state action.
		/// </summary>
		/// <param name="context">Context</param>
		internal abstract void Handle( UpdateContext context );
		/// <summary>
		/// Transits state.
		/// </summary>
		/// <param name="context">Context</param>
		internal abstract void Transit( UpdateContext context);
    }
}


