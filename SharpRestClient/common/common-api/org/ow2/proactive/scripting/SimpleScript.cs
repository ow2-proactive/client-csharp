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
namespace org.ow2.proactive.scripting
{



	/// <summary>
	/// A simple script implementation
	/// 
	/// @author ProActive team
	/// 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI @XmlAccessorType(XmlAccessType.FIELD) public class SimpleScript extends Script<Object>
	[Serializable]
	public class SimpleScript : Script<object>
	{

		/// <summary>
		/// Directly create a script with a string. </summary>
		/// <param name="script"> a String containing script code </param>
		/// <param name="engineName"> script's engine execution name. </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SimpleScript(String script, String engineName) throws InvalidScriptException
		public SimpleScript(string script, string engineName) : base(script, engineName)
		{
		}

		/// <summary>
		/// Directly create a script from an URL. </summary>
		/// <param name="url"> representing a script source code. </param>
		/// <param name="engineName"> script's engine execution name. </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SimpleScript(java.net.URL url, String engineName) throws InvalidScriptException
		public SimpleScript(Uri url, string engineName) : base(url, engineName, false)
		{
		}

		/// <summary>
		/// Create a script from a file. </summary>
		/// <param name="file"> a file containing script code. </param>
		/// <param name="parameters"> execution parameters </param>
		/// <exception cref="InvalidScriptException"> if creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SimpleScript(java.io.File file, java.io.Serializable[] parameters) throws InvalidScriptException
		public SimpleScript(FileInfo file, string[] parameters) : base(file, parameters)
		{
		}

		/// <summary>
		/// Create a script from an URL. </summary>
		/// <param name="url"> an URL containing script code. </param>
		/// <param name="parameters"> execution parameters </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SimpleScript(java.net.URL url, java.io.Serializable[] parameters) throws InvalidScriptException
		public SimpleScript(Uri url, string[] parameters) : base(url, parameters, false)
		{
			// standard scripts (i.e. other than Selection scripts) use url late binding by default
		}

		/// <summary>
		/// Create a script from an URL. </summary>
		/// <param name="url"> an URL containing script code. </param>
		/// <param name="engineName"> script's engine execution name. </param>
		/// <param name="parameters"> execution parameters </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SimpleScript(java.net.URL url, String engineName, java.io.Serializable[] parameters) throws InvalidScriptException
		public SimpleScript(Uri url, string engineName, string[] parameters) : base(url, engineName, parameters, false)
		{
		}

		/// <summary>
		/// Directly create a script with a string. </summary>
		/// <param name="script"> a String containing script code </param>
		/// <param name="engineName"> script's engine execution name. </param>
		/// <param name="parameters"> execution parameters </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SimpleScript(String script, String engineName, java.io.Serializable[] parameters) throws InvalidScriptException
		public SimpleScript(string script, string engineName, string[] parameters) : base(script, engineName, parameters)
		{
		}

		protected internal override string DefaultScriptName
		{
			get
			{
				return "SimpleScript";
			}
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="original"> script to copy </param>
		/// <exception cref="InvalidScriptException">  </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SimpleScript(Script<?> original) throws InvalidScriptException
//JAVA TO C# CONVERTER TODO TASK: Wildcard generics in constructor parameters are not converted. Move the generic type parameter and constraint to the class header:
		public SimpleScript(Script<object> original) : base(original)
		{
		}

		/// <seealso cref= org.ow2.proactive.scripting.Script#getId() </seealso>
		public override string Id
		{
			get
			{
				return this.id;
			}
		}

		protected internal override TextReader Reader
		{
			get
			{
				return new StringReader(script);
			}
		}
	}

}