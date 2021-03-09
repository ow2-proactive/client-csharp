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
namespace org.ow2.proactive.scheduler.common.task.dataspaces
{
	/// <summary>
	/// FileSystemException exception thrown when errors happen with LocalSpace or RemoteSpace usage
	/// 
	/// @author The ProActive Team
	/// 
	/// </summary>
	public class FileSystemException : Exception
	{

		public FileSystemException() : base()
		{
		}

		public FileSystemException(string message) : base(message)
		{
		}

		public FileSystemException(string message, Exception cause) : base(message, cause)
		{
		}

		public FileSystemException(Exception cause) : base("", cause)
		{
		}

	}

}