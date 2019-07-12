using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AbhCare.Workflow
{
    public class NotifyFinishWorkflow : StepBody
    {
        public ExeWorkItem WorkItem { get; set; }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine($"workflow {context.Workflow.Id} had been notified");
            WorkItem.RaiseFinish("WaitEvent", WorkItem.Id);
            return ExecutionResult.Next();
        }
    }
}