using System.Collections.Generic;
using NUnit.Framework;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Tests.Util.Exceptions
{
    [TestFixture]
    public class UiNodeNotFoundExceptionTests
    {
        [Test]
        public void Constructor_WhenCreatingExceptionWithASingleBy_ShouldGetCorrectErrorMessage()
        {
            Assert.AreEqual("Could not find node where resource id equals \"test\"", new UiNodeNotFoundException(new List<By> { By.ResourceId("test")}).Message);
        }

        [Test]
        public void Constructor_WhenCreatingExceptionWithTwoBys_ShouldGetCorrectErrorMessage()
        {
            Assert.AreEqual("Could not find node where resource id equals \"test\" and package equals \"myPackage\"", new UiNodeNotFoundException(new List<By> { By.ResourceId("test"), By.Package("myPackage") }).Message);
        }

        [Test]
        public void Constructor_WhenCreatingExceptionWithThreeBys_ShouldGetCorrectErrorMessage()
        {
            Assert.AreEqual("Could not find node where resource id equals \"test\", package equals \"myPackage\" and index equals 2", new UiNodeNotFoundException(new List<By> { By.ResourceId("test"), By.Package("myPackage"), By.Index(2) }).Message);
        }
    }
}
