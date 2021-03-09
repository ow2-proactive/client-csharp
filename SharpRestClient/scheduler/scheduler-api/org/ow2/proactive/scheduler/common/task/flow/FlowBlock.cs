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
namespace org.ow2.proactive.scheduler.common.task.flow
{


	/// <summary>
	/// Possible values for Task Block declaration
	/// <para>
	/// Each Task can hold a FlowBlock element;
	/// at least two tasks are needed to create a Task Block
	/// 
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 2.2
	/// 
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public enum FlowBlock
	public sealed class FlowBlock
	{

		/// <summary>
		/// No specific block information
		/// </summary>
		public static readonly FlowBlock NONE = new FlowBlock("NONE", InnerEnum.NONE, "none");

		/// <summary>
		/// Marks the beginning of a new block
		/// </summary>
		public static readonly FlowBlock START = new FlowBlock("START", InnerEnum.START, "start");

		/// <summary>
		/// Marks the ending of the last opened block
		/// </summary>
		public static readonly FlowBlock END = new FlowBlock("END", InnerEnum.END, "end");

		private static readonly List<FlowBlock> valueList = new List<FlowBlock>();

		static FlowBlock()
		{
			valueList.Add(NONE);
			valueList.Add(START);
			valueList.Add(END);
		}

		public enum InnerEnum
		{
			NONE,
			START,
			END
		}

		public readonly InnerEnum innerEnumValue;
		private readonly string nameValue;
		private readonly int ordinalValue;
		private static int nextOrdinal = 0;

		private string str = "";

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="str"> string representation </param>
		private FlowBlock(string name, InnerEnum innerEnum, string str)
		{
			this.str = str;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		public override string ToString()
		{
			return this.str;
		}

		/// <summary>
		/// Parses a string containing the textual representation of a FlowBlock
		/// </summary>
		/// <param name="str"> the string to parse </param>
		/// <returns> the type reflected by the string, or NONE if none matches </returns>
		public static FlowBlock parse(string str)
		{
			if (string.ReferenceEquals(str, null))
			{
				return FlowBlock.NONE;
			}
			if (str.Equals(FlowBlock.START.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				return FlowBlock.START;
			}
			else if (str.Equals(FlowBlock.END.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				return FlowBlock.END;
			}
			else
			{
				return FlowBlock.NONE;
			}
		}


		public static FlowBlock[] values()
		{
			return valueList.ToArray();
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static FlowBlock valueOf(string name)
		{
			foreach (FlowBlock enumInstance in FlowBlock.valueList)
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