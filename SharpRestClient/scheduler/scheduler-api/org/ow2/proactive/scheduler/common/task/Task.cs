using org.ow2.proactive.scheduler.common.util;
using System;
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
namespace org.ow2.proactive.scheduler.common.task
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.ow2.proactive.scheduler.common.util.LogFormatter.addIndent;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.ow2.proactive.scheduler.common.util.LogFormatter.line;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.ow2.proactive.scheduler.common.util.LogFormatter.lineWithQuotes;



	using FileSelector = org.objectweb.proactive.extensions.dataspaces.vfs.selector.FileSelector;
	using SchedulerConstants = org.ow2.proactive.scheduler.common.SchedulerConstants;
	using Job = org.ow2.proactive.scheduler.common.job.Job;
	using InputAccessMode = org.ow2.proactive.scheduler.common.task.dataspaces.InputAccessMode;
	using InputSelector = org.ow2.proactive.scheduler.common.task.dataspaces.InputSelector;
	using OutputAccessMode = org.ow2.proactive.scheduler.common.task.dataspaces.OutputAccessMode;
	using OutputSelector = org.ow2.proactive.scheduler.common.task.dataspaces.OutputSelector;
	using FlowAction = org.ow2.proactive.scheduler.common.task.flow.FlowAction;
	using FlowBlock = org.ow2.proactive.scheduler.common.task.flow.FlowBlock;
	using FlowScript = org.ow2.proactive.scheduler.common.task.flow.FlowScript;
	using Script = org.ow2.proactive.scripting.Script<object>;
	using SelectionScript = org.ow2.proactive.scripting.SelectionScript;


	/// <summary>
	/// This class is the super class of the every task that can be integrated in a job.<br>
	/// A task contains some properties that can be set but also : <ul>
	/// <li>A selection script that can be used to select a specific execution node for this task.</li>
	/// <li>A preScript that will be launched before the real task (can be used to set environment vars).</li>
	/// <li>A postScript that will be launched just after the end of the real task.
	/// (this can be used to transfer files that have been created by the task).</li>
	/// <li>A CleaningScript that will be launched by the resource manager to perform some cleaning. (deleting files or resources).</li>
	/// </ul>
	/// You will also be able to add dependences (if necessary) to
	/// this task. The dependences mechanism are best describe below.
	/// </summary>
	/// <seealso cref= #addDependence(Task)
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 0.9 </seealso>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI @XmlAccessorType(XmlAccessType.FIELD) @XmlRootElement(name = "task") public abstract class Task extends CommonAttribute
	[Serializable]
	public abstract class Task : CommonAttribute
	{

		/// <summary>
		/// Name of the task. </summary>
		protected internal string name = SchedulerConstants.TASK_DEFAULT_NAME;

		/// <summary>
		/// block declaration : syntactic scopes used for workflows
		/// string representation of a FlowBlock enum 
		/// </summary>
		protected internal string flowBlock = FlowBlock.NONE.ToString();

		/// <summary>
		/// Description of the task. </summary>
		protected internal string description = null;

		/// <summary>
		/// tag of the task </summary>
		protected internal string tag = null;

		/// <summary>
		/// DataSpace inputFiles </summary>
		protected internal IList<InputSelector> inputFiles = null;

		/// <summary>
		/// DataSpace outputFiles </summary>
		protected internal IList<OutputSelector> outputFiles = null;

		/// <summary>
		/// The parallel environment of the task included number of nodes and topology definition
		/// required to execute this task.
		/// </summary>
		protected internal ParallelEnvironment parallelEnvironment = null;

		/// <summary>
		/// selection script : can be launched before getting a node in order to
		/// verify some computer specificity.
		/// </summary>
		protected internal IList<SelectionScript> sScripts;

		/// <summary>
		/// PreScript : can be used to launch script just before the task
		/// execution.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: protected org.ow2.proactive.scripting.Script<?> preScript;
		protected internal Script preScript;

		/// <summary>
		/// PostScript : can be used to launch script just after the task
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: protected org.ow2.proactive.scripting.Script<?> postScript;
		protected internal Script postScript;

		/// <summary>
		/// CleaningScript : can be used to launch script just after the task or the postScript (if set)
		/// Started even if a problem occurs.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: protected org.ow2.proactive.scripting.Script<?> cScript;
		protected internal Script cScript;

		/// <summary>
		/// FlowScript: Control Flow Action Script executed after the task,
		/// returns a <seealso cref="FlowAction"/> which will perform flow actions on the job
		/// </summary>
		protected internal FlowScript flowScript;

		/// <summary>
		/// Tell whether this task has a precious result or not. </summary>
		protected internal bool preciousResult;

		/// <summary>
		/// If true, the stdout/err of this tasks are stored in a file in LOCALSPACE and copied back to USERSPACE </summary>
		protected internal bool preciousLogs;

		protected internal bool runAsMe;

		/// <summary>
		/// If true, task is ran in a forked JVM, this parameter is optional (nullable) . </summary>
		protected internal bool? fork;

		/// <summary>
		/// List of dependences if necessary. </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlTransient private java.util.List<Task> dependences = null;
		private IList<Task> dependences = null;

		/// <summary>
		/// maximum execution time of the task (in milliseconds), the variable is only valid if isWallTime is true </summary>
		protected internal long wallTime = 0;

		protected internal ForkEnvironment forkEnvironment;

		/// <summary>
		/// A map to hold task variables </summary>
		protected internal IDictionary<string, TaskVariable> variables = new Dictionary<string, TaskVariable>();

		private IDictionary<string, TaskVariable> unresolvedVariables = new Dictionary<string, TaskVariable>();

		/// <summary>
		/// Add a dependence to the task. <font color="red">Warning : the dependence order is very
		/// important.</font>
		/// <para>
		/// In fact, it is in this order that you will get back the result in the children task.
		/// </para>
		/// <para>
		/// For example: if you add to the task t3, the dependences t1 then t2, the parents of t3 will be t1 and t2 in this order
		/// and the parameters of t3 will be the results of t1 and t2 in this order.
		/// 
		/// </para>
		/// </summary>
		/// <param name="task">
		///            the parent task to add to this task. </param>
		public virtual void addDependence(Task task)
		{
			if (dependences == null)
			{
				dependences = new List<Task>();
			}
			dependences.Add(task);
		}

		/// <summary>
		/// Same as the <seealso cref="addDependence(Task)"/> method, but for a list of dependences.
		/// </summary>
		/// <param name="tasks">
		///            the parent list of tasks to add to this task. </param>
		public virtual void addDependences(IList<Task> tasks)
		{
			if (dependences == null)
			{
				dependences = new List<Task>();
			}
			foreach (Task t in tasks)
			{
				addDependence(t);
			}
		}

		/// <summary>
		/// To get the description of this task.
		/// </summary>
		/// <returns> the description of this task. </returns>
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
		/// To know if the result of this task is precious.
		/// </summary>
		/// <returns> true if the result is precious, false if not. </returns>
		public virtual bool PreciousResult
		{
			get
			{
				return preciousResult;
			}
			set
			{
				this.preciousResult = value;
			}
		}


		/// <summary>
		/// To know if the logs of this task are precious.
		/// </summary>
		/// <returns> true if the logs are precious, false if not. </returns>
		public virtual bool PreciousLogs
		{
			get
			{
				return preciousLogs;
			}
			set
			{
				this.preciousLogs = value;
			}
		}


		/// <summary>
		/// To know if the task will be executed under the user identity or not.
		/// </summary>
		/// <returns> true if the task will be executed as the user identity. </returns>
		public virtual bool RunAsMe
		{
			get
			{
				return this.runAsMe;
			}
			set
			{
				this.runAsMe = value;
			}
		}


		/// <summary>
		/// To know if the task will be run in a forked JVM </summary>
		/// <returns> fork true if the task will be run in a forked JVM; false if the task will be ran in the node's JVM. </returns>
		public virtual bool? Fork
		{
			get
			{
				return fork;
			}
			set
			{
				this.fork = value;
			}
		}


		/// <summary>
		/// To get the name of this task.
		/// </summary>
		/// <returns> the name of this task. </returns>
		public virtual string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (string.ReferenceEquals(value, null))
				{
					return;
				}
				if (value.Length > 255)
				{
					throw new System.ArgumentException("The name is too long, it must have 255 chars length max : " + value);
				}
				this.name = value;
			}
		}


		/// <summary>
		/// Get the tag of this task.
		/// Return null if this task has no tag.
		/// </summary>
		/// <returns> the tag of this task </returns>
		public virtual string Tag
		{
			get
			{
				return this.tag;
			}
			set
			{
				this.tag = value;
			}
		}


		/// <summary>
		/// Defining Control Flow Blocks with pairs of <seealso cref="FlowBlock.START"/> and <seealso cref="FlowBlock.END"/>
		/// is a semantic requirement to be able to create specific control flow constructs.
		/// <para>
		/// Refer to the documentation for detailed instructions.
		/// 
		/// </para>
		/// </summary>
		/// <returns> the FlowBlock attribute of this task indicating if it is the beginning or the ending
		///  of a new block scope </returns>
		public virtual FlowBlock FlowBlock
		{
			get
			{
				return FlowBlock.parse(this.flowBlock);
			}
			set
			{
				this.flowBlock = value.ToString();
			}
		}


		/// <summary>
		/// To get the preScript of this task.
		/// </summary>
		/// <returns> the preScript of this task. </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: public org.ow2.proactive.scripting.Script<?> getPreScript()
		public virtual Script PreScript
		{
			get
			{
				return preScript;
			}
			set
			{
				this.preScript = value;
				this.preScript.overrideDefaultScriptName("PreScript");
			}
		}


		/// <summary>
		/// To get the postScript of this task.
		/// </summary>
		/// <returns> the postScript of this task. </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: public org.ow2.proactive.scripting.Script<?> getPostScript()
		public virtual Script PostScript
		{
			get
			{
				return postScript;
			}
			set
			{
				this.postScript = value;
				this.postScript.overrideDefaultScriptName("PostScript");
			}
		}

		/// <summary>
		/// To perform complex Control Flow Actions in a TaskFlowJob,
		/// a FlowScript needs to be attached to specific tasks.
		/// <para>
		/// Some Control Flow constructs need to be used within the bounds of
		/// a <seealso cref="FlowBlock"/>, which is defined a the level of the Task using
		/// <seealso cref="Task.setFlowBlock(FlowBlock)"/>.
		/// </para>
		/// <para>
		/// Refer to the user documentation for further instructions.
		/// 
		/// </para>
		/// </summary>
		/// <returns> the Control Flow Script used for workflow actions </returns>
		public virtual FlowScript FlowScript
		{
			get
			{
				return flowScript;
			}
			set
			{
				this.flowScript = value;
			}
		}



		/// <summary>
		/// To get the cleaningScript of this task.
		/// </summary>
		/// <returns> the cleaningScript of this task. </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: public org.ow2.proactive.scripting.Script<?> getCleaningScript()
		public virtual Script CleaningScript
		{
			get
			{
				return cScript;
			}
			set
			{
				this.cScript = value;
				this.cScript.overrideDefaultScriptName("CleanScript");
			}
		}


		/// <summary>
		/// Checks if the task is parallel. </summary>
		/// <returns> true if the task is parallel, false otherwise. </returns>
		public virtual bool Parallel
		{
			get
			{
				return parallelEnvironment != null && parallelEnvironment.NodesNumber >= 1;
			}
		}

		/// <summary>
		/// Returns the parallel environment of the task. </summary>
		/// <returns> the parallel environment of the task. </returns>
		public virtual ParallelEnvironment ParallelEnvironment
		{
			get
			{
				return parallelEnvironment;
			}
			set
			{
				this.parallelEnvironment = value;
			}
		}


		/// <summary>
		/// To get the number of execution for this task.
		/// </summary>
		/// <returns> the number of times this task can be executed. </returns>

		/// <summary>
		/// To get the selection script. This is the script that will select a node.
		/// </summary>
		/// <returns> the selection Script. </returns>
		public virtual IList<SelectionScript> SelectionScripts
		{
			get
			{
				if (sScripts == null || sScripts.Count == 0)
				{
					return null;
				}
				else
				{
					return sScripts;
				}
			}
			set
			{
				this.sScripts = value;
			}
		}

		/// <summary>
		/// Set a selection script. It is the script that will be in charge of selecting a node.
		/// </summary>
		public virtual SelectionScript SelectionScript
		{
			set
			{
				if (value == null)
				{
					throw new System.ArgumentException("The given selection script cannot be null !");
				}
				IList<SelectionScript> selScriptsList = new List<SelectionScript>();
				selScriptsList.Add(value);
				SelectionScripts = selScriptsList;
			}
		}


		/// <summary>
		/// To add a selection script to the list of selection script.
		/// </summary>
		/// <param name="selectionScript">
		///            the selectionScript to add. </param>
		public virtual void addSelectionScript(SelectionScript selectionScript)
		{
			if (selectionScript == null)
			{
				throw new System.ArgumentException("The given selection script cannot be null !");
			}
			if (this.sScripts == null)
			{
				this.sScripts = new List<SelectionScript>();
			}
			this.sScripts.Add(selectionScript);
		}

		/// <summary>
		/// To get the list of dependencies of the task.
		/// </summary>
		/// <returns> the the list of dependencies of the task. </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlTransient public java.util.List<Task> getDependencesList()
		public virtual IList<Task> DependencesList
		{
			get
			{
				return dependences;
			}
		}

		/// <summary>
		/// Get the number of nodes needed for this task (by default: 1).
		/// </summary>
		/// <returns> the number of Nodes Needed </returns>
		public virtual int NumberOfNodesNeeded
		{
			get
			{
				return Parallel ? ParallelEnvironment.NodesNumber : 1;
			}
		}

		/// <returns> the walltime </returns>
		public virtual long WallTime
		{
			get
			{
				return wallTime;
			}
			set
			{
				if (value < 0)
				{
					throw new System.ArgumentException("The walltime must be a positive or nul integer value (>=0) !");
				}
				this.wallTime = value;
			}
		}

		/// <summary>
		/// Return the working Directory.
		/// </summary>
		/// <returns> the working Directory. </returns>
		public virtual string WorkingDir
		{
			get
			{
				if (forkEnvironment == null)
				{
					return null;
				}
    
				return forkEnvironment.WorkingDir;
			}
		}


		/// <summary>
		/// Return true if wallTime is set.
		/// </summary>
		/// <returns> the isWallTime </returns>
		public virtual bool WallTimeSet
		{
			get
			{
				return wallTime > 0;
			}
		}

		/// <summary>
		/// Set the number of nodes needed for this task.
		/// <para>
		/// This number represents the total number of nodes that you need. You may remember that
		/// default number is 1.
		/// 
		/// </para>
		/// </summary>
		/// <param name="numberOfNodesNeeded"> the number Of Nodes Needed to set. </param>
		[Obsolete]
		public virtual int NumberOfNeededNodes
		{
			set
			{
				if (parallelEnvironment != null)
				{
					throw new System.InvalidOperationException("Cannot set numberOfNodesNeeded as it could be inconsistent with the parallel environment");
				}
				this.parallelEnvironment = new ParallelEnvironment(value);
			}
		}

		/// <summary>
		/// Add the files value to the given files value
		/// according to the provided access mode.
		/// <para>
		/// Mode define the way the files will be bring to LOCAL space.
		/// 
		/// </para>
		/// </summary>
		/// <param name="files"> the input Files to add </param>
		/// <param name="mode"> the way to provide files to LOCAL space </param>
		public virtual void addInputFiles(FileSelector files, InputAccessMode mode)
		{
			if (files == null)
			{
				throw new System.ArgumentException("Argument files is null");
			}

			if (inputFiles == null)
			{
				inputFiles = new List<InputSelector>();
			}

			inputFiles.Add(new InputSelector(files, mode));
		}

		/// <summary>
		/// Add the files value to the given files value
		/// according to the provided access mode.
		/// <para>
		/// Mode define the way the files will be send to OUTPUT space.
		/// 
		/// </para>
		/// </summary>
		/// <param name="files"> the output Files to add </param>
		/// <param name="mode"> the way to send files to OUTPUT space </param>
		public virtual void addOutputFiles(FileSelector files, OutputAccessMode mode)
		{
			if (files == null)
			{
				throw new System.ArgumentException("Argument files is null");
			}

			if (outputFiles == null)
			{
				outputFiles = new List<OutputSelector>();
			}

			outputFiles.Add(new OutputSelector(files, mode));
		}

		/// <summary>
		/// Add the files to the given filesToInclude value
		/// according to the provided access mode.
		/// <para>
		/// Mode define the way the files will be bring to LOCAL space.
		/// filesToInclude can represent one file or many files defined by a regular expression.
		/// </para>
		/// </summary>
		/// <seealso cref= FileSelector for details
		/// </seealso>
		/// <param name="filesToInclude"> the input files to add </param>
		/// <param name="mode"> the way to provide files to LOCAL space </param>
		public virtual void addInputFiles(string filesToInclude, InputAccessMode mode)
		{
			if (string.ReferenceEquals(filesToInclude, null))
			{
				throw new System.ArgumentException("Argument filesToInclude is null");
			}
			if (inputFiles == null)
			{
				inputFiles = new List<InputSelector>();
			}
			inputFiles.Add(new InputSelector(new FileSelector(filesToInclude), mode));
		}

		/// <summary>
		/// Add the files to the given filesToInclude value
		/// according to the provided access mode.
		/// <para>
		/// Mode define the way the files will be send to OUTPUT space.
		/// filesToInclude can represent one file or many files defined by a regular expression.
		/// </para>
		/// </summary>
		/// <seealso cref= FileSelector for details
		/// </seealso>
		/// <param name="filesToInclude"> the output files to add </param>
		/// <param name="mode"> the way to send files to OUTPUT space </param>
		public virtual void addOutputFiles(string filesToInclude, OutputAccessMode mode)
		{
			if (string.ReferenceEquals(filesToInclude, null))
			{
				throw new System.ArgumentException("Argument filesToInclude is null");
			}
			if (outputFiles == null)
			{
				outputFiles = new List<OutputSelector>();
			}
			outputFiles.Add(new OutputSelector(new FileSelector(filesToInclude), mode));
		}

		/// <summary>
		/// Get the input file selectors list.
		/// This list represents every couple of input FileSelector and its associated access mode
		/// The first element is the first added couple.<br>
		/// This method returns null if nothing was added to the inputFiles.
		/// </summary>
		/// <returns> the input file selectors list </returns>
		public virtual IList<InputSelector> InputFilesList
		{
			get
			{
				return inputFiles;
			}
		}

		/// <summary>
		/// Get the output file selectors list.
		/// This list represents every couple of output FileSelector and its associated access mode
		/// The first element is the first added couple.<br>
		/// This method returns null if nothing was added to the outputFiles.
		/// </summary>
		/// <returns> the output file selectors list </returns>
		public virtual IList<OutputSelector> OutputFilesList
		{
			get
			{
				return outputFiles;
			}
		}

		/// <summary>
		/// Sets the variable map for this task.
		/// </summary>
		/// <param name="variables"> the variables map </param>
		public virtual IDictionary<string, TaskVariable> Variables
		{
			set
			{
				Job.verifyVariableMap(value);
				this.variables = new Dictionary<string, TaskVariable>(value);
			}
			get
			{
				return this.variables;
			}
		}


		/// <summary>
		/// Returns the unresolved variable map of this job.
		/// </summary>
		/// <returns> an unresolved variable map </returns>
		public virtual IDictionary<string, TaskVariable> UnresolvedVariables
		{
			get
			{
				return this.unresolvedVariables;
			}
			set
			{
				verifyVariableMap(value);
				this.unresolvedVariables = new Dictionary<string, TaskVariable>(value);
			}
		}


		public static void verifyVariableMap<T>(IDictionary<string, T> variables) where T : TaskVariable
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: for (java.util.Map.Entry<String, ? extends TaskVariable> entry : variables.entrySet())
			foreach (KeyValuePair<string, T> entry in variables.SetOfKeyValuePairs())
			{
				if (!entry.Key.Equals(entry.Value.Name))
				{
					throw new System.ArgumentException("Variables map entry key (" + entry.Key + ") is different from variable name (" + entry.Value.Name + ")");
				}
			}
		}

		/// <summary>
		/// Returns the variable map of this task.
		/// </summary>
		/// <returns> a variable map </returns>
		public virtual IDictionary<string, string> getVariablesOverriden(Job job)
		{
			IDictionary<string, string> taskVariables = new Dictionary<string, string>();
			if (job != null)
			{
				taskVariables.PutAll(job.VariablesAsReplacementMap);
			}
			foreach (TaskVariable variable in Variables.Values)
			{
				if (!variable.JobInherited)
				{
					taskVariables[variable.Name] = variable.Value;
				}
			}
			return taskVariables;
		}

		public override string ToString()
		{
			return name;
		}

		public virtual string display()
		{
			return "Task '" + name + "' : " + Environment.NewLine + 
				LogFormatter.addIndent(
					LogFormatter.lineWithQuotes("Description", description) + Environment.NewLine +
					LogFormatter.line("restartTaskOnError", restartTaskOnError) + Environment.NewLine +
					LogFormatter.line("taskRetryDelay", taskRetryDelay) + Environment.NewLine +
					LogFormatter.line("onTaskError", onTaskError) + Environment.NewLine +
					LogFormatter.line("maxNumberOfExecution", maxNumberOfExecution, () => maxNumberOfExecution.Value.IntegerValue) + Environment.NewLine +
					LogFormatter.line("tag", tag) + Environment.NewLine +
					LogFormatter.line("variables", variables) + Environment.NewLine +
					LogFormatter.line("genericInformation", genericInformation) + Environment.NewLine +
					LogFormatter.line("InputFiles", inputFiles) + Environment.NewLine +
					LogFormatter.line("OutputFiles", outputFiles) + Environment.NewLine +
					LogFormatter.line("ParallelEnvironment", parallelEnvironment) + Environment.NewLine +
					LogFormatter.lineWithQuotes("FlowBlock", flowBlock) + Environment.NewLine +
					LogFormatter.line("SelectionScripts", displaySelectionScripts()) + Environment.NewLine +
					LogFormatter.line("ForkEnvironment", forkEnvironment) + Environment.NewLine +
					LogFormatter.line("PreScript", preScript, () => preScript.display()) + Environment.NewLine +
					LogFormatter.line("PostScript", postScript, () => postScript.display()) + Environment.NewLine +
					LogFormatter.line("CleanScript", cScript, () => cScript.display()) + Environment.NewLine +
					LogFormatter.line("FlowScript", flowScript, () => flowScript.display()) + Environment.NewLine +
					LogFormatter.line("PreciousResult", preciousResult) + Environment.NewLine +
					LogFormatter.line("PreciousLogs", preciousLogs) + Environment.NewLine +
					LogFormatter.line("RunAsMe", runAsMe) + Environment.NewLine +
					LogFormatter.line("fork", fork) + Environment.NewLine +
					LogFormatter.line("WallTime", wallTime) + Environment.NewLine +
					LogFormatter.line("Dependences", dependences));
		}

		private string displaySelectionScripts()
		{
			StringBuilder answer = new StringBuilder("[");
			if (sScripts != null && sScripts.Count > 0)
			{
				answer.Append(Environment.NewLine);
				foreach (SelectionScript sScript in sScripts)
				{
					answer.Append(LogFormatter.addIndent(sScript.display()));
					answer.Append("," + Environment.NewLine);
				}
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:				
			}
			answer.Append("]");
			return answer.ToString();
		}

		public virtual ForkEnvironment ForkEnvironment
		{
			get
			{
				return forkEnvironment;
			}
			set
			{
				this.forkEnvironment = value;
			}
		}


	}

}