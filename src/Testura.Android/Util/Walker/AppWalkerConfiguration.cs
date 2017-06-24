namespace Testura.Android.Util.Walker
{
    /// <summary>
    /// Represent the configuration of an app walker run.
    /// </summary>
    public class AppWalkerConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppWalkerConfiguration"/> class.
        /// </summary>
        public AppWalkerConfiguration()
        {
            MaxInputBeforeGoingBack = 5;
            WalkDuration = 0;
            ShouldStartActivity = true;
            ShouldGoBackToActivity = true;
            InputCooldown = 0;
        }

        /// <summary>
        /// Gets or sets the maximum input on the same activity page before we go back. Minus one means that we never go back.
        /// </summary>
        public int MaxInputBeforeGoingBack { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should start the walker by starting the activity.
        /// </summary>
        public bool ShouldStartActivity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should go back to the selected activity if we accidentally quit.
        /// </summary>
        public bool ShouldGoBackToActivity { get; set; }

        /// <summary>
        /// Gets or sets the walk duration in minutes (0 means infinity).
        /// </summary>
        public int WalkDuration { get; set; }

        /// <summary>
        /// Gets or sets the cooldown between input in milliseconds.
        /// </summary>
        public int InputCooldown { get; set; }
    }
}
