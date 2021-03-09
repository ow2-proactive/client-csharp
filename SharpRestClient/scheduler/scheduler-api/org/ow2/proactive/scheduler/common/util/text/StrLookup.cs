using System.Collections.Generic;
using System.Configuration;

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
namespace org.ow2.proactive.scheduler.common.util.text
{
	/*
	 * Licensed to the Apache Software Foundation (ASF) under one or more
	 * contributor license agreements.  See the NOTICE file distributed with
	 * this work for additional information regarding copyright ownership.
	 * The ASF licenses this file to You under the Apache License, Version 2.0
	 * (the "License"); you may not use this file except in compliance with
	 * the License.  You may obtain a copy of the License at
	 *
	 *      http://www.apache.org/licenses/LICENSE-2.0
	 *
	 * Unless required by applicable law or agreed to in writing, software
	 * distributed under the License is distributed on an "AS IS" BASIS,
	 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	 * See the License for the specific language governing permissions and
	 * limitations under the License.
	 */



	/// <summary>
	/// Lookup a String key to a String value.
	/// <para>
	/// This class represents the simplest form of a string to string map.
	/// It has a benefit over a map in that it can create the result on
	/// demand based on the key.
	/// </para>
	/// <para>
	/// This class comes complete with various factory methods.
	/// If these do not suffice, you can subclass and implement your own matcher.
	/// </para>
	/// <para>
	/// For example, it would be possible to implement a lookup that used the
	/// key as a primary key, and looked up the value on demand from the database
	/// 
	/// @since 2.2
	/// @version $Id$
	/// </para>
	/// </summary>
	public abstract class StrLookup
	{
		/// <summary>
		/// Lookup that always returns null.
		/// </summary>
		private static readonly StrLookup NONE_LOOKUP = new MapStrLookup(null);

		//-----------------------------------------------------------------------
		/// <summary>
		/// Returns a lookup which always returns null.
		/// </summary>
		/// <returns> a lookup that always returns null, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: public static StrLookup<?> noneLookup()
		public static StrLookup noneLookup()
		{
			return NONE_LOOKUP;
		}

		/// <summary>
		/// Creates a copy of the given properties instance.
		/// </summary>
		/// <param name="input"> the Properties instance to copy. </param>
		/// <returns> a copy of {@code input}. </returns>
		private static IDictionary<string, object> copyProperties(SettingsPropertyCollection input)
		{
			if (input == null)
			{
				return null;
			}
			IDictionary<string, object> output = new Dictionary<string, object>();
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") java.util.Iterator<String> propertyNames = (java.util.Iterator<String>) input.propertyNames();
			IEnumerator<SettingsProperty> propertyEnumerator = (IEnumerator<SettingsProperty>) input.GetEnumerator();
			while (propertyEnumerator.MoveNext())
			{
				SettingsProperty currentProperty = propertyEnumerator.Current;

				output.Add(currentProperty.Name, currentProperty.DefaultValue.ToString());
			}
			return output;
		}

		/// <summary>
		/// Returns a new lookup which uses a copy of the current
		/// <seealso cref="System.getProperties() System properties"/>.
		/// <para>
		/// If a security manager blocked access to system properties, then null will
		/// be returned from every lookup.
		/// </para>
		/// <para>
		/// If a null key is used, this lookup will throw a NullPointerException.
		/// 
		/// </para>
		/// </summary>
		/// <returns> a lookup using system properties, not null </returns>
		public static StrLookup systemPropertiesLookup()
		{
			SettingsPropertyCollection systemProperties = systemProperties = SharpRestClient.Properties.Settings.Default.Properties;
			IDictionary<string, object> propertiesMap = copyProperties(systemProperties);
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") final java.util.Map<String, String> propertiesMap = (java.util.Map) properties;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			return new MapStrLookup(propertiesMap);
		}

		/// <summary>
		/// Returns a lookup which looks up values using a map.
		/// <para>
		/// If the map is null, then null will be returned from every lookup.
		/// The map result object is converted to a string using toString().
		/// 
		/// </para>
		/// </summary>
		/// @param <V> the type of the values supported by the lookup </param>
		/// <param name="map">  the map of keys to values, may be null </param>
		/// <returns> a lookup using the map, not null </returns>
		public static StrLookup mapLookup(IDictionary<string, object> map)
		{
			return new MapStrLookup(map);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Constructor.
		/// </summary>
		protected internal StrLookup() : base()
		{
		}

		/// <summary>
		/// Looks up a String key to a String value.
		/// <para>
		/// The internal implementation may use any mechanism to return the value.
		/// The simplest implementation is to use a Map. However, virtually any
		/// implementation is possible.
		/// </para>
		/// <para>
		/// For example, it would be possible to implement a lookup that used the
		/// key as a primary key, and looked up the value on demand from the database
		/// Or, a numeric based implementation could be created that treats the key
		/// as an integer, increments the value and return the result as a string -
		/// converting 1 to 2, 15 to 16 etc.
		/// </para>
		/// <para>
		/// The <seealso cref="lookup(string)"/> method always returns a String, regardless of
		/// the underlying data, by converting it as necessary. For example:
		/// <pre>
		/// Map&lt;String, Object&gt; map = new HashMap&lt;String, Object&gt;();
		/// map.put("number", Integer.valueOf(2));
		/// assertEquals("2", StrLookup.mapLookup(map).lookup("number"));
		/// </pre>
		/// </para>
		/// </summary>
		/// <param name="key">  the key to be looked up, may be null </param>
		/// <returns> the matching value, null if no match </returns>
		public abstract string lookup(string key);

		//-----------------------------------------------------------------------
		/// <summary>
		/// Lookup implementation that uses a Map.
		/// </summary>
		internal class MapStrLookup : StrLookup
		{
			/// <summary>
			/// Map keys are variable names and value. </summary>
			internal readonly IDictionary<string, object> map;

			/// <summary>
			/// Creates a new instance backed by a Map.
			/// </summary>
			/// <param name="map">  the map of keys to values, may be null </param>
			internal MapStrLookup(IDictionary<string, object> map)
			{
				this.map = map;
			}

			/// <summary>
			/// Looks up a String key to a String value using the map.
			/// <para>
			/// If the map is null, then null is returned.
			/// The map result object is converted to a string using toString().
			/// 
			/// </para>
			/// </summary>
			/// <param name="key">  the key to be looked up, may be null </param>
			/// <returns> the matching value, null if no match </returns>
			public override string lookup(string key)
			{
				if (map == null)
				{
					return null;
				}
				//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
				//ORIGINAL LINE: final Object obj = map.get(key);
				object obj = map[key];
				if (obj == null)
				{
					return null;
				}
				return obj.ToString();
			}
		}
	}

}