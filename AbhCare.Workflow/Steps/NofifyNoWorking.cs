using RabbitLogging;
using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AbhCare.Workflow
{
    public class NofifyWorkflowTimeout : StepBody
    {
        private readonly RLogger _rLogger;

        public NofifyWorkflowTimeout(RLogger rLogger)
        {
            _rLogger = rLogger;
        }

        public WorkItem WorkItem { get; set; }
       
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine($"CareItem id: {WorkItem.Id}, Workflow id: {WorkItem.WorkflowId} Not working, please find out why?");
            WorkItem.RaiseErrorEvent("NofifyNoWorking", "time out!");
            _rLogger.WriteDebug("執行結果失敗，超過30分鐘沒有結果： " + WorkItem.Id);
            return ExecutionResult.Next();
        }
    }
}