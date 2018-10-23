using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWbemLibrary;
using SWbemLibrary.Exceptions;

namespace SwbemLibrary.Tests {
    [TestClass]
    public class SWbemLocatorTest {

        private SWbemLocator locator;

        private string localServer = ".";
        private string rootcimv2 = "root/cimv2";

        [TestInitialize]
        public void Initialize() {
            locator = new SWbemLocator();
        }

        [TestMethod]
        public void CorrectNamespaceReturnsSWbemServiceObject() {
            SWbemService service = locator.ConnectServer(localServer, rootcimv2);
            Assert.IsNotNull(service);
        }


    }
}
