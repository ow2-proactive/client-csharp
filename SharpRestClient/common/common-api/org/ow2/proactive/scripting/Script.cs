using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using System.Text;

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
	/// A simple script to evaluate using java 6 scripting API.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 0.9
	/// 
	/// </summary>
	/// @param <E> Template class's type of the result. </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public abstract class Script<E> implements java.io.Serializable
	[Serializable]
	public abstract class Script<E>
	{

		// default output size in chars
		public const int DEFAULT_OUTPUT_MAX_SIZE = 1024 * 1024; // 1 million characters ~ 2 Mb

		/// <summary>
		/// Variable name for script arguments </summary>
		public const string ARGUMENTS_NAME = "args";

		public const string MD5 = "MD5";

		/// <summary>
		/// Name of the script engine or file path to script file (extension will be used to lookup) </summary>
		protected internal string scriptEngineLookupName;

		/// <summary>
		/// The script to evaluate </summary>
		protected internal string script;

		/// <summary>
		/// url used to define this script, if aaplicable * </summary>
		protected internal Uri url;

		/// <summary>
		/// Id of this script </summary>
		protected internal string id;

		/// <summary>
		/// The parameters of the script </summary>
		protected internal string[] parameters;

		/// <summary>
		/// Name of the script * </summary>
		private string scriptName;

		private static bool? lazyFetch = null;

		/// <summary>
		/// ProActive needed constructor </summary>
		public Script()
		{
		}

		/// <summary>
		/// Directly create a script with a string. </summary>
		/// <param name="script"> String representing the script's source code </param>
		/// <param name="engineName"> String representing the execution engine </param>
		/// <param name="parameters"> script's execution arguments. </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public Script(String script, String engineName, java.io.Serializable[] parameters) throws InvalidScriptException
		public Script(string script, string engineName, string[] parameters)
		{
			this.scriptEngineLookupName = engineName;
			this.script = script;
			this.id = script;
			this.parameters = parameters;
			this.scriptName = DefaultScriptName;
		}

		protected internal abstract string DefaultScriptName {get;}

		/// <summary>
		/// Directly create a script with a string. </summary>
		/// <param name="script"> String representing the script's source code </param>
		/// <param name="engineName"> String representing the execution engine </param>
		/// <param name="parameters"> script's execution arguments. </param>
		/// <param name="scriptName"> name of the script </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public Script(String script, String engineName, java.io.Serializable[] parameters, String scriptName) throws InvalidScriptException
		public Script(string script, string engineName, string[] parameters, string scriptName)
		{
			this.scriptEngineLookupName = engineName;
			this.script = script;
			this.id = script;
			this.parameters = parameters;
			this.scriptName = scriptName;
		}

		/// <summary>
		/// Directly create a script with a string. </summary>
		/// <param name="script"> String representing the script's source code </param>
		/// <param name="engineName"> String representing the execution engine </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public Script(String script, String engineName) throws InvalidScriptException
		public Script(string script, string engineName) : this(script, engineName, (string[]) null)
		{
		}

		/// <summary>
		/// Directly create a script with a string. </summary>
		/// <param name="script"> String representing the script's source code </param>
		/// <param name="engineName"> String representing the execution engine </param>
		/// <param name="scriptName"> name of the script </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public Script(String script, String engineName, String scriptName) throws InvalidScriptException
		public Script(string script, string engineName, string scriptName) : this(script, engineName, null, scriptName)
		{
		}

		/// <summary>
		/// Create a script from a file.
		/// </summary>
		/// <param name="file"> a file containing the script's source code. </param>
		/// <param name="parameters"> script's execution arguments. </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public Script(java.io.File file, java.io.Serializable[] parameters) throws InvalidScriptException
		public Script(FileInfo file, string[] parameters)
		{
			this.scriptEngineLookupName = file.Extension;

			try
			{
				script = readFile(file);
			}
			catch (IOException e)
			{
				throw new InvalidScriptException("Unable to read script : " + file.FullName, e);
			}
			this.id = file.FullName;
			this.parameters = parameters;
			this.scriptName = file.Name;
		}

		/// <summary>
		/// Create a script from a file. </summary>
		/// <param name="file"> a file containing a script's source code. </param>
		/// <exception cref="InvalidScriptException"> if Constructor fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public Script(java.io.File file) throws InvalidScriptException
		public Script(FileInfo file) : this(file, null)
		{
		}

		/// <summary>
		/// Create a script from an URL. </summary>
		/// <param name="url"> representing a script source code. </param>
		/// <param name="parameters"> execution arguments. </param>
		/// <param name="fetchImmediately"> true if the script at the given url must be fetched when the job is parsed by the server, false if the script must be fetch only at execution time. </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public Script(java.net.URL url, java.io.Serializable[] parameters, boolean fetchImmediately) throws InvalidScriptException
		public Script(Uri url, string[] parameters, bool fetchImmediately) : this(url, null, parameters, fetchImmediately)
		{

		}

		/// <summary>
		/// Create a script from an URL. </summary>
		/// <param name="url"> representing a script source code. </param>
		/// <param name="engineName"> String representing the execution engine </param>
		/// <param name="parameters"> execution arguments. </param>
		/// <param name="fetchImmediately"> true if the script at the given url must be fetched when the job is parsed by the server, false if the script must be fetch only at execution time. </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public Script(java.net.URL url, String engineName, java.io.Serializable[] parameters, boolean fetchImmediately) throws InvalidScriptException
		public Script(Uri url, string engineName, string[] parameters, bool fetchImmediately)
		{
			this.url = url;
			this.id = url.AbsoluteUri;
			this.scriptEngineLookupName = engineName;
			this.parameters = parameters;
			this.scriptName = url.AbsolutePath;
		}

		/// <summary>
		/// Create a script from an URL. </summary>
		/// <param name="url"> representing a script source code. </param>
		/// <param name="fetchImmediately"> true if the script at the given url must be fetched when the job is parsed by the server, false if the script must be fetch only at execution time. </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public Script(java.net.URL url, boolean fetchImmediately) throws InvalidScriptException
		public Script(Uri url, bool fetchImmediately) : this(url, (string) null, fetchImmediately)
		{
		}

		/// <summary>
		/// Create a script from an URL. </summary>
		/// <param name="url"> representing a script source code. </param>
		/// <param name="engineName"> String representing the execution engine </param>
		/// <param name="fetchImmediately"> true if the script at the given url must be fetched when the job is parsed by the server, false if the script must be fetch only at execution time. </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public Script(java.net.URL url, String engineName, boolean fetchImmediately) throws InvalidScriptException
		public Script(Uri url, string engineName, bool fetchImmediately) : this(url, engineName, null, fetchImmediately)
		{
		}

		/// <summary>
		/// Create a script from another script object </summary>
		/// <param name="script2"> script object source </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public Script(Script<?> script2) throws InvalidScriptException
//JAVA TO C# CONVERTER TODO TASK: Wildcard generics in constructor parameters are not converted. Move the generic type parameter and constraint to the class header:
		public Script(Script<E> script2) : this(script2.GetScript(), script2.scriptEngineLookupName, script2.Parameters, script2.ScriptName)
		{
			this.url = script2.url;
			this.id = script2.id;
		}

		/// <summary>
		/// Create a script from another script object </summary>
		/// <param name="script2"> script object source </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public Script(Script<?> script2, String scriptName) throws InvalidScriptException
//JAVA TO C# CONVERTER TODO TASK: Wildcard generics in constructor parameters are not converted. Move the generic type parameter and constraint to the class header:
		public Script(Script<object> script2, string scriptName) : this(script2.script, script2.scriptEngineLookupName, script2.parameters, scriptName)
		{
			this.url = script2.url;
			this.id = script2.id;
		}

		/// <summary>
		/// Get the script.
		/// </summary>
		/// <returns> the script. </returns>
		public virtual string GetScript()
		{
			return script;
		}

		/// <summary>
		/// Get the script.
		/// </summary>
		/// <returns> the script. </returns>
		public virtual void SetScript(string value)
		{
			this.script = value;
		}

		/// <summary>
		/// Get the script name.
		/// </summary>
		/// <returns> the script name. </returns>
		public virtual string ScriptName
		{
			get
			{
				return scriptName;
			}
		}

		/// <summary>
		/// Get the script url, if applicable </summary>
		/// <returns> the script url </returns>
		public virtual Uri ScriptUrl
		{
			get
			{
				return url;
			}
		}


		/// <summary>
		/// Get the parameters.
		/// </summary>
		/// <returns> the parameters. </returns>
		public virtual string[] Parameters
		{
			get
			{
				return parameters;
			}
		}

		/// <summary>
		/// String identifying the script. </summary>
		/// <returns> a String identifying the script. </returns>
		public virtual string Id
		{
			get
			{
				return this.id;
			}
		}

		/// <summary>
		/// The reader used to read the script. </summary>
		protected internal virtual TextReader Reader
		{
			get
			{
				return new StringReader(this.script);
			}
		}

		/// <summary>
		/// Create string script from file </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static String readFile(java.io.File file) throws java.io.IOException
		public static string readFile(FileInfo file)
		{
			using (StreamReader buf = new StreamReader(new FileStream(file.FullName, FileMode.Open, FileAccess.Read)))
			{
				StringBuilder builder = new StringBuilder();
				string tmp;

				while (!string.ReferenceEquals((tmp = buf.ReadLine()), null))
				{
					builder.Append(tmp).Append("\n");
				}

				return builder.ToString();
			}
		}

		public virtual string EngineName
		{
			get
			{
				return scriptEngineLookupName;
			}
		}

		public override int GetHashCode()
		{
			const int prime = 31;
			int result = 1;
			result = prime * result + ((string.ReferenceEquals(id, null)) ? 0 : id.GetHashCode());
			return result;
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj == null)
			{
				return false;
			}
			if (!(obj is Script<E>))
			{
				return false;
			}
			Script<E> other = (Script<E>) obj;
			if (string.ReferenceEquals(this.Id, null))
			{
				if (!string.ReferenceEquals(other.Id, null))
				{
					return false;
				}
			}
			else if (!this.Id.Equals(other.Id))
			{
				return false;
			}
			return true;
		}

		public override string ToString()
		{
			return ScriptName;
		}

		public virtual string display()
		{
			string nl = Environment.NewLine;
			return " { " + nl + "Script '" + ScriptName + '\'' + nl + "\tscriptEngineLookupName = '" + scriptEngineLookupName + '\'' + nl + "\tscript = " + nl + script + nl + "\tid = " + nl + id + nl + "\tparameters = " + string.Join(",", parameters) + nl + '}';
		}

		public virtual void overrideDefaultScriptName(string defaultScriptName)
		{
			if (ScriptName.Equals(DefaultScriptName))
			{
				scriptName = defaultScriptName;
			}
		}

		private class ScriptContentAndEngineName
		{
			internal readonly string scriptContent;

			internal readonly string engineName;

			public ScriptContentAndEngineName(string scriptContent, string engineName)
			{
				this.scriptContent = scriptContent;
				this.engineName = engineName;
			}

			public virtual string ScriptContent
			{
				get
				{
					return scriptContent;
				}
			}

			public virtual string EngineName
			{
				get
				{
					return engineName;
				}
			}
		}
	}

}