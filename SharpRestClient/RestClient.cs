using Newtonsoft.Json;
using RestSharp;
using RestSharp.Contrib;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharpRestClient.Exceptions;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using org.ow2.proactive.scheduler.common.job;
using org.ow2.proactive.scheduler.common.job.factories;

namespace SharpRestClient
{

    /// <summary>
    /// Entry-Point of the Scheduler API
    /// </summary>
    public class SchedulerClient
    {
        private const int DEFAULT_REQUEST_TIMEOUT_MS = 600000;
        private const int RETRY_INTERVAL_MS = 1000;

        private readonly RestClient _restClient;
        private readonly string _username;
        private readonly string _password; // to do later use SecureString see http://www.experts-exchange.com/Programming/Languages/.NET/Q_22829139.html
        private readonly byte[] _credentialBytes;      

        private SchedulerClient(RestClient restClient, string username, string password)
        {
            this._restClient = restClient;
            this._username = username;
            this._password = password;
        }

        private SchedulerClient(RestClient restClient, byte[] credentialBytes)
        {
            this._restClient = restClient;
            this._credentialBytes = credentialBytes;
        }

        /// <summary>
        /// Connects to a running ProActive Scheduler using login and password
        /// </summary>
        /// <param name="restUrl">rest url of the scheduler</param>
        /// <param name="username">login name of the user</param>
        /// <param name="password">password of the user</param>
        public static SchedulerClient Connect(string restUrl, string username, string password)
        { return Connect(restUrl, username, password, null, DEFAULT_REQUEST_TIMEOUT_MS); }

        /// <summary>
        /// Connects to a running ProActive Scheduler using a credential file
        /// </summary>
        /// <param name="restUrl">rest url of the scheduler</param>
        /// <param name="credentialFile">path to a credential file on the local file system</param>
        public static SchedulerClient Connect(string restUrl, string credentialFile)
        { return Connect(restUrl, null, null, credentialFile, DEFAULT_REQUEST_TIMEOUT_MS); }

        /// <summary>
        /// Connects to a running ProActive Scheduler
        /// </summary>
        /// <param name="restUrl">rest url of the scheduler</param>
        /// <param name="username">login name of the user</param>
        /// <param name="password">password of the user</param>
        /// <param name="credentialFile">path to a credential file (replace username and password)</param>
        /// <param name="requestTimeoutInMs">timeout in milliseconds for the connection</param>
        public static SchedulerClient Connect(string restUrl, string username, string password, string credentialFile, int requestTimeoutInMs)
        {
            byte[] credentialBytes = null;
            RestClient restClient = new RestClient(restUrl);
            restClient.Timeout = requestTimeoutInMs;

            RestRequest request = new RestRequest("/scheduler/login", Method.POST);
            request.AddHeader("Content-Type", "multipart/form-data");
            if (username != null)
            {
                request.AddParameter("username", username, ParameterType.GetOrPost);
            }
            if (password != null)
            {
                request.AddParameter("password", password, ParameterType.GetOrPost);
            }
            if (credentialFile != null)
            {
                if (File.Exists(credentialFile))
                {
                    using (FileStream cred = new FileStream(credentialFile, FileMode.Open))
                    {
                        credentialBytes = ReadToEnd(cred);
                        request.AddFile("credential", credentialBytes, credentialFile, "application/octet-stream");
                    }
                } else
                {
                    throw new IOException("Credential File " + credentialFile + " does not exit.");
                }
            }

            IRestResponse response = restClient.Execute(request);

            if (response.ErrorException != null)
            {
                throw new InvalidOperationException("Unable to connect to " + restUrl, response.ErrorException);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new InvalidOperationException("Unable to connect to " + restUrl + " description: " + response.Content);
            }

            // if not exception and the response content size is correct then it's ok
            string sessionid = response.Content;
            restClient.Authenticator = new SIDAuthenticator(sessionid);

            if (credentialBytes != null)
            {
                return new SchedulerClient(restClient, credentialBytes);
            } else
            {
                return new SchedulerClient(restClient, username, password);
            }

            
        }

        /// <summary>
        /// Serialize the given object using NetDataContractSerializer
        /// </summary>
        /// <param name="obj">object to serialize</param>
        public static string SerializeWithNetDcs(object obj)
        {
            using (var ms = new MemoryStream())
            {
                using (var sr = new StreamReader(ms, Encoding.UTF8))
                {
                    var serializer = new NetDataContractSerializer();
                    serializer.WriteObject(ms, obj);
                    ms.Position = 0;
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Deserialize the given xml string using NetDataContractSerializer
        /// </summary>
        /// <param name="xml">xml string to deserialize</param>
        public static object DeserializeWithNetDcs(string xml)
        {
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(ms, Encoding.UTF8))
                {
                    sw.Write(xml);
                    sw.Flush();
                    ms.Position = 0;
                    var deserializer = new NetDataContractSerializer();
                    return deserializer.ReadObject(ms);
                }
            }
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
                    throw ExceptionMapper.FromInternalError((string)obj.exceptionClass, (string)obj.errorMessage, (string)obj.stackTrace);
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

        /// <summary>
        /// Returns true is there is a valid connection to the scheduler
        /// </summary>
        public bool IsConnected()
        {
            RestRequest request = new RestRequest("/scheduler/isconnected", Method.GET);
            request.AddHeader("Accept", "application/json");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<bool>(response.Content);
        }

        /// <summary>
        /// Renew the current user session
        /// </summary>
        private void renewSession()
        {

            RestRequest request = new RestRequest("/scheduler/session", Method.PUT);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("sessionid", ((SIDAuthenticator) _restClient.Authenticator).sessionid);
            if (_username != null)
            {
                request.AddParameter("username", _username, ParameterType.GetOrPost);
            }
            if (_password != null)
            {
                request.AddParameter("password", _password, ParameterType.GetOrPost);
            }
            if (_credentialBytes != null)
            {
                request.AddFile("credential", _credentialBytes, "credentials", "application/octet-stream");                
            }

            IRestResponse response = _restClient.Execute(request);

            if (response.ErrorException != null)
            {
                throw new InvalidOperationException("Unable to renew session of " + _restClient.BaseUrl, response.ErrorException);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new InvalidOperationException("Unable to renew session of " + _restClient.BaseUrl + " description: " + response.Content);
            }

            // if not exception and the response content size is correct then it's ok
            string sessionid = response.Content;
            _restClient.Authenticator = new SIDAuthenticator(sessionid);
        }

        /// <summary>
        /// Create a credential file using the provided username and password
        /// </summary>
        /// <param name="username">login name of the user</param>
        /// <param name="password">password of the user</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="PermissionException">if you are not allowed to create credentials</exception>
        public byte[] CreateCredentials(string username, string password)
        {
            RestRequest request = new RestRequest("/scheduler/createcredential", Method.POST);
            request.AddParameter("username", username, ParameterType.GetOrPost);
            request.AddParameter("password", password, ParameterType.GetOrPost);
            request.AlwaysMultipartFormData = true;
            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            byte[] data = _restClient.DownloadData(request);
            return data;
        }

        /// <summary>
        /// Returns the scheduler version
        /// </summary>
        public Version GetVersion()
        {
            renewSession();
            RestRequest request = new RestRequest("/scheduler/version", Method.GET);
            request.AddHeader("Accept", "application/json");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<Version>(response.Content);
        }

        /// <summary>
        /// Gets the current status of the scheduler (Started, Stopped, etc)
        /// </summary>
        public SchedulerStatus GetStatus()
        {
            renewSession();
            RestRequest request = new RestRequest("/scheduler/status", Method.GET);
            request.AddHeader("Accept", "application/json");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<SchedulerStatus>(response.Content);
        }

        /// <summary>
        /// Pause the running job given its jobId
        /// </summary>
        /// <param name="jobId">id of the job</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="UnknownJobException">if the job does not exist</exception>
        /// <exception cref="PermissionException">if you can't access to this particular job</exception>
        /// <exception cref="JobAlreadyFinishedException">if you want to pause an already finished job</exception>
        public bool PauseJob(JobIdData jobId)
        {
            renewSession();
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/pause", Method.PUT);
            request.AddHeader("Accept", "application/json");
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<bool>(response.Content);
        }

        /// <summary>
        /// Resume a paused job given its jobId
        /// </summary>
        /// <param name="jobId">id of the job</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="UnknownJobException">if the job does not exist</exception>
        /// <exception cref="PermissionException">if you can't access to this particular job</exception>
        /// <exception cref="JobAlreadyFinishedException">if you want to resume an already finished job</exception>
        public bool ResumeJob(JobIdData jobId)
        {
            renewSession();
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/resume", Method.PUT);
            request.AddHeader("Accept", "application/json");
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<bool>(response.Content);
        }

        /// <summary>
        /// Kill a job given its jobId
        /// </summary>
        /// <param name="jobId">id of the job</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="UnknownJobException">if the job does not exist</exception>
        /// <exception cref="PermissionException">if you can't access to this particular job</exception>
        public bool KillJob(JobIdData jobId)
        {
            renewSession();
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/kill", Method.PUT);
            request.AddHeader("Accept", "application/json");
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<bool>(response.Content);
        }

        /// <summary>
        /// Remove a job from the scheduler memory. This call can also kill a running job.
        /// </summary>
        /// <param name="jobId">id of the job</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="UnknownJobException">if the job does not exist</exception>
        /// <exception cref="PermissionException">if you can't access to this particular job</exception>
        public bool RemoveJob(JobIdData jobId)
        {
            renewSession();
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
        /// <param name="jobId">id of the job</param>
        /// <param name="priority"></param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="UnknownJobException">if the job does not exist</exception>
        /// <exception cref="PermissionException">if you can't access to this particular job</exception>
        /// <exception cref="JobAlreadyFinishedException">if you want to change the priority on a finished job</exception>
        public void ChangeJobPriority(JobIdData jobId, JobPriorityData priority)
        {
            renewSession();
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/priority/byvalue/{value}", Method.PUT);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddUrlSegment("value", Convert.ToString((int)priority));

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
        }

        // todo add stop/start/shutdown ...

        /// <summary>
        /// Submits a xml workflow accessible from the local file system       
        /// </summary>
        /// <param name="job">Task Flow job to submit</param>
        /// <param name="variables">a dictionary of job variables to configure the job execution</param>
        /// <param name="genericInfo">a dictionary of generic information to configure the job execution</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception> 
        /// <exception cref="PermissionException">if you are not allowed to submit a job</exception>
        public JobIdData SubmitJob(TaskFlowJob job, IDictionary<string, string> variables = null, IDictionary<string, string> genericInfo = null, bool printXml = false)
        {
            Job2XMLTransformer transformer = new Job2XMLTransformer();
            string jobString = transformer.jobToxmlString(job);
            if (printXml)
            {
                Console.WriteLine(jobString);
            }            
            string url = getSubmitUrlWithVariables("/scheduler/submit", variables);
            return _SubmitString(url, jobString, genericInfo);
        }


        /// <summary>
        /// Submits a xml workflow accessible from the local file system
        /// </summary>
        /// <param name="filePath">path to the xml workflow on the local file system</param>
        /// <param name="variables">a dictionary of job variables to configure the job execution</param>
        /// <param name="genericInfo">a dictionary of generic information to configure the job execution</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception> 
        /// <exception cref="PermissionException">if you are not allowed to submit a job</exception>
        public JobIdData SubmitXml(string filePath, IDictionary<string,string> variables = null, IDictionary<string,string> genericInfo = null) {
            string url = getSubmitUrlWithVariables("/scheduler/submit", variables);          
            return _SubmitXml(url, filePath, genericInfo);
        }


        /// <summary>
        /// Submits a xml workflow accessible from the given url
        /// </summary>
        /// <param name="workflowUrl">url used to access the xml workflow</param>
        /// <param name="variables">a dictionary of job variables to configure the job execution</param>
        /// <param name="genericInfo">a dictionary of generic information to configure the job execution</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception> 
        /// <exception cref="PermissionException">if you are not allowed to submit a job</exception>
        public JobIdData SubmitFromUrl(string workflowUrl, IDictionary<string, string> variables = null, IDictionary<string, string> genericInfo = null)
        {
            string submissionUrl = getSubmitUrlWithVariables("/scheduler/jobs", variables); ;                   
            return _SubmitUrl(submissionUrl, workflowUrl, genericInfo);
        }

        private string getSubmitUrlWithVariables(string baseUrl, IDictionary<string, string> variables)
        {
            String url = null;
            if (variables == null || variables.Count == 0)
            {
                url = baseUrl;
            }
            else
            {
                StringBuilder buf = new StringBuilder(baseUrl);
                foreach (KeyValuePair<string, string> keyValue in variables)
                {
                    buf.Append(';').Append(HttpUtility.UrlEncode(keyValue.Key)).Append("=").Append(HttpUtility.UrlEncode(keyValue.Value));
                }
                url = buf.ToString();
            }
            return url;
        }

        private JobIdData _SubmitString(string url, string jobString, IDictionary<string, string> genericInfo = null)
        {
            renewSession();
            RestRequest request = new RestRequest(url, Method.POST);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("Accept", "application/json");
            if (genericInfo != null && genericInfo.Count > 0)
            {
                foreach (KeyValuePair<string, string> pair in genericInfo)
                {
                    request.AddParameter(pair.Key, pair.Value, ParameterType.QueryString);
                }
            }

            Encoding encoding = Encoding.UTF8;

            request.AddFile("file", encoding.GetBytes(jobString), "job", "application/xml");

            var response = _restClient.Execute(request);

            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<JobIdData>(response.Content);
        }

        private JobIdData _SubmitXml(string url, string filePath, IDictionary<string, string> genericInfo)
        {
            renewSession();
            RestRequest request = new RestRequest(url, Method.POST);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("Accept", "application/json");
            if (genericInfo != null && genericInfo.Count > 0) {
                foreach (KeyValuePair<string, string> pair in genericInfo) {
                    request.AddParameter(pair.Key, pair.Value, ParameterType.QueryString);
                }
            }
                    
            string name = Path.GetFileName(filePath);
            using (var xml = new FileStream(filePath, FileMode.Open))
            {
                request.AddFile("file", ReadToEnd(xml), name, "application/xml");
            }

            var response = _restClient.Execute(request);

            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<JobIdData>(response.Content);
        }

        private JobIdData _SubmitUrl(string submissionUrl, string workflowUrl, IDictionary<string, string> genericInfo)
        {
            renewSession();
            RestRequest request = new RestRequest(submissionUrl, Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("link", workflowUrl);
            if (genericInfo != null && genericInfo.Count > 0)
            {
                foreach (KeyValuePair<string, string> pair in genericInfo)
                {
                    request.AddParameter(pair.Key, pair.Value, ParameterType.QueryString);
                }
            }

            var response = _restClient.Execute(request);

            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<JobIdData>(response.Content);
        }

        /// <summary>
        /// Returns the state of a job given its jobId
        /// </summary>
        /// <param name="jobId">id of the job</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="UnknownJobException">if the job does not exist</exception>
        /// <exception cref="PermissionException">if you can't access to this particular job</exception>
        public JobState GetJobState(JobIdData jobId)
        {
            renewSession();
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}", Method.GET);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddHeader("Accept", "application/json");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<JobState>(response.Content);
        }

        /// <summary>
        /// Returns the state of a task given its jobId and task name
        /// </summary>
        /// <param name="jobId">id of the job</param>
        /// <param name="taskName">the task name</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="UnknownJobException">if the job does not exist</exception>
        /// <exception cref="UnknownTaskException">if the task does not exist</exception>
        /// <exception cref="PermissionException">if you can't access to this particular job</exception>
        public TaskState GetTaskState(JobIdData jobId, string taskName)
        {
            renewSession();
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/tasks/{taskName}", Method.GET);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddUrlSegment("taskName", taskName);
            request.AddHeader("Accept", "application/json");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<TaskState>(response.Content);
        }



        /// <summary>
        /// Returns true if the job is not finished yet
        /// </summary>
        /// <param name="jobId">id of the job</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="UnknownJobException">if the job does not exist</exception>
        /// <exception cref="PermissionException">if you can't access to this particular job</exception>
        public bool isJobAlive(JobIdData jobId)
        {
            return this.GetJobState(jobId).JobInfo.IsAlive();
        }

        /// <summary>
        /// Returns the result of the given job. The JobResult structure contains multiple information regarding the job execution
        /// </summary>
        /// <param name="jobId">id of the job</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="UnknownJobException">if the job does not exist</exception>
        /// <exception cref="PermissionException">if you can't access to this particular job</exception>
        public JobResult GetJobResult(JobIdData jobId)
        {
            renewSession();
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/result", Method.GET);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddHeader("Accept-Encoding", "gzip");
            request.AddHeader("Accept", "application/json");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<JobResult>(response.Content);
        }

        /// <summary>
        /// Returns the string result value of the given job
        /// </summary>
        /// <param name="jobId">id of the job</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="UnknownJobException">if the job does not exist</exception>
        /// <exception cref="PermissionException">if you can't access to this particular job</exception>
        public IDictionary<string, string> GetJobResultValue(JobIdData jobId)
        {
            renewSession();
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/result/value", Method.GET);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddHeader("Accept-Encoding", "gzip");
            request.AddHeader("Accept", "application/json");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return JsonConvert.DeserializeObject<IDictionary<string,string>>(response.Content);
        }

        /// <summary>
        /// Wait until the given job is finished and return its result.
        /// NotConnectedException, UnknownJobException, PermissionException, TimeoutException
        /// </summary>
        /// <param name="jobId">id of the job</param>
        /// <param name="timeoutInMs">maximum wait time in milliseconds</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="UnknownJobException">if the job does not exist</exception>
        /// <exception cref="PermissionException">if you can't access to this particular job</exception>
        public JobResult WaitForJobResult(JobIdData jobId, int timeoutInMs)
        {
            renewSession();
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
        /// Wait until the given job is finished and return its string value result.
        /// </summary>
        /// <param name="jobId">id of the job</param>
        /// <param name="timeoutInMs">maximum wait time in milliseconds</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="UnknownJobException">if the job does not exist</exception>
        /// <exception cref="PermissionException">if you can't access to this particular job</exception>
        public IDictionary<string,string> WaitForJobResultValue(JobIdData jobId, int timeoutInMs)
        {
            renewSession();
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

        private async Task<JobResult> WaitForJobResultAsync(JobIdData jobId, CancellationToken cancelToken)
        {
            renewSession();
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

        private async Task<IDictionary<string, string>> WaitForJobResultValueAsync(JobIdData jobId, CancellationToken cancelToken)
        {
            renewSession();
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
        /// Returns the result of the given task. The TaskResult structure contains multiple information regarding the task execution
        /// </summary>
        /// <param name="jobId">id of the job</param>
        /// <param name="taskName">name of the task inside the given job</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="UnknownJobException">if the job does not exist</exception>
        /// <exception cref="PermissionException">if you can't access to this particular job</exception>
        public TaskResult GetTaskResult(JobIdData jobId, string taskName) 
        {
            renewSession();
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
        /// Returns the output and error log of the given finished task.
        /// </summary>
        /// <param name="jobId">id of the job</param>
        /// <param name="taskName">name of the task inside the given job</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="UnknownJobException">if the job does not exist</exception>
        /// <exception cref="PermissionException">if you can't access to this particular job</exception>
        public string GetAllTaskLogs(JobIdData jobId, string taskName)
        {
            renewSession();
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/tasks/{taskname}/result/log/all", Method.GET);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddUrlSegment("taskname", taskName);
            request.AddHeader("Accept-Encoding", "gzip");
            request.AddHeader("Accept", "text/plain");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return response.Content;
        }

        /// <summary>
        /// Returns the output log of the given finished task.
        /// </summary>
        /// <param name="jobId">id of the job</param>
        /// <param name="taskName">name of the task inside the given job</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="UnknownJobException">if the job does not exist</exception>
        /// <exception cref="PermissionException">if you can't access to this particular job</exception>
        public string GetStdOutTaskLogs(JobIdData jobId, string taskName)
        {
            renewSession();
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/tasks/{taskname}/result/log/out", Method.GET);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddUrlSegment("taskname", taskName);
            request.AddHeader("Accept-Encoding", "gzip");
            request.AddHeader("Accept", "text/plain");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return response.Content;
        }

        /// <summary>
        /// Returns the error log of the given finished task.
        /// </summary>
        /// <param name="jobId">id of the job</param>
        /// <param name="taskName">name of the task inside the given job</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="UnknownJobException">if the job does not exist</exception>
        /// <exception cref="PermissionException">if you can't access to this particular job</exception>
        public string GetStdErrTaskLogs(JobIdData jobId, string taskName)
        {
            renewSession();
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/tasks/{taskname}/result/log/err", Method.GET);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddUrlSegment("taskname", taskName);
            request.AddHeader("Accept-Encoding", "gzip");
            request.AddHeader("Accept", "text/plain");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return response.Content;
        }

        /// <summary>
        /// Returns the string value of the given task result.
        /// </summary>
        /// <param name="jobId">id of the job</param>
        /// <param name="taskName">name of the task inside the given job</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="UnknownJobException">if the job does not exist</exception>
        /// <exception cref="PermissionException">if you can't access to this particular job</exception>
        public string GetTaskResultValue(JobIdData jobId, string taskName)
        {
            renewSession();
            RestRequest request = new RestRequest("/scheduler/jobs/{jobid}/tasks/{taskname}/result/value", Method.GET);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddUrlSegment("taskname", taskName);
            request.AddHeader("Accept-Encoding", "gzip");
            request.AddHeader("Accept", "*/*");

            IRestResponse response = _restClient.Execute(request);
            ThrowIfNotOK(response);
            return response.Content;
        }

        /// <summary>
        /// Push the given file, accessible from the local file system to a remote scheduler server data space (GLOBALSPACE or USERSPACE).
        /// 
        /// example PushFile("GLOBALSPACE", "", "file.txt", "c:\tmp\file.txt")
        /// </summary>
        /// <param name="spacename">Server dataspace name, can either be GLOBALSPACE or USERSPACE</param>
        /// <param name="pathname">Remote path inside the remote dataspace, use "" to push the file inside the dataspace root</param>
        /// <param name="filename">file name to use on the remote dataspace</param>
        /// <param name="file">path to the local file</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="PermissionException">if you are not allowed to push files</exception>
        /// <exception cref="SchedulerException">if an error occurs on the server side</exception>
        /// <exception cref="ArgumentException">if arguments are not valid</exception>
        public bool PushFile(string spacename, string pathname, string filename, string file)
        {
            renewSession();
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
        /// Pull the given file, accessible from a remote data space (GLOBALSPACE or USERSPACE), to a local file.
        /// 
        /// example PullFile("GLOBALSPACE", "file.txt", "c:\tmp\file.txt")
        /// </summary>
        /// <param name="spacename">Server dataspace name, can either be GLOBALSPACE or USERSPACE</param>
        /// <param name="pathname">Remote path inside the remote dataspace to copy from</param>
        /// <param name="outputFile">path to the local file</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="PermissionException">if you are not allowed to push files</exception>
        /// <exception cref="SchedulerException">if an error occurs on the server side</exception>
        /// <exception cref="ArgumentException">if arguments are not valid</exception>
        public bool PullFile(string spacename, string pathname, string outputFile)
        {
            renewSession();
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
            renewSession();
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
        /// Delete a file from the remote data space (GLOBALSPACE or USERSPACE).
        /// 
        /// example DeleteFile("GLOBALSPACE", "file.txt")
        /// </summary>
        /// <param name="spacename">Server dataspace name, can either be GLOBALSPACE or USERSPACE</param>
        /// <param name="pathname">Remote path inside the remote dataspace, use "" to push the file inside the dataspace root</param>
        /// <exception cref="NotConnectedException">if you are not authenticated</exception>
        /// <exception cref="PermissionException">if you are not allowed to push files</exception>
        /// <exception cref="SchedulerException">if an error occurs on the server side</exception>
        /// <exception cref="ArgumentException">if arguments are not valid</exception>
        public bool DeleteFile(string spacename, string pathname)
        {
            renewSession();
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
        private static byte[] ReadToEnd(System.IO.Stream stream)
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
        public readonly string sessionid;

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
