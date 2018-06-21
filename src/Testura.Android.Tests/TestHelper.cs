﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using Moq;
using Testura.Android.Device;
using Testura.Android.Device.Server;
using Testura.Android.Device.Services;
using Testura.Android.Device.Services.Ui;
using Testura.Android.Device.Ui.Nodes;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Objects;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Tests
{
    internal class TestHelper
    {
        private readonly UiService _uiService;

        public TestHelper()
        {
            NodeFinderService = new Mock<INodeFinderService>();
            DeviceMock = new Mock<AndroidDevice>();

            _uiService = new UiService(
                new Mock<IUiAutomatorServer>().Object, 
                new Mock<INodeParser>().Object,
                new Mock<INodeFinder>().Object);
        }

        public Mock<INodeFinderService> NodeFinderService { get; }

        public Mock<AndroidDevice> DeviceMock { get; }

        public UiObject CreateUiObject(With with, int delayInMiliSec, bool shouldThrowExeception = false)
        {
            if (shouldThrowExeception)
            {
                NodeFinderService.Setup(u => u.FindNode(new[] { with }, It.IsAny<TimeSpan>()))
                    .Callback(() => Thread.Sleep(delayInMiliSec))
                    .Throws<UiNodeNotFoundException>();
            }
            else
            {
                NodeFinderService.Setup(u => u.FindNode(new[] { with }, It.IsAny<TimeSpan>()))
                   .Callback(() => Thread.Sleep(delayInMiliSec))
                   .Returns(new Node(new XElement("mm"), null));
            }
            return new UiObject(null, NodeFinderService.Object, new[] { with });
        }

        public UiObjects CreateUiObjects(With with, int delayInMiliSec, bool shouldThrowExeception = false)
        {
            if (shouldThrowExeception)
            {
                NodeFinderService.Setup(u => u.FindNode(new[] { with }, It.IsAny<TimeSpan>()))
                    .Callback(() => Thread.Sleep(delayInMiliSec))
                    .Throws<UiNodeNotFoundException>();
            }
            else
            {
                NodeFinderService.Setup(u => u.FindNode(new [] { with }, It.IsAny<TimeSpan>()))
                   .Callback(() => Thread.Sleep(delayInMiliSec))
                   .Returns(new Node(new XElement("mm"), null));
            }
            return new UiObjects(NodeFinderService.Object, new[] { with });
        }
    }
}
