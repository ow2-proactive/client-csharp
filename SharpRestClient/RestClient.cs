using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharpRestClient.Exceptions;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SharpRestClient
{
    public class SchedulerClient
    {
        private const int DEFAULT_REQUEST_TIMEOUT_MS = 600000;
        private const int RETRY_INTERVAL_MS = 1000;

        private readonly RestClient _restClient;
        private readonly string _username;
        private readonly string _password; // to do later use SecureString see http://www.experts-exchange.com/Programming/Languages/.NET/Q_22829139.html

        private SchedulerClient(RestClient restClient, string username, string password)
        {
            this._restClient = restClient;
            this._username = username;
            this._password = password;
        }
        public static SchedulerClient Connect(string restUrl, string username, string password)
        { return Connect(restUrl, username, password, DEFAULT_REQUEST_TIMEOUT_MS); }

        public static SchedulerClient Connect(string restUrl, string username, string password, int requestTimeoutInMs)
        {
            RestClient restClient = new RestClient(restUrl);
            restClient.Timeout = requestTimeoutInMs;

            RestRequest request = new RestRequest("/scheduler/login", Method.POST);
            request.AddParameter("username", username, ParameterType.GetOrPost);
            request.AddParameter("password", password, ParameterType.GetOrPost);

            IRestResponse response = restClient.Execute(request);

            if (response.ErrorException != null)
            {
                throw new InvalidOperationException("Unable to connect to " + restUrl, response.ErrorException);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new InvalidOperationException("Unable to connect to " + restUrl + " descrition: " + response.StatusDescription);
            }

            // if not exception and the response contect size is correct then it's ok
            string sessionid = response.Content;
            restClient.Authenticator = new SIDAuthenticator(sessionid);

            return new SchedulerClient(restClient, username, password);
        }

        private static void ThrowIfNotOK(IRestResponse response)
        {
            dynamic obj;
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.NotFound:
                    obj = JObject.Parse(response.Content);
                    throw ExceptionMapper.GetNotFound((string)obj.exceptionClass, (string)obj.errorMessage);
                case System.Net.HttpStatusCode.InternalServerError:
                    obj = JObject.Parse(response.Content);
                    throw ExceptionMapper.FromInternalError((string)obj.exceptionClass, (string)obj.errorMessage);
                case System.Net.HttpStatusCode.Forbidden:
                    obj = JObject.Parse(response.Content);
                    throw ExceptionMapper.FromForbidden((string)obj.exceptionClass, (string)obj.errorMessage);
                case System.Net.HttpStatusCode.Unauthorized:
                    obj = JObject.Parse(response.Content);
                    throw ExceptionMapper.FromUnauthorized((string)obj.exceptionClass, (string)obj.errorMessage);
                default:
                    break;
            }
        }

        public bool IsConnected()
        {
            RestRequest request = new RestRequest("/scheduler/isconnected", Method.GET);
            request.AddHeader("Accept", "application/json");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<bool>(response.Content);
        }

        public Version GetVersion()
        {
            RestRequest request = new RestRequest("/scheduler/version", Method.GET);
            request.AddHeader("Accept", "application/json");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<Version>(response.Content);
        }

        public SchedulerStatus GetStatus()
        {
            RestRequest request = new RestRequest("/scheduler/status", Method.GET);
            request.AddHeader("Accept", "application/json");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<SchedulerStatus>(response.Content);
        }

        /// <summary>
        /// throws NotConnectedException, UnknownJobException, PermissionException
        /// </summary>
        public bool PauseJob(JobId jobId)
        {
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/pause", Method.PUT);
            request.AddHeader("Accept", "application/json");
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<bool>(response.Content);
        }

        /// <summary>
        /// throws NotConnectedException, UnknownJobException, PermissionException
        /// </summary>
        public bool ResumeJob(JobId jobId)
        {
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/resume", Method.PUT);
            request.AddHeader("Accept", "application/json");
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<bool>(response.Content);
        }

        /// <summary>
        /// throws NotConnectedException, UnknownJobException, PermissionException
        /// </summary>
        public bool KillJob(JobId jobId)
        {
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/kill", Method.PUT);
            request.AddHeader("Accept", "application/json");
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<bool>(response.Content);
        }

        /// <summary>
        /// throws NotConnectedException, UnknownJobException, PermissionException
        /// </summary>
        public bool RemoveJob(JobId jobId)
        {
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}", Method.DELETE);
            request.AddHeader("Accept", "application/json");
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<bool>(response.Content);
        }

        /// <summary>
        /// Change the priority of the job represented by jobId. 
        /// Only administrator can change the priority to HIGH, HIGEST, IDLE.
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="priority"></param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="UnknownJobException">if the job does not exist</exception>
        /// <exception cref="PermissionException">if you can't access to this particular job</exception>
        /// <exception cref="JobAlreadyFinishedException">if you want to change the priority on a finished job</exception>
        public void ChangeJobPriority(JobId jobId, JobPriority priority)
        {
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/priority/byvalue/{value}", Method.PUT);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddUrlSegment("value", Convert.ToString((int)priority));

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
        }

        // todo add stop/start/shutdown ...
        /// <summary>
        /// throws NotConnectedException, UnknownJobException, PermissionException
        /// </summary>
        public JobId SubmitXml(string filePath)
        {
            RestRequest request = new RestRequest("/scheduler/submit", Method.POST);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("Accept", "application/json");

            string name = Path.GetFileName(filePath);
            
            using (var xml = new FileStream(filePath, FileMode.Open))
            {
                request.AddFile("file", ReadToEnd(xml), name, "application/xml");
            }
            var response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<JobId>(response.Content);
        }

        /// <summary>
        /// throws NotConnectedException, UnknownJobException, PermissionException
        /// </summary>
        public JobState GetJobState(JobId jobId)
        {
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}", Method.GET);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddHeader("Accept", "application/json");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<JobState>(response.Content);
        }

        /// <summary>
        /// throws NotConnectedException, UnknownJobException, PermissionException
        /// </summary>
        public bool isJobAlive(JobId jobId)
        {
            return this.GetJobState(jobId).JobInfo.IsAlive();
        }

        /// <summary>
        /// throws NotConnectedException, UnknownJobException, PermissionException
        /// </summary>
        public JobResult GetJobResult(JobId jobId)
        {
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/result", Method.GET);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddHeader("Accept-Encoding", "gzip");
            request.AddHeader("Accept", "application/json");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<JobResult>(response.Content);
        }

        /// <summary>
        /// throws NotConnectedException, UnknownJobException, PermissionException
        /// </summary>
        public IDictionary<string, string> GetJobResultValue(JobId jobId)
        {
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/result/value", Method.GET);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddHeader("Accept-Encoding", "gzip");
            request.AddHeader("Accept", "application/json");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<IDictionary<string,string>>(response.Content);
        }

        /// <summary>
        /// The job is paused waiting for user to resume it.
        /// NotConnectedException, UnknownJobException, PermissionException, TimeoutException
        /// </summary>
        public JobResult WaitForJobResult(JobId jobId, int timeoutInMs)
        {
            var cts = new CancellationTokenSource(timeoutInMs);
            Task<JobResult> tr = Task.Run(async delegate
                {
                    return await WaitForJobResultAsync(jobId, cts.Token);
                }, cts.Token);
            try
            {
                tr.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    if (e is TaskCanceledException) // occurs in case of timeout
                    {
                        throw new TimeoutException("Timeout waiting for the job " + jobId);
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
            return tr.Result;
        }

        /// <summary>
        /// The job is paused waiting for user to resume it.
        /// NotConnectedException, UnknownJobException, PermissionException, TimeoutException
        /// </summary>
        public IDictionary<string,string> WaitForJobResultValue(JobId jobId, int timeoutInMs)
        {
            var cts = new CancellationTokenSource(timeoutInMs);
            Task<IDictionary<string, string>> tr = Task.Run(async delegate
            {
                return await WaitForJobResultValueAsync(jobId, cts.Token);
            }, cts.Token);
            try
            {
                tr.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    if (e is TaskCanceledException) // occurs in case of timeout
                    {
                        throw new TimeoutException("Timeout waiting for the job " + jobId);
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
            return tr.Result;
        }

        private async Task<JobResult> WaitForJobResultAsync(JobId jobId, CancellationToken cancelToken)
        {
            JobState state = GetJobState(jobId);
            if (!state.JobInfo.IsAlive())
            {
                return GetJobResult(jobId);
            }
            else
            {
                await Task.Delay(RETRY_INTERVAL_MS, cancelToken);
                return await WaitForJobResultAsync(jobId, cancelToken);
            }
        }

        private async Task<IDictionary<string, string>> WaitForJobResultValueAsync(JobId jobId, CancellationToken cancelToken)
        {
            JobState state = GetJobState(jobId);
            if (!state.JobInfo.IsAlive())
            {
                return GetJobResultValue(jobId);
            }
            else
            {
                await Task.Delay(RETRY_INTERVAL_MS, cancelToken);
                return await WaitForJobResultValueAsync(jobId, cancelToken);
            }
        }

        /// <summary>
        /// throws NotConnectedException, UnknownJobException, PermissionException
        /// </summary>
        public TaskResult GetTaskResult(JobId jobId, string taskName) 
        {
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/tasks/{taskname}/result", Method.GET);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddUrlSegment("taskname", taskName);
            request.AddHeader("Accept-Encoding", "gzip");
            request.AddHeader("Accept", "application/json");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            TaskResult tr = JsonConvert.DeserializeObject<TaskResult>(response.Content);

            tr.TaskLogs = new TaskLogs();
            tr.TaskLogs.AllLogs = GetAllTaskLogs(jobId, taskName);
            tr.TaskLogs.StdOutLogs = GetStdOutTaskLogs(jobId, taskName);
            tr.TaskLogs.StdErrLogs = GetStdErrTaskLogs(jobId, taskName);

            return tr;
        }

        /// <summary>
        /// throws NotConnectedException, UnknownJobException, PermissionException
        /// </summary>
        public string GetAllTaskLogs(JobId jobId, string taskName)
        {
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/tasks/{taskname}/result/log/all", Method.GET);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddUrlSegment("taskname", taskName);
            request.AddHeader("Accept-Encoding", "gzip");
            request.AddHeader("Accept", "application/json");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return response.Content;
        }

        /// <summary>
        /// throws NotConnectedException, UnknownJobException, PermissionException
        /// </summary>
        public string GetStdOutTaskLogs(JobId jobId, string taskName)
        {
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/tasks/{taskname}/result/log/out", Method.GET);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddUrlSegment("taskname", taskName);
            request.AddHeader("Accept-Encoding", "gzip");
            request.AddHeader("Accept", "application/json");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return response.Content;
        }

        /// <summary>
        /// throws NotConnectedException, UnknownJobException, PermissionException
        /// </summary>
        public string GetStdErrTaskLogs(JobId jobId, string taskName)
        {
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/tasks/{taskname}/result/log/err", Method.GET);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddUrlSegment("taskname", taskName);
            request.AddHeader("Accept-Encoding", "gzip");
            request.AddHeader("Accept", "application/json");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return response.Content;
        }

        /// <summary>
        /// throws NotConnectedException, UnknownJobException, PermissionException
        /// </summary>
        public string GetTaskResultValue(JobId jobId, string taskName)
        {
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/tasks/{taskname}/result/value", Method.GET);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddUrlSegment("taskname", taskName);
            request.AddHeader("Accept-Encoding", "gzip");
            request.AddHeader("Accept", "*/*");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return response.Content;
        }

        // example PushFile("GLOBALSPACE", "", "file.txt", "c:\tmp\file.txt")
        /// <summary>
        /// throws NotConnectedException, UnknownJobException, PermissionException
        /// </summary>
        public bool PushFile(string spacename, string pathname, string filename, string file)
        {
            StringBuilder urlBld = new StringBuilder("/scheduler/dataspace/");
            // spacename: GLOBALSPACE or USERSPACE
            urlBld.Append(spacename).Append("/");
            // path example: /dir1/dir2/..
            urlBld.Append(pathname);

            RestRequest request = new RestRequest(urlBld.ToString(), Method.POST);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("Accept", "application/json");
            request.AddParameter("fileName", filename, ParameterType.GetOrPost);
            using (FileStream xml = new FileStream(file, FileMode.Open))
            {
                request.AddFile("fileContent", ReadToEnd(xml), filename, "application/octet-stream");
            }

            var response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<bool>(response.Content);
        }

        // !! DANGEROUS !! - Loads all data in memory before writing to a file
        /// <summary>
        /// throws NotConnectedException, UnknownJobException, PermissionException
        /// </summary>
        public bool PullFile(string spacename, string pathname, string outputFile)
        {
            StringBuilder urlBld = new StringBuilder("/scheduler/dataspace/");
            // spacename: GLOBALSPACE or USERSPACE
            urlBld.Append(spacename).Append("/");
            // path example: /dir1/dir2/..
            urlBld.Append(pathname);

            RestRequest request = new RestRequest(urlBld.ToString(), Method.GET);
            request.AddHeader("Accept", "application/octet-stream");
            byte[] data = _restClient.DownloadData(request);
            File.WriteAllBytes(outputFile, data);
            return true;
        }

        /// <summary>
        /// Pushes files of a given directory to the dataspace.
        /// Internally calls PushFile per file to be pushed.
        /// Example PushDirectory("GLOBALSPACE", "", "c:\tmp\", "*")
        /// </summary>
        /// <param name="spacename">either GLOBALSPACE or USERSPACE</param>
        /// <param name="pathname">pathname to be used in the server, for instance /dir1/</param>
        /// <param name="localdir">path of the local directory whose content will be transferred</param>
        /// <param name="wildcard">wildcard to select some of the local files to be transferred</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="SchedulerException">if an error occurs on the server side</exception>
        /// <exception cref="ArgumentException">if arguments are not valid</exception>
        public bool PushDirectory(string spacename, string pathname, 
            string localdir, string wildcard)
        {
            
            string[] filenames = Directory.GetFiles(
                localdir, wildcard, SearchOption.AllDirectories);

            bool b = true;
            foreach (string f in filenames)
            {
                b = b && PushFile(spacename, pathname, GetRelativePath(f, localdir), f);
            }
 
            return b;
        }

        /// <summary>
        /// throws NotConnectedException, UnknownJobException, PermissionException
        /// </summary>
        public bool DeleteFile(string spacename, string pathname)
        {
            StringBuilder urlBld = new StringBuilder("/scheduler/dataspace/");
            // spacename: GLOBALSPACE or USERSPACE
            urlBld.Append(spacename).Append("/");
            // path example: /dir1/dir2/..
            urlBld.Append(pathname);
            RestRequest request = new RestRequest(urlBld.ToString(), Method.DELETE);

            var response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<bool>(response.Content);
        }

        //method for converting stream to byte[]
        private byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = stream.Position;
            stream.Position = 0;

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                stream.Position = originalPosition;
            }
        }

        // Method to get a relative path given a reference
        // directory
        private static string GetRelativePath(string filespec, string dir)
        {
            Uri pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!dir.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                dir += Path.DirectorySeparatorChar;
            }
            Uri dirUri = new Uri(dir);
            return Uri.UnescapeDataString(
                dirUri
                    .MakeRelativeUri(pathUri)
                    .ToString()
                    .Replace('/', Path.DirectorySeparatorChar)
            );
        }

    }

    /// <summary>
    /// Retains the sessionid required for each rest request
    /// </summary>
    sealed class SIDAuthenticator : IAuthenticator
    {
        private readonly string sessionid;

        public SIDAuthenticator(string newSessionid)
        {
            this.sessionid = newSessionid;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            // NetworkCredentials always makes two trips, even if with PreAuthenticate,
            // it is also unsafe for many partial trust scenarios
            // request.Credentials = Credentials;
            // thanks TweetSharp!
            // request.Credentials = new NetworkCredential(_username, _password);
            // only add the Authorization parameter if it hasn't been added by a previous Execute
            if (!request.Parameters.Any(p => p.Name.Equals("sessionid", StringComparison.OrdinalIgnoreCase)))
            {
                //var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", _username, _password)));
                //var authHeader = string.Format("Basic {0}", token);
                request.AddParameter("sessionid", this.sessionid, ParameterType.HttpHeader);
            }
        }
    }
}
