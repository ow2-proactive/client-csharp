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
namespace org.ow2.proactive.scheduler.common
{


	/// <summary>
	/// Constant types in the Scheduler.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 0.9.1
	/// 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public class SchedulerConstants
	public class SchedulerConstants
	{

		/// <summary>
		/// Default scheduler node name </summary>
		public const string SCHEDULER_DEFAULT_NAME = "SCHEDULER";

		/// <summary>
		/// Default name for store active object binding * </summary>
		public const string SYNCHRONIZATION_DEFAULT_NAME = "SYNCHRONIZATION";

		/// <summary>
		/// Default job name </summary>
		public const string JOB_DEFAULT_NAME = "NOT SET";

		/// <summary>
		/// Default task name </summary>
		public const string TASK_DEFAULT_NAME = "NOT SET";

		/// <summary>
		/// If the task name is not set, this is the generated one </summary>
		public const string TASK_NAME_IFNOTSET = "task_";

		/// <summary>
		/// Name of the environment variable for windows home directory on the common file system. </summary>
		public const string WINDOWS_HOME_ENV_VAR = "WINDOWS_HOME";

		/// <summary>
		/// Name of the environment variable for unix home directory on the common file system. </summary>
		public const string UNIX_HOME_ENV_VAR = "UNIX_HOME";

		/// <summary>
		/// Name of the GlobalSpace for DataSpaces registration </summary>
		public const string GLOBALSPACE_NAME = "GLOBALSPACE";

		/// <summary>
		/// Name of the UserSpace for DataSpaces registration </summary>
		public const string USERSPACE_NAME = "USERSPACE";

		/// <summary>
		/// Default taskid directory name (used in TaskLauncher) </summary>
		public const string TASKID_DIR_DEFAULT_NAME = "TASKID";

		public const string MULTI_NODE_TASK_NODESURL_BINDING_NAME = "nodesurl";

		public const string VARIABLES_BINDING_NAME = "variables";

		public const string GENERIC_INFO_BINDING_NAME = "genericInformation";

		public const string RESULT_MAP_BINDING_NAME = "resultMap";

		public const string DS_SCRATCH_BINDING_NAME = "localspace";

		public const string DS_CACHE_BINDING_NAME = "cachespace";

		public const string DS_INPUT_BINDING_NAME = "inputspace";

		public const string DS_OUTPUT_BINDING_NAME = "outputspace";

		public const string DS_GLOBAL_BINDING_NAME = "globalspace";

		public const string DS_USER_BINDING_NAME = "userspace";

		public const string DS_GLOBAL_API_BINDING_NAME = "globalspaceapi";

		public const string DS_USER_API_BINDING_NAME = "userspaceapi";

		public const string FORK_ENVIRONMENT_BINDING_NAME = "forkEnvironment";

		/// <summary>
		/// The variable containing a proxy to the scheduler server
		/// </summary>
		public const string SCHEDULER_CLIENT_BINDING_NAME = "schedulerapi";

		public const string RM_CLIENT_BINDING_NAME = "rmapi";

		/// <summary>
		/// The variable containing a proxy to the key/value store
		/// </summary>
		public const string SYNCHRONIZATION_API_BINDING_NAME = "synchronizationapi";

		/// <summary>
		/// The variable containing a proxy to the signal API
		/// </summary>
		public const string SIGNAL_API_BINDING_NAME = "signalapi";

		/// <summary>
		/// Marker in the task output to locate the remote connection hint
		/// 
		/// for the hint to be detected client side, the following String has 
		/// to be printed to the task log :
		/// 
		///  PA_REMOTE_CONNECTION;TaskId;type;url
		/// 
		///  example : 'PA_REMOTE_CONNECTION;10005;vnc;localhost:5901'
		/// </summary>
		public const string REMOTE_CONNECTION_MARKER = "PA_REMOTE_CONNECTION";

		/// <summary>
		/// Separator character for the String located by the <seealso cref="REMOTE_CONNECTION_MARKER"/> </summary>
		public const char REMOTE_CONNECTION_SEPARATOR = ';';

		/// <summary>
		/// Attribute name in task the generic information indicating that the task requires a node protedcted by token </summary>
		public const string NODE_ACCESS_TOKEN = "NODE_ACCESS_TOKEN";

		/// <summary>
		/// Generic information containing the parent job id </summary>
		public const string PARENT_JOB_ID = "PARENT_JOB_ID";

		/// <summary>
		/// The variable name to access results from dependent tasks (an array).
		/// </summary>
		public const string RESULTS_VARIABLE = "results";

		/// <summary>
		/// The variable name to access result metadata from dependent tasks (a map).
		/// </summary>
		public const string RESULT_METADATA_VARIABLE = "resultMetadata";

		/// <summary>
		/// Specific metadata values
		/// </summary>
		public const string METADATA_CONTENT_TYPE = "content.type";

		public const string METADATA_FILE_NAME = "file.name";

		public const string METADATA_FILE_EXTENSION = "file.extension";

		/// <summary>
		/// The variable name to access the user's third party credentials.
		/// </summary>
		public const string CREDENTIALS_VARIABLE = "credentials";

		/// <summary>
		/// The variable used to get or set the task progress
		/// </summary>
		public const string PROGRESS_BINDING_NAME = "progress";

		/// <summary>
		/// The Application ID used by the scheduler for local Dataspaces
		/// </summary>
		public static string SCHEDULER_DATASPACE_APPLICATION_ID = "0";

		/// <summary>
		/// This generic information can be used to disable Process Tree Killer execution
		/// </summary>
		public static string DISABLE_PROCESS_TREE_KILLER_GENERIC_INFO = "DISABLE_PTK";

		/// <summary>
		/// This generic information can be used to configure a task walltime
		/// </summary>
		public static string TASK_WALLTIME_GENERIC_INFO = "WALLTIME";

	}

}