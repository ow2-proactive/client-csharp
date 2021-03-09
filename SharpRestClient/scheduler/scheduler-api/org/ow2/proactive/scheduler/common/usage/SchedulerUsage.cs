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
namespace org.ow2.proactive.scheduler.common.usage
{
	using NotConnectedException = org.ow2.proactive.scheduler.common.exception.NotConnectedException;
	using PermissionException = org.ow2.proactive.scheduler.common.exception.PermissionException;


	/// <summary>
	/// Scheduler interface for accounting information, usage data and statistics.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 3.4
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public interface SchedulerUsage
	public interface SchedulerUsage
	{

		/// <summary>
		/// Returns details on job and task execution times for the caller's executions.
		/// <para>
		/// Only the jobs finished between the start date and the end date will be returned:
		/// i.e {@code startDate <= job.finishedTime <= endDate}.
		/// </para> </summary>
		/// <param name="startDate"> must not be null, inclusive </param>
		/// <param name="endDate"> must not be null, inclusive </param>
		/// <returns> a list of <seealso cref="JobUsage"/> objects where job finished times are between start date and end date </returns>
		/// <exception cref="NotConnectedException"> if the caller is not connected </exception>
		/// <exception cref="PermissionException"> if the caller hasn't the permission to call this method </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: java.util.List<JobUsage> getMyAccountUsage(java.util.Date startDate, java.util.Date endDate) throws org.ow2.proactive.scheduler.common.exception.NotConnectedException, org.ow2.proactive.scheduler.common.exception.PermissionException;
		IList<JobUsage> getMyAccountUsage(DateTime startDate, DateTime endDate);

		/// <summary>
		/// Returns details on job and task execution times for a given user's executions.
		/// <para>
		/// Only the jobs finished between the start date and the end date will be returned:
		/// i.e {@code startDate <= job.finishedTime <= endDate}.
		/// </para>
		/// <para>
		/// If user is the same as the caller, then it will fallback to to <seealso cref="getMyAccountUsage(System.DateTime, System.DateTime)"/>.
		/// 
		/// </para>
		/// </summary>
		/// <param name="user"> must match a username as defined in the Scheduler's users </param>
		/// <param name="startDate"> must not be null, inclusive </param>
		/// <param name="endDate"> must not be null, inclusive </param>
		/// <returns> a list of <seealso cref="JobUsage"/> objects where job finished times are between start date and end date </returns>
		/// <exception cref="NotConnectedException"> if the caller is not connected </exception>
		/// <exception cref="PermissionException"> if the caller hasn't the permission to call this method </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: java.util.List<JobUsage> getAccountUsage(String user, java.util.Date startDate, java.util.Date endDate) throws org.ow2.proactive.scheduler.common.exception.NotConnectedException, org.ow2.proactive.scheduler.common.exception.PermissionException;
		IList<JobUsage> getAccountUsage(string user, DateTime startDate, DateTime endDate);
	}

}