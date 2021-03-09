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
namespace org.objectweb.proactive.extensions.dataspaces.vfs.selector
{

	/// <summary>
	/// The purpose of this class is to select files according to a list of include
	/// patterns and a list of excludes patterns. This selector will respond {@code
	/// true} if the file that is analyzed matches one of the includes patterns and
	/// does not match any of the excludes patterns. In order to perform the match
	/// operation, the base folder URI is extracted from the file URI and the
	/// remaining is matched against the patterns.
	/// <p/>
	/// The matching operation is delegated to the JDK implementation. By default, if
	/// no syntax is specified with a pattern, then glob patterns are used. Other
	/// syntaxes are supported, by prefixing patterns with a prefix such as {@code
	/// regex:} for REGEX patterns.
	/// 
	/// @author The ProActive Team </summary>
	/// <seealso cref= FileSystem#getPathMatcher(String) </seealso>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI @XmlAccessorType(XmlAccessType.FIELD) public class FileSelector implements org.apache.commons.vfs2.FileSelector, java.io.Serializable
	[Serializable]
	public class FileSelector
	{

		private const string PREFIX_GLOB_PATTERN = "glob:";

		private const string PREFIX_REGEX_PATTERN = "regex:";

		private ISet<string> includes;

		private ISet<string> excludes;

		public FileSelector()
		{
			includes = new HashSet<string>();
			excludes = new HashSet<string>();
		}

		public FileSelector(ICollection<string> includes, ICollection<string> excludes) : this()
		{
			addAll(this.includes, includes);
			addAll(this.excludes, excludes);
		}

		public FileSelector(string[] includes, string[] excludes) : this()
		{
			addAll(this.includes, includes);
			addAll(this.excludes, excludes);
		}

		public FileSelector(params string[] includes) : this()
		{
			addAll(this.includes, includes);
		}

		public virtual void clear()
		{
			includes.Clear();
			excludes.Clear();
		}

		public virtual void addIncludes(ICollection<string> patterns)
		{
			addAll(includes, patterns);
		}

		public virtual void addIncludes(params string[] patterns)
		{
			addAll(includes, patterns);
		}

		public virtual void addExcludes(ICollection<string> patterns)
		{
			addAll(excludes, patterns);
		}

		public virtual void addExcludes(params string[] patterns)
		{
			addAll(excludes, patterns);
		}

		public virtual ISet<string> getIncludes()
		{
			return new HashSet<string>(includes);
		}

		public virtual ISet<string> getExcludes()
		{
			return new HashSet<string>(excludes);
		}

		public virtual void setIncludes(ICollection<string> patterns)
		{
			includes.Clear();
			addAll(includes, patterns);
		}

		public virtual void setIncludes(params string[] patterns)
		{
			includes.Clear();
			addAll(includes, patterns);
		}

		public virtual void setExcludes(ICollection<string> patterns)
		{
			excludes.Clear();
			addAll(excludes, patterns);
		}

		public virtual void setExcludes(params string[] patterns)
		{
			excludes.Clear();
			addAll(excludes, patterns);
		}


		private void addAll<T>(ISet<T> receivingSet, ICollection<T> elementsToAdd)
		{
			if (elementsToAdd != null && elementsToAdd.Count > 0)
			{
				receivingSet.UnionWith(elementsToAdd);
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SafeVarargs private final <T> void addAll(java.util.Set<T> receivingSet, T... elementsToAdd)
		private void addAll<T>(ISet<T> receivingSet, params T[] elementsToAdd)
		{
			if (elementsToAdd != null && elementsToAdd.Length > 0)
			{
				foreach (T element in elementsToAdd)
				{
					receivingSet.Add(element);
				}
			}
		}

		/// <summary>
		/// Return a string representation of this selector. All selection patterns
		/// are displayed.
		/// </summary>
		public override string ToString()
		{
			return "FileSelector{" + "includes=" + includes + ", excludes=" + excludes + '}';
		}

	}

}