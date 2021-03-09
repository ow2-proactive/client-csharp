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
namespace org.ow2.proactive.scheduler.common.util
{

	using StrMatcher = org.ow2.proactive.scheduler.common.util.text.StrMatcher;
	using StrSubstitutor = org.ow2.proactive.scheduler.common.util.text.StrSubstitutor;
	using Script = org.ow2.proactive.scripting.Script<object>;


	/// <summary>
	/// Utility class which facilitates the filtering of variables defined in strings
	/// and scripts by using a variable map. These variables should have ${...} or $...
	/// format. They will be replace with corresponding values specified in the
	/// variable map.
	/// </summary>
	public class VariableSubstitutor
	{

		public const string SUBSITUTE_PREFIX = "${";

		public const string SUBSITUTE_PREFIX_SIMPLE = "$";

		public const string SUBSTITUTE_SUFFIX = "}";

		private const char DOLLAR = '$';

		private const char LCB = '{';

		private const char RCB = '}';

		private const char DASH = '-';

		private const char DOT = '.';

		private const char UND = '_';

		// non-instantiable
		private VariableSubstitutor()
		{
		}

		/// <summary>
		/// Replaces variables in {@code input} map values using keys defined in {@code variables} map.
		/// </summary>
		/// <param name="input">     a map that may contain values to replace. </param>
		/// <param name="variables"> a map which contains variable name and value pairs. </param>
		public static IDictionary<string, string> filterAndUpdate(IDictionary<string, string> input, IDictionary<string, string> variables)
		{

			IDictionary<string, string> result = new Dictionary<string, string>(variables.Count);

			foreach (KeyValuePair<string, string> entry in input.SetOfKeyValuePairs())
			{
				result[entry.Key] = filterAndUpdate(entry.Value, variables);
			}

			return result;
		}

		/// <summary>
		/// Creates a HashMap with variablesDictionary where values are resolved.
		/// 
		/// Solve variables value if bound to another variable, for instance "var log=${LOG_ENV_VAR}", we
		/// expect that LOG_ENV_VAR is replaced by its value. To do so we have a variables hash, that must have
		/// all references and then for each variable we do a filterAndUpdate that will recursively replace </summary>
		/// when needed, <seealso cref= VariableSubstitutor.
		/// 
		/// Some limitations to consider: recursive substitution limit is VariableSubstitutor.MAXIMUM_DEPTH,
		/// if the variable value is a complex data structure (array, List, Vector) we will not substitute it.
		/// </seealso>
		/// <param name="variables"> input hash containing variables and their values may reference other variables </param>
		/// <returns> dictionary with the same variables however with their values resolved </returns>
		public static IDictionary<string, string> resolveVariables(IDictionary<string, string> variables, IDictionary<string, string> dictionary)
		{

			StrSubstitutor substitutor = buildSubstitutor(dictionary);

			IDictionary<string, string> resolvedVariables = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> entry in variables.SetOfKeyValuePairs())
			{
				if (entry.Value is string)
				{
					resolvedVariables[entry.Key] = substitutor.replace((string) entry.Value);
				}
				else
				{
					resolvedVariables[entry.Key] = entry.Value;
				}
			}
			return resolvedVariables;
		}

		/// <summary>
		/// Filters the specified string and replaces the variables with values
		/// specified in the map.
		/// </summary>
		/// <param name="input">     the string which need to be filtered </param>
		/// <param name="variables"> a map which contains variable values </param>
		/// <returns> the filtered string </returns>
		/// <seealso cref= VariableSubstitutor#filterAndUpdate(String, Map) </seealso>
		public static string filterAndUpdate(string input, IDictionary<string, string> variables)
		{
			if (string.ReferenceEquals(input, null) || input.Length == 0)
			{
				return input;
			}

			string output = input;
			StrSubstitutor substitutor = buildSubstitutor(variables);
			output = replaceRecursively(output, substitutor);

			return output;
		}

		/// <summary>
		/// Replace the given string with a list of substitutions recursively. Recursion will be limited to MAXIMUM_DEPTH.
		/// </summary>
		/// <param name="value">       string used to apply replacement </param>
		/// <param name="substitutor"> substitution handler </param>
		/// <returns> a new string where all replacements were performed </returns>
		private static string replaceRecursively(in string value, StrSubstitutor substitutor)
		{
			return substitutor.replace(value);
		}

		/// <summary>
		/// Filters the specified script object. It replaces the variables in the
		/// script content and parameter array with the values specified in the
		/// variable map.
		/// </summary>
		/// <param name="script">    the script to filter </param>
		/// <param name="variables"> a map which contains variables values </param>
		public static void filterAndUpdate(Script script, IDictionary<string, string> variables)
		{
			script.SetScript(filterAndUpdate(script.GetScript(), variables));
			string[] @params = script.Parameters;
			if (@params != null)
			{
				for (int i = 0; i < @params.Length; i++)
				{
					if (@params[i] != null)
					{
						@params[i] = filterAndUpdate(@params[i].ToString(), variables);
					}
				}
			}
		}

		public static StrSubstitutor buildSubstitutor(IDictionary<string, string> variables)
		{

			IDictionary<string, object> replacements = new Dictionary<string, object>();

			if (variables != null)
			{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: for (java.util.Map.Entry<? extends java.io.Serializable, ? extends java.io.Serializable> variable : variables.entrySet())
				foreach (KeyValuePair<string, string> variable in variables.SetOfKeyValuePairs())
				{
					if (variable.Value != null)
					{
						string key = variable.Key.ToString();
						string value = variable.Value.ToString();

						replacements[key] = value;
						replacements[key.ToUpper().Replace(".", "_")] = value;
					}
				}
			}

			StrSubstitutor substitutor = new StrSubstitutor(replacements, SUBSITUTE_PREFIX, SUBSTITUTE_SUFFIX, (char) 0);

			// match the beginning of a variable, can either be $ or ${
			substitutor.setVariablePrefixMatcher(new StrMatcherAnonymousInnerClass());

			// match the end of a variable, can be an explicit }, the end of a NCName or the buffer end
			substitutor.setVariableSuffixMatcher(new StrMatcherAnonymousInnerClass2());
			substitutor.EnableSubstitutionInVariables = true;

			return substitutor;
		}

		private class StrMatcherAnonymousInnerClass : StrMatcher
		{
			public override int isMatch(in char[] buffer, in int pos, in int bufferStart, in int bufferEnd)
			{
				if (pos + 1 >= bufferEnd)
				{
					return -1;
				}
				char firstChar = buffer[pos];
				char secondChar = buffer[pos + 1];
				if (firstChar == DOLLAR)
				{
					if (secondChar == LCB)
					{
						// dollar with a {
						return 2;
					}
					// dollar and the beginning of a NCName
					if (char.IsLetterOrDigit(secondChar))
					{
						return 1;
					}
					switch (secondChar)
					{
						case DASH:
						case DOT:
						case UND:
							return 1;
					}
				}
				// no match
				return -1;
			}
		}

		private class StrMatcherAnonymousInnerClass2 : StrMatcher
		{
			public override int isMatch(in char[] buffer, in int pos, in int bufferStart, in int bufferEnd)
			{
				char aChar = buffer[pos];
				// end of the buffer is the end of a variable
				if (pos == bufferEnd)
				{
					return 0;
				}
				// any character part of a NCName is not an end
				if (char.IsLetterOrDigit(aChar))
				{
					return -1;
				}
				switch (aChar)
				{
					case DASH:
					case DOT:
					case UND:
						return -1;
				}
				// an explicit }
				if (aChar == RCB)
				{
					return 1;
				}
				// anything else is the end of a variable
				return 0;
			}

		}

	}

}