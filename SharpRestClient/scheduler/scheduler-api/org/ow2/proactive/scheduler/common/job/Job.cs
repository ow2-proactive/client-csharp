using org.ow2.proactive.scheduler.common.util;
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
namespace org.ow2.proactive.scheduler.common.job
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.ow2.proactive.scheduler.common.util.LogFormatter.addIndent;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.ow2.proactive.scheduler.common.util.LogFormatter.line;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.ow2.proactive.scheduler.common.util.LogFormatter.lineWithQuotes;

	using SchedulerConstants = org.ow2.proactive.scheduler.common.SchedulerConstants;
	using CommonAttribute = org.ow2.proactive.scheduler.common.task.CommonAttribute;
	using OnTaskError = org.ow2.proactive.scheduler.common.task.OnTaskError;


	/// <summary>
	/// Definition of a job for the user.
	/// You can create a job by using this class. Job will be used to set some properties,
	/// and give it the different tasks to run.
	/// <para>
	/// Here's a definition of the different parts of a job:<br>
	/// <seealso cref="setName(string)"/> will be used to identified the job.<br>
	/// <seealso cref="setDescription(string)"/> to set a short description of your job.<br>
	/// <seealso cref="setPriority(JobPriority)"/> to set the priority for the job, see <seealso cref="JobPriority"/> for more details.<br>
	/// <seealso cref="setOnTaskError(OnTaskError)"/> to set a predefined action when an exception occurred in at least one of the task.<br>
	/// </para>
	/// <para>
	/// Once the job created, you can submit it to the scheduler using the UserSchedulerInterface.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 0.9
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public abstract class Job extends org.ow2.proactive.scheduler.common.task.CommonAttribute
	[Serializable]
	public abstract class Job : CommonAttribute
	{

		public const string JOB_DDL = "JOB_DDL";

		public const string JOB_EXEC_TIME = "JOB_EXEC_TIME";

		/// <summary>
		/// Name of the job
		/// </summary>
		protected internal string name = SchedulerConstants.JOB_DEFAULT_NAME;

		/// <summary>
		/// Short description of this job
		/// </summary>
		protected internal string description = "";

		/// <summary>
		/// Project name for this job
		/// </summary>
		protected internal string projectName = "";

		/// <summary>
		/// Job priority
		/// </summary>
		protected internal JobPriority priority = JobPriority.NORMAL;

		protected internal string inputSpace = null;

		protected internal string outputSpace = null;

		protected internal string globalSpace = null;

		protected internal string userSpace = null;

		/// <summary>
		/// SVG Visualization rendering of this job
		/// </summary>
		protected internal string visualization = null;

		/// <summary>
		/// A map to holds job descriptor variables
		/// </summary>
		protected internal IDictionary<string, JobVariable> variables = new Dictionary<string, JobVariable>();

		/// <summary>
		/// A map to holds job descriptor variables with their values unresolved against other variables
		/// </summary>
		protected internal IDictionary<string, JobVariable> unresolvedVariables = new Dictionary<string, JobVariable>();

		/// <summary>
		/// represent xml submitted, where variables and genetic info were updated according provided maps
		/// </summary>
		private string jobContent = null;

		private long? parentId = null;

		/// <summary>
		/// ProActive Empty Constructor
		/// </summary>
		public Job()
		{
		}

		/// <summary>
		/// To get the type of this job
		/// </summary>
		/// <returns> the type of this job </returns>
		public abstract JobType Type {get;}

		/// <summary>
		/// To get the id
		/// </summary>
		/// <returns> the id </returns>
		public abstract JobId Id {get;}

		/// <summary>
		/// To get the description
		/// </summary>
		/// <returns> the description </returns>
		public virtual string Description
		{
			get
			{
				return description;
			}
			set
			{
				this.description = value;
			}
		}


		/// <summary>
		/// To get the name of the job.
		/// </summary>
		/// <returns> the name of the job. </returns>
		public virtual string Name
		{
			get
			{
				return name;
			}
			set
			{
				this.name = value;
			}
		}


		/// <summary>
		/// To get the priority of the job.
		/// </summary>
		/// <returns> the priority of the job. </returns>
		public virtual JobPriority Priority
		{
			get
			{
				return priority;
			}
			set
			{
				this.priority = value;
			}
		}


		/// <summary>
		/// Returns the project Name.
		/// </summary>
		/// <returns> the project Name. </returns>
		public virtual string ProjectName
		{
			get
			{
				return projectName;
			}
			set
			{
				this.projectName = value;
			}
		}


		/// <summary>
		/// Get the input Space
		/// </summary>
		/// <returns> the input Space </returns>
		public virtual string InputSpace
		{
			get
			{
				return inputSpace;
			}
			set
			{
				this.inputSpace = value;
			}
		}


		/// <summary>
		/// Get the output Space
		/// </summary>
		/// <returns> the output Space </returns>
		public virtual string OutputSpace
		{
			get
			{
				return outputSpace;
			}
			set
			{
				this.outputSpace = value;
			}
		}


		public virtual string GlobalSpace
		{
			get
			{
				return globalSpace;
			}
			set
			{
				this.globalSpace = value;
			}
		}


		public virtual string UserSpace
		{
			get
			{
				return userSpace;
			}
			set
			{
				this.userSpace = value;
			}
		}


		/// <summary>
		/// Get the job visualization rendering in SVG format </summary>
		/// <returns> Job visualization SVG code </returns>
		public virtual string Visualization
		{
			get
			{
				return visualization;
			}
			set
			{
				this.visualization = value;
			}
		}


		/// <summary>
		/// Sets the variable map for this job.
		/// </summary>
		/// <param name="variables"> the variables map </param>
		public virtual IDictionary<string, JobVariable> Variables
		{
			set
			{
				verifyVariableMap(value);
				this.variables = new Dictionary<string, JobVariable>(value);
			}
			get
			{
				return this.variables;
			}
		}

		/// <summary>
		/// Sets the unresolved variable map for this job.
		/// </summary>
		/// <param name="unresolvedVariables"> the unresolved variables map </param>
		public virtual IDictionary<string, JobVariable> UnresolvedVariables
		{
			set
			{
				verifyVariableMap(value);
				this.unresolvedVariables = new Dictionary<string, JobVariable>(value);
			}
			get
			{
				return this.unresolvedVariables;
			}
		}

		public static void verifyVariableMap<T>(IDictionary<string, T> variables) where T : JobVariable
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: for (java.util.Map.Entry<String, ? extends JobVariable> entry : variables.entrySet())
			foreach (KeyValuePair<string, T> entry in variables.SetOfKeyValuePairs())
			{
				if (!entry.Key.Equals(entry.Value.Name))
				{
					throw new System.ArgumentException("Variables map entry key (" + entry.Key + ") is different from variable name (" + entry.Value.Name + ")");
				}
			}
		}



		/// <summary>
		/// Returns a map containing the variable names and their values.
		/// </summary>
		/// <returns> a variable map </returns>
		public virtual IDictionary<string, string> VariablesAsReplacementMap
		{
			get
			{
				Dictionary<string, string> replacementVariables = new Dictionary<string, string>(variables.Count);
				foreach (JobVariable variable in variables.Values)
				{
					replacementVariables[variable.Name] = variable.Value;
				}
				return replacementVariables;
			}
		}

		public override string ToString()
		{
			return name;
		}

		public virtual string display()
		{
			return "Job '" + name + "' : " + Environment.NewLine + 
				LogFormatter.addIndent(LogFormatter.lineWithQuotes("Description", description) + Environment.NewLine +
				LogFormatter.lineWithQuotes("ProjectName", projectName) + Environment.NewLine +
				LogFormatter.line("onTaskError", onTaskError) + Environment.NewLine +
				LogFormatter.line("restartTaskOnError", restartTaskOnError) + Environment.NewLine +
				LogFormatter.line("taskRetryDelay", taskRetryDelay) + Environment.NewLine +
				LogFormatter.line("maxNumberOfExecution", maxNumberOfExecution, () => maxNumberOfExecution.Value.IntegerValue) + Environment.NewLine +
				LogFormatter.line("genericInformation", genericInformation) + Environment.NewLine +
				LogFormatter.line("Priority", priority) + Environment.NewLine +
				LogFormatter.lineWithQuotes("InputSpace", inputSpace) + Environment.NewLine +
				LogFormatter.lineWithQuotes("OutputSpace", outputSpace) + Environment.NewLine +
				LogFormatter.lineWithQuotes("GlobalSpace", globalSpace) + Environment.NewLine +
				LogFormatter.lineWithQuotes("UserSpace", userSpace) + Environment.NewLine +
				LogFormatter.line("Variables", variables));
		}


		public virtual string JobContent
		{
			get
			{
				return jobContent;
			}
			set
			{
				this.jobContent = value;
			}
		}


		public virtual long? ParentId
		{
			get
			{
				return parentId;
			}
			set
			{
				this.parentId = value;
			}
		}

	}

}