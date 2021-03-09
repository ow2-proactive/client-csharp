using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

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
namespace org.ow2.proactive.scheduler.common.job.factories
{


	using FileSelector = org.objectweb.proactive.extensions.dataspaces.vfs.selector.FileSelector;
	using JobVariable = org.ow2.proactive.scheduler.common.job.JobVariable;
	using TaskFlowJob = org.ow2.proactive.scheduler.common.job.TaskFlowJob;
	using ForkEnvironment = org.ow2.proactive.scheduler.common.task.ForkEnvironment;
	using NativeTask = org.ow2.proactive.scheduler.common.task.NativeTask;
	using ParallelEnvironment = org.ow2.proactive.scheduler.common.task.ParallelEnvironment;
	using ScriptTask = org.ow2.proactive.scheduler.common.task.ScriptTask;
	using Task = org.ow2.proactive.scheduler.common.task.Task;
	using TaskVariable = org.ow2.proactive.scheduler.common.task.TaskVariable;
	using InputSelector = org.ow2.proactive.scheduler.common.task.dataspaces.InputSelector;
	using OutputSelector = org.ow2.proactive.scheduler.common.task.dataspaces.OutputSelector;
	using FlowActionType = org.ow2.proactive.scheduler.common.task.flow.FlowActionType;
	using FlowBlock = org.ow2.proactive.scheduler.common.task.flow.FlowBlock;
	using FlowScript = org.ow2.proactive.scheduler.common.task.flow.FlowScript;
	using Script = org.ow2.proactive.scripting.Script<object>;
	using SelectionScript = org.ow2.proactive.scripting.SelectionScript;
	using ArbitraryTopologyDescriptor = org.ow2.proactive.topology.descriptor.ArbitraryTopologyDescriptor;
	using BestProximityDescriptor = org.ow2.proactive.topology.descriptor.BestProximityDescriptor;
	using DifferentHostsExclusiveDescriptor = org.ow2.proactive.topology.descriptor.DifferentHostsExclusiveDescriptor;
	using MultipleHostsExclusiveDescriptor = org.ow2.proactive.topology.descriptor.MultipleHostsExclusiveDescriptor;
	using SingleHostDescriptor = org.ow2.proactive.topology.descriptor.SingleHostDescriptor;
	using SingleHostExclusiveDescriptor = org.ow2.proactive.topology.descriptor.SingleHostExclusiveDescriptor;
	using ThresholdProximityDescriptor = org.ow2.proactive.topology.descriptor.ThresholdProximityDescriptor;
	using TopologyDescriptor = org.ow2.proactive.topology.descriptor.TopologyDescriptor;


	/// <summary>
	/// Helper class to transform a job into its xml representation.
	/// <para>
	/// The xml produced conforms to the definition in <seealso cref="Schemas"/>.
	/// </para>
	/// <para>
	/// The order of elements is sensitive.
	/// 
	/// @author esalagea
	/// </para>
	/// </summary>
	public class Job2XMLTransformer
	{

		public Job2XMLTransformer()
		{

		}

		/// <summary>
		/// Creates the xml representation of the job in argument
		/// </summary>
		/// <exception cref="TransformerException"> </exception>
		/// <exception cref="ParserConfigurationException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public java.io.InputStream jobToxml(org.ow2.proactive.scheduler.common.job.TaskFlowJob job) throws javax.xml.transform.TransformerException, javax.xml.parsers.ParserConfigurationException
		public virtual Stream jobToxml(TaskFlowJob job)
		{
			XmlDocument doc = new XmlDocument();

			// create the xml tree corresponding to this job
			XmlElement rootJob = createRootJobElement(doc, job);
			doc.AppendChild(rootJob);

			MemoryStream xmlStream = new MemoryStream();
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = "\t";
			XmlWriter writer = XmlWriter.Create(xmlStream, settings);
			doc.Save(writer);
			return xmlStream;
		}

		/// <summary>
		/// Creates the xml representation of the job in argument
		/// </summary>
		/// <exception cref="TransformerException"> </exception>
		/// <exception cref="ParserConfigurationException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public String jobToxmlString(org.ow2.proactive.scheduler.common.job.TaskFlowJob job) throws javax.xml.transform.TransformerException, javax.xml.parsers.ParserConfigurationException, java.io.IOException
		public virtual string jobToxmlString(TaskFlowJob job)
		{
			Stream @is = jobToxml(job);
			@is.Seek(0, SeekOrigin.Begin);
			string answer;
			using (StreamReader reader = new StreamReader(@is, Encoding.UTF8))
			{
				answer = reader.ReadToEnd();
			}		
			return answer;
		}

		/// <summary>
		/// Serializes the given job as xml and writes it to a file.
		/// </summary>
		/// <param name="job">
		///            TaskFlowJob to be serialized </param>
		/// <param name="f">
		///            The file where the xml will be written </param>
		/// <exception cref="ParserConfigurationException"> </exception>
		/// <exception cref="TransformerException"> </exception>
		/// <exception cref="IOException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public void job2xmlFile(org.ow2.proactive.scheduler.common.job.TaskFlowJob job, java.io.File f) throws javax.xml.parsers.ParserConfigurationException, javax.xml.transform.TransformerException, java.io.IOException
		public virtual void job2xmlFile(TaskFlowJob job, FileInfo f)
		{
			string xmlString = jobToxmlString(job);
			using (StreamWriter fw = new StreamWriter(f.FullName))
			{
				fw.Write(xmlString);
			}
		}

		/// <summary>
		/// Creates the "job" element <define name="job">
		/// </summary>
		private XmlElement createRootJobElement(XmlDocument doc, TaskFlowJob job)
		{
			XmlElement rootJob = doc.CreateElement("job", Schemas.SCHEMA_LATEST.Namespace);

			// ********** attributes ***********
			XmlAttribute attribute = doc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
			attribute.Value = Schemas.SCHEMA_LATEST.Namespace + " http://www.activeeon.com/public_content/schemas/proactive/jobdescriptor/" + Schemas.SCHEMA_LATEST.Version + "/schedulerjob.xsd";
			rootJob.Attributes.Append(attribute);			

			if (job.ProjectName.Length > 0)
			{
				setAttribute(rootJob, XMLAttributes.JOB_PROJECT_NAME, job.ProjectName, true);
			}
			setAttribute(rootJob, XMLAttributes.JOB_PRIORITY, job.Priority.ToString());
			if (job.OnTaskErrorProperty.Set)
			{
				setAttribute(rootJob, XMLAttributes.COMMON_ON_TASK_ERROR, job.OnTaskErrorProperty.Value.ToString(), true);
			}
			if (job.MaxNumberOfExecutionProperty.Set)
			{
				setAttribute(rootJob, XMLAttributes.COMMON_MAX_NUMBER_OF_EXECUTION, Convert.ToString(job.MaxNumberOfExecution));
			}
			setAttribute(rootJob, XMLAttributes.COMMON_NAME, job.Name, true);
			if (job.RestartTaskOnErrorProperty.Set)
			{
				setAttribute(rootJob, XMLAttributes.COMMON_RESTART_TASK_ON_ERROR, job.RestartTaskOnError.ToString());
			}
			if (job.TaskRetryDelayProperty.Set)
			{
				setAttribute(rootJob, XMLAttributes.COMMON_TASK_RETRY_DELAY, formatDate(job.getTaskRetryDelay().Value));
			}

			// *** elements ***

			// <ref name="variables"/>
			if (job.Variables != null && job.Variables.Count > 0)
			{
				XmlElement variablesE = createJobVariablesElement(doc, job.Variables);
				rootJob.AppendChild(variablesE);
			}

			// <ref name="jobDescription"/>
			if (!string.ReferenceEquals(job.Description, null))
			{
				XmlElement descrNode = createCDataElement(doc, XMLTags.COMMON_DESCRIPTION.XMLName, job.Description);
				rootJob.AppendChild(descrNode);
			}

			// <ref name="genericInformation"/>
			if ((job.GenericInformation != null) && (job.GenericInformation.Count > 0))
			{
				XmlElement genericInfo = createGenericInformation(doc, job.GenericInformation);
				rootJob.AppendChild(genericInfo);
			}

			// <ref name="inputSpace"/>
			if (!string.ReferenceEquals(job.InputSpace, null))
			{
				XmlElement inputspace = createElement(doc, XMLTags.DS_INPUT_SPACE.XMLName, null, new Attribute(XMLAttributes.DS_URL.XMLName, job.InputSpace));
				rootJob.AppendChild(inputspace);
			}

			// <ref name="outputSpace"/>
			if (!string.ReferenceEquals(job.OutputSpace, null))
			{
				XmlElement outputSpace = createElement(doc, XMLTags.DS_OUTPUT_SPACE.XMLName, null, new Attribute(XMLAttributes.DS_URL.XMLName, job.OutputSpace));
				rootJob.AppendChild(outputSpace);
			}

			// <ref name="globalSpace"/>
			if (!string.ReferenceEquals(job.GlobalSpace, null))
			{
				XmlElement globalSpace = createElement(doc, XMLTags.DS_GLOBAL_SPACE.XMLName, null, new Attribute(XMLAttributes.DS_URL.XMLName, job.GlobalSpace));
				rootJob.AppendChild(globalSpace);
			}

			// <ref name="userSpace"/>
			if (!string.ReferenceEquals(job.UserSpace, null))
			{
				XmlElement userSpace = createElement(doc, XMLTags.DS_USER_SPACE.XMLName, null, new Attribute(XMLAttributes.DS_URL.XMLName, job.UserSpace));
				rootJob.AppendChild(userSpace);
			}

			// <ref name="taskFlow"/>
			XmlElement taskFlow = createTaskFlowElement(doc, job);
			rootJob.AppendChild(taskFlow);

			if (!string.ReferenceEquals(job.Visualization, null))
			{
				//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
				//ORIGINAL LINE: final org.w3c.dom.Element metadata = createElement(doc, XMLTags.METADATA.getXMLName(), null);
				XmlElement metadata = createElement(doc, XMLTags.METADATA.XMLName, null);
				metadata.AppendChild(createCDataElement(doc, XMLTags.METADATA_VISUALIZATION.XMLName, job.Visualization));
				rootJob.AppendChild(metadata);
			}

			return rootJob;
		}

		/// <summary>
		/// Creates an element and set the value of its attributes
		/// 
		/// </summary>
		private XmlElement createElement(XmlDocument doc, string tagName, string elementText, params Attribute[] attribs)
		{
			XmlElement el = doc.CreateElement(tagName, Schemas.SCHEMA_LATEST.Namespace);
			foreach (Attribute a in attribs)
			{
				el.SetAttribute(a.Name, a.Value);
			}
			if (!string.ReferenceEquals(elementText, null))
			{
				XmlText text = doc.CreateTextNode(elementText);
				el.AppendChild(text);
			}
			return el;
		}

		private XmlElement createCDataElement(XmlDocument doc, string tagName, string elementText, params Attribute[] attribs)
		{
			XmlElement el = doc.CreateElement(tagName, Schemas.SCHEMA_LATEST.Namespace);
			foreach (Attribute a in attribs)
			{
				el.SetAttribute(a.Name, a.Value);
			}
			if (!string.ReferenceEquals(elementText, null))
			{
				XmlCDataSection section = doc.CreateCDataSection(elementText);
				el.AppendChild(section);
			}
			return el;
		}

		/*
		 * Creates the job variables element
		 */
		private XmlElement createJobVariablesElement(XmlDocument doc, IDictionary<string, JobVariable> jobVariables)
		{
			if (jobVariables == null)
			{
				return null;
			}
			XmlElement variablesE = doc.CreateElement(XMLTags.VARIABLES.XMLName, Schemas.SCHEMA_LATEST.Namespace);
			foreach (string name in jobVariables.Keys)
			{
				XmlElement variableE = createElement(doc, XMLTags.VARIABLE.XMLName, null, new Attribute(XMLAttributes.VARIABLE_NAME.XMLName, name), new Attribute(XMLAttributes.VARIABLE_VALUE.XMLName, jobVariables[name].Value), new Attribute(XMLAttributes.VARIABLE_MODEL.XMLName, jobVariables[name].Model));
				variablesE.AppendChild(variableE);
			}
			return variablesE;
		}

		/// <summary>
		/// Creates the task variables element
		/// </summary>
		private XmlElement createTaskVariablesElement(XmlDocument doc, IDictionary<string, TaskVariable> variables)
		{
			if (variables == null)
			{
				return null;
			}
			XmlElement variablesE = doc.CreateElement(XMLTags.VARIABLES.XMLName, Schemas.SCHEMA_LATEST.Namespace);
			foreach (TaskVariable variable in variables.Values)
			{
				XmlElement variableE = createElement(doc, XMLTags.VARIABLE.XMLName, null, new Attribute(XMLAttributes.VARIABLE_NAME.XMLName, variable.Name), new Attribute(XMLAttributes.VARIABLE_VALUE.XMLName, variable.Value), new Attribute(XMLAttributes.VARIABLE_MODEL.XMLName, variable.Model), new Attribute(XMLAttributes.VARIABLE_JOB_INHERITED.XMLName, variable.JobInherited.ToString()));
				variablesE.AppendChild(variableE);
			}
			return variablesE;
		}

		/// <summary>
		/// Creates the generic information element corresponding to <define
		/// name="genericInformation">
		/// 
		/// </summary>
		private XmlElement createGenericInformation(XmlDocument doc, IDictionary<string, string> info)
		{
			if (info == null)
			{
				return null;
			}

			XmlElement el = doc.CreateElement(XMLTags.COMMON_GENERIC_INFORMATION.XMLName, Schemas.SCHEMA_LATEST.Namespace);

			// <oneOrMore>
			// <ref name="info"/>
			// </oneOrMore>
			foreach (string name in info.Keys)
			{
				XmlElement infoElement = createElement(doc, XMLTags.COMMON_INFO.XMLName, null, new Attribute(XMLAttributes.COMMON_NAME.XMLName, name), new Attribute(XMLAttributes.COMMON_VALUE.XMLName, info[name]));
				el.AppendChild(infoElement);
			}
			return el;
		}

		/// <summary>
		/// Sets the value of the given attribute to lowerCase, if the value is not
		/// null, otherwise it doesn't do anything
		/// </summary>
		/// <param name="e">
		///            element to set the attribute value for </param>
		private void setAttribute(XmlElement e, XMLAttributes attrib, string attribVal)
		{
			if (string.ReferenceEquals(attribVal, null))
			{
				return;
			}
			e.SetAttribute(attrib.XMLName, attribVal.ToLower());
		}

		/// <summary>
		/// Sets the value of the given attribute.
		/// </summary>
		/// <param name="caseSensitive">
		///            if true, the attribVal case is kept, if false, the value is
		///            set as lowercase </param>
		private void setAttribute(XmlElement e, XMLAttributes attrib, string attribVal, bool caseSensitive)
		{
			if (string.ReferenceEquals(attribVal, null))
			{
				return;
			}

			if (caseSensitive)
			{
				e.SetAttribute(attrib.XMLName, attribVal);
			}
			else
			{
				setAttribute(e, attrib, attribVal);
			}
		}

		/// <summary>
		/// Creates the taskflow element, corresponding to <define name="taskFlow">
		/// 
		/// </summary>
		private XmlElement createTaskFlowElement(XmlDocument doc, TaskFlowJob job)
		{
			XmlElement taskFlowElement = doc.CreateElement(XMLTags.TASK_FLOW.XMLName, Schemas.SCHEMA_LATEST.Namespace);

			List<Task> tasks = job.Tasks;

			// <oneOrMore>
			// <ref name="task"/>
			// </oneOrMore>
			foreach (Task task in tasks)
			{
				XmlElement taskE = createTaskElement(doc, task);
				taskFlowElement.AppendChild(taskE);
			}
			return taskFlowElement;
		}

		/// <summary>
		/// Creates the task element, corressponding to <define name="task">
		/// 
		/// </summary>
		private XmlElement createTaskElement(XmlDocument doc, Task task)
		{
			XmlElement taskE = doc.CreateElement(XMLTags.TASK.XMLName, Schemas.SCHEMA_LATEST.Namespace);

			// **** attributes *****
			// **** common attributes ***

			if (task.OnTaskErrorProperty.Set)
			{
				setAttribute(taskE, XMLAttributes.COMMON_ON_TASK_ERROR, task.OnTaskErrorProperty.Value.ToString(), true);
			}
			if (task.MaxNumberOfExecutionProperty.Set)
			{
				setAttribute(taskE, XMLAttributes.COMMON_MAX_NUMBER_OF_EXECUTION, Convert.ToString(task.MaxNumberOfExecution));
			}
			setAttribute(taskE, XMLAttributes.COMMON_NAME, task.Name, true);
			if (task.RestartTaskOnErrorProperty.Set)
			{
				setAttribute(taskE, XMLAttributes.COMMON_RESTART_TASK_ON_ERROR, task.RestartTaskOnError.ToString());
			}
			if (task.TaskRetryDelayProperty.Set)
			{
				setAttribute(taskE, XMLAttributes.COMMON_TASK_RETRY_DELAY, formatDate(task.getTaskRetryDelay().Value));
			}

			// *** task attributes ***
			if (task.WallTime != 0)
			{
				setAttribute(taskE, XMLAttributes.TASK_WALLTIME, formatDate(task.WallTime));
			}

			if (task.RunAsMe)
			{
				setAttribute(taskE, XMLAttributes.TASK_RUN_AS_ME, "true");
			}

			if (task.Fork != null && task.Fork.Value)
			{
				setAttribute(taskE, XMLAttributes.TASK_FORK, "true");
			}

			if (task.PreciousResult)
			{
				setAttribute(taskE, XMLAttributes.TASK_PRECIOUS_RESULT, "true");
			}

			if (task.PreciousLogs)
			{
				setAttribute(taskE, XMLAttributes.TASK_PRECIOUS_LOGS, "true");
			}

			// *** elements ****

			// <ref name="taskDescription"/>
			if (!string.ReferenceEquals(task.Description, null))
			{
				XmlElement descrNode = createCDataElement(doc, XMLTags.COMMON_DESCRIPTION.XMLName, task.Description);
				taskE.AppendChild(descrNode);
			}

			// <ref name="variables"/>
			if (task.Variables != null && task.Variables.Count > 0)
			{
				XmlElement variablesE = createTaskVariablesElement(doc, task.Variables);
				taskE.AppendChild(variablesE);
			}

			// <ref name="genericInformation"/>
			if ((task.GenericInformation != null) && (task.GenericInformation.Count > 0))
			{
				XmlElement genericInfoE = createGenericInformation(doc, task.GenericInformation);
				taskE.AppendChild(genericInfoE);
			}

			// <ref name="depends"/>
			IList<Task> dependencies = task.DependencesList;
			if ((dependencies != null) && (dependencies.Count > 0))
			{
				XmlElement dependsE = doc.CreateElement(XMLTags.TASK_DEPENDENCES.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				foreach (Task dep in dependencies)
				{
					XmlElement dependsTask = doc.CreateElement(XMLTags.TASK_DEPENDENCES_TASK.XMLName, Schemas.SCHEMA_LATEST.Namespace);
					setAttribute(dependsTask, XMLAttributes.TASK_DEPENDS_REF, dep.Name, true);
					dependsE.AppendChild(dependsTask);
				}
				taskE.AppendChild(dependsE);
			} // if has dependencies

			// <ref name="inputFiles"/>
			IList<InputSelector> inputFiles = task.InputFilesList;
			if (inputFiles != null)
			{
				XmlElement inputFilesE = doc.CreateElement(XMLTags.DS_INPUT_FILES.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				foreach (InputSelector inputSelector in inputFiles)
				{
					FileSelector fs = inputSelector.InputFiles;
					XmlElement filesE = doc.CreateElement(Schemas.SCHEMA_LATEST.Namespace, XMLTags.DS_FILES.XMLName);
					// the xml only supports one value for the includes/excludes
					// pattern
					if (!(fs.getIncludes().Count == 0))
					{
						setAttribute(filesE, XMLAttributes.DS_INCLUDES, fs.getIncludes().First(), true);
					}
					if (!(fs.getExcludes().Count == 0))
					{
						setAttribute(filesE, XMLAttributes.DS_EXCLUDES, fs.getExcludes().First(), true);
					}
					if (inputSelector.Mode != null)
					{
						setAttribute(filesE, XMLAttributes.DS_ACCESS_MODE, inputSelector.Mode.ToString(), true);
					}
					inputFilesE.AppendChild(filesE);
				}
				taskE.AppendChild(inputFilesE);
			}

			// <ref name="parallel"/>
			XmlElement parallelEnvE = createParallelEnvironment(doc, task);
			if (parallelEnvE != null)
			{
				taskE.AppendChild(parallelEnvE);
			}

			// <ref name="selection"/>
			IList<SelectionScript> selectionScripts = task.SelectionScripts;
			if (selectionScripts != null && selectionScripts.Count > 0)
			{
				XmlElement selectionE = doc.CreateElement(XMLTags.SCRIPT_SELECTION.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				foreach (SelectionScript selectionScript in selectionScripts)
				{
					XmlElement scriptE = createScriptElement(doc, selectionScript);
					selectionE.AppendChild(scriptE);
				}
				taskE.AppendChild(selectionE);
			}

			// <ref name="forkEnvironment"/>
			if (task.ForkEnvironment != null)
			{
				XmlElement forkEnvE = createForkEnvironmentElement(doc, task.ForkEnvironment);
				taskE.AppendChild(forkEnvE);
			}

			// <ref name="pre"/>
			Script preScript = task.PreScript;
			if (preScript != null)
			{
				XmlElement preE = doc.CreateElement(XMLTags.SCRIPT_PRE.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				XmlElement scriptE = createScriptElement(doc, preScript);
				preE.AppendChild(scriptE);
				taskE.AppendChild(preE);
			}

			// <ref name="executable"/>
			XmlElement executableE = null;
			if (task is NativeTask)
			{
				executableE = createNativeExecutableElement(doc, (NativeTask) task);
			}
			else if (task is ScriptTask)
			{
				executableE = createScriptExecutableElement(doc, (ScriptTask) task);
			}
			taskE.AppendChild(executableE);

			// <ref name="flow"/>
			XmlElement controlFlowE = createFlowControlElement(doc, task);
			if (controlFlowE != null)
			{
				taskE.AppendChild(controlFlowE);
			}

			// <ref name="post"/>
			Script postScript = task.PostScript;
			if (postScript != null)
			{
				XmlElement postE = doc.CreateElement(XMLTags.SCRIPT_POST.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				XmlElement scriptE = createScriptElement(doc, postScript);
				postE.AppendChild(scriptE);
				taskE.AppendChild(postE);
			}

			// <ref name="cleaning"/>
			Script cleanScript = task.CleaningScript;
			if (cleanScript != null)
			{
				XmlElement cleanE = doc.CreateElement(XMLTags.SCRIPT_CLEANING.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				XmlElement scriptE = createScriptElement(doc, cleanScript);
				cleanE.AppendChild(scriptE);
				taskE.AppendChild(cleanE);
			}

			// <ref name="outputFiles"/>
			IList<OutputSelector> outputFiles = task.OutputFilesList;
			if (outputFiles != null)
			{
				XmlElement outputFilesE = doc.CreateElement(XMLTags.DS_OUTPUT_FILES.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				foreach (OutputSelector outputSelector in outputFiles)
				{
					FileSelector fs = outputSelector.OutputFiles;
					XmlElement filesE = doc.CreateElement(XMLTags.DS_FILES.XMLName, Schemas.SCHEMA_LATEST.Namespace);
					// the xml only supports one value for the includes/excludes
					// pattern
					if (!(fs.getIncludes().Count == 0))
					{
						setAttribute(filesE, XMLAttributes.DS_INCLUDES, fs.getIncludes().First(), true);
					}
					if (!(fs.getExcludes().Count == 0))
					{
						setAttribute(filesE, XMLAttributes.DS_EXCLUDES, fs.getExcludes().First(), true);
					}
					if (outputSelector.Mode != null)
					{
						setAttribute(filesE, XMLAttributes.DS_ACCESS_MODE, outputSelector.Mode.ToString(), true);
					}
					outputFilesE.AppendChild(filesE);
				}
				taskE.AppendChild(outputFilesE);
			}
			return taskE;
		}

		/// <summary>
		/// Creates the parallel environment element for the given task. Corresponds
		/// to <define name="parallel">
		/// </summary>
		/// <returns> the <seealso cref="XMLTags.PARALLEL_ENV"/> element if the task has a
		///         parallel environment, null otherwise </returns>
		private XmlElement createParallelEnvironment(XmlDocument doc, Task task)
		{
			ParallelEnvironment penv = task.ParallelEnvironment;
			if (penv == null)
			{
				return null;
			}

			XmlElement parallelEnvE = doc.CreateElement(XMLTags.PARALLEL_ENV.XMLName, Schemas.SCHEMA_LATEST.Namespace);
			setAttribute(parallelEnvE, XMLAttributes.TASK_NB_NODES, Convert.ToString(penv.NodesNumber));

			// <ref name="topology"/>
			TopologyDescriptor topologyDescr = penv.TopologyDescriptor;
			if (topologyDescr != null)
			{

				// <choice>
				// <ref name="arbitrary"/>
				// <ref name="bestProximity"/>
				// <ref name="thresholdProximity"/>
				// <ref name="singleHost"/>
				// <ref name="singleHostExclusive"/>
				// <ref name="multipleHostsExclusive"/>
				// <ref name="differentHostsExclusive"/>
				// </choice>

				XmlElement topologyE = doc.CreateElement(XMLTags.TOPOLOGY.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				XmlElement topologyDescrE = null;

				if (topologyDescr is ArbitraryTopologyDescriptor)
				{
					topologyDescrE = doc.CreateElement(XMLTags.TOPOLOGY_ARBITRARY.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				}
				else if (topologyDescr is ThresholdProximityDescriptor)
				{
					topologyDescrE = doc.CreateElement(XMLTags.TOPOLOGY_THRESHOLD_PROXIMITY.XMLName, Schemas.SCHEMA_LATEST.Namespace);
					long threshold = ((ThresholdProximityDescriptor) topologyDescr).Threshold;
					topologyDescrE.SetAttribute(XMLAttributes.TOPOLOGY_THRESHOLD.XMLName, Convert.ToString(threshold));
				}
				else if (topologyDescr is BestProximityDescriptor)
				{
					topologyDescrE = doc.CreateElement(XMLTags.TOPOLOGY_BEST_PROXIMITY.XMLName, Schemas.SCHEMA_LATEST.Namespace);

				}
				else if (topologyDescr is SingleHostExclusiveDescriptor)
				{
					topologyDescrE = doc.CreateElement(XMLTags.TOPOLOGY_SINGLE_HOST_EXCLUSIVE.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				}
				else if (topologyDescr is SingleHostDescriptor)
				{
					topologyDescrE = doc.CreateElement(XMLTags.TOPOLOGY_SINGLE_HOST.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				}
				else if (topologyDescr is MultipleHostsExclusiveDescriptor)
				{
					topologyDescrE = doc.CreateElement(XMLTags.TOPOLOGY_MULTIPLE_HOSTS_EXCLUSIVE.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				}
				if (topologyDescr is DifferentHostsExclusiveDescriptor)
				{
					topologyDescrE = doc.CreateElement(XMLTags.TOPOLOGY_DIFFERENT_HOSTS_EXCLUSIVE.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				}

				if (topologyDescrE != null)
				{
					topologyE.AppendChild(topologyDescrE);
				}

				parallelEnvE.AppendChild(topologyE);
			}
			return parallelEnvE;
		}

		/// <summary>
		/// Creates a flow control element for the given task <element
		/// name="controlFlow">
		/// </summary>
		/// <returns> the xml Element corresponding to the flow control, if the task
		///         contains a flow control, null otherwise </returns>
		private XmlElement createFlowControlElement(XmlDocument doc, Task task)
		{
			XmlElement controlFlowE = null;

			// <ref name="block"/>
			if (task.FlowBlock != FlowBlock.NONE)
			{
				controlFlowE = doc.CreateElement(XMLTags.FLOW.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				setAttribute(controlFlowE, XMLAttributes.FLOW_BLOCK, task.FlowBlock.ToString());
			}

			FlowScript flowScript = task.FlowScript;
			if (flowScript != null)
			{

				XmlElement flowActionE = null;

				// flowActionE can be if, loop, replicate or null.
				// if not null, it contains a script element

				// <ref name="actionIf"/>
				// <ref name="actionReplicate"/>
				// <ref name="actionLoop"/>
				// </choice>

				// *** if ***
				// <element name="if">
				if (flowScript.ActionType.Equals(FlowActionType.IF.ToString()))
				{
					flowActionE = doc.CreateElement(XMLTags.FLOW_IF.XMLName, Schemas.SCHEMA_LATEST.Namespace);
					setAttribute(flowActionE, XMLAttributes.FLOW_TARGET, flowScript.ActionTarget, true);
					setAttribute(flowActionE, XMLAttributes.FLOW_ELSE, flowScript.ActionTargetElse, true);
					setAttribute(flowActionE, XMLAttributes.FLOW_CONTINUATION, flowScript.ActionContinuation, true);
				}

				// *** loop ***
				// <element name="loop">
				if (flowScript.ActionType.Equals(FlowActionType.LOOP.ToString()))
				{
					flowActionE = doc.CreateElement(XMLTags.FLOW_LOOP.XMLName, Schemas.SCHEMA_LATEST.Namespace);
					setAttribute(flowActionE, XMLAttributes.FLOW_TARGET, flowScript.ActionTarget, true);
				}

				// *** replicate ***
				// <element name="replicate">
				if (flowScript.ActionType.Equals(FlowActionType.REPLICATE.ToString()))
				{
					flowActionE = doc.CreateElement(XMLTags.FLOW_REPLICATE.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				}

				if (flowActionE != null)
				{
					if (controlFlowE == null)
					{
						controlFlowE = doc.CreateElement(XMLTags.FLOW.XMLName, Schemas.SCHEMA_LATEST.Namespace);
					}
					XmlElement scriptE = createScriptElement(doc, flowScript);
					flowActionE.AppendChild(scriptE);
					controlFlowE.AppendChild(flowActionE);
				}
			} // flowScript !=null

			return controlFlowE;

		}

		/// <summary>
		/// Corresponds to <element name="script">
		/// 
		/// The schema allows the specification of a script either by writing the
		/// script code either by providing a file with arguments. Both will result
		/// in the same <seealso cref="org.ow2.proactive.scripting.Script"/> object. In the
		/// current translation we will always translate a Script object by inlining
		/// the script code using a "codeScript"element (first option).
		/// 
		/// The xml specification does not allow addding arguments to a script
		/// defined by its code. Therefore, when we translate the script object to
		/// xml, if we encounter arguments, we will insert their value directly in
		/// the script's code by inserting a line like:
		/// <p/>
		/// var args = ["argument_1",...,"argument_n"];
		/// 
		/// </summary>
		private XmlElement createScriptElement(XmlDocument doc, Script script)
		{
			XmlElement scriptElement = doc.CreateElement(XMLTags.SCRIPT_SCRIPT.XMLName, Schemas.SCHEMA_LATEST.Namespace);
			if (script.ScriptUrl != null && script.GetScript() == null)
			{
				XmlElement fileE = doc.CreateElement(XMLTags.SCRIPT_FILE.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				setAttribute(fileE, XMLAttributes.SCRIPT_URL, script.ScriptUrl.ToString(), true);
				if (script.EngineName != null)
				{
					setAttribute(fileE, XMLAttributes.LANGUAGE, script.EngineName, true);
				}
				string[] @params = script.Parameters;
				if (@params != null && @params.Length > 0)
				{
					XmlElement parametersE = doc.CreateElement(XMLTags.SCRIPT_ARGUMENTS.XMLName, Schemas.SCHEMA_LATEST.Namespace);
					foreach (string param in @params)
					{
						XmlElement parameterE = doc.CreateElement(XMLTags.SCRIPT_ARGUMENT.XMLName, Schemas.SCHEMA_LATEST.Namespace);
						setAttribute(parameterE, XMLAttributes.COMMON_VALUE, param.ToString(), true);
						parametersE.AppendChild(parameterE);
					}
					fileE.AppendChild(parametersE);
				}
				scriptElement.AppendChild(fileE);

			}
			else
			{
				XmlElement codeE = doc.CreateElement(XMLTags.SCRIPT_CODE.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				setAttribute(codeE, XMLAttributes.LANGUAGE, script.EngineName, true);
				codeE.AppendChild(doc.CreateCDataSection(script.GetScript()));
				scriptElement.AppendChild(codeE);
				string[] @params = script.Parameters;
				if (@params != null && @params.Length > 0)
				{
					XmlElement parametersE = doc.CreateElement(XMLTags.SCRIPT_ARGUMENTS.XMLName, Schemas.SCHEMA_LATEST.Namespace);
					foreach (string param in @params)
					{
						XmlElement parameterE = doc.CreateElement(XMLTags.SCRIPT_ARGUMENT.XMLName, Schemas.SCHEMA_LATEST.Namespace);
						setAttribute(parameterE, XMLAttributes.COMMON_VALUE, param.ToString(), true);
						parametersE.AppendChild(parameterE);
					}
					scriptElement.AppendChild(parametersE);
				}
			}
			return scriptElement;
		}


		/// <summary>
		/// Corresponds to <element name="forkEnvironment">
		/// 
		/// </summary>
		private XmlElement createForkEnvironmentElement(XmlDocument doc, ForkEnvironment fe)
		{
			XmlElement forkEnvE = doc.CreateElement(XMLTags.FORK_ENVIRONMENT.XMLName, Schemas.SCHEMA_LATEST.Namespace);
			// attributes
			setAttribute(forkEnvE, XMLAttributes.TASK_WORKDING_DIR, fe.WorkingDir, true);
			setAttribute(forkEnvE, XMLAttributes.FORK_JAVA_HOME, fe.JavaHome, true);

			// <ref name="sysProps"/>
			if ((fe.SystemEnvironment != null) && (fe.SystemEnvironment.Keys.Count > 0))
			{
				// <element name="SystemEnvironment">
				XmlElement sysEnvE = doc.CreateElement(XMLTags.FORK_SYSTEM_PROPERTIES.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				if (fe.SystemEnvironment != null)
				{

					// <oneOrMore>
					// <ref name="sysProp"/>
					// </oneOrMore>
					foreach (KeyValuePair<string, string> entry in fe.SystemEnvironment.SetOfKeyValuePairs())
					{
						XmlElement variableE = doc.CreateElement(XMLTags.VARIABLE.XMLName, Schemas.SCHEMA_LATEST.Namespace);
						setAttribute(variableE, XMLAttributes.COMMON_NAME, entry.Key);
						setAttribute(variableE, XMLAttributes.COMMON_VALUE, entry.Value);

						sysEnvE.AppendChild(variableE);
					}
				}
				forkEnvE.AppendChild(sysEnvE);
			}

			// <ref name="jvmArgs"/>
			IList<string> args = fe.JVMArguments;
			if (args != null && (args.Count > 0))
			{
				XmlElement jvmArgsE = doc.CreateElement(XMLTags.FORK_JVM_ARGS.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				foreach (string arg in args)
				{
					XmlElement argE = doc.CreateElement(XMLTags.FORK_JVM_ARG.XMLName, Schemas.SCHEMA_LATEST.Namespace);
					setAttribute(argE, XMLAttributes.COMMON_VALUE, arg, true);
					jvmArgsE.AppendChild(argE);
				}
				forkEnvE.AppendChild(jvmArgsE);
			}

			// <ref name="additionalClasspath"/>
			IList<string> additionalCP = fe.AdditionalClasspath;
			if ((additionalCP != null) && (additionalCP.Count > 0))
			{
				XmlElement additionalCPE = doc.CreateElement(XMLTags.FORK_ADDITIONAL_CLASSPATH.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				foreach (string pathElement in additionalCP)
				{
					XmlElement pathE = doc.CreateElement(XMLTags.FORK_PATH_ELEMENT.XMLName, Schemas.SCHEMA_LATEST.Namespace);
					setAttribute(pathE, XMLAttributes.PATH, pathElement, true);
					additionalCPE.AppendChild(pathE);
				}

				forkEnvE.AppendChild(additionalCPE);
			}

			// <ref name="envScript"/>
			if (fe.EnvScript != null)
			{
				XmlElement envScriptE = doc.CreateElement(XMLTags.SCRIPT_ENV.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				XmlElement scriptElement = createScriptElement(doc, fe.EnvScript);
				envScriptE.AppendChild(scriptElement);
				forkEnvE.AppendChild(envScriptE);
			}
			return forkEnvE;
		}

		/// <summary>
		/// Corresponds to <element name="nativeExecutable">
		/// </summary>
		private XmlElement createNativeExecutableElement(XmlDocument doc, NativeTask t)
		{
			XmlElement nativeExecE = doc.CreateElement(XMLTags.NATIVE_EXECUTABLE.XMLName, Schemas.SCHEMA_LATEST.Namespace);

			// <choice>
			// <ref name="staticCommand"/>
			// <ref name="dynamicCommand"/>
			// </choice>
			string[] cmd = t.CommandLine;
			if (cmd != null && cmd.Length > 0)
			{
				// <element name="staticCommand">
				XmlElement staticCmdE = doc.CreateElement(XMLTags.NATIVE_TASK_STATIC_COMMAND.XMLName, Schemas.SCHEMA_LATEST.Namespace);
				setAttribute(staticCmdE, XMLAttributes.TASK_COMMAND_VALUE, cmd[0], true);

				// <ref name="commandArguments"/>
				if (cmd.Length > 1)
				{
					// <element name="arguments">
					XmlElement argsE = doc.CreateElement(XMLTags.NATIVE_TASK_ARGUMENTS.XMLName, Schemas.SCHEMA_LATEST.Namespace);
					for (int i = 1; i < cmd.Length; i++)
					{
						// <element name="argument">
						XmlElement argE = doc.CreateElement(XMLTags.NATIVE_TASK_ARGUMENT.XMLName, Schemas.SCHEMA_LATEST.Namespace);
						setAttribute(argE, XMLAttributes.COMMON_VALUE, cmd[i], true);
						argsE.AppendChild(argE);
					}
					staticCmdE.AppendChild(argsE);
				}
				nativeExecE.AppendChild(staticCmdE);
			}
			else
			{
				Console.Error.WriteLine("The task " + t.Name + " does not define a command");
			}

			return nativeExecE;
		}

		private XmlElement createScriptExecutableElement(XmlDocument doc, ScriptTask t)
		{
			XmlElement scriptExecE = doc.CreateElement(XMLTags.SCRIPT_EXECUTABLE.XMLName, Schemas.SCHEMA_LATEST.Namespace);
			XmlElement scriptE = createScriptElement(doc, t.Script);
			scriptExecE.AppendChild(scriptE);
			return scriptExecE;
		}

		private static string formatDate(long millis)
		{
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(millis);
			string formatted = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
			// replace heading 00: as it's not accepted by the schema validation
			return Regex.Replace(formatted, "^(00:)+", "");
		}
	}

	internal class Attribute
	{
		private string name;

		private string value;

		public Attribute(string name, string value)
		{
			this.name = name;
			this.value = value;
		}

		public virtual string Name
		{
			get
			{
				return name;
			}
			set
			{
				this.name = value;
			}
		}


		public virtual string Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = value;
			}
		}


	}

}