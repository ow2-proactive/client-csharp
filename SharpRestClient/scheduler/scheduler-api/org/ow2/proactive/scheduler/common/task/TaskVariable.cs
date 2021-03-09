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

	using JobVariable = org.ow2.proactive.scheduler.common.job.JobVariable;


	/// 
	/// 
	/// <summary>
	/// @author The ProActive Team
	/// @since ProActive Scheduling 7.20
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI @XmlAccessorType(XmlAccessType.FIELD) public class TaskVariable extends org.ow2.proactive.scheduler.common.job.JobVariable implements java.io.Serializable
	[Serializable]
	public class TaskVariable : JobVariable
	{

		private bool jobInherited = false;

		public TaskVariable()
		{
			//Empty constructor
		}

		public TaskVariable(string name, string value) : this(name, value, null, false)
		{
		}

		public TaskVariable(string name, string value, string model, bool isJobInherited) : base(name, value, model)
		{
			this.jobInherited = isJobInherited;
		}

		public virtual bool JobInherited
		{
			get
			{
				return jobInherited;
			}
			set
			{
				this.jobInherited = value;
			}
		}


		public override bool Equals(object @object)
		{
			if (this == @object)
			{
				return true;
			}
			if (@object == null)
			{
				return false;
			}
			if (this.GetType() != @object.GetType())
			{
				return false;
			}

			TaskVariable taskVariable = (TaskVariable) @object;
			if (jobInherited != taskVariable.JobInherited)
			{
				return false;
			}
			if (string.ReferenceEquals(Name, null))
			{
				if (!string.ReferenceEquals(taskVariable.Name, null))
				{
					return false;
				}
			}
			else if (!Name.Equals(taskVariable.Name))
			{
				return false;
			}
			if (string.ReferenceEquals(Value, null))
			{
				if (!string.ReferenceEquals(taskVariable.Value, null))
				{
					return false;
				}
			}
			else if (!Value.Equals(taskVariable.Value))
			{
				return false;
			}
			if (string.ReferenceEquals(Model, null))
			{
				if (!string.ReferenceEquals(taskVariable.Model, null))
				{
					return false;
				}
			}
			else if (!Model.Equals(taskVariable.Model))
			{
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			const int primeNumber = 31;
			int result = 1;
			result = primeNumber * result + (jobInherited ? 3 : 5);
			result = primeNumber * result + ((string.ReferenceEquals(Name, null)) ? 0 : Name.GetHashCode());
			result = primeNumber * result + ((string.ReferenceEquals(Value, null)) ? 0 : Value.GetHashCode());
			result = primeNumber * result + ((string.ReferenceEquals(Model, null)) ? 0 : Model.GetHashCode());
			return result;
		}

		public override string ToString()
		{
			return "TaskVariable{" + "name='" + Name + '\'' + ", value='" + Value + '\'' + ", model='" + Model + '\'' + ", jobInherited=" + jobInherited + '}';
		}

	}

}