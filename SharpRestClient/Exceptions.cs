using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.IO;
namespace SharpRestClient.Exceptions
{
    /// <summary>
    /// Internal Exception of the ProActive Scheduler
    /// </summary>
    public class SchedulerException : Exception
    {
        public SchedulerException() : base() { }
        public SchedulerException(string message) : base(message) { }
        public SchedulerException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Exception occurring during the login phase
    /// </summary>
    public class LoginException : SchedulerException
    {
        public LoginException() : base() { }
        public LoginException(string message) : base(message) { }
        public LoginException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// The session is not currently connected to the scheduler
    /// </summary>
    public class NotConnectedException : SchedulerException
    {
        public NotConnectedException() : base() { }
        public NotConnectedException(string message) : base(message) { }
        public NotConnectedException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// The session is not allowed to perform the requested operation
    /// </summary>
    public class PermissionException : SchedulerException
    {
        public PermissionException() : base() { }
        public PermissionException(string message) : base(message) { }
        public PermissionException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// SubmissionClosedException
    /// </summary>
    public class SubmissionClosedException : SchedulerException
    {
        public SubmissionClosedException() : base() { }
        public SubmissionClosedException(string message) : base(message) { }
        public SubmissionClosedException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// The job is already finished and the requested operation cannot be performed
    /// </summary>
    public class JobAlreadyFinishedException : SchedulerException
    {
        public JobAlreadyFinishedException() : base() { }
        public JobAlreadyFinishedException(string message) : base(message) { }
        public JobAlreadyFinishedException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// The job could not be created, for example due to a parsing error
    /// </summary>
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

    /// <summary>
    /// The requested task is unknown
    /// </summary>
    public class UnknownTaskException : SchedulerException
    {
        public UnknownTaskException() : base() { }
        public UnknownTaskException(string message) : base(message) { }
        public UnknownTaskException(string message, Exception innerException) : base(message, innerException) { }
        public UnknownTaskException(string taskName, JobIdData jobId) : base("The task " + taskName + " of job " + jobId + " is unknown!") { }
        public UnknownTaskException(TaskIdData taskId, JobIdData jobId) : base("The task " + taskId + " of job " + jobId + " is unknown!") { }
    }

    /// <summary>
    /// The requested job is unknown
    /// </summary>
    public class UnknownJobException : SchedulerException
    {
        public UnknownJobException() : base() { }
        public UnknownJobException(string message) : base(message) { }
        public UnknownJobException(string message, Exception innerException) : base(message, innerException) { }
        public UnknownJobException(JobIdData jobId) : base("The job " + jobId + " does not exists!") { }
    }

    /// <summary>
    /// helper used to transform exceptions
    /// </summary>
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
        public const string UTRE = "org.ow2.proactive_grid_cloud_portal.scheduler.exception.UnknownTaskRestException";

        /// <summary>
        /// find an exception
        /// </summary>
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
                case UTRE:
                    return new UnknownTaskException(errorMessage);
                default:
                    return new SchedulerException(errorMessage);
            }
        }

        /// <summary>
        /// find an exception from an internal error
        /// </summary>
        public static Exception FromInternalError(string exceptionClass, string errorMessage, string stackTrace)
        {
            switch (exceptionClass)
            {
                case RE:
                    return new SystemException(errorMessage, new SystemException(stackTrace));
                case IAE:
                    return new ArgumentException(errorMessage, new SystemException(stackTrace));
                case T:
                    return new Exception(errorMessage, new SystemException(stackTrace));
                default:
                    return new SchedulerException(errorMessage, new SystemException(stackTrace));
            }
        }

        /// <summary>
        /// find an exception from a permission error
        /// </summary>
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

        /// <summary>
        /// find an exception from a permission error
        /// </summary>
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