using WowLogAnalyzer.Wow.Logs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;

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
        [TestMethod()]
        public void ReadEventTest()
        {
            string[] testFiles = {
                                     "General/WoWCombatLog_1.txt",
                                     "General/WoWCombatLog_2.txt",
                                     "Firelands/WoWCombatLog_20110801_Firelands_Lord_Rhyolith.txt"
                                 };
            int[] testFilesLength = {
                                        1159,
                                        29897,
                                        766944
                                    };
            for ( int i = 0; i < testFiles.Length; i++ ) {
                Stopwatch watch = new Stopwatch();
                string testFile = testFiles[i];
                int testFileEventLength = testFilesLength[i];
                Stream stream = File.OpenRead(testFile);

                watch.Start();

                LogReader target = new LogReader(new BufferedStream(stream));
                int eventCount = 0;
                while ( target.ReadEvent() != null )
                    eventCount++;

                watch.Stop();
                TimeSpan timeTaken = watch.Elapsed;
                Assert.AreEqual<int>(testFileEventLength, eventCount);
                // Process 50 000 lines per seconds. Include 250ms grace period to account for system variations.
                Assert.IsTrue(timeTaken.TotalSeconds < ((double)testFileEventLength) / 50 / 1000 + 250);
            }
        }
    }
}
