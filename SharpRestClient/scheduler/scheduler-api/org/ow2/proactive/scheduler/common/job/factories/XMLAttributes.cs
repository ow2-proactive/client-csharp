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
	/// XMLAttributes defines attributes allowed in XML job descriptors.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 2.1
	/// </summary>
	public sealed class XMLAttributes
	{

		// JOBS
		public static readonly XMLAttributes JOB_PRIORITY = new XMLAttributes("JOB_PRIORITY", InnerEnum.JOB_PRIORITY, "priority");
		public static readonly XMLAttributes JOB_PROJECT_NAME = new XMLAttributes("JOB_PROJECT_NAME", InnerEnum.JOB_PROJECT_NAME, "projectName");

		// COMMON
		public static readonly XMLAttributes COMMON_CANCEL_JOB_ON_ERROR = new XMLAttributes("COMMON_CANCEL_JOB_ON_ERROR", InnerEnum.COMMON_CANCEL_JOB_ON_ERROR, "cancelJobOnError");
		public static readonly XMLAttributes COMMON_ON_TASK_ERROR = new XMLAttributes("COMMON_ON_TASK_ERROR", InnerEnum.COMMON_ON_TASK_ERROR, "onTaskError");
		public static readonly XMLAttributes COMMON_MAX_NUMBER_OF_EXECUTION = new XMLAttributes("COMMON_MAX_NUMBER_OF_EXECUTION", InnerEnum.COMMON_MAX_NUMBER_OF_EXECUTION, "maxNumberOfExecution");
		public static readonly XMLAttributes COMMON_NAME = new XMLAttributes("COMMON_NAME", InnerEnum.COMMON_NAME, "name");
		public static readonly XMLAttributes COMMON_RESTART_TASK_ON_ERROR = new XMLAttributes("COMMON_RESTART_TASK_ON_ERROR", InnerEnum.COMMON_RESTART_TASK_ON_ERROR, "restartTaskOnError");
		public static readonly XMLAttributes COMMON_TASK_RETRY_DELAY = new XMLAttributes("COMMON_TASK_RETRY_DELAY", InnerEnum.COMMON_TASK_RETRY_DELAY, "taskRetryDelay");
		public static readonly XMLAttributes COMMON_VALUE = new XMLAttributes("COMMON_VALUE", InnerEnum.COMMON_VALUE, "value");

		// VARIABLE
		public static readonly XMLAttributes VARIABLE_NAME = new XMLAttributes("VARIABLE_NAME", InnerEnum.VARIABLE_NAME, "name");
		public static readonly XMLAttributes VARIABLE_VALUE = new XMLAttributes("VARIABLE_VALUE", InnerEnum.VARIABLE_VALUE, "value");
		public static readonly XMLAttributes VARIABLE_MODEL = new XMLAttributes("VARIABLE_MODEL", InnerEnum.VARIABLE_MODEL, "model");
		public static readonly XMLAttributes VARIABLE_JOB_INHERITED = new XMLAttributes("VARIABLE_JOB_INHERITED", InnerEnum.VARIABLE_JOB_INHERITED, "inherited");

		// TASKS
		public static readonly XMLAttributes TASK_CLASS_NAME = new XMLAttributes("TASK_CLASS_NAME", InnerEnum.TASK_CLASS_NAME, "class");
		public static readonly XMLAttributes TASK_DEPENDS_REF = new XMLAttributes("TASK_DEPENDS_REF", InnerEnum.TASK_DEPENDS_REF, "ref");
		public static readonly XMLAttributes TASK_PRECIOUS_LOGS = new XMLAttributes("TASK_PRECIOUS_LOGS", InnerEnum.TASK_PRECIOUS_LOGS, "preciousLogs");
		public static readonly XMLAttributes TASK_PRECIOUS_RESULT = new XMLAttributes("TASK_PRECIOUS_RESULT", InnerEnum.TASK_PRECIOUS_RESULT, "preciousResult");
		public static readonly XMLAttributes TASK_RUN_AS_ME = new XMLAttributes("TASK_RUN_AS_ME", InnerEnum.TASK_RUN_AS_ME, "runAsMe");
		public static readonly XMLAttributes TASK_FORK = new XMLAttributes("TASK_FORK", InnerEnum.TASK_FORK, "fork");
		public static readonly XMLAttributes TASK_WALLTIME = new XMLAttributes("TASK_WALLTIME", InnerEnum.TASK_WALLTIME, "walltime");

		// NATIVE TASK ATTRIBUTES
		public static readonly XMLAttributes TASK_COMMAND_VALUE = new XMLAttributes("TASK_COMMAND_VALUE", InnerEnum.TASK_COMMAND_VALUE, "value");
		public static readonly XMLAttributes TASK_NB_NODES = new XMLAttributes("TASK_NB_NODES", InnerEnum.TASK_NB_NODES, "numberOfNodes");
		public static readonly XMLAttributes TASK_PARAMETER_NAME = new XMLAttributes("TASK_PARAMETER_NAME", InnerEnum.TASK_PARAMETER_NAME, "name");
		public static readonly XMLAttributes TASK_PARAMETER_VALUE = new XMLAttributes("TASK_PARAMETER_VALUE", InnerEnum.TASK_PARAMETER_VALUE, "value");
		public static readonly XMLAttributes TASK_WORKDING_DIR = new XMLAttributes("TASK_WORKDING_DIR", InnerEnum.TASK_WORKDING_DIR, "workingDir");

		// TOPOLOGY
		public static readonly XMLAttributes TOPOLOGY_THRESHOLD = new XMLAttributes("TOPOLOGY_THRESHOLD", InnerEnum.TOPOLOGY_THRESHOLD, "threshold");

		// SCRIPTS
		public static readonly XMLAttributes SCRIPT_URL = new XMLAttributes("SCRIPT_URL", InnerEnum.SCRIPT_URL, "url");

		// FORK ENVIRONMENT
		public static readonly XMLAttributes FORK_JAVA_HOME = new XMLAttributes("FORK_JAVA_HOME", InnerEnum.FORK_JAVA_HOME, "javaHome");

		// FLOW CONTROL
		public static readonly XMLAttributes FLOW_BLOCK = new XMLAttributes("FLOW_BLOCK", InnerEnum.FLOW_BLOCK, "block");
		public static readonly XMLAttributes FLOW_CONTINUATION = new XMLAttributes("FLOW_CONTINUATION", InnerEnum.FLOW_CONTINUATION, "continuation");
		public static readonly XMLAttributes FLOW_ELSE = new XMLAttributes("FLOW_ELSE", InnerEnum.FLOW_ELSE, "else");
		public static readonly XMLAttributes FLOW_TARGET = new XMLAttributes("FLOW_TARGET", InnerEnum.FLOW_TARGET, "target");

		// DATASPACES
		public static readonly XMLAttributes DS_ACCESS_MODE = new XMLAttributes("DS_ACCESS_MODE", InnerEnum.DS_ACCESS_MODE, "accessMode");
		public static readonly XMLAttributes DS_EXCLUDES = new XMLAttributes("DS_EXCLUDES", InnerEnum.DS_EXCLUDES, "excludes");
		public static readonly XMLAttributes DS_INCLUDES = new XMLAttributes("DS_INCLUDES", InnerEnum.DS_INCLUDES, "includes");
		public static readonly XMLAttributes DS_URL = new XMLAttributes("DS_URL", InnerEnum.DS_URL, "url");

		// NOT USED IN XML FACTORY BUT USED IN XML DESCRIPTION
		public static readonly XMLAttributes PATH = new XMLAttributes("PATH", InnerEnum.PATH, "path");
		public static readonly XMLAttributes LANGUAGE = new XMLAttributes("LANGUAGE", InnerEnum.LANGUAGE, "language");

		private static readonly List<XMLAttributes> valueList = new List<XMLAttributes>();

		static XMLAttributes()
		{
			valueList.Add(JOB_PRIORITY);
			valueList.Add(JOB_PROJECT_NAME);
			valueList.Add(COMMON_CANCEL_JOB_ON_ERROR);
			valueList.Add(COMMON_ON_TASK_ERROR);
			valueList.Add(COMMON_MAX_NUMBER_OF_EXECUTION);
			valueList.Add(COMMON_NAME);
			valueList.Add(COMMON_RESTART_TASK_ON_ERROR);
			valueList.Add(COMMON_TASK_RETRY_DELAY);
			valueList.Add(COMMON_VALUE);
			valueList.Add(VARIABLE_NAME);
			valueList.Add(VARIABLE_VALUE);
			valueList.Add(VARIABLE_MODEL);
			valueList.Add(VARIABLE_JOB_INHERITED);
			valueList.Add(TASK_CLASS_NAME);
			valueList.Add(TASK_DEPENDS_REF);
			valueList.Add(TASK_PRECIOUS_LOGS);
			valueList.Add(TASK_PRECIOUS_RESULT);
			valueList.Add(TASK_RUN_AS_ME);
			valueList.Add(TASK_FORK);
			valueList.Add(TASK_WALLTIME);
			valueList.Add(TASK_COMMAND_VALUE);
			valueList.Add(TASK_NB_NODES);
			valueList.Add(TASK_PARAMETER_NAME);
			valueList.Add(TASK_PARAMETER_VALUE);
			valueList.Add(TASK_WORKDING_DIR);
			valueList.Add(TOPOLOGY_THRESHOLD);
			valueList.Add(SCRIPT_URL);
			valueList.Add(FORK_JAVA_HOME);
			valueList.Add(FLOW_BLOCK);
			valueList.Add(FLOW_CONTINUATION);
			valueList.Add(FLOW_ELSE);
			valueList.Add(FLOW_TARGET);
			valueList.Add(DS_ACCESS_MODE);
			valueList.Add(DS_EXCLUDES);
			valueList.Add(DS_INCLUDES);
			valueList.Add(DS_URL);
			valueList.Add(PATH);
			valueList.Add(LANGUAGE);
		}

		public enum InnerEnum
		{
			JOB_PRIORITY,
			JOB_PROJECT_NAME,
			COMMON_CANCEL_JOB_ON_ERROR,
			COMMON_ON_TASK_ERROR,
			COMMON_MAX_NUMBER_OF_EXECUTION,
			COMMON_NAME,
			COMMON_RESTART_TASK_ON_ERROR,
			COMMON_TASK_RETRY_DELAY,
			COMMON_VALUE,
			VARIABLE_NAME,
			VARIABLE_VALUE,
			VARIABLE_MODEL,
			VARIABLE_JOB_INHERITED,
			TASK_CLASS_NAME,
			TASK_DEPENDS_REF,
			TASK_PRECIOUS_LOGS,
			TASK_PRECIOUS_RESULT,
			TASK_RUN_AS_ME,
			TASK_FORK,
			TASK_WALLTIME,
			TASK_COMMAND_VALUE,
			TASK_NB_NODES,
			TASK_PARAMETER_NAME,
			TASK_PARAMETER_VALUE,
			TASK_WORKDING_DIR,
			TOPOLOGY_THRESHOLD,
			SCRIPT_URL,
			FORK_JAVA_HOME,
			FLOW_BLOCK,
			FLOW_CONTINUATION,
			FLOW_ELSE,
			FLOW_TARGET,
			DS_ACCESS_MODE,
			DS_EXCLUDES,
			DS_INCLUDES,
			DS_URL,
			PATH,
			LANGUAGE
		}

		public readonly InnerEnum innerEnumValue;
		private readonly string nameValue;
		private readonly int ordinalValue;
		private static int nextOrdinal = 0;

		private string xmlName;

		internal XMLAttributes(string name, InnerEnum innerEnum, string xmlName)
		{
			this.xmlName = xmlName;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		/// <summary>
		/// Return the XML tag name of the attribute as a String.
		/// </summary>
		/// <returns> the XML tag name of the attribute as a String. </returns>
		public string XMLName
		{
			get
			{
				return this.xmlName;
			}
		}

		private static IDictionary<string, XMLAttributes> namesToEnum = null;

		/// <summary>
		/// Get the XMLAttributes enum corresponding to the given xml attribute name.
		/// This method ignores the case.
		/// </summary>
		/// <param name="xmlName"> the XML attribute name as a string </param>
		/// <returns> the corresponding XML attribute. </returns>
		/// <exception cref="IllegalArgumentException"> if the attribute name does not exist </exception>
		public static XMLAttributes getFromXMLName(string xmlName)
		{
			string toCheck = xmlName.ToUpper();
			if (namesToEnum == null)
			{
				namesToEnum = new Dictionary<string, XMLAttributes>();
				foreach (XMLAttributes attrValues in values())
				{
					namesToEnum[attrValues.XMLName.ToUpper()] = attrValues;
				}
			}
			XMLAttributes attr = namesToEnum[toCheck];
			if (attr == null)
			{
				throw new System.ArgumentException("XML attribute name '" + xmlName + "' does not exist");
			}
			else
			{
				return attr;
			}
		}

		/// <summary>
		/// Return true if the given XML name matches this XMLAttributes
		/// </summary>
		/// <param name="xmlName"> the XML attribute name as a String. </param>
		/// <returns> true only if the given XML name matches this XMLAttributes </returns>
		public bool matches(string xmlName)
		{
			return xmlName.Equals(this.xmlName, StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// {@inheritDoc}
		/// </summary>
		public override string ToString()
		{
			return XMLName;
		}


		public static XMLAttributes[] values()
		{
			return valueList.ToArray();
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static XMLAttributes valueOf(string name)
		{
			foreach (XMLAttributes enumInstance in XMLAttributes.valueList)
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