using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.IO;
namespace SharpRestClient.Exceptions
{
    public class SchedulerException : Exception
    {
        public SchedulerException() : base() { }
        public SchedulerException(string message) : base(message) { }
        public SchedulerException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class LoginException : SchedulerException
    {
        public LoginException() : base() { }
        public LoginException(string message) : base(message) { }
        public LoginException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class NotConnectedException : SchedulerException
    {
        public NotConnectedException() : base() { }
        public NotConnectedException(string message) : base(message) { }
        public NotConnectedException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class PermissionException : SchedulerException
    {
        public PermissionException() : base() { }
        public PermissionException(string message) : base(message) { }
        public PermissionException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class SubmissionClosedException : SchedulerException
    {
        public SubmissionClosedException() : base() { }
        public SubmissionClosedException(string message) : base(message) { }
        public SubmissionClosedException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class JobAlreadyFinishedException : SchedulerException
    {
        public JobAlreadyFinishedException() : base() { }
        public JobAlreadyFinishedException(string message) : base(message) { }
        public JobAlreadyFinishedException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class JobCreationException : SchedulerException
    {
        public JobCreationException() : base() { }
        public JobCreationException(string message) : base(message) { }
        public JobCreationException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class UserException : SchedulerException
    {
        public UserException() : base() { }
        public UserException(string message) : base(message) { }
        public UserException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class UnknownTaskException : SchedulerException
    {
        public UnknownTaskException() : base() { }
        public UnknownTaskException(string message) : base(message) { }
        public UnknownTaskException(string message, Exception innerException) : base(message, innerException) { }
        public UnknownTaskException(string taskName, JobId jobId) : base("The task " + taskName + " of job " + jobId + " is unknown!") { }
        public UnknownTaskException(TaskId taskId, JobId jobId) : base("The task " + taskId + " of job " + jobId + " is unknown!") { }
    }

    public class UnknownJobException : SchedulerException
    {
        public UnknownJobException() : base() { }
        public UnknownJobException(string message) : base(message) { }
        public UnknownJobException(string message, Exception innerException) : base(message, innerException) { }
        public UnknownJobException(JobId jobId) : base("The job " + jobId + " does not exists!") { }
    }

    public sealed class ExceptionMapper
    {
        public const string T = "java.lang.Throwable";
        public const string RE = "java.lang.RuntimeException";
        public const string IAE = "java.lang.IllegalArgumentException";
        public const string IOE = "java.io.IOException";
        public const string KE = "java.security.KeyException";
        public const string LE = "javax.security.auth.login.LoginException";
        public const string NCE = "org.ow2.proactive.scheduler.common.exception.NotConnectedException";
        public const string JAFE = "org.ow2.proactive.scheduler.common.exception.JobAlreadyFinishedException";
        public const string UTE = "org.ow2.proactive.scheduler.common.exception.UnknownTaskException";
        public const string PE = "org.ow2.proactive.scheduler.common.exception.PermissionException";
        public const string JCRE = "org.ow2.proactive_grid_cloud_portal.scheduler.exception.JobCreationRestException";
        public const string NCRE = "org.ow2.proactive_grid_cloud_portal.scheduler.exception.NotConnectedRestException";
        public const string PRE = "org.ow2.proactive_grid_cloud_portal.scheduler.exception.PermissionRestException";
        public const string SRE = "org.ow2.proactive_grid_cloud_portal.scheduler.exception.SchedulerRestException";
        public const string SCRE = "org.ow2.proactive_grid_cloud_portal.scheduler.exception.SubmissionClosedRestException";
        public const string UJRE = "org.ow2.proactive_grid_cloud_portal.scheduler.exception.UnknownJobRestException";

        public static Exception GetNotFound(string exceptionClass, string errorMessage)
        {
            switch (exceptionClass)
            {
                case RE:
                    return new SystemException(errorMessage);
                case IOE:
                    return new IOException(errorMessage);
                case LE:
                    return new LoginException(errorMessage);
                case JAFE:
                    return new JobAlreadyFinishedException(errorMessage);
                case JCRE:
                    return new JobCreationException(errorMessage);
                case SRE:
                    return new SchedulerException(errorMessage);
                case SCRE:
                    return new SubmissionClosedException(errorMessage);
                case UJRE:
                    return new UnknownJobException(errorMessage);
                default:
                    return new SchedulerException(errorMessage);
            }
        }

        public static Exception FromInternalError(string exceptionClass, string errorMessage)
        {
            switch (exceptionClass)
            {
                case RE:
                    return new SystemException(errorMessage);
                case IAE:
                    return new ArgumentException(errorMessage);
                case T:
                    return new Exception(errorMessage);
                default:
                    return new SchedulerException(errorMessage);
            }
        }

        public static Exception FromForbidden(string exceptionClass, string errorMessage)
        {
            switch (exceptionClass)
            {
                case PRE:
                case PE:
                    return new PermissionException(errorMessage);
                default:
                    return new SchedulerException(errorMessage);
            }
        }

        public static Exception FromUnauthorized(string exceptionClass, string errorMessage)
        {
            switch (exceptionClass)
            {
                case NCRE:
                case NCE:
                    return new NotConnectedException(errorMessage);
                default:
                    return new SchedulerException(errorMessage);
            }
        }
    }
}