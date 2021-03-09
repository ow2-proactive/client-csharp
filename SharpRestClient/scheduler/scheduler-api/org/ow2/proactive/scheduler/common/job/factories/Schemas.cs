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
namespace org.ow2.proactive.scheduler.common.job.factories
{


	public sealed class Schemas
	{

		public static readonly Schemas SCHEMA_3_0 = new Schemas("SCHEMA_3_0", InnerEnum.SCHEMA_3_0, "/org/ow2/proactive/scheduler/common/xml/schemas/jobdescriptor/3.0/schedulerjob.rng", "urn:proactive:jobdescriptor:3.0");
		public static readonly Schemas SCHEMA_3_1 = new Schemas("SCHEMA_3_1", InnerEnum.SCHEMA_3_1, "/org/ow2/proactive/scheduler/common/xml/schemas/jobdescriptor/3.1/schedulerjob.rng", "urn:proactive:jobdescriptor:3.1");
		public static readonly Schemas SCHEMA_3_2 = new Schemas("SCHEMA_3_2", InnerEnum.SCHEMA_3_2, "/org/ow2/proactive/scheduler/common/xml/schemas/jobdescriptor/3.2/schedulerjob.rng", "urn:proactive:jobdescriptor:3.2");
		public static readonly Schemas SCHEMA_3_3 = new Schemas("SCHEMA_3_3", InnerEnum.SCHEMA_3_3, "/org/ow2/proactive/scheduler/common/xml/schemas/jobdescriptor/3.3/schedulerjob.rng", "urn:proactive:jobdescriptor:3.3");
		public static readonly Schemas SCHEMA_3_4 = new Schemas("SCHEMA_3_4", InnerEnum.SCHEMA_3_4, "/org/ow2/proactive/scheduler/common/xml/schemas/jobdescriptor/3.4/schedulerjob.rng", "urn:proactive:jobdescriptor:3.4");
		public static readonly Schemas SCHEMA_3_5 = new Schemas("SCHEMA_3_5", InnerEnum.SCHEMA_3_5, "/org/ow2/proactive/scheduler/common/xml/schemas/jobdescriptor/3.5/schedulerjob.rng", "urn:proactive:jobdescriptor:3.5");
		public static readonly Schemas SCHEMA_3_6 = new Schemas("SCHEMA_3_6", InnerEnum.SCHEMA_3_6, "/org/ow2/proactive/scheduler/common/xml/schemas/jobdescriptor/3.6/schedulerjob.rng", "urn:proactive:jobdescriptor:3.6");
		public static readonly Schemas SCHEMA_3_7 = new Schemas("SCHEMA_3_7", InnerEnum.SCHEMA_3_7, "/org/ow2/proactive/scheduler/common/xml/schemas/jobdescriptor/3.7/schedulerjob.rng", "urn:proactive:jobdescriptor:3.7");
		public static readonly Schemas SCHEMA_3_8 = new Schemas("SCHEMA_3_8", InnerEnum.SCHEMA_3_8, "/org/ow2/proactive/scheduler/common/xml/schemas/jobdescriptor/3.8/schedulerjob.rng", "urn:proactive:jobdescriptor:3.8");
		public static readonly Schemas SCHEMA_3_9 = new Schemas("SCHEMA_3_9", InnerEnum.SCHEMA_3_9, "/org/ow2/proactive/scheduler/common/xml/schemas/jobdescriptor/3.9/schedulerjob.rng", "urn:proactive:jobdescriptor:3.9");
		public static readonly Schemas SCHEMA_3_10 = new Schemas("SCHEMA_3_10", InnerEnum.SCHEMA_3_10, "/org/ow2/proactive/scheduler/common/xml/schemas/jobdescriptor/3.10/schedulerjob.rng", "urn:proactive:jobdescriptor:3.10");
		public static readonly Schemas SCHEMA_3_11 = new Schemas("SCHEMA_3_11", InnerEnum.SCHEMA_3_11, "/org/ow2/proactive/scheduler/common/xml/schemas/jobdescriptor/3.11/schedulerjob.rng", "urn:proactive:jobdescriptor:3.11");
		public static readonly Schemas SCHEMA_3_12 = new Schemas("SCHEMA_3_12", InnerEnum.SCHEMA_3_12, "/org/ow2/proactive/scheduler/common/xml/schemas/jobdescriptor/3.12/schedulerjob.rng", "urn:proactive:jobdescriptor:3.12");
		public static readonly Schemas SCHEMA_DEV = new Schemas("SCHEMA_DEV", InnerEnum.SCHEMA_DEV, "/org/ow2/proactive/scheduler/common/xml/schemas/jobdescriptor/dev/schedulerjob.rng", "urn:proactive:jobdescriptor:dev");

		// should contain a reference to the last one declared, see #validate
		public static readonly Schemas SCHEMA_LATEST = new Schemas("SCHEMA_LATEST", InnerEnum.SCHEMA_LATEST, SCHEMA_3_12.location, SCHEMA_3_12.@namespace);

		private static readonly List<Schemas> valueList = new List<Schemas>();

		public enum InnerEnum
		{
			SCHEMA_3_0,
			SCHEMA_3_1,
			SCHEMA_3_2,
			SCHEMA_3_3,
			SCHEMA_3_4,
			SCHEMA_3_5,
			SCHEMA_3_6,
			SCHEMA_3_7,
			SCHEMA_3_8,
			SCHEMA_3_9,
			SCHEMA_3_10,
			SCHEMA_3_11,
			SCHEMA_3_12,
			SCHEMA_DEV,
			SCHEMA_LATEST
		}

		public readonly InnerEnum innerEnumValue;
		private readonly string nameValue;
		private readonly int ordinalValue;
		private static int nextOrdinal = 0;

		private string location;

		private string @namespace;

		private static readonly IDictionary<string, Schemas> SCHEMAS_BY_NAMESPACE;

		static Schemas()
		{
			SCHEMAS_BY_NAMESPACE = new Dictionary<string, Schemas>(Schemas.values().Length);

			foreach (Schemas schema in Schemas.values())
			{
				SCHEMAS_BY_NAMESPACE[schema.@namespace] = schema;
			}

			valueList.Add(SCHEMA_3_0);
			valueList.Add(SCHEMA_3_1);
			valueList.Add(SCHEMA_3_2);
			valueList.Add(SCHEMA_3_3);
			valueList.Add(SCHEMA_3_4);
			valueList.Add(SCHEMA_3_5);
			valueList.Add(SCHEMA_3_6);
			valueList.Add(SCHEMA_3_7);
			valueList.Add(SCHEMA_3_8);
			valueList.Add(SCHEMA_3_9);
			valueList.Add(SCHEMA_3_10);
			valueList.Add(SCHEMA_3_11);
			valueList.Add(SCHEMA_3_12);
			valueList.Add(SCHEMA_DEV);
			valueList.Add(SCHEMA_LATEST);
		}

		internal Schemas(string name, InnerEnum innerEnum, string location, string @namespace)
		{
			this.location = location;
			this.@namespace = @namespace;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		public string Location
		{
			get
			{
				return location;
			}
		}

		public string Namespace
		{
			get
			{
				return @namespace;
			}
		}

		public static Schemas getSchemaByNamespace(string @namespace)
		{
			return SCHEMAS_BY_NAMESPACE[@namespace];
		}

		public string Version
		{
			get
			{
				return @namespace.Substring(@namespace.LastIndexOf(":", StringComparison.Ordinal) + 1);
			}
		}

		public static Schemas[] values()
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

		public static Schemas valueOf(string name)
		{
			foreach (Schemas enumInstance in Schemas.valueList)
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