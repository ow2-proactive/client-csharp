using System.Collections.Generic;
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
namespace org.ow2.proactive.scheduler.common.util.text
{
	/*
	 * Licensed to the Apache Software Foundation (ASF) under one or more
	 * contributor license agreements.  See the NOTICE file distributed with
	 * this work for additional information regarding copyright ownership.
	 * The ASF licenses this file to You under the Apache License, Version 2.0
	 * (the "License"); you may not use this file except in compliance with
	 * the License.  You may obtain a copy of the License at
	 *
	 *      http://www.apache.org/licenses/LICENSE-2.0
	 *
	 * Unless required by applicable law or agreed to in writing, software
	 * distributed under the License is distributed on an "AS IS" BASIS,
	 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	 * See the License for the specific language governing permissions and
	 * limitations under the License.
	 */


	/// <summary>
	/// Substitutes variables within a string by values.
	/// <para>
	/// This class takes a piece of text and substitutes all the variables within it.
	/// The default definition of a variable is <code>${variableName}</code>.
	/// The prefix and suffix can be changed via constructors and set methods.
	/// </para>
	/// <para>
	/// Variable values are typically resolved from a map, but could also be resolved
	/// from system properties, or by supplying a custom variable resolver.
	/// </para>
	/// <para>
	/// The simplest example is to use this class to replace Java System properties. For example:
	/// <pre>
	/// StrSubstitutor.replaceSystemProperties(
	///      "You are running with java.version = ${java.version} and os.name = ${os.name}.");
	/// </pre>
	/// </para>
	/// <para>
	/// Typical usage of this class follows the following pattern: First an instance is created
	/// and initialized with the map that contains the values for the available variables.
	/// If a prefix and/or suffix for variables should be used other than the default ones,
	/// the appropriate settings can be performed. After that the <code>replace()</code>
	/// method can be called passing in the source text for interpolation. In the returned
	/// text all variable references (as long as their values are known) will be resolved.
	/// The following example demonstrates this:
	/// <pre>
	/// Map valuesMap = HashMap();
	/// valuesMap.put(&quot;animal&quot;, &quot;quick brown fox&quot;);
	/// valuesMap.put(&quot;target&quot;, &quot;lazy dog&quot;);
	/// String templateString = &quot;The ${animal} jumped over the ${target}.&quot;
	/// StrSubstitutor sub = new StrSubstitutor(valuesMap);
	/// String resolvedString = sub.replace(templateString);
	/// </pre>
	/// yielding:
	/// <pre>
	///      The quick brown fox jumped over the lazy dog.
	/// </pre>
	/// </para>
	/// <para>
	/// Also, this class allows to set a default value for unresolved variables.
	/// The default value for a variable can be appended to the variable name after the variable
	/// default value delimiter. The default value of the variable default value delimiter is ':-',
	/// as in bash and other *nix shells, as those are arguably where the default ${} delimiter set originated.
	/// The variable default value delimiter can be manually set by calling <seealso cref="setValueDelimiterMatcher(StrMatcher)"/>,
	/// <seealso cref="setValueDelimiter(char)"/> or <seealso cref="setValueDelimiter(string)"/>.
	/// The following shows an example with varialbe default value settings:
	/// <pre>
	/// Map valuesMap = HashMap();
	/// valuesMap.put(&quot;animal&quot;, &quot;quick brown fox&quot;);
	/// valuesMap.put(&quot;target&quot;, &quot;lazy dog&quot;);
	/// String templateString = &quot;The ${animal} jumped over the ${target}. ${undefined.number:-1234567890}.&quot;
	/// StrSubstitutor sub = new StrSubstitutor(valuesMap);
	/// String resolvedString = sub.replace(templateString);
	/// </pre>
	/// yielding:
	/// <pre>
	///      The quick brown fox jumped over the lazy dog. 1234567890.
	/// </pre>
	/// </para>
	/// <para>
	/// In addition to this usage pattern there are some static convenience methods that
	/// cover the most common use cases. These methods can be used without the need of
	/// manually creating an instance. However if multiple replace operations are to be
	/// performed, creating and reusing an instance of this class will be more efficient.
	/// </para>
	/// <para>
	/// Variable replacement works in a recursive way. Thus, if a variable value contains
	/// a variable then that variable will also be replaced. Cyclic replacements are
	/// detected and will cause an exception to be thrown.
	/// </para>
	/// <para>
	/// Sometimes the interpolation's result must contain a variable prefix. As an example
	/// take the following source text:
	/// <pre>
	///   The variable ${${name}} must be used.
	/// </pre>
	/// Here only the variable's name referred to in the text should be replaced resulting
	/// in the text (assuming that the value of the <code>name</code> variable is <code>x</code>):
	/// <pre>
	///   The variable ${x} must be used.
	/// </pre>
	/// To achieve this effect there are two possibilities: Either set a different prefix
	/// and suffix for variables which do not conflict with the result text you want to
	/// produce. The other possibility is to use the escape character, by default '$'.
	/// If this character is placed before a variable reference, this reference is ignored
	/// and won't be replaced. For example:
	/// <pre>
	///   The variable $${${name}} must be used.
	/// </pre>
	/// </para>
	/// <para>
	/// In some complex scenarios you might even want to perform substitution in the
	/// names of variables, for instance
	/// <pre>
	/// ${jre-${java.specification.version}}
	/// </pre>
	/// <code>StrSubstitutor</code> supports this recursive substitution in variable
	/// names, but it has to be enabled explicitly by setting the
	/// <seealso cref="setEnableSubstitutionInVariables(bool) enableSubstitutionInVariables"/>
	/// property to <b>true</b>.
	/// 
	/// NOTICE: this class has been modified to support suffix StrMatcher able to match the ending
	/// of variable patterns with no suffixes, such as:
	/// $variable
	/// 
	/// An issue still remains where two variable definitions are glued without any intermediate character, such as:
	/// $var1$var2
	/// 
	/// 
	/// @version $Id$
	/// @since 2.2
	/// </para>
	/// </summary>
	public class StrSubstitutor
	{
		/// <summary>
		/// Constant for the default escape character.
		/// </summary>
		public const char DEFAULT_ESCAPE = '$';

		/// <summary>
		/// Constant for the default variable prefix.
		/// </summary>
		public static readonly StrMatcher DEFAULT_PREFIX = StrMatcher.stringMatcher("${");

		/// <summary>
		/// Constant for the default variable suffix.
		/// </summary>
		public static readonly StrMatcher DEFAULT_SUFFIX = StrMatcher.stringMatcher("}");

		/// <summary>
		/// Constant for the default value delimiter of a variable.
		/// @since 3.2
		/// </summary>
		public static readonly StrMatcher DEFAULT_VALUE_DELIMITER = StrMatcher.stringMatcher(":-");

		/// <summary>
		/// Stores the escape character.
		/// </summary>
		private char escapeChar;

		/// <summary>
		/// Stores the variable prefix.
		/// </summary>
		private StrMatcher prefixMatcher;

		/// <summary>
		/// Stores the variable suffix.
		/// </summary>
		private StrMatcher suffixMatcher;

		/// <summary>
		/// Stores the default variable value delimiter
		/// </summary>
		private StrMatcher valueDelimiterMatcher;

		/// <summary>
		/// Variable resolution is delegated to an implementor of VariableResolver.
		/// </summary>
		//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
		//ORIGINAL LINE: private StrLookup<?> variableResolver;
		private StrLookup variableResolver;

		/// <summary>
		/// The flag whether substitution in variable names is enabled.
		/// </summary>
		private bool enableSubstitutionInVariables;

		//-----------------------------------------------------------------------
		/// <summary>
		/// Replaces all the occurrences of variables in the given source object with
		/// their matching values from the map.
		/// </summary>
		/// @param <V> the type of the values in the map </param>
		/// <param name="source">  the source text containing the variables to substitute, null returns null </param>
		/// <param name="valueMap">  the map with the values, may be null </param>
		/// <returns> the result of the replace operation </returns>
		public static string replace(in object source, in IDictionary<string, object> valueMap)
		{
			return (new StrSubstitutor(valueMap)).replace(source);
		}

		/// <summary>
		/// Replaces all the occurrences of variables in the given source object with
		/// their matching values from the map. This method allows to specifiy a
		/// custom variable prefix and suffix
		/// </summary>
		/// @param <V> the type of the values in the map </param>
		/// <param name="source">  the source text containing the variables to substitute, null returns null </param>
		/// <param name="valueMap">  the map with the values, may be null </param>
		/// <param name="prefix">  the prefix of variables, not null </param>
		/// <param name="suffix">  the suffix of variables, not null </param>
		/// <returns> the result of the replace operation </returns>
		/// <exception cref="IllegalArgumentException"> if the prefix or suffix is null </exception>
		public static string replace(in object source, in IDictionary<string, object> valueMap, in string prefix, in string suffix)
		{
			return (new StrSubstitutor(valueMap, prefix, suffix)).replace(source);
		}

		/// <summary>
		/// Replaces all the occurrences of variables in the given source object with their matching
		/// values from the properties.
		/// </summary>
		/// <param name="source"> the source text containing the variables to substitute, null returns null </param>
		/// <param name="valueProperties"> the properties with values, may be null </param>
		/// <returns> the result of the replace operation </returns>
		public static string replace(in object source, in IDictionary<string, string> valueProperties)
		{
			if (valueProperties == null)
			{
				return source.ToString();
			}
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final java.util.Map<String, String> valueMap = new java.util.HashMap<String, String>();
			IDictionary<string, string> valueMap = new Dictionary<string, string>();
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final java.util.Iterator<?> propNames = valueProperties.propertyNames();
			//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
			IEnumerator<object> propNames = valueProperties.Keys.GetEnumerator();
			while (propNames.MoveNext())
			{
				//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
				//ORIGINAL LINE: final String propName = (String) propNames.Current;
				string propName = (string)propNames.Current;
				//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
				//ORIGINAL LINE: final String propValue = valueProperties.getProperty(propName);
				string propValue = valueProperties[propName];
				valueMap[propName] = propValue;
			}
			return StrSubstitutor.replace(source, valueMap);
		}

		/// <summary>
		/// Replaces all the occurrences of variables in the given source object with
		/// their matching values from the system properties.
		/// </summary>
		/// <param name="source">  the source text containing the variables to substitute, null returns null </param>
		/// <returns> the result of the replace operation </returns>
		public static string replaceSystemProperties(in object source)
		{
			return (new StrSubstitutor(StrLookup.systemPropertiesLookup())).replace(source);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Creates a new instance with defaults for variable prefix and suffix
		/// and the escaping character.
		/// </summary>
		public StrSubstitutor() : this((StrLookup)null, DEFAULT_PREFIX, DEFAULT_SUFFIX, DEFAULT_ESCAPE)
		{
			//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
			//ORIGINAL LINE: this((StrLookup<?>) null, DEFAULT_PREFIX, DEFAULT_SUFFIX, DEFAULT_ESCAPE);
		}

		/// <summary>
		/// Creates a new instance and initializes it. Uses defaults for variable
		/// prefix and suffix and the escaping character.
		/// </summary>
		/// @param <V> the type of the values in the map </param>
		/// <param name="valueMap">  the map with the variables' values, may be null </param>
		public StrSubstitutor(IDictionary<string, object> valueMap) : this(StrLookup.mapLookup(valueMap), DEFAULT_PREFIX, DEFAULT_SUFFIX, DEFAULT_ESCAPE)
		{
		}

		/// <summary>
		/// Creates a new instance and initializes it. Uses a default escaping character.
		/// </summary>
		/// @param <V> the type of the values in the map </param>
		/// <param name="valueMap">  the map with the variables' values, may be null </param>
		/// <param name="prefix">  the prefix for variables, not null </param>
		/// <param name="suffix">  the suffix for variables, not null </param>
		/// <exception cref="IllegalArgumentException"> if the prefix or suffix is null </exception>
		public StrSubstitutor(IDictionary<string, object> valueMap, in string prefix, in string suffix) : this(StrLookup.mapLookup(valueMap), prefix, suffix, DEFAULT_ESCAPE)
		{
		}

		/// <summary>
		/// Creates a new instance and initializes it.
		/// </summary>
		/// @param <V> the type of the values in the map </param>
		/// <param name="valueMap">  the map with the variables' values, may be null </param>
		/// <param name="prefix">  the prefix for variables, not null </param>
		/// <param name="suffix">  the suffix for variables, not null </param>
		/// <param name="escape">  the escape character </param>
		/// <exception cref="IllegalArgumentException"> if the prefix or suffix is null </exception>
		public StrSubstitutor(IDictionary<string, object> valueMap, in string prefix, in string suffix, in char escape) : this(StrLookup.mapLookup(valueMap), prefix, suffix, escape)
		{
		}

		/// <summary>
		/// Creates a new instance and initializes it.
		/// </summary>
		/// @param <V> the type of the values in the map </param>
		/// <param name="valueMap">  the map with the variables' values, may be null </param>
		/// <param name="prefix">  the prefix for variables, not null </param>
		/// <param name="suffix">  the suffix for variables, not null </param>
		/// <param name="escape">  the escape character </param>
		/// <param name="valueDelimiter">  the variable default value delimiter, may be null </param>
		/// <exception cref="IllegalArgumentException"> if the prefix or suffix is null
		/// @since 3.2 </exception>
		public StrSubstitutor(IDictionary<string, object> valueMap, in string prefix, in string suffix, in char escape, in string valueDelimiter) : this(StrLookup.mapLookup(valueMap), prefix, suffix, escape, valueDelimiter)
		{
		}

		/// <summary>
		/// Creates a new instance and initializes it.
		/// </summary>
		/// <param name="variableResolver">  the variable resolver, may be null </param>
		//JAVA TO C# CONVERTER TODO TASK: Wildcard generics in constructor parameters are not converted. Move the generic type parameter and constraint to the class header:
		//ORIGINAL LINE: public StrSubstitutor(final StrLookup<?> variableResolver)
		public StrSubstitutor(in StrLookup variableResolver) : this(variableResolver, DEFAULT_PREFIX, DEFAULT_SUFFIX, DEFAULT_ESCAPE)
		{
		}

		/// <summary>
		/// Creates a new instance and initializes it.
		/// </summary>
		/// <param name="variableResolver">  the variable resolver, may be null </param>
		/// <param name="prefix">  the prefix for variables, not null </param>
		/// <param name="suffix">  the suffix for variables, not null </param>
		/// <param name="escape">  the escape character </param>
		/// <exception cref="IllegalArgumentException"> if the prefix or suffix is null </exception>
		//JAVA TO C# CONVERTER TODO TASK: Wildcard generics in constructor parameters are not converted. Move the generic type parameter and constraint to the class header:
		//ORIGINAL LINE: public StrSubstitutor(final StrLookup<?> variableResolver, final String prefix, final String suffix, final char escape)
		public StrSubstitutor(in StrLookup variableResolver, in string prefix, in string suffix, in char escape)
		{
			this.VariableResolver = variableResolver;
			this.setVariablePrefix(prefix);
			this.setVariableSuffix(suffix);
			this.EscapeChar = escape;
			this.valueDelimiterMatcher = DEFAULT_VALUE_DELIMITER;
		}

		/// <summary>
		/// Creates a new instance and initializes it.
		/// </summary>
		/// <param name="variableResolver">  the variable resolver, may be null </param>
		/// <param name="prefix">  the prefix for variables, not null </param>
		/// <param name="suffix">  the suffix for variables, not null </param>
		/// <param name="escape">  the escape character </param>
		/// <param name="valueDelimiter">  the variable default value delimiter string, may be null </param>
		/// <exception cref="IllegalArgumentException"> if the prefix or suffix is null
		/// @since 3.2 </exception>
		//JAVA TO C# CONVERTER TODO TASK: Wildcard generics in constructor parameters are not converted. Move the generic type parameter and constraint to the class header:
		//ORIGINAL LINE: public StrSubstitutor(final StrLookup<?> variableResolver, final String prefix, final String suffix, final char escape, final String valueDelimiter)
		public StrSubstitutor(in StrLookup variableResolver, in string prefix, in string suffix, in char escape, in string valueDelimiter)
		{
			this.VariableResolver = variableResolver;
			this.setVariablePrefix(prefix);
			this.setVariableSuffix(suffix);
			this.EscapeChar = escape;
			this.setValueDelimiter(valueDelimiter);
		}

		/// <summary>
		/// Creates a new instance and initializes it.
		/// </summary>
		/// <param name="variableResolver">  the variable resolver, may be null </param>
		/// <param name="prefixMatcher">  the prefix for variables, not null </param>
		/// <param name="suffixMatcher">  the suffix for variables, not null </param>
		/// <param name="escape">  the escape character </param>
		/// <exception cref="IllegalArgumentException"> if the prefix or suffix is null </exception>
		//JAVA TO C# CONVERTER TODO TASK: Wildcard generics in constructor parameters are not converted. Move the generic type parameter and constraint to the class header:
		//ORIGINAL LINE: public StrSubstitutor(final StrLookup<?> variableResolver, final StrMatcher prefixMatcher, final StrMatcher suffixMatcher, final char escape)
		public StrSubstitutor(in StrLookup variableResolver, in StrMatcher prefixMatcher, in StrMatcher suffixMatcher, in char escape) : this(variableResolver, prefixMatcher, suffixMatcher, escape, DEFAULT_VALUE_DELIMITER)
		{
		}

		/// <summary>
		/// Creates a new instance and initializes it.
		/// </summary>
		/// <param name="variableResolver">  the variable resolver, may be null </param>
		/// <param name="prefixMatcher">  the prefix for variables, not null </param>
		/// <param name="suffixMatcher">  the suffix for variables, not null </param>
		/// <param name="escape">  the escape character </param>
		/// <param name="valueDelimiterMatcher">  the variable default value delimiter matcher, may be null </param>
		/// <exception cref="IllegalArgumentException"> if the prefix or suffix is null
		/// @since 3.2 </exception>
		//JAVA TO C# CONVERTER TODO TASK: Wildcard generics in constructor parameters are not converted. Move the generic type parameter and constraint to the class header:
		//ORIGINAL LINE: public StrSubstitutor(final StrLookup<?> variableResolver, final StrMatcher prefixMatcher, final StrMatcher suffixMatcher, final char escape, final StrMatcher valueDelimiterMatcher)
		public StrSubstitutor(in StrLookup variableResolver, in StrMatcher prefixMatcher, in StrMatcher suffixMatcher, in char escape, in StrMatcher valueDelimiterMatcher)
		{
			this.VariableResolver = variableResolver;
			this.prefixMatcher = prefixMatcher;
			this.suffixMatcher = suffixMatcher;
			this.EscapeChar = escape;
			this.valueDelimiterMatcher = valueDelimiterMatcher;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Replaces all the occurrences of variables with their matching values
		/// from the resolver using the given source string as a template.
		/// </summary>
		/// <param name="source">  the string to replace in, null returns null </param>
		/// <returns> the result of the replace operation </returns>
		public virtual string replace(in string source)
		{
			if (string.ReferenceEquals(source, null))
			{
				return null;
			}
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrBuilder buf = new StrBuilder(source);
			StringBuilder buf = new StringBuilder(source);
			if (substitute(buf, 0, source.Length) == false)
			{
				return source;
			}
			return buf.ToString();
		}

		/// <summary>
		/// Replaces all the occurrences of variables with their matching values
		/// from the resolver using the given source string as a template.
		/// <para>
		/// Only the specified portion of the string will be processed.
		/// The rest of the string is not processed, and is not returned.
		/// 
		/// </para>
		/// </summary>
		/// <param name="source">  the string to replace in, null returns null </param>
		/// <param name="offset">  the start offset within the array, must be valid </param>
		/// <param name="length">  the length within the array to be processed, must be valid </param>
		/// <returns> the result of the replace operation </returns>
		public virtual string replace(in string source, in int offset, in int length)
		{
			if (string.ReferenceEquals(source, null))
			{
				return null;
			}
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrBuilder buf StringBuilder new StrBuilder(length).append(source, offset, length);
			StringBuilder buf = (new StringBuilder(length)).Append(source, offset, length);
			if (substitute(buf, 0, length) == false)
			{
				return source.Substring(offset, length);
			}
			return buf.ToString();
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Replaces all the occurrences of variables with their matching values
		/// from the resolver using the given source array as a template.
		/// The array is not altered by this method.
		/// </summary>
		/// <param name="source">  the character array to replace in, not altered, null returns null </param>
		/// <returns> the result of the replace operation </returns>
		public virtual string replace(in char[] source)
		{
			if (source == null)
			{
				return null;
			}
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrBuilder buf = new StrBuilder(source.length).append(source);
			StringBuilder buf = (new StringBuilder(source.Length)).Append(source);
			substitute(buf, 0, source.Length);
			return buf.ToString();
		}

		/// <summary>
		/// Replaces all the occurrences of variables with their matching values
		/// from the resolver using the given source array as a template.
		/// The array is not altered by this method.
		/// <para>
		/// Only the specified portion of the array will be processed.
		/// The rest of the array is not processed, and is not returned.
		/// 
		/// </para>
		/// </summary>
		/// <param name="source">  the character array to replace in, not altered, null returns null </param>
		/// <param name="offset">  the start offset within the array, must be valid </param>
		/// <param name="length">  the length within the array to be processed, must be valid </param>
		/// <returns> the result of the replace operation </returns>
		public virtual string replace(in char[] source, in int offset, in int length)
		{
			if (source == null)
			{
				return null;
			}
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrBuilder buf = new StrBuilder(length).append(source, offset, length);
			StringBuilder buf = (new StringBuilder(length)).Append(source, offset, length);
			substitute(buf, 0, length);
			return buf.ToString();
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Replaces all the occurrences of variables with their matching values
		/// from the resolver using the given source buffer as a template.
		/// The buffer is not altered by this method.
		/// </summary>
		/// <param name="source">  the buffer to use as a template, not changed, null returns null </param>
		/// <returns> the result of the replace operation </returns>
		public virtual string replace(in StringBuilder source)
		{
			if (source == null)
			{
				return null;
			}
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrBuilder buf = new StrBuilder(source.length()).append(source);
			StringBuilder buf = (new StringBuilder(source.Length)).Append(source);
			substitute(buf, 0, buf.Length);
			return buf.ToString();
		}

		/// <summary>
		/// Replaces all the occurrences of variables with their matching values
		/// from the resolver using the given source buffer as a template.
		/// The buffer is not altered by this method.
		/// <para>
		/// Only the specified portion of the buffer will be processed.
		/// The rest of the buffer is not processed, and is not returned.
		/// 
		/// </para>
		/// </summary>
		/// <param name="source">  the buffer to use as a template, not changed, null returns null </param>
		/// <param name="offset">  the start offset within the array, must be valid </param>
		/// <param name="length">  the length within the array to be processed, must be valid </param>
		/// <returns> the result of the replace operation </returns>
		public virtual string replace(in StringBuilder source, in int offset, in int length)
		{
			if (source == null)
			{
				return null;
			}
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrBuilder buf = new StrBuilder(length).append(source, offset, length);
			StringBuilder buf = (new StringBuilder(length)).Append(source.ToString(), offset, length);
			substitute(buf, 0, length);
			return buf.ToString();
		}

		/// <summary>
		/// Replaces all the occurrences of variables with their matching values
		/// from the resolver using the given source as a template.
		/// The source is not altered by this method.
		/// </summary>
		/// <param name="source">  the buffer to use as a template, not changed, null returns null </param>
		/// <returns> the result of the replace operation
		/// @since 3.2 </returns>
		public virtual string Replace(in string source)
		{
			if (source == null)
			{
				return null;
			}
			return Replace(source, 0, source.Length);
		}

		/// <summary>
		/// Replaces all the occurrences of variables with their matching values
		/// from the resolver using the given source as a template.
		/// The source is not altered by this method.
		/// <para>
		/// Only the specified portion of the buffer will be processed.
		/// The rest of the buffer is not processed, and is not returned.
		/// 
		/// </para>
		/// </summary>
		/// <param name="source">  the buffer to use as a template, not changed, null returns null </param>
		/// <param name="offset">  the start offset within the array, must be valid </param>
		/// <param name="length">  the length within the array to be processed, must be valid </param>
		/// <returns> the result of the replace operation
		/// @since 3.2 </returns>
		public virtual string Replace(in string source, in int offset, in int length)
		{
			if (source == null)
			{
				return null;
			}
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrBuilder buf = new StrBuilder(length).append(source, offset, length);
			StringBuilder buf = (new StringBuilder(length)).Append(source, offset, length);
			substitute(buf, 0, length);
			return buf.ToString();
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Replaces all the occurrences of variables in the given source object with
		/// their matching values from the resolver. The input source object is
		/// converted to a string using <code>toString</code> and is not altered.
		/// </summary>
		/// <param name="source">  the source to replace in, null returns null </param>
		/// <returns> the result of the replace operation </returns>
		public virtual string replace(in object source)
		{
			if (source == null)
			{
				return null;
			}
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrBuilder buf = new StrBuilder().append(source);
			StringBuilder buf = (new StringBuilder()).Append(source);
			substitute(buf, 0, buf.Length);
			return buf.ToString();
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Replaces all the occurrences of variables within the given source buffer
		/// with their matching values from the resolver.
		/// The buffer is updated with the result.
		/// </summary>
		/// <param name="source">  the buffer to replace in, updated, null returns zero </param>
		/// <returns> true if altered </returns>
		public virtual bool replaceIn(in StringBuilder source)
		{
			if (source == null)
			{
				return false;
			}
			return replaceIn(source, 0, source.Length);
		}

		/// <summary>
		/// Replaces all the occurrences of variables within the given source buffer
		/// with their matching values from the resolver.
		/// The buffer is updated with the result.
		/// <para>
		/// Only the specified portion of the buffer will be processed.
		/// The rest of the buffer is not processed, but it is not deleted.
		/// 
		/// </para>
		/// </summary>
		/// <param name="source">  the buffer to replace in, updated, null returns zero </param>
		/// <param name="offset">  the start offset within the array, must be valid </param>
		/// <param name="length">  the length within the buffer to be processed, must be valid </param>
		/// <returns> true if altered </returns>
		public virtual bool replaceIn(in StringBuilder source, in int offset, in int length)
		{
			if (source == null)
			{
				return false;
			}
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrBuilder buf = new StrBuilder(length).append(source, offset, length);
			StringBuilder buf = (new StringBuilder(length)).Append(source.ToString(), offset, length);
			if (substitute(buf, 0, length) == false)
			{
				return false;
			}
			source.Remove(offset, offset + length - offset).Insert(offset, buf.ToString());
			return true;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Replaces all the occurrences of variables within the given source
		/// builder with their matching values from the resolver.
		/// </summary>
		/// <param name="source">  the builder to replace in, updated, null returns zero </param>
		/// <returns> true if altered </returns>
		public virtual bool ReplaceIn(in StringBuilder source)
		{
			if (source == null)
			{
				return false;
			}
			return substitute(source, 0, source.Length);
		}

		/// <summary>
		/// Replaces all the occurrences of variables within the given source
		/// builder with their matching values from the resolver.
		/// <para>
		/// Only the specified portion of the builder will be processed.
		/// The rest of the builder is not processed, but it is not deleted.
		/// 
		/// </para>
		/// </summary>
		/// <param name="source">  the builder to replace in, null returns zero </param>
		/// <param name="offset">  the start offset within the array, must be valid </param>
		/// <param name="length">  the length within the builder to be processed, must be valid </param>
		/// <returns> true if altered </returns>
		public virtual bool ReplaceIn(in StringBuilder source, in int offset, in int length)
		{
			if (source == null)
			{
				return false;
			}
			return substitute(source, offset, length);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Internal method that substitutes the variables.
		/// <para>
		/// Most users of this class do not need to call this method. This method will
		/// be called automatically by another (public) method.
		/// </para>
		/// <para>
		/// Writers of subclasses can override this method if they need access to
		/// the substitution process at the start or end.
		/// 
		/// </para>
		/// </summary>
		/// <param name="buf">  the string builder to substitute into, not null </param>
		/// <param name="offset">  the start offset within the builder, must be valid </param>
		/// <param name="length">  the length within the builder to be processed, must be valid </param>
		/// <returns> true if altered </returns>
		protected internal virtual bool substitute(in StringBuilder buf, in int offset, in int length)
		{
			return substitute(buf, offset, length, null) > 0;
		}

		/// <summary>
		/// Recursive handler for multiple levels of interpolation. This is the main
		/// interpolation method, which resolves the values of all variable references
		/// contained in the passed in text.
		/// </summary>
		/// <param name="buf">  the string builder to substitute into, not null </param>
		/// <param name="offset">  the start offset within the builder, must be valid </param>
		/// <param name="length">  the length within the builder to be processed, must be valid </param>
		/// <param name="priorVariables">  the stack keeping track of the replaced variables, may be null </param>
		/// <returns> the length change that occurs, unless priorVariables is null when the int
		///  represents a boolean flag as to whether any change occurred. </returns>
		private int substitute(in StringBuilder buf, in int offset, in int length, IList<string> priorVariables)
		{
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrMatcher pfxMatcher = getVariablePrefixMatcher();
			StrMatcher pfxMatcher = VariablePrefixMatcher;
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrMatcher suffMatcher = getVariableSuffixMatcher();
			StrMatcher suffMatcher = VariableSuffixMatcher;
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final char escape = getEscapeChar();
			char escape = EscapeChar;
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrMatcher valueDelimMatcher = getValueDelimiterMatcher();
			StrMatcher valueDelimMatcher = ValueDelimiterMatcher;
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final boolean substitutionInVariablesEnabled = isEnableSubstitutionInVariables();
			bool substitutionInVariablesEnabled = EnableSubstitutionInVariables;
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final boolean top = priorVariables == null;
			bool top = priorVariables == null;
			bool altered = false;
			int lengthChange = 0;
			char[] chars = buf.ToString().ToCharArray();
			int bufEnd = offset + length;
			int pos = offset;
			while (pos < bufEnd)
			{
				//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
				//ORIGINAL LINE: final int startMatchLen = pfxMatcher.isMatch(chars, pos, offset, bufEnd);
				int startMatchLen = pfxMatcher.isMatch(chars, pos, offset, bufEnd);
				if (startMatchLen < 0)
				{
					pos++;
				}
				else
				{
					// found variable start marker
					if (pos > offset && chars[pos - 1] == escape)
					{
						// escaped
						buf.Remove(pos - 1, 1);
						chars = buf.ToString().ToCharArray(); // in case buffer was altered
						lengthChange--;
						altered = true;
						bufEnd--;
					}
					else
					{
						// find suffix
						//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
						//ORIGINAL LINE: final int startPos = pos;
						int startPos = pos;
						pos += startMatchLen;
						int endMatchLen = 0;
						int nestedVarCount = 0;
						while (pos <= bufEnd)
						{
							if (substitutionInVariablesEnabled && (endMatchLen = pfxMatcher.isMatch(chars, pos, offset, bufEnd)) >= 0)
							{
								// found a nested variable start
								nestedVarCount++;
								pos += endMatchLen;
								continue;
							}
							endMatchLen = suffMatcher.isMatch(chars, pos, offset, bufEnd);
							if (endMatchLen < 0)
							{
								pos++;
							}
							else
							{
								// found variable end marker
								if (nestedVarCount == 0)
								{
									string varNameExpr = new string(chars, startPos + startMatchLen, pos - startPos - startMatchLen);
									if (substitutionInVariablesEnabled)
									{
										//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
										//ORIGINAL LINE: final StrBuilder bufName = new StrBuilder(varNameExpr);
										StringBuilder bufName = new StringBuilder(varNameExpr);
										substitute(bufName, 0, bufName.Length);
										varNameExpr = bufName.ToString();
									}
									pos += endMatchLen;
									//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
									//ORIGINAL LINE: final int endPos = pos;
									int endPos = pos;
									string varName = varNameExpr;
									string varDefaultValue = null;
									if (valueDelimMatcher != null)
									{
										//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
										//ORIGINAL LINE: final char[] varNameExprChars = varNameExpr.toCharArray();
										char[] varNameExprChars = varNameExpr.ToCharArray();
										int valueDelimiterMatchLen = 0;
										for (int i = 0; i < varNameExprChars.Length; i++)
										{
											// if there's any nested variable when nested variable substitution disabled, then stop resolving name and default value.
											if (!substitutionInVariablesEnabled && pfxMatcher.isMatch(varNameExprChars, i, i, varNameExprChars.Length) >= 0)
											{
												break;
											}
											if ((valueDelimiterMatchLen = valueDelimMatcher.isMatch(varNameExprChars, i)) >= 0)
											{
												varName = varNameExpr.Substring(0, i);
												varDefaultValue = varNameExpr.Substring(i + valueDelimiterMatchLen);
												break;
											}
										}
									}
									// on the first call initialize priorVariables
									if (priorVariables == null)
									{
										priorVariables = new List<string>();
										priorVariables.Add(new string(chars, offset, length));
									}
									// handle cyclic substitution
									checkCyclicSubstitution(varName, priorVariables);
									priorVariables.Add(varName);
									// resolve the variable
									string varValue = resolveVariable(varName, buf, startPos, endPos);
									if (string.ReferenceEquals(varValue, null))
									{
										varValue = varDefaultValue;
									}
									if (!string.ReferenceEquals(varValue, null))
									{
										// recursive replace
										//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
										//ORIGINAL LINE: final int varLen = varValue.length();
										int varLen = varValue.Length;
										string oldValue = buf.ToString(startPos, endPos - startPos);
										buf.Remove(startPos, endPos - startPos);
										if (varValue != null)
										{
											buf.Insert(startPos, varValue, 1);
										}
										altered = true;
										int change = substitute(buf, startPos, varLen, priorVariables);
										change = change + varLen - (endPos - startPos);
										pos += change;
										bufEnd += change;
										lengthChange += change;
										chars = buf.ToString().ToCharArray(); // in case buffer was
																			  // altered
									}
									// remove variable from the cyclic stack
									priorVariables.RemoveAt(priorVariables.Count - 1);
									break;
								}
								nestedVarCount--;
								pos += endMatchLen;
							}
						}
					}
				}
			}
			if (top)
			{
				return altered ? 1 : 0;
			}
			return lengthChange;
		}

		/// <summary>
		/// Checks if the specified variable is already in the stack (list) of variables.
		/// </summary>
		/// <param name="varName">  the variable name to check </param>
		/// <param name="priorVariables">  the list of prior variables </param>
		private void checkCyclicSubstitution(in string varName, in IList<string> priorVariables)
		{
			if (priorVariables.Contains(varName) == false)
			{
				return;
			}
			IList<string> priorVariablesClone = new List<string>(priorVariables);
			priorVariables.RemoveAt(0);
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrBuilder buf = new StrBuilder(256);
			StringBuilder buf = new StringBuilder(256);
			buf.Append("Infinite loop in property interpolation of ");
			buf.Append(priorVariablesClone);
			buf.Append(": ");
			foreach (string variable in priorVariablesClone)
			{
				buf.Append(priorVariablesClone);
				buf.Append("->");
			}

			throw new System.InvalidOperationException(buf.ToString());
		}

		/// <summary>
		/// Internal method that resolves the value of a variable.
		/// <para>
		/// Most users of this class do not need to call this method. This method is
		/// called automatically by the substitution process.
		/// </para>
		/// <para>
		/// Writers of subclasses can override this method if they need to alter
		/// how each substitution occurs. The method is passed the variable's name
		/// and must return the corresponding value. This implementation uses the
		/// <seealso cref="getVariableResolver()"/> with the variable's name as the key.
		/// 
		/// </para>
		/// </summary>
		/// <param name="variableName">  the name of the variable, not null </param>
		/// <param name="buf">  the buffer where the substitution is occurring, not null </param>
		/// <param name="startPos">  the start position of the variable including the prefix, valid </param>
		/// <param name="endPos">  the end position of the variable including the suffix, valid </param>
		/// <returns> the variable's value or <b>null</b> if the variable is unknown </returns>
		protected internal virtual string resolveVariable(in string variableName, in StringBuilder buf, in int startPos, in int endPos)
		{
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrLookup<?> resolver = getVariableResolver();
			//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
			StrLookup resolver = VariableResolver;
			if (resolver == null)
			{
				return null;
			}
			return resolver.lookup(variableName);
		}

		// Escape
		//-----------------------------------------------------------------------
		/// <summary>
		/// Returns the escape character.
		/// </summary>
		/// <returns> the character used for escaping variable references </returns>
		public virtual char EscapeChar
		{
			get
			{
				return this.escapeChar;
			}
			set
			{
				this.escapeChar = value;
			}
		}


		// Prefix
		//-----------------------------------------------------------------------
		/// <summary>
		/// Gets the variable prefix matcher currently in use.
		/// <para>
		/// The variable prefix is the characer or characters that identify the
		/// start of a variable. This prefix is expressed in terms of a matcher
		/// allowing advanced prefix matches.
		/// 
		/// </para>
		/// </summary>
		/// <returns> the prefix matcher in use </returns>
		public virtual StrMatcher VariablePrefixMatcher
		{
			get
			{
				return prefixMatcher;
			}

		}

		/// <summary>
		/// Sets the variable prefix matcher currently in use.
		/// <para>
		/// The variable prefix is the characer or characters that identify the
		/// start of a variable. This prefix is expressed in terms of a matcher
		/// allowing advanced prefix matches.
		/// 
		/// </para>
		/// </summary>
		/// <param name="prefixMatcher">  the prefix matcher to use, null ignored </param>
		/// <returns> this, to enable chaining </returns>
		/// <exception cref="IllegalArgumentException"> if the prefix matcher is null </exception>
		public virtual StrSubstitutor setVariablePrefixMatcher(in StrMatcher prefixMatcher)
		{
			if (prefixMatcher == null)
			{
				throw new System.ArgumentException("Variable prefix matcher must not be null!");
			}
			this.prefixMatcher = prefixMatcher;
			return this;
		}

		/// <summary>
		/// Sets the variable prefix to use.
		/// <para>
		/// The variable prefix is the character or characters that identify the
		/// start of a variable. This method allows a single character prefix to
		/// be easily set.
		/// 
		/// </para>
		/// </summary>
		/// <param name="prefix">  the prefix character to use </param>
		/// <returns> this, to enable chaining </returns>
		public virtual StrSubstitutor setVariablePrefix(in char prefix)
		{
			return setVariablePrefixMatcher(StrMatcher.charMatcher(prefix));
		}

		/// <summary>
		/// Sets the variable prefix to use.
		/// <para>
		/// The variable prefix is the characer or characters that identify the
		/// start of a variable. This method allows a string prefix to be easily set.
		/// 
		/// </para>
		/// </summary>
		/// <param name="prefix">  the prefix for variables, not null </param>
		/// <returns> this, to enable chaining </returns>
		/// <exception cref="IllegalArgumentException"> if the prefix is null </exception>
		public virtual StrSubstitutor setVariablePrefix(in string prefix)
		{
			if (string.ReferenceEquals(prefix, null))
			{
				throw new System.ArgumentException("Variable prefix must not be null!");
			}
			return setVariablePrefixMatcher(StrMatcher.stringMatcher(prefix));
		}

		// Suffix
		//-----------------------------------------------------------------------
		/// <summary>
		/// Gets the variable suffix matcher currently in use.
		/// <para>
		/// The variable suffix is the characer or characters that identify the
		/// end of a variable. This suffix is expressed in terms of a matcher
		/// allowing advanced suffix matches.
		/// 
		/// </para>
		/// </summary>
		/// <returns> the suffix matcher in use </returns>
		public virtual StrMatcher VariableSuffixMatcher
		{
			get
			{
				return suffixMatcher;
			}
		}

		/// <summary>
		/// Sets the variable suffix matcher currently in use.
		/// <para>
		/// The variable suffix is the characer or characters that identify the
		/// end of a variable. This suffix is expressed in terms of a matcher
		/// allowing advanced suffix matches.
		/// 
		/// </para>
		/// </summary>
		/// <param name="suffixMatcher">  the suffix matcher to use, null ignored </param>
		/// <returns> this, to enable chaining </returns>
		/// <exception cref="IllegalArgumentException"> if the suffix matcher is null </exception>
		public virtual StrSubstitutor setVariableSuffixMatcher(in StrMatcher suffixMatcher)
		{
			if (suffixMatcher == null)
			{
				throw new System.ArgumentException("Variable suffix matcher must not be null!");
			}
			this.suffixMatcher = suffixMatcher;
			return this;
		}

		/// <summary>
		/// Sets the variable suffix to use.
		/// <para>
		/// The variable suffix is the characer or characters that identify the
		/// end of a variable. This method allows a single character suffix to
		/// be easily set.
		/// 
		/// </para>
		/// </summary>
		/// <param name="suffix">  the suffix character to use </param>
		/// <returns> this, to enable chaining </returns>
		public virtual StrSubstitutor setVariableSuffix(in char suffix)
		{
			return setVariableSuffixMatcher(StrMatcher.charMatcher(suffix));
		}

		/// <summary>
		/// Sets the variable suffix to use.
		/// <para>
		/// The variable suffix is the character or characters that identify the
		/// end of a variable. This method allows a string suffix to be easily set.
		/// 
		/// </para>
		/// </summary>
		/// <param name="suffix">  the suffix for variables, not null </param>
		/// <returns> this, to enable chaining </returns>
		/// <exception cref="IllegalArgumentException"> if the suffix is null </exception>
		public virtual StrSubstitutor setVariableSuffix(in string suffix)
		{
			if (string.ReferenceEquals(suffix, null))
			{
				throw new System.ArgumentException("Variable suffix must not be null!");
			}
			return setVariableSuffixMatcher(StrMatcher.stringMatcher(suffix));
		}

		// Variable Default Value Delimiter
		//-----------------------------------------------------------------------
		/// <summary>
		/// Gets the variable default value delimiter matcher currently in use.
		/// <para>
		/// The variable default value delimiter is the characer or characters that delimite the
		/// variable name and the variable default value. This delimiter is expressed in terms of a matcher
		/// allowing advanced variable default value delimiter matches.
		/// </para>
		/// <para>
		/// If it returns null, then the variable default value resolution is disabled.
		/// 
		/// </para>
		/// </summary>
		/// <returns> the variable default value delimiter matcher in use, may be null
		/// @since 3.2 </returns>
		public virtual StrMatcher ValueDelimiterMatcher
		{
			get
			{
				return valueDelimiterMatcher;
			}

		}

		/// <summary>
		/// Sets the variable default value delimiter matcher to use.
		/// <para>
		/// The variable default value delimiter is the characer or characters that delimite the
		/// variable name and the variable default value. This delimiter is expressed in terms of a matcher
		/// allowing advanced variable default value delimiter matches.
		/// </para>
		/// <para>
		/// If the <code>valueDelimiterMatcher</code> is null, then the variable default value resolution
		/// becomes disabled.
		/// 
		/// </para>
		/// </summary>
		/// <param name="valueDelimiterMatcher">  variable default value delimiter matcher to use, may be null </param>
		/// <returns> this, to enable chaining
		/// @since 3.2 </returns>
		public virtual StrSubstitutor setValueDelimiterMatcher(in StrMatcher valueDelimiterMatcher)
		{
			this.valueDelimiterMatcher = valueDelimiterMatcher;
			return this;
		}

		/// <summary>
		/// Sets the variable default value delimiter to use.
		/// <para>
		/// The variable default value delimiter is the characer or characters that delimite the
		/// variable name and the variable default value. This method allows a single character
		/// variable default value delimiter to be easily set.
		/// 
		/// </para>
		/// </summary>
		/// <param name="valueDelimiter">  the variable default value delimiter character to use </param>
		/// <returns> this, to enable chaining
		/// @since 3.2 </returns>
		public virtual StrSubstitutor setValueDelimiter(in char valueDelimiter)
		{
			return setValueDelimiterMatcher(StrMatcher.charMatcher(valueDelimiter));
		}

		/// <summary>
		/// Sets the variable default value delimiter to use.
		/// <para>
		/// The variable default value delimiter is the characer or characters that delimite the
		/// variable name and the variable default value. This method allows a string
		/// variable default value delimiter to be easily set.
		/// </para>
		/// <para>
		/// If the <code>valueDelimiter</code> is null or empty string, then the variable default
		/// value resolution becomes disabled.
		/// 
		/// </para>
		/// </summary>
		/// <param name="valueDelimiter">  the variable default value delimiter string to use, may be null or empty </param>
		/// <returns> this, to enable chaining
		/// @since 3.2 </returns>
		public virtual StrSubstitutor setValueDelimiter(in string valueDelimiter)
		{
			if (string.IsNullOrEmpty(valueDelimiter))
			{
				valueDelimiterMatcher = null;
				return this;
			}
			return setValueDelimiterMatcher(StrMatcher.stringMatcher(valueDelimiter));
		}

		// Resolver
		//-----------------------------------------------------------------------
		/// <summary>
		/// Gets the VariableResolver that is used to lookup variables.
		/// </summary>
		/// <returns> the VariableResolver </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: public StrLookup<?> getVariableResolver()
		public virtual StrLookup VariableResolver
		{
			get
			{
				return this.variableResolver;
			}
			set
			{
				this.variableResolver = value;
			}
		}


		// Substitution support in variable names
		//-----------------------------------------------------------------------
		/// <summary>
		/// Returns a flag whether substitution is done in variable names.
		/// </summary>
		/// <returns> the substitution in variable names flag
		/// @since 3.0 </returns>
		public virtual bool EnableSubstitutionInVariables
		{
			get
			{
				return enableSubstitutionInVariables;
			}
			set
			{
				this.enableSubstitutionInVariables = value;
			}
		}

	}

}