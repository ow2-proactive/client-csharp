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
namespace org.ow2.proactive.scheduler.common.task.dataspaces
{


	/// <summary>
	/// OutputAccessMode provide a way to define how output files should be managed
	/// after the execution.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 1.1
	/// </summary>
	//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
	//ORIGINAL LINE: @PublicAPI public enum OutputAccessMode
	[Serializable]
	public sealed class OutputAccessMode
	{
		/// <summary>
		/// Transfer files from LOCAL space to OUTPUT space </summary>
		public static readonly OutputAccessMode TransferToOutputSpace = new OutputAccessMode("TransferToOutputSpace", InnerEnum.TransferToOutputSpace, "transferToOutputSpace");
		/// <summary>
		/// LOCAL to GLOBAL </summary>
		public static readonly OutputAccessMode TransferToGlobalSpace = new OutputAccessMode("TransferToGlobalSpace", InnerEnum.TransferToGlobalSpace, "transferToGlobalSpace");
		/// <summary>
		/// LOCAL to GLOBAL </summary>
		public static readonly OutputAccessMode TransferToUserSpace = new OutputAccessMode("TransferToUserSpace", InnerEnum.TransferToUserSpace, "transferToUserSpace");
		/// <summary>
		/// Do nothing </summary>
		public static readonly OutputAccessMode none = new OutputAccessMode("none", InnerEnum.none, "none");

		private static readonly List<OutputAccessMode> valueList = new List<OutputAccessMode>();

		static OutputAccessMode()
		{
			valueList.Add(TransferToOutputSpace);
			valueList.Add(TransferToGlobalSpace);
			valueList.Add(TransferToUserSpace);
			valueList.Add(none);
		}

		public enum InnerEnum
		{
			TransferToOutputSpace,
			TransferToGlobalSpace,
			TransferToUserSpace,
			none
		}

		public readonly InnerEnum innerEnumValue;
		private readonly string nameValue;
		private readonly int ordinalValue;
		private static int nextOrdinal = 0;

		internal string title;

		private OutputAccessMode(string name, InnerEnum innerEnum, string s)
		{
			title = s;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		public static OutputAccessMode getAccessMode(string accessMode)
		{
			if (TransferToOutputSpace.title.Equals(accessMode, StringComparison.InvariantCultureIgnoreCase))
			{
				return TransferToOutputSpace;
			}
			else if (TransferToGlobalSpace.title.Equals(accessMode, StringComparison.InvariantCultureIgnoreCase))
			{
				return TransferToGlobalSpace;
			}
			else if (TransferToUserSpace.title.Equals(accessMode, StringComparison.InvariantCultureIgnoreCase))
			{
				return TransferToUserSpace;
			}
			else if (none.title.Equals(accessMode, StringComparison.InvariantCultureIgnoreCase))
			{
				return none;
			}
			else
			{
				throw new System.ArgumentException("Unknow Output access mode : " + accessMode);
			}
		}

		public override string ToString()
		{
			return this.title;
		}

		public static OutputAccessMode[] values()
		{
			return valueList.ToArray();
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static OutputAccessMode valueOf(string name)
		{
			foreach (OutputAccessMode enumInstance in OutputAccessMode.valueList)
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