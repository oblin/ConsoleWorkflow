using System.Collections.Generic;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using static System.Console;

namespace ConsoleUserWorkflow
{
    public class TestWorkflow : IWorkflow<Approval>
    {
        public void Build(IWorkflowBuilder<Approval> builder)
        {
            builder.StartWith(_ => WriteLine("Process started"))
                .While(d => CheckExitWhileLoop(d.UserList, d.IsCancel))
                    .Do(x => 
                        x.StartWith<DisplayContext>()
                            .Input(s => s.CurrentPointer, data => data.Pointer)
                            .Input(s => s.UserList, d => d.UserList)
                            .Output(d => d.IsCancel, s => s.CloseApproval)
                            .Output(d => d.UserList, s => s.UserList)
                        .Then<IncrementStep>()
                            .Input(step => step.Count, data => data.Pointer)
                            .Output(data => data.Pointer, step => step.Count)
                    )
                
                .Then(_ => WriteLine("good bye!"));
        }

        private bool CheckExitWhileLoop(List<User> users, bool isCancel)
        {
            if (isCancel)
                return !isCancel;

            var closeApproval = true;
            foreach (var user in users)
            {
                if (!user.IsApproved)
                {
                    closeApproval = false;
                    break;
                }
            }

            return !closeApproval;
        }

        public string Id => nameof(TestWorkflow);

        public int Version => 1;
    }
}