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
	using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// A selection Script : return true if the resource tested is correct.
    /// 
    /// There are 2 type of selection scripts :<br>
    /// -static scripts, aimed to test test static property of a resource (node), OS type
    /// RAM total space, dynamic libraries present....
    /// -dynamic script, aimed to test dynamic properties if a resource, free disk space...
    /// 
    ///  A static script is executed once on a node and result of script's execution is memorized
    ///  for a next script execution request, so that we avoid a second execution of a static script.
    ///  A dynamic script is always executed, because we suppose that script tests dynamic properties
    ///  able to change. By default a script is dynamic.
    /// 
    /// 
    /// @author The ProActive Team
    /// @since ProActive Scheduling 0.9
    /// </summary>
    //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
    //ORIGINAL LINE: @PublicAPI public class SelectionScript extends Script<bool>
    [Serializable]
	public class SelectionScript : Script<object>
	{

		/// <summary>
		/// The variable name which must be set after the evaluation
		/// of a verifying script.
		/// </summary>
		public const string RESULT_VARIABLE = "selected";

		/// <summary>
		/// If true, script result is not cached </summary>
		private bool dynamic = true;

		/// <summary>
		/// Hash digest of the script
		/// </summary>
		protected internal byte[] hash = null;

		/// <summary>
		/// ProActive needed constructor </summary>
		public SelectionScript()
		{
		}

		protected internal override string DefaultScriptName
		{
			get
			{
				return "SelectionScript";
			}
		}

		/// <summary>
		/// Directly create a script with a String. </summary>
		/// <param name="script"> String representing a script code </param>
		/// <param name="engineName"> String a script execution engine. </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SelectionScript(String script, String engineName) throws InvalidScriptException
		public SelectionScript(string script, string engineName) : base(script, engineName, "SelectionScript")
		{
		}

		/// <summary>
		/// Directly create a script with a string. </summary>
		/// <param name="script"> String representing a script code </param>
		/// <param name="engineName"> String a script execution engine. </param>
		/// <param name="dynamic"> tell if the script is dynamic or static </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SelectionScript(String script, String engineName, boolean dynamic) throws InvalidScriptException
		public SelectionScript(string script, string engineName, bool dynamic) : base(script, engineName, "SelectionScript")
		{
			this.dynamic = dynamic;
		}

		/// <summary>
		/// Directly create a script with a string. </summary>
		/// <param name="script"> String representing a script code </param>
		/// <param name="parameters"> script execution arguments. </param>
		/// <param name="engineName"> String a script execution engine. </param>
		/// <param name="dynamic"> tell if the script is dynamic or static </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SelectionScript(String script, String engineName, java.io.Serializable[] parameters, boolean dynamic) throws InvalidScriptException
		public SelectionScript(string script, string engineName, string[] parameters, bool dynamic) : base(script, engineName, parameters, "SelectionScript")
		{
			this.dynamic = dynamic;
		}

		/// <summary>
		/// Create a selection script from a file. </summary>
		/// <param name="file"> a file containing the script </param>
		/// <param name="parameters"> script execution arguments. </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SelectionScript(java.io.File file, java.io.Serializable[] parameters) throws InvalidScriptException
		public SelectionScript(FileInfo file, string[] parameters) : base(file, parameters)
		{
		}

		/// <summary>
		/// Create a selection script from a file. </summary>
		/// <param name="file"> a file containing script code </param>
		/// <param name="parameters"> script execution arguments. </param>
		/// <param name="dynamic"> tell if script is dynamic or static </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SelectionScript(java.io.File file, java.io.Serializable[] parameters, boolean dynamic) throws InvalidScriptException
		public SelectionScript(FileInfo file, string[] parameters, bool dynamic) : base(file, parameters)
		{
			this.dynamic = dynamic;
		}

		/// <summary>
		/// Create a selection script from an URL. </summary>
		/// <param name="url"> an URL representing a script code </param>
		/// <exception cref="InvalidScriptException"> if the creation fails </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SelectionScript(java.net.URL url) throws InvalidScriptException
		public SelectionScript(Uri url) : this(url, true)
		{
		}

		/// <summary>
		/// Create a selection script from an URL. </summary>
		/// <param name="url"> an URL representing a script code </param>
		/// <exception cref="InvalidScriptException"> if the creation fails </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SelectionScript(java.net.URL url, boolean dynamic) throws InvalidScriptException
		public SelectionScript(Uri url, bool dynamic) : this(url, (string) null, dynamic)
		{
		}

		/// <summary>
		/// Create a selection script from an URL. </summary>
		/// <param name="url"> an URL representing a script code </param>
		/// <param name="engineName"> script's engine execution name. </param>
		/// <exception cref="InvalidScriptException"> if the creation fails </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SelectionScript(java.net.URL url, String engineName) throws InvalidScriptException
		public SelectionScript(Uri url, string engineName) : this(url, engineName, true)
		{
		}

		/// <summary>
		/// Create a selection script from an URL. </summary>
		/// <param name="url"> an URL representing a script code </param>
		/// <param name="engineName"> script's engine execution name. </param>
		/// <param name="dynamic"> true if the script is dynamic </param>
		/// <exception cref="InvalidScriptException"> if the creation fails </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SelectionScript(java.net.URL url, String engineName, boolean dynamic) throws InvalidScriptException
		public SelectionScript(Uri url, string engineName, bool dynamic) : this(url, engineName, null, dynamic)
		{
		}

		/// <summary>
		/// Create a selection script from an URL. </summary>
		/// <param name="url"> an URL representing a script code </param>
		/// <param name="parameters"> script execution argument. </param>
		/// <exception cref="InvalidScriptException"> if the creation fails </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SelectionScript(java.net.URL url, java.io.Serializable[] parameters) throws InvalidScriptException
		public SelectionScript(Uri url, string[] parameters) : this(url, parameters, true)
		{
		}

		/// <summary>
		/// Create a selection script from an URL. </summary>
		/// <param name="url"> an URL representing a script code </param>
		/// <param name="parameters"> execution arguments </param>
		/// <param name="dynamic"> true if the script is dynamic </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SelectionScript(java.net.URL url, java.io.Serializable[] parameters, boolean dynamic) throws InvalidScriptException
		public SelectionScript(Uri url, String[] parameters, bool dynamic) : this(url, null, parameters, dynamic)
		{
		}

		/// <summary>
		/// Create a selection script from an URL. </summary>
		/// <param name="url"> an URL representing a script code </param>
		/// <param name="engineName"> script's engine execution name. </param>
		/// <param name="parameters"> script execution argument. </param>
		/// <exception cref="InvalidScriptException"> if the creation fails </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SelectionScript(java.net.URL url, String engineName, java.io.Serializable[] parameters) throws InvalidScriptException
		public SelectionScript(Uri url, string engineName, string[] parameters) : this(url, engineName, parameters, true)
		{
		}

		/// <summary>
		/// Create a selection script from an URL. </summary>
		/// <param name="url"> an URL representing a script code </param>
		/// <param name="engineName"> script's engine execution name. </param>
		/// <param name="parameters"> execution arguments </param>
		/// <param name="dynamic"> true if the script is dynamic </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SelectionScript(java.net.URL url, String engineName, java.io.Serializable[] parameters, boolean dynamic) throws InvalidScriptException
		public SelectionScript(Uri url, string engineName, string[] parameters, bool dynamic) : base(url, engineName, parameters, false)
		{
			this.dynamic = dynamic;
		}

		/// <summary>
		/// Create a selection script from another selection script </summary>
		/// <param name="script"> selection script source </param>
		/// <param name="dynamic"> true if the script is dynamic </param>
		/// <exception cref="InvalidScriptException"> if the creation fails. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public SelectionScript(Script<?> script, boolean dynamic) throws InvalidScriptException
//JAVA TO C# CONVERTER TODO TASK: Wildcard generics in constructor parameters are not converted. Move the generic type parameter and constraint to the class header:
		public SelectionScript(Script<object> script, bool dynamic) : base(script)
		{
			this.dynamic = dynamic;
		}

		/// <summary>
		/// Build script ID. ID is a way to compare a script with another.
		/// ID is a String made of script_code+script_parameters+type
		/// 
		/// </summary>
		private void buildSelectionScriptHash()
		{
			string stringId;
			if (script != null)
			{
				stringId = script;
			} else
			{
				stringId = url.AbsoluteUri;
			}
		

			//concatenate the script type (dynamic or static)
			stringId += this.dynamic;

			//concatenate parameters if any
			if (this.parameters != null)
			{
				foreach (string param in this.parameters)
				{
					stringId += param;
				}
			}
			using (HashAlgorithm algorithm = SHA256.Create())
				this.hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(stringId));
		}

		private byte[] Hash
		{
			get
			{
				if (hash == null)
				{
					buildSelectionScriptHash();
				}
				return hash;
			}
		}

		protected internal override TextReader Reader
		{
			get
			{
				return new StringReader(script);
			}
		}

		/// <summary>
		/// Say if the script is static or dynamic </summary>
		/// <returns> true if the script is dynamic, false otherwise </returns>
		public virtual bool Dynamic
		{
			get
			{
				return dynamic;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}

			if (obj is SelectionScript)
			{
				return compareByteArray(Hash, ((SelectionScript) obj).Hash);
			}
			return false;
		}

		/// <summary>
		/// Compare two arrays of bytes </summary>
		/// <param name="array1"> first array to compare </param>
		/// <param name="array2"> second array to compare </param>
		/// <returns> true is the two contains the same bytes values, false otherwise </returns>
		public virtual bool compareByteArray(byte[] array1, byte[] array2)
		{
			if (array1.Length != array2.Length)
			{
				return false;
			}
			else
			{
				for (int i = 0; i < array1.Length; i++)
				{
					if (array1[i] != array2[i])
					{
						return false;
					}
				}
				return true;
			}
		}

		public override string ToString()
		{
			return "" + id;
		}
	}

}