# AzurePowershellDevOps
Collection of cmdlets for devops-ing Azure resources

## Building
Build the .sln using Visual Studio or MsBuild

## Dependencies
This cmdlet only depends on one particular assembly - Microsoft.ServiceBus.dll 

## Registering
Copy the .dll and .psd1 to a folder named "AzureServiceBusCmdlets" in a Modules folder somewhere in your 
local Powershell's environment. If you are able and have permission, somewhere like  
<WindowsSystem32Folder>\WindowsPowerShell\v1.0\Modules.

Once the assembly and .psd1 are in place, import the module

<pre>Import-Module AzureServiceBusCmdlets</pre>

## Running
You will need a connection string to an Azure Service Bus namespace.

<pre>
Get-AzureServiceBusQueue 
-ConnectionString "Endpoint=sb://NotTelling;SharedAccessKeyName=StillNotTelling;SharedAccessKey=NotMyPassword"
</pre>

This will return details of all the queues in that namespace. To filter the results to specific queues, you can use the Name option:

<pre>Get-AzureServiceBusQueue -ConnectionString "Endpoint=..." -Name "MyQueue"</pre>

Or 

<pre>Get-AzureServiceBusQueue -ConnectionString "Endpoint=..." -Name "MyQueue", "MyOtherQueue"</pre>

## Sample Output
The output for each queue is the name, active message count and the dead letter message count:

<pre>
Name             ActiveCount  DeadLetterCount
----             -----------  ---------------

MyQueue                    0                2
MyOtherQueue              10                0
</pre>


