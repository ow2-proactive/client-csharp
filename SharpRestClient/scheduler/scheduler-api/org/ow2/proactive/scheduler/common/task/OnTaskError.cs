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
namespace org.ow2.proactive.scheduler.common.task
{



//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI @XmlRootElement(name = "onTaskError") @XmlAccessorType(XmlAccessType.FIELD) public class OnTaskError implements java.io.Serializable
	[Serializable]
	public class OnTaskError
	{

		// PUBLIC AND PRIVATE CONSTANTS
		private const string CANCEL_JOB_STRING = "cancelJob";

		public static readonly OnTaskError CANCEL_JOB = new OnTaskError(CANCEL_JOB_STRING);

		private const string SUSPEND_TASK_STRING = "suspendTask";

		public static readonly OnTaskError PAUSE_TASK = new OnTaskError(SUSPEND_TASK_STRING);

		private const string PAUSE_JOB_STRING = "pauseJob";

		public static readonly OnTaskError PAUSE_JOB = new OnTaskError(PAUSE_JOB_STRING);

		private const string CONTINUE_JOB_EXECUTION_STRING = "continueJobExecution";

		public static readonly OnTaskError CONTINUE_JOB_EXECUTION = new OnTaskError(CONTINUE_JOB_EXECUTION_STRING);

		private const string NONE_STRING = "none";

		public static readonly OnTaskError NONE = new OnTaskError(NONE_STRING);

		// Member
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAttribute private final String descriptor;
		private readonly string descriptor;

		private OnTaskError(string descriptor)
		{
			this.descriptor = descriptor;
		}

		/// <summary>
		/// Get a OnTaskError instance based on a descriptor string. If the descriptor string is not found,
		/// 'none' is returned. </summary>
		/// <param name="descriptor"> Descriptor string. </param>
		/// <returns> OnTaskError instance or 'not set' if descriptor string is not recognized. </returns>
		public static OnTaskError getInstance(string descriptor)
		{
			switch (descriptor)
			{
				case CANCEL_JOB_STRING:
					return CANCEL_JOB;
				case SUSPEND_TASK_STRING:
					return PAUSE_TASK;
				case PAUSE_JOB_STRING:
					return PAUSE_JOB;
				case CONTINUE_JOB_EXECUTION_STRING:
					return CONTINUE_JOB_EXECUTION;
				default:
					return NONE;
			}
		}

		public override string ToString()
		{
			return this.descriptor;
		}

		public override bool Equals(object onTaskError)
		{
			if (onTaskError == null)
			{
				return false;
			}
			if (onTaskError == this)
			{
				return true;
			}
			if (onTaskError.GetType() != this.GetType())
			{
				return this.ToString().Equals(onTaskError.ToString());
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return this.descriptor.GetHashCode();
		}
	}

}