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
namespace org.ow2.proactive.topology.descriptor
{


	/// 
	/// <summary>
	/// Class represents the descriptor of the nodes topology (network location)
	/// which could be used with {@code ResourceManager.getAtMostNodes} request.
	/// <para>
	/// User may create its own instance of available descriptors and parameterize it or
	/// use one of predefined constants when it is sufficient.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public class TopologyDescriptor implements java.io.Serializable
	[Serializable]
	public class TopologyDescriptor
	{

		/// <summary>
		/// no constraint on node location </summary>
		public static readonly TopologyDescriptor ARBITRARY = new ArbitraryTopologyDescriptor();

		/// <summary>
		/// the set of closest nodes </summary>
		public static readonly TopologyDescriptor BEST_PROXIMITY = new BestProximityDescriptor();

		/// <summary>
		/// the set of nodes on a single host </summary>
		public static readonly TopologyDescriptor SINGLE_HOST = new SingleHostDescriptor();

		/// <summary>
		/// the set of nodes of a single host exclusively
		/// (the host is reserved for the user)
		/// </summary>
		public static readonly TopologyDescriptor SINGLE_HOST_EXCLUSIVE = new SingleHostExclusiveDescriptor();

		/// <summary>
		/// the set of nodes of several hosts exclusively
		/// (hosts are reserved for the user)
		/// </summary>
		public static readonly TopologyDescriptor MULTIPLE_HOSTS_EXCLUSIVE = new MultipleHostsExclusiveDescriptor();

		/// <summary>
		/// the set of nodes one per host exclusively
		/// (hosts are reserved for the user)
		/// </summary>
		public static readonly TopologyDescriptor DIFFERENT_HOSTS_EXCLUSIVE = new DifferentHostsExclusiveDescriptor();

		/// <summary>
		/// the flag indicated that descriptor requires the topology information in the resource manager.
		/// It affects the scripts execution strategy: if true selection scripts are executed on all nodes
		/// first and then the topology information is taken into account. If false scripts are executed
		/// only on required number of nodes.
		/// If this field is set to false the descriptor could be used even when the topology is
		/// disabled in the resource manager.
		/// </summary>
		private bool topologyBased;

		/// <summary>
		/// Creates the descriptor. </summary>
		/// <param name="topologyBased"> indicates that descriptor requires the topology information in the resource manager.
		/// It affects the scripts execution strategy: if true selection scripts are executed on all nodes
		/// first and then the topology information is taken into account. If false scripts are executed
		/// only on required number of nodes.
		/// If this field is set to false the descriptor could be used even when the topology is
		/// disabled in the resource manager. </param>
		protected internal TopologyDescriptor(bool topologyBased)
		{
			this.topologyBased = topologyBased;
		}

		/// <summary>
		/// Defines if selection scripts have to be executed on all
		/// available nodes before the topology information will be processed.
		/// </summary>
		/// <returns> true in case of greedy behavior false otherwise </returns>
		public virtual bool TopologyBased
		{
			get
			{
				return topologyBased;
			}
		}

		/// <summary>
		/// Returns the string representation of the descriptor.
		/// </summary>
		public override string ToString()
		{
			return this.GetType().Name;
		}
	}

}