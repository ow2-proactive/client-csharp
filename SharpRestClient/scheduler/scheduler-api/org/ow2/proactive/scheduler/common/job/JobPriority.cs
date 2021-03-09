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
namespace org.ow2.proactive.scheduler.common.job
{


	/// <summary>
	/// This is the different job priorities.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 0.9
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public enum JobPriority implements java.io.Serializable
	public sealed class JobPriority
	{
		/// <summary>
		/// Lowest priority </summary>
		public static readonly JobPriority IDLE = new JobPriority("IDLE", InnerEnum.IDLE, "Idle", 0);
		/// <summary>
		/// Lowest priority </summary>
		public static readonly JobPriority LOWEST = new JobPriority("LOWEST", InnerEnum.LOWEST, "Lowest", 1);
		/// <summary>
		/// Low priority </summary>
		public static readonly JobPriority LOW = new JobPriority("LOW", InnerEnum.LOW, "Low", 2);
		/// <summary>
		/// Normal Priority </summary>
		public static readonly JobPriority NORMAL = new JobPriority("NORMAL", InnerEnum.NORMAL, "Normal", 3);
		/// <summary>
		/// High priority </summary>
		public static readonly JobPriority HIGH = new JobPriority("HIGH", InnerEnum.HIGH, "High", 4);
		/// <summary>
		/// Highest priority </summary>
		public static readonly JobPriority HIGHEST = new JobPriority("HIGHEST", InnerEnum.HIGHEST, "Highest", 5);

		private static readonly List<JobPriority> valueList = new List<JobPriority>();

		static JobPriority()
		{
			valueList.Add(IDLE);
			valueList.Add(LOWEST);
			valueList.Add(LOW);
			valueList.Add(NORMAL);
			valueList.Add(HIGH);
			valueList.Add(HIGHEST);
		}

		public enum InnerEnum
		{
			IDLE,
			LOWEST,
			LOW,
			NORMAL,
			HIGH,
			HIGHEST
		}

		public readonly InnerEnum innerEnumValue;
		private readonly string nameValue;
		private readonly int ordinalValue;
		private static int nextOrdinal = 0;
		/// <summary>
		/// Name of the priority </summary>
		private string name;

		/// <summary>
		/// Priority representing by an integer </summary>
		private int priority;

		/// <summary>
		/// Implicit constructor of job priority.
		/// </summary>
		/// <param name="name"> the name of the priority. </param>
		/// <param name="priority"> the integer representing the priority. </param>
		internal JobPriority(string name, InnerEnum innerEnum, string readableName, int priority)
		{
			this.name = readableName;
			this.priority = priority;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		/// <seealso cref= java.lang.Enum#toString() </seealso>
		public override string ToString()
		{
			return name;
		}

		/// <summary>
		/// Return the integer representing the priority.
		/// </summary>
		/// <returns> the integer representing the priority. </returns>
		public int Priority
		{
			get
			{
				return priority;
			}
		}

		/// <summary>
		/// Get the priority associated with the given name.
		/// </summary>
		/// <param name="name"> the name of the priority to find as a string. </param>
		/// <returns> the job priority corresponding to the string or the NORMAL priority if not found. </returns>
		public static JobPriority findPriority(string name)
		{
			if (name.Equals(IDLE.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				return IDLE;
			}

			if (name.Equals(LOWEST.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				return LOWEST;
			}

			if (name.Equals(LOW.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				return LOW;
			}

			if (name.Equals(HIGH.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				return HIGH;
			}

			if (name.Equals(HIGHEST.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				return HIGHEST;
			}

			return NORMAL;
		}

		/// <summary>
		/// Get the priority associated with the given priorityValue.
		/// </summary>
		/// <param name="priorityValue"> the priority value to find. </param>
		/// <returns> the job priority corresponding to the value or the NORMAL priority if not found. </returns>
		public static JobPriority findPriority(int priorityValue)
		{
			if (priorityValue == IDLE.Priority)
			{
				return IDLE;
			}

			if (priorityValue == LOWEST.Priority)
			{
				return LOWEST;
			}

			if (priorityValue == LOW.Priority)
			{
				return LOW;
			}

			if (priorityValue == HIGH.Priority)
			{
				return HIGH;
			}

			if (priorityValue == HIGHEST.Priority)
			{
				return HIGHEST;
			}

			return NORMAL;
		}


		public static JobPriority[] values()
		{
			return valueList.ToArray();
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static JobPriority valueOf(string name)
		{
			foreach (JobPriority enumInstance in JobPriority.valueList)
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