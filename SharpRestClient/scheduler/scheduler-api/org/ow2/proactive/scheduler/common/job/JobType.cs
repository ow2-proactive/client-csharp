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
	/// Class representing the type of the job.
	/// Type are best describe below.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 0.9
	/// </summary>
	//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
	//ORIGINAL LINE: @PublicAPI public enum JobType implements java.io.Serializable
	[Serializable]
	public sealed class JobType
	{

		/// <summary>
		/// Tasks can be executed one by one or all at the same time but
		/// every task represents the same native or java task.
		/// Only the parameters given to the task will change.
		/// </summary>
		public static readonly JobType PARAMETER_SWEEPING = new JobType("PARAMETER_SWEEPING", InnerEnum.PARAMETER_SWEEPING, "Parameter Sweeping");
		/// <summary>
		/// Tasks flow with dependences.
		/// Only the task that have their dependences finished
		/// can be executed.
		/// </summary>
		public static readonly JobType TASKSFLOW = new JobType("TASKSFLOW", InnerEnum.TASKSFLOW, "Tasks Flow");

		private static readonly List<JobType> valueList = new List<JobType>();

		static JobType()
		{
			valueList.Add(PARAMETER_SWEEPING);
			valueList.Add(TASKSFLOW);
		}

		public enum InnerEnum
		{
			PARAMETER_SWEEPING,
			TASKSFLOW
		}

		public readonly InnerEnum innerEnumValue;
		private readonly string nameValue;
		private readonly int ordinalValue;
		private static int nextOrdinal = 0;

		private string name;

		internal JobType(string name, InnerEnum innerEnum, string readableName)
		{
			this.name = readableName;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		/// <seealso cref= java.lang.Enum#toString() </seealso>
		public override string ToString()
		{
			return name;
		}

		internal static JobType getJobType(string typeName)
		{
			if (typeName.Equals("taskFlow", StringComparison.OrdinalIgnoreCase))
			{
				return TASKSFLOW;
			}
			else
			{
				return PARAMETER_SWEEPING;
			}
		}

		public static JobType[] values()
		{
			return valueList.ToArray();
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static JobType valueOf(string name)
		{
			foreach (JobType enumInstance in JobType.valueList)
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