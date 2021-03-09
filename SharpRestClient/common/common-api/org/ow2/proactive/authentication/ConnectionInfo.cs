using System;
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
namespace org.ow2.proactive.authentication
{


	/// <summary>
	/// Composite object storing information to connect to the scheduler/rm
	/// 
	/// @author The ProActive Team
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public class ConnectionInfo implements java.io.Serializable
	[Serializable]
	public class ConnectionInfo
	{
		private string url;

		private string login;

		private string password;

		private FileInfo credentialFile;

		private bool insecure;

		/// <param name="url">            the REST server URL </param>
		/// <param name="login">          the login </param>
		/// <param name="password">       the password </param>
		/// <param name="credentialFile"> path to a file containing encrypted credentials </param>
		/// <param name="insecure">       if true the server certificate will not be verified </param>
		public ConnectionInfo(string url, string login, string password, FileInfo credentialFile, bool insecure)
		{
			this.url = url;
			this.login = login;
			this.password = password;
			this.credentialFile = credentialFile;
			this.insecure = insecure;
		}

		public virtual string Url
		{
			get
			{
				return url;
			}
			set
			{
				this.url = value;
			}
		}

		public virtual string Login
		{
			get
			{
				return login;
			}
			set
			{
				this.login = value;
			}
		}

		public virtual string Password
		{
			get
			{
				return password;
			}
			set
			{
				this.password = value;
			}
		}

		public virtual FileInfo CredentialFile
		{
			get
			{
				return credentialFile;
			}
			set
			{
				this.credentialFile = value;
			}
		}

		public virtual bool Insecure
		{
			get
			{
				return insecure;
			}
			set
			{
				this.insecure = value;
			}
		}





	}

}