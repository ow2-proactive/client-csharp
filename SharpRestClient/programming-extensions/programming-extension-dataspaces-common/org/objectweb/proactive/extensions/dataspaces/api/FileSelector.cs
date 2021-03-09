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
namespace org.objectweb.proactive.extensions.dataspaces.api
{
	public enum FileSelector
	{

		/// <summary>
		/// A <seealso cref="FileSelector"/> that selects only the base file/folder.
		/// </summary>
		SELECT_SELF,

		/// <summary>
		/// A <seealso cref="FileSelector"/> that selects the base file/folder and its
		/// direct children.
		/// </summary>
		SELECT_SELF_AND_CHILDREN,

		/// <summary>
		/// A <seealso cref="FileSelector"/> that selects only the direct children
		/// of the base folder.
		/// </summary>
		SELECT_CHILDREN,

		/// <summary>
		/// A <seealso cref="FileSelector"/> that selects all the descendents of the
		/// base folder, but does not select the base folder itself.
		/// </summary>
		EXCLUDE_SELF,

		/// <summary>
		/// A <seealso cref="FileSelector"/> that only files (not folders).
		/// </summary>
		SELECT_FILES,

		/// <summary>
		/// A <seealso cref="FileSelector"/> that only folders (not files).
		/// </summary>
		SELECT_FOLDERS,

		/// <summary>
		/// A <seealso cref="FileSelector"/> that selects the base file/folder, plus all
		/// its descendents.
		/// </summary>
		SELECT_ALL
	}

}