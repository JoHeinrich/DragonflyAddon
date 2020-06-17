using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VoiceControl;

namespace DragonflyTest
{
    [TestClass]
    public class PythonProviderTest
    {
        [TestMethod]
        public void CheckAvailable()
        {
            var provider = new PythonProvider();
            Assert.AreNotEqual(0, provider.Available.Count());
        }
        [TestMethod]
        public void CheckInstantiation()
        {
            var provider = new PythonProvider();
            Assert.AreNotEqual(0, provider.Available.Count());
            foreach (var item in provider.Available)
            {
                provider.Instantiate(item);
            }
        }
        [TestMethod]
        public void CheckExecute()
        {

            var result = new PythonConnection().ExecuteCommand("print(\"h\")");
            Assert.AreEqual("h\r\n", result);

        }
    }
}
