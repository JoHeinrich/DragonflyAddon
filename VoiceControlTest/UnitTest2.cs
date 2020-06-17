using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Python.Runtime;
using VoiceControl;

namespace VoiceControlTest
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (Py.GIL())
            {

                dynamic imp = Py.Import("imp");
                dynamic test = imp.load_source("action_map", @"C:\Users\laise\Desktop\test (2).py");
                Console.WriteLine(test.test);
                Console.WriteLine(test.test_list);
                //Console.WriteLine(test.action_map.count);
                //Console.WriteLine(test.action_map["after"]);
                test.action_map["after"].execute();
                List<Words> list = test.test_list;
                foreach (var item in list)
                {
                    Console.WriteLine(item.Content);
                }
                //Words w = action_map["name"];
                //Console.WriteLine(w.Content);
                //Console.WriteLine(np.cos(np.pi * 2));

                //dynamic sin = np.sin;
                //Console.WriteLine(sin(5));

                //double c = np.cos(5) + sin(5);
                //Console.WriteLine(c);

                //dynamic a = np.array(new List<float> { 1, 2, 3 });
                //Console.WriteLine(a.dtype);

                //dynamic b = np.array(new List<float> { 6, 5, 4 }, dtype: np.int32);
                //Console.WriteLine(b.dtype);

                //Console.WriteLine(a * b);
                //Console.ReadKey();
            }
        }
    }
}
