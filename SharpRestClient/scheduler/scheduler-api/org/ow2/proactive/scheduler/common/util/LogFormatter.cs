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
namespace org.ow2.proactive.scheduler.common.util
{

	using org.ow2.proactive.scheduler.common.task;
    using System.Linq;

    public class LogFormatter
	{

		private LogFormatter()
		{
		}

		public static string line<T1, T2>(string name, IDictionary<T1, T2> content)
		{
			if (content.Count == 0)
			{
				return name + " = {}";
			}
			else
			{
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
				return name + " = {" + Environment.NewLine + content.SetOfKeyValuePairs().Select(pair => pair.Key + " = " + pair.Value).Select(s => "\t" + s).Aggregate("", (s1, s2) => s1 + Environment.NewLine + s2) + Environment.NewLine + "}";
			}
		}

		public static string addIndent(string content)
		{
			return addIndent(content, 1);
		}

		public static string addIndent(string content, int numberOfIndents)
		{
			if (numberOfIndents <= 0)
			{
				return content;
			}
			else
			{
				string lineIndent = Enumerable.Range(1, numberOfIndents).Select(i => "\t").Aggregate("", (s1, s2) => string.Concat(s1, s2));
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
				return content.Split(Environment.NewLine, true).Select(s => lineIndent + s).Aggregate("", (s1, s2) => s1 + Environment.NewLine + s2);
			}
		}

		public static string lineWithQuotes(string name, object value)
		{
			if (value != null)
			{
				return string.Format("{0} = '{1}'", name, value);
			}
			else
			{
				return string.Format("{0} = null", name);
			}
		}

		public static string line(string name, object value)
		{
			if (value is UpdatableProperties<object>)
			{
				UpdatableProperties<object> prop = (UpdatableProperties<object>) value;
				return displayKeyValueIfSet(name, prop, () => prop.Value);
			}
			else
			{
				return displayKeyValue(name, value);
			}
		}

		private static string displayKeyValue(string name, object value)
		{
			return string.Format("{0} = {1}", name, value);
		}

		private static string displayKeyValueIfSet(string name, UpdatableProperties<object> prop, System.Func<object> supplier)
		{
			if (prop.Set)
			{
				return string.Format("{0} = '{1}'", name, supplier());
			}
			else
			{
				return "";
			}
		}

		public static string line<T1>(string name, object value, System.Func<T1> supplier)
		{
			if (value != null)
			{
				return string.Format("{0} = {1}", name, supplier());
			}
			else
			{
				return string.Format("{0} = null", name);
			}
		}

	}

}