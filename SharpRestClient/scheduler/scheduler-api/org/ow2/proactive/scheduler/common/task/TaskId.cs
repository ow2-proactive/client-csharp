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

	using JobId = org.ow2.proactive.scheduler.common.job.JobId;
	using FlowActionType = org.ow2.proactive.scheduler.common.task.flow.FlowActionType;


	/// <summary>
	/// Definition of a task identification.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 0.9
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public interface TaskId extends Comparable<TaskId>, java.io.Serializable
	public interface TaskId : IComparable<TaskId>
	{

		/// <summary>
		/// Returns the job identifier associated with this task.
		/// </summary>
		/// <returns> the job identifier associated with this task. </returns>
		JobId JobId {get;}

		/// <summary>
		/// Return the human readable name associated with this id.
		/// </summary>
		/// <returns> the human readable name associated with this id. </returns>
		string ReadableName {get;}

		/// <summary>
		/// Return the tag of the task. </summary>
		/// <returns> the tag of the task. </returns>
		string Tag {get;}

		/// <summary>
		/// Returns a String representation that is unique for the job
		/// it is related to. Please look at <seealso cref="toString"/> for a String
		/// representation unique for the scheduler instance it was built for.
		/// </summary>
		/// <returns> a String representation that is unique for the job
		/// it is related to only. </returns>
		string value();

		/// <summary>
		/// Returns a long representation that is unique for the job
		/// it is related to.
		/// </summary>
		/// <returns> a long representation that is unique for the job
		/// it is related to. </returns>
		long longValue();

		/// <summary>
		/// When Control Flow actions are performed on Tasks, some tasks are replicated. 
		/// A task replicated by a <seealso cref="FlowActionType.IF"/> action
		/// is differentiated from the original by an incremented Iteration Index.
		/// This index is reflected in the readable name of the Task's id (<seealso cref="getReadableName()"/>),
		/// this methods safely extracts it and returns it as an int.
		/// </summary>
		/// <returns> the iteration number of this task if it was replicated by a IF flow operation ({@code >= 0}) </returns>
		int IterationIndex {get;}

		/// <summary>
		/// When Control Flow actions are performed on Tasks, some tasks are replicated. 
		/// A task replicated by a <seealso cref="FlowActionType.REPLICATE"/> action
		/// is differentiated from the original by an incremented Replication Index.
		/// This index is reflected in the readable name of the Task's id (<seealso cref="getReadableName()"/>),
		/// this methods safely extracts it and returns it as an int.
		/// </summary>
		/// <returns> the iteration number of this task if it was replicated by a IF flow operation ({@code >= 0}) </returns>
		int ReplicationIndex {get;}

		/// <summary>
		/// Returns a String representation that is unique for
		/// the scheduler instance it was built for.
		/// </summary>
		/// <returns> a String representation that is unique for
		/// the scheduler instance it was built for. </returns>
		string ToString();

	}

}