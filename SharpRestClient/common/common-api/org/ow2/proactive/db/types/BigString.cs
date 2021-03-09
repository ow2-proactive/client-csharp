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
namespace org.ow2.proactive.db.types
{


	/// <summary>
	/// BigString is the Scheduler Wrapper object for String.
	/// It is mostly used for Hibernate when using string as value in a hashMap.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 0.9.1
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI @XmlAccessorType(XmlAccessType.FIELD) public final class BigString implements java.io.Serializable
	[Serializable]
	public sealed class BigString
	{

		private string value;

		public BigString()
		{
		}

		/// <summary>
		/// Create a new instance of BigString.
		/// </summary>
		/// <param name="value"> </param>
		public BigString(string value)
		{
			this.value = value;
		}

		/// <summary>
		/// Get the String value.
		/// </summary>
		/// <returns> the String value. </returns>
		public string Value
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


	}

}