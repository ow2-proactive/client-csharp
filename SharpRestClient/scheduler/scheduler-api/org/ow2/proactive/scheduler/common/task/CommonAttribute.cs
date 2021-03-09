using System;
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
namespace org.ow2.proactive.scheduler.common.task
{


	using IntegerWrapper = org.ow2.proactive.scheduler.common.task.util.IntegerWrapper;
	using LongWrapper = org.ow2.proactive.scheduler.common.task.util.LongWrapper;
	using VariableSubstitutor = org.ow2.proactive.scheduler.common.util.VariableSubstitutor;


	/// <summary>
	/// Definition of the common attributes between job and task.
	/// 
	/// @author The ProActive Team
	/// @since ProActive Scheduling 0.9.1
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PublicAPI @XmlAccessorType(XmlAccessType.FIELD) public abstract class CommonAttribute implements java.io.Serializable
	[Serializable]
	public abstract class CommonAttribute
	{

		/// <summary>
		/// The key for specifying start at time as generic information </summary>
		public const string GENERIC_INFO_START_AT_KEY = "START_AT";

		/// <summary>
		/// The default value of defining how long to wait before restart task in error (zero or negative value means restart immediately) </summary>
		public const long DEFAULT_TASK_RETRY_DELAY = -1l;

		/// <summary>
		/// Define where will a task be restarted if an error occurred (default is ANYWHERE).
		/// <para>
		/// It will be restarted according to the number of execution remaining.
		/// </para>
		/// <para>
		/// You can override this property inside each task.
		/// </para>
		/// </summary>
		protected internal UpdatableProperties<RestartMode> restartTaskOnError = new UpdatableProperties<RestartMode>(RestartMode.ANYWHERE);

		/// <summary>
		/// Specify how long to wait before restart the task if an error occurred. </summary>
		protected internal UpdatableProperties<LongWrapper> taskRetryDelay = new UpdatableProperties<LongWrapper>(new LongWrapper(DEFAULT_TASK_RETRY_DELAY));

		/// <summary>
		/// The maximum number of execution for a task (default 1).
		/// <para>
		/// You can override this property inside each task.
		/// </para>
		/// </summary>
		protected internal UpdatableProperties<IntegerWrapper> maxNumberOfExecution = new UpdatableProperties<IntegerWrapper>(new IntegerWrapper(1));

		/// <summary>
		/// Common user informations </summary>
		protected internal IDictionary<string, string> genericInformation = new Dictionary<string, string>();

		protected internal IDictionary<string, string> unresolvedGenericInformation = new Dictionary<string, string>();

		/// <summary>
		/// OnTaskError defines the behavior happening when a task fails.
		/// </summary>
		protected internal UpdatableProperties<OnTaskError> onTaskError = new UpdatableProperties<OnTaskError>(OnTaskError.NONE);

		/// <summary>
		/// Set onTaskError property value. </summary>
		/// <param name="onTaskError"> A OnTaskError instance. </param>
		/// <exception cref="IllegalArgumentException"> If set to null. </exception>
		public virtual OnTaskError OnTaskError
		{
			set
			{
				if (value == null)
				{
					throw new System.ArgumentException("OnTaskError cannot be set to null.");
				}
				this.onTaskError.Value = value;
			}
		}

		/// <summary>
		/// Get the OnTaskError UpdatableProperties. </summary>
		/// <returns> Reference to the UpdatableProperties instance hold by this class. </returns>
		public virtual UpdatableProperties<OnTaskError> OnTaskErrorProperty
		{
			get
			{
				return this.onTaskError;
			}
		}

		/// <summary>
		/// Get how long to wait before restart the task if an error occurred.
		/// </summary>
		/// <returns> delay to restart a task in error </returns>
		public virtual long? getTaskRetryDelay()
		{
			return taskRetryDelay.Value.LongValue;
		}

		/// <summary>
		/// Get taskRetryDelay UpdatableProperties </summary>
		/// <returns> taskRetryDelay UpdatableProperties </returns>
		public virtual UpdatableProperties<LongWrapper> TaskRetryDelayProperty
		{
			get
			{
				return this.taskRetryDelay;
			}
		}

		/// <summary>
		/// Set how long to wait before restart the task if an error occurred.
		/// </summary>
		/// <param name="taskRetryDelay"> delay to restart a task in error </param>
		public virtual void setTaskRetryDelay(long taskRetryDelay)
		{
			this.taskRetryDelay.Value = new LongWrapper(taskRetryDelay);
		}

		/// <summary>
		/// Returns the restartTaskOnError state.
		/// </summary>
		/// <returns> the restartTaskOnError state. </returns>
		public virtual RestartMode RestartTaskOnError
		{
			get
			{
				return restartTaskOnError.Value;
			}
			set
			{
				this.restartTaskOnError.Value = value;
			}
		}


		/// <summary>
		/// Get the restartTaskOnError updatable property.
		/// </summary>
		/// <returns> the restartTaskOnError updatable property. </returns>
		public virtual UpdatableProperties<RestartMode> RestartTaskOnErrorProperty
		{
			get
			{
				return restartTaskOnError;
			}
		}

		/// <summary>
		/// Get the number of execution allowed for this task.
		/// </summary>
		/// <returns> the number of execution allowed for this task </returns>
		public virtual int MaxNumberOfExecution
		{
			get
			{
				return (maxNumberOfExecution.Value.IntegerValue).Value;
			}
			set
			{
				if (value <= 0)
				{
					throw new System.ArgumentException("The number of execution must be a non negative integer (>0) !");
				}
				this.maxNumberOfExecution.Value = new IntegerWrapper(value);
			}
		}


		/// <summary>
		/// Get the maximum number Of Execution updatable property.
		/// </summary>
		/// <returns> the maximum number Of Execution updatable property </returns>
		public virtual UpdatableProperties<IntegerWrapper> MaxNumberOfExecutionProperty
		{
			get
			{
				return maxNumberOfExecution;
			}
		}

		/// <summary>
		/// Returns generic information.
		/// <para>
		/// These information are transmitted to the policy that can use it for the scheduling.
		/// 
		/// </para>
		/// </summary>
		/// <returns> generic information. </returns>
		public virtual IDictionary<string, string> GenericInformation
		{
			get
			{
				ISet<KeyValuePair<string, string>> entries = this.genericInformation.SetOfKeyValuePairs();
				IDictionary<string, string> result = new Dictionary<string, string>(entries.Count);
				foreach (KeyValuePair<string, string> entry in entries)
				{
					result[entry.Key] = entry.Value;
				}
				return result;
			}
			set
			{
				if (value != null)
				{
					this.genericInformation = value;
				}
				else
				{
					this.genericInformation = new Dictionary<string, string>();
				}
    
			}
		}

		/// <summary>
		/// Returns the generic information without variable replacements.
		/// </summary>
		/// <returns> unresolved generic information. </returns>
		public virtual IDictionary<string, string> UnresolvedGenericInformation
		{
			get
			{
				ISet<KeyValuePair<string, string>> entries = this.unresolvedGenericInformation.SetOfKeyValuePairs();
				IDictionary<string, string> result = new Dictionary<string, string>(entries.Count);
				foreach (KeyValuePair<string, string> entry in entries)
				{
					result[entry.Key] = entry.Value;
				}
				return result;
			}
			set
			{
				if (value != null)
				{
					this.unresolvedGenericInformation = value;
				}
				else
				{
					this.unresolvedGenericInformation = new Dictionary<string, string>();
				}
    
			}
		}

		/// <summary>
		/// Add an information to the generic informations map field.
		/// This information will be given to the scheduling policy.
		/// </summary>
		/// <param name="key"> the key in which to store the informations. </param>
		/// <param name="genericInformation"> the information to store. </param>
		public virtual void addGenericInformation(string key, string genericInformation)
		{
			if (!string.ReferenceEquals(key, null) && key.Length > 255)
			{
				throw new System.ArgumentException("Key is too long, it must have 255 chars length max : " + key);
			}
			this.genericInformation[key] = genericInformation;
		}

		/// <summary>
		/// Add a map of generic informations into the generic informations map field.
		/// </summary>
		/// <param name="genericInformations"> the generic informations map to add. </param>
		public virtual void addGenericInformations(IDictionary<string, string> genericInformations)
		{
			if (genericInformations != null)
			{
				ISet<KeyValuePair<string, string>> entries = genericInformations.SetOfKeyValuePairs();
				foreach (KeyValuePair<string, string> entry in entries)
				{
					addGenericInformation(entry.Key, entry.Value);
				}
			}
		}



		protected internal static IDictionary<string, string> applyReplacementsOnGenericInformation(IDictionary<string, string> genericInformation, IDictionary<string, string> variables)
		{
			return VariableSubstitutor.filterAndUpdate(genericInformation, variables);
		}

	}

}