using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using NUnit.Framework;
using Testura.Android.Device;
using Testura.Android.Device.Configurations;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Device.Ui.Server;
using Testura.Android.Util;
using Testura.Android.Util.Terminal;
using Testura.Android.Util.Walker;
using Testura.Android.Util.Walker.Cases;
using Assert = NUnit.Framework.Assert;

namespace Testura.Android.Tests.Integration.Device.UiAutomator.Server
{
    [TestFixture]
    public class UiAutomatorServerTests
    {
        private UiAutomatorServer _server;

        [SetUp]
        public void SetUp()
        {
            _server = new UiAutomatorServer(new Terminal(new DeviceConfiguration()), 9008);
        }

        [Test]
        public void UiAutomatorServer_WhenStartingServer_ShouldNotThrowException()
        {
            _server.Start();
        }

        [Test]
        public void UiAUtomatorServer_WhenDumpingUi_ShouldGetContentBack()
        {
            var random = new Random();

            var d1 = new AndroidDevice(new DeviceConfiguration() { Serial = "ZY223TFQ63", Port = 9021});
            //var d2 = new AndroidDevice(new DeviceConfiguration() { Serial = "163545225D0178", Port = 9022});

            var u1 = d1.Ui.CreateUiObject(With.Index(0));
            //var u2 = d2.Ui.CreateUiObject(With.Index(0));

            var count = 1;
            while (true)
            {
                u1.IsVisible();
                //u2.IsVisible();

                count++;

                if (random.Next(0, 100) > 25)
                {
                   d1.Ui.StopUiServer();
                }

                //if (random.Next(0, 100) > 25)
                //{
                //    d2.Ui.StopUiServer();
                //}


                Thread.Sleep(3000);
                Debug.WriteLine("Cound: " + count);
            }
        }
    }
}