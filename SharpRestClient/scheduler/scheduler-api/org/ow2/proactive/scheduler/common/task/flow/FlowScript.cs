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
namespace org.ow2.proactive.scheduler.common.task.flow
{

	using Task = org.ow2.proactive.scheduler.common.task.Task;
	using InvalidScriptException = org.ow2.proactive.scripting.InvalidScriptException;
	using Script = org.ow2.proactive.scripting.Script<object>;
	using ScriptResult = org.ow2.proactive.scripting.ScriptResult<object>;
	using SimpleScript = org.ow2.proactive.scripting.SimpleScript;


	/// <summary>
	/// Dynamic evaluation of this script determines at runtime if a specific 
	/// Control Flow operation should be performed in a TaskFlow.
	/// <para>
	/// This class wraps information around a <seealso cref="org.ow2.proactive.scripting.Script"/>
	/// to determine which <seealso cref="FlowAction"/> is attached to this script.
	/// </para>
	/// <para>
	/// When using the action type <seealso cref="FlowActionType.REPLICATE"/>, the value of the 
	/// <seealso cref="FlowScript.replicateRunsVariable"/> determines the number of parallel runs.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 2.2
	/// </para>
	/// </summary>
	/// <seealso cref= FlowAction </seealso>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI @XmlAccessorType(XmlAccessType.FIELD) public class FlowScript extends org.ow2.proactive.scripting.Script<FlowAction>
	public class FlowScript : Script
	{

		/// <summary>
		/// String representation of a FlowActionType
		/// see <seealso cref="FlowActionType.parse(string)"/> 
		/// </summary>
		private string actionType = null;

		// implementation note:
		// target / targetElse / targetContinuation
		// would be much better represented with an InternalTask or a TaskId, but :
		// - InternalTask cannot be used because of project setup :
		//   it is not exported on the worker on which this script executes;
		//   it cannot be used on user side, and this class is PublicAPI
		// - TaskId cannot be used by the user prior to submission
		// A complete solution would involve exposing one class to the user,
		// copying the info onto another more complete InternalFlowScript,
		// and holding a TaskId, which would present some of the problems
		// of the current string-based implementation
		//
		// In the end, using strings is less safe, but faster and simpler ;
		// also, this is only internals never exposed to the user

		/// <summary>
		/// Name of the target task of this action if it requires one </summary>
		private string target = null;

		/// <summary>
		/// Name of the 'Else' target task if this action is an 'If' </summary>
		private string targetElse = null;

		/// <summary>
		/// Name of the 'Continuation' target task if this action is an 'If' </summary>
		private string targetContinuation = null;

		/// <summary>
		/// Name of the boolean variable to set in the script to determine 
		/// if a LOOP action is enabled or if the execution should continue 
		/// </summary>
		public const string loopVariable = "loop";

		/// <summary>
		/// Name of the Integer variable to set in the script to determine
		/// the number of parallel runs of a REPLICATE action 
		/// </summary>
		public const string replicateRunsVariable = "runs";

		/// <summary>
		/// Name of the variable to set in the script to determine
		/// which one of the IF or ELSE branch is selected in an IF
		/// control flow action 
		/// </summary>
		public const string branchSelectionVariable = "branch";

		/// <summary>
		/// Value to set <seealso cref="branchSelectionVariable"/> to
		/// signify the IF branch should be selected 
		/// </summary>
		public const string ifBranchSelectedVariable = "if";

		/// <summary>
		/// Value to set <seealso cref="branchSelectionVariable"/> to
		/// signify the ELSE branch should be selected 
		/// </summary>
		public const string elseBranchSelectedVariable = "else";

		/// <summary>
		/// Hibernate default constructor,
		/// use <seealso cref="createContinueFlowScript()"/>,
		/// <seealso cref="createLoopFlowScript(Script, string)"/> or
		/// <seealso cref="createReplicateFlowScript(Script)"/> to
		/// create a FlowScript
		/// </summary>
		public FlowScript()
		{
		}

		protected internal override string DefaultScriptName
		{
			get
			{
				return "FlowScript";
			}
		}

		/// <summary>
		/// Copy constructor 
		/// </summary>
		/// <param name="fl"> Source script </param>
		/// <exception cref="InvalidScriptException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public FlowScript(FlowScript fl) throws org.ow2.proactive.scripting.InvalidScriptException
		public FlowScript(FlowScript fl) : base(fl)
		{
			if (!string.ReferenceEquals(fl.ActionType, null))
			{
				this.actionType = fl.ActionType;
			}
			if (!string.ReferenceEquals(fl.ActionTarget, null))
			{
				this.target = fl.ActionTarget;
			}
			if (!string.ReferenceEquals(fl.ActionTargetElse, null))
			{
				this.targetElse = fl.ActionTargetElse;
			}
			if (!string.ReferenceEquals(fl.ActionContinuation, null))
			{
				this.targetContinuation = fl.ActionContinuation;
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private FlowScript(org.ow2.proactive.scripting.Script<?> scr) throws org.ow2.proactive.scripting.InvalidScriptException
//JAVA TO C# CONVERTER TODO TASK: Wildcard generics in constructor parameters are not converted. Move the generic type parameter and constraint to the class header:
		private FlowScript(Script scr) : base(scr)
		{
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static FlowScript createContinueFlowScript() throws org.ow2.proactive.scripting.InvalidScriptException
		public static FlowScript createContinueFlowScript()
		{
			FlowScript fs = new FlowScript(new SimpleScript("", "javascript"));
			fs.setActionType(FlowActionType.CONTINUE);
			return fs;
		}

		/// <summary>
		/// Creates a Control Flow Script configured to perform a LOOP control flow action
		/// the code will be run using a javascript engine
		/// </summary>
		/// <param name="script"> code of the Javascript script </param>
		/// <param name="target"> target of the LOOP action </param>
		/// <returns> a newly allocated and configured Control Flow Script </returns>
		/// <exception cref="InvalidScriptException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static FlowScript createLoopFlowScript(String script, String target) throws org.ow2.proactive.scripting.InvalidScriptException
		public static FlowScript createLoopFlowScript(string script, string target)
		{
			return createLoopFlowScript(script, "javascript", target);
		}

		/// <summary>
		/// Creates a Control Flow Script configured to perform a LOOP control flow action
		/// </summary>
		/// <param name="script"> code of the script </param>
		/// <param name="engine"> engine running the script </param>
		/// <param name="target"> target of the LOOP action </param>
		/// <returns> a newly allocated and configured Control Flow Script </returns>
		/// <exception cref="InvalidScriptException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static FlowScript createLoopFlowScript(String script, String engine, String target) throws org.ow2.proactive.scripting.InvalidScriptException
		public static FlowScript createLoopFlowScript(string script, string engine, string target)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: org.ow2.proactive.scripting.Script<?> scr = new org.ow2.proactive.scripting.SimpleScript(script, engine);
			Script scr = new SimpleScript(script, engine);
			return createLoopFlowScript(scr, target);
		}

		/// <summary>
		/// Creates a Control Flow Script configured to perform a LOOP control flow action
		/// </summary>
		/// <param name="script"> code of the script </param>
		/// <param name="engine"> engine running the script </param>
		/// <param name="target"> target of the LOOP action </param>
		/// <param name="parameters"> execution parameters </param>
		/// <returns> a newly allocated and configured Control Flow Script </returns>
		/// <exception cref="InvalidScriptException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static FlowScript createLoopFlowScript(String script, String engine, String target, java.io.Serializable[] parameters) throws org.ow2.proactive.scripting.InvalidScriptException
		public static FlowScript createLoopFlowScript(string script, string engine, string target, string[] parameters)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: org.ow2.proactive.scripting.Script<?> scr = new org.ow2.proactive.scripting.SimpleScript(script, engine, parameters);
			Script scr = new SimpleScript(script, engine, parameters);
			return createLoopFlowScript(scr, target);
		}

		/// <summary>
		/// Creates a Control Flow Script configured to perform a LOOP control flow action
		/// </summary>
		/// <param name="url"> url of the script </param>
		/// <param name="engine"> engine running the script </param>
		/// <param name="target"> target of the LOOP action </param>
		/// <param name="parameters"> execution parameters </param>
		/// <returns> a newly allocated and configured Control Flow Script </returns>
		/// <exception cref="InvalidScriptException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static FlowScript createLoopFlowScript(java.net.URL url, String engine, String target, java.io.Serializable[] parameters) throws org.ow2.proactive.scripting.InvalidScriptException
		public static FlowScript createLoopFlowScript(Uri url, string engine, string target, string[] parameters)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: org.ow2.proactive.scripting.Script<?> scr = new org.ow2.proactive.scripting.SimpleScript(url, engine, parameters);
			Script scr = new SimpleScript(url, engine, parameters);
			return createLoopFlowScript(scr, target);
		}

		/// <summary>
		/// Creates a Control Flow Script configured to perform a LOOP control flow action
		/// </summary>
		/// <param name="script"> the script to execute </param>
		/// <param name="target"> target of the LOOP action </param>
		/// <returns> a newly allocated and configured Control Flow Script </returns>
		/// <exception cref="InvalidScriptException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static FlowScript createLoopFlowScript(org.ow2.proactive.scripting.Script<?> script, String target) throws org.ow2.proactive.scripting.InvalidScriptException
		public static FlowScript createLoopFlowScript(Script script, string target)
		{
			FlowScript flow = new FlowScript(script);
			flow.setActionType(FlowActionType.LOOP);
			flow.setActionTarget(target);
			return flow;
		}

		/// <summary>
		/// Creates a Control Flow Script configured to perform an IF control flow action
		/// the code will be run using a javascript engine
		/// </summary>
		/// <param name="script"> code of the Javascript script </param>
		/// <param name="targetIf"> IF branch </param>
		/// <param name="targetElse"> ELSE branch </param>
		/// <param name="targetCont"> CONTINUATION branch, can be null </param>
		/// <returns> a newly allocated and configured Control Flow Script </returns>
		/// <exception cref="InvalidScriptException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static FlowScript createIfFlowScript(String script, String targetIf, String targetElse, String targetCont) throws org.ow2.proactive.scripting.InvalidScriptException
		public static FlowScript createIfFlowScript(string script, string targetIf, string targetElse, string targetCont)
		{
			return createIfFlowScript(script, "javascript", targetIf, targetElse, targetCont);
		}

		/// <summary>
		/// Creates a Control Flow Script configured to perform an IF control flow action
		/// </summary>
		/// <param name="script"> code of the script </param>
		/// <param name="engine"> engine running the script </param>
		/// <param name="targetIf"> IF branch </param>
		/// <param name="targetElse"> ELSE branch </param>
		/// <param name="targetCont"> CONTINUATION branch, can be null </param>
		/// <returns> a newly allocated and configured Control Flow Script </returns>
		/// <exception cref="InvalidScriptException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static FlowScript createIfFlowScript(String script, String engine, String targetIf, String targetElse, String targetCont) throws org.ow2.proactive.scripting.InvalidScriptException
		public static FlowScript createIfFlowScript(string script, string engine, string targetIf, string targetElse, string targetCont)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: org.ow2.proactive.scripting.Script<?> scr = new org.ow2.proactive.scripting.SimpleScript(script, engine);
			Script scr = new SimpleScript(script, engine);
			return createIfFlowScript(scr, targetIf, targetElse, targetCont);
		}

		/// <summary>
		/// Creates a Control Flow Script configured to perform an IF control flow action
		/// </summary>
		/// <param name="script"> code of the script </param>
		/// <param name="engine"> engine running the script </param>
		/// <param name="targetIf"> IF branch </param>
		/// <param name="targetElse"> ELSE branch </param>
		/// <param name="targetCont"> CONTINUATION branch, can be null </param>
		/// <param name="parameters"> execution parameters </param>
		/// <returns> a newly allocated and configured Control Flow Script </returns>
		/// <exception cref="InvalidScriptException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static FlowScript createIfFlowScript(String script, String engine, String targetIf, String targetElse, String targetCont, java.io.Serializable[] parameters) throws org.ow2.proactive.scripting.InvalidScriptException
		public static FlowScript createIfFlowScript(string script, string engine, string targetIf, string targetElse, string targetCont, string[] parameters)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: org.ow2.proactive.scripting.Script<?> scr = new org.ow2.proactive.scripting.SimpleScript(script, engine, parameters);
			Script scr = new SimpleScript(script, engine, parameters);
			return createIfFlowScript(scr, targetIf, targetElse, targetCont);
		}

		/// <summary>
		/// Creates a Control Flow Script configured to perform an IF control flow action
		/// </summary>
		/// <param name="url"> url of the script </param>
		/// <param name="engine"> engine running the script </param>
		/// <param name="targetIf"> IF branch </param>
		/// <param name="targetElse"> ELSE branch </param>
		/// <param name="targetCont"> CONTINUATION branch, can be null </param>
		/// <param name="parameters"> execution parameters </param>
		/// <returns> a newly allocated and configured Control Flow Script </returns>
		/// <exception cref="InvalidScriptException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static FlowScript createIfFlowScript(java.net.URL url, String engine, String targetIf, String targetElse, String targetCont, java.io.Serializable[] parameters) throws org.ow2.proactive.scripting.InvalidScriptException
		public static FlowScript createIfFlowScript(Uri url, string engine, string targetIf, string targetElse, string targetCont, string[] parameters)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: org.ow2.proactive.scripting.Script<?> scr = new org.ow2.proactive.scripting.SimpleScript(url, engine, parameters);
			Script scr = new SimpleScript(url, engine, parameters);
			return createIfFlowScript(scr, targetIf, targetElse, targetCont);
		}

		/// <summary>
		/// Creates a Control Flow Script configured to perform an IF control flow action
		/// </summary>
		/// <param name="script"> the script to execute </param>
		/// <param name="targetIf"> IF branch </param>
		/// <param name="targetElse"> ELSE branch </param>
		/// <param name="targetCont"> CONTINUATION branch, can be null </param>
		/// <returns> a newly allocated and configured Control Flow Script </returns>
		/// <exception cref="InvalidScriptException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static FlowScript createIfFlowScript(org.ow2.proactive.scripting.Script<?> script, String targetIf, String targetElse, String targetCont) throws org.ow2.proactive.scripting.InvalidScriptException
		public static FlowScript createIfFlowScript(Script script, string targetIf, string targetElse, string targetCont)
		{
			FlowScript flow = new FlowScript(script);
			flow.setActionType(FlowActionType.IF);
			flow.setActionTarget(targetIf);
			flow.setActionTargetElse(targetElse);
			flow.setActionContinuation(targetCont);
			return flow;
		}

		/// <summary>
		/// Creates a Control Flow Script configured to perform a REPLICATE control flow action
		/// the code will be run using a javascript engine
		/// </summary>
		/// <param name="script"> code of the Javascript script </param>
		/// <returns> a newly allocated and configured Control Flow Script </returns>
		/// <exception cref="InvalidScriptException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static FlowScript createReplicateFlowScript(String script) throws org.ow2.proactive.scripting.InvalidScriptException
		public static FlowScript createReplicateFlowScript(string script)
		{
			return createReplicateFlowScript(script, "javascript");
		}

		/// <summary>
		/// Creates a Control Flow Script configured to perform a Replicate control flow action
		/// </summary>
		/// <param name="script"> code of the script </param>
		/// <param name="engine"> engine running the script </param>
		/// <returns> a newly allocated and configured Control Flow Script </returns>
		/// <exception cref="InvalidScriptException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static FlowScript createReplicateFlowScript(String script, String engine) throws org.ow2.proactive.scripting.InvalidScriptException
		public static FlowScript createReplicateFlowScript(string script, string engine)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: org.ow2.proactive.scripting.Script<?> scr = new org.ow2.proactive.scripting.SimpleScript(script, engine);
			Script scr = new SimpleScript(script, engine);
			return createReplicateFlowScript(scr);
		}

		/// <summary>
		/// Creates a Control Flow Script configured to perform a Replicate control flow action
		/// </summary>
		/// <param name="script"> code of the script </param>
		/// <param name="engine"> engine running the script </param>
		/// <param name="parameters"> execution parameters </param>
		/// <returns> a newly allocated and configured Control Flow Script </returns>
		/// <exception cref="InvalidScriptException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static FlowScript createReplicateFlowScript(String script, String engine, java.io.Serializable[] parameters) throws org.ow2.proactive.scripting.InvalidScriptException
		public static FlowScript createReplicateFlowScript(string script, string engine, string[] parameters)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: org.ow2.proactive.scripting.Script<?> scr = new org.ow2.proactive.scripting.SimpleScript(script, engine, parameters);
			Script scr = new SimpleScript(script, engine, parameters);
			return createReplicateFlowScript(scr);
		}

		/// <summary>
		/// Creates a Control Flow Script configured to perform a Replicate control flow action
		/// </summary>
		/// <param name="url"> url of the script </param>
		/// <param name="engine"> engine running the script </param>
		/// <param name="parameters"> execution parameters </param>
		/// <returns> a newly allocated and configured Control Flow Script </returns>
		/// <exception cref="InvalidScriptException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static FlowScript createReplicateFlowScript(java.net.URL url, String engine, java.io.Serializable[] parameters) throws org.ow2.proactive.scripting.InvalidScriptException
		public static FlowScript createReplicateFlowScript(Uri url, string engine, string[] parameters)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: org.ow2.proactive.scripting.Script<?> scr = new org.ow2.proactive.scripting.SimpleScript(url, engine, parameters);
			Script scr = new SimpleScript(url, engine, parameters);
			return createReplicateFlowScript(scr);
		}

		/// <summary>
		/// Creates a Control Flow Script configured to perform a REPLICATE control flow action
		/// </summary>
		/// <param name="script"> the script to execute </param>
		/// <returns> a newly allocated and configured Control Flow Script </returns>
		/// <exception cref="InvalidScriptException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static FlowScript createReplicateFlowScript(org.ow2.proactive.scripting.Script<?> script) throws org.ow2.proactive.scripting.InvalidScriptException
		public static FlowScript createReplicateFlowScript(Script script)
		{
			FlowScript flow = new FlowScript(script);
			flow.setActionType(FlowActionType.REPLICATE);
			return flow;
		}

		/// <summary>
		/// The Action Type does not have any effect on the execution of the script,
		/// but will be used after the execution to determine what Control Flow Action
		/// should be performed on the TaskFlow.
		/// </summary>
		/// <param name="actionType"> the String representation of the new ActionType of this script, </param>
		/// <seealso cref= FlowActionType#parse(String) </seealso>
		public virtual void setActionType(string actionType)
		{
			this.actionType = actionType;
		}

		/// <summary>
		/// The Action Type does not have any effect on the execution of the script,
		/// but will be used after the execution to determine what Control Flow Action
		/// should be performed on the TaskFlow.
		/// </summary>
		/// <param name="type"> the ActionType of this script, </param>
		public virtual void setActionType(FlowActionType type)
		{
			this.actionType = type.ToString();
		}

		/// <summary>
		/// The Action Type does not have any effect on the execution of the script,
		/// but will be used after the execution to determine what Control Flow Action
		/// should be performed on the TaskFlow.
		/// </summary>
		/// <returns> the String representation of the ActionType of this script, </returns>
		/// <seealso cref= FlowActionType#parse(String) </seealso>
		public virtual string ActionType
		{
			get
			{
				return this.actionType;
			}
		}

		/// <summary>
		/// If the Action type (see <seealso cref="getActionType()"/>) of this FlowScript 
		/// is <seealso cref="FlowActionType.LOOP"/>, the target is the entry point of the next loop iteration.
		/// If the Action type is <seealso cref="FlowActionType.IF"/>, the target is the branch executed when 
		/// the If condition succeeds.
		/// <para>
		/// This value has no effect on the execution of the script
		/// 
		/// </para>
		/// </summary>
		/// <param name="target"> the main target of the action of this script. </param>
		public virtual void setActionTarget(string target)
		{
			this.target = target;
		}

		/// <summary>
		/// If the Action type (see <seealso cref="getActionType()"/>) of this FlowScript 
		/// is <seealso cref="FlowActionType.LOOP"/>, the target is the entry point of the next loop iteration.
		/// If the Action type is <seealso cref="FlowActionType.IF"/>, the target is the branch executed when 
		/// the If condition succeeds.
		/// <para>
		/// This value has no effect on the execution of the script
		/// 
		/// </para>
		/// </summary>
		/// <param name="target"> the main target of the action of this script. </param>
		public virtual void setActionTarget(Task target)
		{
			this.target = target.Name;
		}

		/// <summary>
		/// If the Action type (see <seealso cref="getActionType()"/>) of this FlowScript 
		/// is <seealso cref="FlowActionType.LOOP"/>, the target is the entry point of the next loop iteration.
		/// If the Action type is <seealso cref="FlowActionType.IF"/>, the target is the branch executed when 
		/// the If condition succeeds.
		/// <para>
		/// This value has no effect on the execution of the script
		/// 
		/// </para>
		/// </summary>
		/// <returns> the main target of the action of this script </returns>
		public virtual string ActionTarget
		{
			get
			{
				return this.target;
			}
		}

		/// <summary>
		/// If the Action type (see <seealso cref="getActionType()"/>) of this FlowScript 
		/// is <seealso cref="FlowActionType.IF"/>, the targetElse is the branch executed when 
		/// the If condition fails.
		/// <para>
		/// This value has no effect on the execution of the script
		/// 
		/// </para>
		/// </summary>
		/// <param name="target"> the Else target of the action of this script </param>
		public virtual void setActionTargetElse(string target)
		{
			this.targetElse = target;
		}

		/// <summary>
		/// If the Action type (see <seealso cref="getActionType()"/>) of this FlowScript 
		/// is <seealso cref="FlowActionType.IF"/>, the targetElse is the branch executed when 
		/// the If condition fails.
		/// <para>
		/// This value has no effect on the execution of the script
		/// 
		/// </para>
		/// </summary>
		/// <param name="target"> the Else target of the action of this script </param>
		public virtual void setActionTargetElse(Task target)
		{
			this.targetElse = target.Name;
		}

		/// <summary>
		/// If the Action type (see <seealso cref="getActionType()"/>) of this FlowScript 
		/// is <seealso cref="FlowActionType.IF"/>, the targetElse is the branch executed when 
		/// the If condition fails.
		/// <para>
		/// This value has no effect on the execution of the script
		/// 
		/// </para>
		/// </summary>
		/// <returns> the Else target of the action of this script </returns>
		public virtual string ActionTargetElse
		{
			get
			{
				return this.targetElse;
			}
		}

		/// <summary>
		/// If the Action type (see <seealso cref="getActionType()"/>) of this FlowScript 
		/// is <seealso cref="FlowActionType.IF"/>, the targetContinuation is the Task on which both
		/// if and else branches will join after either one has been executed.
		/// <para>
		/// This value has no effect on the execution of the script
		/// 
		/// </para>
		/// </summary>
		/// <param name="target"> the Continuation target of the action of this script </param>
		public virtual void setActionContinuation(string target)
		{
			this.targetContinuation = target;
		}

		/// <summary>
		/// If the Action type (see <seealso cref="getActionType()"/>) of this FlowScript 
		/// is <seealso cref="FlowActionType.IF"/>, the targetContinuation is the Task on which both
		/// if and else branches will join after either one has been executed.
		/// <para>
		/// This value has no effect on the execution of the script
		/// 
		/// </para>
		/// </summary>
		/// <param name="target"> the Continuation target of the action of this script </param>
		public virtual void setActionContinuation(Task target)
		{
			this.targetContinuation = target.Name;
		}

		/// <summary>
		/// If the Action type (see <seealso cref="getActionType()"/>) of this FlowScript 
		/// is <seealso cref="FlowActionType.IF"/>, the targetContinuation is the Task on which both
		/// if and else branches will join after either one has been executed.
		/// <para>
		/// This value has no effect on the execution of the script
		/// 
		/// </para>
		/// </summary>
		/// <returns> the Continuation target of the action of this script </returns>
		public virtual string ActionContinuation
		{
			get
			{
				return this.targetContinuation;
			}
		}

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