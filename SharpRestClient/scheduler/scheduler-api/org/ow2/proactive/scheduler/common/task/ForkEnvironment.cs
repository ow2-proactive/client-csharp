using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

	using ExecutableCreationException = org.ow2.proactive.scheduler.common.exception.ExecutableCreationException;
	using InvalidScriptException = org.ow2.proactive.scripting.InvalidScriptException;
	using Script = org.ow2.proactive.scripting.Script<object>;
	using SimpleScript = org.ow2.proactive.scripting.SimpleScript;


	/// <summary>
	/// Class representing a forked environment of a JVM created specifically for an execution of a Java Task.
	/// A Java Task can be executed in the current JVM - then all Java Tasks are dependent on the same JVM (provider) and JVM options (like memory),
	/// or can be executed in a dedicated JVM with additional options specified like {@code javaHome}, {@code javaArguments}, {@code classpath}, ...
	/// 
	/// @author ProActive team
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI @XmlAccessorType(XmlAccessType.FIELD) public class ForkEnvironment implements java.io.Serializable
	[Serializable]
	public class ForkEnvironment
	{

		public const string DOCKER_FORK_WINDOWS2LINUX = "pa.scheduler.task.docker.windows2linux";

		/// <summary>
		/// Path to directory with Java installed, to this path '/bin/java' will be added.
		/// If the path is null only 'java' command will be called.
		/// </summary>
		private string javaHome;

		/// <summary>
		/// Path to the working directory.
		/// </summary>
		private string workingDir;

		/// <summary>
		/// User custom system environment.
		/// </summary>
		private IDictionary<string, string> systemEnvironment;

		/// <summary>
		/// Arguments passed to Java (not an application) (example: memory settings or properties).
		/// </summary>
		private IList<string> jvmArguments;

		/// <summary>
		/// Additional classpath when new JVM will be started.
		/// </summary>
		private IList<string> additionalClasspath;

		/// <summary>
		/// EnvScript: can be used to initialize environment just before JVM fork.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: private org.ow2.proactive.scripting.Script<?> script;
		private Script script;

		/// <summary>
		/// Command and parameters to add before java executable
		/// </summary>
		private IList<string> preJavaCommand;

		/// <summary>
		/// Does the current fork environment aims at running a linux docker container on a windows host?
		/// </summary>
		private bool isDockerWindowsToLinux = false;

		public ForkEnvironment()
		{
			additionalClasspath = new List<string>();
			jvmArguments = new List<string>();
			systemEnvironment = new Dictionary<string, string>();
			preJavaCommand = new List<string>();
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="forkEnvironment"> the object to copy </param>
		/// <exception cref="ExecutableCreationException"> script copy failed </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public ForkEnvironment(ForkEnvironment forkEnvironment) throws org.ow2.proactive.scheduler.common.exception.ExecutableCreationException
		public ForkEnvironment(ForkEnvironment forkEnvironment) : this()
		{

			if (!string.ReferenceEquals(forkEnvironment.javaHome, null))
			{
				this.javaHome = forkEnvironment.javaHome;
			}

			if (!string.ReferenceEquals(forkEnvironment.workingDir, null))
			{
				this.workingDir = forkEnvironment.workingDir;
			}

			if (forkEnvironment.systemEnvironment != null)
			{
				this.systemEnvironment.PutAll(forkEnvironment.systemEnvironment);
			}

			if (forkEnvironment.jvmArguments != null)
			{
				foreach (string entry in forkEnvironment.jvmArguments)
				{
					this.addJVMArgument(entry);
				}
			}

			if (forkEnvironment.additionalClasspath != null)
			{
				foreach (string entry in forkEnvironment.additionalClasspath)
				{
					this.addAdditionalClasspath(entry);
				}
			}

			if (forkEnvironment.preJavaCommand != null)
			{
				this.PreJavaCommand = forkEnvironment.preJavaCommand;
			}

			if (forkEnvironment.script != null)
			{
				try
				{
					this.script = new SimpleScript(forkEnvironment.script);
				}
				catch (InvalidScriptException e)
				{
					throw new ExecutableCreationException("Failed to copy ForkEnvironment script", e);
				}
			}
		}

		public ForkEnvironment(string workingDir) : this()
		{
			this.workingDir = workingDir;
		}

		/// <summary>
		/// Returns the javaHome.
		/// </summary>
		/// <returns> the javaHome. </returns>
		public virtual string JavaHome
		{
			get
			{
				return javaHome;
			}
			set
			{
				this.javaHome = value;
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
				return workingDir;
			}
			set
			{
				this.workingDir = value;
			}
		}


		/// <summary>
		/// Return a copy of the system environment, empty map if no variables.
		/// </summary>
		/// <returns> a copy of the system environment, empty map if no variables. </returns>
		public virtual IDictionary<string, string> SystemEnvironment
		{
			get
			{
				if (systemEnvironment == null)
				{
					return new Dictionary<string, string>(0);
				}
    
				return new Dictionary<string, string>(systemEnvironment);
			}
		}

		/// <summary>
		/// Get the system environment variable value associated with the given name.
		/// </summary>
		/// <param name="name"> the name of the variable value to get </param>
		/// <returns> the system variable value associated with the given name, or {@code null} if the variable does not exist. </returns>
		public virtual string getSystemEnvironmentVariable(string name)
		{
			if (systemEnvironment == null)
			{
				return null;
			}

			return systemEnvironment[name];
		}

		/// <summary>
		/// Add a new system environment variables value from its name and value.
		/// </summary>
		/// <param name="name"> the name of the variable to add </param>
		/// <param name="value"> the the value associated to the given name </param>
		/// <exception cref="IllegalArgumentException"> if name is null
		/// </exception>
		/// <returns> the previous value associated to the system environment variable. </returns>
		public virtual string addSystemEnvironmentVariable(string name, string value)
		{
			if (string.ReferenceEquals(name, null))
			{
				throw new System.ArgumentException("Name cannot be null");
			}

			return systemEnvironment[name] = value;
		}

		/// <summary>
		/// Return a copy of the JVM arguments, empty list if no arguments.
		/// </summary>
		/// <returns> a copy of the JVM arguments, empty list if no arguments. </returns>
		public virtual IList<string> JVMArguments
		{
			get
			{
				if (jvmArguments == null)
				{
					return new List<string>(0);
				}
    
				return new List<string>(jvmArguments);
			}
		}

		/// <summary>
		/// Add a new JVM argument value. (-Dname=value, -Xmx=.., -server)
		/// </summary>
		/// <param name="value"> the value of the property to be added. </param>
		/// <exception cref="IllegalArgumentException"> if value is null. </exception>
		public virtual void addJVMArgument(string value)
		{
			if (value == null)
			{
				throw new NullReferenceException("value");
			}
			jvmArguments.Add(value);
		}

		/// <summary>
		/// Return a copy of the additional classpath, empty list if no arguments.
		/// </summary>
		/// <returns> a copy of the additional classpath, empty list if no arguments. </returns>
		public virtual IList<string> AdditionalClasspath
		{
			get
			{
				if (additionalClasspath == null)
				{
					return new List<string>(0);
				}
    
				return new List<string>(additionalClasspath);
			}
		}

		/// <summary>
		/// Add one or more classpath entries to the forked JVM classpath
		/// </summary>
		/// <param name="values"> one or more classpath entries </param>
		public virtual void addAdditionalClasspath(params string[] values)
		{
			foreach (string value in values)
			{
				addAdditionalClasspath(value);
			}
		}

		/// <summary>
		/// Add a new additional Classpath value.
		/// </summary>
		/// <param name="value"> the additional Classpath to add. </param>
		/// <exception cref="IllegalArgumentException"> if value is null. </exception>
		public virtual void addAdditionalClasspath(string value)
		{
			if (value == null)
			{
				throw new NullReferenceException("value");
			}
			this.additionalClasspath.Add(value);
		}

		/// <summary>
		/// Returns the list of (command + argument) which will be prepended to the java command
		/// e.g. ["docker", "run", "--rm"] </summary>
		/// <returns> a list containing the command + arguments </returns>
		public virtual IList<string> PreJavaCommand
		{
			get
			{
				return preJavaCommand;
			}
			set
			{
				this.preJavaCommand = value;
			}
		}


		/// <summary>
		/// Add an item to the list of (command + argument) which will be prepended to the java command </summary>
		/// <param name="commandOrParameter"> the command (e.g. "docker") or an argument (e.g. "run") </param>
		public virtual void addPreJavaCommand(string commandOrParameter)
		{
			this.preJavaCommand.Add(commandOrParameter);
		}

		/// <summary>
		/// Get the environment script.
		/// </summary>
		/// <returns> the environment script. </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: public org.ow2.proactive.scripting.Script<?> getEnvScript()
		public virtual Script EnvScript
		{
			get
			{
				return script;
			}
			set
			{
				this.script = value;
			}
		}


		/// <summary>
		/// Returns true if the current fork environment aims at running a linux docker container on a windows host </summary>
		/// <returns> isDockerWindowsToLinux </returns>
		public virtual bool DockerWindowsToLinux
		{
			get
			{
				return isDockerWindowsToLinux;
			}
			set
			{
				isDockerWindowsToLinux = value;
			}
		}


		public static string convertToLinuxPath(string windowsPath)
		{
			if (Regex.IsMatch(windowsPath, "[a-zA-Z]:.*"))
			{
				return "/" + windowsPath[0] + windowsPath.Substring(2).Replace("\\", "/");
			}
			else
			{
				return windowsPath.Replace("\\", "/");
			}
		}

		public static string convertToLinuxPathInJVMArgument(string jvmArgument)
		{
			if (jvmArgument.StartsWith("-D", StringComparison.Ordinal) && jvmArgument.Contains("="))
			{
				int equalSignPos = jvmArgument.IndexOf("=", StringComparison.Ordinal);
				return jvmArgument.Substring(0, equalSignPos + 1) + convertToLinuxClassPath(jvmArgument.Substring(equalSignPos + 1));
			}
			else
			{
				return jvmArgument;
			}
		}

		public static string convertToLinuxClassPath(string windowsClassPath)
		{
			IList<string> linuxClassPathEntries = new List<string>();
			foreach (string windowsPath in windowsClassPath.Split(";", true))
			{
				if (Regex.IsMatch(windowsPath, "[a-zA-Z]:.*"))
				{
					linuxClassPathEntries.Add("/" + windowsPath[0] + windowsPath.Substring(2).Replace("\\", "/"));
				}
				else
				{
					linuxClassPathEntries.Add(windowsPath.Replace("\\", "/"));
				}
			}
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
			return linuxClassPathEntries.Aggregate("", (s1, s2) => s1 + ":" + s2);
		}

		public override string ToString()
		{
			string nl = Environment.NewLine;
			return "ForkEnvironment {" + nl + "\tjavaHome = '" + javaHome + '\'' + nl + "\tisDockerWindowsToLinux = '" + isDockerWindowsToLinux + '\'' + nl + "\tworkingDir = '" + workingDir + '\'' + nl + "\tsystemEnvironment = " + systemEnvironment + nl + "\tjvmArguments = " + jvmArguments + nl + "\tadditionalClasspath = " + additionalClasspath + nl + "\tscript = " + (script != null ? script.display() : null) + nl + '}';
		}

	}

}