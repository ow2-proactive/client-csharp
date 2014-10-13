using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers;
using System;
using System.IO;
using System.Linq;

namespace SharpRestClient
{
    public class SchedulerClient
    {

        private readonly RestClient restClient;        
        private readonly string username;
        private readonly string password; // to do later use SecureString see http://www.experts-exchange.com/Programming/Languages/.NET/Q_22829139.html        

        private SchedulerClient(RestClient newRestClient, string newUsername, string newPassword)
        {
            this.restClient = newRestClient;
            this.username = newUsername;
            this.password = newPassword;
        }

        public static SchedulerClient connect(string restUrl, string username, string password)
        {
            
            RestClient restClient = new RestClient(restUrl);
            //restClient.Authenticator = new SimpleAuthenticator("username", username, "password", password);

            var request = new RestRequest("/scheduler/login", Method.POST);
            request.AddParameter("username", username, ParameterType.GetOrPost);
            request.AddParameter("password", password, ParameterType.GetOrPost);
           
            IRestResponse response = restClient.Execute(request);

            // if not exception and the response contect size is correct then it's ok
            string sessionid = response.Content;
            restClient.Authenticator = new SIDAuthenticator(sessionid);            
            
            return new SchedulerClient(restClient, username, password);
        }

        public SchedulerStatus GetStatus()
        {
            var request = new RestRequest("/scheduler/status", Method.GET);
            request.AddHeader("Accept", "application/json");

            IRestResponse response = restClient.Execute(request);
            string data = response.Content;
            return JsonConvert.DeserializeObject<SchedulerStatus>(data);
        }

        public bool PauseJob(JobId jobId) 
        {
            var request = new RestRequest("/scheduler/jobs/{jobid}/pause", Method.PUT);
            request.AddHeader("Accept", "application/json");
            request.AddUrlSegment( "jobid", Convert.ToString(jobId.Id));

            IRestResponse response = restClient.Execute(request);
            string data = response.Content;
            return JsonConvert.DeserializeObject<bool>(data);
        }

        public bool ResumeJob(JobId jobId)
        {
            var request = new RestRequest("/scheduler/jobs/{jobid}/resume", Method.PUT);
            request.AddHeader("Accept", "application/json");
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));

            IRestResponse response = restClient.Execute(request);
            string data = response.Content;
            return JsonConvert.DeserializeObject<bool>(data);
        }

        public bool KillJob(JobId jobId)
        {
            var request = new RestRequest("/scheduler/jobs/{jobid}/kill", Method.PUT);
            request.AddHeader("Accept", "application/json");
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));

            IRestResponse response = restClient.Execute(request);
            string data = response.Content;
            return JsonConvert.DeserializeObject<bool>(data);
        }

        public bool RemoveJob(JobId jobId)
        {
            var request = new RestRequest("/scheduler/jobs/{jobid}", Method.DELETE);
            request.AddHeader("Accept", "application/json");
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));

            IRestResponse response = restClient.Execute(request);
            string data = response.Content;
            return JsonConvert.DeserializeObject<bool>(data);
        }

        // todo add stop/start/shutdown ...

        public JobId SubmitXml(string filePath)
        {
            var request = new RestRequest("/scheduler/submit", Method.POST);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("Accept", "application/json");
            request.Timeout = 600000;
            
            string name = Path.GetFileName(filePath);

            FileStream xml = new FileStream(filePath, FileMode.Open);
            request.AddFile("file", ReadToEnd(xml), name, "application/xml");
            var response = restClient.Execute(request);

            Console.WriteLine("------> " + response.Content);            
            return JsonConvert.DeserializeObject<JobId>(response.Content);
        }

        public JobState GetJobState(JobId jobId)        
        {
            var request = new RestRequest("/scheduler/jobs/{jobid}", Method.GET);
            request.AddUrlSegment("jobid", Convert.ToString(jobId.Id));
            request.AddHeader("Accept", "application/json");

            IRestResponse response = restClient.Execute(request);
            string data = response.Content;

            Console.WriteLine("-------JSON DATA: " + data);

            return JsonConvert.DeserializeObject<JobState>(response.Content);
        }

        //method for converting stream to byte[]
        public byte[] ReadToEnd(System.IO.Stream stream)
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

    }

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
