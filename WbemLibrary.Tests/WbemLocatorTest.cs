using Microsoft.VisualStudio.TestTools.UnitTesting;
using WbemLibrary;

namespace WbemLibrary.Tests {
    [TestClass]
    public class WbemLocatorTest {

        private WbemLocator locator;

        private string localServer = ".";
        private string rootcimv2 = "root/cimv2";

        [TestInitialize]
        public void Initialize() {
            locator = new WbemLocator();
        }

        [TestMethod]
        public void CorrectNamespaceReturnsSWbemServiceObject() {
            WbemService service = locator.ConnectServer(localServer, rootcimv2);
            Assert.IsNotNull(service);
        }


    }
}
