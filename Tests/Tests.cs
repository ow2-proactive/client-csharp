using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpRestClient;

namespace Tests
{
    [TestClass]
    public class RestClientTests
    {
        public static readonly string REST_SERVER_URL = "http://localhost:8080/rest";        

        public static SchedulerClient sc;

        [ClassInitialize]
        public static void BeforeAll(TestContext ctx)
        {
            Console.WriteLine("----------------CONNECTING TO THE SCHEDULER---------------");
            sc = SchedulerClient.connect(REST_SERVER_URL, "admin", "admin");            
        }

        [ClassCleanup]
        public static void AfterAll()
        {
            Console.WriteLine("----------------HLKJH---------------");
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
