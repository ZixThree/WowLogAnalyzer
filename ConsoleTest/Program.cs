using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WowDataTest;
using System.Diagnostics;
using System.IO;
using WowLogAnalyzer.Wow.Logs;
using WowLogAnalyzer.Wow.Logs.Events;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] testFiles = {
                                     "General/WoWCombatLog_1.txt",
                                     "General/WoWCombatLog_2.txt",
                                     "4_1-The_Shattering/WoWCombatLog_BOT+BWD.txt",
                                     "4_2-Rage_of_the_Firelands/WoWCombatLog_20110801_Firelands_Lord_Rhyolith.txt",
                                     "4_2-Rage_of_the_Firelands/WoWCombatLog_20110802_Firelands_Shannox.txt",
                                     "4_2-Rage_of_the_Firelands/WoWCombatLog_20110803_Firelands_Beth'tilac_wipeonly.txt",
                                     "4_2-Rage_of_the_Firelands/WoWCombatLog_20110808_Firelands_Baleroc+Zul'Gurub.txt",
                                     "4_2-Rage_of_the_Firelands/WoWCombatLog_20110809_Firelands_Alysrazor_wipeonly_flying.txt",
                                     @"C:\Games\World of Warcraft\Logs\WoWCombatLog.txt"
                                 };
            int[] testFilesLength = {
                                        1159,
                                        29897,
                                        418387,
                                        766944,
                                        835321,
                                        173288,
                                        302090,
                                        453767,
                                        0
                                    };

            //for ( int i = 0; i < testFiles.Length; i++ ) {
            //    string testFile = testFiles[i];
            //    Console.WriteLine(testFile);
            //    Stopwatch watch = new Stopwatch();
            //    watch.Start();
            //    int count = 0;
            //    int count2 = 0;
            //    using ( Stream stream = File.OpenRead(testFile) ) {
            //        CombatLog log = new CombatLog(new BufferedStream(stream));
            //        //var events = from e in log.Events
            //        //             where (e.Source.Flags & CombatLogUnitFlags.Outsider) == 0 && (e.Destination.Flags & CombatLogUnitFlags.Outsider) == 0
            //        //             select e;

            //        count = log.Events.Count();
            //        count2 = log.TotalCount;

            //    }
            //    watch.Stop();
            //    Console.WriteLine("\t{0} ({1} useful events from {2})", watch.Elapsed, count, count2);
            //}

            ReadWithLogReader(testFiles, testFilesLength);
            Console.ReadKey();
        }

        private static TimeSpan ReadWithLogReader(string[] testFiles, int[] testFilesLength)
        {
            TimeSpan totalTime = TimeSpan.Zero;

            int totalLine = 0;
            long totalSize = 0;

            for ( int i = 0; i < testFiles.Length; i++ ) {

                Stopwatch watch = new Stopwatch();
                string testFile = testFiles[i];
                int testFileEventLength = testFilesLength[i];
                int eventCount = 0;
                int eventRelatedToSubmitter = 0;

                Dictionary<string, int> mobCount = new Dictionary<string, int>();
                Dictionary<long, string> mobName = new Dictionary<long, string>();

                using ( Stream stream = File.Open(testFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite) ) {
                    //MemoryStream memStream = new MemoryStream();
                    //stream.CopyTo(memStream, 32768);
                    //memStream.Position = 0;
                    totalSize += stream.Length;

                    //List<LogEvent> logEvents = new List<LogEvent>(testFileEventLength + 100);

                    watch.Start();

                    LogReader target = new LogReader(new BufferedStream(stream));
                    //LogReader target = new LogReader(memStream);
                    LogEvent current;

					//ICombatLogEventVisitor outputVisitor = new ReverseCombatLogVisitor(Console.Out);

                    while ( (current = target.ReadEvent()) != null ) {
                        eventCount++;

						//MemoryStream mem = new MemoryStream();
						//ICombatLogEventVisitor outputVisitor = new ReverseCombatLogVisitor(mem);
						//current.Event.Accept(outputVisitor);
						//mem.Seek(0,SeekOrigin.Begin);
						//StreamReader memReader = new StreamReader(mem);
						//string output = memReader.ReadToEnd();
						//if( !output.Equals(current.Line) ) {
						//    System.Diagnostics.Debugger.Break();
						//}


                        bool sourceOutsider = (current.Source.Flags & CombatLogUnitFlags.Outsider) != 0;
                        bool destOutsider = (current.Destination.Flags & CombatLogUnitFlags.Outsider) != 0;
                        bool relatedToUs = !sourceOutsider || !destOutsider;
                        if ( relatedToUs ) {
                            //logEvents.Add(current);
                            //logEvents[logEventsIndex++] = current;
                            eventRelatedToSubmitter++;
                            if ( (current.Source.Flags & CombatLogUnitFlags.Hostile) != 0 ) {
                                CombatLogUnit hostile = current.Source;
                                if ( !mobName.ContainsKey(hostile.Id) ) {
                                    mobName.Add(hostile.Id, hostile.Name);
                                    if ( !mobCount.ContainsKey(hostile.Name) ) {
                                        mobCount[hostile.Name] = 0;
                                    }
                                    mobCount[hostile.Name]++;
                                }
                            }

                            if ( (current.Destination.Flags & CombatLogUnitFlags.Hostile) != 0 ) {
                                CombatLogUnit hostile = current.Destination;
                                if ( !mobName.ContainsKey(hostile.Id) ) {
                                    mobName.Add(hostile.Id, hostile.Name);
                                    if ( !mobCount.ContainsKey(hostile.Name) ) {
                                        mobCount[hostile.Name] = 0;
                                    }
                                    mobCount[hostile.Name]++;
                                }
                            }
                        }
                    }

                    watch.Stop();
                }

                TimeSpan timeTaken = watch.Elapsed;
				totalLine += eventCount;
                totalTime += timeTaken;
                Console.WriteLine("------------------------------");
                Console.WriteLine("Filename: {0}", testFile);
                Console.WriteLine("Elapsed: {0}", timeTaken);
                Console.WriteLine("Total Events: {0} ({1})", eventCount, eventRelatedToSubmitter);
                Console.WriteLine("Mobs: {0} individual, {1} total", mobCount.Count, mobName.Count);
                Console.WriteLine("Lines: {0} ({1})", eventCount, testFileEventLength);
                Console.WriteLine("Lines per seconds: {0,6:F2} lines per second", eventCount / timeTaken.TotalSeconds);
            }

            Console.WriteLine("==============================");
            Console.WriteLine("Total Time elapsed: {0}.", totalTime);
            Console.WriteLine("Lines per seconds: {0}", totalLine / totalTime.TotalSeconds);
            Console.WriteLine("Average character per line: {0}", totalSize / (double)totalLine);
            return totalTime;
        }
    }
}
