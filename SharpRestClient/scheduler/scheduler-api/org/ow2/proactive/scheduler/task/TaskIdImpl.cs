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
namespace org.ow2.proactive.scheduler.task
{

	using SchedulerConstants = org.ow2.proactive.scheduler.common.SchedulerConstants;
	using JobId = org.ow2.proactive.scheduler.common.job.JobId;
	using TaskId = org.ow2.proactive.scheduler.common.task.TaskId;
	using JobIdImpl = org.ow2.proactive.scheduler.job.JobIdImpl;


	/// <summary>
	/// Definition of a task identification.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 0.9
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAccessorType(XmlAccessType.FIELD) public final class TaskIdImpl implements org.ow2.proactive.scheduler.common.task.TaskId
	[Serializable]
	public sealed class TaskIdImpl : TaskId
	{

		// Task identifier
		private long id;

		/// <summary>
		/// Human readable name </summary>
		private string readableName = SchedulerConstants.TASK_DEFAULT_NAME;

		/// <summary>
		/// tag of the task </summary>
		private string tag;

		private JobId jobId;

		public const String REPLICATION_SEPARATOR = "*";

		public const String ITERATION_SEPARATOR = "#";

		private TaskIdImpl(JobId jobId, long taskId)
		{
			this.jobId = jobId;
			this.id = taskId;
		}

		private TaskIdImpl(JobId jobId, string name, long taskId) : this(jobId, taskId)
		{
			this.readableName = name;
		}

		/// <summary>
		/// Create task id, and set task name.
		/// </summary>
		/// <param name="jobId">        the id of the enclosing job. </param>
		/// <param name="readableName"> the human readable name of the task. </param>
		/// <param name="taskId">       the task identifier value. </param>
		/// <returns> new TaskId instance. </returns>
		public static TaskId createTaskId(JobId jobId, string readableName, long taskId)
		{
			return new TaskIdImpl(jobId, readableName, taskId);
		}

		/// <summary>
		/// Create task id, and set task name + tag.
		/// </summary>
		/// <param name="jobId">        the id of the enclosing job. </param>
		/// <param name="readableName"> the human readable name of the task. </param>
		/// <param name="taskId">       the task identifier value. </param>
		/// <param name="tag">          the tag of the task. </param>
		/// <returns> new TaskId instance. </returns>
		public static TaskId createTaskId(JobId jobId, string readableName, long taskId, string tag)
		{
			TaskIdImpl t = new TaskIdImpl(jobId, readableName, taskId);
			t.Tag = tag;
			return t;
		}

		/// <summary>
		/// {@inheritDoc}
		/// </summary>
		public JobId JobId
		{
			get
			{
				return jobId;
			}
			set
			{
				this.jobId = value;
			}
		}


		public string Tag
		{
			get
			{
				return this.tag;
			}
			set
			{
				this.tag = value;
			}
		}


		/// <summary>
		/// Return the human readable name associated to this id.
		/// </summary>
		/// <returns> the human readable name associated to this id. </returns>
		public string ReadableName
		{
			get
			{
				return this.readableName;
			}
			set
			{
				this.readableName = value;
			}
		}


		/// <summary>
		/// {@inheritDoc}
		/// </summary>
		public int CompareTo(TaskId that)
		{
			return this.id.CompareTo(((TaskIdImpl) that).id);
		}

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (o == null || this.GetType() != o.GetType())
			{
				return false;
			}

			TaskIdImpl taskId = (TaskIdImpl) o;

			return id == taskId.id;
		}

		/// <seealso cref= java.lang.Object#hashCode() </seealso>
		public override int GetHashCode()
		{
			return (int)(id % int.MaxValue);
		}

		/// <summary>
		/// {@inheritDoc}
		/// </summary>
		public string value()
		{
			return Convert.ToString(id);
		}

		/// <summary>
		/// {@inheritDoc}
		/// </summary>
		public long longValue()
		{
			return id;
		}

		/// <seealso cref= TaskId#getIterationIndex() </seealso>
		public int IterationIndex
		{
			get
			{
				// implementation note :
				// this has to match what is done in InternalTask#setName(String)
				int iterationPos;
				if ((iterationPos = this.readableName.IndexOf(ITERATION_SEPARATOR)) != -1)
				{
					int replicationPos = this.readableName.IndexOf(REPLICATION_SEPARATOR);
					if (replicationPos == -1)
					{
						replicationPos = readableName.Length;
					}
					int read = int.Parse(this.readableName.Substring(iterationPos + 1, replicationPos - (iterationPos + 1)));
					return Math.Max(0, read);
				}
				return 0;
			}
		}

		/// <seealso cref= TaskId#getReplicationIndex() </seealso>
		public int ReplicationIndex
		{
			get
			{
				// implementation note :
				// this has to match what is done in InternalTask#setName(String)
				int pos;
				if ((pos = this.readableName.IndexOf(REPLICATION_SEPARATOR)) != -1)
				{
					int read = int.Parse(this.readableName.Substring(pos + 1));
					return Math.Max(0, read);
				}
				return 0;
			}
		}

		/// <summary>
		/// {@inheritDoc}
		/// </summary>
		public override string ToString()
		{
			return jobId.value() + 't' + value();
		}

		/// <summary>
		/// Make a new TaskId with the given arguments.
		/// </summary>
		/// <param name="str"> the string on which to base the id. </param>
		/// <returns> the new taskId </returns>
		public static TaskId makeTaskId(string str)
		{
			string[] strSplitted = str.Split("t", true);

			if (!str.Contains("t") || strSplitted.Length != 2)
			{
				throw new System.ArgumentException("A valid task id must be supplied");
			}
			JobId jobId = JobIdImpl.makeJobId(strSplitted[0]);
			long taskId = long.Parse(strSplitted[1]);
			return new TaskIdImpl(jobId, taskId);
		}

	}

}