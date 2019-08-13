using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace AbhCare.Workflow.UserManage
{
    public class HrRevisedWorkflow : IWorkflow<RevisedResult>
    {
        public string Id => "HrRevisedWorkflow";

        public int Version => 1;

        public int FilterAcitvedUser { get; private set; }

        public void Build(IWorkflowBuilder<RevisedResult> builder)
        {
            builder
                .StartWith<GetAllUsers>()
                    .Output(s => s.ActivedUsers, step => step.ActivedUsers)
                    .Output(s => s.InActivedUsers, step => step.InActivedUsers)
                .Then<FilterDisabledUsers>()
                    .Input(s => s.ActivedUsers, step => step.ActivedUsers)
                    .Output(s => s.DisabledUsers, step => step.DisabledUsers)
                .Then<FilterAcitvedUsers> ()
                    .Input(s => s.InActivedUsers, step => step.InActivedUsers)
                    .Output(s => s.EnabledUsers, step => step.EnabledUsers)
                .Then<AddNewUsers>()
                    .Input(s => s.ActivedUsers, step => step.ActivedUsers)
                    .Input(s => s.InActivedUsers, step => step.InActivedUsers)
                    .Output(s => s.NewUsers, step => step.NewUsers)
                .Then<LoggedRevisedResult>()
                    .Input(s => s.RevisedResult, d => d);
        }
    }
}
