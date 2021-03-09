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
namespace org.ow2.proactive.scheduler.common
{

	using NotConnectedException = org.ow2.proactive.scheduler.common.exception.NotConnectedException;
	using PermissionException = org.ow2.proactive.scheduler.common.exception.PermissionException;
	using SchedulerException = org.ow2.proactive.scheduler.common.exception.SchedulerException;


	/// <summary>
	/// Third-party credentials are key-value pairs associated to a particular user and saved in the database.
	/// They can be used for instance to authenticate to third-party services in tasks.
	/// </summary>
	public interface ThirdPartyCredentials
	{

		/// <summary>
		/// Stores a third-party credential key-value pair in the database.
		/// </summary>
		/// <param name="key"> the third-party credential key to store </param>
		/// <param name="value"> the third-party credential value to store, it will be encrypted </param>
		/// <exception cref="NotConnectedException"> if you are not authenticated. </exception>
		/// <exception cref="PermissionException"> if you can't access this particular method. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: void putThirdPartyCredential(String key, String value) throws org.ow2.proactive.scheduler.common.exception.SchedulerException;
		void putThirdPartyCredential(string key, string value);

		/// <returns> all third-party credential keys stored for the current user </returns>
		/// <exception cref="NotConnectedException"> if you are not authenticated. </exception>
		/// <exception cref="PermissionException"> if you can't access this particular method. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: java.util.Set<String> thirdPartyCredentialsKeySet() throws org.ow2.proactive.scheduler.common.exception.SchedulerException;
		ISet<string> thirdPartyCredentialsKeySet();

		/// 
		/// <param name="key"> the third-party credential key to remove </param>
		/// <exception cref="NotConnectedException"> if you are not authenticated. </exception>
		/// <exception cref="PermissionException"> if you can't access this particular method. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: void removeThirdPartyCredential(String key) throws org.ow2.proactive.scheduler.common.exception.SchedulerException;
		void removeThirdPartyCredential(string key);
	}

}