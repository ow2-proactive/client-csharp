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


	/// <summary>
	/// SchedulerUser is an internal representation of a user.<br>
	/// It provides some information like user name, admin status, etc...
	/// 
	/// @author The ProActive Team
	/// 
	/// $Id$
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public abstract class UserIdentification implements java.io.Serializable, Comparable<UserIdentification>
	[Serializable]
	public abstract class UserIdentification : IComparable<UserIdentification>
	{

		/// <summary>
		/// Value for </summary>
		public const int SORT_BY_NAME = 1;

		public const int SORT_BY_SUBMIT = 3;

		public const int SORT_BY_HOST = 4;

		public const int SORT_BY_CONNECTION = 5;

		public const int SORT_BY_LASTSUBMIT = 6;

		public const int ASC_ORDER = 1;

		public const int DESC_ORDER = 2;

		private static int currentSort = SORT_BY_NAME;

		private static int currentOrder = ASC_ORDER;

		protected internal volatile bool toRemove = false;

		/// <summary>
		/// To get the user name
		/// </summary>
		/// <returns> the user name </returns>
		public abstract string Username {get;}

		/// <summary>
		/// To get the groups associated with this user name
		/// </summary>
		/// <returns> a set of groups </returns>
		public abstract ISet<string> Groups {get;}

		/// <summary>
		/// Get the number of submit for this user.
		/// </summary>
		/// <returns> the number of submit for this user. </returns>
		public abstract int SubmitNumber {get;}

		/// <summary>
		/// Get the host name of this user.
		/// </summary>
		/// <returns> the host name of this user. </returns>
		public abstract string HostName {get;}

		/// <summary>
		/// Get the time of the connection of this user.
		/// </summary>
		/// <returns> the time of the connection of this user. </returns>
		public abstract long ConnectionTime {get;}

		/// <summary>
		/// Get the last time this user has submit a job.
		/// </summary>
		/// <returns> the last time this user has submit a job. </returns>
		public abstract long LastSubmitTime {get;}

		/// <summary>
		/// Get the myEventsOnly.
		/// </summary>
		/// <returns> the myEventsOnly. </returns>
		public abstract bool MyEventsOnly {get;}

		/// <summary>
		/// Set the field to sort on.
		/// </summary>
		/// <param name="sortBy">
		///            the field on which the sort will be made. </param>
		public static int SortingBy
		{
			set
			{
				currentSort = value;
			}
		}

		/// <summary>
		/// Set the order for the next sort.
		/// </summary>
		/// <param name="order"> the new order to set. </param>
		public static int SortingOrder
		{
			set
			{
				if ((value == ASC_ORDER) || (value == DESC_ORDER))
				{
					currentOrder = value;
				}
				else
				{
					currentOrder = ASC_ORDER;
				}
			}
		}

		/// <seealso cref= java.lang.Comparable#compareTo(java.lang.Object) </seealso>
		/// <param name="user"> The user to compare to <i>this</i> user. </param>
		/// <returns>  a negative integer, zero, or a positive integer as this object
		///		is less than, equal to, or greater than the specified object. </returns>
		public virtual int CompareTo(UserIdentification user)
		{
			switch (currentSort)
			{
				case SORT_BY_SUBMIT:
					return (currentOrder == ASC_ORDER) ? SubmitNumber - user.SubmitNumber : user.SubmitNumber - SubmitNumber;
				case SORT_BY_HOST:
					return (currentOrder == ASC_ORDER) ? HostName.CompareTo(user.HostName) : user.HostName.CompareTo(HostName);
				case SORT_BY_CONNECTION:
					return (currentOrder == ASC_ORDER) ? (int)(ConnectionTime - user.ConnectionTime) : (int)(user.ConnectionTime - ConnectionTime);
				case SORT_BY_LASTSUBMIT:
					return (currentOrder == ASC_ORDER) ? (int)(LastSubmitTime - user.LastSubmitTime) : (int)(user.LastSubmitTime - LastSubmitTime);
				default:
					return (currentOrder == ASC_ORDER) ? HostName.CompareTo(user.HostName) : user.HostName.CompareTo(HostName);
			}
		}

		/// <summary>
		/// Returns true if this user has to be removed, false if not.
		/// </summary>
		/// <returns> true if this user has to be removed, false if not. </returns>
		public virtual bool ToRemove
		{
			get
			{
				return toRemove;
			}
		}

	}

}