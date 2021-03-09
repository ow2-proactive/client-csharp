using System;
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
namespace org.ow2.proactive.utils
{



	/// <summary>
	/// Provides some methods used by the scheduler or the GUI to display some tips properly.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 0.9
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public class Tools
	public class Tools
	{

		/// <summary>
		/// Format the given integer 'toFormat' to a String containing 'nbChar'
		/// characters
		/// </summary>
		/// <param name="toFormat">
		///            the number to format </param>
		/// <param name="nbChar">
		///            the number of characters of the formatted result string </param>
		/// <returns> the given integer formatted as a 'nbChar' length String. </returns>
		public static string formatNChar(int toFormat, int nbChar, char replacement)
		{
			string formatted = toFormat + "";

			while (formatted.Length < nbChar)
			{
				formatted = replacement + formatted;
			}

			return formatted;
		}

		/// <summary>
		/// Format 2 long times into a single duration as a String. The string will
		/// contains the duration in Days, hours, minutes, seconds, and millis.
		/// </summary>
		/// <param name="start">
		///            the first date (time) </param>
		/// <param name="end">
		///            the second date (time) </param>
		/// <returns> the duration as a formatted string. </returns>
		public static string getFormattedDuration(long start, long end)
		{
			if ((start < 0) || (end < 0))
			{
				return "Not yet";
			}

			long duration = Math.Abs(start - end);
			string formatted = "";
			int tmp;
			// Millisecondes
			tmp = (int) duration % 1000;
			duration = duration / 1000;
			formatted = formatNChar(tmp, 3, ' ') + "ms" + formatted;
			// Secondes
			tmp = (int) duration % 60;
			duration = duration / 60;

			if (tmp > 0)
			{
				formatted = formatNChar(tmp, 2, ' ') + "s " + formatted;
			}

			// Minutes
			tmp = (int) duration % 60;
			duration = duration / 60;

			if (tmp > 0)
			{
				formatted = formatNChar(tmp, 2, ' ') + "m " + formatted;
			}

			// Hours
			tmp = (int) duration % 24;
			duration = duration / 24;

			if (tmp > 0)
			{
				formatted = formatNChar(tmp, 2, ' ') + "h " + formatted;
			}

			// Days
			tmp = (int) duration;

			if (tmp > 0)
			{
				formatted = tmp + " day" + ((tmp > 1) ? "s" : "") + " - " + formatted;
			}

			return formatted;
		}

		/// <summary>
		/// Return the given date as a formatted string.
		/// </summary>
		/// <param name="time"> the date as a long. </param>
		/// <returns> the given date as a formatted string. </returns>
		public static string getFormattedDate(long time)
		{
			if (time < 0)
			{
				return "Not yet";
			}

			DateTime sdf = new DateTime(time);		
			return sdf.ToString();
		}

		public static string getElapsedTime(long time)
		{
			long seconds = (DateTimeHelper.CurrentUnixTimeMillis() - time) / 1000;
			long day, hou, min, sec;
			StringBuilder ret = new StringBuilder();

			day = seconds / (3600 * 24);
			seconds -= day * (3600 * 24);
			hou = seconds / 3600;
			seconds -= hou * 3600;
			min = seconds / 60;
			sec = seconds % 60;

			if (day > 0)
			{
				ret.Append(day + "d ");
				ret.Append(hou + "h ");
			}
			else if (hou > 0)
			{
				ret.Append(hou + "h");
				ret.Append(min + "mn ");
			}
			else if (min > 0)
			{
				ret.Append(min + "mn ");
			}
			else
			{
				ret.Append(sec + "s ");
			}

			ret.Append("ago");

			return ret.ToString();
		}

		/// <summary>
		/// Format the given string and return a long that correspond
		/// to the time represented by the given string.
		/// If the string is not proper, {@code 0} will be returned.
		/// </summary>
		/// <param name="pattern"> a time pattern that must be in [[HH:]MM:]SS
		/// 			where HH, MM, and SS are numbers </param>
		/// <returns> a long corresponding to the given time. </returns>
		public static long formatDate(string pattern)
		{
			string[] splitted = pattern.Split(new char[] { ':' });
			int[] factor = new int[] {60 * 60 * 1000, 60 * 1000, 1000};
			if (splitted.Length < 0 || splitted.Length > 3)
			{
				return 0;
			}
			long date = 0;
			try
			{
				for (int i = splitted.Length - 1; i >= 0; i--)
				{
					date += int.Parse(splitted[i]) * factor[i + 3 - splitted.Length];
				}
				return date;
			}
			catch (System.FormatException)
			{
				return 0;
			}
		}

		/// <summary>
		/// Normalize the given URL into an URL that only contains protocol://host:port/
		/// </summary>
		/// <param name="url"> the url to transform </param>
		/// <returns> an URL that only contains protocol://host:port/ </returns>
		public static string getHostURL(string url)
		{
			Uri uri = new Uri(url);
			string scheme = (uri.Scheme == null) ? "rmi" : uri.Scheme;
			string host = (uri.Host == null) ? "localhost" : uri.Host;
			int port = uri.Port;
			if (port == -1)
			{
				return scheme + "://" + host + "/";
			}
			else
			{
				return scheme + "://" + host + ":" + port + "/";
			}
		}

		/// <summary>
		/// Parse a command line in order to split it into a string array.
		/// This method provides the parsing as followed :<br>
		///  - It is split according to the 'white space' character.<br>
		///  - It is possible to escape white space character using the '%' character.<br>
		///  - To write this '%' special char, just escape it ( '%%' ).<br>
		/// For example, the string "cmd arg1 arg% 2 arg%%% 3 arg4%% 5" will return the following string array :<br>
		///   [cmd,arg1,arg 2,arg% 3,arg4%,5]
		/// <para>
		/// This method can be mostlikely used for Runtime.exec(String[]) method.
		/// 
		/// </para>
		/// </summary>
		/// <param name="cmdLine"> The command line to parse. </param>
		/// <returns> a string array that represents the parsed command line. </returns>
		public static string[] parseCommandLine(string cmdLine)
		{
			const char specialToken = '%';
			List<string> tokens = new List<string>();
			int i = 0;
			StringBuilder tmp = new StringBuilder();
			char[] cs = cmdLine.ToCharArray();
			while (i < cs.Length)
			{
				switch (cs[i])
				{
					case specialToken:
						if (i + 1 < cs.Length)
						{
							tmp.Append(cs[i + 1]);
							i++;
						}
						break;
					case ' ':
						tokens.Add(tmp.ToString());
						tmp = new StringBuilder();
						break;
					default:
						tmp.Append(cs[i]);
					break;
				}
				i++;
			}
			if (tmp.Length > 0)
			{
				tokens.Add(tmp.ToString());
			}
			return tokens.ToArray();
		}

		/// <summary>
		/// Parses the given dataspace configuration property to an array of strings
		/// The parsing handles double quotes and space separators </summary>
		/// <param name="property"> dataspace configuration property </param>
		/// <returns> an array of string urls </returns>
		public static string[] dataSpaceConfigPropertyToUrls(string property)
		{
			if (property.Trim().Length == 0)
			{
				return new string[0];
			}
			if (property.Contains("\""))
			{
				// if the input contains quote, split it along space delimiters and quotes "A" "B" etc...
				// the pattern uses positive look-behind and look-ahead
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String[] outputWithQuotes = property.trim().split("(?<=\") +(?=\")");
				string[] outputWithQuotes = property.Trim().Split("(?<=\") +(?=\")", true);
				// removing quotes
				List<string> output = new List<string>();
				foreach (string outputWithQuote in outputWithQuotes)
				{
					int len = outputWithQuote.Length;
					if (outputWithQuote.Length > 2)
					{
						output.Add(outputWithQuote.Substring(1, (len - 1) - 1));
					}
				}
				return output.ToArray();
			}
			else
			{
				// if the input contains no quote, split it along space delimiters
				return property.Trim().Split(" +", true);
			}
		}

		/// <summary>
		/// Get the columned string according to the given ObjectArrayFormatter descriptor.
		/// </summary>
		/// <param name="oaf"> the ObjectArrayFormatter describing how to print the array. </param>
		/// <returns> the columned string according the given descriptor </returns>
		public static string getStringAsArray(ObjectArrayFormatter oaf)
		{
			return oaf.AsString;
		}

		/// <summary>
		/// Translates time period string into milliseconds. Period string should
		/// contain symbol indication length of the period: 
		/// <ul>
		/// <li>'s' - seconds
		/// <li>'m' - minutes
		/// <li>'h' - hours
		/// <li>'d' - days
		/// </ul> 
		/// 
		/// Examples of valid time period expressions:
		/// <ul>
		/// <li>'1m' - one minute
		/// <li>'1d 10h' - one day and ten hours
		/// </ul>
		/// </summary>
		public static long parsePeriod(string periodString)
		{
			periodString = periodString.Trim();
			if (periodString.Length == 0)
			{
				throw new System.ArgumentException("Period string is empty");
			}

			long total = 0;

			StringBuilder numberStr = new StringBuilder();
			for (int i = 0; i < periodString.Length; i++)
			{
				char ch = periodString[i];
				if (Char.IsWhiteSpace(ch))
				{
					continue;
				}
				else if (char.IsDigit(ch))
				{
					numberStr.Append(ch);
				}
				else
				{
					long millis;
					if (ch == 's')
					{
						millis = 1000;
					}
					else if (ch == 'm')
					{
						millis = 1000 * 60;
					}
					else if (ch == 'h')
					{
						millis = 1000 * 60 * 60;
					}
					else if (ch == 'd')
					{
						millis = 1000 * 60 * 60 * 24;
					}
					else
					{
						throw new System.ArgumentException("Invalid period string: " + ch);
					}
					if (numberStr.Length == 0)
					{
						throw new System.ArgumentException("Period length isn't specified");
					}

					total += Convert.ToInt64(numberStr.ToString()) * millis;
					numberStr = new StringBuilder();
				}
			}

			if (numberStr.Length != 0)
			{
				throw new System.ArgumentException("Period string isn't specified");
			}

			return total;
		}

	}

}