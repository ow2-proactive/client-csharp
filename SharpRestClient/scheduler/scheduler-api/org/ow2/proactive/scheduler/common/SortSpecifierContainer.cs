using System;
using System.Collections.Generic;
using System.Text;

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


	/// <summary>
	/// Utility container to pass multiple parameters for tasks sorting.
	/// 
	/// @author ActiveEon Team
	/// </summary>
	[Serializable]
	public sealed class SortSpecifierContainer
	{

		public static readonly SortSpecifierContainer EMPTY_CONTAINER = new SortSpecifierContainer();

		private readonly List<SortSpecifierItem> sortParameters;

		public SortSpecifierContainer()
		{
			sortParameters = new List<SortSpecifierItem>();
		}

		[Serializable]
		public sealed class SortSpecifierItem
		{
			private readonly SortSpecifierContainer outerInstance;


			internal readonly string field;

			internal readonly string order;

			internal SortSpecifierItem(SortSpecifierContainer outerInstance, string field, string order)
			{
				this.outerInstance = outerInstance;
				this.field = field;
				this.order = order;
			}

			public SortSpecifierItem(SortSpecifierContainer outerInstance)
			{
				this.outerInstance = outerInstance;
				this.field = "NOTSET";
				this.order = "ASCENDING";
			}

			public override string ToString()
			{
				return field + "," + order;
			}

			public string Field
			{
				get
				{
					return field;
				}
			}

			public string Order
			{
				get
				{
					return order;
				}
			}
		}

		public SortSpecifierContainer(int size)
		{
			sortParameters = new List<SortSpecifierItem>(size);
		}

		public SortSpecifierContainer(string values)
		{
			sortParameters = new List<SortSpecifierItem>();
			if (!string.ReferenceEquals(values, null) && "".CompareTo(values) != 0)
			{
				foreach (string s in values.Split("\\s*;\\s*", true))
				{
					string[] sortParam = s.Split("\\s*,\\s*", true);
					add(sortParam[0], sortParam[1]);
				}
			}

		}

		public void add(string field, string order)
		{
			sortParameters.Add(new SortSpecifierItem(this, field, order));
		}

		public IList<SortSpecifierItem> SortParameters
		{
			get
			{
				return sortParameters;
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			int paddedSize = sortParameters.Count - 1;
			for (int i = 0; i < sortParameters.Count; i++)
			{
				sb.Append(sortParameters[i].ToString());
				if (i < paddedSize)
				{
					sb.Append(";");
				}
			}
			return sb.ToString();
		}

	}

}