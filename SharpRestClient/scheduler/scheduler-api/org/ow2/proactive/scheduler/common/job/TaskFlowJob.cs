using System;
using System.Collections.Generic;
using System.Text;

/*
 * ProActive Parallel Suite(TM):
 * The Open Source library for parallel and distributed
 * Workflows & Scheduling, Orchestration, Cloud Automation
 * and Big Data Analysis on Enterprise Grids & Clouds.
 *
 * Copyright (c) 2007 - 2017 ActiveEon
 * Contact: contact@activeeon.com
 *
 * This library is free software: you can redistribute it and/or
 * modify it under the terms of the GNU Affero General Public License
 * as published by the Free Software Foundation: version 3 of
 * the License.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 *
 * If needed, contact us to obtain a release under GPL Version 2 or 3
 * or a different license than the AGPL.
 */
namespace org.ow2.proactive.scheduler.common.job
{

	using SchedulerConstants = org.ow2.proactive.scheduler.common.SchedulerConstants;
	using UserException = org.ow2.proactive.scheduler.common.exception.UserException;
	using Task = org.ow2.proactive.scheduler.common.task.Task;
	using LogFormatter = org.ow2.proactive.scheduler.common.util.LogFormatter;


	/// <summary>
	/// Use this class to create your job if you want to define a task flow job.<br>
	/// A task flow job or data flow job, is a job that can contain
	/// one or more task(s) with the dependencies you want.<br>
	/// To make this type of job, just use the default no arg constructor,
	/// and set the properties you want to set.<br>
	/// Then add tasks with the given method <seealso cref="addTask(Task)"/> in order to fill the job with your own tasks.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 0.9
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public class TaskFlowJob extends Job
	[Serializable]
	public class TaskFlowJob : Job
	{

		/// <summary>
		/// Task count for unset task name </summary>
		private int taskCountForUnSetTaskName = 1;

		/// <summary>
		/// List of task for the task flow job </summary>
		private IDictionary<string, Task> tasks = new Dictionary<string, Task>();

		/// <summary>
		/// ProActive Empty Constructor </summary>
		public TaskFlowJob()
		{
		}

		/// <seealso cref= org.ow2.proactive.scheduler.common.job.Job#getType() </seealso>
		public override JobType Type
		{
			get
			{
				return JobType.TASKSFLOW;
			}
		}

		/// <summary>
		/// Add a task to this task flow job.<br>
		/// The task name must not be null as it is not by default.<br>
		/// The task name must also be different for each task as it is used to identify each task result.<br>
		/// <br>
		/// If not set, the task name will be a generated one : 'task_X' (where X is the Xth added task number)
		/// </summary>
		/// <param name="task"> the task to add. </param>
		/// <exception cref="UserException"> if a problem occurred while the task is being added. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public void addTask(org.ow2.proactive.scheduler.common.task.Task task) throws org.ow2.proactive.scheduler.common.exception.UserException
		public virtual void addTask(Task task)
		{
			if (string.ReferenceEquals(task.Name, null))
			{
				throw new UserException("The name of the task must not be null !");
			}
			if (task.Name.Equals(SchedulerConstants.TASK_DEFAULT_NAME))
			{
				task.Name = SchedulerConstants.TASK_NAME_IFNOTSET + taskCountForUnSetTaskName;
				taskCountForUnSetTaskName++;
			}
			if (tasks.ContainsKey(task.Name))
			{
				throw new UserException("The name of the task is already used : " + task.Name);
			}
			tasks[task.Name] = task;
		}

		/// <summary>
		/// Add a list of tasks to this task flow job.
		/// The task names must not be null as it is not by default.<br>
		/// The task names must also be different for each task as it is used to identify each task result.<br>
		/// <br>
		/// If not set, the task names will be generated : 'task_X' (where X is the Xth added task number)
		/// </summary>
		/// <param name="tasks"> the list of tasks to add. </param>
		/// <exception cref="UserException"> if a problem occurred while the task is being added. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public void addTasks(java.util.List<org.ow2.proactive.scheduler.common.task.Task> tasks) throws org.ow2.proactive.scheduler.common.exception.UserException
		public virtual void addTasks(IList<Task> tasks)
		{
			foreach (Task task in tasks)
			{
				addTask(task);
			}
		}

		/// <summary>
		/// To get the list of tasks.
		/// </summary>
		/// <returns> the list of tasks. </returns>
		public virtual List<Task> Tasks
		{
			get
			{
				return new List<Task>(tasks.Values);
			}
		}

		/// <summary>
		/// Get the task corresponding to the given name.
		/// </summary>
		/// <param name="name"> the name of the task to look for. </param>
		/// <returns> the task corresponding to the given name. </returns>
		public virtual Task getTask(string name)
		{
			return tasks[name];
		}

		/// <seealso cref= org.ow2.proactive.scheduler.common.job.Job#getId() </seealso>
		public override JobId Id
		{
			get
			{
				// Not yet assigned
				return null;
			}
		}

		public override string display()
		{
			string nl = Environment.NewLine;
			return base.display() + nl + LogFormatter.addIndent(displayAllTasks());
		}

		private string displayAllTasks()
		{
			string nl = Environment.NewLine;
			StringBuilder answer = new StringBuilder("Tasks = {");
			answer.Append(nl);
			foreach (string tid in tasks.Keys)
			{
				answer.Append(LogFormatter.addIndent(tasks[tid].display())).Append(nl).Append(nl);
			}
			answer.Append("}");
			return answer.ToString();
		}
	}

}