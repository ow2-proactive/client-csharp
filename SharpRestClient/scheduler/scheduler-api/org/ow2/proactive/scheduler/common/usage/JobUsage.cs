using System;
using System.Collections.Generic;

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
	/// Job information for accounting / usage purpose.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 3.4
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI @XmlRootElement public class JobUsage implements java.io.Serializable
	[Serializable]
	public class JobUsage
	{
		private readonly string owner;

		private readonly string project;

		private readonly string jobId;

		private readonly string jobName;

		private readonly long jobDuration;

		private readonly IList<TaskUsage> taskUsages = new List<TaskUsage>();

		private readonly string status;

		private readonly long submittedTime;

		private readonly long? parentId;

		public JobUsage(string owner, string project, string jobId, string jobName, long jobDuration, string status, long submittedTime, long? parentId)
		{
			this.owner = owner;
			this.project = project;
			this.jobId = jobId;
			this.jobName = jobName;
			this.jobDuration = jobDuration;
			this.status = status;
			this.submittedTime = submittedTime;
			this.parentId = parentId;
		}

		public virtual void add(TaskUsage taskUsage)
		{
			taskUsages.Add(taskUsage);
		}

		public virtual string JobId
		{
			get
			{
				return jobId;
			}
		}

		public virtual string JobName
		{
			get
			{
				return jobName;
			}
		}

		public virtual string Owner
		{
			get
			{
				return owner;
			}
		}

		public virtual string Project
		{
			get
			{
				return project;
			}
		}

		public virtual long JobDuration
		{
			get
			{
				return jobDuration;
			}
		}

		public virtual IList<TaskUsage> TaskUsages
		{
			get
			{
				return taskUsages;
			}
		}

		public virtual string Status
		{
			get
			{
				return status;
			}
		}

		public virtual long SubmittedTime
		{
			get
			{
				return submittedTime;
			}
		}

		public virtual long? ParentId
		{
			get
			{
				return parentId;
			}
		}
	}

}