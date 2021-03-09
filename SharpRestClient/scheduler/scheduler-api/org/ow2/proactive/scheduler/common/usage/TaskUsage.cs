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
namespace org.ow2.proactive.scheduler.common.usage
{


	/// <summary>
	/// Task information for accounting / usage purpose.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 3.4
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public class TaskUsage implements java.io.Serializable
	[Serializable]
	public class TaskUsage
	{

		private readonly string taskId;

		private readonly string taskName;

		private readonly long taskStartTime;

		private readonly long taskFinishedTime;

		private readonly long taskExecutionDuration;

		private readonly int taskNodeNumber;

		private readonly string taskStatus;

		private readonly string taskTag;

		private readonly string taskDescription;

		private readonly string executionHostName;

		private readonly int numberOfExecutionLeft;

		private readonly int numberOfExecutionOnFailureLeft;

		private readonly int maxNumberOfExecution;

		private readonly int maxNumberOfExecutionOnFailure;

		public TaskUsage(string taskId, string taskStatus, string taskName, string taskTag, long taskStartTime, long taskFinishedTime, long taskExecutionDuration, int taskNodeNumber, string taskDescription, string executionHostName, int numberOfExecutionLeft, int numberOfExecutionOnFailureLeft, int maxNumberOfExecution, int maxNumberOfExecutionOnFailure)
		{
			this.taskId = taskId;
			this.taskStatus = taskStatus;
			this.taskName = taskName;
			this.taskTag = taskTag;
			this.taskStartTime = taskStartTime;
			this.taskFinishedTime = taskFinishedTime;
			this.taskExecutionDuration = taskExecutionDuration;
			this.taskNodeNumber = taskNodeNumber;
			this.taskDescription = taskDescription;
			this.executionHostName = executionHostName;
			this.numberOfExecutionLeft = numberOfExecutionLeft;
			this.numberOfExecutionOnFailureLeft = numberOfExecutionOnFailureLeft;
			this.maxNumberOfExecution = maxNumberOfExecution;
			this.maxNumberOfExecutionOnFailure = maxNumberOfExecutionOnFailure;
		}

		public virtual string TaskId
		{
			get
			{
				return taskId;
			}
		}

		public virtual string TaskStatus
		{
			get
			{
				return taskStatus;
			}
		}

		public virtual string TaskName
		{
			get
			{
				return taskName;
			}
		}

		public virtual string TaskTag
		{
			get
			{
				return taskTag;
			}
		}

		public virtual long TaskStartTime
		{
			get
			{
				return taskStartTime;
			}
		}

		public virtual long TaskFinishedTime
		{
			get
			{
				return taskFinishedTime;
			}
		}

		public virtual long TaskExecutionDuration
		{
			get
			{
				return taskExecutionDuration;
			}
		}

		public virtual int TaskNodeNumber
		{
			get
			{
				return taskNodeNumber;
			}
		}

		public virtual string TaskDescription
		{
			get
			{
				return taskDescription;
			}
		}

		public virtual string ExecutionHostName
		{
			get
			{
				return executionHostName;
			}
		}

		public virtual int NumberOfExecutionLeft
		{
			get
			{
				return numberOfExecutionLeft;
			}
		}

		public virtual int NumberOfExecutionOnFailureLeft
		{
			get
			{
				return numberOfExecutionOnFailureLeft;
			}
		}

		public virtual int MaxNumberOfExecution
		{
			get
			{
				return maxNumberOfExecution;
			}
		}

		public virtual int MaxNumberOfExecutionOnFailure
		{
			get
			{
				return maxNumberOfExecutionOnFailure;
			}
		}
	}

}