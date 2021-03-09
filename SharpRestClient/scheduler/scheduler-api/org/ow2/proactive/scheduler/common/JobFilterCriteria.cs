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
namespace org.ow2.proactive.scheduler.common
{


//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public class JobFilterCriteria implements java.io.Serializable
	[Serializable]
	public class JobFilterCriteria
	{

		private readonly bool myJobsOnly;

		private readonly bool pending;

		private readonly bool running;

		private readonly bool finished;

		public JobFilterCriteria(bool myJobsOnly, bool pending, bool running, bool finished)
		{
			this.myJobsOnly = myJobsOnly;
			this.pending = pending;
			this.running = running;
			this.finished = finished;
		}

		public virtual bool MyJobsOnly
		{
			get
			{
				return myJobsOnly;
			}
		}

		public virtual bool Pending
		{
			get
			{
				return pending;
			}
		}

		public virtual bool Running
		{
			get
			{
				return running;
			}
		}

		public virtual bool Finished
		{
			get
			{
				return finished;
			}
		}

	}

}