# PWSClient: ProActive Workflows & Scheduling .NET Client

## Introduction
PWSClient is a .NET client for [ProActive Workflows & Scheduling](https://www.activeeon.com/products/workflows-scheduling/) server based on the HTTP protocol.

PWSClient allows to:
 - Create and submit workflows
 - Wait for workflow termination, receive workflow output and results
 - Transfer data files to/from the ProActive scheduler server
 - Monitor activity of the scheduler server (state of the server, list of jobs, etc)

## Quick Start Notes

Submit a Hello World Job

```csharp
using PWSClient;
using System.IO;
using PWSClient.Exceptions;
using System.Collections.Generic;
using org.ow2.proactive.scheduler.common.task;
using org.ow2.proactive.scheduler.common.job;
using org.ow2.proactive.scripting;

class TestClass
{
    static void Main(string[] args)
    {
        // Connect to the ProActive Scheduler server
        SchedulerClient sc = SchedulerClient.Connect("http://localhost:8080/rest", "admin", "admin")
        // Create a workflow
        TaskFlowJob job = new TaskFlowJob();
        job.Name = "Hello World Job";
        ScriptTask task = new ScriptTask();
        task.Name = "hello_task";
        // Add a groovy script task 
        TaskScript script = new TaskScript(new SimpleScript("println 'Hello World'; result = 'OK'", "groovy", new string[0]));
        task.Script = script;
        job.addTask(task);
        // submit the workflow
        JobIdData jid = sc.SubmitJob(job);
        // wait for results
        IDictionary<string, string> jr = sc.WaitForJobResultValue(jid, 30000);
        string taskResult = jr["hello_task"];
        Console.Out.WriteLine("Result of job " + jid.Id + " = " + taskResult);
        // disconnect from the server
        sc.Disconnect();
    }
}
```

## API Reference
[Client API](api/PWSClient.SchedulerClient.html)

[Workflow Creation API](api/org.ow2.proactive.scheduler.common.job.TaskFlowJob.html)
