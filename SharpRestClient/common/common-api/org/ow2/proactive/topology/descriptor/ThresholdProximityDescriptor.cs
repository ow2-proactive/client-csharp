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


	/// <summary>
	/// The descriptor allows to select a set of nodes within a threshold proximity.
	/// If pivot is specified the set of required nodes has to be within the
	/// threshold proximity with all nodes from the pivot.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public class ThresholdProximityDescriptor extends BestProximityDescriptor
	[Serializable]
	public class ThresholdProximityDescriptor : BestProximityDescriptor
	{

		private long threshold = 0;

		/// <summary>
		/// Default constructor for hibernate
		/// </summary>
		protected internal ThresholdProximityDescriptor()
		{
		}

		/// <summary>
		/// Creates a new instance of the descriptor with specified threshold.
		/// </summary>
		/// <param name="threshold"> parameter defining maximum distance value between nodes (in microseconds) </param>
		public ThresholdProximityDescriptor(long threshold)
		{
			if (threshold < 0)
			{
				throw new System.ArgumentException("Threshold cannot be negative");
			}

			this.threshold = threshold;
		}

		//
		//    /**
		//     * Creates a new instance of the descriptor with specified threshold and pivot.
		//     *
		//     * @param threshold parameter defining maximum distance value between nodes
		//     * @param pivot a set of nodes which must be within threshold proximity
		//     * with nodes user requires
		//     */
		//    public ThresholdProximityDescriptor(long threshold, List<Node> pivot) {
		//        this(threshold);
		//        this.pivot = pivot;
		//    }

		/// <summary>
		/// Gets the threshold value (in microseconds) </summary>
		/// <returns> the threshold value </returns>
		public virtual long Threshold
		{
			get
			{
				return threshold;
			}
		}

		public override string ToString()
		{
			return base.ToString() + " threshold is " + threshold;
		}
	}

}