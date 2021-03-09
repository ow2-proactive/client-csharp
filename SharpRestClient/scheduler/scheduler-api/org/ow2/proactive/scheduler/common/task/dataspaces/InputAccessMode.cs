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
namespace org.ow2.proactive.scheduler.common.task.dataspaces
{


	/// <summary>
	/// InputAccessMode provide a way to define how files should be accessed
	/// in the executable.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 1.1
	/// </summary>
	//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
	//ORIGINAL LINE: @PublicAPI public enum InputAccessMode
	[Serializable]
	public sealed class InputAccessMode
	{
		/// <summary>
		/// Transfer files from INPUT space to LOCAL space </summary>
		public static readonly InputAccessMode TransferFromInputSpace = new InputAccessMode("TransferFromInputSpace", InnerEnum.TransferFromInputSpace, "transferFromInputSpace");
		/// <summary>
		/// Transfer files from OUTPUT space to LOCAL space </summary>
		public static readonly InputAccessMode TransferFromOutputSpace = new InputAccessMode("TransferFromOutputSpace", InnerEnum.TransferFromOutputSpace, "transferFromOutputSpace");
		/// <summary>
		/// Transfer files from GLOBAL space to LOCAL space
		/// </summary>
		public static readonly InputAccessMode TransferFromGlobalSpace = new InputAccessMode("TransferFromGlobalSpace", InnerEnum.TransferFromGlobalSpace, "transferFromGlobalSpace");
		/// <summary>
		/// Transfer files from USER space to LOCAL space </summary>
		public static readonly InputAccessMode TransferFromUserSpace = new InputAccessMode("TransferFromUserSpace", InnerEnum.TransferFromUserSpace, "transferFromUserSpace");
		/// <summary>
		/// cache files from INPUT space to CACHE space
		/// </summary>
		public static readonly InputAccessMode CacheFromInputSpace = new InputAccessMode("CacheFromInputSpace", InnerEnum.CacheFromInputSpace, "cacheFromInputSpace");
		/// <summary>
		/// cache files from OUTPUT space to CACHE space
		/// </summary>
		public static readonly InputAccessMode CacheFromOutputSpace = new InputAccessMode("CacheFromOutputSpace", InnerEnum.CacheFromOutputSpace, "cacheFromOutputSpace");
		/// <summary>
		/// cache files from GLOBAL space to CACHE space
		/// </summary>
		public static readonly InputAccessMode CacheFromGlobalSpace = new InputAccessMode("CacheFromGlobalSpace", InnerEnum.CacheFromGlobalSpace, "cacheFromGlobalSpace");
		/// <summary>
		/// cache files from GLOBAL space to CACHE space
		/// </summary>
		public static readonly InputAccessMode CacheFromUserSpace = new InputAccessMode("CacheFromUserSpace", InnerEnum.CacheFromUserSpace, "cacheFromUserSpace");
		/// <summary>
		/// Do nothing </summary>
		public static readonly InputAccessMode none = new InputAccessMode("none", InnerEnum.none, "none");

		private static readonly List<InputAccessMode> valueList = new List<InputAccessMode>();

		static InputAccessMode()
		{
			valueList.Add(TransferFromInputSpace);
			valueList.Add(TransferFromOutputSpace);
			valueList.Add(TransferFromGlobalSpace);
			valueList.Add(TransferFromUserSpace);
			valueList.Add(CacheFromInputSpace);
			valueList.Add(CacheFromOutputSpace);
			valueList.Add(CacheFromGlobalSpace);
			valueList.Add(CacheFromUserSpace);
			valueList.Add(none);
		}

		public enum InnerEnum
		{
			TransferFromInputSpace,
			TransferFromOutputSpace,
			TransferFromGlobalSpace,
			TransferFromUserSpace,
			CacheFromInputSpace,
			CacheFromOutputSpace,
			CacheFromGlobalSpace,
			CacheFromUserSpace,
			none
		}

		public readonly InnerEnum innerEnumValue;
		private readonly string nameValue;
		private readonly int ordinalValue;
		private static int nextOrdinal = 0;

		internal string title;

		private InputAccessMode(string name, InnerEnum innerEnum, string s)
		{
			title = s;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		public static InputAccessMode getAccessMode(string accessMode)
		{
			if (TransferFromInputSpace.title.Equals(accessMode, StringComparison.InvariantCultureIgnoreCase))
			{
				return TransferFromInputSpace;
			}
			else if (TransferFromOutputSpace.title.Equals(accessMode, StringComparison.InvariantCultureIgnoreCase))
			{
				return TransferFromOutputSpace;
			}
			else if (TransferFromGlobalSpace.title.Equals(accessMode, StringComparison.InvariantCultureIgnoreCase))
			{
				return TransferFromGlobalSpace;
			}
			else if (TransferFromUserSpace.title.Equals(accessMode, StringComparison.InvariantCultureIgnoreCase))
			{
				return TransferFromUserSpace;
			}
			else if (CacheFromInputSpace.title.Equals(accessMode, StringComparison.InvariantCultureIgnoreCase))
			{
				return CacheFromInputSpace;
			}
			else if (CacheFromOutputSpace.title.Equals(accessMode, StringComparison.InvariantCultureIgnoreCase))
			{
				return CacheFromOutputSpace;
			}
			else if (CacheFromGlobalSpace.title.Equals(accessMode, StringComparison.InvariantCultureIgnoreCase))
			{
				return CacheFromGlobalSpace;
			}
			else if (CacheFromUserSpace.title.Equals(accessMode, StringComparison.InvariantCultureIgnoreCase))
			{
				return CacheFromUserSpace;
			}
			else if (none.title.Equals(accessMode, StringComparison.InvariantCultureIgnoreCase))
			{
				return none;
			}
			else
			{
				throw new System.ArgumentException("Unknow input access mode : " + accessMode);
			}
		}

		public override string ToString()
		{
			return this.title;
		}

		public static InputAccessMode[] values()
		{
			return valueList.ToArray();
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static InputAccessMode valueOf(string name)
		{
			foreach (InputAccessMode enumInstance in InputAccessMode.valueList)
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