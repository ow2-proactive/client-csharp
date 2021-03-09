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
namespace org.ow2.proactive.authentication
{


	/// <summary>
	/// A class representing a user information
	/// 
	/// @author ActiveEon Team
	/// @since 28/07/2017
	/// </summary>
	[Serializable]
	public class UserData
	{

		public UserData() : base()
		{
		}

		private string userName;

		private ISet<string> groups;

		public virtual string UserName
		{
			get
			{
				return userName;
			}
			set
			{
				this.userName = value;
			}
		}


		public virtual ISet<string> Groups
		{
			get
			{
				return groups;
			}
			set
			{
				this.groups = value;
			}
		}

	}

}