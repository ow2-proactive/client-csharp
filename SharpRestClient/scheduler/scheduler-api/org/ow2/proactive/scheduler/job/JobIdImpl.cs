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
namespace org.ow2.proactive.scheduler.job
{

	using SchedulerConstants = org.ow2.proactive.scheduler.common.SchedulerConstants;
	using JobId = org.ow2.proactive.scheduler.common.job.JobId;


	/// <summary>
	/// Definition of a job identification, this will be used during scheduling to identify your job.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 0.9
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlRootElement(name = "jobid") @XmlAccessorType(XmlAccessType.FIELD) public final class JobIdImpl implements org.ow2.proactive.scheduler.common.job.JobId
	[Serializable]
	public sealed class JobIdImpl : JobId
	{

		/// <summary>
		/// Default job name </summary>
		public const string DEFAULT_JOB_NAME = SchedulerConstants.JOB_DEFAULT_NAME;

		/// <summary>
		/// current instance id </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlElement(name = "id") private long id;
		private long id;

		/// <summary>
		/// Human readable name </summary>
		private string readableName = DEFAULT_JOB_NAME;

		/// <summary>
		/// Default Job id constructor
		/// </summary>
		/// <param name="id"> the id to put in the jobId </param>
		private JobIdImpl(long id)
		{
			this.id = id;
		}

		/// <summary>
		/// Default Job id constructor
		/// </summary>
		/// <param name="id"> the id to put in the jobId </param>
		/// <param name="readableName"> the human readable name associated with this jobid </param>
		public JobIdImpl(long id, string readableName) : this(id)
		{
			this.readableName = readableName;
		}

		/// <summary>
		/// {@inheritDoc}
		/// </summary>
		public string ReadableName
		{
			get
			{
				return this.readableName;
			}
		}

		/// <summary>
		/// {@inheritDoc}
		/// </summary>
		public string value()
		{
			return this.id.ToString();
		}

		/// <summary>
		/// {@inheritDoc}
		/// </summary>
		public long longValue()
		{
			return this.id;
		}

		/// <summary>
		/// Make a new JobId with the given arguments.
		/// </summary>
		/// <param name="str"> the string on which to base the id. </param>
		/// <returns> the new jobId </returns>
		public static JobId makeJobId(string str)
		{
			return new JobIdImpl(long.Parse(str.Trim()));
		}

		/// <summary>
		/// {@inheritDoc}
		/// </summary>
		public int CompareTo(JobId jobId)
		{
			return id.CompareTo(((JobIdImpl) jobId).id);
		}

		/// <summary>
		/// {@inheritDoc}
		/// </summary>
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

			JobIdImpl jobId = (JobIdImpl) o;

			if (id != jobId.id)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// <font color="red"><b>Do not use this method to get the value as an INTEGER.
		/// It does not ensure that this integer value is the real Job ID Value.</b></font>
		/// <para>
		/// Use the <seealso cref="org.ow2.proactive.scheduler.common.job.JobId.value()"/> method instead.
		/// 
		/// </para>
		/// </summary>
		/// <seealso cref= java.lang.Object#hashCode() </seealso>
		public override int GetHashCode()
		{
			return (int) this.id;
		}

		/// <summary>
		/// {@inheritDoc}
		/// </summary>
		public override string ToString()
		{
			return this.value();
		}

	}

}