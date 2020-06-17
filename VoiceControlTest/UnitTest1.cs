using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonflyTest
{
    [TestClass]
    public class UnitTest1
    {
        OnlineDownloader online =new OnlineDownloader("http://jonasheinrich.de");

        [TestMethod]
        public void TestMethod1()
        {
            string path = "Backend/EasyVoiceCode/Setup/";
            var results = online.ListDirectories(path);
            results.ForEach(x => Console.WriteLine(x));
            Assert.AreNotEqual(0,results.Count );
            Assert.IsTrue(results.Contains("Backend/EasyVoiceCode/Setup/autorun.inf"), "Does not contain Backend/EasyVoiceCode/Setup/autorun.inf");
        }

        [TestMethod]
        public void TestFileDownload()
        {
            var file=online.DownloadFile("Backend/EasyVoiceCode/Setup/autorun.inf");
            Assert.AreNotEqual(null, file);
            Assert.AreNotEqual(0, file.Length);
        }
    }
}
