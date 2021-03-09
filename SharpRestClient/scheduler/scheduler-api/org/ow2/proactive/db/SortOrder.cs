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
namespace org.ow2.proactive.db
{


//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public enum SortOrder
	public sealed class SortOrder
	{
		public static readonly SortOrder ASC = new SortOrder("ASC", InnerEnum.ASC, true);
		public static readonly SortOrder DESC = new SortOrder("DESC", InnerEnum.DESC, false);

		private static readonly List<SortOrder> valueList = new List<SortOrder>();

		static SortOrder()
		{
			valueList.Add(ASC);
			valueList.Add(DESC);
		}

		public enum InnerEnum
		{
			ASC,
			DESC
		}

		public readonly InnerEnum innerEnumValue;
		private readonly string nameValue;
		private readonly int ordinalValue;
		private static int nextOrdinal = 0;

		private readonly bool isAscending;

		private SortOrder(string name, InnerEnum innerEnum, bool isAscending)
		{
			this.isAscending = isAscending;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		public bool Ascending
		{
			get
			{
				return isAscending;
			}
		}


		public static SortOrder[] values()
		{
			return valueList.ToArray();
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public override string ToString()
		{
			return nameValue;
		}

		public static SortOrder valueOf(string name)
		{
			foreach (SortOrder enumInstance in SortOrder.valueList)
			{
				if (enumInstance.nameValue == name)
				{
					return enumInstance;
				}
			}
			throw new System.ArgumentException(name);
		}
	}

}