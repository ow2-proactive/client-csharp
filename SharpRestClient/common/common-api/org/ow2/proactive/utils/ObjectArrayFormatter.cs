using System.Collections.Generic;
using System.Text;

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
namespace org.ow2.proactive.utils
{


	/// <summary>
	/// ObjectArrayFormatter is used to format (as string) an object in ordered column.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 2.0
	/// </summary>
	public class ObjectArrayFormatter
	{

		private IList<string> title;

		private IList<IList<string>> lines = new List<IList<string>>();

		private int spaces;

		private int maxColumnLength = 20;

		/// <summary>
		/// Set the title value to the given title value.
		/// Must have the same number of elements than each added lines.
		/// </summary>
		/// <param name="title"> the title to set </param>
		public virtual IList<string> Title
		{
			set
			{
				this.title = value;
			}
		}

		/// <summary>
		/// Add the given lines value to existing lines.
		/// Must have the same number of elements than the titles.
		/// </summary>
		/// <param name="lines"> the lines to add </param>
		public virtual void addLine(IList<string> lines)
		{
			if (lines == null || lines.Count == 0)
			{
				throw new System.ArgumentException("Lines must be a non-empty list.");
			}
			this.lines.Add(lines);
		}

		/// <summary>
		/// Add a empty line to the list of lines.
		/// This will be printed as a newline.
		/// </summary>
		public virtual void addEmptyLine()
		{
			this.lines.Add(null);
		}

		/// <summary>
		/// Set the space value to the given space value.
		/// It represent the space between each column.
		/// </summary>
		/// <param name="spaces"> the space to set </param>
		public virtual int Space
		{
			set
			{
				if (value < 1)
				{
					throw new System.ArgumentException("spaces must be a positive value.");
				}
				this.spaces = value;
			}
		}

		/// <summary>
		/// Set the maxColumnLength value to the given maxColumnLength value
		/// </summary>
		/// <param name="maxColumnLength"> the maxColumnLength to set </param>
		public virtual int MaxColumnLength
		{
			set
			{
				if (value < 1)
				{
					throw new System.ArgumentException("maxColumnLength must be a positive");
				}
				this.maxColumnLength = value;
			}
		}

		/// <summary>
		/// Get the columned string according to the titles, lines and spaces.
		/// </summary>
		public virtual string AsString
		{
			get
			{
				//if title does not exist
				if (title == null || title.Count == 0)
				{
					throw new System.InvalidOperationException("Title array does not contain anything !");
				}
				//check that each lines has the same length
				foreach (IList<string> l in lines)
				{
					if (l != null && l.Count != title.Count)
					{
						throw new System.InvalidOperationException("One of the line is not as long as the other or the title array !");
					}
				}
				//init length array with title length
				int[] columnLengths = new int[title.Count];
				for (int i = 0; i < columnLengths.Length; i++)
				{
					columnLengths[i] = title[i].Length;
				}
				//get max length for each fields
				foreach (IList<string> l in lines)
				{
					if (l != null)
					{
						int i = 0;
						foreach (string s in l)
						{
							columnLengths[i] = (s.Length > columnLengths[i]) ? s.Length : columnLengths[i];
							i++;
						}
					}
				}
				//make the maxColumnLength the top limit of the int array and add the separator number of spaces
				for (int i = 0; i < columnLengths.Length; i++)
				{
					if (columnLengths[i] > maxColumnLength)
					{
						columnLengths[i] = maxColumnLength;
					}
					columnLengths[i] += spaces;
				}
				//write the string with the computed limits
				StringBuilder sb = new StringBuilder();
				//print title line
				sb.Append("\t");
				for (int i = 0; i < title.Count; i++)
				{
	//JAVA TO C# CONVERTER TODO TASK: The following line has a Java format specifier which cannot be directly translated to .NET:
					sb.Append(string.Format(" %1$-" + columnLengths[i] + "s", cutNchar(title[i], maxColumnLength)));
				}
				sb.Append(System.Environment.NewLine);
				foreach (IList<string> l in lines)
				{
					if (l == null)
					{
						sb.Append(System.Environment.NewLine);
					}
					else
					{
						sb.Append("\t");
						for (int i = 0; i < l.Count; i++)
						{
	//JAVA TO C# CONVERTER TODO TASK: The following line has a Java format specifier which cannot be directly translated to .NET:
							sb.Append(string.Format(" %1$-" + columnLengths[i] + "s", cutNchar(l[i], maxColumnLength)));
						}
						sb.Append(System.Environment.NewLine);
					}
				}
				return sb.ToString();
			}
		}

		/// <summary>
		/// Cut the given string to the specified number of character
		/// </summary>
		/// <param name="str"> the string to cut. </param>
		/// <param name="nbChar"> the maximum length of the string to be returned </param>
		/// <returns> a string that is the same as the given one if str.length() is lesser or equals to nbChar,
		/// 			otherwise a shortcut string endind with '...' </returns>
		private string cutNchar(string str, int nbChar)
		{
			if (string.ReferenceEquals(str, null))
			{
				return "";
			}
			nbChar--; //use to have a space after the returned string
			if (str.Length <= nbChar)
			{
				return str;
			}
			return str.Substring(0, nbChar - 3) + "...";
		}

	}

}