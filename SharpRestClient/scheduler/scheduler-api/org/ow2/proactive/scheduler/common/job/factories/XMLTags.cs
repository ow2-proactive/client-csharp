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


	/// <summary>
	/// XMLTags defines all tags that can be used in an XML job descriptor.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 2.1
	/// </summary>
	public sealed class XMLTags
	{

		// Only needed tags are defined. If more are required, just create them.

		// JOBS
		public static readonly XMLTags JOB = new XMLTags("JOB", InnerEnum.JOB, "job");
		public static readonly XMLTags JOB_CLASSPATHES = new XMLTags("JOB_CLASSPATHES", InnerEnum.JOB_CLASSPATHES, "jobClasspath");
		public static readonly XMLTags JOB_PATH_ELEMENT = new XMLTags("JOB_PATH_ELEMENT", InnerEnum.JOB_PATH_ELEMENT, "pathElement");
		public static readonly XMLTags TASK_FLOW = new XMLTags("TASK_FLOW", InnerEnum.TASK_FLOW, "taskFlow");

		// COMMON
		public static readonly XMLTags COMMON_DESCRIPTION = new XMLTags("COMMON_DESCRIPTION", InnerEnum.COMMON_DESCRIPTION, "description");
		public static readonly XMLTags COMMON_GENERIC_INFORMATION = new XMLTags("COMMON_GENERIC_INFORMATION", InnerEnum.COMMON_GENERIC_INFORMATION, "genericInformation");
		public static readonly XMLTags COMMON_INFO = new XMLTags("COMMON_INFO", InnerEnum.COMMON_INFO, "info");

		// VARIABLES
		public static readonly XMLTags VARIABLE = new XMLTags("VARIABLE", InnerEnum.VARIABLE, "variable");
		public static readonly XMLTags VARIABLES = new XMLTags("VARIABLES", InnerEnum.VARIABLES, "variables");

		// TASKS
		public static readonly XMLTags JAVA_EXECUTABLE = new XMLTags("JAVA_EXECUTABLE", InnerEnum.JAVA_EXECUTABLE, "javaExecutable");
		public static readonly XMLTags NATIVE_EXECUTABLE = new XMLTags("NATIVE_EXECUTABLE", InnerEnum.NATIVE_EXECUTABLE, "nativeExecutable");
		public static readonly XMLTags NATIVE_TASK_ARGUMENT = new XMLTags("NATIVE_TASK_ARGUMENT", InnerEnum.NATIVE_TASK_ARGUMENT, "argument");
		public static readonly XMLTags NATIVE_TASK_ARGUMENTS = new XMLTags("NATIVE_TASK_ARGUMENTS", InnerEnum.NATIVE_TASK_ARGUMENTS, "arguments");
		public static readonly XMLTags NATIVE_TASK_STATIC_COMMAND = new XMLTags("NATIVE_TASK_STATIC_COMMAND", InnerEnum.NATIVE_TASK_STATIC_COMMAND, "staticCommand");
		public static readonly XMLTags SCRIPT_EXECUTABLE = new XMLTags("SCRIPT_EXECUTABLE", InnerEnum.SCRIPT_EXECUTABLE, "scriptExecutable");
		public static readonly XMLTags TASK = new XMLTags("TASK", InnerEnum.TASK, "task");
		public static readonly XMLTags TASK_DEPENDENCES = new XMLTags("TASK_DEPENDENCES", InnerEnum.TASK_DEPENDENCES, "depends");
		public static readonly XMLTags TASK_DEPENDENCES_TASK = new XMLTags("TASK_DEPENDENCES_TASK", InnerEnum.TASK_DEPENDENCES_TASK, "task");
		public static readonly XMLTags TASK_PARAMETER = new XMLTags("TASK_PARAMETER", InnerEnum.TASK_PARAMETER, "parameter");
		public static readonly XMLTags TASK_PARAMETERS = new XMLTags("TASK_PARAMETERS", InnerEnum.TASK_PARAMETERS, "parameters");

		// TOPOLOGY
		public static readonly XMLTags PARALLEL_ENV = new XMLTags("PARALLEL_ENV", InnerEnum.PARALLEL_ENV, "parallel");
		public static readonly XMLTags TOPOLOGY = new XMLTags("TOPOLOGY", InnerEnum.TOPOLOGY, "topology");
		public static readonly XMLTags TOPOLOGY_ARBITRARY = new XMLTags("TOPOLOGY_ARBITRARY", InnerEnum.TOPOLOGY_ARBITRARY, "arbitrary");
		public static readonly XMLTags TOPOLOGY_BEST_PROXIMITY = new XMLTags("TOPOLOGY_BEST_PROXIMITY", InnerEnum.TOPOLOGY_BEST_PROXIMITY, "bestProximity");
		public static readonly XMLTags TOPOLOGY_DIFFERENT_HOSTS_EXCLUSIVE = new XMLTags("TOPOLOGY_DIFFERENT_HOSTS_EXCLUSIVE", InnerEnum.TOPOLOGY_DIFFERENT_HOSTS_EXCLUSIVE, "differentHostsExclusive");
		public static readonly XMLTags TOPOLOGY_MULTIPLE_HOSTS_EXCLUSIVE = new XMLTags("TOPOLOGY_MULTIPLE_HOSTS_EXCLUSIVE", InnerEnum.TOPOLOGY_MULTIPLE_HOSTS_EXCLUSIVE, "multipleHostsExclusive");
		public static readonly XMLTags TOPOLOGY_SINGLE_HOST = new XMLTags("TOPOLOGY_SINGLE_HOST", InnerEnum.TOPOLOGY_SINGLE_HOST, "singleHost");
		public static readonly XMLTags TOPOLOGY_SINGLE_HOST_EXCLUSIVE = new XMLTags("TOPOLOGY_SINGLE_HOST_EXCLUSIVE", InnerEnum.TOPOLOGY_SINGLE_HOST_EXCLUSIVE, "singleHostExclusive");
		public static readonly XMLTags TOPOLOGY_THRESHOLD_PROXIMITY = new XMLTags("TOPOLOGY_THRESHOLD_PROXIMITY", InnerEnum.TOPOLOGY_THRESHOLD_PROXIMITY, "thresholdProximity");

		// SCRIPTS
		public static readonly XMLTags SCRIPT_ARGUMENT = new XMLTags("SCRIPT_ARGUMENT", InnerEnum.SCRIPT_ARGUMENT, "argument");
		public static readonly XMLTags SCRIPT_ARGUMENTS = new XMLTags("SCRIPT_ARGUMENTS", InnerEnum.SCRIPT_ARGUMENTS, "arguments");
		public static readonly XMLTags SCRIPT_CLEANING = new XMLTags("SCRIPT_CLEANING", InnerEnum.SCRIPT_CLEANING, "cleaning");
		public static readonly XMLTags SCRIPT_CODE = new XMLTags("SCRIPT_CODE", InnerEnum.SCRIPT_CODE, "code");
		public static readonly XMLTags SCRIPT_FILE = new XMLTags("SCRIPT_FILE", InnerEnum.SCRIPT_FILE, "file");
		public static readonly XMLTags SCRIPT_PRE = new XMLTags("SCRIPT_PRE", InnerEnum.SCRIPT_PRE, "pre");
		public static readonly XMLTags SCRIPT_POST = new XMLTags("SCRIPT_POST", InnerEnum.SCRIPT_POST, "post");
		public static readonly XMLTags SCRIPT_SCRIPT = new XMLTags("SCRIPT_SCRIPT", InnerEnum.SCRIPT_SCRIPT, "script");
		public static readonly XMLTags SCRIPT_SELECTION = new XMLTags("SCRIPT_SELECTION", InnerEnum.SCRIPT_SELECTION, "selection");

		// FORK ENVIRONMENT
		public static readonly XMLTags FORK_ADDITIONAL_CLASSPATH = new XMLTags("FORK_ADDITIONAL_CLASSPATH", InnerEnum.FORK_ADDITIONAL_CLASSPATH, "additionalClasspath");
		public static readonly XMLTags FORK_ENVIRONMENT = new XMLTags("FORK_ENVIRONMENT", InnerEnum.FORK_ENVIRONMENT, "forkEnvironment");
		public static readonly XMLTags FORK_JVM_ARG = new XMLTags("FORK_JVM_ARG", InnerEnum.FORK_JVM_ARG, "jvmArg");
		public static readonly XMLTags FORK_JVM_ARGS = new XMLTags("FORK_JVM_ARGS", InnerEnum.FORK_JVM_ARGS, "jvmArgs");
		public static readonly XMLTags FORK_PATH_ELEMENT = new XMLTags("FORK_PATH_ELEMENT", InnerEnum.FORK_PATH_ELEMENT, "pathElement");
		public static readonly XMLTags FORK_SYSTEM_PROPERTIES = new XMLTags("FORK_SYSTEM_PROPERTIES", InnerEnum.FORK_SYSTEM_PROPERTIES, "SystemEnvironment");
		public static readonly XMLTags FORK_SYSTEM_PROPERTY = new XMLTags("FORK_SYSTEM_PROPERTY", InnerEnum.FORK_SYSTEM_PROPERTY, "variable");
		public static readonly XMLTags SCRIPT_ENV = new XMLTags("SCRIPT_ENV", InnerEnum.SCRIPT_ENV, "envScript");

		// FLOW CONTROL
		public static readonly XMLTags FLOW = new XMLTags("FLOW", InnerEnum.FLOW, "controlFlow");
		public static readonly XMLTags FLOW_IF = new XMLTags("FLOW_IF", InnerEnum.FLOW_IF, "if");
		public static readonly XMLTags FLOW_LOOP = new XMLTags("FLOW_LOOP", InnerEnum.FLOW_LOOP, "loop");
		public static readonly XMLTags FLOW_REPLICATE = new XMLTags("FLOW_REPLICATE", InnerEnum.FLOW_REPLICATE, "replicate");

		// DATASPACES
		public static readonly XMLTags DS_FILES = new XMLTags("DS_FILES", InnerEnum.DS_FILES, "files");
		public static readonly XMLTags DS_GLOBAL_SPACE = new XMLTags("DS_GLOBAL_SPACE", InnerEnum.DS_GLOBAL_SPACE, "globalSpace");
		public static readonly XMLTags DS_INPUT_FILES = new XMLTags("DS_INPUT_FILES", InnerEnum.DS_INPUT_FILES, "inputFiles");
		public static readonly XMLTags DS_INPUT_SPACE = new XMLTags("DS_INPUT_SPACE", InnerEnum.DS_INPUT_SPACE, "inputSpace");
		public static readonly XMLTags DS_OUTPUT_FILES = new XMLTags("DS_OUTPUT_FILES", InnerEnum.DS_OUTPUT_FILES, "outputFiles");
		public static readonly XMLTags DS_OUTPUT_SPACE = new XMLTags("DS_OUTPUT_SPACE", InnerEnum.DS_OUTPUT_SPACE, "outputSpace");
		public static readonly XMLTags DS_USER_SPACE = new XMLTags("DS_USER_SPACE", InnerEnum.DS_USER_SPACE, "userSpace");

		// METADATA
		public static readonly XMLTags METADATA = new XMLTags("METADATA", InnerEnum.METADATA, "metadata");
		public static readonly XMLTags METADATA_VISUALIZATION = new XMLTags("METADATA_VISUALIZATION", InnerEnum.METADATA_VISUALIZATION, "visualization");

		private static readonly List<XMLTags> valueList = new List<XMLTags>();

		static XMLTags()
		{
			valueList.Add(JOB);
			valueList.Add(JOB_CLASSPATHES);
			valueList.Add(JOB_PATH_ELEMENT);
			valueList.Add(TASK_FLOW);
			valueList.Add(COMMON_DESCRIPTION);
			valueList.Add(COMMON_GENERIC_INFORMATION);
			valueList.Add(COMMON_INFO);
			valueList.Add(VARIABLE);
			valueList.Add(VARIABLES);
			valueList.Add(JAVA_EXECUTABLE);
			valueList.Add(NATIVE_EXECUTABLE);
			valueList.Add(NATIVE_TASK_ARGUMENT);
			valueList.Add(NATIVE_TASK_ARGUMENTS);
			valueList.Add(NATIVE_TASK_STATIC_COMMAND);
			valueList.Add(SCRIPT_EXECUTABLE);
			valueList.Add(TASK);
			valueList.Add(TASK_DEPENDENCES);
			valueList.Add(TASK_DEPENDENCES_TASK);
			valueList.Add(TASK_PARAMETER);
			valueList.Add(TASK_PARAMETERS);
			valueList.Add(PARALLEL_ENV);
			valueList.Add(TOPOLOGY);
			valueList.Add(TOPOLOGY_ARBITRARY);
			valueList.Add(TOPOLOGY_BEST_PROXIMITY);
			valueList.Add(TOPOLOGY_DIFFERENT_HOSTS_EXCLUSIVE);
			valueList.Add(TOPOLOGY_MULTIPLE_HOSTS_EXCLUSIVE);
			valueList.Add(TOPOLOGY_SINGLE_HOST);
			valueList.Add(TOPOLOGY_SINGLE_HOST_EXCLUSIVE);
			valueList.Add(TOPOLOGY_THRESHOLD_PROXIMITY);
			valueList.Add(SCRIPT_ARGUMENT);
			valueList.Add(SCRIPT_ARGUMENTS);
			valueList.Add(SCRIPT_CLEANING);
			valueList.Add(SCRIPT_CODE);
			valueList.Add(SCRIPT_FILE);
			valueList.Add(SCRIPT_PRE);
			valueList.Add(SCRIPT_POST);
			valueList.Add(SCRIPT_SCRIPT);
			valueList.Add(SCRIPT_SELECTION);
			valueList.Add(FORK_ADDITIONAL_CLASSPATH);
			valueList.Add(FORK_ENVIRONMENT);
			valueList.Add(FORK_JVM_ARG);
			valueList.Add(FORK_JVM_ARGS);
			valueList.Add(FORK_PATH_ELEMENT);
			valueList.Add(FORK_SYSTEM_PROPERTIES);
			valueList.Add(FORK_SYSTEM_PROPERTY);
			valueList.Add(SCRIPT_ENV);
			valueList.Add(FLOW);
			valueList.Add(FLOW_IF);
			valueList.Add(FLOW_LOOP);
			valueList.Add(FLOW_REPLICATE);
			valueList.Add(DS_FILES);
			valueList.Add(DS_GLOBAL_SPACE);
			valueList.Add(DS_INPUT_FILES);
			valueList.Add(DS_INPUT_SPACE);
			valueList.Add(DS_OUTPUT_FILES);
			valueList.Add(DS_OUTPUT_SPACE);
			valueList.Add(DS_USER_SPACE);
			valueList.Add(METADATA);
			valueList.Add(METADATA_VISUALIZATION);
		}

		public enum InnerEnum
		{
			JOB,
			JOB_CLASSPATHES,
			JOB_PATH_ELEMENT,
			TASK_FLOW,
			COMMON_DESCRIPTION,
			COMMON_GENERIC_INFORMATION,
			COMMON_INFO,
			VARIABLE,
			VARIABLES,
			JAVA_EXECUTABLE,
			NATIVE_EXECUTABLE,
			NATIVE_TASK_ARGUMENT,
			NATIVE_TASK_ARGUMENTS,
			NATIVE_TASK_STATIC_COMMAND,
			SCRIPT_EXECUTABLE,
			TASK,
			TASK_DEPENDENCES,
			TASK_DEPENDENCES_TASK,
			TASK_PARAMETER,
			TASK_PARAMETERS,
			PARALLEL_ENV,
			TOPOLOGY,
			TOPOLOGY_ARBITRARY,
			TOPOLOGY_BEST_PROXIMITY,
			TOPOLOGY_DIFFERENT_HOSTS_EXCLUSIVE,
			TOPOLOGY_MULTIPLE_HOSTS_EXCLUSIVE,
			TOPOLOGY_SINGLE_HOST,
			TOPOLOGY_SINGLE_HOST_EXCLUSIVE,
			TOPOLOGY_THRESHOLD_PROXIMITY,
			SCRIPT_ARGUMENT,
			SCRIPT_ARGUMENTS,
			SCRIPT_CLEANING,
			SCRIPT_CODE,
			SCRIPT_FILE,
			SCRIPT_PRE,
			SCRIPT_POST,
			SCRIPT_SCRIPT,
			SCRIPT_SELECTION,
			FORK_ADDITIONAL_CLASSPATH,
			FORK_ENVIRONMENT,
			FORK_JVM_ARG,
			FORK_JVM_ARGS,
			FORK_PATH_ELEMENT,
			FORK_SYSTEM_PROPERTIES,
			FORK_SYSTEM_PROPERTY,
			SCRIPT_ENV,
			FLOW,
			FLOW_IF,
			FLOW_LOOP,
			FLOW_REPLICATE,
			DS_FILES,
			DS_GLOBAL_SPACE,
			DS_INPUT_FILES,
			DS_INPUT_SPACE,
			DS_OUTPUT_FILES,
			DS_OUTPUT_SPACE,
			DS_USER_SPACE,
			METADATA,
			METADATA_VISUALIZATION
		}

		public readonly InnerEnum innerEnumValue;
		private readonly string nameValue;
		private readonly int ordinalValue;
		private static int nextOrdinal = 0;

		/// <summary>
		/// pattern that matches for open tag for provided tag name.
		/// e.g.: <code>String.format(OPEN_TAG_PATTERN, XMLTags.VARIABLES</code> seeks
		/// for the string like this: &lt;variables&gt;, &lt;   variables&gt;, &lt;variables   &gt;, etc.
		/// </summary>
		public const string OPEN_TAG_PATTERN = "<[ ]*%s[^>]*>";

		/// <summary>
		/// pattern that matches for open tag for provided tag name.
		/// e.g.: <code>String.format(CLOSE_TAG_PATTERN, XMLTags.VARIABLES</code> seeks
		/// for the string like this: &lt;/variables&gt;, &lt;variables/&gt; &lt;/   variables&gt;, &lt;  /  variables   &gt;, etc.
		/// </summary>
		public const string CLOSE_TAG_PATTERN = "<[ ]*/[ ]*%s[ ]*>|<[ ]*%s[ ]*/[ ]*>";

		private string xmlName;

		internal XMLTags(string name, InnerEnum innerEnum, string xmlName)
		{
			this.xmlName = xmlName;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		/// <summary>
		/// Return the XML tag name of the element as a String.
		/// </summary>
		/// <returns> the XML tag name of the element as a String. </returns>
		public string XMLName
		{
			get
			{
				return this.xmlName;
			}
		}

		public string OpenTagPattern
		{
			get
			{
				return string.Format(OPEN_TAG_PATTERN, this.xmlName);
			}
		}

		public string CloseTagPattern
		{
			get
			{
				return string.Format(CLOSE_TAG_PATTERN, this.xmlName, this.xmlName);
			}
		}

		private static IDictionary<string, XMLTags> namesToEnum = null;

		/// <summary>
		/// Get the XMLTags enum corresponding to the given xml tag name. This method
		/// ignores the case. Argument cannot be {@code null}.
		/// </summary>
		/// <param name="xmlName"> the XML tag name as a string. </param>
		/// <returns> the corresponding XML tag. </returns>
		/// <exception cref="IllegalArgumentException"> if the tag name does not exist. </exception>
		public static XMLTags getFromXMLName(string xmlName)
		{
			if (xmlName == null)
			{
				throw new ArgumentNullException("xmlName");
			}

			string toCheck = xmlName.ToUpper();

			if (namesToEnum == null)
			{
				IDictionary<string, XMLTags> result = new Dictionary<string, XMLTags>(valueList.Count);

				foreach (XMLTags atag in valueList)
				{
					result[atag.XMLName.ToUpper()] = atag;
				}

				namesToEnum = result;
			}

			XMLTags tag = namesToEnum[toCheck];

			if (tag == null)
			{
				throw new System.ArgumentException("XML tag name '" + xmlName + "' does not exist");
			}
			else
			{
				return tag;
			}
		}

		/// <summary>
		/// Return true if the given XML name matches this XMLAttributes
		/// </summary>
		/// <param name="xmlName"> the XML tag name as a String. </param>
		/// <returns> true only if the given XML name matches this XMLTags </returns>
		public bool matches(string xmlName)
		{
			return xmlName.Equals(this.xmlName, StringComparison.OrdinalIgnoreCase);
		}

		public string withContent(string content)
		{
			return "  <" + this.xmlName + ">\n" + content + "  </" + this.xmlName + ">";
		}

		/// <summary>
		/// {@inheritDoc}
		/// </summary>
		public override string ToString()
		{
			return XMLName;
		}


		public static XMLTags[] values()
		{
			return valueList.ToArray();
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static XMLTags valueOf(string name)
		{
			foreach (XMLTags enumInstance in XMLTags.valueList)
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