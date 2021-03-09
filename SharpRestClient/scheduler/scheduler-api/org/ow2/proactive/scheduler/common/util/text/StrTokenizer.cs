using System;
using System.Collections;
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
	/// Tokenizes a string based based on delimiters (separators)
	/// and supporting quoting and ignored character concepts.
	/// <para>
	/// This class can split a String into many smaller strings. It aims
	/// to do a similar job to <seealso cref="java.util.StringTokenizer StringTokenizer"/>,
	/// however it offers much more control and flexibility including implementing
	/// the <code>ListIterator</code> interface. By default, it is set up
	/// like <code>StringTokenizer</code>.
	/// </para>
	/// <para>
	/// The input String is split into a number of <i>tokens</i>.
	/// Each token is separated from the next String by a <i>delimiter</i>.
	/// One or more delimiter characters must be specified.
	/// </para>
	/// <para>
	/// Each token may be surrounded by quotes.
	/// The <i>quote</i> matcher specifies the quote character(s).
	/// A quote may be escaped within a quoted section by duplicating itself.
	/// </para>
	/// <para>
	/// Between each token and the delimiter are potentially characters that need trimming.
	/// The <i>trimmer</i> matcher specifies these characters.
	/// One usage might be to trim whitespace characters.
	/// </para>
	/// <para>
	/// At any point outside the quotes there might potentially be invalid characters.
	/// The <i>ignored</i> matcher specifies these characters to be removed.
	/// One usage might be to remove new line characters.
	/// </para>
	/// <para>
	/// Empty tokens may be removed or returned as null.
	/// <pre>
	/// "a,b,c"         - Three tokens "a","b","c"   (comma delimiter)
	/// " a, b , c "    - Three tokens "a","b","c"   (default CSV processing trims whitespace)
	/// "a, ", b ,", c" - Three tokens "a, " , " b ", ", c" (quoted text untouched)
	/// </pre>
	/// </para>
	/// <para>
	/// 
	/// This tokenizer has the following properties and options:
	/// 
	/// <table summary="Tokenizer Properties">
	///  <tr>
	///   <th>Property</th><th>Type</th><th>Default</th>
	///  </tr>
	///  <tr>
	///   <td>delim</td><td>CharSetMatcher</td><td>{ \t\n\r\f}</td>
	///  </tr>
	///  <tr>
	///   <td>quote</td><td>NoneMatcher</td><td>{}</td>
	///  </tr>
	///  <tr>
	///   <td>ignore</td><td>NoneMatcher</td><td>{}</td>
	///  </tr>
	///  <tr>
	///   <td>emptyTokenAsNull</td><td>boolean</td><td>false</td>
	///  </tr>
	///  <tr>
	///   <td>ignoreEmptyTokens</td><td>boolean</td><td>true</td>
	///  </tr>
	/// </table>
	/// 
	/// @since 2.2
	/// @version $Id$
	/// </para>
	/// </summary>
	public class StrTokenizer : IEnumerator, ICloneable
	{
		private static readonly StrTokenizer CSV_TOKENIZER_PROTOTYPE;

		private static readonly StrTokenizer TSV_TOKENIZER_PROTOTYPE;
		static StrTokenizer()
		{
			CSV_TOKENIZER_PROTOTYPE = new StrTokenizer();
			CSV_TOKENIZER_PROTOTYPE.delimMatcher = StrMatcher.commaMatcher();
			CSV_TOKENIZER_PROTOTYPE.quoteMatcher = StrMatcher.doubleQuoteMatcher();
			CSV_TOKENIZER_PROTOTYPE.ignoredMatcher = StrMatcher.noneMatcher();
			CSV_TOKENIZER_PROTOTYPE.trimmerMatcher = StrMatcher.trimMatcher();
			CSV_TOKENIZER_PROTOTYPE.emptyAsNull = false;
			CSV_TOKENIZER_PROTOTYPE.ignoreEmptyTokens = false;
			TSV_TOKENIZER_PROTOTYPE = new StrTokenizer();
			TSV_TOKENIZER_PROTOTYPE.delimMatcher = StrMatcher.tabMatcher();
			TSV_TOKENIZER_PROTOTYPE.quoteMatcher = StrMatcher.doubleQuoteMatcher();
			TSV_TOKENIZER_PROTOTYPE.ignoredMatcher = StrMatcher.noneMatcher();
			TSV_TOKENIZER_PROTOTYPE.trimmerMatcher = StrMatcher.trimMatcher();
			TSV_TOKENIZER_PROTOTYPE.emptyAsNull = false;
			TSV_TOKENIZER_PROTOTYPE.ignoreEmptyTokens = false;
		}

		/// <summary>
		/// The text to work on. </summary>
		private char[] chars;

		/// <summary>
		/// The parsed tokens </summary>
		private string[] tokens;

		/// <summary>
		/// The current iteration position </summary>
		private int tokenPos = 1;

		/// <summary>
		/// The delimiter matcher </summary>
		private StrMatcher delimMatcher = StrMatcher.splitMatcher();

		/// <summary>
		/// The quote matcher </summary>
		private StrMatcher quoteMatcher = StrMatcher.noneMatcher();

		/// <summary>
		/// The ignored matcher </summary>
		private StrMatcher ignoredMatcher = StrMatcher.noneMatcher();

		/// <summary>
		/// The trimmer matcher </summary>
		private StrMatcher trimmerMatcher = StrMatcher.noneMatcher();

		/// <summary>
		/// Whether to return empty tokens as null </summary>
		private bool emptyAsNull = false;

		/// <summary>
		/// Whether to ignore empty tokens </summary>
		private bool ignoreEmptyTokens = true;

		//-----------------------------------------------------------------------
		/// <summary>
		/// Returns a clone of <code>CSV_TOKENIZER_PROTOTYPE</code>.
		/// </summary>
		/// <returns> a clone of <code>CSV_TOKENIZER_PROTOTYPE</code>. </returns>
		private static StrTokenizer CSVClone
		{
			get
			{
				return (StrTokenizer)CSV_TOKENIZER_PROTOTYPE.Clone();
			}
		}

		/// <summary>
		/// Gets a new tokenizer instance which parses Comma Separated Value strings
		/// initializing it with the given input.  The default for CSV processing
		/// will be trim whitespace from both ends (which can be overridden with
		/// the setTrimmer method).
		/// <para>
		/// You must call a "reset" method to set the string which you want to parse.
		/// </para>
		/// </summary>
		/// <returns> a new tokenizer instance which parses Comma Separated Value strings </returns>
		public static StrTokenizer CSVInstance
		{
			get
			{
				return CSVClone;
			}
		}

		/// <summary>
		/// Gets a new tokenizer instance which parses Comma Separated Value strings
		/// initializing it with the given input.  The default for CSV processing
		/// will be trim whitespace from both ends (which can be overridden with
		/// the setTrimmer method).
		/// </summary>
		/// <param name="input">  the text to parse </param>
		/// <returns> a new tokenizer instance which parses Comma Separated Value strings </returns>
		public static StrTokenizer getCSVInstance(in string input)
		{
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrTokenizer tok = getCSVClone();
			StrTokenizer tok = CSVClone;
			tok.reset(input);
			return tok;
		}

		/// <summary>
		/// Gets a new tokenizer instance which parses Comma Separated Value strings
		/// initializing it with the given input.  The default for CSV processing
		/// will be trim whitespace from both ends (which can be overridden with
		/// the setTrimmer method).
		/// </summary>
		/// <param name="input">  the text to parse </param>
		/// <returns> a new tokenizer instance which parses Comma Separated Value strings </returns>
		public static StrTokenizer getCSVInstance(in char[] input)
		{
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrTokenizer tok = getCSVClone();
			StrTokenizer tok = CSVClone;
			tok.reset(input);
			return tok;
		}

		/// <summary>
		/// Returns a clone of <code>TSV_TOKENIZER_PROTOTYPE</code>.
		/// </summary>
		/// <returns> a clone of <code>TSV_TOKENIZER_PROTOTYPE</code>. </returns>
		private static StrTokenizer TSVClone
		{
			get
			{
				return (StrTokenizer)TSV_TOKENIZER_PROTOTYPE.Clone();
			}
		}

		/// <summary>
		/// Gets a new tokenizer instance which parses Tab Separated Value strings.
		/// The default for CSV processing will be trim whitespace from both ends
		/// (which can be overridden with the setTrimmer method).
		/// <para>
		/// You must call a "reset" method to set the string which you want to parse.
		/// </para>
		/// </summary>
		/// <returns> a new tokenizer instance which parses Tab Separated Value strings. </returns>
		public static StrTokenizer TSVInstance
		{
			get
			{
				return TSVClone;
			}
		}

		/// <summary>
		/// Gets a new tokenizer instance which parses Tab Separated Value strings.
		/// The default for CSV processing will be trim whitespace from both ends
		/// (which can be overridden with the setTrimmer method). </summary>
		/// <param name="input">  the string to parse </param>
		/// <returns> a new tokenizer instance which parses Tab Separated Value strings. </returns>
		public static StrTokenizer getTSVInstance(in string input)
		{
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrTokenizer tok = getTSVClone();
			StrTokenizer tok = TSVClone;
			tok.reset(input);
			return tok;
		}

		/// <summary>
		/// Gets a new tokenizer instance which parses Tab Separated Value strings.
		/// The default for CSV processing will be trim whitespace from both ends
		/// (which can be overridden with the setTrimmer method). </summary>
		/// <param name="input">  the string to parse </param>
		/// <returns> a new tokenizer instance which parses Tab Separated Value strings. </returns>
		public static StrTokenizer getTSVInstance(in char[] input)
		{
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrTokenizer tok = getTSVClone();
			StrTokenizer tok = TSVClone;
			tok.reset(input);
			return tok;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Constructs a tokenizer splitting on space, tab, newline and formfeed
		/// as per StringTokenizer, but with no text to tokenize.
		/// <para>
		/// This constructor is normally used with <seealso cref="reset(string)"/>.
		/// </para>
		/// </summary>
		public StrTokenizer() : base()
		{
			this.chars = null;
		}

		/// <summary>
		/// Constructs a tokenizer splitting on space, tab, newline and formfeed
		/// as per StringTokenizer.
		/// </summary>
		/// <param name="input">  the string which is to be parsed </param>
		public StrTokenizer(in string input) : base()
		{
			if (!string.ReferenceEquals(input, null))
			{
				chars = input.ToCharArray();
			}
			else
			{
				chars = null;
			}
		}

		/// <summary>
		/// Constructs a tokenizer splitting on the specified delimiter character.
		/// </summary>
		/// <param name="input">  the string which is to be parsed </param>
		/// <param name="delim">  the field delimiter character </param>
		public StrTokenizer(in string input, in char delim) : this(input)
		{
			setDelimiterChar(delim);
		}

		/// <summary>
		/// Constructs a tokenizer splitting on the specified delimiter string.
		/// </summary>
		/// <param name="input">  the string which is to be parsed </param>
		/// <param name="delim">  the field delimiter string </param>
		public StrTokenizer(in string input, in string delim) : this(input)
		{
			setDelimiterString(delim);
		}

		/// <summary>
		/// Constructs a tokenizer splitting using the specified delimiter matcher.
		/// </summary>
		/// <param name="input">  the string which is to be parsed </param>
		/// <param name="delim">  the field delimiter matcher </param>
		public StrTokenizer(in string input, in StrMatcher delim) : this(input)
		{
			setDelimiterMatcher(delim);
		}

		/// <summary>
		/// Constructs a tokenizer splitting on the specified delimiter character
		/// and handling quotes using the specified quote character.
		/// </summary>
		/// <param name="input">  the string which is to be parsed </param>
		/// <param name="delim">  the field delimiter character </param>
		/// <param name="quote">  the field quoted string character </param>
		public StrTokenizer(in string input, in char delim, in char quote) : this(input, delim)
		{
			setQuoteChar(quote);
		}

		/// <summary>
		/// Constructs a tokenizer splitting using the specified delimiter matcher
		/// and handling quotes using the specified quote matcher.
		/// </summary>
		/// <param name="input">  the string which is to be parsed </param>
		/// <param name="delim">  the field delimiter matcher </param>
		/// <param name="quote">  the field quoted string matcher </param>
		public StrTokenizer(in string input, in StrMatcher delim, in StrMatcher quote) : this(input, delim)
		{
			setQuoteMatcher(quote);
		}

		/// <summary>
		/// Constructs a tokenizer splitting on space, tab, newline and formfeed
		/// as per StringTokenizer.
		/// </summary>
		/// <param name="input">  the string which is to be parsed, not cloned </param>
		public StrTokenizer(in char[] input) : base()
		{
			this.chars = new char[input.Length];
			Array.Copy(input, this.chars, input.Length);
		}

		/// <summary>
		/// Constructs a tokenizer splitting on the specified character.
		/// </summary>
		/// <param name="input">  the string which is to be parsed, not cloned </param>
		/// <param name="delim"> the field delimiter character </param>
		public StrTokenizer(in char[] input, in char delim) : this(input)
		{
			setDelimiterChar(delim);
		}

		/// <summary>
		/// Constructs a tokenizer splitting on the specified string.
		/// </summary>
		/// <param name="input">  the string which is to be parsed, not cloned </param>
		/// <param name="delim"> the field delimiter string </param>
		public StrTokenizer(in char[] input, in string delim) : this(input)
		{
			setDelimiterString(delim);
		}

		/// <summary>
		/// Constructs a tokenizer splitting using the specified delimiter matcher.
		/// </summary>
		/// <param name="input">  the string which is to be parsed, not cloned </param>
		/// <param name="delim">  the field delimiter matcher </param>
		public StrTokenizer(in char[] input, in StrMatcher delim) : this(input)
		{
			setDelimiterMatcher(delim);
		}

		/// <summary>
		/// Constructs a tokenizer splitting on the specified delimiter character
		/// and handling quotes using the specified quote character.
		/// </summary>
		/// <param name="input">  the string which is to be parsed, not cloned </param>
		/// <param name="delim">  the field delimiter character </param>
		/// <param name="quote">  the field quoted string character </param>
		public StrTokenizer(in char[] input, in char delim, in char quote) : this(input, delim)
		{
			setQuoteChar(quote);
		}

		/// <summary>
		/// Constructs a tokenizer splitting using the specified delimiter matcher
		/// and handling quotes using the specified quote matcher.
		/// </summary>
		/// <param name="input">  the string which is to be parsed, not cloned </param>
		/// <param name="delim">  the field delimiter character </param>
		/// <param name="quote">  the field quoted string character </param>
		public StrTokenizer(in char[] input, in StrMatcher delim, in StrMatcher quote) : this(input, delim)
		{
			setQuoteMatcher(quote);
		}

		// API
		//-----------------------------------------------------------------------
		/// <summary>
		/// Gets the number of tokens found in the String.
		/// </summary>
		/// <returns> the number of matched tokens </returns>
		public virtual int size()
		{
			checkTokenized();
			return tokens.Length;
		}

		/// <summary>
		/// Gets the next token from the String.
		/// Equivalent to <seealso cref="next()"/> except it returns null rather than
		/// throwing <seealso cref="NoSuchElementException"/> when no tokens remain.
		/// </summary>
		/// <returns> the next sequential token, or null when no more tokens are found </returns>
		public virtual string nextToken()
		{
			//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
			if (hasNext())
			{
				return tokens[++tokenPos];
			}
			return null;
		}

		/// <summary>
		/// Gets the previous token from the String.
		/// </summary>
		/// <returns> the previous sequential token, or null when no more tokens are found </returns>
		public virtual string previousToken()
		{
			if (hasPrevious())
			{
				return tokens[--tokenPos];
			}
			return null;
		}

		/// <summary>
		/// Gets a copy of the full token list as an independent modifiable array.
		/// </summary>
		/// <returns> the tokens as a String array </returns>
		public virtual string[] TokenArray
		{
			get
			{
				checkTokenized();
				return (string[])tokens.Clone();
			}
		}

		/// <summary>
		/// Gets a copy of the full token list as an independent modifiable list.
		/// </summary>
		/// <returns> the tokens as a String array </returns>
		public virtual IList<string> TokenList
		{
			get
			{
				checkTokenized();
				//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
				//ORIGINAL LINE: final java.util.List<String> list = new java.util.ArrayList<String>(tokens.length);
				IList<string> list = new List<string>(tokens.Length);
				foreach (String element in tokens)
				{
					list.Add(element);
				}
				return list;
			}
		}

		/// <summary>
		/// Resets this tokenizer, forgetting all parsing and iteration already completed.
		/// <para>
		/// This method allows the same tokenizer to be reused for the same String.
		/// 
		/// </para>
		/// </summary>
		/// <returns> this, to enable chaining </returns>
		public virtual StrTokenizer reset()
		{
			tokenPos = 0;
			tokens = null;
			return this;
		}

		/// <summary>
		/// Reset this tokenizer, giving it a new input string to parse.
		/// In this manner you can re-use a tokenizer with the same settings
		/// on multiple input lines.
		/// </summary>
		/// <param name="input">  the new string to tokenize, null sets no text to parse </param>
		/// <returns> this, to enable chaining </returns>
		public virtual StrTokenizer reset(in string input)
		{
			reset();
			if (!string.ReferenceEquals(input, null))
			{
				this.chars = input.ToCharArray();
			}
			else
			{
				this.chars = null;
			}
			return this;
		}

		/// <summary>
		/// Reset this tokenizer, giving it a new input string to parse.
		/// In this manner you can re-use a tokenizer with the same settings
		/// on multiple input lines.
		/// </summary>
		/// <param name="input">  the new character array to tokenize, not cloned, null sets no text to parse </param>
		/// <returns> this, to enable chaining </returns>
		public virtual StrTokenizer reset(in char[] input)
		{
			reset();
			this.chars = new char[input.Length];
			Array.Copy(input, this.chars, input.Length);
			return this;
		}

		// ListIterator
		//-----------------------------------------------------------------------
		/// <summary>
		/// Checks whether there are any more tokens.
		/// </summary>
		/// <returns> true if there are more tokens </returns>
		public bool hasNext()
		{
			checkTokenized();
			return tokenPos < tokens.Length;
		}

		/// <summary>
		/// Gets the next token.
		/// </summary>
		/// <returns> the next String token </returns>
		public string next()
		{
			//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
			if (hasNext())
			{
				return tokens[++tokenPos];
			}
			throw new InvalidOperationException();
		}

		/// <summary>
		/// Gets the next token.
		/// </summary>
		/// <returns> the next String token </returns>
		public bool MoveNext()
		{
			//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
			tokenPos++;
			return tokenPos < tokens.Length;
		}

		public string Current
		{
			get
			{
				try
				{
					return tokens[tokenPos];
				}
				catch (IndexOutOfRangeException)
				{
					throw new InvalidOperationException();
				}
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return Current;
			}
		}

		public void Reset()
		{
			tokenPos = -1;
		}

		/// <summary>
		/// Gets the index of the next token to return.
		/// </summary>
		/// <returns> the next token index </returns>
		public int nextIndex()
		{
			return tokenPos + 1;
		}

		/// <summary>
		/// Checks whether there are any previous tokens that can be iterated to.
		/// </summary>
		/// <returns> true if there are previous tokens </returns>
		public bool hasPrevious()
		{
			checkTokenized();
			return tokenPos > 0;
		}

		/// <summary>
		/// Gets the token previous to the last returned token.
		/// </summary>
		/// <returns> the previous token </returns>
		public string previous()
		{
			if (hasPrevious())
			{
				return tokens[--tokenPos];
			}
			throw new InvalidOperationException();
		}

		/// <summary>
		/// Gets the index of the previous token.
		/// </summary>
		/// <returns> the previous token index </returns>
		public int previousIndex()
		{
			return tokenPos - 1;
		}

		/// <summary>
		/// Unsupported ListIterator operation.
		/// </summary>
		/// <exception cref="UnsupportedOperationException"> always </exception>
		public void remove()
		{
			throw new System.NotSupportedException("remove() is unsupported");
		}

		/// <summary>
		/// Unsupported ListIterator operation. </summary>
		/// <param name="obj"> this parameter ignored. </param>
		/// <exception cref="UnsupportedOperationException"> always </exception>
		public void set(in string obj)
		{
			throw new System.NotSupportedException("set() is unsupported");
		}

		/// <summary>
		/// Unsupported ListIterator operation. </summary>
		/// <param name="obj"> this parameter ignored. </param>
		/// <exception cref="UnsupportedOperationException"> always </exception>
		public void add(in string obj)
		{
			throw new System.NotSupportedException("add() is unsupported");
		}

		// Implementation
		//-----------------------------------------------------------------------
		/// <summary>
		/// Checks if tokenization has been done, and if not then do it.
		/// </summary>
		private void checkTokenized()
		{
			if (tokens == null)
			{
				if (chars == null)
				{
					// still call tokenize as subclass may do some work
					//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
					//ORIGINAL LINE: final java.util.List<String> split = tokenize(null, 0, 0);
					IList<string> split = tokenize(null, 0, 0);
					tokens = ((List<string>)split).ToArray();
				}
				else
				{
					//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
					//ORIGINAL LINE: final java.util.List<String> split = tokenize(chars, 0, chars.length);
					IList<string> split = tokenize(chars, 0, chars.Length);
					tokens = ((List<string>)split).ToArray();
				}
			}
		}

		/// <summary>
		/// Internal method to performs the tokenization.
		/// <para>
		/// Most users of this class do not need to call this method. This method
		/// will be called automatically by other (public) methods when required.
		/// </para>
		/// <para>
		/// This method exists to allow subclasses to add code before or after the
		/// tokenization. For example, a subclass could alter the character array,
		/// offset or count to be parsed, or call the tokenizer multiple times on
		/// multiple strings. It is also be possible to filter the results.
		/// </para>
		/// <para>
		/// <code>StrTokenizer</code> will always pass a zero offset and a count
		/// equal to the length of the array to this method, however a subclass
		/// may pass other values, or even an entirely different array.
		/// 
		/// </para>
		/// </summary>
		/// <param name="srcChars">  the character array being tokenized, may be null </param>
		/// <param name="offset">  the start position within the character array, must be valid </param>
		/// <param name="count">  the number of characters to tokenize, must be valid </param>
		/// <returns> the modifiable list of String tokens, unmodifiable if null array or zero count </returns>
		protected internal virtual IList<string> tokenize(in char[] srcChars, in int offset, in int count)
		{
			if (srcChars == null || count == 0)
			{
				return new List<string>();
			}
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrBuilder buf = new StrBuilder();
			StringBuilder buf = new StringBuilder();
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final java.util.List<String> tokenList = new java.util.ArrayList<String>();
			IList<string> tokenList = new List<string>();
			int pos = offset;

			// loop around the entire buffer
			while (pos >= 0 && pos < count)
			{
				// find next token
				pos = readNextToken(srcChars, pos, count, buf, tokenList);

				// handle case where end of string is a delimiter
				if (pos >= count)
				{
					addToken(tokenList, "");
				}
			}
			return tokenList;
		}

		/// <summary>
		/// Adds a token to a list, paying attention to the parameters we've set.
		/// </summary>
		/// <param name="list">  the list to add to </param>
		/// <param name="tok">  the token to add </param>
		private void addToken(in IList<string> list, string tok)
		{
			if (string.IsNullOrEmpty(tok))
			{
				if (IgnoreEmptyTokens)
				{
					return;
				}
				if (EmptyTokenAsNull)
				{
					tok = null;
				}
			}
			list.Add(tok);
		}

		/// <summary>
		/// Reads character by character through the String to get the next token.
		/// </summary>
		/// <param name="srcChars">  the character array being tokenized </param>
		/// <param name="start">  the first character of field </param>
		/// <param name="len">  the length of the character array being tokenized </param>
		/// <param name="workArea">  a temporary work area </param>
		/// <param name="tokenList">  the list of parsed tokens </param>
		/// <returns> the starting position of the next field (the character
		///  immediately after the delimiter), or -1 if end of string found </returns>
		private int readNextToken(in char[] srcChars, int start, in int len, in StringBuilder workArea, in IList<string> tokenList)
		{
			// skip all leading whitespace, unless it is the
			// field delimiter or the quote character
			while (start < len)
			{
				//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
				//ORIGINAL LINE: final int removeLen = Math.max(getIgnoredMatcher().isMatch(srcChars, start, start, len), getTrimmerMatcher().isMatch(srcChars, start, start, len));
				int removeLen = Math.Max(IgnoredMatcher.isMatch(srcChars, start, start, len), TrimmerMatcher.isMatch(srcChars, start, start, len));
				if (removeLen == 0 || DelimiterMatcher.isMatch(srcChars, start, start, len) > 0 || QuoteMatcher.isMatch(srcChars, start, start, len) > 0)
				{
					break;
				}
				start += removeLen;
			}

			// handle reaching end
			if (start >= len)
			{
				addToken(tokenList, "");
				return -1;
			}

			// handle empty token
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final int delimLen = getDelimiterMatcher().isMatch(srcChars, start, start, len);
			int delimLen = DelimiterMatcher.isMatch(srcChars, start, start, len);
			if (delimLen > 0)
			{
				addToken(tokenList, "");
				return start + delimLen;
			}

			// handle found token
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final int quoteLen = getQuoteMatcher().isMatch(srcChars, start, start, len);
			int quoteLen = QuoteMatcher.isMatch(srcChars, start, start, len);
			if (quoteLen > 0)
			{
				return readWithQuotes(srcChars, start + quoteLen, len, workArea, tokenList, start, quoteLen);
			}
			return readWithQuotes(srcChars, start, len, workArea, tokenList, 0, 0);
		}

		/// <summary>
		/// Reads a possibly quoted string token.
		/// </summary>
		/// <param name="srcChars">  the character array being tokenized </param>
		/// <param name="start">  the first character of field </param>
		/// <param name="len">  the length of the character array being tokenized </param>
		/// <param name="workArea">  a temporary work area </param>
		/// <param name="tokenList">  the list of parsed tokens </param>
		/// <param name="quoteStart">  the start position of the matched quote, 0 if no quoting </param>
		/// <param name="quoteLen">  the length of the matched quote, 0 if no quoting </param>
		/// <returns> the starting position of the next field (the character
		///  immediately after the delimiter, or if end of string found,
		///  then the length of string </returns>
		private int readWithQuotes(in char[] srcChars, in int start, in int len, in StringBuilder workArea, in IList<string> tokenList, in int quoteStart, in int quoteLen)
		{
			// Loop until we've found the end of the quoted
			// string or the end of the input
			workArea.Clear();
			int pos = start;
			bool quoting = quoteLen > 0;
			int trimStart = 0;

			while (pos < len)
			{
				// quoting mode can occur several times throughout a string
				// we must switch between quoting and non-quoting until we
				// encounter a non-quoted delimiter, or end of string
				if (quoting)
				{
					// In quoting mode

					// If we've found a quote character, see if it's
					// followed by a second quote.  If so, then we need
					// to actually put the quote character into the token
					// rather than end the token.
					if (isQuote(srcChars, pos, len, quoteStart, quoteLen))
					{
						if (isQuote(srcChars, pos + quoteLen, len, quoteStart, quoteLen))
						{
							// matched pair of quotes, thus an escaped quote
							workArea.Append(srcChars, pos, quoteLen);
							pos += quoteLen * 2;
							trimStart = workArea.Length;
							continue;
						}

						// end of quoting
						quoting = false;
						pos += quoteLen;
						continue;
					}

					// copy regular character from inside quotes
					workArea.Append(srcChars[pos++]);
					trimStart = workArea.Length;

				}
				else
				{
					// Not in quoting mode

					// check for delimiter, and thus end of token
					//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
					//ORIGINAL LINE: final int delimLen = getDelimiterMatcher().isMatch(srcChars, pos, start, len);
					int delimLen = DelimiterMatcher.isMatch(srcChars, pos, start, len);
					if (delimLen > 0)
					{
						// return condition when end of token found
						addToken(tokenList, workArea.ToString().Substring(0, trimStart));
						return pos + delimLen;
					}

					// check for quote, and thus back into quoting mode
					if (quoteLen > 0 && isQuote(srcChars, pos, len, quoteStart, quoteLen))
					{
						quoting = true;
						pos += quoteLen;
						continue;
					}

					// check for ignored (outside quotes), and ignore
					//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
					//ORIGINAL LINE: final int ignoredLen = getIgnoredMatcher().isMatch(srcChars, pos, start, len);
					int ignoredLen = IgnoredMatcher.isMatch(srcChars, pos, start, len);
					if (ignoredLen > 0)
					{
						pos += ignoredLen;
						continue;
					}

					// check for trimmed character
					// don't yet know if its at the end, so copy to workArea
					// use trimStart to keep track of trim at the end
					//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
					//ORIGINAL LINE: final int trimmedLen = getTrimmerMatcher().isMatch(srcChars, pos, start, len);
					int trimmedLen = TrimmerMatcher.isMatch(srcChars, pos, start, len);
					if (trimmedLen > 0)
					{
						workArea.Append(srcChars, pos, trimmedLen);
						pos += trimmedLen;
						continue;
					}

					// copy regular character from outside quotes
					workArea.Append(srcChars[pos++]);
					trimStart = workArea.Length;
				}
			}

			// return condition when end of string found
			addToken(tokenList, workArea.ToString().Substring(0, trimStart));
			return -1;
		}

		/// <summary>
		/// Checks if the characters at the index specified match the quote
		/// already matched in readNextToken().
		/// </summary>
		/// <param name="srcChars">  the character array being tokenized </param>
		/// <param name="pos">  the position to check for a quote </param>
		/// <param name="len">  the length of the character array being tokenized </param>
		/// <param name="quoteStart">  the start position of the matched quote, 0 if no quoting </param>
		/// <param name="quoteLen">  the length of the matched quote, 0 if no quoting </param>
		/// <returns> true if a quote is matched </returns>
		private bool isQuote(in char[] srcChars, in int pos, in int len, in int quoteStart, in int quoteLen)
		{
			for (int i = 0; i < quoteLen; i++)
			{
				if (pos + i >= len || srcChars[pos + i] != srcChars[quoteStart + i])
				{
					return false;
				}
			}
			return true;
		}

		// Delimiter
		//-----------------------------------------------------------------------
		/// <summary>
		/// Gets the field delimiter matcher.
		/// </summary>
		/// <returns> the delimiter matcher in use </returns>
		public virtual StrMatcher DelimiterMatcher
		{
			get
			{
				return this.delimMatcher;
			}
		}

		/// <summary>
		/// Sets the field delimiter matcher.
		/// <para>
		/// The delimitier is used to separate one token from another.
		/// 
		/// </para>
		/// </summary>
		/// <param name="delim">  the delimiter matcher to use </param>
		/// <returns> this, to enable chaining </returns>
		public virtual StrTokenizer setDelimiterMatcher(in StrMatcher delim)
		{
			if (delim == null)
			{
				this.delimMatcher = StrMatcher.noneMatcher();
			}
			else
			{
				this.delimMatcher = delim;
			}
			return this;
		}

		/// <summary>
		/// Sets the field delimiter character.
		/// </summary>
		/// <param name="delim">  the delimiter character to use </param>
		/// <returns> this, to enable chaining </returns>
		public virtual StrTokenizer setDelimiterChar(in char delim)
		{
			return setDelimiterMatcher(StrMatcher.charMatcher(delim));
		}

		/// <summary>
		/// Sets the field delimiter string.
		/// </summary>
		/// <param name="delim">  the delimiter string to use </param>
		/// <returns> this, to enable chaining </returns>
		public virtual StrTokenizer setDelimiterString(in string delim)
		{
			return setDelimiterMatcher(StrMatcher.stringMatcher(delim));
		}

		// Quote
		//-----------------------------------------------------------------------
		/// <summary>
		/// Gets the quote matcher currently in use.
		/// <para>
		/// The quote character is used to wrap data between the tokens.
		/// This enables delimiters to be entered as data.
		/// The default value is '"' (double quote).
		/// 
		/// </para>
		/// </summary>
		/// <returns> the quote matcher in use </returns>
		public virtual StrMatcher QuoteMatcher
		{
			get
			{
				return quoteMatcher;
			}
		}

		/// <summary>
		/// Set the quote matcher to use.
		/// <para>
		/// The quote character is used to wrap data between the tokens.
		/// This enables delimiters to be entered as data.
		/// 
		/// </para>
		/// </summary>
		/// <param name="quote">  the quote matcher to use, null ignored </param>
		/// <returns> this, to enable chaining </returns>
		public virtual StrTokenizer setQuoteMatcher(in StrMatcher quote)
		{
			if (quote != null)
			{
				this.quoteMatcher = quote;
			}
			return this;
		}

		/// <summary>
		/// Sets the quote character to use.
		/// <para>
		/// The quote character is used to wrap data between the tokens.
		/// This enables delimiters to be entered as data.
		/// 
		/// </para>
		/// </summary>
		/// <param name="quote">  the quote character to use </param>
		/// <returns> this, to enable chaining </returns>
		public virtual StrTokenizer setQuoteChar(in char quote)
		{
			return setQuoteMatcher(StrMatcher.charMatcher(quote));
		}

		// Ignored
		//-----------------------------------------------------------------------
		/// <summary>
		/// Gets the ignored character matcher.
		/// <para>
		/// These characters are ignored when parsing the String, unless they are
		/// within a quoted region.
		/// The default value is not to ignore anything.
		/// 
		/// </para>
		/// </summary>
		/// <returns> the ignored matcher in use </returns>
		public virtual StrMatcher IgnoredMatcher
		{
			get
			{
				return ignoredMatcher;
			}
		}

		/// <summary>
		/// Set the matcher for characters to ignore.
		/// <para>
		/// These characters are ignored when parsing the String, unless they are
		/// within a quoted region.
		/// 
		/// </para>
		/// </summary>
		/// <param name="ignored">  the ignored matcher to use, null ignored </param>
		/// <returns> this, to enable chaining </returns>
		public virtual StrTokenizer setIgnoredMatcher(in StrMatcher ignored)
		{
			if (ignored != null)
			{
				this.ignoredMatcher = ignored;
			}
			return this;
		}

		/// <summary>
		/// Set the character to ignore.
		/// <para>
		/// This character is ignored when parsing the String, unless it is
		/// within a quoted region.
		/// 
		/// </para>
		/// </summary>
		/// <param name="ignored">  the ignored character to use </param>
		/// <returns> this, to enable chaining </returns>
		public virtual StrTokenizer setIgnoredChar(in char ignored)
		{
			return setIgnoredMatcher(StrMatcher.charMatcher(ignored));
		}

		// Trimmer
		//-----------------------------------------------------------------------
		/// <summary>
		/// Gets the trimmer character matcher.
		/// <para>
		/// These characters are trimmed off on each side of the delimiter
		/// until the token or quote is found.
		/// The default value is not to trim anything.
		/// 
		/// </para>
		/// </summary>
		/// <returns> the trimmer matcher in use </returns>
		public virtual StrMatcher TrimmerMatcher
		{
			get
			{
				return trimmerMatcher;
			}
		}

		/// <summary>
		/// Sets the matcher for characters to trim.
		/// <para>
		/// These characters are trimmed off on each side of the delimiter
		/// until the token or quote is found.
		/// 
		/// </para>
		/// </summary>
		/// <param name="trimmer">  the trimmer matcher to use, null ignored </param>
		/// <returns> this, to enable chaining </returns>
		public virtual StrTokenizer setTrimmerMatcher(in StrMatcher trimmer)
		{
			if (trimmer != null)
			{
				this.trimmerMatcher = trimmer;
			}
			return this;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Gets whether the tokenizer currently returns empty tokens as null.
		/// The default for this property is false.
		/// </summary>
		/// <returns> true if empty tokens are returned as null </returns>
		public virtual bool EmptyTokenAsNull
		{
			get
			{
				return this.emptyAsNull;
			}
		}

		/// <summary>
		/// Sets whether the tokenizer should return empty tokens as null.
		/// The default for this property is false.
		/// </summary>
		/// <param name="emptyAsNull">  whether empty tokens are returned as null </param>
		/// <returns> this, to enable chaining </returns>
		public virtual StrTokenizer setEmptyTokenAsNull(in bool emptyAsNull)
		{
			this.emptyAsNull = emptyAsNull;
			return this;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Gets whether the tokenizer currently ignores empty tokens.
		/// The default for this property is true.
		/// </summary>
		/// <returns> true if empty tokens are not returned </returns>
		public virtual bool IgnoreEmptyTokens
		{
			get
			{
				return ignoreEmptyTokens;
			}
		}

		/// <summary>
		/// Sets whether the tokenizer should ignore and not return empty tokens.
		/// The default for this property is true.
		/// </summary>
		/// <param name="ignoreEmptyTokens">  whether empty tokens are not returned </param>
		/// <returns> this, to enable chaining </returns>
		public virtual StrTokenizer setIgnoreEmptyTokens(in bool ignoreEmptyTokens)
		{
			this.ignoreEmptyTokens = ignoreEmptyTokens;
			return this;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Gets the String content that the tokenizer is parsing.
		/// </summary>
		/// <returns> the string content being parsed </returns>
		public virtual string Content
		{
			get
			{
				if (chars == null)
				{
					return null;
				}
				return new string(chars);
			}
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Creates a new instance of this Tokenizer. The new instance is reset so
		/// that it will be at the start of the token list.
		/// If a <seealso cref="CloneNotSupportedException"/> is caught, return <code>null</code>.
		/// </summary>
		/// <returns> a new instance of this Tokenizer which has been reset. </returns>
		public object Clone()
		{
			try
			{
				return cloneReset();
			}
			//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
			//ORIGINAL LINE: catch (final CloneNotSupportedException ex)
			catch (Exception e)
			{
				return null;
			}
		}

		/// <summary>
		/// Creates a new instance of this Tokenizer. The new instance is reset so that
		/// it will be at the start of the token list.
		/// </summary>
		/// <returns> a new instance of this Tokenizer which has been reset. </returns>
		/// <exception cref="CloneNotSupportedException"> if there is a problem cloning </exception>
		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: Object cloneReset() throws CloneNotSupportedException
		internal virtual object cloneReset()
		{
			// this method exists to enable 100% test coverage
			//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
			//ORIGINAL LINE: final StrTokenizer cloned = (StrTokenizer) super.clone();
			StrTokenizer cloned = (StrTokenizer)base.MemberwiseClone();
			if (cloned.chars != null)
			{
				cloned.chars = (char[])cloned.chars.Clone();
			}
			cloned.reset();
			return cloned;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Gets the String content that the tokenizer is parsing.
		/// </summary>
		/// <returns> the string content being parsed </returns>
		public override string ToString()
		{
			if (tokens == null)
			{
				return "StrTokenizer[not tokenized yet]";
			}
			return "StrTokenizer" + TokenList;
		}


	}
}