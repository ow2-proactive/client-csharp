using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpRestClient;

namespace Tests
{
    [TestClass]
    public class RestClientTests
    {
        public static readonly string LOCAL_REST_SERVER_URL = "http://localhost:8080/restt";

        public static readonly string TRY_REST_SERVER_URL = "https://try.activeeon.com/rest";

        public static SchedulerClient sc;

        [ClassInitialize]
        public static void BeforeAll(TestContext ctx)
        {
            try
            {
                Console.WriteLine("--- Trying to connect to " + LOCAL_REST_SERVER_URL);
                sc = SchedulerClient.connect(LOCAL_REST_SERVER_URL, "admin", "admin");
            }
            catch (Exception)
            {
                Console.WriteLine("--- !!! No Scheduler running on localhost !!! ... Trying to connect to " + TRY_REST_SERVER_URL);
                try
                {
                    sc = SchedulerClient.connect(TRY_REST_SERVER_URL, "demo", "*****");
                }
                catch (Exception)
                {
                    Console.WriteLine("--- !!! No Scheduler running on try !!! ... Unable to run the tests");
                }
            }
        }

        [ClassCleanup]
        public static void AfterAll()
        {
            Console.WriteLine("----------------ClassCleanup---------------");
        }

        [TestMethod]
        public void TestGetStatus()
        {
            Console.WriteLine("Scheduler status: " + sc.GetStatus());

            Assert.AreEqual<SchedulerStatus>(SchedulerStatus.STARTED, sc.GetStatus());
        }

        [TestMethod]
        public void TestSubmitXml()
        {
            JobId jid = sc.SubmitXml(@"C:\tmp\ProActiveWorkflowsScheduling-windows-x64-6.0.1\samples\workflows\01_simple_task.xml");

            Console.WriteLine("---> jobid.id = " + jid.Id);
            //Console.WriteLine("---> jobid.ReadableName = " + jid.ReadableName);

            Assert.AreNotEqual<long>(0, jid.Id);
            Assert.AreEqual<string>("01_simple_task", jid.ReadableName);

            Console.WriteLine("---> asking job state ...");
            JobState jobState = sc.GetJobState(jid);
            Console.WriteLine("---> GOT: " + jobState);

            System.Threading.Thread.Sleep(5000);

            Assert.AreEqual<bool>(false, sc.PauseJob(jid));
            Assert.AreEqual<bool>(false, sc.ResumeJob(jid));
            Assert.AreEqual<bool>(true, sc.KillJob(jid));
            Assert.AreEqual<bool>(true, sc.RemoveJob(jid));
        }


    }
}
