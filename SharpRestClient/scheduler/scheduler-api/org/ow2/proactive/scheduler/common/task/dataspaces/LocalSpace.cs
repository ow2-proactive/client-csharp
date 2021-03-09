using System.Collections.Generic;
using System.IO;

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
	/// LocalSpace is the interface used to access the local file space of the ProActive Node used to executed the Task
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 4.0
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public interface LocalSpace
	public interface LocalSpace
	{

		/// <summary>
		/// Returns a File object corresponding to the path given as parameter relative to the LocalSpace root </summary>
		/// <param name="path"> path to resolve </param>
		/// <returns> a File object </returns>
		/// <exception cref="FileSystemException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: java.io.File getFile(String path) throws FileSystemException;
		FileInfo getFile(string path);

		/// <summary>
		/// Returns a list of File objects found by resolving the pattern given in parameter  <br>
		/// The following special characters can be used : <br>
		///  ** matches zero or more directories <br>
		///  * matches zero or more characters<br>
		///  ? matches one character
		/// 
		/// </summary>
		/// <param name="pattern"> pattern to locate files </param>
		/// <returns> a set of File objects </returns>
		/// <exception cref="FileSystemException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: java.util.Set<java.io.File> getFiles(String pattern) throws FileSystemException;
		ISet<FileInfo> getFiles(string pattern);

		/// <summary>
		/// Deletes all files found by resolving the given pattern
		/// The following special characters can be used : <br>
		///  ** matches zero or more directories <br>
		///  * matches zero or more characters<br>
		///  ? matches one character </summary>
		/// <param name="pattern"> pattern to locate files </param>
		/// <exception cref="FileSystemException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: void deleteFiles(String pattern) throws FileSystemException;
		void deleteFiles(string pattern);

		/// <summary>
		/// Delete the given file or folder(including its content) inside the local space </summary>
		/// <param name="file"> path to the local file (relative to the LocalSpace root) to delete </param>
		/// <exception cref="FileSystemException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: void deleteFile(java.io.File file) throws FileSystemException;
		void deleteFile(File file);

		/// <summary>
		/// Returns the root File of the LocalSpace </summary>
		/// <returns> root File object </returns>
		/// <exception cref="FileSystemException"> </exception>
		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: java.io.File getLocalRoot() throws FileSystemException;
		FileInfo LocalRoot {get;}
	}

}