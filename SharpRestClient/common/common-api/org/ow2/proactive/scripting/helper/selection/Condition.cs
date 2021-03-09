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
namespace org.ow2.proactive.scripting.helper.selection
{


	/// <summary>
	/// A Condition object is a structure which defines
	/// - the name of the property
	/// - the value which will compare
	/// - the operator which will be used to operate this
	/// 
	/// In order to compare some values with a local property file, create one or
	/// several Condition object and call CheckProperty or CheckProperties method
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 1.0
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public class Condition
	public class Condition
	{

		/// <summary>
		/// Name of the condition </summary>
		private string name;

		/// <summary>
		/// Operator </summary>
		private int @operator;

		/// <summary>
		/// Value to compare </summary>
		private string value;

		/// <summary>
		/// Create a new condition using its name, operator and value.
		/// </summary>
		/// <param name="name"> of the property to check </param>
		/// <param name="operator"> the operator on which to base the comparison </param>
		/// <param name="value"> the value to be compared </param>
		public Condition(string name, int @operator, string value)
		{
			this.name = name;
			this.@operator = @operator;
			this.value = value;
		}

		/// <summary>
		/// Get the name of the condition
		/// </summary>
		/// <returns> the name of the condition </returns>
		public virtual string Name
		{
			get
			{
				return this.name;
			}
		}

		/// <summary>
		/// Get the operator of the condition
		/// </summary>
		/// <returns> the operator of the condition </returns>
		public virtual int Operator
		{
			get
			{
				return this.@operator;
			}
		}

		/// <summary>
		/// Get the value of the condition
		/// </summary>
		/// <returns> the value of the condition as a String </returns>
		public virtual string Value
		{
			get
			{
				return this.value;
			}
		}

	}

}