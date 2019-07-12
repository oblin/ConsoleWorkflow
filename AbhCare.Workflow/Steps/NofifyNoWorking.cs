using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AbhCare.Workflow
{
    public class NofifyWorkflowTimeout : StepBody
    {
        public WorkItem WorkItem { get; set; }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine($"CareItem id: {WorkItem.Id}, Workflow id: {WorkItem.WorkflowId} Not working, please find out why?");
            WorkItem.RaiseError("NofifyNoWorking", "time out!");
            return ExecutionResult.Next();
        }
    }
}