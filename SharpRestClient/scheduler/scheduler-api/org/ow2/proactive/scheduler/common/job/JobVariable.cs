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
namespace org.ow2.proactive.scheduler.common.job
{



	/// 
	/// 
	/// <summary>
	/// @author The ProActive Team
	/// @since ProActive Scheduling 7.24
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI @XmlAccessorType(XmlAccessType.FIELD) public class JobVariable implements java.io.Serializable
	[Serializable]
	public class JobVariable
	{

		private string name;

		private string value;

		private string model;

		public JobVariable()
		{
			//Empty constructor
		}

		public JobVariable(string name, string value) : this(name, value, null)
		{
		}

		public JobVariable(string name, string value, string model)
		{
			this.name = name;
			this.value = value;
			this.model = model;
		}

		public virtual string Name
		{
			get
			{
				return name;
			}
			set
			{
				this.name = value;
			}
		}


		public virtual string Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = value;
			}
		}


		public virtual string Model
		{
			get
			{
				return model;
			}
			set
			{
				this.model = value;
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

			JobVariable jobVariable = (JobVariable) @object;
			if (string.ReferenceEquals(name, null))
			{
				if (!string.ReferenceEquals(jobVariable.name, null))
				{
					return false;
				}
			}
			else if (!name.Equals(jobVariable.name))
			{
				return false;
			}
			if (string.ReferenceEquals(value, null))
			{
				if (!string.ReferenceEquals(jobVariable.value, null))
				{
					return false;
				}
			}
			else if (!value.Equals(jobVariable.value))
			{
				return false;
			}
			if (string.ReferenceEquals(model, null))
			{
				if (!string.ReferenceEquals(jobVariable.model, null))
				{
					return false;
				}
			}
			else if (!model.Equals(jobVariable.model))
			{
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			const int primeNumber = 31;
			int result = 1;
			result = primeNumber * result + ((string.ReferenceEquals(name, null)) ? 0 : name.GetHashCode());
			result = primeNumber * result + ((string.ReferenceEquals(value, null)) ? 0 : value.GetHashCode());
			result = primeNumber * result + ((string.ReferenceEquals(model, null)) ? 0 : model.GetHashCode());
			return result;
		}

		public override string ToString()
		{
			return "JobVariable{" + "name='" + name + '\'' + ", value='" + value + '\'' + ", model='" + model + '\'' + '}';
		}

	}

}