using System.Collections.Generic;

namespace Testura.Android.Util.Walker
{
    public class AppWalkerConfiguration
    {
        public AppWalkerConfiguration()
        {
            ShouldOnlyTapClickAbleNodes = true;
            Inputs = new List<WalkerInputs> { WalkerInputs.Tap };
            MaxInputBeforeGoingBack = 5;
        }

        /// <summary>
        /// Gets or sets a value indicating whether we should only tap nodes that have clickable set to true
        /// </summary>
        public bool ShouldOnlyTapClickAbleNodes { get; set; }

        /// <summary>
        /// Gets or sets a list with allowed inputs through the walk
        /// </summary>
        public IList<WalkerInputs> Inputs { get; set; }

        /// <summary>
        /// Gets or sets the maximum input on the same activity page before we go back
        /// </summary>
        public int MaxInputBeforeGoingBack { get; set; }
    }
}
