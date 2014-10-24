using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpRestClient;
using System.IO;
using SharpRestClient.Exceptions;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class RestClientTests
    {
        public static readonly string LOCAL_REST_SERVER_URL = "http://localhost:8080/rest";

        public static readonly string TRY_REST_SERVER_URL = "https://try.activeeon.com/rest";

        public static SchedulerClient sc;

        [ClassInitialize]
        public static void BeforeAll(TestContext ctx)
        {
            try
            {
                Console.WriteLine("Trying to connect to " + LOCAL_REST_SERVER_URL);
                sc = SchedulerClient.Connect(LOCAL_REST_SERVER_URL, "admin", "admin");
            }
            catch (Exception)
            {
                Console.WriteLine("No Scheduler running on localhost, trying to connect to " + TRY_REST_SERVER_URL);
                try
                {
                    string p = Environment.GetEnvironmentVariable("TRY_DEMO_PASS", EnvironmentVariableTarget.Machine);
                    sc = SchedulerClient.Connect(TRY_REST_SERVER_URL, "demo", p);
                }
                catch (Exception)
                {
                    Assert.Fail("Unable to run tests! There is no scheduler running on try!");
                }
            }

            // Check Scheduler Verison is 6.X
            SharpRestClient.Version ver = sc.GetVersion();
            Assert.IsNotNull(ver.Scheduler);
            Assert.IsNotNull(ver.Rest);
            if (!ver.Scheduler.StartsWith("6"))
            {
                Assert.Fail("The Scheduler version is not 6.X");
            }
            if (!ver.Rest.StartsWith("6"))
            {
                Assert.Fail("The Rest version is not 6.X");
            }
        }

        [ClassCleanup]
        public static void AfterAll()
        {
            //Console.WriteLine("----------------ClassCleanup---------------");
        }

        [TestMethod]
        public void IsConnected()
        {
            Assert.AreEqual<bool>(true, sc.IsConnected());
        }

        [TestMethod]
        public void GetSchedulerStatus()
        {
            Assert.AreEqual<SchedulerStatus>(SchedulerStatus.STARTED, sc.GetStatus());
        }

        [TestMethod]
        public void PushPullDeleteFile()
        {
            // Create temp file in temp dir
            string tempFilePath = Path.GetTempFileName();
            FileInfo fileInfo = new FileInfo(tempFilePath);
            try
            {
                // Upload a temp file
                Assert.IsTrue(sc.PushFile("GLOBALSPACE", "", fileInfo.Name, tempFilePath));
            }
            finally
            {
                // Delete the local file
                fileInfo.Delete(); 
            }
            Assert.IsTrue(sc.PullFile("GLOBALSPACE", fileInfo.Name, fileInfo.FullName));
            Assert.IsTrue(fileInfo.Exists);
            // Delete the pulled local file
            fileInfo.Delete();
            // Delete the remote file
            Assert.IsTrue(sc.DeleteFile("GLOBALSPACE", fileInfo.Name));
            try
            {
                Assert.IsFalse(sc.DeleteFile("GLOBALSPACE", fileInfo.Name));
            } catch (Exception e) {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void SubmitXml()
        {
            string jobname = "script_task_with_result";
            JobId jid = sc.SubmitXml(GetWorkflowPath(jobname));
            try
            {
                Assert.AreNotEqual<long>(0, jid.Id, "After submission the job id is invalid!");
                Assert.AreEqual<string>(jobname, jid.ReadableName, "After submission the job name is invalid!");
            }
            finally
            {
                sc.KillJob(jid);
                sc.RemoveJob(jid);
            }
        }

        [TestMethod]
        public void PauseResumeJob()
        {
            string jobname = "script_task_with_result";
            JobId jid = sc.SubmitXml(GetWorkflowPath(jobname));
            try
            {
                bool isPaused = sc.PauseJob(jid);
                Assert.AreEqual<bool>(true, isPaused, "Unable to pause the job!");
                bool isResumed = sc.ResumeJob(jid);
                Assert.AreEqual<bool>(true, isResumed, "Unable to resume the job!");
            }
            finally
            {
                sc.RemoveJob(jid);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownJobException))]
        public void PauseJob_UnknwonJobException()
        {
            // Check unknown jobid
            JobId invalidJid = new JobId();
            sc.PauseJob(invalidJid);
        }

        [TestMethod]
        public void WaitForJobResult()
        {
            string jobname = "script_task_with_result";
            JobId jid = sc.SubmitXml(GetWorkflowPath(jobname));
            try
            {
                JobResult jr = sc.WaitForJobResult(jid, 30000);
                Assert.IsNotNull(jr);
                TaskResult tr = jr.Tasks["simple_task"];
                Assert.IsNotNull(tr);
            }
            finally
            {
                sc.KillJob(jid);
                sc.RemoveJob(jid);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void WaitForJobResult_TimeoutException()
        {
            string jobname = "one_minute_script_task";
            JobId jid = sc.SubmitXml(GetWorkflowPath(jobname));
            try
            {
                sc.WaitForJobResult(jid, 1000);
            }
            finally
            {
                sc.KillJob(jid);
                sc.RemoveJob(jid);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownJobException))]
        public void WaitForJobResult_UnknownJobException()
        {
            JobId invalidJid = new JobId();
            sc.WaitForJobResult(invalidJid, 1000);
        }

        [TestMethod]
        public void WaitForJobResultValue()
        {
            string jobName = "stask_result_out_err";
            string taskName = "simple_task";
            string expectedResult = "hello";
            JobId jid = sc.SubmitXml(GetWorkflowPath(jobName));
            try
            {
                IDictionary<string,string> jr = sc.WaitForJobResultValue(jid, 30000);
                Assert.IsNotNull(jr);
                string value = jr[taskName];
                Assert.AreEqual<string>(expectedResult, value, "Invalid result value!");

                TaskResult taskResult = sc.GetTaskResult(jid, taskName);
                Assert.IsNotNull(taskResult);
                Assert.IsTrue(taskResult.TaskLogs.StdOutLogs.Contains("outout"));
                Assert.IsTrue(taskResult.TaskLogs.StdErrLogs.Contains("errerr"));

                string taskResultValue = sc.GetTaskResultValue(jid, taskName);
                Assert.AreEqual<string>(expectedResult, taskResultValue, "Invalid task result value!");
            }
            finally
            {
                sc.KillJob(jid);
                sc.RemoveJob(jid);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void WaitForJobResultValue_TimeoutException()
        {
            string jobname = "one_minute_script_task";
            JobId jid = sc.SubmitXml(GetWorkflowPath(jobname));
            try
            {
                sc.WaitForJobResultValue(jid, 1000);
            }
            finally
            {
                sc.KillJob(jid);
                sc.RemoveJob(jid);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownJobException))]
        public void WaitForJobResultValue_UnknownJobException()
        {
            JobId invalidJid = new JobId();
            sc.WaitForJobResultValue(invalidJid, 1000);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownJobException))]
        public void GetJobState_UnknownJobException()
        {
            JobId invalidJid = new JobId();
            sc.GetJobState(invalidJid);
        }

        [TestMethod]
        public void ChangeJobPriority()
        {
            string jobname = "one_minute_script_task";
            JobId jid = sc.SubmitXml(GetWorkflowPath(jobname));
            try
            {
                sc.ChangeJobPriority(jid, JobPriority.IDLE);
                JobState jobState = sc.GetJobState(jid);
                Assert.AreEqual<JobPriority>(JobPriority.IDLE, jobState.Priority, "Invalid job priority, the ChangeJobPriority method has no effect!");
            }
            finally
            {
                sc.KillJob(jid);
                sc.RemoveJob(jid);
            }
        }

        private static string GetWorkflowPath(string name)
        {
            return Path.Combine(Environment.CurrentDirectory, @"workflow\" + name + ".xml");
        }
    }
}
