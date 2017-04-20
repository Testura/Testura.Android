using System;
using System.Collections.Generic;
using Testura.Android.Device;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Util.Walker
{
    public class WalkerTrigger
    {
        /// <summary>
        /// Gets or sets the with for the special case node
        /// </summary>
        public IList<With> Withs { get; set; }

        /// <summary>
        /// Gets or sets the func that should be executed when we find the special case node. Returns
        /// true if we should still click on the node, false if not.
        /// </summary>
        public Func<IAndroidDevice, Node, bool> Case { get; set; }
    }
}
