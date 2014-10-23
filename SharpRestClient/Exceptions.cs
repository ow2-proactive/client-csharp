using System;
namespace SharpRestClient.Exceptions
{
    public class SchedulerException : Exception
    {
        public SchedulerException() : base() { }
        public SchedulerException(string message) : base(message) { }
        public SchedulerException(string message, Exception innerException) : base(message, innerException) { }
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

    public class JobAlreadyFinishedException : SchedulerException
    {
        public JobAlreadyFinishedException() : base() { }
        public JobAlreadyFinishedException(string message) : base(message) { }
        public JobAlreadyFinishedException(string message, Exception innerException) : base(message, innerException) { }
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
}