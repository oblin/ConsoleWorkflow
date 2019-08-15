using AbhCare.Identity.Models;
using System.Collections.Generic;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AbhCare.Workflow.UserManage
{
    internal class FilterAcitvedUsers : StepBody
    {
        public List<ApplicationUser> InActivedUsers { get; set; }
        public List<ApplicationUser> EnabledUsers { get; set; }
        
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}