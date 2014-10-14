using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SharpRestClient
{

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SchedulerStatus
    {
        /// <summary>The scheduler is running. Jobs can be submitted. Get the jobs results is possible.
        /// It can be paused, stopped or shutdown.</summary>
        [EnumMember(Value = "Started")]
        STARTED,

        /// <summary> The scheduler is stopped. Jobs cannot be submitted anymore. It will terminate every submitted jobs.
        /// Get the jobs results is possible. It can be started or shutdown.</summary>
        [EnumMember(Value = "Stopped")]
        STOPPED,

        /// <summary>The scheduler is in freeze mode. It means that every running tasks will be terminated,
        ///  but the running jobs will wait for the scheduler to resume. It can be resumed, stopped, paused or shutdown.</summary>                         
        [EnumMember(Value = "Frozen")]
        FROZEN,

        /// <summary>The scheduler is paused. It means that every running jobs will be terminated.
        /// It can be resumed, stopped, frozen or shutdown.</summary>        
        [EnumMember(Value = "Paused")]
        PAUSED,

        /// <summary>The scheduler is shutting down, it will terminate all running jobs (during this time, get jobs results is possible),
        /// then it will serialize every remaining jobs results that still are in the finished queue.
        ///  Finally, it will shutdown the scheduler.</summary>
        [EnumMember(Value = "Shutting down")]
        SHUTTING_DOWN,

        /// <summary>The scheduler is unlinked with RM,
        /// This can be due to the crash of the resource manager.
        /// This status will block every called to the scheduler except the terminate one
        /// and the call to reconnect to a new Resource Manager.</summary>
        [EnumMember(Value = "Unlinked from RM")]
        UNLINKED,

        /// <summary>The scheduler has been killed, nothing can be done anymore. (Similar to Ctrl-C)</summary>
        [EnumMember(Value = "Killed")]
        KILLED,

        /// <summary>The scheduler has been killed due to a db disconnection, nothing can be done anymore.</summary>
        [EnumMember(Value = "Killed (DB down)")]
        DB_DOWN
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum JobPriority
    {
        /// <summary>Lowest priority</summary>
        [EnumMember(Value = "Idle")]
        IDLE,
        /// <summary>Low priority</summary>
        [EnumMember(Value = "Lowest")]
        LOWEST,
        /// <summary>Low priority</summary>
        [EnumMember(Value = "Low")]
        LOW,
        /// <summary>Normal Priority</summary>
        [EnumMember(Value = "Normal")]
        NORMAL,
        /// <summary>High priority</summary>
        [EnumMember(Value = "High")]
        HIGH,
        /// <summary>Highest priority</summary>
        [EnumMember(Value = "Highest")]
        HIGHEST
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum JobStatus
    {
        /// <summary>The job is waiting to be scheduled.</summary>
        [EnumMember(Value = "Pending")]
        PENDING,
        /// <summary>
        /// The job is running. Actually at least one of its task has been scheduled.
        /// </summary>
        [EnumMember(Value = "Running")]
        RUNNING,
        /// <summary>
        /// The job has been launched but no task are currently running.
        /// </summary>
        [EnumMember(Value = "Stalled")]
        STALLED,
        /// <summary>
        /// The job is finished. Every tasks are finished.
        /// </summary>
        [EnumMember(Value = "Finished")]
        FINISHED,
        /// <summary>
        /// The job is paused waiting for user to resume it.
        /// </summary>
        [EnumMember(Value = "Paused")]
        PAUSED,
        /// <summary>
        /// The job has been canceled due to user exception and order.
        /// This status runs when a user exception occurs in a task
        /// and when the user has asked to cancel On exception.
        /// </summary>
        [EnumMember(Value = "Canceled")]
        CANCELED,
        /// <summary>
        /// The job has failed. One or more tasks have failed (due to resources failure).
        /// There is no more executionOnFailure left for a task.
        /// </summary>
        [EnumMember(Value = "Failed")]
        FAILED,
        /// <summary>
        /// The job has been killed by a user..
        /// Nothing can be done anymore on this job expect read execution informations
        /// such as output, time, etc...
        /// </summary>
        [EnumMember(Value = "Killed")]
        KILLED
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaskStatus
    {
        /// <summary>The task has just been submitted by the user.</summary>
        [EnumMember]
        SUBMITTED,
        /// <summary>
        /// The task is in the scheduler pending queue.
        /// </summary>
        [EnumMember]
        PENDING,
        /// <summary>
        /// The task is paused.
        /// </summary>
        [EnumMember]
        PAUSED,
        /// <summary>
        /// The task is executing.
        /// </summary>
        [EnumMember]
        RUNNING,
        /// <summary>
        /// The task is waiting for restart after an error. (ie:native code != 0 or exception)
        /// </summary>
        [EnumMember]
        WAITING_ON_ERROR,
        /// <summary>
        /// The task is waiting for restart after a failure. (ie:node down)
        /// </summary>
        [EnumMember]
        WAITING_ON_FAILURE,
        /// <summary>
        /// The task is failed (only if max execution time has been reached and the node on which it was started is down).
        /// </summary>
        [EnumMember]
        FAILED,
        /// <summary>
        /// The task could not be started. It means that the task could not be started due to dependences failure.
        /// </summary>
        [EnumMember]
        NOT_STARTED,
        /// <summary>
        /// The task could not be restarted. It means that the task could not be restarted after an error during the previous execution.
        /// </summary>
        [EnumMember]
        NOT_RESTARTED,
        /// <summary>
        /// The task has been aborted by an exception on an other task while the task is running. (job is cancelOnError=true)
        /// Can be also in this status if the job is killed while the concerned task was running.
        /// </summary>
        [EnumMember]
        ABORTED,
        /// <summary>
        /// The task has finished execution with error code (!=0) or exception.
        /// </summary>
        [EnumMember]
        FAULTY,
        /// <summary>
        /// The task has finished execution.
        /// </summary>
        [EnumMember]
        FINISHED,
        /// <summary>
        /// The task was not executed: it was the non-selected branch of an IF/ELSE control flow action.
        /// </summary>
        [EnumMember]
        SKIPPED
    }

    public sealed class Version
    {
        [JsonProperty("scheduler")]
        public string Scheduler { get; set; }
        [JsonProperty("rest")]
        public string Rest { get; set; }
    }

    public sealed class TaskId
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("readableName")]
        public string ReadableName { get; set; }
        public bool ShouldSerializeReadableName()
        {
            return false;
        }
    }

    public sealed class JobId
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("readableName")]
        public string ReadableName { get; set; }
        public bool ShouldSerializeReadableName()
        {
            return false;
        }
    }

    public sealed class TaskInfo
    {
        [JsonProperty("jobId")]
        public JobId JobId { get; set; }
        [JsonProperty("taskId")]
        public TaskId TaskId { get; set; }
        [JsonProperty("startTime")]
        public long StartTime { get; set; }
        [JsonProperty("finishedTime")]
        public long FinishedTime { get; set; }
        [JsonProperty("executionDuration")]
        public string ExecutionDuration { get; set; }
        [JsonProperty("taskStatus")]
        public TaskStatus TaskStatus { get; set; }
        [JsonProperty("executionHostName")]
        public string ExecutionHostName { get; set; }
        [JsonProperty("numberOfExecutionLeft")]
        public int NumberOfExecutionLeft { get; set; }
        [JsonProperty("numberOfExecutionOnFailureLeft")]
        public int NumberOfExecutionOnFailureLeft { get; set; }        
    }

    public sealed class TaskState
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("iterationIndex")]
        public int IterationIndex { get; set; }
        [JsonProperty("replicationIndex")]
        public int ReplicationIndex { get; set; }
        [JsonProperty("maxNumberOfExecution")]
        public int MaxNumberOfExecution { get; set; }
        [JsonProperty("maxNumberOfExecutionOnFailure")]
        public int MaxNumberOfExecutionOnFailure { get; set; }
        [JsonProperty("taskInfo")]
        public TaskInfo TaskInfo { get; set; }
        //[JsonProperty("parallelEnvironment")]
        //public string ParallelEnvironment { get; set; }
    }

    public sealed class JobInfo
    {
        [JsonProperty("startTime")]
        public long StartTime { get; set; }
        [JsonProperty("finishedTime")]
        public long FinishedTime { get; set; }
        [JsonProperty("submittedTime")]
        public long SubmittedTime { get; set; }
        [JsonProperty("removedTime")]
        public long RemovedTime { get; set; }
        [JsonProperty("status")]
        public JobStatus Status { get; set; }
        [JsonProperty("id")]
        public string JobId { get; set; }
        [JsonProperty("totalNumberOfTasks")]
        public int TotalNumberOfTasks { get; set; }
        [JsonProperty("numberOfPendingTasks")]
        public int NumberOfPendingTasks { get; set; }
        [JsonProperty("numberOfRunningTasks")]
        public int NumberOfRunningTasks { get; set; }
        [JsonProperty("numberOfFinishedTasks")]
        public int NumberOfFinishedTasks { get; set; }
        [JsonProperty("priority")]
        public JobPriority Priority { get; set; }
        [JsonProperty("jobOwner")]
        public string Owner { get; set; }

        public bool IsAlive()
        {            
            return this.Status == JobStatus.PENDING || this.Status == JobStatus.RUNNING || this.Status == JobStatus.STALLED || this.Status == JobStatus.PAUSED;
        }
    }

    public sealed class JobState
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("priority")]
        public JobPriority Priority { get; set; }
        [JsonProperty("owner")]
        public string Owner { get; set; }
        [JsonProperty("jobInfo")]
        public JobInfo JobInfo { get; set; }
        [JsonProperty("projectName")]
        public string ProjectName { get; set; }
        [JsonProperty("tasks")]
        public IDictionary<string, TaskState> Tasks { get; set; }
    }
}
