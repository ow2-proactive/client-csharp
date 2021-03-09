using System.Collections.Generic;
using System.Text.RegularExpressions;

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



	/// <summary>
	/// RegexpMatcher should help you when looking for patterns in a string.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 0.9.1
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public class RegexpMatcher
	public class RegexpMatcher
	{

		private Regex pattern = null;

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods of the current type:
		private List<string> matchesList;

		private int cursor;

		/// <summary>
		/// Create a new instance of RegexpMatcher.
		/// </summary>
		/// <param name="pattern"> the pattern to find in the string. It is no need to surround your string with the .* characters. </param>
		public RegexpMatcher(string pattern)
		{
			this.pattern = new Regex("(" + pattern + ")");
		}

		/// <summary>
		/// Matches the regular expression with the given string.
		/// </summary>
		/// <param name="toMatch"> the String where to find the regular expression. </param>
		public virtual void matches(string toMatch)
		{
			var matches = pattern.Matches(toMatch);
			matchesList = new List<string>();

			if (matches != null)
			{
				foreach (Match m in matches)
					matchesList.Add(m.Groups[1].Value);
			}
			cursor = 0;
		}

		/// <summary>
		/// Once matches, returns the next string instance matches by the pattern.
		/// </summary>
		/// <returns> the next string instance matches by the pattern. </returns>
		public virtual string next()
		{
			return matchesList[cursor++];
		}

		/// <summary>
		/// Tell if an other matches is existing.
		/// </summary>
		/// <returns> true if there is an other matches, false otherwise. </returns>
		public virtual bool hasNext()
		{
			return matchesList.Count > cursor;
		}

		/// <summary>
		/// Return an array that contains all the string matched by the given pattern in the given string.
		/// </summary>
		/// <param name="pattern"> the pattern to find in the string. It is no need to surround your string with the .* characters. </param>
		/// <param name="toMatch"> the String where to find the regular expression. </param>
		/// <returns> an array that contains all the string matched by the given pattern in the given string.
		/// 			return an empty array if nothing found. </returns>
		public static string[] matches(string pattern, string toMatch)
		{
			RegexpMatcher matcher = new RegexpMatcher(pattern);
			matcher.matches(toMatch);
			return matcher.matchesList.ToArray();
		}

	}

}