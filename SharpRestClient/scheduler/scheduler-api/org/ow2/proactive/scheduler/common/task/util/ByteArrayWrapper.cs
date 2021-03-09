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
namespace org.ow2.proactive.scheduler.common.task.util
{


	/// <summary>
	/// ByteArrayWrapper is the Scheduler Wrapper object for byte[].
	/// It is mostly used for Hibernate when using byte array with parametric type.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 1.1
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public class ByteArrayWrapper implements java.io.Serializable
	[Serializable]
	public class ByteArrayWrapper
	{
		private sbyte[] byteArray;

		/// <summary>
		/// Create a new instance of ByteArrayWrapper.
		/// </summary>
		/// <param name="value"> the value of this wrapper. </param>
		public ByteArrayWrapper(sbyte[] value)
		{
			this.byteArray = value;
		}

		/// <summary>
		/// Get the ByteArray value.
		/// </summary>
		/// <returns> the ByteArray value. </returns>
		public virtual sbyte[] ByteArray
		{
			get
			{
				return byteArray;
			}
		}

	}

}