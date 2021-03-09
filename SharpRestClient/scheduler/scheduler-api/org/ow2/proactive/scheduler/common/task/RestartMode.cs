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


	/// <summary>
	/// This class represents the different restart mode for a task if an error occurred during its execution.<br>
	/// !! It is no longer an Enum since the rise of Hibernate.
	/// 
	/// @author The ProActive Team
	/// @since ProActive 4.0
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI @XmlRootElement(name = "restartmode") @XmlAccessorType(XmlAccessType.FIELD) public class RestartMode implements java.io.Serializable
	[Serializable]
	public class RestartMode
	{

		/// <summary>
		/// The task will be restarted according to its possible resources.
		/// </summary>
		public static readonly RestartMode ANYWHERE = new RestartMode(1, "Anywhere");

		/// <summary>
		/// The task will be restarted on an other node.
		/// </summary>
		public static readonly RestartMode ELSEWHERE = new RestartMode(2, "Elsewhere");

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAttribute private int index;
		private int index;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAttribute private String description;
		private string description;

		public RestartMode()
		{
		}

		/// <summary>
		/// Implicit constructor of a restart mode.
		/// </summary>
		/// <param name="description"> the name of the restart mode. </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @VisibleForTesting protected RestartMode(int index, String description)
		protected internal RestartMode(int index, string description)
		{
			this.index = index;
			this.description = description;
		}

		/// <summary>
		/// Return the RestartMode corresponding to the given {@code description}.
		/// </summary>
		/// <param name="description"> a string representing the restart mode.
		/// </param>
		/// <returns> the RestartMode. </returns>
		public static RestartMode getMode(string description)
		{
			if (ELSEWHERE.description.Equals(description, StringComparison.OrdinalIgnoreCase))
			{
				return ELSEWHERE;
			}
			else
			{
				return ANYWHERE;
			}
		}

		public static RestartMode getMode(int restartModeId)
		{
			switch (restartModeId)
			{
				case 1:
					return RestartMode.ANYWHERE;
				case 2:
					return RestartMode.ELSEWHERE;
				default:
					throw new System.ArgumentException("Unknown restart mode: " + restartModeId);
			}
		}

		public virtual string Description
		{
			get
			{
				return description;
			}
		}

		public virtual int Index
		{
			get
			{
				return index;
			}
		}

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}

			if (o == null || this.GetType() != o.GetType())
			{
				return false;
			}

			RestartMode that = (RestartMode) o;

			return index == that.index;
		}

		public override int GetHashCode()
		{
			return index;
		}

		public override string ToString()
		{
			return description;
		}

	}

}