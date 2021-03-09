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
namespace org.ow2.proactive.scheduler.common.task
{


	using TopologyDescriptor = org.ow2.proactive.topology.descriptor.TopologyDescriptor;


	/// <summary>
	/// The parallel environment of the multi-nodes tasks that defines
	/// the number of nodes needed by task to be executed and topology
	/// descriptor optimizing its execution.
	/// 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI @XmlAccessorType(XmlAccessType.FIELD) public class ParallelEnvironment implements java.io.Serializable
	[Serializable]
	public class ParallelEnvironment
	{

		/// <summary>
		/// Number of nodes asked by the user. </summary>
		private int nodesNumber;

		/// <summary>
		/// The topology descriptor of the task
		/// </summary>
		private TopologyDescriptor topologyDescriptor = null;

		/// <summary>
		/// Default constructor for hibernate
		/// </summary>
		protected internal ParallelEnvironment()
		{
		}

		/// <summary>
		/// Creates new instance of the parallel environment.
		/// </summary>
		/// <param name="nodesNumber"> the number of nodes required by task. </param>
		public ParallelEnvironment(int nodesNumber)
		{
			if (nodesNumber < 1)
			{
				throw new System.ArgumentException("nodesNumber must be greater than 1");
			}
			this.nodesNumber = nodesNumber;
		}

		/// <summary>
		/// Creates new instance of the parallel environment.
		/// </summary>
		/// <param name="nodesNumber"> the number of nodes required by task. </param>
		/// <param name="topologyDescriptor"> nodes topology that will be used for this task. </param>
		public ParallelEnvironment(int nodesNumber, TopologyDescriptor topologyDescriptor) : this(nodesNumber)
		{
			this.topologyDescriptor = topologyDescriptor;
		}

		/// <summary>
		/// Copy constructor
		/// <para>
		/// Does not properly copy the topology descriptor, shares the same reference
		/// 
		/// </para>
		/// </summary>
		/// <param name="original"> the original parallel env to copy </param>
		public ParallelEnvironment(ParallelEnvironment original)
		{
			this.nodesNumber = original.NodesNumber;
			this.topologyDescriptor = original.topologyDescriptor;
		}

		/// <summary>
		/// Returns the node number of the parallel task. </summary>
		/// <returns> the node number of the parallel task. </returns>
		public virtual int NodesNumber
		{
			get
			{
				return nodesNumber;
			}
		}

		/// <summary>
		/// Returns the topology descriptor of the parallel task. </summary>
		/// <returns> the topology descriptor of the parallel task. </returns>
		public virtual TopologyDescriptor TopologyDescriptor
		{
			get
			{
				return topologyDescriptor;
			}
			set
			{
				this.topologyDescriptor = value;
			}
		}


		public override string ToString()
		{
			return "ParallelEnvironment { " + " nodesNumber = " + nodesNumber + ", topologyDescriptor = " + topologyDescriptor + '}';
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

			ParallelEnvironment that = (ParallelEnvironment) o;

			if (nodesNumber != that.nodesNumber)
			{
				return false;
			}
			return topologyDescriptor != null ? topologyDescriptor.Equals(that.topologyDescriptor) : that.topologyDescriptor == null;
		}

		public override int GetHashCode()
		{
			int result = nodesNumber;
			result = 31 * result + (topologyDescriptor != null ? topologyDescriptor.GetHashCode() : 0);
			return result;
		}
	}

}