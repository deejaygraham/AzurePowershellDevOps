using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Management.Automation;  

namespace AzureServiceBusCmdlets
{
	[Cmdlet(
		VerbsCommon.Get, 
		"AzureServiceBusQueue",
		SupportsShouldProcess = false,
		ConfirmImpact=ConfirmImpact.Low
		)]
	[OutputType(typeof(List<ServiceBusQueueInfo>))]
	public class GetAzureServiceBusQueueCommand : Cmdlet
	{
		[Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The full connection string for service bus namespace")]
		[ValidateNotNullOrEmpty]
		public string ConnectionString { get; set; }

		[Parameter(Position = 1, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The name of the message queue")]
		public string[] Name { get; set; }

		// Overide the ProcessRecord method to process
		// the supplied user name and write out a 
		// greeting to the user by calling the WriteObject
		// method.
		protected override void ProcessRecord()
		{
			var queueDescriptions = new List<QueueDescription>();
			
			NamespaceManager namespaceManager = null;

			try
			{
				namespaceManager = NamespaceManager.CreateFromConnectionString(this.ConnectionString);
			}
			catch(Exception ex)
			{
				WriteError(new ErrorRecord(ex, "Unable to connect to namespace using connection string", ErrorCategory.ReadError, this.ConnectionString));
				return;
			}
			
			if (this.Name == null)
			{
				queueDescriptions.AddRange(namespaceManager.GetQueues());
			}
			else
			{
				foreach(string name in this.Name)
				{
					try
					{
						if (namespaceManager.QueueExists(name))
						{
							queueDescriptions.Add(namespaceManager.GetQueue(name));
						}
					}
					catch(Exception ex)
					{
						WriteError(new ErrorRecord(ex, "Unable to get queue information", ErrorCategory.ReadError, name));
						continue;
					}
				}
			}

			List<ServiceBusQueueInfo> list = new List<ServiceBusQueueInfo>();

			foreach (var description in queueDescriptions)
			{
				list.Add(FromDescription(description));
			}

			WriteObject(list, true);
		}

		private ServiceBusQueueInfo FromDescription(QueueDescription description)
		{
			return new ServiceBusQueueInfo
			{
				Name = description.Path,
				ActiveCount = description.MessageCountDetails.ActiveMessageCount,
				DeadLetterCount = description.MessageCountDetails.DeadLetterMessageCount
			};
		}
	}

	public class ServiceBusQueueInfo
	{
		public string Name { get; set; }

		public long ActiveCount { get; set; }

		public long DeadLetterCount { get; set; }
	}
}
