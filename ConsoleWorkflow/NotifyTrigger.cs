using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ConsoleWorkflow
{
    public class NotifyTrigger : StepBody
    {
        public WorkItem WorkItem { get; set; }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine($"Event {WorkItem.EventId} had been trigger");
            return ExecutionResult.Next();
        }
    }
}