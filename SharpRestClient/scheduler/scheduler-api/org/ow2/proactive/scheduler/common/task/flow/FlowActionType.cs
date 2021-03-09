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
	/// Control Flow Action types
	/// 
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 2.2
	/// 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public enum FlowActionType
	public sealed class FlowActionType
	{

		/// <summary>
		/// Fallback case: no action is performed
		/// </summary>
		public static readonly FlowActionType CONTINUE = new FlowActionType("CONTINUE", InnerEnum.CONTINUE, "continue");

		/// <summary>
		/// Exclusive branching with optional join
		/// </summary>
		public static readonly FlowActionType IF = new FlowActionType("IF", InnerEnum.IF, "if");

		/// <summary>
		/// Parallel split with join
		/// </summary>
		public static readonly FlowActionType REPLICATE = new FlowActionType("REPLICATE", InnerEnum.REPLICATE, "replicate");

		/// <summary>
		/// Loop back in the flow to a previously executed task
		/// </summary>
		public static readonly FlowActionType LOOP = new FlowActionType("LOOP", InnerEnum.LOOP, "loop");

		private static readonly List<FlowActionType> valueList = new List<FlowActionType>();

		static FlowActionType()
		{
			valueList.Add(CONTINUE);
			valueList.Add(IF);
			valueList.Add(REPLICATE);
			valueList.Add(LOOP);
		}

		public enum InnerEnum
		{
			CONTINUE,
			IF,
			REPLICATE,
			LOOP
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
		private FlowActionType(string name, InnerEnum innerEnum, string str)
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
		/// Parses a string containing the textual representation of a FlowActionType
		/// </summary>
		/// <param name="str"> the string to parse </param>
		/// <returns> the type reflected by the string, or continue if none matches </returns>
		public static FlowActionType parse(string str)
		{
			if (string.ReferenceEquals(str, null))
			{
				return FlowActionType.CONTINUE;
			}
			if (str.Equals(FlowActionType.IF.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				return FlowActionType.IF;
			}
			else if (str.Equals(FlowActionType.REPLICATE.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				return FlowActionType.REPLICATE;
			}
			else if (str.Equals(FlowActionType.LOOP.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				return FlowActionType.LOOP;
			}
			else
			{
				return FlowActionType.CONTINUE;
			}
		}


		public static FlowActionType[] values()
		{
			return valueList.ToArray();
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static FlowActionType valueOf(string name)
		{
			foreach (FlowActionType enumInstance in FlowActionType.valueList)
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