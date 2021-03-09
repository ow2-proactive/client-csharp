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

	using NotConnectedException = org.ow2.proactive.scheduler.common.exception.NotConnectedException;
	using PermissionException = org.ow2.proactive.scheduler.common.exception.PermissionException;


	/// <summary>
	/// RemoteSpace is the interface used to access remote file spaces
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 4.0
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public interface RemoteSpace
	public interface RemoteSpace
	{

		/// <summary>
		/// List the content of the given remote directory, using a glob pattern
		/// <para>
		/// The following special characters can be used inside the pattern : <br>
		/// ** matches zero or more directories <br>
		/// * matches zero or more characters<br>
		/// ? matches one character
		/// 
		/// </para>
		/// </summary>
		/// <param name="remotePath"> path in the RemoteSpace where files should be listed. Use "." for remote root path. </param>
		/// <param name="pattern">    pattern to locate files </param>
		/// <returns> a list of remote paths matching the pattern </returns>
		/// <exception cref="FileSystemException"> </exception>
		/// <seealso cref= "https://docs.oracle.com/javase/7/docs/api/java/nio/file/FileSystem.html#getPathMatcher(java.lang.String)" </seealso>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: java.util.List<String> listFiles(String remotePath, String pattern) throws FileSystemException;
		IList<string> listFiles(string remotePath, string pattern);

		/// <summary>
		/// Push the given file or directory(including its content) from the local space to the remote space.
		/// 
		/// When pushing a file or directory, the remotePath must contain the target new file or directory </summary>
		/// <param name="localPath"> path to the local file or directory </param>
		/// <param name="remotePath"> path in the RemoteSpace where to put the file or folder. Use "." for remote root path. </param>
		/// <exception cref="FileSystemException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: void pushFile(java.io.File localPath, String remotePath) throws FileSystemException;
		void pushFile(FileInfo localPath, string remotePath);

		/// <summary>
		/// Push the local files described by the given pattern to the remote space
		/// 
		///  The following special characters can be used inside the pattern : <br>
		///  ** matches zero or more directories <br>
		///  * matches zero or more characters<br>
		///  ? matches one character
		/// </summary>
		/// <param name="localDirectory"> local directory used as base </param>
		/// <param name="pattern"> pattern to locate files inside the localDirectory </param>
		/// <param name="remotePath"> path in the RemoteSpace where to put the files. Use "." for remote root path. </param>
		/// <exception cref="FileSystemException"> </exception>
		/// <seealso cref= "https://docs.oracle.com/javase/7/docs/api/java/nio/file/FileSystem.html#getPathMatcher(java.lang.String)" </seealso>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: void pushFiles(java.io.File localDirectory, String pattern, String remotePath) throws FileSystemException;
		void pushFiles(FileInfo localDirectory, string pattern, string remotePath);

		/// <summary>
		/// Pull the given file or folder(including its content) from the remote space to the local space
		/// </summary>
		/// <param name="remotePath"> path to the remote file (relative to the RemoteSpace root) </param>
		/// <param name="localPath"> path in the local file system where to put the file or folder </param>
		/// <returns> the path to the file or directory pulled </returns>
		/// <exception cref="FileSystemException"> </exception>
		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: java.io.File pullFile(String remotePath, java.io.File localPath) throws FileSystemException;
		FileInfo pullFile(string remotePath, FileInfo localPath);

		/// <summary>
		/// Pull the remote files described by the given pattern to the local space
		/// 
		///  The following special characters can be used inside the pattern : <br>
		///  ** matches zero or more directories <br>
		///  * matches zero or more characters<br>
		///  ? matches one character
		/// </summary>
		/// <param name="remotePath"> path in the RemoteSpace used as base for the pattern (e.g. "/") </param>
		/// <param name="pattern"> pattern to locate files inside the RemoteSpace </param>
		/// <param name="localPath"> path in the local file system where to put the files </param>
		/// <returns> a set of files pulled </returns>
		/// <exception cref="FileSystemException"> </exception>
		/// <seealso cref= "https://docs.oracle.com/javase/7/docs/api/java/nio/file/FileSystem.html#getPathMatcher(java.lang.String)" </seealso>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: java.util.Set<java.io.File> pullFiles(String remotePath, String pattern, java.io.File localPath) throws FileSystemException;
		ISet<FileInfo> pullFiles(string remotePath, string pattern, FileInfo localPath);

		/// <summary>
		/// Delete the given file or folder(including its content) inside the remote space </summary>
		/// <param name="remotePath"> path to the remote file (relative to the RemoteSpace root) to delete </param>
		/// <exception cref="FileSystemException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: void deleteFile(String remotePath) throws FileSystemException;
		void deleteFile(string remotePath);

		/// <summary>
		/// Delete the remote files described by the given pattern
		/// 
		///  The following special characters can be used inside the pattern : <br>
		///  ** matches zero or more directories <br>
		///  * matches zero or more characters<br>
		///  ? matches one character
		/// </summary>
		/// <param name="remotePath"> path in the RemoteSpace used as base for the pattern (e.g. "/") </param>
		/// <param name="pattern"> pattern to locate files inside the RemoteSpace </param>
		/// <exception cref="FileSystemException"> </exception>
		/// <seealso cref= "https://docs.oracle.com/javase/7/docs/api/java/nio/file/FileSystem.html#getPathMatcher(java.lang.String)" </seealso>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: void deleteFiles(String remotePath, String pattern) throws FileSystemException;
		void deleteFiles(string remotePath, string pattern);

		/// <summary>
		/// Returns the URLs of the RemoteSpace </summary>
		/// <returns> URL to the space </returns>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: java.util.List<String> getSpaceURLs() throws org.ow2.proactive.scheduler.common.exception.NotConnectedException, org.ow2.proactive.scheduler.common.exception.PermissionException, FileSystemException;
		IList<string> SpaceURLs {get;}

		/// <summary>
		/// Returns an input stream on the specified remote file. It will throw an exception if the file does not exist. </summary>
		/// <param name="remotePath"> path to the remote file </param>
		/// <returns> an input stream </returns>
		/// <exception cref="FileSystemException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: java.io.InputStream getInputStream(String remotePath) throws FileSystemException;
		Stream getInputStream(string remotePath);

		/// <summary>
		/// Returns an output stream on the specified remote file. If the file does not exist, it will be created. </summary>
		/// <param name="remotePath"> path to the remote file </param>
		/// <returns> an output stream </returns>
		/// <exception cref="FileSystemException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: java.io.OutputStream getOutputStream(String remotePath) throws FileSystemException;
		Stream getOutputStream(string remotePath);

	}

}