using AbhCare.Identity.Models;
using System.Collections.Generic;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AbhCare.Workflow.UserManage
{
    internal class FilterDisabledUsers : StepBody
    {
        public List<ApplicationUser> ActivedUsers { get; set; }
        public List<ApplicationUser> DisabledUsers { get; set; }
        
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}