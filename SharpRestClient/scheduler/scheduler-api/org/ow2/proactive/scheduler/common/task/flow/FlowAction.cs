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
namespace org.ow2.proactive.scheduler.common.task.flow
{



	/// <summary>
	/// Describes a Control Flow Action that enables workflow operations in TaskFlow jobs.
	/// <para>
	/// Different types are described in <seealso cref="FlowActionType"/>
	/// 
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 2.2
	/// </para>
	/// </summary>
	/// <seealso cref= FlowActionType
	///  </seealso>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAccessorType(XmlAccessType.FIELD) public class FlowAction implements java.io.Serializable
	[Serializable]
	public class FlowAction
	{

		/// <summary>
		/// Type of the Action stored as a String for convenience,
		/// see <seealso cref="FlowActionType.parse(string)"/> 
		/// </summary>
		private string type;

		/// <summary>
		/// Number of parallel runs if <seealso cref="type"/> is <seealso cref="FlowActionType.REPLICATE"/> </summary>
		private int dupNumber = 1;

		/// <summary>
		/// Main action target if  <seealso cref="type"/> is <seealso cref="FlowActionType.LOOP"/>
		/// or selected branch if <seealso cref="type"/> <seealso cref="FlowActionType.IF"/> 
		/// </summary>
		private string target = "";

		/// <summary>
		/// Continuation task for If and Else branches if  <seealso cref="type"/> is <seealso cref="FlowActionType.IF"/> </summary>
		private string targetContinuation = "";

		/// <summary>
		/// Branch that was NOT chosen if  <seealso cref="type"/> is <seealso cref="FlowActionType.IF"/> </summary>
		private string targetElse = "";

		private string cronExpr = "";

		/// <summary>
		/// Default constructor
		/// </summary>
		public FlowAction()
		{
			this.type = FlowActionType.CONTINUE.ToString();
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="type"> the default type </param>
		public FlowAction(FlowActionType type)
		{
			this.type = type.ToString();
		}

		/// <summary>
		/// If a FlowScript cannot be performed and the execution of the
		/// job must continue, a default 'neutral' action must be provided
		/// <para>
		/// 
		/// </para>
		/// </summary>
		/// <param name="script"> the FlowScript defining the action type and parameters </param>
		/// <returns> the neutral FlowAction mathcing the provided parameters </returns>
		public static FlowAction getDefaultAction(FlowScript script)
		{
			FlowAction ret = null;

			FlowActionType type = FlowActionType.parse(script.ActionType);
			if (type == FlowActionType.REPLICATE) { 
				
					// this is equivalent to REPLICATE with runs==1
					ret = new FlowAction(FlowActionType.CONTINUE);
				
			} 
			else if (type == FlowActionType.LOOP)
			{
				ret = new FlowAction(FlowActionType.CONTINUE);
			}
			else if (type == FlowActionType.IF)
			{
				// if we CONTINUE here the flow will be blocked indefinitely
				// we perform the IF action as if the first target was selected
				ret = new FlowAction(FlowActionType.IF);
				ret.Target = script.ActionTarget;
				ret.TargetElse = script.ActionTargetElse;
				ret.TargetContinuation = script.ActionContinuation;
			}
			else if (type == FlowActionType.CONTINUE)
			{
				ret = new FlowAction(FlowActionType.CONTINUE);
			}			

			return ret;
		}

		/// <returns> the type of this FlowAction </returns>
		public virtual FlowActionType Type
		{
			get
			{
				return FlowActionType.parse(this.type);
			}
			set
			{
				if (value != null)
				{
					this.type = value.ToString();
				}
			}
		}

		/// <returns> the number of parallel runs if the type of
		/// this action is <seealso cref="FlowActionType.REPLICATE"/> </returns>
		public virtual int DupNumber
		{
			get
			{
				return this.dupNumber;
			}
			set
			{
				this.dupNumber = value;
			}
		}

		/// <returns> the main action target if
		/// this action is <seealso cref="FlowActionType.REPLICATE"/> or
		/// <seealso cref="FlowActionType.IF"/> </returns>
		public virtual string Target
		{
			get
			{
				return this.target;
			}
			set
			{
				if (!string.ReferenceEquals(value, null))
				{
					this.target = value;
				}
			}
		}




		/// <returns> the name of the task that was NOT selected if
		///  this action is <seealso cref="FlowActionType.IF"/> </returns>
		public virtual string TargetElse
		{
			get
			{
				return this.targetElse;
			}
			set
			{
				if (!string.ReferenceEquals(value, null))
				{
					this.targetElse = value;
				}
			}
		}


		/// <param name="t"> the Continuation task for the If and Else branches,
		///  if this action is <seealso cref="FlowActionType.IF"/> </param>
		public virtual string TargetContinuation
		{
			set
			{
				if (!string.ReferenceEquals(value, null))
				{
					this.targetContinuation = value;
				}
			}
			get
			{
				return this.targetContinuation;
			}
		}


		public virtual string CronExpr
		{
			set
			{
				this.cronExpr = value;
			}
			get
			{
				return cronExpr;
			}
		}

	}

}