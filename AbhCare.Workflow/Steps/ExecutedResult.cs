using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AbhCare.Workflow
{
    public class ExecutedResult : StepBody
    {
        public DateTime? DoneTime { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var timeString = DoneTime == null ? "Not yet" : DoneTime.ToString();
            Console.WriteLine($"{context.Workflow.Id}: had done at {timeString}");
            return ExecutionResult.Next();
        }
    }
}
