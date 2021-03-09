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
namespace org.ow2.proactive.utils
{
	/// <summary>
	/// OperatingSystem
	/// 
	/// @author The ProActive Team
	/// </summary>
	public sealed class OperatingSystem
	{
		public static readonly OperatingSystem MAC_OSX = new OperatingSystem("MAC_OSX", InnerEnum.MAC_OSX, "Mac OS X", OperatingSystemFamily.MAC);
		public static readonly OperatingSystem WINDOWS_95 = new OperatingSystem("WINDOWS_95", InnerEnum.WINDOWS_95, "Windows 95", OperatingSystemFamily.WINDOWS);
		public static readonly OperatingSystem WINDOWS_98 = new OperatingSystem("WINDOWS_98", InnerEnum.WINDOWS_98, "Windows 98", OperatingSystemFamily.WINDOWS);
		public static readonly OperatingSystem WINDOWS_ME = new OperatingSystem("WINDOWS_ME", InnerEnum.WINDOWS_ME, "Windows Me", OperatingSystemFamily.WINDOWS);
		public static readonly OperatingSystem WINDOWS_NT = new OperatingSystem("WINDOWS_NT", InnerEnum.WINDOWS_NT, "Windows NT", OperatingSystemFamily.WINDOWS);
		public static readonly OperatingSystem WINDOWS_2000 = new OperatingSystem("WINDOWS_2000", InnerEnum.WINDOWS_2000, "Windows 2000", OperatingSystemFamily.WINDOWS);
		public static readonly OperatingSystem WINDOWS_XP = new OperatingSystem("WINDOWS_XP", InnerEnum.WINDOWS_XP, "Windows XP", OperatingSystemFamily.WINDOWS);
		public static readonly OperatingSystem WINDOWS_7 = new OperatingSystem("WINDOWS_7", InnerEnum.WINDOWS_7, "Windows 7", OperatingSystemFamily.WINDOWS);
		public static readonly OperatingSystem WINDOWS_10 = new OperatingSystem("WINDOWS_10", InnerEnum.WINDOWS_10, "Windows 10", OperatingSystemFamily.WINDOWS);
		public static readonly OperatingSystem WINDOWS_2003 = new OperatingSystem("WINDOWS_2003", InnerEnum.WINDOWS_2003, "Windows 2003", OperatingSystemFamily.WINDOWS);
		public static readonly OperatingSystem WINDOWS_2008 = new OperatingSystem("WINDOWS_2008", InnerEnum.WINDOWS_2008, "Windows 2008", OperatingSystemFamily.WINDOWS);
		public static readonly OperatingSystem SUN_OS = new OperatingSystem("SUN_OS", InnerEnum.SUN_OS, "SunOS", OperatingSystemFamily.UNIX);
		public static readonly OperatingSystem MPE_IX = new OperatingSystem("MPE_IX", InnerEnum.MPE_IX, "MPE/iX", OperatingSystemFamily.UNIX);
		public static readonly OperatingSystem HP_UX = new OperatingSystem("HP_UX", InnerEnum.HP_UX, "HP-UX", OperatingSystemFamily.UNIX);
		public static readonly OperatingSystem AIX = new OperatingSystem("AIX", InnerEnum.AIX, "AIX", OperatingSystemFamily.UNIX);
		public static readonly OperatingSystem OS_390 = new OperatingSystem("OS_390", InnerEnum.OS_390, "OS/390", OperatingSystemFamily.UNIX);
		public static readonly OperatingSystem zOS = new OperatingSystem("zOS", InnerEnum.zOS, "z/OS", OperatingSystemFamily.UNIX);
		public static readonly OperatingSystem FREEBSD = new OperatingSystem("FREEBSD", InnerEnum.FREEBSD, "FreeBSD", OperatingSystemFamily.UNIX);
		public static readonly OperatingSystem IRIX = new OperatingSystem("IRIX", InnerEnum.IRIX, "Irix", OperatingSystemFamily.UNIX);
		public static readonly OperatingSystem DIGITAL_UNIX = new OperatingSystem("DIGITAL_UNIX", InnerEnum.DIGITAL_UNIX, "Digital Unix", OperatingSystemFamily.UNIX);
		public static readonly OperatingSystem NETWARE = new OperatingSystem("NETWARE", InnerEnum.NETWARE, "NetWare", OperatingSystemFamily.UNIX);
		public static readonly OperatingSystem OSF1 = new OperatingSystem("OSF1", InnerEnum.OSF1, "OSF1", OperatingSystemFamily.UNIX);
		public static readonly OperatingSystem OPENVMS = new OperatingSystem("OPENVMS", InnerEnum.OPENVMS, "OpenVMS", OperatingSystemFamily.DEC_OS);
		public static readonly OperatingSystem WINDOWS_GENERIC = new OperatingSystem("WINDOWS_GENERIC", InnerEnum.WINDOWS_GENERIC, "Windows", OperatingSystemFamily.WINDOWS);
		public static readonly OperatingSystem LINUX_OS = new OperatingSystem("LINUX_OS", InnerEnum.LINUX_OS, "Linux", OperatingSystemFamily.LINUX);
		public static readonly OperatingSystem MAC_OS = new OperatingSystem("MAC_OS", InnerEnum.MAC_OS, "Mac OS", OperatingSystemFamily.MAC);

		private static readonly List<OperatingSystem> valueList = new List<OperatingSystem>();

		static OperatingSystem()
		{
			valueList.Add(MAC_OSX);
			valueList.Add(WINDOWS_95);
			valueList.Add(WINDOWS_98);
			valueList.Add(WINDOWS_ME);
			valueList.Add(WINDOWS_NT);
			valueList.Add(WINDOWS_2000);
			valueList.Add(WINDOWS_XP);
			valueList.Add(WINDOWS_7);
			valueList.Add(WINDOWS_10);
			valueList.Add(WINDOWS_2003);
			valueList.Add(WINDOWS_2008);
			valueList.Add(SUN_OS);
			valueList.Add(MPE_IX);
			valueList.Add(HP_UX);
			valueList.Add(AIX);
			valueList.Add(OS_390);
			valueList.Add(zOS);
			valueList.Add(FREEBSD);
			valueList.Add(IRIX);
			valueList.Add(DIGITAL_UNIX);
			valueList.Add(NETWARE);
			valueList.Add(OSF1);
			valueList.Add(OPENVMS);
			valueList.Add(WINDOWS_GENERIC);
			valueList.Add(LINUX_OS);
			valueList.Add(MAC_OS);
		}

		public enum InnerEnum
		{
			MAC_OSX,
			WINDOWS_95,
			WINDOWS_98,
			WINDOWS_ME,
			WINDOWS_NT,
			WINDOWS_2000,
			WINDOWS_XP,
			WINDOWS_7,
			WINDOWS_10,
			WINDOWS_2003,
			WINDOWS_2008,
			SUN_OS,
			MPE_IX,
			HP_UX,
			AIX,
			OS_390,
			zOS,
			FREEBSD,
			IRIX,
			DIGITAL_UNIX,
			NETWARE,
			OSF1,
			OPENVMS,
			WINDOWS_GENERIC,
			LINUX_OS,
			MAC_OS
		}

		public readonly InnerEnum innerEnumValue;
		private readonly string nameValue;
		private readonly int ordinalValue;
		private static int nextOrdinal = 0;

		private readonly string label;

		private readonly OperatingSystemFamily family;

		private OperatingSystem(string name, InnerEnum innerEnum, string label, OperatingSystemFamily family)
		{
			this.label = label;
			this.family = family;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		public string Label
		{
			get
			{
				return label;
			}
		}

		public OperatingSystemFamily Family
		{
			get
			{
				return family;
			}
		}

		public static OperatingSystem resolve(string osName)
		{
			foreach (OperatingSystem os in OperatingSystem.values())
			{
				if (osName.StartsWith(os.label, StringComparison.Ordinal))
				{
					return os;
				}
			}
			return null;
		}

		public static OperatingSystem resolveOrError(string osName)
		{
			foreach (OperatingSystem os in OperatingSystem.values())
			{
				if (osName.StartsWith(os.label, StringComparison.Ordinal))
				{
					return os;
				}
			}
			throw new System.ArgumentException("Unknown Operating System " + osName);
		}


		public static OperatingSystem[] values()
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

		public static OperatingSystem valueOf(string name)
		{
			foreach (OperatingSystem enumInstance in OperatingSystem.valueList)
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