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
namespace org.ow2.proactive.db
{
	/// <summary>
	/// DatabaseManagerException is thrown by the DataBaseManager when Hibernate exception occurs.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 0.9.1
	/// </summary>
	public class DatabaseManagerException : Exception
	{

		/// <summary>
		/// Create a new instance of DatabaseManagerException.
		/// </summary>
		public DatabaseManagerException() : base()
		{
		}

		/// <summary>
		/// Create a new instance of DatabaseManagerException.
		/// </summary>
		/// <param name="message"> the message to be display </param>
		/// <param name="cause"> the throwable that cause this exception </param>
		public DatabaseManagerException(string message, Exception cause) : base(message, cause)
		{
		}

		/// <summary>
		/// Create a new instance of DatabaseManagerException.
		/// </summary>
		/// <param name="message"> the message to be display </param>
		public DatabaseManagerException(string message) : base(message)
		{
		}

		/// <summary>
		/// Create a new instance of DatabaseManagerException.
		/// </summary>
		/// <param name="cause"> the throwable that cause this exception </param>
		public DatabaseManagerException(Exception cause) : base("", cause)
		{
		}

	}

}