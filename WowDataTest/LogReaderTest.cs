using WowLogAnalyzer.Wow.Logs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace WowDataTest
{
    
    
    /// <summary>
    ///This is a test class for LogReaderTest and is intended
    ///to contain all LogReaderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LogReaderTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Tokenize
        ///</summary>
        [TestMethod()]
        [DeploymentItem("WowData.dll")]
        public void TokenizeTest()
        {
            string line;
            string[] expectedValues;

            line = "abcdef";
            expectedValues = new string[] { "abcdef" };
            TokenizeVerify(LogReader_Accessor.Tokenize(line), expectedValues);

            line = "abcdef,ghijklm";
            expectedValues = new string[] { "abcdef", "ghijklm" };
            TokenizeVerify(LogReader_Accessor.Tokenize(line), expectedValues);

            line = "abcdef,ghijklm,opqrst,uvwxyz ";
            expectedValues = new string[] { "abcdef", "ghijklm", "opqrst", "uvwxyz " };
            TokenizeVerify(LogReader_Accessor.Tokenize(line), expectedValues);

            line = "ab,\"test\"";
            expectedValues = new string[] { "ab", "test" };
            TokenizeVerify(LogReader_Accessor.Tokenize(line), expectedValues);

            line = "ab,\"test\",cdef";
            expectedValues = new string[] { "ab", "test", "cdef" };
            TokenizeVerify(LogReader_Accessor.Tokenize(line), expectedValues);

            line = "ab,\"test\",\"cdef\"";
            expectedValues = new string[] { "ab", "test", "cdef" };
            TokenizeVerify(LogReader_Accessor.Tokenize(line), expectedValues);

            line = "\"ab\",\"test\",cdef";
            expectedValues = new string[] { "ab", "test", "cdef" };
            TokenizeVerify(LogReader_Accessor.Tokenize(line), expectedValues);
        }

        private void TokenizeVerify(IEnumerable<string> enumerable, string[] expectedValues)
        {
            int index = 0;
            foreach (string value in enumerable)
            {
                if (index >= expectedValues.Length)
                {
                    Assert.Fail("Too many token returned.");
                }
                Assert.AreEqual(expectedValues[index++], value);
            }
            Assert.AreEqual<int>(expectedValues.Length, index);
        }

        /// <summary>
        ///A test for ReadEvent
        ///</summary>
        //[DeploymentItem("Data/WoWCombatLog_1.txt.gz")]
        [TestMethod()]
        public void ReadEventTest()
        {
            Stream stream = new GZipStream(File.OpenRead("WoWCombatLog_1.txt.gz"), CompressionMode.Decompress);
            LogReader target = new LogReader(stream);
            int eventCount = 0;
            while ( target.ReadEvent() != null )
                eventCount++;
            Assert.AreEqual<int>(1159, eventCount);
        }
    }
}
