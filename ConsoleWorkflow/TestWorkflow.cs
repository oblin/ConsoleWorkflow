using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ConsoleWorkflow
{
    internal class TestWorkflow : IWorkflow<WorkItem>
    {
        public string Id => nameof(TestWorkflow);

        public int Version => 1;

        public void Build(IWorkflowBuilder<WorkItem> builder)
        {
            builder.StartWith(_ => ExecutionResult.Next())
                .WaitFor("Trigger", (data, context) => context.Workflow.Id)
                    .Output(d => d.EventId, e => e.EventKey)
                .Then<NotifyTrigger>()
                    .Input(s => s.WorkItem, d => d)
                .Then(context => Console.WriteLine("workflow complete"));
        }
    }
}