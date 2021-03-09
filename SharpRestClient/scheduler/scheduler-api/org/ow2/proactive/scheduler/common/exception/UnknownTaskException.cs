using System;

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
namespace org.ow2.proactive.scheduler.common.exception
{
	using JobId = org.ow2.proactive.scheduler.common.job.JobId;
	using TaskId = org.ow2.proactive.scheduler.common.task.TaskId;


	/// <summary>
	/// Exception generated when trying to get task result and the task result does not exist.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 1.0
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public class UnknownTaskException extends SchedulerException
	public class UnknownTaskException : SchedulerException
	{

		private JobId jobId;

		private string taskName;

		private TaskId taskId;

		/// <summary>
		/// Create a new instance of UnknownTaskException
		/// </summary>
		/// <param name="msg"> the message to attach. </param>
		public UnknownTaskException(string msg) : base(msg)
		{
		}

		/// <summary>
		/// Create a new instance of UnknownTaskException
		/// </summary>
		public UnknownTaskException()
		{
		}

		public UnknownTaskException(string taskName, JobId jobId) : base("The task " + taskName + "of job " + jobId + " is unknown !")
		{
			this.jobId = jobId;
			this.taskName = taskName;
		}

		public UnknownTaskException(TaskId taskId, JobId jobId) : base("The task " + taskId + "of job " + jobId + " is unknown !")
		{
			this.jobId = jobId;
			this.taskId = taskId;
		}

		/// <summary>
		/// Create a new instance of UnknownTaskException
		/// </summary>
		/// <param name="msg"> the message to attach. </param>
		/// <param name="cause"> the cause of the exception. </param>
		public UnknownTaskException(string msg, Exception cause) : base(msg, cause)
		{
		}

		/// <summary>
		/// Create a new instance of UnknownTaskException
		/// </summary>
		/// <param name="cause"> the cause of the exception. </param>
		public UnknownTaskException(Exception cause) : base(cause)
		{
		}

	}

}