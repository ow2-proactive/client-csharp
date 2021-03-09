using System;
using System.Collections.Generic;
using System.Linq;
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
namespace org.ow2.proactive.scheduler.common.exception
{

	using XMLAttributes = org.ow2.proactive.scheduler.common.job.factories.XMLAttributes;
	using XMLTags = org.ow2.proactive.scheduler.common.job.factories.XMLTags;


	/// <summary>
	/// Exceptions Generated if a problem occurred while creating a job.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 0.9
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI public class JobCreationException extends SchedulerException
	public class JobCreationException : SchedulerException
	{

		private string taskName = null;

		private Stack<XMLTags> tags = new Stack<XMLTags>();

		private XMLAttributes attribute = null;

		private bool isSchemaException = false;

		private IDictionary<string, string> updatedVariables = null;

		private IDictionary<string, string> updatedModels = null;

		/// <summary>
		/// Create a new instance of JobCreationException using the given message string
		/// </summary>
		/// <param name="message"> the reason of the exception </param>
		public JobCreationException(string message) : base(message)
		{
		}

		/// <summary>
		/// Create a new instance of JobCreationException using the given message string
		/// </summary>
		/// <param name="cause"> the reason of the exception </param>
		public JobCreationException(Exception cause) : base(cause.Message, cause)
		{
		}

		/// <summary>
		/// Create a new instance of JobCreationException using the given message string and cause
		/// </summary>
		/// <param name="message"> the reason of the exception </param>
		/// <param name="cause"> the cause of this exception </param>
		public JobCreationException(string message, Exception cause) : base(message, cause)
		{
		}

		/// <summary>
		/// Create a new instance of JobCreationException and tell if it was a schema exception
		/// </summary>
		/// <param name="schemaException"> true if this exception is due to a Schema exception, false otherwise. </param>
		/// <param name="cause"> the cause of this exception </param>
		public JobCreationException(bool schemaException, Exception cause) : base(cause.Message, cause)
		{
			this.isSchemaException = schemaException;
		}

		/// <summary>
		/// Create a new instance of JobCreationException using the given tag, attribute and cause.
		/// Tag and attribute can be null.
		/// </summary>
		/// <param name="tag"> the XML tag name where the exception is thrown </param>
		/// <param name="attribute"> the XML attribute name where the exception is thrown </param>
		/// <param name="cause"> the cause of this exception </param>
		public JobCreationException(string tag, string attribute, Exception cause) : this(string.ReferenceEquals(tag, null) ? null : XMLTags.getFromXMLName(tag), attribute, cause)
		{
		}

		/// <summary>
		/// Create a new instance of JobCreationException using the given tag, attribute and cause.
		/// Tag and attribute can be null.
		/// </summary>
		/// <param name="tag"> the XML tag where the exception is thrown </param>
		/// <param name="attribute"> the XML attribute name where the exception is thrown </param>
		/// <param name="cause"> the cause of this exception </param>
		public JobCreationException(XMLTags tag, string attribute, Exception cause) : base(cause.Message, cause)
		{
			if (tag != null)
			{
				this.tags.Push(tag);
			}
			if (!string.ReferenceEquals(attribute, null))
			{
				this.attribute = XMLAttributes.getFromXMLName(attribute);
			}
		}

		/// <summary>
		/// Set the task name on which the problem was found
		/// </summary>
		/// <param name="taskName"> the name of the task that generate the problem </param>
		public virtual string TaskName
		{
			set
			{
				this.taskName = value;
			}
			get
			{
				return taskName;
			}
		}

		/// <summary>
		/// Push a new tag on the stack of tag for this exception
		/// </summary>
		/// <param name="currentTag"> the tag name to stack. </param>
		public virtual void pushTag(string currentTag)
		{
			this.pushTag(XMLTags.getFromXMLName(currentTag));
		}

		/// <summary>
		/// Push a new tag on the stack of tag for this exception
		/// </summary>
		/// <param name="currentTag"> the tag to stack. </param>
		public virtual void pushTag(XMLTags currentTag)
		{
			this.tags.Push(currentTag);
		}

		/// <summary>
		/// Return a stack that contains every tags (path) to the element that causes the exception.
		/// The first element is always the 'job' tag.
		/// </summary>
		/// <returns> a stack that contains every tags (path) to the element that causes the exception. </returns>
		public virtual Stack<XMLTags> XMLTagsStack
		{
			get
			{
				return this.tags;
			}
		}

		/// <summary>
		/// Return the detail message string of this exception.
		/// This message contains the task name, tag hierarchy, and attribute that generate the exception and
		/// then the short message associate to this exception.
		/// </summary>
		/// <returns> the detail message string of this exception. </returns>
		public override string Message
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				if (!string.ReferenceEquals(taskName, null))
				{
					sb.Append("[task=" + taskName + "] ");
				}
				if (tags != null && tags.Count > 0)
				{
	//JAVA TO C# CONVERTER TODO TASK: There is no direct .NET Stack equivalent to Java Stack methods based on internal indexing:
					sb.Append("[tag:" + tags.ElementAt(tags.Count - 1));
					for (int i = tags.Count - 2; i >= 0; i--)
					{
	//JAVA TO C# CONVERTER TODO TASK: There is no direct .NET Stack equivalent to Java Stack methods based on internal indexing:
						sb.Append("/" + tags.ElementAt(i));
					}
					sb.Append("] ");
				}
				if (attribute != null)
				{
					sb.Append("[attribute:" + attribute + "] ");
				}
				if (sb.Length > 0)
				{
					sb = new StringBuilder("At " + sb.ToString() + ": ");
				}
				sb.Append(ShortMessage);
				return sb.ToString();
			}
		}

		/// <summary>
		/// Return only the message that is the cause of the exception.
		/// </summary>
		/// <returns> the message that is the cause of the exception. </returns>
		public virtual string ShortMessage
		{
			get
			{
				return base.Message;
			}
		}

		/// <summary>
		/// Get the isSchemaException
		/// </summary>
		/// <returns> the isSchemaException </returns>
		public virtual bool SchemaException
		{
			get
			{
				return isSchemaException;
			}
		}


		/// <summary>
		/// Get the attribute where the exception has been thrown.
		/// </summary>
		/// <returns> the attribute where the exception has been thrown, null if no attribute was set (ie: exception
		/// 			was thrown in a tag that does not contains an attribute.) </returns>
		public virtual XMLAttributes Attribute
		{
			get
			{
				return attribute;
			}
		}

		public virtual IDictionary<string, string> UpdatedVariables
		{
			get
			{
				return updatedVariables;
			}
			set
			{
				this.updatedVariables = value;
			}
		}


		public virtual IDictionary<string, string> UpdatedModels
		{
			get
			{
				return updatedModels;
			}
			set
			{
				this.updatedModels = value;
			}
		}

	}

}