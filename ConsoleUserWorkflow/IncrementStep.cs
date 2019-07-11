using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ConsoleUserWorkflow
{
    public class IncrementStep : StepBody
    {
        public int Count { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Count += 1;
            return ExecutionResult.Next();
        }
    }
}
