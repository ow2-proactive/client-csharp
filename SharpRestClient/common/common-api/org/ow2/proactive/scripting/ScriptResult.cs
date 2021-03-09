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
namespace org.ow2.proactive.scripting
{


	/// 
	/// <summary>
	/// The class implements a script result container.
	/// The script result is an object typed with the template class, or
	/// an exception raised by the script execution.
	/// @author ProActive team
	/// </summary>
	/// @param <E> template class for the result. </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public class ScriptResult<E> implements java.io.Serializable
	[Serializable]
	public class ScriptResult<E> where E : class
	{

		/// <summary>
		/// Result of the script </summary>
		private readonly E result;

		/// <summary>
		/// Exception in the result if so </summary>
		private readonly Exception exception;

		/// <summary>
		/// Output of the script </summary>
		private string output;

		/// <summary>
		/// Host on which the script was executed </summary>
		private string hostname;

		/// <summary>
		/// ProActive empty constructor
		/// </summary>
		public ScriptResult()
		{
			this.result = default(E);
			this.exception = null;
		}

		/// <summary>
		/// Create a new instance of ScriptResult. </summary>
		/// <param name="result"> result to store </param>
		public ScriptResult(E result) : this(result, null)
		{
		}

		/// <summary>
		/// Create a new instance of ScriptResult. </summary>
		/// <param name="exception"> script exception </param>
		public ScriptResult(Exception exception) : this(null, exception)
		{
		}

		/// <summary>
		/// Constructor </summary>
		/// <param name="result"> result to store </param>
		/// <param name="exception"> eventual exception representing the result </param>
		public ScriptResult(E result, Exception exception)
		{
			this.result = result;
			this.exception = exception;
		}

		/// <summary>
		/// tell if an exception has been raised during
		/// script execution. </summary>
		/// <returns> true if an exception occured, false otherwise. </returns>
		public virtual bool errorOccured()
		{
			return exception != null;
		}

		/// <summary>
		/// Return the eventual exception of the script's execution. </summary>
		/// <returns> Throwable representing the exception. </returns>
		public virtual Exception Exception
		{
			get
			{
				return exception;
			}
		}

		/// <summary>
		/// Return the result's object </summary>
		/// <returns> result object. </returns>
		public virtual E Result
		{
			get
			{
				return result;
			}
		}

		/// <summary>
		/// Return the script's output. </summary>
		/// <returns> output string </returns>
		public virtual string Output
		{
			get
			{
				return output;
			}
			set
			{
				this.output = value;
			}
		}


		/// <summary>
		/// Return the name of the host that executes the script </summary>
		/// <returns> script execution host name </returns>
		public virtual string Hostname
		{
			get
			{
				return hostname;
			}
			set
			{
				this.hostname = value;
			}
		}


	}

}