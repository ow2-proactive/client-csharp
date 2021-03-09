using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace SharpRestClient
{

    /// <summary>
    /// Status of the scheduler
    /// </summary>
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

    /// <summary>
    /// Priority of a job
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum JobPriorityData
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

    /// <summary>
    /// Status of a job
    /// </summary>
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
        KILLED,
        /// <summary>
        /// The job is currently in-error state
        /// </summary>
        [EnumMember(Value = "In_Error")]
        IN_ERROR

    }

    /// <summary>
    /// Status of a task
    /// </summary>
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
        SKIPPED,
        /// <summary>
        /// The task is currently in-error state, waiting for a user action.
        /// </summary>
        [EnumMember]
        IN_ERROR
    }

    /// <summary>
    /// version of the scheduler and rest api
    /// </summary>
    public sealed class Version
    {
        /// <summary>
        /// version of the scheduler
        /// </summary>
        [JsonProperty("scheduler")]
        public string Scheduler { get; set; }
        /// <summary>
        /// version of the rest api
        /// </summary>
        [JsonProperty("rest")]
        public string Rest { get; set; }
    }

    /// <summary>
    /// A task's id
    /// </summary>
    public sealed class TaskIdData
    {
        /// <summary>
        /// the id itself
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }
        /// <summary>
        /// textual name
        /// </summary>
        [JsonProperty("readableName")]
        public string ReadableName { get; set; }
        /// <summary>
        /// always false
        /// </summary>
        public bool ShouldSerializeReadableName()
        {
            return false;
        }
    }

    /// <summary>
    /// A job's id
    /// </summary>
    public sealed class JobIdData
    {
        /// <summary>
        /// the id itself
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }
        /// <summary>
        /// textual name
        /// </summary>
        [JsonProperty("readableName")]
        public string ReadableName { get; set; }
        /// <summary>
        /// always false
        /// </summary>
        public bool ShouldSerializeReadableName()
        {
            return false;
        }
    }

    /// <summary>
    /// Summary information of this task 
    /// </summary>
    public sealed class TaskInfo
    {
        /// <summary>
        /// id of the Job containing this Task 
        /// </summary>
        [JsonProperty("jobId")]
        public JobIdData JobId { get; set; }
        /// <summary>
        /// id of this Task 
        /// </summary>
        [JsonProperty("taskId")]
        public TaskIdData TaskId { get; set; }
        /// <summary>
        /// system time when this Task was started 
        /// </summary>
        [JsonProperty("startTime")]
        public long StartTime { get; set; }
        /// <summary>
        /// system time when this Task was finished 
        /// </summary>
        [JsonProperty("finishedTime")]
        public long FinishedTime { get; set; }
        /// <summary>
        /// system time at which the Task has been terminated for the last time (last attempt)
        /// </summary>
        [JsonProperty("inErrorTime")]
        public long InErrorTime { get; set; }
        /// <summary>
        /// system time when this Task was scheduled (not started yet) 
        /// </summary>
        [JsonProperty("scheduledTime")]       
        public long ScheduledTime { get; set; }
        /// <summary>
        /// execution duration of the Task. It is the real execution time on the worker. 
        /// </summary>
        [JsonProperty("executionDuration")]
        public string ExecutionDuration { get; set; }
        /// <summary>
        /// status of the Task. 
        /// </summary>
        [JsonProperty("taskStatus")]
        public TaskStatus TaskStatus { get; set; }
        /// <summary>
        /// Get the last execution HostName of the task. 
        /// </summary>
        [JsonProperty("executionHostName")]
        public string ExecutionHostName { get; set; }
        /// <summary>
        /// Get the number of execution left for this task (prior to failure). 
        /// </summary>
        [JsonProperty("numberOfExecutionLeft")]
        public int NumberOfExecutionLeft { get; set; }
        /// <summary>
        /// Get the number of execution on failure left for this task. 
        /// </summary>
        [JsonProperty("numberOfExecutionOnFailureLeft")]
        public int NumberOfExecutionOnFailureLeft { get; set; }
        /// <summary>
        /// Return the task progress (the progress must be explicitely handled by the Task). 
        /// </summary>
        [JsonProperty("progress")]
        public int Progress { get; set; }
    }

    /// <summary>
    /// Parallel Environment info
    /// </summary>
    public sealed class ParallelEnvironmentData
    {
        /// <summary>
        /// number of nodes used to execute the task
        /// </summary>
        [JsonProperty("nodesNumber")]
        public long NodesNumber { get; set; }
    }

    /// <summary>
    /// the Task current state  
    /// </summary>
    public sealed class TaskState
    {
        /// <summary>
        /// the Task readable name  
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// the Task description  
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
        /// <summary>
        /// a Tag associated with this task (if any)  
        /// </summary>
        [JsonProperty("tag")]
        public string Tag { get; set; }
        /// <summary>
        /// the iteration index of this Task (if it's part of a workflow loop)  
        /// </summary>
        [JsonProperty("iterationIndex")]
        public int IterationIndex { get; set; }
        /// <summary>
        /// the replication index of this Task (if it's part of a workflow replication)  
        /// </summary>
        [JsonProperty("replicationIndex")]
        public int ReplicationIndex { get; set; }
        /// <summary>
        /// the maximum number of executions for this Task 
        /// </summary>
        [JsonProperty("maxNumberOfExecution")]        
        public int MaxNumberOfExecution { get; set; }
        /// <summary>
        /// the maximum number of faulty executions for this Task 
        /// </summary>
        [JsonProperty("maxNumberOfExecutionOnFailure")]        
        public int MaxNumberOfExecutionOnFailure { get; set; }
        /// <summary>
        /// the generic information dictionary associated with this Task 
        /// </summary>
        [JsonProperty("genericInformation")]        
        public IDictionary<string, string> GenericInformation { get; set; }
        /// <summary>
        /// the associated task info object 
        /// </summary>
        [JsonProperty("taskInfo")]
        public TaskInfo TaskInfo { get; set; }

        [JsonProperty("parallelEnvironment")]
        public ParallelEnvironmentData ParallelEnvironment { get; set; }
    }

    /// <summary>
    /// Summary information of this job
    /// </summary>
    public sealed class JobInfo
    {
        /// <summary>
        /// id of this Job 
        /// </summary>
        [JsonProperty("id")]
        public JobIdData JobId { get; set; }
        /// <summary>
        /// This job's starting time (one task at least started) 
        /// </summary>
        [JsonProperty("startTime")]
        public long StartTime { get; set; }
        /// <summary>
        /// Returns the time at which a Job was seen as in-error for the last time.
        /// </summary>
        [JsonProperty("inErrorTime")]
        public long InErrorTime { get; set; }
        /// <summary>
        /// This job's finished time
        /// </summary>
        [JsonProperty("finishedTime")]
        public long FinishedTime { get; set; }
        /// <summary>
        /// This job's submission time
        /// </summary>
        [JsonProperty("submittedTime")]
        public long SubmittedTime { get; set; }
        /// <summary>
        /// Time when this job was removed
        /// </summary>
        [JsonProperty("removedTime")]
        public long RemovedTime { get; set; }
        /// <summary>
        /// This job's status
        /// </summary>
        [JsonProperty("status")]
        public JobStatus Status { get; set; } 
        /// <summary>
        /// Total number of tasks for this job (may vary in case of iterations or replications)
        /// </summary>
        [JsonProperty("totalNumberOfTasks")]
        public int TotalNumberOfTasks { get; set; }
        /// <summary>
        /// Total number of pending tasks for this job
        /// </summary>
        [JsonProperty("numberOfPendingTasks")]
        public int NumberOfPendingTasks { get; set; }
        /// <summary>
        /// Total number of running tasks for this job
        /// </summary>
        [JsonProperty("numberOfRunningTasks")]       
        public int NumberOfRunningTasks { get; set; }
        /// <summary>
        /// Total number of finished tasks for this job
        /// </summary>
        [JsonProperty("numberOfFinishedTasks")]       
        public int NumberOfFinishedTasks { get; set; }
        /// <summary>
        /// Total number of failed tasks for this job. A task is failed if the execution node failed multiple times.
        /// </summary>
        [JsonProperty("numberOfFailedTasks")]        
        public int NumberOfFailedTasks { get; set; }
        /// <summary>
        /// Total number of faulty tasks for this job. A task is faulty in case of a user-code error or internal error.
        /// </summary>
        [JsonProperty("numberOfFaultyTasks")]
        public int NumberOfFaultyTasks { get; set; }
        /// <summary>
        /// Total number of tasks in in-error state.
        /// </summary>
        [JsonProperty("numberOfInErrorTasks")]
        public int NumberOfInErrorTasks { get; set; }
        /// <summary>
        /// Generic information attached to this job.
        /// </summary>
        [JsonProperty("genericInformation")]
        public IDictionary<string, string> GenericInformation { get; set; }
        /// <summary>
        /// Variables attached to this job.
        /// </summary>
        [JsonProperty("variables")]
        public IDictionary<string, string> Variables { get; set; }
        /// <summary>
        /// Priority attached to this job.
        /// </summary>
        [JsonProperty("priority")]
        public JobPriorityData Priority { get; set; }
        /// <summary>
        /// Owner of this job.
        /// </summary>
        [JsonProperty("jobOwner")]
        public string Owner { get; set; }

        /// <summary>
        /// True if this job is not finished yet.
        /// </summary>
        public bool IsAlive()
        {            
            return this.Status == JobStatus.PENDING || this.Status == JobStatus.RUNNING || this.Status == JobStatus.STALLED || this.Status == JobStatus.PAUSED || this.Status == JobStatus.IN_ERROR;
        }
    }

    /// <summary>
    /// current state of this job and all this job's tasks
    /// </summary>
    public sealed class JobState
    {
        /// <summary>
        /// The name of this job.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// The priority of this job.
        /// </summary>
        [JsonProperty("priority")]
        public JobPriorityData Priority { get; set; }
        /// <summary>
        /// The owner of this job.
        /// </summary>
        [JsonProperty("owner")]
        public string Owner { get; set; }
        /// <summary>
        /// The job information.
        /// </summary>
        [JsonProperty("jobInfo")]
        public JobInfo JobInfo { get; set; }
        /// <summary>
        /// The project associated with this job.
        /// </summary>
        [JsonProperty("projectName")]
        public string ProjectName { get; set; }
        /// <summary>
        /// All tasks contained in this job (the dictionary is indexed by task ids, rather than task names).
        /// </summary>
        [JsonProperty("tasks")]
        public IDictionary<string, TaskState> Tasks { get; set; }
        /// <summary>
        /// The generic information associated with this job.
        /// </summary>
        [JsonProperty("genericInformation")]
        public IDictionary<string, string> GenericInformation { get; set; }
    }

    /// <summary>
    /// An object containing a task logs
    /// </summary>
    public sealed class TaskLogs
    {
        public string StdOutLogs{ get; set; }
        public string StdErrLogs { get; set; }
        public string AllLogs { get; set; }
    }

    /// <summary>
    /// An object containing a task result
    /// </summary>
    public sealed class TaskResult
    {
        /// <summary>
        /// The task id
        /// </summary>
        [JsonProperty("taskId")]
        public TaskIdData TaskId { get; set; }
        /// <summary>
        /// The task result in composed Java Serialization / Base64 format. Unless the result is a raw byte array, this cannot be used in C# due to java serialization.
        /// </summary>
        [JsonProperty("serializedValue")]
        public string SerializedValue { get; set; }
        /// <summary>
        /// The task result in textual format.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
        /// <summary>
        /// The task exception in serialized format. This cannot be used in C# as the serialization mechanism is java-based.
        /// </summary>
        [JsonProperty("serializedException")]
        public string SerializedException { get; set; }
        /// <summary>
        /// The task exception message in textual format.
        /// </summary>
        [JsonProperty("exceptionMessage")]
        public string ExceptionMessage { get; set; }
        /// <summary>
        /// A dictionary containing the task's metadata, if metadata were set inside the task.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> MetaData { get; set; }
        /// <summary>
        /// A dictionary containing the 's propagated variables in textual format.
        /// </summary>
        [JsonProperty("propagatedVariables")]
        public IDictionary<string, string> PropagatedVariables { get; set; }
        /// <summary>
        /// A dictionary containing the 's propagated variables in serialized format. This cannot be used in C# as the serialization mechanism is java-based.
        /// </summary>
        [JsonProperty("serializedPropagatedVariables")]
        public IDictionary<string, string> SerializedPropagatedVariables { get; set; }
        /// <summary>
        /// This task log output
        /// </summary>
        [JsonProperty("output")]
        public TaskLogs TaskLogs { get; set; }
        /// <summary>
        /// True if this task result is a raw byte array
        /// </summary>
        [JsonProperty("raw")]
        public bool Raw { get; set; }
    }

    /// <summary>
    /// An object containing a job result
    /// </summary>
    public sealed class JobResult
    {
        /// <summary>
        /// The job id
        /// </summary>
        [JsonProperty("id")]
        public JobIdData JobId { get; set; }
        /// <summary>
        /// The job information
        /// </summary>
        [JsonProperty("jobInfo")]
        public JobInfo JobInfo { get; set; }
        /// <summary>
        /// A dictionary containing all task results (dictionary is indexed by task names).
        /// </summary>
        [JsonProperty("allResults")]
        public IDictionary<string, TaskResult> Tasks { get; set; }
        /// <summary>
        /// A dictionary containing all task results with the property "preciousResult" (dictionary is indexed by task names).
        /// </summary>
        [JsonProperty("preciousResults")]
        public IDictionary<string, TaskResult> PreciousTasks { get; set; }
        /// <summary>
        /// A dictionary containing all failed task results (dictionary is indexed by task names).
        /// </summary>
        [JsonProperty("exceptionResults")]
        public IDictionary<string, TaskResult> ExceptionTasks { get; set; }
        /// <summary>
        /// A dictionary containing the job resultMap.
        /// </summary>
        [JsonProperty("resultMap")]
        public IDictionary<string, string> ResultMap { get; set; }

        public override string ToString()
        {
            return string.Format("Job#{0} have {1} tasks results", JobId.Id, Tasks.Count);
        }
    }
}
