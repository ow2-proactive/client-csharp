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
	/// A matcher class that can be queried to determine if a character array
	/// portion matches.
	/// <para>
	/// This class comes complete with various factory methods.
	/// If these do not suffice, you can subclass and implement your own matcher.
	/// 
	/// NOTICE: change from the original source code.
	/// Semantic of isMatch has been modified to return:
	/// -1 in case of no match
	/// a positive value in case of a match (the match can contain 0 characters)
	/// 
	/// @since 2.2
	/// @version $Id$
	/// </para>
	/// </summary>
	public abstract class StrMatcher
	{
		/// <summary>
		/// Matches the comma character.
		/// </summary>
		private static readonly StrMatcher COMMA_MATCHER = new CharMatcher(',');

		/// <summary>
		/// Matches the tab character.
		/// </summary>
		private static readonly StrMatcher TAB_MATCHER = new CharMatcher('\t');

		/// <summary>
		/// Matches the space character.
		/// </summary>
		private static readonly StrMatcher SPACE_MATCHER = new CharMatcher(' ');

		/// <summary>
		/// Matches the same characters as StringTokenizer,
		/// namely space, tab, newline, formfeed.
		/// </summary>
		private static readonly StrMatcher SPLIT_MATCHER = new CharSetMatcher(" \t\n\r\f".ToCharArray());

		/// <summary>
		/// Matches the String trim() whitespace characters.
		/// </summary>
		private static readonly StrMatcher TRIM_MATCHER = new TrimMatcher();

		/// <summary>
		/// Matches the double quote character.
		/// </summary>
		private static readonly StrMatcher SINGLE_QUOTE_MATCHER = new CharMatcher('\'');

		/// <summary>
		/// Matches the double quote character.
		/// </summary>
		private static readonly StrMatcher DOUBLE_QUOTE_MATCHER = new CharMatcher('"');

		/// <summary>
		/// Matches the single or double quote character.
		/// </summary>
		private static readonly StrMatcher QUOTE_MATCHER = new CharSetMatcher("'\"".ToCharArray());

		/// <summary>
		/// Matches no characters.
		/// </summary>
		private static readonly StrMatcher NONE_MATCHER = new NoMatcher();

		// -----------------------------------------------------------------------
		/// <summary>
		/// Returns a matcher which matches the comma character.
		/// </summary>
		/// <returns> a matcher for a comma </returns>
		public static StrMatcher commaMatcher()
		{
			return COMMA_MATCHER;
		}

		/// <summary>
		/// Returns a matcher which matches the tab character.
		/// </summary>
		/// <returns> a matcher for a tab </returns>
		public static StrMatcher tabMatcher()
		{
			return TAB_MATCHER;
		}

		/// <summary>
		/// Returns a matcher which matches the space character.
		/// </summary>
		/// <returns> a matcher for a space </returns>
		public static StrMatcher spaceMatcher()
		{
			return SPACE_MATCHER;
		}

		/// <summary>
		/// Matches the same characters as StringTokenizer,
		/// namely space, tab, newline and formfeed.
		/// </summary>
		/// <returns> the split matcher </returns>
		public static StrMatcher splitMatcher()
		{
			return SPLIT_MATCHER;
		}

		/// <summary>
		/// Matches the String trim() whitespace characters.
		/// </summary>
		/// <returns> the trim matcher </returns>
		public static StrMatcher trimMatcher()
		{
			return TRIM_MATCHER;
		}

		/// <summary>
		/// Returns a matcher which matches the single quote character.
		/// </summary>
		/// <returns> a matcher for a single quote </returns>
		public static StrMatcher singleQuoteMatcher()
		{
			return SINGLE_QUOTE_MATCHER;
		}

		/// <summary>
		/// Returns a matcher which matches the double quote character.
		/// </summary>
		/// <returns> a matcher for a double quote </returns>
		public static StrMatcher doubleQuoteMatcher()
		{
			return DOUBLE_QUOTE_MATCHER;
		}

		/// <summary>
		/// Returns a matcher which matches the single or double quote character.
		/// </summary>
		/// <returns> a matcher for a single or double quote </returns>
		public static StrMatcher quoteMatcher()
		{
			return QUOTE_MATCHER;
		}

		/// <summary>
		/// Matches no characters.
		/// </summary>
		/// <returns> a matcher that matches nothing </returns>
		public static StrMatcher noneMatcher()
		{
			return NONE_MATCHER;
		}

		/// <summary>
		/// Constructor that creates a matcher from a character.
		/// </summary>
		/// <param name="ch">  the character to match, must not be null </param>
		/// <returns> a new Matcher for the given char </returns>
		public static StrMatcher charMatcher(in char ch)
		{
			return new CharMatcher(ch);
		}

		/// <summary>
		/// Constructor that creates a matcher from a set of characters.
		/// </summary>
		/// <param name="chars">  the characters to match, null or empty matches nothing </param>
		/// <returns> a new matcher for the given char[] </returns>
		public static StrMatcher charSetMatcher(params char[] chars)
		{
			if (chars == null || chars.Length == 0)
			{
				return NONE_MATCHER;
			}
			if (chars.Length == 1)
			{
				return new CharMatcher(chars[0]);
			}
			return new CharSetMatcher(chars);
		}

		/// <summary>
		/// Constructor that creates a matcher from a string representing a set of characters.
		/// </summary>
		/// <param name="chars">  the characters to match, null or empty matches nothing </param>
		/// <returns> a new Matcher for the given characters </returns>
		public static StrMatcher charSetMatcher(in string chars)
		{
			if (String.IsNullOrEmpty(chars))
			{
				return NONE_MATCHER;
			}
			if (chars.Length == 1)
			{
				return new CharMatcher(chars[0]);
			}
			return new CharSetMatcher(chars.ToCharArray());
		}

		/// <summary>
		/// Constructor that creates a matcher from a string.
		/// </summary>
		/// <param name="str">  the string to match, null or empty matches nothing </param>
		/// <returns> a new Matcher for the given String </returns>
		public static StrMatcher stringMatcher(in string str)
		{
			if (String.IsNullOrEmpty(str))
			{
				return NONE_MATCHER;
			}
			return new StringMatcher(str);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Constructor.
		/// </summary>
		protected internal StrMatcher() : base()
		{
		}

		/// <summary>
		/// Returns the number of matching characters, zero for no match.
		/// <para>
		/// This method is called to check for a match.
		/// The parameter <code>pos</code> represents the current position to be
		/// checked in the string <code>buffer</code> (a character array which must
		/// not be changed).
		/// The API guarantees that <code>pos</code> is a valid index for <code>buffer</code>.
		/// </para>
		/// <para>
		/// The character array may be larger than the active area to be matched.
		/// Only values in the buffer between the specifed indices may be accessed.
		/// </para>
		/// <para>
		/// The matching code may check one character or many.
		/// It may check characters preceding <code>pos</code> as well as those
		/// after, so long as no checks exceed the bounds specified.
		/// </para>
		/// <para>
		/// It must return -1 for no match, or a positive number if a match was found.
		/// The number indicates the number of characters that matched.
		/// 0 means that a match was found but contains no character (for example, when matching
		/// the end of a character sequence without ending delimiter)
		/// 
		/// </para>
		/// </summary>
		/// <param name="buffer">  the text content to match against, do not change </param>
		/// <param name="pos">  the starting position for the match, valid for buffer </param>
		/// <param name="bufferStart">  the first active index in the buffer, valid for buffer </param>
		/// <param name="bufferEnd">  the end index (exclusive) of the active buffer, valid for buffer </param>
		/// <returns> the number of matching characters, -1 for no match </returns>
		public abstract int isMatch(in char[] buffer, in int pos, in int bufferStart, in int bufferEnd);

		/// <summary>
		/// Returns the number of matching characters, zero for no match.
		/// <para>
		/// This method is called to check for a match.
		/// The parameter <code>pos</code> represents the current position to be
		/// checked in the string <code>buffer</code> (a character array which must
		/// not be changed).
		/// The API guarantees that <code>pos</code> is a valid index for <code>buffer</code>.
		/// </para>
		/// <para>
		/// The matching code may check one character or many.
		/// It may check characters preceding <code>pos</code> as well as those after.
		/// </para>
		/// <para>
		/// It must return -1 for no match, or a positive number if a match was found.
		/// 0 means that a match was found but contains no character (for example, when matching
		/// the end of a character sequence without ending delimiter)
		/// 
		/// </para>
		/// </summary>
		/// <param name="buffer">  the text content to match against, do not change </param>
		/// <param name="pos">  the starting position for the match, valid for buffer </param>
		/// <returns> the number of matching characters, -1 for no match
		/// @since 2.4 </returns>
		public virtual int isMatch(in char[] buffer, in int pos)
		{
			return isMatch(buffer, pos, 0, buffer.Length);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Class used to define a set of characters for matching purposes.
		/// </summary>
		internal sealed class CharSetMatcher : StrMatcher
		{
			/// <summary>
			/// The set of characters to match. </summary>
			internal readonly char[] chars;

			/// <summary>
			/// Constructor that creates a matcher from a character array.
			/// </summary>
			/// <param name="chars">  the characters to match, must not be null </param>
			internal CharSetMatcher(in char[] chars) : base()
			{
				this.chars = (char[])chars.Clone();
				Array.Sort(this.chars);
			}

			/// <summary>
			/// Returns whether or not the given character matches.
			/// </summary>
			/// <param name="buffer">  the text content to match against, do not change </param>
			/// <param name="pos">  the starting position for the match, valid for buffer </param>
			/// <param name="bufferStart">  the first active index in the buffer, valid for buffer </param>
			/// <param name="bufferEnd">  the end index of the active buffer, valid for buffer </param>
			/// <returns> the number of matching characters, -1 for no match </returns>
			public override int isMatch(in char[] buffer, in int pos, in int bufferStart, in int bufferEnd)
			{
				return Array.BinarySearch(chars, buffer[pos]) >= 0 ? 1 : -1;
			}
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Class used to define a character for matching purposes.
		/// </summary>
		internal sealed class CharMatcher : StrMatcher
		{
			/// <summary>
			/// The character to match. </summary>
			internal readonly char ch;

			/// <summary>
			/// Constructor that creates a matcher that matches a single character.
			/// </summary>
			/// <param name="ch">  the character to match </param>
			internal CharMatcher(in char ch) : base()
			{
				this.ch = ch;
			}

			/// <summary>
			/// Returns whether or not the given character matches.
			/// </summary>
			/// <param name="buffer">  the text content to match against, do not change </param>
			/// <param name="pos">  the starting position for the match, valid for buffer </param>
			/// <param name="bufferStart">  the first active index in the buffer, valid for buffer </param>
			/// <param name="bufferEnd">  the end index of the active buffer, valid for buffer </param>
			/// <returns> the number of matching characters, -1 for no match </returns>
			public override int isMatch(in char[] buffer, in int pos, in int bufferStart, in int bufferEnd)
			{
				return ch == buffer[pos] ? 1 : -1;
			}
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Class used to define a set of characters for matching purposes.
		/// </summary>
		internal sealed class StringMatcher : StrMatcher
		{
			/// <summary>
			/// The string to match, as a character array. </summary>
			internal readonly char[] chars;

			/// <summary>
			/// Constructor that creates a matcher from a String.
			/// </summary>
			/// <param name="str">  the string to match, must not be null </param>
			internal StringMatcher(in string str) : base()
			{
				chars = str.ToCharArray();
			}

			/// <summary>
			/// Returns whether or not the given text matches the stored string.
			/// </summary>
			/// <param name="buffer">  the text content to match against, do not change </param>
			/// <param name="pos">  the starting position for the match, valid for buffer </param>
			/// <param name="bufferStart">  the first active index in the buffer, valid for buffer </param>
			/// <param name="bufferEnd">  the end index of the active buffer, valid for buffer </param>
			/// <returns> the number of matching characters, -1 for no match </returns>
			public override int isMatch(in char[] buffer, in int pos, in int bufferStart, in int bufferEnd)
			{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int len = chars.length;
				int len = chars.Length;
				int tmpPos = pos;
				if (tmpPos + len > bufferEnd)
				{
					return -1;
				}
				for (int i = 0; i < chars.Length; i++, tmpPos++)
				{
					if (chars[i] != buffer[tmpPos])
					{
						return -1;
					}
				}
				return len;
			}
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Class used to match no characters.
		/// </summary>
		internal sealed class NoMatcher : StrMatcher
		{
			/// <summary>
			/// Constructs a new instance of <code>NoMatcher</code>.
			/// </summary>
			internal NoMatcher() : base()
			{
			}

			/// <summary>
			/// Always returns <code>false</code>.
			/// </summary>
			/// <param name="buffer">  the text content to match against, do not change </param>
			/// <param name="pos">  the starting position for the match, valid for buffer </param>
			/// <param name="bufferStart">  the first active index in the buffer, valid for buffer </param>
			/// <param name="bufferEnd">  the end index of the active buffer, valid for buffer </param>
			/// <returns> the number of matching characters, -1 for no match </returns>
			public override int isMatch(in char[] buffer, in int pos, in int bufferStart, in int bufferEnd)
			{
				return -1;
			}
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Class used to match whitespace as per trim().
		/// </summary>
		internal sealed class TrimMatcher : StrMatcher
		{
			/// <summary>
			/// Constructs a new instance of <code>TrimMatcher</code>.
			/// </summary>
			internal TrimMatcher() : base()
			{
			}

			/// <summary>
			/// Returns whether or not the given character matches.
			/// </summary>
			/// <param name="buffer">  the text content to match against, do not change </param>
			/// <param name="pos">  the starting position for the match, valid for buffer </param>
			/// <param name="bufferStart">  the first active index in the buffer, valid for buffer </param>
			/// <param name="bufferEnd">  the end index of the active buffer, valid for buffer </param>
			/// <returns> the number of matching characters, -1 for no match </returns>
			public override int isMatch(in char[] buffer, in int pos, in int bufferStart, in int bufferEnd)
			{
				return buffer[pos] <= (char)32 ? 1 : -1;
			}
		}
	}

}