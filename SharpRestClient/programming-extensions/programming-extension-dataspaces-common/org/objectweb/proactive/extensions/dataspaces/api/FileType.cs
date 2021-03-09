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
namespace org.objectweb.proactive.extensions.dataspaces.api
{
	public sealed class FileType
	{

		/// <summary>
		/// Represents a folder type
		/// </summary>
		public static readonly FileType FOLDER = new FileType("FOLDER", InnerEnum.FOLDER, true, false, true);

		/// <summary>
		/// Represents an ordinary file type
		/// </summary>
		public static readonly FileType FILE = new FileType("FILE", InnerEnum.FILE, false, true, true);

		/// <summary>
		/// Represents yet unknown file type
		/// </summary>
		public static readonly FileType ABSTRACT = new FileType("ABSTRACT", InnerEnum.ABSTRACT, false, false, false);

		private static readonly List<FileType> valueList = new List<FileType>();

		static FileType()
		{
			valueList.Add(FOLDER);
			valueList.Add(FILE);
			valueList.Add(ABSTRACT);
		}

		public enum InnerEnum
		{
			FOLDER,
			FILE,
			ABSTRACT
		}

		public readonly InnerEnum innerEnumValue;
		private readonly string nameValue;
		private readonly int ordinalValue;
		private static int nextOrdinal = 0;

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods of the current type:
		private readonly bool hasChildren_Conflict;

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods of the current type:
		private readonly bool hasContent_Conflict;

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods of the current type:
		private readonly bool hasAttrs_Conflict;

		private FileType(string name, InnerEnum innerEnum, in bool hasChildren, in bool hasContent, in bool hasAttrs)
		{

			this.hasChildren_Conflict = hasChildren;
			this.hasContent_Conflict = hasContent;
			this.hasAttrs_Conflict = hasAttrs;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		public bool hasChildren()
		{
			return hasChildren_Conflict;
		}

		public bool hasContent()
		{
			return hasContent_Conflict;
		}

		public bool hasAttrs()
		{
			return hasAttrs_Conflict;
		}

		public static FileType[] values()
		{
			return valueList.ToArray();
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public override string ToString()
		{
			return nameValue;
		}

		public static FileType valueOf(string name)
		{
			foreach (FileType enumInstance in FileType.valueList)
			{
				if (enumInstance.nameValue == name)
				{
					return enumInstance;
				}
			}
			throw new System.ArgumentException(name);
		}
	}

}