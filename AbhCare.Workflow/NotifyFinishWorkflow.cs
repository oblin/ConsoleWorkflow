using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AbhCare.Workflow
{
    public class NotifyFinishWorkflow : StepBody
    {
        public ExeWorkItem WorkItem { get; set; }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            WorkItem.RaiseFinish("WaitEvent", WorkItem.Id);
            return ExecutionResult.Next();
        }
    }
}