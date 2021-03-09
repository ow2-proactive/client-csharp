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
namespace org.ow2.proactive.scheduler.common.util
{
	using JobId = org.ow2.proactive.scheduler.common.job.JobId;
	using TaskId = org.ow2.proactive.scheduler.common.task.TaskId;


	public class TaskLoggerRelativePathGenerator
	{

		// the prefix for log file produced in localspace
		private const string LOG_FILE_PREFIX = "TaskLogs";

		private readonly string relativePath;

		private readonly string fileName;

		public TaskLoggerRelativePathGenerator(TaskId taskId)
		{
			this.fileName = LOG_FILE_PREFIX + "-" + taskId.JobId + "-" + taskId.value() + ".log";
			this.relativePath = taskId.JobId.ToString() + "/" + fileName;
		}

		/// <summary>
		/// Return an include pattern to find all log files associated with the given job id </summary>
		/// <param name="jobId"> id of a job </param>
		/// <returns> an include pattern; </returns>
		public static string getIncludePatternForAllLogFiles(JobId jobId)
		{
			return jobId.ToString() + "/" + LOG_FILE_PREFIX + "-" + jobId.ToString() + "-*.log";
		}

		/// <summary>
		/// Return the containing folder of log files for a given job id </summary>
		/// <param name="jobId"> id of the job associated with logs </param>
		/// <returns> a containing folder relative path </returns>
		public static string getContainingFolderForLogFiles(JobId jobId)
		{
			return jobId.ToString();
		}

		public virtual string RelativePath
		{
			get
			{
				return this.relativePath;
			}
		}

		public virtual string FileName
		{
			get
			{
				return this.fileName;
			}
		}

	}

}