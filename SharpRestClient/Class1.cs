using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRestClient
{
    public class SchedulerClient
    {

        private RestClient restClient;        

        public string connect(string restUrl, string username, string password)
        {
            this.restClient = new RestClient(restUrl);
            //restClient.Authenticator = new SimpleAuthenticator("username", username, "password", password);

            var request = new RestRequest("/scheduler/login", Method.POST);
            request.AddParameter("username", username, ParameterType.GetOrPost);
            request.AddParameter("password", password, ParameterType.GetOrPost);
           
            IRestResponse response = restClient.Execute(request);
            return response.Content;
        }
    }

    sealed class SchedulerAuthenticator : IAuthenticator
    {
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
                var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", _username, _password)));
                var authHeader = string.Format("Basic {0}", token);
                request.AddParameter("Authorization", authHeader, ParameterType.HttpHeader);
            }
        }
    }
}
