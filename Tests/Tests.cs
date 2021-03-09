using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpRestClient;
using System.IO;
using SharpRestClient.Exceptions;
using System.Collections.Generic;
using System.Threading;
using org.ow2.proactive.scheduler.common.task;
using org.ow2.proactive.scheduler.common.job;
using org.ow2.proactive.scripting;

namespace Tests
{
    [TestClass]
    public class RestClientTests
    {

        public static readonly string LOCAL_SERVER_URL = "http://localhost:8080";

        public static readonly string LOCAL_REST_SERVER_URL = LOCAL_SERVER_URL + "/rest";

        public static readonly string TRY_SERVER_URL = "https://system-tests-only.activeeon.com";

        public static readonly string TRY_REST_SERVER_URL = TRY_SERVER_URL + "/rest";

        public static SchedulerClient sc;

        private static string username;
        private static string password;

        private static string schedulerUrl;
        private static string schedulerRestUrl;

        [ClassInitialize]
        public static void BeforeAll(TestContext ctx)
        {
            try
            {
                Console.WriteLine("Trying to connect to " + LOCAL_REST_SERVER_URL);
                sc = SchedulerClient.Connect(LOCAL_REST_SERVER_URL, "admin", "admin");
                schedulerRestUrl = LOCAL_REST_SERVER_URL;
                schedulerUrl = LOCAL_SERVER_URL;
                username = "admin";
                password = "admin";
            }
            catch (Exception)
            {
                Console.WriteLine("No Scheduler running on localhost, trying to connect to " + TRY_REST_SERVER_URL);

                string user;
                string pass;
                if (Properties.Settings.Default.TRY_USER != null)
                {
                    user = (string)Properties.Settings.Default.TRY_USER;
                    pass = (string)Properties.Settings.Default.TRY_PASS;
                }
                else
                {
                    user = Environment.GetEnvironmentVariable("TRY_USER", EnvironmentVariableTarget.Machine);
                    pass = Environment.GetEnvironmentVariable("TRY_PASS", EnvironmentVariableTarget.Machine);
                }
                if (String.IsNullOrEmpty(user) || String.IsNullOrEmpty(pass))
                {
                    Assert.Fail("Unable to run tests! No local scheduler running and no user configured for the try platform, please edit the project properties TRY_USER and TRY_PASS");
                }
                try
                {
                    sc = SchedulerClient.Connect(TRY_REST_SERVER_URL, user, pass);
                    schedulerRestUrl = TRY_REST_SERVER_URL;
                    schedulerUrl = TRY_SERVER_URL;
                    username = user;
                    password = pass;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.ToString());
                    Assert.Fail("Unable to run tests! There is no scheduler running on try!");
                }
            }

            // Check Scheduler Verison is 6.X
            SharpRestClient.Version ver = sc.GetVersion();
            Assert.IsNotNull(ver.Scheduler);
            Assert.IsNotNull(ver.Rest);
            /*            if (!ver.Scheduler.StartsWith("7"))
                        {
                            Assert.Fail("The Scheduler version is not 7.X");
                        }
                        if (!ver.Rest.StartsWith("7"))
                        {
                            Assert.Fail("The Rest version is not 7.X");
                        }*/
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
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void PushDirectoryPullDelete()
        {
            // This test will create a set of directories and files:
            //   <testtmp>/
            //   <testtmp>/<uuid>/
            //   <testtmp>/<uuid>/file
            // and upload <testtmp>. It means the whole directory
            // <uuid> will be uploaded.

            // Create test temp dir
            string tempDirPath = GetTemporaryDirectory();

            // Create dir to be pushed
            string targetDirName = Guid.NewGuid().ToString();
            string targetDirPath = Path.Combine(tempDirPath, targetDirName);
            Directory.CreateDirectory(targetDirPath);

            // Create a file in that dir
            createFile(targetDirPath, "file");

            string remoteBaseDir = Guid.NewGuid().ToString();
            try
            {
                // Upload the temp folder
                Assert.IsTrue(
                    sc.PushDirectory("GLOBALSPACE", remoteBaseDir, tempDirPath, "*"));
            }
            finally
            {
                // Delete the target dir
                Directory.Delete(targetDirPath, true);
                Assert.IsFalse(Directory.Exists(targetDirPath));
            }

            // Pull the just uploaded files
            string file = Path.GetTempFileName();
            Assert.IsTrue(sc.PullFile("GLOBALSPACE",
                remoteBaseDir + "/" + targetDirName + "/file", file));

            // Check the downloaded file now exists
            Assert.IsTrue(File.Exists(file));

            // Delete the downloaded file
            File.Delete(file);

            // Delete the remote dir
            Assert.IsTrue(sc.DeleteFile("GLOBALSPACE", remoteBaseDir));

            // Delete the created local dir
            Directory.Delete(tempDirPath, true);
            Assert.IsFalse(Directory.Exists(tempDirPath));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PushDirectoryInvalid()
        {
            sc.PushDirectory("GLOBALSPACE", "..", "", "*");
        }

        [TestMethod]
        [ExpectedException(typeof(SchedulerException))]
        public void PushDirectoryToInvalidPath()
        {
            // Create test temp dir
            string tempDirPath = GetTemporaryDirectory();
            createFile(tempDirPath, "file");

            try
            {
                sc.PushDirectory("GLOBALSPACE", "/../../", tempDirPath, "*");
            }
            finally
            {
                Directory.Delete(tempDirPath, true);
            }
        }

        [TestMethod]
        public void SubmitXml()
        {
            string jobname = "script_task_with_result";
            JobIdData jid = sc.SubmitXml(GetWorkflowPath(jobname));
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
        public void SubmitFromUrl()
        {
            string jobname = "Pre_Post_Clean_Scripts";
            JobIdData jid = sc.SubmitFromUrl(schedulerUrl + "/catalog/buckets/basic-examples/resources/Pre_Post_Clean_Scripts/raw");
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

        private TaskFlowJob createHelloWorldJob()
        {
            TaskFlowJob job = new TaskFlowJob();
            job.Name = "Hello World Job";
            ScriptTask task = new ScriptTask();
            task.Name = "hello_task";
            TaskScript script = new TaskScript(new SimpleScript("println 'Hello World'; result = 'OK'", "groovy", new string[0]));
            task.Script = script;
            task.PreciousResult = true;
            job.addTask(task);
            return job;
        }

        private TaskFlowJob createGetVarJob(string varName, string giName)
        {
            TaskFlowJob job = new TaskFlowJob();
            job.Name = "Get Var Job";
            ScriptTask task = new ScriptTask();
            task.Name = "get_var_task";
            TaskScript script = new TaskScript(new SimpleScript("result = variables.get('" + varName + "')", "groovy", new string[0]));
            task.Script = script;
            task.PreciousResult = true;
            job.addTask(task);
            ScriptTask task2 = new ScriptTask();
            task2.Name = "get_gi_task";
            TaskScript script2 = new TaskScript(new SimpleScript("result = genericInformation.get('" + giName + "')", "groovy", new string[0]));
            task2.Script = script2;
            task2.PreciousResult = true;
            job.addTask(task2);
            return job;
        }

        [TestMethod]
        public void SubmitJobAndWait()
        {
            TaskFlowJob job = createHelloWorldJob();
            JobIdData jid = sc.SubmitJob(job);
            try
            {
                Assert.AreNotEqual<long>(0, jid.Id, "After submission the job id is invalid!");
                Assert.AreEqual<string>("Hello World Job", jid.ReadableName, "After submission the job name is invalid!");

                JobResult jr = sc.WaitForJobResult(jid, 30000);
                Assert.IsNotNull(jr);
                TaskResult tr = jr.Tasks["hello_task"];
                Assert.IsNotNull(tr);
                Assert.IsNotNull(tr.PropagatedVariables);
                Assert.AreEqual<string>("Hello World Job", tr.PropagatedVariables["PA_JOB_NAME"]);
                Assert.AreEqual("OK", tr.Value);
                TaskResult tr2 = jr.PreciousTasks["hello_task"];
                Assert.IsNotNull(tr2);
                Assert.AreEqual("OK", tr2.Value);
            }
            finally
            {
                sc.KillJob(jid);
                sc.RemoveJob(jid);
            }
        }

        [TestMethod]
        public void SubmitJobExtraVarsAndGenericInfoAndWait()
        {
            TaskFlowJob job = createGetVarJob("my_var", "my_gi");
            Dictionary<string, string> variables = new Dictionary<string, string>()
            {
                {  "my_var", "my_value" }
            };
            Dictionary<string, string> genericInfo = new Dictionary<string, string>()
            {
                {  "my_gi", "my_gi_value" }
            };
            JobIdData jid = sc.SubmitJob(job, variables, genericInfo);
            try
            {
                Assert.AreNotEqual<long>(0, jid.Id, "After submission the job id is invalid!");
                Assert.AreEqual<string>("Get Var Job", jid.ReadableName, "After submission the job name is invalid!");

                JobResult jr = sc.WaitForJobResult(jid, 30000);
                Assert.IsNotNull(jr);
                TaskResult tr = jr.Tasks["get_var_task"];
                Assert.IsNotNull(tr);
                Assert.IsNotNull(tr.PropagatedVariables);
                Assert.AreEqual<string>("Get Var Job", tr.PropagatedVariables["PA_JOB_NAME"]);
                Assert.AreEqual("my_value", tr.Value);
                TaskResult tr2 = jr.Tasks["get_gi_task"];
                Assert.IsNotNull(tr2);
                Assert.AreEqual("my_gi_value", tr2.Value);
            }
            finally
            {
                sc.KillJob(jid);
                sc.RemoveJob(jid);
            }
        }

        [TestMethod]
        public void ConnectWithCredentials()
        {
            byte[] credentials = sc.CreateCredentials(username, password);

            Assert.AreNotEqual<int>(0, credentials.Length, "Empty array returned by CreateCredentials!");

            string tempFilePath = Path.GetTempFileName();
            File.WriteAllBytes(tempFilePath, credentials);

            Console.WriteLine("Trying to connect to " + schedulerRestUrl);
            SchedulerClient sc2 = SchedulerClient.Connect(schedulerRestUrl, tempFilePath);

            // verify that we can perform an operation with the new connection
            byte[] credentials2 = sc2.CreateCredentials(username, password);

            Assert.AreNotEqual<int>(0, credentials2.Length, "Empty array returned by CreateCredentials!");

        }

        [TestMethod]
        public void PauseResumeJob()
        {
            string jobname = "LoopJob";
            JobIdData jid = sc.SubmitXml(GetWorkflowPath(jobname));
            Thread.Sleep(1000);
            try
            {
                bool isPaused = sc.PauseJob(jid);
                Assert.AreEqual<bool>(true, isPaused, "Unable to pause the job!");
                bool isResumed = sc.ResumeJob(jid);
                Assert.AreEqual<bool>(true, isResumed, "Unable to resume the job!");
            }
            finally
            {
                sc.KillJob(jid);
                sc.RemoveJob(jid);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownJobException))]
        public void PauseJob_UnknwonJobException()
        {
            // Check unknown jobid
            JobIdData invalidJid = new JobIdData();
            sc.PauseJob(invalidJid);
        }

        [TestMethod]
        public void WaitForJobResult()
        {
            string jobname = "script_task_with_result";
            JobIdData jid = sc.SubmitXml(GetWorkflowPath(jobname));
            try
            {
                JobResult jr = sc.WaitForJobResult(jid, 30000);
                Assert.IsNotNull(jr);
                TaskResult tr = jr.Tasks["simple_task"];
                Assert.IsNotNull(tr);
                Assert.IsNotNull(tr.PropagatedVariables);
                Assert.AreEqual<string>(jobname, tr.PropagatedVariables["PA_JOB_NAME"]);
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
            JobIdData jid = sc.SubmitXml(GetWorkflowPath(jobname));
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
            JobIdData invalidJid = new JobIdData();
            sc.WaitForJobResult(invalidJid, 1000);
        }

        [TestMethod]
        public void WaitForJobResultValue()
        {
            string jobName = "stask_result_out_err";
            string taskName = "simple_task";
            string expectedResult = "hello";
            JobIdData jid = sc.SubmitXml(GetWorkflowPath(jobName));
            try
            {
                IDictionary<string, string> jr = sc.WaitForJobResultValue(jid, 30000);
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
            JobIdData jid = sc.SubmitXml(GetWorkflowPath(jobname));
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
            JobIdData invalidJid = new JobIdData();
            sc.WaitForJobResultValue(invalidJid, 1000);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownJobException))]
        public void GetJobState_UnknownJobException()
        {
            JobIdData invalidJid = new JobIdData();
            sc.GetJobState(invalidJid);
        }

        [TestMethod]
        public void GetJobTaskState()
        {
            string jobname = "one_minute_script_task";
            JobIdData jid = sc.SubmitXml(GetWorkflowPath(jobname));
            try
            {
                Thread.Sleep(1000);
                JobState jobState = sc.GetJobState(jid);
                Assert.AreEqual<JobStatus>(JobStatus.RUNNING, jobState.JobInfo.Status);
                Assert.AreEqual<TaskStatus>(TaskStatus.RUNNING, jobState.Tasks["0"].TaskInfo.TaskStatus);
                TaskState taskState = sc.GetTaskState(jid, "simple_task");
                Assert.IsNotNull(taskState);
                Assert.AreEqual<TaskStatus>(TaskStatus.RUNNING, taskState.TaskInfo.TaskStatus);
            }
            finally
            {
                sc.KillJob(jid);
                sc.RemoveJob(jid);
            }
        }

        [TestMethod]
        public void ChangeJobPriority()
        {
            string jobname = "one_minute_script_task";
            JobIdData jid = sc.SubmitXml(GetWorkflowPath(jobname));
            try
            {
                sc.ChangeJobPriority(jid, JobPriorityData.IDLE);
                JobState jobState = sc.GetJobState(jid);
                Assert.AreEqual<JobPriorityData>(JobPriorityData.IDLE, jobState.Priority, "Invalid job priority, the ChangeJobPriority method has no effect!");
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

        private static string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        private static string createFile(string targetDir, string fileName)
        {
            string filePath = Path.Combine(targetDir, fileName);
            using (File.Create(filePath)) { }
            return filePath;
        }


    }
}
