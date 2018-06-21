﻿using System;
using System.Collections.Generic;
using Testura.Android.Device.Services.Ui;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;
#pragma warning disable 1591

namespace Testura.Android.Device.Ui.Objects
{
    /// <summary>
    /// Represent multiple UI Objects on the screen
    /// </summary>
    public class UiObjects : BaseUiObject
    {
        private readonly INodeFinderService _nodeFinderService;

        internal UiObjects(INodeFinderService nodeFinderService, IList<With> withs)
            : base(withs)
        {
            _nodeFinderService = nodeFinderService;
        }

        /// <summary>
        /// Get a list of nodes that contain all values.
        /// </summary>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <returns>A list of nodes that contain all values.</returns>
        public IList<Node> Values(TimeSpan timeout)
        {
            return TryFindNode(timeout);
        }

        protected override IList<Node> TryFindNode(TimeSpan timeout)
        {
            return _nodeFinderService.FindNodes(Withs, timeout);
        }
    }
}
