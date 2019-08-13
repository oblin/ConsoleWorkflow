using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AbhCare.Workflow.UserManage
{
    internal class LoggedRevisedResult : StepBody
    {
        public RevisedResult RevisedResult { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}