using System;

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


	using TaskFlowJob = org.ow2.proactive.scheduler.common.job.TaskFlowJob;


	/// <summary>
	/// Use this class to build a native task that will use a 'org.ow2.proactive.scheduler.task.NativeExecutable' and be integrated in a <seealso cref="TaskFlowJob"/>.
	/// <para>
	/// A native task just includes a command line that can be set using <seealso cref="setCommandLine(string[])"/>.
	/// </para>
	/// <para>
	/// You don't have to extend this class to launch your own native executable.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 0.9
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public class NativeTask extends Task
	[Serializable]
	public class NativeTask : Task
	{

		/// <summary>
		/// Command line for this native task </summary>
		private string[] commandLine = null;

		/// <summary>
		/// Empty constructor.
		/// </summary>
		public NativeTask()
		{
		}

		/// <summary>
		/// Get the command line for this task.
		/// </summary>
		/// <returns> the command line </returns>
		public virtual string[] CommandLine
		{
			get
			{
				return commandLine;
			}
			set
			{
				this.commandLine = value;
			}
		}


		public override string display()
		{
			string nl = Environment.NewLine;

			return base.display() + nl + "\tCommandLine=" + String.Join(",", commandLine);
		}

	}

}