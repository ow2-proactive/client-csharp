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
	/// UpdatableProperties allow to know if a specified value has been modified or not.
	/// <para>
	/// Useful to know if the default value has been kept.
	/// </para>
	/// <para>
	/// Managed parameter entities are RestartMode, BooleanWrapper, IntegerWrapper.
	/// If you want to add more entities, just add it in the @anyMetaDef annotation.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 0.9.1
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAccessorType(XmlAccessType.FIELD) @PublicAPI public class UpdatableProperties<T> implements java.io.Serializable
	[Serializable]
	public class UpdatableProperties<T>
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlValue private T value = null;
		private T value = default(T);

		/// <summary>
		/// If the property has been set. </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlTransient private boolean set = false;
		private bool set = false;

		/// <summary>
		/// HIBERNATE default constructor </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unused") private UpdatableProperties()
		private UpdatableProperties()
		{
		}

		/// <summary>
		/// Create a new instance of UpdatableProperties using a specified value.
		/// <para>
		/// This value will be considered has the default one.
		/// </para>
		/// </summary>
		public UpdatableProperties(T defaultValue)
		{
			this.value = defaultValue;
		}

		/// <summary>
		/// Get the value of the property.
		/// </summary>
		/// <returns> the value of the property. </returns>
		public virtual T Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = value;
				this.set = true;
			}
		}


		/// <summary>
		/// Tell if the value has been set or if it is the default one.
		/// </summary>
		/// <returns> true if the default value has been changed. </returns>
		public virtual bool Set
		{
			get
			{
				return set;
			}
		}

	}

}