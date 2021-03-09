using System;
using System.Collections.Generic;
using System.IO;

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

	using JobCreationException = org.ow2.proactive.scheduler.common.exception.JobCreationException;
	using Job = org.ow2.proactive.scheduler.common.job.Job;
	using TaskFlowJob = org.ow2.proactive.scheduler.common.job.TaskFlowJob;
	using NativeTask = org.ow2.proactive.scheduler.common.task.NativeTask;
	using InvalidScriptException = org.ow2.proactive.scripting.InvalidScriptException;
	using SelectionScript = org.ow2.proactive.scripting.SelectionScript;
	using SimpleScript = org.ow2.proactive.scripting.SimpleScript;
	using Tools = org.ow2.proactive.utils.Tools;


	/// <summary>
	/// This class implements static methods use to parse a text file containing commands, and from it build
	/// a ProActive Scheduler job made of native tasks. each task of the jobs corresponds to a line of the
	/// parsed file. This is a way to avoid creation of XML job descriptor for creation of simple jobs.
	/// 
	/// Every line of the text file is taken and considered as a native command from which a native task is built,
	/// except lines beginning with <seealso cref="FlatJobFactory.CMD_FILE_COMMENT_CHAR"/> and empty lines.
	/// dependencies between tasks cannot be set, task names are automatically set. A log file can be specified.
	/// A selection script can be associated for all the tasks, but not specific selection script for each tasks.
	/// A Job name can be specified too.
	/// 
	/// This class does not intend to provide a job specification with all ProActive Scheduler jobs feature, but is
	/// way to define quickly jobs made of native tasks to execute in parallel.
	/// If you need to define jobs with dependencies, jobs with java Tasks, specific selection script for each task,
	/// or generation scripts... you should rather use XML job descriptors and <seealso cref="JobFactory"/>.
	/// 
	/// 
	/// the class presents too a way to create a job made of one task from a String representing a native command to launch.
	/// 
	/// @author ProActive team
	/// 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public class FlatJobFactory
	public class FlatJobFactory
	{


		/// <summary>
		/// comment character used to ignore line in text file containing
		/// native commands
		/// </summary>
		public const string CMD_FILE_COMMENT_CHAR = "#";

		/// <summary>
		/// String prefix used to build default job name (if no job name is specified).
		/// </summary>
		public const string JOB_DEFAULT_NAME_PREFIX = "Job_";

		/// <summary>
		/// Singleton Pattern
		/// </summary>
		private static FlatJobFactory factory = null;

		/// <summary>
		/// Return the instance of the jobFactory.
		/// </summary>
		/// <returns> the instance of the jobFactory. </returns>
		public static FlatJobFactory Factory
		{
			get
			{
				if (factory == null)
				{
					factory = new FlatJobFactory();
				}
				return factory;
			}
		}

		/// <summary>
		/// Create a job from a String representing file path, this text file contains native commands to launch
		/// Every line of the text file is taken and considered as a native command from which a native task is built,
		/// except lines beginning with <seealso cref="FlatJobFactory.JOB_DEFAULT_NAME_PREFIX"/> and empty lines.
		/// So job in result is made of several native tasks without dependencies.
		/// </summary>
		/// <param name="commandFilePath"> a string representing a text file containing native commands. </param>
		/// <param name="jobName"> A String representing a name to give to the job. If null, default job name is made of
		/// <seealso cref="FlatJobFactory.JOB_DEFAULT_NAME_PREFIX"/> + userName parameter. </param>
		/// <param name="selectionScriptPath"> a Path to a file containing a selection script, or null if
		/// no script is needed. </param>
		/// <param name="userName"> name of connected user that asked job creation, null otherwise. This parameter
		/// is only used for default job's name creation. </param>
		/// <returns> a job object representing created job and ready-to-schedule job. </returns>
		/// <exception cref="JobCreationException"> with a relevant error message if an error occurs. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public org.ow2.proactive.scheduler.common.job.Job createNativeJobFromCommandsFile(String commandFilePath, String jobName, String selectionScriptPath, String userName) throws org.ow2.proactive.scheduler.common.exception.JobCreationException
		public virtual Job createNativeJobFromCommandsFile(string commandFilePath, string jobName, string selectionScriptPath, string userName)
		{

			if (string.ReferenceEquals(jobName, null))
			{
				jobName = JOB_DEFAULT_NAME_PREFIX + userName;
			}
			Job nativeJob = new TaskFlowJob();
			nativeJob.Name = jobName;

			try
			{
				FileInfo commandFile = new FileInfo(commandFilePath);
				if (!commandFile.Exists)
				{
					throw new JobCreationException("Error occured during Job creation, " + "check that file " + commandFilePath + " exists and is a readable file");
				}
				string commandLine;
				int task_number = 0;

				List<string> commandList = new List<string>();
				using (StreamReader reader = new StreamReader(commandFile.FullName))
				{
					while (!string.ReferenceEquals((commandLine = reader.ReadLine()), null))
					{
						commandLine = commandLine.Trim();
						if (!commandLine.StartsWith(CMD_FILE_COMMENT_CHAR, 0) && !"".Equals(commandLine))
						{
							commandList.Add(commandLine);
						}
					}
				}
				if (commandList.Count == 0)
				{
					throw new JobCreationException("Error occured during Job creation, " + "No any valid command line has been built from" + commandFilePath + "");
				}

				//compute padding for task number
				int numberOfDigit = Convert.ToString(commandList.Count).Length;

				foreach (string command in commandList)
				{
					NativeTask t = createNativeTaskFromCommandString(command, "task_" + (++task_number), selectionScriptPath);
					t.PreciousResult = true;
					((TaskFlowJob) nativeJob).addTask(t);
				//	logger.debug("-> Task Name = " + t.Name);
				//	logger.debug("-> command = " + t.CommandLine + "\n");
				}
			}
			catch (Exception e)
			{
				throw new JobCreationException(e);
			}
			return nativeJob;
		}

		/// <summary>
		/// Creates a job from a String representing a native command to launch. So job in result is made
		/// of one native task.
		/// </summary>
		/// <param name="command"> a string representing an executable command to launch. </param>
		/// <param name="jobName"> A String representing a name to give to the job, if null. default job name is made of
		/// {link FlatJobFactory#JOB_DEFAULT_NAME_PREFIX} + userName parameter. </param>
		/// <param name="selectionScriptPath"> A Path to a file containing a selection script, or null if
		/// no script is needed. </param>
		/// <param name="userName"> name of connected user that asked job creation, null otherwise. This parameter
		/// is just used for default job's name creation. </param>
		/// <returns> a job object representing created job and ready-to-schedule job. </returns>
		/// <exception cref="JobCreationException"> with a relevant error message if an error occurs. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public org.ow2.proactive.scheduler.common.job.Job createNativeJobFromCommand(String command, String jobName, String selectionScriptPath, String userName) throws org.ow2.proactive.scheduler.common.exception.JobCreationException
		public virtual Job createNativeJobFromCommand(string command, string jobName, string selectionScriptPath, string userName)
		{
			if (string.ReferenceEquals(command, null) || "".Equals(command, StringComparison.OrdinalIgnoreCase))
			{
				throw new JobCreationException("Error, command cannot be null");
			}

			if (string.ReferenceEquals(jobName, null))
			{
				jobName = JOB_DEFAULT_NAME_PREFIX + userName;
			}
			Job nativeJob = new TaskFlowJob();
			nativeJob.Name = jobName;
			//logger.debug("Job : " + nativeJob.Name);
			try
			{
				NativeTask t = createNativeTaskFromCommandString(command, "task1", selectionScriptPath);
				t.PreciousResult = true;
				((TaskFlowJob) nativeJob).addTask(t);
				//logger.debug("-> Task Name = " + t.Name);
				//logger.debug("-> command = " + t.CommandLine + "\n");
			}
			catch (Exception e)
			{
				throw new JobCreationException(e);
			}
			return nativeJob;
		}

		/// <summary>
		/// Creates a native task from a string representing a native command to execute. </summary>
		/// <param name="command"> a String representing a native command. </param>
		/// <param name="taskName"> an eventual name for the task. </param>
		/// <param name="selectionScriptPath"> path to an existing file containing a selection script code. </param>
		/// <returns> a NativeTask object that can be put in a Job Object. </returns>
		/// <exception cref="InvalidScriptException"> if an error occurs in definition of selection script
		/// from file path specified. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private org.ow2.proactive.scheduler.common.task.NativeTask createNativeTaskFromCommandString(String command, String taskName, String selectionScriptPath) throws org.ow2.proactive.scripting.InvalidScriptException
		private NativeTask createNativeTaskFromCommandString(string command, string taskName, string selectionScriptPath)
		{
			NativeTask desc = new NativeTask();
			desc.CommandLine = Tools.parseCommandLine(command);
			desc.Name = taskName;

			if (!string.ReferenceEquals(selectionScriptPath, null))
			{
				SelectionScript script = new SelectionScript(new SimpleScript(new FileInfo(selectionScriptPath), null), true);
				desc.addSelectionScript(script);
			}
			return desc;
		}
	}

}