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
	/// The descriptor allows to select a set of closest nodes which is currently
	/// available in the resource manager.
	/// 
	/// In order to specify more precisely what "closets" means user may select various distance
	/// functions (for details see http://en.wikipedia.org/wiki/Cluster_analysis#Agglomerative_hierarchical_clustering)
	/// 
	/// AVG - the mean distance between elements of each cluster (also called average linkage clustering).
	/// MAX - the maximum distance between elements of each cluster (also called complete linkage clustering).
	/// MIN - the minimum distance between elements of each cluster (also called single-linkage clustering).
	/// 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public class BestProximityDescriptor extends TopologyDescriptor
	[Serializable]
	public class BestProximityDescriptor : TopologyDescriptor
	{

		protected internal DistanceFunction function;

		/// <summary>
		/// AVG - the mean distance between elements of each cluster (also called average linkage clustering)
		/// The similarity of two clusters is the similarity of their centers.
		/// This complete-link merge criterion is non-local; the entire structure of the
		/// clustering can influence merge decisions.
		/// </summary>
		public static readonly DistanceFunction AVG = new DistanceFunctionAnonymousInnerClass();

		private class DistanceFunctionAnonymousInnerClass : DistanceFunction
		{
			public long distance(long d1, long d2)
			{
				// not connected
				if (d1 < 0 || d2 < 0)
				{
					return -1;
				}

				return (d1 + d2) / 2;
			}
		}

		/// <summary>
		/// MAX - the maximum distance between elements of each cluster (also called complete linkage clustering)
		/// The similarity of two clusters is the similarity of their most dissimilar members.
		/// This complete-link merge criterion is non-local; the entire structure of the
		/// clustering can influence merge decisions.
		/// </summary>
		public static readonly DistanceFunction MAX = new DistanceFunctionAnonymousInnerClass2();

		private class DistanceFunctionAnonymousInnerClass2 : DistanceFunction
		{
			public long distance(long d1, long d2)
			{
				// not connected
				if (d1 < 0 || d2 < 0)
				{
					return -1;
				}

				return Math.Max(d1, d2);
			}
		}

		/// <summary>
		/// MIN - the minimum distance between elements of each cluster (also called single-linkage clustering)
		/// The similarity of two clusters is the similarity of their most similar members.
		/// This single-link merge criterion is local. We pay attention solely to the area where
		/// the two clusters come closest to each other.
		/// </summary>
		public static readonly DistanceFunction MIN = new DistanceFunctionAnonymousInnerClass3();

		private class DistanceFunctionAnonymousInnerClass3 : DistanceFunction
		{
			public long distance(long d1, long d2)
			{
				return Math.Min(d1, d2);
			}
		}

		/// <summary>
		/// Constructs new instance of the class.
		/// In this case the function for clustering is BestProximityDescriptor.MAX, pivot is empty.
		/// </summary>
		public BestProximityDescriptor() : base(true)
		{
			function = MAX;
		}

		/// <summary>
		/// Gets the distance function. AVG by default. </summary>
		/// <returns> the distance function </returns>
		public virtual DistanceFunction DistanceFunction
		{
			get
			{
				return function == null ? MAX : function;
			}
		}
	}

}