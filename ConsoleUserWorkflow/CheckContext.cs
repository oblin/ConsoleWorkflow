using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ConsoleUserWorkflow
{
    public class CheckContext : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Add User? (Y/N)");
            //var result = Console.ReadLine();
            var result = "N";
            if (result == "Y")
            {
                var newuser = new User();
                return ExecutionResult.Outcome(true);
            }
            else
            {
                return ExecutionResult.Outcome(false);
            }
        }
    }
}