using AbhCare.Identity.Models;
using System.Collections.Generic;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AbhCare.Workflow.UserManage
{
    internal class AddNewUsers : StepBody
    {
        public List<ApplicationUser> ActivedUsers { get; internal set; }

        public List<ApplicationUser> InActivedUsers { get; internal set; }

        public List<ApplicationUser> NewUsers { get; internal set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}